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
using Tunynet.Repositories;

namespace Tunynet.Common
{
    ///<summary>
    ///计数仓储接口
    ///</summary>
    public interface ICountRepository
    {
        /// <summary>
        /// 注册计数
        /// </summary>
        void CheckCountTable(string tenantTypeId);

        /// <summary>
        /// 注册每日计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        void CheckCountPerDayTable(string tenantTypeId);

        /// <summary>
        /// 调整计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="objectId">计数对象Id</param>
        /// <param name="ownerId">ownerId</param>
        /// <param name="changeCount">变化数</param>
        /// <param name="stageCountTypes">阶段计数集合</param>
        /// <param name="isRealTime">是否立即更新显示计数</param>
        void ChangeCount(string tenantTypeId, string countType, long objectId, long ownerId, int changeCount = 1, IList<string> stageCountTypes = null, bool isRealTime = false);

        /// <summary>
        /// 执行队列
        /// </summary>
        void ExecQueue();

        /// <summary>
        /// 批量更新计数表中的阶段计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="countType2Days">阶段计数类型-统计天数字典集合</param>
        void UpdateStageCountPerDay(string tenantTypeId, string countType, Dictionary<string, int> countType2Days);

        /// <summary>
        /// 删除每日计数表中的过期的历史计数记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="maxValue">保留记录的最大天数</param>
        void DeleteTrashCountPerDays(string tenantTypeId, string countType, int maxValue);

        /// <summary>
        /// 获取计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="objectId">计数对象Id</param>
        int Get(string tenantTypeId, string countType, long objectId);

        /// <summary>
        /// 删除计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="objectId">计数对象Id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        bool Delete(string tenantTypeId, long objectId);

        /// <summary>
        /// 获取计数对象Id集合
        /// </summary>
        /// <param name="topNumber">前几条</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns>计数对象Id集合</returns>
        IEnumerable<long> GetTops(int topNumber, string tenantTypeId, string countType, long? ownerId = null);

        /// <summary>
        /// 获取技术
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">技术类型</param>
        /// <param name="ownerId">拥有者id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PagingEntityIdCollection Gets(string tenantTypeId, string countType, long? ownerId = null, int pageIndex = 1);

        /// <summary>
        /// 获取计数对象Id的所有每天计数记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <returns>每天计数记录集合</returns>
        IEnumerable<CountPerDayEntity> GetAllCountPerDays(string tenantTypeId, string countType);

        /// <summary>
        /// 获取计数表名
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>计数表名</returns>
        string GetTableName_Counts(string tenantTypeId);
        /// <summary>
        /// 获取每日计数表名
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>每日计数表名</returns>
        string GetTableName_CountsPerDay(string tenantTypeId);

    }
}
