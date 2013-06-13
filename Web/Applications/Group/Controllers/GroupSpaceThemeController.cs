//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.UI;
using Tunynet.Utilities;

namespace Spacebuilder.Group.Controllers
{
    [Themed(PresentAreaKeysOfBuiltIn.GroupSpace, IsApplication = false)]
    [AnonymousBrowseCheck]
    [TitleFilter(IsAppendSiteName = true)]
    [GroupSpaceAuthorize]
    public class GroupSpaceThemeController : Controller
    {
        private GroupService groupService = new GroupService();
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private VisitService visitService = new VisitService(TenantTypeIds.Instance().Group());
        private SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().Group());
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private FollowService followService = new FollowService();
        private ActivityService activityService = new ActivityService();
        private ApplicationService applicationService = new ApplicationService();


        /// <summary>
        /// 页头
        /// </summary>
        /// <returns></returns>
        public ActionResult _Header(string spaceKey)
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

            //查询用于快捷搜索的搜索器
            IEnumerable<ISearcher> searchersQuickSearch = SearcherFactory.GetQuickSearchers(4);
            ViewData["searchersQuickSearch"] = searchersQuickSearch;

            NavigationService service = new NavigationService();
            ViewData["Navigations"] = service.GetRootNavigations(PresentAreaKeysOfBuiltIn.Channel).Where(n => n.IsVisible(UserContext.CurrentUser) == true);

            return PartialView();
        }

        /// <summary>
        /// 页脚
        /// </summary>
        /// <returns></returns>
        public ActionResult _Footer(string spaceKey)
        {

            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            ViewData["SiteSettings"] = siteSettingsManager.Get();

            return PartialView();
        }

        /// <summary>
        /// 群组头部信息
        /// </summary>
        /// <param name="spaceKey">用户标识</param>
        /// <param name="showManageButton">显示管理按钮</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _GroupHeader(string spaceKey, bool showManageButton)
        {
            GroupEntity group = groupService.Get(spaceKey);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewData["showManageButton"] = showManageButton;
            return View(group);
        }

        /// <summary>
        /// 群组空间主页
        /// </summary>
        /// <param name="spaceKey">群组标识</param>
        /// <returns></returns>
        public ActionResult Home(string spaceKey)
        {
            GroupEntity group = groupService.Get(spaceKey);
            
            //已修改
            if (group == null)
                return HttpNotFound();

            IEnumerable<ApplicationBase> applications = applicationService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfBuiltIn.GroupSpace, group.GroupId);
            

            //这里先判断group是否为空，再插入了群组名
            pageResourceManager.InsertTitlePart(group.GroupName);

            //浏览计数
            new CountService(TenantTypeIds.Instance().Group()).ChangeCount(CountTypes.Instance().HitTimes(), group.GroupId, group.UserId);

            
            //1.为什么匿名用户就不让访问？
            //2.这里有个大问题，私密群组应该不允许非群组成员访问，
            //可以参考Common\Mvc\Attributes\UserSpaceAuthorizeAttribute.cs，在Group\Extensions\增加一个GroupSpaceAuthorizeAttribute
            //3.当设置为私密群组并且允许用户申请加入时，应允许用户浏览群组首页，但只能看部分信息，具体需求可找宝声确认；
            
            //当前用户
            IUser user = UserContext.CurrentUser;
            if (user != null)
            {
                
                //ok，传递这些结果可以吗？
                //已修改
                //这样做很不好，直接用Authorizer
                bool isMember = groupService.IsMember(group.GroupId, user.UserId);
                visitService.CreateVisit(user.UserId, user.DisplayName, group.GroupId, group.GroupName);
                ViewData["isMember"] = isMember;
            }
            ViewData["isPublic"] = group.IsPublic;
            TempData["GroupMenu"] = GroupMenu.Home;
            ViewData["applications"] = applications;

            return View(group);
        }

        /// <summary>
        /// 群组空间主导航
        /// </summary>
        /// <param name="spaceKey">群组空间标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Menu_App(string spaceKey)
        {
            long groupId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            GroupEntity group = groupService.Get(groupId);
            if (group == null)
                return Content(string.Empty);

            int currentNavigationId = RouteData.Values.Get<int>("CurrentNavigationId", 0);


            NavigationService navigationService = new NavigationService();
            Navigation navigation = navigationService.GetNavigation(PresentAreaKeysOfBuiltIn.GroupSpace, currentNavigationId, group.GroupId);

            IEnumerable<Navigation> navigations = new List<Navigation>();
            if (navigation != null)
            {
                if (navigation.Depth >= 1 && navigation.Parent != null)
                {
                    navigations = navigation.Parent.Children;
                }
                else if (navigation.Depth == 0)
                {
                    navigations = navigation.Children;
                }


                ManagementOperationService managementOperationService = new ManagementOperationService();
                IEnumerable<ApplicationManagementOperation> applicationManagementOperations = managementOperationService.GetShortcuts(PresentAreaKeysOfBuiltIn.GroupSpace, false);
                if (applicationManagementOperations != null)
                {
                    ViewData["ApplicationManagementOperations"] = applicationManagementOperations.Where(n => n.ApplicationId == navigation.ApplicationId);
                }
            }

            return View(navigations);
        }

        /// <summary>
        /// 群组空间主导航
        /// </summary>
        /// <param name="spaceKey">群组空间标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Menu_Main(string spaceKey)
        {
            long groupId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            GroupEntity group = groupService.Get(groupId);
            if (group == null)
                return Content(string.Empty);

            ManagementOperationService managementOperationService = new ManagementOperationService();
            ViewData["ApplicationManagementOperations"] = managementOperationService.GetShortcuts(PresentAreaKeysOfBuiltIn.GroupSpace, false);

            NavigationService navigationService = new NavigationService();
            return View(navigationService.GetRootNavigations(PresentAreaKeysOfBuiltIn.GroupSpace, group.GroupId));
        }

        /// <summary>
        /// 群组信息
        /// </summary>
        /// <param name="spaceKey">群组标识</param>
        /// <param name="showGroupLogo">显示群组Logo</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _GroupInfo(string spaceKey, bool? showGroupLogo)
        {
            long groupId = GroupIdToGroupKeyDictionary.GetGroupId(spaceKey);
            GroupEntity group = groupService.Get(groupId);
            if (group == null)
                return Content(string.Empty);

            ViewData["showGroupLogo"] = showGroupLogo;

            return View(group);
        }

    }
}