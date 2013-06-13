//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 腾讯社区开放平台OAuth2，腾讯社区开放平台额外增加了一个OpenId的参数
    /// </summary>
    public class QQOAuthAuthenticator : OAuth2Authenticator
    {
        private readonly string openId;
        private readonly string consumerKey;

        /// <summary>
        /// 构造函数，OAuth2.0协议必须传入的通用参数
        /// </summary>
        /// <param name="openId">用户唯一ID</param>
        /// <param name="accessToken"></param>
        /// <param name="consumerkey">申请QQ登录成功后，分配给应用的appid </param>
        public QQOAuthAuthenticator(string openId, string accessToken, string consumerkey)
            :base(accessToken)
        {
            this.openId = openId;
            this.consumerKey = consumerkey;
        }

        /// <summary>
        /// 调用所有OpenAPI时，除了各接口私有的参数外，所有OpenAPI都需要传入基于OAuth2.0协议的通用参数
        /// 将这些通用参数加入RestRequest
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddParameter("access_token", AccessToken, ParameterType.GetOrPost);
            request.AddParameter("openid", openId, ParameterType.GetOrPost);
            request.AddParameter("oauth_consumer_key", consumerKey, ParameterType.GetOrPost);
        }
    }
}
