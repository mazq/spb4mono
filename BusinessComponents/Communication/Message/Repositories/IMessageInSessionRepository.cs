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
    /// 私信与会话关联数据访问接口
    /// </summary>
    public interface IMessageInSessionRepository : IRepository<MessageInSession>
    {
        /// <summary>
        /// 获取会话下的所有私信Id
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <param name="topNumber">获取记录数</param>
        IEnumerable<object> GetMessageIds(long sessionId, int topNumber);
    }
}
