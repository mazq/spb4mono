//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using Tunynet;
using Tunynet.Utilities;
using Spacebuilder.UI;
using System.Xml.Linq;
using Newtonsoft.Json;
using PetaPoco;
using System.Web.Helpers;


namespace Spacebuilder.Common.Repositories
{
    /// <summary>
    /// 自定义样式Repository
    /// </summary>
    public class CustomStyleRepository : ICustomStyleRepository
    {
        // 缓存服务
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 默认PetaPocoDatabase实例
        /// </summary>
        private PetaPocoDatabase CreateDAO()
        {
            return PetaPocoDatabase.CreateInstance();
        }

        /// <summary>
        /// 获取预置的配色方案
        /// </summary>
        /// <param name="presentAreaKey"></param>
        public IEnumerable<CustomStyle> GetColorSchemes(string presentAreaKey)
        {
            string cacheKey = "ColorSchemes-" + presentAreaKey;
            List<CustomStyle> customStyles = cacheService.Get<List<CustomStyle>>(cacheKey);
            if (customStyles == null)
            {
                customStyles = new List<CustomStyle>();
                XElement colorSchemesElement = XElement.Load(WebUtility.GetPhysicalFilePath(string.Format("~/Themes/{0}/Custom/ColorScheme.config", presentAreaKey)));
                if (colorSchemesElement != null)
                {
                    IEnumerable<XElement> colorSchemeElements = colorSchemesElement.Elements("colorScheme");
                    foreach (XElement colorSchemeElement in colorSchemeElements)
                    {
                        Dictionary<string, string> definedColours = new Dictionary<string, string>();
                        CustomStyle customStyle = new CustomStyle();
                        var isDarkAttr = colorSchemeElement.Attribute("isDark");
                        bool isDark = false;
                        if (isDarkAttr != null)
                            bool.TryParse(isDarkAttr.Value, out isDark);
                        customStyle.IsDark = isDark;
                        var imageUrlAttr = colorSchemeElement.Attribute("imageUrl");
                        if (imageUrlAttr != null)
                            customStyle.ImageUrl = imageUrlAttr.Value;

                        IEnumerable<XElement> colorElements = colorSchemeElement.Elements("color");
                        foreach (XElement colorElement in colorElements)
                        {
                            var labelAttr = colorElement.Attribute("label");
                            if (labelAttr == null)
                                continue;
                            string colorLabel = labelAttr.Value;
                            var valueAttr = colorElement.Attribute("value");
                            if (valueAttr == null)
                                continue;
                            definedColours[colorLabel] = valueAttr.Value;
                        }
                        customStyle.DefinedColours = definedColours;
                        customStyles.Add(customStyle);
                    }
                }
                cacheService.Add(cacheKey, customStyles, CachingExpirationType.RelativelyStable);
            }
            return customStyles;
        }

        /// <summary>
        /// 获取用户自定义风格
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">OwnerId</param>
        /// <returns>无相应数据返回null</returns>
        public CustomStyleEntity Get(string presentAreaKey, long ownerId)
        {
            string cacheKey = GetCacheKey_CustomStyleEntity(presentAreaKey, ownerId);

            CustomStyleEntity result = cacheService.GetFromFirstLevel<CustomStyleEntity>(cacheKey);
            if (result == null)
            {
                PetaPocoDatabase database = CreateDAO();
                var sql = PetaPoco.Sql.Builder;
                sql.Select("*").From("spb_CustomStyles").Where("PresentAreaKey=@0", presentAreaKey).Where("OwnerId=@0", ownerId);
                result = database.FirstOrDefault<CustomStyleEntity>(sql);
                if (result != null)
                    result.CustomStyle = Deserialize(result.SerializedCustomStyle);

                cacheService.Add(cacheKey, result, CachingExpirationType.UsualSingleObject);
            }
            return result;
        }

        /// <summary>
        /// 保存用户自定义风格
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">OwnerId</param>
        /// <param name="customStyle">自定义风格实体</param>
        public void Save(string presentAreaKey, long ownerId, CustomStyle customStyle)
        {
            PetaPocoDatabase database = CreateDAO();
            database.OpenSharedConnection();
            CustomStyleEntity entity = Get(presentAreaKey, ownerId);
            string customStyleXml = Serialize(customStyle);

            if (entity != null)
            {
                customStyle.LastModified = DateTime.UtcNow;
                entity.CustomStyle = customStyle;
                entity.SerializedCustomStyle = customStyleXml;
                entity.LastModified = DateTime.UtcNow;
                database.Update(entity);
            }
            else
            {
                entity = CustomStyleEntity.New();
                entity.CustomStyle = customStyle;
                entity.PresentAreaKey = presentAreaKey;
                entity.OwnerId = ownerId;
                entity.SerializedCustomStyle = customStyleXml;
                database.Insert(entity);
            }

            database.CloseSharedConnection();
            cacheService.Set(GetCacheKey_CustomStyleEntity(presentAreaKey, ownerId), entity, CachingExpirationType.UsualSingleObject);
        }

        /// <summary>
        /// 获取ClassType
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey_CustomStyleEntity(string presentAreaKey, long ownerId)
        {
            return string.Format("CustomStyle::P:{0}-O:{1}", presentAreaKey, ownerId);
        }

        /// <summary>
        /// 把CustomStyleEntity对象转换成xml
        /// </summary>
        /// <param name="customStyle">被转换的对象</param>
        /// <returns>序列化后的xml字符串</returns>
        private string Serialize(CustomStyle customStyle)
        {
            string xml = null;
            if (customStyle != null)
            {
                xml = Json.Encode(customStyle);
            }
            return xml;
        }

        /// <summary>
        /// 把json的字符串反序列化成CustomStyle对象
        /// </summary>
        /// <param name="json">被反序列化的json字符串</param>
        /// <returns>反序列化后的CustomStyle</returns>
        private CustomStyle Deserialize(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return Json.Decode<CustomStyle>(json);
            }
            return null;
        }
    }
}
