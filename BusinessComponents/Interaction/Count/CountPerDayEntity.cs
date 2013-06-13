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
    /// 每日计数实体
    /// </summary>
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true)]
    [Serializable]
    public class CountPerDayEntity : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static CountPerDayEntity New()
        {
            CountPerDayEntity countsPerDayEntity = new CountPerDayEntity();
            return countsPerDayEntity;
        }

        #region 需持久化属性

        /// <summary>
        ///id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///拥有者id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///计数对象id
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        ///统计日期的年份
        /// </summary>
        public int ReferenceYear { get; set; }

        /// <summary>
        ///统计日期的月份
        /// </summary>
        public int ReferenceMonth { get; set; }

        /// <summary>
        ///统计日期的天
        /// </summary>
        public int ReferenceDay { get; set; }

        /// <summary>
        ///当天计数
        /// </summary>
        public int StatisticsCount { get; set; }

        /// <summary>
        ///计数类型
        /// </summary>
        public string CountType { get; set; }

        #endregion

        /// <summary>
        /// 统计日期
        /// </summary>
        [Ignore]
        public DateTime ReferenceDate
        {
            get
            {
                return new DateTime(ReferenceYear, ReferenceMonth, ReferenceDay);
            }
        }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}