//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common
{

    /// <summary>
    /// 私信数据访问接口
    /// </summary>
    public interface IMessageRepository : IRepository<Message>
    {

        /// <summary>
        /// 从数据库中删除实体
        /// </summary>
        /// <param name="entity">待删除私信实体</param>
        /// <param name="userId">私信会话拥有者</param>
        /// <param name="sessionId">私信会话Id</param>
        /// <returns>操作后影响行数</returns>
        int Delete(Message entity, long sessionId);

        /// <summary>
        /// 更新私信的阅读状态
        /// </summary>
        /// <param name="sessionId">私信会话Id</param>
        /// <param name="userId">会话拥有者UserId</param>
        bool SetIsRead(long sessionId, long userId);

        /// <summary>
        /// 获取用户的前N条私信
        /// </summary>
        /// <param name="userId">私信拥有者Id</param>
        /// <param name="sortBy">私信排序字段</param>
        /// <param name="topNumber">获取的前N条数据</param>
        IEnumerable<Message> GetTopMessagesOfUser(long userId, SortBy_Message? sortBy, int topNumber);

        /// <summary>
        /// 获取未读私信数
        /// </summary>
        int GetUnReadCount(long userId);

        /// <summary>
        /// 获取所有未读的私信
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserReminderInfo> GetUserReminderInfos();
    }
}