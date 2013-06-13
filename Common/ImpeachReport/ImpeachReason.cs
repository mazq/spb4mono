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

namespace Spacebuilder.Common
{

    ///<summary>
    ///举报原因
    /// </summary>
    public enum ImpeachReason
    {

        /// <summary>
        ///内容反动
        ///</summary>
        [Display(Name = "内容反动")]
        Reactionary = 1,

        /// <summary>
        /// 色情内容
        /// </summary>
        [Display(Name = "色情内容")]
        Sexy = 2,

       /// <summary>
       /// 内容侵权
       /// </summary>
        [Display(Name = "内容侵权")]
        Tortious = 3,

        /// <summary>
        ///垃圾广告
        ///</summary>
        [Display(Name = "垃圾广告")]
        Spam = 4,

        /// <summary>
        ///其他
        ///</summary>
        [Display(Name = "其他")]
        Other = 99
    }

}
