
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Common.Repositories;
using Tunynet.Utilities;

namespace Tunynet.Common
{
    /// <summary>
    /// 学校实体类
    /// </summary>
    [TableName("tn_Schools")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "AreaCode", ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class School : IEntity
    {
        public static School New()
        {
            School school = new School()
            {
                Name = string.Empty,
                PinyinName = string.Empty,
                ShortPinyinName = string.Empty

            };
            return school;
        }
        /// <summary>
        /// 标识列
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// 院校名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称的拼音（例如“汉语”：hanyu）
        /// </summary>
        public string PinyinName { get; set; }

        /// <summary>
        /// 名称的简写拼音（例如“汉语”的简写拼音：hy）
        /// </summary>
        public string ShortPinyinName { get; set; }

        /// <summary>
        /// 学校类型
        /// </summary>
        public SchoolType SchoolType { get; set; }

        /// <summary>
        /// 所在地区编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public long DisplayOrder { get; set; }


        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
