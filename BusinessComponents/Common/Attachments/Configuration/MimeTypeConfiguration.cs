//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Tunynet.Utilities;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// mime配置
    /// </summary>
    public static class MimeTypeConfiguration
    {
        private static Dictionary<string, string> MimeTypes = null;

        static MimeTypeConfiguration()
        {
            MimeTypes = new Dictionary<string, string>();
            XElement xElement = XElement.Load(WebUtility.GetPhysicalFilePath("~/Config/MimeType.config"));
            if (xElement != null)
            {
                foreach (var mimeTypeElement in xElement.Elements("mimeType"))
                {
                    if (mimeTypeElement.NodeType == XmlNodeType.Element)
                    {
                        string extensions = mimeTypeElement.Attribute("extensions").Value.Trim();
                        string mimeType = mimeTypeElement.Attribute("mimeType").Value.Trim();

                        foreach (string ext in extensions.Split(new char[] { ',' }))
                        {
                            string trimedExt = ext.Trim().ToLower();
                            if (trimedExt.StartsWith("."))
                                trimedExt = trimedExt.Substring(1);

                            if (trimedExt.Length > 0)
                                MimeTypes[trimedExt] = mimeType;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通过filename获取mime
        /// </summary>
        public static string GetMimeType(string fileName)
        {
            int index = fileName.LastIndexOf('.');
            if (index > 0 && index > fileName.LastIndexOf('\\'))
            {
                string extension = fileName.Substring(index + 1).ToLower(System.Globalization.CultureInfo.InvariantCulture);
                if (MimeTypes != null && MimeTypes.ContainsKey(extension))
                    return MimeTypes[extension];
            }
            return "application/octet-stream";
        }

        /// <summary>
        /// 通过mimeType获取文件扩展名
        /// </summary>
        public static string GetExtension(string mimeType)
        {
            if (MimeTypes != null)
            {
                mimeType = mimeType.ToLower();

                foreach (string extension in MimeTypes.Keys)
                {
                    if (MimeTypes[extension].ToLower() == mimeType)
                        return extension;
                }
            }

            return "unknown";
        }

        private static IList<string> imageExtensions = null;

        /// <summary>
        /// 获取所有是图片的后缀名集合
        /// </summary>
        public static IList<string> GetImageExtensions()
        {
            if (imageExtensions == null)
            {
                imageExtensions = MimeTypes.Where(n => n.Value.IndexOf("image") > -1).Select(n => n.Key).ToList();
            }
            return imageExtensions;
        }

        private static IList<string> extensions = null;

        /// <summary>
        /// 扩展名集合
        /// </summary>
        public static IList<string> Extensions
        {
            get
            {
                if (extensions == null)
                {
                    extensions = MimeTypes.Select(n => n.Key).ToList();
                }
                return extensions;
            }
        }
    }
}