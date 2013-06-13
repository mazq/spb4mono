//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Tunynet.Common;
using Tunynet;
using Tunynet.UI;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 删除用户时，清除用户平台级的内容
    /// </summary>
    public class DeleteUserEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<User, DeleteUserEventArgs>.Instance().After += new CommonEventHandler<User, DeleteUserEventArgs>(DeleteUserEventMoudle_After);
        }

        private void DeleteUserEventMoudle_After(IUser sender, DeleteUserEventArgs eventArgs)
        {
            IUserService userService = DIContainer.Resolve<IUserService>();

            IUser takeOverUser = userService.GetUser(eventArgs.TakeOverUserName);
            if (takeOverUser != null)
            {
                //将sender的内容转交给takeOverUser，同时还可根据eventArgs.TakeOverAll判断是否接管被删除用户的全部内容
            }

            
            
            

            #region 数据
            //清除应用数据
            ApplicationService applicationService = new ApplicationService();
            applicationService.DeleteUser(sender.UserId, eventArgs.TakeOverUserName, eventArgs.TakeOverAll);

            //删除用户信息
            new UserProfileService().Delete(sender.UserId);

            //清除用户内容计数数据
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            ownerDataService.ClearOwnerData(sender.UserId);

            
            

            
            //清除用户关于分类的数据
            CategoryService categoryService = new CategoryService();
            categoryService.CleanByUser(sender.UserId);

            //清除用户动态
            ActivityService activityService = new ActivityService();
            activityService.CleanByUser(sender.UserId);

            //清除用户评论
            new CommentService().DeleteUserComments(sender.UserId, false);

            #endregion

            #region 消息

            
            //清除用户关于私信的数据
            MessageService messageService = new MessageService();
            messageService.ClearSessionsFromUser(sender.UserId);

            //清除请求的用户数据
            InvitationService invitationService = new InvitationService();
            invitationService.CleanByUser(sender.UserId);

            //清除通知的用户数据
            NoticeService noticeService = new NoticeService();
            noticeService.CleanByUser(sender.UserId);

            InviteFriendService inviteFriendService = new InviteFriendService();
            inviteFriendService.CleanByUser(sender.UserId);

            //清除站外提醒的用户数据
            ReminderService reminderService = new ReminderService();
            reminderService.CleanByUser(sender.UserId);

            #endregion

            #region 关注/访客

            
            //清除用户关于关注用户的数据
            FollowService followService = new FollowService();
            followService.CleanByUser(sender.UserId);

            
            //清除访客记录的用户数据
            VisitService visitService = new VisitService(string.Empty);
            visitService.CleanByUser(sender.UserId);

            #endregion

            #region 帐号

            //清除帐号绑定数据
            var accountBindingService = new AccountBindingService();
            var accountBindings = new AccountBindingService().GetAccountBindings(sender.UserId);
            foreach (var accountBinding in accountBindings)
            {
                accountBindingService.DeleteAccountBinding(accountBinding.UserId, accountBinding.AccountTypeKey);
            }

            #endregion

            #region 装扮

            //调整皮肤文件使用次数
            var user = userService.GetFullUser(sender.UserId);
            if (user == null)
                return;
            var presentArea = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.UserSpace);
            string defaultThemeAppearance = string.Join(",", presentArea.DefaultThemeKey, presentArea.DefaultAppearanceKey);
            if (!user.IsUseCustomStyle)
                new ThemeService().ChangeThemeAppearanceUserCount(PresentAreaKeysOfBuiltIn.UserSpace, null, !string.IsNullOrEmpty(user.ThemeAppearance) ? user.ThemeAppearance : defaultThemeAppearance);

            #endregion
        }
    }
}