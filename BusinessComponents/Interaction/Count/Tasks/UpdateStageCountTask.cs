//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Linq;
using Tunynet.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Tunynet.Common
{
    //done:zhengw,by mazq 建议合并DeleteTrashCountPerDaysTask的内容，使用名称：UpdateStageCountTask，通过说明建议在任务中如何配置执行时间
    //zhengw回复：已修改

    /// <summary>
    /// 每天批量更新阶段计数任务
    /// </summary>
    public class UpdateStageCountTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            SystemDataService systemDataService = new SystemDataService();

            foreach (var perDayTenantTypeId in StageCountTypeManager.GetAllTenantTypeIds())
            {
                var stageCountTypeManager = StageCountTypeManager.Instance(perDayTenantTypeId);
                ConcurrentDictionary<string, List<int>> allStageCountTypes = stageCountTypeManager.GetAllStageCountTypes();

                foreach (var base2StageCountType in allStageCountTypes)
                {
                    //批量更新计数表中的阶段计数
                    Dictionary<string, int> countType2Days = base2StageCountType.Value.ToDictionary(n => stageCountTypeManager.GetStageCountType(base2StageCountType.Key, n), n => n);
                    new CountRepository().UpdateStageCountPerDay(perDayTenantTypeId, base2StageCountType.Key, countType2Days);
                    //删除每日计数表中的过期的历史计数记录
                    int maxValue = stageCountTypeManager.GetMaxDayCount(base2StageCountType.Key);
                    new CountRepository().DeleteTrashCountPerDays(perDayTenantTypeId, base2StageCountType.Key, maxValue);
                }
            }
        }
    }
}
