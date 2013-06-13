//using Tunynet.Repositories;
//using System.Collections.Generic;
//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Tunynet.Common
{
    /// <summary>
    /// 阶段计数管理类
    /// </summary>
    public class StageCountTypeManager
    {
        //计数类型-阶段计数统计天数字典集合
        private ConcurrentDictionary<string, List<int>> countType2stageDays = new ConcurrentDictionary<string, List<int>>();

        #region Instance
        private static ConcurrentDictionary<string, StageCountTypeManager> stageCountTypeManagers = new ConcurrentDictionary<string, StageCountTypeManager>();
        private static object lockObject = new object();

        private string tenantTypeId;

        /// <summary>
        /// 通过tenantTypeId获取StageCountTypeManager实例
        /// </summary>
        /// <param name="tenantTypeId">tenantTypeId</param>
        public static StageCountTypeManager Instance(string tenantTypeId)
        {
            if (stageCountTypeManagers.ContainsKey(tenantTypeId))
            {
                return stageCountTypeManagers[tenantTypeId];
            }
            else
            {
                lock (lockObject)
                {
                    StageCountTypeManager StageCountTypeManager = new StageCountTypeManager(tenantTypeId);
                    stageCountTypeManagers[tenantTypeId] = StageCountTypeManager;
                    return StageCountTypeManager;
                }
            }
        }

        private StageCountTypeManager(string tenantTypeId)
        {
            this.tenantTypeId = tenantTypeId;
        }

        #endregion

        /// <summary>
        /// 注册阶段计数
        /// </summary>
        /// <param name="baseCountType">基础阶段计数</param>
        /// <param name="stageDays">阶段计数统计天数集合</param>
        public void AddStageCounts(string baseCountType, params int[] stageDays)
        {
            if (stageDays == null || stageDays.Count() == 0)
                return;
            countType2stageDays[baseCountType] = stageDays.ToList();
        }

        /// <summary>
        /// 解析阶段计数的countType
        /// </summary>
        /// <param name="baseCountType">基础阶段计数</param>
        /// <param name="dayCount">统计天数</param>
        /// <returns></returns>
        public string GetStageCountType(string baseCountType, int dayCount)
        {
            if (!countType2stageDays.ContainsKey(baseCountType) || !countType2stageDays[baseCountType].Contains(dayCount))
                return string.Empty;

            return baseCountType + "-" + dayCount;
        }

        /// <summary>
        /// 获取所有阶段计数类型
        /// </summary>
        /// <param name="baseCountType">基础阶段计数</param>
        /// <returns></returns>
        public List<string> GetStageCountTypes(string baseCountType)
        {
            if (!countType2stageDays.ContainsKey(baseCountType))
                return null;
            return countType2stageDays[baseCountType].Select(n => GetStageCountType(baseCountType, n)).ToList();
        }

        /// <summary>
        /// 获取所有阶段计数类型
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, List<int>> GetAllStageCountTypes()
        {
            return countType2stageDays;
        }


        /// <summary>
        /// 获取每日计数记录的最大保留天数
        /// </summary>
        /// <param name="baseCountType">基础阶段计数</param>
        public int GetMaxDayCount(string baseCountType)
        {
            if (!countType2stageDays.ContainsKey(baseCountType))
                return 0;
            return countType2stageDays[baseCountType].Max();
        }

        /// <summary>
        /// 获取所有使用阶段计数的租户类型Id集合
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAllTenantTypeIds()
        {
            return stageCountTypeManagers.Keys;
        }


    }
}
