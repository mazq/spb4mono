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
    /// 通过导入CSV文件的方式邀请好友的方式
    /// </summary>
    public class InviteFriendByCsvEditModel
    {
        /// <summary>
        /// 上传的文件名
        /// </summary>
        [Required(ErrorMessage = "请选择文件")]
        public string fileName { get; set; }
    }
}
