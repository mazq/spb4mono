//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Tunynet.Common
{
    /// <summary>
    /// 表情实体类
    /// </summary>
    public class Emotion
    {
        /// <summary>
        /// 创建实体示例
        /// </summary>
        /// <param name="xElement"></param>
        /// <returns></returns>
        public static Emotion New(XElement xElement)
        {
            Emotion emotion = new Emotion();

            if (xElement != null)
            {
                XAttribute attr = xElement.Attribute("code");
                if (attr != null)
                    emotion.code = attr.Value;
                attr = xElement.Attribute("fileName");
                if (attr != null)
                    emotion.fileName = attr.Value;
                attr = xElement.Attribute("description");
                if (attr != null)
                    emotion.description = attr.Value;
            }

            return emotion;
        }

        #region Properties

        private string code;

        /// <summary>
        /// 表情代码
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private string fileName;

        /// <summary>
        /// 表情图片文件名
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string description;

        /// <summary>
        /// 表情描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion Properties

        private string formatedCode;

        /// <summary>
        /// 用于在内容中表示的表情符号
        /// </summary>
        public string FormatedCode
        {
            get { return formatedCode; }
            set { formatedCode = value; }
        }

        private string imageUrl;

        /// <summary>
        /// 表情图片url
        /// </summary>
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }
    }
}