//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Collections;
using System.Web.Helpers;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Link的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperTopBottomExtensions
    {
        /// <summary>
        /// 封装顶踩控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="objectId">被顶踩的对象id</param>
        /// <param name="userId">被顶踩的对象的UserId，用于限制不能顶踩自己</param>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="mode">双向操作还是单向操作</param>
        /// <param name="style">顶踩的样式</param>
        /// <param name="onSuccessCallBack">js回调函数</param>
        /// <returns></returns>
        public static MvcHtmlString SupportOppose(this HtmlHelper htmlHelper, string tenantTypeId, long objectId, long userId, AttitudeMode mode, AttitudeStyle? style = null, string onSuccessCallBack = null)
        {
            IUser CurrentUser = UserContext.CurrentUser;
            AttitudeService attitudeService = new AttitudeService(tenantTypeId);

            //查询当前登录用户是否顶踩过该对象
            bool? isSupport = null;
            if (CurrentUser != null)
            {
                isSupport = attitudeService.IsSupport(objectId, CurrentUser.UserId);
            }
            htmlHelper.ViewData["isSupport"] = isSupport;

            //查询该对象总的顶踩次数
            Attitude attitude = attitudeService.Get(objectId);
            htmlHelper.ViewData["attitude"] = attitude;

            //向View传递用户设置参数
            htmlHelper.ViewData["tenantTypeId"] = tenantTypeId;
            htmlHelper.ViewData["objectId"] = objectId;
            htmlHelper.ViewData["userId"] = userId;
            htmlHelper.ViewData["mode"] = mode;
            htmlHelper.ViewData["style"] = style;
            htmlHelper.ViewData["onSuccessCallBack"] = onSuccessCallBack;

            //顶踩的全局设置
            htmlHelper.ViewData["attitudeSettings"] = DIContainer.Resolve<IAttitudeSettingsManager>().Get();
            ;

            return htmlHelper.DisplayForModel("SupportOppose");
        }
    }
}
