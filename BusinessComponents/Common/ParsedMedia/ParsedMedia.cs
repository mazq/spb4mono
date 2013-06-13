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
    [TableName("tn_ParsedMedias")]
    [PrimaryKey("Alias", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class ParsedMedia : IEntity
    {

        /// <summary>
        /// 创建示例
        /// </summary>
        /// <param name="alias">Url别名</param>
        /// <returns></returns>
        public static ParsedMedia New(string alias = "")
        {
            ParsedMedia parsedMedia = new ParsedMedia()
            {
                Url = string.Empty,
                Description = string.Empty,
                ThumbnailUrl = string.Empty,
                PlayerUrl = string.Empty,
                SourceFileUrl = string.Empty,
                DateCreated = DateTime.UtcNow

            };
            return parsedMedia;
        }

        #region 需持久化属性

        /// <summary>
        ///Url别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        ///网址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///多媒体类型
        /// </summary>
        public MediaType MediaType { get; set; }

        /// <summary>
        ///多媒体名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///缩略图地址
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        ///播放器地址
        /// </summary>
        public string PlayerUrl { get; set; }

        /// <summary>
        ///源文件地址
        /// </summary>
        public string SourceFileUrl { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Alias; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        /// <summary>
        /// 短网址
        /// </summary>
        [Ignore]
        public string ShortUrl
        {
            get
            {
                ShortUrlService shortUrlService = new ShortUrlService();
                return shortUrlService.GetShortUrl(this.Alias);
            }
        }
    }
}
