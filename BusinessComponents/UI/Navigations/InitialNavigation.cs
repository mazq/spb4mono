//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.UI
{
    /// <summary>
    /// 初始化导航实体
    /// </summary>
    [TableName("tn_InitialNavigations")]
    [PrimaryKey("NavigationId", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable, PropertyNamesOfArea = "PresentAreaKey")]
    [Serializable]
    public class InitialNavigation : IEntity
    {

        #region 需持久化属性
        /// <summary>
        ///NavigationId
        /// </summary>
        public int NavigationId { get; set; }

        /// <summary>
        ///ParentNavigationId
        /// </summary>
        public int ParentNavigationId { get; set; }

        /// <summary>
        ///深度（从上到下以0开始）
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        ///呈现区域标识
        /// </summary>
        public string PresentAreaKey { get; set; }

        /// <summary>
        ///ApplicationId
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///导航类型
        /// </summary>
        public NavigationType NavigationType { get; set; }

        /// <summary>
        ///导航文字
        /// </summary>
        public string NavigationText { get; set; }

        /// <summary>
        ///导航文字资源名称（如果同时设置NavigationText则以NavigationText优先）
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        ///导航url， 如果是来源于应用,并且该字段为空,则根据UrlRouteName获取 
        /// </summary>
        public string NavigationUrl { get; set; }

        /// <summary>
        ///应用导航路由规则名称 将会根据该规则名称获取应用导航地址
        /// </summary>
        public string UrlRouteName { get; set; }

        /// <summary>
        ///路由数据名称(Url中包含的路由数据)
        /// </summary>
        public string RouteDataName { get; set; }

        /// <summary>
        /// 内置图标名称
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        ///菜单文字旁边的图标url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///是新开窗口还是在当前窗口（默认:_self）
        /// </summary>
        public string NavigationTarget { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///是否仅拥有者可见
        /// </summary>
        public bool OnlyOwnerVisible { get; set; }

        /// <summary>
        ///是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.NavigationId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        #region 扩展属性

        /// <summary>
        /// 子导航列表
        /// </summary>
        [Ignore]
        public IEnumerable<InitialNavigation> Children
        {
            get
            {
                IEnumerable<InitialNavigation> allInitialNavigations = new NavigationService().GetInitialNavigations(this.PresentAreaKey);
                return allInitialNavigations.Where(n => n.ParentNavigationId == this.NavigationId);
            }
        }

        #endregion

        #region Help Methods

        /// <summary>
        /// InitialNavigation转换成PresentAreaNavigation
        /// </summary>
        public PresentAreaNavigation AsPresentAreaNavigation()
        {
            PresentAreaNavigation presentAreaNavigation = new PresentAreaNavigation()
            {
                NavigationId = this.NavigationId,
                ParentNavigationId = this.ParentNavigationId,
                Depth = this.Depth,
                PresentAreaKey = this.PresentAreaKey,
                ResourceName = this.ResourceName,
                NavigationUrl = this.NavigationUrl,
                UrlRouteName = this.UrlRouteName,
                RouteDataName = this.RouteDataName,
                ApplicationId = this.ApplicationId,
                NavigationType = this.NavigationType,
                NavigationText = this.NavigationText,
                NavigationTarget = this.NavigationTarget,
                ImageUrl = this.ImageUrl,
                DisplayOrder = this.DisplayOrder,
                OnlyOwnerVisible = this.OnlyOwnerVisible,
                IsLocked = this.IsLocked,
                IsEnabled = this.IsEnabled,
                OwnerId = 0,
                IconName = this.IconName

            };

            return presentAreaNavigation;
        }


        #endregion

    }
}
