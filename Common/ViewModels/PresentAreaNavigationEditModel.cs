using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Tunynet.Mvc;
using Tunynet.UI;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 呈现区域导航编辑实体
    /// </summary>
    public class PresentAreaNavigationEditModel
    {
        /// <summary>
        /// 呈现区域导航ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 呈现区域导航显示文本
        /// </summary>
        [Required(ErrorMessage="名称不能为空")]
        [StringLength(32,ErrorMessage="最多可输入32个字符")]
        [WaterMark(Content="导航名称")]
        [DataType(DataType.Text)]
        public string NavigationText { get; set; }

        /// <summary>
        /// 用户添加的地址
        /// </summary>
        [StringLength(127, ErrorMessage = "最多可输入127个字符")]
        public string NavigationUrl { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [StringLength(127, ErrorMessage = "最多可输入127个字符")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 目标窗口打开方式
        /// </summary>
        public string NavigationTarget { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 添加方式
        /// </summary>
        public NavigationType NavigationType { get; set; }

        public PresentAreaNavigation AsPresentAreaNavigation()
        {
            PresentAreaNavigation presentAreaNavigation = null;

            if (Id==0)
            {
                presentAreaNavigation = new PresentAreaNavigation();
                presentAreaNavigation.ResourceName = string.Empty;
                presentAreaNavigation.NavigationUrl = NavigationUrl;
                presentAreaNavigation.RouteDataName = string.Empty;
                presentAreaNavigation.UrlRouteName = string.Empty;
            }
            else
            {
                presentAreaNavigation = new NavigationService().GetPresentAreaNavigation(Id);
            }

            presentAreaNavigation.NavigationText = NavigationText;
            presentAreaNavigation.ImageUrl = ImageUrl;
            presentAreaNavigation.NavigationUrl = NavigationUrl!=null?NavigationUrl:string.Empty;
            presentAreaNavigation.NavigationTarget = NavigationTarget;
            if (!presentAreaNavigation.IsLocked)
            {
                presentAreaNavigation.IsEnabled = IsEnabled;   
            }
            presentAreaNavigation.PresentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace;
            if (NavigationType == NavigationType.AddedByOwner)
            {
                presentAreaNavigation.NavigationType = NavigationType.AddedByOwner;
            }
            presentAreaNavigation.OwnerId = UserContext.CurrentUser.UserId;
            
            Random rd=new Random();
            string rdStr=null;
            for (int i = 0; i < 8; i++)
			{
			 rdStr+=rd.Next(9).ToString();
			}
            presentAreaNavigation.NavigationId = int.Parse(rdStr);

            return presentAreaNavigation;
        }
    }

    public static class UserSpacePresentAreaNavigationExtensions
    {
        public static PresentAreaNavigationEditModel AsPresentAreaNavigationEditModel(this PresentAreaNavigation presentAreaNavigation)
        {
            return new PresentAreaNavigationEditModel
            {
                Id=presentAreaNavigation.Id,
                IsEnabled=presentAreaNavigation.IsEnabled,
                IsLocked=presentAreaNavigation.IsLocked,
                ImageUrl=presentAreaNavigation.ImageUrl,
                NavigationUrl=presentAreaNavigation.NavigationUrl,
                NavigationTarget=presentAreaNavigation.NavigationTarget,
                NavigationType=presentAreaNavigation.NavigationType,
                NavigationText=presentAreaNavigation.NavigationText          
            }; 
        }
    }
}
