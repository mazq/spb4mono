//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;

namespace Tunynet.Common
{
    /// <summary>
    /// 关注用户查询条件
    /// </summary>
    public enum Follow_SortBy
    {
        /// <summary>
        /// 关注时间
        /// </summary>
        DateCreated_Desc,

        /// <summary>
        /// 内容最后更新时间
        /// </summary>
        LastContent_Desc,

        /// <summary>
        /// 粉丝数
        /// </summary>
        FollowerCount_Desc
    }


    /// <summary>
    /// 关注特殊分组Id
    /// </summary>
    public static class FollowSpecifyGroupIds
    {

        /// <summary>
        /// 未分组
        /// </summary>
        public const int UnGrouped = 0;

        /// <summary>
        /// 所有分组
        /// </summary>
        public const int All = -1;

        /// <summary>
        /// 相互关注
        /// </summary>        
        public const int Mutual = -2;

        /// <summary>
        /// 悄悄关注
        /// </summary>
        public const int Quietly = -3;

    }
}