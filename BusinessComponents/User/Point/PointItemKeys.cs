//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 积分类型配置类（便于使用PointItemKey）
    /// </summary>
    /// <remarks>
    /// 各Application应该对该配置类的方法进行扩展
    /// </remarks>
    public class PointItemKeys
    {
        #region Instance
        private static PointItemKeys _instance = new PointItemKeys();

        /// <summary>
        /// 获取该类的单例
        /// </summary>
        /// <returns></returns>
        public static PointItemKeys Instance()
        {
            return _instance;
        }

        private PointItemKeys()
        { }
        #endregion


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public string Register()
        {
            return "Register";
        }

        /// <summary>
        /// 首次上传头像
        /// </summary>
        /// <returns></returns>
        public string FirstUploadAvatar()
        {
            return "FirstUploadAvatar";
        }

        /// <summary>
        /// 邀请用户注册
        /// </summary>
        /// <returns></returns>
        public string InviteUserRegister()
        {
            return "InviteUserRegister";
        }

        /// <summary>
        /// 被邀请用户解管制
        /// </summary>
        /// <returns></returns>
        public string FreeModeratedUser()
        {
            return "FreeModeratedUser";
        }

        /// <summary>
        /// 被邀请用户删除
        /// </summary>
        /// <returns></returns>
        public string DeleteInvitedUser()
        {
            return "DeleteInvitedUser";
        }


        /// <summary>
        /// 关注用户
        /// </summary>
        /// <returns></returns>
        public string FollowUser()
        {
            return "FollowUser";
        }

        /// <summary>
        /// 取消关注用户
        /// </summary>
        /// <returns></returns>
        public string CancelFollowUser()
        {
            return "CancelFollowUser";
        }


        /// <summary>
        /// 用户被推荐
        /// </summary>
        /// <returns></returns>
        public string RecommendUser()
        {
            return "RecommendUser";
        }


        /// <summary>
        /// 内容被推荐
        /// </summary>
        /// <returns></returns>
        public string RecommendContent()
        {
            return "RecommendContent";
        }


        /// <summary>
        /// 内容被加精
        /// </summary>
        /// <returns></returns>
        public string EssentialContent()
        {
            return "EssentialContent";
        }


        /// <summary>
        /// 内容被置顶
        /// </summary>
        /// <returns></returns>
        public string StickyContent()
        {
            return "StickyContent";
        }

        /// <summary>
        /// 发表评论
        /// </summary>
        /// <returns></returns>
        public string CreateComment()
        {
            return "CreateComment";
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <returns></returns>
        public string DeleteComment()
        {
            return "DeleteComment";
        }

        /// <summary>
        /// 发表评价
        /// </summary>
        /// <returns></returns>
        public string CreateEvaluation()
        {
            return "CreateEvaluation";
        }


        /// <summary>
        /// 取消评价
        /// </summary>
        /// <returns></returns>
        public string CancelEvaluation()
        {
            return "CancelEvaluation";
        }
    }
}
