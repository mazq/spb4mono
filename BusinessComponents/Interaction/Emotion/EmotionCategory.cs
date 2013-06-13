//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using PetaPoco;

namespace Tunynet.Common
{
    /// <summary>
    /// 表情图片类别实体
    /// </summary>
    [TableName("tn_EmotionCategories")]
    [PrimaryKey("DirectoryName", autoIncrement = false)]
    [Serializable]
    public class EmotionCategory : IEntity
    {
        /// <summary>
        /// 创建实体示例
        /// </summary>
        /// <param name="directoryName">表情包目录名</param>
        public static EmotionCategory New(string directoryName)
        {
            EmotionCategory category = new EmotionCategory();

            if (!string.IsNullOrEmpty(directoryName))
                category.DirectoryName = directoryName;

            return category;
        }

        /// <summary>
        ///目录名
        /// </summary>
        public string DirectoryName
        {
            get;
            protected set;
        }

        private string codePrefix;
        /// <summary>
        /// 用来区分表情不同分组表情的表情代码前缀
        /// </summary>
        [Ignore]
        public string CodePrefix
        {
            get { return codePrefix; }
        }

        private string categoryName;
        /// <summary>
        /// 类别名称
        /// </summary>
        [Ignore]
        public string CategoryName
        {
            get { return categoryName; }
        }

        private string description = string.Empty;
        /// <summary>
        /// 类别描述
        /// </summary>
        [Ignore]
        public string Description
        {
            get { return description; }
        }

        private int emotionMaxHeight;
        /// <summary>
        /// 表情最大高度
        /// </summary>
        [Ignore]
        public int EmotionMaxHeight
        {
            get { return emotionMaxHeight; }
        }

        private int emotionMaxWidth;
        /// <summary>
        /// 表情最大宽度
        /// </summary>
        [Ignore]
        public int EmotionMaxWidth
        {
            get { return emotionMaxWidth; }
        }

        private int displayOrder = 100;
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        private bool isEnabled = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        #region 扩展属性

        private string physicalDirectoryPath;
        /// <summary>
        ///  物理目录
        /// </summary>
        [Ignore]
        public string PhysicalDirectoryPath
        {
            get { return physicalDirectoryPath; }
        }

        private List<Emotion> emotions = null;
        /// <summary>
        /// 表情图片
        /// </summary>
        [Ignore]
        public List<Emotion> Emotions
        {
            get
            {
                if (emotions == null)
                    emotions = new List<Emotion>();

                return emotions;
            }
        }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.DirectoryName; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        /// <summary>
        ///初始化非数据库获取的属性值 
        /// </summary>
        /// <param name="xElement">表情分类配置节点</param>
        /// <param name="physicalDirectoryPath">表情包物理目录</param>
        public void InitPropertyValue(XElement xElement, string physicalDirectoryPath)
        {
            if (xElement != null)
            {
                XAttribute attr = null;
                attr = xElement.Attribute("categoryName");
                if (attr != null)
                    this.categoryName = attr.Value;
                attr = xElement.Attribute("description");
                if (attr != null)
                    this.description = attr.Value;
                attr = xElement.Attribute("codePrefix");
                if (attr != null)
                    this.codePrefix = attr.Value;
                attr = xElement.Attribute("emotionMaxHeight");
                if (attr != null)
                    int.TryParse(attr.Value, out this.emotionMaxHeight);
                attr = xElement.Attribute("emotionMaxWidth");
                if (attr != null)
                    int.TryParse(attr.Value, out this.emotionMaxWidth);
            }

            if (!string.IsNullOrEmpty(physicalDirectoryPath))
                this.physicalDirectoryPath = physicalDirectoryPath;
        }

        /// <summary>
        /// 用于EmotionCategory排序
        /// </summary>
        internal class EmotionCategoryComparer : IComparer<EmotionCategory>
        {
            #region IComparer<EmotionCategory> 成员

            public int Compare(EmotionCategory x, EmotionCategory y)
            {
                if (x == null || y == null)
                    return 0;

                return (x.DisplayOrder.CompareTo(y.DisplayOrder));
            }

            #endregion
        }
    }
}