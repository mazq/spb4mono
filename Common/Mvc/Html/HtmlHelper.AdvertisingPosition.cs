//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tunynet.Common;
using System.Collections;
using System.Collections.Generic;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 扩展对AdvertisingPosition的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperAdvertisingPositionExtensions
    {
        /// <summary>
        /// 在广告位中显示广告的HtmlHelper方法
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="positionId">广告位Id</param>
        /// <returns></returns>
        public static MvcHtmlString AdvertisingPosition(this HtmlHelper htmlHelper, string positionId)
        {
            AdvertisingService advertisingService = new AdvertisingService();
            AdvertisingPosition position = advertisingService.GetPosition(positionId);
            IEnumerable<Advertising> advertisings = new List<Advertising>();

            htmlHelper.ViewData["position"] = position;

            if (position != null && position.IsEnable)
            {
                advertisings = advertisingService.GetAdvertisingsByPositionId(positionId);
            }
            htmlHelper.ViewData["advertisingsInPosition"] = advertisings;

            return htmlHelper.DisplayForModel("AdvertisingPosition");
        }
    }
}
