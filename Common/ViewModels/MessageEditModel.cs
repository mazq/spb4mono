//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户表单呈现的Message实体
    /// </summary>
    public class MessageEditModel
    {

        [Display(Name = "收件人")]
        [Required(ErrorMessage = "请选择收件人")]
        public string ToUserIds { get; set; }

        /// <summary>
        ///私信标题
        /// </summary>
        [Display(Name = "私信标题")]
        [WaterMark(Content = "私信标题")]
        [DataType(DataType.Text)]
        public string Subject { get; set; }

        /// <summary>
        ///私信内容
        /// </summary>
        [Required(ErrorMessage = "内容为必填项")]
        [WaterMark(Content = "私信内容")]
        [Display(Name = "私信内容")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        /// <summary>
        /// 转换为Message用于数据库存储
        /// </summary>
        public Message AsMessage()
        {

            #region 对内容和标题的相关处理

            WordFilterStatus status = WordFilterStatus.Replace;
            string newBody = WordFilter.SensitiveWordFilter.Filter(this.Body, out status);
            newBody = HtmlUtility.CleanHtml(this.Body, TrustedHtmlLevel.Basic);

            #endregion

            Message message = Message.New();
            message.Subject = this.Subject;
            message.Body = this.Body;

            message.IsRead = false;
            IUser user = UserContext.CurrentUser;
            if (user != null)
            {
                message.Sender = user.DisplayName;
                message.SenderUserId = user.UserId;
            }
            return message;
        }
    }
}