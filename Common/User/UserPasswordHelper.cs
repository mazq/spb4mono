//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户密码辅助工具类
    /// </summary>
    public class UserPasswordHelper
    {
        /// <summary>
        /// 检查用户密码是否正确
        /// </summary>
        /// <param name="password">用户录入的用户密码（尚未加密的密码）</param>
        /// <param name="storedPassword">数据库存储的密码（即加密过的密码）</param>
        /// <param name="passwordFormat">用户密码存储格式</param>
        public static bool CheckPassword(string password, string storedPassword, UserPasswordFormat passwordFormat)
        {
            string encodedPassword = EncodePassword(password, passwordFormat);

            if (encodedPassword != null)
                return encodedPassword.Equals(storedPassword, StringComparison.CurrentCultureIgnoreCase);
            else
                return false;
        }

        /// <summary>
        /// 对用户密码进行编码
        /// </summary>
        /// <param name="password">需要加密的用户密码</param>
        /// <param name="passwordFormat">用户密码存储格式</param>
        public static string EncodePassword(string password, UserPasswordFormat passwordFormat)
        {
            if (passwordFormat == UserPasswordFormat.MD5)
                return Tunynet.Utilities.EncryptionUtility.MD5(password);
            return password;
        }
    }
}
