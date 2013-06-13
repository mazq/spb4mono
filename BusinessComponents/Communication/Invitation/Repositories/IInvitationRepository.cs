
using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 请求数据访问接口
    /// </summary>
    public interface IInvitationRepository : IRepository<Invitation>
    {
        /// <summary>
        /// 清空接收人的请求记录
        /// </summary>
        /// <param name="userId">接收人Id</param>
        void ClearAll(long userId);

        /// <summary>
        /// 删除用户的请求数据（删除用户时使用）
        /// </summary>
        /// <param name="userId"></param>
        void CleanByUser(long userId);

        /// <summary>
        /// 批量更改处理状态
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="status">要更改的状态</param>
        void BatchSetStatus(long userId, InvitationStatus status);

        /// <summary>
        /// 获取用户最近几条未处理的通知
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="topNumber">获取最前面的条数</param>
        /// <returns></returns>
        IEnumerable<Invitation> GetTops(long userId, int topNumber);

        /// <summary>
        /// 更新请求状态
        /// </summary>
        /// <param name="invitation">状态的id</param>
        /// <param name="status">要更新的状态</param>
        void SetStatus(Invitation invitation, InvitationStatus status);
        
        /// <summary>
        /// 获取用户未处理的请求数目
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetUnhandledCount(long userId);

        /// <summary>
        /// 获取用户请求的分页集合
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="status">通知状态</param>
        /// <param name="invitationTypeKey">通知类型</param>
        /// <param name="applicationId">应用id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>通知分页集合</returns>
        PagingDataSet<Invitation> Gets(long userId, InvitationStatus? status, string invitationTypeKey, int? applicationId, int? pageIndex);

        /// <summary>
        /// 获取请求需提醒信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserReminderInfo> GetUserReminderInfos();

        /// <summary>
        /// 获取我请求的用户id
        /// </summary>
        /// <param name="senderUserId">发送请求id</param>
        /// <param name="invitationTypeKey">请求类型</param>
        /// <param name="applicationId">applicationId</param>
        /// <returns>我请求过的用户id</returns>
        IEnumerable<long> GetMyInvitationUserId(long senderUserId, string invitationTypeKey, int applicationId);
    }
}