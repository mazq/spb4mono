//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-07-19</createdate>
//<author>ful</author>
//<email>ful@tunynet.com</email>
//<log date="2012-07-19" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 应用数据业务逻辑类
    /// </summary>
    public class ApplicationDataService
    {
        #region 构造器

        private IApplicationDataRepository applicationDataRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public ApplicationDataService()
            : this(new ApplicationDataRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="applicationDataRepository">应用数据仓储</param>
        public ApplicationDataService(IApplicationDataRepository applicationDataRepository)
        {
            this.applicationDataRepository = applicationDataRepository;
        }

        #endregion

        

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(int applicationId, string dataKey, long value)
        {
            //当DataKey不存在时，插入新数据
            //同时更新缓存

            applicationDataRepository.Change(applicationId, dataKey, value);
        }

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        public void Change(int applicationId, string dataKey, decimal value)
        {
            //当DataKey不存在时，插入新数据
            //同时更新缓存

            applicationDataRepository.Change(applicationId, dataKey, value);
        }

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的值</param>
        public void Change(int applicationId, string dataKey, string value)
        {
            //当DataKey不存在时，插入新数据
            //同时更新缓存

            applicationDataRepository.Change(applicationId, dataKey, value);
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
            //当DataKey不存在时，插入新数据
            //同时更新缓存

            applicationDataRepository.Change(applicationId, tenantTypeId, dataKey, value);
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
            //当DataKey不存在时，插入新数据
            //同时更新缓存

            applicationDataRepository.Change(applicationId, tenantTypeId, dataKey, value);
        }

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的值</param>
        public void Change(int applicationId, string tenantTypeId, string dataKey, string value)
        {
            //当DataKey不存在时，插入新数据
            //同时更新缓存

            applicationDataRepository.Change(applicationId, tenantTypeId, dataKey, value);
        }

        /// <summary>
        /// 获取DataKey对应的长整形
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">DataKey</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <returns>dataKey不存在时返回0</returns>
        public long GetLong(int applicationId, string dataKey, string tenantTypeId = "")
        {
            //以ApplicationId为单位读取并存入缓存，从中获取过滤dataKey     
            IEnumerable<ApplicationData> applicationDatas = applicationDataRepository.GetAll(applicationId, tenantTypeId);
            if (applicationDatas == null)
                return default(long);
            ApplicationData applicationData = applicationDatas.FirstOrDefault(n => n.Datakey == dataKey);
            if (applicationData == null)
                return default(long);
            return applicationData.LongValue;
        }

        /// <summary>
        /// 获取DataKey对应的Decimal
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">DataKey</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>dataKey不存在时返回0</returns>
        public decimal GetDecimal(int applicationId, string dataKey, string tenantTypeId = "")
        {
            IEnumerable<ApplicationData> applicationDatas = applicationDataRepository.GetAll(applicationId, tenantTypeId);
            if (applicationDatas == null)
                return default(decimal);
            ApplicationData applicationData = applicationDatas.FirstOrDefault(n => n.Datakey == dataKey);
            if (applicationData == null)
                return default(decimal);
            return applicationData.DecimalValue;
        }

        /// <summary>
        /// 获取DataKey对应的String
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">DataKey</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>dataKey不存在时返回空字符串</returns>
        public string GetString(int applicationId, string dataKey, string tenantTypeId = "")
        {
            IEnumerable<ApplicationData> applicationDatas = applicationDataRepository.GetAll(applicationId, tenantTypeId);
            if (applicationDatas == null)
                return default(string);
            ApplicationData applicationData = applicationDatas.FirstOrDefault(n => n.Datakey == dataKey);
            if (applicationData == null)
                return default(string);
            return applicationData.StringValue;
        }
    }
}
