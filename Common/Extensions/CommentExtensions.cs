//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 评论的扩展类
    /// </summary>
    public static class CommentExtensions
    {
        /// <summary>
        /// 获取评论的用户
        /// </summary>
        /// <returns></returns>
        public static User User(this Comment comement)
        {
            return new UserService().GetFullUser(comement.UserId);
        }
    }
}
