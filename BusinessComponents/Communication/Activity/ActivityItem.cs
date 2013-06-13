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
    /// 动态项目
    /// </summary>
    [TableName("tn_ActivityItems")]
    [PrimaryKey("ItemKey", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class ActivityItem : IEntity
    {

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static ActivityItem New()
        {
            ActivityItem activityItem = new ActivityItem() { };
            return activityItem;
        }

        #region 需持久化属性

        /// <summary>
        ///动态项目标识
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///应用程序Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///每个Owner是否仅生成一个动态
        /// </summary>
        public bool IsOnlyOnce { get; set; }

        /// <summary>
        ///是否推送给用户
        /// </summary>
        public bool IsUserReceived { get; set; }

        /// <summary>
        ///是否推送给站点
        /// </summary>
        public bool IsSiteReceived { get; set; }

        #endregion


        #region IEntity 成员

        object IEntity.EntityId { get { return this.ItemKey; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
