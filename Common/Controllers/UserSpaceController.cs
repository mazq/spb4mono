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
using System.Web;
using System.Web.Mvc;
using Spacebuilder.Common.Configuration;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Mvc;
using Tunynet.UI;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户空间Controller
    /// </summary>
    [TitleFilter(IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.UserSpace, IsApplication = false)]
    [AnonymousBrowseCheck]
    public class UserSpaceController : Controller
    {
        #region service

        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private UserProfileService userProfileService = new UserProfileService();
        private FollowService followService = new FollowService();
        private VisitService visitService = new VisitService(TenantTypeIds.Instance().User());
        private ActivityService activityService = new ActivityService();
        private CategoryService categoryService = new CategoryService();
        private InvitationService invitationService = DIContainer.Resolve<InvitationService>();
        private IdentificationService identificationService = new IdentificationService();
        private ILogoSettingsManager logoSettingsManager = DIContainer.Resolve<ILogoSettingsManager>();
        private PrivacyService privacyService = new PrivacyService();
        private ApplicationService applicationService = new ApplicationService();

        #endregion

        #region 我的首页

        /// <summary>
        /// 我的首页
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult MyHome(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            ViewData["user"] = user;
            if (UserContext.CurrentUser == null || UserContext.CurrentUser.UserId != user.UserId)
            {
                return RedirectToAction("SpaceHome");
            }

            #region 显示用户资料向导

            int integrity = 0;
            if (user.Profile != null)
            {
                integrity = user.Profile.Integrity;
            }

            UserProfileSettings userProfileSetting = new UserProfileSettings();

            if (integrity < userProfileSetting.MinIntegrity && !user.Profile.IsNeedGuide)
            {
                //读取Cookie，判断是否显示用户资料向导
                HttpCookie GuideDisplayed = Request.Cookies.Get("GuideDisplayed");
                if (GuideDisplayed == null)
                {
                    GuideDisplayed = new HttpCookie("GuideDisplayed", "true");
                    GuideDisplayed.Expires = DateTime.Now.Date.AddDays(1);
                    Response.Cookies.Add(GuideDisplayed);

                    return RedirectToAction("UserProfileGuideAvatar", "UserSpaceSettings");
                }
            }

            ViewData["integrity"] = integrity;

            #endregion

            #region Title

            pageResourceManager.InsertTitlePart("我的首页");
            ViewBag.Title = user.DisplayName + "的首页";

            #endregion

            #region 分组栏
            List<Category> categoriesShow = null;//前三个分组
            List<Category> categoriesMore = null;//更多分组
            IEnumerable<Category> categories = categoryService.GetOwnerCategories(user.UserId, TenantTypeIds.Instance().User());
            if (categories != null)
            {
                categoriesShow = categories.Take(3).ToList();
                categoriesMore = categories.Skip(3).ToList();
            }
            ViewData["categoriesShow"] = categoriesShow;
            if (categoriesMore != null && categoriesMore.Count > 0)
                ViewData["menuItem"] = categoriesMore.Select(n => new MenuItem { Text = n.CategoryName, Value = n.CategoryId.ToString(), Url = SiteUrls.Instance()._ActivitiesList(Url.SpaceKey(), n.CategoryId) });

            #endregion

            #region 导航&应用

            NavigationService service = new NavigationService();
            ViewData["applications"] = applicationService.GetAll();

            IEnumerable<ApplicationModel> apps = applicationService.GetAll(true);
            IEnumerable<PresentAreaNavigation> navigations = service.GetRootPresentAreaNavigations(PresentAreaKeysOfBuiltIn.UserSpace, user.UserId);

            if (navigations != null && apps != null)
            {
                ViewData["navigations"] = navigations
                                          .ToDictionary(v => v, k =>
                                          {
                                              var app = apps.FirstOrDefault(n => k.ApplicationId == n.ApplicationId);
                                              if (app != null)
                                                  return app.ApplicationKey;
                                              return string.Empty;
                                          });
            }

            #endregion

            #region 气泡

            //获取内容数的链接
            string tenantTypeId = TenantTypeIds.Instance().User();
            Dictionary<int, List<OwnerStatisticData>> OwnerStatisticDataDictionary = new Dictionary<int, List<OwnerStatisticData>>();
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            IEnumerable<string> dataKeys = OwnerDataSettings.GetDataKeys(tenantTypeId);
            IEnumerable<ApplicationBase> applicationBase = applicationService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfBuiltIn.UserSpace, user.UserId);
            foreach (var application in applicationBase)
            {
                var applicationDataKeys = dataKeys.Where(n => n.StartsWith(application.ApplicationKey));
                var ownerStatisticDataList = new List<OwnerStatisticData>();
                foreach (var dataKey in applicationDataKeys)
                {
                    OwnerStatisticData ownerStatisticData = new OwnerStatisticData();
                    IOwnerDataGetter dataGetter = OwnerDataGetterFactory.Get(dataKey);
                    if (dataGetter != null)
                    {
                        ownerStatisticData.DataName = dataGetter.DataName;
                        ownerStatisticData.DataUrl = dataGetter.GetDataUrl(spaceKey, user.UserId);
                        ownerStatisticData.ContentCount = new OwnerDataService(TenantTypeIds.Instance().User()).GetLong(user.UserId, dataKey);
                        ownerStatisticDataList.Add(ownerStatisticData);
                    }
                }
                dictionary[application.ApplicationId] = application.Config.ApplicationName;
                OwnerStatisticDataDictionary[application.ApplicationId] = ownerStatisticDataList;
            }
            ViewData["OwnerStatisticDataDictionary"] = OwnerStatisticDataDictionary;
            ViewData["dictionary"] = dictionary;
            ViewData["applicationBase"] = applicationBase;


            #endregion


            return View();
        }


        #region MyHome控件

        /// <summary>
        /// 我的首页用户信息
        /// </summary>
        [HttpGet]
        public ActionResult _MyHomeUserInfo(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            int integrity = 0;
            if (user.Profile != null)
            {
                integrity = user.Profile.Integrity;
            }
            ViewData["integrity"] = integrity;
            ViewData["user"] = user;

            //获取内容数的链接
            string tenantTypeId = TenantTypeIds.Instance().User();
            Dictionary<int, List<OwnerStatisticData>> OwnerStatisticDataDictionary = new Dictionary<int, List<OwnerStatisticData>>();
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            IEnumerable<string> dataKeys = OwnerDataSettings.GetDataKeys(tenantTypeId);
            IEnumerable<ApplicationBase> applicationBase = applicationService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfBuiltIn.UserSpace, user.UserId).Where(n => n.ApplicationKey != "PointMall");
            foreach (var application in applicationBase)
            {
                var applicationDataKeys = dataKeys.Where(n => n.StartsWith(application.ApplicationKey));
                var ownerStatisticDataList = new List<OwnerStatisticData>();
                foreach (var dataKey in applicationDataKeys)
                {
                    OwnerStatisticData ownerStatisticData = new OwnerStatisticData();
                    IOwnerDataGetter dataGetter = OwnerDataGetterFactory.Get(dataKey);
                    if (dataGetter != null)
                    {
                        ownerStatisticData.DataName = dataGetter.DataName;
                        ownerStatisticData.DataUrl = dataGetter.GetDataUrl(spaceKey, user.UserId);
                        ownerStatisticData.ContentCount = new OwnerDataService(TenantTypeIds.Instance().User()).GetLong(user.UserId, dataKey);
                        ownerStatisticDataList.Add(ownerStatisticData);
                    }
                }
                dictionary[application.ApplicationId] = application.Config.ApplicationName;
                OwnerStatisticDataDictionary[application.ApplicationId] = ownerStatisticDataList;
            }
            ViewData["OwnerStatisticDataDictionary"] = OwnerStatisticDataDictionary;
            ViewData["dictionary"] = dictionary;
            ViewData["applicationBase"] = applicationBase;
            return View();
        }

        /// <summary>
        /// 我的主页导航
        /// </summary>
        /// <param name="spaceKey">用户标识</param>
        /// <param name="showOperate">显示操作（如果显示操作就不会显示基础导航）</param>
        [HttpGet]
        [BuildNavigationRouteData(PresentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace)]
        public ActionResult _MyHomeNavigations(string spaceKey, bool showOperation = false)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            IEnumerable<ApplicationModel> apps = applicationService.GetAll(true);

            ViewData["MicroblogApplication"] = new ApplicationService().Get("Microblog");
            IEnumerable<Navigation> navigations = new NavigationService().GetRootNavigations(PresentAreaKeysOfBuiltIn.UserSpace, user.UserId);
            navigations = navigations.Where(n => n.ApplicationId == 0 || (apps != null && apps.Select(s => s.ApplicationId).Contains(n.ApplicationId)));
            ViewData["navigations"] = navigations;

            if (showOperation)
            {
                IEnumerable<ApplicationManagementOperation> operations = new ManagementOperationService().GetShortcuts(PresentAreaKeysOfBuiltIn.UserSpace, true);

                operations = operations.Where(n => n.ApplicationId == 0 || (apps != null && apps.Select(s => s.ApplicationId).Contains(n.ApplicationId)));
                ViewData["ApplicationManagementOperations"] = operations;
                ViewData["ShowOperation"] = showOperation;
            }

            ViewData["user"] = user;
            return View();
        }

        /// <summary>
        /// 我的主页动态
        /// </summary>
        public ActionResult _MyHomeActivities(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            ViewData["user"] = user;

            ApplicationService applicationService = new ApplicationService();

            List<ApplicationBase> application_List = new List<ApplicationBase>();
            IEnumerable<ApplicationBase> applications = applicationService.GetAll();
            foreach (var application in applications)
            {
                if (application.IsEnabled)
                {
                    application_List.Add(application);
                }
            }
            ViewData["applications"] = application_List;

            #region 分组栏
            List<Category> categoriesShow = null;//前三个分组
            List<Category> categoriesMore = null;//更多分组
            IEnumerable<Category> categories = categoryService.GetOwnerCategories(user.UserId, TenantTypeIds.Instance().User());
            if (categories != null)
            {
                categoriesShow = categories.Take(3).ToList();
                categoriesMore = categories.Skip(3).ToList();
            }
            ViewData["categoriesShow"] = categoriesShow;
            if (categoriesMore != null && categoriesMore.Count > 0)
                ViewData["menuItem"] = categoriesMore.Select(n => new MenuItem { Text = n.CategoryName, Value = n.CategoryId.ToString(), Url = SiteUrls.Instance()._ActivitiesList(Url.SpaceKey(), n.CategoryId) });

            #endregion

            ViewData["microblogType"] = MicroblogType(string.Empty);
            return View();
        }
        #endregion

        /// <summary>
        /// 用户指定数量数据相关控件
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="topNumber">获取条数</param>
        /// <param name="sortBy">排序依据</param>
        public ActionResult _ListTopUsers(string spaceKey, int topNumber, SortBy_User sortBy = SortBy_User.HitTimes)
        {
            IEnumerable<IUser> users = userService.GetTopUsers(topNumber, sortBy);
            return View(users);
        }

        /// <summary>
        /// 删除该条访客记录
        /// </summary>
        [HttpPost]
        public ActionResult DeleteHomeVisitors(string spaceKey)
        {
            
            long id = Request.Form.Get<long>("id", 0);
            visitService.Delete(id);
            return RedirectToAction("_HomeLastVisitList");
        }

        /// <summary>
        /// 最近访客控件
        /// </summary>
        public ActionResult _HomeLastVisitList(string spaceKey, int pageIndex = 1)
        {
            ViewData["spacekey"] = spaceKey;
            ViewData["pageIndex"] = pageIndex;
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            IEnumerable<Visit> visits = visitService.GetTopVisits(userId, 24);

            //实现分页显示
            IEnumerable<Visit>[] visitsArry = new IEnumerable<Visit>[3];
            int pageCount = 1;

            if (visits.Count() > 16)
            {
                pageCount = 3;
                visitsArry[0] = visits.Take(8);
                visitsArry[1] = visits.Skip(8);
                visitsArry[2] = visitsArry[1].Skip(8);
                visitsArry[1] = visitsArry[1].Take(8);
            }
            else if (visits.Count() > 8)
            {
                pageCount = 2;
                visitsArry[0] = visits.Take(8);
                visitsArry[1] = visits.Skip(8);
            }
            else
            {
                visitsArry[0] = visits;
            }
            ViewData["pageCount"] = pageCount;

            return View(visitsArry[pageIndex - 1]);
        }

        /// <summary>
        /// 屏蔽此人信息
        /// </summary>
        [HttpPost]
        public JsonResult HideUserAllInformation(string spaceKey, long ownerId)
        {
            long currentUserId = UserContext.CurrentUser.UserId;
            activityService.RemoveInboxAboutOwner(currentUserId, ownerId, ActivityOwnerTypes.Instance().User());
            return Json(new StatusMessageData(StatusMessageType.Success, "屏蔽此人信息！"));
        }

        /// <summary>
        /// 隐藏这条信息
        /// </summary>
        [HttpPost]
        public JsonResult HideCurrentInformation(string spaceKey, long microblogId)
        {
            long currentUserId = UserContext.CurrentUser.UserId;
            activityService.DeleteFromUserInbox(currentUserId, 1);
            return Json(new StatusMessageData(StatusMessageType.Success, "隐藏这条信息！"));
        }

        /// <summary>
        ///  查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        [HttpPost]
        public JsonResult GetNewerCount(string spaceKey, long lastActivityId, int? applicationId)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            string name;
            int count = activityService.GetNewerCount(userId, lastActivityId, applicationId, out name);
            if (count == 0)
            {
                return Json(new JsonResult { Data = new { lastActivityId = lastActivityId, hasNew = false } });
            }
            else
            {
                string showName;
                if (count == 1)
                    showName = name + "更新了动态，点击查看";
                else
                    showName = name + "等多位好友更新了动态，点击查看";
                return Json(new JsonResult { Data = new { hasNew = true, showName = showName } });
            }
        }

        /// <summary>
        /// 获取以后进入用户时间线的动态
        /// </summary>
        [HttpPost]
        public ActionResult _GetNewerActivities(string spaceKey, long lastActivityId, int? applicationId)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IEnumerable<Activity> newActivities = activityService.GetNewerActivities(userId, lastActivityId, applicationId);
            if (newActivities != null && newActivities.Count() > 0)
            {
                ViewData["lastActivityId"] = newActivities.FirstOrDefault().ActivityId;
            }
            return View(newActivities);
        }

        /// <summary>
        /// 获取更多
        /// </summary>
        public ActionResult _GetNewerActivities(string spaceKey, int? pageIndex, int? applicationId, long? groupId, bool? isOriginalThread, MediaType? mediaType)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<Activity> activities = activityService.GetMyTimeline(userId, groupId, applicationId, mediaType, isOriginalThread, pageIndex ?? 1);
            if (activities.FirstOrDefault() != null)
            {
                ViewData["lastActivityId"] = activities.FirstOrDefault().ActivityId;
            }
            ViewData["applicationId"] = applicationId;
            ViewData["groupId"] = groupId;

            return View(activities);
        }

        /// <summary>
        /// 首页动态列表
        /// </summary>
        public ActionResult _ActivitiesList(string spaceKey, int? pageIndex, int? applicationId, long? groupId, bool? isOriginalThread, MediaType? mediaType)
        {
            string microblogType = Request.Form.Get<string>("microblogType", null);
            if (!string.IsNullOrEmpty(microblogType))
            {
                if (microblogType == "0")
                {
                    isOriginalThread = true;
                }
                else
                {
                    mediaType = (MediaType)int.Parse(microblogType);
                }
            }
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<Activity> activities = activityService.GetMyTimeline(userId, groupId, applicationId, mediaType, isOriginalThread, pageIndex ?? 1);
            if (activities.FirstOrDefault() != null)
            {
                ViewData["lastActivityId"] = activities.FirstOrDefault().ActivityId;
            }
            ViewData["applicationId"] = applicationId;
            ViewData["groupId"] = groupId;
            ViewData["isOriginalThread"] = isOriginalThread;
            ViewData["mediaType"] = mediaType;
            return View(activities);
        }

        /// <summary>
        /// 微博类型
        /// </summary>
        /// <param name="selectedValue">选中值</param>
        /// <returns></returns>
        public SelectList MicroblogType(string selectedValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "原创", Value = "0" });
            list.Add(new SelectListItem { Text = "图片", Value = ((int)MediaType.Image).ToString() });
            list.Add(new SelectListItem { Text = "视频", Value = ((int)MediaType.Video).ToString() });
            list.Add(new SelectListItem { Text = "音乐", Value = ((int)MediaType.Audio).ToString() });
            return new SelectList(list, "Value", "Text", selectedValue);
        }

        /// <summary>
        /// 首页动态列表
        /// </summary>
        [HttpGet]
        public ActionResult _NewActivitie(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<Activity> activities = activityService.GetMyTimeline(userId, null, 1001, null, null, 1);
            if (activities != null)
            {
                return View(activities.FirstOrDefault());
            }

            return View();
        }

        /// <summary>
        /// 删除动态
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteUserActivity(string spaceKey, long activityId)
        {
            var currentUser = UserContext.CurrentUser;
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (currentUser == null || userId != currentUser.UserId)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有删除动态的权限"));
            activityService.DeleteActivity(activityId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除动态成功！"));
        }

        #endregion

        #region 我/他的主页

        /// <summary>
        /// 我的主页
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize]
        public ActionResult SpaceHome(string spaceKey, int? pageIndex, int? applicationId = null)
        {
            IUser currentUser = UserContext.CurrentUser;
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            PagingDataSet<Activity> activities = activityService.GetOwnerActivities(ActivityOwnerTypes.Instance().User(), user.UserId, applicationId, null, null, false, pageIndex ?? 1);
            ViewData["applicationId"] = applicationId;
            ViewData["pageIndex"] = pageIndex;

            if (Request.IsAjaxRequest())
                return View("_Activities", activities);

            ViewData["user"] = user;

            CountService countService = new CountService(TenantTypeIds.Instance().User());
            countService.ChangeCount(CountTypes.Instance().HitTimes(), user.UserId, user.UserId, 1, true);
            AuthorizationService authorizationService = new AuthorizationService();

            #region View中需要的信息

            //是否是匿名用户
            bool isAnonymousUser = false;
            if (currentUser == null)
                isAnonymousUser = true;
            else if (user.UserId != currentUser.UserId)
            {
                visitService.CreateVisit(currentUser.UserId, currentUser.DisplayName, user.UserId, user.DisplayName);
            }
            ViewData["isAnonymousUser"] = isAnonymousUser;


            if (!isAnonymousUser)
            {
                ViewData["isSuperAdmin_CurrentUser"] = authorizationService.IsSuperAdministrator(currentUser);//当前用户是否为超级管理员
                ViewData["isSuperAdmin_User"] = authorizationService.IsSuperAdministrator(user);//被浏览用户是否为超级管理员
                ViewData["isRequestFollow"] = !followService.IsFollowed(user.UserId, currentUser.UserId); //是否需要求关注
            }

            //是否为同一用户
            bool isSameUser = false;
            if (!isAnonymousUser && user.UserId == currentUser.UserId)
            {
                isSameUser = true;
            }
            ViewData["isSameUser"] = isSameUser;

            //是否关注和悄悄关注
            if (currentUser != null)
            {
                FollowEntity entity = followService.Get(currentUser.UserId, user.UserId);
                if (entity != null)
                {
                    ViewData["noteName"] = entity.NoteName;
                }
                bool isQuietly;
                bool isFollowed = followService.IsFollowed(currentUser.UserId, user.UserId, out isQuietly);
                ViewData["isFollowed"] = isFollowed;
                ViewData["isQuietly"] = isQuietly;
                if (isFollowed)
                {
                    IEnumerable<string> groupNames = new List<string>();
                    followService.IsFollowed(currentUser.UserId, user.UserId, out groupNames);

                    if (groupNames.Count() > 0)
                        ViewData["editGroupShow"] = groupNames.FirstOrDefault();
                    else
                        ViewData["editGroupShow"] = "编辑分组";
                }
            }

            //简介显示
            string introduction;
            if (user.Profile == null || !user.Profile.HasIntroduction)
            {
                introduction = isSameUser ? "可以在此填写个性简介" : "该用户尚未填写简介";
            }
            else
            {
                introduction = user.Profile.Introduction;
            }

            ViewData["introduction"] = introduction;

            //共同关注的内容
            if (!isSameUser && currentUser != null)
            {
                FollowUserSearcher followUserSearcher = (FollowUserSearcher)SearcherFactory.GetSearcher(FollowUserSearcher.CODE);
                UserSearcher userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);


                IEnumerable<User> sameFollowedUsers = followUserSearcher.SearchInterestedWithFollows(currentUser.UserId, user.UserId);

                IEnumerable<string> sameTagNames = new List<string>();
                IEnumerable<string> sameCompanyNames = new List<string>();
                IEnumerable<string> sameSchoolNames = new List<string>();
                userSearcher.SearchInterested(currentUser.UserId, user.UserId, out sameTagNames, out sameCompanyNames, out sameSchoolNames);


                ViewData["sameFollowedUsers"] = sameFollowedUsers;
                ViewData["sameTagNames"] = sameTagNames;
                ViewData["sameCompanyNames"] = sameCompanyNames;
                ViewData["sameSchoolNames"] = sameSchoolNames;
            }

            #endregion

            #region Title

            pageResourceManager.InsertTitlePart(isSameUser ? "我" : user.DisplayName + "的主页");

            #endregion

            #region 身份认证
            List<Identification> identifications = identificationService.GetUserIdentifications(user.UserId);
            if (identifications.Count() > 0)
            {
                ViewData["identificationTypeVisiable"] = true;
            }
            #endregion

            ViewData["applications"] = applicationService.GetAll();
            return View(activities);
        }

        #region SpaceHome控件

        /// <summary>
        /// 用户内容数的气泡
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public ActionResult _ContentPop(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            IUser currentUser = UserContext.CurrentUser;

            //获取内容数的链接
            string tenantTypeId = TenantTypeIds.Instance().User();
            Dictionary<int, List<OwnerStatisticData>> OwnerStatisticDataDictionary = new Dictionary<int, List<OwnerStatisticData>>();
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            IEnumerable<string> dataKeys = OwnerDataSettings.GetDataKeys(tenantTypeId);
            IEnumerable<ApplicationBase> applicationBase = applicationService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfBuiltIn.UserSpace, user.UserId).Where(n => n.ApplicationKey != "PointMall");
            foreach (var application in applicationBase)
            {
                var applicationDataKeys = dataKeys.Where(n => n.StartsWith(application.ApplicationKey));
                var ownerStatisticDataList = new List<OwnerStatisticData>();
                foreach (var dataKey in applicationDataKeys)
                {
                    OwnerStatisticData ownerStatisticData = new OwnerStatisticData();
                    IOwnerDataGetter dataGetter = OwnerDataGetterFactory.Get(dataKey);
                    if (dataGetter != null)
                    {
                        ownerStatisticData.DataName = dataGetter.DataName;
                        ownerStatisticData.DataUrl = dataGetter.GetDataUrl(spaceKey, user.UserId);
                        ownerStatisticData.ContentCount = new OwnerDataService(TenantTypeIds.Instance().User()).GetLong(user.UserId, dataKey);
                        ownerStatisticDataList.Add(ownerStatisticData);
                    }
                }
                dictionary[application.ApplicationId] = application.Config.ApplicationName;
                OwnerStatisticDataDictionary[application.ApplicationId] = ownerStatisticDataList;
            }
            ViewData["OwnerStatisticDataDictionary"] = OwnerStatisticDataDictionary;
            ViewData["dictionary"] = dictionary;
            ViewData["applicationBase"] = applicationBase;

            return View(user);
        }

        /// <summary>
        /// 用户信息菜单控件
        /// </summary>
        public ActionResult _UserInfo(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            IUser currentUser = UserContext.CurrentUser;
            AuthorizationService authorizationService = new AuthorizationService();
            bool isQuietly;
            bool isSameUser = false;

            if (currentUser != null)
            {
                bool isFollowed = followService.IsFollowed(currentUser.UserId, user.UserId, out isQuietly);

                if (currentUser != null && user.UserId == currentUser.UserId)
                {
                    isSameUser = true;
                }

                ViewData["isSuperAdmin_CurrentUser"] = authorizationService.IsSuperAdministrator(currentUser);//当前用户是否为超级管理员
                ViewData["isSuperAdmin_User"] = authorizationService.IsSuperAdministrator(user);//被浏览用户是否为超级管理员
                ViewData["isRequestFollow"] = !followService.IsFollowed(user.UserId, currentUser.UserId); //是否需要求关注

                ViewData["isSameUser"] = isSameUser;
                ViewData["isFollowed"] = isFollowed;
                ViewData["isQuietly"] = isQuietly;

            }

            //获取内容数的链接
            string tenantTypeId = TenantTypeIds.Instance().User();
            Dictionary<int, List<OwnerStatisticData>> OwnerStatisticDataDictionary = new Dictionary<int, List<OwnerStatisticData>>();
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            IEnumerable<string> dataKeys = OwnerDataSettings.GetDataKeys(tenantTypeId);
            IEnumerable<ApplicationBase> applicationBase = applicationService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfBuiltIn.UserSpace, user.UserId).Where(n => n.ApplicationKey != "PointMall");
            foreach (var application in applicationBase)
            {
                var applicationDataKeys = dataKeys.Where(n => n.StartsWith(application.ApplicationKey));
                var ownerStatisticDataList = new List<OwnerStatisticData>();
                foreach (var dataKey in applicationDataKeys)
                {
                    OwnerStatisticData ownerStatisticData = new OwnerStatisticData();
                    IOwnerDataGetter dataGetter = OwnerDataGetterFactory.Get(dataKey);
                    if (dataGetter != null)
                    {
                        ownerStatisticData.DataName = dataGetter.DataName;
                        ownerStatisticData.DataUrl = dataGetter.GetDataUrl(spaceKey, user.UserId);
                        ownerStatisticData.ContentCount = new OwnerDataService(TenantTypeIds.Instance().User()).GetLong(user.UserId, dataKey);
                        ownerStatisticDataList.Add(ownerStatisticData);
                    }
                }
                dictionary[application.ApplicationId] = application.Config.ApplicationName;
                OwnerStatisticDataDictionary[application.ApplicationId] = ownerStatisticDataList;
            }
            ViewData["OwnerStatisticDataDictionary"] = OwnerStatisticDataDictionary;
            ViewData["dictionary"] = dictionary;
            ViewData["applicationBase"] = applicationBase;


            return View(user);
        }

        /// <summary>
        /// SpaceHome下局部页
        /// </summary>
        public ActionResult UserInfo(string spaceKey)
        {
            IUser currentUser = UserContext.CurrentUser;
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            ViewData["user"] = user;
            AuthorizationService authorizationService = new AuthorizationService();

            #region View中需要的信息

            //是否是匿名用户
            bool isAnonymousUser = false;
            if (currentUser == null)
                isAnonymousUser = true;
            else if (user.UserId != currentUser.UserId)
            {
                visitService.CreateVisit(currentUser.UserId, currentUser.DisplayName, user.UserId, user.DisplayName);
            }
            ViewData["isAnonymousUser"] = isAnonymousUser;


            if (!isAnonymousUser)
            {
                ViewData["isSuperAdmin_CurrentUser"] = authorizationService.IsSuperAdministrator(currentUser);//当前用户是否为超级管理员
                ViewData["isSuperAdmin_User"] = authorizationService.IsSuperAdministrator(user);//被浏览用户是否为超级管理员
                ViewData["isRequestFollow"] = !followService.IsFollowed(user.UserId, currentUser.UserId); //是否需要求关注
            }

            //是否为同一用户
            bool isSameUser = false;
            if (!isAnonymousUser && user.UserId == currentUser.UserId)
            {
                isSameUser = true;
            }
            ViewData["isSameUser"] = isSameUser;

            //是否关注和悄悄关注
            if (currentUser != null)
            {
                FollowEntity entity = followService.Get(currentUser.UserId, user.UserId);
                if (entity != null)
                {
                    ViewData["noteName"] = entity.NoteName;
                }
                bool isQuietly;
                bool isFollowed = followService.IsFollowed(currentUser.UserId, user.UserId, out isQuietly);
                ViewData["isFollowed"] = isFollowed;
                ViewData["isQuietly"] = isQuietly;
                if (isFollowed)
                {
                    IEnumerable<string> groupNames = new List<string>();
                    followService.IsFollowed(currentUser.UserId, user.UserId, out groupNames);

                    if (groupNames.Count() > 0)
                        ViewData["editGroupShow"] = groupNames.FirstOrDefault();
                    else
                        ViewData["editGroupShow"] = "编辑分组";
                }
            }

            //简介显示
            string introduction;
            if (user.Profile == null || !user.Profile.HasIntroduction)
            {
                introduction = isSameUser ? "可以在此填写个性简介" : "该用户尚未填写简介";
            }
            else
            {
                introduction = user.Profile.Introduction;
            }

            ViewData["introduction"] = introduction;

            //共同关注的内容
            if (!isSameUser && currentUser != null)
            {
                FollowUserSearcher followUserSearcher = (FollowUserSearcher)SearcherFactory.GetSearcher(FollowUserSearcher.CODE);
                UserSearcher userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);


                IEnumerable<User> sameFollowedUsers = followUserSearcher.SearchInterestedWithFollows(currentUser.UserId, user.UserId);

                IEnumerable<string> sameTagNames = new List<string>();
                IEnumerable<string> sameCompanyNames = new List<string>();
                IEnumerable<string> sameSchoolNames = new List<string>();
                userSearcher.SearchInterested(currentUser.UserId, user.UserId, out sameTagNames, out sameCompanyNames, out sameSchoolNames);


                ViewData["sameFollowedUsers"] = sameFollowedUsers;
                ViewData["sameTagNames"] = sameTagNames;
                ViewData["sameCompanyNames"] = sameCompanyNames;
                ViewData["sameSchoolNames"] = sameSchoolNames;
            }

            #endregion

            #region 身份认证

            List<Identification> identifications = identificationService.GetUserIdentifications(user.UserId);
            if (identifications.Count() > 0)
            {
                ViewData["identificationTypeVisiable"] = true;
            }

            #endregion

            return View();
        }

        /// <summary>
        /// 我的主页侧边栏
        /// </summary>
        public ActionResult _SpaceHomeAside(string spaceKey)
        {
            IUser currentUser = UserContext.CurrentUser;
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            //是否是匿名用户
            bool isAnonymousUser = currentUser == null ? true : false;
            bool isSameUser = false;

            if (!isAnonymousUser && user.UserId == currentUser.UserId)
            {
                isSameUser = true;
            }

            ViewData["isSameUser"] = isSameUser;
            ViewData["isAnonymousUser"] = isAnonymousUser;
            ViewData["user"] = user;

            return View();
        }

        /// <summary>
        /// 用户状态菜单控件
        /// </summary>
        public ActionResult _UserStatus(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            ViewData["accessedCount"] = user.PreWeekHitTimes + "/" + user.HitTimes;

            return View(user);
        }

        
        /// <summary>
        /// 标签菜单控件
        /// </summary>
        public ActionResult _TopUserTags(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);

            if (user.Profile != null)
                ViewData["user"] = user;

            TagService tagService = new TagService(TenantTypeIds.Instance().User());
            IEnumerable<ItemInTag> tags = tagService.GetItemInTagsOfItem(user.UserId);

            return View(tags);
        }

        /// <summary>
        /// 关注的用户菜单控件
        /// </summary>
        public ActionResult _FollowedUserList(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            IUser currentUser = UserContext.CurrentUser;
            bool isAnonymousUser = false;
            if (currentUser == null)
                isAnonymousUser = true;
            ViewData["isAnonymousUser"] = isAnonymousUser;

            if (!isAnonymousUser && currentUser.UserId == user.UserId)
            {
                ViewData["isSameUser"] = true;
            }
            if (user.Profile != null)
                ViewData["gender"] = user.Profile.Gender;

            ViewData["followedUsersCount"] = user.FollowedCount;
            IEnumerable<long> ids = followService.GetTopFollowedUserIds(user.UserId, 6, null, Follow_SortBy.FollowerCount_Desc);
            IEnumerable<User> users = new List<User>();
            if (ids != null)
                users = userService.GetFullUsers(ids);
            return View(users);
        }

        /// <summary>
        /// 粉丝菜单控件
        /// </summary>
        public ActionResult _FollowerList(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            IUser currentUser = UserContext.CurrentUser;
            bool isAnonymousUser = false;
            if (currentUser == null)
                isAnonymousUser = true;
            ViewData["isAnonymousUser"] = isAnonymousUser;

            if (!isAnonymousUser && currentUser.UserId == user.UserId)
            {
                ViewData["isSameUser"] = true;
            }
            if (user.Profile != null)
                ViewData["gender"] = user.Profile.Gender;
            ViewData["followerCount"] = user.FollowerCount;

            IEnumerable<long> ids = followService.GetTopFollowerUserIds(user.UserId, Follow_SortBy.FollowerCount_Desc, 6);
            IEnumerable<User> users = new List<User>();
            if (ids != null)
                users = userService.GetFullUsers(ids);
            return View(users);
        }

        /// <summary>
        /// 最近访客控件
        /// </summary>
        public ActionResult _SpaceLastVisitList(string spaceKey)
        {

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IEnumerable<Visit> visits = visitService.GetTopVisits(userId, 3);
            bool isSameUser = false;
            bool isAnonymousUser = false;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserId == userId)
            {
                isSameUser = true;
            }
            if (currentUser == null)
                isAnonymousUser = true;
            else
            {
                ViewData["currentUserId"] = currentUser.UserId;
            }

            ViewData["isSameUser"] = isSameUser;
            ViewData["isAnonymousUser"] = isAnonymousUser;

            return View(visits);
        }

        /// <summary>
        /// 他的粉丝也关注了的控件
        /// </summary>
        public ActionResult _FollowedUsersOfFollowersList(string spaceKey)
        {
            bool isAnonymousUser = false;
            IUser currentUser = UserContext.CurrentUser;
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            ViewData["userName"] = user.UserName;
            if (user.Profile != null)
                ViewData["gender"] = user.Profile.Gender;
            long visitorId = 0;
            if (currentUser != null)
            {
                visitorId = currentUser.UserId;
            }
            else
            {
                isAnonymousUser = true;
            }
            ViewData["isAnonymousUser"] = isAnonymousUser;
            IEnumerable<long> ids = followService.GetTopFollowedUserIdsOfFollowers(user.UserId, visitorId, 6);
            IEnumerable<User> users = new List<User>();
            if (ids != null)
                users = userService.GetFullUsers(ids);

            return View(users);
        }

        /// <summary>
        /// 这些人也关注了他的控件
        /// </summary>
        public ActionResult _FollowedUsersFromUserList(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            if (user.Profile != null)
                ViewData["gender"] = user.Profile.Gender;
            IUser currentUser = UserContext.CurrentUser;
            IEnumerable<long> ids = null;
            if (currentUser != null)
                ids = followService.GetTopFollowedUserIdsFromUser(currentUser.UserId, user.UserId, 6);
            IEnumerable<User> users = new List<User>();
            if (ids != null)
                users = userService.GetFullUsers(ids);
            return View(users);
        }

        /// <summary>
        /// 我和他都关注了的控件
        /// </summary>
        public ActionResult _TogetherFollowedUsersList(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            if (user.Profile != null)
                ViewData["gender"] = user.Profile.Gender;
            IUser currentUser = UserContext.CurrentUser;
            IEnumerable<long> ids = null;
            if (currentUser != null)
                ids = followService.GetTogetherFollowedUserIds(currentUser.UserId, user.UserId, 6);
            IEnumerable<User> users = new List<User>();
            if (ids != null)
                users = userService.GetFullUsers(ids);
            return View(users);
        }


        /// <summary>
        /// 选择菜单控件
        /// </summary>
        public ActionResult _SelectMenu(string spaceKey)
        {

            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return new EmptyResult();

            ViewData["user"] = user;
            IEnumerable<Navigation> navigations = new NavigationService().GetRootNavigations(PresentAreaKeysOfBuiltIn.UserSpace, user.UserId);
            return View(navigations);
        }

        /// <summary>
        /// 编辑简介控件
        /// </summary>
        public ActionResult _EditUserIntroduction(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user.Profile != null)
                ViewData["introduction"] = user.Profile.Introduction;

            return View();
        }

        /// <summary>
        /// 编辑简介
        /// </summary>
        [HttpPost]
        public JsonResult EditUserIntroduction(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            UserProfile profile = userProfileService.Get(userId);
            string introduction = Request.Form.GetString("introduction", string.Empty);

            bool isBanned = false;
            TextFilter(introduction, out isBanned);
            if (isBanned)
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "内容中包含非法词语！"));
            }

            if (profile == null)
            {
                profile = new UserProfile();
                profile.UserId = userId;
            }

            profile.Introduction = Formatter.FormatMultiLinePlainTextForStorage(introduction, true);
            userProfileService.Update(profile);

            string integrity = "0%";
            profile = userProfileService.Get(userId);
            if (profile != null)
                integrity = profile.Integrity + "%";

            return Json(new JsonResult() { Data = new { integrity = integrity, introduction = profile.Introduction } });
        }

        #endregion

        /// <summary>
        /// 删除该条访客记录
        /// </summary>
        [HttpPost]
        public ActionResult DeleteSpaceVisitors(string spaceKey)
        {
            long id = Request.Form.Get<long>("id", 0);
            visitService.Delete(id);

            //long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            //IEnumerable<Visit> visits = visitService.GetTopVisits(userId, 3);
            return RedirectToAction("_SpaceLastVisitList");
        }

        #endregion

        #region 公共页

        /// <summary>
        /// 页头
        /// </summary>
        /// <returns></returns>
        public ActionResult _Header(string spaceKey)
        {

            #region 消息统计数

            MessageService messageService = new MessageService();
            InvitationService invitationService = new InvitationService();
            NoticeService noticeService = new NoticeService();


            if (UserContext.CurrentUser != null)
            {
                int count = 0;
                count = invitationService.GetUnhandledCount(UserContext.CurrentUser.UserId);
                count += messageService.GetUnreadCount(UserContext.CurrentUser.UserId);
                count += noticeService.GetUnhandledCount(UserContext.CurrentUser.UserId);
                ViewData["PromptCount"] = count;
            }

            #endregion

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

            //查询用于快捷搜索的搜索器
            IEnumerable<ISearcher> searchersQuickSearch = SearcherFactory.GetQuickSearchers(4);
            ViewData["searchersQuickSearch"] = searchersQuickSearch;

            return View();
        }

        /// <summary>
        /// 页脚
        /// </summary>
        /// <returns></returns>
        public ActionResult _Footer(string spaceKey)
        {
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            ViewData["SiteSettings"] = siteSettingsManager.Get();
            return View();
        }

        /// <summary>
        /// 用户空间应用相关面板
        /// </summary>
        /// <param name="spaceKey">空间标示</param>
        /// <param name="avatarSizeType">用户头像尺寸</param>
        [HttpGet]
        public ActionResult _App_Panel(string spaceKey, AvatarSizeType? avatarSizeType)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return new EmptyResult();

            int currentNavigationId = RouteData.Values.Get<int>("CurrentNavigationId", 0);

            NavigationService navigationService = new NavigationService();
            Navigation navigation = navigationService.GetNavigation(PresentAreaKeysOfBuiltIn.UserSpace, currentNavigationId, user.UserId);

            IEnumerable<Navigation> navigations = new List<Navigation>();

            if (navigation != null)
            {
                if (navigation.Depth == 0)
                {
                    navigations = navigation.Children;
                    ViewData["ParentNavigation"] = navigation;
                }
                else if (navigation.Parent != null)
                {
                    navigations = navigation.Parent.Children;
                    ViewData["ParentNavigation"] = navigation.Parent;
                }

                ApplicationModel app = new ApplicationService().Get(navigation.ApplicationId);
                if (app != null)
                {
                    ViewData["Application"] = app;
                    ViewData["AppCount"] = new OwnerDataService(TenantTypeIds.Instance().User()).GetLong(user.UserId, app.ApplicationKey + "-ThreadCount");
                }
                IEnumerable<ApplicationManagementOperation> applicationManagementOperations = new ManagementOperationService().GetShortcuts(PresentAreaKeysOfBuiltIn.UserSpace, false);

                ViewData["ApplicationManagementOperations"] = applicationManagementOperations.Where(n => n.ApplicationId == navigation.ApplicationId && n.PresentAreaKey == PresentAreaKeysOfBuiltIn.UserSpace);
            }

            ViewData["User"] = user;
            ViewData["AvatarSizeType"] = avatarSizeType;

            return View(navigations);
        }

        /// <summary>
        /// 头像上传
        /// </summary>
        /// <param name="spaceKey">空间标示</param>
        [HttpPost]
        public JsonResult _EditAvatar(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            string userName = Request.Form.Get<string>("userName");

            IUserProfileSettingsManager userProfileSettingsManager = DIContainer.Resolve<IUserProfileSettingsManager>();
            UserProfileSettings userProfileSettings = userProfileSettingsManager.GetUserProfileSettings();

            if (Request.Files.Count > 0 && Request.Files["Filedata"] != null)
            {
                Image image = Image.FromStream(Request.Files["Filedata"].InputStream);

                //检查是否需要缩放原图
                if (image.Height < userProfileSettings.AvatarHeight || image.Width < userProfileSettings.AvatarHeight)
                {
                    return Json(new { error = "尺寸太小" });
                }
                else
                {
                    new UserService().UploadOriginalAvatar(userId, Request.Files["Filedata"].InputStream);
                    return Json(new { success = true });
                }
            }
            return null;
        }

        /// <summary>
        /// 无权访问页面
        /// </summary>
        /// <param name="spaceKey">用户空间名</param>
        /// <returns>无权访问页面</returns>
        [HttpGet]
        public ActionResult PrivacyHome(string spaceKey)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (UserContext.CurrentUser == null)
                return Redirect(SiteUrls.Instance().Login(true));
            pageResourceManager.InsertTitlePart("无权访问");
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            ViewData["followedUsers"] = followService.GetTopFollowedUserIds(userId, 30).ToDictionary(n => n, m => userService.GetFullUser(m) == null ? UserIdToUserNameDictionary.GetUserName(m) : userService.GetFullUser(m).DisplayName);
            ViewData["followerUsers"] = followService.GetFollowerUserIds(userId, Follow_SortBy.LastContent_Desc, 1).ToDictionary(n => n, m => userService.GetFullUser(m) == null ? UserIdToUserNameDictionary.GetUserName(m) : userService.GetFullUser(m).DisplayName);
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            CountService countService = new CountService(TenantTypeIds.Instance().User());
            int countStageDay = countService.GetStageCount(CountTypes.Instance().HitTimes(), 7, user.UserId);
            int countAll = countService.Get(CountTypes.Instance().HitTimes(), user.UserId);
            ViewData["accessedCount"] = countStageDay + "/" + countAll;
            ViewData["user"] = user;

            bool seeFollow = false;
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().InviteFollow()))
            {
                seeFollow = true;
            }
            ViewData["seeFollow"] = seeFollow;

            return View();
        }


        /// <summary>
        /// 求关注方法
        /// </summary>
        /// <param name="spaceKey">被求的人的名字</param>
        /// <param name="userId">求关注的人的id</param>
        /// <returns>求关注的结果</returns>
        [HttpPost]
        public ActionResult InviteFollow(string spaceKey, long userId, string remark)
        {
            if (!invitationService.IsSendedInvitation(userId, UserIdToUserNameDictionary.GetUserId(spaceKey), InvitationTypeKeys.Instance().InviteFollow(), 0))
            {
                User user = userService.GetFullUser(userId);

                if (user == null)
                    return Json(new StatusMessageData(StatusMessageType.Error, "找不到您想关注的用户"));

                Invitation invitation = Invitation.New();
                invitation.ApplicationId = 0;
                invitation.InvitationTypeKey = InvitationTypeKeys.Instance().InviteFollow();
                invitation.RelativeObjectId = userId;
                invitation.RelativeObjectName = user.DisplayName;
                invitation.RelativeObjectUrl = SiteUrls.Instance().SpaceHome(userId);
                invitation.Sender = user.DisplayName;
                invitation.SenderUrl = SiteUrls.Instance().SpaceHome(userId);
                invitation.SenderUserId = userId;
                invitation.UserId = UserIdToUserNameDictionary.GetUserId(spaceKey);
                invitation.Remark = remark;
                invitationService.Create(invitation);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "您的求关注请求已经发送"));
        }

        /// <summary>
        /// 求关注局部页
        /// </summary>
        /// <param name="spaceKey">被请求人</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _InviteFollow(string spaceKey)
        {
            IUser currentUser = UserContext.CurrentUser;
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (!privacyService.Validate(userId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().InviteFollow()))
            {
                return Json(new StatusMessageData(StatusMessageType.Hint, "该用户不允许你求关注！"), JsonRequestBehavior.AllowGet);
            }

            if (UserContext.CurrentUser == null)
                return Redirect(SiteUrls.Instance().Login(loginModal: SiteUrls.LoginModal._LoginInModal));
            if (UserIdToUserNameDictionary.GetUserId(spaceKey) == UserContext.CurrentUser.UserId)
            {
                return Json(new StatusMessageData(StatusMessageType.Hint, "不能向自己求关注哦！"), JsonRequestBehavior.AllowGet);
            }

            User user = userService.GetFullUser(spaceKey);
            if (!invitationService.IsSendedInvitation(UserContext.CurrentUser.UserId, UserIdToUserNameDictionary.GetUserId(spaceKey), InvitationTypeKeys.Instance().InviteFollow(), 0))
                return View(new InviteFollowEditModel { DisplayName = user.DisplayName });
            return Json(new StatusMessageData(StatusMessageType.Hint, "您已经发送过了邀请，请耐心等待对方处理"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _FollowUser_Menu(MessageCenterMenu subMenu)
        {
            ViewData["MessageCenterMenu"] = subMenu;
            return View();
        }


        #endregion

        #region 身份认证

        /// <summary>
        /// 创建/编辑身份认证页
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="identificationId">身份认证ID</param>
        [HttpGet]
        public ActionResult EditIdentification(string spaceKey, long identificationId = 0)
        {


            User user = (User)UserContext.CurrentUser;
            if (user == null)
            {
                return Redirect(SiteUrls.Instance().Login(true));
            }

            //对访问权限的判断
            long userid = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (userid != user.UserId)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "没有查看该身份认证的权限",
                    Title = "没有权限",
                    StatusMessageType = StatusMessageType.Hint
                }));
            }


            IdentificationEditModel editModel = null;

            //获取认证标识下拉框
            IEnumerable<IdentificationType> identificationTypes = identificationService.GetIdentificationTypes(true);
            SelectList identificationTypeList = null;
            //创建
            if (identificationId == 0)
            {
                editModel = new IdentificationEditModel();

                //获取当前帐号的默认资料
                user = userService.GetFullUser(user.UserId);
                if (user.Profile != null)
                {
                    editModel.TrueName = user.TrueName;
                    editModel.Email = user.Profile.Email;
                    editModel.Mobile = user.Profile.Mobile;
                }
                pageResourceManager.InsertTitlePart("申请身份认证");
                //创建的话只取该用户没有申请的身份验证标识
                List<Identification> identifications = identificationService.GetUserIdentifications(user.UserId);
                //获取其没申请的标识ID
                IEnumerable<long> notApplyIdentificationTypeIds = identificationTypes.Select(n => n.IdentificationTypeId).Except(identifications.Select(u => u.IdentificationTypeId));
                List<IdentificationType> notApplyidentificationTypes = new List<IdentificationType>();
                foreach (var identificationTypeId in notApplyIdentificationTypeIds)
                {
                    IdentificationType IdentificationType = identificationTypes.Where(n => n.IdentificationTypeId == identificationTypeId).SingleOrDefault();
                    notApplyidentificationTypes.Add(IdentificationType);
                }
                identificationTypeList = new SelectList(notApplyidentificationTypes, "IdentificationTypeId", "Name", editModel.IdentificationTypeId);
                if (notApplyIdentificationTypeIds.Count() == 0)
                {
                    pageResourceManager.InsertTitlePart("身份认证状态");
                    return RedirectToAction("IdentificationResult");
                }
            }
            //编辑
            else
            {
                Identification identification = identificationService.GetIdentification(identificationId);
                if (identification.UserId != user.UserId)
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "没有查看该身份认证的权限",
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }));
                }

                editModel = identification.AsEditModel();
                identificationTypeList = new SelectList(identificationTypes, "IdentificationTypeId", "Name", editModel.IdentificationTypeId);
                ViewData["identificationTypeName"] = identificationTypes.Where(n => n.IdentificationTypeId == editModel.IdentificationTypeId).First().Name;

                pageResourceManager.InsertTitlePart("编辑身份认证");
            }

            ViewData["identificationTypeList"] = identificationTypeList;
            return View(editModel);
        }

        /// <summary>
        /// 创建/编辑身份认证
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="editModel">IdentificationEditModel</param>
        [HttpPost]
        public ActionResult EditIdentification(string spaceKey, IdentificationEditModel editModel)
        {
            //获取认证标识下拉框
            IEnumerable<IdentificationType> identificationTypes = identificationService.GetIdentificationTypes(true);
            SelectList identificationTypeList = null;
            identificationTypeList = new SelectList(identificationTypes, "IdentificationTypeId", "Name", editModel.IdentificationTypeId);
            ViewData["identificationTypeList"] = identificationTypeList;

            string fileName = string.Empty;
            Stream stream = null;
            IUser user = UserContext.CurrentUser;

            HttpPostedFileBase logo = Request.Files["identificationFile"];
            if (logo.ContentLength > 0)
            {
                fileName = logo.FileName;
                LogoSettings logoSettings = logoSettingsManager.Get();
                //校验附件的扩展名
                if (!logoSettings.ValidateFileExtensions(fileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "只允许上传后缀名为" + logoSettings.AllowedFileExtensions.TrimEnd(',') + "的文件");
                    return View(editModel);
                }

                //校验附件的大小
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Identification());
                if (!tenantLogoSettings.ValidateFileLength(logo.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}KB", tenantLogoSettings.MaxLogoLength));
                    return View(editModel);
                }

                stream = logo.InputStream;
            }

            //编辑
            if (editModel.IdentificationId > 0)
            {
                Identification identification = identificationService.GetIdentification(editModel.IdentificationId);
                if (identification.UserId != user.UserId)
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "没有编辑该身份认证的权限",
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }));
                }

                //如果该次申请的身份认证与之前的身份认证相同就不需要判断有没有申请过该认证
                long identificationTypeId = identificationService.GetIdentification(editModel.IdentificationId).IdentificationTypeId;
                if (identificationTypeId != editModel.IdentificationTypeId)
                {
                    //判断该用户有没有申请过该认证
                    if (IsAppliedCommon(editModel.IdentificationTypeId))
                    {
                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "您已经申请过该认证");
                        return View(editModel);
                    }
                }

                identification = editModel.AsIdentification();
                identificationService.UpdateIdentification(identification, stream);
            }
            //创建
            else
            {
                if (logo.ContentLength == 0)
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "证件扫描不能为空");
                    return View(editModel);
                }
                //判断该用户有没有申请过该认证
                if (IsAppliedCommon(editModel.IdentificationTypeId))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "您已经申请过该认证");
                    return View(editModel);
                }

                Identification identification = editModel.AsIdentification();
                identificationService.CreateIdentification(identification, stream);
            }

            return RedirectToAction("IdentificationResult", new { IdentificationTypeId = editModel.IdentificationTypeId });
        }

        /// <summary>
        /// 认证申请状态页
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="identificationTypeId">认证标识ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IdentificationResult(string spaceKey, long identificationTypeId = 0)
        {
            User user = (User)UserContext.CurrentUser;
            if (user == null)
            {
                return Redirect(SiteUrls.Instance().Login(true));
            }

            //对访问权限的判断
            long userid = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (userid != user.UserId)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "没有查看该用户身份认证的权限",
                    Title = "没有权限",
                    StatusMessageType = StatusMessageType.Hint
                }));
            }

            pageResourceManager.InsertTitlePart("身份认证状态");
            Dictionary<IdentificationType, Identification> dic = new Dictionary<IdentificationType, Identification>();
            IEnumerable<IdentificationType> identificationTypes = identificationService.GetIdentificationTypes(user.UserId, false);
            if (identificationTypes == null)
            {
                return View(dic);
            }
            foreach (var identificationType in identificationTypes)
            {
                Identification identification = identificationService.GetUserIdentifications(user.UserId, identificationType.IdentificationTypeId).First();
                dic[identificationType] = identification;
            }
            return View(dic);
        }

        /// <summary>
        ///  删除(取消)身份认证
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="identificationId">身份认证ID</param>
        [HttpPost]
        public ActionResult _DeleteIdentification(string spaceKey, long identificationId)
        {
            Identification identification = identificationService.GetIdentification(identificationId);
            if (identification == null)
            {
                return Json("fail");
            }
            if (identification.UserId != UserContext.CurrentUser.UserId)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "没有取消该身份认证的权限",
                    Title = "没有权限",
                    StatusMessageType = StatusMessageType.Hint
                }));
            }
            identificationService.DeleteIdentification(identificationId);
            return Json("success");
        }

        /// <summary>
        /// 判断该用户有没有申请过某认证
        /// </summary>
        /// <param name="identificationTypeId">认证标识ID</param>
        [HttpGet]
        public ActionResult IsApplied(long identificationTypeId)
        {
            if (IsAppliedCommon(identificationTypeId))
            {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 判断该用户有没有申请过某认证
        /// </summary>
        /// <param name="identificationTypeId">认证标识ID</param>
        /// <returns>bool</returns>
        private bool IsAppliedCommon(long identificationTypeId)
        {
            List<Identification> identifications = identificationService.GetUserIdentifications(UserContext.CurrentUser.UserId, identificationTypeId);
            if (identifications.Count > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 个人资料

        /// <summary>
        /// 个人资料显示
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <returns></returns>
        [UserSpaceAuthorize]
        public ActionResult PersonalInformation(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("个人资料");
            User user = userService.GetFullUser(spaceKey);
            IUser currentUser = UserContext.CurrentUser;

            IEnumerable<WorkExperience> workExperiences = userProfileService.GetWorkExperiences(user.UserId);
            ViewData["workExperiences"] = workExperiences;
            IEnumerable<EducationExperience> educationExperiences = userProfileService.GetEducationExperiences(user.UserId);
            ViewData["educationExperiences"] = educationExperiences;

            PrivacyService privacyService = new PrivacyService();
            bool seeBirthDay = false;
            bool seeMobile = false;
            bool seeEmail = false;
            bool seeQQ = false;
            bool seeMsn = false;
            bool seeWork = false;
            bool seeEducation = false;
            bool seeUserSpace = false;
            bool seeFollow = false;
            bool seeMessage = false;
            bool seeTrueName = false;
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().Birthday()))
            {
                seeBirthDay = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().Mobile()))
            {
                seeMobile = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().Email()))
            {
                seeEmail = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().QQ()))
            {
                seeQQ = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().Msn()))
            {
                seeMsn = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().WorkExperience()))
            {
                seeWork = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().EducationExperience()))
            {
                seeEducation = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().VisitUserSpace()))
            {
                seeUserSpace = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().InviteFollow()))
            {
                seeFollow = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().Message()))
            {
                seeMessage = true;
            }
            if (privacyService.Validate(user.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().TrueName()))
            {
                seeTrueName = true;
            }

            ViewData["seeBirthDay"] = seeBirthDay;
            ViewData["seeMobile"] = seeMobile;
            ViewData["seeEmail"] = seeEmail;
            ViewData["seeQQ"] = seeQQ;
            ViewData["seeMsn"] = seeMsn;
            ViewData["seeWork"] = seeWork;
            ViewData["seeEducation"] = seeEducation;
            ViewData["seeUserSpace"] = seeUserSpace;
            ViewData["seeFollow"] = seeFollow;
            ViewData["seeMessage"] = seeMessage;
            ViewData["seeTrueName"] = seeTrueName;
            return View(user);
        }

        #endregion

        #region Helper Method

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

    #region MessageCenterMenu

    /// <summary>
    /// MessageCenterMenu
    /// </summary>
    public enum MessageCenterMenu
    {

        /// <summary>
        /// 私信
        /// </summary>
        Message,

        /// <summary>
        /// 请求
        /// </summary>
        Invitation,

        /// <summary>
        /// 通知
        /// </summary>
        Notices,

        /// <summary>
        /// 邀请好友
        /// </summary>
        InviteFriend,

        /// <summary>
        /// 我的评论
        /// </summary>
        CommentsInBox,

        /// <summary>
        /// 用户提醒
        /// </summary>
        UserReminder

    }

    #endregion MessageCenterMenu

    /// <summary>
    /// 用户内容数的数据
    /// </summary>
    public class OwnerStatisticData
    {

        public string DataName { set; get; }

        public string DataUrl { set; get; }

        public long ContentCount { set; get; }
    }
}