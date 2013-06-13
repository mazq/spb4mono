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
using System.Xml.Linq;

namespace Tunynet.Common
{
    /// <summary>
    /// 请求类型实体类
    /// </summary>
    public class InvitationType
    {
        private static ConcurrentDictionary<string, InvitationType> registeredInvitationTypes = new ConcurrentDictionary<string, InvitationType>();

        /// <summary>
        /// 构造器
        /// </summary>
        public InvitationType()
        {

        }

        /// <summary>
        /// 获取所有请求类型
        /// </summary>
        /// <returns>请求类型集合</returns>
        public static IEnumerable<InvitationType> GetAll()
        {
            return registeredInvitationTypes.Values;
        }

        /// <summary>
        /// 获取请求类型
        /// </summary>
        /// <param name="typeKey">类型Key</param>
        /// <returns>请求类型</returns>
        public static InvitationType Get(string typeKey)
        {
            InvitationType InvitationType;
            if (registeredInvitationTypes.TryGetValue(typeKey, out InvitationType))
                return InvitationType;

            return null;
        }

        /// <summary>
        /// 注册请求类型
        /// </summary>
        /// <param name="InvitationType">请求类型</param>
        public static void Register(InvitationType InvitationType)
        {
            if (InvitationType == null)
                return;
            registeredInvitationTypes[InvitationType.Key] = InvitationType;
        }
              

        #region 属性

        /// <summary>
        /// 类型Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型描述
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
