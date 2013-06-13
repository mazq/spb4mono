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
using System.Configuration;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// CommentRepository
    /// </summary>
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        /// <summary>
        /// 微博应用可对外显示的审核状态
        /// </summary>
        protected PubliclyAuditStatus PubliclyAuditStatus
        {
            get
            {
                AuditService auditService = new AuditService();
                return auditService.GetPubliclyAuditStatus(0);
            }
        }

        private int ChildPageSize = 5;

        private int PageSize = 20;

        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        #region   Delete & Update

        /// <summary>
        /// 更新父级ChildCount
        /// </summary>
        /// <param name="parentId">父级Id</param>
        /// <param name="isReduce">是否减少</param>
        public void UpdateChildCount(long parentId, bool isReduce)
        {
            var sql = PetaPoco.Sql.Builder;
            if (isReduce)
                sql.Append(" UPDATE tn_Comments Set ChildCount=ChildCount-1 ").Where("Id =@0", parentId);
            else
                sql.Append(" UPDATE tn_Comments Set ChildCount=ChildCount+1 ").Where("Id =@0", parentId);

            int rows = CreateDAO().Execute(sql);

            #region 处理缓存
            if (rows > 0)
            {
                RealTimeCacheHelper.IncreaseEntityCacheVersion(parentId);
            }
            #endregion
        }

        /// <summary>
        /// 删除评论 
        /// </summary>
        /// <param name="id">评论Id</param>
        public int Delete(long id)
        {
            Comment comment = Get(id);

            var sql = PetaPoco.Sql.Builder;
            sql.Append("DELETE  FROM tn_Comments ").Where("Id=@0  or  ParentId=@0", id);
            int affectCount = CreateDAO().Execute(sql);

            #region 处理缓存
            if (affectCount > 0)
            {
                //列表缓存 
                RealTimeCacheHelper.MarkDeletion(comment);
            }
            #endregion

            return affectCount;
        }

        /// <summary>
        /// 删除被评论对象的所有评论
        /// </summary>
        /// <remarks>
        /// 供被评论对象删除时调用
        /// </remarks>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <returns></returns>
        public int DeleteCommentedObjectComments(long commentedObjectId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("DELETE FROM tn_Comments ");
            sql.Where("CommentedObjectId = @0", commentedObjectId);
            int affectCount = CreateDAO().Execute(sql);

            #region 处理缓存

            if (affectCount > 0)
                RealTimeCacheHelper.IncreaseAreaVersion("CommentedObjectId", commentedObjectId);

            #endregion

            return affectCount;
        }

        /// <summary>
        ///  删除用户发布的评论
        /// </summary>
        /// <remarks>
        /// 供用户删除时处理用户相关信息时调用
        /// </remarks>
        /// <param name="userId">UserId</param>
        /// <param name="reserveCommnetsAsAnonymous">true=保留用户发布的评论，但是修改为匿名用户；false=直接删除评论</param>
        /// <returns></returns>
        public int DeleteUserComments(long userId, bool reserveCommnetsAsAnonymous)
        {
            var sql = PetaPoco.Sql.Builder;

            if (reserveCommnetsAsAnonymous)
                sql.Append("UPDATE tn_Comments  SET UserId=0");
            else
                sql.Append("DELETE FROM tn_Comments ");

            sql.Where("UserId=@0", userId);
            int rows = CreateDAO().Execute(sql);

            #region 处理缓存
            if (rows > 0)
                RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
            #endregion

            return rows;
        }

        /// <summary>
        /// 删除垃圾数据
        /// </summary>
        public void DeleteTrashDatas()
        {
            IEnumerable<TenantType> tenantTypes = new TenantTypeService().Gets(MultiTenantServiceKeys.Instance().Comment());

            List<Sql> sqls = new List<Sql>();
            sqls.Add(Sql.Builder.Append("delete from tn_Comments where not exists (select 1 from tn_Users where UserId = tn_Comments.UserId)"));

            foreach (var tenantType in tenantTypes)
            {
                Type type = Type.GetType(tenantType.ClassType);
                if (type == null)
                    continue;
                var pd = PetaPoco.Database.PocoData.ForType(type);
                sqls.Add(Sql.Builder.Append("delete from tn_Comments")
                                    .Where("not exists (select 1 from " + pd.TableInfo.TableName + " where CommentedObjectId = " + pd.TableInfo.PrimaryKey + ") and TenantTypeId = @0"
                                    , tenantType.TenantTypeId));
            }

            CreateDAO().Execute(sqls);
        }

        #endregion

        #region  Get

        /// <summary>
        /// 获取被评论对象的所有评论（用于删除被评论对象时的积分处理）
        /// </summary>
        /// <param name="commentedObjectId">被评论对象ID</param>
        /// <returns></returns>
        public IEnumerable<Comment> GetCommentedObjectComments(long commentedObjectId)
        {
            var sql = PetaPoco.Sql.Builder
                .Select("*")
                .From("tn_Comments")
                .Where("CommentedObjectId = @0", commentedObjectId);

            IEnumerable<Comment> CommentedObjectComments = CreateDAO().Fetch<Comment>(sql);
            return CommentedObjectComments;
            
        }


        /// <summary>
        /// 获取顶级评论列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public PagingDataSet<Comment> GetRootComments(string tenantTypeId, long commentedObjectId, int pageIndex, SortBy_Comment sortBy)
        {
            return GetPagingEntities(PageSize, pageIndex, CachingExpirationType.ObjectCollection,
                //获取CacheKey
                () =>
                {
                    return GetCachekey_GetRootComments(tenantTypeId, commentedObjectId, sortBy);
                },
                //生成PetaPoco.Sql
                () =>
                {
                    var sql = PetaPoco.Sql.Builder;

                    sql.Where("ParentId = 0");

                    if (!String.IsNullOrEmpty(tenantTypeId))
                        sql.Where("TenantTypeId = @0", tenantTypeId);

                    if (commentedObjectId > 0)
                        sql.Where("CommentedObjectId = @0", commentedObjectId);

                    switch (this.PubliclyAuditStatus)
                    {
                        case PubliclyAuditStatus.Again:
                        case PubliclyAuditStatus.Fail:
                        case PubliclyAuditStatus.Pending:
                        case PubliclyAuditStatus.Success:
                            sql.Where("AuditStatus = @0", this.PubliclyAuditStatus);
                            break;
                        case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                        case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                            sql.Where("AuditStatus > @0", this.PubliclyAuditStatus);
                            break;
                        default:
                            break;
                    }

                    switch (sortBy)
                    {
                        case SortBy_Comment.DateCreated:
                            sql.OrderBy("Id ASC"); break;
                        case SortBy_Comment.DateCreatedDesc:
                            sql.OrderBy("Id DESC"); break;
                        default:
                            sql.OrderBy("Id ASC"); break;
                    }

                    return sql;
                });
        }

        /// <summary>
        /// 获取子级评论列表
        /// </summary>
        /// <param name="parentId">父评论Id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public PagingDataSet<Comment> GetChildren(long parentId, int pageIndex, SortBy_Comment sortBy)
        {
            return GetPagingEntities(ChildPageSize, pageIndex, CachingExpirationType.ObjectCollection,
                //获取CacheKey
                () =>
                {
                    return GetCacheKey_GetChildren(parentId, sortBy);
                },
                //生成PetaPoco.Sql
                () =>
                {
                    var sql = PetaPoco.Sql.Builder;

                    if (parentId > 0)
                        sql.Where("ParentId = @0", parentId);

                    switch (this.PubliclyAuditStatus)
                    {
                        case PubliclyAuditStatus.Again:
                        case PubliclyAuditStatus.Fail:
                        case PubliclyAuditStatus.Pending:
                        case PubliclyAuditStatus.Success:
                            sql.Where("AuditStatus = @0", this.PubliclyAuditStatus);
                            break;
                        case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                        case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                            sql.Where("AuditStatus > @0", this.PubliclyAuditStatus);
                            break;
                        default:
                            break;
                    }

                    switch (sortBy)
                    {
                        case SortBy_Comment.DateCreated:
                            sql.OrderBy("Id ASC"); break;
                        case SortBy_Comment.DateCreatedDesc:
                            sql.OrderBy("Id DESC"); break;
                        default:
                            sql.OrderBy("Id ASC"); break;
                    }

                    return sql;
                }
                 );
        }

        /// <summary>
        /// 获取拥有者的评论
        /// </summary>
        /// <param name="ownerId">评论拥有者Id</param>
        /// <param name="userId">评论发布人UserId</param>
        /// <param name="tenantTypeId">租户类型Id（如果为null，则获取该拥有者所有评论）</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagingDataSet<Comment> GetUserComments(long? ownerId, long? userId, string tenantTypeId, DateTime? startDate, DateTime? endDate, int pageIndex)
        {
            

            PagingDataSet<Comment> pds = null;

            var sql = PetaPoco.Sql.Builder;


            if (ownerId.HasValue && ownerId > 0)
            {
                sql.Where("OwnerId = @0", ownerId);
                sql.Where(" OwnerId != UserId  ");
            }
            if (userId.HasValue && userId > 0)
                sql.Where("UserId = @0", userId);

            if (startDate.HasValue)
                sql.Where(" DateCreated >= @0", startDate);

            if (endDate.HasValue)
                sql.Where(" DateCreated <= @0", endDate);

            if (!string.IsNullOrEmpty(tenantTypeId))
                sql.Where("TenantTypeId=@0", tenantTypeId);

            sql.OrderBy("Id  DESC");

            if (startDate.HasValue || endDate.HasValue)
            {
                GetPagingEntities(PageSize, pageIndex, sql);
            }
            else
            {
                pds = GetPagingEntities(PageSize, pageIndex, CachingExpirationType.ObjectCollection,
                 () =>
                 {
                     StringBuilder cacheKey = new StringBuilder();
                     if (ownerId.HasValue && ownerId > 0)
                         cacheKey.Append(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
                     else if (userId.HasValue && userId > 0)
                         cacheKey.Append(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
                     cacheKey.AppendFormat("TenantTypeId-{0}", tenantTypeId);

                     return cacheKey.ToString();
                 },
                 () =>
                 {
                     return sql;
                 }
                  );
            }

            return pds;
        }


        /// <summary>
        /// 获取前topNumber条评论
        /// </summary>
        /// <param name="ownerId">评论拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">获取的评论数量</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<Comment> GetTopComments(long ownerId, string tenantTypeId, int topNumber, SortBy_Comment sortBy)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
                 () =>
                 {
                     StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
                     cacheKey.AppendFormat("OwnerId-{0}:SortBy-{1}", ownerId, (int)sortBy);

                     return cacheKey.ToString();
                 },
                 () =>
                 {
                     var sql = PetaPoco.Sql.Builder.Where("TenantTypeId = @0 ", tenantTypeId);

                     if (ownerId > 0)
                         sql.Where("OwnerId = @0", ownerId);

                     switch (sortBy)
                     {
                         case SortBy_Comment.DateCreated:
                             sql.OrderBy("Id ASC"); break;
                         case SortBy_Comment.DateCreatedDesc:
                             sql.OrderBy("Id DESC"); break;
                         default:
                             sql.OrderBy("Id ASC"); break;
                     }

                     return sql;
                 });
        }

        /// <summary>
        /// 查询用户评论
        /// </summary>
        /// <param name="publiclyAuditStatus">审核状态</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">评论发布人UserId</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagingDataSet<Comment> GetComments(PubliclyAuditStatus? publiclyAuditStatus, string tenantTypeId, long? userId, DateTime? startDate, DateTime? endDate, int pageSize, int pageIndex)
        {
            var sql = PetaPoco.Sql.Builder;

            if (publiclyAuditStatus.HasValue)
            {
                switch (publiclyAuditStatus)
                {
                    case PubliclyAuditStatus.Again:
                    case PubliclyAuditStatus.Fail:
                    case PubliclyAuditStatus.Pending:
                    case PubliclyAuditStatus.Success:
                        sql.Where("AuditStatus = @0", publiclyAuditStatus);
                        break;
                    case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                    case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                        sql.Where("AuditStatus > @0", publiclyAuditStatus);
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(tenantTypeId))
                sql.Where("TenantTypeId=@0", tenantTypeId);

            if (userId.HasValue && userId > 0)
                sql.Where(" UserId = @0", userId);

            if (startDate.HasValue)
                sql.Where(" DateCreated >= @0", startDate);

            if (endDate.HasValue)
                sql.Where(" DateCreated <= @0", endDate);

            sql.OrderBy("Id  DESC");


            PagingDataSet<Comment> pds = GetPagingEntities(pageSize, pageIndex, sql);
            return pds;
        }

        /// <summary>
        /// 获取解析后的内容
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <returns></returns>
        public string GetResolvedBody(long id)
        {
            Comment comment = Get(id);
            if (comment == null)
                return string.Empty;

            ICacheService cacheService = DIContainer.Resolve<ICacheService>();

            string cacheKey = string.Format("CommentResolvedBody{0}::{1}", RealTimeCacheHelper.GetEntityVersion(id), id);
            string resolveBody = cacheService.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(resolveBody))
            {
                resolveBody = comment.Body;
                ICommentBodyProcessor commentBodyProcessor = DIContainer.Resolve<ICommentBodyProcessor>();
                resolveBody = commentBodyProcessor.Process(comment.Body, TenantTypeIds.Instance().Comment(), comment.Id, comment.UserId);
                cacheService.Set(cacheKey, resolveBody, CachingExpirationType.SingleObject);
            }

            return resolveBody;
        }


        #endregion


        #region 获取CacheKey的私有方法

        /// <summary>
        /// 获取一级回复的CacheKey
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="commentedObjectId"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        private string GetCachekey_GetRootComments(string tenantTypeId, long commentedObjectId, SortBy_Comment sortBy)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "CommentedObjectId", commentedObjectId));
            cacheKey.AppendFormat("TenantTypeId-{0}:SortBy-{1}", tenantTypeId, (int)sortBy);

            return cacheKey.ToString();
        }

        /// <summary>
        /// 获取子级回复的CacheKey
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        private string GetCacheKey_GetChildren(long parentId, SortBy_Comment sortBy)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ParentId", parentId));
            cacheKey.AppendFormat("Pagings:SortBy-{0}", (int)sortBy);

            return cacheKey.ToString();
        }

        #endregion




        /// <summary>
        /// 获取一级评论在评论列表中的页码数
        /// </summary>
        /// <param name="commentId">评论id</param>
        /// <param name="tenantType">租户类型id</param>
        /// <param name="commentedObjectId">被评论对象id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        public int GetPageIndexForCommentInCommens(long commentId, string tenantType, long commentedObjectId, SortBy_Comment sortBy)
        {
            int pageIndex = 1;
            string cacheKey = GetCachekey_GetRootComments(tenantType, commentedObjectId, sortBy);
            PagingEntityIdCollection peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
            if (peic == null)
            {
                peic = CreateDAO().FetchPagingPrimaryKeys<Comment>(PrimaryMaxRecords, PageSize * CacheablePageCount, 1, GetSql_CommentPageIndex(tenantType, commentedObjectId, sortBy));
                peic.IsContainsMultiplePages = true;
                cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
            }
            if (peic != null)
            {
                IList<long> commentIds = peic.GetTopEntityIds(peic.Count).Cast<long>().ToList();
                int commentIndex = commentIds.IndexOf(commentId);
                if (commentIndex > 0)
                    pageIndex = commentIndex / PageSize + 1;
                else
                {
                    PetaPoco.Sql sql = PetaPoco.Sql.Builder
                        .Select("count(Id)")
                        .From("tn_Comments")
                        .Where("TenantTypeId=@0", tenantType)
                        .Where("CommentedObjectId=@0", commentedObjectId);
                    switch (sortBy)
                    {
                        case SortBy_Comment.DateCreated:
                            sql.Where("Id<@0", commentId);
                            break;
                        case SortBy_Comment.DateCreatedDesc:
                            sql.Where("Id>@0", commentId);
                            break;
                        default:
                            sql.Where("Id>@0", commentId);
                            break;
                    }
                    commentIndex = CreateDAO().FirstOrDefault<int>(sql);
                    if (commentIndex > 0)
                        pageIndex = commentIndex / PageSize + 1;
                }
            }
            return pageIndex;
        }

        private PetaPoco.Sql GetSql_CommentPageIndex(string tenantType, long commentedObjectId, SortBy_Comment sortBy)
        {
            PetaPoco.Sql sql = PetaPoco.Sql.Builder;
            sql.Where("tenantTypeId=@0", tenantType);
            sql.Where("commentedObjectId=@0", commentedObjectId);
            sql.Where("parentId=0");
            if (sortBy == SortBy_Comment.DateCreated)
                sql.OrderBy("id");
            else
                sql.OrderBy("id desc");
            return sql;
        }


        public int GetPageIndexForCommentInParentCommens(long commentId, long parentId, SortBy_Comment sortBy)
        {
            int pageIndex = 1;
            string cacheKey = GetCacheKey_GetChildren(parentId, sortBy);

            PagingEntityIdCollection peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
            if (peic == null)
            {
                peic = CreateDAO().FetchPagingPrimaryKeys<Comment>(PrimaryMaxRecords, PageSize * CacheablePageCount, 1, GetSql_CommentPageIndexInParentComments(parentId, sortBy));
                peic.IsContainsMultiplePages = true;
                cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
            }
            if (peic != null)
            {
                IList<long> commentIds = peic.GetTopEntityIds(peic.Count).Cast<long>().ToList();
                int commentIndex = commentIds.IndexOf(commentId);
                if (commentIndex > 0)
                {
                    pageIndex = commentIndex / ChildPageSize + 1;
                }
                else
                {
                    PetaPoco.Sql sql = PetaPoco.Sql.Builder
                        .Select("Count(Id)")
                        .From("tn_Comments")
                        .Where("ParentId=@0", parentId);
                    switch (sortBy)
                    {
                        case SortBy_Comment.DateCreated:
                            sql.Where("Id<@0", commentId);
                            break;
                        case SortBy_Comment.DateCreatedDesc:
                            sql.Where("Id>@0", commentId);
                            break;
                        default:
                            sql.Where("Id<@0", commentId);
                            break;
                    }
                    commentIndex = CreateDAO().FirstOrDefault<int>(sql);
                    if (commentIndex > 0)
                        pageIndex = commentIndex / ChildPageSize + 1;
                }
            }
            return pageIndex;
        }

        private PetaPoco.Sql GetSql_CommentPageIndexInParentComments(long parentId, SortBy_Comment sortBy)
        {
            PetaPoco.Sql sql = PetaPoco.Sql.Builder;
            sql.Where("parentId=@0", parentId);
            if (sortBy == SortBy_Comment.DateCreated)
                sql.OrderBy("id");
            else
                sql.OrderBy("id desc");
            return sql;
        }
    }
}
