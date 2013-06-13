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

namespace Tunynet.Common
{
    /// <summary>
    /// 呈现区域实体
    /// </summary>
    [TableName("tn_PresentAreas")]
    [PrimaryKey("PresentAreaKey", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class PresentArea : IEntity
    {
        #region 需持久化属性

        /// <summary>
        /// 呈现区域标识（与目录名称相同）
        /// </summary>
        public string PresentAreaKey { get; set; }

        /// <summary>
        /// 是否可有多个实例
        /// </summary>
        public bool AllowMultipleInstances { get; set; }

        /// <summary>
        /// 是否启用皮肤
        /// </summary>
        public bool EnableThemes { get; set; }


        /// <summary>
        /// 默认皮肤（格式：PresentAreaKey,ThemeKey,AppearanceKey，与AppearanceID相同）
        /// </summary>
        public string DefaultAppearanceID { get; set; }

        /// <summary>
        /// 皮肤文件所在位置（以”~/目录”表示）
        /// </summary>
        public string ThemeLocation { get; set; }

        #endregion


        #region 扩展属性

        private string defaultThemeKey;
        /// <summary>
        /// 默认ThemeKey
        /// </summary>
        [Ignore]
        public string DefaultThemeKey
        {
            get
            {
                if (defaultThemeKey == null)
                    SplitThemeKeyAndAppearanceKey();

                return defaultThemeKey;
            }
        }

        private string defaultAppearanceKey;
        /// <summary>
        /// 默认AppearanceKey
        /// </summary>
        [Ignore]
        public string DefaultAppearanceKey
        {
            get
            {
                if (defaultAppearanceKey == null)
                    SplitThemeKeyAndAppearanceKey();

                return defaultAppearanceKey;
            }
        }

        /// <summary>
        /// 从DefaultAppearanceId分隔ThemeKey和AppearanceKey
        /// </summary>
        private void SplitThemeKeyAndAppearanceKey()
        {
            if (string.IsNullOrEmpty(DefaultAppearanceID))
                return;

            string[] arrayOfThemeAppearance = DefaultAppearanceID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (arrayOfThemeAppearance.Length == 3)
            {
                defaultThemeKey = arrayOfThemeAppearance[1];
                defaultAppearanceKey = arrayOfThemeAppearance[2];
            }
        }

        #endregion


        #region IEntity 成员
        object IEntity.EntityId { get { return this.PresentAreaKey; } }
        bool IEntity.IsDeletedInDatabase { get; set; }
        #endregion

    }
}
