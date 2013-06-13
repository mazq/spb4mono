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

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 帖吧拥有者类型
    /// </summary>
    public static class ActivityOwnerTypesExtension
    {
        /// <summary>
        /// 帖吧
        /// </summary>
        public static int BarSection(this ActivityOwnerTypes ActivityOwnerTypes)
        {
            return 12;
        }
    }

}
