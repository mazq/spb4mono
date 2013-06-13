//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;

using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 公告实体类
    /// </summary>
    [TableName("spb_Announcements")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true)]
    [Serializable]
    public class Announcement : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Announcement New()
        {
            Announcement announcement = new Announcement()
            {
                Subject = string.Empty,
                SubjectStyle = string.Empty,
                Body = string.Empty,
                HyperLinkUrl = string.Empty,
                ReleaseDate = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                CreatDate = DateTime.UtcNow.ToLocalTime()

            };
            return announcement;
        }

        #region 需持久化属性

        /// <summary>
        ///Primary key
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///公告主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///主题字体风格
        /// </summary>
        public string SubjectStyle { get; set; }

        /// <summary>
        ///公告内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///是否是连接
        /// </summary>
        public bool IsHyperLink { get; set; }

        /// <summary>
        ///链接地址
        /// </summary>
        public string HyperLinkUrl { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool EnabledDescription { get; set; }

        /// <summary>
        ///发布时间
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        ///过期时间
        /// </summary>
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        ///更新时间
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public DateTime CreatDate { get; set; }

        /// <summary>
        ///创建人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///显示顺序
        /// </summary>
        public long DisplayOrder { get; set; }

        /// <summary>
        ///展示区域
        /// </summary>
        public string DisplayArea { get; set; }

        #endregion

        #region 扩展属性和方法

        /// <summary>
        /// 浏览数
        /// </summary>
        [Ignore]
        public int HitTimes
        {
            get
            {
                CountService countService = new CountService(TenantTypeIds.Instance().Announcement());
                return countService.Get(CountTypes.Instance().HitTimes(), this.Id);
            }
        }

        /// <summary>
        /// 撰稿人
        /// </summary>
        [Ignore]
        public string UserName
        { get; set; }

        /// <summary>
        /// 管理员标示
        /// </summary>
        [Ignore]
        public bool IsAdministrator
        { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
