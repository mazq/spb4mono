//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Mvc;
using System.Web.Routing;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 拥有者计数类型扩展类
    /// </summary>
    public static class UserExtensionByGroup
    {
        /// <summary>
        /// 创建的群组数
        /// </summary>
        public static long CreatedGroupCount(this User user)
        {
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            return ownerDataService.GetLong(user.UserId, OwnerDataKeys.Instance().CreatedGroupCount());
        }
        /// <summary>
        /// 加入的群组数
        /// </summary>
        public static long JoinedGroupCount(this User user)
        {
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            return ownerDataService.GetLong(user.UserId, OwnerDataKeys.Instance().JoinedGroupCount());
        }
    }
}