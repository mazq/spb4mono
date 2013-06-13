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

namespace Tunynet.Search
{
    /// <summary>
    /// 搜索词
    /// </summary>
    [TableName("tn_SearchedTerms")]
    [PrimaryKey("Id", autoIncrement = false)]
    [CacheSetting(true, PropertyNamesOfArea = "SearchTypeCode", ExpirationPolicy = EntityCacheExpirationPolicies.Usual)]
    [Serializable]
    public class SearchedTerm : IEntity
    {

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static SearchedTerm New()
        {
            SearchedTerm searchedTerm = new SearchedTerm()
            {
                Term = string.Empty,
                SearchTypeCode = string.Empty,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow

            };
            return searchedTerm;
        }

        #region 需持久化属性

        /// <summary>
        ///Id（使用Id生成器自动生成）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///搜索词
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        ///搜索类型编码
        /// </summary>
        public string SearchTypeCode { get; set; }

        /// <summary>
        ///是否由管理员添加（人工干预）
        /// </summary>
        public bool IsAddedByAdministrator { get; set; }

        /// <summary>
        ///排序字段（默认与Id相同）
        /// </summary>
        public long DisplayOrder { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        /// <summary>
        ///最后使用日期
        /// </summary>
        public DateTime LastModified { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}

