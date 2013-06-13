//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Tunynet.Utilities;
using System.Web.Helpers;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using RestSharp;
using System.IO;

namespace Spacebuilder.Common
{
    /// <summary>
    /// QQ帐号获取器
    /// </summary>
    public class QQAccountGetter : ThirdAccountGetter
    {

        private RestClient _restClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QQAccountGetter()
        {
            _restClient = new RestClient("https://graph.qq.com");
        }

        /// <summary>
        /// 名称
        /// </summary>
        public override string AccountTypeName
        {
            get { return "QQ帐号"; }
        }

        /// <summary>
        /// 官方网站地址
        /// </summary>
        public override string AccountTypeUrl
        {
            get { return "http://i.qq.com/"; }
        }

        /// <summary>
        /// 帐号类型Key
        /// </summary>
        public override string AccountTypeKey
        {
            get { return AccountTypeKeys.Instance().QQ(); }
        }

        /// <summary>
        /// 获取第三方网站空间主页地址
        /// </summary>
        /// <param name="identification"></param>
        /// <returns></returns>
        public override string GetSpaceHomeUrl(string identification)
        {
            return string.Format("http://user.qzone.qq.com/{0}", identification);
        }

        /// <summary>
        /// 获取身份认证Url
        /// </summary>
        /// <returns></returns>
        public override string GetAuthorizationUrl()
        {
            string getAuthorizationCodeUrlPattern = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&scope=add_idol,add_t,add_pic_t";
            return string.Format(getAuthorizationCodeUrlPattern, AccountType.AppKey, WebUtility.UrlEncode(CallbackUrl));
        }

        /// <summary>
        /// 获取当前第三方帐号上的访问授权
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public override string GetAccessToken(HttpRequestBase Request)
        {
            //Step1：通过Authorization Code获取Access Token
            _restClient.Authenticator = null;
            string code = Request.QueryString.GetString("code", string.Empty);
            var request = new RestRequest(Method.GET);
            request.Resource = "oauth2.0/token?grant_type=authorization_code&client_id={appkey}&client_secret={appsecret}&code={code}&redirect_uri={callbackurl}";
            request.AddParameter("appkey", AccountType.AppKey, ParameterType.UrlSegment);
            request.AddParameter("appsecret", AccountType.AppSecret, ParameterType.UrlSegment);
            request.AddParameter("code", code, ParameterType.UrlSegment);
            request.AddParameter("callbackurl", CallbackUrl, ParameterType.UrlSegment);
            var response = Execute(_restClient, request);
            string access_token = GetParmFromContent(response.Content, @"access_token=(?<accessToken>[^&]+)&expires_in", "accessToken");
            return access_token;
        }

        /// <summary>
        /// 获取当前第三方帐号上的用户
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="identification">标识</param>
        /// <returns></returns>
        public override ThirdUser GetThirdUser(string accessToken, string identification = null)
        {
            //Step1：通根据access_token获得对应用户身份的openid
            _restClient.Authenticator = null;
            var request = new RestRequest(Method.GET);
            if (string.IsNullOrEmpty(identification))
            {
                request.Resource = "oauth2.0/me?access_token={accesstoken}";
                request.AddParameter("accesstoken", accessToken, ParameterType.UrlSegment);
                var response = Execute(_restClient, request);
                identification = GetParmFromContent(response.Content, @"""openid"":""(?<openId>[^""]+)""", "openId");
                if (string.IsNullOrEmpty(identification))
                    return null;
            }

            //Step2：调用OpenAPI，获取用户信息
            _restClient.Authenticator = new QQOAuthAuthenticator(identification, accessToken, AccountType.AppKey);
            request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Content-Type", "application/json");
            request.Resource = "user/get_user_info";
            var getUserInfoResponse = Execute(_restClient, request);
            var qqUser = Json.Decode(getUserInfoResponse.Content);
            return new ThirdUser
            {
                AccountTypeKey = AccountType.AccountTypeKey,
                Identification = identification,
                AccessToken = accessToken,
                NickName = qqUser.nickname,
                Gender = qqUser.gender == "男" ? GenderType.Male : GenderType.FeMale,
                UserAvatarUrl = qqUser.figureurl_2
            };
        }

        /// <summary>
        /// 发一条纯文本的微博消息
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="content">微博内容</param>
        /// <param name="identification">身份标识</param>
        public override bool CreateMicroBlog(string accessToken, string content, string identification = null)
        {
            _restClient.Authenticator = new QQOAuthAuthenticator(identification, accessToken, AccountType.AppKey);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Resource = "t/add_t";
            request.AddParameter("content", content);
            var response = Execute(_restClient, request);
            var payload = Json.Decode(response.Content);
            return payload.errcode == 0;
        }

        /// <summary>
        /// 发一条可带图片的微博消息
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="content">微博内容</param>
        /// <param name="bytes">图片流</param>
        /// <param name="identification">身份标识</param>
        public override bool CreatePhotoMicroBlog(string accessToken, string content, byte[] bytes, string fileName, string identification = null)
        {
            if (bytes == null)
                return CreateMicroBlog(accessToken, content, identification);

            _restClient.Authenticator = new QQOAuthAuthenticator(identification, accessToken, AccountType.AppKey);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            var boundary = string.Concat("--", Utility.GenerateRndNonce());
            request.AddHeader("Content-Type", string.Concat("multipart/form-data; boundary=", boundary));
            request.Resource = "t/add_pic_t";
            request.AddParameter("content", content);
            request.AddFile("pic", bytes, fileName);
            var response = Execute(_restClient, request);
            var payload = Json.Decode(response.Content);
            return payload.errcode == 0;
        }

        /// <summary>
        /// 关注指定帐号
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="userName">指定帐号</param>
        /// <param name="identification">身份标识</param>
        public override bool Follow(string accessToken, string userName, string identification = null)
        {
            _restClient.Authenticator = new QQOAuthAuthenticator(identification, accessToken, AccountType.AppKey);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Resource = "relation/add_idol";
            if (!string.IsNullOrEmpty(userName))
            {
                request.AddParameter("name", userName);
            }
            var response = Execute(_restClient, request);
            var payload = Json.Decode(response.Content);
            return payload.errcode == 0;
        }
    }
}