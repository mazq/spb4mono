//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using Tunynet.Repositories;
using Tunynet.Utilities;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 标签仓储的具体实现类
    /// </summary>
    public class TagRepository<T> : Repository<T>, ITagRepository<T> where T : Tag
    {
        int pageSize = 20;

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entity">待创建实体</param>
        /// <returns></returns>
        public override object Insert(T entity)
        {
            Sql sql = Sql.Builder;
            sql.Append("select count(*) from tn_Tags where TagName = @0 and TenantTypeId = @1", entity.TagName, entity.TenantTypeId);

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();
            object id = 0;
            if (dao.ExecuteScalar<int>(sql) == 0)
            {
                id = base.Insert(entity);
            }
            dao.CloseSharedConnection();

            return id;
        }

        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="ids">标签Id列表</param>
        /// <param name="isApproved">是否通过审核</param>
        public void UpdateAuditStatus(IEnumerable<long> ids, bool isApproved)
        {
            List<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();
            foreach (var id in ids)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Append("UPDATE tn_Tags SET AuditStatus = @0", isApproved ? (int)AuditStatus.Success : (int)AuditStatus.Fail)
                   .Where("TagId = @0", id);
                sqls.Add(sql);
            }

            int affectCount = CreateDAO().Execute(sqls);

            //更新缓存版本号
            if (affectCount > 0)
            {
                AuditStatus status = isApproved ? AuditStatus.Success : AuditStatus.Fail;
                foreach (var id in ids)
                {
                    var tag = Get(id);
                    if (tag != null)
                    {
                        tag.AuditStatus = status;
                        RealTimeCacheHelper.IncreaseEntityCacheVersion(id);
                    }
                }

                RealTimeCacheHelper.IncreaseGlobalVersion();
            }
        }

        /// <summary>
        /// 从数据库删除实体
        /// </summary>
        /// <param name="entity">标签实体</param>
        /// <returns>影响行数</returns>
        public override int Delete(T entity)
        {
            IList<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();

            int affectCount = 0;
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            sqls.Add(PetaPoco.Sql.Builder.Append("delete from tn_Tags where TagId = @0", entity.TagId));
            sqls.Add(PetaPoco.Sql.Builder.Append("delete from tn_ItemsInTags where TagInOwnerId in (Select DISTINCT Id from tn_TagsInOwners where TenantTypeId = @0 and TagName = @1)", entity.TenantTypeId, entity.TagName));
            sqls.Add(PetaPoco.Sql.Builder.Append("delete from tn_TagsInOwners where TenantTypeId = @0 and TagName = @1", entity.TenantTypeId, entity.TagName));
            sqls.Add(PetaPoco.Sql.Builder.Append("delete from tn_TagsInGroups where TenantTypeId = @0 and TagName = @1", entity.TenantTypeId, entity.TagName));

            using (var transaction = dao.GetTransaction())
            {
                affectCount = dao.Execute(sqls);
                transaction.Complete();
            }

            if (affectCount > 0)
            {
                //更新实体缓存
                OnDeleted(entity);
                RealTimeCacheHelper.IncreaseGlobalVersion();
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 获取标签实体
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public T Get(string tagName, string tenantTypeId)
        {
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            string cacheKey = "TagIdToTagNames::TenantTypeId:" + tenantTypeId;
            Dictionary<string, long> tagNameToIds = cacheService.Get<Dictionary<string, long>>(cacheKey);

            long tagId = 0;
            if (tagNameToIds == null)
                tagNameToIds = new Dictionary<string, long>();
            if (!tagNameToIds.ContainsKey(tagName) || tagNameToIds[tagName] == 0)
            {
                Sql sql = Sql.Builder;
                sql.Select("TagId")
                   .From("tn_Tags")
                   .Where("TagName = @0", StringUtility.StripSQLInjection(tagName))
                   .Where("TenantTypeId = @0", tenantTypeId);

                PetaPocoDatabase dao = CreateDAO();
                tagId = dao.ExecuteScalar<long>(sql);
                tagNameToIds[tagName] = tagId;

                cacheService.Set(cacheKey, tagNameToIds, CachingExpirationType.UsualObjectCollection);
            }
            else
                tagId = tagNameToIds[tagName];

            return tagId == 0 ? null : Get(tagId);
        }

        /// <summary>
        /// 获取前N个标签
        /// </summary>
        /// <remarks>智能提示时也使用该方法获取数据</remarks>
        ///<param name="tenantTypeId">租户类型Id</param>
        ///<param name="topNumber">前N条数据</param>
        ///<param name="isFeatured">是否为特色标签</param>
        ///<param name="sortBy">标签排序字段</param>
        ///<param name="isTagCloud">为true时则不启用缓存</param>
        public IEnumerable<T> GetTopTags(string tenantTypeId, int topNumber, bool? isFeatured, SortBy_Tag? sortBy, bool isTagCloud = false)
        {
            IEnumerable<T> topTags = new List<T>();
            if (!isTagCloud)
            {
                topTags = GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                () =>
                {
                    //获取缓存
                    StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
                    //cacheKey.AppendFormat("TopNumber-{0}", topNumber);
                    if (sortBy.HasValue)
                        cacheKey.AppendFormat(":SortBy-{0}", (int)sortBy);
                    if (isFeatured.HasValue)
                        cacheKey.AppendFormat(":IsFeatured-{0}", isFeatured);
                    return cacheKey.ToString();
                },
                () =>
                {
                    var sql = Sql.Builder;
                    var whereSql = Sql.Builder;
                    var orderSql = Sql.Builder;
                    sql.Select("tn_Tags.*")
                       .From("tn_Tags");
                    if (!string.IsNullOrEmpty(tenantTypeId))
                        whereSql.Where("TenantTypeId = @0", tenantTypeId);
                    if (isFeatured.HasValue)
                        whereSql.Where("IsFeatured = @0", isFeatured);

                    PubliclyAuditStatus publiclyAuditStatus = new AuditService().GetPubliclyAuditStatus(0);
                    whereSql.Where("AuditStatus >= @0", publiclyAuditStatus);
                    CountService countService = new CountService(TenantTypeIds.Instance().Tag());
                    string countTableName = countService.GetTableName_Counts();
                    StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().Tag());
                    switch (sortBy)
                    {
                        case SortBy_Tag.OwnerCountDesc:
                            orderSql.OrderBy("OwnerCount desc");
                            break;
                        case SortBy_Tag.ItemCountDesc:
                            orderSql.OrderBy("ItemCount desc");
                            break;
                        case SortBy_Tag.PreDayItemCountDesc:
                            string preDayCountType = stageCountTypeManager.GetStageCountTypes(CountTypes.Instance().ItemCounts()).Min();
                            sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, preDayCountType))
                            .On("TagId = c.ObjectId");
                            orderSql.OrderBy("c.StatisticsCount desc");
                            break;
                        case SortBy_Tag.PreWeekItemCountDesc:
                            string preWeekCountType = stageCountTypeManager.GetStageCountTypes(CountTypes.Instance().ItemCounts()).Max();
                            sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, preWeekCountType))
                            .On("TagId = c.ObjectId");
                            orderSql.OrderBy("c.StatisticsCount desc");
                            break;
                        default:
                            orderSql.OrderBy("TagId desc");
                            break;
                    }
                    sql.Append(whereSql).Append(orderSql);
                    return sql;
                });
            }
            else
            {
                var sql = Sql.Builder;
                var whereSql = Sql.Builder;
                var orderSql = Sql.Builder;
                sql.Select("tn_Tags.*")
                   .From("tn_Tags");
                if (!string.IsNullOrEmpty(tenantTypeId))
                    whereSql.Where("TenantTypeId = @0", tenantTypeId);
                if (isFeatured.HasValue)
                    whereSql.Where("IsFeatured = @0", isFeatured);

                PubliclyAuditStatus publiclyAuditStatus = new AuditService().GetPubliclyAuditStatus(0);
                whereSql.Where("AuditStatus >= @0", publiclyAuditStatus);
                CountService countService = new CountService(TenantTypeIds.Instance().Tag());
                string countTableName = countService.GetTableName_Counts();
                StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().Tag());
                switch (sortBy)
                {
                    case SortBy_Tag.OwnerCountDesc:
                        orderSql.OrderBy("OwnerCount desc");
                        break;
                    case SortBy_Tag.ItemCountDesc:
                        orderSql.OrderBy("ItemCount desc");
                        break;
                    case SortBy_Tag.PreDayItemCountDesc:
                        string preDayCountType = stageCountTypeManager.GetStageCountTypes(CountTypes.Instance().ItemCounts()).Min();
                        sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, preDayCountType))
                        .On("TagId = c.ObjectId");
                        orderSql.OrderBy("c.StatisticsCount desc");
                        break;
                    case SortBy_Tag.PreWeekItemCountDesc:
                        string preWeekCountType = stageCountTypeManager.GetStageCountTypes(CountTypes.Instance().ItemCounts()).Max();
                        sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, preWeekCountType))
                        .On("TagId = c.ObjectId");
                        orderSql.OrderBy("c.StatisticsCount desc");
                        break;
                    default:
                        orderSql.OrderBy("TagId desc");
                        break;
                }
                sql.Append(whereSql).Append(orderSql);
                IEnumerable<object> ids = CreateDAO().FetchTopPrimaryKeys<T>(topNumber, sql);
                topTags = PopulateEntitiesByEntityIds(ids);
            }

            return topTags;
        }

        /// <summary>
        /// 获取前N个标签名
        /// </summary>
        /// <remarks>用于智能提示</remarks>
        ///<param name="tenantTypeId">租户类型Id</param>
        ///<param name="ownerId">拥有者Id</param>
        ///<param name="keyword">标签名称关键字</param>
        ///<param name="topNumber">前N条数据</param>
        public IEnumerable<string> GetTopTagNames(string tenantTypeId, long ownerId, string keyword, int topNumber)
        {
            IEnumerable<string> topTagNames = new List<string>();
            var sql = PetaPoco.Sql.Builder;
            sql.Select("tn_Tags.TagName")
               .From("tn_Tags T")
               .InnerJoin("tn_TagsInOwners TIO")
               .On("T.TagName = TIO.TagName and T.TenantTypeId = TIO.TenantTypeId")
               .Where("T.TenantTypeId = @0", tenantTypeId)
               .Where("TIO.OwnerId = @0", ownerId)
               .Where("T.TagName like @0", "%" + StringUtility.StripSQLInjection(keyword) + "%")
               .OrderBy("TIO.OwnerId Desc")
               .OrderBy("TIO.ItemCount Desc")
               .OrderBy("T.OwnerCount")
               .OrderBy("T.ItemCount");

            topTagNames = CreateDAO().FetchTopPrimaryKeys(topNumber, "T.TagName", sql).Cast<string>();

            return topTagNames;
        }

        /// <summary>
        ///分页检索标签
        /// </summary>
        ///<param name="query">查询条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public PagingDataSet<T> GetTags(TagQuery query, int pageIndex, int pageSize)
        {
            PagingDataSet<T> tags = null;

            var sql = PetaPoco.Sql.Builder;

            if (!string.IsNullOrEmpty(query.Keyword))
            {
                //防sql注入
                query.Keyword = StringUtility.StripSQLInjection(query.Keyword);
                sql.Where("TagName like @0", "%" + query.Keyword + "%");

            }
            if (query.PubliclyAuditStatus.HasValue)
            {
                sql.Where("AuditStatus = @0", (int)query.PubliclyAuditStatus);
            }
            if (!string.IsNullOrEmpty(query.TenantTypeId))
            {
                sql.Where("TenantTypeId = @0", query.TenantTypeId);
            }
            if (query.IsFeatured.HasValue)
            {
                sql.Where("IsFeatured = @0", query.IsFeatured);
            }
            sql.OrderBy("ItemCount Desc");

            tags = GetPagingEntities(pageSize, pageIndex, sql);

            return tags;
        }

        /// <summary>
        ///分页检索标签
        /// </summary>
        ///<param name="groupId">标签分组Id</param>
        ///<param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public PagingDataSet<T> GetTagsOfGroup(long groupId, string tenantTypeId, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection
                                     , () =>
                                     {
                                         StringBuilder sb = new StringBuilder();
                                         sb.Append(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
                                         sb.AppendFormat("TagsOfGroupPagings:GroupId:{0}", groupId);

                                         return sb.ToString();
                                     }
                                     , () =>
                                     {
                                         var sql = Sql.Builder;
                                         sql.Select("tn_Tags.*")
                                            .From("tn_Tags")
                                            .InnerJoin("tn_TagsInGroups TIG")
                                            .On("tn_Tags.TagName = TIG.TagName")
                                            .Where("TIG.GroupId = @0", groupId)
                                            .Where("TIG.TenantTypeId = @0 and tn_Tags.TenantTypeId = @0", tenantTypeId);

                                         return sql;
                                     });

        }

        /// <summary>
        /// 删除垃圾数据
        /// </summary>
        /// <param name="serviceKey">服务标识</param>
        public void DeleteTrashDatas()
        {
            IEnumerable<TenantType> tenantTypes = new TenantTypeService().Gets(MultiTenantServiceKeys.Instance().Tag());
            List<Sql> sqls = new List<Sql>();

            foreach (var tenantType in tenantTypes)
            {

                Type type = Type.GetType(tenantType.ClassType);
                if (type == null)
                    continue;

                var pd = PetaPoco.Database.PocoData.ForType(type);

                if (tenantType.TenantTypeId == TenantTypeIds.Instance().User() || tenantType.TenantTypeId == TenantTypeIds.Instance().Group())
                {
                    sqls.Add(Sql.Builder.Append("delete from tn_TagsInOwners")
                                        .Where("not exists (select 1 from " + pd.TableInfo.TableName + " where OwnerId = " + pd.TableInfo.PrimaryKey + ") and TenantTypeId = @0"
                                        , tenantType.TenantTypeId));
                }
                else
                {
                    sqls.Add(Sql.Builder.Append("delete from tn_ItemsInTags")
                                        .Where("not exists (select 1 from " + pd.TableInfo.TableName + " where ItemId = " + pd.TableInfo.PrimaryKey + ") and TenantTypeId = @0"
                                        , tenantType.TenantTypeId));
                }
            }

            CreateDAO().Execute(sqls);
        }
    }
}