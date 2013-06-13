using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;
using Tunynet.UI;
using Tunynet.Mvc;
using System.Web;
using System.Web.Security;
using System.Linq;
using System;
using Tunynet.Utilities;
using System.Security.Policy;
using Tunynet.Common.Configuration;
using Tunynet.License;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 后台控制面板Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = false)]
    public class ControlPanelController : Controller
    {
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private IMembershipService membershipService = DIContainer.ResolvePerHttpRequest<IMembershipService>();
        private UserService userService = new UserService();
        private RecommendService recommendService = new RecommendService();
        private NavigationService navigationService = new NavigationService();

        #region 后台首页
        /// <summary>
        /// 后台首页
        /// </summary>
        [HttpGet]
        [ManageAuthorize(CheckApplication = false)]
        public ActionResult Home()
        {
            pageResourceManager.InsertTitlePart("后台首页");

            ApplicationService applicationService = new ApplicationService();
            //获得所有应用的应用名
            IEnumerable<ApplicationBase> applications = applicationService.GetAll();
            //初始化待处理应用数组
            List<ApplicationStatisticData> allManageableDatas = new List<ApplicationStatisticData>();
            //初始化数据统计项实体数组
            List<ApplicationStatisticItem> statisticItems = new List<ApplicationStatisticItem>();

            var authorizer = new Authorizer();
            var isApplicationAdmin = !UserContext.CurrentUser.IsInRoles(RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator());

            //遍历应用名
            foreach (var application in applications)
            {
                if (isApplicationAdmin)
                {
                    if (!authorizer.IsAdministrator(application.ApplicationId))
                        continue;
                }
                //获取所有待处理数据实体
                IEnumerable<ApplicationStatisticData> manageableDatas = DIContainer.ResolveNamed<IApplicationStatisticDataGetter>(application.ApplicationKey).GetManageableDatas();
                //获取所有数据统计应用数据实体
                IEnumerable<ApplicationStatisticData> applicationStatisticDatas = DIContainer.ResolveNamed<IApplicationStatisticDataGetter>(application.ApplicationKey).GetStatisticDatas();
                //将所有待处理数据实体添加到未处理应用数组
                allManageableDatas.AddRange(manageableDatas);
                //遍历所有应用简称
                foreach (string shortName in applicationStatisticDatas.Select(n => n.ShortName))
                {
                    //如果数组中已存在该应用则继续
                    var item = statisticItems.Where(n => n.ShortName == shortName).FirstOrDefault();
                    if (statisticItems.Contains(item))
                    {
                        continue;
                    }
                    //获取该简称下的应用数组
                    IEnumerable<ApplicationStatisticData> datas = applicationStatisticDatas.Where(n => n.ShortName == shortName);
                    //获取该简称下的总数应用
                    ApplicationStatisticData applicationStatisticDataTotal = datas.Where(n => n.DataKey == ApplicationStatisticDataKeys.Instance().TotalCount()).FirstOrDefault();
                    //初始化总数
                    long totalCount = 0;
                    //初始化URL
                    string itemUrl = null;
                    //如果存在总数应用
                    if (applicationStatisticDataTotal != null)
                    {
                        //为数据统计项实体总数和url赋值
                        totalCount = applicationStatisticDataTotal.Value;
                        itemUrl = applicationStatisticDataTotal.Url;
                    }
                    //获取该简称下的24小时新增数应用
                    var applicationStatisticDataLast24H = datas.Where(n => n.DataKey == ApplicationStatisticDataKeys.Instance().Last24HCount()).FirstOrDefault();
                    //初始化24小时新增数
                    long Last24H = 0;
                    //如果存在24小时新增数应用
                    if (applicationStatisticDataLast24H != null)
                        //为总数项实体24小时新增数赋值
                        Last24H = applicationStatisticDataLast24H.Value;
                    //实例化数据统计项实体
                    ApplicationStatisticItem appItem = new ApplicationStatisticItem(shortName, totalCount, Last24H);
                    appItem.Url = itemUrl;
                    //添加到数组
                    statisticItems.Add(appItem);
                }
            }
            //获取待处理事项实体数组
            ViewData["allManageableDatas"] = allManageableDatas;

            //增加有多少个待审核评论
            PagingDataSet<Comment> comments = new CommentService().GetComments(PubliclyAuditStatus.Pending,null,null,null,null,20,1);
            ViewData["commentsCount"] = comments.TotalRecords;

            //多少未读客服消息
            ViewData["customMessageCount"]=new MessageService().GetUnreadCount((long)BuildinMessageUserId.CustomerService);

            //待处理用户举报
            ViewData["impeachCount"]= new ImpeachReportService().GetsForAdmin(false,null).TotalRecords;

            //待激活的用户
            UserQuery userQuery = new UserQuery();
            userQuery.IsActivated = false;
            ViewData["activatedUserCount"]= userService.GetUsers(userQuery,20,1).TotalRecords;

            //24小时新增用户数
            ViewData["userCount24H"] =  userService.GetUser24H();

            //总用户数
            UserQuery usersQuery = new UserQuery();
            ViewData["userCount"] =  userService.GetUsers(usersQuery, 20, 1).TotalRecords;

            //获取系统信息
            
            SystemInfo systemInfo = new SystemInfo();
            ViewData["systemInfo"] = systemInfo;
            return View(statisticItems);
        }

        #region 版本信息
        /// <summary>
        /// 版本信息内容块
        /// </summary>
        /// <returns></returns>
        public ActionResult _VersionInfo()
        {
            TunyNetLicenseManager.Instance().Validate();
            ViewData["tunyNetLicenses"] = GetLicenses();
            string friendlyVersion = string.Empty;
            var meta = pageResourceManager.GetRegisteredMetas().FirstOrDefault(n => n.Name == "generator");
            if (meta != null)
                friendlyVersion = meta.Content;
            ViewData["spacebuilderVersion"] = string.Format("{0}({1})", friendlyVersion, GetSpacebuilderVersion());
            return View();
        }

        private List<TunyNetLicense> GetLicenses()
        {
            Dictionary<string, TunyNetLicense> licenseCollection = TunyNetLicenseManager.Instance().LicenseCollection;
            List<TunyNetLicense> tunyNetLicenses = null;
            if (licenseCollection.Any(n => n.Value.Product.Level != "Free"))
                tunyNetLicenses = licenseCollection.Values.Where(n => n.Product.Level != "Free").ToList();
            else
                tunyNetLicenses = new List<TunyNetLicense>(licenseCollection.Values);
            return tunyNetLicenses;
        }

        /// <summary>
        /// 获取SPB的详细版本信息
        /// </summary>
        /// <returns></returns>
        private string GetSpacebuilderVersion()
        {

            Type t = typeof(ControlPanelController);
            Version spaceBuilderVersion = t.Assembly.GetName().Version;
            return spaceBuilderVersion.ToString();
        }

        /// <summary>
        /// 获取SpaceBuilder最新版本信息
        /// </summary>
        public ActionResult GetMostRecentVersion()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://get.tunynet.com/version.ashx");
                request.Timeout = 90000;

                // Request the latest version
                XmlTextReader reader = new XmlTextReader(request.GetResponse().GetResponseStream());
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);

                // Find the most recent version
                var mostRecentVersion = new Version(doc.SelectSingleNode("root/*[translate(name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = \"spacebuilder\"]/mostRecentVersion").InnerText);
                var mostRecentVersionInfo = doc.SelectSingleNode("root/*[translate(name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = \"spacebuilder\"]/mostRecentVersionInfo").InnerText;
                var mostRecentVersionHistoryUrl = doc.SelectSingleNode("root/*[translate(name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = \"spacebuilder\"]/mostRecentVersionHistoryUrl").InnerText;
                Type t = typeof(ControlPanelController);
                Version spaceBuilderVersion = t.Assembly.GetName().Version;
                return Json(new { mostRecentVersionInfo = mostRecentVersionInfo, mostRecentVersion = mostRecentVersion.ToString(), mostRecentVersionHistoryUrl = mostRecentVersionHistoryUrl, hasRecentVersion = mostRecentVersion > spaceBuilderVersion }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 后台登录页
        /// </summary>
        [HttpGet]
        public ActionResult ManageLogin(string returnUrl)
        {

            pageResourceManager.InsertTitlePart("管理后台");
            pageResourceManager.InsertTitlePart("登录");

            SiteSettings siteSettings = DIContainer.Resolve<ISiteSettingsManager>().Get();
            ViewData["siteSettings"] = siteSettings;
            return View(new LoginEditModel { loginInModal = false, ReturnUrl = returnUrl });
        }

        /// <summary>
        /// 后台登录验证
        /// </summary>
        [HttpPost]
        [CaptchaVerify(VerifyScenarios.Login)]
        public ActionResult ManageLogin(LoginEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            Authorizer authorizer = new Authorizer();

            User user = model.AsUser();
            UserLoginStatus userLoginStatus = membershipService.ValidateUser(user.UserName, user.Password);

            if (userLoginStatus == UserLoginStatus.InvalidCredentials)
            {
                IUser userByEmail = userService.FindUserByEmail(user.UserName);
                if (userByEmail != null)
                {
                    user = userByEmail as User;
                    userLoginStatus = membershipService.ValidateUser(userByEmail.UserName, model.Password);
                }
            }
            else
            {
                user = userService.GetFullUser(model.UserName);
            }

            if (userLoginStatus == UserLoginStatus.InvalidCredentials)
            {
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "帐号或密码错误，请重新输入！");
            }
            if (userLoginStatus == UserLoginStatus.Success)
            {
                if (user.IsAllowEntryControlPannel())
                {
                    HttpCookie adminCookie = new HttpCookie("SpacebuilderAdminCookie");
                    adminCookie.Value = Utility.EncryptTokenForAdminCookie(true.ToString());
                    if (!string.IsNullOrEmpty(FormsAuthentication.CookieDomain))
                        adminCookie.Domain = FormsAuthentication.CookieDomain;
                    adminCookie.HttpOnly = true;
                    Response.Cookies.Add(adminCookie);
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                }
                else
                    return Redirect(SiteUrls.Instance().ManageLogin());

                string redirectUrl = null;
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                    redirectUrl = model.ReturnUrl;
                else
                    redirectUrl = SiteUrls.Instance().ManageHome();
                return Redirect(redirectUrl);
            }
            return View(model);
        }

        /// <summary>
        /// 正在执行
        /// </summary>
        public ActionResult Operating(string message, string backUrl, string operationUrl)
        {
            return View();
        }

        /// <summary>
        /// 执行成功
        /// </summary>
        public ActionResult Success(string message, string backUrl)
        {
            return View();
        }

        /// <summary>
        /// 执行失败
        /// </summary>
        public ActionResult Error(string message, string operationUrl, string backUrl)
        {
            return View();
        }

        /// <summary>
        /// 页头
        /// </summary>
        /// <returns></returns>
        public ActionResult _Header()
        {

            NavigationService service = new NavigationService();

            if (RouteData.Values != null && RouteData.Values.ContainsKey("CurrentNavigationId"))
            {
                int currentNavigationId = Convert.ToInt32(RouteData.Values["CurrentNavigationId"].ToString());
                IEnumerable<int> currentNavigationPath = service.GetCurrentNavigationPath(PresentAreaKeysOfBuiltIn.ControlPanel, 0, currentNavigationId);
                if (currentNavigationPath != null && currentNavigationPath.Count() > 0)
                    ViewData["CurrentNavigationId"] = currentNavigationPath.First();
            }

            ViewData["rootnavigations"] = service.GetRootNavigations(PresentAreaKeysOfBuiltIn.ControlPanel);

            return View();
        }

        /// <summary>
        /// 页脚
        /// </summary>
        /// <returns></returns>
        public ActionResult _Footer()
        {
            return View();
        }

        /// <summary>
        /// 应用管理数据内容块
        /// </summary>
        /// <returns></returns>
        public ActionResult _ManageableDataDetail(string applicationKey, string tenantTypeId)
        {
            
            IEnumerable<ApplicationStatisticData> statisticDatas = DIContainer.ResolveNamed<IApplicationStatisticDataGetter>(applicationKey).GetManageableDatas(tenantTypeId);
            return PartialView(statisticDatas);
        }

        /// <summary>
        /// 应用统计数据内容块
        /// </summary>
        /// <returns></returns>
        public ActionResult _StatisticDataDetail(string applicationKey, string tenantTypeId)
        {
            
            IEnumerable<ApplicationStatisticData> statisticDatas = DIContainer.ResolveNamed<IApplicationStatisticDataGetter>(applicationKey).GetStatisticDatas(tenantTypeId);

            return PartialView(statisticDatas);
        }

        
        
        

        /// <summary>
        /// 推荐项管理
        /// </summary>
        /// <param name="recommendTypeId"></param>
        /// <returns></returns>
        public ActionResult _ManageRecommendItems(string recommendTypeId, bool showLinkButton = true)
        {
            IEnumerable<RecommendItem> recommendItems = recommendService.GetTops(1000, recommendTypeId);
            ViewData["RecommendType"] = recommendService.GetRecommendType(recommendTypeId);
            ViewData["showLinkButton"] = showLinkButton;
            return View(recommendItems);
        }

        /// <summary>
        /// 推荐内容页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _RecommendItem(string tenantTypeId, long itemId = 0, string itemName = null, string recommendItemTypeId = null, bool showLink = false, long recommendId = 0, bool showInList = true, long userId = 0)
        {
            RecommendItem recommendItem = null;
            if (recommendId > 0)
            {
                recommendItem = recommendService.Get(recommendId);
                if (recommendItem == null)
                    return HttpNotFound();
                tenantTypeId = recommendItem.TenantTypeId;
                itemId = recommendItem.ItemId;
                recommendItemTypeId = recommendItem.TypeId;
                ViewData["RecommendTypeName"] = recommendItem.RecommendItemType.Name;
                ViewData["RecommendTypeId"] = recommendItem.TypeId;
            }
            if (!new Authorizer().RecommendItem_Manage(tenantTypeId))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "没有管理推荐内容的权限",
                    Title = "没有权限",
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            IEnumerable<RecommendItemType> itemTypes = recommendService.GetRecommendTypes(tenantTypeId);
            IEnumerable<RecommendItem> recommendItems = recommendService.Gets(itemId, tenantTypeId);
            
            //已修改
            RecommendItemEditModel itemEditModel = new RecommendItemEditModel();

            ViewData["recommendItems"] = recommendItems;
            
            //已修改
            ViewData["TypeId"] = new SelectList(itemTypes, "TypeId", "Name", recommendItemTypeId);
            if (recommendId != 0)
            {
                itemEditModel = recommendItem.AsEditModel();
                ViewData["HasFeaturedImage"] = recommendItem.RecommendItemType.HasFeaturedImage;
            }
            else
            {
                if (itemTypes != null && itemTypes.Count() > 0 && string.IsNullOrEmpty(recommendItemTypeId))
                {
                    
                    //已修改
                    recommendItemTypeId = itemTypes.First().TypeId;
                }
                if (!string.IsNullOrEmpty(recommendItemTypeId))
                {
                    recommendItem = recommendService.Get(itemId, recommendItemTypeId);
                    RecommendItemType recommendType = recommendService.GetRecommendType(recommendItemTypeId);
                    ViewData["HasFeaturedImage"] = recommendType.HasFeaturedImage;
                    ViewData["RecommendTypeName"] = recommendType.Name;
                    ViewData["RecommendTypeId"] = recommendType.TypeId;
                    if (recommendItem != null && !recommendItem.IsLink)
                    {
                        ViewData["ExpiredDate"] = recommendItem.ExpiredDate;
                        itemEditModel = recommendItem.AsEditModel();
                    }
                    else
                    {
                        RecommendItem newItem = RecommendItem.New();
                        newItem.ItemName = itemName;
                        newItem.ItemId = itemId;
                        newItem.TenantTypeId = tenantTypeId;
                        newItem.ExpiredDate = DateTime.UtcNow.AddMonths(1);
                        newItem.UserId = userId;
                        itemEditModel = newItem.AsEditModel();
                    }
                }
                
                else
                {
                    StatusMessageData message = null;
                    TenantTypeService tenantTypeService = new TenantTypeService();
                    TenantType tenantType = tenantTypeService.Get(tenantTypeId);
                    
                    if (tenantType == null)
                    {
                        message = new StatusMessageData(StatusMessageType.Hint, "没有推荐类别");
                    }
                    else
                    {
                        message = new StatusMessageData(StatusMessageType.Hint, tenantType.Name + "下没有推荐类别");
                    }
                    ViewData["statusMessageData"] = message;
                }
            }
            ViewData["showLink"] = showLink || itemEditModel.IsLink;
            ViewData["showInList"] = showInList;
            return View(itemEditModel);
        }

        /// <summary>
        /// 推荐内容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _RecommendItem(RecommendItemEditModel recommendItemEditModel)
        {
            if (!new Authorizer().RecommendItem_Manage(recommendItemEditModel.TenantTypeId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有管理权限"));
            StatusMessageData message = null;
            System.IO.Stream stream = null;
            HttpPostedFileBase logoImage = Request.Files["LogoImage"];
            
            if (logoImage != null && !string.IsNullOrEmpty(logoImage.FileName))
            {
                stream = logoImage.InputStream;
                recommendItemEditModel.FeaturedImage = logoImage.FileName;
            }
            RecommendItem item = recommendItemEditModel.AsRecommendItem();

            if (recommendItemEditModel.Id == 0)
            {

                bool result = recommendService.Create(item);
                if (result)
                {
                    message = new StatusMessageData(StatusMessageType.Success, "推荐成功！");
                }
                else
                {
                    message = new StatusMessageData(StatusMessageType.Error, "推荐失败！");
                }
            }
            else
            {
                recommendService.Update(item);
                message = new StatusMessageData(StatusMessageType.Success, "编辑成功！" + "&" + StringUtility.Trim(item.ItemName, 18) + "&" + item.ItemName);
            }

            
            //已修改
            if (stream != null)
            {
                recommendService.UploadLogo(item.Id, stream);
            }

            return Content(System.Web.Helpers.Json.Encode(message));
        }

        
        //已修改
        /// <summary>
        /// 删除推荐内容
        /// </summary>
        /// <param name="recommendId">推荐实体ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _DeleteRecommend(long recommendId)
        {
            RecommendItem recommendItem = recommendService.Get(recommendId);
            if (recommendItem == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到所要删除的推荐内容！"));
            if (!new Authorizer().RecommendItem_Manage(recommendItem.TenantTypeId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有管理权限"));
            StatusMessageData message = null;
            bool result = recommendService.Delete(recommendId);
            if (result)
            {
                message = new StatusMessageData(StatusMessageType.Success, "删除成功！");
            }
            else
            {
                message = new StatusMessageData(StatusMessageType.Error, "删除失败！");
            }
            return Json(message);
            
            //已修改
        }

        /// <summary>
        /// 删除推荐内容Logo
        /// </summary>
        /// <param name="recommendId">推荐实体ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _DeleteRecommendLogo(long recommendId)
        {
            RecommendItem recommendItem = recommendService.Get(recommendId);
            if (recommendItem == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到所要删除的推荐内容！"));
            if (!new Authorizer().RecommendItem_Manage(recommendItem.TenantTypeId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有管理权限"));

            recommendService.DeleteLogo(recommendId);

            recommendItem.FeaturedImage = string.Empty;
            recommendService.Update(recommendItem);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除标题图成功！"));
        }

        /// <summary>
        /// 改变推荐内容的排序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _ChangeRecommendOrder(long id, long referenceId)
        {
            recommendService.ChangeDisplayOrder(id, referenceId);
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }

        /// <summary>
        /// 侧边菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult _AsideMenu()
        {
            NavigationService service = new NavigationService();
            int currentNavigationId = RouteData.Values.Get<int>("CurrentNavigationId", 0);

            IEnumerable<Navigation> navigations = null;
            if (currentNavigationId > 0)
            {
                IEnumerable<int> currentNavigationPath = service.GetCurrentNavigationPath(PresentAreaKeysOfBuiltIn.ControlPanel, 0, currentNavigationId);
                IEnumerable<Navigation> rootNavigations = service.GetRootNavigations(PresentAreaKeysOfBuiltIn.ControlPanel);

                Navigation parentNavigation = null;
                int parentNavigationId = 0;
                if (currentNavigationPath.Count() > 1)
                {
                    parentNavigationId = currentNavigationPath.ElementAt(currentNavigationPath.Count() - 2);
                }
                else if (currentNavigationPath.Count() == 1)
                {
                    parentNavigationId = currentNavigationPath.First();
                }

                parentNavigation = service.GetNavigation(PresentAreaKeysOfBuiltIn.ControlPanel, parentNavigationId);
                if (parentNavigation != null)
                {
                    if (parentNavigation.Depth > 0)
                    {
                        Navigation navigation = service.GetNavigation(PresentAreaKeysOfBuiltIn.ControlPanel, parentNavigation.ParentNavigationId);
                        navigations = navigation.Children;
                    }
                    else
                    {
                        navigations = parentNavigation.Children;
                    }
                }
            }

            return View(navigations);
        }

        #region 后台功能地图、常用操作、功能搜索
        /// <summary>
        /// 后台功能地图
        /// </summary>
        /// <returns></returns>
        public ActionResult _FunctionMap()
        {
            IEnumerable<InitialNavigation> controlPanelNavigations = navigationService.GetRootInitialNavigation(PresentAreaKeysOfBuiltIn.ControlPanel);
            return View(controlPanelNavigations);
        }

        /// <summary>
        /// 更新常用操作页
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult UpdateCommonOperations()
        {
            pageResourceManager.InsertTitlePart("添加常用操作");
            IEnumerable<InitialNavigation> controlPanelNavigations = navigationService.GetRootInitialNavigation(PresentAreaKeysOfBuiltIn.ControlPanel);
            return View(controlPanelNavigations);
        }

        /// <summary>
        /// 添加常用操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCommonOperations(List<int> navigationIds)
        {
            var userId = UserContext.CurrentUser.UserId;
            List<CommonOperation> commonOperations = new List<CommonOperation>();

            if (navigationIds != null)
            {
                foreach (var navigationId in navigationIds)
                {
                    CommonOperation commonOperation = new CommonOperation();
                    commonOperation.UserId = userId;
                    commonOperation.NavigationId = navigationId;
                    commonOperations.Add(commonOperation);
                }
                navigationService.AddCommonOperations(userId, commonOperations);
            }
            else
            {
                navigationService.ClearUserCommonOperations(userId);
            }
            TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "更新成功");
            return RedirectToAction("UpdateCommonOperations");
        }

        /// <summary>
        /// 常用操作侧边栏
        /// </summary>
        /// <returns></returns>
        public ActionResult _CommonOperationMenu()
        {
            if (!UserContext.CurrentUser.IsInRoles(RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator()))
                return new EmptyResult();
            var userId = UserContext.CurrentUser.UserId;
            IEnumerable<InitialNavigation> commonOperations = navigationService.GetCommonOperations(userId);
            return View(commonOperations);
        }

        /// <summary>
        /// 功能搜索
        /// </summary>
        /// <param name="searchOperationKeyword">关键字</param>
        /// <returns></returns>
        public ActionResult SearchOperations(string searchOperationKeyword)
        {
            pageResourceManager.InsertTitlePart("功能搜索");
            IEnumerable<InitialNavigation> commonOperations = navigationService.SearchOperations(searchOperationKeyword);
            return View(commonOperations);
        }

        /// <summary>
        /// 最近访问
        /// </summary>
        /// <returns></returns>
        public ActionResult _RecentVisit()
        {
            int currentNavigationId = RouteData.Values.Get<int>("CurrentNavigationId", 0);

            if (currentNavigationId != 0)
            {
                int depth = navigationService.GetInitialNavigation(currentNavigationId).Depth;
                HttpCookie recentVisitCookies = Request.Cookies["ControlPanel_RecentVisit"];
                string temp = null;
                if (recentVisitCookies != null && depth != 0)
                {
                    temp = recentVisitCookies.Value.Replace("," + currentNavigationId.ToString(), "") + "," + currentNavigationId.ToString();
                }
                else if (recentVisitCookies != null)
                {
                    temp = recentVisitCookies.Value;
                }
                else if (depth != 0)
                {
                    temp = "," + currentNavigationId.ToString();
                }
                else
                {
                    return null;
                }
                HttpCookie cookie = new HttpCookie("ControlPanel_RecentVisit", temp);
                Response.Cookies.Add(cookie);
                List<InitialNavigation> recentVisitList = new List<InitialNavigation>();
                IEnumerable<InitialNavigation> finallyRecentVisitList = new List<InitialNavigation>();

                if (cookie != null)
                {
                    string list = cookie.Value;
                    if (!string.IsNullOrEmpty(list))
                    {
                        list = list.Replace("%2C", ",");
                        string[] idsstr = list.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string ids in idsstr)
                        {
                            int navigationId = 0;
                            int.TryParse(ids, out navigationId);
                            InitialNavigation navigation = navigationService.GetInitialNavigation(navigationId);
                            if (navigation != null)
                                recentVisitList.Add(navigation);
                        }
                        recentVisitList.Reverse();
                    }
                }
                if (recentVisitList != null)
                {
                    return View(recentVisitList.Take(5));
                }

            }
            return null;
        }

        #endregion
    }
}