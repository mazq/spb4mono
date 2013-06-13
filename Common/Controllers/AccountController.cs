//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Email;
using Tunynet.Logging;
using Tunynet.UI;
using Tunynet.Utilities;
using Tunynet.Mvc;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.IO;
using log4net.Repository.Hierarchy;
using System.Net;
using Tunynet.Globalization;
using System.Web.Helpers;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户账户Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = false)]
    public class AccountController : Controller
    {
        #region private items
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private InviteFriendService inviteFriendService = new InviteFriendService();
        private IAuthenticationService authenticationService = DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
        private FollowService followService = new FollowService();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private IMembershipService membershipService = DIContainer.Resolve<IMembershipService>();
        private EmailService emailService = new EmailService();
        private UserSettings userSettings = DIContainer.Resolve<IUserSettingsManager>().Get();
        #endregion

        #region 注册 & 登录
        
        

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            string returnUrl = Request.QueryString.Get<string>("returnUrl", null);

            SystemMessageViewModel message;
            bool isContinue = InviteRegister(out message);
            if (!isContinue)
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, message));
            pageResourceManager.InsertTitlePart("注册");
            ViewData["MinRequiredPasswordLength"] = userSettings.MinPasswordLength;
            ViewData["MaxUserNameLength"] = userSettings.MaxUserNameLength;

            if (!string.IsNullOrEmpty(returnUrl))
            {
                Response.Cookies.Remove("returnUrl");
                HttpCookie cookie = new HttpCookie("returnUrl", Tunynet.Utilities.WebUtility.UrlEncode(returnUrl));
                Response.Cookies.Add(cookie);
            }
            else
            {
                Response.Cookies["returnUrl"].Expires = DateTime.Now;
            }

            return View(new RegisterEditModel() { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken(Salt = "Spacebuilder")]
        [CaptchaVerify(VerifyScenarios.Register)]
        [HttpPost]
        public ActionResult Register(RegisterEditModel model)
        {
            IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            SystemMessageViewModel message;
            bool isContinue = InviteRegister(out message);
            if (!isContinue)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, message));
            }
            if (!ModelState.IsValid)
                return View(model);
            User user = model.AsUser();

            //根据站点配置设置用户属性
            user.IsActivated = userSettings.AccountActivation == AccountActivation.Automatic;

            //尝试创建用户
            IMembershipService membershipService = DIContainer.Resolve<IMembershipService>();
            UserCreateStatus userCreateStatus = UserCreateStatus.UnknownFailure;
            membershipService.CreateUser(user, model.Password, out userCreateStatus);

            
            
            if (userCreateStatus != UserCreateStatus.Created)
            {
                switch (userCreateStatus)
                {
                    case UserCreateStatus.DisallowedUsername:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "重复的用户名");
                        break;
                    case UserCreateStatus.DuplicateEmailAddress:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "重复的邮箱");
                        break;
                    case UserCreateStatus.DuplicateMobile:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "重复的手机号");
                        break;
                    case UserCreateStatus.DuplicateUsername:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "重复的用户名");
                        break;
                    case UserCreateStatus.InvalidPassword:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "不合法的密码");
                        break;
                    case UserCreateStatus.InvalidQuestionAnswer:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "不合法的问题答案");
                        break;
                    default:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "发生未知错误");
                        break;
                }
                model.Password = string.Empty;
                model.ConfirmPassword = string.Empty;
                return View(model);
            }

            
            UserProfileService userProfileService = new UserProfileService();
            UserProfile userProfile = UserProfile.New();
            userProfile.UserId = user.UserId;
            userProfileService.Create(userProfile);

            
            
            
            

            if (userSettings.EnableNotActivatedUsersToLogin || user.IsActivated)
                authenticationService.SignIn(user, false);

            if (Request.Cookies["invite"] != null)
            {
                string invite = Request.Cookies["invite"].Value;
                InvitationCode invitationCode = inviteFriendService.GetInvitationCodeEntity(invite);
                if (invitationCode != null)
                {
                    string errorMessage;
                    InviteFriendRecord inviteFriendRecord = new InviteFriendRecord
                    {
                        Code = invitationCode.Code,
                        DateCreated = DateTime.UtcNow,
                        InvitedUserId = user.UserId,
                        UserId = invitationCode.UserId
                    };
                    inviteFriendService.CreateInviteFriendRecord(inviteFriendRecord);
                    if (!invitationCode.IsMultiple)
                    {
                        bool isSuccess = MutualFollow(invitationCode, userService.GetFullUser(user.UserName), out errorMessage);
                        Response.Cookies.Remove("invite");
                        if (!isSuccess)
                        {
                            return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                            {
                                Title = "关注失败",
                                Body = errorMessage,
                                StatusMessageType = StatusMessageType.Error
                            }));
                        }
                    }
                    else
                    {
                        User userFromDB = userService.GetFullUser(user.UserName);
                        if (userFromDB != null)
                        {
                            string token = Utility.EncryptTokenForInviteFriend(0.01, userFromDB.UserId);
                            return Redirect(SiteUrls.Instance().ConfirmFollow(token, Request.Cookies["invite"].Value));
                        }
                    }
                }

                //IUser userFromDB = userService.GetFullUser(model.UserName);
                //string token = null;
                //if (!userSettings.EnableNotActivatedUsersToLogin)
                //{
                //    token = Utility.EncryptTokenForInviteFriend(0.01, userFromDB.UserId);
                //    return Redirect(SiteUrls.Instance().ConfirmFollow(token));
                //}
                //User userFormDB = userService.GetFullUser(model.UserName);
                //string errorMessage;
                //AcceptInvitation(Request.Cookies["invite"].Value, userFormDB, out errorMessage);
            }

            return RegisterJumpByconfig(user);
        }

        /// <summary>
        /// 根据系统配置跳转到对应页面的方法
        /// </summary>
        /// <param name="model">注册的实体</param>
        /// <param name="user">注册成功的用户</param>
        /// <returns></returns>
        private ActionResult RegisterJumpByconfig(IUser user)
        {
            //根据讨论结果无论什么注册情况都需要发送注册邮件的
            #region 发送 邮件

            bool sendEmailSuccess = false;

            //需要邮箱激活    
            try
            {
                System.Net.Mail.MailMessage mailMessage = EmailBuilder.Instance().RegisterValidateEmail(user);
                //异步发送
                sendEmailSuccess = emailService.SendAsyn(mailMessage, false);


                
                
                
                

                
                
                
                

                
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger().Log(LogLevel.Error, ex, "创建帐号给用户发送激活邮件时发生错误");
            }

            #endregion


            
            
            
            
            if (userSettings.AccountActivation == AccountActivation.Automatic)
            {
                #region 回跳地址

                if (Request.Cookies != null)
                {
                    if (Request.Cookies.Get("returnUrl") != null && !string.IsNullOrEmpty(Request.Cookies.Get("returnUrl").Value))
                    {
                        string returnUrl = Request.Cookies.Get("returnUrl").Value;
                        Response.Cookies["returnUrl"].Expires = DateTime.Now;
                        return Redirect(Tunynet.Utilities.WebUtility.UrlDecode(returnUrl));
                    }

                }

                #endregion

                //根据站点配置判断应该跳转到什么页面
                if (userSettings.MyHomePageAsSiteEntry)
                    return Redirect(SiteUrls.Instance().MyHome(user.UserId));
                return Redirect(SiteUrls.Instance().SiteHome());
            }
            else if (userSettings.AccountActivation == AccountActivation.Email)
            {
                if (sendEmailSuccess)
                {
                    TempData["SendEmailSucceedViewModel"] = SendEmailSucceedViewModelFactory.GetRegisterSendEmailSucceedViewModel(user.AccountEmail);
                    return Redirect(SiteUrls.Instance().SendEmailSucceed());
                }

                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "发送邮件失败",
                    Body = "您的帐号已经注册成功，但是发送激活邮件失败，管理员会尽快给您激活帐号。请耐心等待",
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            else if (userSettings.AccountActivation == AccountActivation.SMS)
            {
                //预置功能
            }
            else if (userSettings.AccountActivation == AccountActivation.Administrator)
            {
                return Redirect(SiteUrls.Instance().WaitForAdminExamine());
            }
            
            


            
            
            
            

            return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
            {
                Title = "注册时发生未知错误",
                Body = "对不起，在注册时发生了未知错误请及时联系管理员",
                StatusMessageType = StatusMessageType.Error
            }));
        }

        /// <summary>
        /// 确认关注页面（邀请注册的时候使用）
        /// </summary>
        /// <returns>关注确认页面</returns>
        [HttpGet]
        public ActionResult ConfirmFollow(string token, string invite)
        {
            //需要判断现在是否允许用户选择是否关注
            //如果允许用户选择是否关注，判断邀请码是否有效，如果有效显示。如果无效继续流程。
            pageResourceManager.InsertTitlePart("确认关注");
            IUser user = null;
            User invitationUser = null;
            if (!string.IsNullOrEmpty(token))
            {
                bool isTimeout = true;
                long userId = Utility.DecryptTokenForInviteFriend(token, out isTimeout);
                if (!isTimeout)
                    user = userService.GetFullUser(userId);
                else
                    return Redirect(SiteUrls.Instance().SystemMessage());
                InvitationCode invitationCode = inviteFriendService.GetInvitationCodeEntity(invite);
                if (invitationCode == null || invitationCode.ExpiredDate < DateTime.UtcNow)
                    return Redirect(SiteUrls.Instance().SystemMessage());
                invitationUser = userService.GetFullUser(invitationCode.UserId);
                if (invitationUser == null)
                    return Redirect(SiteUrls.Instance().SystemMessage());
                string errorMessage;
                if (!CanFollowUser(userId, invitationCode.UserId, out errorMessage))
                {

                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Title = "您不能接受邀请",
                        Body = errorMessage,
                        StatusMessageType = StatusMessageType.Error
                    }));
                }
            }
            else
                return Redirect(SiteUrls.Instance().SystemMessage());

            ConfirmFollowViewModel confirmFollowViewModel = new ConfirmFollowViewModel();
            confirmFollowViewModel.FollowUserName = invitationUser.DisplayName;
            confirmFollowViewModel.Token = token;
            confirmFollowViewModel.invite = invite;
            return View(confirmFollowViewModel);
        }

        /// <summary>
        /// 处理用户是否添加关注
        /// </summary>
        /// <param name="token">登录凭证</param>
        /// <param name="confirm">是否要验证</param>
        /// <returns>登录的凭证（有效期约十分钟）</returns>
        [HttpPost]
        public ActionResult ConfirmFollow(string token, string invite, bool confirm = true)
        {
            Response.Cookies.Remove("invite");
            User user = null;
            if (!string.IsNullOrEmpty(token))
            {
                bool isTimeout = true;
                long userId = Utility.DecryptTokenForInviteFriend(token, out isTimeout);
                if (!isTimeout)
                    user = userService.GetFullUser(userId);
                else
                    return Redirect(SiteUrls.Instance().SystemMessage());
            }
            else
                return Redirect(SiteUrls.Instance().SystemMessage());
            if (!confirm)
                return RegisterJumpByconfig(user);
            InvitationCode InvitationCode = inviteFriendService.GetInvitationCodeEntity(invite);
            IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            string errorMessage = string.Empty;


            if (!MutualFollow(invite, user, out errorMessage) && inviteFriendSettings.AllowInvitationCodeUseOnce)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "关注失败",
                    Body = errorMessage,
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            //跳转到跳转方法
            return RegisterJumpByconfig(user);
        }

        
        
        /// <summary>
        /// 登录的页面
        /// </summary>
        /// <returns>登录的页面</returns>
        [HttpGet]
        public ActionResult Login()
        {
            string returnUrl = Request.QueryString.Get<string>("returnUrl", null);

            if (UserContext.CurrentUser != null && !string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                Response.Cookies.Remove("returnUrl");
                HttpCookie cookie = new HttpCookie("returnUrl", Tunynet.Utilities.WebUtility.UrlEncode(returnUrl));
                Response.Cookies.Add(cookie);
            }
            else
            {
                Response.Cookies["returnUrl"].Expires = DateTime.Now;
            }

            pageResourceManager.InsertTitlePart("登录");
            if (!string.IsNullOrEmpty(returnUrl))
                ViewData["PresetMessage"] = "您访问的页面需要登录才能查看";

            if (TempData["PromptState"] != null)
                ViewData["PresetMessage"] = TempData["PromptState"];
            ViewData["CanRegister"] = userSettings.RegistrationMode == RegistrationMode.All;
            return View(new LoginEditModel { loginInModal = false, ReturnUrl = returnUrl });
        }

        /// <summary>
        /// 模式窗口下的登录模式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult _LoginInModal(string ReturnUrl, string callBack = null)
        {
            ViewData["CanRegister"] = userSettings.RegistrationMode == RegistrationMode.All;
            ViewData["CallBackName"] = callBack;
            return View(new LoginEditModel { loginInModal = true, ReturnUrl = ReturnUrl });
        }

        /// <summary>
        /// 局部视图的登录模式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult _Login(string ReturnUrl)
        {
            ViewData["CanRegister"] = userSettings.RegistrationMode == RegistrationMode.All;
            return PartialView(new LoginEditModel { loginInModal = false, ReturnUrl = ReturnUrl });
        }

        /// <summary>
        /// 处理登录
        /// </summary>
        /// <param name="model">用户输入的内容</param>
        /// <returns></returns>
        [CaptchaVerify(VerifyScenarios.Login)]
        [HttpPost]
        public ActionResult Login(LoginEditModel model)
        {
            pageResourceManager.InsertTitlePart("登录");
            ViewData["CanRegister"] = userSettings.RegistrationMode == RegistrationMode.All;

            if (!ModelState.IsValid)
            {
                model.Password = string.Empty;
                return View(model);
            }

            //尝试登录
            User user = model.AsUser();

            //使用用户名作为用户名和邮件分别尝试登录
            UserLoginStatus userLoginStatus = membershipService.ValidateUser(user.UserName, user.Password);
            if (userLoginStatus == UserLoginStatus.InvalidCredentials)
            {
                IUser userByEmail = userService.FindUserByEmail(user.UserName);
                if (userByEmail != null)
                {
                    user = userByEmail as User;
                    userLoginStatus = membershipService.ValidateUser(userByEmail.UserName, model.Password);
                }
                if (userLoginStatus != UserLoginStatus.InvalidCredentials && !userByEmail.IsEmailVerified)
                {
                    ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "您的邮箱没有激活，请您使用帐号登录");
                    model.Password = string.Empty;
                    return View(model);
                }
            }
            else
            {
                user = userService.GetUser(model.UserName) as User;
            }

            
            
            

            if (userLoginStatus == UserLoginStatus.Success || (userLoginStatus == UserLoginStatus.NotActivated && userSettings.EnableNotActivatedUsersToLogin))
            {
                //让用户登录
                user.UserId = UserIdToUserNameDictionary.GetUserId(user.UserName);
                authenticationService.SignIn(user, model.RememberPassword);
            }

            if (userLoginStatus == UserLoginStatus.Success)
            {
                if (Request.Cookies["invite"] != null)
                {
                    InvitationCode invitationCode = inviteFriendService.GetInvitationCodeEntity(Request.Cookies["invite"].Value);
                    if (invitationCode != null)
                    {
                        Response.Cookies.Remove("invite");
                        string token = Utility.EncryptTokenForInviteFriend(0.01, user.UserId);
                        return Redirect(SiteUrls.Instance().ConfirmFollow(token, invitationCode.Code));
                    }
                }

                if (Request.Cookies.Get("returnUrl") != null)
                {
                    Response.Cookies["returnUrl"].Expires = DateTime.Now;
                }


                if (!string.IsNullOrEmpty(model.ReturnUrl))
                    return Redirect(Tunynet.Utilities.WebUtility.UrlDecode(model.ReturnUrl));

                if (Request.Cookies != null)
                {
                    string returnUrl = Request.Cookies.Get("returnUrl") != null ? Request.Cookies.Get("returnUrl").Value : string.Empty;
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(Tunynet.Utilities.WebUtility.UrlDecode(returnUrl));
                }

                if (model.loginInModal && Request.UrlReferrer != null)
                    return Redirect(Request.UrlReferrer.AbsoluteUri);
                //判断站点设置选择登录之后的页面
                if (userSettings.MyHomePageAsSiteEntry)
                    return Redirect(SiteUrls.Instance().MyHome(user.UserId));
                return Redirect(SiteUrls.Instance().SiteHome());
            }
            else if (userLoginStatus == UserLoginStatus.InvalidCredentials)
            {
                ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "帐号密码不匹配，请检查您的帐号密码");
                model.Password = string.Empty;
                return View(model);
            }
            else if (userLoginStatus == UserLoginStatus.Banned)
            {
                
                
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "帐号被封禁！",
                    Body = "不好意思，由于您的非法操作，帐号已经被封禁，封禁截止到" + user.BanDeadline.ToFriendlyDate(),
                    StatusMessageType = StatusMessageType.Error
                }, model.ReturnUrl));
            }
            else if (userLoginStatus == UserLoginStatus.NotActivated)
            {
                
                
                

                
                

                string token = Utility.EncryptTokenForValidateEmail(0.004, user.UserId);

                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "帐号未激活！",
                    Body = "您的帐号还未激活，请尽快{0}您的帐号，以方便您更多操作！",
                    BodyLink = new Dictionary<string, string> { { "激活", SiteUrls.Instance()._ActivateByEmail(user.AccountEmail, token) } },
                    StatusMessageType = StatusMessageType.Error
                }, model.ReturnUrl));
            }

            ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "系统发生未知错误");
            model.Password = string.Empty;
            return View(model);
        }

        /// <summary>
        /// 模式框登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CaptchaVerify(VerifyScenarios.Login)]
        [HttpPost]
        public ActionResult LoginInModel(LoginEditModel model)
        {
            if (!ModelState.IsValid)
                return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Error, "请检查您录入的信息是否正确")));

            //尝试登录
            User user = model.AsUser();

            //使用用户名作为用户名和邮件分别尝试登录
            UserLoginStatus userLoginStatus = membershipService.ValidateUser(user.UserName, user.Password);
            if (userLoginStatus == UserLoginStatus.InvalidCredentials)
            {
                IUser userByEmail = userService.FindUserByEmail(user.UserName);
                if (userByEmail != null)
                {
                    user = userByEmail as User;
                    userLoginStatus = membershipService.ValidateUser(userByEmail.UserName, model.Password);
                }
                if (userLoginStatus != UserLoginStatus.InvalidCredentials && !userByEmail.IsEmailVerified)
                {
                    return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Error, "您的邮箱没有通过验证，请使用帐号登录")));
                }
            }
            else
            {
                user = userService.GetUser(model.UserName) as User;
            }

            //获取站点设置
            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();

            if (userLoginStatus == UserLoginStatus.Success || (userLoginStatus == UserLoginStatus.NotActivated && userSettings.EnableNotActivatedUsersToLogin))
            {
                Response.Cookies["returnUrl"].Expires = DateTime.Now;

                //让用户登录
                user.UserId = UserIdToUserNameDictionary.GetUserId(user.UserName);
                authenticationService.SignIn(user, model.RememberPassword);
                return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Success, "登录成功")));
            }

            string message;
            switch (userLoginStatus)
            {
                case UserLoginStatus.Banned:
                    message = "用户被封禁";
                    break;
                case UserLoginStatus.InvalidCredentials:
                    message = "用户名、密码不匹配";
                    break;
                case UserLoginStatus.NotActivated:
                    message = "帐号未激活";
                    break;
                default:
                    message = "未知错误";
                    break;
            }
            return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(UserContext.CurrentUser == null ? StatusMessageType.Error : StatusMessageType.Success, message)));
        }

        /// <summary>
        /// 忘记密码的页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            pageResourceManager.InsertTitlePart("重设密码");
            if (string.IsNullOrEmpty(token))
                //表示连接无效
                return Redirect(SiteUrls.Instance().SystemMessage());
            bool isTimeout = true;
            long userId = Utility.DecryptTokenForFindPassword(token, out isTimeout);
            string userName = UserIdToUserNameDictionary.GetUserName(userId);
            if (string.IsNullOrEmpty(userName))
            {
                //没有此用户
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                 {
                     Title = "没有找到对应用户",
                     Body = "可能由于您的长时间未登录帐号被注销，请尝试联系管理员协助解决",
                     StatusMessageType = StatusMessageType.Error
                 }));
            }
            if (!isTimeout)
                return View(new ResetPasswordEditModel { Token = token, UserName = userName });
            //表示已经过期。不能使用
            return Redirect(SiteUrls.Instance().SystemMessage());
        }

        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="resetPasswordEditModel"></param>
        [CaptchaVerify(VerifyScenarios.Post)]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordEditModel resetPasswordEditModel)
        {
            pageResourceManager.InsertTitlePart("重设密码");
            if (!ModelState.IsValid)
                
                
                return View(resetPasswordEditModel);
            IMembershipService membershipService = DIContainer.Resolve<IMembershipService>();
            bool isTimeout = true;
            long userId = Utility.DecryptTokenForFindPassword(resetPasswordEditModel.Token, out isTimeout);
            if (isTimeout)
                //表示过期
                return Redirect(SiteUrls.Instance().SystemMessage());
            string userName = UserIdToUserNameDictionary.GetUserName(userId);
            if (string.IsNullOrEmpty(userName))
            {
                //表示无此用户
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "没有找到对应用户",
                    Body = "可能由于您的长时间未登录帐号被注销，请尝试联系管理员协助解决",
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            bool resetSucceed = membershipService.ResetPassword(userName, resetPasswordEditModel.Password);
            if (!resetSucceed)
            {
                TempData["PromptState"] = "密码重设失败，请重新设置密码并使用新密码登录";
                //修改失败
                return View(resetPasswordEditModel);
            }
            TempData["PromptState"] = "重设密码成功请使用新密码登录";
            return Redirect(SiteUrls.Instance().Login());
        }

        /// <summary>
        /// 忘记密码的页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult FindPassword(string accountEmail = null, bool isPartial = false)
        {
            pageResourceManager.InsertTitlePart("找回密码");
            if (isPartial)
                return View("_FindPassword", new FindPasswordEditModel { AccountEmail = accountEmail });
            return View(new FindPasswordEditModel { AccountEmail = accountEmail });
        }

        /// <summary>
        /// 处理用户忘记密码的方法
        /// </summary>
        /// <returns></returns>
        [CaptchaVerify(VerifyScenarios.Register)]
        [HttpPost]
        public ActionResult FindPassword(FindPasswordEditModel FindPasswordEditModel)
        {
            pageResourceManager.InsertTitlePart("找回密码");
            if (!ModelState.IsValid)
                return View(FindPasswordEditModel);
            IMembershipService membershipService = DIContainer.Resolve<IMembershipService>();
            IUser user = userService.FindUserByEmail(FindPasswordEditModel.AccountEmail);
            if (user == null)
            {
                ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "找不到对应的帐号，请填写正确的邮箱");
                return View(FindPasswordEditModel);
            }
            try
            {
                if (membershipService.SendRecoverPasswordEmail(FindPasswordEditModel.AccountEmail))
                {
                    TempData["SendEmailSucceedViewModel"] = SendEmailSucceedViewModelFactory.GetFindPasswordSendEmailSucceedViewModel(FindPasswordEditModel.AccountEmail);
                    return Redirect(SiteUrls.Instance().SendEmailSucceed());
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger().Log(LogLevel.Error, ex, "发送重设密码邮件时发生错误");
            }
            ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "发送邮件失败，请重新发送");
            return View(FindPasswordEditModel);
        }

        /// <summary>
        /// 通过邮件激活激活帐号页面
        /// </summary>
        /// <returns>激活帐号页面</returns>
        [HttpGet]
        public ActionResult _ActivateByEmail(string accountEmail = null, string token = null)
        {
            return View("_ActivateByEmail", new ActivateByEmail
            {
                AccountEmail = accountEmail,
                Token = token
            });
        }

        /// <summary>
        /// 验证邮箱的方法
        /// </summary>
        /// <returns>是否通过验证</returns>
        [HttpGet]
        public ActionResult ActivateEmail(string token)
        {
            bool isTimeout = false;
            long userId = Utility.DecryptTokenForValidateEmail(token, out isTimeout);
            SystemMessageViewModel model = null;
            if (!isTimeout)
            {
                if (!(userId > 0))
                    return Redirect(SiteUrls.Instance().SystemMessage());
                if (userSettings.AccountActivation == AccountActivation.Automatic || userSettings.AccountActivation == AccountActivation.Email)
                    membershipService.ActivateUsers(new List<long> { userId });
                userService.UserEmailVerified(userId);
                IUser user = userService.GetUser(userId);
                string title = string.Empty;
                if (user.IsActivated)
                    title = "帐号丶邮箱";
                else
                    title = "邮箱";
                model = new SystemMessageViewModel
                  {
                      StatusMessageType = Tunynet.Mvc.StatusMessageType.Success,
                      Title = string.Format("{0}通过验证", title),
                      Body = string.Format("您的{0}已经通过验证，以后可以通过邮箱登录您的帐号", title)
                  };
                authenticationService.SignIn(user, false);

            }
            return Redirect(SiteUrls.Instance().SystemMessage(TempData, model));
        }

        /// <summary>
        /// 通过邮箱激活的处理方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CaptchaVerify(VerifyScenarios.Register | VerifyScenarios.Post)]
        [HttpPost]
        public ActionResult ActivateByEmail(ActivateByEmail model)
        {
            if (string.IsNullOrEmpty(model.Token))
                return Json(SiteUrls.Instance().SystemMessage());

            if (!ModelState.IsValid)
                return Json(new StatusMessageData(StatusMessageType.Error, "验证码填写错误，发送失败"));

            bool isTimeout = false;
            long userId = Utility.DecryptTokenForValidateEmail(model.Token, out isTimeout);
            if (isTimeout)
            {
                return Json(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "链接已经失效",
                    Body = "您使用的链接已经失效，请重新登陆",
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            if (userId <= 0)
            {
                return Json(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "查找不到对应的用户",
                    Body = "对不起，你访问的用户不存在",
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            IUser user = userService.GetFullUser(userId);
            User userSaveToDB = user as User;
            if (user.AccountEmail != model.AccountEmail)
            {
                string errorMessage;
                bool isValidateEmail = Utility.ValidateEmail(model.AccountEmail, out errorMessage);
                if (!isValidateEmail)
                {
                    Tunynet.Utilities.WebUtility.SetStatusCodeForError(Response);
                    TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, Tunynet.Utilities.WebUtility.HtmlDecode(errorMessage));
                    return _ActivateByEmail(model.AccountEmail, model.Token);
                }
                userSaveToDB.AccountEmail = model.AccountEmail;
                userSaveToDB.IsEmailVerified = false;
                membershipService.UpdateUser(userSaveToDB);
            }

            //需要邮箱激活    
            try
            {
                System.Net.Mail.MailMessage mailMessage = EmailBuilder.Instance().RegisterValidateEmail(userSaveToDB);
                EmailService emailService = new EmailService();
                //异步发送
                if (emailService.SendAsyn(mailMessage))
                {
                    SiteSettings siteSettings = new SiteSettings();
                    TempData["SendEmailSucceedViewModel"] = SendEmailSucceedViewModelFactory.GetRegisterSendEmailSucceedViewModel(user.AccountEmail);
                    return Json(SiteUrls.Instance().SendEmailSucceed());
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger().Log(LogLevel.Error, ex, "创建帐号给用户发送激活邮件时发生错误");
            }
            return Json(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
            {
                Title = "发送失败",
                Body = "邮件发送失败",
                StatusMessageType = StatusMessageType.Error
            }));
        }

        /// <summary>
        /// 显示发送了重设密码的邮件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SendEmailSucceed()
        {
            pageResourceManager.InsertTitlePart("成功发送了邮件");
            if (TempData["SendEmailSucceedViewModel"] == null)
                return Redirect(SiteUrls.Instance().SystemMessage());
            return View(TempData["SendEmailSucceedViewModel"]);
        }

        /// <summary>
        /// 等待管理员审核的页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult WaitForAdminExamine()
        {
            pageResourceManager.InsertTitlePart("等待管理员为您激活帐号");
            
            
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();
            ViewData["SiteName"] = siteSettings.SiteName;
            return View();
        }
        
        
        
        /// <summary>
        /// 帐号异常页面，帐号未激活或者帐号被封禁之类
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemMessage(string returnUrl)
        {
            if (TempData["SystemMessageViewModel"] == null)
                TempData["SystemMessageViewModel"] = new SystemMessageViewModel
                {
                    Body = "您访问的页面已经失效",
                    ReturnUrl = returnUrl,
                    Title = "链接失效",
                    StatusMessageType = StatusMessageType.Error
                };
            SystemMessageViewModel systemMessageViewModel = TempData["SystemMessageViewModel"] as SystemMessageViewModel;
            pageResourceManager.InsertTitlePart(systemMessageViewModel.Title);
            systemMessageViewModel.ButtonLink.Clear();
            systemMessageViewModel.ButtonLink.Add("站点首页", SiteUrls.Instance().SiteHome());
            IUser iUser = authenticationService.GetAuthenticatedUser();
            if (iUser != null)
            {
                systemMessageViewModel.ButtonLink.Add("个人首页", SiteUrls.Instance().MyHome(iUser.UserId));
            }
            return View(TempData["SystemMessageViewModel"]);
        }

        /// <summary>
        /// 验证用户名是否可用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateUserName(string userName)
        {
            string errorMessage;
            bool valid = Utility.ValidateUserName(userName, out errorMessage);
            if (valid)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证密码的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidatePassword(string password)
        {
            string errorMessage;
            bool valid = Utility.ValidatePassword(password, out errorMessage);
            if (valid)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证邮箱的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateEmail(string accountEmail)
        {
            string errorMessage;
            bool valid = Utility.ValidateEmail(accountEmail, out errorMessage);
            if (valid)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(Tunynet.Utilities.WebUtility.HtmlDecode(errorMessage), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 验证昵称
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateNickName(string nickName)
        {
            string errorMessage;
            bool valid = Utility.ValidateNickName(nickName, out errorMessage);
            if (valid)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(Tunynet.Utilities.WebUtility.HtmlDecode(errorMessage), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑资料页面的验证昵称
        /// </summary>
        /// <param name="nickName">昵称</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateNickNameForEditUserProfile(string nickName)
        {
            if (UserContext.CurrentUser == null)
                return Json("您还没有登录，请登录之后重试", JsonRequestBehavior.AllowGet);

            if (nickName.Equals(UserContext.CurrentUser.NickName))
                return Json(true, JsonRequestBehavior.AllowGet);

            string errorMessage;
            bool valid = Utility.ValidateNickName(nickName, out errorMessage);
            if (valid)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(Tunynet.Utilities.WebUtility.HtmlDecode(errorMessage), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证用户是否同意了条款
        /// </summary>
        /// <param name="acceptableProvision"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateAcceptableProvision(bool acceptableProvision)
        {
            return Json(acceptableProvision, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 注册条款
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult _Provision()
        {
            return View();
        }

        /// <summary>
        /// 登出方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            authenticationService.SignOut();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            else
                return Redirect(SiteUrls.Instance().SiteHome());
        }

        /// <summary>
        /// 邀请注册的对应处理代码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool InviteRegister(out SystemMessageViewModel message)
        {
            message = new SystemMessageViewModel
            {
                Title = "不允许注册",
                Body = "站点现在不允许注册，请联系管理员，索要帐号",
                StatusMessageType = StatusMessageType.Error
            };
            if (userSettings.RegistrationMode == RegistrationMode.All)
                return true;
            if (userSettings.RegistrationMode == RegistrationMode.Disabled)
                return false;
            message = new SystemMessageViewModel
            {
                Title = "邀请注册",
                Body = "站点开启了邀请注册，只有在拥有邀请码的情况才允许注册",
                StatusMessageType = StatusMessageType.Error
            };
            if (Request.Cookies["invite"] == null)
                return false;
            InvitationCode code = inviteFriendService.GetInvitationCodeEntity(Request.Cookies["invite"].Value);
            if (code == null || code.ExpiredDate.CompareTo(DateTime.UtcNow) < 0)
            {
                message.Body = "您的邀请码已经过期了！";
                return false;
            }
            return true;
        }

        #endregion 注册 & 登录

        #region 邀请好友页面
        /// <summary>
        /// 邀请好友页面
        /// </summary>
        /// <param name="invite">邀请码</param>
        /// <returns>是否成功邀请</returns>
        [HttpGet]
        public ActionResult Invite(string invite)
        {
            InvitationCode invitationCode = inviteFriendService.GetInvitationCodeEntity(invite);
            if (invitationCode == null || invitationCode.ExpiredDate.CompareTo(DateTime.UtcNow) < 0)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "链接失效",
                    Body = "邀请链接已经过期",
                    StatusMessageType = StatusMessageType.Error
                }));
            }
            IUser user = authenticationService.GetAuthenticatedUser();
            if (user == null)
            {
                HttpCookie httpCookie = new System.Web.HttpCookie("invite", invite);
                httpCookie.Expires = DateTime.Now.AddHours(0.16);
                Response.Cookies.Add(httpCookie);
                return Redirect(SiteUrls.Instance().Register());
            }
            string errorMessage;
            bool canFollow = CanFollowUser(user.UserId, invitationCode.UserId, out errorMessage);
            if (canFollow)
            {
                string token = Utility.EncryptTokenForInviteFriend(0.01, user.UserId);
                return Redirect(SiteUrls.Instance().ConfirmFollow(token, invitationCode.Code));
            }
            else
            {
                TempData["SystemMessageViewModel"] = new SystemMessageViewModel
                {
                    Title = "确认失败",
                    Body = errorMessage,
                    StatusMessageType = StatusMessageType.Error
                };
                return Redirect(SiteUrls.Instance().SystemMessage());
            }
        }

        /// <summary>
        /// 相互关注，其中，只是根据邀请码来判断是否可以执行而已
        /// </summary>
        /// <param name="invitationCode">邀请码</param>
        /// <param name="user">被邀请的用户</param>
        /// <param name="errorMessage">返回错误的信息</param>
        /// <returns>要返回的页面</returns>
        private bool MutualFollow(InvitationCode invitationCode, IUser user, out string errorMessage)
        {
            if (invitationCode.ExpiredDate < DateTime.UtcNow)
            {
                errorMessage = "邀请链接已经过期";
                return false;
            }
            //判断是否过期
            User inviteUser = userService.GetFullUser(invitationCode.UserId);
            if (inviteUser == null)
            {
                errorMessage = "找不到邀请人，可能邀请人已经将帐号注销";
                return false;
            }
            if (followService.IsFollowed(user.UserId, invitationCode.UserId))
            {
                errorMessage = "您已经添加过了关注不需要再次添加关注";
                return false;
            }
            if (!invitationCode.IsMultiple)
                inviteFriendService.DeleteInvitationCode(invitationCode.UserId, invitationCode.Code);
            followService.Follow(user.UserId, invitationCode.UserId);
            followService.Follow(invitationCode.UserId, user.UserId);

            errorMessage = string.Format("成功接收了{0}邀请", user.UserName);
            return true;
        }

        /// <summary>
        /// 接受邀请
        /// </summary>
        /// <param name="invite">邀请码</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="user">被邀请的用户</param>
        private bool MutualFollow(string invite, IUser user, out string errorMessage)
        {
            InvitationCode invitationCode = inviteFriendService.GetInvitationCodeEntity(invite);
            if (invitationCode == null)
            {
                errorMessage = "链接已经超时";
                return false;
            }
            return MutualFollow(invitationCode, user, out errorMessage);
        }

        /// <summary>
        /// 是否能够关注用户
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="toUserId">被检测图表</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns></returns>
        private bool CanFollowUser(long userId, long toUserId, out string errorMessage)
        {
            if (userId == toUserId)
            {
                errorMessage = "不能与自己相互关注";
                return false;
            }
            bool isFollowed = followService.IsFollowed(userId, toUserId);
            if (isFollowed)
            {
                errorMessage = "您已经添加过关注了哦，不需要再次添加关注";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        #endregion

        #region 第三方帐号
        /// <summary>
        /// 使用第三方帐号登录
        /// </summary>
        /// <param name="accountTypeKey"></param>
        /// <returns></returns>
        public ActionResult LoginToThird(string accountTypeKey)
        {
            ThirdAccountGetter thirdAccountGetter = ThirdAccountGetterFactory.GetThirdAccountGetter(accountTypeKey);
            return Redirect(thirdAccountGetter.GetAuthorizationUrl());
        }

        /// <summary>
        /// 登录第三方网站回调地址
        /// </summary>
        /// <param name="accountTypeKey"></param>
        /// <returns></returns>
        public ActionResult ThirdCallBack(string accountTypeKey)
        {
            ThirdAccountGetter thirdAccountGetter = ThirdAccountGetterFactory.GetThirdAccountGetter(accountTypeKey);
            string returnUrl = string.Empty;
            string accessToken = thirdAccountGetter.GetAccessToken(Request);
            if (string.IsNullOrEmpty(accessToken))
            {
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "授权失败,请稍后再试！");
                return View();
            }
            var thirdCurrentUser = thirdAccountGetter.GetThirdUser(accessToken, null);
            if (thirdCurrentUser != null)
            {
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "登录成功");
                ViewData["thirdCurrentUser"] = thirdCurrentUser;
                TempData["thirdCurrentUser"] = thirdCurrentUser;
                var systemCurrentUser = UserContext.CurrentUser;
                AccountBindingService accountBindingService = new AccountBindingService();
                //是否已绑定过其他帐号
                long userId = accountBindingService.GetUserId(accountTypeKey, thirdCurrentUser.Identification);
                User systemUser = userService.GetFullUser(userId);

                //登录用户直接绑定帐号
                if (systemCurrentUser != null)
                {
                    if (systemUser != null)
                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Hint, "此帐号已在网站中绑定过，不可再绑定其他网站帐号");
                    else
                    {
                        AccountBinding account = AccountBinding.New();
                        account.AccountTypeKey = accountTypeKey;
                        account.Identification = thirdCurrentUser.Identification;
                        account.UserId = systemCurrentUser.UserId;
                        account.AccessToken = accessToken;
                        accountBindingService.CreateAccountBinding(account);
                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "绑定成功");
                    }
                }
                else
                {
                    //已经绑定过，直接登录
                    if (systemUser != null)
                    {
                        //获取站点设置
                        IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
                        UserSettings userSettings = userSettingsManager.Get();

                        if ((!systemUser.IsActivated && !userSettings.EnableNotActivatedUsersToLogin)) //帐号未激活
                        {
                            ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "帐号未激活！");
                            ViewData["ShowSystemMessage"] = true;
                            string token = Utility.EncryptTokenForValidateEmail(0.004, systemUser.UserId);
                            returnUrl = SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                            {
                                Title = "帐号未激活！",
                                Body = "您的帐号还未激活，请尽快{0}您的帐号，以方便您更多操作！",
                                BodyLink = new Dictionary<string, string> { { "激活", SiteUrls.Instance()._ActivateByEmail(systemUser.AccountEmail, token) } },
                                StatusMessageType = StatusMessageType.Error
                            });
                        }
                        else if (systemUser.IsBanned) //帐号被封禁
                        {
                            ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "帐号被封禁！");
                            ViewData["ShowSystemMessage"] = true;
                            returnUrl = SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                            {
                                Title = "帐号被封禁！",
                                Body = "不好意思，由于您的非法操作，帐号已经被封禁，封禁截止到" + systemUser.BanDeadline.ToFriendlyDate(),
                                StatusMessageType = StatusMessageType.Error
                            });
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(systemUser.UserName, true);
                            if (userSettings.MyHomePageAsSiteEntry)
                                returnUrl = SiteUrls.Instance().MyHome(systemUser.UserName);
                            else
                                returnUrl = SiteUrls.Instance().SiteHome();

                            #region 关于第三方站好登陆之后的回跳地址

                            if (Request.Cookies != null)
                            {
                                if (Request.Cookies.Get("returnUrl") != null && !String.IsNullOrEmpty(Request.Cookies.Get("returnUrl").Value))
                                {
                                    string returnUrlFromCookie = Request.Cookies.Get("returnUrl").Value;

                                    if (!string.IsNullOrEmpty(returnUrlFromCookie))
                                        returnUrl = Tunynet.Utilities.WebUtility.UrlDecode(returnUrlFromCookie);

                                    if (Response.Cookies != null)
                                        Response.Cookies["returnUrl"].Expires = DateTime.Now;
                                }
                            }

                            #endregion

                            accountBindingService.UpdateAccessToken(systemUser.UserId, thirdCurrentUser.AccountTypeKey, thirdCurrentUser.Identification, thirdCurrentUser.AccessToken);
                            ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "登录成功");
                        }
                    }
                    else
                    {
                        if (userSettings.RegistrationMode == RegistrationMode.Disabled)
                        {
                            return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                            {
                                Title = "禁止注册",
                                Body = "管理员限制了新帐号的创建，请过后再试",
                                StatusMessageType = StatusMessageType.Error
                            }));
                        }
                        else
                        {
                            ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, string.Empty);
                            ViewData["FirstLogin"] = true;
                            returnUrl = SiteUrls.Instance().ThirdRegister();
                        }
                    }
                }
            }
            else
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "参数错误，授权失败！");
            ViewData["SiteName"] = DIContainer.Resolve<ISiteSettingsManager>().Get().SiteName;
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 首次登录完善帐号资料
        /// </summary>
        [HttpGet]
        public ActionResult ThirdRegister()
        {
            pageResourceManager.InsertTitlePart("首次登录完善帐号资料");
            ThirdUser thirdUser = TempData.Get<ThirdUser>("thirdCurrentUser", null);
            TempData["thirdCurrentUser"] = thirdUser;
            ViewData["thirdCurrentUser"] = thirdUser;

            if (thirdUser == null)
                return Redirect(SiteUrls.Instance().Login());

            if (new AccountBindingService().GetUserId(thirdUser.AccountTypeKey, thirdUser.Identification) > 0)
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "重复绑定",
                    Body = "已经有网站帐号绑定过，不允许重复绑定第三方帐号",
                    StatusMessageType = StatusMessageType.Error
                }));

            ThirdAccountGetter thirdAccountGetter = ThirdAccountGetterFactory.GetThirdAccountGetter(thirdUser.AccountTypeKey);
            ViewData["AccountType"] = new AccountBindingService().GetAccountType(thirdUser.AccountTypeKey);
            ViewData["AccountTypeName"] = thirdAccountGetter.AccountTypeName;
            thirdUser.NickName = thirdUser.NickName.Replace("%", "").Replace("+", "").Replace(" ", "").Replace("/", "").Replace("?", "").Replace("&", "").Replace("=", "").Replace("#", "");
            var model = new ThirdRegisterEditModel();
            model.UserName = thirdUser.NickName;
            model.ShareToFirend = true;
            model.FollowOfficial = true;

            ViewData["Content"] = string.Format(ResourceAccessor.GetString("AccountBinding_ShareToFirend"), thirdAccountGetter.AccountTypeName, DIContainer.Resolve<ISiteSettingsManager>().Get().SiteName, SiteUrls.FullUrl(SiteUrls.Instance().SiteHome()));

            return View(model);
        }

        /// <summary>
        /// 首次登录完善帐号资料
        /// </summary>
        [HttpPost]
        public ActionResult ThirdRegister(ThirdRegisterEditModel model)
        {
            ThirdUser thirdUser = TempData.Get<ThirdUser>("thirdCurrentUser", null);
            TempData["thirdCurrentUser"] = thirdUser;
            if (!ModelState.IsValid)
                return View(model.CleanUp());
            if (thirdUser != null && new AccountBindingService().GetUserId(thirdUser.AccountTypeKey, thirdUser.Identification) > 0)
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "重复绑定",
                    Body = "已经有网站帐号绑定过，不允许重复绑定第三方帐号",
                    StatusMessageType = StatusMessageType.Error
                }));
            //数据通过验证，创建用户，并且将对应用的用户与第三方账户绑定。

            UserCreateStatus userCreateStatus = UserCreateStatus.UnknownFailure;
            User user = model.AsUser();

            user.IsActivated = userSettings.AccountActivation == AccountActivation.Automatic;

            membershipService.CreateUser(user, model.PassWord, out userCreateStatus);
            //如果没有创建成功
            if (userCreateStatus != UserCreateStatus.Created)
            {
                switch (userCreateStatus)
                {
                    case UserCreateStatus.DisallowedUsername:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "重复的用户名");
                        break;
                    case UserCreateStatus.DuplicateEmailAddress:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "重复的邮箱");
                        break;
                    case UserCreateStatus.DuplicateMobile:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "重复的手机号");
                        break;
                    case UserCreateStatus.DuplicateUsername:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "重复的用户名");
                        break;
                    case UserCreateStatus.InvalidPassword:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "不合法的密码");
                        break;
                    case UserCreateStatus.InvalidQuestionAnswer:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "不合法的问题答案");
                        break;
                    default:
                        ViewData["StatusMessage"] = new StatusMessageData(StatusMessageType.Error, "发生未知错误");
                        break;
                }
                return View(model.CleanUp());
            }

            // 处理头像
            //if (!string.IsNullOrEmpty(thirdUser.UserAvatarUrl))
            //{
            //    try
            //    {
            //        WebRequest webRequest = WebRequest.Create(thirdUser.UserAvatarUrl);
            //        HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            //        Stream stream = httpWebResponse.GetResponseStream();

            //        bool isImage = httpWebResponse.ContentType.IndexOf("image") > -1;
            //        if (isImage && stream != null && stream.CanRead)
            //        {
            //            MemoryStream memoryStream = new MemoryStream();
            //            const int bufferLength = 1024;
            //            int actual;
            //            byte[] buffer = new byte[bufferLength];

            //            while ((actual = stream.Read(buffer, 0, bufferLength)) > 0)
            //            {
            //                memoryStream.Write(buffer, 0, actual);
            //            }

            //            userService.UploadOriginalAvatar(user.UserId, memoryStream);
            //            userService.CropAvatar(user.UserId, 100, 100, 0, 0);
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        ILogger logger = LoggerFactory.GetLogger();
            //        logger.Error(e, "第三方帐号登陆时，下载用户头像报错");
            //    }
            //}

            UserProfileService userProfileService = new UserProfileService();
            UserProfile userProfile = UserProfile.New();
            userProfile.UserId = user.UserId;
            userProfile.Gender = thirdUser.Gender;
            userProfileService.Create(userProfile);

            if (userSettings.EnableNotActivatedUsersToLogin || user.IsActivated)
                authenticationService.SignIn(user, false);

            if (!string.IsNullOrEmpty(thirdUser.AccountTypeKey))
            {
                //绑定帐号
                AccountBinding account = new AccountBinding();
                account.AccountTypeKey = thirdUser.AccountTypeKey;
                account.Identification = thirdUser.Identification;
                account.UserId = user.UserId;
                account.AccessToken = thirdUser.AccessToken;
                new AccountBindingService().CreateAccountBinding(account);

                var thirdAccountGetter = ThirdAccountGetterFactory.GetThirdAccountGetter(thirdUser.AccountTypeKey);
                if (model.FollowOfficial)
                {
                    //自动关注官方帐号                 
                    thirdAccountGetter.FollowOfficialMicroBlog(thirdUser.AccessToken, thirdUser.Identification);
                }
                if (model.ShareToFirend)
                {
                    //自动推送一条微博
                    //string siteName = DIContainer.Resolve<ISiteSettingsManager>().Get().SiteName;
                    //string content = string.Format(ResourceAccessor.GetString("AccountBinding_ShareToFirend"), thirdAccountGetter.AccountTypeName, siteName, SiteUrls.FullUrl(SiteUrls.Instance().SiteHome()));
                    string content = Request.Form.Get<string>("Content", string.Empty);
                    if (!string.IsNullOrEmpty(content))
                        thirdAccountGetter.CreateMicroBlog(thirdUser.AccessToken, content, thirdUser.Identification);
                }
            }

            if (Request.Cookies["invite"] != null)
            {
                string invite = Request.Cookies["invite"].Value;
                InvitationCode invitationCode = inviteFriendService.GetInvitationCodeEntity(invite);
                if (invitationCode != null)
                {
                    InviteFriendRecord inviteFriendRecord = new InviteFriendRecord
                    {
                        Code = invitationCode.Code,
                        DateCreated = DateTime.UtcNow,
                        InvitedUserId = user.UserId,
                        UserId = invitationCode.UserId
                    };
                    inviteFriendService.CreateInviteFriendRecord(inviteFriendRecord);
                    if (!invitationCode.IsMultiple)
                    {
                        Response.Cookies.Remove("invite");
                        string errorMessage;
                        bool isSuccess = MutualFollow(invitationCode, userService.GetFullUser(user.UserName), out errorMessage);
                        if (!isSuccess)
                        {
                            return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                            {
                                Title = "关注失败",
                                Body = errorMessage,
                                StatusMessageType = StatusMessageType.Error
                            }));
                        }
                    }
                    else
                    {
                        User userFromDB = userService.GetFullUser(user.UserName);
                        if (userFromDB != null)
                        {
                            string token = Utility.EncryptTokenForInviteFriend(0.01, userFromDB.UserId);
                            return Redirect(SiteUrls.Instance().ConfirmFollow(token, Request.Cookies["invite"].Value));
                        }
                    }
                }
            }

            return RegisterJumpByconfig(user);
        }

        #endregion

        /// <summary>
        /// 获取当前用户状态
        /// </summary>
        /// <returns></returns>
        public ContentResult GetCurrentUser()
        {
            string jsoncall = Request.QueryString.GetString("jsoncallback", string.Empty);
            var currentUser = UserContext.CurrentUser;
            var jsonContent = string.Empty;
            if (currentUser != null)
                jsonContent = System.Web.Helpers.Json.Encode(new
                {
                    displayName = currentUser.DisplayName,
                    avatarUrl = SiteUrls.FullUrl(SiteUrls.Instance().UserAvatarUrl(currentUser, AvatarSizeType.Micro)),
                    myHomeUrl = SiteUrls.FullUrl(SiteUrls.Instance().MyHome(currentUser.UserId)),
                    logoutUrl = SiteUrls.FullUrl(SiteUrls.Instance().Logout())
                });
            return Content(string.Format("{0}({1})", jsoncall, jsonContent));
        }
    }
}
