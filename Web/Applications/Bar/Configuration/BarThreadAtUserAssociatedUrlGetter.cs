//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common;
using System;
using Tunynet.Globalization;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// At用户关联项Url获取
    /// </summary>
    public class BarThreadAtUserAssociatedUrlGetter : IAtUserAssociatedUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().BarThread(); }
        }

        public AssociatedInfo GetAssociatedInfo(long associateId, string tenantTypeId = "")
        {
            BarThreadService barService = new BarThreadService();
            BarThread barThread = new BarThreadService().Get(associateId);

            if (barThread != null)
            {
                IBarUrlGetter urlGetter = BarUrlGetterFactory.Get(barThread.TenantTypeId);

                return new AssociatedInfo()
                {
                    DetailUrl = urlGetter.ThreadDetail(barThread.ThreadId),
                    Subject = barThread.Subject
                };
            }

            return null;
        }

        public string GetOwner()
        {
            return "帖子";
        }
    }
}