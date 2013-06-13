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

namespace Tunynet.UI
{
    /// <summary>
    /// PresentAreaNavigation仓储接口
    /// </summary>
    public interface IPresentAreaNavigationRepository : IRepository<PresentAreaNavigation>
    {
        /// <summary>
        /// 获取呈现区域实例的所有PresentAreaNavigation
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        IEnumerable<PresentAreaNavigation> GetNavigations(string presentAreaKey, long ownerId);

        /// <summary>
        /// 强制所有呈现区域实例Owner安装导航
        /// </summary>
        /// <param name="initialNavigation">初始化导航实体</param>
        void ForceOwnerCreate(InitialNavigation initialNavigation);

        /// <summary>
        /// 强制所有呈现区域实例Owner更新导航
        /// </summary>
        /// <param name="initialNavigation">初始化导航实体</param>
        void ForceOwnerUpdate(InitialNavigation initialNavigation);

        /// <summary>
        /// 强制所有呈现区域实例Owner卸载导航
        /// </summary>
        /// <param name="initialNavigations">初始化导航实体集合</param>
        void ForceOwnerDelete(IEnumerable<InitialNavigation> initialNavigations);

        /// <summary>
        /// 清除呈现区域实例Owner的所有导航
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实体OwnerId</param>
        void ClearOwnerNavigations(string presentAreaKey, long ownerId);

    }
}
