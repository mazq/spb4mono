using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spacebuilder.Bar.Search;
using Spacebuilder.Common;
using Spacebuilder.Group;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Mvc;
using Tunynet.Search;
using Tunynet.UI;
using Tunynet.Utilities;

namespace Spacebuilder.Bar.Controllers
{
    /// <summary>
    /// 帖吧
    /// </summary>
    [TitleFilter(IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    [AnonymousBrowseCheck]
    public class BarController : Controller
    {
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private BarSectionService barSectionService = new BarSectionService();
        private CategoryService categoryService = new CategoryService();
        private BarThreadService barThreadService = new BarThreadService();
        private BarPostService barPostService = new BarPostService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().BarThread());
        private AttachmentService attachementService = new AttachmentService(TenantTypeIds.Instance().BarThread());
        private AttachmentDownloadService attachmentDownloadService = new AttachmentDownloadService();
        private PointService pointService = new PointService();
        private SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().BarSection());
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private RecommendService recommendService = new RecommendService();
        private GroupService groupService = new GroupService();
        private IdentificationService identificationService = new IdentificationService();
        private BarRatingService barRatingService = new BarRatingService();
        private BarSettings barSettings = DIContainer.Resolve<IBarSettingsManager>().Get();

        /// <summary>
        /// 帖吧首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            pageResourceManager.InsertTitlePart("帖吧首页");
            return View();
        }

        #region 帖吧内容块

        /// <summary>
        /// 推荐帖子
        /// </summary>
        /// <param name="topNum"></param>
        /// <param name="recommendTypeId"></param>
        /// <returns></returns>
        public ActionResult _RecommendThread(int topNum = 8, string recommendTypeId = null)
        {
            IEnumerable<RecommendItem> recommendItems = recommendService.GetTops(8, recommendTypeId);
            return View(recommendItems);
        }

        /// <summary>
        /// 帖吧列表
        /// </summary>
        /// <returns></returns>
        public ActionResult _TopSections(int topNumber = 10, long? categoryId = null, SortBy_BarSection sortBy = SortBy_BarSection.ThreadAndPostCount, DisplayTemplate_TopSections displayTemplate = DisplayTemplate_TopSections.Headline)
        {
            IEnumerable<BarSection> sections = barSectionService.GetTops(topNumber, categoryId, sortBy);
            if (displayTemplate == DisplayTemplate_TopSections.Headline && sections != null && sections.Count() > 0)
            {
                ViewData["TopThreads"] = barThreadService.Gets(sections.First().SectionId, null, null, SortBy_BarThread.LastModified_Desc, 1).Take(2);
            }
            ViewData["SortBy"] = sortBy;
            return View("_TopSections_" + displayTemplate.ToString(), sections);
        }

        /// <summary>
        /// 帖吧列表
        /// </summary>
        /// <returns></returns>
        public ActionResult _RecommendSections(int topNumber = 10, string recommendTypeId = null, DisplayTemplate_RecommendSections displayTemplate = DisplayTemplate_RecommendSections.Summary)
        {
            
            IEnumerable<RecommendItem> recommendSections = recommendService.GetTops(topNumber, recommendTypeId);
            return View("_RecommendSections_" + displayTemplate.ToString(), recommendSections);
        }


        /// <summary>
        /// 帖吧分类内容块
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ActionResult _CategorySections(long? categoryId)
        {
            PagingDataSet<BarSection> pds = barSectionService.Gets(string.Empty, categoryId, SortBy_BarSection.FollowedCount, 1);
            IEnumerable<Category> childCategories = null;
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                var category = categoryService.Get(categoryId.Value);
                if (category != null)
                {
                    if (category.ChildCount == 0) //若是叶子节点，则取同辈分类
                    {
                        if (category.Parent != null)
                            childCategories = category.Parent.Children;
                    }
                    else
                        childCategories = category.Children;
                    List<Category> allParentCategories = new List<Category>();
                    //递归获取所有父级类别，若不是叶子节点，则包含其自身

                    RecursiveGetAllParentCategories(category.Parent, ref allParentCategories);
                    ViewData["allParentCategories"] = allParentCategories;
                    ViewData["currentCategory"] = category;
                }
            }
            if (childCategories == null)
                childCategories = categoryService.GetRootCategories(TenantTypeIds.Instance().BarSection());

