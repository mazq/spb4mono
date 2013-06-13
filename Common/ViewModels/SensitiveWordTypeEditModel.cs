//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 敏感词类型编辑模型
    /// </summary>
    public class SensitiveWordTypeEditModel
    {
        /// <summary>
        /// TypeId
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// 敏感词类型名
        /// </summary>
        [Display(Name = "类别名称")]
        [Required(ErrorMessage = "请输入类别名称")]
        public string Name { get; set; }

        /// <summary>
        /// 转化为SensitiveWordType用于数据库存储
        /// </summary>
        /// <returns>SensitiveWord</returns>
        public SensitiveWordType AsSensitiveWordType()
        {
            
            SensitiveWordType sensitiveWordType = null;

            if (this.TypeId > 0)
                sensitiveWordType = new SensitiveWordService().GetSensitiveWordType(this.TypeId.Value);
            else
                sensitiveWordType = SensitiveWordType.New();

            sensitiveWordType.Name = this.Name;

            return sensitiveWordType;
        }
    }

    /// <summary>
    /// 敏感词类别的扩展方法
    /// </summary>
    public static class SensitiveWordTypeExtensions
    {
        /// <summary>
        /// 转换成AsEditModel
        /// </summary>
        public static SensitiveWordTypeEditModel AsEditModel(this SensitiveWordType sensitiveWord)
        {
            return new SensitiveWordTypeEditModel
            {
                Name = sensitiveWord.Name,

                TypeId = sensitiveWord.TypeId
            };
        }
    }
}