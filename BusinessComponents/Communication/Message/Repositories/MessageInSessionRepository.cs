//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;
using System.Text;
using Tunynet.Caching;
using System.Linq;

namespace Tunynet.Common
{
    /// <summary>
    /// 私信与会话关联数据访问
    /// </summary>
    public class MessageInSessionRepository : Repository<MessageInSession>, IMessageInSessionRepository
    {

        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取会话下的所有私信Id
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <param name="topNumber">获取记录数</param>
        public IEnumerable<object> GetMessageIds(long sessionId, int topNumber)
        {
            //获取缓存
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "SessionId", sessionId));
            cacheKey.Append("MessageIds");

            IEnumerable<object> messageIds = cacheService.Get<IEnumerable<object>>(cacheKey.ToString());
            if (messageIds == null)
            {
                //组装sql语句
                var sql = PetaPoco.Sql.Builder;
                sql.Select("MessageId")
                   .From("tn_MessagesInSessions")
                   .Where("SessionId = @0", sessionId)
                   .OrderBy("MessageId desc");

                messageIds = CreateDAO().FetchTopPrimaryKeys(SecondaryMaxRecords, "MessageId", sql);
                cacheService.Add(cacheKey.ToString(), messageIds, CachingExpirationType.ObjectCollection);
            }

            return messageIds.Count() > topNumber ? messageIds.Take(topNumber) : messageIds;
        }
    }
}
