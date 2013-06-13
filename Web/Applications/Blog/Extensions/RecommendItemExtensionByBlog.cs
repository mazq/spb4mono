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

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 推荐项类型扩展类
    /// </summary>
    public static class RecommendItemExtensionByBlog
    {
        /// <summary>
        /// 创建的群组数
        /// </summary>
        public static BlogThread GetBlogThread(this RecommendItem item)
        {
            return new BlogService().Get(item.ItemId);
        }
    }
}