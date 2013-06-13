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
using Tunynet.Search;
using Spacebuilder.Search;
using Tunynet.Mvc;
using Tunynet.Common.Configuration;
using System.Text.RegularExpressions;
using Tunynet.Utilities;

namespace Spacebuilder.Group.Controllers
{
    /// <summary>
    /// 频道群组控制器
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    [AnonymousBrowseCheck]
    [TitleFilter(IsAppendSiteName = true)]
    public class ChannelGroupController : Controller
    {
        private ActivityService activityService = new ActivityService();
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private CategoryService categoryService = new CategoryService();
        private GroupService groupService = new GroupService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Group());
        private Authorizer authorizer = new Authorizer();
        private IdentificationService identificationService = new IdentificationService();
        private RecommendService recommendService = new RecommendService();
        private UserService userService = new UserService();
        private AreaService areaService = new AreaService();
        private FollowService followService = new FollowService();

        /// <summary>
        /// 频道群组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Home()
        {
            pageResourceManager.InsertTitlePart("群组首页");
            return View();
        }

        /// <summary>
        /// 群组顶部的局部页面
        /// </summary>
        /// <returns></returns>
        public ActionResult _GroupSubmenu()
        {
            return View();
        }

        /// <summary>
        /// 验证群组Key的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateGroupKey(string groupKey, long groupId)
        {
            bool result = false;
            if (groupId > 0)
            {
                result = true;
            }
            else
            {
                GroupEntity group = groupService.Get(groupKey);
                if (group != null)
                {
                    return Json("此群组Key已存在", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = Regex.IsMatch(groupKey, @"^[A-Za-z0-9_\-\u4e00-\u9fa5]+$", RegexOptions.IgnoreCase);
                    if (!result)
                    {
                        return Json("只能输入字母数字汉字或-号", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            pageResourceManager.InsertTitlePart("创建群组");
            string errorMessage = null;
            if (!new Authorizer().Group_Create(out errorMessage))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = errorMessage,
                    Title = errorMessage,
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            GroupEditModel group = new GroupEditModel();
            return View(group);
        }

        
        //已修改
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(GroupEditModel groupEditModel)
        {
            string errorMessage = null;
            if (ModelState.HasBannedWord(out errorMessage))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = errorMessage,
                    Title = "创建失败",
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            System.IO.Stream stream = null;
            HttpPostedFileBase groupLogo = Request.Files["GroupLogo"];

            
            //已修改
            IUser user = UserContext.CurrentUser;
            if (user == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));

            if (!new Authorizer().Group_Create(out errorMessage))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = errorMessage,
                    Title = errorMessage,
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            if (groupLogo != null && !string.IsNullOrEmpty(groupLogo.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Group());
                if (!tenantLogoSettings.ValidateFileLength(groupLogo.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return View(groupEditModel);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                if (!logoSettings.ValidateFileExtensions(groupLogo.FileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return View(groupEditModel);
                }
                stream = groupLogo.InputStream;
                groupEditModel.Logo = groupLogo.FileName;
            }
            GroupEntity group = groupEditModel.AsGroupEntity();

            bool result = groupService.Create(user.UserId, group);

            if (stream != null)
            {
                groupService.UploadLogo(group.GroupId, stream);
            }
            //设置分类
            if (groupEditModel.CategoryId > 0)
            {
                categoryService.AddItemsToCategory(new List<long>() { group.GroupId }, groupEditModel.CategoryId, 0);
            }
            //设置标签
            string relatedTags = Request.Form.Get<string>("RelatedTags");
            if (!string.IsNullOrEmpty(relatedTags))
            {
                tagService.AddTagsToItem(relatedTags, group.GroupId, group.GroupId);
            }
            //发送邀请
            if (!string.IsNullOrEmpty(groupEditModel.RelatedUserIds))
            {
                
                //已修改
                IEnumerable<long> userIds = Request.Form.Gets<long>("RelatedUserIds", null);
                groupService.SendInvitations(group, user, string.Empty, userIds);
            }
            return Redirect(SiteUrls.Instance().GroupHome(group.GroupKey));
        }

        #region 群组全文检索

        /// <summary>
        /// 群组搜索
        /// </summary>
        public ActionResult Search(GroupFullTextQuery query)
        {
            query.PageSize = 20;//每页记录数

            //调用搜索器进行搜索
            GroupSearcher groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
            PagingDataSet<GroupEntity> groups = groupSearcher.Search(query);

            //添加到用户搜索历史 
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    SearchHistoryService searchHistoryService = new SearchHistoryService();
                    searchHistoryService.SearchTerm(CurrentUser.UserId, GroupSearcher.CODE, query.Keyword);
                }
            }

            //添加到热词
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(GroupSearcher.CODE, query.Keyword);
            }

            //设置页面Meta
            if (string.IsNullOrWhiteSpace(query.Keyword))
            {
                pageResourceManager.InsertTitlePart("群组搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart('“' + query.Keyword + '”' + "的相关群组");//设置页面Title
            }

            return View(groups);
        }

        /// <summary>
        /// 群组全局搜索
        /// </summary>
        public ActionResult _GlobalSearch(GroupFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;

            //调用搜索器进行搜索
            GroupSearcher groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
            PagingDataSet<GroupEntity> groups = groupSearcher.Search(query);

            return PartialView(groups);
        }

        /// <summary>
        /// 群组快捷搜索
        /// </summary>
        public ActionResult _QuickSearch(GroupFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.Range = GroupSearchRange.GROUPNAME;
            query.Keyword = Server.UrlDecode(query.Keyword);
            //调用搜索器进行搜索
            GroupSearcher GroupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
            PagingDataSet<GroupEntity> groups = GroupSearcher.Search(query);

            return PartialView(groups);
        }

        /// <summary>
        /// 群组搜索自动完成
        /// </summary>
        public JsonResult SearchAutoComplete(string keyword, int topNumber)
        {
            //调用搜索器进行搜索
            GroupSearcher groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
            IEnumerable<string> terms = groupSearcher.AutoCompleteSearch(keyword, topNumber);

            var jsonResult = Json(terms.Select(t => new { tagName = t, tagNameWithHighlight = SearchEngine.Highlight(keyword, string.Join("", t.Take(34)), 100) }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>
        /// 可能感兴趣的群组
        /// </summary>
        public ActionResult _InterestGroup()
        {
            TagService tagService = new TagService(TenantTypeIds.Instance().User());
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null)
            {
                GroupFullTextQuery query = new GroupFullTextQuery();
                query.PageSize = 20;
                query.PageIndex = 1;
                query.Range = GroupSearchRange.TAG;
                query.Tags = tagService.GetTopTagsOfItem(currentUser.UserId, 100).Select(n => n.TagName);
                query.GroupIds = groupService.GetMyJoinedGroups(currentUser.UserId, 100, 1).Select(n => n.GroupId.ToString());
                //调用搜索器进行搜索
                GroupSearcher GroupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
                IEnumerable<GroupEntity> groupsTag = null;
                if (GroupSearcher.Search(query, true).Count == 0)
                {
                    return View();
                }
                else
                {
                    groupsTag = GroupSearcher.Search(query, true).AsEnumerable<GroupEntity>();
                }
                if (groupsTag.Count() < 20)
                {
                    IEnumerable<GroupEntity> groupsFollow = groupService.FollowedUserAlsoJoinedGroups(currentUser.UserId, 20 - groupsTag.Count());
                    return View(groupsTag.Union(groupsFollow));
                }
                else
                {
                    return View(groupsTag);
                }
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region 动态内容块
        /// <summary>
        /// 创建群组动态内容块
        /// </summary>
        [HttpGet]
        public ActionResult _CreateGroup(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);
            GroupEntity group = groupService.Get(activity.SourceId);
            if (group == null)
                return Content(string.Empty);
            ViewData["ActivityId"] = ActivityId;
            return View(group);
        }

        /// <summary>
        /// 用户加入群组动态内容快
        /// </summary>
        /// <param name="ActivityId">动态id</param>
        /// <returns>用户加入群组动态内容快</returns>
        [HttpGet]
        public ActionResult _CreateGroupMember(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);

            GroupEntity group = groupService.Get(activity.ReferenceId);
            if (group == null)
                return Content(string.Empty);
            IUser currentUser = UserContext.CurrentUser;

            IEnumerable<GroupMember> groupMembers = groupService.GetGroupMembers(group.GroupId, true, SortBy_GroupMember.DateCreated_Desc);

            //设置当前登录用户对当前页用户的关注情况
            Dictionary<long, bool> isCurrentUserFollowDic = new Dictionary<long, bool>();
            foreach (GroupMember user in groupMembers)
            {
                //如果当前登录用户关注了该用户
                if (followService.IsFollowed(currentUser == null ? 0 : currentUser.UserId, user.UserId))
                {
                    isCurrentUserFollowDic[user.UserId] = true;
                }
                else
                {
                    isCurrentUserFollowDic[user.UserId] = false;
                }
            }

            ViewData["isCurrentUserFollowDic"] = isCurrentUserFollowDic;
            ViewData["ActivityId"] = ActivityId;
            ViewData["GroupMembers"] = groupMembers;
            ViewData["ActivityUserId"] = activity.UserId;
            return View(group);
        }

        /// <summary>
        /// 用户加入群组动态内容快
        /// </summary>
        /// <param name="ActivityId">动态id</param>
        /// <returns>用户加入群组动态内容快</returns>
        [HttpGet]
        public ActionResult _JoinGroup(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);

            GroupEntity group = groupService.Get(activity.ReferenceId);
            if (group == null)
                return Content(string.Empty);

            ViewData["ActivityId"] = ActivityId;
            ViewData["ownerName"] = activity.OwnerName;
            ViewData["ownerId"] = activity.OwnerId;
            
            return View(group);
        }

        #endregion

        #region 屏蔽群组

        /// <summary>
        /// 屏蔽群组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BlockGroup(long groupId)
        {
            GroupEntity blockedGroup = groupService.Get(groupId);
            if (blockedGroup == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到被屏蔽群组"));
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您还没有登录"));
            new UserBlockService().BlockGroup(currentUser.UserId, blockedGroup.GroupId, blockedGroup.GroupName);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        /// <summary>
        /// 屏蔽群组的post方法
        /// </summary>
        /// <param name="spaceKey">屏蔽的spacekey</param>
        /// <param name="groupIds">被屏蔽的分组名称</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BlockGroups(string spaceKey, List<long> groupIds)
        {
            int addCount = 0;

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            UserBlockService service = new UserBlockService();

            if (userId > 0 && groupIds != null && groupIds.Count > 0)
                foreach (var groupId in groupIds)
                {
                    GroupEntity group = groupService.Get(groupId);
                    if (group == null || service.IsBlockedGroup(userId, groupId))
                        continue;
                    service.BlockGroup(userId, group.GroupId, group.GroupName);
                    addCount++;
                }
            if (addCount > 0)
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, string.Format("成功添加{0}个群组添加到屏蔽列表", addCount));
            else
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "没有任何群组被添加到屏蔽列表中");
            return Redirect(SiteUrls.Instance().BlockGroups(spaceKey));
        }

        /// <summary>
        /// 屏蔽群组
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>屏蔽群组名</returns>
        public ActionResult _BlockGroups(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (UserContext.CurrentUser == null || (UserContext.CurrentUser.UserId != userId && new Authorizer().IsAdministrator(new TenantTypeService().Get(TenantTypeIds.Instance().Group()).ApplicationId)))
                return Content(string.Empty);

            IEnumerable<UserBlockedObject> blockedGroups = new UserBlockService().GetBlockedGroups(userId);

            List<UserBlockedObjectViewModel> blockedObjectes = new List<UserBlockedObjectViewModel>();

            if (blockedGroups != null && blockedGroups.Count() > 0)
            {
                groupService.GetGroupEntitiesByIds(blockedGroups.Select(n => n.ObjectId));
                foreach (var item in blockedGroups)
                {
                    GroupEntity group = groupService.Get(item.ObjectId);
                    if (group == null)
                        continue;

                    UserBlockedObjectViewModel entitiy = item.AsViewModel();
                    entitiy.Logo = group.Logo;
                    blockedObjectes.Add(entitiy);
                }
            }

            return View(blockedObjectes);
        }

        #endregion

        #region 加入群组
        /// <summary>
        /// 申请加入按钮
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>   
        [HttpGet]
        public ActionResult _ApplyJoinButton(long groupId, bool showQuit = false, string buttonName = null)
        {
            
            GroupEntity group = groupService.Get(groupId);
            if (group == null)
                return new EmptyResult();
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return new EmptyResult();
            bool isApplied = groupService.IsApplied(currentUser.UserId, groupId);
            bool isMember = groupService.IsMember(group.GroupId, currentUser.UserId);
            bool isOwner = groupService.IsOwner(group.GroupId, currentUser.UserId);
            bool isManager = groupService.IsManager(group.GroupId, currentUser.UserId);
            ViewData["isMember"] = isMember;
            ViewData["showQuit"] = showQuit;
            ViewData["buttonName"] = buttonName;
            ViewData["isOwner"] = isOwner;
            ViewData["isManager"] = isManager;
            ViewData["isApplied"] = isApplied;
            return View(group);
        }

        /// <summary>
        /// 退出群组
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _QuitGroup(long groupId)
        {
            StatusMessageData message = new StatusMessageData(StatusMessageType.Success, "退出群组成功！");
            GroupEntity group = groupService.Get(groupId);
            if (group == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到群组！"));
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            try
            {
                groupService.DeleteGroupMember(group.GroupId, currentUser.UserId);
            }
            catch
            {
                message = new StatusMessageData(StatusMessageType.Error, "退出群组失败！");
            }
            return Json(message);
        }

        /// <summary>
        /// 用户加入群组（群组无验证时）
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult JoinGroup(long groupId)
        {
            //需判断是否已经加入过群组
            StatusMessageData message = null;
            GroupEntity group = groupService.Get(groupId);
            if (group == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到群组！"));

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            if (group.JoinWay != JoinWay.Direct)
                return Json(new StatusMessageData(StatusMessageType.Error, "当前加入方式不是直接加入"));
            
            //已修改
            
            //判断是否加入过该群组
            bool isMember = groupService.IsMember(groupId, currentUser.UserId);

            //未加入
            if (!isMember)
            {
                GroupMember member = GroupMember.New();
                member.UserId = currentUser.UserId;
                member.GroupId = group.GroupId;
                member.IsManager = false;
                groupService.CreateGroupMember(member);
                message = new StatusMessageData(StatusMessageType.Success, "加入群组成功！");
            }
            else
            {
                message = new StatusMessageData(StatusMessageType.Hint, "您已加入过该群组！");
            }
            return Json(message);
        }

        /// <summary>
        /// 用户加入群组（群组有验证时）
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditApply(long groupId)
        {
            
            //已修改
            
            bool isApplied = groupService.IsApplied(UserContext.CurrentUser.UserId, groupId);
            ViewData["isApplied"] = isApplied;
            return View();
        }

        /// <summary>
        /// 用户加入群组（群组有验证时）
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditApply(long groupId, string applyReason)
        {
            StatusMessageData message = null;
            GroupEntity group = groupService.Get(groupId);
            if (group == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到群组！"));

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            if (group.JoinWay != JoinWay.ByApply)
                return Json(new StatusMessageData(StatusMessageType.Error, "当前加入方式不是需要申请"));

            
            //已修改
            bool isApplied = groupService.IsApplied(currentUser.UserId, group.GroupId);
            if (!isApplied)
            {
                GroupMemberApply apply = GroupMemberApply.New();
                apply.ApplyReason = applyReason;
                apply.ApplyStatus = GroupMemberApplyStatus.Pending;
                apply.GroupId = group.GroupId;
                apply.UserId = UserContext.CurrentUser.UserId;
                groupService.CreateGroupMemberApply(apply);
                message = new StatusMessageData(StatusMessageType.Success, "申请已发出，请等待！");
            }
            else
            {
                message = new StatusMessageData(StatusMessageType.Hint, "您已给该群组发送过申请！");
            }
            return Json(message);
        }

        /// <summary>
        ///  用户加入群组（通过问题验证）
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ValidateQuestion(long groupId)
        {
            GroupEntity group = groupService.Get(groupId);
            ViewData["Question"] = group.Question;
            return View();
        }

        /// <summary>
        /// 用户加入群组（通过问题验证）
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ValidateQuestion(long groupId, string myAnswer)
        {
            StatusMessageData message = null;
            GroupEntity group = groupService.Get(groupId);
            if (group == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到群组！"));

            
            //已修改
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            if (group.JoinWay != JoinWay.ByQuestion)
                return Json(new StatusMessageData(StatusMessageType.Error, "当前加入方式不是问题验证"));


            bool isMember = groupService.IsMember(group.GroupId, currentUser.UserId);
            if (!isMember)
            {
                if (group.Answer == myAnswer)
                {
                    GroupMember member = GroupMember.New();
                    member.UserId = UserContext.CurrentUser.UserId;
                    member.GroupId = group.GroupId;
                    member.IsManager = false;
                    groupService.CreateGroupMember(member);
                    message = new StatusMessageData(StatusMessageType.Success, "加入群组成功！");
                }
                else
                {
                    message = new StatusMessageData(StatusMessageType.Error, "答案错误！");
                }
            }
            else
            {
                message = new StatusMessageData(StatusMessageType.Hint, "您已加入过该群组！");
            }
            return Json(message);
        }

        #endregion

        #region 推荐群组
        /// <summary>
        /// 推荐群组
        /// </summary>
        /// <returns></returns>
        public ActionResult _RecommendedGroup()
        {
            IEnumerable<RecommendItem> recommendItems = recommendService.GetTops(6, "10110001");
            return View(recommendItems);
        }
        #endregion

        #region 页面

        /// <summary>
        /// 发现群组
        /// </summary>
        /// <returns></returns>
        public ActionResult FindGroup(string nameKeyword, string areaCode, long? categoryId, SortBy_Group? sortBy, int pageIndex = 1)
        {
            nameKeyword = WebUtility.UrlDecode(nameKeyword);
            string pageTitle = string.Empty;
            IEnumerable<Category> childCategories = null;
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                var category = categoryService.Get(categoryId.Value);
                if (category != null)
                {
                    
                    
                    if (category.ChildCount > 0)
                    {
                        childCategories = category.Children;
                    }
                    else//若是叶子节点，则取同辈分类
                    {
                        if (category.Parent != null)
                            childCategories = category.Parent.Children;
                    }
                    List<Category> allParentCategories = new List<Category>();
                    //递归获取所有父级类别，若不是叶子节点，则包含其自身
                    RecursiveGetAllParentCategories(category.ChildCount > 0 ? category : category.Parent, ref allParentCategories);
                    ViewData["allParentCategories"] = allParentCategories;
                    ViewData["currentCategory"] = category;
                    pageTitle = category.CategoryName;
                }
            }


            if (childCategories == null)
                childCategories = categoryService.GetRootCategories(TenantTypeIds.Instance().Group());

            ViewData["childCategories"] = childCategories;

            AreaSettings areaSettings = DIContainer.Resolve<IAreaSettingsManager>().Get();
            IEnumerable<Area> childArea = null;
            if (!string.IsNullOrEmpty(areaCode))
            {
                var area = areaService.Get(areaCode);
                if (area != null)
                {
                    
                    
                    if (area.ChildCount > 0)
                    {
                        childArea = area.Children;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(area.ParentCode))
                        {
                            var parentArea = areaService.Get(area.ParentCode);
                            if (parentArea != null)
                                childArea = parentArea.Children;
                        }
                    }
                }
                List<Area> allParentAreas = new List<Area>();
                RecursiveGetAllParentArea(area.ChildCount > 0 ? area : areaService.Get(area.ParentCode), areaSettings.RootAreaCode, ref allParentAreas);
                ViewData["allParentAreas"] = allParentAreas;
                ViewData["currentArea"] = area;
                if (!string.IsNullOrEmpty(pageTitle))
                    pageTitle += ",";
                pageTitle += area.Name;
            }

            if (childArea == null)
            {
                Area rootArea = areaService.Get(areaSettings.RootAreaCode);
                if (rootArea != null)
                    childArea = rootArea.Children;
                else
                    childArea = areaService.GetRoots();
            }

            ViewData["childArea"] = childArea;

            if (!string.IsNullOrEmpty(nameKeyword))
            {
                if (!string.IsNullOrEmpty(pageTitle))
                    pageTitle += ",";
                pageTitle += nameKeyword;
            }

            if (string.IsNullOrEmpty(pageTitle))
                pageTitle = "发现群组";
            pageResourceManager.InsertTitlePart(pageTitle);
            PagingDataSet<GroupEntity> groups = groupService.Gets(areaCode, categoryId, sortBy ?? SortBy_Group.DateCreated_Desc, pageIndex: pageIndex);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_List", groups);
            }

            return View(groups);

        }
        /// <summary>
        /// 迭代获取类别
        /// </summary>
        /// <param name="category"></param>
        /// <param name="allParentCategories"></param>
        private void RecursiveGetAllParentCategories(Category category, ref List<Category> allParentCategories)
        {
            if (category == null)
                return;
            allParentCategories.Insert(0, category);
            Category parent = category.Parent;
            if (parent != null)
                RecursiveGetAllParentCategories(parent, ref allParentCategories);
        }
        /// <summary>
        /// 迭代获取地区
        /// </summary>
        /// <param name="area"></param>
        /// <param name="rootAreaCode"></param>
        /// <param name="allParentAreas"></param>
        private void RecursiveGetAllParentArea(Area area, string rootAreaCode, ref List<Area> allParentAreas)
        {
            if (area == null || area.AreaCode == rootAreaCode)
                return;


            allParentAreas.Insert(0, area);
            Area parent = areaService.Get(area.ParentCode);
            if (parent != null)
            {
                RecursiveGetAllParentArea(parent, rootAreaCode, ref allParentAreas);
            }
        }

        
        /// <summary>
        /// 用户的群组页
        /// </summary>
        /// <returns></returns>
        [UserSpaceAuthorize]
        public ActionResult UserJoinedGroups(string spaceKey, int pageIndex = 1)
        {
            string title = "我加入的群组";
            IUserService userService = DIContainer.Resolve<IUserService>();
            User spaceUser = userService.GetFullUser(spaceKey);
            var currentUser = UserContext.CurrentUser;
            if (spaceUser == null)
                return HttpNotFound();
            
            
            if (currentUser != null)
            {
                if (currentUser.UserId != spaceUser.UserId)
                {
                    title = spaceUser.DisplayName + "加入的群组";
                }
            }
            else
            {
                title = spaceUser.DisplayName + "加入的群组";
            }
            
            
            PagingDataSet<GroupEntity> groups = groupService.GetMyJoinedGroups(spaceUser.UserId, pageIndex: pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_List", groups);

            ViewData["spaceUser"] = spaceUser;
            ViewData["currentUser"] = currentUser;
            pageResourceManager.InsertTitlePart(title);

            #region 身份认证
            List<Identification> identifications = identificationService.GetUserIdentifications(spaceUser.UserId);
            if (identifications.Count() > 0)
            {
                ViewData["identificationTypeVisiable"] = true;
            }
            #endregion

            
            
            //设置当前登录用户对当前页用户的关注情况


            


            return View(groups);
        }

        
        
        /// <summary>
        /// 用户创建的群组页
        /// </summary>
        /// <returns></returns>
        [UserSpaceAuthorize]
        public ActionResult UserCreatedGroups(string spaceKey)
        {
            string title = "我创建的群组";
            IUserService userService = DIContainer.Resolve<IUserService>();
            User spaceUser = userService.GetFullUser(spaceKey);
            if (spaceUser == null)
                return HttpNotFound();
            bool ignoreAudit = false;
            var currentUser = UserContext.CurrentUser;
            if (currentUser != null)
            {
                if (currentUser.UserId == spaceUser.UserId || new Authorizer().IsAdministrator(GroupConfig.Instance().ApplicationId))
                    ignoreAudit = true;

                if (currentUser.UserId != spaceUser.UserId)
                {
                    title = spaceUser.DisplayName + "创建的群组";
                }
            }
            else
            {
                title = spaceUser.DisplayName + "创建的群组";
            }

            pageResourceManager.InsertTitlePart(title);
            var groups = groupService.GetMyCreatedGroups(spaceUser.UserId, ignoreAudit);
            if (Request.IsAjaxRequest())
                return PartialView("_List", groups);

            ViewData["spaceUser"] = spaceUser;
            ViewData["currentUser"] = currentUser;


            return View(groups);
        }

        /// <summary>
        /// 标签显示群组列表
        /// </summary>
        public ActionResult ListByTag(string tagName, SortBy_Group sortBy = SortBy_Group.DateCreated_Desc, int pageIndex = 1)
        {
            tagName = WebUtility.UrlDecode(tagName);

            PagingDataSet<GroupEntity> groups = groupService.GetsByTag(tagName, sortBy, pageIndex: pageIndex);
            pageResourceManager.InsertTitlePart(tagName);
            ViewData["tagName"] = tagName;
            ViewData["sortBy"] = sortBy;
            return View(groups);
        }

        #endregion

        #region 内容块

        
        /// <summary>
        /// 顶部群组导航
        /// </summary>
        /// <returns></returns>
        public ActionResult _GroupGlobalNavigations()
        {
            IUser CurrentUser = UserContext.CurrentUser;
            IEnumerable<GroupEntity> groups = null;
            if (CurrentUser != null)
            {
                groups = groupService.GetMyCreatedGroups(CurrentUser.UserId, true);

                if (groups.Count() >= 9)
                    return View(groups.Take(9));

                PagingDataSet<GroupEntity> joinedGroups = groupService.GetMyJoinedGroups(CurrentUser.UserId);
                groups = groups.Union(joinedGroups).Take(9);
            }

            return View(groups);
        }

        /// <summary>
        /// 群组排行内容块
        /// </summary>
        /// <returns></returns>
        public ActionResult _TopGroups(int topNumber, string areaCode, long? categoryId, SortBy_Group? sortBy, string viewName = "_TopGroups_List")
        {
            var groups = groupService.GetTops(topNumber, areaCode, categoryId, sortBy ?? SortBy_Group.DateCreated_Desc);
            
            

            ViewData["SortBy"] = sortBy;
            return PartialView(viewName, groups);
        }


        /// <summary>
        /// 群组分类导航内容块（包含1、2级）
        /// </summary>
        /// <returns></returns>
        public ActionResult _CategoryGroups()
        {
            IEnumerable<Category> categories = categoryService.GetRootCategories(TenantTypeIds.Instance().Group());
            return PartialView(categories);
        }

        /// <summary>
        /// 群组地区导航内容块
        /// </summary>
        /// <returns></returns>
        public ActionResult _AreaGroups(int topNumber, string areaCode, long? categoryId, SortBy_Group sortBy = SortBy_Group.DateCreated_Desc)
        {
            IUser iUser = (User)UserContext.CurrentUser;
            User user = null;
            if (iUser == null)
            {
                user = new User();
            }
            else
            {
                user = userService.GetFullUser(iUser.UserId);
            }
            if (string.IsNullOrEmpty(areaCode) && Request.Cookies["AreaGroupCookie" + user.UserId] != null && !string.IsNullOrEmpty(Request.Cookies["AreaGroupCookie" + user.UserId].Value))
                areaCode = Request.Cookies["AreaGroupCookie" + user.UserId].Value;

            if (string.IsNullOrEmpty(areaCode))
            {
                string ip = WebUtility.GetIP();
                areaCode = IPSeeker.Instance().GetAreaCode(ip);
                if (string.IsNullOrEmpty(areaCode) && user.Profile != null)
                {
                    areaCode = user.Profile.NowAreaCode;
                }
            }
            ViewData["areaCode"] = areaCode;
            if (!string.IsNullOrEmpty(areaCode))
            {
                Area area = areaService.Get(areaCode);
                if (!string.IsNullOrEmpty(area.ParentCode))
                {
                    Area parentArea = areaService.Get(area.ParentCode);
                    ViewData["parentCode"] = parentArea.AreaCode;
                }
            }

            IEnumerable<GroupEntity> groups = groupService.GetTops(topNumber, areaCode, categoryId, sortBy);

            HttpCookie cookie = new HttpCookie("AreaGroupCookie" + user.UserId, areaCode);
            Response.Cookies.Add(cookie);

            return PartialView(groups);
        }

        /// <summary>
        /// 人气群主
        /// </summary>
        /// <returns></returns>
        public ActionResult _RecommendedGroupOwners(int topNumber = 5, string recommendTypeId = null)
        {
            IEnumerable<RecommendItem> recommendUsers = recommendService.GetTops(topNumber, recommendTypeId);

            
            return PartialView(recommendUsers);
        }
        /// <summary>
        /// 标签地图
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupTagMap()
        {
            pageResourceManager.InsertTitlePart("标签云图");
            return View();
        }
        /// <summary>
        /// 关注按钮
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="followedUser">要关注的用户</param>
        /// <returns></returns>
        public ActionResult _FollowedButton(User currentUser, User followedUser)
        {
            ViewData["currentUser"] = currentUser;
            ViewData["followedUser"] = followedUser;
            if (currentUser != null && currentUser.UserId != followedUser.UserId)
            {
                bool currentUserIsFollowedUser = false;
                ViewData["currentUserIsFollowedUser"] = currentUserIsFollowedUser;
            }
            bool isFollowed = currentUser.IsFollowed(followedUser.UserId);
            ViewData["isFollowed"] = isFollowed;

            return View();
        }

        #endregion

    }
}
