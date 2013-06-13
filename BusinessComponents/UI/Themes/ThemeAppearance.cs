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
    /// 外观实体
    /// </summary>
    [TableName("tn_ThemeAppearances")]
    [PrimaryKey("Id", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class ThemeAppearance : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="themeKey"></param>
        /// <param name="appearanceKey"></param>
        /// <param name="name"></param>
        /// <param name="previewImage"></param>
        /// <returns></returns>
        public static ThemeAppearance New(string presentAreaKey, string themeKey, string appearanceKey, string name, string previewImage)
        {
            ThemeAppearance themeAppearance = new ThemeAppearance()
            {
                PresentAreaKey = presentAreaKey,
                ThemeKey = themeKey,
                AppearanceKey = appearanceKey,
                Id = string.Format("{0},{1},{2}", presentAreaKey, themeKey, appearanceKey),
                Name = name,
                PreviewImage = previewImage,

                PreviewLargeImage = string.Empty,
                //LogoFileName = string.Empty,
                Description = string.Empty,
                Tags = string.Empty,
                Author = string.Empty,
                Copyright = string.Empty,
                LastModified = DateTime.UtcNow,
                Version = string.Empty,
                ForProductVersion = string.Empty,
                DateCreated = DateTime.UtcNow,
                Roles = string.Empty
            };
            return themeAppearance;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ThemeAppearance()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xElement">XElement</param>
        /// <param name="appearanceKey">appearanceKey</param>
        public ThemeAppearance(XElement xElement, string appearanceKey)
        {
            if (xElement != null)
            {
                XAttribute attr;
                attr = xElement.Attribute("presentAreaKey");
                if (attr != null)
                    this.PresentAreaKey = attr.Value;
                attr = xElement.Attribute("themeKey");
                if (attr != null)
                    this.ThemeKey = attr.Value;
                attr = xElement.Attribute("name");
                if (attr != null)
                    this.Name = attr.Value;
                attr = xElement.Attribute("previewImage");
                if (attr != null)
                    this.PreviewImage = attr.Value;
                attr = xElement.Attribute("previewLargeImage");
                if (attr != null)
                    this.PreviewLargeImage = attr.Value;
                attr = xElement.Attribute("description");
                if (attr != null)
                    this.Description = attr.Value;
                attr = xElement.Attribute("tags");
                if (attr != null)
                    this.Tags = attr.Value;
                attr = xElement.Attribute("author");
                if (attr != null)
                    this.Author = attr.Value;
                attr = xElement.Attribute("copyright");
                if (attr != null)
                    this.Copyright = attr.Value;
                attr = xElement.Attribute("lastModified");
                if (attr != null)
                {
                    DateTime lastModified = DateTime.MinValue;
                    if (DateTime.TryParse(attr.Value, out lastModified))
                        this.LastModified = lastModified;
                }
                attr = xElement.Attribute("version");
                if (attr != null)
                    this.Version = attr.Value;
                attr = xElement.Attribute("forProductVersion");
                if (attr != null)
                    this.ForProductVersion = attr.Value;
            }
            this.AppearanceKey = appearanceKey;
            this.Id = string.Join(",", this.PresentAreaKey, this.ThemeKey, this.AppearanceKey);
            this.LastModified = DateTime.UtcNow;
            this.DateCreated = DateTime.UtcNow;
            this.Roles = string.Empty;
            this.IsEnabled = true;
        }

        #region 需持久化属性

        /// <summary>
        /// Id（格式：PresentAreaKey,ThemeKey,AppearanceKey）
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// PresentAreaKey
        /// </summary>
        public string PresentAreaKey { get; set; }

        /// <summary>
        /// ThemeKey
        /// </summary>
        public string ThemeKey { get; set; }

        /// <summary>
        /// AppearanceKey
        /// </summary>
        public string AppearanceKey { get; set; }

        /// <summary>
        /// 外观名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 皮肤预览图片
        /// </summary>
        public string PreviewImage { get; set; }

        /// <summary>
        /// 皮肤大预览图片
        /// </summary>
        public string PreviewLargeImage { get; set; }

        ///// <summary>
        ///// 重置的网站Logo图片名称
        ///// </summary>
        //public string LogoFileName { get; set; }

        /// <summary>
        /// 皮肤描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 标签（多个标签用逗号分隔）
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 皮肤作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 版权声明
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// 皮肤最后更新日期
        /// </summary>
        public DateTime LastModified { get; protected set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 适用产品版本号
        /// </summary>
        public string ForProductVersion { get; set; }

        /// <summary>
        /// 皮肤安装日期
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        /// <summary>
        /// 皮肤是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 使用人数
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 允许使用的角色名称 多个角色用’,’分隔 
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// 允许的最小等级(用户等级或群组等级)
        /// </summary>
        public int RequiredRank { get; set; }

        #endregion

        #region IEntity 成员
        object IEntity.EntityId { get { return this.Id; } }
        bool IEntity.IsDeletedInDatabase { get; set; }
        #endregion

    }
}
