//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Globalization;
using Tunynet.Common.Configuration;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 改变用户积分的时候
    /// </summary>
    public class RegisterEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<IUser, ChangePointsEventArgs>.Instance().After += new CommonEventHandler<IUser, ChangePointsEventArgs>(ChangePointsEventModule_After);
            EventBus<User, CreateUserEventArgs>.Instance().After += new CommonEventHandler<User, CreateUserEventArgs>(RegisterEventModule_After);
        }

        /// <summary>
        /// 创建用户的时候修改积分
        /// </summary>
        /// <param name="sender">创建用户角色</param>
        /// <param name="eventArgs">参数</param>
        void RegisterEventModule_After(User sender, CreateUserEventArgs eventArgs)
        {
            PointService pointService = new PointService();
            string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_Register"), sender.UserName);
            pointService.GenerateByRole(sender.UserId, PointItemKeys.Instance().Register(), description);
        }

        /// <summary>
        /// 更改用户积分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void ChangePointsEventModule_After(IUser sender, ChangePointsEventArgs eventArgs)
        {
            if (eventArgs.ExperiencePoints <= 0 && eventArgs.ReputationPoints <= 0)
                return;

            IUserService userService = DIContainer.Resolve<IUserService>();

            //自动升级
            UserRankService userRankService = new UserRankService();
            SortedList<int, UserRank> userRanks = userRankService.GetAll();
            UserRank maxUserRank = null;
            if (userRanks != null && userRanks.Count > 0)
                maxUserRank = userRanks.ElementAt(0).Value;
            IPointSettingsManager pointSettingsManger = DIContainer.Resolve<IPointSettingsManager>();
            PointSettings pointSettings = pointSettingsManger.Get();
            int totalPoints = pointSettings.CalculateIntegratedPoint(sender.ExperiencePoints, sender.ReputationPoints);
            foreach (KeyValuePair<int, UserRank> userRank in userRanks)
                if (totalPoints > userRank.Value.PointLower && userRank.Value.PointLower > maxUserRank.PointLower)
                    maxUserRank = userRank.Value;
            if (maxUserRank.Rank > sender.Rank)
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
        }
    }
}
