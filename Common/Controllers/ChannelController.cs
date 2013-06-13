//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spacebuilder.Search;
using Spacebuilder.UI;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Mvc;
using Tunynet.Search;
using Tunynet.UI;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 频道Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = false)]
    public class ChannelController : Controller
    {
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private CategoryService categoryService = new CategoryService();
        private SearchedTermService searchedTermService = new SearchedTermService();
        private EmotionService emotionService = DIContainer.Resolve<EmotionService>();
        private CommentService commentService = new CommentService();
        private FollowService followService = new FollowService();
        private UserService userService = new UserService();
        private ActivityService activityService = new ActivityService();
        private SchoolService schoolService = new SchoolService();
        private ImpeachReportService impeachReportService = new ImpeachReportService();
        private AnnouncementService announcementService = new AnnouncementService();
        private IdentificationService dentificationService = new IdentificationService();
        private LinkService linkService = new LinkService();
        private LogoService logoService = new LogoService(TenantTypeIds.Instance().Link());
        private AttachmentDownloadService attachmentDownloadService = new AttachmentDownloadService();
        private ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
        private PrivacyService privacyService = new PrivacyService();
        private UserSettings userSettings = DIContainer.Resolve<IUserSettingsManager>().Get();
        /// <summary>
        /// 用于跳转的站点首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SiteHome()
        {
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();

            if (siteSettings.EnableSimpleHome && UserContext.CurrentUser == null)
                return Redirect(SiteUrls.Instance().SimpleHome());

            return Redirect(SiteUrls.Instance().SiteHome());
        }

        #region 暂停站点页面
        /// <summary>
        /// 暂停站点
        /// </summary>
        /// <returns></returns>
        public ActionResult PausePage()
        {
            pageResourceManager.InsertTitlePart("暂停站点");
            IPauseSiteSettingsManager pauseSiteSettingsManager = DIContainer.Resolve<IPauseSiteSettingsManager>();
            PauseSiteSettings pauseSiteSettings = pauseSiteSettingsManager.get();

            if (pauseSiteSettings.PausePageType)
            {
                return View(pauseSiteSettings);
            }
            else
            {
                return Redirect(pauseSiteSettings.PauseLink);
            }

        }

        /// <summary>
        /// 暂停页header
        /// </summary>
        /// <returns></returns>
        public ActionResult PausePageHeader()
        {
            if (UserContext.CurrentUser != null)
            {
                MessageService messageService = new MessageService();
                InvitationService invitationService = new InvitationService();
                NoticeService noticeService = new NoticeService();

                long userId = UserIdToUserNameDictionary.GetUserId(UserContext.CurrentUser.UserName);
                int count = 0;
                count = invitationService.GetUnhandledCount(userId);
                count += messageService.GetUnreadCount(userId);
                count += noticeService.GetUnhandledCount(userId);
                ViewData["PromptCount"] = count;
            }

            //获取当前是在哪个应用下搜索
            RouteValueDictionary routeValueDictionary = Request.RequestContext.RouteData.DataTokens;
            string areaName = routeValueDictionary.Get<string>("area", null) + "Search";
            ViewData["search"] = areaName;

            NavigationService service = new NavigationService();
            IEnumerable<Navigation> navigations = service.GetRootNavigations(PresentAreaKeysOfBuiltIn.Channel).Where(n => n.IsVisible(UserContext.CurrentUser) == true).Where(n => n.ApplicationId != 0);

            if (navigations != null)
            {
                ViewData["Navigations"] = navigations.OrderBy(n => n.DisplayOrder);
            }

            return View();
        }

        #endregion

        #region 广告位
        /// <summary>
        /// 显示广告位下的广告
        /// </summary>
        /// <returns></returns>
        public ActionResult _AdvertisingPosition(long advertisingId)
        {
            Advertising advertising = new AdvertisingService().GetAdvertising(advertisingId);
            ViewData["advertising"] = advertising;
            return View();
        }
        #endregion

        #region 公共区域

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns>返回主页信息</returns>
        public ActionResult Home()
        {
            SiteSettings siteSettings = siteSettingsManager.Get();
            pageResourceManager.InsertTitlePart(siteSettings.SiteDescription);
            pageResourceManager.InsertTitlePart(siteSettings.SiteName);
            pageResourceManager.SetMetaOfDescription(siteSettings.SearchMetaDescription);
            pageResourceManager.SetMetaOfKeywords(siteSettings.SearchMetaKeyWords);

            //近期人气用户
            IEnumerable<IUser> hotUsers = userService.GetTopUsers(20, SortBy_User.PreWeekHitTimes);
            ViewData["hotUsers"] = hotUsers;
            return View();
        }

        /// <summary>
        /// 简单首页
        /// </summary>
        /// <returns>简单首页</returns>
        [HttpGet]
        public ActionResult SimpleHome()
        {
            SiteSettings siteSettings = siteSettingsManager.Get();

            //当前网页如果是作为站点首页。并且用户是登陆的情况下。再查看此页面是不合适的。
            if (siteSettings.EnableSimpleHome && UserContext.CurrentUser != null)
                return Redirect(SiteUrls.Instance().SiteHome());

            pageResourceManager.InsertTitlePart(siteSettings.SiteDescription);
            pageResourceManager.InsertTitlePart(siteSettings.SiteName);
            pageResourceManager.SetMetaOfDescription(siteSettings.SearchMetaDescription);
            pageResourceManager.SetMetaOfKeywords(siteSettings.SearchMetaKeyWords);
            return View();
        }


        public ActionResult _ActivitiesList(int pageIndex = 1)
        {
            IEnumerable<Activity> activities = activityService.GetSiteActivities(null, 15, pageIndex);
            if (activities.Count() > 0)
            {
                ViewData["haveData"] = true;
            }
            else
            {
                ViewData["haveData"] = false;
            }
            return View(activities);
        }

        /// <summary>
        /// 获取以后进入用户时间线的动态
        /// </summary>
        [HttpPost]
        public ActionResult _GetNewerActivities(int pageIndex)
        {
            IEnumerable<Activity> newActivities = activityService.GetSiteActivities(null, 15, pageIndex);
            return View(newActivities);
        }

        /// <summary>
        /// 页头
        /// </summary>
        /// <returns></returns>
        public ActionResult _Header()
        {
            if (UserContext.CurrentUser != null)
            {
                MessageService messageService = new MessageService();
                InvitationService invitationService = new InvitationService();
                NoticeService noticeService = new NoticeService();

                long userId = UserIdToUserNameDictionary.GetUserId(UserContext.CurrentUser.UserName);
                int count = 0;
                count = invitationService.GetUnhandledCount(userId);
                count += messageService.GetUnreadCount(userId);
                count += noticeService.GetUnhandledCount(userId);
                ViewData["PromptCount"] = count;
            }

            //获取当前是在哪个应用下搜索
            RouteValueDictionary routeValueDictionary = Request.RequestContext.RouteData.DataTokens;
            string areaName = routeValueDictionary.Get<string>("area", null) + "Search";
            ViewData["search"] = areaName;

            NavigationService service = new NavigationService();
            IEnumerable<Navigation> navigations = service.GetRootNavigations(PresentAreaKeysOfBuiltIn.Channel).Where(n => n.IsVisible(UserContext.CurrentUser) == true);
            bool groupIsEnable = false;
            ApplicationBase groupApplication = new ApplicationService().Get(1011);
            if (groupApplication != null && groupApplication.IsEnabled)
            {
                groupIsEnable = true;
            }
            ViewData["groupIsEnable"] = groupIsEnable;

            if (navigations != null)
            {
                ViewData["Navigations"] = navigations.OrderBy(n => n.DisplayOrder);
            }

            return PartialView();
        }

        /// <summary>
        /// 页脚
        /// </summary>
        /// <returns></returns>
        public ActionResult _Footer()
        {
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            ViewData["SiteSettings"] = siteSettingsManager.Get();

            return PartialView();
        }

        /// <summary>
        /// 主导航
        /// </summary>
        /// <returns>所有导航</returns>
        public ActionResult _ChannelNavigation()
        {
            NavigationService navigationService = new NavigationService();
            return View(navigationService.GetRootNavigations("Channel"));
        }

        /// <summary>
        /// 内容Header
        /// </summary>
        [BuildNavigationRouteData(PresentAreaKey = PresentAreaKeysOfBuiltIn.Channel)]
        public ActionResult _ContentHeader()
        {
            #region 获取导航

            NavigationService navigationService = new NavigationService();
            int currentNavigationId = RouteData.Values.Get<int>("CurrentNavigationId", 0);

            Navigation currentNavigation = navigationService.GetNavigation(PresentAreaKeysOfBuiltIn.Channel, currentNavigationId);
            if (currentNavigation == null)
                return new EmptyResult();

            IEnumerable<Navigation> navigations = new List<Navigation>();
            if (currentNavigationId > 0)
            {
                IEnumerable<int> currentNavigationPath = navigationService.GetCurrentNavigationPath(PresentAreaKeysOfBuiltIn.ControlPanel, 0, currentNavigationId);
                IEnumerable<Navigation> rootNavigations = navigationService.GetRootNavigations(PresentAreaKeysOfBuiltIn.Channel);

                int parentNavigationId = 0;
                if (currentNavigationPath.Count() > 1)
                {
                    parentNavigationId = currentNavigationPath.ElementAt(currentNavigationPath.Count() - 2);
                }
                else if (currentNavigationPath.Count() == 1)
                {
                    parentNavigationId = currentNavigationPath.First();
                }
                else if (currentNavigation.ParentNavigationId > 0)
                {
                    parentNavigationId = currentNavigation.ParentNavigationId;
                }

                Navigation parentNavigation = new Navigation();
                if (parentNavigationId == 0)
                {
                    parentNavigation = currentNavigation;
                }
                else
                {
                    parentNavigation = navigationService.GetNavigation(PresentAreaKeysOfBuiltIn.Channel, parentNavigationId);
                }

                if (parentNavigation.NavigationId > 0)
                {

                    navigations = parentNavigation.Children;

                    ApplicationService applicationService = new ApplicationService();
                    ApplicationBase application = applicationService.Get(parentNavigation.ApplicationId);

                    ViewData["application"] = application;
                }
            }

            #endregion

            #region 快捷导航

            ManagementOperationService managementOperationService = new ManagementOperationService();
            ViewData["ManagementOperations"] = managementOperationService.GetShortcuts(PresentAreaKeysOfBuiltIn.Channel, false);

            #endregion

            return View(navigations.OrderBy(n => n.DisplayOrder));
        }

        #endregion 公共区域

        #region 表情选择器

        /// <summary>
        /// 编辑器用表情选择器
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EmotionSelector()
        {
            IList<EmotionCategory> categories = emotionService.GetEmotionCategories(true);
            return View(categories);
        }

        /// <summary>
        /// 获取表情json数据
        /// </summary>
        /// <param name="directoryName">表情目录名</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetEmotions(string directoryName)
        {
            EmotionCategory category = emotionService.GetEmotionCategory(directoryName);

            return Json(new
            {
                MaxWidth = category.EmotionMaxWidth,
                MaxHeight = category.EmotionMaxHeight,
                Emotions = category.Emotions.Select(n => new
                {
                    Code = n.FormatedCode,
                    ImgUrl = n.ImageUrl,
                    Description = n.Description,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion 表情选择器

        #region @用户

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textareaId"></param>
        /// <param name="seletorId"></param>
        /// <returns></returns>
        public ActionResult _AtUsers(string textareaId = null, string seletorId = null)
        {
            ViewData["textareaId"] = textareaId;
            ViewData["seletorId"] = seletorId;
            return View();
        }

        #endregion

        #region 附件管理器

        /// <summary>
        /// 附件上传的管理
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        public ActionResult _AttachmentManage(string tenantTypeId, long associateId = 0)
        {
            ViewData["associateId"] = associateId;
            ViewData["tenantTypeId"] = tenantTypeId;
            return View();
        }

        /// <summary>
        /// 附件上传编辑页面
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        /// <returns></returns>
        public ActionResult _EditAttachment(string tenantTypeId, long associateId = 0)
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);
            ViewData["attachmentLength"] = tenantAttachmentSettings.MaxAttachmentLength / 1024;
            ViewData["associateId"] = associateId;
            ViewData["tenantTypeId"] = tenantTypeId;
            ViewData["allowedFileExtensions"] = tenantAttachmentSettings.AllowedFileExtensions;
            return View();
        }

        /// <summary>
        /// 附件列表
        /// </summary>
        /// <param name="associateId">附件关联Id</param>
        public ActionResult _ListAttachments(string tenantTypeId, long associateId = 0)
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            List<List<object>> attachmentMessages = new List<List<object>>();
            IEnumerable<Attachment> attachments = null;
            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
            if (associateId == 0)
            {
                attachments = attachementService.GetTemporaryAttachments(user.UserId, tenantTypeId);
            }
            else
            {
                attachments = attachementService.GetsByAssociateId(associateId);
            }
            if (attachments != null)
            {
                attachments = attachments.Where(n => n.MediaType != MediaType.Image);
            }
            return View(attachments);
        }

        /// <summary>
        /// 附件库页面
        /// </summary>
        /// <returns></returns>
        public ActionResult _EditAttachmentLibraries()
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            return View();
        }

        /// <summary>
        /// 网络文件上传
        /// </summary>
        public ActionResult _EditNetAttachment()
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            return View();
        }

        /// <summary>
        /// 保存售价
        /// </summary>
        ///<param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _SavePrice(string tenantTypeId)
        {
            string price = Request.Form.GetString("Price", string.Empty).Trim();
            int priceValue = 0;
            if (price != "0" && price != string.Empty)
            {
                int.TryParse(price, out priceValue);
                if (priceValue == 0)
                    return Json(new StatusMessageData(StatusMessageType.Error, "请输入正整数"));
            }
            long attachementId = Request.Form.Get<long>("attachementId", 0);
            AttachmentService attachementService = new AttachmentService(tenantTypeId);
            Attachment attachment = attachementService.Get(attachementId);
            if (attachment == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "抱歉，找不到您所要出售的附件"));
            attachementService.UpdatePrice(attachementId, priceValue);
            return Json(new { price = priceValue });
        }

        #endregion 附件管理器

        #region 标题图设置

        /// <summary>
        /// 设置标题图
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">关联附件Id</param>
        /// <param name="attachmentIds">已经选中的附件Id</param>
        /// <param name="htmlFieldName">隐藏域全名称</param>
        /// <param name="isMultiSelect">是否多选</param>
        /// <param name="maxSelect">最大选择数量</param>
        public ActionResult _SetTitleImage(string tenantTypeId, long associateId = 0, string htmlFieldName = "", bool isMultiSelect = false, string attachmentIds = "", int maxSelect = 0)
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return Redirect(SiteUrls.Instance().Login());
            }

            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
            IEnumerable<Attachment> attachments = null;
            if (associateId == 0)
            {
                attachments = attachementService.GetTemporaryAttachments(user.UserId, tenantTypeId);
            }
            else
            {
                attachments = attachementService.GetsByAssociateId(associateId);
            }

            IEnumerable<Attachment> images = attachments.Where(n => n.MediaType == MediaType.Image);

            ViewData["associateId"] = associateId;
            ViewData["htmlFieldName"] = htmlFieldName;
            ViewData["isMultiSelect"] = isMultiSelect;
            ViewData["attachmentIds"] = attachmentIds;
            ViewData["tenantTypeId"] = tenantTypeId;
            ViewData["maxSelect"] = maxSelect;

            return View();
        }

        /// <summary>
        /// 呈现已上传图片的列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="attachmentIds">附件Id</param>
        /// <param name="associateId">关联附件Id</param>
        /// <returns></returns>
        public ActionResult _TitleImageList(string tenantTypeId, string attachmentIds, long associateId)
        {
            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
            IEnumerable<Attachment> attachments = null;
            if (associateId == 0)
            {
                attachments = attachementService.GetTemporaryAttachments(UserContext.CurrentUser.UserId, tenantTypeId);
            }
            else
            {
                attachments = attachementService.GetsByAssociateId(associateId);
            }

            IEnumerable<Attachment> images = attachments.Where(n => n.MediaType == MediaType.Image);
            ViewData["_attachmentIds"] = attachmentIds;
            ViewData["_tenantTypeId"] = tenantTypeId;
            return View(images);
        }

        #endregion 标题图设置

        #region 图片管理器

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="attachmentId">附件Id</param>
        [HttpPost]
        public ActionResult _DeleteAttachment(string tenantTypeId, long attachmentId)
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
            attachementService.Delete(attachmentId);
            if (attachmentId > 0)
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        /// <summary>
        /// 网络图片上传
        /// </summary>
        public ActionResult _EditNetImage()
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            return View();
        }

        /// <summary>
        /// 相册图片
        /// </summary>
        public ActionResult _EditPhoto()
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            return View();
        }

        /// <summary>
        /// 图片上传的管理
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        public ActionResult _ImageManage(string tenantTypeId, long associateId = 0)
        {
            ViewData["associateId"] = associateId;
            ViewData["tenantTypeId"] = tenantTypeId;
            return View();
        }

        /// <summary>
        /// 图片上传编辑页
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        public ActionResult _EditImage(string tenantTypeId, long associateId = 0)
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);
            ViewData["attachmentLength"] = tenantAttachmentSettings.MaxAttachmentLength;
            ViewData["associateId"] = associateId;
            ViewData["tenantTypeId"] = tenantTypeId;
            ViewData["imageHeight"] = tenantAttachmentSettings.InlinedImageHeight;
            ViewData["imageWidth"] = tenantAttachmentSettings.InlinedImageWidth;
            return View();
        }

        /// <summary>
        /// 图片列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        public ActionResult _ListImages(string tenantTypeId, long associateId = 0)
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }

            IEnumerable<Attachment> attachments = null;
            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
            if (associateId == 0)
            {
                attachments = attachementService.GetTemporaryAttachments(user.UserId, tenantTypeId);
            }
            else
            {
                attachments = attachementService.GetsByAssociateId(associateId);
            }

            if (attachments != null && attachments.Count() > 0)
            {
                attachments = attachments.Where(n => n.MediaType == MediaType.Image);
            }

            TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);
            ViewData["inlinedImageWidth"] = tenantAttachmentSettings.InlinedImageWidth;
            ViewData["inlinedImageHeight"] = tenantAttachmentSettings.InlinedImageHeight;

            return View(attachments);
        }

        #endregion 图片管理器

        #region 附件上传

        /// <summary>
        /// 统一的附件上传
        /// </summary>
        [HttpPost]
        public ActionResult UploadFile(string tenantTypeId, string requestName, long associateId, bool resize = false)
        {
            IUser user = UserContext.CurrentUser;

            if (user == null)
            {
                return new EmptyResult();
            }

            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
            long userId = user.UserId, ownerId = user.UserId;
            string userDisplayName = user.DisplayName;
            long attachmentId = 0;
            if (Request.Files.Count > 0 && !string.IsNullOrEmpty(Request.Files[requestName].FileName))
            {
                HttpPostedFileBase postFile = Request.Files[requestName];
                string fileName = postFile.FileName;
                TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);

                //图片类型支持：gif,jpg,jpeg,png
                string[] picTypes = { ".gif", ".jpg", ".jpeg", ".png" };
                if (!tenantAttachmentSettings.ValidateFileExtensions(fileName) && !picTypes.Contains(fileName.Substring(fileName.LastIndexOf('.'))))
                {
                    throw new ExceptionFacade(string.Format("只允许上传后缀名为{0}的文件", tenantAttachmentSettings.AllowedFileExtensions));
                }
                if (!tenantAttachmentSettings.ValidateFileLength(postFile.ContentLength))
                {
                    throw new ExceptionFacade(string.Format("文件大小不允许超过{0}", tenantAttachmentSettings.MaxAttachmentLength));
                }
                string contentType = MimeTypeConfiguration.GetMimeType(postFile.FileName);
                Attachment attachment = new Attachment(postFile, contentType);
                attachment.UserId = userId;
                attachment.AssociateId = associateId;
                attachment.TenantTypeId = tenantTypeId;
                attachment.OwnerId = ownerId;
                attachment.UserDisplayName = userDisplayName;

                using (Stream stream = postFile.InputStream)
                {
                    attachementService.Create(attachment, stream);
                }

                attachmentId = attachment.AttachmentId;
            }
            return Json(new { AttachmentId = attachmentId });
        }

        #endregion 附件上传

        #region 地区



        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <returns></returns>
        public JsonResult GetChildAreas()
        {
            AreaService areaService = new AreaService();
            string parentAreaCode = string.Empty;
            if (Request.QueryString["Id"] != null)
                parentAreaCode = Request.QueryString["Id"].ToString();
            Area area = areaService.Get(parentAreaCode);
            if (area == null)
                return Json(new { }, JsonRequestBehavior.AllowGet);
            return Json(area.Children.Select(n => new { id = n.AreaCode, name = n.Name }), JsonRequestBehavior.AllowGet);
        }

        #endregion 地区

        #region 分类
        /// <summary>
        /// 获取子级分类
        /// </summary>
        /// <returns></returns>
        public JsonResult GetChildCategories(long exceptCategoryId)
        {
            long parentId = 0;
            if (Request.QueryString["Id"] != null)
                long.TryParse(Request.QueryString["Id"].ToString(), out parentId);
            Category category = categoryService.Get(parentId);
            if (category == null)
                return Json(new { }, JsonRequestBehavior.AllowGet);
            return Json(category.Children.Where(n => n.CategoryId != exceptCategoryId).Select(n => new { id = n.CategoryId, name = n.CategoryName }), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 全文检索

        #region 用户搜索

        /// <summary>
        /// 用户搜索页面
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AnonymousBrowseCheck]
        public ActionResult UserSearch(UserFullTextQuery query)
        {
            query.PageSize = 20;//每页记录数

            //调用搜索器进行搜索
            UserSearcher userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            PagingDataSet<User> users = userSearcher.Search(query);
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                //设置当前登录用户对当前页用户的关注情况
                Dictionary<long, bool> isCurrentUserFollowDic = new Dictionary<long, bool>();
                foreach (var user in users)
                {
                    //如果当前登录用户关注了该用户
                    if (followService.IsFollowed(CurrentUser.UserId, user.UserId))
                    {
                        if (!isCurrentUserFollowDic.ContainsKey(user.UserId))
                        {
                            isCurrentUserFollowDic.Add(user.UserId, true);
                        }
                    }
                    else
                    {
                        if (!isCurrentUserFollowDic.ContainsKey(user.UserId))
                        {
                            isCurrentUserFollowDic.Add(user.UserId, false);
                        }
                    }
                }
                ViewData["isCurrentUserFollowDic"] = isCurrentUserFollowDic;
            }

            //添加到用户搜索历史 
            if (CurrentUser != null)
            {
                SearchHistoryService searchHistoryService = new SearchHistoryService();
                searchHistoryService.SearchTerm(CurrentUser.UserId, UserSearcher.CODE, query.Keyword);
            }
            //添加到热词
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(UserSearcher.CODE, query.Keyword);
            }

            //设置页面Meta
            if (string.IsNullOrWhiteSpace(query.Keyword))
            {
                pageResourceManager.InsertTitlePart("用户搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart('“' + query.Keyword + '”' + "的相关用户");//设置页面Title
            }

            pageResourceManager.SetMetaOfKeywords("用户搜索");//设置Keyords类型的Meta
            pageResourceManager.SetMetaOfDescription("用户搜索");//设置Description类型的Meta

            return View(users);
        }

        /// <summary>
        /// 用户全局搜索
        /// </summary>
        /// <param name="query"></param>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        public ActionResult _UserGlobalSearch(UserFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.SearchRange = UserSearchRange.NAME;
            //调用搜索器进行搜索
            UserSearcher userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            PagingDataSet<User> users = userSearcher.Search(query);
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                //设置当前登录用户对当前页用户的关注情况
                Dictionary<long, bool> isCurrentUserFollowDic = new Dictionary<long, bool>();
                foreach (var user in users)
                {
                    //如果当前登录用户关注了该用户
                    if (followService.IsFollowed(CurrentUser.UserId, user.UserId))
                    {
                        isCurrentUserFollowDic.Add(user.UserId, true);
                    }
                    else
                    {
                        isCurrentUserFollowDic.Add(user.UserId, false);
                    }
                }
                ViewData["isCurrentUserFollowDic"] = isCurrentUserFollowDic;
            }

            return PartialView(users);
        }

        /// <summary>
        /// 用户快捷搜索
        /// </summary>
        /// <param name="query"></param>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        public ActionResult _UserQuickSearch(UserFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.SearchRange = UserSearchRange.NAME;

            //调用搜索器进行搜索
            UserSearcher userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            PagingDataSet<User> user = userSearcher.Search(query);

            return PartialView(user);
        }

        /// <summary>
        /// 用户搜索自动完成
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult UserSearchAutoComplete(string keyword)
        {
            UserFullTextQuery query = new UserFullTextQuery();
            query.SearchRange = UserSearchRange.NAME;
            query.Keyword = keyword;//查询关键字
            query.PageSize = 6;//最大记录数

            //调用搜索器进行搜索
            UserSearcher userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            IEnumerable<User> users = userSearcher.AutoCompleteSearch(query);

            var jsonResult = Json(users.Select(user => new { UserId = user.UserId, DisplayName = user.DisplayName, DisplayNameWithHighlight = SearchEngine.Highlight(keyword, user.DisplayName, 100), Introduction = user.Profile == null ? "" : string.Join("", user.Profile.Introduction.Take(30)), AvatarImage = SiteUrls.Instance().UserAvatarUrl(user, AvatarSizeType.Small, true) }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>
        /// 根据地区获取下一级子节点
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAreasByParentCode(string parentCode)
        {
            AreaService areaService = DIContainer.Resolve<AreaService>();
            //获取父节点
            Area parentArea = areaService.Get(parentCode);
            if (parentArea == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            //获取父节点的下一级子节点
            IEnumerable<Area> childrenAreas = parentArea.Children;
            //若没有子节点
            if (childrenAreas.Count() == 0)
            {
                //获取当前节点的父节点名称
                string parenAreaName = areaService.Get(parentArea.ParentCode).Name;
                //构建一个Json数组
                List<object> areaNoChildList = new List<object>();
                areaNoChildList.Add(new { areaCode = parentArea.AreaCode, areaName = parentArea.Name, parentAreaCode = parentArea.ParentCode, parentAreaName = parenAreaName, haveChild = "no" });

                return Json(areaNoChildList, JsonRequestBehavior.AllowGet);
            }
            var jsonResult = Json(childrenAreas.Select(a => new { areaCode = a.AreaCode, areaName = a.Name, parentAreaCode = a.ParentCode, parentAreaName = parentArea.Name, depth = a.Depth, haveChild = "yes" }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion 用户搜索

        #region 全局搜索

        /// <summary>
        /// 全局搜索页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GlobalSearch(string keyword)
        {
            //查询用于自动完成·的搜索器
            IEnumerable<ISearcher> searchersAutoComplete = SearcherFactory.GetQuickSearchers(4);
            ViewData["searchersAutoComplete"] = searchersAutoComplete;
            ViewData["keyword"] = keyword;

            //添加到用户搜索历史 
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                SearchHistoryService searchHistoryService = new SearchHistoryService();
                searchHistoryService.SearchTerm(CurrentUser.UserId, SearcherFactory.GlobalSearchCode, keyword);
            }

            //添加到热词
            if (!string.IsNullOrEmpty(keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(SearcherFactory.GlobalSearchCode, keyword);
            }

            //设置页面Meta
            if (string.IsNullOrWhiteSpace(keyword))
            {
                pageResourceManager.InsertTitlePart("全局搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart('“' + keyword + '”' + "的相关搜索");//设置页面Title
            }

            //查询所有搜索器
            IEnumerable<ISearcher> searchers = SearcherFactory.GetDisplaySearchers();

            pageResourceManager.SetMetaOfKeywords("全局搜索");//设置Keyords类型的Meta
            pageResourceManager.SetMetaOfDescription("全局搜索");//设置Description类型的Meta

            return View(searchers);
        }

        /// <summary>
        /// 全局搜索自动完成
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult GlobalAutoComplete(string keyword, int topNumber)
        {
            IEnumerable<SearchedTerm> searchedAdminTerms = searchedTermService.GetManuals(keyword, SearcherFactory.GlobalSearchCode);
            IEnumerable<SearchedTerm> searchedUserTerms = searchedTermService.GetTops(keyword, topNumber, SearcherFactory.GlobalSearchCode);
            IEnumerable<SearchedTerm> listSearchAdminUserTerms = searchedAdminTerms.Union(searchedUserTerms);
            if (listSearchAdminUserTerms.Count() > topNumber)
            {
                listSearchAdminUserTerms.Take(topNumber);
            }
            IEnumerable<string> terms = listSearchAdminUserTerms.Select(t => t.Term);
            var jsonResult = Json(terms.Select(t => new { tagName = t, tagNameWithHighlight = SearchEngine.Highlight(keyword, string.Join("", t.Take(34)), 100) }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion 全局搜索

        #region 搜索公共页面

        /// <summary>
        /// 快捷搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult _QuickSearch()
        {
            //获取当前是在哪个应用下搜索
            RouteValueDictionary routeValueDictionary = Request.RequestContext.RouteData.DataTokens;
            string areaName = routeValueDictionary.Get<string>("area", null);

            //查询用于快捷搜索的搜索器
            IEnumerable<ISearcher> searchersQuickSearch = SearcherFactory.GetQuickSearchers(100, areaName);
            ViewData["searchersQuickSearch"] = searchersQuickSearch.Take(5);
            IEnumerable<ISearcher> searcherWaterMark = searchersQuickSearch.Where(n => n.Code == areaName + "Searcher");
            if (searcherWaterMark.Count() > 0)
            {
                ViewData["waterMark"] = searcherWaterMark.First().WaterMark;
                ViewData["goRelevantApp"] = true;  //是否跳到相应的搜索页面
            }
            if (string.IsNullOrEmpty(ViewData.Get<string>("waterMark", null)))
            {
                ViewData["waterMark"] = "请输入关键字";
            }
            ViewData["appname"] = areaName;

            return View();
        }


        /// <summary>
        /// 全局搜索页面
        /// </summary>
        /// <returns></returns>
        public ActionResult _SearchItems(string searcherCode)
        {
            //查询所有搜索器
            List<ISearcher> searchers = SearcherFactory.GetDisplaySearchers().ToList();
            ViewData["searcherCode"] = searcherCode;

            return View(searchers);
        }

        /// <summary>
        /// 搜索热词
        /// </summary>
        /// <returns></returns>
        public ActionResult _SearchHotWords(int topNumber, string searcherCode)
        {
            SearchedTermService searchedTermService = new SearchedTermService();
            //获取管理员添加的热词
            IEnumerable<SearchedTerm> searchedAdminTerms = searchedTermService.GetManuals(searcherCode);
            int totalCount = 0; //获取指定总字数的热词 42个
            int indexAdmin = 0;
            if (searchedAdminTerms.Count() > 0)
            {
                if (searchedAdminTerms.First().Term.Length > 42)
                {
                    searchedAdminTerms.ElementAt(0).Term = "";
                }
            }

            foreach (var item in searchedAdminTerms)
            {
                indexAdmin++;
                totalCount += item.Term.Length + 1;
                if (totalCount > 42)
                {
                    indexAdmin--;
                    break;
                }
            }
            if (totalCount > 42)
            {
                return View(searchedAdminTerms.Take(indexAdmin));
            }
            else
            {
                int indexUser = 0;
                //获取用户搜索的热词
                IEnumerable<SearchedTerm> searchedUserTerms = searchedTermService.GetTops(topNumber, searcherCode);
                foreach (var item in searchedUserTerms)
                {
                    indexUser++;
                    totalCount += item.Term.Length + 1;
                    if (totalCount > 42)
                    {
                        indexUser--;
                        break;
                    }
                }
                IEnumerable<SearchedTerm> listSearchAdminUserTerms = searchedAdminTerms.Take(indexAdmin).Union(searchedUserTerms.Take(indexUser));
                return View(listSearchAdminUserTerms);
            }
        }

        /// <summary>
        /// 搜索历史
        /// </summary>
        /// <param name="searcherCode">搜索器代码</param>
        /// <param name="clear">是否是清空</param>
        /// <returns></returns>
        public ActionResult _SearchHistories(string searcherCode, bool clear = false)
        {
            SearchHistoryService searchHistoryService = new SearchHistoryService();
            IUser CurrentUser = UserContext.CurrentUser; //获取当前登录用户
            if (CurrentUser == null)
            {
                return View();
            }

            //清空搜索历史
            if (clear)
            {
                searchHistoryService.Clear(CurrentUser.UserId, searcherCode);
                return View();
            }

            //获取搜索历史
            IEnumerable<string> searchHistories = searchHistoryService.Gets(CurrentUser.UserId, searcherCode);
            if (searchHistories == null)
            {
                return View();
            }

            ViewData["searcherCode"] = searcherCode;
            return View(searchHistories.Take(10));
        }

        #endregion 搜索公共页面

        #endregion 全文检索

        #region 用户选择器

        /// <summary>
        /// 获取当前用户的关注列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMyFollowedUsers(int? categoryId)
        {
            long userId = 0;
            if (UserContext.CurrentUser != null)
                userId = UserContext.CurrentUser.UserId;
            
            var followService = new FollowService();
            var followedUsers = followService.GetFollows(userId, categoryId, Follow_SortBy.FollowerCount_Desc);
            IUserService userService = DIContainer.Resolve<IUserService>();
            return Json(userService.GetFullUsers(followedUsers.Select(n => n.FollowedUserId))
                   .Select(n => new
                   {
                       userId = n.UserId,
                       displayName = GetDisplayName(n.DisplayName, GetNoteName(followedUsers, n.UserId)),
                       userName = n.UserName,
                       trueName = n.TrueName,
                       nickName = n.NickName,
                       noteName = GetNoteName(followedUsers, n.UserId),
                       userAvatarUrl = SiteUrls.Instance().UserAvatarUrl(n, AvatarSizeType.Small)
                   }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取displayName
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="noteName"></param>
        /// <returns></returns>
        private string GetDisplayName(string displayName, string noteName)
        {
            if (!string.IsNullOrEmpty(noteName))
                return noteName;
            return displayName;
        }

        /// <summary>
        /// 获取备注名称
        /// </summary>
        /// <param name="followedUsers"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        private string GetNoteName(IEnumerable<FollowEntity> followedUsers, long toUserId)
        {
            FollowEntity followEntity = followedUsers.FirstOrDefault(n => n.FollowedUserId == toUserId);
            if (followEntity != null)
                return followEntity.NoteName;
            return string.Empty;
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        [HttpGet]
        [AnonymousBrowseCheck]
        public JsonResult SearchUsers(UserSelectorSearchScope searchScope)
        {
            string term = Request.QueryString.GetString("q", string.Empty);
            int topNumber = 8;
            if (string.IsNullOrEmpty(term))
                return Json(new { }, JsonRequestBehavior.AllowGet);
            term = WebUtility.UrlDecode(term);
            term = term.ToLower();
            long userId = 0;
            if (UserContext.CurrentUser != null)
                userId = UserContext.CurrentUser.UserId;
            IEnumerable<User> users = null;
            var followService = new FollowService();
            

            var followedUsers = followService.GetFollows(userId, null, Follow_SortBy.FollowerCount_Desc);
            if (searchScope == UserSelectorSearchScope.FollowedUser)
            {
                IUserService userService = DIContainer.Resolve<IUserService>();
                users = userService.GetFullUsers(followedUsers.Select(n => n.FollowedUserId))
                       .Where(n => n.TrueName.ToLower().Contains(term)
                                   || n.NickName.ToLower().Contains(term)
                                   || GetNoteName(followedUsers, n.UserId).ToLower().Contains(term)
                                   || n.UserName.ToLower().Contains(term))
                                   .Take(topNumber);
            }
            else
            {

                UserSearcher userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
                UserFullTextQuery query = new UserFullTextQuery();
                query.Keyword = term;
                query.PageIndex = 1;
                query.PageSize = topNumber;
                users = userSearcher.Search(query);
            }
            return Json(users
                   .Select(n => new
                   {
                       userId = n.UserId,
                       displayName = n.DisplayName,
                       userName = n.UserName,
                       trueName = n.TrueName,
                       nickName = n.NickName,
                       noteName = GetNoteName(followedUsers, n.UserId),
                       userAvatarUrl = SiteUrls.Instance().UserAvatarUrl(n, AvatarSizeType.Small)
                   }), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 标签

        /// <summary>
        /// 从全文检索中搜索标签并返回json
        /// </summary>
        public JsonResult GetSuggestTags(string tenantTypeId, long? ownerId = null)
        {
            //标签名称过滤条件
            string tagNameFilter = Request.QueryString.GetString("q", string.Empty);

            if (string.IsNullOrEmpty(tagNameFilter))
                return Json(new { }, JsonRequestBehavior.AllowGet);

            tagNameFilter = WebUtility.UrlDecode(tagNameFilter);

            //最大返回记录数
            int maxRecords = 10;

            //调用搜索器进行搜索
            TagSearcher tagSearcher = (TagSearcher)SearcherFactory.GetSearcher(TagSearcher.CODE);
            tagSearcher.tenantTypeId = tenantTypeId;
            IEnumerable<string> tagNames = null;
            if (ownerId.HasValue && ownerId.Value > 0)
            {
                IEnumerable<string> tagInOwners = tagSearcher.Search(tagNameFilter, maxRecords, ownerId.Value);
                if (tagInOwners != null && tagInOwners.Count() > 0)
                {
                    tagNames = tagInOwners;
                }
            }
            if (tagNames == null)
                tagNames = new List<string>();
            if (tagNames.Count() < maxRecords)
            {
                maxRecords = maxRecords - tagNames.Count();
                tagNames = tagNames.Union(tagSearcher.Search(tagNameFilter, maxRecords, 0));
            }
            return Json(tagNames.Select(n => new { value = n }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重建索引(待删除)
        /// </summary>
        /// <returns></returns>
        public ActionResult RebuildIndexTag()
        {
            TagSearcher tagSearcher = (TagSearcher)SearcherFactory.GetSearcher(TagSearcher.CODE);
            tagSearcher.RebuildIndex();
            return Json(tagSearcher, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取标签云
        /// </summary>_CommentList
        /// <param name="tenantTypeIds">租户类型Id</param>
        /// <param name="num">显示的标签数量</param>
        /// <returns></returns>
        public ActionResult _TagCloud(string tenantTypeId = "", int num = 20, bool isBlank = false)
        {
            TagService tagService = new TagService(tenantTypeId);
            Dictionary<Tag, int> tags = tagService.GetTopTags(num, null);

            ViewData["tenantTypeId"] = tenantTypeId;
            ViewData["isBlank"] = isBlank;
            return View(tags);
        }


        /// <summary>
        /// 验证标签名的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateTagName(string tagName, long tagId = 0)
        {
            bool result = false;
            if (tagId > 0)
            {
                result = true;
            }
            else
            {
                TagService tagService = new TagService(TenantTypeIds.Instance().Tag());
                Tag tag = tagService.Get(tagName);
                if (tag != null)
                {
                    return Json("该内容已存在", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = true;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 用户隐私设置器
        /// <summary>
        /// 用户隐私设置器
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="itemName">隐私项表单名称</param>
        /// <param name="itemPrivacyStatus">隐私状态</param>
        /// <param name="selectedUserIds">选中的用户id列表</param>
        /// <param name="selectedUserGroupIds">选中的分组列表</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _PrivacyStatusSetter(long userId, string itemName, PrivacyStatus itemPrivacyStatus, string selectedUserIds = null, string selectedUserGroupIds = null)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "所有人可见", Value = PrivacyStatus.Public.ToString()},
                new SelectListItem{ Text = "部分人可见", Value = PrivacyStatus.Part.ToString()},
                new SelectListItem{ Text = "仅自己可见", Value = PrivacyStatus.Private.ToString()}
            };

            List<SelectListItem> followSelectListItems = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "所有人可见", Value = PrivacyStatus.Public.ToString()},
                new SelectListItem{ Text = "仅自己可见", Value = PrivacyStatus.Private.ToString()}
            };
            if (itemName != PrivacyItemKeys.Instance().Follow())
            {
                ViewData[itemName] = new SelectList(selectListItems, "Value", "Text", itemPrivacyStatus);
            }
            else
            {
                if (itemPrivacyStatus == PrivacyStatus.Part)
                    itemPrivacyStatus = PrivacyStatus.Public;
                ViewData[itemName] = new SelectList(followSelectListItems, "Value", "Text", itemPrivacyStatus);
            }
            Dictionary<int, Dictionary<long, string>> userPrivacySpecifyObjects = new Dictionary<int, Dictionary<long, string>>();
            List<long> userIds = new List<long>();
            List<long> groupIds = new List<long>();
            string[] splitGroupIds = new string[0];
            if (!string.IsNullOrEmpty(selectedUserGroupIds))
                splitGroupIds = selectedUserGroupIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            groupIds.AddRange(splitGroupIds.Select(n => { long id = 0; long.TryParse(n, out id); return id; }).Where(n => n != 0));
            string[] splitUserIds = new string[0];
            if (!string.IsNullOrEmpty(selectedUserIds))
                splitUserIds = selectedUserIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            userIds.AddRange(splitUserIds.Select(n => { long id = 0; long.TryParse(n, out id); return id; }).Where(n => n > 0));

            if (splitGroupIds.Length > 0 || splitUserIds.Length > 0)
                itemPrivacyStatus = PrivacyStatus.Part;

            IUserService userService = DIContainer.Resolve<IUserService>();
            IEnumerable<User> users = userService.GetFullUsers(userIds);
            if (users.Count() > 0)
                userPrivacySpecifyObjects[SpecifyObjectTypeIds.Instance().User()] = users.ToDictionary(k => k.UserId, v => v.DisplayName);

            if (!groupIds.Contains(FollowSpecifyGroupIds.All) && itemPrivacyStatus == PrivacyStatus.Part)
            {
                Dictionary<long, string> categary = new Dictionary<long, string>();
                foreach (long categoryId in groupIds.Distinct())
                {
                    if (categoryId == FollowSpecifyGroupIds.Mutual)
                    {
                        categary.Add(categoryId, "相互关注");
                        continue;
                    }
                    Category category = categoryService.Get(categoryId);
                    if (category != null)
                        categary.Add(category.CategoryId, category.CategoryName);
                }
                if (categary.Count > 0)
                    userPrivacySpecifyObjects[SpecifyObjectTypeIds.Instance().UserGroup()] = categary;
            }
            else
            {
                Dictionary<long, string> categary = new Dictionary<long, string>();
                categary.Add(FollowSpecifyGroupIds.All, "我关注的所有人");
                userPrivacySpecifyObjects[SpecifyObjectTypeIds.Instance().UserGroup()] = categary;
            }
            ViewData["userPrivacySpecifyObjects"] = userPrivacySpecifyObjects;
            ViewData["itemName"] = itemName;
            ViewData["userId"] = userId;
            ViewData["selectedUserIds"] = selectedUserIds;
            ViewData["selectedUserGroupIds"] = selectedUserGroupIds;

            return View("_PrivacyStatusSetter");
        }

        /// <summary>
        /// 显示自定义隐私设置模式框
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="itemKey">隐私项</param>
        /// <param name="selectUserIds">选中用户的ID列表</param>
        /// <param name="selectUserGroupIds">选中用户的ID列表</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _PrivacySpecifyObjectSelector(long userId, string itemName, string selectUserIds, string selectUserGroupIds)
        {
            
            //Dictionary<int, IEnumerable<UserPrivacySpecifyObject>> userPrivacySpecifyObjects = new PrivacyService().GetUserPrivacySpecifyObjects(userId, itemKey);
            Dictionary<int, IEnumerable<long>> userPrivacySpecifyObjectIds = new Dictionary<int, IEnumerable<long>>();
            //if (!string.IsNullOrEmpty(selectUserIds) || !string.IsNullOrEmpty(selectUserGroupIds))
            //{

            if (!string.IsNullOrEmpty(selectUserIds))
            {
                List<long> privacySpecifyUserIds = new List<long>();
                string[] userIds = selectUserIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in userIds)
                {
                    long Id = 0;
                    if (long.TryParse(item, out Id))
                        privacySpecifyUserIds.Add(Id);
                }
                userPrivacySpecifyObjectIds.Add(SpecifyObjectTypeIds.Instance().User(), privacySpecifyUserIds);
            }

            if (!string.IsNullOrEmpty(selectUserGroupIds))
            {

                List<long> privacySpecifyUserGroupIds = new List<long>();
                string[] userGroupIds = selectUserGroupIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in userGroupIds)
                {
                    long Id = 0;
                    if (long.TryParse(item, out Id))
                        privacySpecifyUserGroupIds.Add(Id);
                }
                userPrivacySpecifyObjectIds.Add(SpecifyObjectTypeIds.Instance().UserGroup(), privacySpecifyUserGroupIds);
            }
            //}
            //else
            //{
            //    foreach (var userPrivacySpecifyObject in userPrivacySpecifyObjects)
            //    {
            //        List<long> ids = new List<long>();
            //        foreach (var item in userPrivacySpecifyObject.Value)
            //            ids.Add(item.SpecifyObjectId);
            //        userPrivacySpecifyObjectIds.Add(userPrivacySpecifyObject.Key, ids);
            //    }
            //}
            IPrivacySettingsManager privacySettingsManager = DIContainer.Resolve<IPrivacySettingsManager>();
            ViewData["specifyUserMaxCount"] = privacySettingsManager.Get().SpecifyUserMaxCount;
            ViewData["userPrivacySpecifyObjects"] = userPrivacySpecifyObjectIds;
            ViewData["categories"] = new CategoryService().GetOwnerCategories(userId, TenantTypeIds.Instance().User());
            ViewData["userId"] = userId;
            ViewData["itemName"] = itemName;
            return View();
        }

        /// <summary>
        /// 显示自定义隐私设置模式框
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _PrivacySpecifyObjectSelectorPost(long userId, string itemName)
        {
            
            string selectedUserIds = string.Empty;
            if (Request.Form.Get<bool>(itemName + "_SpecifyUser", false))
                selectedUserIds = Request.Form.Get<string>(itemName + SpecifyObjectTypeIds.Instance().User());
            string selectedUserGroupIds = string.Empty;
            if (Request.Form.Get<bool>(itemName + "_SpecifyUserGroup", false))
                if (Request.Form.Get<long>(itemName + "_IsSpecifyAllUser", 0) == FollowSpecifyGroupIds.All)
                    selectedUserGroupIds = "-1";
                else
                    selectedUserGroupIds = Request.Form.Get<string>(itemName + SpecifyObjectTypeIds.Instance().UserGroup());

            return _PrivacyStatusSetter(userId, itemName, PrivacyStatus.Part, selectedUserIds, selectedUserGroupIds);
        }
        #endregion

        #region 网址解析器

        /// <summary>
        /// 网址解析器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public ActionResult ParseMedia(MediaType mediaType, string url = null)
        {

            if (UserContext.CurrentUser == null)
                return Json(new { errorMessage = "您没有登录" }, JsonRequestBehavior.AllowGet);
            if (string.IsNullOrEmpty(url))
                return Json(new { errorMessage = "url不能为空" }, JsonRequestBehavior.AllowGet);

            if (!Utility.IsAllowableReferrer(Request))
                return Json(new { errorMessage = "站点不支持跨域访问" }, JsonRequestBehavior.AllowGet);
            List<string> keys = GetUrlParserKeys(mediaType);
            string key = keys.FirstOrDefault(n => url.ToLower().Contains(n.ToLower()));
            //若已经在短网址中存在，则直接返回结果
            ShortUrlService shortUrlService = new ShortUrlService();
            bool urlExists = false;
            string alias = shortUrlService.GetUrlAlias(url, out urlExists);
            //string alias = IdGenerator.Next().ToString();
            ParsedMediaService parsedMediaService = new ParsedMediaService();
            ParsedMedia entity = parsedMediaService.Get(alias);
            if (entity != null)
                return Json(entity, JsonRequestBehavior.AllowGet);

            if (string.IsNullOrEmpty(key))
                return View("~/Plugins/MediaParsers/" + mediaType.ToString() + "/Default.cshtml");

            return View("~/Plugins/MediaParsers/" + mediaType.ToString() + "/" + key + ".cshtml");
        }

        /// <summary>
        /// 获取所有url解析器的Key
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        private List<string> GetUrlParserKeys(MediaType mediaType)
        {
            string cacheKey = "UrlParserKeys-" + (int)mediaType;
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            List<string> list = cacheService.Get<List<string>>(cacheKey);
            if (list == null)
            {
                string systemPath = WebUtility.GetPhysicalFilePath(WebUtility.ResolveUrl("~/Plugins/MediaParsers/" + mediaType.ToString() + "/"));
                string[] fileNames = Directory.GetFiles(systemPath);
                list = fileNames.Select(n => n.Remove(n.IndexOf(".cshtml")).Remove(0, systemPath.Length)).Where(n => !n.Equals("Default", StringComparison.CurrentCultureIgnoreCase)).ToList();
                cacheService.Add(cacheKey, list, CachingExpirationType.Stable);
            }
            return list;
        }

        #endregion

        #region 评论


        /// <summary>
        /// 根据评论的id异步加载一条评论
        /// </summary>
        /// <param name="id">评论的id</param>
        /// <returns>根据评论id异步加载一条评论</returns>
        public ActionResult _OneComment(long id)
        {
            Comment comment = commentService.Get(id);
            


            


            if (comment == null)
                return Content(string.Empty);


            if (!new Authorizer().Comment_Show(comment))
                return Content(string.Empty);

            return View(comment);
        }

        /// <summary>
        /// 留言局部页面
        /// </summary>
        /// <param name="commentedObjectId">评论对象id</param>
        /// <param name="originalAuthor">原作者名称（如果是转发的微博的情况添加此参数）</param>
        /// <param name="ownerId">被评论对象的拥有者Id</param>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns>留言布局页面</returns>
        [HttpGet]
        public ActionResult _Comment(long commentedObjectId, long ownerId, string tenantTypeId, long toUserId = 0, long? commentId = null, bool enableComment = true, string subject = null, string commentClass = null)
        {
            CommentEditModel commentModel = new CommentEditModel
            {
                ToUserId = toUserId,
                CommentedObjectId = commentedObjectId,
                OwnerId = ownerId,
                TenantTypeId = tenantTypeId,
                Subject = subject
            };

            if (commentId.HasValue)
            {
                long parentId = commentId.Value;
                Comment comment = commentService.Get(commentId.Value);
                if (comment != null)
                {
                    if (comment.ParentId != 0)
                        parentId = comment.ParentId;
                    ViewData["CommentId"] = parentId;
                    ViewData["CommentIndex"] = commentService.GetPageIndexForCommentInCommens(parentId, tenantTypeId, commentedObjectId, SortBy_Comment.DateCreated);
                    if (comment.ParentId != 0)
                        ViewData["ChildIndex"] = commentService.GetPageIndexForCommentInParentCommens(comment.Id, comment.ParentId, SortBy_Comment.DateCreatedDesc);
                }
            }

            ViewData["tenantType"] = tenantTypeId;
            ViewData["enableComment"] = enableComment;
            ViewData["CommentClass"] = commentClass;

            return View(commentModel);
        }

        /// <summary>
        /// 创建一条新的评论
        /// </summary>
        /// <returns>返回处理之后的id</returns>
        [HttpPost]
        public ActionResult Comment(CommentEditModel commentModel)
        {
            

            if (commentModel.ToUserId == 0 && commentModel.ParentId != 0)
            {
                var parentComment = commentService.Get(commentModel.ParentId);
                commentModel.ToUserId = parentComment.OwnerId;
            }
            if (commentModel.IsValidate && new Authorizer().Comment_Create(commentModel.TenantTypeId, commentModel.ToUserId))
            {
                TenantCommentSettings settings = TenantCommentSettings.GetRegisteredSettings(commentModel.TenantTypeId);
                if (!settings.EnablePrivate)
                    commentModel.IsPrivate = false;

                if (!settings.EnableComment)
                {
                    WebUtility.SetStatusCodeForError(Response);
                    return Json(new StatusMessageData(StatusMessageType.Error, "该应用没有启用评论！"));
                }

                Comment comment = commentModel.AsComment();
                if (comment.ParentId != 0)
                {
                    Comment parentComment = commentService.Get(comment.ParentId);
                    if (parentComment != null)
                        comment.IsPrivate = parentComment.IsPrivate ? true : comment.IsPrivate;
                    comment.ToUserId = parentComment.UserId;
                    comment.ToUserDisplayName = comment.Author;
                }

                if (HtmlUtility.TrimHtml(comment.Body, TextLengthSettings.TEXT_BODY_MAXLENGTH).Length > settings.MaxCommentLength)
                {
                    WebUtility.SetStatusCodeForError(Response);
                    return Json(new StatusMessageData(StatusMessageType.Error, "内容超过了允许录入的长度！"));
                }

                if (commentService.Create(comment))
                    return Json(new { commentId = comment.Id, parentId = comment.ParentId });
            }
            WebUtility.SetStatusCodeForError(Response);
            return Json(new StatusMessageData(StatusMessageType.Error, "创建留言失败了！"));
        }

        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="tenantType">评论的租户类型</param>
        /// <param name="commentedObjectId">被评论对象id</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="childCommentLink">子级评论的链接</param>
        /// <returns>评论列表</returns>
        [HttpGet]
        public ActionResult _CommentList(string tenantType, long commentedObjectId, SortBy_Comment sortBy = SortBy_Comment.DateCreated, int pageIndex = 1, string childCommentLink = null, bool showBefor = true, bool showAfter = false, long? commentId = null, int? childIndex = null, bool enableComment = true)
        {
            ViewData["enableComment"] = enableComment;

            ViewData["ChildCommentLink"] = childCommentLink;
            ViewData["commentedObjectId"] = commentedObjectId;
            ViewData["ShowAfter"] = showAfter;
            ViewData["ShowBefor"] = showBefor;
            ViewData["CommentId"] = commentId;
            ViewData["ChildIndex"] = childIndex;
            ViewData["tenantType"] = tenantType;
            return View(commentService.GetRootComments(tenantType, commentedObjectId, pageIndex, sortBy));
        }

        /// <summary>
        /// 子级评论（首先加载评论控件。异步添加评论）
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>子级评论</returns>
        [HttpGet]
        public ActionResult _ChildComment(long parentId, bool showBefor = true, bool showAfter = false, int pageIndex = 1, bool enableComment = true)
        {
            ViewData["enableComment"] = enableComment;

            Comment comment = commentService.Get(parentId);
            

            if (comment == null)
                return Content(string.Empty);
            ViewData["ShowBefor"] = showBefor;
            ViewData["ShowAfter"] = showAfter;
            ViewData["PageIndex"] = pageIndex;

            ViewData["TenantTypeId"] = comment.TenantTypeId;

            return View(comment.AsEditModel());
        }

        

        /// <summary>
        /// 获取子级评论列表
        /// </summary>
        /// <param name="parentId">父级回复的id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns>子级评论列表</returns>
        [HttpGet]
        public ActionResult _ChildCommentList(long parentId, int pageIndex = 1, SortBy_Comment sortBy = SortBy_Comment.DateCreatedDesc, bool showBefor = true, bool showAfter = false, bool enableComment = true)
        {
            Comment comment = commentService.Get(parentId);
            

            if (comment == null)
                return Content(string.Empty);

            ViewData["enableComment"] = enableComment;

            ViewData["ShowBefor"] = showBefor;
            ViewData["ShowAfter"] = showAfter;
            ViewData["parentId"] = comment.Id;
            ViewData["commentedObjectId"] = comment.CommentedObjectId;
            ViewData["TenantTypeId"] = comment.TenantTypeId;
            return View(commentService.GetChildren(parentId, pageIndex, sortBy));
        }

        /// <summary>
        /// （通用）删除评论
        /// </summary>
        /// <param name="commentId">评论id</param>
        /// <returns>是否成功删除的json数据</returns>
        [HttpPost]
        public ActionResult _DeleteComment(long commentId)
        {
            Comment comment = commentService.Get(commentId);
            if (comment == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到对应的评论"));
            if (new Authorizer().Comment_Delete(comment))
            {
                if (commentService.Delete(commentId))
                    return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败了"));
            }
            return Json(new StatusMessageData(StatusMessageType.Error, "没有权限删除此评论"));
        }

        /// <summary>
        /// 返回指定用户的头像连接
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="avatarSizeType">头像的大小</param>
        /// <param name="enableClientCaching">是否启用缓存</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _UserAvatarLink(AvatarSizeType avatarSizeType = AvatarSizeType.Small, bool enableClientCaching = false)
        {
            string imgPath = SiteUrls.Instance().UserAvatarUrl(UserContext.CurrentUser, AvatarSizeType.Small, enableClientCaching);
            return Json(new StatusMessageData(StatusMessageType.Success, imgPath), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region @用户
        /// <summary>
        /// 获取@用户提醒的后台数据
        /// </summary>
        [HttpGet]
        public JsonResult _AtRemindUser()
        {
            int topNumber = 500;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                return null;
            }

            var followedUsers = followService.GetFollows(currentUser.UserId, null, Follow_SortBy.FollowerCount_Desc);
            return Json(userService.GetFullUsers(followedUsers.Select(n => n.FollowedUserId).Take(topNumber))
                   .Select(n => new
                   {
                       nickName = n.NickName,
                       noteName = GetNoteName(followedUsers, n.UserId)
                   }), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 顶踩

        /// <summary>
        /// 顶踩单击事件
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="objectId">顶踩的对象id</param>
        /// <param name="userId">被顶踩的对象的UserId，用于限制不能顶踩自己</param>
        /// <param name="mode">单项操作还是双向操作</param>
        /// <param name="operation">顶踩的方向，true为顶，false为踩</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _SupportOppose(string tenantTypeId, long objectId, long userId, AttitudeMode mode, bool operation)
        {
            IUser currentUser = UserContext.CurrentUser;

            //未登录用户不能操作
            if (currentUser == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "未登录用户不能操作！"));
            }

            //当前用户不能顶踩自己
            if (currentUser.UserId == userId)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "不能顶踩自己！"));
            }

            AttitudeService attitudeService = new AttitudeService(tenantTypeId, mode);
            AttitudeSettings attitudeSettings = DIContainer.Resolve<IAttitudeSettingsManager>().Get();

            //操作结果
            bool success = false;
            bool? isSupport = attitudeService.IsSupport(objectId, currentUser.UserId);

            if (mode == AttitudeMode.Bidirection)
            {
                //对于双向操作，已顶踩过的不能再进行同方向操作
                if (isSupport.HasValue && isSupport.Value == operation)
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "不能进行重复操作！"));
                }

                //对于双向操作，如果不可修改，也不可以进行反方向操作
                if (!attitudeSettings.IsModify && isSupport.HasValue && isSupport.Value != operation)
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "不能修改原有的操作！"));
                }

                if (operation)
                {
                    success = attitudeService.Support(objectId, currentUser.UserId);
                }
                else
                {
                    success = attitudeService.Oppose(objectId, currentUser.UserId);
                }
            }
            else
            {
                //对于单向操作，如果已经操作过，并且不可取消，则直接返回错误提示
                if (isSupport.HasValue && !attitudeSettings.EnableCancel)
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "不能取消原有的操作！"));
                }

                if (!isSupport.HasValue || !isSupport.Value)
                {
                    success = attitudeService.Support(objectId, currentUser.UserId);
                }
                else
                {
                    success = attitudeService.Oppose(objectId, currentUser.UserId);
                }
            }

            //返回操作结果
            if (success)
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "操作失败，请稍后再试！"));
            }
        }

        #endregion

        #region 动态操作

        /// <summary>
        /// 在动态收件箱中隐藏动态
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteActivityFromSiteInbox(long activityId)
        {
            Activity activity = activityService.Get(activityId);
            if (activity == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到动态"));
            if (!new Authorizer().IsAdministrator(activity.ApplicationId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有删除站点动态的权限"));
            activityService.DeleteFromSiteInbox(activityId);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }


        /// <summary>
        /// 在动态收件箱中隐藏动态
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteActivityFromUserInbox(long activityId)
        {
            Activity activity = activityService.Get(activityId);
            if (activity == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到动态"));
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您还没有登录"));
            activityService.DeleteFromUserInbox(currentUser.UserId, activityId);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        /// <summary>
        /// 屏蔽用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BlockUser(long userId)
        {
            User blockedUser = userService.GetFullUser(userId);
            if (blockedUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到被屏蔽用户"));
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您还没有登录"));
            new UserBlockService().BlockUser(currentUser.UserId, blockedUser.UserId, blockedUser.DisplayName);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        #endregion

        #region 自定义皮肤
        /// <summary>
        /// 上传背景图
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public ActionResult _UploadBackgroundImage(string presentAreaKey, long ownerId)
        {
            //bool isTimeout = false;
            //long currentUserId = Utility.DecryptTokenForUploadfile(Request.QueryString.GetString("userId", string.Empty), out isTimeout);
            //UserService userService = new UserService();
            //IUser user = userService.GetUser(currentUserId);


            //IUser currentUser = UserContext.CurrentUser;
            //if (currentUser == null)
            //{
            //    return Redirect(SiteUrls.Instance().Login());
            //}
            Stream stream = null;
            string imgname = "";
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase postFile = Request.Files[0];
                imgname = postFile.FileName;
                stream = postFile.InputStream;
            }
            new CustomStyleService().UploadBackgroundImage(presentAreaKey, ownerId, stream);

            return Json(new { Data = new { imgname = "imgname" } });

        }

        /// <summary>
        /// 皮肤设置
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <param name="enableCustomStyle"></param>
        /// <param name="isUseCustomStyle"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ThemeSettings(string presentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace, long ownerId = 0, bool enableCustomStyle = false, bool isUseCustomStyle = false)
        {
            if (!ThemeService.Validate(presentAreaKey, ownerId))
                return Content("没有设置皮肤的权限");
            ViewData["selectedThemeAppearance"] = ThemeService.GetThemeAppearance(presentAreaKey, ownerId);
            IEnumerable<ThemeAppearance> appearances = new ThemeService().GetThemeAppearances(presentAreaKey, true);
            return View(appearances);
        }

        /// <summary>
        /// 皮肤设置
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ChangeThemeAppearance(string presentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace, long ownerId = 0)
        {
            if (!ThemeService.Validate(presentAreaKey, ownerId))
                return Json(new StatusMessageData(StatusMessageType.Error, "没有设置皮肤的权限"));

            string themeAppearance = Request.Form.GetString("themeAppearance", string.Empty);
            if (string.IsNullOrEmpty(themeAppearance))
                return Json(new StatusMessageData(StatusMessageType.Error, "请先选择皮肤"));
            ThemeService.ChangeThemeAppearance(presentAreaKey, ownerId, false, themeAppearance);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        /// <summary>
        /// 自定义皮肤设置
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <param name="isUseCustomStyle"></param>
        /// <returns></returns>
        public ActionResult _CustomSettings(string presentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace, long ownerId = 0, bool isUseCustomStyle = false)
        {
            if (!ThemeService.Validate(presentAreaKey, ownerId))
                return Content("没有设置皮肤的权限");

            var customStyleEntity = new CustomStyleService().Get(presentAreaKey, ownerId);
            CustomStyle customStyle = null;
            if (customStyleEntity != null)
            {
                customStyle = customStyleEntity.CustomStyle;
            }
            //配色方案相关操作
            IEnumerable<CustomStyle> colorSchemes = new CustomStyleService().GetColorSchemes(presentAreaKey);
            ViewData["colorSchemes"] = colorSchemes;

            if (customStyle == null)
            {
                customStyle = CustomStyle.New();
                BackgroundImageStyle backgroundImageStyle = new BackgroundImageStyle();
                customStyle.BackgroundImageStyle = backgroundImageStyle;
                Dictionary<string, string> definedColours = null;
                if (colorSchemes.Count() > 0)
                    definedColours = colorSchemes.First().DefinedColours;
                else
                {
                    definedColours = new Dictionary<string, string>();
                    definedColours[ColorLabel.PageBackground.ToString()] = "#f2e3bf";
                    definedColours[ColorLabel.ContentBackground.ToString()] = "#f8f0e6";
                    definedColours[ColorLabel.BorderBackground.ToString()] = "#ebe6d9";
                    definedColours[ColorLabel.MainTextColor.ToString()] = "#666";
                    definedColours[ColorLabel.SubTextColor.ToString()] = "#ccc";
                    definedColours[ColorLabel.MainLinkColor.ToString()] = "#cc6673";
                    definedColours[ColorLabel.SubLinkColor.ToString()] = "#efc0ca";
                }
                customStyle.DefinedColours = definedColours;
            }

            Dictionary<string, object> customStyleCssBlock = new Dictionary<string, object>();
            if (customStyle.IsUseBackgroundImage)
            {
                customStyleCssBlock.TryAdd("background-image", "url('" + customStyle.BackgroundImageStyle.Url + "')");
                customStyleCssBlock.TryAdd("background-repeat", customStyle.BackgroundImageStyle.IsRepeat ? "repeat" : "no-repeat");
                customStyleCssBlock.TryAdd("background-attachment", customStyle.BackgroundImageStyle.IsFix ? "fixed" : "scroll");
                string position = "center";
                switch (customStyle.BackgroundImageStyle.BackgroundPosition)
                {
                    case BackgroundPosition.Left:
                        position = "left";
                        break;
                    case BackgroundPosition.Center:
                        position = "center";
                        break;
                    case BackgroundPosition.Right:
                        position = "right";
                        break;
                    default:
                        position = "center";
                        break;
                }
                customStyleCssBlock.TryAdd("background-position", position + " top");
            }

            ViewData["customStyleCssBlock"] = System.Web.Helpers.Json.Encode(customStyleCssBlock);
            List<SelectListItem> selectHeaderHeight = new List<SelectListItem> { new SelectListItem { Text = "20", Value = "20" }, new SelectListItem { Text = "60", Value = "60" }, new SelectListItem { Text = "100", Value = "100" } };
            ViewData["HeaderHeight"] = new SelectList(selectHeaderHeight, "Value", "Text", customStyle.HeaderHeight);

            return View(customStyle);
        }

        /// <summary>
        /// 保存自定义皮肤
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <param name="customStyle"></param>
        /// <returns></returns>
        public ActionResult _SaveCustomSettings(string presentAreaKey, long ownerId, CustomStyle customStyle)
        {
            if (!ThemeService.Validate(presentAreaKey, ownerId))
                return Json(new StatusMessageData(StatusMessageType.Error, "没有设置皮肤的权限"));
            new CustomStyleService().Save(presentAreaKey, ownerId, customStyle);
            ThemeService.ChangeThemeAppearance(presentAreaKey, ownerId, true, string.Empty);
            return Json(new StatusMessageData(StatusMessageType.Success, "保存成功!"));
        }

        #endregion

        #region 学校选择器
        /// <summary>
        /// 学校选择器
        /// </summary>
        /// <param name="inputId">模式框ID</param>
        /// <param name="areaCode">地区编码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="schoolType">学习类型</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _SchoolSelector(string inputId, string areaCode = null, string keyword = null, SchoolType? schoolType = null)
        {
            ViewData["inputId"] = inputId;

            int pageSize = string.IsNullOrEmpty(areaCode) ? 100 : 1000;

            PagingDataSet<School> schools = schoolService.Gets(areaCode, keyword, schoolType, pageSize, 1);


            AreaService areaService = new AreaService();

            var area = areaService.Get(areaCode);
            if (area != null)
            {
                while (area.Depth >= 3)
                {
                    area = areaService.Get(area.ParentCode);
                }
                areaCode = area.AreaCode;
            }
            ViewData["areaCode"] = areaCode;
            return View(schools);

        }

        #endregion 学校选择器

        #region 用户卡片

        /// <summary>
        /// 用户卡片
        /// </summary>
        public ActionResult _UserCard(long userId)
        {
            IUser user = userService.GetFullUser(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null)
            {
                bool isStopped = new PrivacyService().IsStopedUser(currentUser.UserId, user.UserId);
                ViewData["isStopped"] = isStopped;
            }

            bool seeMessage = false;
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().Message()))
            {
                seeMessage = true;
            }
            ViewData["seeMessage"] = seeMessage;

            return View(user);
        }

        /// <summary>
        /// 是否显示拉黑标识
        /// </summary>
        [HttpGet]
        public JsonResult _IsStopped(long userId)
        {
            if (UserContext.CurrentUser != null)
            {
                Dictionary<long, StopedUser> stopedUsers = new PrivacyService().GetStopedUsers(UserContext.CurrentUser.UserId);
                if (stopedUsers.Keys.Contains(userId))
                {
                    return Json(new { isStoped = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { isStoped = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加关注用户
        /// </summary>
        public ActionResult _AddFollowUser(long userId)
        {
            ViewData["userId"] = userId;
            return View();
        }

        #endregion

        #region 用户举报

        /// <summary>
        /// 举报用户
        /// </summary>
        /// <param name="userId">举报人Id</param>
        /// <param name="reportedUserId">被举报人Id</param>
        /// <param name="url">举报链接</param>
        /// <param name="title">举报标题</param>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult _ImpeachReport(long userId, long reportedUserId, string url, string title)
        {
            ViewData["url"] = url;
            ViewData["reportedUserId"] = reportedUserId;
            ViewData["title"] = title;
            User user = new UserService().GetFullUser(userId);
            if (user != null)
            {
                ViewData["user"] = user;
            }

            return View();
        }

        /// <summary>
        /// 创建用户举报的Post方法
        /// </summary>
        /// <param name="reportCreateModel"></param>
        /// <returns>表示操作成功或失败的json</returns>
        [CaptchaVerify(VerifyScenarios.Post)]
        [HttpPost]
        public ActionResult _ImpeachReport(ImpeachReportCreateModel reportCreateModel)
        {
            if (reportCreateModel == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "举报失败！"));
            }

            if (!ModelState.IsValid)
            {
                WebUtility.SetStatusCodeForError(Response);
                return View(reportCreateModel);
            }

            if (impeachReportService.Create(reportCreateModel.AsReportEntity()))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "举报成功！"));
            }
            return Json(new StatusMessageData(StatusMessageType.Error, "举报失败！"));

        }

        #endregion

        #region 站点公告

        /// <summary>
        /// 公告列表
        /// </summary>
        /// <param name="pageSize">pageSize</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <returns>公告分页列表</returns>
        [HttpGet]
        public ActionResult AnnouncementList(int pageSize = 20, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("公告管理");
            PagingDataSet<Announcement> announcements = announcementService.Gets(pageSize, pageIndex);
            return View(announcements);
        }

        /// <summary>
        /// 公告详细页
        /// </summary>
        /// <param name="announcementId">公告Id</param>
        /// <returns>公告实体</returns>
        public ActionResult AnnouncementDetail(long announcementId)
        {

            pageResourceManager.InsertTitlePart("公告");

            Announcement announcement = announcementService.Get(announcementId);

            announcement.UserName = new UserService().GetUser(announcement.UserId).DisplayName;

            announcement.IsAdministrator = new Authorizer().IsAdministrator(0);

            CountService countService = new CountService(TenantTypeIds.Instance().Announcement());

            countService.ChangeCount(CountTypes.Instance().HitTimes(), announcementId, announcement.UserId, 1, false);

            return View(announcement);
        }

        /// <summary>
        /// 公告显示的局部页
        /// </summary>
        /// <param name="displayArea">展示区域</param>
        /// <returns>公告列表</returns>
        public ActionResult _AnnouncementItem(string displayArea)
        {

            List<Announcement> newAnnouncements = new List<Announcement>();

            IEnumerable<Announcement> announcements = announcementService.Gets(displayArea).ToList();

            List<string> ids = new List<string>();
            HttpCookie announcementList = Request.Cookies["AnnouncementList"];
            if (announcementList != null)
            {
                string list = announcementList.Value;
                if (!string.IsNullOrEmpty(list))
                {
                    list = list.Replace("%2C", ",").Replace("%3B", ";").Replace("%2F", "/").Replace("%20", " ").Replace("%3A", ":");

                    string[] idsstr = list.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in idsstr)
                    {

                        if (!ids.Contains(item))
                        {
                            ids.Add(item);
                        }
                    }
                }
            }
            foreach (var announcement in announcements)
            {
                string aaa = string.Format("{0};{1}", announcement.Id, announcement.LastModified);
                if (!ids.Contains(aaa))
                {
                    newAnnouncements.Add(announcement);
                }
            }
            ViewData["displayArea"] = displayArea;
            return View(newAnnouncements);
        }

        /// <summary>
        /// 频道主页公告显示列表的局部页
        /// </summary>
        /// <returns>公告列表</returns>
        public ActionResult _AnnouncementList()
        {
            IEnumerable<Announcement> announcements = announcementService.Gets("Home");

            if (announcements.Count() == 0 || announcements == null)
                return null;

            IEnumerable<Announcement> finallyAnnouncements = announcements.Take(10);

            return View(finallyAnnouncements);

        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="id">公告Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAnnouncement(long id)
        {
            if (id != 0)
            {
                announcementService.Delete(id);
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"));
            }

        }

        #endregion

        #region 友情链接
        /// <summary>
        /// 用户/群组空间友情链接列表显示
        /// </summary>
        /// <returns></returns>
        public ActionResult _OwnerLinks(int ownerType, long ownerId, int topNumber = 1000)
        {
            ViewData["ownerType"] = ownerType;
            ViewData["ownerId"] = ownerId;

            IEnumerable<LinkEntity> links = linkService.GetsOfOwner(ownerType, ownerId, topNumber);
            ViewData["manage"] = new Authorizer().Link_Manage(ownerType, ownerId);
            return View(links);
        }

        /// <summary>
        /// 管理用户/群组空间友情链接
        /// </summary>
        /// <returns></returns>
        public ActionResult _ManageLinks(int ownerType, long ownerId)
        {
            ViewData["ownerType"] = ownerType;
            ViewData["ownerId"] = ownerId;

            IEnumerable<LinkEntity> links = linkService.GetsOfOwner(ownerType, ownerId, 1000);
            return View(links);
        }

        /// <summary>
        /// 创建/编辑文字友情链接
        /// </summary>
        /// <param name="linkId">链接标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditTextLink(long linkId = 0)
        {
            LinkEditModel linkEditModel = new LinkEditModel();

            if (linkId > 0)
            {
                LinkEntity link = linkService.Get(linkId);
                if (!new Authorizer().Link_Edit(link))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "对不起，您没有这个权限",
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }));
                }

                //编辑
                linkEditModel = link.AsLinkEditModel();
            }
            return View(linkEditModel);
        }

        /// <summary>
        /// 创建/编辑文字友情链接
        /// </summary>
        /// <param name="linkEditModel">LinkEditModel</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _EditTextLink(LinkEditModel linkEditModel)
        {
            string statusMsg = string.Empty;
            LinkEntity link = linkEditModel.AsLink();

            if (link.LinkId > 0)
            {
                //编辑操作
                if (!new Authorizer().Link_Edit(link))
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有这个权限"));
                }
                linkService.Update(link);

                statusMsg = "编辑成功！";
            }
            else
            {
                //创建操作
                long ownerIdResult = 0;
                int ownerTypeResult = 0;
                long.TryParse(Request.Form.Get("ownerId"), out ownerIdResult);
                int.TryParse(Request.Form.Get("ownerType"), out ownerTypeResult);
                link.OwnerId = ownerIdResult;
                link.OwnerType = ownerTypeResult;

                linkService.Create(link);

                if (link.LinkId > 0)
                {
                    statusMsg = "创建成功！";
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, statusMsg));
        }

        /// <summary>
        /// 创建/编辑图片友情链接
        /// </summary>
        /// <param name="linkId">链接标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditImageLink(long linkId = 0)
        {
            LinkEditModel linkEditModel = new LinkEditModel();

            if (linkId > 0)
            {
                LinkEntity link = linkService.Get(linkId);
                if (!new Authorizer().Link_Edit(link))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "对不起，您没有这个权限",
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }));
                }

                //编辑
                linkEditModel = linkService.Get(linkId).AsLinkEditModel();
            }
            return View(linkEditModel);
        }

        /// <summary>
        /// 添加/编辑图片友情链接
        /// </summary>
        /// <param name="linkEditModel">LinkEditModel</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditImageLink(LinkEditModel linkEditModel)
        {
            LinkEntity link = linkEditModel.AsLink();
            link.LinkType = LinkType.ImageLink;
            ;
            string statusMsg = string.Empty;

            HttpPostedFileBase localImage = Request.Files["localImage"];

            if (link.LinkId > 0)
            {
                //编辑操作操作
                if (!new Authorizer().Link_Edit(link))
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有这个权限"));
                }

                if (localImage != null && !string.IsNullOrEmpty(localImage.FileName))
                {
                    //本地图片
                    link.ImageUrl = logoService.UploadLogo(link.LinkId, localImage.InputStream);
                }

                linkService.Update(link);
                statusMsg = "编辑成功";
            }
            else
            {
                //添加操作
                long ownerIdResult = 0;
                int ownerTypeResult = 0;
                long.TryParse(Request.Form.Get("ownerId"), out ownerIdResult);
                int.TryParse(Request.Form.Get("ownerType"), out ownerTypeResult);
                link.OwnerId = ownerIdResult;
                link.OwnerType = ownerTypeResult;

                linkService.Create(link);
                if (localImage != null)
                {
                    //本地图片
                    link.ImageUrl = logoService.UploadLogo(link.LinkId, localImage.InputStream);
                    linkService.Update(link);
                }
                statusMsg = "添加成功";
            }

            return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Success, statusMsg)));
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="linkId">链接标识</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _DeleteLink(long linkId)
        {
            LinkEntity link = linkService.Get(linkId);
            if (new Authorizer().Link_Delete(link))
            {
                linkService.Delete(link);
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            return Json(new StatusMessageData(StatusMessageType.Error, "您不能删除该链接！"));

        }

        /// <summary>
        /// 批量更改启用状态
        /// </summary>
        /// <param name="linkIds"></param>
        /// <param name="isEnabled"></param>
        [HttpPost]
        public ActionResult _EditLinksStatus(IEnumerable<long> linkIds, bool isEnabled)
        {
            LinkEntity link = new LinkEntity();
            foreach (long linkId in linkIds)
            {
                link = linkService.Get(linkId);

                if (!new Authorizer().Link_Edit(link))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "对不起，您没有这个权限",
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }));
                }

                link.IsEnabled = isEnabled;
                linkService.Update(link);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 站点友情链接展示
        /// </summary>
        /// <returns></returns>
        public ActionResult _SiteLinks()
        {
            Dictionary<long, IEnumerable<LinkEntity>> dic = new Dictionary<long, IEnumerable<LinkEntity>>();
            IEnumerable<Category> categories = categoryService.GetRootCategories(TenantTypeIds.Instance().Link());
            foreach (long categoryId in categories.Select(n => n.CategoryId))
            {
                IEnumerable<LinkEntity> links = linkService.GetsOfSite(categoryId, 1000).Where(n => n.IsEnabled == true);
                dic.Add(categoryId, links);
            }
            ViewData["linksOfCategoryDic"] = dic;
            return View(categories);
        }
        #endregion

        #region 附件列表

        /// <summary>
        /// 附件列表
        /// </summary>
        /// <param name="teantTypeId">租户类型id</param>
        /// <param name="threadId">帖子id</param>
        /// <returns>附件列表</returns>
        public ActionResult _ListAttachement(string teantTypeId, long threadId)
        {
            AttachmentService attachementService = new AttachmentService(teantTypeId);
            ViewData["Attachments"] = attachementService.GetsByAssociateId(threadId);
            return View();
        }

        #endregion

        #region 前台-购买附件

        /// <summary>
        /// 购买附件显示的局部页面
        /// </summary>
        /// <param name="attachementId">准备购买的附件id</param>
        /// <returns>购买附件显示的局部页面</returns>
        [HttpGet]
        public ActionResult _BuyAttachement(string tenantTypeId, long attachementId)
        {







            AttachmentService attachementService = new AttachmentService(tenantTypeId);
            Attachment attachment = attachementService.Get(attachementId);
            if (attachment == null)
                return Content(string.Empty);
            return View(attachment);
        }

        /// <summary>
        /// 附件购买记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _BuyAttachementRecord(int attachementId, int pageIndex = 1)
        {

            return View(attachmentDownloadService.GetsByAttachmentId(attachementId, pageIndex));
        }

        #endregion

        #region 音乐视频

        /// <summary>
        /// 添加音乐
        /// </summary>
        /// <param name="textareaId">textareaId</param>
        [HttpGet]
        public ActionResult _AddMusic(string textareaId)
        {
            return View();
        }

        /// <summary>
        /// 添加视频
        /// </summary>
        /// <param name="textareaId">textareaId</param>
        public ActionResult _AddVideo(string textareaId)
        {
            return View();
        }

        /// <summary>
        /// 音乐详情
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _MusicDetail(string alias)
        {
            if (string.IsNullOrEmpty(alias))
            {
                return new EmptyResult();
            }

            ParsedMedia pm = new ParsedMediaService().Get(alias);
            if (pm == null)
            {
                return new EmptyResult();
            }

            return View(pm);
        }


        /// <summary>
        /// 视频详情
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _VideoDetail(string alias)
        {
            if (string.IsNullOrEmpty(alias))
            {
                return new EmptyResult();
            }

            ParsedMedia pm = new ParsedMediaService().Get(alias);
            if (pm == null)
            {
                return new EmptyResult();
            }

            return View(pm);
        }


        #endregion

        /// <summary>
        /// 关注用户动态
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public ActionResult _FollowUserActivity(long activityId)
        {
            ActivityService activityService = new ActivityService();
            Activity activity = activityService.Get(activityId);

            if (activity == null)
                return Content(string.Empty);

            IUser user = userService.GetUser(activity.OwnerId);
            if (user == null)
                return Content(string.Empty);

            IUser currentUser = UserContext.CurrentUser;
            IEnumerable<long> followedUserIds = followService.GetFollowedUserIds(user.UserId, null, Follow_SortBy.DateCreated_Desc, 1);
            List<User> followedUsers = new List<User>();

            Dictionary<long, bool> isCurrentUserFollowDic = new Dictionary<long, bool>();

            int i = 1;
            foreach (long userId in followedUserIds)
            {
                User tempUser = userService.GetFullUser(userId);
                if (tempUser == null)
                    continue;

                if (i >= 3)
                    break;

                if (followService.IsFollowed(currentUser == null ? 0 : currentUser.UserId, tempUser.UserId))
                {
                    isCurrentUserFollowDic[tempUser.UserId] = true;
                }
                else
                {
                    isCurrentUserFollowDic[tempUser.UserId] = false;
                }

                followedUsers.Add(tempUser);
                i++;
            }


            if (followedUsers.Count == 0)
            {
                return new EmptyResult();
            }

            ViewData["isCurrentUserFollowDic"] = isCurrentUserFollowDic;
            ViewData["FollowUsers"] = followedUsers;
            ViewData["ActivityUserId"] = activity.UserId;

            return View(activity);
        }

        /// <summary>
        /// 短网址重定向
        /// </summary>
        /// <param name="alias">Url别名</param>
        /// <returns></returns>
        public ActionResult RedirectUrl(string alias)
        {
            if (string.IsNullOrEmpty(alias))
                return new EmptyResult();
            ShortUrlEntity entity = new ShortUrlService().Get(alias);
            if (entity == null)
                return new EmptyResult();

            return Redirect(entity.Url);
        }
    }
}
