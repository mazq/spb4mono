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
    /// 通知类型实体类
    /// </summary>
    public class NoticeType
    {
        private static ConcurrentDictionary<int, NoticeType> registeredNoticeTypes = new ConcurrentDictionary<int, NoticeType>();

        /// <summary>
        /// 静态构造器
        /// </summary>
        static NoticeType()
        {
            registeredNoticeTypes[NoticeTypeIds.Instance().Reply()] = new NoticeType() { TypeId = NoticeTypeIds.Instance().Reply(), TypeName = "回复", Description = "例如：新评论、新回复" };
            registeredNoticeTypes[NoticeTypeIds.Instance().Manage()] = new NoticeType() { TypeId = NoticeTypeIds.Instance().Manage(), TypeName = "管理", Description = "例如：群组中有新成员需批准" };
            registeredNoticeTypes[NoticeTypeIds.Instance().Hint()] = new NoticeType() { TypeId = NoticeTypeIds.Instance().Hint(), TypeName = "提示", Description = "例如：文章被设为精华" };
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public NoticeType()
        {

        }

        /// <summary>
        /// 获取所有通知类型
        /// </summary>
        /// <returns>通知类型</returns>
        public static IEnumerable<NoticeType> GetAll()
        {
            return registeredNoticeTypes.Values;
        }

        /// <summary>
        /// 获取通知类型
        /// </summary>
        /// <param name="typeId">类型Id</param>
        /// <returns>通知类型</returns>
        public static NoticeType Get(int typeId)
        {
            NoticeType noticeType;
            if (registeredNoticeTypes.TryGetValue(typeId, out noticeType))
                return noticeType;

            return null;
        }

        /// <summary>
        /// 添加通知类型
        /// </summary>
        /// <param name="noticeType">通知类型</param>
        public static void Add(NoticeType noticeType)
        {
            if (noticeType == null)
                return;
            registeredNoticeTypes[noticeType.TypeId] = noticeType;
        }

        /// <summary>
        /// 删除通知类型
        /// </summary>
        /// <param name="typeId">类型Id</param>
        public static void Remove(int typeId)
        {
            NoticeType noticeType;
            registeredNoticeTypes.TryRemove(typeId, out noticeType);
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
