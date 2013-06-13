//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Combres;
using Tunynet.Utilities;
using System.Web.Mvc;

namespace Tunynet.UI
{
    /// <summary>
    /// IResourceManager的默认实现
    /// </summary>
    public class PageResourceManager : IPageResourceManager
    {
        private readonly List<string> _titleParts = new List<string>();

        private readonly List<ResourceEntry> _includedScripts = new List<ResourceEntry>();
        private readonly List<ResourceEntry> _includedCsss = new List<ResourceEntry>();
        private readonly Dictionary<ResourceRenderLocation, List<String>> _registeredScriptBlocks = new Dictionary<ResourceRenderLocation, List<String>>();
        private readonly Dictionary<ResourceRenderLocation, List<String>> _registeredCssBlocks = new Dictionary<ResourceRenderLocation, List<String>>();
        private readonly Dictionary<string, MetaEntry> _metas;

        /// <summary>
        /// 构造函数
        /// </summary>        
        public PageResourceManager()
            : this(null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productVersionInfo">产品版本信息（用于生成generator）</param>
        public PageResourceManager(string productVersionInfo)
        {
            if (!string.IsNullOrEmpty(productVersionInfo))
            {
                _metas = new Dictionary<string, MetaEntry> 
                {
                    { "generator", new MetaEntry("generator",productVersionInfo)}
                };
            }
            else
            {
                _metas = new Dictionary<string, MetaEntry>();
            }

            //LogoUrl = "~/images/logo.png";
        }

        /// <summary>
        /// 是否启用调试
        /// </summary>
        public bool DebugEnabled { get; set; }

        /// <summary>
        /// 独立的资源站（例如：http://www.resouce.com）
        /// </summary>
        public string ResourceSite { get; set; }



        #region Tilte & Shoutcut icon & Meta

        private string titleSeparator = " - ";
        /// <summary>
        /// 页面Title各组成部分分隔符
        /// </summary>
        public string TitleSeparator
        {
            get { return titleSeparator; }
            set { titleSeparator = value; }
        }

        private bool isAppendSiteName;
        /// <summary>
        /// 是否在title中附加站点名称
        /// </summary>
        public bool IsAppendSiteName
        {
            get { return isAppendSiteName; }
            set { isAppendSiteName = value; }
        }

        /// <summary>
        /// 附加TitlePart
        /// </summary>
        public void AppendTitleParts(params string[] titleParts)
        {
            if (titleParts.Length > 0)
                foreach (string titlePart in titleParts)
                    if (!string.IsNullOrEmpty(titlePart))
                        _titleParts.Add(titlePart);
        }

        /// <summary>
        /// 把TitlePart插入到第一位
        /// </summary>
        public void InsertTitlePart(string titlePart)
        {
            if (!string.IsNullOrEmpty(titlePart))
                _titleParts.Insert(0, titlePart);
        }

        /// <summary>
        /// 生成Title
        /// </summary>
        public string GenerateTitle()
        {
            return _titleParts.Count == 0
                ? String.Empty
                : String.Join(TitleSeparator, _titleParts.AsEnumerable().ToArray());
        }

        private string shortcutIcon;
        /// <summary>
        /// shortcut icon
        /// </summary>
        public string ShortcutIcon
        {
            get
            {
                if (string.IsNullOrEmpty(shortcutIcon))
                    shortcutIcon = "~/favicon.ico";

                return ResolveUrlWithResourceSite(shortcutIcon);
            }
            set { shortcutIcon = value; }
        }

        ///// <summary>
        ///// Logo Url(默认=~/images/logo.png)
        ///// </summary>
        ///// <remarks>
        ///// 个别浏览器不支持png透明图片，为了兼容这些浏览器需要输出一段特殊的css代码
        ///// </remarks>
        //public string LogoUrl { get; set; }

        /// <summary>
        /// 设置description类型的meta
        /// </summary>
        /// <param name="content">设置的Description内容</param>
        public void SetMetaOfDescription(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            MetaEntry meta = new MetaEntry("description", content);
            SetMeta(meta);
        }

        /// <summary>
        /// 设置keywords类型的meta
        /// </summary>
        /// <param name="content">设置的Keyword内容</param>
        public void SetMetaOfKeywords(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            MetaEntry meta = new MetaEntry("keywords", content);
            SetMeta(meta);
        }

        /// <summary>
        /// 附加keywords类型的meta
        /// </summary>
        /// <param name="content">附加的Keyword内容</param>
        public void AppendMetaOfKeywords(string content)
        {
            MetaEntry meta = new MetaEntry("keywords", content);
            AppendMeta(meta, ",");
        }

        /// <summary>
        /// 设置Meta
        /// </summary>
        /// <param name="meta">Meta实体</param>
        public void SetMeta(MetaEntry meta)
        {
            if (meta == null || String.IsNullOrEmpty(meta.Name))
                return;

            _metas[meta.Name] = meta;
        }

        /// <summary>
        /// 附加Meta
        /// </summary>
        /// <param name="meta">Meta实体</param>
        /// <param name="contentSeparator">合并content时使用的分隔符</param>
        public void AppendMeta(MetaEntry meta, string contentSeparator)
        {
            if (meta == null || String.IsNullOrEmpty(meta.Name))
                return;

            MetaEntry existingMeta;
            if (_metas.TryGetValue(meta.Name, out existingMeta))
                meta = MetaEntry.Combine(existingMeta, meta, contentSeparator);

            _metas[meta.Name] = meta;
        }

        /// <summary>
        /// 获取所有的Meta
        /// </summary>
        public IList<MetaEntry> GetRegisteredMetas()
        {
            return _metas.Values.ToList().AsReadOnly();
        }

        #endregion


        #region Include js/css

        /// <summary>
        /// 基于Combres的ResourceName引入js/css
        /// </summary>
        /// <param name="resourceType"><see cref="PageResourceType"/></param>
        /// <param name="resouceSetName">Combres中设置的ResourceSet</param>
        public void IncludeCombresResouceSet(PageResourceType resourceType, string resouceSetName)
        {
            IncludeCombresResouceSet(resourceType, resouceSetName, null);
        }

        /// <summary>
        /// 基于Combres的ResourceName引入js/css
        /// </summary>
        /// <param name="resourceType"><see cref="PageResourceType"/></param>
        /// <param name="resouceSetName">Combres中设置的ResourceSet</param>
        /// <param name="debugResouceSetName">debug时使用的ResouceSetName</param>
        /// <param name="renderPriority">呈现优先级</param>
        /// <param name="renderLocation">在页面中的呈现位置</param>
        /// <param name="htmlAttributes">设置的html属性</param>
        public void IncludeCombresResouceSet(PageResourceType resourceType, string resouceSetName, string debugResouceSetName = null, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified, ResourceRenderLocation renderLocation = ResourceRenderLocation.Head, IDictionary<string, string> htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(resouceSetName))
                throw new ArgumentNullException("resouceSetName");

            string url = WebExtensions.CombresUrl(resouceSetName);
            string debugUrl = null;
            if (debugResouceSetName != null)
                debugUrl = WebExtensions.CombresUrl(debugResouceSetName);

            ResourceEntry resource = new ResourceEntry(resourceType, null, url, debugUrl, renderPriority, renderLocation, htmlAttributes);
            switch (resourceType)
            {
                case PageResourceType.JS:
                    _includedScripts.Add(resource);
                    break;
                case PageResourceType.CSS:
                    _includedCsss.Add(resource);
                    break;
            }
        }

