//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 邀请码配额数据访问类
    /// </summary>
    public interface IInvitationCodeStatisticsRepository : IRepository<InvitationCodeStatistic>
    {
        /// <summary>
        /// 变更用户邀请码配额
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userInvitationCodeUnUsedCount">用户未使用邀请码配额增量（若减少请使用负数）</param>
        /// <param name="userInvitationCodeUsedCount">用户使用的邀请码配额增量（若减少请使用负数）</param>
        /// <param name="userInvitationCodeBuyedCount">用户购买的邀请码配额增量（若减少请使用负数）</param>
        /// <returns>是否更新成功</returns>
        bool ChangeUserInvitationCodeCount(long userId, int userInvitationCodeUnUsedCount, int userInvitationCodeUsedCount, int userInvitationCodeBuyedCount);

        /// <summary>
        /// 删除用户的所有邀请好友记录（删除用户的时候使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        void CleanByUser(long userId);
    }
}
