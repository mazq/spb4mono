//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Security;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 验证码可用字符集枚举
    /// </summary>
    [Flags]
    public enum CaptchaCharacterSet
    {
        /// <summary>
        /// 小写字母
        /// </summary>
        LowercaseLetters = 1,
        /// <summary>
        /// 大写字母
        /// </summary>
        UppercaseLetters = 2,
        /// <summary>
        ///大小写混合
        /// </summary>
        Letters = LowercaseLetters | UppercaseLetters,
        /// <summary>
        /// 数字0-9
        /// </summary>
        Digits = 4,
        /// <summary>
        /// 数字字母混合 
        /// </summary>
        LettersAndDigits = Letters | Digits
    }
}