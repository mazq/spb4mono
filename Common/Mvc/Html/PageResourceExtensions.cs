//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using Tunynet.UI;
using Combres.Mvc;

namespace Tunynet.Mvc
{
    /// <summary>
    /// PageResource在HtmlHelper的扩展方法
    /// </summary>
    public static class PageResourceExtensions
    {
        /// <summary>
        /// 输出&lt;head&gt;及预置的StyleSheet
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHead BeginHead(this HtmlHelper htmlHelper)
        {
            return BeginHead(htmlHelper, title: null);
        }

        /// <summary>
        /// 输出&lt;head&gt;及预置的StyleSheet
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="title">标题</param>
        /// <param name="disableClientCache">是否禁用客户端缓存</param>
        /// <param name="charset">字符集</param>
        /// <returns>返回MvcHead对象</returns>
        public static MvcHead BeginHead(this HtmlHelper htmlHelper, string title = null, bool disableClientCache = false, string charset = "UTF-8")
        {
            return new MvcHead(htmlHelper.ViewContext, title, disableClientCache, charset);
        }


        /// <summary>
        /// 在页面引用Combres Script
        /// </summary>
        /// <param name="html"></param>
        /// <param name="resourceType"><see cref="PageResourceType"/></param>
        /// <param name="resourceSetName">在Combres设置的ResourceSet名称</param>
        /// <param name="renderPriority">资源呈现优先级<see cref="ResourceRenderPriority"/></param>
        public static void IncludeCombresScript(this HtmlHelper html, PageResourceType resourceType, string resourceSetName, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            pageResourceManager.IncludeCombresResouceSet(resourceType, resourceSetName, renderPriority: renderPriority);
        }

        /// <summary>
        /// 在页面引用Script
        /// </summary>
        /// <param name="html"></param>
        /// <param name="scriptUrl">script的url</param>
        /// <param name="renderPriority">呈现优先级<see cref="ResourceRenderPriority"/></param>
        public static void IncludeScript(this HtmlHelper html, string scriptUrl, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified)
        {
            IncludeScript(html, null, scriptUrl, renderPriority: renderPriority);
        }

        /// <summary>
        /// 在页面引用Script
        /// </summary>
        /// <param name="html"></param>
        /// <param name="basePath">基路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="debugFileName">用于调试的文件名称</param>
        /// <param name="renderPriority"><see cref="ResourceRenderPriority"/></param>
        /// <param name="renderLocation"><see cref="ResourceRenderLocation"/></param>
        /// <param name="htmlAttributes">html属性</param>
        public static void IncludeScript(this HtmlHelper html, string basePath, string fileName, string debugFileName = null, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified, ResourceRenderLocation renderLocation = ResourceRenderLocation.Head, IDictionary<string, string> htmlAttributes = null)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            pageResourceManager.IncludeScript(basePath, fileName, debugFileName, renderPriority, renderLocation, htmlAttributes);
        }

        /// <summary>
        /// 设置css引用
        /// </summary>
        /// <param name="html"></param>
        /// <param name="cssUrl">css路径</param>
        /// <param name="renderPriority"><see cref="ResourceRenderPriority"/></param>
        public static void IncludeCss(this HtmlHelper html, string cssUrl, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified)
        {
            IncludeCss(html, null, cssUrl, renderPriority: renderPriority);
        }

