//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户数据Repository
    /// </summary>
    public class OwnerDataRepository : Repository<OwnerData>, IOwnerDataRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();
        private int pageSize = 30;

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="ownerId">ownerId</param>
        /// <param name="tenantTypeId">租户类型</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(long ownerId, string tenantTypeId, string dataKey, long value)
        {
            PetaPocoDatabase dao = CreateDAO();

            //当DataKey不存在时，插入新数据
            OwnerData ownerData = Get(ownerId, tenantTypeId, dataKey);
            if (ownerData == null || ownerData.Id == 0)
            {
                ownerData = OwnerData.New();
                ownerData.OwnerId = ownerId;
                ownerData.TenantTypeId = tenantTypeId;
                ownerData.Datakey = dataKey;
                ownerData.LongValue = value;

                Sql sql = Sql.Builder;
                sql.Append("update tn_OwnerData set LongValue = @3 where DataKey = @0 and OwnerId = @1 and TenantTypeId = @2", dataKey, ownerId, tenantTypeId, value);

                int affectCount = dao.Execute(sql);
                if (affectCount == 0)
                {
                    dao.Insert(ownerData);
                }
            }
            else
            {
                ownerData.LongValue = ownerData.LongValue + value > 0 ? ownerData.LongValue + value : 0;
                dao.Update(ownerData);
            }

            string cacheKey = GetCacheKey_GetOwnerData(ownerId, dataKey, tenantTypeId);
            OwnerData cacheOwnerdata = cacheService.Get<OwnerData>(cacheKey);
            if (cacheOwnerdata != null)
            {
                cacheService.Set(cacheKey, ownerData, CachingExpirationType.SingleObject);
            }
        }

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="ownerId">ownerId</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(long ownerId, string tenantTypeId, string dataKey, decimal value)
        {
            PetaPocoDatabase dao = CreateDAO();

            OwnerData ownerData = Get(ownerId, tenantTypeId, dataKey);
            if (ownerData == null || ownerData.Id == 0)
            {
                ownerData = OwnerData.New();
                ownerData.OwnerId = ownerId;
                ownerData.Datakey = dataKey;
                ownerData.DecimalValue = value;
                ownerData.TenantTypeId = tenantTypeId;

                Sql sql = Sql.Builder;
                sql.Append("update tn_OwnerData set DecimalValue = @3 where DataKey = @0 and OwnerId = @1 and TenantTypeId = @2", dataKey, ownerId, tenantTypeId, value);

                int affectCount = dao.Execute(sql);
                if (affectCount == 0)
                {
                    dao.Insert(ownerData);
                }
            }
            else
            {
                ownerData.DecimalValue = ownerData.DecimalValue + value > 0 ? ownerData.DecimalValue + value : 0;
                dao.Update(ownerData);
            }

            //done:libsh,by zhengw: 同上
            //replay:已修改
            string cacheKey = GetCacheKey_GetOwnerData(ownerId, dataKey, tenantTypeId);
            OwnerData cacheOwnerdata = cacheService.Get<OwnerData>(cacheKey);
            if (cacheOwnerdata != null)
            {
                cacheService.Set(cacheKey, ownerData, CachingExpirationType.SingleObject);
            }
        }

        /// <summary>
        /// 变更用户数据
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的值</param>
        public void Change(long ownerId, string tenantTypeId, string dataKey, string value)
        {
            PetaPocoDatabase dao = CreateDAO();
            OwnerData ownerData = Get(ownerId, tenantTypeId, dataKey);
            if (ownerData == null || ownerData.Id == 0)
            {
                ownerData = OwnerData.New();
                ownerData.OwnerId = ownerId;
                ownerData.Datakey = dataKey;
                ownerData.StringValue = value;
                ownerData.TenantTypeId = tenantTypeId;

                Sql sql = Sql.Builder;
                sql.Append("update tn_OwnerData set StringValue =@3 where DataKey = @0 and OwnerId = @1 and tenantTypeId = @2", dataKey, ownerId, tenantTypeId, value);

                int affectCount = dao.Execute(sql);
                if (affectCount == 0)
                {
                    dao.Insert(ownerData);
                }
            }
            else
            {
                ownerData.StringValue = value;
                dao.Update(ownerData);
            }

            string cacheKey = GetCacheKey_GetOwnerData(ownerId, dataKey, tenantTypeId);
            OwnerData cacheOwnerdata = cacheService.Get<OwnerData>(cacheKey);
            if (cacheOwnerdata != null)
            {
                cacheService.Set(cacheKey, ownerData, CachingExpirationType.SingleObject);
            }
        }

        /// <summary>
        /// 获取OwnerData
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="dataKey">数据标识</param>
        public OwnerData Get(long ownerId, string tenantTypeId, string dataKey)
        {
            string cacheKey = GetCacheKey_GetOwnerData(ownerId, dataKey, tenantTypeId);

            OwnerData ownerData = cacheService.Get(cacheKey) as OwnerData;
            if (ownerData == null || ownerData.Id == 0)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("*")
                   .From("tn_OwnerData")
                   .Where("OwnerId = @0", ownerId)
                   .Where("Datakey = @0", dataKey)
                   .Where("TenantTypeId = @0", tenantTypeId);

                ownerData = CreateDAO().FirstOrDefault<OwnerData>(sql);
                if (ownerData == null)
                {
                    ownerData = OwnerData.New();

                    ownerData.OwnerId = ownerId;
                    ownerData.TenantTypeId = tenantTypeId;
                    ownerData.Datakey = dataKey;
                }

                cacheService.Add(cacheKey, ownerData, CachingExpirationType.SingleObject);
            }

            return ownerData;
        }

        /// <summary>
        /// 获取DataKey对应的值
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<OwnerData> GetAll(long ownerId, string tenantTypeId)
        {
            IList<OwnerData> ownerDatas = new List<OwnerData>();

            string cacheKey = GetCacheKey_GetAllOwnerData(ownerId, tenantTypeId);

            IEnumerable<long> ownerDataIds = cacheService.Get(cacheKey) as IEnumerable<long>;
            if (ownerDataIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Id")
                   .From("tn_OwnerData")
                   .Where("OwnerId = @0", ownerId)
                   .Where("TenantTypeId = @0", tenantTypeId);

                IEnumerable<object> ownerDataIds_obj = CreateDAO().FetchFirstColumn(sql);

                if (ownerDataIds_obj != null)
                {
                    ownerDataIds = ownerDataIds_obj.Select(n => Convert.ToInt64(n));
                    foreach (var id in ownerDataIds)
                    {
                        OwnerData appData = new OwnerData();
                        appData = Get(id);
                        if (appData != null)
                            ownerDatas.Add(appData);
                    }

                    cacheService.Add(cacheKey, ownerDatas, CachingExpirationType.ObjectCollection);
                }
            }

            return ownerDatas;
        }

        /// <summary>
        /// 获取用户Id分页数据
        /// </summary>
        /// <param name="dataKey">dataKey</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingOwnerIds(string dataKey, string tenantTypeId, int pageIndex, OwnerData_SortBy? sortBy = null)
        {
            PagingEntityIdCollection peic = null;

            Sql sql = Sql.Builder;
            sql.Select("OwnerId")
               .From("tn_OwnerData")
               .Where("DataKey = @0", dataKey)
               .Where("TenantTypeId = @0", tenantTypeId);

            if (sortBy.HasValue)
            {
                switch (sortBy)
                {
                    case OwnerData_SortBy.LongValue:
                        sql.OrderBy("LongValue ASC");
                        break;
                    case OwnerData_SortBy.LongValue_DESC:
                        sql.OrderBy("LongValue DESC");
                        break;
                    case OwnerData_SortBy.DecimalValue:
                        sql.OrderBy("DecimalValue ASC");
                        break;
                    case OwnerData_SortBy.DecimalValue_DESC:
                        sql.OrderBy("DecimalValue DESC");
                        break;
                }
            }

            PetaPocoDatabase dao = CreateDAO();
            if (pageIndex < CacheablePageCount)
            {
                string cacheKey = string.Format("PagingOwnerIdsFromOwnerData::DataKey:{0}-SortBy:{1}-TenantTypeId:{2}", dataKey, sortBy, tenantTypeId);
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "OwnerId", sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "OwnerId", sql);
            }

            IEnumerable<object> temIds = peic.GetPagingEntityIds(pageSize, pageIndex);
            PagingDataSet<long> pagingOwnerIds = new PagingDataSet<long>(temIds.Select(n => Convert.ToInt64(n)))
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = peic.TotalRecords
            };

            return pagingOwnerIds;
        }

        /// <summary>
        /// 获取前N个用户Id数据
        /// </summary>
        /// <param name="dataKey">dataKey</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">获取记录的个数</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopOwnerIds(string dataKey, string tenantTypeId, int topNumber, OwnerData_SortBy? sortBy = null)
        {
            PagingEntityIdCollection peic = null;
            Sql sql = Sql.Builder;
            sql.Select("OwnerId")
               .From("tn_OwnerData")
               .Where("DataKey = @0", dataKey)
               .Where("TenantTypeId = @0", tenantTypeId);

            if (sortBy.HasValue)
            {
                switch (sortBy)
                {
                    case OwnerData_SortBy.LongValue:
                        sql.OrderBy("LongValue ASC");
                        break;
                    case OwnerData_SortBy.LongValue_DESC:
                        sql.OrderBy("LongValue DESC");
                        break;
                    case OwnerData_SortBy.DecimalValue:
                        sql.OrderBy("DecimalValue ASC");
                        break;
                    case OwnerData_SortBy.DecimalValue_DESC:
                        sql.OrderBy("DecimalValue DESC");
                        break;
                }
            }

            string cacheKey = string.Format("TopOwnerIdsFromOwnerData::DataKey:{0}-SortBy:{1}-TenantTypeId:{2}", dataKey, sortBy, tenantTypeId);
            peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);

            if (peic == null)
            {
                IEnumerable<object> ownerIds = CreateDAO().FetchTopPrimaryKeys(SecondaryMaxRecords, "OwnerId", sql);
                peic = new PagingEntityIdCollection(ownerIds);
                cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
            }

            return peic.GetTopEntityIds(topNumber).Select(n => Convert.ToInt64(n));
        }

        /// <summary>
        /// 清除指定用户的用户数据
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public void ClearOwnerData(long ownerId, string tenantTypeId)
        {
            Sql sql = Sql.Builder;
            sql.Append("delete from tn_OwnerData where OwnerId = @0 and TenantTypeId = @1", ownerId, tenantTypeId);
        }

        #region 获取CacheKey

        /// <summary>
        /// 获取OwnerData的CacheKey
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        private string GetCacheKey_GetOwnerData(long ownerId, string dataKey, string tenantTypeId)
        {
            return string.Format("OwnerData::OwnerId:{0}-dataKey:{1}-TenantTypeId:{2}", ownerId, dataKey, tenantTypeId);
        }

        /// <summary>
        /// 获取所有OwnerData的CacheKey
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        private string GetCacheKey_GetAllOwnerData(long ownerId, string tenantTypeId)
        {
            return string.Format("AllOwnerData::OwnerId:{0}-TenantTypeId:{1}", ownerId, tenantTypeId);
        }

        #endregion
    }
}