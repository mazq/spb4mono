using Tunynet;
using Tunynet.Common;
using System.Web.Mvc;
using System.Web;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户上下文
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// 获取当前用户
        /// </summary>
        public static IUser CurrentUser
        {
            get
            {
                string token = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {

                    token = HttpContext.Current.Request.Form.Get<string>("CurrentUserIdToken", string.Empty);

                    if (string.IsNullOrEmpty(token))
                        token = HttpContext.Current.Request.QueryString.Get<string>("CurrentUserIdToken", string.Empty);
                }



                if (!string.IsNullOrEmpty(token))
                {

                    token = Tunynet.Utilities.WebUtility.UrlDecode(token);

                    bool isTimeOut = false;
                    long userId = Utility.DecryptTokenForUploadfile(token.ToString(), out isTimeOut);
                    if (userId > 0)
                    {
                        IUserService userService = DIContainer.Resolve<IUserService>();
                        IUser currentUser = userService.GetUser(userId);
                        if (currentUser != null)
                        {
                            return currentUser;
                        }
                    }
                }

                IAuthenticationService authenticationService = DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
                return authenticationService.GetAuthenticatedUser();
            }
        }

        /// <summary>
        /// 获取当前空间域名
        /// </summary>
        public static string CurrentSpaceKey(ControllerContext controllerContext)
        {
            return controllerContext.RequestContext.GetParameterFromRouteDataOrQueryString("SpaceKey");
        }

    }
}