        /// <summary>
        /// 设置css引用
        /// </summary>
        /// <param name="html"></param>
        /// <param name="basePath">基路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="debugFileName">用于调试的文件名称</param>
        /// <param name="renderPriority">
        /// <see cref="ResourceRenderPriority"/></param>
        /// <param name="htmlAttributes">html属性</param>
        public static void IncludeCss(this HtmlHelper html, string basePath, string fileName, string debugFileName = null, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified, IDictionary<string, string> htmlAttributes = null)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            pageResourceManager.IncludeCss(basePath, fileName, debugFileName, renderPriority, htmlAttributes);
        }

        /// <summary>
        /// 在页面链接Javascript文件，并于代码引入位置呈现
        /// </summary>
        /// <param name="html">要扩展的HtmlHelper</param>
        /// <param name="scriptUrl">需要引入的Script资源</param>
        /// <returns></returns>
        public static MvcHtmlString LinkScript(this HtmlHelper html, string scriptUrl)
        {
            if (string.IsNullOrEmpty(scriptUrl))
            {
                throw new ArgumentNullException("resouceSetName");
            }

            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();

            TagBuilder tagBuilder;
            tagBuilder = new TagBuilder("script");
            tagBuilder.MergeAttribute("src", pageResourceManager.ResolveUrlWithResourceSite(scriptUrl));
            tagBuilder.MergeAttribute("type", "text/javascript");

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));

        }

        /// <summary>
        /// 在页面链接css文件，并于代码引入位置呈现
        /// </summary>
        /// <param name="html">要扩展的HtmlHelper</param>
        /// <param name="cssUrl">需要引入的css资源</param>
        /// <returns></returns>
        public static MvcHtmlString LinkCss(this HtmlHelper html, string cssUrl)
        {
            if (string.IsNullOrEmpty(cssUrl))
            {
                throw new ArgumentNullException("cssUrl");
            }

            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();

            TagBuilder tagBuilder;
            tagBuilder = new TagBuilder("link");
            tagBuilder.MergeAttribute("href", pageResourceManager.ResolveUrlWithResourceSite(cssUrl));
            tagBuilder.MergeAttribute("type", "text/css");
            tagBuilder.MergeAttribute("rel", "stylesheet");

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// 在页面链接Combress资源，并于代码引入位置呈现
        /// </summary>
        /// <param name="html"></param>
        /// <param name="resourceSetName"></param>
        /// <returns></returns>
        public static MvcHtmlString LinkCombres(this HtmlHelper html, string resourceSetName)
        {
            return html.CombresLink(resourceSetName);
        }

        /// <summary>
        /// 在页面底部呈现引入的js
        /// </summary>
        /// <remarks>
        /// &lt;/body&gt;结束以前
        /// </remarks>
        /// <param name="html"></param>
        public static void RenderIncludedScriptsAtFoot(this HtmlHelper html)
        {
            TextWriter _writer = html.ViewContext.Writer;
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            IList<ResourceEntry> includeScripts = pageResourceManager.GetIncludedScripts(ResourceRenderLocation.Foot);
            foreach (var includeScript in includeScripts)
            {
                _writer.WriteLine(pageResourceManager.GetRenderingHtml(includeScript));
            }
        }

        /// <summary>
        /// 在页面底部呈现注册的js代码块
        /// </summary>
        /// <remarks>
        /// &lt;/body&gt;结束以前
        /// </remarks>
        /// <param name="html"></param>
        public static void RenderRegisteredScriptBlocksAtFoot(this HtmlHelper html)
        {
            TextWriter _writer = html.ViewContext.Writer;

            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            IList<string> registeredScriptBlocks = pageResourceManager.GetRegisteredScriptBlocks(ResourceRenderLocation.Foot);
            if (registeredScriptBlocks != null && registeredScriptBlocks.Count > 0)
            {
                _writer.WriteLine("<script type=\"text/javascript\">");
                foreach (var registeredScriptBlock in registeredScriptBlocks)
                {
                    _writer.WriteLine(registeredScriptBlock);
                }
                _writer.WriteLine("</script>");
            }
        }

        /// <summary>
        /// 在页面底部呈现注册的css代码块
        /// </summary>
        /// <remarks>
        /// &lt;/body&gt;结束以前
        /// </remarks>
        /// <param name="html"></param>
        public static void RenderRegisteredCssBlocksAtFoot(this HtmlHelper html)
        {
            TextWriter _writer = html.ViewContext.Writer;

            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            IList<string> registeredCssBlocks = pageResourceManager.GetRegisteredCssBlocks(ResourceRenderLocation.Foot);
            if (registeredCssBlocks != null && registeredCssBlocks.Count > 0)
            {
                _writer.WriteLine("<style type=\"text/css\">");
                foreach (var registeredCssBlock in registeredCssBlocks)
                {
                    _writer.WriteLine(registeredCssBlock);
                }
                _writer.WriteLine("</style>");
            }
        }


    }
}
