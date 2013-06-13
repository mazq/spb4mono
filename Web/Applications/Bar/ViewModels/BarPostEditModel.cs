//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using Spacebuilder.Common;
using System.Web.Mvc;
using Tunynet.Utilities;
using System.ComponentModel.DataAnnotations;
using Tunynet;
using Spacebuilder.Bar.Resources;
using Tunynet.Mvc;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 回帖编辑的数据传递载体
    /// </summary>
    public class BarPostEditModel
    {
        private static BarSettings barSettings = DIContainer.Resolve<IBarSettingsManager>().Get();


        /// <summary>
        /// 所属主题帖Id
        /// </summary>
        public long ThreadId { get; set; }

        /// <summary>
        /// 所属帖吧的id
        /// </summary>
        public long SectionId { get; set; }

        /// <summary>
        /// 父回帖的Id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 回帖的标题
        /// </summary>
        [StringLength(TextLengthSettings.TEXT_SUBJECT_MAXLENGTH, ErrorMessage = "最多可以输入{1}个字")]
        public string Subject { get; set; }

        /// <summary>
        /// 帖子的主题内容
        /// </summary>
        [AllowHtml]
        [DataType(DataType.Html)]
        [Required(ErrorMessage = "请输入内容")]
        public string Body { get; set; }

        /// <summary>
        /// 帖子的主题内容
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "请输入内容")]
        public string MultilineBody { get; set; }

        /// <summary>
        /// 回帖的Id
        /// </summary>
        public long? PostId { get; set; }

        /// <summary>
        /// 转换成BarPost类型
        /// </summary>
        /// <returns></returns>
        public BarPost AsBarPost()
        {
            BarThread thread = new BarThreadService().Get(this.ThreadId);

            BarPostService service = new BarPostService();
            BarPost post = null;
            //编辑的情况
            if (this.PostId.HasValue)
            {
                post = service.Get(this.PostId ?? 0);
                if (post == null)
                    return null;
            }
            else
            {
                //创建的情况
                post = BarPost.New();
                post.AuditStatus = AuditStatus.Success;
                post.TenantTypeId = thread.TenantTypeId;
                post.ThreadId = this.ThreadId;
                if (UserContext.CurrentUser != null)
                {
                    post.UserId = UserContext.CurrentUser.UserId;
                    post.Author = UserContext.CurrentUser.DisplayName;
                }
                else
                {
                    post.UserId = 0;
                    post.Author = "匿名用户";
                }
                post.OwnerId = thread == null ? 0 : thread.OwnerId;
                post.SectionId = thread == null ? 0 : thread.SectionId;
                post.ParentId = this.ParentId;
            }

            if (!string.IsNullOrEmpty(this.Body))
            {
                post.Body = this.Body;
            }
            else
            {
                this.MultilineBody = HtmlUtility.CleanHtml(this.MultilineBody, TrustedHtmlLevel.Basic);
                this.MultilineBody = new EmotionService().EmoticonTransforms(this.MultilineBody);
                post.Body = this.MultilineBody;
            }
            return post;
        }
    }

    /// <summary>
    /// 回帖的扩展方法
    /// </summary>
    public static class BarPostEditModelExtensions
    {
        /// <summary>
        /// 数据库中的对象转换为EditModel
        /// </summary>
        /// <returns></returns>
        public static BarPostEditModel AsEditModel(this BarPost post)
        {
            BarSettings barSettings = DIContainer.Resolve<IBarSettingsManager>().Get();
            BarThread thread = new BarThreadService().Get(post.ThreadId);
            return new BarPostEditModel
            {
                Body = post.GetBody(),
                ParentId = post.ParentId,
                PostId = post.PostId,
                Subject = thread.Subject,
                ThreadId = post.ThreadId,
                SectionId = post.SectionId
            };
        }
    }
}