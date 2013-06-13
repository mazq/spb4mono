//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Google.Contacts;
using Google.GData.Client;
using Google.GData.Extensions;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// Gmail邮箱联系人管理器
    /// </summary>
    public class GmailContactAccessor : IEmailContactAccessor
    {
        /// <summary>
        /// 邮箱后缀
        /// </summary>
        public string EmailDomainName
        {
            get { return "gmail.com"; }
        }

        /// <summary>
        /// 获取邮箱联系人
        /// </summary>
        /// <param name="userName">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="isSuccessLogin">是否成功登录</param>
        /// <returns>Key:联系人Email地址，Value：联系人名称</returns>
        public Dictionary<string, string> GetContacts(string userName, string password, out bool isSuccessLogin)
        {
            Dictionary<string, string> dicContacts = getContact(userName, password);
            isSuccessLogin = dicContacts.Count <= 0 ? false : true;
            return dicContacts;
        }

        /// <summary>
        /// 得到Gmail通讯录的内容，利用googleAPI获取联系人
        /// </summary>
        /// <returns>通讯录集合</returns>
        public Dictionary<string, string> getContact(string userName, string password)
        {
            Dictionary<string, string> dicContact = new Dictionary<string, string>();
            RequestSettings rs = new RequestSettings("", userName, password);

            rs.AutoPaging = true;
            ContactsRequest cr = new ContactsRequest(rs);

            Feed<Contact> f = cr.GetContacts();

            try
            {
                foreach (Contact contact in f.Entries)
                {
                    string name = string.Empty;
                    string emailStr = string.Empty;
                    foreach (EMail email in contact.Emails)
                    {
                        if (!string.IsNullOrEmpty(contact.Title))
                        {
                            name = contact.Title;
                        }
                        else
                        {
                            name = "暂无名称";
                        }
                        emailStr = email.Address;
                        dicContact[emailStr] = name;
                    }
                }
            }
            catch (Exception)
            {
                return new Dictionary<string, string>();
            }
            return dicContact;
        }
    }
}