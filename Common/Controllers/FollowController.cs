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
using Tunynet;
using Tunynet.Common;
using Tunynet.Email;
using Tunynet.Mvc;
using Tunynet.UI;
using Tunynet.Utilities;
using System.Text.RegularExpressions;
using Tunynet.Common.Configuration;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邀请好友Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.UserSpace, IsApplication = false)]
    public class FollowController : Controller
    {
        #region private items
        private IEnumerable<IEmailContactAccessor> contactAccessors = DIContainer.Resolve<IEnumerable<IEmailContactAccessor>>();
        private IAuthenticationService authenticationService = DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private FollowService followService = new FollowService();
        private InviteFriendService inviteFriendService = new InviteFriendService();
        private IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private UserSettings userSettings = DIContainer.Resolve<IUserSettingsManager>().Get();
        private AreaService areaService = new AreaService();
        private UserProfileService userProfileService = new UserProfileService();
        private CategoryService categoryService = new CategoryService();
        private PrivacyService privacyService = new PrivacyService();

        #endregion

        #region 邀请好友
        /// <summary>
        /// 邀请好友的页面
        /// </summary>
        /// <param name="spaceKey">被访问的空间名</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult InviteFriend(string spaceKey, int pageIndex = 1)
        {
            if (userSettings.RegistrationMode == RegistrationMode.Disabled)
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "站点不允许注册，所以邀请好友功能被关闭",
                    Title = "未启用的功能",
                    StatusMessageType = StatusMessageType.Error
                }));
            }

            pageResourceManager.InsertTitlePart("邀请好友");
            IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<InvitationCode> invitationCodes = null;
            if (inviteFriendSettings.AllowInvitationCodeUseOnce)
            {
                invitationCodes = inviteFriendService.GetMyInvitationCodes(userId, pageIndex);
            }
            else
            {
                List<InvitationCode> codes = new List<InvitationCode>();
                string code = inviteFriendService.GetInvitationCode(userId);
                InvitationCode invitationCode = inviteFriendService.GetInvitationCodeEntity(code);
                codes.Add(invitationCode);
                invitationCodes = new PagingDataSet<InvitationCode>(codes);
            }
            InvitationCodeStatistic invitationCodeStatistic = inviteFriendService.GetUserInvitationCodeStatistic(userId);
            ViewData["CodeUnUsedCount"] = invitationCodeStatistic.CodeUnUsedCount;
            if (!Request.IsAjaxRequest())
                return View("InviteFriend", invitationCodes);
            return PartialView("_InvitationCode", invitationCodes);
        }

        /// <summary>
        /// 领取一条邀请码
        /// </summary>
        /// <returns>获取邀请码之后显示的页面</returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult GetNewInvite(string spaceKey)
        {

            

            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            string inviteFriend = SiteUrls.Instance().InviteFriend(spaceKey);
            if (!inviteFriendSettings.AllowInvitationCodeUseOnce)
            {
                TempData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "网站未开启邀请码，领取失败");
                return Redirect(inviteFriend);
            }
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            InvitationCodeStatistic invitationCodeStatistic = inviteFriendService.GetUserInvitationCodeStatistic(userId);
            if (invitationCodeStatistic == null)
            {
                TempData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "领取失败");
                return Redirect(inviteFriend);
            }
            if (invitationCodeStatistic.CodeUnUsedCount <= 0)
            {
                TempData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "配额不足，领取失败");
                return Redirect(inviteFriend);
            }
            string code = inviteFriendService.GetInvitationCode(userId);
            if (string.IsNullOrEmpty(code))
            {
                TempData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "领取失败");
                return Redirect(inviteFriend);
            }
            TempData["statusMessageData"] = new StatusMessageData(StatusMessageType.Success, "领取成功");
            return Redirect(inviteFriend);
        }

        /// <summary>
        /// 购买邀请码的局部视图
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>购买邀请码的的局部页面</returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _BuyInviteCount(string spaceKey)
        {
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            ViewData["InvitationCodeUnitPrice"] = inviteFriendSettings.InvitationCodeUnitPrice;
            return View(new BuyInviteCountEditModel());
        }

        /// <summary>
        /// 购买邀请码处理的方法
        /// </summary>
        /// <param name="spaceKey">用户空间名</param>
        /// <param name="invitationCodeCount">购买的个数</param>
        /// <returns>是否成功购买提示信息</returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _BuyInviteCount(string spaceKey, int invitationCodeCount)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            bool isBuyed = inviteFriendService.BuyInvitationCodes(userId, invitationCodeCount);
            if (isBuyed)
                return Json(new StatusMessageData(StatusMessageType.Success, string.Format("您成功购买了{0}个邀请码配额", invitationCodeCount)));
            WebUtility.SetStatusCodeForError(Response);
            return Json(new StatusMessageData(StatusMessageType.Error, "购买失败"));
        }

        /// <summary>
        /// 使用邮箱邀请好友
        /// </summary>
        /// <param name="spaceKey">被访问用户名</param>
        /// <returns></returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _InviteFriendByEmail(string spaceKey)
        {
            ViewData["emailDomainName"] = new List<SelectListItem>() {
                 new SelectListItem() { Text = "163.com" },
                 new SelectListItem() { Text = "126.com" },
                 new SelectListItem() { Text = "Gmail.com" }
            };
            return View(new InviteFriendByEmailEditMode());
        }

        /// <summary>
        /// 通过邮箱发送邀请邮件
        /// </summary>
        /// <param name="spaceKey">被访问空间名</param>
        /// <param name="model">邀请邮箱中的好友的实体</param>
        /// <returns></returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _InviteFriendByEmail(string spaceKey, InviteFriendByEmailEditMode model)
        {
            

            if (!ModelState.IsValid)
                return Json(new { status = false });
            //判断用户使用哪种类型的邮箱
            string email = string.Format("{0}@{1}", model.userName, model.emailDomainName);
            bool isSuccessLogin = false;
            IEmailContactAccessor contactAccessor = contactAccessors.FirstOrDefault(n => n.EmailDomainName.ToLower().Equals(model.emailDomainName.ToLower()));
            Dictionary<string, string> email2Names = new Dictionary<string, string>();
            if (contactAccessor != null)
                email2Names = contactAccessor.GetContacts(email, model.password, out isSuccessLogin);
            else
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "暂时不支持此类邮箱，你可以通过导入CSV文件或者手动录入的方式邀请好友"));
            }
            if (!isSuccessLogin && (email2Names == null || email2Names.Count == 0))
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "帐号登录失败，请检查您的帐号和密码是否输入正确"));
            }
            return _ChoiceUser(spaceKey, email2Names);
        }

        /// <summary>
        /// 根据用户输入邀请好友
        /// </summary>
        /// <param name="spaceKey">被访问用户名</param>
        /// <returns>邀请好友页面</returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _InviteFriendByUserInput(string spaceKey)
        {
            return View(new InviteFriendByUserInputEditModel());
        }

        /// <summary>
        /// 根据用户输入的信息
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="model">用户输入的邮箱</param>
        /// <returns>返回的页面</returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _InviteFriendByUserInput(string spaceKey, InviteFriendByUserInputEditModel model)
        {
            if (string.IsNullOrEmpty(model.emails))
                return Json(new { status = true });
            string[] emailsStr = model.emails.Replace("\n", "").Replace("\r", "").Replace(" ", "").Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            if (emailsStr.Length > 20)
                //超过可以处理的范围，提示并且返回
                return null;
            Dictionary<string, string> email2Names = new Dictionary<string, string>();
            foreach (string item in emailsStr)
            {
                int subscript = item.IndexOf("@");
                if (subscript > 0 && !email2Names.ContainsKey(item))
                    email2Names.Add(item, item.Substring(0, subscript));
            }
            return _ChoiceUser(spaceKey, email2Names);
        }

        /// <summary>
        /// 根据csv文件邀请好友
        /// </summary>
        /// <param name="spaceKey">被访问用户名</param>
        /// <returns></returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _InviteFriendByCsv(string spaceKey)
        {
            return View(new InviteFriendByCsvEditModel());
        }

        /// <summary>
        /// 根据csv文件邀请好友
        /// </summary>
        /// <param name="spaceKey">被访问用户名</param>
        /// <returns></returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _InviteFriendByCsvPost(string spaceKey)
        {
            Dictionary<string, string> email2Names = new Dictionary<string, string>();
            

            if (Request.Files["fileName"] == null)
            {
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "解析文件失败");
                return View("_ChoiceUser");
            }
            if (Request.Files.Count > 0 && !string.IsNullOrEmpty(Request.Files["fileName"].FileName))
            {
                HttpPostedFileBase postFile = Request.Files["fileName"];
                email2Names = CSVParser.GetContactAccessor(postFile.InputStream);
            }
            else
            {
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "解析文件失败");
                return View("_ChoiceUser");
            }
            return _ChoiceUser(spaceKey, email2Names);
        }

        /// <summary>
        /// 根据msn获取好友
        /// </summary>
        /// <param name="spaceKey">被访问用户名</param>
        /// <returns>访问的页面</returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _InviteFriendByMsn(string spaceKey)
        {
            return View(new InviteFriendByMsnEditModel());
        }

        

        /// <summary>
        /// 通过MSN获取好友
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="model">通过msn邀请好友的实体</param>
        /// <returns>通过msn邀请好友</returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _InviteFriendByMsn(string spaceKey, InviteFriendByMsnEditModel model)
        {
            IMsnContactAccessor msnContactAccessor = DIContainer.ResolvePerHttpRequest<IMsnContactAccessor>();
            


            //根据用户输入的帐号跟密码获取用户的联系人信息
            bool isSuccessLogin = false;
            Dictionary<string, string> email2Names = msnContactAccessor.GetContacts(model.userName, model.password, out isSuccessLogin);
            if (!isSuccessLogin)
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "登录失败"));
            }
            //传递给发送邮件的方法
            return _ChoiceUser(spaceKey, email2Names);
        }

        /// <summary>
        /// 准备发送邮件的时候选择用户的时候使用
        /// </summary>
        /// <returns>选择的局部页面</returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _ChoiceUser(string spaceKey, Dictionary<string, string> email2Names)
        {
            List<IUser> listFollowUsers = new List<IUser>();
            Dictionary<string, string> dictionarySendEmails = new Dictionary<string, string>();
            IUser user = authenticationService.GetAuthenticatedUser();
            int isFollowedCount = 0;
            if (email2Names != null && email2Names.Count > 0)
                foreach (var item in email2Names)
                {
                    Regex regex = new Regex(userSettings.EmailRegex, RegexOptions.ECMAScript);
                    if (!regex.IsMatch(item.Key))
                        continue;
                    IUser userFromEmail = userService.FindUserByEmail(item.Key);
                    bool isFollowed = false;
                    


                    if (userFromEmail != null)
                        isFollowed = followService.IsFollowed(user.UserId, userFromEmail.UserId);
                    

                    if (isFollowed)
                    {
                        isFollowedCount++;
                        continue;
                    }
                    if (userFromEmail != null && !listFollowUsers.Any(n => n.AccountEmail.Equals(item.Key)))
                    {
                        if (userFromEmail.UserId != UserContext.CurrentUser.UserId)
                            listFollowUsers.Add(userFromEmail);
                    }
                    else if (userFromEmail == null && !dictionarySendEmails.ContainsKey(item.Key))
                    {
                        if (item.Key != UserContext.CurrentUser.AccountEmail)
                            dictionarySendEmails.Add(item.Key, item.Value);
                    }
                }
            

            if (listFollowUsers.Count <= 0 && dictionarySendEmails.Count <= 0)
            {
                if (isFollowedCount > 0)
                    return Json(new StatusMessageData(StatusMessageType.Error, string.Format("除了已经关注的{0}人，没有新的好友可以添加了", isFollowedCount)), "text/html; charset=utf-8");
                else
                    return Json(new StatusMessageData(StatusMessageType.Error, "没有可以添加的好友"), "text/html; charset=utf-8");
            }
            //已经注册过的用户获取userid集合
            ViewData["FollowUsers"] = listFollowUsers;
            //没有注册过的用户获取邮箱集合
            ViewData["SendEmails"] = dictionarySendEmails;
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            InvitationCodeStatistic invitationCodeStatistic = inviteFriendService.GetUserInvitationCodeStatistic(user.UserId);

            if (inviteFriendSettings.AllowInvitationCodeUseOnce && dictionarySendEmails.Count > invitationCodeStatistic.CodeUnUsedCount)
                ViewData["PresetMessage"] = string.Format("您还差{0}个配额才能够给这么多用户发送邀请", dictionarySendEmails.Count - invitationCodeStatistic.CodeUnUsedCount);
            return View("_ChoiceUser");
        }

        /// <summary>
        /// 发送邮件或者关注的用户
        /// </summary>
        /// <param name="spaceKey">关注别人的名</param>
        /// <param name="followUsers">准备要关注的用户名</param>
        /// <param name="sendEmails">发送对应的邮件</param>
        /// <returns></returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _ChoiceUser(string spaceKey, string[] followUsers, string[] sendEmails)
        {
            int emialCount = 0;
            if (sendEmails != null && sendEmails.Length > 0)
            {
                foreach (var item in sendEmails)
                {
                    if (item != "false")
                        emialCount++;
                }
            }
            IUser user = authenticationService.GetAuthenticatedUser();
            int followCount = 0;
            if (followUsers != null && followUsers.Length > 0)
                foreach (string userIdStr in followUsers)
                {
                    if (!userIdStr.ToLower().Equals(false.ToString().ToLower()))
                    {
                        long userId;
                        long.TryParse(userIdStr, out userId);
                        if (userId <= 0)
                            continue;
                        followService.Follow(user.UserId, userId);
                        followCount++;
                    }
                }
            List<string> emails = new List<string>();
            if (sendEmails != null && sendEmails.Length > 0)
                foreach (var item in sendEmails)
                {
                    int subscript = item.IndexOf("@");
                    if (subscript > 0 && !emails.Contains(item))
                        emails.Add(item);
                }
            if ((followUsers == null || followUsers.Length <= 0) && (emails == null || emails.Count <= 0))
                return Json(new StatusMessageData(StatusMessageType.Error, "没有新的朋友要添加"));
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            InvitationCodeStatistic invitationCodeStatistic = inviteFriendService.GetUserInvitationCodeStatistic(user.UserId);
            if (inviteFriendSettings.AllowInvitationCodeUseOnce && emialCount > invitationCodeStatistic.CodeUnUsedCount)
                return Json(new StatusMessageData(StatusMessageType.Error, "您的配额不够了哦"));
            StatusMessageType statusMessageType = StatusMessageType.Error;
            string statusMessage = "";
            if (followCount > 0)
            {
                statusMessageType = StatusMessageType.Success;
                statusMessage += string.Format("成功关注了{0}人", followCount);
            }
            if (SendEmailToContact(emails))
            {
                statusMessageType = StatusMessageType.Success;
                statusMessage += string.Format("成功的发送了邮件");
            }
            if (statusMessageType == StatusMessageType.Success)
                return Json(new StatusMessageData(StatusMessageType.Success, statusMessage));
            return Json(new StatusMessageData(StatusMessageType.Error, "操作失败"));
        }

        /// <summary>
        /// 发送邀请邮件给联系人
        /// </summary>
        /// <param name="sendEmails">准备发送的邮件</param>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        private bool SendEmailToContact(List<string> sendEmails)
        {
            if (sendEmails.Count <= 0)
                return false;
            IUser user = authenticationService.GetAuthenticatedUser();
            if (user == null)
                return false;

            EmailService emailService = new EmailService();
            foreach (var contact in sendEmails)
            {
                string code = inviteFriendService.GetInvitationCode(user.UserId);
                if (string.IsNullOrEmpty(code))
                    return false;
                string url = SiteUrls.FullUrl(SiteUrls.Instance().Invite(code));
                

                System.Net.Mail.MailMessage mailMessage = EmailBuilder.Instance().InviteFriend(contact, url, "我来了，你在那呢？");
                emailService.Enqueue(mailMessage);
            }
            return true;

        }

        /// <summary>
        /// 邀请过的好友
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>邀请过的好友页面</returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult InvitedFriends(string spaceKey, int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("我邀请过的好友");
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            List<InvitedFriendViewModel> invitedFriends = new List<InvitedFriendViewModel>();
            PagingEntityIdCollection userIds = inviteFriendService.GetMyInviteFriendRecords(userId, pageIndex ?? 1);
            IEnumerable<object> itemIds = userIds.GetPagingEntityIds(12, pageIndex ?? 1);
            foreach (var item in itemIds)
            {
                long InvitedUserId = (long)item;
                User user = userService.GetFullUser(InvitedUserId);
                UserProfile userProfile = userProfileService.Get(InvitedUserId);
                if (user == null)
                    continue;
                InvitedFriendViewModel invitedFriend = new InvitedFriendViewModel
                {
                    UserId = InvitedUserId,
                    MicroblogCount = 0,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    FollowedCount = user.FollowedCount,
                    FollowerCount = user.FollowerCount
                };
                invitedFriend.AreaName = userProfile != null && userProfile.NowAreaCode != null ? Formatter.FormatArea(userProfile.NowAreaCode, false) : "";
                invitedFriend.GenderType = user.Profile != null ? user.Profile.Gender : GenderType.NotSet;

                invitedFriends.Add(invitedFriend);
            }

            PagingDataSet<InvitedFriendViewModel> pagingDataSetInvitedFriends = new PagingDataSet<InvitedFriendViewModel>(invitedFriends)
            {
                PageIndex = pageIndex ?? 1,
                PageSize = 12,
                TotalRecords = userIds.TotalRecords
            };
            return View(pagingDataSetInvitedFriends);
        }
        #endregion

        #region 我的关注

        /// <summary>
        /// 管理关注用户
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult ManageFollowedUsers(string spaceKey, int? pageIndex, long? groupId, int sortBy = 0)
        {
            
            //已修改
            pageResourceManager.InsertTitlePart("我的关注");
            IEnumerable<Category> userCategories = null;
            List<Category> categoriesShowList = null;
            List<Category> categoriesLeftList = null;
            IEnumerable<MenuItem> menuItem = null;
            Dictionary<long, string> noteNameDic = new Dictionary<long, string>();
            Dictionary<long, bool> isMutualDic = new Dictionary<long, bool>();
            Dictionary<long, bool> isFollowedDic = new Dictionary<long, bool>();
            Dictionary<long, string> userCategoriesShowDic = new Dictionary<long, string>();

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            User user = userService.GetFullUser(userId);
            if (user != null)
                ViewData["followedCount"] = user.FollowedCount;


            ViewData["sortBy"] = (Follow_SortBy)sortBy;
            ViewData["userId"] = userId;

            string tenantTypeId = TenantTypeIds.Instance().User();

            #region 分组栏设置
            IEnumerable<Category> categories = categoryService.GetOwnerCategories(userId, tenantTypeId);
            if (categories != null)
            {
                
                //已修改
                categoriesShowList = categories.Take(4).ToList();
                if (groupId > 0 && !categories.Select(n => n.CategoryId).Contains(groupId ?? -99))
                {
                    groupId = null;
                    ViewData["noGroupId"] = true;
                }

                if (categories.Count() > 4)
                {
                    categoriesLeftList = categories.ToList().GetRange(4, categories.Count() - 4);

                    if (groupId != null && groupId > 0 && !categoriesShowList.Select(n => n.CategoryId).Contains(groupId ?? -99))
                    {
                        int index = categoriesLeftList.Select(n => n.CategoryId).ToList().IndexOf(groupId ?? -99);
                        Category midCategory = categoriesShowList[3];
                        categoriesShowList[3] = categoriesLeftList[index];
                        categoriesLeftList[index] = midCategory;
                    }
                }
            }
            ViewData["categoriesShow"] = categoriesShowList;
            if (categoriesLeftList != null)
                menuItem = categoriesLeftList.Select(n => new MenuItem { Text = n.CategoryName, Value = n.CategoryId.ToString(), Url = SiteUrls.Instance().ManageFollowedUsers(Url.SpaceKey(), n.CategoryId) });
            #endregion

            ViewData["categoriesLeft"] = menuItem;
            ViewData["groupId"] = groupId;
            PagingDataSet<long> pds = followService.GetFollowedUserIds(userId, groupId, (Follow_SortBy)sortBy == null ? Follow_SortBy.DateCreated_Desc : (Follow_SortBy)sortBy, pageIndex ?? 1);

            IEnumerable<User> users = userService.GetFullUsers(pds);
            bool isFollowed;

            #region 字典
            foreach (var id in pds)
            {
                string userCategoriesShowString = string.Empty;
                noteNameDic[id] = followService.GetNoteName(userId, id);
                isMutualDic[id] = followService.IsMutualFollowed(userId, id);
                followService.IsFollowed(userId, id, out isFollowed);
                isFollowedDic[id] = isFollowed;

                FollowEntity follow = followService.Get(userId, id);
                if (follow == null)
                    continue;

                userCategories = categoryService.GetCategoriesOfItem(follow.Id, userId, tenantTypeId);

                if (userCategories != null && userCategories.Count() > 0)
                {

                    for (int i = 0; i < (userCategories.Count() >= 2 ? 2 : userCategories.Count()); i++)
                    {
                        userCategoriesShowString = userCategories.ElementAt(userCategories.Count() - 1 - i).CategoryName + "," + userCategoriesShowString;
                    }
                    userCategoriesShowString = userCategoriesShowString.Remove(userCategoriesShowString.Count() - 1);

                }
                userCategoriesShowDic[id] = userCategoriesShowString;
            }
            ViewData["noteNameDic"] = noteNameDic;
            ViewData["isMutualDic"] = isMutualDic;
            ViewData["isFollowedDic"] = isFollowedDic;
            ViewData["userCategoriesShowDic"] = userCategoriesShowDic;
            #endregion

            PagingDataSet<User> pagingUsers = new PagingDataSet<User>(users);
            pagingUsers.PageSize = pds.PageSize;
            pagingUsers.PageIndex = pds.PageIndex;
            pagingUsers.TotalRecords = pds.TotalRecords;

            return View(pagingUsers);
        }

        /// <summary>
        /// 编辑分组控件
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _EditFollowedUsersGroup(string spaceKey, long groupId)
        {
            ViewData["spaceKey"] = spaceKey;
            ViewData["groupId"] = groupId;
            Category category = categoryService.Get(groupId);
            if (category != null)
                ViewData["groupName"] = category.CategoryName;
            return View();
        }

        /// <summary>
        /// 编辑分组
        /// </summary>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult EditFollowedUsersGroup(string spaceKey, long groupId)
        {
            Category category = null;
            
            //已修改
            string groupName = Request.Form.GetString("groupName", string.Empty).Trim();
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            if (groupId >= 0)
            {
                category = categoryService.Get(groupId);
                category.CategoryName = groupName;
                categoryService.Update(category);
            }
            else
            {
                if (!string.IsNullOrEmpty(groupName))
                {
                    category = Category.New();
                    category.TenantTypeId = TenantTypeIds.Instance().User();
                    category.CategoryName = groupName;
                    category.OwnerId = userId;
                    categoryService.Create(category);
                }
            }
            return Json(new JsonResult
            {
                Data = new
                {
                    url = SiteUrls.Instance().ManageFollowedUsers(Url.SpaceKey(), category.CategoryId)
                }
            });
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult DeleteFollowedUsersGroup(string spaceKey, long groupId)
        {

            categoryService.Delete(groupId);
            return Json(new StatusMessageData(StatusMessageType.Success, "已删除该分组！"));
        }

        /// <summary>
        /// 设置关注用户备注名控件
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _UpdateFollowedUserNoteName(string spaceKey, long followedUserId)
        {

            ViewData["spaceKey"] = spaceKey;
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            ViewData["followedUserId"] = followedUserId;
            FollowEntity entity = followService.Get(userId, followedUserId);
            if (entity != null)
                ViewData["noteName"] = entity.NoteName;
            return View();
        }

        /// <summary>
        /// 设置关注用户备注名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult UpdateFollowedUserNoteName()
        {
            string spaceKey = Request.Form.GetString("spaceKey", string.Empty);
            long followedUserId = Request.Form.Get<long>("followedUserId", 0);
            string noteName = Request.Form.GetString("noteName", string.Empty).Trim();
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            FollowEntity entity = followService.Get(userId, followedUserId);
            if (entity != null)
            {
                entity.NoteName = noteName;
                followService.Update(entity);
            }
            return Json(new JsonResult { Data = new { noteName = StringUtility.Trim(noteName, 7) } });
        }

        /// <summary>
        /// 批量添加关注控件
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _BatchFollow(string spaceKey, long groupId, int? pageIndex)
        {

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            
            //已删
            ViewData["groupId"] = groupId;
            //IEnumerable<long> allFollowedUsers = followService.GetTopFollowedUserIds(userId, 1000);
            IEnumerable<FollowEntity> allFollowesUsers = followService.GetTopFollows(userId, 1000);
            IEnumerable<FollowEntity> follows = followService.GetTopFollows(userId, 1000, groupId);

            IEnumerable<User> users = null;
            if (follows != null)
            {
                IEnumerable<FollowEntity> exceptFollowedUsers = allFollowesUsers.Except(follows);
                users = userService.GetFullUsers(exceptFollowedUsers.Select(n => n.FollowedUserId));
                ViewData["UserId_FollowIdDict"] = exceptFollowedUsers.ToDictionary(n => n.FollowedUserId, n => n.Id);
            }

            return View(users);
        }

        /// <summary>
        /// 批量添加关注
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult BatchFollow(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IEnumerable<long> followedUserIds = Request.Form.Gets<long>("CheckBoxGroup");
            //List<long> itemIds = new List<long>();
            //foreach (var followedUserId in followedUserIds)
            //{
            //    itemIds.Add(followService.Get(userId, followedUserId);
            //}
            long groupId = Request.Form.Get<long>("groupId", 0);
            categoryService.AddItemsToCategory(followedUserIds, groupId, UserIdToUserNameDictionary.GetUserId(spaceKey));
            return Json(new StatusMessageData(StatusMessageType.Success, "设置分组成功！"));
        }

        #region 批量添加关注

        /// <summary>
        /// 批量添加关注页
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="followedUserIds">被关注用户ID集合</param>
        /// <returns></returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _BatchAddFollowedUsers(string spaceKey, string followedUserIds)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            string tenantTypeId = TenantTypeIds.Instance().User();
            IEnumerable<Category> categorys = categoryService.GetOwnerCategories(userId, tenantTypeId);
            return View(categorys);
        }

        /// <summary>
        /// 批量添加关注
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="followedUserIds">被关注用户ID集合</param>
        /// <param name="categoryIds">分组ID集合</param>
        /// <returns></returns>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult _BatchAddFollowedUsers(string spaceKey, string followedUserIds, IEnumerable<long> categoryIds)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            StatusMessageData message = null;
            string ids = followedUserIds.TrimEnd(',');
            if (!string.IsNullOrEmpty(ids))
            {
                IEnumerable<long> userIds = ids.Split(',').Select(t => Convert.ToInt64(t));
                List<long> followedIds = userIds.ToList<long>();
                followService.BatchFollow(userId, followedIds);
                //依次添加到多个分组
                if (categoryIds != null && categoryIds.Count() > 0)
                {
                    foreach (var followedId in userIds)
                    {
                        FollowEntity follow = followService.Get(userId, followedId);
                        categoryService.AddCategoriesToItem(categoryIds, follow.Id, userId);

                    }
                }
                message = new StatusMessageData(StatusMessageType.Success, "批量关注成功！");
            }
            return Json(message);
        }

        #endregion
        /// <summary>
        /// 添加加关注控件
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _AddFollowedUser(string spaceKey, long followedUserId)
        {
            var currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return HttpNotFound();
            if (currentUser.UserName != spaceKey)
                return Json(new StatusMessageData(StatusMessageType.Hint, "关注失败！"), JsonRequestBehavior.AllowGet);

            bool isFollowed = currentUser.IsFollowed(followedUserId);
            long currentUserId = currentUser.UserId;
            Dictionary<long, bool> inGroup = new Dictionary<long, bool>();
            string tenantTypeId = TenantTypeIds.Instance().User();

            
            if (isFollowed)
            {
                ViewData["title"] = "编辑分组";
                FollowEntity entity = followService.Get(currentUserId, followedUserId);
                IEnumerable<Category> categoriesOfItem = categoryService.GetCategoriesOfItem(entity.Id, currentUserId, tenantTypeId);
                if (categoriesOfItem != null)
                    ViewData["categoryIdsOfItem"] = categoriesOfItem.Select(n => n.CategoryId);
            }
            
            //已修改


            IEnumerable<Category> categorys = categoryService.GetOwnerCategories(currentUserId, tenantTypeId);

            if (!isFollowed)
            {
                Dictionary<string, PrivacyStatus> privacyStatusDic = privacyService.GetUserPrivacySettings(followedUserId);

                if (privacyStatusDic.Count() > 0 && privacyStatusDic.ContainsKey(PrivacyItemKeys.Instance().Follow()) && privacyStatusDic[PrivacyItemKeys.Instance().Follow()] == PrivacyStatus.Private)
                {
                    ViewData["MessageData"] = "该用户不允许被加关注!";
                    return View();
                }

                //在我的黑名单中
                if (privacyService.GetStopedUsers(currentUserId).Keys.Contains(followedUserId))
                {
                    ViewData["MessageData"] = "您已将该用户加入黑名单，请解除黑名单后再添加关注!";
                    return View();
                }
                else
                {
                    if (privacyService.IsStopedUser(followedUserId, currentUserId))
                    {
                        ViewData["MessageData"] = "由于对方的设置，您暂时无法关注该用户!";
                        return View();
                    }
                }
            }

            ViewData["followedUserId"] = followedUserId;

            return View(categorys);
        }

        /// <summary>
        /// 添加关注-设置分组
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="followedUserId">被关注用户Id</param>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult AddFollowedUser(string spaceKey, long followedUserId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));

            long currentUserId = currentUser.UserId;
            bool result = false;
            long followId = 0;
            IEnumerable<long> categoryIds = Request.Form.Gets<long>("category");
            string noteName = Request.Form.Get("noteName", string.Empty);
            string tenantTypeId = TenantTypeIds.Instance().User();
            string editGroupShow = "编辑分组";
            IEnumerable<Category> categoriesOfItem = new List<Category>();

            if (!currentUser.IsFollowed(followedUserId))
            {
                result = followService.Follow(UserContext.CurrentUser.UserId, followedUserId);
                if (!result)
                {
                    return Json(new StatusMessageData(StatusMessageType.Hint, "关注失败！"));
                }
            }

            FollowEntity entity = followService.Get(currentUserId, followedUserId);
            followId = entity.Id;

            categoryService.ClearCategoriesFromItem(followId, currentUserId, tenantTypeId);

            if (categoryIds != null && categoryIds.Count() > 0)
            {
                categoryService.AddCategoriesToItem(categoryIds, followId, currentUserId);//为被关注用户设置分组
            }

            if (!string.IsNullOrEmpty(noteName))//更改备注名
            {
                if (entity != null)
                {
                    entity.NoteName = noteName;
                    followService.Update(entity);
                }
            }
            categoriesOfItem = categoryService.GetCategoriesOfItem(followId, currentUserId, tenantTypeId);
            if (categoriesOfItem != null && categoriesOfItem.Count() > 0)
                editGroupShow = categoriesOfItem.FirstOrDefault().CategoryName;


            return Json(new StatusMessageData(StatusMessageType.Success, editGroupShow));
            //return Json(new JsonResult { Data = new { editGroupShow = editGroupShow } });
        }

        /// <summary>
        /// 设置关注用户分组中创建分组
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="groupName">新建分组名称</param>
        /// <param name="followedUserId">被关注用户Id</param>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult CreateNewGroup(string spaceKey, string groupName, long? followedUserId = null)
        {
            if (UserContext.CurrentUser == null)
                return HttpNotFound();
            Category category = null;
            if (!string.IsNullOrEmpty(groupName))
            {
                category = Category.New();
                category.TenantTypeId = TenantTypeIds.Instance().User();
                category.CategoryName = groupName;
                category.OwnerId = UserContext.CurrentUser.UserId;
                categoryService.Create(category);
            }
            IEnumerable<Category> categorys = categoryService.GetOwnerCategories(UserContext.CurrentUser.UserId, TenantTypeIds.Instance().User());
            ViewData["CheckedGroup"] = category.CategoryId;
            ViewData["followedUserId"] = followedUserId;

            return Json(new JsonResult { Data = new { groupId = category.CategoryId, groupName = groupName } });
        }

        /// <summary>
        /// 关注用户列表
        /// </summary>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = false)]
        public ActionResult ListFollowedUsers(string spaceKey, int sortBy = 0, int pageIndex = 1)
        {
            

            IUser currentUser = UserContext.CurrentUser;
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            if ((currentUser == null ? 0 : currentUser.UserId) == userId)
                return RedirectToAction("ManageFollowedUsers", new { spaceKey = spaceKey });

            UserProfile userProfile = userProfileService.Get(userId);
            if (userProfile != null)
            {
                if (userProfile.Gender == GenderType.FeMale)
                    pageResourceManager.InsertTitlePart("她的关注");
                else
                    pageResourceManager.InsertTitlePart("他的关注");

                ViewData["Gender"] = userProfile.Gender;
            }

            Dictionary<long, bool> isCurrentUserFollowDic = new Dictionary<long, bool>();

            PagingDataSet<long> userFollowedUserIds = followService.GetFollowedUserIds(userId, null, (Follow_SortBy)sortBy, pageIndex);
            
            //已修改
            foreach (var id in userFollowedUserIds)
            {
                if (currentUser != null)
                {
                    isCurrentUserFollowDic[id] = followService.IsFollowed(currentUser.UserId, id);
                }
            }
            ViewData["isCurrentUserFollowDic"] = isCurrentUserFollowDic;

            IEnumerable<User> users = userService.GetFullUsers(userFollowedUserIds.ToList());


            PagingDataSet<User> pagingUsers = new PagingDataSet<User>(users);
            pagingUsers.PageSize = userFollowedUserIds.PageSize;
            pagingUsers.PageIndex = userFollowedUserIds.PageIndex;
            pagingUsers.TotalRecords = userFollowedUserIds.TotalRecords;
            ViewData["sortBy"] = (Follow_SortBy)sortBy;
            return View(pagingUsers);
        }

        /// <summary>
        /// 我的关注页面 设置分组下拉
        /// </summary>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _SetGroupForUser(string spaceKey, long followUserId, long? groupId)
        {
            IEnumerable<Category> userCategoriesAddDic = new List<Category>();
            IEnumerable<Category> userCategories = new List<Category>();

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            IEnumerable<Category> categories = categoryService.GetOwnerCategories(userId, TenantTypeIds.Instance().User());//取我的所有分组
            FollowEntity follow = followService.Get(userId, followUserId);
            userCategories = categoryService.GetCategoriesOfItem(follow == null ? 0 : follow.Id, userId, TenantTypeIds.Instance().User());//关注的用户在我的哪些分组
            userCategoriesAddDic = categories.Where(n => n.CategoryId > 0);

            ViewData["userCategoriesAddDic"] = userCategoriesAddDic;
            ViewData["userCategories"] = userCategories;
            ViewData["followId"] = follow.Id;
            ViewData["userId"] = followUserId;
            ViewData["groupId"] = groupId;

            return View();
        }

        /// <summary>
        /// 在下拉列表中 设置关注用户分组中创建分组
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="groupName">新建分组名称</param>
        /// <param name="followedUserId">被关注用户Id</param>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult CreateNewGroupInList(string spaceKey, string groupName, long followedUserId)
        {
            //if (UserContext.CurrentUser == null)
            //    return HttpNotFound();
            Category category = null;
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (!string.IsNullOrEmpty(groupName))
            {
                category = Category.New();
                category.TenantTypeId = TenantTypeIds.Instance().User();
                category.CategoryName = groupName;
                category.OwnerId = UserContext.CurrentUser.UserId;
                categoryService.Create(category);
            }

            IList<long> categoryIds = new List<long>();
            categoryIds.Add(category.CategoryId);
            FollowEntity follow = followService.Get(userId, followedUserId);
            categoryService.AddCategoriesToItem(categoryIds, follow.Id, userId);//为被关注用户设置分组

            IEnumerable<Category> userCategoriesAddDic = new List<Category>();
            IEnumerable<Category> userCategories = new List<Category>();

            

            IEnumerable<Category> categories = categoryService.GetOwnerCategories(userId, TenantTypeIds.Instance().User());//取我的所有分组

            userCategories = categoryService.GetCategoriesOfItem(follow.Id, userId, TenantTypeIds.Instance().User());//关注的用户在我的哪些分组
            userCategoriesAddDic = categories.Where(n => n.CategoryId > 0);

            ViewData["userCategoriesAddDic"] = userCategoriesAddDic;
            ViewData["userCategories"] = userCategories;
            ViewData["CheckedGroup"] = category.CategoryId;
            ViewData["userId"] = followedUserId;

            string newShow = null;
            if (!string.IsNullOrEmpty(groupName))
            {
                newShow = groupName;
                if (userCategories != null && userCategories.Count() > 1)
                {
                    newShow = userCategories.ToList()[userCategories.Count() - 2].CategoryName + "," + newShow;
                }
            }
            ViewData["newShow"] = newShow;
            ViewData["newGroupName"] = groupName;

            //return View("_SetGroupForUser");

            return Json(new JsonResult { Data = new { newGroupName = groupName, CheckedGroup = category.CategoryId, userId = followedUserId, newShow = newShow, count = userCategoriesAddDic.Count(), url = SiteUrls.Instance().ManageFollowedUsers(Url.SpaceKey(), category.CategoryId) } });
        }

        /// <summary>
        /// 为用户添加分组或者取消分组
        /// </summary>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult SetGroupForFollowedUser(string spaceKey, int userGroupId, long followedUserId, long followId, bool isChecked)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IList<long> categoryIds = new List<long>();
            categoryIds.Add(userGroupId);
            string newShow = null;

            if (isChecked)
                categoryService.AddCategoriesToItem(categoryIds, followId, userId);//为被关注用户设置分组
            else
                categoryService.DeleteItemInCategory(userGroupId, followId, userId);

            IEnumerable<Category> userCategoriesAddDic = new List<Category>();
            IEnumerable<Category> userCategories = new List<Category>();
            IEnumerable<Category> categories = categoryService.GetOwnerCategories(userId, TenantTypeIds.Instance().User());//取我的所有分组
            FollowEntity follow = followService.Get(userId, followedUserId);
            userCategories = categoryService.GetCategoriesOfItem(follow.Id, userId, TenantTypeIds.Instance().User());//关注的用户在我的哪些分组
            userCategoriesAddDic = categories.Where(n => n.CategoryId > 0);

            ViewData["userCategoriesAddDic"] = userCategoriesAddDic;
            ViewData["userCategories"] = userCategories;
            ViewData["followId"] = followId;
            ViewData["followedUserId"] = followedUserId;

            
            //首先，判断是否是添加还是删除
            if (userCategories != null && userCategories.Count() > 0)
            {
                for (int i = 0; i < (userCategories.Count() >= 2 ? 2 : userCategories.Count()); i++)
                {
                    newShow = userCategories.ElementAt(userCategories.Count() - 1 - i).CategoryName + "," + newShow;
                }

                newShow = newShow.Remove(newShow.Count() - 1);
            }

            if (string.IsNullOrEmpty(newShow))
            {
                newShow = "未分组";
            }

            return Json(new JsonResult { Data = new { newShow = newShow, userId = followedUserId } });
        }

        /// <summary>
        /// 悄悄关注
        /// </summary>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult QuietlyFollow(string spaceKey, long? followedUserId)
        {
            if (string.IsNullOrEmpty(spaceKey) || !followedUserId.HasValue)
            {
                return Json(new { message = "悄悄关注失败!" });
            }

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                return Json(new { message = "您尚未登录，请先登录!" });
            }
            else if (new PrivacyService().IsStopedUser(currentUser.UserId, followedUserId.Value))
            {
                return Json(new { message = "您已将该用户加入黑名单，请解除黑名单后再添加关注!" });
            }
            else if (new Authorizer().Follow(followedUserId.Value))
            {
                followService.Follow(userId, followedUserId ?? 0, true);
                return Json(new { success = true, message = "悄悄关注成功" });
            }

            return Json(new { message = "由于空间主人的隐私设置或程序错误导致悄悄关注失败!" });
        }



        #endregion

        #region 我的粉丝

        /// <summary>
        /// 管理我的粉丝页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="sortBy">搜索条件</param>
        /// <param name="pageIndex">当前页码</param>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult ManageFollowers(string spaceKey, int sortBy = 0, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("我的粉丝");
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<long> followerUserIds = followService.GetFollowerUserIds(userId, (Follow_SortBy)sortBy, pageIndex);
            IEnumerable<User> listUsers = userService.GetFullUsers(followerUserIds);

            User user = userService.GetFullUser(userId);
            if (user != null)
                ViewData["followerCount"] = user.FollowerCount;

            PagingDataSet<User> followerUsers = new PagingDataSet<User>(listUsers);
            followerUsers.PageIndex = followerUserIds.PageIndex;
            followerUsers.PageSize = followerUserIds.PageSize;
            followerUsers.TotalRecords = followerUserIds.TotalRecords;

            followService.ClearNewFollowerCount(userId);//清除最新粉丝数

            ViewData["sortBy"] = (Follow_SortBy)sortBy;
            return View(followerUsers);
        }

        /// <summary>
        /// 查看用户粉丝页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="sortBy">搜索条件</param>
        /// <param name="pageIndex">当前页码</param>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = false)]
        public ActionResult ListFollowers(string spaceKey, int sortBy = 0, int pageIndex = 1)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            long currentUserId = UserContext.CurrentUser == null ? 0 : UserContext.CurrentUser.UserId;

            if (currentUserId == userId)
                return RedirectToAction("ManageFollowers", new { spaceKey = spaceKey });

            UserProfile currentUser = userProfileService.Get(userId);
            if (currentUser != null)
            {
                if (currentUser.Gender == GenderType.FeMale)
                    pageResourceManager.InsertTitlePart("她的粉丝");
                else
                    pageResourceManager.InsertTitlePart("他的粉丝");

                ViewData["Gender"] = currentUser.Gender;
            }

            PagingDataSet<long> followerUserIds = followService.GetFollowerUserIds(userId, (Follow_SortBy)sortBy, pageIndex);
            IEnumerable<User> listUsers = userService.GetFullUsers(followerUserIds.ToList());

            PagingDataSet<User> followerUsers = new PagingDataSet<User>(listUsers);
            followerUsers.PageIndex = followerUserIds.PageIndex;
            followerUsers.PageSize = followerUserIds.PageSize;
            followerUsers.TotalRecords = followerUserIds.TotalRecords;

            ViewData["sortBy"] = (Follow_SortBy)sortBy;
            return View(followerUsers);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="followedUserId">被关注用户Id</param>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult CancelFollow(string spaceKey, long followedUserId)
        {
            if (UserContext.CurrentUser != null)
            {
                followService.CancelFollow(UserContext.CurrentUser.UserId, followedUserId);
            }
            return Json(new JsonResult { Data = new { followId = followedUserId } });
        }

        /// <summary>
        /// 黑名单
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="stopedUserId">被设为黑名单的用户Id</param>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult CreateStopedUser(string spaceKey, long stopedUserId)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            IUser user = userService.GetUser(stopedUserId);
            if (user != null)
            {
                StopedUser stopedUser = new StopedUser
                {
                    ToUserDisplayName = user.DisplayName,
                    ToUserId = user.UserId,
                    UserId = userId
                };
                privacyService.CreateStopedUser(stopedUser);
            }
            return new EmptyResult();
        }

        #endregion

        /// <summary>
        /// 用与显示SpaceHome控件中的更多
        /// </summary>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = false)]
        public ActionResult SpaceHomeMore(string spaceKey, int type = 0)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Redirect(SiteUrls.Instance().Login(true));
            GenderType gender = GenderType.NotSet;
            if (user.Profile != null)
                gender = user.Profile.Gender;
            ViewData["type"] = (SpaceHomeMoreType)type;
            IEnumerable<long> ids = new List<long>();
            string showString = string.Empty;
            switch ((SpaceHomeMoreType)type)
            {
                case SpaceHomeMoreType.FollowedUsersFromUser:
                    ids = followService.GetTopFollowedUserIdsFromUser(currentUser.UserId, user.UserId, 50);
                    if (gender == GenderType.FeMale)
                        showString = "我关注的人也关注了她";
                    else
                        showString = "我关注的人也关注了他";
                    break;

                case SpaceHomeMoreType.FollowedUsersOfFollowers:
                    ids = followService.GetTopFollowedUserIdsOfFollowers(user.UserId, currentUser.UserId, 50);
                    showString = "关注" + user.DisplayName + "的人同时关注了这些人";
                    break;

                case SpaceHomeMoreType.TogetherFollowedUsers:
                    ids = followService.GetTogetherFollowedUserIds(currentUser.UserId, user.UserId, 50);
                    showString = "我和" + user.DisplayName + "共同关注了";
                    break;
            }
            ViewData["showString"] = showString;
            IEnumerable<User> users = new List<User>();
            if (ids != null)
                users = userService.GetFullUsers(ids);
            return View(users);
        }

        /// <summary>
        /// 移除粉丝
        /// </summary>
        [HttpPost]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult RemoveFollower(string spaceKey, long followerId)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            followService.RemoveFollower(userId, followerId);
            return Json(null);
        }

        /// <summary>
        /// 添加关注话题
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult _AddFollowedTopic(string spaceKey, long tagId = 0)
        {
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请先登录！"), JsonRequestBehavior.AllowGet);
            }

            FavoriteService FavoriteService = new FavoriteService(TenantTypeIds.Instance().Tag());
            if (!FavoriteService.IsFavorited(tagId, CurrentUser.UserId))
            {
                FavoriteService.Favorite(tagId, CurrentUser.UserId);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "添加关注成功！"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消关注话题
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public JsonResult _CancelFollowedTopic(string spaceKey, long tagId = 0)
        {
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请先登录！"), JsonRequestBehavior.AllowGet);
            }

            FavoriteService FavoriteService = new FavoriteService(TenantTypeIds.Instance().Tag());
            if (FavoriteService.IsFavorited(tagId, CurrentUser.UserId))
            {
                FavoriteService.CancelFavorite(tagId, CurrentUser.UserId);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "取消关注成功！"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 关注用户动态
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public ActionResult _FollowUserActivity(long activityId)
        {
            ActivityService activityService = new ActivityService();
            Activity activity = activityService.Get(activityId);

            if (activity == null)
                return Content(string.Empty);

            IUser user = userService.GetUser(activity.OwnerId);
            if (user == null)
                return Content(string.Empty);

            IUser currentUser = UserContext.CurrentUser;
            IEnumerable<long> followedUserIds = followService.GetTopFollowedUserIds(user.UserId, 3, sortBy: Follow_SortBy.DateCreated_Desc);
            List<User> followedUsers = new List<User>();

            Dictionary<long, bool> isCurrentUserFollowDic = new Dictionary<long, bool>();
            foreach (long userId in followedUserIds)
            {
                IUser tempUser = userService.GetUser(userId);
                if (tempUser == null)
                    continue;

                if (followService.IsFollowed(currentUser == null ? 0 : currentUser.UserId, tempUser.UserId))
                {
                    isCurrentUserFollowDic[tempUser.UserId] = true;
                }
                else
                {
                    isCurrentUserFollowDic[tempUser.UserId] = false;
                }
            }

            ViewData["isCurrentUserFollowDic"] = isCurrentUserFollowDic;
            ViewData["FollowUsers"] = followedUsers;
            ViewData["ActivityUserId"] = activity.UserId;

            return View(activity);
        }

        /// <summary>
        /// 关注用户菜单
        /// </summary>
        /// <param name="subMenu"></param>
        /// <returns></returns>
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult _FollowUser_Menu(FollowUserMenu menu)
        {
            UserSettings userSettings = DIContainer.Resolve<IUserSettingsManager>().Get();
            ViewData["RegistrationMode"] = userSettings.RegistrationMode;
            ViewData["FollowUserMenu"] = menu;

            return View();
        }

    }

    /// <summary>
    /// 用与获取SpaceHome控件中的更多的类型
    /// </summary>
    public enum SpaceHomeMoreType
    {
        /// <summary>
        /// 他的粉丝也关注了
        /// </summary>
        FollowedUsersOfFollowers = 0,

        /// <summary>
        /// 这些人也关注了他
        /// </summary>
        FollowedUsersFromUser = 1,

        /// <summary>
        /// 我和他都关注了
        /// </summary>
        TogetherFollowedUsers = 2
    }

    /// <summary>
    /// 用与获取SpaceHome控件中的更多的类型
    /// </summary>
    public enum FollowUserMenu
    {
        /// <summary>
        /// 他的粉丝关注
        /// </summary>
        MyFolloweds,

        /// <summary>
        /// 他的粉丝
        /// </summary>
        MyFollowers,

        /// <summary>
        /// 邀请好友
        /// </summary>
        InviteFriend
    }
}