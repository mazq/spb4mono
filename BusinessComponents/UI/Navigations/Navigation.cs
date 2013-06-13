//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Utilities;
using System.Resources;

namespace Tunynet.UI
{
    /// <summary>
    /// 用于呈现的导航实体
    /// </summary>
    /// <remarks>
    /// 下一步开发设想：
    /// 1、使用扩展方法IsVisible(User currentUser)
    /// 2、使用扩展方法GetUrl()、GetUrl(string spacekey)
    /// </remarks>
    [Serializable]
    public class Navigation : IComparable
    {

        #region 普通属性
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
        ///呈现区域实例OwnerId
        /// </summary>
        public long OwnerId { get; set; }

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
        ///导航url
        /// </summary>
        public string NavigationUrl { get; set; }

        /// <summary>
        ///应用导航路由规则名称
        /// </summary>
        public string UrlRouteName { get; set; }

        /// <summary>
        ///路由数据名称(Url中包含的路由数据)
        /// </summary>
        public string RouteDataName { get; set; }

        /// <summary>
        ///菜单文字旁边的图标url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 图标名称
        /// </summary>
        public string IconName { get; set; }

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


        #region 扩展属性

        /// <summary>
        /// 用于呈现的导航文本
        /// </summary>
        public string Text
        {
            get
            {
                if (!string.IsNullOrEmpty(NavigationText))
                    return NavigationText;
                else if (!string.IsNullOrEmpty(ResourceName))
                    return Tunynet.Globalization.ResourceAccessor.GetString(ResourceName);

                return string.Empty;
            }
        }

        private Navigation parent;
        /// <summary>
        /// 父导航
        /// </summary>
        public Navigation Parent
        {
            get
            {
                if (Depth == 0 || ParentNavigationId == 0)
                    return null;
                else
                    return parent;
            }
            internal protected set { parent = value; }
        }

        private List<Navigation> children;
        /// <summary>
        /// 所有子导航
        /// </summary>
        public IEnumerable<Navigation> Children
        {
            get
            {
                if (children == null)
                    return new List<Navigation>().ToReadOnly();

                return children;
            }
        }

        /// <summary>
        /// 添加子导航
        /// </summary>
        internal protected void AppendChild(Navigation childNavigation)
        {
            if (children == null)
                children = new List<Navigation>();

            children.Add(childNavigation);
        }

        #endregion


        #region Help Methods

        /// <summary>
        /// 判断是否属于当前导航
        /// </summary>
        /// <remarks>
        /// 单元测试用例，仅需测试NavigationService.GetCurrentNavigationPath()
        /// </remarks>
        /// <param name="currentNavigationId"></param>
        /// <returns></returns>
        public bool IsCurrent(int currentNavigationId)
        {
            NavigationService navigationService = new NavigationService();
            IEnumerable<int> currentNavigationPath = navigationService.GetCurrentNavigationPath(this.PresentAreaKey, this.OwnerId, currentNavigationId);
            return currentNavigationPath.Contains(this.NavigationId) || NavigationId == currentNavigationId;
        }

        /// <summary>
        /// 转换成InitialNavigation
        /// </summary>
        /// <returns></returns>
        internal protected InitialNavigation AsInitialNavigation()
        {
            if (NavigationType != UI.NavigationType.Application)
                return null;

            InitialNavigation nav = new InitialNavigation()
            {
                NavigationId = this.NavigationId,
                ParentNavigationId = this.ParentNavigationId,
                Depth = this.Depth,
                PresentAreaKey = this.PresentAreaKey,
                ApplicationId = this.ApplicationId,
                NavigationType = this.NavigationType,
                ResourceName = this.ResourceName,
                NavigationUrl = this.NavigationUrl,
                UrlRouteName = this.UrlRouteName,
                NavigationText = this.NavigationText,
                NavigationTarget = this.NavigationTarget,
                ImageUrl = this.ImageUrl,
                DisplayOrder = this.DisplayOrder,
                OnlyOwnerVisible = this.OnlyOwnerVisible,
                IsLocked = this.IsLocked,
                IsEnabled = this.IsEnabled
            };
            return nav;
        }

        #endregion

        /// <summary>
        /// Navigation比较（用于排序）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Navigation other = obj as Navigation;
            if (other == null)
                return 1;

            if (this.Depth != other.Depth)
                return this.Depth.CompareTo(other.Depth);
            else
                return this.DisplayOrder.CompareTo(other.DisplayOrder);
        }
    }






}
