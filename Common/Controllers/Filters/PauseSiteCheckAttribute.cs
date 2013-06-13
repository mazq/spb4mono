using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tunynet.Common;
using Tunynet;
using System.Web;

namespace Spacebuilder.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PauseSiteCheckAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.IsChildAction)
                return;

            IPauseSiteSettingsManager pauseSiteSettingsManager = DIContainer.Resolve<IPauseSiteSettingsManager>();
            PauseSiteSettings pauseSiteSettings = pauseSiteSettingsManager.get();

            HttpContext context = HttpContext.Current;
            if (!pauseSiteSettings.IsEnable)
            {
                var routeDataDictionary = context.Request.RequestContext.RouteData.Values;
                if (!routeDataDictionary.ContainsKey("Controller"))
                    return;
                string controllerName = routeDataDictionary["Controller"].ToString();
                if (!controllerName.ToLower().Contains("controlpanel"))
                {
                    if (filterContext.ActionDescriptor.ActionName == "PausePage")
                    {
                        return;
                    }else{
                        filterContext.Result = new RedirectResult(SiteUrls.Instance().PausePage());
                    }
                    return;
                }
            }
            
        }
    }
}
