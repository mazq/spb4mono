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
    /// 通用OAuth2协议授权验证
    /// </summary>
    public class CommonOAuthAuthenticator : OAuth2Authenticator
    {

        /// <summary>
        /// 构造函数，OAuth2.0协议必须传入的通用参数
        /// </summary>
        /// <param name="accessToken"></param>
        public CommonOAuthAuthenticator(string accessToken)
            :base(accessToken)
        {
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
        }
    }
}
