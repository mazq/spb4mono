//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Logging;
using Tunynet.Utilities;
using Tunynet.Common;
using System.Web;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 当前操作者信息获取器
    /// </summary>
    public class OperatorInfoGetter : IOperatorInfoGetter
    {
        /// <summary>
        /// 获取当前操作者信息
        /// </summary>
        /// <returns></returns>
        public OperatorInfo GetOperatorInfo()
        {
            OperatorInfo operatorInfo = new OperatorInfo();
            if (HttpContext.Current == null)
                return operatorInfo;
            IUser currentUser = UserContext.CurrentUser;
            operatorInfo.OperatorIP = WebUtility.GetIP();
            operatorInfo.AccessUrl = HttpContext.Current.Request.RawUrl;
            operatorInfo.OperatorUserId = currentUser != null ? currentUser.UserId : 0;
            operatorInfo.Operator = currentUser != null ? currentUser.DisplayName : string.Empty;
            return operatorInfo;
        }
    }
}