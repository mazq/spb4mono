//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet.Common;
using Tunynet.UI;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博动态
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    public class MicroblogActivityController : Controller
    {
        #region 私有对象

        private ActivityService activityService = new ActivityService();
        private MicroblogService microBlogService = new MicroblogService();
        private ParsedMediaService parsedMediaService = new ParsedMediaService();
        private AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(TenantTypeIds.Instance().Microblog());

        #endregion

        #region 微博列表相应控件展示

        /// <summary>
        /// 微博列表，单条空间 - 通过动态获取
        /// </summary>
        /// <param name="activityId">动态Id</param>
        [HttpGet]
        public ActionResult _Microblog_Activity(long activityId)
        {
            Activity activity = activityService.Get(activityId);
            if (activity == null)
                return new EmptyResult();

            MicroblogEntity entity = microBlogService.Get(activity.SourceId);
            ViewData["ActivityId"] = activityId;

            return View("_Microblog", entity);
        }

        /// <summary>
        /// 微博列表，单条控件
        /// </summary>
        /// <param name="microblogId">微博Id</param>
        [HttpGet]
        public ActionResult _Microblog(long microblogId)
        {
            MicroblogEntity microblog = microBlogService.Get(microblogId);
            return View(microblog);
        }

        /// <summary>
        /// 微博列表，单条控件 - 附件列表
        /// </summary>
        /// <param name="microblogId">微博Id</param>
        /// <param name="forwardMicroblogId">转发微博Id</param>
        public ActionResult _Microblog_Attachments(long microblogId, long? forwardMicroblogId)
        {
            MicroblogEntity microblog = microBlogService.Get(microblogId);
            if (microblog == null) return new EmptyResult();

            if (microblog.HasPhoto)
            {
                IEnumerable<Attachment> attachments = attachementService.GetsByAssociateId(microblog.MicroblogId);

                if (attachments != null && attachments.Count() > 0)
                {
                    microblog.ImageUrl = SiteUrls.Instance().ImageUrl(attachments.First(), TenantTypeIds.Instance().Microblog(), ImageSizeTypeKeys.Instance().Medium());
                    ViewData["imageCount"] = attachments.Count();
                }
            }

            if (microblog.HasVideo)
            {
                ParsedMedia parsedMedia = parsedMediaService.Get(microblog.VideoAlias);
                if (parsedMedia != null)
                {
                    ViewData["videoThumbnailUrl"] = parsedMedia.ThumbnailUrl;
                }
            }

            ViewData["ForwardMicroblogId"] = forwardMicroblogId;

            return View(microblog);
        }

        /// <summary>
        /// 微博列表，单条控件 - 附件列表 - 图片
        /// </summary>
        /// <param name="forwardMicroblogId">转发微博Id</param>
        [HttpGet]
        public ActionResult _Microblog_Attachments_Images(long userId, int microblogId, long? forwardMicroblogId)
        {
            ViewData["ForwardMicroblogId"] = forwardMicroblogId;
            return View(attachementService.GetsByAssociateId(microblogId));
        }

        /// <summary>
        /// 微博列表，单条控件 - 附件列表 - 视频
        /// </summary>
        /// <param name="forwardMicroblogId">转发微博Id</param>
        [HttpGet]
        public ActionResult _Microblog_Attachments_Video(long microblogId, string alias, long? forwardMicroblogId)
        {
            ParsedMedia entity = parsedMediaService.Get(alias);
            if (entity == null) { return new EmptyResult(); }

            ViewData["ForwardMicroblogId"] = forwardMicroblogId;

            return View(entity);
        }

        /// <summary>
        /// 微博列表，单条控件 - 附件列表 - 音乐
        /// </summary>
        /// <param name="forwardMicroblogId">转发微博Id</param>
        [HttpGet]
        public ActionResult _Microblog_Attachments_Music(long microblogId, string alias, long? forwardMicroblogId)
        {
            ParsedMedia entity = parsedMediaService.Get(alias);
            if (entity == null) { return new EmptyResult(); }

            ViewData["ForwardMicroblogId"] = forwardMicroblogId;

            return View(entity);
        }

        #endregion
    }
}