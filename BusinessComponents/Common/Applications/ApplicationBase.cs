//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Tunynet.UI;

namespace Tunynet.Common
{
    /// <summary>
    /// 应用模块基类
    /// </summary>
    public abstract class ApplicationBase : ApplicationModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">应用元数据</param>
        internal protected ApplicationBase(ApplicationModel model, ApplicationConfig applicationConfig)
        {
            this.ApplicationId = model.ApplicationId;
            this.ApplicationKey = model.ApplicationKey;
            this.DisplayOrder = model.DisplayOrder;
            this.Description = model.Description;
            this.IsEnabled = model.IsEnabled;
            this.IsLocked = model.IsLocked;
            this.config = applicationConfig;
        }



        #region 在呈现区域安装/卸载

        /// <summary>
        /// 为呈现区域实例安装应用
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        internal protected abstract bool Install(string presentAreaKey, long ownerId);


        /// <summary>
        /// 为呈现区域实例卸载应用
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        internal protected abstract bool UnInstall(string presentAreaKey, long ownerId);

        #endregion


        #region 删除用户

        /// <summary>
        /// 删除用户在应用中的数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="takeOverUserName">用于接管删除用户时不能删除的内容(例如：用户创建的群组)</param>
        /// <param name="isTakeOver">是否接管被删除用户可被接管的内容</param>
        internal protected abstract void DeleteUser(long userId, string takeOverUserName, bool isTakeOver);

        #endregion


        #region 动态导航

        /// <summary>
        /// 获取应用在程序区域实例的动态导航（默认无动态导航，如果需要在ApplicationBase具体实现类进行重写）
        /// </summary>
        /// <remarks>
        /// 例如：资讯栏目
        /// </remarks>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        internal protected virtual IEnumerable<Navigation> GetDynamicNavigations(string presentAreaKey, long ownerId = 0)
        {
            return Enumerable.Empty<Navigation>();
        }

        #endregion



        #region ApplicationConfig

        private ApplicationConfig config;
        /// <summary>
        /// ApplicationConfig
        /// </summary>
        public ApplicationConfig Config
        {
            get { return config; }
        }

        #endregion


    }
}
