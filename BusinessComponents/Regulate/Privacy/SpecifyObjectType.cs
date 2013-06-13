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
    /// 隐私设置指定对象类型
    /// </summary>
    public class SpecifyObjectType
    {
        private static ConcurrentDictionary<int, SpecifyObjectType> registeredPrivacyItemTypes = new ConcurrentDictionary<int, SpecifyObjectType>();

        /// <summary>
        /// 静态构造器
        /// </summary>
        static SpecifyObjectType()
        {
            registeredPrivacyItemTypes[SpecifyObjectTypeIds.Instance().User()] = new SpecifyObjectType() { TypeId = SpecifyObjectTypeIds.Instance().User(), TypeName = "用户", Description = "" };
            registeredPrivacyItemTypes[SpecifyObjectTypeIds.Instance().UserGroup()] = new SpecifyObjectType() { TypeId = SpecifyObjectTypeIds.Instance().UserGroup(), TypeName = "关注分组", Description = "" };
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public SpecifyObjectType()
        {

        }

        /// <summary>
        /// 获取所有隐私设置指定对象类型
        /// </summary>
        /// <returns>隐私类型</returns>
        public static IEnumerable<SpecifyObjectType> GetAll()
        {
            return registeredPrivacyItemTypes.Values;
        }

        /// <summary>
        /// 获取隐私设置指定对象类型
        /// </summary>
        /// <param name="typeId">类型Id</param>
        /// <returns>隐私设置指定对象类型</returns>
        public static SpecifyObjectType Get(int typeId)
        {
            SpecifyObjectType PrivacyItemType;
            if (registeredPrivacyItemTypes.TryGetValue(typeId, out PrivacyItemType))
                return PrivacyItemType;

            return null;
        }

        /// <summary>
        /// 添加隐私设置指定对象类型
        /// </summary>
        /// <param name="PrivacyItemType">隐私设置指定对象类型</param>
        public static void Add(SpecifyObjectType PrivacyItemType)
        {
            if (PrivacyItemType == null)
                return;
            registeredPrivacyItemTypes[PrivacyItemType.TypeId] = PrivacyItemType;
        }


        #region 属性

        /// <summary>
        /// 类型Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 类型描述
        /// </summary>
        public string Description { get; set; }

        #endregion
    }

}