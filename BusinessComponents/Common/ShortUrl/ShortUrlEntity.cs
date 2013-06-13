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
    /// 短网址实体
    /// </summary>
    [TableName("tn_ShortUrls")]
    [PrimaryKey("Alias", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class ShortUrlEntity : IEntity
    {

        /// <summary>
        /// 创建示例
        /// </summary>
        /// <param name="alias">Url别名</param>
        /// <returns></returns>
        public static ShortUrlEntity New(string alias = "")
        {
            ShortUrlEntity shortUrl = new ShortUrlEntity()
            {
                Alias = alias,
                Url = string.Empty,
                OtherShortUrl = string.Empty,
                DateCreated = DateTime.UtcNow
            };

            return shortUrl;
        }

        /// <summary>
        /// Url别名
        /// </summary>
        public string Alias { get; protected set; }

        /// <summary>
        /// 实际的Url地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 第三方服务处理后的短网址
        /// </summary>
        public string OtherShortUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Alias; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
