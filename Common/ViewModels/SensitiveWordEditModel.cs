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
    /// 敏感词编辑模型
    /// </summary>
    public class SensitiveWordEditModel
    {
        /// <summary>
        ///Id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 敏感词
        /// </summary>
        [Display(Name = "敏感词")]
        [Required(ErrorMessage = "请输入敏感词")]
        [NoFilterWord]
        public string Word { get; set; }

        /// <summary>
        /// 敏感词类型Id
        /// </summary>
        [Display(Name = "类别")]
        public int? TypeId { get; set; }

        /// <summary>
        /// 替换后的字符
        /// </summary>
        [Display(Name = "级别")]
        [Required(ErrorMessage = "请输入要替换的内容")]
        public string Replacement { get; set; }

        /// <summary>
        /// 转化为SensitiveWord用于数据库存储
        /// </summary>
        /// <returns>SensitiveWord</returns>
        public SensitiveWord AsSensitiveWord()
        {
            SensitiveWord sensitiveWord = null;

            if (this.Id > 0)
                sensitiveWord = new SensitiveWordService().Get(this.Id.Value);
            else
                sensitiveWord = SensitiveWord.New();


            sensitiveWord.Word = this.Word;
            if (this.TypeId != null)
            {
                sensitiveWord.TypeId = this.TypeId.Value;
            }
            sensitiveWord.Replacement = this.Replacement;

            return sensitiveWord;
        }
    }

    /// <summary>
    /// 敏感词的扩展方法
    /// </summary>
    public static class SensitiveWordExtensions
    {
        /// <summary>
        /// 转换成AsEditModel
        /// </summary>
        public static SensitiveWordEditModel AsEditModel(this SensitiveWord sensitiveWord)
        {
            return new SensitiveWordEditModel
            {
                Word = sensitiveWord.Word,
                TypeId = sensitiveWord.TypeId,
                Replacement = sensitiveWord.Replacement,
                Id = sensitiveWord.Id
            };
        }
    }
}
