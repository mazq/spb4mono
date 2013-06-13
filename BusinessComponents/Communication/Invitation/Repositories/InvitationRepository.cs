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
    /// 请求数据访问
    /// </summary>
    public class InvitationRepository : Repository<Invitation>, IInvitationRepository
    {
        private int PageSize = 30;
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 创建请求的方法
        /// </summary>
        /// <param name="entity">请求实体</param>
        /// <returns>刚插入数据的id</returns>
        public override object Insert(Tunynet.Common.Invitation entity)
        {
            base.Insert(entity);

            //确定已经将数据插入到数据库因为id已经更改
            if (entity.Id > 0)
            {
                string cacheKey = GetCacheKey_UnhandledInvitationCount(entity.UserId);
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
        /// 根据实体的id删除请求
        /// </summary>
        /// <param name="entityId">实体的id</param>
        /// <returns>被删除的条数</returns>
        public override int DeleteByEntityId(object entityId)
        {
            Invitation invitation = Get(entityId);

            int affectCount = base.Delete(invitation);

            string cacheKey = GetCacheKey_UnhandledInvitationCount(invitation.UserId);
            int? count = cacheService.Get(cacheKey) as int?;
            if (count != null)
            {
                count--;
                if (count >= 0 && invitation.Status == InvitationStatus.Unhandled)
                {
                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
                }

            }
            return affectCount;
        }

        /// <summary>
        /// 更新请求状态
        /// </summary>
        /// <param name="invitation">状态的id</param>
        /// <param name="status">要更新的状态</param>
        public void SetStatus(Invitation invitation, InvitationStatus status)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("update tn_Invitations set Status=@0 where id=@1", (int)status, invitation.Id);
            CreateDAO().Execute(sql);
            #region 处理缓存
            string cacheKey = GetCacheKey_UnhandledInvitationCount(invitation.UserId);
            int? count = cacheService.Get(cacheKey) as int?;
            if (count != null)
            {
                count--;
                if (count >= 0)
                {
                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
                }
            }

            //解决非分布式缓存情况下更新实体缓存问题
            invitation.Status = status;
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", invitation.UserId);
            RealTimeCacheHelper.IncreaseEntityCacheVersion(invitation.Id);
            #endregion

        }

        /// <summary>
        /// 批量更改处理状态
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="status">要更改的状态</param>
        public void BatchSetStatus(long userId, InvitationStatus status)
        {
            PetaPocoDatabase dao = CreateDAO();
            var sql = PetaPoco.Sql.Builder;
            sql.Select("Id")
                .From("tn_Invitations")
                .Where("UserId=@0 and Status=@1", userId, (int)InvitationStatus.Unhandled);

            dao.OpenSharedConnection();
            IEnumerable<object> ids = dao.FetchFirstColumn(sql);
            sql = PetaPoco.Sql.Builder;
            sql.Append("update tn_Invitations set Status=@0 where UserId=@1 and Status =@2", (int)status, userId, (int)InvitationStatus.Unhandled);
            dao.Execute(sql);
            dao.CloseSharedConnection();

            //维护缓存
            string cacheKey = GetCacheKey_UnhandledInvitationCount(userId);
            int? count = cacheService.Get(cacheKey) as int?;
            if (count != null)
            {
                count = 0;
                cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
            }
            //更新列表缓存
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
            //更新实体缓存
            foreach (var id in ids)
            {
                Invitation invitation = Get(id);
                if (invitation != null)
                {
                    invitation.Status = status;
                }
                RealTimeCacheHelper.IncreaseEntityCacheVersion(id);
            }
        }

        /// <summary>
        /// 清空接受人的通知记录
        /// </summary>
        /// <param name="userId">用户的id</param>
        public void ClearAll(long userId)
        {
            var sql = PetaPoco.Sql.Builder.Append("delete from tn_Invitations where UserId=@0 and Status <> @1", userId, (int)InvitationStatus.Unhandled);
            CreateDAO().Execute(sql);

            //更新列表缓存缓存
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        /// <summary>
        /// 根据用户删除用户的所有记录（删除用户时使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            List<Sql> sql_Delete = new List<Sql>();
            sql_Delete.Add(PetaPoco.Sql.Builder.Append("delete from tn_Invitations where UserId = @0 or SenderUserId = @0", userId));
            sql_Delete.Add(PetaPoco.Sql.Builder.Append("delete from tn_UserInvitationSettings where UserId=@0", userId));
            CreateDAO().Execute(sql_Delete);
        }

        /// <summary>
        /// 获取用户未处理的请求数目
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUnhandledCount(long userId)
        {
            //从缓存中读取未处理数目
            string cacheKey = GetCacheKey_UnhandledInvitationCount(userId);
            int? count = cacheService.Get(cacheKey) as int?;
            //如果缓存中没有对应数目从数据库中查询
            if (count == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Count(*)")
                    .From("tn_Invitations")
                    .Where("UserId=@0 and Status=@1", userId, (int)InvitationStatus.Unhandled);
                count = CreateDAO().FirstOrDefault<int?>(sql);

                if (count != null)
                    cacheService.Add(cacheKey, count, CachingExpirationType.SingleObject);
            }
            return count ?? 0;
        }

        /// <summary>
        /// 获取用户最近几条未处理的通知
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="topNumber">获取最前面的条数</param>
        /// <returns></returns>
        public IEnumerable<Invitation> GetTops(long userId, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                 () =>
                 {
                     StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
                     cacheKey.Append("Invitations");
                     return cacheKey.ToString();
                 },
                 () =>
                 {
                     var sql = PetaPoco.Sql.Builder;
                     sql.Where("UserId=@0 and Status=@1", userId, (int)InvitationStatus.Unhandled);
                     sql.OrderBy("Id  DESC");
                     return sql;
                 });
        }

        /// <summary>
        /// 获取用户请求的分页集合
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="status">通知状态</param>
        /// <param name="invitationTypeKey">通知类型</param>
        /// <param name="applicationId">应用id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>通知分页集合</returns>
        public PagingDataSet<Invitation> Gets(long userId, InvitationStatus? status, string invitationTypeKey, int? applicationId, int? pageIndex)
        {
            return GetPagingEntities(PageSize, pageIndex ?? 1, CachingExpirationType.ObjectCollection,
                //获取CacheKey
                            () =>
                            {
                                StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));

                                cacheKey.AppendFormat("Status-{0}:InvitationTypeKey-{1}:ApplicationId-{2}", (int?)status, invitationTypeKey, applicationId);
                                return cacheKey.ToString();
                            },
                //生成PetaPoco.Sql
                            () =>
                            {
                                var sql = PetaPoco.Sql.Builder;
                                sql.Where(" UserId = @0", userId);

                                if (status.HasValue)
                                    sql.Where("Status = @0", (int)status);

                                if (invitationTypeKey != string.Empty && invitationTypeKey != null)
                                    sql.Where("InvitationTypeKey=@0", invitationTypeKey);

                                if (applicationId != null)
                                    sql.Where("ApplicationId=@0", applicationId);
                                sql.OrderBy("Status,Id  DESC");
                                return sql;
                            });
        }

        /// <summary>
        /// 获取请求需提醒信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserReminderInfo> GetUserReminderInfos()
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("tn_Invitations.*")
            .From("tn_Invitations")
            .InnerJoin("tn_Users")
            .On("tn_Invitations.UserId = tn_Users.UserId")
            .Where("tn_Invitations.Status = 0")
            .Where("tn_Users.IsEmailVerified = 1")
            .OrderBy("tn_Invitations.UserId")
            .OrderBy("tn_Invitations.Id");

            IEnumerable<Invitation> userReminderInfos_object = CreateDAO().Fetch<Invitation>(sql);
            List<UserReminderInfo> userRminderInfos = new List<UserReminderInfo>();
            foreach (long userId in userReminderInfos_object.Select(n => n.UserId).Distinct())
            {
                UserReminderInfo userReminderInfo = new UserReminderInfo();
                userReminderInfo.UserId = userId;
                userReminderInfo.ReminderInfoType = ReminderInfoType.Get(ReminderInfoTypeIds.Instance().Invitation());
                IEnumerable<ReminderInfo> reminderInfos = userReminderInfos_object.Where(n => n.UserId == userId).Select(n => new
                ReminderInfo { ObjectId = n.Id, Title = n.GetResolvedBody(), DateCreated = n.DateCreated });
                foreach (var reminderInfo in reminderInfos)
                {
                    userReminderInfo.Append(reminderInfo);
                }
                userRminderInfos.Add(userReminderInfo);
            }
            return userRminderInfos;
        }

        /// <summary>
        /// 获取我请求过的用户id
        /// </summary>
        /// <param name="senderUserId">发送者Id</param>
        /// <param name="invitationTypeKey">发送种类id</param>
        /// <param name="applicationId">applicationId</param>
        /// <returns>接受我发送请求的用户id</returns>
        public IEnumerable<long> GetMyInvitationUserId(long senderUserId, string invitationTypeKey, int applicationId)
        {
            string cackey = GetCacheKey_GetInvitationAcceptUserIds(senderUserId, invitationTypeKey, applicationId);
            List<long> AcceptUserIds = cacheService.Get<List<long>>(cackey);
            if (AcceptUserIds != null)
                return AcceptUserIds;
            var sql_Select = Sql.Builder
                                .Select("UserId")
                                .From("tn_Invitations")
                                .Where("SenderUserId = @0", senderUserId)
                                .Where("InvitationTypeKey = @0", invitationTypeKey)
                                .Where("ApplicationId = @0", applicationId);
            IEnumerable<object> AcceptUserIdsObject = CreateDAO().FetchFirstColumn(sql_Select);
            if (AcceptUserIdsObject != null && AcceptUserIdsObject.Count() > 0)
            {
                AcceptUserIds = AcceptUserIdsObject.Select(n => (long)n).ToList();
                cacheService.Set(cackey, AcceptUserIds, CachingExpirationType.ObjectCollection);
            }
            return AcceptUserIds;
        }

        /// <summary>
        /// 根据发送者获取接受者的idCacheKey
        /// </summary>
        /// <param name="senderUserId">发送人id</param>
        /// <param name="invitationTypeKey">请求种类</param>
        /// <param name="applicationId">applicationId</param>
        /// <returns>发送者获取接受者的idCacheKey</returns>
        private string GetCacheKey_GetInvitationAcceptUserIds(long senderUserId, string invitationTypeKey, int applicationId)
        {
            string cacheKeyPrefix = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "SenderUserId", senderUserId);
            return string.Format("{0}::invitationTypeKey-{1};applicationId-{2}", cacheKeyPrefix, invitationTypeKey, applicationId);
        }

        /// <summary>
        /// 获取请求条数的缓存名称
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <returns>缓存名称</returns>
        private string GetCacheKey_UnhandledInvitationCount(long userId)
        {
            return string.Format("UnhandledInvitationCount::UserId-{0}", userId);
        }
    }
}
