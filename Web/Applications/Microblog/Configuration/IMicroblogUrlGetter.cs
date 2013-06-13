//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 贴吧中获取连接的接口
    /// </summary>
    public interface IMicroblogUrlGetter
    {
        /// <summary>
        /// 租户类型id
        /// </summary>
        string TenantTypeId { get; }

        /// <summary>
        /// 动态拥有者类型
        /// </summary>
        int ActivityOwnerType { get; }

        /// <summary>
        /// 是否为私有状态
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        bool IsPrivate(long ownerId);

        /// <summary>
        /// 拥有者名称
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        string GetOwnerName(long ownerId);

        /// <summary>
        /// 话题详细页
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns></returns>
        string TopicDetail(string tagName, long ownerId = 0);

        /// <summary>
        /// 微博详细显示页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="MicroblogId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        string MicroblogDetail(long microblogId, long? commentId = null);

        /// <summary>
        /// 获取拥有者链接
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        string GetOwnerUrl(long ownerId);

        /// <summary>
        /// 后台快捷操局部页面的连接
        /// </summary>
        /// <returns></returns>
        string _ManageSubMenu();
    }
}