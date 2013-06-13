//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace Tunynet.UI
{
    /// <summary>
    /// 资源实体
    /// </summary>
    public class ResourceEntry
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resourceType">资源类型</param>
        /// <param name="basePath">基路径</param>
        /// <param name="url"></param>
        /// <param name="debugUrl"></param>
        /// <param name="renderPriority"></param>
        /// <param name="renderLocation"></param>
        /// <param name="attributes"></param>
        public ResourceEntry(PageResourceType resourceType, string basePath, string url, string debugUrl,
            ResourceRenderPriority renderPriority, ResourceRenderLocation renderLocation, IDictionary<string, string> attributes)
        {
            this.ResourceType = resourceType;
            this.BasePath = basePath;
            this.Url = url;
            this.DebugUrl = debugUrl;
            this.RenderPriority = renderPriority;
            this.RenderLocation = renderLocation;

            if (attributes != null)
                this.Attributes = attributes;
        }

        /// <summary>
        /// 用于生成html的TagBuilder
        /// </summary>
        public TagBuilder TagBuilder { get; private set; }

        /// <summary>
        /// Html标签呈现模式
        /// </summary>
        public TagRenderMode TagRenderMode { get; private set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public PageResourceType ResourceType { get; private set; }

        /// <summary>
        /// BasePath
        /// </summary>
        public string BasePath { get; private set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 用于debug的Url
        /// </summary>
        public string DebugUrl { get; private set; }

        /// <summary>
        /// 资源呈现优先级
        /// </summary>
        public ResourceRenderPriority RenderPriority { get; private set; }

        /// <summary>
        /// 资源在页面呈现的位置
        /// </summary>
        public ResourceRenderLocation RenderLocation { get; private set; }

        /// <summary>
        /// Attributes
        /// </summary>
        public IDictionary<string, string> Attributes { get; private set; }

        /// <summary>
        /// 设置Attribute
        /// </summary>
        public ResourceEntry SetAttribute(string name, string value)
        {
            if (Attributes == null)
                Attributes = new Dictionary<string, string>();

            Attributes[name] = value;
            return this;
        }

        /// <summary>
        /// 是否设置了Attribute
        /// </summary>
        public bool HasAttributes
        {
            get { return Attributes != null && Attributes.Any(a => a.Value != null); }
        }
    }
}
