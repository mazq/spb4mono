//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 通知数据访问接口
    /// </summary>
    public interface INoticeRepository : IRepository<Notice>
    {
        /// <summary>
        /// 清空接收人的通知记录
        /// </summary>
        /// <param name="userId">接收人Id</param>
        /// <param name="status">通知状态</param>
        void ClearAll(long userId, NoticeStatus? status = null);

        /// <summary>
        /// 删除用户的记录（删除用户时调用）
        /// </summary>
        /// <param name="userId"></param>
        void CleanByUser(long userId);

        /// <summary>
        /// 将通知设置为已处理状态
        /// </summary>
        /// <param name="id">通知Id</param>
        void SetIsHandled(long id);

        /// <summary>
        /// 批量将所有未处理通知修改为已处理状态
        /// </summary>
        /// <param name="userId">接收人Id</param>
        void BatchSetIsHandled(long userId);

        /// <summary>
        /// 获取某人的未处理通知数
        /// </summary>
        int GetUnhandledCount(long userId);

        /// <summary>
        /// 获取用户最近几条未处理的通知
        /// </summary>
        /// <param name="topNumber"></param>
        /// <param name="userId">通知接收人Id</param>
        IEnumerable<Notice> GetTops(long userId, int topNumber);

        /// <summary>
        /// 获取用户通知的分页集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">通知状态</param>
        /// <param name="typeId">通知类型Id</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>通知分页集合</returns>
        PagingDataSet<Notice> Gets(long userId, NoticeStatus? status, int? typeId, int? applicationId, int pageIndex);

        /// <summary>
        /// 获取通知需提醒信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserReminderInfo> GetUserReminderInfos();
    }
}