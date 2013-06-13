//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Mvc;
using Tunynet.UI;
using Tunynet;
using Tunynet.Common;
using System.Collections.Generic;
using Tunynet.Utilities;
using System.Linq;
using Spacebuilder.Common;
using Tunynet.Mvc;
using Spacebuilder.Group;
using Spacebuilder.Bar.Search;
using Spacebuilder.Search;
using Tunynet.Search;

namespace Spacebuilder.Bar.Controllers
{
    /// <summary>
    /// 群组贴吧管理
    /// </summary>
    [TitleFilter(IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.GroupSpace, IsApplication = true)]
    [AnonymousBrowseCheck]
    [GroupSpaceAuthorize]
    public class GroupSpaceBarController : Controller
    {
        private Authorizer authorizer = new Authorizer();
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private BarThreadService barThreadService = new BarThreadService();
        private BarSectionService barSectionService = new BarSectionService();
        private CategoryService categoryService = new CategoryService();
        private BarSettings barSettings = DIContainer.Resolve<IBarSettingsManager>().Get();
        private BarPostService barPostService = new BarPostService();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private GroupService groupService = new GroupService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().BarThread());


        #region 详细显示
        /// <summary>
        /// 帖子详细显示页面
        /// </summary>
        /// <param name="threadId">帖子id</param>
        /// <param name="pageIndex">回帖页码</param>
        /// <param name="onlyLandlord">只看楼主</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>帖子详细显示页面</returns>
        [HttpGet]
        public ActionResult Detail(string spaceKey, long threadId, int pageIndex = 1, bool onlyLandlord = false, SortBy_BarPost sortBy = SortBy_BarPost.DateCreated, long? postId = null, long? childPostIndex = null)
        {
            BarThread barThread = barThreadService.Get(threadId);
            if (barThread == null)
                return HttpNotFound();

            GroupEntity group = groupService.Get(spaceKey);
            if (group == null)
                return HttpNotFound();
            BarSection section = barSectionService.Get(barThread.SectionId);
            if (section == null || section.TenantTypeId != TenantTypeIds.Instance().Group())
                return HttpNotFound();

            //验证是否通过审核
            long currentSpaceUserId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (!authorizer.IsAdministrator(BarConfig.Instance().ApplicationId) && barThread.UserId != currentSpaceUserId
                && (int)barThread.AuditStatus < (int)(new AuditService().GetPubliclyAuditStatus(BarConfig.Instance().ApplicationId)))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "尚未通过审核",
                    Body = "由于当前帖子尚未通过审核，您无法浏览当前内容。",
                    StatusMessageType = StatusMessageType.Hint
                }));


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
                return Detail(spaceKey, threadId, barPosts.PageCount);
            if (Request.IsAjaxRequest())
                return PartialView("~/Applications/Bar/Views/Bar/_ListPost.cshtml", barPosts);

            ViewData["barThread"] = barThread;
            ViewData["group"] = group;
            return View(barPosts);
        }

        /// <summary>
        /// 帖吧详细显示页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SectionDetail(string spaceKey, long? categoryId = null, bool? isEssential = null, SortBy_BarThread? sortBy = null, bool? isPosted = null, int pageIndex = 1)
        {
            IUser currentUser = UserContext.CurrentUser;
            long sectionId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            BarSection barSection = barSectionService.Get(sectionId);
            if (barSection == null)
                return HttpNotFound();

            if (barSection.AuditStatus != AuditStatus.Success && !new Authorizer().BarSection_Manage(barSection))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "没有权限",
                    Body = "此群组还未通过审核，不能查看",
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            PagingDataSet<BarThread> pds = barThreadService.Gets(sectionId, categoryId, isEssential, sortBy ?? SortBy_BarThread.LastModified_Desc, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("~/Applications/Bar/Views/Bar/_List.cshtml", pds);
            pageResourceManager.InsertTitlePart(barSection.Name);
            Category currentThreadCategory = null;
            if (categoryId.HasValue && categoryId.Value > 0)
                currentThreadCategory = categoryService.Get(categoryId.Value);
            if (currentThreadCategory != null)
            {
                ViewData["currentThreadCategory"] = currentThreadCategory;
            }

            //若当前用户是在浏览自己的帖子列表，或者是管理员，则忽略审核；
            bool ignoreAudit = currentUser != null && UserContext.CurrentUser.UserId == currentUser.UserId || new Authorizer().IsAdministrator(BarConfig.Instance().ApplicationId);
            if (isPosted.HasValue)
            {
                pds = barThreadService.GetUserThreads(TenantTypeIds.Instance().Group(), currentUser.UserId, ignoreAudit, isPosted.Value, pageIndex, sectionId);
            }

            ViewData["section"] = barSection;
            ViewData["threadCategories"] = categoryService.GetOwnerCategories(sectionId, TenantTypeIds.Instance().BarThread());
            ViewData["sortBy"] = sortBy;
            return View(pds);
        }
        #endregion

        #region 编辑的方法
        /// <summary>
        /// 编辑帖子
        /// </summary>
        /// <param name="spaceKey">群组名</param>
        /// <param name="threadId">帖子id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string spaceKey, long? threadId)
        {
            long sectionId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            BarSection section = barSectionService.Get(sectionId);
            if (UserContext.CurrentUser == null)
                return Redirect(SiteUrls.Instance().Login(true));

            if (section == null)
                return HttpNotFound();

            GroupEntity group = groupService.Get(spaceKey);
            if (group == null)
                return HttpNotFound();

            pageResourceManager.InsertTitlePart(section.Name);

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

            ViewData["group"] = group;
            ViewData["BarSection"] = section;
            return View("Edit", barThread == null ? new BarThreadEditModel { SectionId = sectionId } : barThread.AsEditModel());
        }

        /// <summary>
        /// 编辑回帖页面
        /// </summary>
        /// <param name="spaceKey">群组名</param>
        /// <param name="postId">回帖id</param>
        /// <returns>编辑回帖页面</returns>
        [HttpGet]
        public ActionResult EditPost(long threadId, long? postId)
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
            pageResourceManager.InsertTitlePart(thread.BarSection.Name);

            BarPost post = null;
            if (postId.HasValue && postId.Value > 0)
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
                if (post == null)
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "没有找到你要编辑的回帖",
                        Title = "没有找到回帖"
                    }));
                }
                pageResourceManager.InsertTitlePart("编辑回帖");
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
                pageResourceManager.InsertTitlePart("发表回帖");
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

            ViewData["PostBodyMaxLength"] = barSettings.PostBodyMaxLength;

            postModel.SectionId = thread.SectionId;
            return View(postModel);
        }
        #endregion

        #region 列表页面
        /// <summary>
        /// 用户帖子列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="isPosted">是否是回帖</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="spaceKey">群组名</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserThreads(string spaceKey, bool isPosted = false, int pageIndex = 1)
        {
            IUser currentUser = UserContext.CurrentUser;
            long sectionId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);

            //若当前用户是在浏览自己的帖子列表，或者是管理员，则忽略审核；
            bool ignoreAudit = currentUser != null && UserContext.CurrentUser.UserId == currentUser.UserId || new Authorizer().IsAdministrator(BarConfig.Instance().ApplicationId);
            PagingDataSet<BarThread> pds = barThreadService.GetUserThreads(TenantTypeIds.Instance().Group(), currentUser.UserId, ignoreAudit, isPosted, pageIndex, sectionId);
            if (Request.IsAjaxRequest())
                return PartialView("~/Applications/Bar/Views/Bar/_List.cshtml", pds);
            var group = groupService.Get(spaceKey);
            pageResourceManager.InsertTitlePart(group.GroupName);
            string title = isPosted ? "我的回帖" : "我的帖子";
            if (UserContext.CurrentUser != null && UserContext.CurrentUser.UserId != currentUser.UserId)
            {
                title = isPosted ? currentUser.DisplayName + "的回帖" : currentUser.DisplayName + "的帖子";
                ViewData["isOwner"] = false;
            }
            pageResourceManager.InsertTitlePart(title);
            ViewData["userId"] = currentUser != null ? currentUser.UserId : 0;
            return View(pds);
        }

        /// <summary>
        /// 标签显示帖子列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ListByTag(string spaceKey, string tagName, SortBy_BarThread? sortBy, bool? isEssential, int pageIndex = 1)
        {
            tagName = WebUtility.UrlDecode(tagName);
            PagingDataSet<BarThread> pds = barThreadService.Gets(TenantTypeIds.Instance().Group(), tagName, isEssential, sortBy ?? SortBy_BarThread.StageHitTimes, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("~/Applications/Bar/Views/Bar/_List.cshtml", pds);
            var group = groupService.Get(spaceKey);
            if (group == null)
                return HttpNotFound();
            pageResourceManager.InsertTitlePart(group.GroupName);
            pageResourceManager.InsertTitlePart(tagName);
            ViewData["sortBy"] = sortBy;
            ViewData["tagName"] = tagName;

            ViewData["SectionId"] = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            ViewData["group"] = group;
            return View(pds);
        }
        #endregion

        #region 管理页面
        /// <summary>
        /// 前台管理帖吧页面（管理帖子）
        /// </summary>
        /// <param name="model">用户填充的实体</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>后台管理帖吧页面</returns>
        [HttpGet]
        public ActionResult ManageThreads(string spaceKey, ManageThreadEditModel model, int pageIndex = 1)
        {
            long groupId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            BarSection section = barSectionService.Get(groupId);
            if (!new Authorizer().BarSection_Manage(section))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = string.Format("您没有权限管理 {0} ！", section == null ? "" : section.Name),
                    Title = "没有权限",
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            var group = groupService.Get(spaceKey);
            pageResourceManager.InsertTitlePart(group.GroupName);

            pageResourceManager.InsertTitlePart("帖吧管理");

            List<SelectListItem> SelectListItem_TrueAndFlase = new List<SelectListItem> { new SelectListItem { Text = "是", Value = true.ToString() }, new SelectListItem { Text = "否", Value = false.ToString() } };

            ViewData["IsEssential"] = new SelectList(SelectListItem_TrueAndFlase, "Value", "Text", model.IsEssential);
            ViewData["IsSticky"] = new SelectList(SelectListItem_TrueAndFlase, "Value", "Text", model.IsSticky);

            IEnumerable<Category> categories = categoryService.GetOwnerCategories(section.SectionId, TenantTypeIds.Instance().BarThread());
            ViewData["CategoryId"] = new SelectList(categories.Select(n => new { text = StringUtility.Trim(n.CategoryName, 20), value = n.CategoryId }), "value", "text", model.CategoryId);

            BarThreadQuery query = model.GetBarThreadQuery();
            query.SectionId = section.SectionId;
            ViewData["BarThreads"] = barThreadService.Gets(TenantTypeIds.Instance().Group(), query, model.PageSize ?? 20, pageIndex);

            model.SectionId = section.SectionId;

            ViewData["TenantType"] = new TenantTypeService().Get(TenantTypeIds.Instance().Group());

            return View(model);
        }

        /// <summary>
        /// 管理回帖页面
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="model">回帖管理的model</param>
        /// <returns>管理回帖</returns>
        [HttpGet]
        public ActionResult ManagePosts(string spaceKey, ManagePostsEditModel model, int pageIndex = 1)
        {
            long sectionId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            BarSection section = barSectionService.Get(sectionId);

            if (!new Authorizer().BarSection_Manage(section))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "没有权限",
                    Body = "您可能没有权限编辑此帖吧"
                }));
            }

            var group = groupService.Get(spaceKey);
            pageResourceManager.InsertTitlePart(group.GroupName);

            pageResourceManager.InsertTitlePart("回帖管理");

            BarPostQuery query = model.AsBarPostQuery();
            query.SectionId = section.SectionId;

            model.SectionId = section.SectionId;

            ViewData["BarPosts"] = barPostService.Gets(TenantTypeIds.Instance().Group(), query, model.PageSize ?? 20, pageIndex);
            return View(model);
        }

        /// <summary>
        /// 管理帖子类别
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageThreadCategories(string spaceKey, int pageIndex = 1)
        {
            long sectionId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);

            if (!new Authorizer().BarSection_Manage(sectionId))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "您没有权限编辑此帖吧的分类",
                    Title = "没有权限"
                }));
            }
            var group = groupService.Get(spaceKey);
            pageResourceManager.InsertTitlePart(group.GroupName);

            pageResourceManager.InsertTitlePart("类别管理");
            ViewData["SectionId"] = sectionId;
            return View(categoryService.GetOwnerCategories(sectionId, TenantTypeIds.Instance().BarThread()));
        }

        #endregion

        #region 辅助方法
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

        #region 局部页面

        /// <summary>
        /// 标签云
        /// </summary>
        /// <param name="spaceKey">群组名</param>
        /// <param name="topNum">前N条数据</param>
        /// <returns>标签云</returns>
        [HttpGet]
        public ActionResult _TagCloud(string spaceKey, int topNum = 20)
        {
            long sectionId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            TagService tagService = new TagService(TenantTypeIds.Instance().BarThread());
            Dictionary<TagInOwner, int> ownerTags = tagService.GetOwnerTopTags(topNum, sectionId);
            return View(ownerTags);
        }

        /// <summary>
        /// 标签云图
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public ActionResult Tags(string spaceKey)
        {
            GroupEntity group = groupService.Get(spaceKey);
            if (group == null)
                return HttpNotFound();
            ViewData["group"] = group;
            return View();
        }

        /// <summary>
        /// 帖子搜索页面
        /// </summary>
        /// <param name="spaceKey">贴吧名称</param>
        /// <returns>帖子搜索页面</returns>
        [HttpGet]
        public ActionResult _BarThreadSearch(string spaceKey)
        {
            long sectionId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            return View(sectionId);
        }
        #endregion

        #region 搜索页面

        /// <summary>
        /// 群组内搜索帖子
        /// </summary>
        /// <param name="spaceKey">群组名</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>群组内搜索帖子</returns>
        public ActionResult Search(string spaceKey, string keyword, BarSearchRange term = BarSearchRange.ALL, int pageIndex = 1)
        {
            long barSectionId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);

            var group = groupService.Get(spaceKey);
            if (group == null)
                return HttpNotFound();

            ViewData["group"] = group;
            BarSection section = barSectionService.Get(barSectionId);
            if (section == null)
                return HttpNotFound();

            ViewData["section"] = section;

            BarFullTextQuery query = new BarFullTextQuery();

            query.Term = term;

            query.PageIndex = pageIndex;
            query.PageSize = 20;//每页记录数
            query.Keyword = keyword;
            query.Range = section.SectionId.ToString();

            //获取群组贴吧的帖子
            query.TenantTypeId = TenantTypeIds.Instance().Group();

            //根据帖吧id查询帖吧名字
            query.TenantTypeId = section.TenantTypeId;
            ViewData["barname"] = section.Name;
            ViewData["TenantTypeId"] = section.TenantTypeId;

            //调用搜索器进行搜索
            BarSearcher BarSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
            PagingDataSet<BarEntity> BarEntities = BarSearcher.Search(query);

            if (Request.IsAjaxRequest())
                return View("~/Applications/Bar/Views/Bar/_ListSearchThread.cshtml", BarEntities);

            //设置页面Meta
            if (string.IsNullOrWhiteSpace(query.Keyword))
            {
                pageResourceManager.InsertTitlePart("群组帖子搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart('“' + query.Keyword + '”' + "的相关帖子");//设置页面Title
            }

            pageResourceManager.SetMetaOfKeywords("帖吧搜索");//设置Keyords类型的Meta
            pageResourceManager.SetMetaOfDescription("帖吧搜索");//设置Description类型的Meta

            return View(BarEntities);
        }

        #endregion

    }

    /// <summary>
    /// 群组帖吧管理菜单
    /// </summary>
    public enum ManageSubMenu
    {
        /// <summary>
        /// 管理帖子
        /// </summary>
        ManageThread,

        /// <summary>
        /// 管理回帖
        /// </summary>
        ManagePost,

        /// <summary>
        /// 管理分组
        /// </summary>
        ManageCategroy
    }
}