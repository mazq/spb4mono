//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 扩展NoticeTemplateNames
    /// </summary>
    public static class NoticeTemplateNamesExtension
    {
        /// <summary>
        /// 新成员申请
        /// </summary>
        /// <param name="questionId">questionId</param>
        public static string NewMemberApply(this NoticeTemplateNames noticeTemplateNames)
        {
            return "NewMemberApply";
        }

        /// <summary>
        /// 成员申请成功
        /// </summary>
        /// <param name="questionId">questionId</param>
        public static string MemberApplyApproved(this NoticeTemplateNames noticeTemplateNames)
        {
            return "MemberApplyApproved";
        }

        /// <summary>
        /// 成员加入
        /// </summary>
        /// <param name="questionId">questionId</param>
        public static string MemberJoin(this NoticeTemplateNames noticeTemplateNames)
        {
            return "MemberJoin";
        }

        /// <summary>
        /// 成员申请被拒绝
        /// </summary>
        /// <param name="questionId">questionId</param>
        public static string MemberApplyDisapproved(this NoticeTemplateNames noticeTemplateNames)
        {
            return "MemberApplyDisapproved";
        }


        /// <summary>
        /// 成员退出
        /// </summary>
        /// <param name="questionId">questionId</param>
        public static string MemberQuit(this NoticeTemplateNames noticeTemplateNames)
        {
            return "MemberQuit";
        }

        /// <summary>
        /// 设置管理员
        /// </summary>
        /// <param name="questionId">questionId</param>
        public static string SetGroupManager(this NoticeTemplateNames noticeTemplateNames)
        {
            return "SetGroupManager";
        }

        /// <summary>
        /// 取消管理员
        /// </summary>
        /// <param name="questionId">questionId</param>
        public static string CannelGroupManager(this NoticeTemplateNames noticeTemplateNames)
        {
            return "CannelGroupManager";
        }
    }
}