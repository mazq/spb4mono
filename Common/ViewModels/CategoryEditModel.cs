//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;
using Tunynet;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.Utilities;
namespace Spacebuilder.Common
{
    /// <summary>
    /// 分类编辑实体
    /// </summary>
    public class CategoryEditModel
    {

        // 摘要:
        //     类别Id
        public long CategoryId { get; set; }

        // 摘要:
        //     类别名称
        [Display(Name = "分类名称")]
        [Required(ErrorMessage = "请输入分类名称")]
        [StringLength(64, ErrorMessage = "最大长度允许64个字符")]
        [WaterMark(Content = "请输入分类名称")]
        [DataType(DataType.Text)]
        public string CategoryName { get; set; }

        // 摘要:
        //     类别描述
        [Display(Name = "类别描述")]
        [StringLength(120, ErrorMessage = "最大长度允许120个字符")]
        [WaterMark(Content = "请输入类别描述")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        // 摘要:
        //     拥有者Id
        public long OwnerId { get; set; }

        // 摘要:
        //     父评论Id（顶级ParentId=0）
        public long ParentId { get; set; }

        public string ParentName { get; set; }

        // 摘要:
        //     租户类型Id
        public string TenantTypeId { get; set; }

        // 摘要:
        //     最后更新日期

        public DateTime LastModified { get; set; }

        // 摘要:
        //     类别深度 顶级类别 Depth=0
        public int Depth { get; set; }

        /// <summary>
        /// 转换为Category用于数据库存储
        /// </summary>
        public Category AsCategory()
        {
            Category category = null;

            if (CategoryId == 0)
            {
                category = Category.New();
            }
            else
            {
                CategoryService categoryService = new CategoryService();
                category = categoryService.Get(CategoryId);

            }

            category.Depth = Depth;
            category.CategoryName = CategoryName;
            category.Description = Formatter.FormatMultiLinePlainTextForStorage(Description, true) ?? string.Empty;
            category.ParentId = ParentId;
            category.OwnerId = OwnerId;
            category.TenantTypeId = TenantTypeId;
            category.LastModified = DateTime.UtcNow;

            return category;
        }
    }

    /// <summary>
    /// 分类的扩展方法
    /// </summary>
    public static class CategoryEditModelExtensions
    {
        /// <summary>
        /// 数据库中的对象转换为EditModel
        /// </summary>
        /// <returns></returns>
        public static CategoryEditModel AsCategoryEditModel(this Category category)
        {
            return new CategoryEditModel
            {
                Depth = category.Depth,
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = Formatter.FormatMultiLinePlainTextForEdit(category.Description, true),
                ParentId = category.ParentId,
                TenantTypeId = category.TenantTypeId
            };
        }
    }
}
