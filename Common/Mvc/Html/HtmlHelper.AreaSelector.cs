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
    /// <summary>
    /// 扩展HtmlHelper
    /// </summary>
    public static class HtmlHelperAreaSelectorExtensions
    {
        
        //1.topAreaCode应该取DIContainer.Resolve<IAreaSettingsManager>().Get().RootAreaCode，不应该公开出来；
        //2.parentAreaCode参数没必要加；
        //3.callback、selectTop没加注释，不知道什么意思；
        /// <summary>
        /// 输出地区选择器
        /// </summary> 
        public static MvcHtmlString AreaSelector(this HtmlHelper htmlHelper, string id, string topAreaCode = "", string parentAreaCode = "", string currentAreaCode = "", int depth = 4, string callback = null, bool selectTop = true)
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("参数名称id不能为空", "id");
            }
            htmlHelper.ViewData["id"] = id;
            htmlHelper.ViewData["topAreaCode"] = topAreaCode;
            htmlHelper.ViewData["parentAreaCode"] = parentAreaCode;
            htmlHelper.ViewData["currentAreaCode"] = currentAreaCode;
            htmlHelper.ViewData["depth"] = depth;
            htmlHelper.ViewData["callback"] = callback;
            htmlHelper.ViewData["selectTop"] = selectTop;

            htmlHelper.ViewData["login"] = "false";
            htmlHelper.ViewData["registerAreaCode"] = "AreaCode";
            htmlHelper.ViewData["registerAreaName"] = "AreaName";
            htmlHelper.ViewData["registerParentAreaCode"] = "ParentAreaCode";
            htmlHelper.ViewData["registerParentAreaName"] = "ParentAreaName";

            // 获取当前用户的注册地
            AreaService areaService = DIContainer.Resolve<AreaService>();
            UserProfileService userProfileService = new UserProfileService();
            IUser currentUser = UserContext.CurrentUser; //获取当前登录用户

            if (currentUser == null)
            {
                return htmlHelper.DisplayForModel("AreaSelector");
            }
            UserProfile userProfile = userProfileService.Get(currentUser.UserId);
            if (userProfile == null || string.IsNullOrEmpty(userProfile.NowAreaCode))
            {
                return htmlHelper.DisplayForModel("AreaSelector");
            }

            Area area = areaService.Get(userProfile.NowAreaCode);
            htmlHelper.ViewData["login"] = "true";
            htmlHelper.ViewData["registerAreaCode"] = userProfile.NowAreaCode;
            htmlHelper.ViewData["registerAreaName"] = area.Name;
            htmlHelper.ViewData["registerParentAreaCode"] = area.ParentCode;
            string parentAreaName = areaService.Get(area.ParentCode).Name;
            htmlHelper.ViewData["registerParentAreaName"] = parentAreaName;


            return htmlHelper.DisplayForModel("AreaSelector");
        }

        /// <summary>
        /// 根据地区编码获取地区名称
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public static MvcHtmlString ShowAreaName(this HtmlHelper htmlHelper, string areaCode)
        {
            if (string.IsNullOrWhiteSpace(areaCode))
            {
                return MvcHtmlString.Empty;
            }
            AreaService areaService = DIContainer.Resolve<AreaService>();
            Area area = areaService.Get(areaCode);
            if (area == null)
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create(area.Name);
        }

        /// <summary>
        /// 根据地区编码获取上一级地区名称
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public static MvcHtmlString ShowParentAreaName(this HtmlHelper htmlHelper, string areaCode)
        {
            if (string.IsNullOrWhiteSpace(areaCode))
            {
                return MvcHtmlString.Empty;
            }
            AreaService areaService = DIContainer.Resolve<AreaService>();
            Area area = areaService.Get(areaCode);
            if (area == null || string.IsNullOrEmpty(area.ParentCode))
            {
                return MvcHtmlString.Empty;
            }

            Area parentArea = areaService.Get(area.ParentCode);
            if (parentArea == null)
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create(parentArea.Name);
        }
    }
}
