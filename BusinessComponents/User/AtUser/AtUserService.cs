//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using System;
using Tunynet.Caching;
using System.Text;
using Tunynet.Common.Configuration;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户关联业务逻辑类
    /// </summary>
    public class AtUserService
    {
        private IAtUserRepository atUserRepository;
        private static int pageSize = 30;
        private string tenantTypeId = string.Empty;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public AtUserService(string tenantTypeId)
            : this(tenantTypeId, new AtUserRepository())
        {
            this.tenantTypeId = tenantTypeId;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="atUserRepository">atUser仓储</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public AtUserService(string tenantTypeId, IAtUserRepository atUserRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.atUserRepository = atUserRepository;
        }

        /// <summary>
        /// 批量创建At用户
        /// </summary>
        /// <param name="userNames">用户Id集合</param>
        /// <param name="associateId">关联项Id</param>
        /// <param name="userId">添加at的用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateSubject">关联项标题</param>
        /// <returns></returns>
        public bool BatchCreateAtUser(List<long> userIds, long associateId, long userId, string associateSubject = "")
        {
            EventBus<long, AtUserEventArgs>.Instance().OnBatchBefore(userIds, new AtUserEventArgs(userId, tenantTypeId, associateId));
            bool isSuccess = atUserRepository.BatchCreateAtUser(userIds, associateId, tenantTypeId);
            EventBus<long, AtUserEventArgs>.Instance().OnBatchAfter(userIds, new AtUserEventArgs(userId, tenantTypeId, associateId));

            return isSuccess;
        }

        /// <summary>
        /// 清除关注用户
        /// </summary>
        /// <param name="associateId">关联项Id</param>
        public void ClearAtUsers(long associateId)
        {
            atUserRepository.ClearAtUsers(associateId, tenantTypeId);
        }

        /// <summary>
        /// 获取用户关联内容的Id分页集合
        /// </summary>
        /// <param name="userId">关联用户Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingAssociateIds(long userId, int pageIndex)
        {
            return atUserRepository.GetPagingAssociateIds(userId, tenantTypeId, pageIndex);
        }

        /// <summary>
        /// 获取用户关联内容的用户Id集合
        /// </summary>
        /// <param name="associateId">关联项Id</param>
        /// <returns></returns>
        public List<long> GetAtUserIds(long associateId)
        {
            return atUserRepository.GetAtUserIds(associateId, tenantTypeId);
        }



        /// <summary>
        /// 解析内容用于创建AtUser
        /// </summary>
        /// <param name="body">待解析的内容</param>
        /// <param name="userId">内容作者UserId</param>
        /// <param name="associateId">关联项Id</param>
        public void ResolveBodyForEdit(string body, long userId, long associateId)
        {
            if (string.IsNullOrEmpty(body) || !body.Contains("@"))
                return;

            List<long> userIds = new List<long>();

            PrivacyService privacyService = new PrivacyService();
            IUserService userService = DIContainer.Resolve<IUserService>();

            string userNameRegex = new UserSettings().NickNameRegex, tempNickName = string.Empty;
            userNameRegex = userNameRegex.TrimStart('^').TrimEnd('$');

            Regex rg = new Regex("(?<=(\\@))" + userNameRegex, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matches = rg.Matches(body);

            if (matches != null)
            {
                foreach (Match m in matches)
                {
                    if (string.IsNullOrEmpty(m.Value) || tempNickName.Equals(m.Value, StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    tempNickName = m.Value;
                    IUser user = userService.GetUserByNickName(tempNickName);

                    if (user == null
                        || userIds.Contains(user.UserId)
                        || !privacyService.Validate(user.UserId, userId, PrivacyItemKeys.Instance().AtUser()))
                        continue;

                    userIds.Add(user.UserId);
                }
            }

            if (userIds.Count > 0)
                BatchCreateAtUser(userIds, associateId, userId);
        }

        /// <summary>
        /// 解析内容中的AtUser用户展示展示
        /// </summary>
        /// <param name="body">待解析的内容</param>
        /// <param name="associateId">关联项Id</param>
        /// <param name="userId">关联项作者Id</param>
        /// <param name="TagGenerate">用户生成对应标签的方法</param>
        public string ResolveBodyForDetail(string body, long associateId, long userId, Func<string, string, string> TagGenerate)
        {
            if (string.IsNullOrEmpty(body) || !body.Contains("@") || userId <= 0)
                return body;

            IList<long> userIds = GetAtUserIds(associateId);

            if (userIds != null)
            {
                PrivacyService privacyService = new PrivacyService();
                IUserService userService = DIContainer.Resolve<IUserService>();
                bool endMatch = false;
                foreach (var atUserId in userIds)
                {
                    if (atUserId == 0)
                        continue;

                    IUser user = userService.GetUser(atUserId);
                    if (user == null)
                        continue;

                    if (privacyService.Validate(user.UserId, userId, PrivacyItemKeys.Instance().AtUser()))
                    {
                        string nickName = user.NickName;

                        body = body.Replace("@" + nickName, TagGenerate(user.UserName, nickName));
                        body = body.Replace("@" + nickName + "</p>", TagGenerate(user.UserName, nickName) + "</p>");

                        if (!endMatch && body.EndsWith("@" + nickName))
                        {
                            endMatch = true;
                            body = body.Remove(body.Length - (nickName.Length + 1), nickName.Length + 1) + TagGenerate(user.UserName, nickName);
                        }
                    }
                }
            }
            return body;
        }
    }
}