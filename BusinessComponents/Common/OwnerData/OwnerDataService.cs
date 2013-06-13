//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户数据业务逻辑类
    /// </summary>
    public class OwnerDataService
    {
        #region 构造器

        private IOwnerDataRepository ownerDataRepository;
        private string tenantTypeId;


        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public OwnerDataService(string tenantTypeId)
            : this(tenantTypeId, new OwnerDataRepository())
        {
            this.tenantTypeId = tenantTypeId;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerDataRepository">用户数据仓储</param>
        public OwnerDataService(string tenantTypeId, IOwnerDataRepository ownerDataRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.ownerDataRepository = ownerDataRepository;
        }

        #endregion

        /// <summary>
        /// 变更用户数据
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(long ownerId, string dataKey, long value)
        {
            ownerDataRepository.Change(ownerId, tenantTypeId, dataKey, value);
        }

        /// <summary>
        /// 变更用户数据
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(long ownerId, string dataKey, decimal value)
        {
            ownerDataRepository.Change(ownerId, tenantTypeId, dataKey, value);
        }

        /// <summary>
        /// 变更用户数据
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的值</param>
        public void Change(long ownerId, string dataKey, string value)
        {
            ownerDataRepository.Change(ownerId, tenantTypeId, dataKey, value);
        }

        /// <summary>
        /// 获取DataKey对应的长整形
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="dataKey">DataKey</param>
        /// <returns>dataKey不存在时返回0</returns>
        public long Get(long ownerId, string dataKey)
        {
            OwnerData ownerData = ownerDataRepository.Get(ownerId, tenantTypeId, dataKey);
            if (ownerData == null)
                return default(long);
            return ownerData.LongValue > 0 ? ownerData.LongValue : 0;
        }

        /// <summary>
        /// 获取DataKey对应的长整形
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="dataKey">DataKey</param>
        /// <returns>dataKey不存在时返回0</returns>
        public long GetLong(long ownerId, string dataKey)
        {
            OwnerData ownerData = ownerDataRepository.Get(ownerId, tenantTypeId, dataKey);
            if (ownerData == null)
                return default(long);
            return ownerData.LongValue > 0 ? ownerData.LongValue : 0;
        }

        /// <summary>
        /// 获取DataKey对应的Decimal
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="dataKey">DataKey</param>
        /// <returns>dataKey不存在时返回0</returns>
        public decimal GetDecimal(long ownerId, string dataKey)
        {
            OwnerData ownerData = ownerDataRepository.Get(ownerId, tenantTypeId, dataKey);
            if (ownerData == null)
                return default(decimal);

            return ownerData.DecimalValue > 0 ? ownerData.DecimalValue : 0;
        }

        /// <summary>
        /// 获取DataKey对应的String
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="dataKey">DataKey</param>
        /// <returns>dataKey不存在时返回空字符串</returns>
        public string GetString(long ownerId, string dataKey)
        {
            OwnerData ownerData = ownerDataRepository.Get(ownerId, tenantTypeId, dataKey);
            if (ownerData == null)
                return default(string);

            return ownerData.StringValue;
        }

        /// <summary>
        /// 获取多个数据的总计数
        /// </summary>
        /// <param name="dataKeys">需要获取统计计数的DataKeys</param>
        /// <param name="ownerId">用户Id</param>
        /// <returns></returns>
        public long GetTotalCount(IEnumerable<string> dataKeys, long ownerId)
        {
            if (dataKeys == null)
                return 0;

            long totalCount = 0;
            foreach (string dataKey in dataKeys)
            {
                totalCount += Get(ownerId, dataKey);
            }

            return totalCount > 0 ? totalCount : 0;
        }

        /// <summary>
        /// 获取前N个用户Id数据
        /// </summary>
        /// <param name="dataKey">dataKey</param>
        /// <param name="topNumber">获取记录的个数</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopOwnerIds(string dataKey, int topNumber, OwnerData_SortBy? sortBy = null)
        {
            return ownerDataRepository.GetTopOwnerIds(dataKey, tenantTypeId, topNumber, sortBy);
        }

        /// <summary>
        /// 获取用户Id分页数据
        /// </summary>
        /// <param name="dataKey">dataKey</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingOwnerIds(string dataKey, int pageIndex, OwnerData_SortBy? sortBy = null)
        {
            return ownerDataRepository.GetPagingOwnerIds(dataKey, tenantTypeId, pageIndex, sortBy);
        }

        /// <summary>
        /// 清除指定用户的用户数据
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        public void ClearOwnerData(long ownerId)
        {
            ownerDataRepository.ClearOwnerData(ownerId, tenantTypeId);
        }
    }
}
