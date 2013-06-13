//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------ 

using System.Web.Mvc;
using Tunynet.Common;
using Tunynet.UI;
using Tunynet;
using Tunynet.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.RegularExpressions;
using Tunynet.Mvc;
using Spacebuilder.Common;
using Tunynet.Utilities;
using Spacebuilder.Common.Configuration;
using System.Web;
using Tunynet.Email;
using System.Net.Mail;



namespace Spacebuilder.Common
{
    /// <summary>
    /// 积分规则控制面板Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = false)]
    [TitleFilter(IsAppendSiteName = true, TitlePart = "后台管理")]
    [ManageAuthorize(RequireSystemAdministrator = true)]
    public class ControlPanelSettingsController : Controller
    {
        #region private items
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private PointService pointService = new PointService();
        private AuditService auditService = new AuditService();
        private RoleService roleService = new RoleService();
        private ApplicationService appService = new ApplicationService();
        private PrivacyService privacyService = new PrivacyService();
        private PermissionService permissionService = new PermissionService();
        private NavigationService navigationService = new NavigationService();
        private ManagementOperationService operationService = new ManagementOperationService();
        private AreaService areaService = new AreaService();
        private SchoolService schoolService = new SchoolService();
        private ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
        private IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
        private IUserProfileSettingsManager userProfileSettingsManager = DIContainer.Resolve<IUserProfileSettingsManager>();
        private ICommentSettingsManager commentSettingsManager = DIContainer.Resolve<ICommentSettingsManager>();
        private IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
        private IAttachmentSettingsManager attachmentSettingsManager = DIContainer.Resolve<IAttachmentSettingsManager>();
        private IEmailSettingsManager emailSettingsManager = DIContainer.Resolve<IEmailSettingsManager>();
        private IReminderSettingsManager reminderSettingsManager = DIContainer.Resolve<IReminderSettingsManager>();
        private IPauseSiteSettingsManager pauseSiteSettingsManager = DIContainer.Resolve<IPauseSiteSettingsManager>();
        private ThemeService themeService = new ThemeService();
        private PresentAreaService presentAreaService = new PresentAreaService();
        private EmotionService emotionService = DIContainer.Resolve<EmotionService>();
        private SensitiveWordService sensitiveWordService = new SensitiveWordService();
        private EmailService emailService = new EmailService();
        private NoticeSettingsManager noticeSettingsManager = new NoticeSettingsManager();
        private InvitationSettingsManager invitationSettingsManager = new InvitationSettingsManager();
        private NoticeService noticeService = new NoticeService();
        #endregion

        #region 设置

        /// <summary>
        /// 站点设置首页
        /// </summary>
        [HttpGet]
        public ActionResult SiteSettings()
        {
            pageResourceManager.InsertTitlePart("基础设置");
            SiteSettings siteSettings = siteSettingsManager.Get();
            CommentSettings commentSettings = commentSettingsManager.Get();

            SiteSettingsEditModel siteSettingsEditModel = new SiteSettingsEditModel(siteSettings, commentSettings);

            //读取分享到其他网站目录
            string systemPath = WebUtility.GetPhysicalFilePath(WebUtility.ResolveUrl("~/Plugins/ShareToThird/"));
            DirectoryInfo dir = new DirectoryInfo(systemPath);
            FileInfo[] finfo = dir.GetFiles();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            SelectListItem itemclose = new SelectListItem();
            itemclose.Text = "关闭";
            itemclose.Value = "";
            selectListItems.Add(itemclose);

            for (int i = 0; i < finfo.Length; i++)
            {
                SelectListItem item = new SelectListItem();
                item.Text = finfo[i].Name.Split('.')[0];
                item.Value = finfo[i].Name.Split('.')[0];
                selectListItems.Add(item);
            }
            SelectList selectListBusiness = new SelectList(selectListItems, "Value", "Text", siteSettingsEditModel.ShareToThirdBusiness);
            ViewData["selectListBusiness"] = selectListBusiness;



            return View(siteSettingsEditModel);
        }

        /// <summary>
        /// 保存站点基础设置
        /// </summary>
        /// <param name="siteSettingsEditModel">站点设置</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSiteSettings(SiteSettingsEditModel siteSettingsEditModel)
        {
            if (siteSettingsEditModel.ShareToThirdBusiness == null || siteSettingsEditModel.ShareToThirdBusiness == "")
                siteSettingsEditModel.ShareToThirdIsEnabled = false;
            else
                siteSettingsEditModel.ShareToThirdIsEnabled = true;

            SiteSettings siteSettings = siteSettingsEditModel.AsSiteSettings();
            CommentSettings commentSettings = siteSettingsEditModel.AsCommentSettings();

            siteSettingsManager.Save(siteSettings);
            commentSettingsManager.Save(commentSettings);

            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }


        /// <summary>
        /// 用户设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserSettings()
        {
            pageResourceManager.InsertTitlePart("用户设置");
            UserSettings userSettings = userSettingsManager.Get();
            UserProfileSettings userProfileSettings = userProfileSettingsManager.GetUserProfileSettings();
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();

            UserSettingsEditModel userSettingsEditModel = new UserSettingsEditModel(userProfileSettings, userSettings, inviteFriendSettings);

            return View(userSettingsEditModel);
        }

        /// <summary>
        /// 保存用户设置
        /// </summary>
        /// <param name="userSettingsEditModel">用户设置EditModel</param>
        /// <returns></returns>
        public JsonResult SaveUserSettings(UserSettingsEditModel userSettingsEditModel)
        {
            UserSettings userSettings = userSettingsEditModel.AsUserSettings();
            UserProfileSettings userProfileSettings = userSettingsEditModel.AsUserProfileSettings();
            InviteFriendSettings inviteFriendSettings = userSettingsEditModel.AsInviteFriendSettings();

            userSettingsManager.Save(userSettings);
            userProfileSettingsManager.SaveUserProfileSettings(userProfileSettings);
            inviteFriendSettingsManager.Save(inviteFriendSettings);

            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }

        /// <summary>
        /// 附件设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AttachmentSettings()
        {
            pageResourceManager.InsertTitlePart("附件设置");
            AttachmentSettings attachmentSettings = attachmentSettingsManager.Get();
            AttachmentSettingsEditModel attachmentSettingsEditModel = new AttachmentSettingsEditModel(attachmentSettings);
            return View(attachmentSettingsEditModel);
        }

        /// <summary>
        /// 保存附件设置
        /// </summary>
        /// <param name="attachmentSettingsEditModel">附件设置</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveAttachmentSettings(AttachmentSettingsEditModel attachmentSettingsEditModel)
        {
            AttachmentSettings attachmentSettings = attachmentSettingsEditModel.AsAttachmentSettings();
            attachmentSettingsManager.Save(attachmentSettings);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }

        /// <summary>
        /// 提醒设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReminderSettings()
        {
            pageResourceManager.InsertTitlePart("提醒设置");
            ReminderSettings reminderSettings = reminderSettingsManager.Get();
            ViewData["roles"] = roleService.GetRoles(true, null, true);
            return View(reminderSettings.AsEditModel());
        }

        /// <summary>
        /// 保存提醒设置
        /// </summary>
        /// <param name="reminderSettingsEditModel">提醒设置</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveReminderSettings(ReminderSettingsEditModel reminderSettingsEditModel)
        {
            reminderSettingsEditModel.ReminderModeSettings = reminderSettingsEditModel.ReminderModeSettings.Select(n =>
            {
                n.ReminderInfoTypeSettingses = n.ReminderInfoTypeSettingses.Select(s =>
                {
                    s.RepeatInterval *= 60;
                    return s;
                }).ToList();

                return n;

            }).ToList();
            ReminderSettings reminderSettings = reminderSettingsEditModel.AsReminderSettings();
            reminderSettingsManager.Save(reminderSettings);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }

        /// <summary>
        /// 暂停站点设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PauseSiteSettings()
        {
            pageResourceManager.InsertTitlePart("暂停站点");
            PauseSiteSettings pauseSiteSettings = pauseSiteSettingsManager.get();
            //pauseSiteSettings.PauseLink = "http://" + pauseSiteSettings.PauseLink;
            PauseSiteSettingsEditModel pauseSiteSettingsEditModel = new PauseSiteSettingsEditModel(pauseSiteSettings);
            return View(pauseSiteSettingsEditModel);
        }

        /// <summary>
        /// 保存暂停站点的设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SavePauseSiteSettings(PauseSiteSettingsEditModel pauseSiteSettingsEditModel)
        {

            PauseSiteSettings pauseSiteSettings = pauseSiteSettingsEditModel.AsPauseSiteSettings();
            pauseSiteSettingsManager.Save(pauseSiteSettings);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功"));
        }

        #endregion

        #region 皮肤管理
        /// <summary>
        /// 皮肤管理
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns></returns>
        public ActionResult ManageThemes(string presentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace)
        {
            pageResourceManager.InsertTitlePart("皮肤管理");
            IEnumerable<ThemeAppearance> appearances = themeService.GetThemeAppearances(presentAreaKey, null);
            PresentArea presentArea = presentAreaService.Get(presentAreaKey);
            ViewData["presentArea"] = presentArea;
            return View(appearances);
        }

