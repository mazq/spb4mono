//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 请求类型Key类
    /// </summary>
    public class InvitationTypeKeys
    {
        #region Instance
        private static InvitationTypeKeys _instance = new InvitationTypeKeys();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static InvitationTypeKeys Instance()
        {
            return _instance;
        }

        private InvitationTypeKeys()
        { }

        #endregion




    }
}