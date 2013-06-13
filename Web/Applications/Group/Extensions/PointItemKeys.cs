//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 积分项
    /// </summary>
    public static class PointItemKeysExtension
    {
        /// <summary>
        /// 创建群组
        /// </summary>
        public static string Group_CreateGroup(this PointItemKeys pointItemKeys)
        {
            return "Group_CreateGroup";
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        public static string Group_DeleteGroup(this PointItemKeys pointItemKeys)
        {
            return "Group_DeleteGroup";
        }
       
    }
}
