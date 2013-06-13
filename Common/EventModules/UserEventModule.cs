//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Tunynet.Common;
using Tunynet;
using Tunynet.UI;
using Tunynet.Globalization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户相关事件
    /// </summary>
    public class UserEventModule : IEventMoudle
    {
        private NoticeService noticeService = new NoticeService();
        IPointSettingsManager pointSettingsManger = DIContainer.Resolve<IPointSettingsManager>();
        private InviteFriendService inviteFriendService = new InviteFriendService();

        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<User, CreateUserEventArgs>.Instance().After += new CommonEventHandler<User, CreateUserEventArgs>(InitAppForUserEventMoudle_After);
            EventBus<IUser, RewardAndPunishmentUserEventArgs>.Instance().After += new CommonEventHandler<IUser, RewardAndPunishmentUserEventArgs>(RewardAndPunishmentUser_After);
            EventBus<User, CropAvatarEventArgs>.Instance().After += new CommonEventHandler<User, CropAvatarEventArgs>(UserPointEventModule_After);
            EventBus<User>.Instance().After += new CommonEventHandler<User, CommonEventArgs>(AutoNoModeratedUserNoticeEventModule_After);
            EventBus<User>.Instance().After += new CommonEventHandler<User, CommonEventArgs>(FreeModeratedUser_After);
        }


        /// <summary>
        /// 用户解除管制后增加邀请人积分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void FreeModeratedUser_After(User sender, CommonEventArgs eventArgs)
        {
            if (sender == null || string.IsNullOrEmpty(eventArgs.EventOperationType))
                return;
            if (eventArgs.EventOperationType == EventOperationType.Instance().CancelModerateUser() || eventArgs.EventOperationType == EventOperationType.Instance().AutoNoModeratedUser())
            {
                PointService pointService = new PointService();
                string pointItemKey = string.Empty;
                pointItemKey = PointItemKeys.Instance().FreeModeratedUser();

                if (sender != null)
                {
                    InviteFriendRecord invitingUser = inviteFriendService.GetInvitingUserId(sender.UserId);
                    if (invitingUser != null)
                    {
                        if (!invitingUser.InvitingUserHasBeingRewarded)
                        {
                            string userName = UserIdToUserNameDictionary.GetUserName(invitingUser.UserId);
                            string invitedName = UserIdToUserNameDictionary.GetUserName(sender.UserId);
                            string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_FreeModeratedUser"), userName, invitedName);
                            pointService.GenerateByRole(invitingUser.UserId, pointItemKey, description);
                            inviteFriendService.RewardingUser(invitingUser.UserId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化用户的应用
        /// </summary>
        /// <param name="sender">用户实体</param>
        /// <param name="eventArgs">事件参数</param>
        private void InitAppForUserEventMoudle_After(User sender, CreateUserEventArgs eventArgs)
        {
            if (sender == null)
                return;

            ApplicationService appService = new ApplicationService();
            appService.InstallApplicationsOfPresentAreaOwner(PresentAreaKeysOfBuiltIn.UserSpace, sender.UserId);

            var presentArea = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.UserSpace);
            new ThemeService().ChangeThemeAppearanceUserCount(PresentAreaKeysOfBuiltIn.UserSpace, null, presentArea.DefaultThemeKey + "," + presentArea.DefaultAppearanceKey);

        }

        private void RewardAndPunishmentUser_After(IUser sender, RewardAndPunishmentUserEventArgs eventArgs)
        {
            PointSettings pointSettings = pointSettingsManger.Get();
            int totalPoints = pointSettings.CalculateIntegratedPoint(sender.ExperiencePoints, sender.ReputationPoints);

            //更新用户等级
            //if (eventArgs.ExperiencePoints <= 0 && eventArgs.ReputationPoints <= 0)
            //    return;
            IUserService userService = DIContainer.Resolve<IUserService>();
            UserRankService userRankService = new UserRankService();
            SortedList<int, UserRank> userRanks = userRankService.GetAll();
            UserRank maxUserRank = null;
            if (userRanks != null && userRanks.Count > 0)
                maxUserRank = userRanks.First().Value;
            foreach (KeyValuePair<int, UserRank> userRank in userRanks)
            {
                if (totalPoints > userRank.Value.PointLower && userRank.Value.PointLower > maxUserRank.PointLower)
                    maxUserRank = userRank.Value;
            }
            userService.UpdateRank(sender.UserId, maxUserRank.Rank);

            //自动解除管制
            var user = userService.GetFullUser(sender.UserId);
            if (user.IsModerated && !user.IsForceModerated)
            {
                UserSettings userSettings = DIContainer.Resolve<IUserSettingsManager>().Get();
                if (totalPoints > userSettings.NoModeratedUserPoint)
                {
                    userService.NoModeratedUser(user.UserId);
                }
            }

            //发送通知
            if (sender == null)
                return;
            Notice notice = Notice.New();
            notice.UserId = sender.UserId;
            notice.ApplicationId = 0;
            notice.TypeId = NoticeTypeIds.Instance().Hint();
            if (eventArgs.ExperiencePoints < 0 || eventArgs.ReputationPoints < 0 || eventArgs.TradePoints < 0)
            {
                notice.Body = "您被系统扣除经验:" + -eventArgs.ExperiencePoints + "、威望:" + -eventArgs.ReputationPoints + "、金币:" + -eventArgs.TradePoints;
            }
            else
            {
                notice.Body = "您收到系统奖励经验:" + eventArgs.ExperiencePoints + "、威望:" + eventArgs.ReputationPoints + "、金币:" + eventArgs.TradePoints;
            }
            notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().ListNotices(sender.UserName, null, null));
            noticeService.Create(notice);
        }

        /// <summary>
        /// 首次上传头像加分
        /// </summary>
        /// <param name="sender">用户实体</param>
        /// <param name="eventArgs">事件参数</param>
        public void UserPointEventModule_After(User sender, CropAvatarEventArgs eventArgs)
        {
            string pointItemKey = string.Empty;
            string eventOperationType = string.Empty;
            if (eventArgs.IsFirst)
            {
                PointService pointService = new PointService();
                pointItemKey = PointItemKeys.Instance().FirstUploadAvatar();
                string description = ResourceAccessor.GetString("PointRecord_Pattern_FirstUploadAvatar");
                pointService.GenerateByRole(sender.UserId, pointItemKey, description);
            }
        }

        /// <summary>
        /// 首次上传头像加分
        /// </summary>
        /// <param name="sender">用户实体</param>
        /// <param name="eventArgs">事件参数</param>
        public void AutoNoModeratedUserNoticeEventModule_After(User sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().AutoNoModeratedUser())
            {
                Notice notice = Notice.New();
                notice.UserId = sender.UserId;
                notice.ApplicationId = 0;
                notice.TypeId = NoticeTypeIds.Instance().Hint();
                notice.TemplateName = "AutoNoModeratedUser";
                notice.RelativeObjectName = "我的权限";
                notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().UserModerated(sender.UserName));
                notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().ListNotices(sender.UserName, null, null));
                noticeService.Create(notice);
            }
        }
    }
}