        /// <summary>
        /// 设置Script引用
        /// </summary>
        /// <param name="scriptUrl">引入的script路径（支持~/）</param>
        /// <param name="renderPriority">呈现优先级</param>
        public void IncludeScript(string scriptUrl, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified)
        {
            if (string.IsNullOrEmpty(scriptUrl))
                throw new ArgumentNullException("scriptUrl");

            IncludeScript(null, scriptUrl, renderPriority: renderPriority);
        }

        /// <summary>
        /// 设置Script引用
        /// </summary>
        /// <param name="basePath">基路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="debugFileName">在调试状态使用的文件</param>
        /// <param name="renderPriority">呈现优先级</param>
        /// <param name="renderLocation">在页面中的呈现位置</param>
        /// <param name="htmlAttributes">设置的html属性</param>
        public void IncludeScript(string basePath, string fileName, string debugFileName = null, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified, ResourceRenderLocation renderLocation = ResourceRenderLocation.Head, IDictionary<string, string> htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            ResourceEntry resource = new ResourceEntry(PageResourceType.JS, basePath, fileName, debugFileName, renderPriority, renderLocation, htmlAttributes);
            _includedScripts.Add(resource);
        }

        /// <summary>
        /// 设置css引用
        /// </summary>
        /// <param name="cssUrl">引入的css路径（支持~/）</param>
        /// <param name="renderPriority">呈现优先级</param>
        public void IncludeCss(string cssUrl, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified)
        {
            if (string.IsNullOrEmpty(cssUrl))
                throw new ArgumentNullException("cssUrl");

            IncludeCss(null, cssUrl, renderPriority: renderPriority);
        }

