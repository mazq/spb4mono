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
    /// 提醒记录数据访问接口
    /// </summary>
    public interface IReminderRecordRepository : IRepository<ReminderRecord>
    {
        /// <summary>
        /// 创建提醒记录
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderMode">提醒方式</param>
        /// <param name="reminderInfoType">提醒信息类型</param>
        /// <param name="objectIds">提醒对象Id集合</param>
        void CreateRecords(long userId, int reminderMode, int reminderInfoType, IEnumerable<long> objectIds);

        /// <summary>
        /// 更新提醒记录（只更新最后提醒时间）
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        /// <param name="objectIds">提醒对象Id集合</param>
        void UpdateRecoreds(long userId, int reminderModeId, int reminderInfoTypeId, IEnumerable<long> objectIds);

        /// <summary>
        /// 删除用户的信息（删除用户时使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        void CleanByUser(long userId);

        /// <summary>
        /// 清除垃圾提醒记录
        /// </summary>
        /// <param name="storageDay">保留天数</param>
        void DeleteTrashRecords(int storageDay);

        /// <summary>
        /// 获取用户所有的提醒记录
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        IEnumerable<ReminderRecord> GetRecords(long userId, int reminderModeId, int reminderInfoTypeId);


    }
}