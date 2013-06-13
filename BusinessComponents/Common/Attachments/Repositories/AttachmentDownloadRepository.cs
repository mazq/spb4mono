//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// AttachmentDownload仓储
    /// </summary>
    public class AttachmentDownloadRepository<T> : Repository<AttachmentDownloadRecord>, IAttachmentDownloadRepository
    {
        private int pageSize = 20;

        #region Create/Update
        /// <summary>
        /// 创建新的下载记录
        /// </summary>
        /// <param name="entity">下载记录实体</param>
        /// <returns>下载记录Id</returns>
        public new object Insert(AttachmentDownloadRecord entity)
        {
            object id = base.Insert(entity);

            if (Convert.ToInt64(id) > 0)
            {
                // 缓存服务
                ICacheService cacheService = DIContainer.Resolve<ICacheService>();
                string cacheKey = GetCacheKey_RecordIds_AttachmentIds(entity.UserId);
                Dictionary<long, long> ids_AttachmentIds = cacheService.Get<Dictionary<long, long>>(cacheKey);
                if (ids_AttachmentIds != null && !ids_AttachmentIds.Values.Contains(entity.AttachmentId))
                {
                    ids_AttachmentIds[entity.Id] = entity.AttachmentId;
                    cacheService.Set(cacheKey, ids_AttachmentIds, CachingExpirationType.UsualObjectCollection);
                }
            }

            return id;
        }

        /// <summary>
        /// 更新最后下载时间
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <param name="attachmentId">附件Id</param>
        public bool UpdateLastDownloadDate(long userId, long attachmentId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("Update tn_AttachmentDownloadRecords set LastDownloadDate = @0", DateTime.UtcNow)
               .Where("UserId = @0 and AttachmentId = @1", userId, attachmentId);

            int count = CreateDAO().Execute(sql);

            Dictionary<long, long> ids_AttachmentIds = GetIds_AttachmentIdsByUser(userId);
            if (ids_AttachmentIds != null && ids_AttachmentIds.Values.Contains(attachmentId))
            {
                //更新实体缓存
                RealTimeCacheHelper.IncreaseEntityCacheVersion(ids_AttachmentIds.FirstOrDefault(n => n.Value == attachmentId).Key);
            }

            return count > 0;
        }

        #endregion

        #region Get/Gets

        /// <summary>
        /// 根据获取用户附件下载记录及附件的Id集合
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        public Dictionary<long, long> GetIds_AttachmentIdsByUser(long userId)
        {
            // 缓存服务
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            string cacheKey = GetCacheKey_RecordIds_AttachmentIds(userId);

            Dictionary<long, long> ids_attachmentIds = cacheService.Get<Dictionary<long, long>>(cacheKey);

            if (ids_attachmentIds == null || ids_attachmentIds.Count == 0)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Id,AttachmentId")
                   .From("tn_AttachmentDownloadRecords")
                   .Where("UserId = @0", userId);

                IEnumerable<dynamic> reuslts = CreateDAO().Fetch<dynamic>(sql);

                if (reuslts != null)
                {
                    ids_attachmentIds = reuslts.ToDictionary<dynamic, long, long>(v => v.Id, v => v.AttachmentId);
                }

                //更新缓存
                cacheService.Set(cacheKey, ids_attachmentIds, CachingExpirationType.RelativelyStable);
            }

            return ids_attachmentIds;
        }

        /// <summary>
        /// 获取附件的前topNumber条下载记录
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="topNumber">返回的记录数</param>
        public IEnumerable<AttachmentDownloadRecord> GetTopsByAttachmentId(long attachmentId, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
                        () =>
                        {
                            //组装CacheKey
                            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "AttachmentId", attachmentId));
                            cacheKey.Append("GetTopsByAttachmentId");
                            return cacheKey.ToString();
                        },
                        () =>
                        {
                            //组装获取实体的sql语句
                            var sql = PetaPoco.Sql.Builder;
                            sql.Where("attachmentID = @0", attachmentId);

                            return sql;
                        }
                   );
        }

        /// <summary>
        /// 获取附件的下载记录分页显示
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="pageIndex">页码</param>
        public PagingDataSet<AttachmentDownloadRecord> GetsByAttachmentId(long attachmentId, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
                        () =>
                        {
                            //组装CacheKey
                            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "AttachmentId", attachmentId));
                            cacheKey.Append("GetsByAttachmentId");
                            return cacheKey.ToString();
                        },
                        () =>
                        {
                            //组装获取实体的sql语句
                            var sql = PetaPoco.Sql.Builder;
                            sql.Where("attachmentId = @0", attachmentId);

                            return sql;
                        }
                   );
        }

        /// <summary>
        /// 获取附件依附对象的前topNumber条下载记录
        /// </summary>
        /// <param name="associateId">附件依附对象Id</param>
        /// <param name="topNumber">获取记录数</param>
        public IEnumerable<AttachmentDownloadRecord> GetTopsByAssociateId(long associateId, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                        () =>
                        {
                            //组装CacheKey
                            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "AssociateId", associateId));
                            cacheKey.Append("GetTopsByAssociateId");
                            return cacheKey.ToString();
                        },
                        () =>
                        {
                            //组装获取实体的sql语句
                            var sql = PetaPoco.Sql.Builder;
                            sql.Where("AssociateId = @0", associateId);

                            return sql;
                        }
                   );
        }

        /// <summary>
        /// 获取附件依附对象的下载记录分页显示
        /// </summary>
        /// <param name="associateId">依附对象Id</param>
        /// <param name="pageIndex">页码</param>
        public PagingDataSet<AttachmentDownloadRecord> GetsByAssociateId(long associateId, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
                        () =>
                        {
                            //组装CacheKey
                            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "AssociateId", associateId));
                            cacheKey.Append("GetsByAssociateId");
                            return cacheKey.ToString();
                        },
                        () =>
                        {
                            //组装获取实体的sql语句
                            var sql = PetaPoco.Sql.Builder;
                            sql.Where("AssociateId = @0", associateId);

                            return sql;
                        }
                   );
        }

        /// <summary>
        /// 获取用户的下载记录分页显示
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="needToBuy">是否需要购买</param>
        public PagingDataSet<AttachmentDownloadRecord> GetsByUserId(long userId, int pageIndex, bool needToBuy = true)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
                        () =>
                        {
                            //组装CacheKey
                            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
                            cacheKey.Append("GetsByUserId-needToBuy:" + needToBuy.ToString());
                            return cacheKey.ToString();
                        },
                        () =>
                        {
                            //组装获取实体的sql语句
                            var sql = PetaPoco.Sql.Builder;
                            sql.Where("UserId = @0", userId);
                            if (needToBuy)
                                sql.Where("Price > 0");
                            else
                                sql.Where("Price = 0");

                            return sql;
                        }
                   );
        }

        /// <summary>
        /// 获取拥有者附件的下载记录分页显示
        /// </summary>
        /// <param name="ownerId">附件拥有者Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="needToBuy">是否需要购买</param>
        public PagingDataSet<AttachmentDownloadRecord> GetsByOwnerId(long ownerId, int pageIndex, bool needToBuy = true)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
                        () =>
                        {
                            //组装CacheKey
                            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
                            cacheKey.Append("GetsByOwnerId-needToBuy:" + needToBuy.ToString());
                            return cacheKey.ToString();
                        },
                        () =>
                        {
                            //组装获取实体的sql语句
                            var sql = PetaPoco.Sql.Builder;
                            sql.Where("OwnerId = @0", ownerId);
                            if (needToBuy)
                                sql.Where("Price > 0");
                            else
                                sql.Where("Price = 0");

                            return sql;
                        }
                   );
        }

        #endregion

        #region GetCacheKey

        /// <summary>
        /// 获取下载记录与附件Id集合的CacheKey
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <returns></returns>
        private string GetCacheKey_RecordIds_AttachmentIds(long userId)
        {
            return "RecordIds_AttachmentIds::UserId:" + userId;
        }

        #endregion

    }
}
