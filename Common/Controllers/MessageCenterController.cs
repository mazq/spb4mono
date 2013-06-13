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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 信息中心Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.UserSpace, IsApplication = false)]
    [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
    public class MessageCenterController : Controller
    {
        #region private item

        private IUserService userService = DIContainer.Resolve<IUserService>();
        private MessageService messageService = new MessageService();
        private FollowService followService;
        private NoticeService noticeService;
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private InvitationService invitationService = DIContainer.Resolve<InvitationService>();
        private IAuthenticationService authenticationService = DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
        private CommentService commentService;
        private ReminderService reminderService = new ReminderService();
        private IReminderSettingsManager reminderSettingsManager = DIContainer.Resolve<IReminderSettingsManager>();
        private TenantTypeService tenantTypeService = new TenantTypeService();

        #endregion private item

        public MessageCenterController()
            : this(new MessageService(), new NoticeService(), new FollowService(), new CommentService())
        {
        }

        public MessageCenterController(MessageService messageService, NoticeService noticeService, FollowService followService, CommentService commentService)
        {
            this.messageService = messageService;
            this.noticeService = noticeService;
            this.followService = followService;
            this.commentService = commentService;
        }

        #region 私信

        /// <summary>
        /// 显示提示私信对话列表
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="itemCount">获取记录条数</param>
        public ActionResult _ListPromptMessages(string spaceKey, int itemCount = 3)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            Dictionary<long, Message> messages = new Dictionary<long, Message>();
            IEnumerable<MessageSession> sessions = messageService.GetTopSessions(userId, itemCount, true);
            if (sessions != null)
            {
                foreach (var session in sessions)
                {
                    Message message = messageService.Get(session.LastMessageId);
                    if (message != null)
                    {
                        messages[session.SessionId] = message;
                    }
                }
            }

            ViewData["Unread"] = messageService.GetUnreadCount(userId);
            ViewData["messages"] = messages;

            return View(sessions);
        }

        /// <summary>
        /// 私信对话列表
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="pageIndex">页码</param>
        public ActionResult ListMessageSessions(string spaceKey, int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("我的私信");

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            Dictionary<long, string> noteNames = new Dictionary<long, string>();

            PagingDataSet<MessageSession> messageSessions = messageService.GetSessionsOfUser(userId, pageIndex ?? 1);

            Dictionary<long, Message> messageDict = new Dictionary<long, Message>();
            messageSessions.ToList().ForEach(n =>
            {
                if (n.LastMessageId > 0)
                {
                    Message message = messageService.Get(n.LastMessageId);
                    if (message != null)
                    {
                        messageDict[n.SessionId] = message;
                        if (message.SenderUserId == userId)
                        {
                            noteNames[message.ReceiverUserId] = followService.GetNoteName(message.SenderUserId, message.ReceiverUserId);
                        }
                    }
                }
            });

            ViewData["userId"] = userId;
            ViewData["MessageDict"] = messageDict;
            ViewData["NoteNames"] = noteNames;

            return View(messageSessions);
        }

        /// <summary>
        /// 私信局部页
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="messageId">私信Id</param>
        /// <param name="sessionId">会话Id</param>
        /// <param name="isFrame">是否有边框</param>
        /// <returns></returns>
        public ActionResult _Message(string spaceKey, long messageId, long sessionId, bool isFrame = false)
        {
            Message message = messageService.Get(messageId);
            ViewData["isFrame"] = isFrame;
            ViewData["sessionId"] = sessionId;
            return View(message);
        }

        /// <summary>
        /// 私信局部列表
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="sessionId">私信会话Id</param>
        /// <param name="isShowAll">是否显示全部</param>
        /// <returns></returns>
        public ActionResult _ListMessages(string spaceKey, long sessionId, long userId, bool isShowAll = false)
        {
            //获取私信会话实体
            MessageSession messageSession = messageService.GetSession(sessionId);
            IEnumerable<Message> iMessages = null;

            if (isShowAll)
            {
                List<Message> messages = messageService.GetTops(sessionId, messageSession.MessageCount).ToList();
                iMessages = messages.Skip<Message>(50);
            }
            else
            {
                iMessages = messageService.GetTops(sessionId, 50);
            }

            IUser user = userService.GetUser(userId);

            ViewData["session"] = messageSession;
            ViewData["userId"] = userId;
            ViewData["otherUser"] = userService.GetUser(messageSession.OtherUserId);


            return View(iMessages);
        }

        /// <summary>
        /// 私信列表
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="sessionId">私信会话Id</param>
        [HttpGet]
        public ActionResult ListMessages(string spaceKey, long sessionId)
        {


            MessageSession messageSession = messageService.GetSession(sessionId);
            if (messageSession == null)
                messageSession = new MessageSession();

            IUser user = userService.GetUser(messageSession.OtherUserId);
            string otherUserName = string.Empty;
            if (user == null)
            {
                if (messageSession.OtherUserId == (long)BuildinMessageUserId.CustomerService)
                {
                    otherUserName = "客服消息";
                }
                else
                {
                    otherUserName = "";
                }
            }
            else
            {
                otherUserName = user.DisplayName;
            }


            pageResourceManager.InsertTitlePart("我和\"" +otherUserName + "\"的私信会话");


            ViewData["displayName"] = user != null ? user.DisplayName : "";
            ViewData["session"] = messageSession;
            ViewData["messages"] = messageService.GetTops(sessionId, 10);
            messageService.SetIsRead(sessionId, messageSession.UserId);
            return View();
        }

        /// <summary>
        /// 创建私信
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="toUserId">收件人用户Id</param>
        [HttpGet]
        public ActionResult _CreateMessage(string spaceKey, long? toUserId)
        {
            MessageEditModel messageEditModel = new MessageEditModel();

            if (toUserId != null && toUserId.Value > 0)
            {
                User user = userService.GetFullUser(toUserId.Value);
                if (user != null)
                {
                    if (!new Authorizer().Message(user.UserId))
                        return Json(new StatusMessageData(StatusMessageType.Hint, "该用户不允许你发私信！"), JsonRequestBehavior.AllowGet);
                    ViewData["ToUserDisplayName"] = user.DisplayName;
                }
            }
            if (toUserId == (long)BuildinMessageUserId.CustomerService)
            {
                ViewData["ToUserDisplayName"] = "客服消息";
            }

            IMessageSettingsManager messageSettingsManager = DIContainer.Resolve<IMessageSettingsManager>();
            ViewData["maxReceiver"] = messageSettingsManager.Get().MaxReceiver;

            return View(messageEditModel);
        }

        /// <summary>
        /// 创建私信表单提交
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="model">用于表单提交的实体</param>
        [HttpPost]
        public ActionResult _CreateMessage(string spaceKey, MessageEditModel model)
        {
            User currentSpaceUser = (User)UserContext.CurrentUser;
            if (currentSpaceUser == null)
                return HttpNotFound();
            IEnumerable<long> toUserIds = Request.Form.Gets<long>("ToUserIds");
            Message message = null;
            foreach (var toUserId in toUserIds)
            {
                //区别客服消息和用户
                if (toUserId == (long)BuildinMessageUserId.CustomerService)
                {
                    message = model.AsMessage();
                    message.MessageType = MessageType.Common;
                    message.Receiver = "客服消息";
                    message.ReceiverUserId = (long)BuildinMessageUserId.CustomerService;
                    message.SenderUserId = currentSpaceUser.UserId;
                    message.Sender = currentSpaceUser.DisplayName;
                    bool value = messageService.Create(message);
                }
                else
                {
                    User toUser = userService.GetFullUser(toUserId);
                    if (new Authorizer().Message(toUser.UserId))
                    {
                        message = model.AsMessage();
                        message.MessageType = MessageType.Common;
                        message.Receiver = toUser.DisplayName;
                        message.ReceiverUserId = toUser.UserId;
                        message.SenderUserId = currentSpaceUser.UserId;
                        message.Sender = currentSpaceUser.DisplayName;
                        bool value = messageService.Create(message);
                    }
                }
            }
            //IEnumerable<User> toUsers = userService.GetFullUsers(toUserIds);
            //Message message = null;
            //foreach (var toUser in toUsers)
            //{
            //    if (new Authorizer().Message(toUser.UserId))
            //    {
            //        message = model.AsMessage();
            //        message.MessageType = MessageType.Common;
            //        message.Receiver = toUser.DisplayName;
            //        message.ReceiverUserId = toUser.UserId;
            //        message.SenderUserId = currentSpaceUser.UserId;
            //        message.Sender = currentSpaceUser.DisplayName;
            //        bool value = messageService.Create(message);
            //    }
            //}
            if (message != null)
                return Json(new { messageId = message.MessageId });
            else
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Hint, "您没有权限对此用户发送私信");
            return Json(new { messageId = 0, hint = new StatusMessageData(StatusMessageType.Hint, "您没有权限对此用户发送私信") });
        }

        /// <summary>
        /// 异步删除私信对话
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="sessionId">对话ID</param>
        [HttpPost]
        public JsonResult DeleteMessageSessionAsyn(string spaceKey, long sessionId)
        {
            if (messageService.DeleteSession(sessionId))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }

            return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        /// <summary>
        /// 删除私信对话
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="sessionId">会话Id</param>
        /// <returns></returns>
        public ActionResult DeleteMessageSession(string spaceKey, long sessionId)
        {
            messageService.DeleteSession(sessionId);
            return RedirectToAction("ListMessageSessions", new { pageIndex = 1 });
        }

        /// <summary>
        /// 删除私信
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="messageId">私信Id</param>
        /// <param name="sessionId">会话Id</param>
        [HttpPost]
        public JsonResult DeleteMessage(string spaceKey, long messageId, long sessionId)
        {
            if (messageService.Delete(messageId, sessionId))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }

            return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        /// <summary>
        /// 获取未读死心数
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUnreadCount(string spaceKey, long userId)
        {
            return Json(new { UnreadCount = messageService.GetUnreadCount(userId) });
        }

        #endregion 私信

        #region Invitation 请求

        
        

        
        

        
        
        /// <summary>
        /// 请求列表页
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>请求列表页</returns>
        public ActionResult ListInvitations(string spaceKey, int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("请求列表");
            
            
            
            
            
            
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<Invitation> invitations = invitationService.Gets(userId, null, string.Empty, null, pageIndex);
            if (invitations.PageCount < pageIndex)
                return ListInvitations(spaceKey, invitations.PageCount);
            ViewData["UnhandledCount"] = invitationService.GetUnhandledCount(userId);

            
            

            if (Request.IsAjaxRequest())
                return PartialView("_ListInvitation", invitations);
            return View(invitations);
        }

        #region 写入类操作

        
        /// <summary>
        /// 删除请求
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>处理之后的请求页面</returns>
        [HttpPost]
        public ActionResult DeleteInvitation(string spaceKey, int? pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("批量删除");
            IEnumerable<string> invitationIds = Request.Form.Get("invitationIds", string.Empty).Split(new string[] { "=", "&", "invitationIds", "false", "," }, StringSplitOptions.RemoveEmptyEntries);
            IUser user = authenticationService.GetAuthenticatedUser();
            InvitationService invitationService = DIContainer.Resolve<InvitationService>();
            if (invitationIds == null || invitationIds.Count() == 0)
                return Redirect(SiteUrls.Instance().ListInvitations(spaceKey, pageIndex, true));
            foreach (string idstr in invitationIds)
            {
                long id;
                long.TryParse(idstr, out id);
                
                
                Invitation invitation = invitationService.Get(id);
                if (invitation.UserId != user.UserId)
                    continue;
                invitationService.Delete(id);
            }
            return ListInvitations(spaceKey, pageIndex);
        }

        
        
        /// <summary>
        /// 批量设置请求状态
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="invitationStatus">邀请状态</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>处理之后的请求页</returns>
        [HttpPost]
        public ActionResult BatchSetStatus(string spaceKey, InvitationStatus invitationStatus = InvitationStatus.Accept, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("批量设置状态");
            IEnumerable<string> invitationIds = Request.Form.Get("invitationIds", string.Empty).Split(new string[] { "=", "&", "invitationIds", "false", "," }, StringSplitOptions.RemoveEmptyEntries);
            IUser user = authenticationService.GetAuthenticatedUser();
            
            
            foreach (string idstr in invitationIds)
            {
                long id;
                long.TryParse(idstr, out id);
                
                
                SetStatus(id, user.UserId, invitationStatus);
            }
            return ListInvitations(spaceKey, pageIndex);
        }

        /// <summary>
        /// 气泡中批量接受请求的方法
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="status">被设置的状态</param>
        /// <returns>设置是否成功</returns>
        [HttpPost]
        public ActionResult PrompSetInvitations(string spaceKey, InvitationStatus status)
        {
            string[] invitationIds = Request.QueryString.Get("invitationIds", string.Empty).Split(new string[] { "=", "&", "invitationIds", "false", "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in invitationIds)
            {
                long id;
                long.TryParse(item, out id);
                SetStatus(id, UserContext.CurrentUser.UserId, status);
            }
            return _ListPrompt(spaceKey);
        }

        /// <summary>
        /// 设置一条信息的状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _InvitationSetStatus(string spaceKey, long id, InvitationStatus status)
        {
            pageResourceManager.InsertTitlePart("设置状态");
            if (UserContext.CurrentUser == null)
                return HttpNotFound();
            
            
            InvitationService invitationService = DIContainer.Resolve<InvitationService>();
            Invitation invitation = invitationService.Get(id);
            User user = authenticationService.GetAuthenticatedUser() as User;
            SetStatus(id, user.UserId, status);
            
            
            
            
            ViewData["UnhandledCount"] = invitationService.GetUnhandledCount(UserIdToUserNameDictionary.GetUserId(spaceKey));
            return PartialView(invitation);
        }

        #endregion 写入类操作

        #region 局部视图

        /// <summary>
        /// 局部页面显示一条请求
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Invitation(string spaceKey, long id)
        {
            
            
            InvitationService invitationService = DIContainer.Resolve<InvitationService>();
            
            
            Invitation invitation = invitationService.Get(id);
            IUser user = authenticationService.GetAuthenticatedUser();
            if (invitation == null || invitation.UserId != user.UserId)
                return HttpNotFound();
            return PartialView(invitation);
        }

        /// <summary>
        /// 显示提示请求对话列表
        /// </summary>
        /// <returns></returns>
        public ActionResult _ListPromptInvitation(string spaceKey, int itemCount = 3)
        {
            IUser user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            InvitationService invitationService = DIContainer.Resolve<InvitationService>();
            
            
            return PartialView(invitationService.GetTops(user.UserId, itemCount));
        }

        #endregion 局部视图

        #region 用户设置

        /// <summary>
        /// 显示请求的设置页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _UserInvitationSettings(string spaceKey)
        {
            IUser user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            
            
            Dictionary<string, bool> userInvitationSettingses = invitationService.GetUserInvitationSettingses(user.UserId);
            IInvitationSettingsManager invitationSettingsManager = DIContainer.Resolve<IInvitationSettingsManager>();
            InvitationSettings settings = invitationSettingsManager.Get();
            Dictionary<string, bool> mergeUserSettingses = new Dictionary<string, bool>();
            foreach (var item in settings.InvitationTypeSettingses)
            {
                if (userInvitationSettingses != null && userInvitationSettingses.ContainsKey(item.TypeKey))
                    mergeUserSettingses[item.TypeKey] = userInvitationSettingses[item.TypeKey];
                else
                    mergeUserSettingses[item.TypeKey] = item.IsAllow;
            }
            return PartialView(mergeUserSettingses);
        }

        /// <summary>
        /// 更新用户请求设置
        /// </summary>
        /// <param name="typeKey2IsAllowDictionary">用户请求设置</param>
        /// <returns>是否成功更新了请求</returns>
        [HttpPost]
        public JsonResult _SetUserInvitationSettings(string spaceKey)
        {
            
            
            IUser userSpace = userService.GetFullUser(spaceKey);
            if (userSpace == null)
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到用户"));
            }

            IInvitationSettingsManager invitationSettingsManager = DIContainer.Resolve<IInvitationSettingsManager>();
            InvitationSettings settings = invitationSettingsManager.Get();
            //Dictionary<string, bool> userInvitationSettingses = settings.TypeKey2IsAllowDictionary();
            IUser user = authenticationService.GetAuthenticatedUser();
            if (user.UserId != userSpace.UserId)
            {
                WebUtility.SetStatusCodeForError(Response);
                return Json(new StatusMessageData(StatusMessageType.Error, "没有权限"));
            }
            Dictionary<string, bool> newUserInvitationSettingses = new Dictionary<string, bool>();
            foreach (var item in settings.InvitationTypeSettingses)
                newUserInvitationSettingses[item.TypeKey] = Request.Form.Get<bool>(item.TypeKey, true);
            invitationService.UpdateUserInvitationSettings(userSpace.UserId, newUserInvitationSettingses);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        #endregion 用户设置

        #region 私有方法

        /// <summary>
        /// 批量设置请求状态的方法
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <param name="invitationIds">被设置的Id集合</param>
        /// <param name="invitationStatus">要设置成的状态</param>
        private void BatchSetStatus(IEnumerable<long> invitationIds, User user, InvitationStatus status)
        {
            foreach (int item in invitationIds)
                SetStatus(item, user.UserId, status);
        }

        /// <summary>
        /// 设置一条信息状态
        /// </summary>
        /// <param name="invitationId">请求id</param>
        /// <param name="userId">当前用户</param>
        /// <param name="status">状态</param>
        private void SetStatus(long invitationId, long userId, InvitationStatus status)
        {
            Invitation invitation = invitationService.Get(invitationId);
            if (invitation == null || invitation.UserId != userId || invitation.Status != InvitationStatus.Unhandled)
                return;
            invitationService.SetStatus(invitationId, status);
        }

        #endregion 私有方法

        #endregion Invitation 请求

        #region 通知

        /// <summary>
        /// 通知页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="status">通知状态</param>
        /// <param name="pageIndex">页码</param>
        [HttpGet]
        public ActionResult ListNotices(string spaceKey, NoticeStatus? status, int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("通知");

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<Notice> notices = noticeService.Gets(userId, status, null, null, pageIndex ?? 1);

            int unhandledCount = noticeService.GetUnhandledCount(userId);
            ViewData["unhandledCount"] = unhandledCount;
            ViewData["status"] = status;
            ViewData["pageIndex"] = pageIndex;

            return View(notices);
        }

        /// <summary>
        /// 通知列表
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="status">通知状态</param>
        /// <param name="pageIndex">页码</param>
        [HttpGet]
        public ActionResult _ListNotices(string spaceKey, NoticeStatus? status, int? pageIndex)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            PagingDataSet<Notice> notices = noticeService.Gets(userId, status, null, null, pageIndex ?? 1);
            ViewData["userId"] = userId;
            ViewData["status"] = status;
            ViewData["pageIndex"] = pageIndex;
            return View(notices);
        }

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="noticeId">通知ID</param>
        [HttpPost]
        public JsonResult DeleteNotice(string spaceKey, long noticeId)
        {
            noticeService.Delete(noticeId);
            if (noticeId > 0)
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        /// <summary>
        /// 将通知设为已读
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="noticeId">通知ID</param>
        /// <param name="status">通知状态</param>
        /// <param name="pageIndex">页码</param>
        [HttpGet]
        public ActionResult SetIsHandled(string spaceKey, long noticeId, NoticeStatus? status, int? pageIndex)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            Notice notice = noticeService.Get(noticeId);
            if (notice != null)
                noticeService.SetIsHandled(noticeId);

            return RedirectToAction("_ListNotices", new { spaceKey = spaceKey, status = status, pageIndex = pageIndex });
        }

        /// <summary>
        /// 将通知全部设为已读
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="status">通知状态</param>
        [HttpPost]
        public ActionResult SetAllNoticeIsHandled(string spaceKey, NoticeStatus? status)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            string selectedNoticeIDsString = Request.Form["noticeId"];
            if (!string.IsNullOrEmpty(selectedNoticeIDsString))
            {
                string[] selectedNoticeIDsStringArray = selectedNoticeIDsString.Split(',');
                if (selectedNoticeIDsStringArray != null && selectedNoticeIDsStringArray.Length > 0)
                {
                    int noticeId;
                    foreach (string Id in selectedNoticeIDsStringArray)
                    {
                        try
                        {
                            noticeId = int.Parse(Id);
                            noticeService.SetIsHandled(noticeId);
                        }
                        catch { }
                    }
                }
            }

            return RedirectToAction("ListNotices", new { spaceKey = spaceKey, status = status, pageIndex = 1 });
        }

        /// <summary>
        /// 删除全部已处理通知
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="status">通知状态</param>
        [HttpGet]
        public ActionResult ClearAll(string spaceKey, NoticeStatus? status)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            noticeService.ClearAll(userId, status);

            return RedirectToAction("ListNotices", new { spaceKey = spaceKey, pageIndex = 1 });
        }

        /// <summary>
        /// 显示提示通知对话列表
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="itemCount">未读通知数</param>
        public ActionResult _ListPromptNotices(string spaceKey, int itemCount = 3)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            IEnumerable<Notice> notices = null;
            int unhandled = noticeService.GetUnhandledCount(userId);
            if (unhandled > 0)
            {
                unhandled = unhandled > itemCount ? itemCount : unhandled;
                notices = noticeService.GetTops(userId, unhandled);
            }
            ViewData["notices"] = notices;
            return View();
        }

        /// <summary>
        /// 提示通知对话列表批量设置状态
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="noticeIds">通知Id集合</param>
        [HttpPost]
        public ActionResult BatchSetIsHandled(string spaceKey, string noticeIds)
        {
            if (string.IsNullOrEmpty(noticeIds))
                return Redirect(SiteUrls.Instance().ListNotices(spaceKey, NoticeStatus.Unhandled, 1));

            IEnumerable<string> ids = noticeIds.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ids != null)
            {
                int noticeId;
                foreach (string Id in ids)
                {
                    try
                    {
                        noticeId = int.Parse(Id);
                        noticeService.SetIsHandled(noticeId);
                    }
                    catch { }
                }
            }

            return _ListPrompt(spaceKey);
        }

        /// <summary>
        /// 显示通知的设置页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        [HttpGet]
        public ActionResult _UserNoticeSettings(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            INoticeSettingsManager noticeSettingsManager = DIContainer.Resolve<INoticeSettingsManager>();
            NoticeSettings noticeSettings = noticeSettingsManager.Get();
            Dictionary<int, bool> userNoticeSettingses = noticeService.GetUserNoticeSettingses(userId);
            Dictionary<int, bool> mergeUserSettingses = new Dictionary<int, bool>();
            foreach (var item in noticeSettings.NoticeTypeSettingses)
            {
                if (userNoticeSettingses != null && userNoticeSettingses.ContainsKey(item.TypeId))
                    mergeUserSettingses[item.TypeId] = userNoticeSettingses[item.TypeId];
                else
                    mergeUserSettingses[item.TypeId] = item.IsAllow;
            }
            return View(mergeUserSettingses);
        }

        /// <summary>
        /// 更新用户通知设置
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        [HttpPost]
        public JsonResult UserNoticeSettings(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            IList<int> typeIds = new List<int>();
            string selectedNoticeTypeIDsString = Request.Form["noticeType"].Replace("false", "");
            if (!string.IsNullOrEmpty(selectedNoticeTypeIDsString))
            {
                string[] selectedNoticeTypeIDsStringArray = selectedNoticeTypeIDsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (selectedNoticeTypeIDsStringArray != null && selectedNoticeTypeIDsStringArray.Length > 0)
                {
                    typeIds = selectedNoticeTypeIDsStringArray.Where(n => !string.IsNullOrEmpty(n)).Select(n => Convert.ToInt32(n)).ToList();
                }
            }

            Dictionary<int, bool> userSettings = new Dictionary<int, bool>();
            IEnumerable<NoticeType> types = NoticeType.GetAll();

            foreach (NoticeType type in types)
            {
                userSettings[type.TypeId] = typeIds.Contains(type.TypeId);
            }

            noticeService.UpdateUserNoticeSettings(userId, userSettings);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        #endregion 通知

        /// <summary>
        /// 消息提示页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ListPrompt(string spaceKey)
        {
            IUser user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            int count_Messages = messageService.GetUnreadCount(user.UserId);
            InvitationService invitationService = DIContainer.Resolve<InvitationService>();
            int count_Invitation = invitationService.GetUnhandledCount(user.UserId);
            int count_Notices = noticeService.GetUnhandledCount(user.UserId);
            int contentCount = 3;  //种类数
            int maxCount = 9;   //总共允许的条数
            int[] allKindCount = new int[contentCount];
            int[] realityKindCount = new int[] { count_Messages, count_Invitation, count_Notices };

            int lastCount = contentCount;

            while (true)
            {
                int conduct = 0;
                for (int i = 0; i < realityKindCount.Length; i++)
                {
                    if (realityKindCount[i] > 0)
                    {
                        maxCount--;
                        realityKindCount[i]--;
                        allKindCount[i]++;
                        conduct++;
                    }
                    if (maxCount <= 0)
                        break;
                }
                if (maxCount <= 0 || conduct <= 0)
                    break;
            }

            ViewData["_ListPromptMessages"] = allKindCount[0];
            ViewData["_ListPromptInvitation"] = allKindCount[1];
            ViewData["_ListPromptNotices"] = allKindCount[2];

            ViewData["AllCount"] = allKindCount.Sum();

            return View("_ListPrompt");
        }

        #region 评论

        /// <summary>
        ///  获取我收到的评论
        /// </summary>
        [HttpGet]
        public ActionResult ListCommentsInBox(string spaceKey, int? pageIndex, string tenantTypeId = "")
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return Redirect(SiteUrls.Instance().Login());
            }
            pageResourceManager.InsertTitlePart("收到的评论");
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<Comment> comments = commentService.GetOwnerComments(userId, tenantTypeId, null, null, pageIndex ?? 1);

            ApplicationService appService = new ApplicationService();
            IEnumerable<ApplicationBase> ownerApps = appService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfBuiltIn.UserSpace, userId);

            Dictionary<long, TenantType> commentId_tenantType = new Dictionary<long, TenantType>();
            Dictionary<long, CommentedObject> commentedObject = new Dictionary<long, CommentedObject>();
            Dictionary<long, Comment> commentId_parentComment = new Dictionary<long, Comment>();

            //排除掉被禁用的应用
            var tenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Comment()).Where(n => n.TenantTypeId != "101202" && n.TenantTypeId != "200101");
            List<TenantType> tennatType_List = new List<TenantType>();
            foreach (var item in tenantTypes)
            {
                if (appService.Get(item.ApplicationId).IsEnabled)
                {
                    tennatType_List.Add(item);
                }
            }
            Dictionary<string, string> tenantTypeDic = tennatType_List.ToDictionary(k => k.TenantTypeId, v => v.Name);

            foreach (var comment in comments)
            {
                TenantType tenantType = tenantTypeService.Get(comment.TenantTypeId);
                if (comment.TenantTypeId != TenantTypeIds.Instance().Group())
                {
                    commentId_tenantType[comment.Id] = tenantType;
                }

                ICommentUrlGetter commentUrlGetter = CommentUrlGetterFactory.Get(comment.TenantTypeId);
                if (commentUrlGetter != null && comment.CommentedObjectId != 0)
                {
                    CommentedObject comment_object = commentUrlGetter.GetCommentedObject(comment.CommentedObjectId);
                    if (comment_object != null)
                    {
                        commentedObject[comment.Id] = comment_object;
                    }
                }

                if (comment.ParentId != 0)
                {
                    commentId_parentComment[comment.Id] = commentService.Get(comment.ParentId);
                }
            }


            ViewData["ownerApps"] = ownerApps;
            ViewData["tenantTypeDic"] = tenantTypeDic;
            ViewData["commentId_tenantType"] = commentId_tenantType;
            ViewData["commentedObject"] = commentedObject;
            ViewData["commentId_parentComment"] = commentId_parentComment;
            return View(comments);
        }

        /// <summary>
        ///  获取我发出去的评论
        /// </summary>
        [HttpGet]
        public ActionResult ListCommentsOutBox(string spaceKey, int? pageIndex, string tenantTypeId = "")
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return Redirect(SiteUrls.Instance().Login());
            }
            pageResourceManager.InsertTitlePart("发出的评论");

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            PagingDataSet<Comment> comments = commentService.GetUserComments(userId, tenantTypeId, null, null, pageIndex ?? 1);

            ApplicationService appService = new ApplicationService();
            IEnumerable<ApplicationBase> ownerApps = appService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfBuiltIn.UserSpace, userId);

            //以下由第二个人编写
            Dictionary<long, TenantType> commentId_tenantType = new Dictionary<long, TenantType>();
            Dictionary<long, CommentedObject> commentedObject = new Dictionary<long, CommentedObject>();
            Dictionary<long, Comment> commentId_parentComment = new Dictionary<long, Comment>();

            //排除掉被禁用的应用
            var tenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Comment()).Where(n => n.TenantTypeId != "101202" && n.TenantTypeId != "200101");
            List<TenantType> tennatType_List = new List<TenantType>();
            foreach (var item in tenantTypes)
            {
                if (appService.Get(item.ApplicationId).IsEnabled)
                {
                    tennatType_List.Add(item);
                }
            }
            Dictionary<string, string> tenantTypeDic = tennatType_List.ToDictionary(k => k.TenantTypeId, v => v.Name);

            foreach (var comment in comments)
            {
                TenantType tenantType = tenantTypeService.Get(comment.TenantTypeId);
                if (comment.TenantTypeId != TenantTypeIds.Instance().Group())
                {
                    commentId_tenantType[comment.Id] = tenantType;
                }

                ICommentUrlGetter commentUrlGetter = CommentUrlGetterFactory.Get(comment.TenantTypeId);
                if (commentUrlGetter != null && comment.CommentedObjectId != 0)
                {
                    CommentedObject comment_object = commentUrlGetter.GetCommentedObject(comment.CommentedObjectId);
                    if (comment_object != null)
                    {
                        commentedObject[comment.Id] = comment_object;
                    }

                }
                if (comment.ParentId != 0)
                {
                    commentId_parentComment[comment.Id] = commentService.Get(comment.ParentId);
                }
            }
            ViewData["tenantTypeDic"] = tenantTypeDic;
            ViewData["commentId_tenantType"] = commentId_tenantType;
            ViewData["commentedObject"] = commentedObject;
            ViewData["commentId_parentComment"] = commentId_parentComment;

            ViewData["ownerApps"] = ownerApps;

            return View(comments);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="commentId">评论ID</param>
        [HttpPost]
        public JsonResult DeleteComment(string spaceKey, long commentId)
        {
            bool result = commentService.Delete(commentId);
            if (result)
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        [HttpPost]
        public JsonResult CreateComment(string spaceKey)
        {
            User user = userService.GetFullUser(spaceKey);
            Comment parentComment = commentService.Get(Request.Form.Get<long>("commentId", 0));

            Comment newComment = Comment.New();
            newComment.AuditStatus = AuditStatus.Success;

            newComment.Author = user.DisplayName;//评论人名
            newComment.UserId = user.UserId;//评论人ID

            newComment.Body = Request.Form.Get<string>("commentBody", string.Empty);

            newComment.CommentedObjectId = parentComment.CommentedObjectId;//被评论对象ID
            newComment.ParentId = parentComment.Id;//评论Id
            newComment.Subject = parentComment.Subject;//博客的标题
            newComment.TenantTypeId = parentComment.TenantTypeId;//租户ID


            newComment.ToUserDisplayName = parentComment.Author;//被回复人姓名
            newComment.ToUserId = parentComment.UserId;//parentComment.UserId;//被回复人ID

            newComment.OwnerId = parentComment.OwnerId;//拥有者ID

            bool result = commentService.Create(newComment);
            if (result)
                return Json(new StatusMessageData(StatusMessageType.Success, "回复评论成功！"));
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "回复评论失败!"));
        }

        /// <summary>
        ///  获取@我的评论
        /// </summary>
        [HttpGet]
        public ActionResult AtMeComments(string spaceKey, int? pageIndex, string tenantTypeId = "")
        {
            pageResourceManager.InsertTitlePart("@我的评论");

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            AtUserService service = new AtUserService(TenantTypeIds.Instance().Comment());
            PagingDataSet<long> objectIds = service.GetPagingAssociateIds(userId, pageIndex ?? 1);

            IList<Comment> commentLists = new List<Comment>();
            foreach (var commentId in objectIds)
            {
                Comment comment = commentService.Get(commentId);
                if (comment != null)
                {
                    commentLists.Add(comment);
                }
            }
            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                commentLists = commentLists.Where(n => n.TenantTypeId == tenantTypeId).ToList();
            }

            PagingDataSet<Comment> comments = new PagingDataSet<Comment>(commentLists)
            {
                PageIndex = pageIndex ?? 1,
                PageSize = objectIds.PageSize,
                TotalRecords = objectIds.TotalRecords
            };

            Dictionary<long, TenantType> commentId_tenantType = new Dictionary<long, TenantType>();
            Dictionary<long, CommentedObject> commentedObject = new Dictionary<long, CommentedObject>();
            Dictionary<long, Comment> commentId_parentComment = new Dictionary<long, Comment>();

            //排除掉被禁用的应用
            ApplicationService appService = new ApplicationService();
            var tenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Comment()).Where(n => n.TenantTypeId != "101202" && n.TenantTypeId != "200101");
            List<TenantType> tennatType_List = new List<TenantType>();
            foreach (var item in tenantTypes)
            {
                if (appService.Get(item.ApplicationId).IsEnabled)
                {
                    tennatType_List.Add(item);
                }
            }
            Dictionary<string, string> tenantTypeDic = tennatType_List.ToDictionary(k => k.TenantTypeId, v => v.Name);

            foreach (var comment in comments)
            {
                TenantType tenantType = tenantTypeService.Get(comment.TenantTypeId);
                if (comment.TenantTypeId != TenantTypeIds.Instance().Group())
                {
                    commentId_tenantType[comment.Id] = tenantType;
                }

                ICommentUrlGetter commentUrlGetter = CommentUrlGetterFactory.Get(comment.TenantTypeId);
                if (commentUrlGetter != null && comment.CommentedObjectId != 0)
                {
                    CommentedObject comment_object = commentUrlGetter.GetCommentedObject(comment.CommentedObjectId);
                    if (comment_object != null)
                    {
                        commentedObject[comment.Id] = comment_object;
                    }

                }
                if (comment.ParentId != 0)
                {
                    commentId_parentComment[comment.Id] = commentService.Get(comment.ParentId);
                }
            }
            ViewData["tenantTypeDic"] = tenantTypeDic;
            ViewData["commentId_tenantType"] = commentId_tenantType;
            ViewData["commentedObject"] = commentedObject;
            ViewData["commentId_parentComment"] = commentId_parentComment;


            return View(comments);
        }
        #endregion

        #region 提醒

        /// <summary>
        /// 提醒
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ListUserReminderSettings(string spaceKey)
        {
            pageResourceManager.InsertTitlePart("提醒");
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            int count = 0;
            Dictionary<string, IEnumerable<UserReminderSettings>> dic = new Dictionary<string, IEnumerable<UserReminderSettings>>();
            List<UserReminderSettings> userReminderSettings = null;
            foreach (var setting in reminderService.GetAllUserReminderSettings(user.UserId))
            {
                userReminderSettings = new List<UserReminderSettings>();
                foreach (var value in setting.Value)
                {
                    userReminderSettings.Add(value);
                    count++;
                }
                dic[setting.Key.ToString()] = userReminderSettings;
            }
            ViewData["count"] = count;
            return View(dic);
        }

        /// <summary>
        /// 保存用户提醒设置
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="userReminderSettings">用户提醒设置</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUserReminderSettings(string spaceKey, List<UserReminderSettings> userReminderSettings)
        {
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                return HttpNotFound();
            List<UserReminderSettings> settings = new List<UserReminderSettings>();
            foreach (var setting in userReminderSettings)
            {
                setting.ReminderThreshold = setting.ReminderThreshold;
                setting.RepeatInterval = setting.RepeatInterval * 60;
                settings.Add(setting);
            }
            reminderService.BatchUpdateUserReminderSettings(user.UserId, settings);
            return Json(new StatusMessageData(StatusMessageType.Success, "保存成功！"));
        }
        #endregion

        /// <summary>
        /// 我的通知、私信、评论等的边栏
        /// </summary>
        /// <param name="subMenu"></param>
        /// <returns></returns>
        public ActionResult _MessageCenter_Menu(MessageCenterMenu subMenu)
        {
            IUser currentUser = UserContext.CurrentUser;
            MessageService messageService = new MessageService();
            InvitationService invitationService = new InvitationService();
            NoticeService noticeService = new NoticeService();

            int invitationCount = invitationService.GetUnhandledCount(currentUser.UserId);
            int messageCount = messageService.GetUnreadCount(currentUser.UserId);
            int noticeCount = noticeService.GetUnhandledCount(currentUser.UserId);
            ViewData["invitationCount"] = invitationCount;
            ViewData["messageCount"] = messageCount;
            ViewData["noticeCount"] = noticeCount;

            ViewData["MessageCenterMenu"] = subMenu;
            return View();
        }


    }
}