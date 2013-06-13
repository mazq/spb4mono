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
using System.Web.Helpers;
using Tunynet.Utilities;
using System.Configuration;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Tunynet.Mvc
{
    /// <summary>
    /// Html编辑器选项
    /// </summary>
    /// <remarks> 
    /// <para> Html编辑器已实现以下功能：</para>
    /// <list type="number">
    /// <item>预置3种编辑器显示模式</item>
    /// <item>可通过设置web.config中的HttpCompressEnabled来启用或禁用Html编辑器的GZIP压缩</item>
    /// <item>可以设置是否延迟加载</item>
    /// <item>对jquery包装集扩展了向编辑器插入内容的快捷方法</item>
    /// </list>
    /// <para>基于tinymce 插件构建，更多信息请参见：</para> 
    /// <list type="number">
    /// <item>http://www.tinymce.com/</item>
    /// </list>    
    /// <para>依赖文件：</para>
    /// <list type="number">
    /// <item>jquery.tinymce.js</item>
    /// <item>jquery.tn.htmlEditor.unobtrusive.js</item>
    /// </list>
    /// </remarks>
    public class HtmlEditorOptions
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public HtmlEditorOptions(HtmlEditorMode editorMode)
        {
            this.EditorMode = editorMode;
            this.ShowSmileyButton = true;
        }

        /// <summary>
        /// 预置的编辑器显示模式
        /// </summary>
        public HtmlEditorMode EditorMode { get; set; }

        /// <summary>
        /// 是否延迟加载
        /// </summary>
        public bool Lazyload { get; set; }

        /// <summary>
        /// 是否显示表情按钮
        /// </summary>
        public bool ShowSmileyButton { get; set; }

        /// <summary>
        /// tinymce插件中提供的其他option选项
        /// </summary>
        public Dictionary<string, object> AdditionalOptions { get; set; }

        /// <summary>
        /// 文本框的html属性集合
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// 自定义工具条
        /// </summary>
        private Dictionary<string, string> CustomButtons { get; set; }

        #region 连缀方法

        /// <summary>
        /// 设置是否显示表情按钮
        /// </summary>
        /// <param name="showSmileyButton">是否显示表情按钮</param>
        public HtmlEditorOptions SetShowSmileyButton(bool showSmileyButton)
        {
            this.ShowSmileyButton = showSmileyButton;
            return this;
        }

        /// <summary>
        /// 设置是否延迟加载
        /// </summary>
        /// <param name="lazyload">是否延迟加载</param>
        public HtmlEditorOptions SetLazyload(bool lazyload)
        {
            this.Lazyload = lazyload;
            return this;
        }
        /// <summary>
        /// tinymce插件中提供的其他option选项
        /// </summary>
        /// <param name="optionName">选项名</param>
        /// <param name="optionValue">选项值</param>
        public HtmlEditorOptions MergeAdditionalOption(string optionName, object optionValue)
        {
            if (this.AdditionalOptions == null)
                this.AdditionalOptions = new Dictionary<string, object>();
            this.AdditionalOptions[optionName] = optionValue;
            return this;
        }

        /// <summary>
        /// 添加html属性
        /// </summary>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">属性值</param>
        /// <remarks>如果存在，则覆盖</remarks>
        public HtmlEditorOptions MergeHtmlAttribute(string attributeName, object attributeValue)
        {
            if (this.HtmlAttributes == null)
                this.HtmlAttributes = new Dictionary<string, object>();
            this.HtmlAttributes[attributeName] = attributeValue;
            return this;
        }

        /// <summary>
        /// 增加上传图片按钮
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="associateId"></param>
        /// <returns></returns>
        public HtmlEditorOptions AddPhotoButton(string tenantTypeId, long associateId = 0)
        {
            if (this.CustomButtons == null)
                this.CustomButtons = new Dictionary<string, string>();

            if (UserContext.CurrentUser != null)
            {
                string url = SiteUrls.Instance()._ImageManage(tenantTypeId, associateId);
                this.CustomButtons["photoButton"] = url;
            }
            return this;
        }

        /// <summary>
        /// 增加上传文件按钮
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="associateId"></param>
        /// <returns></returns>
        public HtmlEditorOptions AddFileButton(string tenantTypeId, long associateId = 0)
        {
            if (this.CustomButtons == null)
                this.CustomButtons = new Dictionary<string, string>();
            if (UserContext.CurrentUser != null)
            {
                string url = SiteUrls.Instance()._AttachmentManage(tenantTypeId, associateId);
                this.CustomButtons["fileButton"] = url;
            }
            return this;
        }

        /// <summary>
        /// 增加插入视频按钮
        /// </summary>
        /// <returns></returns>
        public HtmlEditorOptions AddVideoButton()
        {
            if (this.CustomButtons == null)
                this.CustomButtons = new Dictionary<string, string>();
            string url = SiteUrls.Instance()._AddVideo();
            this.CustomButtons["videoButton"] = url;
            return this;
        }

        /// <summary>
        /// 增加插入音乐按钮
        /// </summary>
        /// <returns></returns>
        public HtmlEditorOptions AddMusicButton()
        {
            if (this.CustomButtons == null)
                this.CustomButtons = new Dictionary<string, string>();
            string url = SiteUrls.Instance()._AddMusic();
            this.CustomButtons["musicButton"] = url;
            return this;
        }

        /// <summary>
        /// 增加@用户按钮
        /// </summary>
        /// <returns></returns>
        public HtmlEditorOptions AddAtUserButton()
        {
            if (this.CustomButtons == null)
                this.CustomButtons = new Dictionary<string, string>();
            
            string url = SiteUrls.Instance()._AtUsers();
            this.CustomButtons["atuserButton"] = url;
            return this;
        }

        #endregion

        /// <summary>
        /// 转为Html属性集合
        /// </summary>
        public IDictionary<string, object> ToUnobtrusiveHtmlAttributes()
        {
            var result = new Dictionary<string, object>();
            if (HtmlAttributes != null)
                result = new Dictionary<string, object>(HtmlAttributes);
            if (result.Any(n => n.Key == "style"))
                result["style"] += "width:100%;";
            else
                result["style"] = "width:100%;";

            result["plugin"] = "tinymce";
            var data = new Dictionary<string, object>();
            if (AdditionalOptions != null)
                data = new Dictionary<string, object>(AdditionalOptions);

            data.TryAdd("editorMode", this.EditorMode.ToString());
            data.TryAdd("lazyload", this.Lazyload);

            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();
            data.TryAdd("language", siteSettings.DefaultLanguage.ToLower());
            bool tinyMCEJavaScriptGzipEnabled = false;
            bool.TryParse(ConfigurationManager.AppSettings["HttpCompressEnabled"], out tinyMCEJavaScriptGzipEnabled);

            if (tinyMCEJavaScriptGzipEnabled)
                data.TryAdd("script_url", WebUtility.ResolveUrl("~/scripts/tinymce/tiny_mce_gzip.ashx"));
            else
                data.TryAdd("script_url", WebUtility.ResolveUrl("~/scripts/tinymce/tiny_mce_src.js"));
            result.Add("data", Json.Encode(data));
            if (this.CustomButtons == null)
                this.CustomButtons = new Dictionary<string, string>();
            if (this.ShowSmileyButton)
                this.CustomButtons["smileyButton"] = SiteUrls.Instance()._EmotionSelector();
            result.TryAdd("customButtons", Json.Encode(this.CustomButtons));
            return result;
        }
    }
}