        /// <summary>
        /// 导入皮肤页面
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <returns></returns>
        public ActionResult _ExtractTheme(string presentAreaKey)
        {
            return View();
        }

        /// <summary>
        /// 导入皮肤
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ExtractThemePost(string presentAreaKey)
        {

            HttpPostedFileBase themeFile = Request.Files["themeFile"];
            if (themeFile == null || themeFile.ContentLength == 0)
            {
                return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Error, "您没有选择文件！")));
            }
            if (themeFile.FileName.IndexOf(".zip") == -1)
            {
                return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Error, "您需要选择.zip格式的文件")));
            }
            try
            {
                themeService.ExtractThemeAppearance(presentAreaKey, themeFile.FileName, themeFile.InputStream);
            }
            catch (Exception e)
            {
                return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Error, e.Message)));
            }
            return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Success, "导入成功！")));
        }

        /// <summary>
        /// 更改皮肤顺序
        /// </summary>
        /// <param name="appearanceId">待调整的</param>
        /// <param name="referenceAppearanceId">参照Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ChangeThemeOrder(string appearanceId, string referenceAppearanceId)
        {
            themeService.ChangeDisplayOrder(appearanceId, referenceAppearanceId);
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }

        /// <summary>
        ///  设置默认皮肤
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="themeKey"></param>
        /// <param name="appearanceKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SetDefaultThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey)
        {
            PresentArea presentArea = presentAreaService.Get(presentAreaKey);
            presentArea.DefaultAppearanceID = string.Join(",", presentAreaKey, themeKey, appearanceKey);
            presentAreaService.Update(presentArea);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }

        /// <summary>
        /// 启用禁用皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeKey">themeKey</param>
        /// <param name="appearanceKey">appearanceKey</param>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EnableThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey, bool isEnabled)
        {
            themeService.SetIsEnabled(presentAreaKey, themeKey + "," + appearanceKey, isEnabled);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        /// <summary>
        /// 锁定、解锁皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeKey">themeKey</param>
        /// <param name="appearanceKey">appearanceKey</param>
        /// <param name="isLocked">是否锁定</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _LockThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey, bool isLocked)
        {
            PresentArea presentArea = presentAreaService.Get(presentAreaKey);
            presentArea.EnableThemes = !isLocked;
            presentAreaService.Update(presentArea);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        /// <summary>
        /// 删除皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeKey"></param>
        /// <param name="appearanceKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey)
        {
            themeService.DeleteThemeAppearance(presentAreaKey, themeKey + "," + appearanceKey);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        #endregion

        #region 积分

        /// <summary>
        /// 积分规则
        /// </summary>
        [HttpGet]
        public ActionResult ManagePointItems()
        {
            IPointSettingsManager pointSettingsManger = DIContainer.Resolve<IPointSettingsManager>();
            PointSettings pointSettings = pointSettingsManger.Get();
            ApplicationService applicationService = new ApplicationService();

            pageResourceManager.InsertTitlePart("积分规则");
            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["traPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("TradePoints")).CategoryName;
            ViewData["expPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ExperiencePoints")).CategoryName;
            ViewData["prePoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;

            ViewData["TransactionTax"] = pointSettings.TransactionTax;
            ViewData["PointCategories"] = pointService.GetPointCategories();
            

            ViewData["UserIntegratedPointRuleText"] = pointSettings.UserIntegratedPointRuleText;
            
            
            IEnumerable<PointItem> pointItems = pointService.GetPointItems(null);
            Dictionary<int, string> applicationDictionary = new Dictionary<int, string> { { 0, "通用" } };

            ViewData["Applications"] = applicationDictionary.Union(applicationService.GetAll().ToDictionary(m => m.ApplicationId, n => n.Config.ApplicationName));

            return View(pointItems);
        }

        /// <summary>
        /// 修改积分设置
        /// </summary>
        [HttpPost]
        public ActionResult EditPointSettings()
        {

            string pointName;
            string unit;
            int quotaPerDay;
            string description;

            IPointSettingsManager pointSettingsManger = DIContainer.Resolve<IPointSettingsManager>();
            PointSettings pointSettings = new PointSettings();
            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            pointSettings.ExperiencePointsCoefficient = Request.Form.Get<float>("Experience", 1);
            pointSettings.ReputationPointsCoefficient = Request.Form.Get<float>("Reputation", 2);

            int tax = Request.Form.GetInt("Tax", 0);
            if (tax > 100 || tax < 0)
                tax = 0;
            pointSettings.TransactionTax = tax;
            pointSettingsManger.Save(pointSettings);

            
            

            foreach (var pointCategory in pointCategories)
            {
                pointName = Request.Form[pointCategory.CategoryKey + "PointName"];
                unit = Request.Form[pointCategory.CategoryKey + "Unit"];
                quotaPerDay = Request.Form.GetInt(pointCategory.CategoryKey + "QuotaPerDay", 0);
                description = Request.Form[pointCategory.CategoryKey + "Description"];



                PointCategory newPointCatefory = pointService.GetPointCategory(pointCategory.CategoryKey);

                if (newPointCatefory != null)
                {
                    newPointCatefory.CategoryName = pointName;
                    newPointCatefory.QuotaPerDay = quotaPerDay;
                    newPointCatefory.Unit = unit;
                }

                pointService.UpdatePointCategory(newPointCatefory);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
        }

        /// <summary>
        /// 修改积分设置控件
        /// </summary>
        [HttpGet]
        public ActionResult _EditPointSettings()
        {
            IPointSettingsManager pointSettingsManger = DIContainer.Resolve<IPointSettingsManager>();
            PointSettings pointSettings = pointSettingsManger.Get();

            ViewData["ExperiencePoints"] = pointService.GetPointCategory("ExperiencePoints").CategoryName;
            ViewData["ReputationPoints"] = pointService.GetPointCategory("ReputationPoints").CategoryName;
            ViewData["Tax"] = pointSettings.TransactionTax.ToString();

            ViewData["ExperienceCoefficient"] = pointSettings.ExperiencePointsCoefficient;
            ViewData["ReputationCoefficient"] = pointSettings.ReputationPointsCoefficient;
            ViewData["PointCategories"] = pointService.GetPointCategories();

            return View();

        }

        
        
        /// <summary>
        /// 修改积分规则
        /// </summary>
        [HttpPost]
        public ActionResult _EditPointRules(PointItemEditModel model)
        {

            PointItem pointItem = model.AsPointItem();
            pointService.UpdatePointItem(pointItem);

            return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
        }

        /// <summary>
        /// 修改积分规则控件
        /// </summary>
        [HttpGet]
        public ActionResult _EditPointRules(string itemKey)
        {
            PointItemEditModel editModel = new PointItemEditModel();
            PointItem pointItem = pointService.GetPointItem(itemKey);
            if (pointItem != null)
                editModel = pointItem.AsEditModel();


            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["traPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("TradePoints")).CategoryName;
            ViewData["expPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ExperiencePoints")).CategoryName;
            ViewData["prePoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;

            return View(editModel);
        }

        #endregion

        #region 审核设置

        /// <summary>
        /// 审核设置
        /// </summary>
        [HttpGet]
        public ActionResult ManageAuditItems()
        {
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();
            pageResourceManager.InsertTitlePart("审核规则");

            //用来装应用下的角色
            Dictionary<int, IEnumerable<Role>> roles = new Dictionary<int, IEnumerable<Role>>();
            //用来装应用下的审核项
            Dictionary<int, IEnumerable<AuditItem>> auditItems = new Dictionary<int, IEnumerable<AuditItem>>();
            //用来装应用下的对外显示状态
            Dictionary<int, PubliclyAuditStatus> applicationStatus = new Dictionary<int, PubliclyAuditStatus>();
            Dictionary<int, Dictionary<string, IEnumerable<AuditItemInUserRole>>> appForAuditItemInUserRole = new Dictionary<int, Dictionary<string, IEnumerable<AuditItemInUserRole>>>();
            IEnumerable<Role> roleLists = null;
            //全局角色
            IEnumerable<Role> globalRoles = roleService.GetRoles(true, 0, true);
            Role moderatedUserRole = new Role
            {
                RoleName = RoleNames.Instance().ModeratedUser(),
                Description = string.Empty,
                FriendlyRoleName = "管制用户",
                ConnectToUser = true,
                IsBuiltIn = true,
                IsEnabled = true,
                RoleImage = string.Empty
            };
            Role registeredUsersRole = new Role
            {
                RoleName = RoleNames.Instance().RegisteredUsers(),
                Description = string.Empty,
                FriendlyRoleName = "注册会员",
                ConnectToUser = true,
                IsBuiltIn = true,
                IsEnabled = true,
                RoleImage = string.Empty
            };

            IList<Role> globalRolesList = globalRoles.ToList();
            globalRolesList.Add(registeredUsersRole);
            globalRolesList.Add(moderatedUserRole);
            globalRoles = globalRolesList;

            roles[0] = globalRoles;//全局内容块下的角色
            auditItems[0] = auditService.GetAuditItems(0);//全局下的审核项
            //用来装审核项跟角色的关联
            Dictionary<string, IEnumerable<AuditItemInUserRole>> auditItemInUserRoles = new Dictionary<string, IEnumerable<AuditItemInUserRole>>();
            foreach (Role gloubRole in globalRoles)//角色的审核设置
            {
                auditService.GetAuditItemsInUserRole(gloubRole.RoleName, 0);
                auditItemInUserRoles[gloubRole.RoleName] = auditService.GetAuditItemsInUserRole(gloubRole.RoleName, 0);
            }

            appForAuditItemInUserRole[0] = auditItemInUserRoles;
            applicationStatus[0] = auditService.GetPubliclyAuditStatus(0);//全局的对外显示状态

            IEnumerable<ApplicationBase> apps = appService.GetAll();//获取所有应用
            foreach (var app in apps)
            {
                roleLists = new List<Role>();
                roleLists = roleLists.Union(globalRoles);//全局角色
                roleLists = roleLists.Union(roleService.GetRoles(true, app.ApplicationId, true));//应用下角色
                roles[app.ApplicationId] = roleLists;

                auditItems[app.ApplicationId] = auditService.GetAuditItems(app.ApplicationId);//角色下的审核项

                auditItemInUserRoles = new Dictionary<string, IEnumerable<AuditItemInUserRole>>();
                foreach (Role role in roleLists)//角色的审核设置
                {
                    auditItemInUserRoles[role.RoleName] = auditService.GetAuditItemsInUserRole(role.RoleName, app.ApplicationId);
                }

                appForAuditItemInUserRole[app.ApplicationId] = auditItemInUserRoles;
                applicationStatus[app.ApplicationId] = auditService.GetPubliclyAuditStatus(app.ApplicationId);//全局的对外显示状态
            }

            ViewData["roles"] = roles;
            ViewData["appForAuditItemInUserRole"] = appForAuditItemInUserRole;
            ViewData["auditItems"] = auditItems;
            ViewData["applicationStatus"] = applicationStatus;
            ViewData["userSettings"] = userSettingsManager.Get();
            ViewData["allRoles"] = roleService.GetRoles(true, null, true);
            return View(apps);
        }

        /// <summary>
        /// 审核设置
        /// </summary>
        [HttpPost]
        public ActionResult UpdateAuditItemsInRole()
        {
            int applicationId = Request.Form.GetInt("applicationId", 0);
            PubliclyAuditStatus status = (PubliclyAuditStatus)Request.Form.Get<int>("AuditStatus_" + applicationId);

            auditService.SavePubliclyAuditStatus(applicationId, status);

            IEnumerable<string> roleNames = Request.Form.Gets<string>("RoleName");
            IList<AuditItemInUserRole> auditItemInUserRoles = new List<AuditItemInUserRole>();

            foreach (var roleName in roleNames)
            {
                var keys = Request.Form.Keys.Cast<string>().Where(n => n.StartsWith(roleName + "-") && !n.EndsWith("-IsLock"));
                if (keys == null)
                    continue;
                foreach (var key in keys)
                {
                    bool isLock = Request.Form.GetBool(key + "-IsLock", false);
                    var value = Request.Form.Get<AuditStrictDegree>(key);
                    auditItemInUserRoles.Add(new AuditItemInUserRole() { IsLocked = isLock, ItemKey = key.Split('-')[1], RoleName = roleName, StrictDegree = value });
                }
            }

            auditService.UpdateAuditItemInUserRole(auditItemInUserRoles);

            var userSettings = userSettingsManager.Get();
            userSettings.EnableAudit = Request.Form.Get<bool>("EnableAudit", userSettings.EnableAudit);
            userSettings.NoAuditedRoleNames = Request.Form.Gets<string>("NoAuditedRoleNames", userSettings.NoAuditedRoleNames).ToList();
            userSettings.MinNoAuditedUserRank = Request.Form.Get<int>("MinNoAuditedUserRank", userSettings.MinNoAuditedUserRank);
            userSettingsManager.Save(userSettings);
            return Json(new StatusMessageData(StatusMessageType.Success, "保存成功！"));
        }

        #endregion

        #region 隐私设置
        /// <summary>
        /// 隐私设置
        /// </summary>
        /// <returns>隐私设置</returns>
        [HttpGet]
        public ActionResult ManagePrivacyItems()
        {
            pageResourceManager.InsertTitlePart("隐私规则");
            IEnumerable<PrivacyItemGroup> PrivacyItemGroups = PrivacyItemGroup.GetAll();
            Dictionary<PrivacyItemGroup, List<PrivacyItem>> privacyItems = new Dictionary<PrivacyItemGroup, List<PrivacyItem>>();
            List<SelectListItem> selectListItems = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "允许所有人", Value = PrivacyStatus.Public.ToString()},
                new SelectListItem{ Text = "允许关注的人", Value = PrivacyStatus.Part.ToString()},
                new SelectListItem{ Text = "不允许", Value = PrivacyStatus.Private.ToString()}
            };
            List<SelectListItem> followListItems = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "允许所有人", Value = PrivacyStatus.Public.ToString()},
                new SelectListItem{ Text = "不允许", Value = PrivacyStatus.Private.ToString()}
            };
            foreach (var item in PrivacyItemGroups)
            {
                IEnumerable<PrivacyItem> privacyItemFormGroup = privacyService.GetPrivacyItems(item.TypeId, null);
                foreach (var privacyItem in privacyItemFormGroup)
                {
                    if (privacyItem.ItemKey == PrivacyItemKeys.Instance().Follow())
                    {
                        ViewData[privacyItem.ItemKey] = new SelectList(followListItems, "Value", "Text", privacyItem.PrivacyStatus != PrivacyStatus.Part ? privacyItem.PrivacyStatus : PrivacyStatus.Public);
                    }
                    else
                    {
                        ViewData[privacyItem.ItemKey] = new SelectList(selectListItems, "Value", "Text", privacyItem.PrivacyStatus);
                    }
                }
                privacyItems.Add(item, privacyItemFormGroup.ToList());
            }
            return View(privacyItems);
        }

        /// <summary>
        /// 处理隐私设置
        /// </summary>
        /// <returns>隐私设置</returns>
        [HttpPost]
        public ActionResult ManagePrivacyItemsPost()
        {
            List<PrivacyItem> systemPrivacyItems = new List<PrivacyItem>();
            
            IEnumerable<PrivacyItemGroup> PrivacyItemGroups = PrivacyItemGroup.GetAll();
            foreach (var item in PrivacyItemGroups)
            {
                IEnumerable<PrivacyItem> privacyItems = privacyService.GetPrivacyItems(item.TypeId, null);
                foreach (var privacyItem in privacyItems)
                {
                    PrivacyItem adminPrivacyItem = new PrivacyItem
                    {
                        ApplicationId = privacyItem.ApplicationId,
                        Description = privacyItem.Description,
                        ItemKey = privacyItem.ItemKey,
                        ItemName = privacyItem.ItemName,
                        DisplayOrder = privacyItem.DisplayOrder,
                        ItemGroupId = privacyItem.ItemGroupId,
                        PrivacyStatus = Request.Form.Get<PrivacyStatus>(privacyItem.ItemKey, PrivacyStatus.Part)
                    };
                    systemPrivacyItems.Add(adminPrivacyItem);
                }
            }
            privacyService.UpdatePrivacyItems(systemPrivacyItems);
            TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "更新隐私规则成功");
            return Redirect(SiteUrls.Instance().ManagePrivacyItems());
        }

        #endregion

        #region 权限设置

        /// <summary>
        /// 查看权限设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManagePermissionItems()
        {
            //应用下的角色名
            IEnumerable<Role> rolesApplication = null;

            //所有的角色名(包括全局角色名)
            IEnumerable<Role> rolesAll = null;

            //应用的权限项
            IEnumerable<PermissionItem> permissionItems = null;

            //存储角色名称获取PermissionItemInUserRole表里对应的记录
            IEnumerable<PermissionItemInUserRole> permissionItemInUserRoles = null;

            //查出所有应用
            IEnumerable<ApplicationBase> applicationBases = appService.GetAll(true);

            //根据应用ID获取全局角色名(ApplicationId=0的)
            IEnumerable<Role> globalRoles = roleService.GetRoles(true, 0, true);


            Role moderatedUserRole = new Role
            {
                RoleName = RoleNames.Instance().ModeratedUser(),
                Description = string.Empty,
                FriendlyRoleName = "管制用户",
                ConnectToUser = true,
                IsBuiltIn = true,
                IsEnabled = true,
                RoleImage = string.Empty
            };
            Role registeredUsersRole = new Role
            {
                RoleName = RoleNames.Instance().RegisteredUsers(),
                Description = string.Empty,
                FriendlyRoleName = "注册会员",
                ConnectToUser = true,
                IsBuiltIn = true,
                IsEnabled = true,
                RoleImage = string.Empty
            };
            IList<Role> globalRolesList = globalRoles.ToList();
            globalRolesList.Add(moderatedUserRole);
            globalRolesList.Add(registeredUsersRole);
            globalRoles = globalRolesList;

            //根据应用ID获取每个应用下的权限角色和应用的权限项
            foreach (var applicationBase in applicationBases)
            {
                //根据应用ID获取出该应用下的角色名
                rolesApplication = roleService.GetRoles(true, applicationBase.ApplicationId, true);
                rolesAll = globalRoles.Union(rolesApplication);
                ViewData["role_" + applicationBase.ApplicationKey] = rolesAll;

                //根据应用ID获取出该应用的权限项
                permissionItems = permissionService.GetPermissionItems(applicationBase.ApplicationId);
                ViewData["permissionItem_" + applicationBase.ApplicationKey] = permissionItems;
            }

            //获取以角色名跟权限key为键，一条记录为值的字典集合
            Dictionary<string, PermissionItemInUserRole> dicPermissionItemInUserRoles = new Dictionary<string, PermissionItemInUserRole>();
            var allApplicationRoles = roleService.GetRoles(true, null, true);
            List<Role> allApplicationRolesList = allApplicationRoles.ToList();
            allApplicationRolesList.RemoveAll(n => n.RoleName == RoleNames.Instance().SuperAdministrator() || n.RoleName == RoleNames.Instance().ContentAdministrator());
            allApplicationRolesList.Add(registeredUsersRole);
            allApplicationRolesList.Add(moderatedUserRole);
            foreach (var role in allApplicationRolesList)
            {
                permissionItemInUserRoles = permissionService.GetPermissionItemsInUserRole(role.RoleName);
                foreach (var permissionItemInUserRole in permissionItemInUserRoles)
                {
                    string key = permissionItemInUserRole.RoleName + "_" + permissionItemInUserRole.ItemKey;
                    dicPermissionItemInUserRoles[key] = permissionItemInUserRole;
                }
            }
            ViewData["PermissionItemsInUserRoles"] = dicPermissionItemInUserRoles;

            pageResourceManager.InsertTitlePart("权限设置");
            return View(applicationBases);
        }

        /// <summary>
        /// 编辑权限设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManagePermissionItemsInUserRoles(string roleName)
        {

            //应用的权限项
            IEnumerable<PermissionItem> permissionItems = null;

            IEnumerable<PermissionItemInUserRole> permissionItemInUserRoles = null;

            //查出所有应用
            IEnumerable<ApplicationBase> applicationBases = appService.GetAll(true);

            //根据应用ID获取出该应用的权限项
            foreach (var applicationBase in applicationBases)
            {
                permissionItems = permissionService.GetPermissionItems(applicationBase.ApplicationId);
                ViewData["permissionItem_" + applicationBase.ApplicationKey] = permissionItems;
            }
            //获取以权限key为键，一条记录为值的字典集合
            Dictionary<string, PermissionItemInUserRole> dicPermissionItemInUserRoles = new Dictionary<string, PermissionItemInUserRole>();

            //根据角色名称获取权限与角色对应表的相应记录
            permissionItemInUserRoles = permissionService.GetPermissionItemsInUserRole(roleName);

            foreach (var permissionItemInUserRole in permissionItemInUserRoles)
            {
                string key = permissionItemInUserRole.ItemKey;
                dicPermissionItemInUserRoles[key] = permissionItemInUserRole;
            }
            ViewData["PermissionItemsInUserRoles"] = dicPermissionItemInUserRoles;
            string friendlyRoleName = string.Empty;
            var role = new RoleService().Get(roleName);
            if (role != null)
                friendlyRoleName = role.FriendlyRoleName;
            else if (roleName == RoleNames.Instance().RegisteredUsers())
                friendlyRoleName = "注册会员";
            else if (roleName == RoleNames.Instance().ModeratedUser())
                friendlyRoleName = "管制用户";
            ViewData["FriendlyRoleName"] = friendlyRoleName;
            pageResourceManager.InsertTitlePart("权限设置");
            return View(applicationBases);
        }

        /// <summary>
        /// 编辑权限设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManagePermissionItemsInUserRoles(IEnumerable<PermissionItemInUserRole> PermissionItemInUserRoles)
        {
            permissionService.UpdatePermissionItemInUserRole(PermissionItemInUserRoles);
            return RedirectToAction("ManagePermissionItems");
        }

        #endregion

        #region 站点导航设置

        
        
        #region 导航设置
        /// <summary>
        /// 管理导航
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageNavigations(string presentAreaKey = PresentAreaKeysOfBuiltIn.Channel)
        {
            pageResourceManager.InsertTitlePart("导航管理");

            IEnumerable<InitialNavigation> initialNavigations = navigationService.GetRootInitialNavigation(presentAreaKey);
            return View(initialNavigations);
        }

        /// <summary>
        /// 设置导航状态
        /// </summary>
        [HttpPost]
        public JsonResult setNavigationStatus(int navigationId, bool isEnabled)
        {
            InitialNavigation navigation = navigationService.GetInitialNavigation(navigationId);
            if (navigation == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到导航！"));
            navigation.IsEnabled = isEnabled;
            navigationService.UpdateInitialNavigation(navigation);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }

        /// <summary>
        /// 更改导航显示顺序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeNavigationOrder(int fromNavigationId, int toNavigationId)
        {
            InitialNavigation fromNavigation = navigationService.GetInitialNavigation(fromNavigationId);
            if (fromNavigation == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到导航！"));

            InitialNavigation toNavigation = navigationService.GetInitialNavigation(toNavigationId);
            if (toNavigation == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到导航！"));

            
            
            int temp = fromNavigation.DisplayOrder;

            fromNavigation.DisplayOrder = toNavigation.DisplayOrder;
            navigationService.UpdateInitialNavigation(fromNavigation);

            toNavigation.DisplayOrder = temp;
            navigationService.UpdateInitialNavigation(toNavigation);

            
            
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建导航
        /// </summary>
        [HttpGet]
        public ActionResult _CreateNavigation(int parentNavigationId, int? applicationId, string presentAreaKey = PresentAreaKeysOfBuiltIn.Channel)
        {
            IEnumerable<ApplicationBase> applications = appService.GetAll().Where(n => appService.IsAvailable(presentAreaKey, n.ApplicationId));
            
            
            if (applicationId == null)
            {
                ViewData["ApplicationId"] = new SelectList(applications.Select(n => new { text = n.Config.ApplicationName, value = n.ApplicationId }), "value", "text");
            }
            else
            {
                ViewData["ApplicationId"] = applicationId;
                if (applicationId != 0)
                {
                    ViewData["ApplicationName"] = appService.Get((int)applicationId).Config.ApplicationName;
                }
            }

            return View();
        }

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreateNavigation(NavigationCreateModel navigationCreateModel)
        {
            
            bool forceOwnerCreate = Request.Form.Get<bool>("ForceOwnerCreate", false);
            string presentAreaKey = Request.QueryString.Get<string>("presentAreaKey", string.Empty);
            int parentNavigationId = Request.QueryString.Get<int>("parentNavigationId", 0);
            int depth = 0;
            
            
            if (parentNavigationId > 0)
            {
                depth = navigationService.GetInitialNavigation(parentNavigationId).Depth + 1;
            }
            InitialNavigation initialNavigation = navigationCreateModel.AsInitialNavigation();
            initialNavigation.PresentAreaKey = presentAreaKey;
            initialNavigation.ParentNavigationId = parentNavigationId;
            initialNavigation.Depth = depth;

            long isCreated = navigationService.CreateInitialNavigation(initialNavigation, forceOwnerCreate);
            if (isCreated == 0)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "创建失败"));
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "创建成功"));
        }

        /// <summary>
        /// 验证导航Id的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateNavigationId(int navigationId)
        {
            InitialNavigation navigation = navigationService.GetInitialNavigation(navigationId);
            return Json(navigation == null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑导航管理模式框
        /// </summary>
        /// <param name="navigationId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditNavigation(int navigationId)
        {
            InitialNavigation navigation = navigationService.GetInitialNavigation(navigationId);
            NavigationEditModel navigationEditModel = new NavigationEditModel();
            if (navigation != null)
            {
                navigationEditModel = navigation.AsNavigationEditModel();
                
                
                if (navigation.ApplicationId > 0)
                {
                    
                    
                    ApplicationBase application = appService.Get(navigation.ApplicationId);
                    if (application != null)
                    {
                        ViewData["applicationName"] = application.Config.ApplicationName;
                    }
                }
            }

            return View(navigationEditModel);
        }

        /// <summary>
        /// 编辑导航管理
        /// </summary>
        /// <param name="navigationEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditNavigation(NavigationEditModel navigationEditModel)
        {
            
            
            InitialNavigation navigation = navigationEditModel.AsInitialNavigation();

            bool forceOwnerUpdate = Request.Form.Get<bool>("ForceOwnerUpdate");
            navigationService.UpdateInitialNavigation(navigation, forceOwnerUpdate);

            
            
            return Json(new StatusMessageData(StatusMessageType.Success, "更新成功"));
        }

        /// <summary>
        /// 删除导航模式框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _DeleteNavigation(int? navigationId)
        {
            
            
            var initialNavigation = navigationService.GetInitialNavigation(navigationId.Value);
            ViewData["navigationId"] = navigationId ?? 0;
            return View(initialNavigation);
        }

        /// <summary>
        /// 删除导航
        /// </summary>
        /// <param name="navigationId"></param>
        /// <param name="forceOwnerDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteNavigation(int navigationId, bool? forceOwnerDelete)
        {
            navigationService.DeleteInitialNavigation(navigationId, forceOwnerDelete ?? false);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        /// <summary>
        /// 批量移除导航模式框
        /// </summary>
        /// <param name="navigationIds"></param>
        /// <param name="forceOwnerDelete"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _BatchRemoveNavigation()
        {
            return View();
        }

        /// <summary>
        /// 批量移除导航
        /// </summary>
        [HttpPost]
        public ActionResult _BatchRemoveNavigation(string navigationIds, bool? forceOwnerDelete)
        {

            string[] str_navIds = navigationIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (str_navIds != null)
            {
                foreach (var navigationId in str_navIds.Select(n => Convert.ToInt32(n)))
                {
                    navigationService.DeleteInitialNavigation(navigationId, forceOwnerDelete ?? false);
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }
        #endregion

        #region 子导航
        /// <summary>
        /// 管理子导航
        /// </summary>
        /// <param name="navigationId"></param>
        /// <returns></returns>
        public ActionResult ManageChildNavigations(int? navigationId)
        {
            
            
            pageResourceManager.InsertTitlePart("导航管理");

            InitialNavigation navigation = navigationService.GetInitialNavigation(navigationId ?? 0);

            IEnumerable<InitialNavigation> navigations = navigationService.GetInitialNavigations(navigation.PresentAreaKey).Where(n => n.ParentNavigationId == navigationId);
            ViewData["applicationId"] = navigation.ApplicationId;
            ViewData["parentNavigationId"] = navigationId;
            ViewData["presentAreaKey"] = navigation.PresentAreaKey;
            
            if (!string.IsNullOrEmpty(navigation.NavigationText))
            {
                ViewData["navigationName"] = navigation.NavigationText;
            }
            else
            {
                ViewData["navigationName"] = Tunynet.Globalization.ResourceAccessor.GetString(navigation.ResourceName);
            }

            return View(navigations);
        }
        #endregion

        
        //已修改

        #region 快捷导航
        /// <summary>
        /// 管理快捷导航
        /// </summary>
        public ActionResult ManageQuickOperations(string presentAreaKey = PresentAreaKeysOfBuiltIn.Channel, ManagementOperationType operationType = ManagementOperationType.Shortcut)
        {
            pageResourceManager.InsertTitlePart("快捷操作管理");
            
            
            Dictionary<string, IEnumerable<ApplicationManagementOperation>> items = new Dictionary<string, IEnumerable<ApplicationManagementOperation>>();

            if (operationType == ManagementOperationType.Shortcut)
            {
                IEnumerable<ApplicationManagementOperation> shortCuts = operationService.GetShortcuts(presentAreaKey, false);

                
                

                //取到应用Id并去重
                List<int> appId = new List<int>();

                appId = shortCuts.Select(n => n.ApplicationId).Distinct().ToList();

                foreach (int applicationId in appId)
                {
                    List<ApplicationManagementOperation> currentshortCuts = shortCuts.Where(n => n.ApplicationId == applicationId).ToList();
                    if (applicationId != 0)
                    {
                        items.Add(appService.Get(applicationId).Config.ApplicationName, currentshortCuts);
                    }
                    else
                    {
                        items.Add("未设置应用", currentshortCuts);
                    }
                }
            }
            else
            {
                IEnumerable<ApplicationManagementOperation> managementMenus = operationService.GetManagementMenus(presentAreaKey, false);

                //取到应用Id并去重
                List<int> appId = new List<int>();

                appId = managementMenus.Select(n => n.ApplicationId).Distinct().ToList();

                foreach (int applicationId in appId)
                {
                    List<ApplicationManagementOperation> currentManagementMenus = managementMenus.Where(n => n.ApplicationId == applicationId).ToList();
                    if (applicationId != 0)
                    {
                        items.Add(appService.Get(applicationId).Config.ApplicationName, currentManagementMenus);
                    }
                    else
                    {
                        items.Add("未设置应用", currentManagementMenus);
                    }
                }
            }
            return View(items);
        }

        /// <summary>
        /// 更改快捷操作显示顺序
        /// </summary>
        /// <returns></returns>
        
        //1、为什么没加httppost请求 
        [HttpPost]
        public JsonResult ChangeOperationOrder(int fromOperationId, int toOperationId)
        {
            ApplicationManagementOperation fromOperation = operationService.Get(fromOperationId);
            if (fromOperation == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到导航！"), JsonRequestBehavior.AllowGet);
            ApplicationManagementOperation toOperation = operationService.Get(toOperationId);
            if (toOperation == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到导航！"), JsonRequestBehavior.AllowGet);
            
            
            int temp = fromOperation.DisplayOrder;

            
            
            fromOperation.DisplayOrder = toOperation.DisplayOrder;
            operationService.Update(fromOperation);

            toOperation.DisplayOrder = temp;
            operationService.Update(toOperation);

            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除快捷操作
        /// </summary>
        [HttpPost]
        public ActionResult DeleteOperation(int operationId)
        {
            
            
            operationService.Delete(operationId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        /// <summary>
        /// 批量移除快捷操作
        /// </summary>
        [HttpPost]
        public ActionResult BatchRemoveOperation(List<int> operationIds)
        {
            
            
            foreach (int operationId in operationIds)
            {
                operationService.Delete(operationId);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        /// <summary>
        /// 设置快捷操作状态
        /// </summary>
        
        
        [HttpPost]
        public JsonResult setOperationStatus(int operationId, bool isEnabled)
        {
            ApplicationManagementOperation operation = operationService.Get(operationId);
            if (operation == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到快捷操作！"));
            
            
            operation.IsEnabled = isEnabled;
            operationService.Update(operation);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }

        /// <summary>
        /// 设置是否新开状态
        /// </summary>
        [HttpPost]
        public JsonResult setOperationTarget(int operationId, bool isTarget)
        {
            ApplicationManagementOperation operation = operationService.Get(operationId);
            if (operation == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到快捷操作！"));
            if (isTarget)
            {
                operation.NavigationTarget = "_blank";
            }
            else
            {
                operation.NavigationTarget = "_self";
            }
            
            
            operationService.Update(operation);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }

        /// <summary>
        /// 验证操作Id的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateOperationId(int? operationId)
        {
            
            
            ApplicationManagementOperation operation = operationService.Get(operationId ?? 0);
            return Json(operation == null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建快捷操作模式框
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <returns></returns>
        
        [HttpGet]
        public ActionResult _CreateOperation(ManagementOperationType operationType, string presentAreaKey = PresentAreaKeysOfBuiltIn.Channel)
        {
            IEnumerable<ApplicationBase> applications = appService.GetAll().Where(n => appService.IsAvailable(presentAreaKey, n.ApplicationId));

            ViewData["ApplicationId"] = new SelectList(applications.Select(n => new { text = n.Config.ApplicationName, value = n.ApplicationId }), "value", "text");

            return View();
        }

        /// <summary>
        /// 创建快捷操作
        /// </summary>
        /// <param name="operationEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreateOperation(OperationEditModel operationEditModel)
        {
            
            string presentAreaKey = Request.QueryString.Get<string>("presentAreaKey", string.Empty);
            ManagementOperationType? operationType = Request.QueryString.Get<ManagementOperationType?>("operationType", null);
            ApplicationManagementOperation operation = operationEditModel.AsApplicationManagementOperation();
            operation.PresentAreaKey = presentAreaKey;
            operation.OperationType = (ManagementOperationType)operationType;
            
            
            operationService.Create(operation);

            return Json(new StatusMessageData(StatusMessageType.Success, "创建成功"));
        }

        /// <summary>
        /// 编辑快捷操作管理模式框
        /// </summary>
        /// <param name="operationId"></param>
        /// <returns></returns>
        
        
        [HttpGet]
        public ActionResult _EditOperation(int? operationId)
        {
            ApplicationManagementOperation operation = operationService.Get(operationId ?? 0);

            OperationEditModel operationEditModel = new OperationEditModel();
            if (operation != null)
            {
                operationEditModel = operation.AsOperationEditModel();
                if (operation.ApplicationId != 0)
                {
                    ViewData["applicationName"] = appService.Get(operation.ApplicationId).Config.ApplicationName;
                }
            }
            ViewData["operationType"] = operation.OperationType;
            return View(operationEditModel);
        }

        /// <summary>
        /// 编辑快捷操作管理
        /// </summary>
        /// <param name="operationEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditOperation(OperationEditModel operationEditModel)
        {
            ApplicationManagementOperation operation = operationEditModel.AsApplicationManagementOperation();
            
            
            operationService.Update(operation);
            return Json(new StatusMessageData(StatusMessageType.Success, "更新成功"));
        }
        #endregion

        #endregion

        #region 后台应用管理
        /// <summary>
        /// 后台应用管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageApplications()
        {
            pageResourceManager.InsertTitlePart("应用管理");
            IEnumerable<ApplicationBase> applications = new ApplicationService().GetAll();
            return View(applications);
        }
        /// <summary>
        /// 后台应用管理禁用按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetApplicationStatus(int applicationId)
        {
            bool isEnabled = Request.Form.Get<bool>("isEnabled", false);
            var applicationService = new ApplicationService();
            var application = applicationService.Get(applicationId);
            if (application == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到应用"));
            new ApplicationService().SetStatus(applicationId, isEnabled);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }
        #endregion 后台应用管理

        #region 表情管理

        /// <summary>
        /// 表情管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageEmotionCategories()
        {
            pageResourceManager.InsertTitlePart("表情管理");
            var emotions = emotionService.GetEmotionCategories();
            return View(emotions);
        }

        /// <summary>
        /// 上传表情包
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExtractEmoticon()
        {

            var emotionName = Request.Files["emotionName"];
            if (emotionName.ContentLength == 0)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "您没有选择文件");
                return RedirectToAction("ManageEmotionCategories");
            }
            if (emotionName.FileName.IndexOf(".zip") == -1)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "您需要选择.zip格式的文件");
                return RedirectToAction("ManageEmotionCategories");
            }
            var judge = emotionService.GetEmotionCategory(emotionName.FileName.Remove(emotionName.FileName.IndexOf(".zip")));
            if (judge != null)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "已存在同名文件");
                return RedirectToAction("ManageEmotionCategories");
            }
            try
            {
                emotionService.ExtractEmoticon(emotionName.FileName, emotionName.InputStream);
            }
            catch (Exception e)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, e.Message);
                return RedirectToAction("ManageEmotionCategories");
            }
            TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "表情上传成功");

            return RedirectToAction("ManageEmotionCategories");
        }

        /// <summary>
        /// 更新表情包启用状态
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetEmotionCategoryStatus(string directoryName)
        {
            bool isEnabled = Request.Form.Get<bool>("isEnabled", false);
            var emotionCategory = emotionService.GetEmotionCategory(directoryName);
            if (emotionCategory == null)
                return HttpNotFound();
            if (emotionCategory.IsEnabled == isEnabled)
                return Json(new StatusMessageData(StatusMessageType.Hint, "您没有修改表情包的启用状态"));
            emotionCategory.IsEnabled = isEnabled;
            emotionService.UpdateEmotionCategory(emotionCategory);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        /// <summary>
        /// 删除表情包
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEmotionCategory(string directoryName)
        {
            var emotionCategory = emotionService.GetEmotionCategory(directoryName);
            if (emotionCategory == null)
                return HttpNotFound();
            emotionService.DeleteEmoticonCategory(directoryName);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        /// <summary>
        /// 交换表情包排列顺序
        /// </summary>
        [HttpPost]
        public JsonResult ChangeEmotionDisplayOrder()
        {
            string id = Request.Form.Get<string>("id", string.Empty);
            string referenceId = Request.Form.Get<string>("referenceId", string.Empty);
            var category = emotionService.GetEmotionCategory(id);
            var referenceCategory = emotionService.GetEmotionCategory(referenceId);
            if (category == null || referenceCategory == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到表情包"));
            int displayOrder = category.DisplayOrder;
            category.DisplayOrder = referenceCategory.DisplayOrder;
            referenceCategory.DisplayOrder = displayOrder;
            emotionService.UpdateEmotionCategory(category);
            emotionService.UpdateEmotionCategory(referenceCategory);
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }

        /// <summary>
        /// 浏览表情包下的表情
        /// </summary>
        /// <param name="directoryName">表情包目录名</param>
        /// <returns></returns>
        public ViewResult _ListEmotions(string directoryName)
        {
            return View(emotionService.GetEmotionCategory(directoryName));
        }

        #endregion

        #region 敏感词管理


        /// <summary>
        /// 获取敏感词
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageSensitiveWords(string keyword = null, int? typeId = null)
        {
            pageResourceManager.InsertTitlePart("敏感词管理");

            IEnumerable<SensitiveWord> sensitiveWords = sensitiveWordService.Gets(keyword, typeId);

            IEnumerable<SensitiveWordType> sensitiveWordTypes = sensitiveWordService.GetAllSensitiveWordTypes();

            ViewData["sensitiveWordTypes"] = sensitiveWordTypes;

            return View(sensitiveWords);
        }

        /// <summary>
        /// 批量添加敏感词、编辑敏感词
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _AddSensitiveWord(int? id = null)
        {
            if (id == null)
            {
                SensitiveWordEditModel model = new SensitiveWordEditModel();
                IEnumerable<SensitiveWordType> sensitiveWordTypes = sensitiveWordService.GetAllSensitiveWordTypes();
                ViewData["sensitiveWordTypes"] = sensitiveWordTypes;
                return View(model);
            }
            else
            {
                var sensitiveWord = sensitiveWordService.Get(id.Value);
                SensitiveWordEditModel model = sensitiveWord.AsEditModel();
                IEnumerable<SensitiveWordType> sensitiveWordTypes = sensitiveWordService.GetAllSensitiveWordTypes();
                ViewData["sensitiveWordTypes"] = sensitiveWordTypes;
                return View(model);
            }
        }

        /// <summary>
        /// 批量添加敏感词、编辑敏感词
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _AddSensitiveWord(SensitiveWordEditModel model, WordFilterStatus select)
        {
            if (select.EnumMetadataDisplay() == WordFilterStatus.Banned.EnumMetadataDisplay())
            {
                model.Replacement = "{Banned}";
            }
            if (model.Id == 0 || model.Id == null)
            {
                string[] words = model.Word.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    model.Word = word;

                    int judge = sensitiveWordService.Create(model.AsSensitiveWord());

                    if (judge == -1)
                        return Json(new StatusMessageData(StatusMessageType.Error, "有重名的敏感词！"));
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "添加成功！"));
            }
            else
            {
                int judge = sensitiveWordService.Update(model.AsSensitiveWord());

                if (judge == -1)
                    return Json(new StatusMessageData(StatusMessageType.Error, "敏感词不能同名，编辑失败！"));

                return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
            }
        }

        /// <summary>
        /// 导出敏感词
        /// </summary>
        /// <returns></returns>
        public EmptyResult _OutputSensitiveWords()
        {
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Accept-Ranges", "bytes");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("words.txt", System.Text.Encoding.Default));
            Response.BinaryWrite(sensitiveWordService.Export());
            Response.End();

            return new EmptyResult();
        }

        /// <summary>
        /// 导入敏感词
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _InputSensitiveWords()
        {
            IEnumerable<SensitiveWordType> sensitiveWordTypes = sensitiveWordService.GetAllSensitiveWordTypes();
            ViewData["sensitiveWordTypes"] = sensitiveWordTypes;
            return View();
        }

        /// <summary>
        /// 导入敏感词
        /// </summary>
        /// <param name="typeId">敏感词类别ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _InputSensitiveWords(int? typeId = null)
        {
            var sensitiveWords = Request.Files["sensitiveWords"];

            if (sensitiveWords.FileName.IndexOf(".txt") == -1)
            {
                return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Error, "您需要选择.txt格式的文件")));

            }
            try
            {
                sensitiveWordService.BatchCreate(sensitiveWords.InputStream, typeId);
            }
            catch (Exception e)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, e.Message));
            }
            return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Success, "添加成功！")));
        }


        /// <summary>
        /// 批量删除敏感词
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult DeleteSensitiveWords(List<int> ids)
        {
            if (ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    sensitiveWordService.Delete(id);
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"));
            }

        }


        /// <summary>
        /// 敏感词类别管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageSensitiveWordTypes()
        {
            pageResourceManager.InsertTitlePart("敏感词类别管理");
            IEnumerable<SensitiveWordType> sensitiveWordTypes = sensitiveWordService.GetAllSensitiveWordTypes();
            return View(sensitiveWordTypes);
        }

        /// <summary>
        /// 添加敏感词类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _AddSensitiveWordType(int? id = null)
        {
            if (id == null)
            {
                SensitiveWordTypeEditModel model = new SensitiveWordTypeEditModel();
                return View(model);

            }
            else
            {
                var sensitiveWordType = sensitiveWordService.GetSensitiveWordType(id.Value);
                SensitiveWordTypeEditModel model = sensitiveWordType.AsEditModel();
                return View(model);
            }

        }

        /// <summary>
        /// 添加敏感词类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _AddSensitiveWordType(SensitiveWordTypeEditModel model)
        {
            if (!ModelState.IsValid)
            {
                WebUtility.SetStatusCodeForError(Response);
                return View(model);
            }
            if (model.TypeId == null || model.TypeId == 0)
            {
                sensitiveWordService.CreateType(model.AsSensitiveWordType());
                return Json(new StatusMessageData(StatusMessageType.Success, "添加成功！"));
            }
            else
            {
                sensitiveWordService.UpdateType(model.AsSensitiveWordType());
                return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
            }
        }

        /// <summary>
        /// 批量删除敏感词类别
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult DeleteSensitiveWordTypes(List<int> ids)
        {
            if (ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    sensitiveWordService.DeleteType(id);
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"));
            }

        }

        #endregion

        #region 地区管理

        /// <summary>
        /// 呈现地区列表，在search按钮处调用
        /// </summary>
        /// <param name="areaCode">父级地区编码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageAreas(string areaCode)
        {
            pageResourceManager.InsertTitlePart("地区管理");
            IEnumerable<Area> childAreas = null;

            if (!string.IsNullOrEmpty(areaCode))
            {
                var area = areaService.Get(areaCode);
                if (area == null)
                    return HttpNotFound();
                childAreas = area.Children;
            }
            else
            {
                childAreas = areaService.GetRoots();
            }
            return View(childAreas);
        }


        /// <summary>
        /// 创建地区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _CreateArea(string parentCode)
        {
            if (!string.IsNullOrEmpty(parentCode))
            {
                Area parentArea = areaService.Get(parentCode);
                ViewData["parentCreateArea"] = parentArea;

            }

            return View();
        }

        /// <summary>
        /// 创建地区
        /// </summary>
        /// <param name="model">用于创建地区的EditModel</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreateArea(AreaCreateModel model)
        {


            if (areaService.Get(model.AreaCode) == null)
            {


                areaService.Create(model.AsArea());
                return Json(new StatusMessageData(StatusMessageType.Success, "创建成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "行政区号有重复，创建失败！"));
            }


        }

        /// <summary>
        /// 编辑地区
        /// </summary>
        /// <param name="areaCode">当前编辑地区的编码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditArea(string areaCode)
        {
            var area = areaService.Get(areaCode);
            if (area == null)
                return HttpNotFound();
            Area parentArea = areaService.Get(area.ParentCode);

            ViewData["parentEditArea"] = parentArea;



            return View(area.AsEditModel());
        }

        /// <summary>
        /// 编辑地区
        /// </summary>
        /// <param name="model">用于编辑地区的EditModel</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditArea(AreaEditModel model)
        {
            bool flag = areaService.IsChildArea(model.ParentCode, model.AreaCode);

            if (flag)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "不能将地区的父地区设置为其子地区！"));
            }
            areaService.Update(model.AsArea());


            return Json(new StatusMessageData(StatusMessageType.Success, "更新成功！"));
        }

        /// <summary>
        /// 删除地区
        /// </summary>
        /// <param name="areaCode">要删除地区的地区编码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteArea(string areaCode)
        {
            if (areaService.Get(areaCode) == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到所要删除的地区"));
            areaService.Delete(areaCode);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        /// <summary>
        /// 更改地区排列顺序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public ActionResult ChangeAreaDisplayOrder(string id, string referenceId)
        {
            var area = areaService.Get(id);
            var referenceArea = areaService.Get(referenceId);
            int displayOrder = area.DisplayOrder;
            area.DisplayOrder = referenceArea.DisplayOrder;
            referenceArea.DisplayOrder = displayOrder;
            areaService.Update(area);
            areaService.Update(referenceArea);
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }



        #endregion

        #region 学校管理
        /// <summary>
        /// 呈现地区列表，在search按钮处调用
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="keyword"></param>
        /// <param name="schoolType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult ManageSchools(string areaCode = null, string keyword = null, SchoolType? schoolType = null, int pageSize = 50, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("学校管理");

            PagingDataSet<School> schools = schoolService.Gets(areaCode, keyword, schoolType, pageSize, pageIndex);
            ViewData["areaCode"] = areaCode;

            return View(schools);
        }

        /// <summary>
        /// 创建与编辑学校
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditSchool(long? id = null, string areaCode = null, SchoolType? schoolType = null)
        {
            if (id == null)
            {
                SchoolEditModel model = new SchoolEditModel();
                model.AreaCode = areaCode;
                model.SchoolType = schoolType ?? SchoolType.University;
                return View(model);
            }
            else
            {
                var school = schoolService.Get(id.Value);
                SchoolEditModel model = school.AsEditModel();
                return View(model);
            }

        }

        /// <summary>
        /// 创建与编辑学校
        /// </summary>
        /// <param name="model">用于创建学校的EditModel</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditSchool(SchoolEditModel model)
        {
            if (!ModelState.IsValid)
            {
                WebUtility.SetStatusCodeForError(Response);
                return View(model);
            }

            if (!model.Id.HasValue || model.Id == 0)
            {
                schoolService.Create(model.AsSchool());
                return Json(new StatusMessageData(StatusMessageType.Success, "创建成功！"));
            }
            else
            {
                schoolService.Update(model.AsSchool());
                return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
            }

        }

        /// <summary>
        /// 删除学校
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSchool(long id)
        {
            if (id != 0)
            {
                schoolService.Delete(id);
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"));
            }
        }

        /// <summary>
        /// 交换学校显示顺序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeSchoolDisplayOrder()
        {
            long id = Request.Form.Get<long>("id", 0);
            long referenceId = Request.Form.Get<long>("referenceId", 0);
            schoolService.ChangeDisplayOrder(id, referenceId);
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }
        #endregion


        #region 消息设置

        /// <summary>
        /// 消息设置
        /// </summary>
        /// <returns></returns>
        public ActionResult MessagesSettings()
        {
            pageResourceManager.InsertTitlePart("消息设置");
            NoticeSettings noticeSettings = noticeSettingsManager.Get();
            InvitationSettings invitationSettings = invitationSettingsManager.Get();

            MessagesSettingEditModel messagesSettingEditModel = new MessagesSettingEditModel(noticeSettings, invitationSettings);
            return View(messagesSettingEditModel);
        }

        /// <summary>
        /// 保存消息
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveMessages(MessagesSettingEditModel messagesSettingEditModel)
        {
            InvitationSettings invitationSettings;
            NoticeSettings noticeSettings = messagesSettingEditModel.AsMessagesSettings(out invitationSettings);
            noticeSettingsManager.Save(noticeSettings);
            invitationSettingsManager.Save(invitationSettings);

            return Json(new StatusMessageData(StatusMessageType.Success, "保存成功！"));
        }
        #endregion

        #region 邮件管理

        /// <summary>
        /// 邮件管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageEmails()
        {
            pageResourceManager.InsertTitlePart("邮件管理");
            EmailSettings emailSettings = emailSettingsManager.Get();
            EmailSettingsEditModel emailSettingsEditModel = new EmailSettingsEditModel(emailSettings);
            return View(emailSettingsEditModel);
        }

        /// <summary>
        /// 检测Email
        /// </summary>
        /// <param name="userEmailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckEmail(string userEmailAddress, string password)
        {
            //检测邮箱配置
            EmailServiceProvider emailProvider;
            string[] userEmailArray = userEmailAddress.Split('@');
            if (userEmailArray.Length != 2)
                return Json(new StatusMessageData(StatusMessageType.Error, "邮箱地址格式不正确！"));
            string domainName = userEmailArray[1];
            string userName1 = userEmailAddress;
            string userName2 = userEmailArray[0];
            try
            {
                emailProvider = EmailServiceProviderService.Instance().GetEmailConfig()[domainName];
            }
            catch (KeyNotFoundException)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "检测失败，请完善更多设置"));
            }

            //保存到数据库，以便发送邮件时使用
            var emailSettings = emailSettingsManager.Get();
            emailSettings.SmtpSettings = new SmtpSettings()
            {
                EnableSsl = emailProvider.SmtpSettings.EnableSsl,
                ForceSmtpUserAsFromAddress = emailProvider.SmtpSettings.ForceSmtpUserAsFromAddress,
                Host = emailProvider.SmtpSettings.Host,
                Port = emailProvider.SmtpSettings.Port,
                UserName = emailProvider.SmtpSettings.UserName,
                Password = emailProvider.SmtpSettings.Password,
                RequireCredentials = emailProvider.SmtpSettings.RequireCredentials,
                UserEmailAddress = emailProvider.SmtpSettings.UserEmailAddress
            };
            emailSettings.SmtpSettings.UserEmailAddress = userEmailAddress;
            emailSettings.SmtpSettings.Password = password;
            emailSettings.SmtpSettings.UserName = userName1;
            emailSettingsManager.Save(emailSettings);

            //发送测试邮件(用户名为邮箱密码)
            string errorMessage;
            MailMessage mail = new MailMessage(userEmailAddress, userEmailAddress);
            mail.Subject = "这是一封测试邮件";
            mail.Body = "邮件设置检测成功！";
            mail.IsBodyHtml = false;

            if (!emailService.Send(mail, out errorMessage))
            {
                //再次发送测试邮件（用户名为根据@截取邮箱）
                emailSettings.SmtpSettings.UserName = userName2;
                emailSettingsManager.Save(emailSettings);
                if (!emailService.Send(mail, out errorMessage))
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, errorMessage));
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "检测成功"));
        }

        /// <summary>
        /// 保存检测邮箱设置
        /// </summary>
        /// <param name="emailSettingsEditModel"></param>
        /// <returns></returns>
        public ActionResult SaveEmail(EmailSettingsEditModel emailSettingsEditModel)
        {
            //保存邮箱设置
            EmailSettings emailSettings = null;
            try
            {
                emailSettings = emailSettingsEditModel.AsEmailSettings();
                emailSettingsManager.Save(emailSettings);
            }
            catch (Exception)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "保存失败！"));
            }

            //发送测试邮件
            string errorMessage;
            MailMessage mail = new MailMessage(emailSettingsEditModel.UserEmailAddress, emailSettingsEditModel.UserEmailAddress);
            mail.Subject = "这是一封测试邮件";
            mail.Body = "邮件设置检测成功！";
            mail.IsBodyHtml = false;

            if (!emailService.Send(mail, out errorMessage))
            {
                //如果用户名与邮箱不一样，则将用户名改为与邮箱一样
                if (emailSettings.SmtpSettings.UserEmailAddress != emailSettings.SmtpSettings.UserName)
                    emailSettings.SmtpSettings.UserName = emailSettings.SmtpSettings.UserEmailAddress;
                else
                    emailSettings.SmtpSettings.UserName = emailSettings.SmtpSettings.UserEmailAddress.Split('@')[0];

                emailSettingsManager.Save(emailSettings);
                if (!emailService.Send(mail, out errorMessage))
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "保存成功，但邮件检测失败，原因是：<br/>" + errorMessage));
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "保存成功！"));
        }

        #endregion

        #region Smtp设置

        /// <summary>
        /// 管理邮箱其他设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageEmailOtherSettings()
        {
            EmailSettings emailSettings = emailSettingsManager.Get();
            EmailSettingsEditModel emailSettingsEditModel = new EmailSettingsEditModel(emailSettings);
            return View(emailSettingsEditModel);
        }

        /// <summary>
        /// 管理邮箱的其他设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageEmailOtherSettings(EmailSettingsEditModel model)
        {
            try
            {
                emailSettingsManager.Save(model.AsEmailSettings());
                return Json(new StatusMessageData(StatusMessageType.Success, "保存成功"));
            }
            catch (Exception e)
            {

            }
            return Json(new StatusMessageData(StatusMessageType.Error, "保存失败"));
        }

        /// <summary>
        /// 发件箱列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ListOutbox()
        {
            pageResourceManager.InsertTitlePart("邮件管理");
            return View(new EmailService().ReLoadSmtpSettings());
        }

        /// <summary>
        /// 自动补全Smtp设置
        /// </summary>
        /// <param name="model">Smtp设置的EditModel</param>
        /// <returns>返回已经自动补全的页面</returns>
        [HttpPost]
        public ActionResult _AutoCompletionSmtpSetting(SmtpSettingsEditModel model)
        {
            if (string.IsNullOrEmpty(model.UserEmailAddress) || model.UserEmailAddress.IndexOf("@") < 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "请输入正确的邮箱之后，在继续尝试"));

            EmailServiceProvider emailProvider;
            string domainName = model.UserEmailAddress.Substring(model.UserEmailAddress.LastIndexOf('@') + 1);
            try
            {
                emailProvider = EmailServiceProviderService.Instance().GetEmailConfig()[domainName];

                string userName = model.UserEmailAddress.Substring(0, model.UserEmailAddress.LastIndexOf('@'));

                model.EnableSsl = emailProvider.SmtpSettings.EnableSsl;
                model.ForceSmtpUserAsFromAddress = emailProvider.SmtpSettings.ForceSmtpUserAsFromAddress;
                model.Host = emailProvider.SmtpSettings.Host;
                model.Port = emailProvider.SmtpSettings.Port;
                model.RequireCredentials = emailProvider.SmtpSettings.RequireCredentials;
                model.UserName = emailProvider.SmtpSettings.UserName.Replace("username", userName);
            }
            catch (KeyNotFoundException)
            {
                //return Json(new StatusMessageData(StatusMessageType.Error, "检测失败，请完善更多设置"));
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "检测失败，请手动设置更多选项");
            }

            TempData["SmtpSettingsEditModel"] = model;

            return _EditOutBox();
        }

        /// <summary>
        /// 编辑发件邮箱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditOutBox(long? id = null)
        {
            SmtpSettings smtpSettings = null;

            SmtpSettingsEditModel smtpSettingsEditModel = smtpSettingsEditModel = TempData.Get<SmtpSettingsEditModel>("SmtpSettingsEditModel", null);
            if (smtpSettingsEditModel != null)
            {
                ViewData["NeedAutoCompletion"] = false;
                TempData.Remove("SmtpSettingsEditModel");
                return View("_EditOutBox", smtpSettingsEditModel);
            }

            if (id.HasValue)
            {
                smtpSettings = emailService.GetSmtpSettings(id.Value);
                if (smtpSettings == null)
                    return Content(string.Empty);
            }

            if (smtpSettings == null)
            {
                return View("_EditOutBox", new SmtpSettingsEditModel());
            }

            ViewData["NeedAutoCompletion"] = false;
            smtpSettingsEditModel = smtpSettings.AsEditModel();
            return View("_EditOutBox", smtpSettings.AsEditModel());
        }

        /// <summary>
        /// 编辑发件邮箱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditOutBox(SmtpSettingsEditModel model)
        {
            ViewData["NeedAutoCompletion"] = false;

            if (!ModelState.IsValid || !model.IsValidate)
            {
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "数据验证失败，请检查数据是否填写完整");
                return View(model);
            }

            emailService.SaveSmtpSetting(model.AsSmtpSettings());
            return Json(new StatusMessageData(StatusMessageType.Success, "保存成功"));
        }

        /// <summary>
        /// 禁用或者启用
        /// </summary>
        /// <param name="id">被操作的发件邮箱id</param>
        /// <param name="count">被设置的已经发送数量</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SetOutBoxSendCount(long id, int count)
        {
            SmtpSettings smtpSettings = emailService.GetSmtpSettings(id);
            if (smtpSettings == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到准备更新的Smtp设置"));

            SmtpSettingsChild settings = new SmtpSettingsChild(smtpSettings);
            settings.TodaySendCount = count;

            return Json(new StatusMessageData(StatusMessageType.Success, count > 0 ? "禁用成功" : "启用成功"));
        }

        /// <summary>
        /// 删除Smtp设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteOutBox(long id)
        {
            emailService.DeleteSmtpSettings(id);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        /// <summary>
        /// 测试发件箱是否正常
        /// </summary>
        /// <param name="id">发件箱id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _TestOutBox(long id)
        {
            string message = null;
            emailService.TestStmpSettings(id, out message);
            if (string.IsNullOrEmpty(message))
                return Json(new StatusMessageData(StatusMessageType.Success, "成功发送邮件，请进入对应的邮箱查看是否成功发送"));

            return Json(new StatusMessageData(StatusMessageType.Error, string.Format("发送测试邮件失败，错误：{0}", message)));
        }

        #endregion

    }


    #region SettingsMenu

    /// <summary>
    /// SettingsMenu
    /// </summary>
    public enum SettingsMenu
    {
        /// <summary>
        /// 积分项目管理
        /// </summary>
        ManagePointItems = 0,

        /// <summary>
        /// 审核项目管理
        /// </summary>
        ManageAuditItems = 1,

        /// <summary>
        /// 管理隐私
        /// </summary>
        ManagePrivacyItems = 2,

        /// <summary>
        /// 权限项目管理
        /// </summary>
        ManagePermissionItems = 3,

        /// <summary>
        /// 应用管理
        /// </summary>
        ManageApplications = 4,

        /// <summary>
        /// 导航管理
        /// </summary>
        ManageNavigations = 5,

        /// <summary>
        /// 地区管理
        /// </summary>
        ManageAreas = 6,

        /// <summary>
        /// 学校管理
        /// </summary>
        ManageSchools = 7,

        /// <summary>
        /// 用户设置
        /// </summary>
        ManageUserSettings = 8,

        /// <summary>
        /// 基础设置
        /// </summary>
        ManageSiteSettings = 9,

        /// <summary>
        /// 附件设置
        /// </summary>
        ManageAttachmentSettings = 10,

        /// <summary>
        /// 皮肤管理
        /// </summary>
        ManageThemeSettings = 11
    }

    #endregion SettingsMenu

}