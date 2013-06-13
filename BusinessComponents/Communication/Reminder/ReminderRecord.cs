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
    /// <summary>
    /// 提醒记录
    /// </summary>
    [TableName("tn_ReminderRecords")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class ReminderRecord : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static ReminderRecord New()
        {
            ReminderRecord reminderRecord = new ReminderRecord()
            {
                DateCreated = DateTime.UtcNow,
                LastReminderTime = DateTime.UtcNow
            };
            return reminderRecord;
        }

        #region 需持久化属性

        /// <summary>
        ///id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///提醒方式(Email=1，手机=2)
        /// </summary>
        public int ReminderModeId { get; set; }

        /// <summary>
        ///提醒信息类型（Message=1，Notice=2，Invitation=3）
        /// </summary>
        public int ReminderInfoTypeId { get; set; }

        /// <summary>
        ///提醒对象Id
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        /// <summary>
        ///最后提醒时间
        /// </summary>
        public DateTime LastReminderTime { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
