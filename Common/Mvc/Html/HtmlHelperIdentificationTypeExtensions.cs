//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Spacebuilder.Common;
using Tunynet.Common;

namespace Tunynet.Mvc
{
    public static class HtmlHelperIdentificationTypeExtensions
    {
        /// <summary>
        /// 获取某人通过验证的身份认证标识
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static MvcHtmlString IdentificationType(this HtmlHelper htmlHelper, long userId)
        {
            if (userId<=0)
            {
                throw new ArgumentException("参数名称userid不能为0", "userId");
            }
            //获取某人通过验证的身份认证
            IdentificationService identificationService = new IdentificationService();
            //List<Identification> identifications = identificationService.GetUserIdentifications(userId).Where(n=>n.Status==IdentificationStatus.success).ToList();
            ////获取可用的身份认证标识
            //IEnumerable<IdentificationType> identificationTypes =  identificationService.GetIdentificationTypes(true);
            ////获取这些身份验证的认证标识
            //htmlHelper.ViewData["identificationTypes"] = identificationTypes.Join(identifications, n => n.IdentificationTypeId, u => u.IdentificationTypeId, (n, u) => n).Distinct();

            htmlHelper.ViewData["identificationTypes"] = identificationService.GetIdentificationTypes(userId);

            return htmlHelper.DisplayForModel("IdentificationType");
        }
    }
}
