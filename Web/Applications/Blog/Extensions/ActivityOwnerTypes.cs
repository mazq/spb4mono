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

namespace Spacebuilder.Blog
{
    public static class ActivityOwnerTypesExtension
    {
        /// <summary>
        /// 日志
        /// </summary>
        public static int Blog(this ActivityOwnerTypes ActivityOwnerTypes)
        {
            return 1002;
        }
    }
}