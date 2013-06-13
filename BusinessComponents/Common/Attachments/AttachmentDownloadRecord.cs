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
using Tunynet.Utilities;
using System.Web;

namespace Tunynet.Common
{
    /// <summary>
    /// 附件下载记录
    /// </summary>
    [TableName("tn_AttachmentDownloadRecords")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId,OwnerId,TenantTypeId")]
    [Serializable]
    public class AttachmentDownloadRecord : IEntity
    {
        /// <summary>
        /// 实例化下载记录对象
        /// </summary>
        /// <param name="attachment">附件实体（用来为下载记录提供一些信息）</param>
        /// <returns></returns>
        public static AttachmentDownloadRecord New(Attachment attachment)
        {
            return new AttachmentDownloadRecord()
            {
                OwnerId = attachment.OwnerId,
                //UserId = attachment.UserId,
                //UserDisplayName = attachment.UserDisplayName,
                AttachmentId = attachment.AttachmentId,
                TenantTypeId = attachment.TenantTypeId,
                AssociateId = attachment.AssociateId,
                FromUrl = HttpContext.Current != null && HttpContext.Current.Request != null
                          ? HttpContext.Current.Request.UrlReferrer.ToString()
                          : string.Empty,
                IP = WebUtility.GetIP(),
                Price = attachment.Price,
                DownloadDate = DateTime.UtcNow,
                LastDownloadDate = DateTime.UtcNow
            };
        }


        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///附件Id
        /// </summary>
        public long AttachmentId { get; set; }

        /// <summary>
        ///附件关联Id（例如：博文Id、帖子Id）
        /// </summary>
        public long AssociateId { get; set; }

        /// <summary>
        ///拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///UserDisplayName
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        ///消费的积分
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        ///最近下载日期
        /// </summary>
        public DateTime LastDownloadDate { get; set; }

        /// <summary>
        ///下载日期
        /// </summary>
        public DateTime DownloadDate { get; set; }

        /// <summary>
        ///下载附件时页面的URL
        /// </summary>
        public string FromUrl { get; set; }

        /// <summary>
        ///附件下载人IP
        /// </summary>
        public string IP { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
