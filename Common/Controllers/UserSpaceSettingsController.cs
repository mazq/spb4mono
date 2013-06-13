//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Spacebuilder.Common.Configuration;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Email;
using Tunynet.FileStore;
using Tunynet.Mvc;
using Tunynet.UI;
using Tunynet.Utilities;
using Tunynet.Logging;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户空间设置Controller
    /// </summary>
    
    
    [Themed(PresentAreaKeysOfBuiltIn.UserSpace, IsApplication = false)]
    [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
    public class UserSpaceSettingsController : Controller
    {
        #region private items

        private UserProfileService userProfileService;
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private UserProfileSettings userProfileSettings = DIContainer.Resolve<IUserProfileSettingsManager>().GetUserProfileSettings();
        private UserBlockService userBlockService = new UserBlockService();
        private PrivacyService privacyService = new PrivacyService();
        private IMembershipService membershipService = DIContainer.Resolve<IMembershipService>();
        private IAuthenticationService authenticationService = DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
        private CategoryService categoryService = new CategoryService();
        private IPrivacySettingsManager privacySettingsManager = DIContainer.Resolve<IPrivacySettingsManager>();
        IPointSettingsManager pointSettingsManger = DIContainer.Resolve<IPointSettingsManager>();
        private IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
        private AccountBindingService accountBindingService = new AccountBindingService();
        #endregion private items

        public UserSpaceSettingsController()
            : this(new UserProfileService())
        {
        }

        public UserSpaceSettingsController(UserProfileService userProfileService)
        {
            this.userProfileService = userProfileService;
        }

        /// <summary>
        /// 编辑用户资料左侧导航
        /// </summary>
        [HttpGet]
        public ActionResult _UserSettingsMenus()
        {
            return View();
        }

        #region 完善个人资料向导

        /// <summary>
        /// 上传头像向导
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfileGuideAvatar(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (UserContext.CurrentUser == null || UserContext.CurrentUser.UserId != user.UserId)
            { return RedirectToAction("SpaceHome"); }

            pageResourceManager.InsertTitlePart("完善个人资料向导");
            pageResourceManager.InsertTitlePart("上传头像");
            return View();
        }

        /// <summary>
        ///填写个人资料向导 
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfileGuideDetail(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (UserContext.CurrentUser == null || UserContext.CurrentUser.UserId != user.UserId)
            { return RedirectToAction("SpaceHome"); }

            if (user == null)
            { return HttpNotFound(); }
            ViewData["userProfile"] = userProfileService.Get(user.UserId);
            pageResourceManager.InsertTitlePart("完善个人资料向导");
            pageResourceManager.InsertTitlePart("填写个人资料");

            #region 年份

            IList<int> startDateList = new List<int>();
            int nowYear = DateTime.Now.Year;
            for (int j = 0; j < 50; j++)
            {
                startDateList.Add(nowYear - j);
            }
            ViewData["StartYear"] = new SelectList(startDateList.Select(n => new { text = n.ToString(), value = n.ToString() }), "value", "text");

            IDictionary<string, string> endDateDict = new Dictionary<string, string>();

            endDateDict["至今"] = DateTime.Now.Year.ToString();
            for (int k = 1; k <= 50; k++)
            {
                endDateDict[(nowYear - k).ToString()] = (nowYear - k).ToString();
            }
            ViewData["EndDate"] = new SelectList(endDateDict.Select(n => new { text = n.Key, value = n.Value }), "value", "text");

            #endregion

            return View();
        }

        /// <summary>
        /// 个人标签向导
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfileGuideTag(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (UserContext.CurrentUser == null || UserContext.CurrentUser.UserId != user.UserId)
            { return RedirectToAction("SpaceHome"); }

            pageResourceManager.InsertTitlePart("完善个人资料向导");
            pageResourceManager.InsertTitlePart("填写个人标签");

            return View();
        }

        /// <summary>
        /// 看看感兴趣的人向导
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfileGuideInterested(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (UserContext.CurrentUser == null || UserContext.CurrentUser.UserId != user.UserId)
            {
                return RedirectToAction("SpaceHome");
            }

            pageResourceManager.InsertTitlePart("完善个人资料向导");
            pageResourceManager.InsertTitlePart("看看感兴趣的人");

            return View();
        }

        /// <summary>
        /// 个人标签向导局部页
        /// </summary>
        /// <param name="spaceKey">空间标示</param>
        /// <returns></returns>
        public ActionResult _EditUserProfileTag()
        {
            return View();
        }

        /// <summary>
        /// 个人资料向导局部页
        /// </summary>
        /// <param name="spaceKey">空间标示</param>
        /// <returns></returns>
        public ActionResult _EditUserProfileDetail(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null) { return HttpNotFound(); }

            UserProfile profile = userProfileService.Get(user.UserId);
            UserProfileEditModel editModel = new UserProfileEditModel(profile, user);

            int minYear = 1919;
            int i = DateTime.Now.Year - minYear;
            IList<SelectListItem> years = new List<SelectListItem>();

            while (i >= 0)
            {
                DateTime dt = DateTime.Now.AddYears(-i);
                years.Add(new SelectListItem() { Text = string.Format("{0}年({1})", ChineseCalendarHelper.GetStemBranch(dt), dt.Year), Value = dt.Year.ToString() });
                i--;
            }

            ViewData["Year"] = years;
            ViewData["minYear"] = minYear;

            ViewData["UserName"] = user.UserName;
            ViewData["statusMessageData"] = TempData["MessageData"];
            #region 隐私设置

            List<SelectListItem> selectListItems = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "所有人可见", Value = PrivacyStatus.Public.ToString()},
                new SelectListItem{ Text = "我关注的人可见", Value = PrivacyStatus.Part.ToString()},
                new SelectListItem{ Text = "仅自己可见", Value = PrivacyStatus.Private.ToString()}
            };
            
            
            Dictionary<string, PrivacyStatus> userPrivacyItems = GetUserPrivacySetting(user.UserId);

            ViewData["PrivacyEmail"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().Email()]);
            ViewData["PrivacyMobile"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().Mobile()]);
            ViewData["PrivacyBirthday"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().Birthday()]);
            ViewData["PrivacyQQ"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().QQ()]);
            ViewData["PrivacyMSN"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().Msn()]);
            ViewData["PrivacyTrueName"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().TrueName()]);

            #endregion

            return View(editModel);
        }

        /// <summary>
        ///上传头像向导局部页 
        /// </summary>
        /// <returns></returns>
        public ActionResult _EditUserProfileAvatar()
        {
            return View();
        }

        /// <summary>
        /// 完成向导局部页
        /// </summary>
        /// <returns></returns>
        public ActionResult _CompleteGuide(string spaceKey)
        {
            return View();
        }

        /// <summary>
        /// 下次登录是否需要向导
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="isNeedGuide">true为不需要</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CompleteGuide(string spaceKey, bool isNeedGuide)
        {
            User user = userService.GetFullUser(spaceKey);

            user.Profile.IsNeedGuide = isNeedGuide;
            userProfileService.Update(user.Profile);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置成功！"));
        }

        #endregion

        #region 上传头像

        /// <summary>
        /// 取消头像裁剪
        /// </summary>
        /// <param name="spaceKey">空间标示</param>
        public JsonResult _CancelAvatar(string spaceKey)
        {
            
            //OK
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IStoreFile iStoreFile = new UserService().GetAvatar(userId, AvatarSizeType.Original);
            if (iStoreFile != null)
                storeProvider.DeleteFile(iStoreFile.RelativePath, iStoreFile.Name);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 头像裁剪
        /// </summary>
        /// <param name="spaceKey">空间标示</param>
        /// <param name="srcWidth">宽</param>
        /// <param name="srcHeight">高</param>
        /// <param name="srcX">左上角X坐标</param>
        /// <param name="srcY">左上角上角Y坐标</param>
        public JsonResult _CropAvatar(string spaceKey, float srcWidth, float srcHeight, float srcX, float srcY)
        {
            UserService userService = new UserService();

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            userService.CropAvatar(userId, srcWidth, srcHeight, srcX, srcY);
            IStoreFile iStoreFile = userService.GetAvatar(userId, AvatarSizeType.Original);
            if (iStoreFile != null)
            {
                storeProvider.DeleteFile(iStoreFile.RelativePath, iStoreFile.Name);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 头像局部视图异步加载
        /// </summary>
        /// <param name="spaceKey">空间标示</param>
        public ActionResult _EditAvatarAsyn(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return new EmptyResult();

            IStoreFile iStoreFile = new UserService().GetAvatar(user.UserId, AvatarSizeType.Original);
            if (!user.HasAvatar && iStoreFile != null)
            {
                string originalImageUrl = SiteUrls.Instance().UserAvatarUrl(user, AvatarSizeType.Original, false);
                string idString = user.UserId.ToString().PadLeft(15, '0');
                string idPart = storeProvider.JoinDirectory(idString.Substring(0, 5), idString.Substring(5, 5), idString.Substring(10, 5));
                idPart = idPart.Replace("\\", "/");
                originalImageUrl = originalImageUrl.Substring(0, originalImageUrl.LastIndexOf('/') + 1)
                                   + idPart + "/" + user.UserId + "_" + AvatarSizeType.Original.ToString()
                                   + originalImageUrl.Substring(originalImageUrl.LastIndexOf('.'));
                ViewData["originalImageUrl"] = originalImageUrl;
            }
            else if (iStoreFile != null)
            {
                ViewData["originalImageUrl"] = SiteUrls.Instance().UserAvatarUrl(user, AvatarSizeType.Original, false);
            }

            ViewData["bigImageUrl"] = SiteUrls.Instance().UserAvatarUrl(user, AvatarSizeType.Big, false);
            ViewData["SmallImageUrl"] = SiteUrls.Instance().UserAvatarUrl(user, AvatarSizeType.Small, false);

            return View();
        }

        /// <summary>
        /// 编辑头像显示页面
        /// </summary>
        /// <param name="spaceKey">空间标示</param>
        public ActionResult EditAvatar(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("上传头像");
            
            //OK
            return View();
        }

        #endregion 上传头像

        #region 教育经历

        /// <summary>
        /// 编辑用户资料-教育经历页面
        /// </summary>
        [HttpGet]
        public ActionResult EditUserEducation(string spaceKey)
        {
            
            Dictionary<string, PrivacyStatus> userPrivacySettings = GetUserPrivacySetting(UserIdToUserNameDictionary.GetUserId(spaceKey));
            PrivacyStatus privacyStatus = userPrivacySettings[PrivacyItemKeys.Instance().EducationExperience()];
            Dictionary<PrivacyStatus, string> privacyStatusNames = new Dictionary<PrivacyStatus, string>
            {
                {PrivacyStatus.Public, "所有人可见"},
                {PrivacyStatus.Part, "我关注的人可见"},
                {PrivacyStatus.Private, "仅自己可见"}
            };
            ViewData["EditUserEducationPrivacy"] = new KeyValuePair<PrivacyStatus, string>(privacyStatus, privacyStatusNames[privacyStatus]);
            pageResourceManager.InsertTitlePart("教育经历");
            return View();
        }

        /// <summary>
        /// 编辑教育经历页面
        /// </summary>
        [HttpGet]
        public ActionResult _UserEducations(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IEnumerable<EducationExperience> userExperiences = userProfileService.GetEducationExperiences(userId);
            return View(userExperiences);
        }

        /// <summary>
        /// 编辑教育经历页面
        /// </summary>
        public ActionResult _EditUserEducationInfo(string spaceKey, long? educationId)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            ViewData["userProfile"] = userProfileService.Get(user.UserId);

            EducationExperienceEditModel editModel = new EducationExperienceEditModel();

            IList<int> startDateList = new List<int>();
            int nowYear = DateTime.Now.Year;
            for (int i = 0; i < 50; i++)
            {
                startDateList.Add(nowYear - i);
            }
            ViewData["StareYear"] = new SelectList(startDateList.Select(n => new { text = n.ToString(), value = n.ToString() }), "value", "text");

            if (educationId.HasValue && educationId > 0)
            {
                EducationExperience edu = userProfileService.GetEducationExperience(educationId.Value, user.UserId);
                if (edu != null)
                    editModel = edu.AsEditModel();
            }

            return View(editModel);
        }

        /// <summary>
        /// 编辑 教育经历
        /// </summary>
        /// <param name="model">用于表单提交的实体</param>
        [HttpPost]
        public JsonResult UpdateEducation(string spaceKey, EducationExperienceEditModel model)
        {
            string message = string.Empty;
            if (ModelState.HasBannedWord(out message))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, message));
            }

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            EducationExperience education = model.AsEducationExperience(userId);
            userProfileService.UpdateEducationExperience(education);

            if (education.Id > 0)
                return Json(new StatusMessageData(StatusMessageType.Success, "更新教育经历成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "更新教育经历失败!"));
        }

        /// <summary>
        /// 添加 教育经历表单提交
        /// </summary>
        /// <param name="model">用于表单提交的实体</param>
        [HttpPost]
        public JsonResult CreateEducation(string spaceKey, EducationExperienceEditModel model)
        {
            string message = string.Empty;
            if (ModelState.HasBannedWord(out message))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, message));
            }

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            EducationExperience education = model.AsEducationExperience(userId);
            bool result = userProfileService.CreateEducationExperience(education);
            if (result)
                return Json(new StatusMessageData(StatusMessageType.Success, "创建教育经历成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "创建教育经历失败!"));
        }

        /// <summary>
        /// 删除教育经历
        /// </summary>
        /// <param name="educationId">教育经历ID</param>
        [HttpPost]
        public JsonResult DeleteUserEducation(string spaceKey, long educationId)
        {
            userProfileService.DeleteEducationExperience(educationId);
            if (educationId > 0)
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        

        /// <summary>
        /// 更新用户教育隐私设置
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="EditUserEducationPrivacy">教育信息隐私设置</param>
        /// <returns>用户教育隐私设置结果</returns>
        [HttpPost]
        public ActionResult EditUserEducationPrivacySetting(string spaceKey, PrivacyStatus EditUserEducationPrivacy)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            Dictionary<string, PrivacyStatus> userSetting = new Dictionary<string, PrivacyStatus>();
            userSetting.Add(PrivacyItemKeys.Instance().EducationExperience(), EditUserEducationPrivacy);
            Dictionary<string, IEnumerable<UserPrivacySpecifyObject>> userPrivacySpecifyObject = new Dictionary<string, IEnumerable<UserPrivacySpecifyObject>>();
            if (EditUserEducationPrivacy == PrivacyStatus.Part)
                userPrivacySpecifyObject.Add(PrivacyItemKeys.Instance().EducationExperience(), new List<UserPrivacySpecifyObject> { GetUserPrivacySpecifyObject_AllGroup() });
            privacyService.UpdateUserPrivacySettings(userId, userSetting, userPrivacySpecifyObject);

            return Json(new StatusMessageData(StatusMessageType.Success, "更新教育隐私设置成功"));
        }


        #endregion 教育经历

        #region 工作经历

        /// <summary>
        /// 编辑用户资料-工作经历页面
        /// </summary>
        [HttpGet]
        public ActionResult EditUserWork(string spaceKey)
        {
            Dictionary<string, PrivacyStatus> userPrivacySettings = GetUserPrivacySetting(UserIdToUserNameDictionary.GetUserId(spaceKey));
            PrivacyStatus privacyStatus = userPrivacySettings[PrivacyItemKeys.Instance().WorkExperience()];
            Dictionary<PrivacyStatus, string> privacyStatusNames = new Dictionary<PrivacyStatus, string>
            {
                {PrivacyStatus.Public, "所有人可见"},
                {PrivacyStatus.Part, "我关注的人可见"},
                {PrivacyStatus.Private, "仅自己可见"}
            };
            ViewData["EditUserWorkPrivacy"] = new KeyValuePair<PrivacyStatus, string>(privacyStatus, privacyStatusNames[privacyStatus]);
            pageResourceManager.InsertTitlePart("工作经历");

            return View();
        }

        /// <summary>
        /// 更新用户教育隐私设置
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="EditUserWorkPrivacy">教育信息隐私设置</param>
        /// <returns>用户教育隐私设置结果</returns>
        [HttpPost]
        public ActionResult EditUserWorkPrivacyPrivacySetting(string spaceKey, PrivacyStatus EditUserWorkPrivacy)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            Dictionary<string, PrivacyStatus> userSetting = new Dictionary<string, PrivacyStatus>();
            userSetting.Add(PrivacyItemKeys.Instance().WorkExperience(), EditUserWorkPrivacy);
            Dictionary<string, IEnumerable<UserPrivacySpecifyObject>> userPrivacySpecifyObject = new Dictionary<string, IEnumerable<UserPrivacySpecifyObject>>();
            if (EditUserWorkPrivacy == PrivacyStatus.Part)
                userPrivacySpecifyObject.Add(PrivacyItemKeys.Instance().EducationExperience(), new List<UserPrivacySpecifyObject> { GetUserPrivacySpecifyObject_AllGroup() });
            privacyService.UpdateUserPrivacySettings(userId, userSetting, userPrivacySpecifyObject);

            return Json(new StatusMessageData(StatusMessageType.Success, "更新工作经历隐私设置成功"));
        }

        /// <summary>
        /// 工作经历页面
        /// </summary>
        [HttpGet]
        public ActionResult _UserWorks(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IEnumerable<WorkExperience> workExperiences = userProfileService.GetWorkExperiences(userId);
            return View(workExperiences);
        }

        /// <summary>
        /// 编辑工作经历页面
        /// </summary>
        [HttpGet]
        public ActionResult _EditUserWorkInfo(string spaceKey, long? workId)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            WorkExperienceEditModel editModel = new WorkExperienceEditModel();

            int nowYear = DateTime.Now.Year;

            IList<string> startDateList = new List<string>();
            for (int i = 0; i <= 50; i++)
            {
                startDateList.Add((nowYear - i).ToString());
            }
            ViewData["StartDate"] = new SelectList(startDateList.Select(n => new { text = n.ToString(), value = n.ToString() }), "value", "text");

            IDictionary<string, string> endDateDict = new Dictionary<string, string>();

            endDateDict["至今"] = DateTime.Now.Year.ToString();
            for (int i = 1; i <= 50; i++)
            {
                endDateDict[(nowYear - i).ToString()] = (nowYear - i).ToString();
            }
            ViewData["EndDate"] = new SelectList(endDateDict.Select(n => new { text = n.Key, value = n.Value }), "value", "text");

            if (workId.HasValue && workId > 0)
            {
                WorkExperience edu = userProfileService.GetWorkExperience(workId.Value, user.UserId);
                if (edu != null)
                    editModel = edu.AsEditModel();
            }

            return View(editModel);
        }

        /// <summary>
        /// 编辑 工作经历
        /// </summary>
        /// <param name="model">用于表单提交的实体</param>
        [HttpPost]
        public JsonResult UpdateWork(string spaceKey, WorkExperienceEditModel model)
        {
            string message = string.Empty;
            if (ModelState.HasBannedWord(out message))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, message));
            }

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            WorkExperience work = model.AsWorkExperience(userId);
            userProfileService.UpdateWorkExperience(work);
            if (work.Id > 0)
                return Json(new StatusMessageData(StatusMessageType.Success, "更新工作经历成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "更新工作经历失败!"));
        }

        /// <summary>
        /// 添加 工作经历表单提交
        /// </summary>
        /// <param name="model">用于表单提交的实体</param>
        [HttpPost]
        public JsonResult CreateWork(string spaceKey, WorkExperienceEditModel model)
        {
            string message = string.Empty;
            if (ModelState.HasBannedWord(out message))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, message));
            }

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            WorkExperience work = model.AsWorkExperience(userId);
            bool result = userProfileService.CreateWorkExperience(work);
            if (result)
                return Json(new StatusMessageData(StatusMessageType.Success, "创建工作经历成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "创建工作经历失败!"));
        }

        /// <summary>
        /// 删除工作经历
        /// </summary>
        /// <param name="WorkId">教育经历ID</param>
        [HttpPost]
        public JsonResult DeleteUserWork(string spaceKey, long WorkId)
        {
            userProfileService.DeleteWorkExperience(WorkId);
            if (WorkId > 0)
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        #endregion 工作经历

        #region 基本资料

        /// <summary>
        /// 编辑用户资料-基本资料页面
        /// </summary>
        [HttpGet]
        public ActionResult EditUserProfile(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            pageResourceManager.InsertTitlePart("基本资料");

            UserProfile profile = userProfileService.Get(user.UserId);
            UserProfileEditModel editModel = new UserProfileEditModel(profile, user);

            int minYear = 1919;
            int i = DateTime.Now.Year - minYear;
            IList<SelectListItem> years = new List<SelectListItem>();

            while (i >= 0)
            {
                DateTime dt = DateTime.Now.AddYears(-i);
                years.Add(new SelectListItem() { Text = string.Format("{0}年({1})", ChineseCalendarHelper.GetStemBranch(dt), dt.Year), Value = dt.Year.ToString() });
                i--;
            }

            ViewData["Year"] = years;
            ViewData["minYear"] = minYear;

            ViewData["UserName"] = user.UserName;
            //ViewData["statusMessageData"] = TempData["MessageData"];
            #region 隐私设置

            List<SelectListItem> selectListItems = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "所有人可见", Value = PrivacyStatus.Public.ToString()},
                new SelectListItem{ Text = "我关注的人可见", Value = PrivacyStatus.Part.ToString()},
                new SelectListItem{ Text = "仅自己可见", Value = PrivacyStatus.Private.ToString()}
            };
            
            
            Dictionary<string, PrivacyStatus> userPrivacyItems = GetUserPrivacySetting(user.UserId);

            ViewData["PrivacyEmail"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().Email()]);
            ViewData["PrivacyMobile"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().Mobile()]);
            ViewData["PrivacyBirthday"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().Birthday()]);
            ViewData["PrivacyQQ"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().QQ()]);
            ViewData["PrivacyMSN"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().Msn()]);
            ViewData["PrivacyTrueName"] = new SelectList(selectListItems, "Value", "Text", userPrivacyItems[PrivacyItemKeys.Instance().TrueName()]);

            #endregion

            return View(editModel);
        }

        /// <summary>
        /// 更新基本资料 表单提交
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="model">用于表单提交的实体</param>
        [HttpPost]
        public JsonResult EditUserProfile(string spaceKey, UserProfileEditModel model)
        {
            string errorMessage = null;
            if (ModelState.HasBannedWord(out errorMessage))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, errorMessage));
            }

            User oldUser = userService.GetFullUser(spaceKey);
            string oldemail = oldUser.AccountEmail;

            UserProfile userProfile = model.AsUserProfile(oldUser.UserId);
            if (userProfileService.Get(oldUser.UserId) != null)
                userProfileService.Update(userProfile);
            else
                userProfileService.Create(userProfile);

            User user = model.AsUser(oldUser.UserId);

            if (oldemail.Equals(model.AccountEmail) || Utility.ValidateEmail(model.AccountEmail, out errorMessage))
            {
                user.AccountEmail = model.AccountEmail;

                if (oldemail != model.AccountEmail)
                    user.IsEmailVerified = false;

                IMembershipService iMembershipService = DIContainer.Resolve<IMembershipService>();
                iMembershipService.UpdateUser(user);
            }

            #region 处理隐私设置
            Dictionary<string, IEnumerable<UserPrivacySpecifyObject>> userPrivacySpecifyObject = new Dictionary<string, IEnumerable<UserPrivacySpecifyObject>>();
            if (model.PrivacyBirthday == PrivacyStatus.Part)
                userPrivacySpecifyObject.Add(PrivacyItemKeys.Instance().Birthday(), new List<UserPrivacySpecifyObject> { GetUserPrivacySpecifyObject_AllGroup() });
            if (model.PrivacyEmail == PrivacyStatus.Part)
                userPrivacySpecifyObject.Add(PrivacyItemKeys.Instance().Email(), new List<UserPrivacySpecifyObject> { GetUserPrivacySpecifyObject_AllGroup() });
            if (model.PrivacyMobile == PrivacyStatus.Part)
                userPrivacySpecifyObject.Add(PrivacyItemKeys.Instance().Mobile(), new List<UserPrivacySpecifyObject> { GetUserPrivacySpecifyObject_AllGroup() });
            if (model.PrivacyMSN == PrivacyStatus.Part)
                userPrivacySpecifyObject.Add(PrivacyItemKeys.Instance().Msn(), new List<UserPrivacySpecifyObject> { GetUserPrivacySpecifyObject_AllGroup() });
            if (model.PrivacyQQ == PrivacyStatus.Part)
                userPrivacySpecifyObject.Add(PrivacyItemKeys.Instance().QQ(), new List<UserPrivacySpecifyObject> { GetUserPrivacySpecifyObject_AllGroup() });

            Dictionary<string, PrivacyStatus> userSettings = new Dictionary<string, PrivacyStatus>();
            userSettings.Add(PrivacyItemKeys.Instance().Birthday(), model.PrivacyBirthday);
            userSettings.Add(PrivacyItemKeys.Instance().Email(), model.PrivacyEmail);
            userSettings.Add(PrivacyItemKeys.Instance().Mobile(), model.PrivacyMobile);
            userSettings.Add(PrivacyItemKeys.Instance().Msn(), model.PrivacyMSN);
            userSettings.Add(PrivacyItemKeys.Instance().QQ(), model.PrivacyQQ);

            privacyService.UpdateUserPrivacySettings(user.UserId, userSettings, userPrivacySpecifyObject);
            #endregion

            if (!string.IsNullOrEmpty(errorMessage))
                return Json(new StatusMessageData(StatusMessageType.Error, "更新邮件失败"));

            return Json(new StatusMessageData(StatusMessageType.Success, "更新用户成功"));

        }

        /// <summary>
        /// 编辑用户资料-基本资料页面
        /// </summary>
        [HttpGet]
        public ActionResult SendAsyn(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            bool sendStatus = false;
            try
            {
                System.Net.Mail.MailMessage mailMessage = EmailBuilder.Instance().RegisterValidateEmail(user);
                EmailService emailService = new EmailService();
                sendStatus = emailService.SendAsyn(mailMessage);
            }
            catch (Exception e)
            {
                ILogger logger = LoggerFactory.GetLogger();
                logger.Error(e, "异步发送激活邮件时报错");
            }

            if (sendStatus)
            {
                TempData["SendEmailSucceedViewModel"] = SendEmailSucceedViewModelFactory.GetRegisterSendEmailSucceedViewModel(user.AccountEmail);
                return Redirect(SiteUrls.Instance().SendEmailSucceed());
            }
            TempData["SendEmailStatus"] = false;
            return RedirectToAction("EditUserProfile", new { spaceKey = spaceKey });
        }

        /// <summary>
        /// 修改用户账户邮箱
        /// </summary>
        [HttpGet]
        public ActionResult _EditUserAccountEmail(string spaceKey)
        {
            
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            ViewData["userId"] = user.UserId;
            ViewData["email"] = Request.QueryString["email"];
            return View();
        }

        /// <summary>
        /// 修改用户账户邮箱表单
        /// </summary>
        [HttpPost]
        public ActionResult EditUserAccountEmail(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            string accountEmail = Request.Form["_accountEmail"];
            
            if (string.IsNullOrEmpty(accountEmail))
            {
                WebUtility.SetStatusCodeForError(Response);
                ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "对不起，邮箱不能为空");
                ViewData["email"] = accountEmail;
                return View("_EditUserAccountEmail");
            }
            string errmessage = "";
            bool isValid = Utility.ValidateEmail(accountEmail, out errmessage);
            if (!isValid)
            {
                WebUtility.SetStatusCodeForError(Response);
                ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, WebUtility.HtmlDecode(errmessage));
                ViewData["email"] = accountEmail;
                return View("_EditUserAccountEmail");
            }

            user.AccountEmail = accountEmail;
            IMembershipService iMembershipService = DIContainer.Resolve<IMembershipService>();
            iMembershipService.UpdateUser(user);

            return RedirectToAction("EditUserProfile", new { spaceKey = spaceKey });
        }

        #endregion 基本资料

        #region 个人标签

        /// <summary>
        /// 编辑用户资料-个人标签
        /// </summary>
        [HttpGet]
        public ActionResult EditUserTags(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("个人标签");

            return View();
        }

        /// <summary>
        /// 编辑用户资料-用户个人标签控件
        /// </summary>
        [HttpGet]
        public ActionResult _ListMyUserTags(string spaceKey)
        {
            
            //OK

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            TagService tagService = new TagService(TenantTypeIds.Instance().User());
            //取用户标签
            IEnumerable<ItemInTag> myUserTags = tagService.GetItemInTagsOfItem(userId);

            return View(myUserTags);
        }

        /// <summary>
        /// 编辑用户资料-所有用户标签
        /// </summary>
        [HttpGet]
        public ActionResult _ListUsersTags(string spaceKey)
        {
            //取所有用户标签
            TagService tagService = new TagService(TenantTypeIds.Instance().User());
            IDictionary<TagGroup, List<string>> tagsInGroupDict = new Dictionary<TagGroup, List<string>>();
            IEnumerable<TagGroup> tagGroups = tagService.GetGroups();
            if (tagGroups != null)
            {
                foreach (TagGroup tagGroup in tagGroups)
                {
                    tagsInGroupDict[tagGroup] = tagService.GetTopTagsOfGroup(tagGroup.GroupId, 20).ToList();
                }
            }

            int count = userProfileSettings.MaxPersonTag;
            if (tagGroups != null)
                ViewData["tagGroupsCount"] = tagGroups.ToList().Count;

            ViewData["count"] = count;
            return View(tagsInGroupDict);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="itemInTagId">标签关联ID</param>
        [HttpPost]
        public JsonResult DeleteMyUserTag(string spaceKey, long itemInTagId)
        {
            TagService tagService = new TagService(TenantTypeIds.Instance().User());
            tagService.DeleteTagFromItem(itemInTagId);
            if (itemInTagId > 0)
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        /// <summary>
        /// 创建个人标签
        /// </summary>
        /// <param name="spaceKey">用户spaceKey</param>
        [HttpPost]
        public JsonResult CreateUserTag(string spaceKey)
        {
            bool isBanned = false;
            string tagName = Request.Form["TagName"];
            tagName = TextFilter(tagName, out isBanned);
            if (isBanned)
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "内容中包含非法词语!"));
            }

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            TagService tagService = new TagService(TenantTypeIds.Instance().User());

            int count = userProfileSettings.MaxPersonTag;
            if (tagService.GetItemInTagsOfItem(userId).Count() >= count)
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "最多只能添加" + count + "个标签!"));
            }

            if (tagName != null)
            {
                Tag tag = Tag.New();
                tag.TagName = tagName;
                tag.OwnerId = userId;
                tag.TenantTypeId = TenantTypeIds.Instance().User();

                tagService.Create(tag);
                tagService.AddTagToItem(tagName, userId, userId);
                return Json(new StatusMessageData(StatusMessageType.Success, "创建成功"));
                // tagService.
            }
            WebUtility.SetStatusCodeForError(Response);

            return Json(new StatusMessageData(StatusMessageType.Error, "标签不能为空！"));
        }

        /// <summary>
        /// 创建关联个人标签
        /// </summary>
        /// <param name="spaceKey">用户spaceKey</param>
        public ActionResult AddTagToItem(string spaceKey, string tagName)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (tagName != null)
            {
                TagService tagService = new TagService(TenantTypeIds.Instance().User());
                tagService.AddTagToItem(tagName, userId, userId);
            }

            return RedirectToAction("_ListMyUserTags", new { spaceKey = spaceKey, l = DateTime.UtcNow.Millisecond });
        }

        /// <summary>
        /// 管理我的标签，根据租户类型id查找
        /// </summary>
        /// <param name="spaceKey">用户spaceKey</param>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns></returns>
        public ActionResult ManageMyTags(string spaceKey, string tenantTypeId)
        {
            //取所有用户标签
            TagService tagService = new TagService(tenantTypeId);
            IEnumerable<TagInOwner> tags = tagService.GetOwnerTags(UserContext.CurrentUser.UserId);

            pageResourceManager.InsertTitlePart("标签管理");

            return View(tags);
        }

        /// <summary>
        /// 创建标签页
        /// </summary>
        /// <returns></returns>
        public ActionResult _CreateMyTag(string spaceKey, string tenantTypeId)
        {
            TagEditModel tagEditModel = new TagEditModel();
            return View(tagEditModel);
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreateMyTag(string spaceKey, string tenantTypeId, TagEditModel tagEditModel)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && !string.IsNullOrEmpty(tagEditModel.TagName))
            {
                TagService tagService = new TagService(tenantTypeId);
                tagService.AddTagInOwner(tagEditModel.TagName, tenantTypeId, currentUser.UserId);
                return Json(new StatusMessageData(StatusMessageType.Success, "创建成功！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "创建失败！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除我的标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteMyTags(string spaceKey, string tenantTypeId, IEnumerable<long> tagIds)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && tagIds.Count() > 0)
            {
                TagService tagService = new TagService(tenantTypeId);
                foreach (var tagId in tagIds)
                {
                    tagService.DeleteOwnerTag(tagId);
                    //tagService.DeleteTagFromItem(tagId);
                }
                return Json(new { MessageType = StatusMessageType.Success, MessageContent = "删除成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 编辑我的标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditMyTag(string spaceKey, TagEditModel tagEditModel, string tenantTypeId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && tagEditModel.TagId > 0 && !string.IsNullOrEmpty(tagEditModel.TagName))
            {
                TagService tagService = new TagService(tenantTypeId);
                tagService.DeleteOwnerTag(tagEditModel.TagId);
                tagService.AddTagInOwner(tagEditModel.TagName, tenantTypeId, currentUser.UserId);
                return Json(new { MessageType = StatusMessageType.Success, MessageContent = "更新成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "更新失败！"), JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 编辑我的标签页
        /// </summary>
        public ActionResult _EditMyTag(string spaceKey, long tagId, string tagName, string tenantTypeId)
        {
            TagEditModel tagEditModel = new TagEditModel();
            tagEditModel.TagId = tagId;
            tagEditModel.TagName = tagName;
            tagEditModel.TenantTypeId = tenantTypeId;
            return View(tagEditModel);
        }
        #endregion 个人标签

        #region 屏蔽用户

        /// <summary>
        /// 屏蔽用户
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>屏蔽群组名</returns>
        [HttpGet]
        public ActionResult BlockUsers(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("屏蔽用户");
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IEnumerable<UserBlockedObject> blockUsers = userBlockService.GetBlockedUsers(userId);
            List<UserBlockedObjectViewModel> blockUserViewModels = new List<UserBlockedObjectViewModel>();
            if (blockUsers != null)
                foreach (var item in blockUsers)
                    blockUserViewModels.Add(item.AsViewModel());
            PrivacySettings privacySettings = privacySettingsManager.Get();
            ViewData["specifyUserMaxCount"] = privacySettings.SpecifyUserMaxCount;
            return View(blockUserViewModels);
        }

        /// <summary>
        /// 屏蔽群组
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>屏蔽群组名</returns>
        [HttpGet]
        public ActionResult BlockGroups(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("屏蔽群组");
            return View();
        }

        /// <summary>
        /// 屏蔽用户处理方法
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="blockUserIds">被屏蔽的用户id</param>
        /// <returns>屏蔽用户页面</returns>
        [HttpPost]
        public ActionResult BlockUsers(string spaceKey, string blockUserIds)
        {
            int addCount = 0;
            string[] blockUser = blockUserIds.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (blockUser != null && blockUser.Length <= 10)
                foreach (string userIdStr in blockUser)
                {
                    long id = -1;
                    if (long.TryParse(userIdStr, out id))
                    {
                        IUser user = userService.GetUser(id);
                        if (user == null)
                            continue;
                        if (userBlockService.IsBlockedUser(userId, id))
                            continue;
                        string blockUserName = user.DisplayName;
                        userBlockService.BlockUser(userId, id, blockUserName);
                        addCount++;
                    }
                }
            //操作完成
            if (addCount > 0)
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, string.Format("成功添加{0}人到屏蔽列表", addCount));
            else
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "没有任何人被添加到屏蔽列表中");
            return Redirect(SiteUrls.Instance().BlockUsers(spaceKey));
        }


        /// <summary>
        /// 解除屏蔽对象
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="UnBlockId">解除限制的id</param>
        /// <returns>解除限制完成页面</returns>
        [HttpPost]
        public ActionResult UnBlock(string spaceKey, long UnBlockId)
        {
            if (userBlockService.Delete(UnBlockId))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
            }
            return Json(new StatusMessageData(StatusMessageType.Error, "解除失败"));
        }

        #endregion 屏蔽用户

        #region 黑名单

        /// <summary>
        /// 黑名单页面
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <returns>用户的黑名单页面</returns>
        [HttpGet]
        public ActionResult Blacklist(string spaceKey)
        {
            ViewData["StatusMessageData"] = TempData["StatusMessageData"];
            TempData.Remove("StatusMessageData");
            pageResourceManager.InsertTitlePart("黑名单");
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            Dictionary<long, StopedUser> stopedUsers = privacyService.GetStopedUsers(userId);
            List<StopedUserViewModel> stopedUserViewModels = new List<StopedUserViewModel>();
            foreach (var item in stopedUsers)
                stopedUserViewModels.Add(item.Value.AsViewModel());
            IPrivacySettingsManager privacySettingsManager = DIContainer.Resolve<IPrivacySettingsManager>();
            PrivacySettings privacySettings = privacySettingsManager.Get();
            ViewData["stopUserMaxCount"] = privacySettings.StopUserMaxCount;
            int canSelectCount = privacySettings.StopUserMaxCount - stopedUserViewModels.Count;
            canSelectCount = canSelectCount < 0 ? 0 : canSelectCount;
            ViewData["specifyUserMaxCount"] = canSelectCount > privacySettings.SpecifyUserMaxCount ? privacySettings.SpecifyUserMaxCount : canSelectCount;
            return View(stopedUserViewModels);
        }

        /// <summary>
        /// 添加用户到黑名单
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="blacklistId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Blacklist(string spaceKey, string blacklistId)
        {
            string[] userIds = new string[0];
            if (!string.IsNullOrEmpty(blacklistId))
                userIds = blacklistId.Split(new char[] { ',', '，', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int addCount = 0;
            long spaceUserId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (userIds.Length <= privacySettingsManager.Get().StopUserMaxCount)
            {
                foreach (var userId in userIds)
                {
                    long Id = -1;
                    if (long.TryParse(userId, out Id))
                    {
                        if (privacyService.IsStopedUser(spaceUserId, Id))
                            continue;
                        IUser user = userService.GetUser(Id);
                        if (user == null)
                            continue;
                        StopedUser stopedUser = new StopedUser
                        {
                            ToUserDisplayName = user.DisplayName,
                            ToUserId = Id,
                            UserId = spaceUserId
                        };
                        if (privacyService.CreateStopedUser(stopedUser))
                            addCount++;
                    }
                }
            }
            if (addCount > 0)
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, string.Format("成功的把{0}人加入黑名单", addCount));
            else
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "没有把任何人加入黑名单");
            return Redirect(SiteUrls.Instance().Blacklist(spaceKey));
        }

        /// <summary>
        /// 将用户从黑名单中删除
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="deleteUserId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteStopedUser(string spaceKey, long deleteUserId)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            privacyService.DeleteStopedUser(userId, deleteUserId);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        #endregion 黑名单

        #region 用户隐私

        /// <summary>
        /// 用户隐私设置
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>用户隐私设置页面</returns>
        [HttpGet]
        public ActionResult UserPrivacyItemsSettings(string spaceKey)
        {
            ViewData["StatusMessageData"] = TempData["StatusMessageData"];
            TempData.Remove("StatusMessageData");
            pageResourceManager.InsertTitlePart("隐私设置");
            
            


            IUser user = userService.GetFullUser(spaceKey);
            ViewData["UserId"] = user.UserId;
            List<SelectListItem> selectListItems = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "允许所有人", Value = PrivacyStatus.Public.ToString()},
                new SelectListItem{ Text = "允许部分人", Value = PrivacyStatus.Part.ToString()},
                new SelectListItem{ Text = "不允许", Value = PrivacyStatus.Private.ToString()}
            };

            List<SelectListItem> followSelectListItems = new List<SelectListItem> 
            {
                new SelectListItem{ Text = "允许所有人", Value = PrivacyStatus.Public.ToString()},
                new SelectListItem{ Text = "不允许", Value = PrivacyStatus.Private.ToString()}
            };

            //站点隐私设置
            IEnumerable<PrivacyItemGroup> PrivacyItemGroups = PrivacyItemGroup.GetAll();
            Dictionary<PrivacyItemGroup, List<PrivacyItem>> privacyItems = new Dictionary<PrivacyItemGroup, List<PrivacyItem>>();

            //用户的隐私设置
            Dictionary<string, PrivacyStatus> UserPrivacySettings = privacyService.GetUserPrivacySettings(user.UserId);
            foreach (var item in PrivacyItemGroups)
            {
                
                

                IEnumerable<PrivacyItem> privacyItemFormGroup = privacyService.GetPrivacyItems(item.TypeId, null);
                if (item.TypeId != PrivacyItemGroupIds.Instance().Profile())
                {
                    foreach (var privacyItem in privacyItemFormGroup)
                    {
                        
                        
                        bool containsKey = UserPrivacySettings.ContainsKey(privacyItem.ItemKey);
                        PrivacyStatus privacyStatus = containsKey ? UserPrivacySettings[privacyItem.ItemKey] : privacyItem.PrivacyStatus;
                        if (privacyItem.ItemKey == PrivacyItemKeys.Instance().Follow())
                            TempData[privacyItem.ItemKey] = new SelectList(followSelectListItems, "Value", "Text", privacyStatus);
                        else
                            TempData[privacyItem.ItemKey] = new SelectList(selectListItems, "Value", "Text", privacyStatus);
                        Dictionary<int, IEnumerable<UserPrivacySpecifyObject>> userPrivacySpecifyObjects = privacyService.GetUserPrivacySpecifyObjects(user.UserId, privacyItem.ItemKey);

                        if (privacyStatus == PrivacyStatus.Part)
                        {
                            if (!containsKey)
                            {
                                ViewData[privacyItem.ItemKey + "_" + SpecifyObjectTypeIds.Instance().UserGroup()] = FollowSpecifyGroupIds.All.ToString();
                            }
                            else
                            {
                                if (userPrivacySpecifyObjects.ContainsKey(SpecifyObjectTypeIds.Instance().User()))
                                    ViewData[privacyItem.ItemKey + "_" + SpecifyObjectTypeIds.Instance().User()] = string.Join(",", userPrivacySpecifyObjects[SpecifyObjectTypeIds.Instance().User()].Select(n => n.SpecifyObjectId));
                                if (userPrivacySpecifyObjects.ContainsKey(SpecifyObjectTypeIds.Instance().UserGroup()))
                                    ViewData[privacyItem.ItemKey + "_" + SpecifyObjectTypeIds.Instance().UserGroup()] = string.Join(",", userPrivacySpecifyObjects[SpecifyObjectTypeIds.Instance().UserGroup()].Select(n => n.SpecifyObjectId));
                            }
                        }
                    }
                }
                privacyItems.Add(item, privacyItemFormGroup.ToList());
            }
            return View(privacyItems);
        }

        /// <summary>
        /// 处理用户设置隐私页面
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>处理完成页面</returns>
        [HttpPost]
        public ActionResult UserPrivacyItemsSettingsPost(string spaceKey)
        {
            bool canSave = true;
            IPrivacySettingsManager privacySettingsManager = DIContainer.Resolve<IPrivacySettingsManager>();
            long specifyUserMaxCount = privacySettingsManager.Get().SpecifyUserMaxCount;
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IUser user = userService.GetFullUser(UserIdToUserNameDictionary.GetUserId(spaceKey));
            IEnumerable<PrivacyItemGroup> PrivacyItemGroups = PrivacyItemGroup.GetAll();
            Dictionary<string, PrivacyStatus> userSettings = new Dictionary<string, PrivacyStatus>();
            Dictionary<string, IEnumerable<UserPrivacySpecifyObject>> specifyObjects = new Dictionary<string, IEnumerable<UserPrivacySpecifyObject>>();
            foreach (var privacyItemGroup in PrivacyItemGroups)
            {
                
                
                if (privacyItemGroup.TypeId == PrivacyItemGroupIds.Instance().Profile())
                    continue;
                IEnumerable<PrivacyItem> PrivacyItems = privacyService.GetPrivacyItems(privacyItemGroup.TypeId, null);
                foreach (var privacyItem in PrivacyItems)
                {
                    if (!string.IsNullOrEmpty(Request.Form.Get(privacyItem.ItemKey)))
                        userSettings[privacyItem.ItemKey] = Request.Form.Get<PrivacyStatus>(privacyItem.ItemKey, PrivacyStatus.Public);

                    if (userSettings[privacyItem.ItemKey] != PrivacyStatus.Part)
                        continue;

                    IEnumerable<long> selectedUserIds = Request.Form.Gets<long>(privacyItem.ItemKey + SpecifyObjectTypeIds.Instance().User());
                    if (selectedUserIds != null && selectedUserIds.Count() > specifyUserMaxCount)
                    {
                        canSave = false;
                        break;
                    }
                    List<UserPrivacySpecifyObject> userPrivacySpecifyObjects = new List<UserPrivacySpecifyObject>();
                    if (selectedUserIds != null && selectedUserIds.Count() > 0)
                    {
                        foreach (var selectedUserId in selectedUserIds)
                        {
                            string userName = UserIdToUserNameDictionary.GetUserName(selectedUserId);
                            if (string.IsNullOrEmpty(userName))
                                continue;
                            UserPrivacySpecifyObject userPrivacySpecifyObject = UserPrivacySpecifyObject.New();
                            userPrivacySpecifyObject.SpecifyObjectId = selectedUserId;
                            userPrivacySpecifyObject.SpecifyObjectName = userName;
                            userPrivacySpecifyObject.SpecifyObjectTypeId = SpecifyObjectTypeIds.Instance().User();
                            userPrivacySpecifyObjects.Add(userPrivacySpecifyObject);
                        }
                    }
                    IEnumerable<long> selectedUserGroupIds = Request.Form.Gets<long>(privacyItem.ItemKey + SpecifyObjectTypeIds.Instance().UserGroup());
                    if (selectedUserGroupIds != null && selectedUserGroupIds.Count() > 0)
                    {
                        foreach (var selectedUserGroupId in selectedUserGroupIds)
                        {
                            UserPrivacySpecifyObject userPrivacySpecifyObject = UserPrivacySpecifyObject.New();
                            if (selectedUserGroupId == FollowSpecifyGroupIds.All)
                            {
                                userPrivacySpecifyObject.SpecifyObjectId = selectedUserGroupId;
                                userPrivacySpecifyObject.SpecifyObjectName = "所有分组";
                                userPrivacySpecifyObject.SpecifyObjectTypeId = SpecifyObjectTypeIds.Instance().UserGroup();
                            }
                            else if (selectedUserGroupId == FollowSpecifyGroupIds.Mutual)
                            {
                                userPrivacySpecifyObject.SpecifyObjectId = selectedUserGroupId;
                                userPrivacySpecifyObject.SpecifyObjectName = "相互关注";
                                userPrivacySpecifyObject.SpecifyObjectTypeId = SpecifyObjectTypeIds.Instance().UserGroup();
                            }
                            else
                            {
                                Category category = categoryService.Get(selectedUserGroupId);
                                if (category == null)
                                    continue;
                                string categoryName = category.CategoryName;
                                userPrivacySpecifyObject.SpecifyObjectId = selectedUserGroupId;
                                userPrivacySpecifyObject.SpecifyObjectName = category.CategoryName;
                                userPrivacySpecifyObject.SpecifyObjectTypeId = SpecifyObjectTypeIds.Instance().UserGroup();
                            }
                            userPrivacySpecifyObjects.Add(userPrivacySpecifyObject);
                        }
                    }
                    specifyObjects.Add(privacyItem.ItemKey, userPrivacySpecifyObjects);
                }
                if (!canSave)
                    break;
            }
            if (canSave)
            {
                privacyService.UpdateUserPrivacySettings(userId, userSettings, specifyObjects);
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "更新成功");
            }
            else
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "更新失败");
            }
            return Redirect(SiteUrls.Instance().UserPrivacyItemsSettings(spaceKey));
        }

        /// <summary>
        /// 重置用户隐私设置
        /// </summary>
        /// <param name="spaceKey">空间</param>
        /// <returns>重置后的提示页面</returns>
        [HttpPost]
        public ActionResult RestoreUserPrivacySettings(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            privacyService.ClearUserPrivacySettings(userId);
            TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "重置用户隐私设置成功");
            return Redirect(SiteUrls.Instance().UserPrivacyItemsSettings(spaceKey));
        }

        #endregion

        #region 修改密码

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="spaceKey">被修改者名</param>
        /// <returns>修改用户密码的页面</returns>
        [HttpGet]
        public ActionResult ChangePassword(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("修改密码");
            return View(new ChangePasswordEditModel { UserName = spaceKey });
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="spaceKey">被修改者名</param>
        /// <param name="model">修改的信息</param>
        /// <returns>修改的结果</returns>
        [HttpPost]
        public ActionResult ChangePassword(string spaceKey, ChangePasswordEditModel model)
        {
            UserLoginStatus userLoginStatus = membershipService.ValidateUser(spaceKey, model.OldPassword);
            if (membershipService.ChangePassword(spaceKey, model.OldPassword, model.Password))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "密码修改成功"));
            }
            WebUtility.SetStatusCodeForError(Response);
            return Json(new StatusMessageData(StatusMessageType.Error, "修改密码失败"));
        }

        #endregion 修改密码

        #region 隐私私有方法

        /// <summary>
        /// 获取用户隐私设置
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="privacyItemKey">隐私设置标识</param>
        /// <param name="privacyStatus">默认状态</param>
        /// <returns>用户的默认设置</returns>
        private Dictionary<string, PrivacyStatus> GetUserPrivacySetting(long userId)
        {
            Dictionary<string, PrivacyStatus> userPrivacySettings = privacyService.GetUserPrivacySettings(userId);
            List<PrivacyItem> systemPrivacyItems = privacyService.GetPrivacyItems(null, null).ToList();
            Dictionary<string, PrivacyStatus> userPrivacySetting = new Dictionary<string, PrivacyStatus>();
            foreach (var item in systemPrivacyItems)
                userPrivacySetting[item.ItemKey] = userPrivacySettings.ContainsKey(item.ItemKey) ? userPrivacySettings[item.ItemKey] : item.PrivacyStatus;
            return userPrivacySetting;
        }

        /// <summary>
        /// 获取所有分组（我关注的所有人）的实体
        /// </summary>
        /// <returns>所有分组（我关注的所有人）的实体</returns>
        private UserPrivacySpecifyObject GetUserPrivacySpecifyObject_AllGroup()
        {
            UserPrivacySpecifyObject userPrivacySpecifyObject = UserPrivacySpecifyObject.New();

            userPrivacySpecifyObject.SpecifyObjectId = FollowSpecifyGroupIds.All;
            
            
            userPrivacySpecifyObject.SpecifyObjectName = "所有分组";
            userPrivacySpecifyObject.SpecifyObjectTypeId = SpecifyObjectTypeIds.Instance().UserGroup();
            return userPrivacySpecifyObject;
        }
        #endregion

        #region 用户分类管理
        /// <summary>
        /// 管理我的分类，根据租户类型id查找
        /// </summary>
        /// <param name="spaceKey">用户spaceKey</param>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns></returns>
        public ActionResult ManageMyCategories(string spaceKey, string tenantTypeId)
        {
            //取所有用户标签
            CategoryService categoryService = new CategoryService();
            IEnumerable<Category> categories = categoryService.GetOwnerCategories(UserContext.CurrentUser.UserId, tenantTypeId);

            pageResourceManager.InsertTitlePart("分类管理");

            return View(categories);
        }


        /// <summary>
        /// 创建用户分类页
        /// </summary>
        /// <returns></returns>
        public ActionResult _CreateMyCategory(string spaceKey, string tenantTypeId)
        {
            CategoryEditModel categoryEditModel = new CategoryEditModel();
            return View(categoryEditModel);
        }

        /// <summary>
        /// 创建用户分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreateMyCategory(string spaceKey, string tenantTypeId, CategoryEditModel categoryEditModel)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && !string.IsNullOrEmpty(categoryEditModel.CategoryName))
            {
                Category category = Category.New();
                category.CategoryName = categoryEditModel.CategoryName;
                category.OwnerId = currentUser.UserId;
                category.TenantTypeId = tenantTypeId;
                categoryService.Create(category);
                return Json(new { MessageType = StatusMessageType.Success, MessageContent = "创建成功！", CategoryId = category.CategoryId, CategoryName = category.CategoryName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "创建失败！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除我的用户分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteMyCategories(string spaceKey, IEnumerable<long> categoryIds)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && categoryIds.Count() > 0)
            {
                foreach (var categoryId in categoryIds)
                {
                    categoryService.ClearItemsFromCategory(categoryId);
                    categoryService.Delete(categoryId);
                }
                return Json(new { MessageType = StatusMessageType.Success, MessageContent = "删除成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 编辑我的用户分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditMyCategory(string spaceKey, CategoryEditModel categoryEditModel)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && categoryEditModel.CategoryId > 0 && !string.IsNullOrEmpty(categoryEditModel.CategoryName))
            {
                Category category = categoryService.Get(categoryEditModel.CategoryId);
                category.CategoryName = categoryEditModel.CategoryName;
                categoryService.Update(category);
                return Json(new { MessageType = StatusMessageType.Success, MessageContent = "更新成功！" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "更新失败！"), JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 编辑我的用户分类页
        /// </summary>
        public ActionResult _EditMyCategory(string spaceKey, long categoryId, string categoryName, string tenantTypeId = "")
        {
            CategoryEditModel categoryEditModel = new CategoryEditModel();
            categoryEditModel.CategoryId = categoryId;
            categoryEditModel.CategoryName = categoryName;
            return View(categoryEditModel);
        }
        #endregion

        #region 应用管理

        /// <summary>
        /// 应用管理
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public ActionResult ManageApplications(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("应用管理");
            var currentSpaceUser = userService.GetFullUser(spaceKey);
            if (currentSpaceUser == null)
                return HttpNotFound();

            var applications = new ApplicationService().GetInstalledApplicationsOfOwner(PresentAreaKeysOfBuiltIn.UserSpace, currentSpaceUser.UserId);

            var applicationService = new ApplicationService();

            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();

            foreach (var application in applications)
            {
                bool judge = applicationService.IsBuiltIn(PresentAreaKeysOfBuiltIn.UserSpace, application.ApplicationId);
                dictionary[application.ApplicationId] = judge;
            }
            ViewData["currentSpaceUser"] = currentSpaceUser;
            ViewData["dictionary"] = dictionary;
            return View(applications);
        }

        /// <summary>
        /// 卸载应用
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UnInstallApplication(string spaceKey, int applicationId)
        {
            var currentSpaceUser = userService.GetFullUser(spaceKey);
            if (currentSpaceUser == null)
            { return HttpNotFound(); }

            new ApplicationService().UnInstall(PresentAreaKeysOfBuiltIn.UserSpace, currentSpaceUser.UserId, applicationId);

            return Json(new StatusMessageData(StatusMessageType.Success, "卸载成功！"));
        }

        /// <summary>
        /// 添加新应用
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public ActionResult AddApplication(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("添加应用");
            var currentSpaceUser = userService.GetFullUser(spaceKey);
            if (currentSpaceUser == null)
                return HttpNotFound();
            var applicationService = new ApplicationService();
            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
            var applications = applicationService.GetAll(true).Where(n => applicationService.IsAvailable(PresentAreaKeysOfBuiltIn.UserSpace, n.ApplicationId));

            foreach (var application in applications)
            {
                bool judge = applicationService.IsInstalled(PresentAreaKeysOfBuiltIn.UserSpace, currentSpaceUser.UserId, application.ApplicationId);
                dictionary[application.ApplicationId] = judge;
            }

            ViewData["currentSpaceUser"] = currentSpaceUser;
            ViewData["dictionary"] = dictionary;
            return View(applications);
        }

        /// <summary>
        /// 安装应用
        /// </summary>   

        [HttpPost]
        public ActionResult InstallApplication(string spaceKey, int applicationId)
        {
            var currentSpaceUser = userService.GetFullUser(spaceKey);
            if (currentSpaceUser == null)
                return HttpNotFound();
            new ApplicationService().Install(PresentAreaKeysOfBuiltIn.UserSpace, currentSpaceUser.UserId, applicationId);

            return Json(new StatusMessageData(StatusMessageType.Success, "安装成功！"));
        }
        #endregion

        #region 导航管理

        /// <summary>
        /// 管理导航
        /// </summary>
        public ActionResult ManagePresentAreaNavigations(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("导航管理");
            var currentSpaceUser = userService.GetFullUser(spaceKey);
            if (currentSpaceUser == null)
                return HttpNotFound();

            var presentAreaNavigations = new NavigationService().GetRootPresentAreaNavigations(PresentAreaKeysOfBuiltIn.UserSpace, currentSpaceUser.UserId);

            ViewData["currentSpaceUser"] = currentSpaceUser;
            return View(presentAreaNavigations);
        }

        /// <summary>
        /// 删除导航
        /// </summary>
        public ActionResult DeletePresentAreaNavigation(string spaceKey, long id)
        {
            var currentSpaceUser = userService.GetFullUser(spaceKey);
            if (currentSpaceUser == null)
            { return HttpNotFound(); }

            new NavigationService().DeletePresentAreaNavigation(id);

            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        /// <summary>
        /// 创建编辑呈现区域导航(页面)
        /// </summary>
        public ActionResult _EditPresentAreaNavigation(string spaceKey, long? id)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            PresentAreaNavigationEditModel editModel = null;
            //创建
            if (id == null)
            {
                editModel = new PresentAreaNavigationEditModel();
                editModel.NavigationType = NavigationType.AddedByOwner;
                return View(editModel);
            }
            //编辑
            else
            {
                PresentAreaNavigation model = new NavigationService().GetPresentAreaNavigation(id.Value);
                return View(model.AsPresentAreaNavigationEditModel());
            }
        }

        /// <summary>
        /// 创建编辑呈现区域导航
        /// </summary>
        [HttpPost]
        public ActionResult _EditPresentAreaNavigation(string spaceKey, PresentAreaNavigationEditModel editModel)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            //创建
            if (editModel.Id == 0)
            {
                new NavigationService().CreatePresentAreaNavigation(editModel.AsPresentAreaNavigation());
                return Json(new StatusMessageData(StatusMessageType.Success, "创建成功！"));
            }
            //编辑
            else
            {
                new NavigationService().UpdatePresentAreaNavigation(editModel.AsPresentAreaNavigation());
                return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
            }
        }

        /// <summary>
        /// 更改显示顺序
        /// </summary>
        [HttpPost]
        public ActionResult ChangePresentAreaNavigationOrder(string spaceKey, int fromId, int toId)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            PresentAreaNavigation fromPan = new NavigationService().GetPresentAreaNavigation(fromId);
            PresentAreaNavigation toPan = new NavigationService().GetPresentAreaNavigation(toId);
            int temp = fromPan.DisplayOrder;
            fromPan.DisplayOrder = toPan.DisplayOrder;
            new NavigationService().UpdatePresentAreaNavigation(fromPan);
            toPan.DisplayOrder = temp;
            new NavigationService().UpdatePresentAreaNavigation(toPan);

            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }

        /// <summary>
        /// 重置
        /// </summary>
        [HttpPost]
        public ActionResult ResetPresentAreaNavigation(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            new NavigationService().ResetPresentAreaNavigations(PresentAreaKeysOfBuiltIn.UserSpace, user.UserId);

            return Json(new StatusMessageData(StatusMessageType.Success, "重置成功！"));
        }

        #endregion

        #region 帐号绑定

        /// <summary>
        /// 管理第三方帐号
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageAccountBindings(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);

            if (!new Authorizer().DeleteAccountBinding(user.UserId))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, SystemMessageViewModel.NoCompetence()));

            if (user == null)
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "没有找到用户",
                    StatusMessageType = StatusMessageType.Error
                }));
            pageResourceManager.InsertTitlePart("帐号绑定");
            return View(accountBindingService.GetAccountTypes(true));
        }

        /// <summary>
        /// 解除绑定第三方帐号
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <param name="accountTypeKey">解绑的第三方TypeKey</param>
        /// <returns>是否成功解除绑定</returns>
        [HttpPost]
        public ActionResult _CancelAccountBinding(string spaceKey, string accountTypeKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (!new Authorizer().DeleteAccountBinding(userId))
                return Json(new StatusMessageData(StatusMessageType.Error, "权限不足"));

            accountBindingService.DeleteAccountBinding(userId, accountTypeKey);
            return Json(new StatusMessageData(StatusMessageType.Success, "解除成功"));
        }

        /// <summary>
        /// 帐号绑定的状态
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <param name="accountTypeKey">第三方帐号的TypeKey</param>
        /// <returns>帐号绑定的状态</returns>
        [HttpGet]
        public ActionResult _AccountBinding(string spaceKey, string accountTypeKey)
        {
            User user = userService.GetFullUser(spaceKey);

            if (user == null || !new Authorizer().DeleteAccountBinding(user.UserId))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, SystemMessageViewModel.NoCompetence()));
            ViewData["AccountBinding"] = accountBindingService.GetAccountBinding(user.UserId, accountTypeKey);

            return View(accountBindingService.GetAccountType(accountTypeKey));
        }

        #endregion

        #region 关于管制

        /// <summary>
        /// 关于管制
        /// </summary>
        /// <returns></returns>
        public ActionResult UserModerated(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("关于管制");

            IUser user = userService.GetUser(spaceKey);
            PointSettings pointSettings = pointSettingsManger.Get();
            int totalPoints = pointSettings.CalculateIntegratedPoint(user.ExperiencePoints, user.ReputationPoints);

            UserSettings userSettings = userSettingsManager.Get();

            //应用的权限项
            IEnumerable<PermissionItem> permissionItems = null;
            ApplicationService applicationService = new ApplicationService();
            PermissionService permissionService = new PermissionService();

            //查出所有应用
            IEnumerable<ApplicationBase> applicationBases = applicationService.GetAll(true);

            if (applicationBases == null)
                return null;

            //根据应用ID获取出该应用的权限项
            foreach (var applicationBase in applicationBases)
            {
                permissionItems = permissionService.GetPermissionItems(applicationBase.ApplicationId);
                ViewData["permissionItem_" + applicationBase.ApplicationKey] = permissionItems;
            }
            //获取以权限key为键，一条记录为值的字典集合
            Dictionary<string, PermissionItemInUserRole> dicModeratedUserRoles = new Dictionary<string, PermissionItemInUserRole>();
            Dictionary<string, PermissionItemInUserRole> dicRegisteredUsersRoles = new Dictionary<string, PermissionItemInUserRole>();

            //根据角色名称获取权限与角色对应表的相应记录
            IEnumerable<PermissionItemInUserRole> ModeratedUserRoles = permissionService.GetPermissionItemsInUserRole("ModeratedUser");
            IEnumerable<PermissionItemInUserRole> RegisteredUsersRoles = permissionService.GetPermissionItemsInUserRole("RegisteredUsers");

            foreach (var ModeratedUserRole in ModeratedUserRoles)
            {
                string key = ModeratedUserRole.ItemKey;
                dicModeratedUserRoles[key] = ModeratedUserRole;
            }
            foreach (var RegisteredUsersRole in RegisteredUsersRoles)
            {
                string key = RegisteredUsersRole.ItemKey;
                dicRegisteredUsersRoles[key] = RegisteredUsersRole;
            }

            ViewData["totalPoints"] = totalPoints;

            ViewData["noModeratedUserPoint"] = userSettings.NoModeratedUserPoint;

            ViewData["dicModeratedUserRoles"] = dicModeratedUserRoles;

            ViewData["dicRegisteredUsersRoles"] = dicRegisteredUsersRoles;

            ViewData["userItem"] = user;
            return View(applicationBases);
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

    #region UserSettingsMenu

    /// <summary>
    /// UserSettingsMenu
    /// </summary>
    public enum UserSettingsMenu
    {
        /// <summary>
        /// 基本资料
        /// </summary>
        UserProfile = 0,

        /// <summary>
        /// 教育经历
        /// </summary>
        UserEducation = 1,

        /// <summary>
        /// 工作经历
        /// </summary>
        UserWork = 2,

        /// <summary>
        /// 个人标签
        /// </summary>
        UserTag = 3,

        /// <summary>
        /// 修改头像
        /// </summary>
        UserAvatar = 4,

        /// <summary>
        /// 更改密码
        /// </summary>
        ChangePassword = 5,

        /// <summary>
        /// 账户绑定
        /// </summary>
        UserAccount = 6,

        /// <summary>
        /// 隐私通用设置
        /// </summary>
        PrivacyUniversal = 7,

        /// <summary>
        /// 屏蔽设置
        /// </summary>
        Blocked = 8,

        /// <summary>
        /// 黑名单
        /// </summary>
        UserBlacklist = 9,

        /// <summary>
        /// 应用管理
        /// </summary>
        ManageApplications=10,

        /// <summary>
        /// 添加应用
        /// </summary>
        AddApplications=11,

        /// <summary>
        /// 导航管理
        /// </summary>
        ManageNavigations=12,
    }

    #endregion UserSettingsMenu
}