//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Tunynet.Common.Configuration;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Link的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperContentTypeThumbnailExtensions
    {
        /// <summary>
        /// 根据文件类型显示缩略图
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="fileName">文件名称</param>
        /// <param name="thumbnailSize">缩略图大小</param>
        /// <param name="navigateUrl">缩略图链接地址</param>
        /// <param name="navigateTarget">链接目标</param>
        /// <returns></returns>
        public static MvcHtmlString ContentTypeThumbnail(this HtmlHelper htmlHelper, string fileName, ContentTypeThumbnailSize thumbnailSize, string navigateUrl = null, HyperLinkTarget navigateTarget = HyperLinkTarget._self)
        {
            string fileExtension = Path.GetExtension(fileName);
            string thumbnail = GetContentTypeThumbnail(fileExtension, thumbnailSize);
            if (!string.IsNullOrEmpty(navigateUrl))
            {
                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", navigateUrl);

                if (navigateTarget != HyperLinkTarget._self)
                    a.MergeAttribute("target", navigateTarget.ToString());

                a.InnerHtml = thumbnail;
                return MvcHtmlString.Create(a.ToString());
            }
            else
                return MvcHtmlString.Create(thumbnail);
        }

        /// <summary>
        /// 根据文件类型显示缩略图
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="fileExtension">文件后缀</param>
        /// <param name="size">缩略图大小</param>
        /// <returns></returns>
        public static MvcHtmlString ContentTypeThumbnail(this HtmlHelper htmlHelper, string fileExtension)
        {
            return MvcHtmlString.Create(GetContentTypeThumbnail(fileExtension, ContentTypeThumbnailSize.Small));
        }

        /// <summary>
        /// 根据文件扩展名获得预览图片路径
        /// </summary>
        private static string GetContentTypeThumbnail(string fileExtension, ContentTypeThumbnailSize size)
        {
            if (string.IsNullOrEmpty(fileExtension))
                return string.Empty;
            if (fileExtension.Length > 1 && fileExtension.StartsWith("."))
                fileExtension = fileExtension.Substring(1).ToLower();
            TagBuilder span = new TagBuilder("span");
            if (!MimeTypeConfiguration.Extensions.Contains(fileExtension))
            {
                fileExtension = "unknow";
            }

            switch (size)
            {
                case ContentTypeThumbnailSize.Large:
                    span.AddCssClass(string.Format("tn-mime-big tn-mime-{0}-big  tn-icon-inline", fileExtension));
                    break;
                case ContentTypeThumbnailSize.Small:
                default:
                    span.AddCssClass(string.Format("tn-mime tn-mime-{0}  tn-icon-inline", fileExtension));
                    break;
            }

            return span.ToString();
        }
    }

    // 摘要:
    //     文件类型缩略图尺寸
    public enum ContentTypeThumbnailSize
    {
        /// <summary>
        /// 大尺寸
        /// </summary>
        Large = 0,

        /// <summary>
        /// 小图
        /// </summary>
        Small = 1
    }
}