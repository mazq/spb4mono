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
    /// 隐私项目分组实体类
    /// </summary>
    public class PrivacyItemGroup
    {
        private static ConcurrentDictionary<int, PrivacyItemGroup> registeredPrivacyItemGroups = new ConcurrentDictionary<int, PrivacyItemGroup>();

        /// <summary>
        /// 静态构造器
        /// </summary>
        static PrivacyItemGroup()
        {
            registeredPrivacyItemGroups[PrivacyItemGroupIds.Instance().VisitSpace()] = new PrivacyItemGroup() { TypeId = PrivacyItemGroupIds.Instance().VisitSpace(), TypeName = "空间访问", Description = "设置哪些人可以访问我的空间" };
            registeredPrivacyItemGroups[PrivacyItemGroupIds.Instance().Profile()] = new PrivacyItemGroup() { TypeId = PrivacyItemGroupIds.Instance().Profile(), TypeName = "个人资料", Description = "包括基本资料、联系方式、工作经历、教育经历" };
            registeredPrivacyItemGroups[PrivacyItemGroupIds.Instance().Interactive()] = new PrivacyItemGroup() { TypeId = PrivacyItemGroupIds.Instance().Interactive(), TypeName = "沟通互动", Description = "向我提交 加关注、发请求、发私信、评论等操作" };
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public PrivacyItemGroup()
        {

        }

        /// <summary>
        /// 获取所有隐私项目类型
        /// </summary>
        /// <returns>通知类型</returns>
        public static IEnumerable<PrivacyItemGroup> GetAll()
        {
            return registeredPrivacyItemGroups.Values;
        }

        /// <summary>
        /// 获取隐私项目类型
        /// </summary>
        /// <param name="typeId">类型Id</param>
        /// <returns>隐私项目类型</returns>
        public static PrivacyItemGroup Get(int typeId)
        {
            PrivacyItemGroup PrivacyItemGroup;
            if (registeredPrivacyItemGroups.TryGetValue(typeId, out PrivacyItemGroup))
                return PrivacyItemGroup;

            return null;
        }

        /// <summary>
        /// 添加隐私项目类型
        /// </summary>
        /// <param name="PrivacyItemGroup">隐私项目类型</param>
        public static void Add(PrivacyItemGroup PrivacyItemGroup)
        {
            if (PrivacyItemGroup == null)
                return;
            registeredPrivacyItemGroups[PrivacyItemGroup.TypeId] = PrivacyItemGroup;
        }

        //done:zhengw,by mazq  什么时候用？
        //zhengw回复：在站点启动时，二次开发者想移除预置类型时使用
        //mazq回复：解释一下在什么场景为什么会移除隐私项目类型，移除以后对正在运行的程序有影响吗？
        //zhengw回复：主要是为了定制项目考虑，因为隐私分组是封装到底层，二次开发者若没有源码，则无法移除已经预置的类型。有影响，需要调整调用位置的代码

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
