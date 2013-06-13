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
    [TableName("tn_InvitationCodes")]
    [PrimaryKey("Code", autoIncrement = false)]
    
    [CacheSetting(true,PropertyNamesOfArea="UserId")]
    [Serializable]
    public class InvitationCode:IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        
        public static InvitationCode New()
        {
            InvitationCode invitationCode = new InvitationCode()
            {
                ExpiredDate = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow

            };
            return invitationCode;
        }

        #region 需持久化属性

        /// <summary>
        ///(使用MD5_16生成)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///是否可以多次使用
        /// </summary>
        public bool IsMultiple { get; set; }

        /// <summary>
        ///过期日期
        /// </summary>
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Code; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
