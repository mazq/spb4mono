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

namespace Tunynet.UI
{
    /// <summary>
    /// Meta实体
    /// </summary>
    public class MetaEntry
    {
        private readonly TagBuilder _builder = new TagBuilder("meta");

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">meta的名称</param>
        /// <param name="content">meta的内容</param>
        public MetaEntry(string name, string content)
        {
            SetAttribute("name", name).SetAttribute("content", content);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        private MetaEntry()
        { }

        /// <summary>
        /// 合并meta1与meta2，Content用contentSeparator分隔
        /// </summary>
        /// <param name="meta1">用于合并的MetaEntry</param>
        /// <param name="meta2">用于合并的MetaEntry</param>
        /// <param name="contentSeparator">Meta的多个内容之间的分隔符</param>
        public static MetaEntry Combine(MetaEntry meta1, MetaEntry meta2, string contentSeparator)
        {
            var newMeta = new MetaEntry();
            Merge(newMeta._builder.Attributes, meta1._builder.Attributes, meta2._builder.Attributes);
            if (!String.IsNullOrEmpty(meta1.Content) && !String.IsNullOrEmpty(meta2.Content))
            {
                newMeta.Content = meta1.Content + contentSeparator + meta2.Content;
            }

            return newMeta;
        }

        /// <summary>
        /// 合并sources的属性到destination（重复的属性sources覆盖destination）
        /// </summary>
        /// <param name="destination">待合并的目标字典集合</param>
        /// <param name="sources">待合并的源字典集合</param>
        private static void Merge(IDictionary<string, string> destination, params IDictionary<string, string>[] sources)
        {
            foreach (var d in sources)
            {
                foreach (var pair in d)
                {
                    destination[pair.Key] = pair.Value;
                }
            }
        }

        /// <summary>
        /// Meta的名称
        /// </summary>
        public string Name
        {
            get
            {
                string value;
                _builder.Attributes.TryGetValue("name", out value);
                return value;
            }
            set { SetAttribute("name", value); }
        }

        /// <summary>
        /// Meta内容
        /// </summary>
        public string Content
        {
            get
            {
                string value;
                _builder.Attributes.TryGetValue("content", out value);
                return value;
            }
            set { SetAttribute("content", value); }
        }

        /// <summary>
        /// 添加Attribute
        /// </summary>
        private MetaEntry AddAttribute(string name, string value)
        {
            _builder.MergeAttribute(name, value);
            return this;
        }

        /// <summary>
        /// 设置Attribute
        /// </summary>
        private MetaEntry SetAttribute(string name, string value)
        {
            _builder.MergeAttribute(name, value, true);
            return this;
        }

        /// <summary>
        /// 设置 http-equiv
        /// </summary>
        public MetaEntry SetHttpEquiv(string httpEquiv)
        {
            return SetAttribute("http-equiv", httpEquiv);
        }

        /// <summary>
        /// 设置charset
        /// </summary>
        public MetaEntry SetCharset(string charset)
        {
            return SetAttribute("charset", charset);
        }

        /// <summary>
        /// 在页面呈现的html标签
        /// </summary>
        public string GetRenderingTag()
        {
            return _builder.ToString(TagRenderMode.SelfClosing);
        }
    }
}
