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
    /// <summary>
    /// 购买邀请码配合实体
    /// </summary>
    public class BuyInviteCountEditModel
    {
        /// <summary>
        /// 购买邀请码的个数
        /// </summary>
        [Display(Name = "购买个数")]
        [RegularExpression(@"\d+", ErrorMessage = "请输入正整数")]
        [Range(1, 100, ErrorMessage = "购买个数必须在1-100之间")]
        [Required(ErrorMessage = "您还没有输入购买个数")]
        public int? invitationCodeCount { get; set; }
    }
}
