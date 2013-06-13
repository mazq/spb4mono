//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Repositories;
using Tunynet.Common;
using System.Linq;
using Tunynet.Utilities;

namespace Spacebuilder.Group
{
    
    //已过滤掉加入方式为仅邀请加入的群组
    /// <summary>
    ///群组Repository
    /// </summary>
    public class GroupRepository : Repository<GroupEntity>, IGroupRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        public GroupRepository() { }

        public GroupRepository(PubliclyAuditStatus publiclyAuditStatus)
        {
            this.publiclyAuditStatus = publiclyAuditStatus;
        }

        #region 维护群组
        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">皮肤标识</param>
        public void ChangeThemeAppearance(long groupId, bool isUseCustomStyle, string themeAppearance)
        {
            GroupEntity group = Get(groupId);
            if (group == null)
                return;

            var sql_Update = PetaPoco.Sql.Builder;
            sql_Update.Append("update spb_Groups set ThemeAppearance = @0,IsUseCustomStyle = @1 where GroupId = @2", themeAppearance ?? string.Empty, isUseCustomStyle, groupId);
            int affectedCount = CreateDAO().Execute(sql_Update);
            if (affectedCount > 0)
            {
                group.ThemeAppearance = themeAppearance ?? string.Empty;
                group.IsUseCustomStyle = isUseCustomStyle;
                base.OnUpdated(group);
            }
        }

        /// <summary>
        /// 更换群主
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="newOwnerUserId">新群主UserId</param>
        public void ChangeGroupOwner(long groupId, long newOwnerUserId)
        {
            Sql sql = Sql.Builder;
            sql.Append("update spb_Groups set UserId = @0 where GroupId = @1", newOwnerUserId, groupId);
            CreateDAO().Execute(sql);
            GroupEntity group = Get(groupId);
            if (group != null)
            {
                group.UserId = newOwnerUserId;
                base.OnUpdated(group);
            }
            
            //已修改
            
            
            
        }

