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
using Tunynet.FileStore;

namespace Tunynet.Common
{
    /// <summary>
    /// 广告位实体
    /// </summary>
    [TableName("tn_AdvertisingPosition")]
    [PrimaryKey("PositionId", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class AdvertisingPosition : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static AdvertisingPosition New()
        {
            AdvertisingPosition advertisingPosition = new AdvertisingPosition()
            {
                Description = string.Empty,
                FeaturedImage = string.Empty

            };
            return advertisingPosition;
        }

        #region 需持久化属性

        /// <summary>
        ///广告位Id
        /// </summary>
        public string PositionId { get; set; }

        /// <summary>
        ///投放区域
        /// </summary>
        public string PresentAreaKey { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///示意图
        /// </summary>
        public string FeaturedImage { get; set; }

        /// <summary>
        ///宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.PositionId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
