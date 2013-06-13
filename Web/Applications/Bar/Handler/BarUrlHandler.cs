//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet.Utilities;
using Tunynet.Mvc;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 处理特殊的BarUrl
    /// </summary>
    public class BarUrlHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string url = string.Empty;
            long anchorPostId = context.Request.QueryString.Get<long>("anchorPostId");

            BarPostService barPostService = new BarPostService();
            BarPost post = barPostService.Get(anchorPostId);
            if (post == null)
                WebUtility.Return404(context);

            IBarUrlGetter urlGetter = BarUrlGetterFactory.Get(post.TenantTypeId);

            if (post != null)
            {
                int? childPostIndex = 0;
                if (post.ParentId != 0)
                {
                    childPostIndex = barPostService.GetPageIndexForChildrenPost(post.ParentId, post.PostId);
                }

                int pageIndex = barPostService.GetPageIndexForPostInThread(post.ThreadId, anchorPostId);

                url = urlGetter.ThreadDetail(post.ThreadId, pageIndex: pageIndex, anchorPostId: anchorPostId, childPostIndex: childPostIndex);

            }


            context.Response.RedirectPermanent(url);
        }
    }
}
