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
    /// 用户手动输入邮箱邀请好友
    /// </summary>
    public class InviteFriendByUserInputEditModel
    {
        /// <summary>
        /// 用户数输入的链接。通过,号分隔
        /// </summary>
        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "至少输入一个邮箱")]
        public string emails { get; set; }
    }
}
