//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Text;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 应用数据Repository
    /// </summary>
    public class ApplicationDataRepository : Repository<ApplicationData>, IApplicationDataRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="applicationId">applicationId</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(int applicationId, string dataKey, long value)
        {
            PetaPocoDatabase dao = CreateDAO();

            //当DataKey不存在时，插入新数据
            ApplicationData applicationData = Get(applicationId, dataKey);
            if (applicationData == null)
            {
                applicationData = new ApplicationData();
                applicationData.ApplicationId = applicationId;
                applicationData.TenantTypeId = string.Empty;
                applicationData.Datakey = dataKey;
                applicationData.LongValue = value;
                applicationData.StringValue = string.Empty;
                dao.Insert(applicationData);
            }
            else
            {
                applicationData.LongValue = value;
                dao.Update(applicationData);
            }
            RealTimeCacheHelper.IncreaseEntityCacheVersion(applicationData);
        }

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(int applicationId, string tenantTypeId, string dataKey, long value)
        {
            PetaPocoDatabase dao = CreateDAO();
            ApplicationData applicationData = Get(applicationId, tenantTypeId, dataKey);
            if (applicationData == null)
            {
                applicationData = new ApplicationData();
                applicationData.ApplicationId = applicationId;
                applicationData.TenantTypeId = tenantTypeId;
                applicationData.Datakey = dataKey;
                applicationData.LongValue = value;
                applicationData.StringValue = string.Empty;
                dao.Insert(applicationData);
            }
            else
            {
                applicationData.LongValue = value;
                dao.Update(applicationData);
            }
            RealTimeCacheHelper.IncreaseEntityCacheVersion(applicationData);
        }

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="applicationId">applicationId</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(int applicationId, string dataKey, decimal value)
        {
            PetaPocoDatabase dao = CreateDAO();
            ApplicationData applicationData = Get(applicationId, dataKey);
            if (applicationData == null)
            {
                applicationData = new ApplicationData();
                applicationData.ApplicationId = applicationId;
                applicationData.Datakey = dataKey;
                applicationData.TenantTypeId = string.Empty;
                applicationData.DecimalValue = value;
                applicationData.StringValue = string.Empty;
                dao.Insert(applicationData);
            }
            else
            {
                applicationData.DecimalValue = value;
                dao.Update(applicationData);
            }
            RealTimeCacheHelper.IncreaseEntityCacheVersion(applicationData);
        }

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(int applicationId, string tenantTypeId, string dataKey, decimal value)
        {
            PetaPocoDatabase dao = CreateDAO();
            ApplicationData applicationData = Get(applicationId, tenantTypeId, dataKey);
            if (applicationData == null)
            {
                applicationData = new ApplicationData();
                applicationData.ApplicationId = applicationId;
                applicationData.TenantTypeId = tenantTypeId;
                applicationData.Datakey = dataKey;
                applicationData.DecimalValue = value;
                applicationData.StringValue = string.Empty;
                dao.Insert(applicationData);
            }
            else
            {
                applicationData.DecimalValue = value;
                dao.Update(applicationData);
            }
            RealTimeCacheHelper.IncreaseEntityCacheVersion(applicationData);
        }

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的值</param>
        public void Change(int applicationId, string dataKey, string value)
        {
            PetaPocoDatabase dao = CreateDAO();
            ApplicationData applicationData = Get(applicationId, dataKey);
            if (applicationData == null)
            {
                applicationData = new ApplicationData();
                applicationData.ApplicationId = applicationId;
                applicationData.Datakey = dataKey;
                applicationData.TenantTypeId = string.Empty;
                applicationData.StringValue = value;
                dao.Insert(applicationData);
            }
            else
            {
                applicationData.StringValue = value;
                dao.Update(applicationData);
            }
            RealTimeCacheHelper.IncreaseEntityCacheVersion(applicationData);
        }

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(int applicationId, string tenantTypeId, string dataKey, string value)
        {
            PetaPocoDatabase dao = CreateDAO();

            ApplicationData applicationData = Get(applicationId, tenantTypeId, dataKey);
            if (applicationData == null)
            {
                applicationData = new ApplicationData();
                applicationData.ApplicationId = applicationId;
                applicationData.TenantTypeId = tenantTypeId;
                applicationData.Datakey = dataKey;
                applicationData.StringValue = value;
                dao.Insert(applicationData);
            }
            else
            {
                applicationData.StringValue = value;
                dao.Update(applicationData);
            }
            RealTimeCacheHelper.IncreaseEntityCacheVersion(applicationData);
        }

        /// <summary>
        /// 获取ApplicationData
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">数据标识</param>
        public ApplicationData Get(int applicationId, string dataKey)
        {
            string cacheKey = GetCacheKey_GetApplicationData(applicationId, dataKey);

            ApplicationData applicationData = cacheService.Get(cacheKey) as ApplicationData;
            if (applicationData == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("*").From("tn_ApplicationData")
                .Where("ApplicationId=@0 and Datakey=@1", applicationId, dataKey);

                applicationData = CreateDAO().FirstOrDefault<ApplicationData>(sql);
                if (applicationData != null)
                    cacheService.Add(cacheKey, applicationData, CachingExpirationType.SingleObject);
            }

            return applicationData;
        }

        /// <summary>
        /// 获取ApplicationData
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">数据标识</param> 
        /// <param name="dataKey">数据标识</param>
        public ApplicationData Get(int applicationId, string tenantTypeId, string dataKey)
        {
            string cacheKey = GetCacheKey_GetApplicationData(applicationId, tenantTypeId, dataKey);

            ApplicationData applicationData = cacheService.Get(cacheKey) as ApplicationData;
            if (applicationData == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("*").From("tn_ApplicationData")
                .Where("ApplicationId=@0 and Datakey=@1 and TenantTypeId=@2", applicationId, dataKey, tenantTypeId);

                applicationData = CreateDAO().FirstOrDefault<ApplicationData>(sql);
                if (applicationData != null)
                    cacheService.Add(cacheKey, applicationData, CachingExpirationType.SingleObject);
            }

            return applicationData;
        }

        /// <summary>
        /// 获取DataKey对应的值
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        public IEnumerable<ApplicationData> GetAll(int applicationId, string tenantTypeId = "")
        {
            IList<ApplicationData> applicationDatas = new List<ApplicationData>();

            string cacheKey = GetCacheKey_GetAllApplicationData(applicationId);

            IEnumerable<int> applicationDataIds = cacheService.Get(cacheKey) as IEnumerable<int>;
            if (applicationDataIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Id").From("tn_ApplicationData")
                .Where("ApplicationId=@0", applicationId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TenantTypeId=@0", tenantTypeId);

                applicationDataIds = CreateDAO().FetchFirstColumn(sql).Cast<int>();

                if (applicationDataIds != null)
                {
                    foreach (var id in applicationDataIds)
                    {
                        ApplicationData appData = new ApplicationData();
                        appData = Get(id);
                        if (appData != null)
                            applicationDatas.Add(appData);
                    }

                    cacheService.Add(cacheKey, applicationId, CachingExpirationType.ObjectCollection);
                }
            }

            return applicationDatas;
        }

        #region 缓存
        
        /// <summary>
        /// 获取ApplicationData的CacheKey
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">数据标识</param>
        private string GetCacheKey_GetApplicationData(int applicationId, string dataKey)
        {
            return string.Format("ApplicationData::ApplicationId:{0}-DataKey:{1}", applicationId, dataKey);
        }

        /// <summary>
        /// 获取ApplicationData的CacheKey
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <param name="dataKey">数据标识</param>
        private string GetCacheKey_GetApplicationData(int applicationId, string tenantTypeId, string dataKey)
        {
            return string.Format("ApplicationData::ApplicationId:{0}-TenantTypeId:{1}-dataKey:{2}", applicationId, tenantTypeId, dataKey);
        }

        /// <summary>
        /// 获取所有ApplicationData的CacheKey
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        private string GetCacheKey_GetAllApplicationData(int applicationId)
        {
            return string.Format("AllApplicationData::ApplicationId:{0}", applicationId);
        }

        #endregion

    }
}