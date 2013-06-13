//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 私信数据访问
    /// </summary>
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 把实体entity添加到数据库
        /// </summary>
        /// <param name="entity">待创建实体</param>
        /// <returns>实体主键</returns>
        public override object Insert(Message entity)
        {
            var sql = Sql.Builder;
            IList<Sql> sqls = new List<Sql>();
            long senderSessionId = 0, receiverSessionId = 0;
            MessageSessionRepository messageSessionRepository = new MessageSessionRepository();

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            object id = base.Insert(entity);
            sql.Append("update tn_MessageSessions")
               .Append("set LastMessageId = @0,MessageCount = MessageCount + 1,LastModified = @1", id, entity.DateCreated)
               .Where("UserId = @0", entity.SenderUserId)
               .Where("OtherUserId = @0", entity.ReceiverUserId);

            //判断发件人的会话是否存在
            if (dao.Execute(sql) == 0)
            {
                sql = Sql.Builder;
                sql.Append(@"insert into tn_MessageSessions(UserId,OtherUserId,LastMessageId,MessageType,MessageCount,LastModified) values (@0,@1,@2,@3,1,@4)", entity.SenderUserId, entity.ReceiverUserId, id, (int)entity.MessageType, entity.DateCreated);
                dao.Execute(sql);
            }

            //获取发件人的会话Id
            sql = Sql.Builder;
            sql.Select("SessionId")
               .From("tn_MessageSessions")
               .Where("UserId = @0", entity.SenderUserId)
               .Where("OtherUserId = @0", entity.ReceiverUserId);
            senderSessionId = dao.FirstOrDefault<long>(sql);

            //判断收件人的会话是否存在
            sql = Sql.Builder;
            sql.Append("update tn_MessageSessions")
               .Append("set LastMessageId = @0,MessageCount = MessageCount + 1,UnreadMessageCount = UnreadMessageCount + 1,LastModified = @1", id, entity.DateCreated)
               .Where("UserId = @0", entity.ReceiverUserId)
               .Where("OtherUserId = @0", entity.SenderUserId);
            if (dao.Execute(sql) == 0)
            {
                sql = Sql.Builder;
                sql.Append("insert into tn_MessageSessions(UserId,OtherUserId,LastMessageId,MessageType,MessageCount,UnreadMessageCount,LastModified) values (@0,@1,@2,@3,1,1,@4)", entity.ReceiverUserId, entity.SenderUserId, id, (int)entity.MessageType, entity.DateCreated);
                dao.Execute(sql);
            }

            //获取收件人的会话Id
            sql = Sql.Builder;
            sql.Select("SessionId")
               .From("tn_MessageSessions")
               .Where("UserId = @0", entity.ReceiverUserId)
               .Where("OtherUserId = @0", entity.SenderUserId);
            receiverSessionId = dao.FirstOrDefault<long>(sql);

            //添加会话与私信的关系
            sqls.Add(Sql.Builder.Append("insert into tn_MessagesInSessions (SessionId,MessageId) values (@0,@1)", senderSessionId, id));
            sqls.Add(Sql.Builder.Append("insert into tn_MessagesInSessions (SessionId,MessageId) values (@0,@1)", receiverSessionId, id));
            dao.Execute(sqls);

            dao.CloseSharedConnection();

            #region 缓存处理

            if (RealTimeCacheHelper.EnableCache)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("UserId", entity.SenderUserId);
                RealTimeCacheHelper.IncreaseAreaVersion("UserId", entity.ReceiverUserId);

                var realTimeCacheHelper = EntityData.ForType(typeof(MessageInSession)).RealTimeCacheHelper;
                realTimeCacheHelper.IncreaseAreaVersion("SessionId", senderSessionId);
                realTimeCacheHelper.IncreaseAreaVersion("SessionId", receiverSessionId);

                realTimeCacheHelper = EntityData.ForType(typeof(MessageSession)).RealTimeCacheHelper;
                realTimeCacheHelper.IncreaseAreaVersion("UserId", entity.SenderUserId);
                realTimeCacheHelper.IncreaseAreaVersion("UserId", entity.ReceiverUserId);

                realTimeCacheHelper.IncreaseEntityCacheVersion(senderSessionId);
                realTimeCacheHelper.IncreaseEntityCacheVersion(receiverSessionId);


                string cacheKey = realTimeCacheHelper.GetCacheKeyOfEntity(senderSessionId);
                MessageSession senderSession = cacheService.Get<MessageSession>(cacheKey);
                if (senderSession != null)
                {
                    senderSession.LastMessageId = entity.MessageId;
                    senderSession.LastModified = entity.DateCreated;
                    senderSession.MessageCount++;

                }

                cacheKey = realTimeCacheHelper.GetCacheKeyOfEntity(receiverSessionId);
                MessageSession receiverSession = cacheService.Get<MessageSession>(cacheKey);
                if (receiverSession != null)
                {
                    receiverSession.LastMessageId = entity.MessageId;
                    receiverSession.LastModified = entity.DateCreated;
                    receiverSession.MessageCount++;
                    receiverSession.UnreadMessageCount++;
                }

                cacheKey = GetCacheKey_UnreadCount(entity.ReceiverUserId);
                int? count = cacheService.Get(cacheKey) as int?;
                count = count ?? 0;
                count++;
                cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
            }
            #endregion 缓存处理

            return id;
        }

        /// <summary>
        /// 从数据库中删除实体
        /// </summary>
        /// <param name="entity">待删除私信实体</param>
        /// <param name="userId">私信会话拥有者</param>
        /// <param name="sessionId">私信会话Id</param>
        /// <returns>操作后影响行数</returns>
        public int Delete(Message entity, long sessionId)
        {
            var sql = Sql.Builder;
            IList<Sql> sqls = new List<Sql>();
            int affectCount = 0;

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            List<string> dd = new List<string>();

            sql.From("tn_MessageSessions")
               .Where("(UserId = @0 and OtherUserId = @1) or (UserId = @1 and OtherUserId = @0)", entity.SenderUserId, entity.ReceiverUserId);

            List<MessageSession> sessions = dao.Fetch<MessageSession>(sql);

            if (sessions.Count > 0)
            {
                //处理相关会话的计数，如果会话中仅当前一条私信则删除会话
                foreach (var session in sessions)
                {
                    if (session.SessionId != sessionId)
                        continue;

                    if (session.MessageCount > 1)
                    {
                        sqls.Add(Sql.Builder.Append("update tn_MessageSessions")
                                            .Append("set MessageCount = MessageCount - 1")
                                            .Where("SessionId = @0", session.SessionId));
                    }
                    else
                    {
                        sqls.Add(Sql.Builder.Append("delete from tn_MessageSessions where SessionId = @0", session.SessionId));
                    }

                    //删除会话与私信的关系
                    sqls.Add(Sql.Builder.Append("delete from tn_MessagesInSessions where SessionId = @0 and MessageId = @1", session.SessionId, entity.MessageId));
                }

                using (var transaction = dao.GetTransaction())
                {
                    affectCount = dao.Execute(sqls);
                    if (sessions.Count == 1)
                    {
                        affectCount += base.Delete(entity);
                    }
                    else
                    {
                        foreach (var session in sessions)
                        {
                            EntityData.ForType(typeof(MessageInSession)).RealTimeCacheHelper.IncreaseAreaVersion("SessionId", session.SessionId);
                        }
                    }

                    transaction.Complete();
                }
            }

            dao.CloseSharedConnection();

            #region 更新缓存

            //更新私信会话的缓存
            var sessionCacheHelper = EntityData.ForType(typeof(MessageSession)).RealTimeCacheHelper;
            sessionCacheHelper.IncreaseAreaVersion("UserId", entity.SenderUserId);
            sessionCacheHelper.IncreaseAreaVersion("UserId", entity.ReceiverUserId);
            sessions.ForEach((n) =>
            {
                sessionCacheHelper.IncreaseEntityCacheVersion(n.SessionId);
            });

            #endregion 更新缓存

            return affectCount;
        }

        /// <summary>
        /// 更新私信的阅读状态
        /// </summary>
        /// <param name="sessionId">私信会话Id</param>
        /// <param name="userId">会话拥有者UserId</param>
        public bool SetIsRead(long sessionId, long userId)
        {
            var sql = Sql.Builder;
            bool isRead = false;

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            sql.Select("MessageId")
               .From("tn_Messages")
               .Append("Where ReceiverUserId = @0 and IsRead = 0", userId)
               .Append(" and exists(select 1 from tn_MessagesInSessions where tn_Messages.MessageId = tn_MessagesInSessions.MessageId and tn_MessagesInSessions.SessionId = @0)", sessionId);

            //获取未读私信Id
            List<long> ids = dao.Fetch<long>(sql);

            if (ids.Count() > 0)
            {
                sql = Sql.Builder.Append("update tn_Messages")
                            .Append("Set IsRead = 1")
                            .Where("MessageId in (@MessageIds)", new { MessageIds = ids });

                dao.Execute(sql);

                

                //更新会话的未读信息数
                MessageSessionRepository repository = new MessageSessionRepository();
                MessageSession session = repository.Get(sessionId);
                if (session != null)
                {
                    session.UnreadMessageCount = session.UnreadMessageCount > ids.Count() ? session.UnreadMessageCount - ids.Count() : 0;
                    repository.Update(session);
                }

                #region 处理缓存

                RealTimeCacheHelper.IncreaseAreaVersion("ReceiverUserId", userId);

                string cacheKey = GetCacheKey_UnreadCount(userId);
                int? count = cacheService.Get(cacheKey) as int?;
                if (count != null)
                {
                    if (count >= ids.Count && count > 0)
                        count -= ids.Count;

                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
                }

                #endregion 处理缓存

                isRead = true;
            }

            dao.CloseSharedConnection();

            return isRead;
        }

        /// <summary>
        /// 获取未读私信数
        /// </summary>
        public int GetUnReadCount(long userId)
        {
            string cacheKey = GetCacheKey_UnreadCount(userId);

            int? count = cacheService.Get(cacheKey) as int?;
            if (count == null)
            {
                var sql = Sql.Builder;
                sql.Select("Count(MessageId)")
                   .From("tn_Messages")
                   .Where("ReceiverUserId = @0", userId)
                   .Where("IsRead = 0");
                count = CreateDAO().FirstOrDefault<int?>(sql);

                count = count ?? 0;
                cacheService.Add(cacheKey, count, CachingExpirationType.SingleObject);
            }

            return count.Value;
        }

        /// <summary>
        /// 获取用户的前N条私信
        /// </summary>
        /// <param name="userId">私信拥有者Id</param>
        /// <param name="sortBy">私信排序字段</param>
        /// <param name="topNumber">获取的前N条数据</param>
        public IEnumerable<Message> GetTopMessagesOfUser(long userId, SortBy_Message? sortBy, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                   () =>
                   {
                       //获取缓存
                       StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ReceiverUserId", userId));
                       cacheKey.AppendFormat("SortBy-{0}", (int)sortBy);

                       return cacheKey.ToString();
                   },
                   () =>
                   {
                       var sql = PetaPoco.Sql.Builder;
                       sql.Where("ReceiverUserId = @0", userId);
                       switch (sortBy)
                       {
                           case SortBy_Message.IsRead:
                               sql.OrderBy("IsRead asc");
                               sql.OrderBy("MessageId desc");
                               break;
                           case SortBy_Message.DateCreated_Desc:
                               sql.OrderBy("MessageId desc");
                               break;
                       }

                       return sql;
                   });
        }

        /// <summary>
        /// 获取私信需提醒信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserReminderInfo> GetUserReminderInfos()
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("tn_Messages.*")
            .From("tn_Messages")
            .InnerJoin("tn_Users")
            .On("tn_Messages.ReceiverUserId = tn_Users.UserId")
            .Where("tn_Users.IsEmailVerified = 1")
            .Where("tn_Messages.IsRead = 0")
            .OrderBy("tn_Messages.ReceiverUserId")
            .OrderBy("tn_Messages.MessageId");
            IEnumerable<Message> userReminderInfos_object = CreateDAO().Fetch<Message>(sql);
            List<UserReminderInfo> userRminderInfos = new List<UserReminderInfo>();
            foreach (long userId in userReminderInfos_object.Select(n => n.ReceiverUserId).Distinct())
            {
                UserReminderInfo userReminderInfo = new UserReminderInfo();
                userReminderInfo.UserId = userId;
                userReminderInfo.ReminderInfoType = ReminderInfoType.Get(ReminderInfoTypeIds.Instance().Message());
                IEnumerable<ReminderInfo> reminderInfos = userReminderInfos_object.Where(n => n.ReceiverUserId == userId).Select(n => new
                ReminderInfo { ObjectId = n.MessageId, Title = string.Format("{0}:{1}", n.Sender, n.Body), DateCreated = n.DateCreated });
                foreach (var reminderInfo in reminderInfos)
                {
                    userReminderInfo.Append(reminderInfo);
                }
                userRminderInfos.Add(userReminderInfo);
            }
            return userRminderInfos;
        }

        /// <summary>
        /// 组装未读通知数CacheKey
        /// </summary>
        /// <param name="userId">私信收件人UserId</param>
        private string GetCacheKey_UnreadCount(long userId)
        {
            return "UnreadCount::UserId" + userId;
        }
    }
}