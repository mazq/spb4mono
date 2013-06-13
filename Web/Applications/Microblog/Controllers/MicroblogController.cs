//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.FileStore;
using Tunynet.Mvc;
using Tunynet.UI;
using Tunynet.Utilities;
using System.Text;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 空间微博
    /// </summary>
    [AnonymousBrowseCheck]
    [Themed(PresentAreaKeysOfBuiltIn.UserSpace, IsApplication = true)]
    public partial class MicroblogController : Controller
    {
        #region Service

        private Authorizer authorizer = new Authorizer();
        private MicroblogService microblogService = new MicroblogService();
        private AttachmentService<Attachment> attachmentService = new AttachmentService<Attachment>(TenantTypeIds.Instance().Microblog());
        private FavoriteService favoriteService = new FavoriteService(TenantTypeIds.Instance().Microblog());
        private ActivityService activityServcice = new ActivityService();

        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();

        private IUserService userService = DIContainer.Resolve<IUserService>();

        #endregion

        #region 维护

        /// <summary>
        /// 创建微博
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="content">微博内容</param>
        /// <param name="imageUrl">图片Url</param>
        /// <param name="isShare">是否为分享微博</param>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _Create(string spaceKey, string tenantTypeId, long? ownerId, string content,
            string imageUrl, bool? isShare)
        {
            string errorMessage = string.Empty;
            tenantTypeId = string.IsNullOrEmpty(tenantTypeId) ? TenantTypeIds.Instance().User() : tenantTypeId;
            if (!new Authorizer().Microblog_Create(out errorMessage, tenantTypeId, ownerId ?? 0))
            {
                if (Request.IsAjaxRequest())
                    return Json(new StatusMessageData(StatusMessageType.Error, errorMessage), JsonRequestBehavior.AllowGet);
                else
                    return new EmptyResult();
            }
            ViewData["isShare"] = isShare ?? false;
            ViewData["content"] = Tunynet.Utilities.WebUtility.UrlDecode(content);
            ViewData["imageUrl"] = Tunynet.Utilities.WebUtility.UrlDecode(imageUrl);

            #region 根据Cookie，删除临时附件

            HttpCookie cookie = Request.Cookies["microblog_PhotoExists"];
            if (cookie != null && cookie.Value.Trim().ToLower().CompareTo("true") == 0)
            {

                //reply:已修改
                attachmentService.DeleteTemporaryAttachments(ownerId ?? 0, tenantTypeId);
                cookie.Value = "";
                Response.Cookies.Set(cookie);
            }

            #endregion

            return View();
        }

        /// <summary>
        /// 创建微博
        /// </summary>
        /// <param name="spaceKey">当前空间用户</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="imageUrl">图片地址</param>
        /// <param name="microblogBody">微博内容</param>
        [HttpPost]
        [ValidateInput(false)]
        [PostInterval(PostIntervalType = PostIntervalType.MicroContent)]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult Create(string spaceKey, string microblogBody, string tenantTypeId = null, long ownerId = 0, string imageUrl = null)
        {
            if (string.IsNullOrEmpty(microblogBody))
                return Json(new { status = "error", message = "内容不能为空！" });
            if (!ValidateContentLength(microblogBody))
                return Json(new { status = "error", message = "内容不能超过140个字！" });

            //当前用户登录
            IUser currentUser = UserContext.CurrentUser;

            bool isBanned = ModelState.HasBannedWord();
            MicroblogEntity entity = MicroblogEntity.New();
            entity.Author = currentUser.DisplayName;
            entity.Body = Tunynet.Utilities.WebUtility.HtmlEncode(microblogBody);
            entity.PostWay = PostWay.Web;
            entity.TenantTypeId = !string.IsNullOrEmpty(tenantTypeId) ? tenantTypeId : TenantTypeIds.Instance().User();
            entity.UserId = currentUser.UserId;
            entity.OwnerId = ownerId > 0 ? ownerId : currentUser.UserId;

            if (!new Authorizer().Microblog_Create(entity.TenantTypeId, entity.OwnerId))
                return HttpNotFound();

            //判断是否当前有，图片附件
            HttpCookie cookie = Request.Cookies["microblog_PhotoExists"];
            if (cookie != null && cookie.Value.Trim().ToLower().Equals("true"))
            {
                entity.HasPhoto = true;
                cookie.Value = "";
                Response.Cookies.Set(cookie);
            }

            if (!string.IsNullOrEmpty(imageUrl))
                entity.HasPhoto = true;

            bool isSuccess = false;
            if (!isBanned)
            {
                isSuccess = microblogService.Create(entity);
            }

            //by zhengw:
            if (isSuccess)
            {
                //处理imageUrl
                if (!string.IsNullOrEmpty(imageUrl))
                    DownloadRemoteImage(imageUrl, entity.MicroblogId);

                //同步微博
                var accountBindingService = new AccountBindingService();
                foreach (var accountType in accountBindingService.GetAccountTypes(true, true))
                {
                    bool isSync = Request.Form.GetBool("sync_" + accountType.AccountTypeKey, false);
                    if (isSync)
                    {
                        var account = accountBindingService.GetAccountBinding(currentUser.UserId, accountType.AccountTypeKey);
                        if (account != null)
                        {
                            var thirdAccountGetter = ThirdAccountGetterFactory.GetThirdAccountGetter(accountType.AccountTypeKey);
                            if (entity.HasPhoto)
                            {
                                byte[] bytes = null;
                                var attachments = attachmentService.GetsByAssociateId(entity.MicroblogId);
                                string fileName = null;
                                if (attachments.Count() > 0)
                                {
                                    var attachment = attachments.First();
                                    IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
                                    IStoreFile storeFile = storeProvider.GetResizedImage(attachment.GetRelativePath(), attachment.FileName, new Size(405, 600), Tunynet.Imaging.ResizeMethod.KeepAspectRatio);
                                    using (Stream stream = storeFile.OpenReadStream())
                                    {
                                        bytes = StreamToBytes(stream);
                                        stream.Dispose();
                                        stream.Close();
                                    }
                                    fileName = attachment.FriendlyFileName;
                                }
                                thirdAccountGetter.CreatePhotoMicroBlog(account.AccessToken, entity.Body, bytes, fileName, account.Identification);

                            }
                            else
                                thirdAccountGetter.CreateMicroBlog(account.AccessToken, entity.Body, account.Identification);
                        }
                    }
                }
                if ((int)entity.AuditStatus > (int)(new AuditService().GetPubliclyAuditStatus(MicroblogConfig.Instance().ApplicationId)))
                {
                    return Json(new { status = "sussess", message = "发布成功", id = entity.MicroblogId });
                }
                else
                {
                    return Json(new { status = "hint", message = "尚未通过审核，请耐心等待", id = entity.MicroblogId });
                }
            }

            if (isBanned)
                return Json(new { status = "error", message = "内容中有非法词语！" });
            else
                return Json(new { status = "error", message = "创建失败请联系管理员！" });
        }

        /// <summary>
        /// 下载远程图片
        /// </summary>
        /// <param name="imageUrl">将要下载的图片地址</param>
        /// <param name="microblogId">微博Id</param>
        private void DownloadRemoteImage(string imageUrl, long microblogId)
        {
            if (UserContext.CurrentUser == null || microblogId <= 0 || string.IsNullOrEmpty(imageUrl))
                return;
            try
            {
                WebRequest webRequest = WebRequest.Create(SiteUrls.FullUrl(imageUrl));
                HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream stream = httpWebResponse.GetResponseStream();
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                bool isImage = httpWebResponse.ContentType.StartsWith("image");
                if (!isImage || stream == null || !stream.CanRead)
                    return;
                string friendlyFileName = imageUrl.Substring(imageUrl.LastIndexOf("/") + 1);
                Attachment attachment = new Attachment(ms, httpWebResponse.ContentType, friendlyFileName);
                attachment.FileLength = httpWebResponse.ContentLength;
                attachment.AssociateId = microblogId;
                attachment.UserId = UserContext.CurrentUser.UserId;
                attachment.UserDisplayName = UserContext.CurrentUser.DisplayName;
                attachment.TenantTypeId = TenantTypeIds.Instance().Microblog();
                var attachmentService = new AttachmentService(TenantTypeIds.Instance().Microblog());
                attachmentService.Create(attachment, ms);
                ms.Dispose();
                ms.Close();
            }
            catch { }
        }

        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        private byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 添加音乐
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _Create_AddMusic()
        {
            return View();
        }

        /// <summary>
        /// 微博话题列表
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _Create_AddTopic()
        {
            TagService tagService = new TagService(TenantTypeIds.Instance().Microblog());
            return View(tagService.GetTopTags(6, null, SortBy_Tag.ItemCountDesc));
        }

        /// <summary>
        /// 添加视频
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _Create_AddVideo()
        {
            return View();
        }

        /// <summary>
        /// 上传图片 - 列表
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _Create_ListImages(string spaceKey, long associateId = 0)
        {

            //reply:已修改
            IUser user = UserContext.CurrentUser;
            string tenantTypeId = TenantTypeIds.Instance().Microblog();

            if (user == null)
            {
                return Redirect(SiteUrls.Instance().Login());
            }
            IEnumerable<Attachment> attachments = null;

            //reply:已修改

            if (associateId == 0)
            {
                attachments = attachmentService.GetTemporaryAttachments(user.UserId, tenantTypeId);

                //reply:已修改
            }
            else
            {
                attachments = attachmentService.GetsByAssociateId(associateId);

                //reply:已修改
            }


            //reply:已修改


            //reply:已修改

            return View(attachments);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _Create_UploadImages(string spaceKey)
        {

            //reply:已修改
            ViewData["TenantAttachmentSettings"] = TenantAttachmentSettings.GetRegisteredSettings(TenantTypeIds.Instance().Microblog());

            return View();
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="spaceKey">空间所有者Key</param>
        /// <param name="attachmentId">附件Id</param>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult _Create_DeleteAttachment(string spaceKey, long attachmentId)
        {
            try
            {
                attachmentService.Delete(attachmentId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("删除失败！", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 转发微博控件
        /// </summary>
        public ActionResult _ForwardMicroblog(string spaceKey, long microblogId)
        {
            MicroblogEntity microblogEntity = microblogService.Get(microblogId);
            return View(microblogEntity);
        }

        /// <summary>
        /// 转发微博
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ForwardMicroblog(string forwardBody)
        {
            if (!ValidateContentLength(forwardBody))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "内容超出字数限制！"));
            }

            bool isBanned = ModelState.HasBannedWord();
            forwardBody = Tunynet.Utilities.WebUtility.HtmlEncode(forwardBody);
            if (isBanned)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "内容中包含非法词语！"));
            }

            bool isCommnet = Request.Form.GetBool("isCommnet", false);
            bool isCommentOriginal = Request.Form.GetBool("isCommentOriginal", false);

            IUser currentUser = UserContext.CurrentUser;

            MicroblogEntity microblog = MicroblogEntity.New();
            microblog.Body = string.IsNullOrEmpty(forwardBody) ? "转发微博" : forwardBody;
            microblog.Author = currentUser.DisplayName;
            microblog.UserId = currentUser.UserId;
            microblog.OwnerId = currentUser.UserId;
            microblog.TenantTypeId = TenantTypeIds.Instance().User();
            microblog.ForwardedMicroblogId = Request.Form.Get<long>("forwardedMicroblogId", 0);
            microblog.OriginalMicroblogId = Request.Form.Get<long>("originalMicroblogId", 0);

            if (!new Authorizer().Microblog_Create(microblog.TenantTypeId, microblog.OwnerId))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有权限进行转发！"));
            }

            long toUserId = Request.Form.Get<long>("toUserId", 0);
            long toOriginalUserId = Request.Form.Get<long>("toOriginalUserId", 0);

            microblogService.Forward(microblog, isCommnet, isCommentOriginal, toUserId, toOriginalUserId);


            //reply:已修改

            return Json(new StatusMessageData(StatusMessageType.Success, "转发成功！"));

        }

        /// <summary>
        /// 删除当前微博
        /// </summary>
        [HttpPost]
        public JsonResult Delete(string spaceKey, long microblogId)
        {
            MicroblogEntity microblog = microblogService.Get(microblogId);
            if (!new Authorizer().Microblog_Delete(microblog))
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"), JsonRequestBehavior.AllowGet);

            long currentUserId = UserContext.CurrentUser.UserId;
            activityServcice.DeleteFromUserInbox(currentUserId, 1);
            microblogService.Delete(microblogId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 空间列表


        /// <summary>
        /// 我的微博
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize]
        public ActionResult Mine(string spaceKey, string type, int? pageIndex)
        {
            IUser owner = userService.GetUser(spaceKey);
            if (owner == null)
            {
                return new EmptyResult();
            }

            pageResourceManager.InsertTitlePart((UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == owner.UserId ? "我" : owner.DisplayName) + "的微博");

            MediaType? mediaType = null;
            bool? isOriginal = null;
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("original"))
                {
                    isOriginal = true;
                }
                else
                {
                    MediaType tempMediaType;
                    Enum.TryParse(type, out tempMediaType);
                    mediaType = tempMediaType;
                }
            }

            PagingDataSet<long> pds = microblogService.GetPaingIds(owner.UserId, mediaType, isOriginal, pageIndex ?? 1);

            return View(pds);
        }

        /// <summary>
        /// 我的微博局部页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ListMyMicroblogs(string spaceKey, string type, int? pageIndex)
        {
            IUser owner = userService.GetUser(spaceKey);
            if (owner == null)
            {
                return new EmptyResult();
            }

            MediaType? mediaType = null;
            bool? isOriginal = null;

            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("original"))
                {
                    isOriginal = true;
                }
                else
                {
                    MediaType tempMediaType;
                    Enum.TryParse(type, out tempMediaType);
                    mediaType = tempMediaType;
                }
            }

            PagingDataSet<long> pds = microblogService.GetPaingIds(owner.UserId, mediaType, isOriginal, pageIndex ?? 1);

            return View(pds);
        }

        //reply:已修改
        /// <summary>
        /// 提到我的
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="pageIndex">页码</param>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult ListReferred(string spaceKey, int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("提到我的");

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            return View(microblogService.GetMicroblogsByReferredUser(userId, pageIndex ?? 1));
        }

        #region 收藏

        /// <summary>
        ///  添加收藏、取消收藏
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="itemId">微博Id</param>
        /// <param name="userId">用户Id</param>
        [HttpPost]
        public JsonResult Favorite(string spaceKey, long itemId, long userId)
        {
            bool isFavorited = favoriteService.IsFavorited(itemId, userId);

            bool result = false;
            if (isFavorited)
                result = favoriteService.CancelFavorite(itemId, userId);
            else
                result = favoriteService.Favorite(itemId, userId);

            //reply:已修改

            if (result)
            {
                return Json(new { ok = true, msg = "操作成功" });
            }
            else
            {
                return Json(new { msg = "操作失败" });
            }
        }

        /// <summary>
        ///  我的收藏页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="pageIndex">页数</param>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult ListFavorites(string spaceKey, int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("我的收藏");

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            PagingDataSet<long> pdsObjectIds = favoriteService.GetPagingObjectIds(userId, pageIndex ?? 1);
            IEnumerable<MicroblogEntity> microblogs = microblogService.GetMicroblogs(pdsObjectIds);
            PagingDataSet<MicroblogEntity> pds = new PagingDataSet<MicroblogEntity>(microblogs)
            {
                TotalRecords = pdsObjectIds.TotalRecords - (microblogs.Count()),
                PageSize = pdsObjectIds.PageSize,
                PageIndex = pdsObjectIds.PageIndex,
                QueryDuration = pdsObjectIds.QueryDuration
            };

            return View(pds);
        }

        #endregion

        #endregion

        #region 详细显示

        /// <summary>
        /// 微博详细页
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize]
        public ActionResult Detail(string spaceKey, long microblogId)
        {
            MicroblogEntity entity = microblogService.Get(microblogId);
            if (entity == null)
            {
                return HttpNotFound();
            }

            //验证是否通过审核
            long currentSpaceUserId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (!authorizer.IsAdministrator(MicroblogConfig.Instance().ApplicationId) && entity.UserId != currentSpaceUserId
                && (int)entity.AuditStatus < (int)(new AuditService().GetPubliclyAuditStatus(MicroblogConfig.Instance().ApplicationId)))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "尚未通过审核",
                    Body = "由于当前微博尚未通过审核，您无法浏览当前内容。",
                    StatusMessageType = StatusMessageType.Hint
                }));

            IUser user = userService.GetUser(spaceKey);
            if (user == null)
            {
                return new EmptyResult();
            }


            pageResourceManager.InsertTitlePart(user.DisplayName + "的微博");

            return View(entity);
        }

        /// <summary>
        /// 微博详细显示局部视图
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="microblogId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Detail(string spaceKey, long microblogId)
        {
            MicroblogEntity entity = microblogService.Get(microblogId);
            return View(entity);
        }

        #endregion

        #region 评论

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="commentedObjectId">评论对象Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="originalAuthor">作者</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Comment(long commentedObjectId, long ownerId, string tenantTypeId, string originalAuthor = null)
        {
            CommentEditModel commentModel = new CommentEditModel
            {
                CommentedObjectId = commentedObjectId,
                OwnerId = ownerId,
                TenantTypeId = tenantTypeId
            };
            return View(commentModel);
        }

        #endregion

        #region 话题/标签

        /// <summary>
        /// 关注话题
        /// </summary>
        [UserSpaceAuthorize]
        public ActionResult _FollowTopics(string spaceKey, int? topNumber)
        {
            FavoriteService favoriteService = new FavoriteService(TenantTypeIds.Instance().Tag());
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IEnumerable<Tag> tags = new TagService(TenantTypeIds.Instance().Microblog()).GetTags(favoriteService.GetTopObjectIds(userId, topNumber ?? 15));

            return View(tags);
        }

        #endregion

        #region 用户

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult _UserInfo()
        {
            User user = userService.GetUser(Url.SpaceKey()) as User;
            MicroblogService microblogService = new MicroblogService();
            return View(user);
        }

        /// <summary>
        /// 用户状态菜单控件
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        public ActionResult _UserStatus(string spaceKey)
        {
            User user = userService.GetUser(spaceKey) as User;
            CountService countService = new CountService(TenantTypeIds.Instance().User());
            int countPerDay = countService.GetStageCount(CountTypes.Instance().HitTimes(), 7, user.UserId);
            int countAll = countService.Get(CountTypes.Instance().HitTimes(), user.UserId);
            ViewData["accessedCount"] = countPerDay + "/" + countAll;

            VisitService visitService = new VisitService(TenantTypeIds.Instance().User());
            IEnumerable<Visit> visits = visitService.GetTopMyVisits(user.UserId, 1);

            if (visits != null && visits.FirstOrDefault() != null)
            {
                ViewData["lastVisitDate"] = visits.FirstOrDefault().LastVisitTime.ToFriendlyDate();
            }

            return View(user);
        }

        /// <summary>
        /// 关注的用户菜单控件
        /// </summary>
        /// <returns></returns>
        public ActionResult _TopFollowedUsers(string spaceKey, int topNumber)
        {
            Dictionary<long, bool> isFollowesUser = new Dictionary<long, bool>();

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            long currentUserId = UserContext.CurrentUser.UserId;

            FollowService followService = new FollowService();
            IEnumerable<long> ids = followService.GetTopFollowedUserIds(userId, topNumber);

            foreach (var id in ids)
            {
                isFollowesUser[id] = followService.IsFollowed(currentUserId, id);
            }

            ViewData["isFollowesUser"] = isFollowesUser;

            if (currentUserId == userId)
            {
                ViewData["isSameUser"] = true;
            }

            ViewData["gender"] = (userService.GetUser(spaceKey) as User).Profile.Gender;
            IEnumerable<User> users = userService.GetFullUsers(ids);

            return View(users);
        }

        #endregion

        #region Helper Method

        /// <summary>
        /// 验证内容的长度
        /// </summary>
        /// <param name="body">待验证的内容</param>
        /// <returns></returns>
        private bool ValidateContentLength(string body)
        {
            if (string.IsNullOrEmpty(body))
                return false;
            int rawStringLength = Encoding.UTF8.GetBytes(body).Length;
            return rawStringLength <= 140 * 3;
        }

        /// <summary>
        /// 内容过滤
        /// </summary>
        /// <param name="body">待过滤内容</param>
        /// <param name="isBanned">是否禁止提交</param>
        private string TextFilter(string body, out bool isBanned)
        {
            isBanned = false;
            if (string.IsNullOrEmpty(body))
            {
                return body;
            }

            string temBody = body;
            WordFilterStatus staus = WordFilterStatus.Replace;
            temBody = WordFilter.SensitiveWordFilter.Filter(body, out staus);

            if (staus == WordFilterStatus.Banned)
            {
                isBanned = true;
                return body;
            }

            body = temBody;
            HtmlUtility.CleanHtml(body, TrustedHtmlLevel.Basic);

            return body;
        }

        #endregion
    }
}