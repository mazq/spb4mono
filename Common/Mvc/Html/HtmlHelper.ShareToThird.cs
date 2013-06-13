using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tunynet.Common;
using System.Web.Mvc.Html;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 分享到其他网站
    /// </summary>
    public static class HtmlHelperShareToThirdExtensions
    {
        /// <summary>
        /// 分享到其他网站
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString ShareToThird(this HtmlHelper htmlHelper)
        {
            SiteSettings siteSettings = DIContainer.Resolve<ISiteSettingsManager>().Get();
            if (!siteSettings.ShareToThirdIsEnabled)
            {
                return MvcHtmlString.Empty;
            }
            //展示形式
            htmlHelper.ViewData["ShareToThirdDisplayType"] = siteSettings.ShareToThirdDisplayType;
            //图标形式展示大小
            htmlHelper.ViewData["ShareDisplayIconSize"] = siteSettings.ShareDisplayIconSize;
            return htmlHelper.Partial("~/Plugins/ShareToThird/" + siteSettings.ShareToThirdBusiness + ".cshtml");
        }
    }


}