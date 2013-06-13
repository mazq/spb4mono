using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tunynet.Common;
using Tunynet.UI;
using Spacebuilder.Common;
using Tunynet;

namespace Spacebuilder.Bar.Controllers
{
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    public class BarActivityController : Controller
    {
        private ActivityService activityService = new ActivityService();
        private BarThreadService barThreadService = new BarThreadService();
        private BarPostService barPostService = new BarPostService();
        private AttachmentService attachementService = new AttachmentService(TenantTypeIds.Instance().BarThread());
        private BarRatingService barRatingService = new BarRatingService();

        #region 前台-动态


        /// <summary>
        /// 创建帖子的动态内容块
        /// </summary>
        /// <param name="ActivityId">动态id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _CreateBarThread(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);
            BarThread thread = barThreadService.Get(activity.SourceId);
            if (thread == null)
                return Content(string.Empty);
            PagingDataSet<BarPost> barPosts = barPostService.Gets(thread.ThreadId, false, SortBy_BarPost.DateCreated_Desc);
            
            
            ViewData["BarPosts"] = barPosts.Take(3);
            IEnumerable<Attachment> attachments = attachementService.GetsByAssociateId(thread.ThreadId);
            if (attachments != null && attachments.Count() > 0)
            {
                IEnumerable<Attachment> attachmentImages = attachments.Where(n => n.MediaType == MediaType.Image);
                if (attachmentImages != null && attachmentImages.Count() > 0)
                    ViewData["Attachments"] = attachmentImages;
            }
            ViewData["ActivityId"] = ActivityId;
            if (thread.BarSection.TenantTypeId != TenantTypeIds.Instance().Bar())
            {
                var tenantType = new TenantTypeService().Get(thread.BarSection.TenantTypeId);
                string tenantTypeName = string.Empty;
                if (tenantType != null)
                    tenantTypeName = tenantType.Name;
                ViewData["tenantTypeName"] = tenantTypeName;
            }
            return View(thread);
        }

        /// <summary>
        /// 创建主题帖的回帖的动态块
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _CreateBarPost(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);
            BarPost post = barPostService.Get(activity.SourceId);
            if (post == null)
                return Content(string.Empty);
            
            
            BarThread thread = barThreadService.Get(activity.ReferenceId);
            if (thread == null)
                return Content(string.Empty);
            ViewData["BarThread"] = thread;
            
            
            ViewData["Attachments"] = thread.Attachments.Where(n => n.MediaType == MediaType.Image).FirstOrDefault();
            ViewData["ActivityId"] = ActivityId;
            if (thread.BarSection.TenantTypeId != TenantTypeIds.Instance().Bar())
            {
                var tenantType = new TenantTypeService().Get(thread.BarSection.TenantTypeId);
                string tenantTypeName = string.Empty;
                if (tenantType != null)
                    tenantTypeName = tenantType.Name;
                ViewData["tenantTypeName"] = tenantTypeName;
            }
            return View(post);
        }

        /// <summary>
        /// 帖子评分的动态
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _CreateBarRating(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return HttpNotFound();
            BarRating rating = barRatingService.Get(activity.SourceId);
            if (rating == null)
                return HttpNotFound();
            ViewData["BarRating"] = rating;
            BarThread thread = barThreadService.Get(activity.ReferenceId);
            if (thread == null)
                return HttpNotFound();
            ViewData["ActivityId"] = ActivityId;
            
            
            ViewData["Attachments"] = thread.Attachments.Where(n => n.MediaType == MediaType.Image).FirstOrDefault();
            if (thread.BarSection.TenantTypeId != TenantTypeIds.Instance().Bar())
            {
                var tenantType = new TenantTypeService().Get(thread.BarSection.TenantTypeId);
                string tenantTypeName = string.Empty;
                if (tenantType != null)
                    tenantTypeName = tenantType.Name;
                ViewData["tenantTypeName"] = tenantTypeName;
            }
            return View(thread);
        }

        #endregion

    }
}
