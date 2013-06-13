//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using System.Text;
using Tunynet.Utilities;
using System.Web.Helpers;
using System.Web;
using RestSharp;


namespace Spacebuilder.Common
{
    /// <summary>
    /// 新浪微博帐号获取器
    /// </summary>
    public class SinaWeiboAccountGetter : ThirdAccountGetter
    {
        private RestClient _restClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SinaWeiboAccountGetter()
        {
            _restClient = new RestClient("https://api.weibo.com");
        }

        /// <summary>
        /// 名称
        /// </summary>
        public override string AccountTypeName
        {
            get { return "新浪微博"; }
        }

        /// <summary>
        /// 官方网站地址
        /// </summary>
        public override string AccountTypeUrl
        {
            get { return "http://www.weibo.com/"; }
        }


        /// <summary>
        /// 帐号类型Key
        /// </summary>
        public override string AccountTypeKey
        {
            get { return AccountTypeKeys.Instance().SinaWeibo(); }
        }

        /// <summary>
        /// 获取第三方网站空间主页地址
        /// </summary>
        /// <param name="identification"></param>
        /// <returns></returns>
        public override string GetSpaceHomeUrl(string identification)
        {
            return string.Format("http://weibo.com/u/{0}", identification);
        }


        /// <summary>
        /// 获取身份认证Url
        /// </summary>
        /// <returns></returns>
        public override string GetAuthorizationUrl()
        {
            string getAuthorizationCodeUrlPattern = "https://api.weibo.com/oauth2/authorize?response_type=code&client_id={0}&redirect_uri={1}";
            return string.Format(getAuthorizationCodeUrlPattern, AccountType.AppKey, WebUtility.UrlEncode(CallbackUrl));
        }

        /// <summary>
        /// 获取当前第三方帐号上的访问授权
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public override string GetAccessToken(HttpRequestBase Request)
        {
            //通过Authorization Code获取Access Token
            _restClient.Authenticator = null;
            string code = Request.QueryString.GetString("code", string.Empty);
            var request = new RestRequest(Method.POST);
            request.Resource = "oauth2/access_token?grant_type=authorization_code&client_id={appkey}&client_secret={appsecret}&code={code}&redirect_uri={callbackurl}";
            request.AddParameter("appkey", AccountType.AppKey, ParameterType.UrlSegment);
            request.AddParameter("appsecret", AccountType.AppSecret, ParameterType.UrlSegment);
            request.AddParameter("code", code, ParameterType.UrlSegment);
            request.AddParameter("callbackurl", CallbackUrl, ParameterType.UrlSegment);
            var response = Execute(_restClient, request);
            string access_token = GetParmFromContent(response.Content, @"""access_token"":""(?<accessToken>[^""]+)""", "accessToken");
            return access_token;
        }

        /// <summary>
        /// 获取当前第三方帐号上的用户
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="identification">身份标识</param>
        /// <returns></returns>
        public override ThirdUser GetThirdUser(string accessToken, string identification)
        {
            //Step1：通根据access_token获得对应用户身份的openid
            _restClient.Authenticator = new CommonOAuthAuthenticator(accessToken);
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            if (string.IsNullOrEmpty(identification))
            {
                request.Resource = "2/account/get_uid.json";
                var response = Execute(_restClient, request);
                identification = GetParmFromContent(response.Content, @"""uid"":(?<uid>[^}]+)", "uid");
                if (string.IsNullOrEmpty(identification))
                    return null;
            }
            //Step2：调用OpenAPI，获取用户信息
            request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Content-Type", "application/json");
            request.Resource = "2/users/show.json";
            request.AddParameter("uid", identification);
            var getUserInfoResponse = Execute(_restClient, request);
            var weiboUser = Json.Decode(getUserInfoResponse.Content);
            GenderType gender = GenderType.NotSet;
            if (weiboUser.gender == "m")
            {
                gender = GenderType.Male;
            }
            else if (weiboUser.gender == "f")
            {
                gender = GenderType.FeMale;
            }
            else
                gender = GenderType.NotSet;
            return new ThirdUser
            {
                AccountTypeKey = AccountType.AccountTypeKey,
                Identification = identification,
                AccessToken = accessToken,
                NickName = weiboUser.screen_name,
                Gender = gender,
                UserAvatarUrl = weiboUser.avatar_large
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
            _restClient.Authenticator = new CommonOAuthAuthenticator(accessToken);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.Resource = "2/statuses/update.json";
            request.AddParameter("status", content);
            var response = Execute(_restClient, request);
            var payload = Json.Decode(response.Content);
            return payload.id > 0;
        }

        /// <summary>
        /// 发一条可带图片的微博消息
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="content">微博内容</param>
        /// <param name="bytes">图片流</param>
        /// <param name="fileName">图片名</param>
        /// <param name="identification">身份标识</param>
        public override bool CreatePhotoMicroBlog(string accessToken, string content, byte[] bytes, string fileName, string identification = null)
        {
            if (bytes == null)
                return CreateMicroBlog(accessToken, content, identification);

            _restClient.Authenticator = new CommonOAuthAuthenticator(accessToken);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            var boundary = string.Concat("--", Utility.GenerateRndNonce());
            request.AddHeader("Content-Type", string.Concat("multipart/form-data; boundary=", boundary));
            request.Resource = "2/statuses/upload.json";
            request.AddFile("pic", bytes, fileName);
            request.AddParameter("status", content);
            var response = Execute(_restClient, request);
            var payload = Json.Decode(response.Content);
            return payload.id > 0;
        }

        /// <summary>
        /// 关注指定帐号
        /// </summary>
        /// <param name="accessToken">访问授权</param>
        /// <param name="userName">指定帐号</param>
        /// <param name="identification">身份标识</param>
        public override bool Follow(string accessToken, string userName, string identification = null)
        {
            _restClient.Authenticator = new CommonOAuthAuthenticator(accessToken);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.Resource = "2/friendships/create.json";
            request.AddParameter("screen_name", userName);
            var response = Execute(_restClient, request);
            var payload = Json.Decode(response.Content);
            return payload.id > 0;
        }
    }
}