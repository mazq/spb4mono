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

namespace Spacebuilder.Bar
{

    /// <summary>
    /// 帖吧动态项
    /// </summary>
    public static class ActivityItemKeysExtension
    {
        /// <summary>
        /// 发布帖子动态项
        /// </summary>
        public static string CreateBarThread(this ActivityItemKeys activityItemKeys)
        {
            return "CreateBarThread";
        }

        /// <summary>
        /// 发布回帖动态项
        /// </summary>
        public static string CreateBarPost(this ActivityItemKeys activityItemKeys)
        {
            return "CreateBarPost";
        }

        /// <summary>
        /// 帖子评分动态项
        /// </summary>
        public static string CreateBarRating(this ActivityItemKeys activityItemKeys)
        {
            return "CreateBarRating";
        }

    }

}
