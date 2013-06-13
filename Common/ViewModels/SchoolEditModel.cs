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
    /// 创建学校的类
    /// </summary>
    public class SchoolEditModel
    {
        /// <summary>
        /// 所在地区编号
        /// </summary>
        [Display(Name = "所在地区")]
        [RegularExpression(@"[A-Za-z0-9]+", ErrorMessage = "只能输入字母或数字")]
        [Required(ErrorMessage = "请选择所在地区")]
        [StringLength(8, ErrorMessage = "不能超过8个字符")]
        public string AreaCode { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        [Display(Name = "学校名称")]
        [StringLength(18, ErrorMessage = "不能超过18个字符")]
        [Required(ErrorMessage = "请输入学校名称")]
        public string SchoolName { get; set; }


        /// <summary>
        /// 学校类型
        /// </summary>
        [Display(Name = "学校类型")]
        public SchoolType SchoolType { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 转化为school用于数据库存储
        /// </summary>
        /// <returns>school</returns>
        public School AsSchool()
        {
            School school = null;

            if (this.Id.HasValue && this.Id > 0)
                school = new SchoolService().Get(this.Id.Value);
            else
                school = School.New();


            school.Name = this.SchoolName;
            school.AreaCode = this.AreaCode;
            school.SchoolType = this.SchoolType;

            return school;
        }

    }
    /// <summary>
    /// 学校的扩展方法
    /// </summary>
    public static class SchoolExtensions
    {
        /// <summary>
        /// 转换成AsEditModel
        /// </summary>
        public static SchoolEditModel AsEditModel(this School school)
        {
            return new SchoolEditModel
            {
                AreaCode = school.AreaCode,
                SchoolName = school.Name,
                SchoolType = school.SchoolType,
                Id = school.Id
            };
        }
    }
}
