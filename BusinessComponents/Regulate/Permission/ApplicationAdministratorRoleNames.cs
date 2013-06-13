//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Tunynet.Common
{
    /// <summary>
    /// 应用管理员角色处理器
    /// </summary>
    public class ApplicationAdministratorRoleNames
    {
        private static ConcurrentDictionary<int, IEnumerable<string>> registeredApplicationAdministratorRoleNames = new ConcurrentDictionary<int, IEnumerable<string>>();

        /// <summary>
        /// 构造器
        /// </summary>
        public ApplicationAdministratorRoleNames()
        {

        }
        /// <summary>
        /// 获取应用管理员角色
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <returns>应用管理员角色</returns>
        public static IEnumerable<string> GetAll()
        {
            List<string> roleNames = new List<string>();
            foreach (var item in registeredApplicationAdministratorRoleNames.Values)
            {
                roleNames.AddRange(item);
            }
            return roleNames;
        }

        /// <summary>
        /// 获取应用管理员角色
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <returns>应用管理员角色</returns>
        public static IEnumerable<string> GetRoleNames(int applicationId)
        {
            IEnumerable<string> roleNames;
            if (registeredApplicationAdministratorRoleNames.TryGetValue(applicationId, out roleNames))
                return roleNames;

            return null;
        }

        /// <summary>
        /// 添加应用管理员角色
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="applicationAdministratorRoleNames">应用管理员角色</param>
        public static void Add(int applicationId, IEnumerable<string> applicationAdministratorRoleNames)
        {
            if (applicationId <= 0 || applicationAdministratorRoleNames == null)
                return;
            registeredApplicationAdministratorRoleNames[applicationId] = applicationAdministratorRoleNames;
        }

        /// <summary>
        /// 删除应用管理员角色
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        public static void Remove(int applicationId)
        {
            IEnumerable<string> roleNames;
            registeredApplicationAdministratorRoleNames.TryRemove(applicationId, out roleNames);
        }
    }
}
