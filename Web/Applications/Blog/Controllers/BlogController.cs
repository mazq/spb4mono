//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.UI;
using Tunynet.Utilities;

namespace Spacebuilder.Blog.Controllers
{
    /// <summary>
    /// 日志控制器
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.UserSpace, IsApplication = true)]
    [AnonymousBrowseCheck]
    [UserSpaceAuthorize]
    public partial class BlogController : Controller
    {
        private AttachmentService<Attachment> attachmentService = new AttachmentService<Attachment>(TenantTypeIds.Instance().BlogThread());
        private AttachmentDownloadService attachmentDownloadService = new AttachmentDownloadService();
        private Authorizer authorizer = new Authorizer();
        private BlogService blogService = new BlogService();
        private BlogSettings blogSettings = DIContainer.Resolve<IBlogSettingsManager>().Get();
        private CategoryService categoryService = new CategoryService();
        private ContentPrivacyService contentPrivacyService = new ContentPrivacyService();
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private TagService tagService = new TagService(TenantTypeIds.Instance().BlogThread());
        private SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().BlogThread());
        private OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());


        #region 日志增删改

        /// <summary>
        /// 写日志/编辑日志（页面）  
        /// </summary>
        /// <param name="threadId">日志id，非空时为编辑操作</param>
        /// <param name="ownerId">所属id，非空时代表群组的日志</param>
        [HttpGet]
        public ActionResult Edit(string spaceKey, long? threadId, long? ownerId)
        {
            BlogThreadEditModel model = null;
            BlogThread blogThread = null;

            //日志用户分类下拉列表
            IEnumerable<Category> ownerCategories = null;

            //写日志
            if (!threadId.HasValue)
            {
                string errorMessage = string.Empty;
                if (!authorizer.BlogThread_Create(spaceKey, out errorMessage))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Title = "没有权限",
                        Body = errorMessage,
                        StatusMessageType = StatusMessageType.Error
                    }));
                }

                model = new BlogThreadEditModel() { PrivacyStatus = PrivacyStatus.Public };
                if (ownerId.HasValue)
                {
                    model.OwnerId = ownerId;
                }

                //获取所有者分类
                if (ownerId.HasValue)
                {
                    ownerCategories = categoryService.GetOwnerCategories(ownerId.Value, TenantTypeIds.Instance().BlogThread());
                }
                else
                {
                    ownerCategories = categoryService.GetOwnerCategories(UserContext.CurrentUser.UserId, TenantTypeIds.Instance().BlogThread());
                }

                pageResourceManager.InsertTitlePart("写日志");
            }

            //编辑日志
            else
            {
                blogThread = blogService.Get(threadId.Value);
                if (blogThread == null)
                {
                    return HttpNotFound();
                }

                if (!authorizer.BlogThread_Edit(blogThread))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Title = "没有权限",
                        Body = "没有权限编辑" + blogThread.Subject + "！",
                        StatusMessageType = StatusMessageType.Error
                    }));
                }

                Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> privacySpecifyObjects = contentPrivacyService.GetPrivacySpecifyObjects(TenantTypeIds.Instance().BlogThread(), blogThread.ThreadId);
                if (privacySpecifyObjects.ContainsKey(SpecifyObjectTypeIds.Instance().User()))
                {
                    IEnumerable<ContentPrivacySpecifyObject> userPrivacySpecifyObjects = privacySpecifyObjects[SpecifyObjectTypeIds.Instance().User()];
                    ViewData["userPrivacySpecifyObjects"] = string.Join(",", userPrivacySpecifyObjects.Select(n => n.SpecifyObjectId));
                }
                if (privacySpecifyObjects.ContainsKey(SpecifyObjectTypeIds.Instance().UserGroup()))
                {
                    IEnumerable<ContentPrivacySpecifyObject> userGroupPrivacySpecifyObjects = privacySpecifyObjects[SpecifyObjectTypeIds.Instance().UserGroup()];
                    ViewData["userGroupPrivacySpecifyObjects"] = string.Join(",", userGroupPrivacySpecifyObjects.Select(n => n.SpecifyObjectId));
                }

                model = blogThread.AsEditModel();

                //获取所有者分类
                ownerCategories = categoryService.GetOwnerCategories(blogThread.OwnerId, TenantTypeIds.Instance().BlogThread());

                IEnumerable<Category> selectedOwnerCategories = blogThread.OwnerCategories;
                Dictionary<long, Category> ownerCategoryDic = new Dictionary<long, Category>();
                if (selectedOwnerCategories != null && selectedOwnerCategories.Count() > 0)
                {
                    ownerCategoryDic = selectedOwnerCategories.ToDictionary(n => n.CategoryId, n => n);
                }
                ViewData["ownerCategoryDic"] = ownerCategoryDic;

                pageResourceManager.InsertTitlePart("编辑日志");
            }

            ViewData["ownerCategories"] = ownerCategories;

            //日志站点分类下拉列表（投稿到）
            if (blogSettings.AllowSetSiteCategory)
            {
                IEnumerable<Category> siteCategories = categoryService.GetOwnerCategories(0, TenantTypeIds.Instance().BlogThread());
                ViewData["siteCategories"] = new SelectList(siteCategories, "CategoryId", "CategoryName", blogThread == null ? null : blogThread.SiteCategoryId);
            }

            return View(model);
        }

        /// <summary>
        /// 写日志/编辑日志（保存表单）
        /// </summary>
        [HttpPost]
        public ActionResult Edit(string spaceKey, BlogThreadEditModel model)
        {
            string errorMessage = string.Empty;
            if (ModelState.HasBannedWord(out errorMessage))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "发布失败",
                    Body = errorMessage,
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            BlogThread blogThread = null;

            //写日志
            if (model.ThreadId == 0)
            {
                if (!authorizer.BlogThread_Create(spaceKey, out errorMessage))
                {
                    if (model.IsDraft)
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "没有权限创建新的日志！"));
                    }
                    else
                    {
                        return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                        {
                            Title = "没有权限",
                            Body = errorMessage,
                            StatusMessageType = StatusMessageType.Error
                        }));
                    }
                }

                blogThread = model.AsBlogThread();
                bool isCreated = blogService.Create(blogThread);

                if (!isCreated)
                {
                    if (model.IsDraft)
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "发布失败，请稍后再试！"));
                    }
                    else
                    {
                        return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                        {
                            Title = "发布失败",
                            Body = "发布失败，请稍后再试！",
                            StatusMessageType = StatusMessageType.Error
                        }));
                    }
                }
            }
            //编辑日志
            else
            {
                blogThread = model.AsBlogThread();

                if (!authorizer.BlogThread_Edit(blogThread))
                {
                    if (model.IsDraft)
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "没有权限编辑" + blogThread.Subject + "！"));
                    }
                    else
                    {
                        return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                        {
                            Title = "没有权限",
                            Body = "没有权限编辑" + blogThread.Subject + "！",
                            StatusMessageType = StatusMessageType.Error
                        }));
                    }
                }

                //如果之前是草稿，现在正式发布，那么需要先删除草稿，然后创建新的日志
                BlogThread oldBlogThread = blogService.Get(blogThread.ThreadId);
                if (oldBlogThread.IsDraft && !blogThread.IsDraft)
                {
                    blogService.Delete(oldBlogThread);
                    bool isCreated = blogService.Create(blogThread);
                }
                else
                {
                    blogService.Update(blogThread);

                    //清除用户分类
                    categoryService.ClearCategoriesFromItem(blogThread.ThreadId, blogThread.OwnerId, TenantTypeIds.Instance().BlogThread());

                    if (blogSettings.AllowSetSiteCategory)
                    {
                        //清除站点分类（投稿到）
                        categoryService.ClearCategoriesFromItem(blogThread.ThreadId, 0, TenantTypeIds.Instance().BlogThread());
                    }

                    //清除标签
                    tagService.ClearTagsFromItem(blogThread.ThreadId, blogThread.UserId);
                }
            }

            //设置隐私状态
            UpdatePrivacySettings(blogThread, model.PrivacyStatus1, model.PrivacyStatus2);

            //设置用户分类
            if (!string.IsNullOrEmpty(model.OwnerCategoryIds))
            {
                string[] ownerCategoryIds = model.OwnerCategoryIds.TrimEnd(',').Split(',');
                categoryService.AddCategoriesToItem(ownerCategoryIds.Select(n => long.Parse(n)), blogThread.ThreadId, blogThread.OwnerId);
            }

            if (blogSettings.AllowSetSiteCategory)
            {
                //设置站点分类（投稿到）
                if (model.SiteCategoryId.HasValue)
                {
                    categoryService.AddCategoriesToItem(new List<long> { model.SiteCategoryId.Value }, blogThread.ThreadId, 0);
                }
            }

            string tags = string.Join(",", model.TagNames);
            if (!string.IsNullOrEmpty(tags))
            {
                tagService.AddTagsToItem(tags, blogThread.UserId, blogThread.ThreadId);
            }

            //如果是保存草稿，则返回Json
            if (blogThread.IsDraft)
            {
                return Json(new { MessageType = StatusMessageType.Success, MessageContent = "保存成功！", ThreadId = blogThread.ThreadId });
            }
            else
            {
                return Redirect(SiteUrls.Instance().BlogDetail(spaceKey, blogThread.ThreadId));
                //return Json(new { MessageType = StatusMessageType.Success, MessageContent = SiteUrls.Instance().BlogDetail(spaceKey, blogThread.ThreadId), ThreadId = blogThread.ThreadId });
            }
        }

        /// <summary>
        /// 转载日志（页面）
        /// </summary>
        [HttpGet]
        public ActionResult _Reproduce(string spaceKey, long threadId)
        {
            BlogThread blogThread = blogService.Get(threadId);
            if (blogThread == null)
            {
                return HttpNotFound();
            }

            if (UserContext.CurrentUser.UserId == blogThread.UserId)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "无效操作",
                    Body = "不能转载自己的日志",
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            BlogThreadEditModel model = new BlogThreadEditModel() { PrivacyStatus = PrivacyStatus.Public };
            model.ThreadId = threadId;

            //日志用户分类下拉列表
            IEnumerable<Category> ownerCategories = categoryService.GetOwnerCategories(UserContext.CurrentUser.UserId, TenantTypeIds.Instance().BlogThread());
            ViewData["ownerCategories"] = new SelectList(ownerCategories, "CategoryId", "CategoryName", null);

            return View(model);
        }

        /// <summary>
        /// 转载日志（保存表单）
        /// </summary>
        [HttpPost]
        public ActionResult _Reproduce(string spaceKey, BlogThreadEditModel model)
        {
            IUser currentUser = UserContext.CurrentUser;

            BlogThread blogThread = blogService.Get(model.ThreadId);
            if (blogThread == null)
            {
                return HttpNotFound();
            }

            BlogThread reproducedBlogThread = BlogThread.New();
            reproducedBlogThread.IsDraft = false;
            reproducedBlogThread.IsReproduced = true;
            reproducedBlogThread.Keywords = blogThread.Keywords;
            reproducedBlogThread.Subject = blogThread.Subject;
            reproducedBlogThread.Body = blogThread.GetBody();
            reproducedBlogThread.Summary = blogThread.Summary;
            reproducedBlogThread.UserId = currentUser.UserId;
            reproducedBlogThread.OwnerId = currentUser.UserId;
            reproducedBlogThread.TenantTypeId = TenantTypeIds.Instance().User();
            reproducedBlogThread.Author = currentUser.DisplayName;
            reproducedBlogThread.OriginalAuthorId = blogThread.OriginalAuthorId;
            reproducedBlogThread.PrivacyStatus = model.PrivacyStatus;
            reproducedBlogThread.FeaturedImage = blogThread.FeaturedImage;
            reproducedBlogThread.FeaturedImageAttachmentId = blogThread.FeaturedImageAttachmentId;
            reproducedBlogThread.OriginalThreadId = blogThread.ThreadId;


            //替换附件
            IEnumerable<Attachment> attachments = attachmentService.GetsByAssociateId(blogThread.ThreadId);
            if (attachments != null && attachments.Count() > 0)
            {
                foreach (var attachment in attachments)
                {
                    Attachment newAttachment = null;
                    //如果该附件有售价并且该用户没有购买过(下载过)该附件则不转载附件
                    if (attachment.Price > 0 && !attachmentDownloadService.IsDownloaded(currentUser.UserId, attachment.AttachmentId))
                    {
                        string oldAttach = "[attach:" + attachment.AttachmentId + "]";
                        reproducedBlogThread.Body = reproducedBlogThread.Body.Replace(oldAttach, "");
                    }
                    else
                    {
                        newAttachment = attachmentService.CloneForUser(attachment, currentUser.UserId);
                        string oldAttach = "[attach:" + attachment.AttachmentId + "]";
                        string newAttach = "[attach:" + newAttachment.AttachmentId + "]";
                        reproducedBlogThread.Body = reproducedBlogThread.Body.Replace(oldAttach, newAttach);

                        //替换标题图
                        if (blogThread.FeaturedImageAttachmentId > 0 && blogThread.FeaturedImageAttachmentId == attachment.AttachmentId)
                        {
                            reproducedBlogThread.FeaturedImage = newAttachment.GetRelativePath() + "\\" + newAttachment.FileName;
                            reproducedBlogThread.FeaturedImageAttachmentId = newAttachment.AttachmentId;
                        }
                    }
                }
            }

            bool isCreated = blogService.Create(reproducedBlogThread);
            if (!isCreated)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "发布失败",
                    Body = "发布失败，请稍后再试！",
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            //设置隐私状态
            UpdatePrivacySettings(reproducedBlogThread, model.PrivacyStatus1, model.PrivacyStatus2);

            //设置用户分类
            if (!string.IsNullOrEmpty(model.OwnerCategoryIds))
            {
                string[] ownerCategoryIds = model.OwnerCategoryIds.TrimEnd(',').Split(',');
                categoryService.AddCategoriesToItem(ownerCategoryIds.Select(n => long.Parse(n)), reproducedBlogThread.ThreadId, reproducedBlogThread.OwnerId);
            }

            //设置标签（如此处理是因为标签选择器会输出两个同名的hidden input）
            if (model.TagNames != null && model.TagNames.Count() == 2 && !string.IsNullOrEmpty(model.TagNames.ElementAt(1)))
            {
                tagService.AddTagsToItem(model.TagNames.ElementAt(1), reproducedBlogThread.UserId, reproducedBlogThread.ThreadId);
            }

            return Redirect(SiteUrls.Instance().BlogDetail(currentUser.UserName, reproducedBlogThread.ThreadId));
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="threadIds">日志id列表</param>
        [HttpPost]
        public JsonResult _Delete(string spaceKey, IEnumerable<long> threadIds)
        {
            foreach (var threadId in threadIds)
            {
                BlogThread blogThread = blogService.Get(threadId);
                if (authorizer.BlogThread_Delete(blogThread))
                {
                    blogService.Delete(blogThread);
                }
                else
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "无权操作！"));
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "删除日志成功！"));
        }

        /// <summary>
        /// 置顶/取消置顶
        /// </summary>
        /// <param name="isSticky">置顶为true；取消置顶为false</param>
        public JsonResult _BlogSetSticky(string spaceKey, long threadId, bool isSticky)
        {
            BlogThread blogThread = blogService.Get(threadId);
            if (blogThread == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "无效操作！"));
            }

            if (!new Authorizer().BlogThread_Edit(blogThread))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "没有权限！"));
            }

            blogService.SetSticky(threadId, isSticky);

            string message = isSticky ? "置顶成功！" : "取消置顶成功！";

            return Json(new StatusMessageData(StatusMessageType.Success, message));
        }

        #endregion

        #region 日志列表及详情页

        /// <summary>
        /// 用户空间日志首页
        /// </summary>
        public ActionResult Home(string spaceKey)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null || currentUser.UserName != spaceKey)
            {
                return Redirect(SiteUrls.Instance().Blog(spaceKey));
            }

            pageResourceManager.InsertTitlePart("日志首页");
            return View();
        }

        /// <summary>
        /// 我的日志/Ta的日志
        /// </summary>
        public ActionResult Blog(string spaceKey, int pageIndex = 1)
        {
            PagingDataSet<BlogThread> blogs = null;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserName == spaceKey)
            {
                blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), currentUser.UserId, true, false, null, null, true, 20, pageIndex);

                pageResourceManager.InsertTitlePart("我的日志");
                return View("My", blogs);
            }
            else
            {
                User user = userService.GetFullUser(spaceKey);
                if (user == null)
                {
                    return HttpNotFound();
                }

                blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), user.UserId, false, true, null, null, true, 20, pageIndex);

                pageResourceManager.InsertTitlePart(user.DisplayName + "的日志");
                return View("Ta", blogs);
            }
        }

        /// <summary>
        /// 日志详细页
        /// </summary>
        public ActionResult Detail(string spaceKey, long threadId)
        {
            BlogThread blogThread = blogService.Get(threadId);

            if (blogThread == null)
            {
                return HttpNotFound();
            }

            if (!authorizer.BlogThread_View(blogThread))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "没有权限",
                    Body = "由于空间主人的权限设置，您无法浏览当前内容。",
                    StatusMessageType = StatusMessageType.Hint
                }));
            }

            long currentSpaceUserId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (!authorizer.IsAdministrator(BlogConfig.Instance().ApplicationId) && blogThread.UserId != currentSpaceUserId
                && (int)blogThread.AuditStatus < (int)(new AuditService().GetPubliclyAuditStatus(BlogConfig.Instance().ApplicationId)))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "尚未通过审核",
                    Body = "由于当前日志尚未通过审核，您无法浏览当前内容。",
                    StatusMessageType = StatusMessageType.Hint
                }));

            //附件信息
            IEnumerable<Attachment> attachments = attachmentService.GetsByAssociateId(threadId);
            if (attachments != null && attachments.Count() > 0)
            {
                ViewData["attachments"] = attachments.Where(n => n.MediaType == MediaType.Other);
            }

            //更新浏览计数
            CountService countService = new CountService(TenantTypeIds.Instance().BlogThread());
            countService.ChangeCount(CountTypes.Instance().HitTimes(), blogThread.ThreadId, blogThread.UserId, 1, false);

            //设置SEO信息
            pageResourceManager.InsertTitlePart(blogThread.Author);
            pageResourceManager.InsertTitlePart(blogThread.ResolvedSubject);

            List<string> keywords = new List<string>();
            keywords.AddRange(blogThread.TagNames);
            keywords.AddRange(blogThread.OwnerCategoryNames);
            string keyword = string.Join(" ", keywords.Distinct());
            if (!string.IsNullOrEmpty(blogThread.Keywords))
            {
                keyword += " " + blogThread.Keywords;
            }

            pageResourceManager.SetMetaOfKeywords(keyword);

            if (!string.IsNullOrEmpty(blogThread.Summary))
            {
                pageResourceManager.SetMetaOfDescription(HtmlUtility.StripHtml(blogThread.Summary, true, false));
            }
            
            
            return View(blogThread);
        }

        /// <summary>
        /// 相关日志（启用输出缓存）
        /// </summary>
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Server)]
        public ActionResult _Related(string spaceKey, long threadId)
        {
            BlogThread blogThread = blogService.Get(threadId);

            //获取标题、标签、用户分类组成的关键字集合
            List<string> keywords = new List<string>();
            IEnumerable<string> subjectKeywords = blogThread.Keywords.Split(' ', ',', '.', '、', '|', '-', '\\', '/', ':', '：', ';', '；').AsEnumerable();
            if (subjectKeywords.Count() > 0)
            {
                keywords.AddRange(subjectKeywords);
            }
            keywords.AddRange(blogThread.TagNames);
            keywords.AddRange(blogThread.OwnerCategoryNames);

            //调用搜索器进行搜索10条相关日志
            BlogFullTextQuery query = new BlogFullTextQuery();
            query.PageSize = 10;
            query.IsRelationBlog = true;
            query.Keywords = keywords;
            query.CurrentThreadId = threadId;
            BlogSearcher blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
            PagingDataSet<BlogThread> blogThreads = blogSearcher.Search(query);

            return View(blogThreads);
        }

        /// <summary>
        /// 关注的日志
        /// </summary>
        public ActionResult Subscribed(string spaceKey, int pageIndex = 1)
        {
            PagingDataSet<BlogThread> blogs = new PagingDataSet<BlogThread>(new List<BlogThread>());
            pageResourceManager.InsertTitlePart("我的关注");
            PagingDataSet<long> threadIds = subscribeService.GetPagingObjectIds(UserContext.CurrentUser.UserId, pageIndex);
            if (threadIds != null && threadIds.Count > 0)
            {
                IEnumerable<BlogThread> blogList = blogService.GetBlogThreads(threadIds.ToList());
                blogs = new PagingDataSet<BlogThread>(blogList)
                {
                    TotalRecords = threadIds.TotalRecords,
                    PageSize = threadIds.PageSize,
                    PageIndex = threadIds.PageIndex,
                    QueryDuration = threadIds.QueryDuration
                };
            }

            return View(blogs);
        }

        /// <summary>
        /// 日志列表
        /// </summary>
        public ActionResult List(string spaceKey, ListType listType, string tag = null, int year = 0, int month = 0, long categoryId = 0, int pageIndex = 1)
        {
            PagingDataSet<BlogThread> blogs = null;
            IUser currentUser = UserContext.CurrentUser;
            string title = string.Empty;

            switch (listType)
            {
                case ListType.Archive:
                    ArchivePeriod archivePeriod = ArchivePeriod.Year;
                    if (month > 0)
                    {
                        archivePeriod = ArchivePeriod.Month;
                    }

                    ArchiveItem archiveItem = new ArchiveItem();
                    archiveItem.Year = year;
                    archiveItem.Month = month;

                    if (currentUser != null && currentUser.UserName == spaceKey)
                    {
                        blogs = blogService.GetsForArchive(TenantTypeIds.Instance().User(), currentUser.UserId, true, false, archivePeriod, archiveItem, 20, pageIndex);
                    }
                    else
                    {
                        blogs = blogService.GetsForArchive(TenantTypeIds.Instance().User(), UserIdToUserNameDictionary.GetUserId(spaceKey), false, true, archivePeriod, archiveItem, 20, pageIndex);
                    }

                    title = "归档：" + year + "年";
                    if (month > 0)
                    {
                        title += month + "月";
                    }

                    break;

                case ListType.Category:
                    if (currentUser != null && currentUser.UserName == spaceKey)
                    {
                        blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), currentUser.UserId, true, false, categoryId, null, false, 20, pageIndex);
                    }
                    else
                    {
                        blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), UserIdToUserNameDictionary.GetUserId(spaceKey), false, true, categoryId, null, false, 20, pageIndex);
                    }

                    Category category = categoryService.Get(categoryId);
                    title = "分类：" + category.CategoryName;

                    break;

                case ListType.Tag:
                    if (currentUser != null && currentUser.UserName == spaceKey)
                    {
                        blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), currentUser.UserId, true, false, null, tag, false, 20, pageIndex);
                    }
                    else
                    {
                        blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), UserIdToUserNameDictionary.GetUserId(spaceKey), false, true, null, tag, false, 20, pageIndex);
                    }

                    title = "标签：" + tag;

                    break;

                default:
                    break;
            }

            ViewData["title"] = title;
            pageResourceManager.InsertTitlePart(title);

            return View(blogs);
        }

        #endregion

        #region 侧边栏及局部页

        /// <summary>
        /// 用户空间日志左侧控制面板
        /// </summary>
        /// <param name="menu">菜单项标识</param>
        public ActionResult _Panel(string spaceKey, string menu = null)
        {
            User user = userService.GetFullUser(spaceKey);
            ViewData["user"] = user;

            //用户日志数
            long threadCount = ownerDataService.GetLong(user.UserId, OwnerDataKeys.Instance().ThreadCount());
            ViewData["threadCount"] = threadCount;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserId != user.UserId)
            {
                bool followed = new FollowService().IsFollowed(currentUser.UserId, user.UserId);
                ViewData["followed"] = followed;
            }

            return View();
        }

        /// <summary>
        /// 用户空间日志左侧日志分类
        /// </summary>
        public ActionResult _Categories(string spaceKey)
        {
            IEnumerable<Category> categories = null;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserName == spaceKey)
            {
                categories = categoryService.GetOwnerCategories(currentUser.UserId, TenantTypeIds.Instance().BlogThread());
            }
            else
            {
                categories = categoryService.GetOwnerCategories(UserIdToUserNameDictionary.GetUserId(spaceKey), TenantTypeIds.Instance().BlogThread());
            }

            return View(categories);
        }

        /// <summary>
        /// 用户空间日志左侧标签云
        /// </summary>
        public ActionResult _Tags(string spaceKey, int num = 30)
        {
            Dictionary<TagInOwner, int> tags = null;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserName == spaceKey)
            {
                tags = tagService.GetOwnerTopTags(num, currentUser.UserId);
            }
            else
            {
                IUser user = userService.GetUser(spaceKey);
                tags = tagService.GetOwnerTopTags(num, user.UserId);
            }

            return View(tags);
        }

        /// <summary>
        /// 用户空间日志左侧存档列表
        /// </summary>
        public ActionResult _Archives(string spaceKey)
        {
            IEnumerable<ArchiveItem> archiveItems = null;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserName == spaceKey)
            {
                archiveItems = blogService.GetArchiveItems(TenantTypeIds.Instance().User(), currentUser.UserId, true, false);
            }
            else
            {
                archiveItems = blogService.GetArchiveItems(TenantTypeIds.Instance().User(), UserIdToUserNameDictionary.GetUserId(spaceKey), false, true);
            }

            return View(archiveItems);
        }

        /// <summary>
        /// 侧边栏最新日志
        /// </summary>
        public ActionResult _Newest(string spaceKey, int topNum = 10)
        {
            PagingDataSet<BlogThread> blogs = null;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserName == spaceKey)
            {
                blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), currentUser.UserId, true, false, null, null, false, topNum, 1);
            }
            else
            {
                blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), UserIdToUserNameDictionary.GetUserId(spaceKey), false, true, null, null, false, topNum, 1);
            }

            return View(blogs);
        }

        /// <summary>
        /// Rss订阅（启用输出缓存）
        /// </summary>
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Server)]
        public ContentResult Rss(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
            {
                return Content(string.Empty);
            }
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();

            SyndicationFeed feed = new SyndicationFeed(user.DisplayName + "的日志 - " + siteSettingsManager.Get().SiteName, string.Empty, new Uri(SiteUrls.FullUrl(SiteUrls.Instance().Blog(spaceKey))));
            feed.Authors.Add(new SyndicationPerson(user.DisplayName));
            feed.LastUpdatedTime = DateTime.Now.ConvertToUserDate();

            List<SyndicationItem> items = new List<SyndicationItem>();
            IEnumerable<BlogThread> blogs = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), user.UserId, false, true, null, null, false, blogSettings.RssPageSize, 1);

            foreach (BlogThread blog in blogs)
            {
                string url = SiteUrls.FullUrl(SiteUrls.Instance().BlogDetail(spaceKey, blog.ThreadId));
                string content = string.Empty;
                if (blogSettings.ShowSummaryInRss)
                {
                    if (!string.IsNullOrEmpty(blog.Summary))
                    {
                        content = blog.Summary + "......<a href='" + url + "'>>>点击查看日志原文</a>";
                    }
                }
                else
                {
                    content = blog.GetResolvedBody();
                }

                SyndicationItem item = new SyndicationItem(blog.ResolvedSubject, content, new Uri(url), blog.ThreadId.ToString(), blog.DateCreated.ConvertToUserDate());
                //item.BaseUri =new Uri(WebUtility.HostPath(Request.Url));
                item.Authors.Add(new SyndicationPerson(blog.Author));
                IEnumerable<string> ownerCategoryNames = blog.OwnerCategoryNames;
                if (ownerCategoryNames != null)
                {
                    foreach (string ownerCategoryName in ownerCategoryNames)
                    {
                        item.Categories.Add(new SyndicationCategory(ownerCategoryName));
                    }
                }
                item.PublishDate = blog.DateCreated.ConvertToUserDate();

                items.Add(item);
            }
            feed.Items = items;

            //输出XML
            string rss = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriter rssWriter = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
                Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(feed);
                rssFormatter.WriteTo(rssWriter);
                rssWriter.Close();
                rss = Encoding.UTF8.GetString(stream.ToArray());
            }

            return Content(rss, "text/xml");
        }

        #endregion

        #region 日志管理及草稿箱

        /// <summary>
        /// 管理日志（主人）
        /// </summary>
        public ActionResult Manage(string spaceKey, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("日志管理");
            IUser currentUser = UserContext.CurrentUser;
            PagingDataSet<BlogThread> blogThread = blogService.GetOwnerThreads(TenantTypeIds.Instance().User(), currentUser.UserId, true, false, null, null, false, 15, pageIndex);

            return View(blogThread);
        }

        /// <summary>
        /// 草稿箱（主人）
        /// </summary>
        public ActionResult Draft(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("草稿箱");
            IUser currentUser = UserContext.CurrentUser;
            IEnumerable<BlogThread> blogThread = blogService.GetDraftThreads(TenantTypeIds.Instance().User(), currentUser.UserId);

            return View(blogThread);
        }

        /// <summary>
        /// 设置分类（页面）
        /// </summary>
        [HttpGet]
        public ActionResult _SetCategories(string spaceKey, IEnumerable<long> threadIds)
        {
            IUser currentUser = UserContext.CurrentUser;
            IEnumerable<Category> categories = categoryService.GetOwnerCategories(currentUser.UserId, TenantTypeIds.Instance().BlogThread());

            ViewData["categories"] = categories;
            ViewData["threadIds"] = threadIds;

            return View();
        }

        /// <summary>
        /// 设置分类（表单提交）
        /// </summary>
        [HttpPost]
        public ActionResult _SetCategories(string spaceKey, IEnumerable<long> threadIds, IEnumerable<long> categoryIds)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (categoryIds != null && categoryIds.Count() > 0)
            {
                foreach (long threadId in threadIds)
                {
                    BlogThread blogThread = blogService.Get(threadId);
                    if (authorizer.BlogThread_Edit(blogThread))
                    {
                        //清除用户分类
                        categoryService.ClearCategoriesFromItem(threadId, currentUser.UserId, TenantTypeIds.Instance().BlogThread());

                        //设置用户分类                    
                        categoryService.AddCategoriesToItem(categoryIds, threadId, currentUser.UserId);
                    }
                    else
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "无权操作！"));
                    }
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "设置分类成功！"));
        }

        /// <summary>
        /// 设置标签（页面）
        /// </summary>
        [HttpGet]
        public ActionResult _SetTags(string spaceKey, IEnumerable<long> threadIds)
        {
            IUser currentUser = UserContext.CurrentUser;
            IEnumerable<TagInOwner> tags = tagService.GetOwnerTags(UserContext.CurrentUser.UserId);

            ViewData["tags"] = tags;
            ViewData["threadIds"] = threadIds;

            return View();
        }

        /// <summary>
        /// 设置标签（表单提交）
        /// </summary>
        [HttpPost]
        public ActionResult _SetTags(string spaceKey, IEnumerable<long> threadIds, IEnumerable<string> tagNames)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (tagNames != null && tagNames.Count() > 0)
            {
                foreach (long threadId in threadIds)
                {
                    BlogThread blogThread = blogService.Get(threadId);
                    if (authorizer.BlogThread_Edit(blogThread))
                    {
                        //清除用户标签
                        tagService.ClearTagsFromItem(threadId, currentUser.UserId);

                        //设置用户标签
                        tagService.AddTagsToItem(tagNames.ToArray(), currentUser.UserId, threadId);
                    }
                    else
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "无权操作！"));
                    }
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "设置标签成功！"));
        }
        #endregion

        #region 关注日志

        /// <summary>
        /// 关注日志按钮
        /// </summary>
        public ActionResult _SubscribeButton(string spaceKey, long threadId, bool isSubscribePage = false)
        {
            if (UserContext.CurrentUser != null)
            {
                ViewData["isSubscribed"] = subscribeService.IsSubscribed(threadId, UserContext.CurrentUser.UserId);
            }

            ViewData["threadId"] = threadId;
            ViewData["isSubscribePage"] = isSubscribePage;

            return View();
        }

        /// <summary>
        /// 关注日志操作
        /// </summary>
        public JsonResult _Subscribe(string spaceKey, long threadId)
        {
            IUser currentUser = UserContext.CurrentUser;

            if (currentUser == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "必须先登录，才能继续操作"));
            }

            BlogThread blogThread = blogService.Get(threadId);

            if (blogThread == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到要被关注的日志"));
            }

            if (subscribeService.IsSubscribed(threadId, currentUser.UserId))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "您已经关注过该日志"));
            }

            subscribeService.Subscribe(threadId, currentUser.UserId);

            return Json(new StatusMessageData(StatusMessageType.Success, "关注成功"));
        }

        /// <summary>
        /// 取消关注日志操作
        /// </summary>
        public JsonResult _SubscribeCancel(string spaceKey, long threadId)
        {
            if (UserContext.CurrentUser == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "必须先登录，才能继续操作"));
            }

            long userId = UserContext.CurrentUser.UserId;

            if (!subscribeService.IsSubscribed(threadId, userId))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有关注过该日志"));
            }

            BlogThread blogThread = blogService.Get(threadId);

            if (blogThread == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到要被关注的日志"));
            }

            subscribeService.CancelSubscribe(threadId, userId);

            return Json(new StatusMessageData(StatusMessageType.Success, "取消关注操作成功"));
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 设置隐私状态
        /// </summary>
        private void UpdatePrivacySettings(BlogThread blogThread, string privacyStatus1, string privacyStatus2)
        {
            Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> privacySpecifyObjects = Utility.GetContentPrivacySpecifyObjects(privacyStatus1, privacyStatus2, ((IPrivacyable)blogThread).TenantTypeId, blogThread.ThreadId);

            if (privacySpecifyObjects.Count > 0)
            {
                contentPrivacyService.UpdatePrivacySettings(blogThread, privacySpecifyObjects);
            }

        }

        #endregion

    }

    /// <summary>
    /// 日志列表类型
    /// </summary>
    public enum ListType
    {
        /// <summary>
        /// 根据标签
        /// </summary>
        Tag = 1,

        /// <summary>
        /// 根据存档
        /// </summary>
        Archive = 2,

        /// <summary>
        /// 根据分类
        /// </summary>
        Category = 3
    }
}