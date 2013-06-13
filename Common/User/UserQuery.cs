//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
namespace Spacebuilder.Common
{
    /// <summary>
    /// 封装后台管理用户时用于查询用户的条件
    /// </summary>
    public class UserQuery
    {
        /// <summary>
        /// 名称（姓名、昵称、用户名）
        /// </summary>
        public string Keyword = string.Empty;

        /// <summary>
        /// 帐号邮件
        /// </summary>
        public string AccountEmailFilter = string.Empty;

        /// <summary>
        /// 是否已激活
        /// </summary>
        public bool? IsActivated = null;

        /// <summary>
        /// 是否已封禁
        /// </summary>
        public bool? IsBanned = null;

        /// <summary>
        /// 用户是否被管制
        /// </summary>
        public bool? IsModerated = null;

        /// <summary>
        /// 用户角色
        /// </summary>
        public string RoleName = string.Empty;

        /// <summary>
        /// 注册时间下限（晚于或等于本时间注册的）
        /// </summary>
        public DateTime? RegisterTimeLowerLimit = null;

        /// <summary>
        /// 注册时间上限（早于或等于本时间注册的）
        /// </summary>
        public DateTime? RegisterTimeUpperLimit = null;

        /// <summary>
        /// 等级下限（大于等于此等级的）
        /// </summary>
        public int? UserRankLowerLimit = null;

        /// <summary>
        /// 等级上线（小于等级此等级的）
        /// </summary>
        public int? UserRankUpperLimit = null;

        /// <summary>
        /// 排序方式
        /// </summary>
        public UserSortBy? UserSortBy = null;
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    public enum UserSortBy
    {
        /// <summary>
        /// 根据id排序
        /// </summary>
        UserId = 1,

        /// <summary>
        /// 根据id倒序排列
        /// </summary>
        UserId_Desc = 2,

        /// <summary>
        /// 根据上次活动时间排序
        /// </summary>
        LastActivityTime = 3,

        /// <summary>
        /// 根据上次活动时间倒序
        /// </summary>
        LastActivityTime_Desc = 4,

        /// <summary>
        /// 是否激活
        /// </summary>
        IsActivated = 5,

        /// <summary>
        /// 是否激活倒序
        /// </summary>
        IsActivated_Desc = 6,

        /// <summary>
        /// 是否被封禁
        /// </summary>
        IsModerated = 7,

        /// <summary>
        /// 是否被封禁倒序排序
        /// </summary>
        IsModerated_Desc = 8
    }
}
