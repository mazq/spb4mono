//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.UI
{
    /// <summary>
    /// 静态资源(js/css)及html页面meta、ShortcutIcon管理
    /// </summary>
    public interface IPageResourceManager
    {

        #region Tilte & Shortcut icon & Meta

        /// <summary>
        /// 是否在title中附加站点名称
        /// </summary>
        bool IsAppendSiteName { get; set; }

        /// <summary>
        /// 把TitlePart插入到第一位
        /// </summary>
        void InsertTitlePart(string titlePart);

        /// <summary>
        /// 附加TitlePart
        /// </summary>
        void AppendTitleParts(params string[] titleParts);

        /// <summary>
        /// 生成Title
        /// </summary>
        string GenerateTitle();

        ///// <summary>
        ///// Logo Url(默认=~/images/logo.png)
        ///// </summary>
        ///// <remarks>
        ///// 个别浏览器不支持png透明图片，为了兼容这些浏览器需要输出一段特殊的css代码
        ///// </remarks>
        //string LogoUrl { get; set; }

        /// <summary>
        /// 设置网站图标
        /// </summary>        
        string ShortcutIcon { get; set; }

        /// <summary>
        /// 设置description类型的meta
        /// </summary>
        /// <remarks>
        /// 请确保content不含html及"
        /// </remarks>
        /// <example>
        /// <![CDATA[<meta name="description" content="" />]]>    
        /// </example>
        /// <param name="content">设置的Description内容</param>
        void SetMetaOfDescription(string content);

        /// <summary>
        /// 设置keywords类型的meta
        /// </summary>
        /// <remarks>
        /// 请确保content不含html及"
        /// </remarks>
        /// <example>
        /// <![CDATA[<meta name="keywords" content="" />]]>    
        /// </example>
        /// <param name="content">设置的Keyword内容</param>
        void SetMetaOfKeywords(string content);

        /// <summary>
        /// 附加keywords类型的meta
        /// </summary>
        /// <example>
        /// <![CDATA[<meta name="keywords" content="" />]]>    
        /// </example>
        /// <param name="content">附加的Keyword内容</param>
        void AppendMetaOfKeywords(string content);

        /// <summary>
        /// 设置Meta
        /// </summary>
        /// <param name="meta">Meta实体</param>
        void SetMeta(MetaEntry meta);

        /// <summary>
        /// 附加Meta
        /// </summary>
        /// <param name="meta">Meta实体</param>
        /// <param name="contentSeparator">合并content时使用的分隔符</param>
        void AppendMeta(MetaEntry meta, string contentSeparator);

        /// <summary>
        /// 获取所有的Meta
        /// </summary>
        /// <returns>返回注册的所有Meta实体</returns>
        IList<MetaEntry> GetRegisteredMetas();

        #endregion


        #region include js & css

        /// <summary>
        /// 基于Combres的ResourceName引入js/css
        /// </summary>
        /// <param name="resourceType"><see cref="PageResourceType"/></param>
        /// <param name="resourceSetName">Combres中设置的ResourceSet</param>
        /// <param name="debugResouceSetName">debug时使用的ResouceSetName</param>
        /// <param name="renderPriority">呈现优先级</param>
        /// <param name="renderLocation">在页面中的呈现位置</param>
        /// <param name="htmlAttributes">设置的html属性</param>
        void IncludeCombresResouceSet(PageResourceType resourceType, string resourceSetName, string debugResouceSetName = null, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified, ResourceRenderLocation renderLocation = ResourceRenderLocation.Head, IDictionary<string, string> htmlAttributes = null);

        /// <summary>
        /// 设置Script引用
        /// </summary>
        /// <example>
        /// <![CDATA[<script src="/Utility/jquery/jquery.min-1.4.2.js"  type="text/javascript"></script>]]> 
        /// </example>
        /// <param name="scriptUrl">引入的script路径（支持~/）</param>
        /// <param name="renderPriority">呈现优先级</param>
        void IncludeScript(string scriptUrl, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified);

        /// <summary>
        /// 设置Script引用
        /// </summary>
        /// <example>
        /// <![CDATA[<script src="/Utility/jquery/jquery.min-1.4.2.js"  type="text/javascript"></script>]]> 
        /// </example>
        /// <param name="basePath">基路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="debugFileName">在调试状态使用的文件</param>
        /// <param name="renderPriority">呈现优先级</param>
        /// <param name="renderLocation">在页面中的呈现位置</param>
        /// <param name="htmlAttributes">设置的html属性</param>
        void IncludeScript(string basePath, string fileName, string debugFileName = null, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified, ResourceRenderLocation renderLocation = ResourceRenderLocation.Head, IDictionary<string, string> htmlAttributes = null);

        /// <summary>
        /// 设置css引用
        /// </summary>
        /// <example>
        /// <![CDATA[<link rel="stylesheet" href="/Themes/Shared/Styles/tn.core_src.css" type="text/css" media="screen" />]]> 
        /// </example>
        /// <param name="cssUrl">引入的css路径（支持~/）</param>
        /// <param name="renderPriority">呈现优先级</param>
        void IncludeCss(string cssUrl, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified);

        /// <summary>
        /// 设置css引用
        /// </summary>
        /// <example>
        /// <![CDATA[<link rel="stylesheet" href="/Themes/Shared/Styles/tn.core_src.css" type="text/css" media="screen" />]]> 
        /// </example>
        /// <param name="basePath">基路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="debugFileName">在调试状态使用的文件</param>
        /// <param name="renderPriority">呈现优先级</param>
        /// <param name="htmlAttributes">html属性集合</param>
        void IncludeCss(string basePath, string fileName, string debugFileName = null, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified, IDictionary<string, string> htmlAttributes = null);

        /// <summary>
        /// 获取所有在location呈现而引入的script
        /// </summary>
        /// <param name="renderLocation"><see cref="ResourceRenderLocation"/></param>
        IList<ResourceEntry> GetIncludedScripts(ResourceRenderLocation renderLocation);

        /// <summary>
        /// 获取所有引入的css
        /// </summary>
        /// <returns>返回所有包含的css</returns>
        IList<ResourceEntry> GetIncludedCsss();

        #endregion


        #region Register script/css 代码块

        /// <summary>
        /// 注册在页面head中呈现Script代码块
        /// </summary>
        /// <param name="scriptBlock">Javascript代码块</param>
        void RegisterScriptBlockAtHead(string scriptBlock);

        /// <summary>
        /// 注册在页面body闭合以前呈现的Script代码块
        /// </summary>
        /// <param name="scriptBlock">Javascript代码块</param>
        void RegisterScriptBlockAtFoot(string scriptBlock);

        /// <summary>
        /// 注册在页面head中呈现css代码块
        /// </summary>
        /// <param name="cssBlock">css代码块</param>
        void RegisterCssBlockAtHead(string cssBlock);

        /// <summary>
        /// 注册在页面body闭合以前呈现的css代码块
        /// </summary>
        /// <param name="cssBlock">css代码块</param>
        void RegisterCssBlockAtFoot(string cssBlock);

        /// <summary>
        /// 获取注册的需在location呈现的script代码块
        /// </summary>
        /// <param name="renderLocation"><see cref="ResourceRenderLocation"/></param>
        IList<string> GetRegisteredScriptBlocks(ResourceRenderLocation renderLocation);

        /// <summary>
        /// 获取注册的需在location呈现css代码块
        /// </summary>
        /// <param name="renderLocation"><see cref="ResourceRenderLocation"/></param>
        IList<string> GetRegisteredCssBlocks(ResourceRenderLocation renderLocation);

        #endregion


        #region Help Methods

        /// <summary>
        /// 获取ResourceEntry呈现的html
        /// </summary>
        /// <param name="resource">资源实体</param>
        /// <returns>返回用于html呈现的字符串</returns>
        string GetRenderingHtml(ResourceEntry resource);

        /// <summary>
        /// 解析资源的完整虚拟路径或带资源网址的全路径
        /// </summary>
        /// <param name="url">待解析的url</param>
        /// <returns>返回完整url路径</returns>
        string ResolveUrlWithResourceSite(string url);

        #endregion

    }
}
