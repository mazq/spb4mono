//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections.Concurrent;
using Tunynet.Common;
using Fasterflect;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 第三方帐号获取器
    /// </summary>
    public static class ThirdAccountGetterFactory
    {

        private static readonly object lockObject = new object();
        private static bool isInitialized;
        private static ConcurrentDictionary<string, ThirdAccountGetter> thirdAccountGetters = null;

        /// <summary>
        /// 初始化第三方帐号获取器
        /// </summary>
        public static void InitializeAll()
        {
            if (!isInitialized)
            {
                lock (lockObject)
                {
                    if (!isInitialized)
                    {
                        thirdAccountGetters = new ConcurrentDictionary<string, ThirdAccountGetter>();
                        foreach (var accountType in new AccountBindingService().GetAccountTypes())
                        {
                            Type thirdAccountGetterClassType = Type.GetType(accountType.ThirdAccountGetterClassType);
                            if (thirdAccountGetterClassType != null)
                            {
                                ConstructorInvoker thirdAccountGetterConstructor = thirdAccountGetterClassType.DelegateForCreateInstance();
                                ThirdAccountGetter thirdAccountGetter = thirdAccountGetterConstructor() as ThirdAccountGetter;
                                if (thirdAccountGetter != null)
                                    thirdAccountGetters[accountType.AccountTypeKey] = thirdAccountGetter;
                            }
                        }
                        isInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// 获取某一个ThirdAccountGetter
        /// </summary>
        /// <param name="accountTypeKey">accountTypeKey</param>
        /// <returns>返回ThirdAccountGetter</returns>
        public static ThirdAccountGetter GetThirdAccountGetter(string accountTypeKey)
        {
            if (thirdAccountGetters != null && thirdAccountGetters.ContainsKey(accountTypeKey))
                return thirdAccountGetters[accountTypeKey];
            return null;
        }

    }
}