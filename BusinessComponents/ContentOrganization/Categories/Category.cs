//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;

namespace Tunynet.Common
{
    /// <summary>
    /// 分类实体类
    /// </summary>
    [TableName("tn_Categories")]
    [PrimaryKey("CategoryId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "OwnerId,ParentId")]
    [Serializable]
    public class Category : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Category New()
        {
            Category categorie = new Category()
            {
                CategoryName = string.Empty,
                Description = string.Empty,
                TenantTypeId = string.Empty,
                AuditStatus = AuditStatus.Success,
                PrivacyStatus = PrivacyStatus.Public,
                LastModified = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow

            };
            return categorie;
        }

        #region 需持久化属性

        /// <summary>
        ///类别Id 
        /// </summary>
        public long CategoryId { get; protected set; }

        /// <summary>
        ///父评论Id（顶级ParentId=0）
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        ///拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///类别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        ///类别描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public long DisplayOrder { get; set; }

        /// <summary>
        ///类别深度 顶级类别 Depth=0
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        ///子类别数目
        /// </summary>
        public int ChildCount { get; set; }

        /// <summary>
        ///内容项数目
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        ///隐私状态
        /// </summary>
        public PrivacyStatus PrivacyStatus { get; set; }

        /// <summary>
        ///审核状态
        /// </summary>
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        ///特征内容项目Id
        /// </summary>
        public long FeaturedItemId { get; set; }

        /// <summary>
        ///最后更新日期
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.CategoryId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        #region 扩展属性

        /// <summary>
        /// 父类别 
        /// </summary>
        public Category Parent
        {
            get
            {
                if (Depth == 0 || ParentId == 0)
                    return null;
                else
                    return new CategoryService<Category>().Get(ParentId);
            }
        }

        private List<Category> children;
        /// <summary>
        /// 子类别列表
        /// </summary>
        public IEnumerable<Category> Children
        {
            get
            {
                if (ChildCount > 0)
                    return new CategoryService<Category>().GetChildren(CategoryId);
                else
                    return new List<Category>(0);
            }
        }

        private int cumulateItemCount = 0;
        /// <summary>
        /// 累积内容项数量(包含所有后代ItemCount)
        /// </summary>
        public long CumulateItemCount
        {
            get
            {
                if (ChildCount > 0)
                {
                    long totalRecords = 0;
                    new CategoryService<Category>().GetItemIds(CategoryId, true, 20, 1, out totalRecords);
                    return totalRecords;
                }
                else
                {
                    return ItemCount;
                }
            }
        }

        /// <summary>
        /// 移动个数
        /// </summary>
        public int MaxDepth
        {
            get
            {
                int depth = 0;
                CategoryService categoryService = new CategoryService();
                IEnumerable<Category> descendants = categoryService.GetDescendants(CategoryId);
                if (descendants != null && descendants.Count() > 0)
                {
                    depth = categoryService.GetDescendants(CategoryId).Select(n => n.Depth).Max() - categoryService.Get(CategoryId).Depth + 1;
                }
                else
                {
                    depth = 1;
                }
                return depth;
            }
        }
        #endregion

        #region 序列化属性

        [Ignore]
        public string Password
        {
            get { return GetExtendedProperty<string>("Password"); }
            set { SetExtendedProperty("Password", value); }
        }

        #endregion
    }

}
