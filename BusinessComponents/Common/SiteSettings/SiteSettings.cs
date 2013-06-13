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
using System.Web;
using Tunynet.Caching;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Tunynet.Common
{
    /// <summary>
    /// 站点设置
    /// </summary>
    /// <remarks>安装站点时，必须设置MainSiteRootUrl</remarks>
    [CacheSetting(true)]
    [Serializable]
    public class SiteSettings : IEntity
    {
        string beiAnScript = string.Empty;
        /// <summary>
        /// 备案信息
        /// </summary>
        [AllowHtml]
        public string BeiAnScript
        {
            get { return beiAnScript; }
            set { beiAnScript = value; }
        }

        string statScript = string.Empty;
        /// <summary>
        /// 页脚统计脚本
        /// </summary>
        [AllowHtml]
        public string StatScript
        {
            get { return statScript; }
            set { statScript = value; }
        }

        string links = string.Empty;
        /// <summary>
        /// 页脚链接
        /// </summary>
        [AllowHtml]
        public string Links
        {
            get { return links; }
            set { links = value; }
        }


        private Guid siteKey = Guid.NewGuid();
        /// <summary>
        /// 站点唯一标识
        /// </summary>
        public Guid SiteKey
        {
            get { return siteKey; }
            set { siteKey = value; }
        }

        string defaultSiteName = "Spacebuilder";
        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName
        {
            get { return defaultSiteName; }
            set { defaultSiteName = value; }
        }


        string defaultSiteDescription = string.Empty;
        /// <summary>
        /// 站点描述
        /// </summary>
        public string SiteDescription
        {
            get { return defaultSiteDescription; }
            set { defaultSiteDescription = value; }
        }

        string searchMetaDescription = "采用Web2.0思想、asp.net2.0技术开发的社区门户产品。是一套Web2.0全面解决方案，包含：空间、群组、活动、论坛、SNS等功能，可以根据用户需求任意组合、无缝集成。它采用了业内领先的技术体系架构、隐私保护功能、用户评价体系、优异的缓存技术、全文检索技术。可以承载千万级的数据，具备优异的扩展性并提供丰富的API，方便用户进行定制开发或者二次开发。";
        /// <summary>
        /// 页面头信息的description
        /// </summary>
        public string SearchMetaDescription
        {
            get { return searchMetaDescription; }
            set { searchMetaDescription = value; }
        }

        string searchMetaKeyWords = string.Empty;
        /// <summary>
        /// 页面头信息的KeyWord
        /// </summary>
        public string SearchMetaKeyWords
        {
            get { return searchMetaKeyWords; }
            set { searchMetaKeyWords = value; }
        }

        private bool enableMultilingual = false;
        /// <summary>
        /// 是否启用国际化
        /// </summary>
        public bool EnableMultilingual
        {
            get { return enableMultilingual; }
            set { enableMultilingual = value; }
        }

        private string defaultLanguage = "zh-cn";
        /// <summary>
        /// 系统默认语言
        /// </summary>
        public string DefaultLanguage
        {
            get { return defaultLanguage; }
            set { defaultLanguage = value; }
        }

        //主站点Url
        private string mainSiteRootUrl = @"http://localhost";
        /// <summary>
        /// 主站URL
        /// </summary>
        /// <remarks>
        /// 安装程序（或者首次启动时）需要自动保存该地址
        /// </remarks>
        public string MainSiteRootUrl
        {
            get { return mainSiteRootUrl; }
            set { mainSiteRootUrl = value; }
        }

        string siteTheme = "Default";
        /// <summary>
        /// 站点主题
        /// </summary>
        public string SiteTheme
        {
            get { return siteTheme; }
            set { siteTheme = value; }
        }

        private string siteThemeAppearance = string.Empty;
        /// <summary>
        /// 站点皮肤外观
        /// </summary>
        public string SiteThemeAppearance
        {
            get { return siteThemeAppearance; }
            set { siteThemeAppearance = value; }
        }

        private bool defaultEnableAnonymousPosting = false;

        /// <summary>
        /// 是否允许匿名发帖
        /// </summary>
        /// <remarks>
        /// 包括所有的：评论、留言、回帖等
        /// </remarks>
        public bool EnableAnonymousPosting
        {
            get { return defaultEnableAnonymousPosting; }
            set { defaultEnableAnonymousPosting = value; }
        }

        private bool enableAnonymousBrowse = false;
        /// <summary>
        /// 是否允许匿名用户访问站点
        /// </summary>
        public bool EnableAnonymousBrowse
        {
            get { return enableAnonymousBrowse; }
            set { enableAnonymousBrowse = value; }
        }


        private bool enableSimpleHome = false;
        /// <summary>
        /// 匿名用户默认访问
        /// </summary>
        public bool EnableSimpleHome
        {
            get { return enableSimpleHome; }
            set { enableSimpleHome = value; }
        }


        private bool shareToThirdIsEnabled = false;
        /// <summary>
        /// 分享到站外是否启用
        /// </summary>
        public bool ShareToThirdIsEnabled
        {
            get { return shareToThirdIsEnabled; }
            set { shareToThirdIsEnabled = value; }
        }

        private ShareDisplayType shareToThirddisplayType = ShareDisplayType.Word;
        /// <summary>
        /// 分享到站外展示类型
        /// </summary>
        public ShareDisplayType ShareToThirdDisplayType
        {
            get { return shareToThirddisplayType; }
            set { shareToThirddisplayType = value; }
        }

        private ShareDisplayIconSize shareDisplayIconSize = ShareDisplayIconSize.middle;
        /// <summary>
        /// 分享到站外图标形式展示大小
        /// </summary>
        public ShareDisplayIconSize ShareDisplayIconSize
        {
            get { return shareDisplayIconSize; }
            set { shareDisplayIconSize = value; }
        }

        private string shareToThirdBusiness;
        /// <summary>
        /// 分享到站外展示商家
        /// </summary>
        public string ShareToThirdBusiness
        {
            get { return shareToThirdBusiness; }
            set { shareToThirdBusiness = value; }
        }



        #region IEntity 成员

        object IEntity.EntityId { get { return typeof(SiteSettings).FullName; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }


    /// <summary>
    /// 分享到站外展示形式
    /// </summary>
    public enum ShareDisplayType
    {
        /// <summary>
        /// 文字
        /// </summary>
        [Display(Name = "文字")]
        Word,
        /// <summary>
        /// 图标
        /// </summary>
        [Display(Name = "图标")]
        Icon,
        /// <summary>
        /// 侧栏
        /// </summary>
        [Display(Name = "侧栏")]
        Sidebar
    }

    /// <summary>
    /// 分享到站外图标形式展示大小
    /// </summary>
    public enum ShareDisplayIconSize
    {
        /// <summary>
        /// 大图标
        /// </summary>
        [Display(Name = "大图标")]
        Big,
        /// <summary>
        /// 中图标
        /// </summary>
        [Display(Name = "中图标")]
        middle,
        /// <summary>
        /// 小图标
        /// </summary>
        [Display(Name = "小图标")]
        small
    }
}