//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web;
using System.Text.RegularExpressions;
using Tunynet.Common;
using Tunynet.Caching;
using Tunynet;
using RestSharp;
using System.Net;
using System;
using System.IO;
namespace Spacebuilder.Common
{
    /// <summary>
    /// 第三方帐号获取器
    /// </summary>
    public abstract class ThirdAccountGetter
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 帐号类型名称
        /// </summary>
        public abstract string AccountTypeName { get; }

        /// <summary>
        /// 帐号类型官方网站地址
        /// </summary>
        public abstract string AccountTypeUrl { get; }

        /// <summary>
        /// 帐号类型Key
        /// </summary>
        public abstract string AccountTypeKey { get; }

        /// <summary>
        /// 获取身份认证Url
        /// </summary>
        /// <returns></returns>
        public abstract string GetAuthorizationUrl();

        /// <summary>
        /// 获取第三方网站用户空间地址Url
        /// </summary>
        /// <returns></returns>
        public abstract string GetSpaceHomeUrl(string identification);


        /// <summary>
        /// 获取当前第三方帐号上的用户标识
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public abstract string GetAccessToken(HttpRequestBase Request);

        /// <summary>
        /// 获取当前第三方帐号上的用户
        /// </summary>
        /// <remarks>使用缓存</remarks>
        /// <param name="accessToken">访问授权</param>
        /// <param name="identification">用户标识</param>
        /// <param name="userCache">是否使用缓存</param>
        /// <returns></returns>
        public ThirdUser GetThirdUser(string accessToken, string identification, bool userCache)
        {
            if (!userCache)
                return GetThirdUser(accessToken, identification);

            string cacheKey = string.Format("ThirdUser::AccountTypeKey-{0}:AccessToken-{1}:Identification-{2}", AccountTypeKey, accessToken, identification ?? string.Empty);
            ThirdUser thirdUser = cacheService.Get<ThirdUser>(cacheKey);
            if (thirdUser == null)
            {
                thirdUser = GetThirdUser(accessToken, identification);
                cacheService.Add(cacheKey, thirdUser, CachingExpirationType.SingleObject);
            }
            return thirdUser;
        }

        /// <summary>
        /// 获取当前第三方帐号上的用户标识
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <returns></returns>
        public abstract ThirdUser GetThirdUser(string accessToken, string identification = null);

        /// <summary>
        /// 发一条纯文本的微博消息
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="content">微博内容</param>
        /// <param name="identification">身份标识</param>
        public abstract bool CreateMicroBlog(string accessToken, string content, string identification = null);

        /// <summary>
        /// 发一条可带图片的微博消息
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="content">微博内容</param>
        /// <param name="imageFullName">图片完整路径名称</param>
        /// <param name="identification">身份标识</param>
        public abstract bool CreatePhotoMicroBlog(string accessToken, string content, byte[] bytes, string fileName, string identification = null);

        /// <summary>
        /// 关注指定帐号
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="identification">身份标识</param>
        public bool FollowOfficialMicroBlog(string accessToken, string identification = null)
        {
            if (AccountType.IsFollowMicroBlog && !string.IsNullOrEmpty(AccountType.OfficialMicroBlogAccount))
            {
                try
                {
                    return Follow(accessToken, AccountType.OfficialMicroBlogAccount, identification);
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 关注指定帐号
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="userName">指定帐号</param>
        /// <param name="identification">身份标识</param>
        public abstract bool Follow(string accessToken, string userName, string identification = null);

        /// <summary>
        /// 回调地址
        /// </summary>
        protected string CallbackUrl
        {
            get
            {
                return SiteUrls.FullUrl(SiteUrls.Instance().ThirdCallBack(AccountTypeKey));
            }
        }

        /// <summary>
        /// 帐号类型
        /// </summary>
        protected AccountType AccountType
        {
            get
            {
                return new AccountBindingService().GetAccountType(AccountTypeKey);
            }
        }


        /// <summary>
        /// 从请求内容中获取AccessToken
        /// </summary>
        /// <remarks>适用于Oauth2.0</remarks>
        /// <param name="content"></param>
        /// <param name="regexPattern">正则表达式</param>
        /// <param name="parmName">参数名称（对应于正则中的分组名）</param>
        /// <returns></returns>
        protected string GetParmFromContent(string content, string regexPattern, string parmName)
        {
            Regex regex = new Regex(regexPattern);
            Match match = regex.Match(content);
            if (!match.Success)
                return string.Empty;
            return match.Groups[parmName].Value;
        }


        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected IRestResponse Execute(RestClient _restClient, RestRequest request)
        {
            var response = _restClient.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ExceptionFacade(response.Content + "\n" + response.ResponseUri);
            }
            return response;
        }

    }
}