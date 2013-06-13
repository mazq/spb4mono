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

namespace Tunynet.Common
{
    /// <summary>
    /// 学校类型
    /// </summary>
    public enum SchoolType
    {
        /// <summary>
        /// 大学
        /// </summary>
        [Display(Name="大学")]
        University = 0,
        /// <summary>
        /// 高中
        /// </summary>
        [Display(Name = "高中")]
        SeniorHighSchool = 1,
        /// <summary>
        /// 初中
        /// </summary>
        [Display(Name = "初中")]
        JuniorHighSchool = 2,
        /// <summary>
        /// 小学
        /// </summary>
        [Display(Name = "小学")]
        GradeSchool = 3
    }
}
