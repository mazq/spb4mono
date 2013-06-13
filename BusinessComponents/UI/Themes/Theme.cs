//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using System.Xml.Linq;

namespace Tunynet.UI
{
    /// <summary>
    /// 主题实体
    /// </summary>
    [TableName("tn_Themes")]
    [PrimaryKey("Id", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class Theme : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Theme New(string presentAreaKey, string themeKey)
        {
            Theme theme = new Theme()
            {
                PresentAreaKey = presentAreaKey,
                ThemeKey = themeKey,
                Id = presentAreaKey + "," + themeKey,

                Parent = string.Empty,
                Version = string.Empty
            };
            return theme;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Theme()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xElement">XElement</param>
        /// <param name="themeKey">themeKey</param>
        public Theme(XElement xElement, string themeKey)
        {
            if (xElement != null)
            {
                XAttribute attr;
                attr = xElement.Attribute("presentAreaKey");
                if (attr != null)
                    this.PresentAreaKey = attr.Value;
                attr = xElement.Attribute("parent");
                if (attr != null)
                    this.Parent = attr.Value;
                attr = xElement.Attribute("version");
                if (attr != null)
                    this.Version = attr.Value;
            }
            this.ThemeKey = themeKey;
            this.Id = string.Join(",", this.PresentAreaKey, themeKey);
        }

        #region 需持久化属性

        /// <summary>
        /// Id(格式：PresentAreaKey,ThemeKey)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// PresentAreaKey
        /// </summary>
        public string PresentAreaKey { get; set; }

        /// <summary>
        /// ThemeKey
        /// </summary>
        public string ThemeKey { get; set; }

        /// <summary>
        /// 父主题ThemeKey
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        #endregion

        #region IEntity 成员
        object IEntity.EntityId { get { return this.Id; } }
        bool IEntity.IsDeletedInDatabase { get; set; }
        #endregion

    }
}
