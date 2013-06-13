//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common
{  /// <summary>
    /// 私信会话数据访问接口
    /// </summary>
    public interface IMessageSessionRepository : IRepository<MessageSession>
    {
        /// <summary>
        /// 获取用户私信会话
        /// </summary>
        ///<param name="userId">用户Id</param>
        ///<param name="pageIndex">当前页码</param>
        PagingDataSet<MessageSession> GetSessionsOfUser(long userId, int pageIndex);

        /// <summary>
        /// 获取私信会话分页数据（后台用）
        /// </summary>
        /// <param name="type">私信类型</param>
        /// <param name="userId">用户Id（用来搜索用户Id相关的私信会话）</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>私信会话分页数据</returns>
        PagingDataSet<MessageSession> GetSessions(MessageType? type, long? userId, int pageIndex);


        /// <summary>
        /// 获取前N个私信会话
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">获取记录条数</param>
        /// <param name="hasUnread">是否仅获取未读会话</param>
        IEnumerable<MessageSession> GetTopSessions(long userId, int topNumber, bool hasUnread = false);

        /// <summary>
        /// 清除用户所有私信会话
        /// </summary>
        /// <param name="userId">用户Id</param>
        void ClearSessionsFromUser(long userId);

        /// <summary>
        /// 获取客服消息
        /// </summary>
        /// <param name="userId">发件人</param>
        /// <param name="roleName">角色</param>
        /// <param name="content">内容</param>
        /// <param name="minRank">最小等级</param>
        /// <param name="maxRank">最大等级</param>
        /// <returns></returns>
        PagingDataSet<MessageSession> GetCustomerMessages(int pageIndex, int pageSize, string userName, string roleName,  long minRank, long maxRank);
    }
}
