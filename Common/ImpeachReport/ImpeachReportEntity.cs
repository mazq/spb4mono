using System;//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Common;

namespace Spacebuilder.Common
{

    /// <summary>
    /// 用户举报实体
    /// </summary>
    [TableName("spb_ImpeachReports")]
    [PrimaryKey("ReportId", autoIncrement = true)]
    [Serializable]
    public class ImpeachReportEntity : IEntity
    {
        #region 构造器

        /// <summary>
        /// 构造器
        /// </summary>
        public ImpeachReportEntity()
        {
        }

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <param name="userId">举报人Id</param>
        /// <returns>用户举报实体</returns>
        public static ImpeachReportEntity New()
        {
            ImpeachReportEntity impeachReport = new ImpeachReportEntity()
            {
                ReportId = 0,
                UserId = 0,
                Reporter = string.Empty,
                ReportedUserId = 0,
                Email = string.Empty,
                Title = string.Empty,
                Telephone = string.Empty,
                Reason = 0,
                Description = string.Empty,
                Url = string.Empty,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                Status = false,
                DisposerId = 0

            };
            return impeachReport;
        }

        #endregion

        #region 需持久化属性

        /// <summary>
        ///ReportId
        /// </summary>
        public long ReportId { get; protected set; }

        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///Reporter
        /// </summary>
        public string Reporter { get; set; }

        /// <summary>
        /// ReportedUserId
        /// </summary>
        public long ReportedUserId { get; set; }

        /// <summary>
        ///Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///Telephone
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        ///Reason
        /// </summary>
        public ImpeachReason Reason { get; set; }

        /// <summary>
        ///Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///DateCreated
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///LastModified
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        ///Status
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        ///DisposerId
        /// </summary>
        public long DisposerId { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.ReportId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        #region 扩展属性

        /// <summary>
        /// 处理人
        /// </summary>
        public IUser Disposer
        {
            get
            {
                UserService userService = new UserService();
                return userService.GetUser(DisposerId);
            }
        }
        #endregion
    }
}

