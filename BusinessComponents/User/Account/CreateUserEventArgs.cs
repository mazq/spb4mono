//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 创建用户事件参数
    /// </summary>
    public class CreateUserEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="password">用户密码</param>
        public CreateUserEventArgs(string password)
            : base(string.Empty)
        {
            this.password = password;
        }


        private string password;
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password
        {
            get { return password; }
        }

    }
}
