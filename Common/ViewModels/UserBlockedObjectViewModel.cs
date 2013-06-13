//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户屏蔽对象
    /// </summary>
    public class UserBlockedObjectViewModel
    {
        /// <summary>
        /// 屏蔽时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// 屏蔽对象的主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 被屏蔽对象的id
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// 被屏蔽对象名
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 被屏蔽对象类型
        /// </summary>
        public int ObjectType { get; set; }

        /// <summary>
        /// 拥有者id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 将ViewModel对象转换成UserBlockedObject
        /// </summary>
        /// <returns></returns>
        public UserBlockedObject AsUserBlockedObject()
        {
            return new UserBlockedObject
            {
                DateCreated = DateTime.UtcNow,
                ObjectId = this.ObjectId,
                ObjectName = this.ObjectName,
                ObjectType = this.ObjectType,
                UserId = this.UserId
            };
        }
    }

    /// <summary>
    /// 屏蔽用户的扩展类
    /// </summary>
    public static class UserBlockedObjectExtensions
    {
        /// <summary>
        /// 将数据载体转换为ViewModel
        /// </summary>
        /// <param name="UserBlockedObject"></param>
        /// <returns></returns>
        public static UserBlockedObjectViewModel AsViewModel(this UserBlockedObject UserBlockedObject)
        {
            return new UserBlockedObjectViewModel
            {
                DateCreated = UserBlockedObject.DateCreated,
                Id = UserBlockedObject.Id,
                ObjectId = UserBlockedObject.ObjectId,
                ObjectName = UserBlockedObject.ObjectName,
                ObjectType = UserBlockedObject.ObjectType,
                UserId = UserBlockedObject.UserId
            };
        }
    }
}
