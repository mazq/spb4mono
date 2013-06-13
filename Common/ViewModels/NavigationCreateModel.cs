using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tunynet.UI;

namespace Spacebuilder.Common
{
    //导航编辑实体
    public class NavigationCreateModel
    {
        
        [Display(Name = "导航Id")]
        [Required(ErrorMessage = "请输入导航Id")]
        [Remote("ValidateNavigationId", "ControlPanelSettings", ErrorMessage = "此导航Id已存在,请重新输入")]
        [RegularExpression("[0-9]{8}", ErrorMessage = "导航Id必须输入8位数字")]
        public int NavigationId { get; set; }

        [Display(Name = "应用Id")]
        public int ApplicationId { get; set; }

        [Display(Name = "名称")]
        [Required(ErrorMessage = "此项为必填项")]
        public string NavigationText { get; set; }

        [Display(Name = "resourceKey")]
        [Required(ErrorMessage = "此项为必填项")]
        public string ResourceName { get; set; }

        [Display(Name = "链接")]        
        public string NavigationUrl { get; set; }

        [Display(Name = "路由名")]        
        public string UrlRouteName { get; set; }

        [Display(Name = "使用站内图标")]
        public bool IsIconName { get; set; }

        [Display(Name = "内置图标名称")]
        public string IconName { get; set; }

        [Display(Name = "图片名称")]
        public string ImageName { get; set; }

        [Display(Name = "链接")]
        public string ImageUrl { get; set; }

        [Display(Name = "新窗口")]
        public string NavigationTarget { get; set; }

        [Display(Name = "仅拥有者可见")]
        public bool OnlyOwnerVisible { get; set; }

        [Display(Name = "启用")]
        public bool IsEnabled { get; set; }

        [Display(Name = "使用资源项")]
        public bool IsUseResourceItem { get; set; }

        [Display(Name = "使用路由")]
        public bool IsUseRoute { get; set; }

        [Display(Name = "完整链接")]
        public bool IsWholeLink { get; set; }

        /// <summary>
        /// 转换为InitialNavigation用于数据库存储
        /// </summary>
        /// <returns></returns>
        public InitialNavigation AsInitialNavigation()
        {
            InitialNavigation initialNavigation = new InitialNavigation();

            initialNavigation.NavigationId = this.NavigationId;
            initialNavigation.ApplicationId = this.ApplicationId;


            //判断应用
            if (this.ApplicationId == 0)
            {
                initialNavigation.NavigationType = NavigationType.PresentAreaInitial;
            }
            else
            {
                initialNavigation.NavigationType = NavigationType.Application;
            }

            //使用资源项
            if (this.IsUseResourceItem)
            {
                initialNavigation.ResourceName = this.ResourceName;
                initialNavigation.NavigationText = string.Empty;
            }
            else
            {
                initialNavigation.NavigationText = this.NavigationText;
                initialNavigation.ResourceName = string.Empty;
            }

            //使用路由
            if (this.IsUseRoute)
            {
                initialNavigation.UrlRouteName = this.UrlRouteName ?? string.Empty;
                initialNavigation.NavigationUrl = string.Empty;
            }
            else
            {
                initialNavigation.NavigationUrl = this.NavigationUrl ?? string.Empty;
                initialNavigation.UrlRouteName = string.Empty;
            }
            if (IsIconName)
            {
                initialNavigation.IconName = this.IconName;
                initialNavigation.ImageUrl = null;
            }
            else
            {
                initialNavigation.IconName = null;
                if (this.IsWholeLink)
                {
                    initialNavigation.ImageUrl = this.ImageUrl;
                }
                else
                {
                    if (ImageName != null)
                    {
                        initialNavigation.ImageUrl = "~/Uploads/NavigationImage/" + this.ImageName;
                    }
                    else
                    {
                        initialNavigation.ImageUrl = null;
                    }
                }
            }
            initialNavigation.NavigationTarget = this.NavigationTarget;
            initialNavigation.DisplayOrder = this.NavigationId;

            initialNavigation.OnlyOwnerVisible = this.OnlyOwnerVisible;
            initialNavigation.IsEnabled = this.IsEnabled;

            return initialNavigation;
        }
    }
}
