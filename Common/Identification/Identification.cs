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
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    [TableName("spb_Identifications")]
    [PrimaryKey("IdentificationId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class Identification : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Identification New()
        {
            Identification identification = new Identification()
            {
                TrueName = string.Empty,
                Email = string.Empty,
                Mobile = string.Empty,
                Description = string.Empty,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                DisposerId = 0
            };
            return identification;
        }

        #region 需持久化属性

        /// <summary>
        ///认证申请Id
        /// </summary>
        public long IdentificationId { get; protected set; }

        /// <summary>
        ///认证标识Id
        /// </summary>
        public long IdentificationTypeId { get; set; }

        /// <summary>
        ///申请人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///申请人真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        ///申请人身份证号
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        ///认证状态(0=fail,1=success,2=pending)
        /// </summary>
        public IdentificationStatus Status { get; set; }

        /// <summary>
        ///申请人电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///申请人手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        ///认证说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///申请时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///处理人Id
        /// </summary>
        public long DisposerId { get; set; }

        /// <summary>
        ///处理时间
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// 扫描证件图
        /// </summary>
        public string IdentificationLogo { get; set; }

        /// <summary>
        ///认证标识
        /// </summary>
        public IdentificationType IdentificationType 
        { 
            get 
            {
                //获得当前认证申请的认证标识实体
                return new IdentificationService().GetIdentificationTypes(null).Where(n=>n.IdentificationTypeId==this.IdentificationTypeId).SingleOrDefault();
            } 
        }

        /// <summary>
        ///用户
        /// </summary>
        public User User
        {
            get
            {
                return new UserService().GetFullUser(this.UserId);
            }
        }

        /// <summary>
        ///处理人
        /// </summary>
        public User DisposerUser
        {
            get
            {
                return new UserService().GetFullUser(this.DisposerId);
            }
        }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.IdentificationId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
