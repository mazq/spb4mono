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
    /// <summary>
    /// ApplicationInPresentAreaInstallation仓储接口
    /// </summary>
    public interface IApplicationInPresentAreaInstallationRepository : IRepository<ApplicationInPresentAreaInstallation>
    {
        /// <summary>
        /// 依据presentAreaKey、ownerId、applicationId获取对应的ApplicationInPresentAreaInstallation
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        /// <param name="applicationId">applicationId</param>
        ApplicationInPresentAreaInstallation Fetch(string presentAreaKey, long ownerId, int applicationId);
        
        /// <summary>
        /// 获取呈现区域实例拥有者已安装的ApplicationId列表
        /// </summary>
        /// <param name="presentAreaKey">区域区域Id</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        /// <returns>返回在呈现区域安装的应用Id集合</returns>
        IEnumerable<int> GetInstalledApplicationIds(string presentAreaKey, long ownerId);


    }
}
