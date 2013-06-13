//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 第三方帐号用户资料信息
    /// </summary>
    [Serializable]
    public class ThirdUser
    {
        /// <summary>
        /// 获取新实例
        /// </summary>
        /// <returns></returns>
        public static ThirdUser New()
        {
            return new ThirdUser
            {
                AccountTypeKey = string.Empty,
                Identification = string.Empty,
                UserAvatarUrl = string.Empty,
                NickName = string.Empty
            };
        }
        /// <summary>
        /// 帐号类型Key
        /// </summary>
        public string AccountTypeKey { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        /// 授权加密串
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string UserAvatarUrl { get; set; }

        /// <summary>
        /// 第三方网站空间地址
        /// </summary>
        public string SpaceHomeUrl
        {
            get
            {
                return ThirdAccountGetterFactory.GetThirdAccountGetter(AccountTypeKey).GetSpaceHomeUrl(Identification);
            }
        }
    }
}