        /// <summary>
        /// 设置css引用
        /// </summary>
        /// <param name="basePath">基路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="debugFileName">在调试状态使用的文件</param>
        /// <param name="renderPriority">呈现优先级</param>
        /// <param name="htmlAttributes">html属性集合</param>
        public void IncludeCss(string basePath, string fileName, string debugFileName = null, ResourceRenderPriority renderPriority = ResourceRenderPriority.Unspecified, IDictionary<string, string> htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            ResourceEntry resource = new ResourceEntry(PageResourceType.CSS, basePath, fileName, debugFileName, renderPriority, ResourceRenderLocation.Head, htmlAttributes);
            _includedCsss.Add(resource);
        }

        /// <summary>
        /// 获取所有在location呈现而引入的script
        /// </summary>
        /// <param name="renderLocation"><see cref="ResourceRenderLocation"/></param>
        public IList<ResourceEntry> GetIncludedScripts(ResourceRenderLocation renderLocation)
        {
            List<ResourceEntry> resources = new List<ResourceEntry>();
            resources.AddRange(FilterResourceEntries(_includedScripts, renderLocation, ResourceRenderPriority.First));
            resources.AddRange(FilterResourceEntries(_includedScripts, renderLocation, ResourceRenderPriority.Unspecified));
            resources.AddRange(FilterResourceEntries(_includedScripts, renderLocation, ResourceRenderPriority.Last));

            return resources;
        }

        /// <summary>
        /// 获取所有引入的css
        /// </summary>
        /// <returns>返回所有包含的css</returns>
        public IList<ResourceEntry> GetIncludedCsss()
        {
            List<ResourceEntry> resources = new List<ResourceEntry>();
            resources.AddRange(FilterResourceEntries(_includedCsss, null, ResourceRenderPriority.First));
            resources.AddRange(FilterResourceEntries(_includedCsss, null, ResourceRenderPriority.Unspecified));
            resources.AddRange(FilterResourceEntries(_includedCsss, null, ResourceRenderPriority.Last));

            return resources;
        }

        /// <summary>
        /// 从sourceResourceEntries按照renderLocation、renderPriority过滤符合要求的ResourceEntry集合
        /// </summary>
        private IEnumerable<ResourceEntry> FilterResourceEntries(IList<ResourceEntry> sourceResourceEntries, ResourceRenderLocation? renderLocation, ResourceRenderPriority renderPriority)
        {
            if (sourceResourceEntries == null)
                return new List<ResourceEntry>();

            if (renderLocation.HasValue)
                return sourceResourceEntries.Where(r => r.RenderLocation == renderLocation.Value && r.RenderPriority == renderPriority);
            else
                return sourceResourceEntries.Where(r => r.RenderPriority == renderPriority);
        }

        #endregion


        #region Register script/css 代码块

        /// <summary>
        /// 注册在页面head中呈现Script代码块
        /// </summary>
        /// <param name="scriptBlock">Javascript代码块</param>
        public void RegisterScriptBlockAtHead(string scriptBlock)
        {
            RegisterScriptBlock(ResourceRenderLocation.Head, scriptBlock);
        }

        /// <summary>
        /// 注册在页面body闭合以前呈现的Script代码块
        /// </summary>
        /// <param name="scriptBlock">Javascript代码块</param>
        public void RegisterScriptBlockAtFoot(string scriptBlock)
        {
            RegisterScriptBlock(ResourceRenderLocation.Foot, scriptBlock);
        }

        /// <summary>
        /// 注册在页面呈现的Script代码块
        /// </summary>
        private void RegisterScriptBlock(ResourceRenderLocation renderLocation, string scriptBlock)
        {
            List<string> scriptBlocksOfLocation;

            if (!_registeredScriptBlocks.TryGetValue(renderLocation, out scriptBlocksOfLocation))
            {
                scriptBlocksOfLocation = new List<string>();
                _registeredScriptBlocks[renderLocation] = scriptBlocksOfLocation;
            }

            scriptBlocksOfLocation.Add(scriptBlock);
        }

