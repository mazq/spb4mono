//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using Tunynet.Common;
using Tunynet.UI;
using System;
using Spacebuilder.Common;
using System.Web.Routing;
using Tunynet.Mvc;

namespace Spacebuilder.Setup
{
    public class SetupApplication : ApplicationBase
    {
        protected SetupApplication(ApplicationModel model, ApplicationConfig config)
            : base(model, config)
        { }

        /// <summary>
        /// 删除用户在应用中的数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="takeOverUserName">用于接管删除用户时不能删除的内容(例如：用户创建的群组)</param>
        /// <param name="isTakeOver">是否接管被删除用户可被接管的内容</param>
        protected override void DeleteUser(long userId, string takeOverUserName, bool isTakeOver)
        {
        }

        protected override bool Install(string presentAreaKey, long ownerId)
        {
            return true;
        }

        protected override bool UnInstall(string presentAreaKey, long ownerId)
        {
            return true;
        }
    }
}