        /// <summary>
        /// 每天定时计算各个群组的成长值
        /// </summary>
        public void CalculateGrowthValues()
        {
            Sql sql = Sql.Builder;
            sql.Append(@"update spb_Groups set GrowthValue=
            (select COUNT(*) from spb_BarThreads where SectionId=GroupId and TenantTypeId=@0) * 2 + 
            (select COUNT(*) from spb_BarPosts where SectionId = GroupId and TenantTypeId=@0) + MemberCount *5 +
            (select COUNT(*) from spb_Microblogs where OwnerId = GroupId and TenantTypeId=@0)", TenantTypeIds.Instance().Group());
            CreateDAO().Execute(sql);
        }


        /// <summary>
        /// 删除群组实体
        /// </summary>
        /// <param name="entity">群组实体</param>
        /// <returns></returns>
        public override int Delete(GroupEntity entity)
        {
            int affectCount = base.Delete(entity);
            if (affectCount > 0)
            {
                var sql = Sql.Builder.Append("delete spb_GroupMemberApplies").Where("GroupId=@0", entity.GroupId);
                CreateDAO().Execute(sql);
            }
            return affectCount;
        }

        /// <summary>
        /// 插入群组实体
        /// </summary>
        /// <param name="entity">群组实体</param>
        /// <returns></returns>
        public override object Insert(GroupEntity entity)
        {
            entity.GroupId = IdGenerator.Next();
            return base.Insert(entity);
        }

        #endregion

        #region 获取群组

        /// <summary>
        /// 根据群组Key获取群组Id
        /// </summary>
        /// <param name="groupKey">群组Key</param>
        /// <returns>群组Id</returns>
        public long GetGroupIdByGroupKey(string groupKey)
        {
            var sql_Select = Sql.Builder.Select("GroupId").From("spb_Groups").Where("GroupKey = @0", groupKey);
            return CreateDAO().FirstOrDefault<long>(sql_Select);
        }

        
        

        /// <summary>
        /// 获取前N个排行群组
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GetTops(int topNumber, string areaCode, long? categoryId, SortBy_Group sortBy)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("TopGroups::areaCode-{0}:categoryId-{1}:sortBy-{2}", areaCode, categoryId, sortBy);
                    return cacheKey.ToString();
                },
                () =>
                {
                    return Getsqls(areaCode, categoryId, null, sortBy);
                });
        }

        /// <summary>
        /// 获取匹配的前N个排行群组
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="keyword">关键字</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GetMatchTops(int topNumber, string keyword, string areaCode, long? categoryId, SortBy_Group sortBy)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("TopGroups::areaCode-{0}:categoryId-{1}:sortBy-{2}:keyword-{3}", areaCode, categoryId, sortBy, keyword);
                    return cacheKey.ToString();
                },
                () =>
                {
                    return Getsqls(areaCode, categoryId, keyword, sortBy);
                });
        }

        /// <summary>
        /// 根据标签名获取群组分页集合
        /// </summary>
        /// <param name="tagName">标签名</param></param>
        /// <param name="sortBy">排序依据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页列表</returns>
        public PagingDataSet<GroupEntity> GetsByTag(string tagName, SortBy_Group sortBy, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("GroupsByTag::TagName-{0}:SortBy-{1}", tagName, sortBy);
                    return cacheKey.ToString();
                },
                () =>
                {
                    Sql sql = Sql.Builder;
                    sql.Select("spb_Groups.*").From("spb_Groups");
                    
                    //已修改

                    
                    
                    sql.InnerJoin("tn_ItemsInTags").On("GroupId = tn_ItemsInTags.ItemId").Where("tn_ItemsInTags.TagName = @0 and tn_ItemsInTags.TenantTypeId = @1", tagName, TenantTypeIds.Instance().Group())
                    .Where("spb_Groups.IsPublic = 1");

                    switch (sortBy)
                    {
                        case SortBy_Group.DateCreated_Desc:
                            sql.OrderBy("DateCreated desc");
                            break;
                        case SortBy_Group.GrowthValue_Desc:
                            sql.OrderBy("GrowthValue desc");
                            break;
                        case SortBy_Group.MemberCount_Desc:
                            sql.OrderBy("MemberCount desc");
                            break;
                        default:
                            sql.OrderBy("GrowthValue desc");
                            break;
                    }
                    return sql;
                });
        }

        
        //已修改
        /// <summary>
        /// 获取用户创建的群组列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="ignoreAudit">是否忽略审核状态（作者或管理员查看时忽略审核状态）</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GetMyCreatedGroups(long userId, bool ignoreAudit)
        {
            
            
            
            string cacheKey = "GroupsOfUser" + "-" + RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId) + "-ignoreAudit" + ignoreAudit;

            IEnumerable<long> groupIds = cacheService.Get<IEnumerable<long>>(cacheKey);
            if (groupIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("GroupId")
                   .From("spb_Groups")
                   .Where("UserId=@0", userId);
                if (!ignoreAudit)
                {
                    
                    
                    switch (this.PubliclyAuditStatus)
                    {
                        case PubliclyAuditStatus.Again:
                        case PubliclyAuditStatus.Fail:
                        case PubliclyAuditStatus.Pending:
                        case PubliclyAuditStatus.Success:
                            sql.Where("AuditStatus=@0", this.PubliclyAuditStatus);
                            break;
                        case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                        case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                            sql.Where("AuditStatus>@0", this.PubliclyAuditStatus);
                            break;
                        default:
                            break;
                    }
                }
                sql.OrderBy("GroupId desc");
                
                
                groupIds = CreateDAO().Fetch<long>(sql);
                if (groupIds != null)
                {
                    cacheService.Add(cacheKey, groupIds, CachingExpirationType.UsualObjectCollection);
                }
            }
            return PopulateEntitiesByEntityIds<long>(groupIds);
        }

        /// <summary>
        /// 分页获取排行数据
        /// </summary>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<GroupEntity> Gets(string areaCode, long? categoryId, SortBy_Group sortBy, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("PagingGroupRanks::areaCode-{0}:categoryId-{1}:sortBy-{2}", areaCode, categoryId, sortBy);
                    return cacheKey.ToString();
                },
                () =>
                {
                    return Getsqls(areaCode, categoryId, null, sortBy);
                });
        }

        /// <summary>
        /// 群组成员也加入的群组
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GroupMemberAlsoJoinedGroups(long groupId, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("GroupMemberAlsoJoinedGroups::groupId-{0}", groupId);
                    return cacheKey.ToString();
                },
                () =>
                {
                    Sql sql = Sql.Builder;
                    sql.Select("distinct spb_Groups.*")
                       .From("spb_Groups")
                       .InnerJoin("spb_GroupMembers M")
                       .On("M.GroupId = spb_Groups.GroupId")
                       .Where("M.GroupId=@0 and spb_Groups.GroupId!=@0", groupId)
                       .Where("spb_Groups.IsPublic = 1");
                    sql.OrderBy("spb_Groups.GrowthValue desc");
                    return sql;
                });
        }

        /// <summary>
        /// 获取我关注的用户加入的群组
        /// </summary>
        /// <param name="userId">当前用户的userId</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> FollowedUserAlsoJoinedGroups(long userId, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
        () =>
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.AppendFormat("FollowedUserAlsoJoinedGroups::userId-{0}", userId);
            return cacheKey.ToString();
        },
        () =>
        {
            Sql sql = Sql.Builder;
            sql.Select("distinct spb_Groups.*")
               .From("spb_Groups")
               .InnerJoin("spb_GroupMembers M")
               .On("M.GroupId = spb_Groups.GroupId")
               .InnerJoin("tn_Follows F")
               .On("F.FollowedUserId = M.UserId")
               .Where("F.UserId = @0 or spb_Groups.UserId = F.FollowedUserId", userId)
               .Where("spb_Groups.IsPublic = 1");
            sql.OrderBy("spb_Groups.GrowthValue desc");
            return sql;
        });
        }

        /// <summary>
        /// 获取用户加入的群组列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<GroupEntity> GetMyJoinedGroups(long userId, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
            () =>
            {
                StringBuilder cacheKey = new StringBuilder();
                cacheKey.Append(EntityData.ForType(typeof(GroupMember)).RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
                cacheKey.AppendFormat("MyJoinedGroups::UserId-{0}", userId);
                return cacheKey.ToString();
            },
            () =>
            {
                Sql sql = Sql.Builder;
                sql.Select("distinct GroupId")
                   .From("spb_GroupMembers")
                   .Where("UserId = @0", userId);
                
                
                //sql.OrderBy("JoinDate desc");
                return sql;
            });
        }

        /// <summary>
        /// 分页获取群组后台管理列表
        /// </summary>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="keywords">名称关键字</param>
        /// <param name="ownerUserId">群主</param>
        /// <param name="minDateTime">创建时间下限值</param>
        /// <param name="maxDateTime">创建时间上限值</param>
        /// <param name="minMemberCount">成员数量下限值</param>
        /// <param name="maxMemberCount">成员数量上限值</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<GroupEntity> GetsForAdmin(AuditStatus? auditStatus = null, long? categoryId = null, string keywords = null, long? ownerUserId = null, DateTime? minDateTime = null, DateTime? maxDateTime = null, int? minMemberCount = null, int? maxMemberCount = null, int pageSize = 20, int pageIndex = 1)
        {
            Sql sql = Sql.Builder;
            sql.Select("*").From("spb_Groups");

            if (categoryId != null && categoryId.Value > 0)
            {
                CategoryService categoryService = new CategoryService();
                IEnumerable<Category> categories = categoryService.GetDescendants(categoryId.Value);
                List<long> categoryIds = new List<long> { categoryId.Value };
                if (categories != null && categories.Count() > 0)
                    categoryIds.AddRange(categories.Select(n => n.CategoryId));
                sql.InnerJoin("tn_ItemsInCategories")
               .On("spb_Groups.GroupId = tn_ItemsInCategories.ItemId");
                sql.Where("tn_ItemsInCategories.CategoryId in(@categoryIds)", new { categoryIds = categoryIds });
            }

            if (auditStatus.HasValue)
            {
                
                //ok
                sql.Where("AuditStatus = @0", auditStatus);
            }

            if (!string.IsNullOrEmpty(keywords))
            {
                sql.Where("spb_Groups.GroupName like @0", "%" + StringUtility.StripSQLInjection(keywords) + "%");
            }
            if (ownerUserId.HasValue)
            {
                sql.Where("spb_Groups.UserId = @0", ownerUserId);
            }
            if (minDateTime.HasValue)
            {
                sql.Where("DateCreated >= @0", minDateTime.Value.Date);
            }
            if (maxDateTime.HasValue)
            {
                
                sql.Where("DateCreated < @0", maxDateTime.Value.Date.AddDays(1));
            }

            if (minMemberCount.HasValue)
            {
                sql.Where("MemberCount >= @0", minMemberCount);
            }
            if (maxMemberCount.HasValue)
            {
                
                sql.Where("MemberCount <= @0", maxMemberCount);
            }
            
            //已修改
            sql.OrderBy("DateCreated desc");
            return GetPagingEntities(pageSize, pageIndex, sql);
        }
        
        


        /// <summary>
        /// Gets和GetTops的sql语句
        /// </summary>
        private Sql Getsqls(string areaCode, long? categoryId, string keyword, SortBy_Group sortBy)
        {
            Sql sql = Sql.Builder;
            var whereSql = Sql.Builder;
            var orderSql = Sql.Builder;

            sql.Select("spb_Groups.*").From("spb_Groups");

            if (categoryId != null && categoryId.Value > 0)
            {
                CategoryService categoryService = new CategoryService();
                IEnumerable<Category> categories = categoryService.GetDescendants(categoryId.Value);
                List<long> categoryIds = new List<long> { categoryId.Value };
                if (categories != null && categories.Count() > 0)
                    categoryIds.AddRange(categories.Select(n => n.CategoryId));
                sql.InnerJoin("tn_ItemsInCategories")
               .On("spb_Groups.GroupId = tn_ItemsInCategories.ItemId");
                whereSql.Where("tn_ItemsInCategories.CategoryId in(@categoryIds)", new { categoryIds = categoryIds });
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                if (areaCode.Equals("A1560000", StringComparison.CurrentCultureIgnoreCase))
                {
                    
                    //已修改
                    whereSql.Where("AreaCode like '1%' or AreaCode like '2%' or AreaCode like '3%' or AreaCode like '4%' or AreaCode like '5%' or AreaCode like '6%' or AreaCode like '7%' or AreaCode like '8%' or AreaCode like '9%' ");
                }
                else
                {
                    areaCode = areaCode.TrimEnd('0');
                    if (areaCode.Length % 2 == 1)
                        areaCode = areaCode + "0";
                    whereSql.Where("AreaCode like @0 ", areaCode + "%");
                }
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereSql.Where("GroupName like @0", keyword + "%");
            }
            whereSql.Where("spb_Groups.IsPublic = 1");
            
            //已修改
            switch (this.PubliclyAuditStatus)
            {
                case PubliclyAuditStatus.Again:
                case PubliclyAuditStatus.Fail:
                case PubliclyAuditStatus.Pending:
                case PubliclyAuditStatus.Success:
                    whereSql.Where("AuditStatus=@0", this.PubliclyAuditStatus);
                    break;
                case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                    whereSql.Where("AuditStatus>@0", this.PubliclyAuditStatus);
                    break;
                default:
                    break;
            }
            CountService countService = new CountService(TenantTypeIds.Instance().Group());
            string countTableName = countService.GetTableName_Counts();
            switch (sortBy)
            {
                case SortBy_Group.DateCreated_Desc:
                    orderSql.OrderBy("DateCreated desc");
                    break;
                case SortBy_Group.GrowthValue_Desc:
                    orderSql.OrderBy("GrowthValue desc");
                    break;
                case SortBy_Group.MemberCount_Desc:
                    orderSql.OrderBy("MemberCount desc");
                    break;
                case SortBy_Group.HitTimes:
                    sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, CountTypes.Instance().HitTimes()))
                    .On("GroupId = c.ObjectId");
                    orderSql.OrderBy("c.StatisticsCount desc");
                    break;
                case SortBy_Group.StageHitTimes:
                    StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().Group());
                    int stageCountDays = stageCountTypeManager.GetMaxDayCount(CountTypes.Instance().HitTimes());
                    string stageCountType = stageCountTypeManager.GetStageCountType(CountTypes.Instance().HitTimes(), stageCountDays);
                    sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, stageCountType))
                    .On("GroupId = c.ObjectId");
                    orderSql.OrderBy("c.StatisticsCount desc");
                    break;
                default:
                    
                    orderSql.OrderBy("DateCreated desc");
                    break;
            }

            sql.Append(whereSql).Append(orderSql);
            return sql;
        }

        #endregion

        
        /// <summary>
        /// 删除用户记录（删除用户时使用）
        /// </summary>
        /// <param name="userId">被删除用户</param>
        /// <param name="takeOver">接管用户</param>
        /// <param name="takeOverAll">是否接管被删除用户的所有内容</param>
        public void DeleteUser(long userId, Common.User takeOver, bool takeOverAll)
        {
            List<Sql> sqls = new List<Sql>();
            if (takeOver != null)
            {
                //更改群主
                sqls.Add(Sql.Builder.Append("update spb_Groups set UserId = @0 where UserId = @1", takeOver.UserId, userId));

                //获取用户Id为userId创建的群组
                Sql havedGroups = Sql.Builder;
                havedGroups.Select("GroupId")
                    .From("spb_Groups")
                    .Where("UserId = @0", userId);
                IEnumerable<long> groupIds = CreateDAO().Fetch<long>(havedGroups);


                //获取我加入用户Id为userId创建的群组的群组ID
                if (groupIds.Count() > 0)
                {
                    Sql joinedGroups = Sql.Builder;
                    joinedGroups.Select("GroupId")
                        .From("spb_GroupMembers")
                        .Where("UserId = @userId and GroupId in (@groupIds)", new { userId = takeOver.UserId }, new { groupIds = groupIds });
                    IEnumerable<long> joinedIds = CreateDAO().Fetch<long>(joinedGroups);


                    sqls.Add(Sql.Builder.Append("delete from spb_GroupMembers where UserId = (@userId) and GroupId in (@groupIds)", new { userId = takeOver.UserId }, new { groupIds = groupIds }));
                    sqls.Add(Sql.Builder.Append("delete from spb_GroupMemberApplies where UserId = (@userId) and GroupId in (@groupIds)", new { userId = takeOver.UserId }, new { groupIds = groupIds }));
                    if (joinedIds.Count() > 0)
                    {
                        sqls.Add(Sql.Builder.Append("update spb_Groups set MemberCount = MemberCount - 1 where GroupId in(@joinedIds)", new { joinedIds = joinedIds }));
                    }
                }
                //此选项尚未用到
                if (takeOverAll)
                { }
            }

            //获取用户ID为userId加入的群组
            Sql userJoinedGroups = Sql.Builder;
            userJoinedGroups.Select("GroupId")
                .From("spb_GroupMembers")
                .Where("UserId = @userId", new { userId = userId });
            IEnumerable<long> userJoinedIds = CreateDAO().Fetch<long>(userJoinedGroups);

            sqls.Add(Sql.Builder.Append("delete from spb_GroupMembers where UserId = @0", userId));
            sqls.Add(Sql.Builder.Append("delete from spb_GroupMemberApplies where UserId = @0", userId));
            if (userJoinedIds.Count() > 0)
            {
                sqls.Add(Sql.Builder.Append("update spb_Groups set MemberCount = MemberCount - 1 where GroupId in(@userJoinedIds)", new { userJoinedIds = userJoinedIds }));
            }

            CreateDAO().Execute(sqls);
        }

        /// <summary>
        /// 群组应用可对外显示的审核状态
        /// </summary>
        private PubliclyAuditStatus? publiclyAuditStatus;
        /// <summary>
        /// 群组应用可对外显示的审核状态
        /// </summary>
        protected PubliclyAuditStatus PubliclyAuditStatus
        {
            get
            {
                if (publiclyAuditStatus == null)
                {
                    AuditService auditService = new AuditService();
                    publiclyAuditStatus = auditService.GetPubliclyAuditStatus(GroupConfig.Instance().ApplicationId);
                }
                return publiclyAuditStatus.Value;
            }
            set
            {
                this.publiclyAuditStatus = value;
            }
        }

        /// <summary>
        /// 根据审核状态获取群组数
        /// </summary>
        /// <returns></returns>
        public Dictionary<GroupManageableCountType, int> GetManageableCounts()
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            Dictionary<GroupManageableCountType, int> countType = new Dictionary<GroupManageableCountType, int>();

            var sql_selectIsActivated = PetaPoco.Sql.Builder;
            sql_selectIsActivated.Select("count(*)").From("spb_Groups");

            sql_selectIsActivated.Where("AuditStatus = @0", AuditStatus.Pending);

            countType[GroupManageableCountType.Pending] = dao.FirstOrDefault<int>(sql_selectIsActivated);

            var sql_selectIsAll = PetaPoco.Sql.Builder;
            sql_selectIsAll.Select("count(*)").From("spb_Groups");

            countType[GroupManageableCountType.IsAll] = dao.FirstOrDefault<int>(sql_selectIsAll);

            var sql_selectIsLast24 = PetaPoco.Sql.Builder;
            sql_selectIsLast24.Select("count(*)").From("spb_Groups");
            sql_selectIsLast24.Where("DateCreated >= @0 and  DateCreated < @1", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);

            countType[GroupManageableCountType.IsLast24] = dao.FirstOrDefault<int>(sql_selectIsLast24);

            dao.CloseSharedConnection();

            return countType;
        }

        /// <summary>
        /// 获取群组管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public Dictionary<string, long> GetManageableDatas(string tenantTypeId = null)
        {
            var dao = CreateDAO();
            Dictionary<string, long> manageableDatas = new Dictionary<string, long>();

            Sql sql = Sql.Builder;
            sql.Select("count(*)")
                .From("spb_Groups")
                .Where("AuditStatus=@0", AuditStatus.Pending);

            manageableDatas.Add(ApplicationStatisticDataKeys.Instance().GroupPendingCount(), dao.FirstOrDefault<long>(sql));
            sql = Sql.Builder;
            sql.Select("count(*)")
                .From("spb_Groups")
                .Where("AuditStatus=@0", AuditStatus.Again);

            manageableDatas.Add(ApplicationStatisticDataKeys.Instance().GroupAgainCount(), dao.FirstOrDefault<long>(sql));

            return manageableDatas;
        }

        /// <summary>
        /// 获取群组统计数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, long> GetStatisticDatas(string tenantTypeId = null)
        {
            var dao = CreateDAO();
            string cacheKey = "GroupStatisticData";
            Dictionary<string, long> statisticDatas = cacheService.Get<Dictionary<string, long>>(cacheKey);
            if (statisticDatas == null)
            {
                statisticDatas = new Dictionary<string, long>();
                Sql sql = Sql.Builder;
                sql.Select("count(*)")
                    .From("spb_Groups");
                statisticDatas.Add(ApplicationStatisticDataKeys.Instance().TotalCount(), dao.FirstOrDefault<long>(sql));

                sql = Sql.Builder;
                sql.Select("count(*)")
                    .From("spb_Groups")
                    .Where("DateCreated > @0", DateTime.UtcNow.AddDays(-1));

                statisticDatas.Add(ApplicationStatisticDataKeys.Instance().Last24HCount(), dao.FirstOrDefault<long>(sql));
                cacheService.Add(cacheKey, statisticDatas, CachingExpirationType.UsualSingleObject);
            }

            return statisticDatas;
        }
    }
}