        /// <summary>
        /// 注册在页面head中呈现css代码块
        /// </summary>
        /// <param name="cssBlock">css代码块</param>
        public void RegisterCssBlockAtHead(string cssBlock)
        {
            RegisterCssBlock(ResourceRenderLocation.Head, cssBlock);
        }

        /// <summary>
        /// 注册在页面foot中呈现css代码块
        /// </summary>
        /// <param name="cssBlock">css代码块</param>
        public void RegisterCssBlockAtFoot(string cssBlock)
        {
            RegisterCssBlock(ResourceRenderLocation.Foot, cssBlock);
        }

        /// <summary>
        /// 注册在页面呈现的css代码块
        /// </summary>
        private void RegisterCssBlock(ResourceRenderLocation renderLocation, string cssBlock)
        {
            List<string> cssBlocksOfLocation;
            if (!_registeredCssBlocks.TryGetValue(renderLocation, out cssBlocksOfLocation))
            {
                cssBlocksOfLocation = new List<string>();
                _registeredCssBlocks[renderLocation] = cssBlocksOfLocation;
            }
            cssBlocksOfLocation.Add(cssBlock);
        }

        /// <summary>
        /// 获取注册的需在location呈现的script代码块
        /// </summary>
        /// <param name="renderLocation"><see cref="ResourceRenderLocation"/></param>
        public IList<string> GetRegisteredScriptBlocks(ResourceRenderLocation renderLocation)
        {
            List<string> scriptBlocksOfLocation;
            if (!_registeredScriptBlocks.TryGetValue(renderLocation, out scriptBlocksOfLocation))
                return new List<string>(0);

            return scriptBlocksOfLocation;
        }

        /// <summary>
        /// 获取注册的需在location呈现css代码块
        /// </summary>
        /// <param name="renderLocation"><see cref="ResourceRenderLocation"/></param>
        public IList<string> GetRegisteredCssBlocks(ResourceRenderLocation renderLocation)
        {
            List<string> cssBlocksOfLocation;
            if (!_registeredCssBlocks.TryGetValue(renderLocation, out cssBlocksOfLocation))
                return new List<string>(0);

            return cssBlocksOfLocation;
        }

        #endregion


        #region Help Methods

        /// <summary>
        /// 获取ResourceEntry呈现的html
        /// </summary>
        /// <param name="resource">资源实体</param>
        /// <returns>返回用于html呈现的字符串</returns>
        public string GetRenderingHtml(ResourceEntry resource)
        {
            TagBuilder tagBuilder;
            TagRenderMode tagRenderMode;

            string originalUrl = resource.BasePath ?? string.Empty;
            if (DebugEnabled && !string.IsNullOrEmpty(resource.DebugUrl))
                originalUrl += resource.DebugUrl;
            else
                originalUrl += resource.Url;

            string url = ResolveUrlWithResourceSite(originalUrl);
            if (resource.ResourceType == PageResourceType.JS)
            {
                tagBuilder = new TagBuilder("script");
                tagRenderMode = TagRenderMode.Normal;
                tagBuilder.MergeAttribute("src", url);
                tagBuilder.MergeAttribute("type", "text/javascript");
            }
            else
            {
                tagBuilder = new TagBuilder("link");
                tagRenderMode = TagRenderMode.SelfClosing;
                tagBuilder.MergeAttribute("href", url);
                tagBuilder.MergeAttribute("type", "text/css");
                tagBuilder.MergeAttribute("rel", "stylesheet");
            }

            if (resource.HasAttributes)
                tagBuilder.MergeAttributes<string, string>(resource.Attributes);

            return tagBuilder.ToString(tagRenderMode);
        }

        /// <summary>
        /// 解析资源的完整虚拟路径或带资源网址的全路径
        /// </summary>
        /// <param name="url">待解析的url</param>
        /// <returns>返回完整url路径</returns>
        public string ResolveUrlWithResourceSite(string url)
        {
            string resultUrl = ResolveUrl(url);
            if (!string.IsNullOrEmpty(ResourceSite) && !url.ToLower().StartsWith("http"))
                resultUrl = ResourceSite + url;

            return resultUrl;
        }

        /// <summary>
        /// 解析url为绝对路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string ResolveUrl(string url)
        {
            return WebUtility.ResolveUrl(url);
        }
        #endregion

    }
}
