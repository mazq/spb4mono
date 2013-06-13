//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Repositories;


namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 邀请码数据访问接口
    /// </summary>
    public interface IInvitationCodesRepository : IRepository<InvitationCode>
    {

        /// <summary>
        /// 删除邀请码（当邀请码被使用时进行调用）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="invitationCode">邀请码</param>
        bool DeleteInvitationCode(long userId, string invitationCode);

        //done:bianchx by libsh,注释不全
        //回复：添加全面了
        /// <summary>
        /// 获取我的未使用邀请码列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>未使用邀请码列表</returns>
        PagingDataSet<InvitationCode> GetMyInvitationCodes(long userId, int pageIndex);

        /// <summary>
        /// 删除用户的所有邀请好友记录（删除用户的时候使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        void CleanByUser(long userId);

        /// <summary>
        /// 批量删除过期的邀请码
        /// </summary>
        void DeleteTrashInvitationCodes();

        //done:bianchx by libsh,返回值说明的太模糊和名字对不起来
        //回复：修改了注释
        /// <summary>
        /// 获取今天的邀请码
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>今天的邀请码</returns>
        string GetTodayCode(long userId);
    }
}
