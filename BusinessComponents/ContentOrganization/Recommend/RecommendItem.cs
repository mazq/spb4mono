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
using Tunynet;

namespace Tunynet.Common
{
    //设计要点：
    //1、从SerializablePropertiesBase派生；
    //2、缓存分区：TypeId；

    /// <summary>
    /// 推荐内容
    /// </summary>
    [TableName("tn_RecommendItems")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TypeId")]
    [Serializable]
    public class RecommendItem : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        ///  推荐内容
        /// </summary>
        public static RecommendItem New()
        {
            RecommendItem recommendItem = new RecommendItem()
            {
                ItemName = string.Empty,
                FeaturedImage = string.Empty,
                ReferrerName = string.Empty,
                DateCreated = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow

            };
            return recommendItem;
        }

        #region 需持久化属性

        /// <summary>
        /// IsLink
        /// </summary>
        public bool IsLink { get; set; }

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///推荐类型Id
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        ///内容实体Id
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        ///推荐标题（默认为内容名称或标题，允许推荐人修改）
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        ///推荐标题图(存储图片文件名或完整图片链接地址)
        /// </summary>
        public string FeaturedImage { get; set; }

        /// <summary>
        ///推荐人DisplayName
        /// </summary>
        public string ReferrerName { get; set; }

        /// <summary>
        ///推荐人用户Id
        /// </summary>
        public long ReferrerId { get; set; }

        /// <summary>
        ///推荐日期
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///推荐期限
        /// </summary>
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        ///排序顺序（默认和Id一致）
        /// </summary>
        public long DisplayOrder { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        #region 扩展属性

        /// <summary>
        /// 推荐类别
        /// </summary>
        [Ignore]
        public RecommendItemType RecommendItemType
        {
            get
            {
                RecommendService recommendService = new RecommendService();
                return recommendService.GetRecommendType(this.TypeId);
            }
        }

        /// <summary>
        /// 详细页面地址
        /// </summary>
        [Ignore]
        public string DetailUrl
        {
            get
            {
                if (this.IsLink)
                    return this.LinkAddress;
                else
                {
                    var urlGetter = RecommendUrlGetterFactory.Get(this.TenantTypeId);
                    if (urlGetter == null)
                        return string.Empty;
                    return urlGetter.RecommendItemDetail(this.ItemId);
                }
            }
        }

        #endregion

        #region 序列化属性

        /// <summary>
        /// 外链地址
        /// </summary>
        [Ignore]
        public string LinkAddress
        {
            get { return GetExtendedProperty<string>("LinkAddress"); }
            set { SetExtendedProperty("LinkAddress", value); }
        }

        /// <summary>
        /// 作者Id
        /// </summary>
       [Ignore]
        public long UserId
        {
            get { return GetExtendedProperty<long>("UserId"); }
            set { SetExtendedProperty("UserId", value); }
        }
        #endregion
    }
}