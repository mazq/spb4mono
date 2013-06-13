//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 替换创建Controller工厂类
    /// </summary>
    public class TunynetControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// 创建Controller
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController iController = base.CreateController(requestContext, controllerName);
            Controller controller = iController as Controller;
            if (iController != null)
                controller.TempDataProvider = new CookieTempDataProvider(requestContext.HttpContext);
            return base.CreateController(requestContext, controllerName);
        }
    }
}
