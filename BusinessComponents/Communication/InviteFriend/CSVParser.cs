//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Tunynet.Common
{
    /// <summary>
    /// CSV文件解析帮助类
    /// </summary>
    public static class CSVParser
    {
        /// <summary>
        /// 从CSV文件总获取联系人的方法
        /// </summary>
        /// <param name="fileStream">CSV文件流</param>
        /// <returns>联系人字典</returns>
        public static Dictionary<string, string> GetContactAccessor(Stream fileStream)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(fileStream, Encoding.Default);
            Dictionary<string, string> contacts = new Dictionary<string, string>(); //存放读取出来的csv联系人
            //根据表头确定联系人的名字在什么地方。
            int lastNameIndex = -1;
            int firstNameIndex = -1;
            if (reader.Peek() > 0)
            {
                string contact = reader.ReadLine();
                contact = contact.Replace("\"", "");
                string[] contactArray = contact.Split(new char[] { ',' });
                for (int i = 0; i < contactArray.Length; i++)
                {
                    if (string.IsNullOrEmpty(contactArray[i]))
                        continue;
                    switch (contactArray[i].ToLower())
                    {
                        case "姓名":
                            lastNameIndex = i;
                            break;
                        case "name":
                            lastNameIndex = i;
                            break;
                        case "名":
                            lastNameIndex = i;
                            break;
                        case "姓":
                            firstNameIndex = i;
                            break;
                        default:
                            break;
                    }
                }
            }
            //循环获取联系人名和Email信息
            while (reader.Peek() > 0)
            {
                string contact = reader.ReadLine();
                contact = contact.Replace("\"", "");
                string[] contactArray = contact.Split(new char[] { ',' });
                string name = string.Empty;
                if (firstNameIndex != -1)
                    name += contactArray[firstNameIndex];
                if (lastNameIndex != -1)
                    name += contactArray[lastNameIndex];
                Regex regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                Match match = regex.Match(contact);
                string email = null;
                if (match.Success)
                    email = match.Value;
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
                    contacts[email] = name;
            }
            return contacts;
        }
    }
}
