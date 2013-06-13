//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 邀请码配额的数据访问类
    /// </summary>
    public class InvitationCodeStatisticsRepository : Repository<InvitationCodeStatistic>, IInvitationCodeStatisticsRepository
    {
        
        //回复：此方法名。郑伟定的。等我问问郑伟。
        /// <summary>
        /// 变更用户邀请码配额
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userInvitationCodeUnUsedCount">用户未使用邀请码配额增量（若减少请使用负数）</param>
        /// <param name="userInvitationCodeUsedCount">用户使用的邀请码配额增量（若减少请使用负数）</param>
        /// <param name="userInvitationCodeBuyedCount">用户购买的邀请码配额增量（若减少请使用负数）</param>
        /// <returns>是否更新成功</returns>
        public bool ChangeUserInvitationCodeCount(long userId, int userInvitationCodeUnUsedCount, int userInvitationCodeUsedCount, int userInvitationCodeBuyedCount)
        {
            InvitationCodeStatistic invitationCodeStatistic = Get(userId);
            if (invitationCodeStatistic == null)
            {
                IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
                InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
                invitationCodeStatistic = new InvitationCodeStatistic
                  {
                      CodeBuyedCount = 0,
                      CodeUnUsedCount = inviteFriendSettings.DefaultUserInvitationCodeCount,
                      CodeUsedCount = 0,
                      UserId = userId
                  };
                Insert(invitationCodeStatistic);
            }
            invitationCodeStatistic.CodeBuyedCount += userInvitationCodeBuyedCount;
            invitationCodeStatistic.CodeUnUsedCount += userInvitationCodeUnUsedCount;
            invitationCodeStatistic.CodeUsedCount += userInvitationCodeUsedCount;
            //done:bianchx by libsh,怎么能防止这些统计数被减到负数
            //回复：在业务逻辑类中做了拦截，请查看业务逻辑类中的ChangeUserInvitationCodeCount方法
            var sql_Update = PetaPoco.Sql.Builder
                .Append("update tn_InvitationCodeStatistics set CodeUnUsedCount=CodeUnUsedCount+@0,CodeUsedCount=CodeUsedCount+@1,CodeBuyedCount=CodeBuyedCount+@2 where UserId=@3", userInvitationCodeUnUsedCount, userInvitationCodeUsedCount, userInvitationCodeBuyedCount, userId);
            //done:bianchx by libsh,应该先做数据库操作再处理缓存
            //回复：修改了执行顺序
           int affectedCount = CreateDAO().Execute(sql_Update);
            OnUpdated(invitationCodeStatistic);
            return affectedCount > 0;
        }

        /// <summary>
        /// 删除用户的所有邀请好友记录（删除用户的时候使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            var sql_Delete = PetaPoco.Sql.Builder
                .Append("delete from tn_InvitationCodeStatistics where UserId =@0", userId);
            CreateDAO().Execute(sql_Delete);
        }
    }
}
