//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common;
using System;
using Tunynet.Globalization;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// At用户关联项Url获取
    /// </summary>
    public class BlogAtUserAssociatedUrlGetter : IAtUserAssociatedUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().BlogThread(); }
        }

        public AssociatedInfo GetAssociatedInfo(long associateId, string tenantTypeId = "")
        {

            BlogThread thread = new BlogService().Get(associateId);
            if (thread != null && thread.User != null)
            {
                return new AssociatedInfo()
                {
                    DetailUrl = SiteUrls.Instance().BlogDetail(thread.User.UserName, associateId),
                    Subject = thread.Subject
                };
            }

            return null;
        }


        public string GetOwner()
        {
            return BlogConfig.Instance().ApplicationName;
        }
    }
}