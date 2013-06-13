//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tunynet.Imaging;
using Tunynet.Caching;
using PetaPoco;
using Tunynet.Common.Configuration;

namespace Tunynet.Common
{
    /// <summary>
    /// 广告实体
    /// </summary>
    [TableName("tn_Advertisings")]
    [PrimaryKey("AdvertisingId", autoIncrement = true)]
    [CacheSetting(true)]
    [Serializable]
    public class Advertising : SerializablePropertiesBase,IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Advertising New()
        {
            Advertising advertising = new Advertising()
            {
                Name = string.Empty,
                AttachmentUrl = string.Empty,
                Url = string.Empty,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow

            };
            return advertising;
        }

        #region 序列化属性

        /// <summary>
        /// 问题
        /// </summary>
        [Ignore]
        public int Width
        {
            get { return GetExtendedProperty<int>("Width"); }
            set { SetExtendedProperty("Width", value); }
        }

        /// <summary>
        /// 答案
        /// </summary>
        [Ignore]
        public int Height
        {
            get { return GetExtendedProperty<int>("Height"); }
            set { SetExtendedProperty("Height", value); }
        }

        #endregion

        #region 需持久化属性

        /// <summary>
        ///广告Id
        /// </summary>
        public long AdvertisingId { get; protected set; }

        /// <summary>
        ///广告名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///呈现方式
        /// </summary>
        public AdvertisingType AdvertisingType { get; set; }

        /// <summary>
        ///广告内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 文本样式
        /// </summary>
        public string TextStyle { get; set; }

        /// <summary>
        ///网络图片地址/上传图片存储地址/flash地址
        /// </summary>
        public string AttachmentUrl { get; set; }

        /// <summary>
        ///广告链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        ///是否新开窗口
        /// </summary>
        public bool IsBlank { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///投放数量
        /// </summary>
        public int UseredPositionCount { get; set; }

        /// <summary>
        ///排序顺序（默认和Id一致）
        /// </summary>
        public long DisplayOrder { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        public DateTime LastModified { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.AdvertisingId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        #region 扩展属性
        
        /// <summary>
        /// 该广告所在的广告位
        /// </summary>
        public IEnumerable<AdvertisingPosition> AdvertisingsPositions
        {
            get
            {
                return new AdvertisingService ().GetPositionsByAdvertisingId(this.AdvertisingId);
            }
        }
        #endregion
    }
}