            ViewData["childCategories"] = childCategories;
            return View(pds.ToList());
        }

        /// <summary>
        /// 递归获取所有的父分类的集合（包含当前分类）
        /// </summary>
        /// <param name="category">当前类别</param>
        /// <param name="allParentCategories">所有的父类别</param>
        private void RecursiveGetAllParentCategories(Category category, ref List<Category> allParentCategories)
        {
            if (category == null)
                return;
            allParentCategories.Insert(0, category);
            Category parent = category.Parent;
            if (parent != null)
                RecursiveGetAllParentCategories(parent, ref allParentCategories);
        }


        #endregion

        #region 帖吧
        /// <summary>
        /// 创建帖吧
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateSection()
        {
            pageResourceManager.InsertTitlePart("创建帖吧");
            if (!new Authorizer().BarSection_Create())
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "没有创建帖吧的权限！",
                    Title = "没有权限",
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            IBarSettingsManager manager = DIContainer.Resolve<IBarSettingsManager>();
            BarSettings settings = manager.Get();
            ViewData["SectionManagerMaxCount"] = settings.SectionManagerMaxCount;
            return View(new BarSectionEditModel());
        }

        /// <summary>
        /// 创建帖吧
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSection(BarSectionEditModel model)
        {
            if (!new Authorizer().BarSection_Create())
                return HttpNotFound();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpPostedFileBase logoImage = Request.Files["LogoImage"];
            string logoImageFileName = string.Empty;
            Stream stream = null;
            if (logoImage != null && !string.IsNullOrEmpty(logoImage.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().BarSection());
                if (!tenantLogoSettings.ValidateFileLength(logoImage.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return View(model);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                if (!logoSettings.ValidateFileExtensions(logoImage.FileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return View(model);
                }
                stream = logoImage.InputStream;
                logoImageFileName = logoImage.FileName;
            }
            IEnumerable<long> managerUserIds = Request.Form.Gets<long>("ManagerUserIds");

            BarSection barSection = model.AsBarSection();
            barSection.IsEnabled = true;
            barSection.LogoImage = logoImageFileName;
            barSection.UserId = UserContext.CurrentUser.UserId;
            //独立帖吧应用中帖吧拥有者指定为0
            barSection.OwnerId = 0;
            barSectionService.Create(barSection, UserContext.CurrentUser.UserId, managerUserIds, stream);
            if (model.CategoryId > 0)
                categoryService.AddItemsToCategory(new List<long> { barSection.SectionId }, model.CategoryId, 0);

            return Redirect(SiteUrls.Instance().SectionDetail(barSection.SectionId));
        }

        /// <summary>
        /// 帖吧详细显示页
        /// </summary>
        /// <returns></returns>
        [BarSectionAuthorize]
        public ActionResult SectionDetail(long sectionId, long? categoryId = null, bool? isEssential = null, SortBy_BarThread? sortBy = null, int pageIndex = 1)
        {
            BarSection barSection = barSectionService.Get(sectionId);

            if (barSection == null)
                return HttpNotFound();

            //当用户没有权限查看该贴吧的时候。提示较为有好的提示信息
            if (barSection.AuditStatus != AuditStatus.Success && !new Authorizer().BarSection_Manage(barSection))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "没有权限",
                    Body = "此帖吧还未通过审核，不能查看",
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            if (!new Authorizer().BarSection_View(barSection))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, SystemMessageViewModel.NoCompetence()));

            PagingDataSet<BarThread> pds = barThreadService.Gets(sectionId, categoryId, isEssential, sortBy ?? SortBy_BarThread.LastModified_Desc, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_List", pds);
            pageResourceManager.InsertTitlePart(barSection.Name);
            IEnumerable<Category> sectionCategories = categoryService.GetCategoriesOfItem(barSection.SectionId, 0, TenantTypeIds.Instance().BarSection());
            if (sectionCategories != null && sectionCategories.Count() > 0)
            {

                Category currentSectionCategory = sectionCategories.First();
                List<Category> allParentCategories = new List<Category>();
                if (currentSectionCategory.Parent != null)
                    RecursiveGetAllParentCategories(currentSectionCategory.Parent, ref allParentCategories);
                ViewData["allParentSectionCategories"] = allParentCategories;
                ViewData["currentSectionCategory"] = currentSectionCategory;
                ViewData["topSections"] = barSectionService.GetTops(10, currentSectionCategory.CategoryId, SortBy_BarSection.ThreadAndPostCount);
            }
            Category currentThreadCategory = null;
            if (categoryId.HasValue && categoryId.Value > 0)
                currentThreadCategory = categoryService.Get(categoryId.Value);
            if (currentThreadCategory != null)
            {
                ViewData["currentThreadCategory"] = currentThreadCategory;
            }

            ViewData["OnlyFollowerCreateThread"] = barSettings.OnlyFollowerCreateThread;
            if (UserContext.CurrentUser != null)
                ViewData["IsSubscribed"] = subscribeService.IsSubscribed(sectionId, UserContext.CurrentUser.UserId);

            IEnumerable<long> topMemberUserIds = subscribeService.GetTopUserIdsOfObject(sectionId, 15);
            ViewData["topMembers"] = userService.GetFullUsers(topMemberUserIds);
            ViewData["section"] = barSection;
            ViewData["threadCategories"] = categoryService.GetRootCategories(TenantTypeIds.Instance().BarThread(), sectionId);
            ViewData["sortBy"] = sortBy;
            return View(pds);
        }

        /// <summary>
        /// 帖吧成员
        /// </summary>
        /// <returns></returns>
        [BarSectionAuthorize]
        public ActionResult SectionMembers(long sectionId, int pageIndex = 1)
        {
            BarSection barSection = barSectionService.Get(sectionId);

            if (!new Authorizer().BarSection_View(barSection))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, SystemMessageViewModel.NoCompetence()));

            pageResourceManager.InsertTitlePart(barSection.Name);
            pageResourceManager.InsertTitlePart("关注本吧的用户");
            PagingDataSet<long> memberUserIds = subscribeService.GetPagingUserIdsOfObject(sectionId, pageIndex);
            PagingDataSet<User> members = new PagingDataSet<User>(userService.GetFullUsers(memberUserIds));
            members.PageIndex = pageIndex;
            members.PageSize = 50;
            members.QueryDuration = memberUserIds.QueryDuration;
            members.TotalRecords = memberUserIds.TotalRecords;
            ViewData["barSection"] = barSection;
            return View(members);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult DeleteManager(long sectionId, long userId)
        {
            if (UserContext.CurrentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "必须先登录，才能继续操作"));
            BarSection barSection = barSectionService.Get(sectionId);
            if (barSection == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到要被关注的帖吧"));
            if (!new Authorizer().BarSection_SetManager(barSection))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有删除管理员的权限"));
            barSectionService.DeleteManager(sectionId, userId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        /// <summary>
        /// 删除帖吧logo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteBarSectionLogo(long sectionId)
        {
            BarSection barSection = barSectionService.Get(sectionId);
            if (barSection == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有该帖吧！"));
            if (!new Authorizer().BarSection_Manage(barSection))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有删除帖吧Logo的权限"));

            barSectionService.DeleteLogo(barSection.SectionId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除帖吧Logo成功！"));
        }


        /// <summary>
        /// 帖吧成员中我关注的人
        /// </summary>
        /// <returns></returns>
        [BarSectionAuthorize]
        public ActionResult MyFollowedUsers(long sectionId)
        {
            if (UserContext.CurrentUser == null)
                return Redirect(SiteUrls.Instance().Login(true));
            BarSection barSection = barSectionService.Get(sectionId);
            if (barSection == null)
                return HttpNotFound();
            pageResourceManager.InsertTitlePart(barSection.Name);
            pageResourceManager.InsertTitlePart("关注本吧的用户");
            IEnumerable<long> memberUserIds = subscribeService.GetFollowedUserIdsOfObject(sectionId, UserContext.CurrentUser.UserId);
            IEnumerable<User> members = userService.GetFullUsers(memberUserIds);
            ViewData["barSection"] = barSection;
            return View(members);
        }

        /// <summary>
        /// 帖吧列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ListSections(string nameKeyword, long? categoryId, SortBy_BarSection? sortBy, int pageIndex = 1)
        {
            nameKeyword = WebUtility.UrlDecode(nameKeyword);
            IEnumerable<Category> childCategories = null;
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                var category = categoryService.Get(categoryId.Value);
                if (category != null)
                {
                    if (category.ChildCount == 0) //若是叶子节点，则取同辈分类
                    {
                        if (category.Parent != null)
                            childCategories = category.Parent.Children;
                    }
                    else
                        childCategories = category.Children;
                    List<Category> allParentCategories = new List<Category>();
                    //递归获取所有父级类别，若不是叶子节点，则包含其自身
                    RecursiveGetAllParentCategories(category.ChildCount > 0 ? category : category.Parent, ref allParentCategories);
                    ViewData["allParentCategories"] = allParentCategories;
                    ViewData["currentCategory"] = category;

                    pageResourceManager.InsertTitlePart(category.CategoryName);
                }
            }
            else
                pageResourceManager.InsertTitlePart("浏览帖吧");

            if (childCategories == null)
                childCategories = categoryService.GetRootCategories(TenantTypeIds.Instance().BarSection());

            ViewData["childCategories"] = childCategories;
            PagingDataSet<BarSection> pds = barSectionService.Gets(nameKeyword, categoryId, sortBy ?? SortBy_BarSection.ThreadAndPostCount, pageIndex);
            ViewData["sortBy"] = sortBy;
            ViewData["categoryId"] = categoryId;
            return View(pds);
        }

        #endregion

        #region 我的帖吧

        /// <summary>
        /// 用户数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="displayTemplate">用户数据内容块显示模板</param>
        /// <returns></returns>
        public ActionResult _UserData(long userId, DisplayTemplate_UserData displayTemplate = DisplayTemplate_UserData.Side)
        {
            User user = userService.GetFullUser(userId);
            if (user == null)
                return HttpNotFound();
            if (displayTemplate == DisplayTemplate_UserData.Side)
            {
                IEnumerable<long> sectionIds = subscribeService.GetAllObjectIds(userId);
                IEnumerable<BarSection> barSections = barSectionService.GetBarsections(sectionIds);
                ViewData["barSections"] = barSections;
            }
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            ViewData["userThreadCount"] = ownerDataService.GetLong(userId, OwnerDataKeys.Instance().ThreadCount());
            ViewData["userPostCount"] = ownerDataService.GetLong(userId, OwnerDataKeys.Instance().PostCount());
            ViewData["userFollowSectionCount"] = ownerDataService.GetLong(userId, OwnerDataKeys.Instance().FollowSectionCount());

            #region 身份认证
            List<Identification> identifications = identificationService.GetUserIdentifications(user.UserId);
            if (identifications.Count() > 0)
            {
                ViewData["identificationTypeVisiable"] = true;
            }
            #endregion

            return PartialView("_UserData_" + displayTemplate.ToString(), user);
        }

        /// <summary>
        /// 用户帖子列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="isPosted"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [UserSpaceAuthorize]
        public ActionResult UserThreads(string spaceKey, bool isPosted = false, int pageIndex = 1)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            //若当前用户是在浏览自己的帖子列表，或者是管理员，则忽略审核；
            bool ignoreAudit = UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == user.UserId || new Authorizer().IsAdministrator(BarConfig.Instance().ApplicationId);
            PagingDataSet<BarThread> pds = barThreadService.GetUserThreads(TenantTypeIds.Instance().Bar(), user.UserId, ignoreAudit, isPosted, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_List", pds);

            string title = isPosted ? user.DisplayName + "的回帖" : user.DisplayName + "的帖子";
            if (UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == user.UserId)
            {
                title = isPosted ? "我的回帖" : "我的帖子";
                ViewData["isOwner"] = true;
            }
            pageResourceManager.InsertTitlePart(title);
            ViewData["userId"] = user.UserId;

            return View(pds);
        }

        /// <summary>
        /// 用户关注的帖吧列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userCreated">是否是用户加入的群组</param>
        /// <returns></returns>
        public ActionResult UserFollowedSections(long userId, bool userCreated = false)
        {
            User user = userService.GetFullUser(userId);
            if (user == null)
                return HttpNotFound();
            IUser currentUser = UserContext.CurrentUser;

            if (UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == userId)
            {
                ViewData["isOwner"] = true;
                pageResourceManager.InsertTitlePart("我关注的帖吧");
            }
            else
                pageResourceManager.InsertTitlePart(user.DisplayName + "关注的帖吧");

            IEnumerable<long> sectionIds = subscribeService.GetAllObjectIds(userId);
            IEnumerable<BarSection> barSections = barSectionService.GetBarsections(sectionIds).Where(n => n.TenantTypeId == TenantTypeIds.Instance().Bar());

            List<BarSection> createdBarSections = new List<BarSection>();

            SelectList followedBarSections = null;
            followedBarSections = new SelectList(new List<SelectListItem>() { new SelectListItem(){Text="全部",Value="false",Selected=true},
                                                                              new SelectListItem(){Text="创建的贴吧",Value = "true"} 
            }, "value", "text");

            ViewData["followedBarSections"] = followedBarSections;

            foreach (var barSection in barSections)
            {
                if (currentUser != null && barSection.UserId == currentUser.UserId)
                {
                    createdBarSections.Add(barSection);
                }

            }
            if (userCreated)
            {
                return View(createdBarSections);
            }
            else
            {
                return View(barSections);
            }

        }

        /// <summary>
        /// 关注帖吧
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public ActionResult _SubscribeSectionButton(long sectionId)
        {
            if (UserContext.CurrentUser != null)
                ViewData["isSubscribed"] = subscribeService.IsSubscribed(sectionId, UserContext.CurrentUser.UserId);
            ViewData["sectionId"] = sectionId;
            return PartialView();
        }

        /// <summary>
        /// 关注帖吧
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public JsonResult SubscribeSection(long sectionId)
        {
            if (UserContext.CurrentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "必须先登录，才能继续操作"));
            BarSection barSection = barSectionService.Get(sectionId);
            if (barSection == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到要被关注的帖吧"));

            long userId = UserContext.CurrentUser.UserId;
            if (subscribeService.IsSubscribed(sectionId, userId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您已经关注过该帖吧"));
            subscribeService.Subscribe(sectionId, userId);
            //增加帖吧的被关注数
            CountService countService = new CountService(TenantTypeIds.Instance().BarSection());
            countService.ChangeCount(CountTypes.Instance().FollowedCount(), sectionId, barSection.UserId, 1, true);
            return Json(new StatusMessageData(StatusMessageType.Success, "关注成功"));
        }

        /// <summary>
        /// 取消关注帖吧
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public JsonResult CancelSubscribeSection(long sectionId)
        {
            if (UserContext.CurrentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "必须先登录，才能继续操作"));
            long userId = UserContext.CurrentUser.UserId;
            if (!subscribeService.IsSubscribed(sectionId, userId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有关注过该帖吧"));
            BarSection barSection = barSectionService.Get(sectionId);
            if (barSection == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到要被关注的帖吧"));
            if (barSection.UserId == userId)
                return Json(new StatusMessageData(StatusMessageType.Error, "吧主不能取消关注帖吧"));
            if (barSectionService.IsSectionManager(userId, sectionId))
                return Json(new StatusMessageData(StatusMessageType.Error, "吧管理员不能取消关注帖吧"));
            subscribeService.CancelSubscribe(sectionId, userId);
            //增加帖吧的被关注数
            CountService countService = new CountService(TenantTypeIds.Instance().BarSection());
            countService.ChangeCount(CountTypes.Instance().FollowedCount(), sectionId, barSection.UserId, -1, true);
            return Json(new StatusMessageData(StatusMessageType.Success, "取消关注操作成功"));
        }

        #endregion

        #region 帖子

        /// <summary>
        /// 帖子的顶部的局部页面
        /// </summary>
        /// <returns>显示帖吧的局部视图</returns>
        public ActionResult _BarSubmenu(long? SectionId)
        {
            BarSection barSection = null;
            if (SectionId.HasValue && SectionId.Value > 0)
            {
                IBarSettingsManager barSettingsManager = DIContainer.Resolve<IBarSettingsManager>();
                BarSettings barSetting = barSettingsManager.Get();
                ViewData["OnlyFollowerCreateThread"] = barSetting.OnlyFollowerCreateThread;

                if (UserContext.CurrentUser != null)
                    ViewData["isSubscribed"] = subscribeService.IsSubscribed(SectionId.Value, UserContext.CurrentUser.UserId);
                barSection = barSectionService.Get(SectionId.Value);
            }
            return View(barSection);
        }

        /// <summary>
        /// 帖子排行
        /// </summary>
        /// <returns></returns>
        public ActionResult Rank(SortBy_BarThread? sortBy, bool? isEssential, int pageIndex = 1)
        {
            PagingDataSet<BarThread> pds = barThreadService.Gets(TenantTypeIds.Instance().Bar(), string.Empty, isEssential, sortBy ?? SortBy_BarThread.StageHitTimes, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_List", pds);
            pageResourceManager.InsertTitlePart("帖子排行");
            ViewData["sortBy"] = sortBy;
            return View(pds);
        }

        /// <summary>
        /// 标签显示帖子列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ListsByTag(string tagName, SortBy_BarThread? sortBy, bool? isEssential, int pageIndex = 1)
        {
            tagName = WebUtility.UrlDecode(tagName);
            PagingDataSet<BarThread> pds = barThreadService.Gets(TenantTypeIds.Instance().Bar(), tagName, isEssential, sortBy ?? SortBy_BarThread.StageHitTimes, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_List", pds);

            pageResourceManager.InsertTitlePart(tagName);
            ViewData["sortBy"] = sortBy;
            ViewData["tagName"] = tagName;
            return View(pds);
        }

        /// <summary>
        /// 帖子TopN列表
        /// </summary>
        /// <returns></returns>
        public ActionResult _Tops(int topNumber, bool? isEssential, SortBy_BarThread? sortBy)
        {
            IEnumerable<BarThread> threads = barThreadService.GetTops(TenantTypeIds.Instance().Bar(), topNumber, isEssential, sortBy ?? SortBy_BarThread.StageHitTimes);
            ViewData["sortBy"] = sortBy;
            ViewData["isEssential"] = isEssential;
            return PartialView(threads);
        }

        /// <summary>
        /// 帖子TopN列表,群组热门讨论
        /// </summary>
        /// <returns></returns>
        public ActionResult _GroupTops(int topNumber, bool? isEssential, SortBy_BarThread sortBy = SortBy_BarThread.PostCount)
        {
            IEnumerable<BarThread> threads = barThreadService.GetTopsThreadOfGroup(TenantTypeIds.Instance().Group(), topNumber, isEssential, sortBy);

            ViewData["sortBy"] = sortBy;
            ViewData["isEssential"] = isEssential;
            return PartialView(threads.Take(topNumber));
        }

        /// <summary>
        /// 标签地图
        /// </summary>
        /// <returns></returns>
        public ActionResult TagMap()
        {
            pageResourceManager.InsertTitlePart("标签地图");
            return View();
        }

        #endregion

        #region 帖吧搜索

        /// <summary>
        /// 帖吧搜索
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Search(BarFullTextQuery query)
        {
            query.PageSize = 20;//每页记录数

            //默认搜公共贴吧（非群组）的帖子
            query.TenantTypeId = TenantTypeIds.Instance().Bar();

            //根据帖吧id查询帖吧名字
            string barId = Request.QueryString["barId"];
            if (barId != "" && barId != null)
            {
                long id = 0;
                if (long.TryParse(barId, out id))
                {
                    BarSection section = barSectionService.Get(id);
                    if (section != null)
                    {
                        query.TenantTypeId = section.TenantTypeId;
                        ViewData["barname"] = section.Name;
                        ViewData["TenantTypeId"] = section.TenantTypeId;
                    }
                }
            }

            //调用搜索器进行搜索
            BarSearcher BarSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
            PagingDataSet<BarEntity> BarEntities = BarSearcher.Search(query);

            //添加到用户搜索历史 
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                SearchHistoryService searchHistoryService = new SearchHistoryService();
                searchHistoryService.SearchTerm(CurrentUser.UserId, BarSearcher.CODE, query.Keyword);
            }
            //添加到热词
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(BarSearcher.CODE, query.Keyword);
            }



            //设置页面Meta
            if (string.IsNullOrWhiteSpace(query.Keyword))
            {
                pageResourceManager.InsertTitlePart("帖吧搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart('“' + query.Keyword + '”' + "的相关帖吧");//设置页面Title
            }

            pageResourceManager.SetMetaOfKeywords("帖吧搜索");//设置Keyords类型的Meta
            pageResourceManager.SetMetaOfDescription("帖吧搜索");//设置Description类型的Meta

            if (Request.IsAjaxRequest())
                return View("_ListSearchThread", BarEntities);

            return View(BarEntities);
        }

        /// <summary>
        /// 帖吧搜索
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult _GlobalSearch(BarFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.TenantTypeId = TenantTypeIds.Instance().Bar();

            //调用搜索器进行搜索
            BarSearcher barSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
            PagingDataSet<BarEntity> BarEntities = barSearcher.Search(query);

            return PartialView(BarEntities);
        }

        /// <summary>
        /// 帖吧快捷搜索
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult _QuickSearch(BarFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.TenantTypeId = TenantTypeIds.Instance().Bar();
            query.Term = BarSearchRange.SUBJECT;
            query.Keyword = Server.UrlDecode(query.Keyword);
            //调用搜索器进行搜索
            BarSearcher barSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
            PagingDataSet<BarEntity> BarEntities = barSearcher.Search(query);

            return PartialView(BarEntities);
        }

        /// <summary>
        /// 帖吧搜索自动完成
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult SearchAutoComplete(string keyword, int topNumber)
        {
            //调用搜索器进行搜索
            BarSearcher barSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
            IEnumerable<string> terms = barSearcher.AutoCompleteSearch(keyword, topNumber);

            var jsonResult = Json(terms.Select(t => new { tagName = t, tagNameWithHighlight = SearchEngine.Highlight(keyword, string.Join("", t.Take(34)), 100) }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        #endregion

        #region 前台-帖子
        /// <summary>
        /// 删除单挑帖子
        /// </summary>
        /// <param name="threadId">帖子id</param>
        /// <returns>删除帖子</returns>
        [HttpPost]
        public ActionResult Delete(long threadId)
        {
            BarThread thread = barThreadService.Get(threadId);
            if (thread == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到对应的帖子，可能该主题帖已经被删除"));
            if (new Authorizer().BarThread_Delete(thread))
            {
                barThreadService.Delete(threadId);
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
            }
            
            
            
            
            
            
            return Json(new StatusMessageData(StatusMessageType.Error, "可能您没有权限删除此帖子"));
        }

        /// <summary>
        /// 编辑帖子
        /// </summary>
        /// <param name="sectionId">帖吧id</param>
        /// <param name="threadId">帖子id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(long sectionId, long? threadId)
        {
            BarSection section = barSectionService.Get(sectionId);
            if (UserContext.CurrentUser == null)
                return Redirect(SiteUrls.Instance().Login(true));
            BarThread barThread = threadId.HasValue ? barThreadService.Get(threadId ?? 0) : null;
            if (threadId.HasValue)
            {
                if (!new Authorizer().BarThread_Edit(barThread))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "没有权限编辑" + barThread.Subject + "！",
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Error
                    }));
                }
            }
            else
            {
                string errorMessage = string.Empty;
                if (!new Authorizer().BarThread_Create(sectionId, out errorMessage))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = errorMessage,
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Error
                    }, Request.RawUrl));
                }
            }
            pageResourceManager.InsertTitlePart(threadId.HasValue ? "编辑帖子" : "发帖");
            if (threadId.HasValue && barThread == null)
                return HttpNotFound();

            ViewData["barSettings"] = barSettings;

            
            

            
            //2.没必要先new SelectListItem，直接new SelectList(categories, "CategoryId", "CategoryName", selectId)
            

            ViewData["BarSection"] = section;
            return View("Edit", barThread == null ? new BarThreadEditModel { SectionId = sectionId } : barThread.AsEditModel());
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="barThreadEditModel">帖子实体</param>
        /// <returns>处理状态</returns>
        [HttpPost]
        public ActionResult Edit(BarThreadEditModel barThreadEditModel)
        {
            SystemMessageViewModel message = new SystemMessageViewModel
            {
                Title = "发帖失败了！",
                Body = "发帖失败，请稍后再试！",
                StatusMessageType = StatusMessageType.Error
            };

            string errorMessage = string.Empty;
            if (ModelState.HasBannedWord(out errorMessage))
            {
                message.Body = errorMessage;
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, message, Request.RawUrl));
            }

            if (barThreadEditModel.ThreadId > 0)
            {
                if (new Authorizer().BarThread_Edit(barThreadEditModel.AsBarThread()))
                {
                    BarThread thread = barThreadEditModel.AsBarThread();
                    //编辑方法
                    barThreadService.Update(thread, UserContext.CurrentUser.UserId);
                    long groupId = Request.Form.Get<long>("BarThreadCategory", 0);
                    categoryService.ClearCategoriesFromItem(barThreadEditModel.ThreadId, barThreadEditModel.SectionId, TenantTypeIds.Instance().BarThread());
                    if (groupId > 0)
                        categoryService.AddCategoriesToItem(new List<long> { groupId }, barThreadEditModel.ThreadId, barThreadEditModel.SectionId);
                    tagService.ClearTagsFromItem(barThreadEditModel.ThreadId, barThreadEditModel.SectionId);
                    string tags = string.Join(",", barThreadEditModel.TagNames);
                    tagService.AddTagsToItem(tags, barThreadEditModel.SectionId, barThreadEditModel.ThreadId);
                    IBarUrlGetter getter = BarUrlGetterFactory.Get(thread.TenantTypeId);
                    return Redirect(getter.ThreadDetail(thread.ThreadId));
                }
                else
                {
                    message.Title = "编辑帖子失败！";
                    message.Body = "您没有权限编辑帖子！";
                }
            }
            else
            {
                if (new Authorizer().BarThread_Create(barThreadEditModel.SectionId, out errorMessage))
                {
                    //新建方法
                    BarThread barThread = barThreadEditModel.AsBarThread();
                    bool isCreated = barThreadService.Create(barThread);
                    if (isCreated)
                    {
                        long groupId = Request.Form.Get<long>("BarThreadCategory", 0);
                        if (groupId > 0)
                            categoryService.AddCategoriesToItem(new List<long> { groupId }, barThread.ThreadId, barThread.SectionId);
                        string tags = string.Join(",", barThreadEditModel.TagNames);

                        tagService.AddTagsToItem(tags, barThread.SectionId, barThread.ThreadId);
                        IBarUrlGetter getter = BarUrlGetterFactory.Get(barThread.TenantTypeId);
                        return Redirect(getter.ThreadDetail(barThread.ThreadId));
                    }
                }
                else
                {
                    message.Body = errorMessage;
                }
            }
            return Redirect(SiteUrls.Instance().SystemMessage(TempData, message, Request.RawUrl));
        }

        /// <summary>
        /// 帖子详细显示页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ThreadDetail(long threadId, int pageIndex = 1, bool onlyLandlord = false, SortBy_BarPost sortBy = SortBy_BarPost.DateCreated, long? postId = null, long? childPostIndex = null)
        {
            BarThread barThread = barThreadService.Get(threadId);
            if (barThread == null)
                return HttpNotFound();

            BarSection section = barSectionService.Get(barThread.SectionId);
            if (!new Authorizer().BarSection_View(section))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, SystemMessageViewModel.NoCompetence()));

            pageResourceManager.InsertTitlePart(section.Name);
            pageResourceManager.InsertTitlePart(barThread.Subject);
            
            

            Category category = categoryService.Get(barThread.CategoryId ?? 0);
            string keyWords = string.Join(",", barThread.TagNames);

            pageResourceManager.SetMetaOfKeywords(category != null ? category.CategoryName + "," + keyWords : keyWords);//设置Keyords类型的Meta
            pageResourceManager.SetMetaOfDescription(HtmlUtility.TrimHtml(barThread.GetResolvedBody(), 120));//设置Description类型的Meta

            
            
            ViewData["EnableRating"] = barSettings.EnableRating;

            //更新浏览计数
            CountService countService = new CountService(TenantTypeIds.Instance().BarThread());
            countService.ChangeCount(CountTypes.Instance().HitTimes(), barThread.ThreadId, barThread.UserId, 1, false);
            
            
            
            

            PagingDataSet<BarPost> barPosts = barPostService.Gets(threadId, onlyLandlord, sortBy, pageIndex);
            if (pageIndex > barPosts.PageCount && pageIndex > 1)
                return ThreadDetail(threadId, barPosts.PageCount);
            
            
            
            
            if (Request.IsAjaxRequest())
                return PartialView("_ListPost", barPosts);
            
            
            ViewData["barThread"] = barThread;


            
            
            
            
            return View(barPosts);
        }

        /// <summary>
        /// 递归获取所有父级（如果没有子级则不包含本级）
        /// </summary>
        /// <param name="category"></param>
        /// <param name="allParentCategories"></param>
        private void GetAllParentCategories(Category category, ref List<Category> allParentCategories)
        {
            if (category == null)
                return;
            if (category.ChildCount != 0)
                allParentCategories.Insert(0, category);
            GetAllParentCategories(category.Parent, ref allParentCategories);
        }

        /// <summary>
        /// 移动帖子
        /// </summary>
        /// <param name="threadId">帖子id</param>
        /// <returns>移动帖子</returns>
        [HttpGet]
        public ActionResult _MoveThread(long threadId, long? categoryId = null)
        {
            IEnumerable<Category> childCategories = null;

            ViewData["CategoryId"] = categoryId;

            if (categoryId.HasValue)
            {
                Category category = categoryService.Get(categoryId ?? 0);
                if (category != null)
                {
                    if (category.ChildCount > 0)
                        childCategories = category.Children;
                    else if (category.Parent != null)
                        childCategories = category.Parent.Children;
                }
                List<Category> allParentCategories = new List<Category>();
                //递归获取所有父级类别，若不是叶子节点，则包含其自身

                GetAllParentCategories(category, ref allParentCategories);
                ViewData["AllParentCategories"] = allParentCategories;
            }

            if (childCategories == null)
                childCategories = categoryService.GetRootCategories(TenantTypeIds.Instance().BarSection());

            ViewData["ChildCategories"] = childCategories;

            ViewData["BarSections"] = barSectionService.GetTops(60, categoryId, SortBy_BarSection.FollowedCount);

            return View(threadId);
        }

        /// <summary>
        /// 移动帖子
        /// </summary>
        /// <param name="threadId">准备移动的帖子</param>
        /// <param name="toSectionId">接受帖子的帖吧</param>
        /// <returns>移动帖子</returns>
        [HttpPost]
        public ActionResult MoveThread(long threadId, long toSectionId)
        {
            if (!new Authorizer().IsAdministrator(BarConfig.Instance().ApplicationId))
                return HttpNotFound();
            BarThread thread = barThreadService.Get(threadId);
            if (thread == null)
                return HttpNotFound();
            BarSection section = barSectionService.Get(toSectionId);
            if (section == null)
                return HttpNotFound();

            barThreadService.MoveThread(threadId, toSectionId);
            return Redirect(SiteUrls.Instance().ThreadDetail(threadId));
        }

        /// <summary>
        /// 他的其他的帖子
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="exceptThreadId">被排除的帖子的id</param>
        /// <returns>他的其他帖子</returns>
        [HttpGet]
        public ActionResult _HisOtherThreads(long userId, long exceptThreadId)
        {
            IEnumerable<BarThread> threads = barThreadService.GetUserThreads(TenantTypeIds.Instance().Bar(), userId, false);
            IEnumerable<BarThread> hisOtherThreads = null;
            if (threads != null && threads.Count() > 0)
                hisOtherThreads = threads.Where(n => n.ThreadId != exceptThreadId);
            return View(hisOtherThreads);
        }

        #endregion

        #region 前台-帖子评分

        
        
        /// <summary>
        /// 评分局部页面
        /// </summary>
        /// <param name="threadId">被评论的帖子id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>评分局部页面</returns>
        public ActionResult _ListBarRatings(long threadId, int pageIndex = 1)
        {
            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["ReputationPoints"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;
            ViewData["ThreadId"] = threadId;
            return View(barRatingService.Gets(threadId, pageIndex));
        }


        /// <summary>
        /// 创建评分页面
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _CreatBarRating(long threadId)
        {
            
            
            
            
            string errorMessage;
            if (!new Authorizer().BarRating(threadId, out errorMessage))
            {
                ViewData["CanCreatBarRating"] = false;
                ViewData["ErrorMessage"] = errorMessage;
                return View();
            }

            int remainReputationPoints = barSettings.UserReputationPointsPerDay - barRatingService.GetUserTodayRatingSum(UserContext.CurrentUser.UserId);

            List<SelectListItem> items = new List<SelectListItem>();

            int selectValue = -1;

            for (int i = barSettings.ReputationPointsMinValue; i < (remainReputationPoints > barSettings.ReputationPointsMaxValue ? barSettings.ReputationPointsMaxValue + 1 : remainReputationPoints + 1); i++)
            {
                if (i == 0)
                    continue;
                if (selectValue < 0)
                    selectValue = i;
                items.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewData["ReputationPoints"] = new SelectList(items, "Value", "Text", selectValue);

            ViewData["threadId"] = threadId;
            return View(new BarRatingEditModel { ThreadId = threadId, RemainReputationPoints = remainReputationPoints });
        }

        /// <summary>
        /// 创建评分
        /// </summary>
        /// <returns>创建评分</returns>
        [HttpPost]
        public ActionResult _CreatBarRating(BarRatingEditModel model)
        {
            if (UserContext.CurrentUser == null)
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "未登录用户不允许评分"));
            }

            if (model.ReputationPoints + barRatingService.GetUserTodayRatingSum(UserContext.CurrentUser.UserId) > barSettings.UserReputationPointsPerDay)
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "积分不足评分失败"));
            }

            BarThread thread = barThreadService.Get(model.ThreadId);

            string errorMessage;
            if (!new Authorizer().BarRating(thread, out errorMessage))
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, errorMessage));
            }

            BarRating rating = model.AsBarRating();
            bool isCreated = barRatingService.Create(rating);
            if (isCreated)
            {
                return Json(new { BarRatingId = rating.RatingId });
            }
            WebUtility.SetStatusCodeForError(Response);
            return Json(new StatusMessageData(StatusMessageType.Error, "创建评分失败"));
        }

        /// <summary>
        /// 一条评分的局部页面
        /// </summary>
        /// <param name="ratingId">评分id</param>
        /// <returns>一条评分的局部页面</returns>
        [HttpGet]
        public ActionResult _OneBarRating(long ratingId)
        {
            BarRating rating = barRatingService.Get(ratingId);
            if (rating == null)
                return HttpNotFound();

            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["ReputationPoints"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;

            return View(rating);
        }

        #endregion

        #region 前台-回帖

        /// <summary>
        /// 编辑回帖
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPost(long threadId, long? postId = null)
        {
            BarThread thread = barThreadService.Get(threadId);

            if (thread == null)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "没有找到你要编辑的回帖",
                    Title = "没有找到回帖",
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            BarPost post = null;
            if (postId.HasValue)
            {
                post = barPostService.Get(postId ?? 0);
                if (!new Authorizer().BarPost_Edit(post))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "您没有权限编辑此回帖",
                        Title = "没有权限"
                    }));
                }
            }
            else
            {
                
                
                string errorMessage = string.Empty;
                if (!new Authorizer().BarPost_Create(thread.SectionId, out errorMessage))
                {
                    
                    
                    if (UserContext.CurrentUser == null)
                        return Redirect(SiteUrls.Instance().Login(true));

                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = errorMessage,
                        Title = "没有权限"
                    }));
                }
            }
            if (postId.HasValue && post == null)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "没有找到你要编辑的回帖",
                    Title = "没有找到回帖"
                }));
            }

            
            
            
            
            BarPostEditModel postModel = null;
            if (post != null)
                postModel = post.AsEditModel();
            else
                postModel = new BarPostEditModel
                {
                    ThreadId = threadId,
                    PostId = postId,
                    Subject = thread.Subject
                };

            string body = Request.QueryString.Get<string>("MultilineBody", null);
            if (!string.IsNullOrEmpty(body))
                postModel.Body = body = new EmotionService().EmoticonTransforms(body);

            ViewData["PostBodyMaxLength"] = barSettings.PostBodyMaxLength;

            postModel.SectionId = thread.SectionId;
            return View(postModel);
        }

        /// <summary>
        /// 编辑回帖
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPost(BarPostEditModel post)
        {
            
            //1.没有判断创建、编辑回帖的权限
            
            //2.代码组织乱，先判断异常情况，正常情况留至最后处理；
            
            //3.如果返回错误信息情况较多，可以抽出一个私有方法(已帮忙写好，查看StatusMessage方法)，在其内部判断是同步还是异步；
            
            BarPost barPost = post.AsBarPost();
            if (barPost == null)
                return StatusMessage(StatusMessageType.Error, "没有找到编辑的内容");

            string errorMessage = string.Empty;
            if (ModelState.HasBannedWord(out errorMessage))
            {
                return StatusMessage(StatusMessageType.Error, errorMessage);
            }

            if (post.PostId.HasValue && post.PostId.Value > 0)
            {
                if (!new Authorizer().BarPost_Edit(post.PostId ?? 0))
                    return StatusMessage(StatusMessageType.Error, "您没有权限编辑此回帖");
                barPostService.Update(barPost, UserContext.CurrentUser.UserId);
                if (Request.IsAjaxRequest())
                    return Json(new { PostId = barPost.PostId });
                else
                    return Redirect(SiteUrls.Instance().ThreadDetailGotoPost(barPost.PostId));
            }
            else
            {
                StatusMessageData hint = null;
                if (!new Authorizer().BarPost_Create(barPost.SectionId, out errorMessage))
                    return StatusMessage(StatusMessageType.Error, errorMessage);
                bool isCreat = barPostService.Create(barPost);
                if (barPost.AuditStatus == AuditStatus.Pending)
                {
                    hint = new StatusMessageData(StatusMessageType.Hint, "您的回复需要通过审核");
                }
                if (isCreat)
                {
                    if (Request.IsAjaxRequest())
                        return Json(new { PostId = barPost.PostId, auditStatus = hint });
                    else
                        return Redirect(SiteUrls.Instance().ThreadDetailGotoPost(barPost.PostId));
                }
                return StatusMessage(StatusMessageType.Error, "创建失败");
            }
        }

        /// <summary>
        /// 返回提示信息（会自动判断是同步请求还是异步请求）
        /// </summary>
        /// <param name="messageType">提示信息类型</param>
        /// <param name="messageContent">提示信息内容</param>
        /// <returns>若是异步请求，则返回json数据；若是同步请求，则跳转到信息提示页面</returns>
        private ActionResult StatusMessage(StatusMessageType messageType, string messageContent)
        {
            if (Request.IsAjaxRequest())
            {
                if (messageType == StatusMessageType.Error)
                    WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(messageType, messageContent));
            }
            else
            {
                
                
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = messageContent,
                    StatusMessageType = messageType
                }));
            }
        }

        
        

        /// <summary>
        /// 删除回帖
        /// </summary>
        /// <param name="postId">回帖的id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePost(long postId)
        {
            BarPost post = barPostService.Get(postId);
            if (post == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到准备删除的回帖"));
            if (new Authorizer().BarPost_Delete(post))
                barPostService.Delete(postId);
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有删除此回帖的权限"));
            return Json(new StatusMessageData(StatusMessageType.Success, "成功删除了回帖"));
        }

        
        
        
        

        
        

        /// <summary>
        /// 回帖列表
        /// </summary>
        /// <param name="threadId">帖子的id</param>
        /// <param name="parentId">父回帖的id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ListPost(long threadId, long parentId, int pageIndex = 1, bool showBeforPage = false)
        {
            BarThread barThread = barThreadService.Get(threadId);

            if (barThread == null)
                return Content(string.Empty);

            ViewData["BarThread"] = barThread;
            ViewData["parentId"] = parentId;
            PagingDataSet<BarPost> posts = barPostService.GetChildren(parentId, SortBy_BarPost.DateCreated_Desc, pageIndex);
            PagingDataSet<BarPost> postsReverse = new PagingDataSet<BarPost>(new List<BarPost>());
            if (posts != null)
            {
                postsReverse = new PagingDataSet<BarPost>(posts.Reverse());
                postsReverse.PageIndex = posts.PageIndex;
                postsReverse.PageSize = posts.PageSize;
                postsReverse.QueryDuration = posts.QueryDuration;
                postsReverse.TotalRecords = posts.TotalRecords;
            }
            return View(postsReverse);
        }

        /// <summary>
        /// 一条回帖的展示
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _BarPost(long postId)
        {
            BarPost post = barPostService.Get(postId);
            if (post == null)
                return HttpNotFound();
            return View(post);
        }

        
        
        /// <summary>
        /// 子级回帖
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ChildPost(long parentId, long childPostIndex = 1, bool reAnchor = false)
        {
            BarPost post = barPostService.Get(parentId);
            if (post == null)
                return HttpNotFound();
            return View(post);
        }



        #endregion

        #region 前后台公用局部页
        
        

        
        
        /// <summary>
        /// 展示帖子的局部分页
        /// </summary>
        /// <param name="threads">要展示的实体</param>
        /// <returns>展示帖子的局部分页</returns>
        public ActionResult _PagingThreads(PagingDataSet<BarThread> threads, bool ShowSectionInfo = false)
        {
            ViewData["ShowSectionInfo"] = ShowSectionInfo;
            ViewData["BarThreads"] = threads;
            return View();
        }

        
        

        
        
        
        /// <summary>
        /// 设置置顶时间的局部页面
        /// </summary>
        /// <param name="threadIds">帖子id</param>
        /// <param name="showTips">是否显示Tips提示。</param>
        /// <returns>设置置顶时间的局部页面</returns>
        [HttpGet]
        public ActionResult _SetStickyDate(List<long> threadIds, bool showTips = true)
        {
            ViewData["ShowTips"] = showTips;
            ViewData["threadId"] = threadIds;
            return View();
        }

        #endregion

        #region 管理帖吧-页面
        /// <summary>
        /// 前台管理帖吧页面（管理帖子）
        /// </summary>
        /// <param name="model">用户填充的实体</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>后台管理帖吧页面</returns>
        public ActionResult ManageThreads(ManageThreadEditModel model, int pageIndex = 1)
        {
            BarSection section = barSectionService.Get(model.SectionId ?? 0);
            if (!new Authorizer().BarSection_Manage(section))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = string.Format("您没有权限管理 {0} ！", section == null ? "" : section.Name),
                    Title = "没有权限",
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            pageResourceManager.InsertTitlePart("帖吧管理");

            List<SelectListItem> SelectListItem_TrueAndFlase = new List<SelectListItem> { new SelectListItem { Text = "是", Value = true.ToString() }, new SelectListItem { Text = "否", Value = false.ToString() } };

            ViewData["IsEssential"] = new SelectList(SelectListItem_TrueAndFlase, "Value", "Text", model.IsEssential);
            ViewData["IsSticky"] = new SelectList(SelectListItem_TrueAndFlase, "Value", "Text", model.IsSticky);

            IEnumerable<Category> categories = categoryService.GetOwnerCategories(model.SectionId ?? 0, TenantTypeIds.Instance().BarThread());
            ViewData["CategoryId"] = new SelectList(categories.Select(n => new { text = StringUtility.Trim(n.CategoryName, 20), value = n.CategoryId }), "value", "text", model.CategoryId);

            ViewData["BarThreads"] = barThreadService.Gets(TenantTypeIds.Instance().Bar(), model.GetBarThreadQuery(), model.PageSize ?? 20, pageIndex);

            return View(model);
        }

        /// <summary>
        /// 管理回帖页面
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="model">回帖管理的model</param>
        /// <returns>管理回帖</returns>
        [HttpGet]
        public ActionResult ManagePosts(ManagePostsEditModel model, int pageIndex = 1)
        {
            if (!new Authorizer().BarSection_Manage(model.SectionId ?? 0))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "没有权限",
                    Body = "您可能没有权限编辑此帖吧"
                }));
            }

            
            
            
            
            pageResourceManager.InsertTitlePart("回帖管理");
            ViewData["BarPosts"] = barPostService.Gets(TenantTypeIds.Instance().Bar(), model.AsBarPostQuery(), model.PageSize ?? 20, pageIndex);
            return View(model);
        }
        
        
        /// <summary>
        /// 管理帖子类别
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageThreadCategories(long sectionId, int pageIndex = 1)
        {
            if (!new Authorizer().BarSection_Manage(sectionId))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "您没有权限编辑此帖吧的分类",
                    Title = "没有权限"
                }));
            }

            pageResourceManager.InsertTitlePart("类别管理");
            ViewData["SectionId"] = sectionId;
            return View(categoryService.GetOwnerCategories(sectionId, TenantTypeIds.Instance().BarThread()));
        }

        #region 帖吧分类管理
        
        
        /// <summary>
        /// 编辑帖子的类别局部页面
        /// </summary>
        /// <returns>编辑帖子的类别局部页面</returns>
        [HttpGet]
        public ActionResult _EditThreadCategory(long OwnerId, long? CategoryId = null)
        {
            CategoryEditModel model = new CategoryEditModel();

            if (CategoryId.HasValue)
            {
                Category category = categoryService.Get(CategoryId ?? 0);
                if (category != null)
                    model = category.AsCategoryEditModel();
            }

            model.OwnerId = OwnerId;

            return View(model);
        }

        /// <summary>
        /// 编辑帖吧的分类
        /// </summary>
        /// <param name="model">编辑</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditThreadCategoryPost(CategoryEditModel model)
        {
            if (!new Authorizer().BarSection_Manage(model.OwnerId))
            {
                
                
                return Json(new StatusMessageData(StatusMessageType.Error, "没有权限"));
            }

            string errorMessage = string.Empty;
            if (ModelState.HasBannedWord(out errorMessage))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, errorMessage));
            }

            BarSection section = barSectionService.Get(model.OwnerId);

            if (section == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "创建分类失败"));

            model.TenantTypeId = TenantTypeIds.Instance().BarThread();
            Category category = model.AsCategory();
            if (model.CategoryId != 0)
            {
                categoryService.Update(category);
                return Json(new StatusMessageData(StatusMessageType.Success, "更新分类成功"));
            }
            else
            {
                bool isCreat = categoryService.Create(category);
                if (isCreat)
                    return Json(new StatusMessageData(StatusMessageType.Success, "创建分类成功"));
                
                

                return Json(new StatusMessageData(StatusMessageType.Error, "创建分类失败"));
            }

        }

        /// <summary>
        /// 改变类别的排序
        /// </summary>
        /// <param name="id">本条的id</param>
        /// <param name="referenceId">准备交换的id</param>
        /// <returns>改变类别的排序</returns>
        public ActionResult ChangeDisplayOrder(long id, long referenceId)
        {
            Category category = categoryService.Get(id);
            Category referenceCategory = categoryService.Get(referenceId);
            if (category != null && referenceCategory != null)
            {
                
                long tem = category.DisplayOrder;
                category.DisplayOrder = referenceCategory.DisplayOrder;
                referenceCategory.DisplayOrder = tem;
                categoryService.Update(category);
                categoryService.Update(referenceCategory);

                return Json(new StatusMessageData(StatusMessageType.Success, "交换成功"));
            }
            else
            {
                
                
                
                
                return Json(new StatusMessageData(StatusMessageType.Error, "交换失败"));
            }
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <returns>删除分类</returns>
        [HttpPost]
        public ActionResult DeleteThreadCategory(long categoryId)
        {
            Category category = categoryService.Get(categoryId);
            if (category == null)
                return Json(new StatusMessageData(StatusMessageType.Success, "该分类已经被删除"));
            if (new Authorizer().BarSection_Manage(category.OwnerId))
            {
                bool isDelete = categoryService.Delete(categoryId);
                if (isDelete)
                    return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
                
                
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败"));
            }
            
            
            return Json(new StatusMessageData(StatusMessageType.Error, "您可能没有权限删除此分类"));
        }
        #endregion

        #region 批量操作-帖子

        
        
        /// <summary>
        /// 设置帖子的审核状态
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="threadIds"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchUpdateThreadAuditStatus(List<long> threadIds, long sectionId, bool isApproved = true)
        {
            
            
            if (new Authorizer().BarSection_Manage(sectionId))
            {
                IEnumerable<BarThread> barThreads = barThreadService.GetBarThreads(threadIds);
                if (barThreads != null && barThreads.Count() > 0)
                {
                    IEnumerable<BarThread> barThreadsForSection = barThreads.Where(n => n.SectionId == sectionId);
                    if (barThreadsForSection != null && barThreadsForSection.Count() > 0)
                        barThreadService.BatchUpdateAuditStatus(barThreadsForSection.Select(n => n.ThreadId), isApproved);
                }
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "您可能没有权限进行此操作"));
            }
            if (isApproved)
                return Json(new StatusMessageData(StatusMessageType.Success, "批量通过审核操作成功"));
            return Json(new StatusMessageData(StatusMessageType.Success, "批量不通过审核操作成功"));
        }

        
        
        /// <summary>
        /// 批量精华
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="threadIds">操作的帖子id</param>
        /// <param name="isEssential">是否精华</param>
        /// <returns>批量精华</returns>
        [HttpPost]
        public ActionResult BatchSetEssential(List<long> threadIds, long sectionId, bool isEssential)
        {
            
            //已经处理
            if (new Authorizer().BarSection_Manage(sectionId))
            {
                IEnumerable<BarThread> barThreads = barThreadService.GetBarThreads(threadIds);
                if (barThreads != null && barThreads.Count() > 0)
                {
                    
                    
                    barThreadService.BatchSetEssential(barThreads.Where(n => n.SectionId == sectionId).Select(n => n.ThreadId), isEssential);
                }
            }
            return Json(new StatusMessageData(StatusMessageType.Success, isEssential ? "批量设置精华状态成功" : "批量取消精华成功"));
        }

        
        
        /// <summary>
        /// 批量置顶
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="threadIds">操作的帖子id</param>
        /// <param name="isSticky">是否置顶</param>
        /// <param name="stickyDate">置顶时间</param>
        /// <returns>批量置顶</returns>
        [HttpPost]
        public ActionResult BatchSetSticky(List<long> threadIds, bool isSticky, DateTime stickyDate)
        {
            
            
            
            
            Dictionary<long, bool> authorizerCache = new Dictionary<long, bool>();

            IEnumerable<BarThread> barThreads = barThreadService.GetBarThreads(threadIds);

            List<long> canMangageThreadIds = new List<long>();
            foreach (var item in barThreads)
            {
                if (!authorizerCache.ContainsKey(item.SectionId))
                    authorizerCache.Add(item.SectionId, new Authorizer().BarSection_Manage(item.SectionId));

                if (authorizerCache[item.SectionId])
                    canMangageThreadIds.Add(item.ThreadId);
            }

            if (canMangageThreadIds.Count > 0)
            {
                
                
                //说明：由于前台方法的修改。所以方法内部的程序也已经修改。
                barThreadService.BatchSetSticky(canMangageThreadIds, isSticky, stickyDate);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "您可能没有权限设置置顶状态"));
            }

            return Json(new StatusMessageData(StatusMessageType.Success, isSticky ? "批量设置置顶状态成功" : "批量取消置顶成功"));
        }

        /// <summary>
        /// 帖吧批量删除操作
        /// </summary>
        /// <param name="threadIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchDeleteThread(List<long> threadIds, long sectionId)
        {
            
            
            if (new Authorizer().BarSection_Manage(sectionId))
            {
                IEnumerable<BarThread> barThreads = barThreadService.GetBarThreads(threadIds);
                if (barThreads != null && barThreads.Count() > 0)
                    foreach (var item in barThreads)
                        if (item.SectionId == sectionId)
                            barThreadService.Delete(item.ThreadId);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "批量删除成功"));
        }
        #endregion

        #region 批量操作-回帖
        
        
        /// <summary>
        /// 批量设置审核状态
        /// </summary>
        /// <param name="postIds">准备设置的id</param>
        /// <param name="isApproved">是否通过审核</param>
        /// <returns>批量设置审核状态</returns>
        [HttpPost]
        public ActionResult BatchUpdatePostAuditStatus(List<long> postIds, bool isApproved = true)
        {
            
            
            Dictionary<long, bool> authorizer = new Dictionary<long, bool>();
            IEnumerable<BarPost> barPosts = barPostService.GetBarPosts(postIds);
            foreach (var post in barPosts)
            {
                if (!authorizer.ContainsKey(post.SectionId))
                    authorizer.Add(post.SectionId, new Authorizer().BarSection_Manage(post.SectionId));
                if (authorizer[post.SectionId])
                    barPostService.UpdateAuditStatus(post.PostId, isApproved);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "批量设置审核状态成功"));
        }
        
        
        /// <summary>
        /// 删除回帖
        /// </summary>
        /// <param name="postIds">准备删除的回帖id</param>
        /// <returns>删除回帖</returns>
        [HttpPost]
        public ActionResult BatchDeletePosts(List<long> postIds)
        {
            foreach (var id in postIds)
            {
                BarPost post = barPostService.Get(id);
                if (new Authorizer().BarPost_Delete(post))
                    barPostService.Delete(id);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        #endregion

        #region 帖吧信息管理

        /// <summary>
        /// 编辑帖吧页面
        /// </summary>
        /// <param name="sectionId">被编辑的帖吧id</param>
        /// <returns>编辑帖吧页面</returns>
        [HttpGet]
        public ActionResult EditSection(long sectionId)
        {
            ViewData["StatusMessageData"] = TempData["StatusMessageData"];
            TempData.Remove("StatusMessageData");
            BarSection section = barSectionService.Get(sectionId);
            
            
            
            
            pageResourceManager.InsertTitlePart("编辑帖吧");
            if (new Authorizer().BarSection_Manage(section))
            {
                IBarSettingsManager barSettingsManager = DIContainer.Resolve<IBarSettingsManager>();
                BarSettings barSetting = barSettingsManager.Get();
                ViewData["AdminCount"] = 5;
                ViewData["ManagerUserIds"] = barSectionService.GetSectionManagers(sectionId).Select(n => n.UserId);
                return View(section.AsEditModel());
            }

            return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
            {
                Body = "您可能没有权限编辑此帖吧！",
                Title = "没有权限编辑此帖吧！"
            }));
        }

        /// <summary>
        /// 管理帖吧信息
        /// </summary>
        /// <param name="model">帖吧信息载体</param>
        /// <returns>管理帖吧信息</returns>
        [HttpPost]
        public ActionResult EditSection(BarSectionEditModel model)
        {

            if (new Authorizer().BarSection_Manage(model.SectionId))
            {
                HttpPostedFileBase logoImage = Request.Files["LogoImage"];
                string logoImageFileName = string.Empty;
                Stream stream = null;
                if (logoImage != null && !string.IsNullOrEmpty(logoImage.FileName))
                {
                    TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().BarSection());
                    if (!tenantLogoSettings.ValidateFileLength(logoImage.ContentLength))
                    {
                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("Logo文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                        return View(model);
                    }

                    LogoSettings logoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                    if (!logoSettings.ValidateFileExtensions(logoImage.FileName))
                    {
                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "Logo文件是不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                        return View(model);
                    }
                    stream = logoImage.InputStream;
                    logoImageFileName = logoImage.FileName;
                }

                BarSection section = model.GetBarSectionByEditForManager();
                if (!string.IsNullOrEmpty(logoImageFileName))
                    section.LogoImage = logoImageFileName;
                IEnumerable<long> managerUserIds = barSectionService.GetSectionManagers(model.SectionId).Select(n => n.UserId);

                if (new Authorizer().BarSection_SetManager(model.SectionId))
                {
                    
                    
                    managerUserIds = Request.Form.Gets<long>("ManagerUserIds") == null ? null : Request.Form.Gets<long>("ManagerUserIds");
                }

                barSectionService.Update(section, UserContext.CurrentUser.UserId, managerUserIds, stream);

                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "更新设置成功");
                return RedirectToAction("EditSection", new { sectionId = model.SectionId });
            }
            else
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "保存失败，您可能没有权限编辑帖子描述",
                    Title = "保存失败"
                }));
            }
        }

        #endregion

        #endregion

        #region 帖吧选择器

        /// <summary>
        /// 帖吧选择器
        /// </summary>
        /// <param name="categoryId">类别id</param>
        /// <param name="selectedSectionId">默认选中的帖吧</param>
        /// <returns>帖吧选择器</returns>
        public ActionResult _BarSetionSelector(string name, long? categoryId = null, long? selectedSectionId = null)
        {
            ViewData["CategoryId"] = categoryId;

            ViewData["Name"] = name;

            BarSection section = null;
            if (!selectedSectionId.HasValue)
                selectedSectionId = Request.QueryString.Get<long?>(name, null);

            section = barSectionService.Get(selectedSectionId ?? 0);

            return View(section);
        }

        /// <summary>
        /// 帖吧选择器显示的页面
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <param name="selectedSectionId">默认被选中的帖吧</param>
        /// <returns>帖吧选择器</returns>
        public ActionResult _BarSetionSelectItem(string name, long? categoryId = null, long? selectedSectionId = null, bool isHidden = false, string tenantTypeId = null)
        {
            tenantTypeId = (string.IsNullOrEmpty(tenantTypeId) || tenantTypeId.Equals(TenantTypeIds.Instance().Bar())) ? TenantTypeIds.Instance().BarSection() : tenantTypeId;

            ViewData["IsHidden"] = isHidden;
            ViewData["TenantTypeId"] = tenantTypeId;
            ViewData["Name"] = name;
            IEnumerable<Category> childCategories = null;
            ViewData["CategoryId"] = categoryId;

            string tenantTypeName = "帖吧";
            var tenantType = new TenantTypeService().Get(tenantTypeId);
            if (tenantType != null)
                tenantTypeName = tenantType.Name;
            ViewData["TenantTypeName"] = tenantTypeName;

            if (categoryId.HasValue)
            {
                Category category = categoryService.Get(categoryId ?? 0);
                if (category != null)
                {
                    if (category.ChildCount > 0)
                        childCategories = category.Children;
                    else if (category.Parent != null)
                        childCategories = category.Parent.Children;
                }
                List<Category> allParentCategories = new List<Category>();

                GetAllParentCategories(category, ref allParentCategories);
                ViewData["AllParentCategories"] = allParentCategories;
            }

            if (childCategories == null)
                childCategories = categoryService.GetRootCategories(tenantTypeId);

            if (categoryId.HasValue)
            {
                List<Category> categories = childCategories.ToList();
                Category category = categories.Where(n => n.CategoryId == (categoryId ?? 0)).FirstOrDefault();
                if (category != null)
                {
                    categories.Remove(category);
                    categories.Insert(0, category);
                    childCategories = categories;
                }
            }

            ViewData["ChildCategories"] = childCategories;

            //ViewData["BarSections"] = barSectionService.GetTops(200, categoryId, SortBy_BarSection.FollowedCount);
            if (string.IsNullOrEmpty(tenantTypeId) || tenantTypeId == TenantTypeIds.Instance().BarSection())
            {
                tenantTypeId = TenantTypeIds.Instance().Bar();
            }

            ViewData["BarSections"] = barSectionService.Gets(tenantTypeId, new BarSectionQuery { CategoryId = categoryId }, 200, 1);

            BarSection section = null;
            if (selectedSectionId.HasValue)
                section = barSectionService.Get(selectedSectionId ?? 0);
            return View(section);
        }

        #endregion

        #region 群组贴吧和贴吧公用的局部页面

        /// <summary>
        /// 编辑贴吧的局部视图
        /// </summary>
        /// <param name="model"></param>
        /// <returns>编辑贴吧的局部视图</returns>
        public ActionResult _Edit(BarThreadEditModel model)
        {
            return View(model);
        }

        #endregion

        #region 设置

        /// <summary>
        /// 群组分类设置
        /// </summary>
        /// <param name="spaceKey">群组名</param>
        /// <returns>群组分类设置</returns>
        [HttpGet]
        public ActionResult _ThreadCategoriesSettings(long sectionId)
        {
            BarSection section = barSectionService.Get(sectionId);
            if (section == null)
                return Content(string.Empty);

            return View(section);
        }

        /// <summary>
        /// 更新贴吧的分类设置
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <param name="threadCategoryStatus">帖子分类状态</param>
        /// <returns>更新帖子的分类设置</returns>
        [HttpPost]
        public ActionResult _ThreadCategoriesSettings(long sectionId, ThreadCategoryStatus threadCategoryStatus)
        {
            if (!new Authorizer().BarSection_Manage(sectionId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您可能没有权限设置"));

            BarSection section = barSectionService.Get(sectionId);
            if (section == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到对应的贴吧"));

            IEnumerable<User> managers = barSectionService.GetSectionManagers(sectionId);
            IEnumerable<long> managersIds = new List<long>();
            if (managers != null && managers.Count() > 0)
                managersIds = managers.Select(n => n.UserId);

            section.ThreadCategoryStatus = threadCategoryStatus;

            barSectionService.Update(section, UserContext.CurrentUser.UserId, managersIds, null);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功"));
        }


        #endregion

        /// <summary>
        /// 获取标签云
        /// </summary>_CommentList
        /// <param name="num">显示的标签数量</param>
        /// <returns></returns>
        public ActionResult _TagCloud(int num = 20)
        {
            IEnumerable<long> tagInOwnerId = barSectionService.GetTopTagsForBar(num, SortBy_Tag.ItemCountDesc);
            Dictionary<TagInOwner, int> tags = tagService.GetTagsByTagInOwnerIds(tagInOwnerId);

            return View(tags);
        }


    }

    /// <summary>
    /// 帖吧排行内容块显示模板
    /// </summary>
    public enum DisplayTemplate_TopSections
    {
        /// <summary>
        /// 头条列表
        /// </summary>
        Headline,
        /// <summary>
        /// 摘要列表
        /// </summary>
        Summary
    }

    /// <summary>
    /// 推荐帖吧内容块显示模板
    /// </summary>
    public enum DisplayTemplate_RecommendSections
    {

        /// <summary>
        /// 幻灯列表
        /// </summary>
        Slide,
        /// <summary>
        /// 摘要列表
        /// </summary>
        Summary
    }
    /// <summary>
    /// 用户数据内容块显示模板
    /// </summary>
    public enum DisplayTemplate_UserData
    {

        /// <summary>
        /// 幻灯列表
        /// </summary>
        Side,
        /// <summary>
        /// 摘要列表
        /// </summary>
        Main
    }

    /// <summary>
    /// 左侧项目的枚举项
    /// </summary>
    public enum ManageSectionSubMenu
    {
        /// <summary>
        /// 管理帖子
        /// </summary>
        ManageThread = 1,

        /// <summary>
        /// 管理回帖
        /// </summary>
        ManagePost,

        /// <summary>
        /// 管理帖子分组
        /// </summary>
        ManageThreadCategories,

        /// <summary>
        /// 编辑帖吧的描述
        /// </summary>
        EditSectionInfo
    }

}
