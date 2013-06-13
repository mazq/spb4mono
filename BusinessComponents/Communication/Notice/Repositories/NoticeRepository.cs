//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using Tunynet.Common;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 通知数据访问
    /// </summary>
    public class NoticeRepository : Repository<Notice>, INoticeRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();
        private int PageSize = 30;

        /// <summary>
        /// 创建通知
        /// </summary>
        /// <param name="entity">通知实体</param>
        public override object Insert(Notice entity)
        {
            base.Insert(entity);

            if (entity.Id > 0)
            {
                //更新缓存
                string cacheKey = GetCacheKey_UnhandledNoticeCount(entity.UserId);
                int? count = cacheService.Get(cacheKey) as int?;
                if (count != null)
                {
                    count++;
                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
                }
            }
            return entity.Id;
        }

        /// <summary>
        /// 删除单条通知
        /// </summary>
        /// <param name="entityId">通知Id</param>
        public override int DeleteByEntityId(object entityId)
        {
            Notice entity = Get(entityId);
            if (entity == null)
                return 0;
            //更新缓存
            string cacheKey = GetCacheKey_UnhandledNoticeCount(entity.UserId);

            int? count = cacheService.Get(cacheKey) as int?;
            if (count != null && entity.Status == NoticeStatus.Unhandled)
            {
                count--;
                if (count >= 0)
                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
            }

            int affectCount = base.DeleteByEntityId(entityId);
            return affectCount;
        }

        /// <summary>
        /// 清空接收人的通知记录
        /// </summary>
        /// <param name="userId">接收人Id</param>
        /// <param name="status">通知状态</param>
        public void ClearAll(long userId, NoticeStatus? status = null)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("Delete from tn_Notices")
               .Where("UserId=@0", userId);

            if (status.HasValue)
            {
                sql.Where("Status = @0", (int)status);
            }

            CreateDAO().Execute(sql);
            //更新缓存            
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        /// <summary>
        /// 删除用户的记录（删除用户时使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            List<Sql> sql_Deletes = new List<Sql>();
            sql_Deletes.Add(PetaPoco.Sql.Builder.Append("delete from tn_Notices where UserId = @0 or LeadingActorUserId = @0", userId));
            sql_Deletes.Add(PetaPoco.Sql.Builder.Append("delete from tn_UserNoticeSettings where UserId = @0", userId));
            CreateDAO().Execute(sql_Deletes);
        }

        /// <summary>
        /// 将通知设置为已处理状态
        /// </summary>
        /// <param name="id">通知Id</param>
        public void SetIsHandled(long id)
        {
            Notice entity = Get(id);
            if (entity == null)
                return;
            var sql = PetaPoco.Sql.Builder;

            sql.Append("UPDATE tn_Notices Set Status=@0", (int)NoticeStatus.Handled)
            .Where("Id=@0", id);
            CreateDAO().Execute(sql);
            //更新缓存
            string cacheKey = GetCacheKey_UnhandledNoticeCount(entity.UserId);
            int? count = cacheService.Get(cacheKey) as int?;
            if (count != null)
            {
                count--;
                if (count >= 0)
                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
            }
            //更新缓存
            entity.Status = NoticeStatus.Handled;
            base.OnUpdated(entity);
        }

        /// <summary>
        /// 批量将所有未处理通知修改为已处理状态
        /// </summary>
        /// <param name="userId">接收人Id</param>
        public void BatchSetIsHandled(long userId)
        {
            PetaPocoDatabase dao = CreateDAO();
            var sql = PetaPoco.Sql.Builder;
            sql.Select("Id").From("tn_Notices")
                .Where("UserId=@0", userId)
                .Where("Status=@0", (int)NoticeStatus.Unhandled);
            IEnumerable<object> ids_object = dao.FetchFirstColumn(sql);
            IEnumerable<long> ids = ids_object.Cast<long>();
            sql = PetaPoco.Sql.Builder;
            sql.Append("UPDATE tn_Notices Set Status=@0", (int)NoticeStatus.Handled)
            .Where("UserId=@0", userId)
            .Where("Status=@0", (int)NoticeStatus.Unhandled);
            dao.Execute(sql);
            //更新缓存
            foreach (long id in ids)
            {
                Notice notice = Get(id);
                if (notice != null)
                    notice.Status = NoticeStatus.Handled;
                RealTimeCacheHelper.IncreaseEntityCacheVersion(id);
            }

            string cacheKey = GetCacheKey_UnhandledNoticeCount(userId);
            int? count = cacheService.Get(cacheKey) as int?;
            if (count != null)
            {
                count = 0;
                cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
            }
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        /// <summary>
        /// 获取某人的未处理通知数
        /// </summary>
        public int GetUnhandledCount(long userId)
        {
            string cacheKey = GetCacheKey_UnhandledNoticeCount(userId);

            int? count = cacheService.Get(cacheKey) as int?;
            if (count == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Count(*)")
                .From("tn_Notices")
                .Where("UserId=@0 and Status=@1", userId, (int)NoticeStatus.Unhandled);
                count = CreateDAO().FirstOrDefault<int?>(sql);
                if (count != null)
                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
            }
            return count ?? 0;
        }

        /// <summary>
        /// 获取用户最近几条未处理的通知
        /// </summary>
        /// <param name="topNumber"></param>
        /// <param name="userId">通知接收人Id</param>
        public IEnumerable<Notice> GetTops(long userId, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                 () =>
                 {
                     StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
                     return cacheKey.ToString();
                 },
                 () =>
                 {
                     var sql = PetaPoco.Sql.Builder;
                     sql.Where("UserId=@0 and Status=@1", userId, (int)NoticeStatus.Unhandled);
                     sql.OrderBy("Id  DESC");
                     return sql;
                 });
        }

        /// <summary>
        /// 获取用户通知的分页集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">通知状态</param>
        /// <param name="typeId">通知类型Id</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>通知分页集合</returns>
        public PagingDataSet<Notice> Gets(long userId, NoticeStatus? status, int? typeId, int? applicationId, int pageIndex)
        {
            return GetPagingEntities(PageSize, pageIndex, CachingExpirationType.ObjectCollection,
                //获取CacheKey
                           () =>
                           {
                               StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
                               cacheKey.AppendFormat("Status-{0}:TypeId-{1}:ApplicationId-{2}", (int?)status, typeId, userId);
                               return cacheKey.ToString();
                           },
                //生成PetaPoco.Sql
                           () =>
                           {
                               var sql = PetaPoco.Sql.Builder;
                               sql.Where(" UserId = @0", userId);

                               if (status.HasValue)
                                   sql.Where("Status = @0", (int)status);

                               if (typeId.HasValue && typeId.Value > 0)
                                   sql.Where("TypeId=@0", typeId.Value);

                               sql.OrderBy("Id  DESC");
                               return sql;
                           }
                            );
        }

        /// <summary>
        /// 获取通知需提醒信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserReminderInfo> GetUserReminderInfos()
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("tn_Notices.*")
            .From("tn_Notices")
            .InnerJoin("tn_Users")
            .On("tn_Notices.UserId = tn_Users.UserId")
            .Where("tn_Notices.Status = 0")
            .Where("tn_Users.IsEmailVerified = 1")
            .OrderBy("tn_Notices.UserId")
            .OrderBy("tn_Notices.Id");
            IEnumerable<Notice> userReminderInfos_object = CreateDAO().Fetch<Notice>(sql);
            List<UserReminderInfo> userRminderInfos = new List<UserReminderInfo>();
            foreach (long userId in userReminderInfos_object.Select(n => n.UserId).Distinct())
            {
                UserReminderInfo userReminderInfo = new UserReminderInfo();
                userReminderInfo.UserId = userId;
                userReminderInfo.ReminderInfoType = ReminderInfoType.Get(ReminderInfoTypeIds.Instance().Notice());
                IEnumerable<ReminderInfo> reminderInfos = userReminderInfos_object.Where(n => n.UserId == userId).Select(n => new
                ReminderInfo { ObjectId = n.Id, Title = n.ResolvedBody, DateCreated = n.DateCreated });
                foreach (var reminderInfo in reminderInfos)
                {
                    userReminderInfo.Append(reminderInfo);
                }
                userRminderInfos.Add(userReminderInfo);
            }
            return userRminderInfos;
        }

        #region Help Methods

        /// <summary>
        /// 获取用户未处理通知数的CacheKey
        /// </summary>
        /// <param name="userId">用户Id</param>
        private string GetCacheKey_UnhandledNoticeCount(long userId)
        {
            return string.Format("UnhandledNoticeCount::UserId-{0}", userId);
        }

        #endregion

    }
}
