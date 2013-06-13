//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.UI;
using Tunynet.Utilities;
using Tunynet.Globalization;
using Tunynet.Common.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using Tunynet.Events;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 后台用户控制面板Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = false)]
    [TitleFilter(IsAppendSiteName = true, TitlePart = "后台管理")]
    [ManageAuthorize(RequireSystemAdministrator = true)]
    public class ControlPanelUserController : Controller
    {
        #region private items

        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private IMembershipService iMembershipService = DIContainer.Resolve<IMembershipService>();
        private RoleService roleService = DIContainer.Resolve<RoleService>();
        private UserRankService userRankService = new UserRankService();
        private IdentificationService identificationService = new IdentificationService();
        private PointService pointService = new PointService();
        private SystemDataService systemDataService = new SystemDataService();
        private Authorizer authorizer = new Authorizer();

        #endregion

        /// <summary>
        /// 用户管理首页
        /// </summary>
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        #region 用户管理



        /// <summary>
        /// 用户管理
        /// </summary>
        
        [HttpGet]
        public ActionResult ManageUsers(int? pageIndex, int pageSize = 30)
        {
            pageResourceManager.InsertTitlePart("用户管理");

            #region 组装搜索条件

            
            UserQuery query = new UserQuery();
            query.Keyword = Request.QueryString.GetString("uname", string.Empty);
            query.AccountEmailFilter = Request.QueryString.GetString("uemail", string.Empty);
            query.RoleName = Request.QueryString.GetString("RoleName", string.Empty);

            if (!string.IsNullOrEmpty(Request.QueryString["IsActivated"]))
                query.IsActivated = Request.QueryString.GetBool("IsActivated", false);

            if (!string.IsNullOrEmpty(Request.QueryString["IsModerated"]))
                query.IsModerated = Request.QueryString.GetBool("IsModerated", false);

            if (!string.IsNullOrEmpty(Request.QueryString["IsBanned"]))
                query.IsBanned = Request.QueryString.GetBool("IsBanned", false);

            
            int result = 0;
            if (int.TryParse(Request.QueryString["rankstart"], out result))
                query.UserRankLowerLimit = Request.QueryString.GetInt("rankstart", 0);

            if (int.TryParse(Request.QueryString["rankend"], out result))
                query.UserRankUpperLimit = Request.QueryString.GetInt("rankend", 0);

            if (Request.QueryString.Get<DateTime>("startdate") != DateTime.MinValue)
                query.RegisterTimeLowerLimit = Request.QueryString.Get<DateTime>("startdate");

            if (Request.QueryString.Get<DateTime>("enddate") != DateTime.MinValue)
                query.RegisterTimeUpperLimit = Request.QueryString.Get<DateTime>("enddate");

            #endregion

            #region 组装搜索下拉列表

            IEnumerable<Role> roles = roleService.GetRoles();
            if (roles != null)
            {
                ViewData["RoleName"] = new SelectList(roles, "RoleName", "FriendlyRoleName", query.RoleName);
            }

            Dictionary<bool, string> activatedValues = new Dictionary<bool, string> { { true, "已激活" }, { false, "未激活" } };
            ViewData["IsActivated"] = new SelectList(activatedValues.Select(n => new { text = n.Value, value = n.Key.ToString().ToLower() }), "value", "text", query.IsActivated);

            Dictionary<bool, string> moderatedValues = new Dictionary<bool, string> { { true, "管制" }, { false, "未管制" } };
            ViewData["IsModerated"] = new SelectList(moderatedValues.Select(n => new { text = n.Value, value = n.Key.ToString().ToLower() }), "value", "text", query.IsModerated);

            Dictionary<bool, string> bannedValues = new Dictionary<bool, string> { { true, "封禁" }, { false, "未封禁" } };
            ViewData["IsBanned"] = new SelectList(bannedValues.Select(n => new { text = n.Value, value = n.Key.ToString().ToLower() }), "value", "text", query.IsBanned);

            #endregion
            pageIndex = pageIndex ?? 1;
            PagingDataSet<User> users = userService.GetUsers(query, pageSize, pageIndex.Value);
            if (pageIndex > 1 && (users == null || users.Count() == 0))
            {
                users = userService.GetUsers(query, pageSize, pageIndex.Value - 1);
                var dd = Request.Url.Query;
            }

            return View(users);
        }

        /// <summary>
        /// 添加用户控件
        /// </summary>
        [HttpGet]
        public ActionResult _CreateUser()
        {
            ManageUsersCreateEditModel model = new ManageUsersCreateEditModel();
            return View(model);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        [HttpPost]
        public ActionResult _CreateUser(ManageUsersCreateEditModel model)
        {
            User user = model.AsUser();
            UserCreateStatus status = UserCreateStatus.UnknownFailure;
            user.IsActivated = true;

            IUser createdUser = iMembershipService.CreateUser(user, model.Password, out status);
            UserProfile profile = UserProfile.New();
            profile.UserId = createdUser.UserId;
            new UserProfileService().Create(profile);
            StatusMessageData statusMessageData = null;
            switch (status)
            {
                case UserCreateStatus.DisallowedUsername:
                    WebUtility.SetStatusCodeForError(Response);
                    statusMessageData = new StatusMessageData(StatusMessageType.Error, "对不起，您输入的帐号禁止使用，请输入其他名称");
                    break;
                case UserCreateStatus.DuplicateEmailAddress:
                    WebUtility.SetStatusCodeForError(Response);
                    statusMessageData = new StatusMessageData(StatusMessageType.Error, "对不起，您输入的电子邮箱地址已经被使用，请输入其他邮箱");
                    break;
                case UserCreateStatus.DuplicateUsername:
                    WebUtility.SetStatusCodeForError(Response);
                    statusMessageData = new StatusMessageData(StatusMessageType.Error, "对不起，您输入的帐号已经被使用，请输入其他名称");
                    break;
                case UserCreateStatus.InvalidPassword:
                    WebUtility.SetStatusCodeForError(Response);
                    statusMessageData = new StatusMessageData(StatusMessageType.Error, "您的密码长度少于本站要求的最小密码长度，请重新输入");
                    break;
                case UserCreateStatus.UnknownFailure:
                    WebUtility.SetStatusCodeForError(Response);
                    statusMessageData = new StatusMessageData(StatusMessageType.Error, "对不起，注册新用户的时候产生了一个未知错误");
                    break;
                default:
                    break;
            }
            ViewData["statusMessageData"] = statusMessageData;
            if (statusMessageData == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "创建成功！"));
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// 批量激活、取消激活
        /// </summary>
        /// <param name="isActivated">是否激活</param>
        [HttpPost]
        public ActionResult ActivatedUsers(bool isActivated)
        {
            
            IEnumerable<long> uids = Request.Form.Gets<long>("CheckBoxGroup");

            string returnUrl = Request.Form.Get<string>("returnUrl");
            if (uids != null && uids.Count() > 0)
            {
                iMembershipService.ActivateUsers(uids, isActivated);
            }

            return Redirect(returnUrl);
        }

        /// <summary>
        /// 批量取消封禁
        /// </summary>
        [HttpPost]
        public ActionResult UnbanUsers(string returnUrl)
        {
            IEnumerable<long> uids = Request.Form.Gets<long>("CheckBoxGroup");
            if (uids != null && uids.Count() > 0)
            {
                foreach (long Id in uids)
                {
                    if (!authorizer.User_Manage(Id))
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有权限对此用户操作"));
                    }
                    try
                    {
                        userService.UnbanUser(Id);
                    }
                    catch { }
                }
            }

            return Redirect(returnUrl);
        }

        /// <summary>
        /// 封禁用户控件
        /// </summary>
        [HttpGet]
        public ActionResult _BannedUser()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string[] reasons = { "亵渎", "广告", "发垃圾广告", "恶意攻击他人", "不雅的用户名", "恶意的链接或签名档", "其他" };
            for (int i = 0; i < reasons.Count(); i++)
            {
                items.Add(new SelectListItem { Value = i.ToString(), Text = reasons.ElementAt(i) });
            }
            ViewData["reasons"] = ViewData["reasons"] = new SelectList(items, "Value", "Text");
            return View();
        }

        /// <summary>
        /// 批量封禁用户
        /// </summary>
        [HttpPost]
        public ActionResult BannedUsers()
        {
            if (Request.Form["BanDeadline"] == null || string.IsNullOrEmpty(Request.Form["BanReason"]))
            {
                WebUtility.SetStatusCodeForError(Response);
                ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "对不起，截止日期或封禁原因不能为空");
                return View("_BannedUser");
            }
            DateTime result = DateTime.UtcNow.AddDays(1);
            if (!DateTime.TryParse(Request.Form["BanDeadline"], out result))
            {
                WebUtility.SetStatusCodeForError(Response);
                ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "对不起，截止日期不是日期类型");
                return View("_BannedUser");
            }

            DateTime banDay = result;
            string banReason = Request.Form["BanReason"];
            string selectedUserIDsString = Request.Form["UserIds"];

            IEnumerable<long> uids = Request.Form.Gets<long>("UserIds");
            if (uids != null && uids.Count() > 0)
            {
                foreach (long Id in uids)
                {
                    if (!authorizer.User_Manage(Id))
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有权限对此用户操作"));
                    }
                    try
                    {
                        userService.BanUser(Id, result, banReason);
                    }
                    catch { }
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        /// <summary>
        /// 删除用户控件
        /// </summary>
        [HttpGet]
        public ActionResult _DeleteUser(long? userId)
        {
            IUser user = null;
            if (userId.HasValue && userId.Value > 0)
                user = userService.GetUser(userId.Value);

            ViewData["userId"] = userId;
            return View(user);
        }

        /// <summary>
        /// 单个删除用户
        /// </summary>
        [HttpPost]
        public ActionResult DeleteUser()
        {
            bool isReassign = Request.Form.GetBool("isReassign", true);

            
            string reassignUserName = Request.Form.GetString("reassignUserName", string.Empty);

            if (isReassign)
            {
                if (string.IsNullOrEmpty(reassignUserName))
                {

                    return Json(new StatusMessageData(StatusMessageType.Error, "对不起，转让用户为空"));
                }
                else
                {
                    IUser changeUser = userService.GetUser(reassignUserName);

                    if (changeUser == null)
                    {

                        return Json(new StatusMessageData(StatusMessageType.Error, "对不起，转让的用户不存在"));
                    }
                    else if (!authorizer.User_Manage(changeUser.UserId))
                    {

                        return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有权限删除此用户"));
                    }
                }
                reassignUserName = reassignUserName.Trim();
            }

            long userId = Request.Form.Get<long>("SingleUserId", 0);

            if (!authorizer.User_Manage(userId))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有权限删除此用户"));
            }
            UserDeleteStatus DelStatus;

            
            //另外isReassign修改为isTakeOver与业务逻辑保持一致
            //要确保reassignUserName不为空
            DelStatus = iMembershipService.DeleteUser(userId, reassignUserName, isReassign);

            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        [HttpPost]
        public ActionResult DeleteUsers()
        {
            bool isReassign = Request.Form.GetBool("isReassign", false);
            string reassignUserName = Request.Form.GetString("reassignUserName", string.Empty);

            if (isReassign)
            {
                if (string.IsNullOrEmpty(reassignUserName))
                {

                    return Json(new StatusMessageData(StatusMessageType.Error, "对不起，转让用户为空"));
                }
                else
                {
                    IUser changeUser = userService.GetUser(reassignUserName);

                    if (changeUser == null)
                    {

                        return Json(new StatusMessageData(StatusMessageType.Error, "对不起，转让的用户不存在"));
                    }
                    else if (!authorizer.User_Manage(changeUser.UserId))
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有权限删除此用户"));
                    }
                }
                reassignUserName = reassignUserName.Trim();
            }

            UserDeleteStatus DelStatus;
            IEnumerable<long> uids = Request.Form.Gets<long>("UserIds");
            if (uids != null && uids.Count() > 0)
            {
                foreach (long userId in uids)
                {
                    if (!authorizer.User_Manage(userId))
                    {
                        return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有权限删除此用户"));
                    }
                    try
                    {
                        
                        DelStatus = iMembershipService.DeleteUser(userId, reassignUserName, isReassign);
                    }
                    catch { }
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        /// <summary>
        /// 编辑用户资料
        /// </summary>
        [HttpGet]
        public ActionResult EditUser(long userId)
        {
            pageResourceManager.InsertTitlePart("后台管理-编辑用户资料");
            ManageUsersEditModel model = null;
            string selectedValue = "0";
            User user = userService.GetUser(userId) as User;
            if (user != null)
                model = user.AsEditModel();
            ViewData["statusMessageData"] = TempData["statusMessage"];
            List<SelectListItem> items = new List<SelectListItem>();
            string[] reasons = { "亵渎", "广告", "发垃圾广告", "恶意攻击他人", "不雅的用户名", "恶意的链接或签名档", "其他" };
            for (int i = 0; i < reasons.Count(); i++)
            {
                items.Add(new SelectListItem { Value = i.ToString(), Text = reasons.ElementAt(i) });
            }

            if (reasons.Contains(user.BanReason))
            {
                selectedValue = items.Where(n => n.Text == user.BanReason).FirstOrDefault().Value;
            }
            else
            {
                selectedValue = (reasons.Count() - 1).ToString();
            }
            ViewData["reasons"] = new SelectList(items, "Value", "Text", selectedValue);
            return View(model);
        }

        /// <summary>
        /// 编辑用户资料
        /// </summary>
        /// <param name="model">ManageUsersEditModel</param>
        [HttpPost]
        public ActionResult EditUser(ManageUsersEditModel model)
        {
            pageResourceManager.InsertTitlePart("后台管理-编辑用户资料");
            if (!authorizer.User_Manage(model.UserId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有权限修改此用户"));

            string mess = "";
            bool result = ValidateEmail(model.AccountEmail, model.UserId, out mess);

            if (!result)
            {
                return Json(new StatusMessageData(StatusMessageType.Hint, WebUtility.HtmlDecode(mess)));
                //ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, WebUtility.HtmlDecode(mess));
                //return View(model);
            }

            //判断管理员是否解除了编辑用户的管制状态
            if (model.Moderated == 0)
            {
                User editedUser = userService.GetFullUser(model.UserId);
                if (editedUser != null && editedUser.IsModerated)
                    userService.NoModeratedUser(model.UserId);
            }
            User user = model.AsUserForEditUser();
            UserProfile profile = user.Profile;
            profile.Mobile = model.Mobile ?? string.Empty;
            new UserProfileService().Update(profile);

            iMembershipService.UpdateUser(user);

            return Json(new StatusMessageData(StatusMessageType.Success, "修改用户资料成功"));
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        ///<param name="returnUrl">用于会跳的Url</param>
        [HttpGet]
        public ActionResult _SetUserRoles(long userId, string returnUrl)
        {
            IEnumerable<Role> AllRoles = roleService.GetRoles();
            IEnumerable<Role> UserRoles = roleService.GetRolesOfUser(userId);
            IEnumerable<Role> roles = null;
            if (AllRoles != null && UserRoles != null)
            {

                IUser currentUser = UserContext.CurrentUser;
                IEnumerable<Role> rolesOfUsers = roleService.GetRolesOfUser(currentUser.UserId);

                long FounderId = systemDataService.GetLong("Founder");

                if (FounderId == currentUser.UserId)
                {
                    roles = AllRoles;
                }
                else if (authorizer.IsSuperAdministrator(currentUser))
                {
                    roles = AllRoles.Where(n => n.RoleName != "SuperAdministrator");
                }
                else
                {
                    roles = AllRoles.Where(n => n.RoleName != "SuperAdministrator" || n.RoleName != "ContentAdministrator");

                }
            }

            ViewData["UserRoles"] = UserRoles;
            ViewData["userId"] = userId;
            ViewData["userName"] = userService.GetUser(userId).UserName;
            ViewData["returnUrl"] = returnUrl;

            return View(roles);
        }

        /// <summary>
        /// 用户密码的Get方法
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ResetUserPassword(long userId)
        {
            ViewData["userId"] = userId;
            ViewData["userName"] = userService.GetUser(userId).UserName;
            return View();
        }

        /// <summary>
        /// 重设用户密码的Post方法
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ResetUserPassword(long userId, string password)
        {
            if (!authorizer.User_Manage(userId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有权限修改此用户"));

            string name = userService.GetUser(userId).UserName;
            if (iMembershipService.ResetPassword(name, password))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "重设密码成功！"));
            }
            return Json(new StatusMessageData(StatusMessageType.Error, "重设密码失败！"));

        }

        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="returnUrl"></param>
        [HttpPost]
        public ActionResult UpdateUserRoles(long userId, string returnUrl)
        {
            if (!authorizer.User_Manage(userId))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有权限对此用户操作"));
            }
            IEnumerable<string> strRoles = null;
            if (Request.Form.Gets<string>("RoleName") == null)
            {
            }
            else
            {
                strRoles = Request.Form.Gets<string>("RoleName").Where(n => !n.Equals("false", StringComparison.CurrentCultureIgnoreCase));
            }

            roleService.RemoveUserRoles(userId);
            if (strRoles != null)
            {
                IUser currentUser = UserContext.CurrentUser;
                IEnumerable<Role> rolesOfUsers = roleService.GetRolesOfUser(currentUser.UserId);

                long FounderId = systemDataService.GetLong("Founder");

                if (currentUser.UserId == FounderId)
                {
                    roleService.AddUserToRoles(userId, strRoles.ToList());
                    return Redirect(returnUrl);
                }
                else if (authorizer.IsSuperAdministrator(currentUser))
                {
                    if (strRoles.Where(n => n.ToString() == RoleNames.Instance().SuperAdministrator()).Count() > 0 ? false : true)
                    {
                        roleService.AddUserToRoles(userId, strRoles.ToList());
                        return Redirect(returnUrl);
                    }
                }
            }
            return Json(new StatusMessageData(StatusMessageType.Error, "对不起，您没有权限执行此操作"));
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email">待验证的邮箱</param>
        /// <param name="userId">用户Id</param>
        /// <param name="errorMessage">输出出错信息</param>
        /// <returns>是否通过验证</returns>
        public bool ValidateEmail(string email, long userId, out string errorMessage)
        {
            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();

            if (string.IsNullOrEmpty(email))
            {
                errorMessage = ResourceAccessor.GetString("Validate_EmailRequired");
                return false;
            }

            Regex regex = new Regex(userSettings.EmailRegex, RegexOptions.ECMAScript);
            if (!regex.IsMatch(email))
            {
                errorMessage = ResourceAccessor.GetString("Validate_EmailStyle");
                return false;
            }

            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.FindUserByEmail(email);

            if (user != null)
            {
                //验证email是否已经存在
                if (userId != user.UserId)
                {
                    errorMessage = "对不起,您输入的帐号邮箱已被使用";
                    return false;
                }
            }
            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// 用户管理右侧
        /// </summary>
        [HttpGet]
        public ActionResult _ManageUserRightMenu()
        {
            Dictionary<UserManageableCountType, int> counts = userService.GetManageableCounts(false, true, true);
            return View(counts);
        }
        /// <summary>
        /// 批量奖惩用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _RewardUsers(long? userId = null)
        {
            IEnumerable<long> userIds = Request.QueryString.Gets<long>("CheckBoxGroup", new List<long>());
            if (userId.HasValue)
            {
                //userIds.Union(new List<long> { userId.Value });
                List<long> tempList = userIds.ToList();
                tempList.Add(userId.Value);
                userIds = tempList.AsEnumerable();
            }
            ViewData["userIds"] = userIds;

            pageResourceManager.InsertTitlePart("奖惩用户");
            PointCategory experiencePoints = pointService.GetPointCategory(PointCategoryKeys.Instance().ExperiencePoints());
            PointCategory reputationPoints = pointService.GetPointCategory(PointCategoryKeys.Instance().ReputationPoints());
            PointCategory tradePoints = pointService.GetPointCategory(PointCategoryKeys.Instance().TradePoints());
            ViewData["experiencePoints"] = experiencePoints;
            ViewData["reputationPoints"] = reputationPoints;
            ViewData["tradePoints"] = tradePoints;
            return View();
        }
        /// <summary>
        /// 批量奖惩用户
        /// </summary>
        /// <param name="description">理由</param>
        /// <param name="experiencePoints">经验积分</param>
        /// <param name="reputationPoints">威望积分</param>
        /// <param name="tradePoints">交易积分</param>
        /// <param name="isIncome">是否收入</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _RewardUsers(string description, int experiencePoints, int reputationPoints, int tradePoints, bool isIncome)
        {
            var userNames = Request.Form.Get<string>("NickName", null);
            var userNameArr = userNames.TrimEnd('，').Split('，');
            List<long> idList = new List<long>();
            foreach (var name in userNameArr)
            {
                var user = userService.GetUserByNickName(name);
                if (user == null)
                    continue;
                idList.Add(user.UserId);
            }
            IEnumerable<long> uids = Request.Form.Gets<long>("UserId");
            if (uids != null && uids.Count() > 0)
            {
                idList.AddRange(uids);
            }
            if (idList != null && idList.Count() > 0)
            {
                foreach (var userId in idList)
                {
                    if (authorizer.User_Manage(userId))
                    {
                        UserService userService = new UserService();
                        userService.RewardAndPunishment(userId, description, experiencePoints, reputationPoints, tradePoints, isIncome);
                    }
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
            }
            return Json(new StatusMessageData(StatusMessageType.Error, "操作失败，可能您没有权限执行这个操作"));
        }
        #endregion

        #region 角色管理

        /// <summary>
        /// 用户角色管理
        /// </summary>
        [HttpGet]
        public ActionResult ManageUserRoles(int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("后台管理-用户角色管理");

            IEnumerable<Role> roles = roleService.GetRoles();

            return View(roles);
        }

        /// <summary>
        /// 添加角色控件
        /// </summary>
        [HttpGet]
        public ActionResult _CreateRole(string roleName)
        {
            RoleEditModel model = new RoleEditModel();
            if (!string.IsNullOrEmpty(roleName))
            {
                Role role = new Role();
                role = roleService.Get(roleName);
                if (role != null)
                    model = role.AsEditModel();
            }

            EditUserRole_GenerateFormItem();

            return View(model);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        [HttpPost]
        public ActionResult CreateRole(RoleEditModel model)
        {

            Stream stream = null;
            HttpPostedFileBase roleImage = Request.Files["RoleImage"];
            Role role = roleService.Get(model.RoleName);
            if (roleImage != null && !string.IsNullOrEmpty(roleImage.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Role());
                if (!tenantLogoSettings.ValidateFileLength(roleImage.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return View(model);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                if (!logoSettings.ValidateFileExtensions(roleImage.FileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return View(model);
                }

                stream = roleImage.InputStream;
                model.RoleImage = roleImage.FileName;
            }
            else        //当取不到上传的图片文件名时RoleImage值保持不变
            {
                model.RoleImage = role != null ? role.RoleImage : string.Empty;
            }

            if (ModelState.IsValid)
            {
                if (role != null)
                {
                    WebUtility.SetStatusCodeForError(Response);
                    EditUserRole_GenerateFormItem();
                    model.RoleName = string.Empty;

                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "角色名已存在！");

                    return View("_CreateRole", model);
                }

                role = model.AsRole();
                roleService.Create(role, stream);
            }

            return RedirectToAction("ManageUsers");
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        [HttpPost]
        public ActionResult EditRole(RoleEditModel model)
        {
            Stream stream = null;
            HttpPostedFileBase roleImage = Request.Files["RoleImage"];
            Role role = roleService.Get(model.RoleName);
            if (roleImage != null && !string.IsNullOrEmpty(roleImage.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Role());
                if (!tenantLogoSettings.ValidateFileLength(roleImage.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return View(model);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                if (!logoSettings.ValidateFileExtensions(roleImage.FileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return View(model);
                }

                stream = roleImage.InputStream;
                model.RoleImage = roleImage.FileName;
            }
            else        //当取不到上传的图片文件名时RoleImage值保持不变
            {
                model.RoleImage = role != null ? role.RoleImage : string.Empty;
            }
            if (model != null && !string.IsNullOrEmpty(model.RoleName))
            {
                if (role != null)
                {
                    role = model.AsRole();
                    roleService.Update(role, stream);
                }
            }

            return RedirectToAction("ManageUsers");
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleName">roleName</param>
        [HttpPost]
        public JsonResult DeleteRole(string roleName)
        {
            roleService.Delete(roleName);
            if (!string.IsNullOrEmpty(roleName))
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }


        #region Private Method

        /// <summary>
        /// 编辑用户角色时生成表单项
        /// </summary>
        private void EditUserRole_GenerateFormItem()
        {
            ApplicationService app = new ApplicationService();
            IEnumerable<ApplicationBase> apps = app.GetAll();

            List<SelectListItem> selectItems = new List<SelectListItem>() { new SelectListItem { Text = "未设置", Value = "0" } };
            selectItems.AddRange(apps.Select(n => new SelectListItem { Text = n.Config.ApplicationName, Value = n.ApplicationId.ToString() }).ToList());

            ViewData["apps"] = new SelectList(selectItems, "value", "text");
        }
        #endregion

        #endregion

        #region 用户等级
        /// <summary>
        /// 等级管理
        /// </summary>
        /// <returns>等级管理页面</returns>
        [HttpGet]
        public ActionResult ManageRanks()
        {
            pageResourceManager.InsertTitlePart("等级管理");
            
            
            
            
            SortedList<int, UserRank> userRanks = userRankService.GetAll();
            
            
            return View(userRanks);
        }
        
        
        /// <summary>
        /// 编辑一个用户等级
        /// </summary>
        /// <returns>编辑用户等级</returns>
        [HttpGet]
        public ActionResult _EditUserRank(int rank = -1, bool isEdit = false)
        {
            UserRank userRank = userRankService.Get(rank);
            UserRankEditModel userRankEditModel = new UserRankEditModel();
            if (userRank != null)
                userRankEditModel = userRank.AsEditModel();
            userRankEditModel.IsEdit = isEdit;
            return View(userRankEditModel);
        }

        /// <summary>
        /// 处理添加或编辑一个用户等级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditUserRank(UserRankEditModel userRankEditModel)
        {
            if (userRankEditModel == null || !ModelState.IsValid)
                return View();
            UserRank userRank = userRankEditModel.AsUserRank();
            bool isCreated = true;
            if (userRankEditModel.IsEdit)
                userRankService.Update(userRank);
            else
            {
                UserRank rank = userRankService.Get(userRankEditModel.Rank ?? 0);
                if (rank != null)
                {
                    WebUtility.SetStatusCodeForError(Response);
                    ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "已经存在的用户等级");
                    return View(userRankEditModel);
                }
                isCreated = userRankService.Create(userRank);
                if (!isCreated)
                {
                    WebUtility.SetStatusCodeForError(Response);
                    ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "创建失败");
                    return View(userRankEditModel);
                }
            }
            return View(userRankEditModel);
        }
        
        /// <summary>
        /// 删除一个用户等级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUserRank(int rank)
        {
            userRankService.Delete(rank);
            return Redirect(SiteUrls.Instance().ManageRanks());
        }

        
        /// <summary>
        /// 根据当前等级规则重新计算所有用户等级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetAllUser()
        {
            userRankService.ResetAllUser();
            return Json(new StatusMessageData(StatusMessageType.Success, "操作完成"));
        }

        #endregion

        #region 认证标识管理

        /// <summary>
        /// 认证标识管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageIdentificationTypes()
        {
            //获得所有的认证标识
            IEnumerable<IdentificationType> identificationTypes = identificationService.GetIdentificationTypes(null);
            return View(identificationTypes);
        }

        /// <summary>
        /// 添加编辑标识模态框
        /// </summary>
        /// <param name="identificationTypeId">如果Id为0则为添加标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditIdentificationType(long identificationTypeId = 0)
        {
            //如果Id为0则为添加标识
            if (identificationTypeId == 0)
            {
                return View();
            }

            //获得所有标识
            IdentificationType identificationType = identificationService.GetIdentificationTypes(null).SingleOrDefault(n => n.IdentificationTypeId == identificationTypeId);

            return View(identificationType.AsEditModel());
        }

        /// <summary>
        /// 添加编辑标识
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditIdentificationType(IdentificationTypeEditModel editModel)
        {
            string fileName = string.Empty;
            Stream stream = null;

            //获取上传图片
            HttpPostedFileBase logo = Request.Files["IdentificationTypeLogo"];

            //如果上传图片不为空则校验其扩展名和大小 
            if (logo != null && logo.ContentLength > 0)
            {
                fileName = logo.FileName;

                //校验附件的扩展名
                ILogoSettingsManager logoSettingsManager = DIContainer.Resolve<ILogoSettingsManager>();
                LogoSettings logoSettings = logoSettingsManager.Get();
                if (!logoSettings.ValidateFileExtensions(fileName))
                {
                    return Content(WarpJsonMessage(new StatusMessageData(StatusMessageType.Error, "只允许上传后缀名为 " + logoSettings.AllowedFileExtensions.TrimEnd(',') + " 的文件")));
                }

                //校验附件的大小
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Identification());
                if (!tenantLogoSettings.ValidateFileLength(logo.ContentLength))
                {
                    return Content(WarpJsonMessage(new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}KB", tenantLogoSettings.MaxLogoLength))));
                }

                stream = logo.InputStream;
            }

            //如果IdentificationTypeId大于0则为编辑标识
            if (editModel.IdentificationTypeId > 0)
            {
                IdentificationType identificationType = editModel.AsIdentificationType();
                identificationService.UpdateIdentificationType(identificationType, stream);
            }
            //否则为创建标识
            else
            {
                if (logo == null || logo.ContentLength == 0)
                {
                    return Content(WarpJsonMessage(new StatusMessageData(StatusMessageType.Error, "图片不能为空！")));
                }
                IdentificationType identificationType = editModel.AsIdentificationType();
                identificationService.CreateIdentificationType(identificationType, stream);
            }
            return Content(WarpJsonMessage(new StatusMessageData(StatusMessageType.Success, "操作成功！")));
        }

        /// <summary>
        /// 包裹消息
        /// </summary>
        /// <remarks>用于解决ajaxform在IE下异步上传文件时，返回不了json数据问题</remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        private string WarpJsonMessage(StatusMessageData data)
        {
            return System.Web.Helpers.Json.Encode(data);
        }

        /// <summary>
        /// 删除标识
        /// </summary>
        /// <param name="identificationTypeId">标识Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteIdentificationType(long identificationTypeId)
        {
            //删除该标识和该标识下的所有实体
            if (identificationService.DeleteIdentificationType(identificationTypeId))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
            }

            return Json(new StatusMessageData(StatusMessageType.Error, "操作失败！"));
        }
        #endregion

        #region 认证申请管理
        /// <summary>
        /// 认证申请管理页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageIdentifications(IdentificationQuery query, string userId, int pageSize = 20, int pageIndex = 1)
        {
            //标识类型下拉列表
            IEnumerable<IdentificationType> identificationTypes = identificationService.GetIdentificationTypes(null);
            SelectList identificationTypeSelectList = new SelectList(identificationTypes.Select(n => new { text = n.Name, value = n.IdentificationTypeId }), "value", "text");
            ViewData["typeSelectList"] = identificationTypeSelectList;

            //获得所有的认证标识
            Dictionary<long, string> identificationTypesDic = identificationTypes.ToDictionary(n => n.IdentificationTypeId, n => n.Name);
            ViewData["identificationTypesDic"] = identificationTypesDic;

            //处理userid为query.UserId赋值
            if (!string.IsNullOrEmpty(userId))
            {
                if (!string.IsNullOrEmpty(userId.Trim(',')))
                {
                    query.UserId = long.Parse(userId.Trim(','));
                }
            }

            //用户选择器默认值
            ViewData["userId"] = query.UserId;

            PagingDataSet<Identification> identifications = identificationService.GetIdentifications(query, pageIndex, pageSize);
            return View(identifications);
        }

        /// <summary>
        /// 查看认证申请详情
        /// </summary>
        /// <param name="identificationId">认证申请Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ViewIdentification(long identificationId)
        {
            //获得所有的认证标识            
            IEnumerable<IdentificationType> identificationTypes = identificationService.GetIdentificationTypes(null);
            Dictionary<long, string> identificationTypesDic = identificationTypes.ToDictionary(n => n.IdentificationTypeId, n => n.Name);
            ViewData["identificationTypesDic"] = identificationTypesDic;

            //根据认证申请Id获得认证申请实体
            Identification identification = identificationService.GetIdentification(identificationId);
            return View(identification);
        }

        /// <summary>
        /// 处理身份认证
        /// </summary>
        /// <param name="identificationIds">认证申请Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DisposeIdentification(IEnumerable<long> identificationIds, IdentificationStatus status)
        {
            foreach (var identificationId in identificationIds)
            {
                //根据认证申请Id获得认证申请实体
                Identification identification = identificationService.GetIdentification(identificationId);
                if (identification.Status != status)
                {
                    identification.Status = status;

                    //将认证申请的处理人Id改成当前用户的Id
                    identification.DisposerId = UserContext.CurrentUser.UserId;

                    //更新处理时间
                    identification.LastModified = DateTime.UtcNow;

                    //更新认证申请
                    identificationService.UpdateIdentification(identification);
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        /// <summary>
        /// 删除身份认证
        /// </summary>
        /// <param name="identificationIds">认证申请Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteIdentification(IEnumerable<long> identificationIds)
        {
            foreach (long identificationId in identificationIds)
            {
                identificationService.DeleteIdentification(identificationId);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        #endregion

        #region 数据验证

        /// <summary>
        /// 验证用户昵称是否可用
        /// </summary>
        /// <param name="nickName">昵称</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateNickName(string nickName, long userId)
        {
            IUser user = userService.GetUser(userId);
            if (user == null)
                return Json("找不到该用户", JsonRequestBehavior.AllowGet);

            if (user.NickName == nickName)
                return Json(true, JsonRequestBehavior.AllowGet);

            string errorMessage;
            bool valid = Utility.ValidateNickName(nickName, out errorMessage);
            if (valid)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(Tunynet.Utilities.WebUtility.HtmlDecode(errorMessage), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
