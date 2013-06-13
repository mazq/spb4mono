//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 邀请好友记录表
    /// </summary>
    public interface IInviteFriendRecordsRepository : IRepository<InviteFriendRecord>
    {
        /// <summary>
        /// 获取我的邀请好友记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>被邀请的好友Id列表</returns>
        PagingEntityIdCollection GetMyInviteFriendRecords(long userId, int pageIndex = 1);

        /// <summary>
        /// 通过被邀请人ID获取邀请人
        /// </summary>
        /// <param name="userId">被邀请人ID</param>
        /// <returns></returns>
        InviteFriendRecord GetInvitingUserId(long userId);

        /// <summary>
        /// 删除用户的所有邀请好友记录（删除用户的时候使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        void CleanByUser(long userId);

        /// <summary>
        /// 记录邀请用户奖励
        /// </summary>
        /// <param name="userId">用户Id</param>
        void RewardingUser(long userId);
    }
}
