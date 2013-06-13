//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 帐号类型扩展类
    /// </summary>
    public static class AccountTypeExtension
    {
        /// <summary>
        /// 帐号类型名称
        /// </summary>
        public static string AccountTypeName(this AccountType accountType)
        {
            ThirdAccountGetter thirdAccountGetter = ThirdAccountGetterFactory.GetThirdAccountGetter(accountType.AccountTypeKey);
            if (thirdAccountGetter == null)
                return string.Empty;
            return thirdAccountGetter.AccountTypeName;
        }
    }
}