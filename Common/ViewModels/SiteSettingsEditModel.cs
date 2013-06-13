//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using Tunynet;
using Tunynet.Utilities;
using System.Web.Mvc;
using Spacebuilder.Common.Configuration;
using Tunynet.Common.Configuration;
using Tunynet.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 站点设置
    /// </summary>
    public class SiteSettingsEditModel
    {
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public SiteSettingsEditModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="siteSettings">站点设置</param>
        /// <param name="commentSettings">评论设置</param>
        public SiteSettingsEditModel(SiteSettings siteSettings, CommentSettings commentSettings)
        {
            if (siteSettings != null)
            {
                SiteName = siteSettings.SiteName;
                SiteDescription = siteSettings.SiteDescription;
                MainSiteRootUrl = siteSettings.MainSiteRootUrl;
                SearchMetaDescription = siteSettings.SearchMetaDescription;
                BeiAnScript = siteSettings.BeiAnScript;
                StatScript = siteSettings.StatScript;
                Links = siteSettings.Links;
                EnableAnonymousBrowse = siteSettings.EnableAnonymousBrowse;
                EnableAnonymousPosting = siteSettings.EnableAnonymousPosting;
                ShareToThirdDisplayType = siteSettings.ShareToThirdDisplayType;
                ShareDisplayIconSize = siteSettings.ShareDisplayIconSize;
                ShareToThirdBusiness = siteSettings.ShareToThirdBusiness;
                ShareToThirdIsEnabled = siteSettings.ShareToThirdIsEnabled;
                SearchMetaKeyWords = siteSettings.SearchMetaKeyWords;
                EnableSimpleHome = siteSettings.EnableSimpleHome;
            }
            if (commentSettings != null)
            {
                EnableComment = commentSettings.EnableComment;
                AllowAnonymousComment = commentSettings.AllowAnonymousComment;
            }
        }

        #region 持久化属性

        #region 站点设置

        /// <summary>
        /// 备案信息
        /// </summary>
        [AllowHtml]
        [WaterMark(Content = "网站备案信息，支持Html代码")]
        public string BeiAnScript { get; set; }

        /// <summary>
        /// 页脚统计脚本
        /// </summary>
        [AllowHtml]
        [WaterMark(Content = "用于统计站点访问记录，支持Html代码")]
        public string StatScript { get; set; }

        /// <summary>
        /// 页脚统计脚本
        /// </summary>
        [AllowHtml]
        [WaterMark(Content = "页脚链接")]
        public string Links { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 站点描述
        /// </summary>
        public string SiteDescription { get; set; }

        /// <summary>
        /// 页面头信息的description
        /// </summary>
        [WaterMark(Content = "主要用于站点seo优化")]
        public string SearchMetaDescription { get; set; }

        /// <summary>
        /// 页面头信息的KeyWord
        /// </summary>
        public string SearchMetaKeyWords { get; set; }

        /// <summary>
        /// 主站URL
        /// </summary>
        /// <remarks>
        /// 安装程序（或者首次启动时）需要自动保存该地址
        /// </remarks>
        public string MainSiteRootUrl { get; set; }

        /// <summary>
        /// 是否允许匿名发帖
        /// </summary>
        /// <remarks>
        /// 包括所有的：评论、留言、回帖等
        /// </remarks>
        public bool EnableAnonymousPosting { get; set; }

        /// <summary>
        /// 是否允许匿名用户访问站点
        /// </summary>
        public bool EnableAnonymousBrowse { get; set; }

        /// <summary>
        /// 匿名用户默认访问
        /// </summary>
        public bool EnableSimpleHome { get; set; }

        /// <summary>
        /// 分享到站外是否启用
        /// </summary>
        public bool ShareToThirdIsEnabled { get; set; }

        /// <summary>
        /// 分享到站外展示类型
        /// </summary>
        public ShareDisplayType ShareToThirdDisplayType { get; set; }

        /// <summary>
        /// 分享到站外图标形式展示大小
        /// </summary>
        public ShareDisplayIconSize ShareDisplayIconSize { get; set; }

        /// <summary>
        /// 分享到站外展示商家
        /// </summary>
        public string ShareToThirdBusiness { get; set; }

        #endregion

        #region 评论设置

        /// <summary>
        /// 是否启用评论
        /// </summary>
        public bool EnableComment { get; set; }


        /// <summary>
        /// 是否允许匿名用户评论
        /// </summary>
        public bool AllowAnonymousComment { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// 转换为站点设置实体
        /// </summary>
        /// <returns></returns>
        public SiteSettings AsSiteSettings()
        {
            SiteSettings siteSettings = DIContainer.Resolve<ISiteSettingsManager>().Get();
            siteSettings.SiteName = SiteName ?? string.Empty;
            siteSettings.SiteDescription = SiteDescription ?? string.Empty;
            siteSettings.MainSiteRootUrl = MainSiteRootUrl ?? string.Empty;
            siteSettings.SearchMetaDescription = SearchMetaDescription ?? string.Empty;
            siteSettings.BeiAnScript = BeiAnScript ?? string.Empty;
            siteSettings.StatScript = StatScript ?? string.Empty;
            siteSettings.EnableAnonymousBrowse = EnableAnonymousBrowse;
            siteSettings.EnableAnonymousPosting = EnableAnonymousPosting;
            siteSettings.ShareToThirdIsEnabled = ShareToThirdIsEnabled;
            siteSettings.ShareToThirdDisplayType = ShareToThirdDisplayType;
            siteSettings.ShareDisplayIconSize = ShareDisplayIconSize;
            siteSettings.ShareToThirdBusiness = ShareToThirdBusiness ?? string.Empty;
            siteSettings.Links = Links ?? string.Empty;
            siteSettings.SearchMetaKeyWords = SearchMetaKeyWords ?? string.Empty;
            siteSettings.EnableSimpleHome = EnableSimpleHome;
            return siteSettings;
        }

        /// <summary>
        /// 转换为评论实体
        /// </summary>
        /// <returns></returns>
        public CommentSettings AsCommentSettings()
        {
            CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
            commentSettings.EnableComment = EnableComment;
            commentSettings.AllowAnonymousComment = AllowAnonymousComment;
            return commentSettings;
        }
    }
}
