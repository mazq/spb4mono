//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 通知处理状态
    /// </summary>
    public class NoticeTemplateNames
    {
        #region Instance
        private static NoticeTemplateNames _instance = new NoticeTemplateNames();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static NoticeTemplateNames Instance()
        {
            return _instance;
        }

        private NoticeTemplateNames()
        { }

        #endregion

        /// <summary>
        /// 新回复
        /// </summary>
        /// <returns></returns>
        public string NewReply()
        {
            return "NewReply";
        }

        /// <summary>
        /// 新评论
        /// </summary>
        /// <returns></returns>
        public string NewComment()
        {
            return "NewComment";
        }
        /// <summary>
        /// 你的“XXX”已被管理员批准
        /// </summary>
        /// <returns></returns>
        public string ManagerApproved()
        {
            return "ManagerApproved";
        }
        /// <summary>
        /// 你的“XXX”未被管理员批准
        /// </summary>
        /// <returns></returns>
        public string ManagerDisapproved()
        {
            return "ManagerDisapproved";
        }
        /// <summary>
        /// 你的“XXX”已被管理员取消精华
        /// </summary>
        /// <returns></returns>
        public string ManagerCancelEssential()
        {
            return "ManagerCancelEssential";
        }
        /// <summary>
        /// 你的“XXX”已被管理员取消置顶
        /// </summary>
        /// <returns></returns>
        public string ManagerCancelSticky()
        {
            return "ManagerCancelSticky";
        }
        /// <summary>
        /// 你的“XXX”已被管理员设为精华
        /// </summary>
        /// <returns></returns>
        public string ManagerSetEssential()
        {
            return "ManagerSetEssential";
        }
        /// <summary>
        /// 你的“XXX”已被管理员设为置顶
        /// </summary>
        /// <returns></returns>
        public string ManagerSetSticky()
        {
            return "ManagerSetSticky";
        }
        /// <summary>
        /// 你的“XXX”已被管理员推荐
        /// </summary>
        /// <returns></returns>
        public string ManagerRecommended()
        {
            return "ManagerRecommended";
        }

        /// <summary>
        /// 你的照片被XXX圈了
        /// </summary>
        /// <returns></returns>
        public string PhotoLabelNotice()
        {
            return "PhotoLabelNotice";
        }

        /// <summary>
        /// 你被XXX圈了
        /// </summary>
        /// <returns></returns>
        public string PhotoLabeledNotice()
        {
            return "PhotoLabeledNotice";
        }
    }
}