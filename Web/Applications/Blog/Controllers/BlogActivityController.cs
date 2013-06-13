//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tunynet.Common;
using Tunynet.UI;
using Spacebuilder.Common;
using Tunynet;

namespace Spacebuilder.Blog.Controllers
{
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    public class BlogActivityController : Controller
    {
        private ActivityService activityService = new ActivityService();
        private BlogService blogService = new BlogService();
        private CommentService commentService = new CommentService();
        private AttachmentService attachementService = new AttachmentService(TenantTypeIds.Instance().BlogThread());

        #region 前台-动态


        /// <summary>
        /// 创建日志的动态内容块
        /// </summary>
        /// <param name="ActivityId">动态id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _CreateBlogThread(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);

            BlogThread thread = blogService.Get(activity.SourceId);
            if (thread == null)
                return Content(string.Empty);

            PagingDataSet<Comment> comments = commentService.GetRootComments(TenantTypeIds.Instance().BlogThread(), thread.ThreadId, 1, SortBy_Comment.DateCreatedDesc);
            ViewData["Comments"] = comments.Take(3);

            //ViewData["Comments"] = commentService.GetTopComments(thread.ThreadId, TenantTypeIds.Instance().BlogThread(), 3, SortBy_Comment.DateCreatedDesc);

            IEnumerable<Attachment> attachments = attachementService.GetsByAssociateId(thread.ThreadId);
            if (attachments != null && attachments.Count() > 0)
            {
                IEnumerable<Attachment> attachmentImages = attachments.Where(n => n.MediaType == MediaType.Image);
                if (attachmentImages != null && attachmentImages.Count() > 0)
                    ViewData["Attachments"] = attachmentImages;
            }
            ViewData["ActivityId"] = ActivityId;
            return View(thread);
        }

        /// <summary>
        /// 创建日志的评论的动态块
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _CreateBlogComment(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);

            BlogThread thread = blogService.Get(activity.ReferenceId);
            if (thread == null)
                return Content(string.Empty);
            IEnumerable<Attachment> attachments = attachementService.GetsByAssociateId(thread.ThreadId);
            if (attachments != null && attachments.Count() > 0)
            {
                IEnumerable<Attachment> attachmentImages = attachments.Where(n => n.MediaType == MediaType.Image);
                if (attachmentImages != null && attachmentImages.Count() > 0)
                    ViewData["Attachments"] = attachmentImages.FirstOrDefault();
            }
            
            Comment comment = commentService.Get(activity.SourceId);
            if (comment == null)
                return Content(string.Empty);

            ViewData["BlogThread"] = thread;
            ViewData["ActivityId"] = ActivityId;

            IUserService userService = DIContainer.Resolve<IUserService>();
            ViewData["UserService"] = userService;

            return View(comment);
        }

        #endregion

    }
}