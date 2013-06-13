using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 租户类型业务逻辑类
    /// </summary>
    public class TenantTypeService
    {

        #region 构造器

        private ITenantTypeRepository tenantTypeRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public TenantTypeService()
            : this(new TenantTypeRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="userDataRepository">用户数据仓储</param>
        public TenantTypeService(ITenantTypeRepository tenantTypeRepository)
        {
            this.tenantTypeRepository = tenantTypeRepository;
        }

        #endregion

        /// <summary>
        /// 依据服务或应用获取租户类型
        /// </summary>
        /// <param name="serviceKey">服务标识</param>
        /// <param name="applicationId">应用Id</param>
        /// <returns>如未满足条件的TenantType则返回空集合</returns>
        public IEnumerable<TenantType> Gets(string serviceKey, int? applicationId = null)
        {
            return tenantTypeRepository.Gets(serviceKey, applicationId);
        }

        /// <summary>
        /// 依据tenantTypeId获取租户类型
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public TenantType Get(string tenantTypeId)
        {
            return tenantTypeRepository.Get(tenantTypeId);
        }
    }
}
