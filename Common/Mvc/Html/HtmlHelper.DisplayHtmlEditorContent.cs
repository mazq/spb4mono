//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Mvc;
using Tunynet.Common.Configuration;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Link的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperDisplayHtmlEditorContentExtensions
    {

        /// <summary>
        /// 输出html编辑器产生的内容，根据显示区域的宽度自动调整图片尺寸，并引入js脚本
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="content">html编辑器的内容</param>
        /// <param name="imageWidth">显示区域的图片宽度</param>
        /// <returns></returns>
        public static MvcHtmlString DisplayHtmlEditorContent(this HtmlHelper htmlHelper, string tenantTypeId, string content, int imageWidth)
        {
            if (string.IsNullOrEmpty(content))
            {
                return MvcHtmlString.Create(string.Empty);
            }

            TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);

            content = content.Replace("width=\"" + tenantAttachmentSettings.InlinedImageWidth + "\"", "width=\"" + imageWidth + "\"");
            content += @"<script>
                            $(function () {
                                SyntaxHighlighter.defaults['toolbar'] = false;
                                SyntaxHighlighter.all();

                                $(" + "\"a[rel='fancybox']\"" + @").fancybox({
                                    'transitionIn': 'elastic',
                                    'transitionOut': 'elastic',
                                    'speedIn': 600,
                                    'speedOut': 200
                                });
                            });
                        </script>
            ";

            return MvcHtmlString.Create(content);
        }
    }
}
