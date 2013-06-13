//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;
using System.Collections.Generic;
using System.Linq;
using Tunynet.Globalization;
using Tunynet.UI;

namespace Spacebuilder.Group.EventModules
{
    /// <summary>
    /// 处理群组动态、积分的EventMoudle
    /// </summary>
    public class GroupEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<GroupEntity, AuditEventArgs>.Instance().After += new CommonEventHandler<GroupEntity, AuditEventArgs>(GroupEntityActivityModule_After);
            EventBus<GroupEntity, AuditEventArgs>.Instance().After += new CommonEventHandler<GroupEntity, AuditEventArgs>(GroupEntityPointModule_After);
            EventBus<GroupEntity>.Instance().After += new CommonEventHandler<GroupEntity, CommonEventArgs>(InstallApplicationsModule_After);
            EventBus<GroupEntity>.Instance().After += new CommonEventHandler<GroupEntity, CommonEventArgs>(ChangeThemeAppearanceUserCountModule_After);
        }



        /// <summary>
        /// 动态处理程序
        /// </summary>
        /// <param name="group"></param>
        /// <param name="eventArgs"></param>
        private void GroupEntityActivityModule_After(GroupEntity group, AuditEventArgs eventArgs)
        {
            //生成动态
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //生成动态
            {
                if (group == null)
                    return;

                //生成Owner为用户的动态
                Activity actvityOfBar = Activity.New();
                actvityOfBar.ActivityItemKey = ActivityItemKeys.Instance().CreateGroup();
                actvityOfBar.ApplicationId = GroupConfig.Instance().ApplicationId;
                actvityOfBar.IsOriginalThread = true;
                actvityOfBar.IsPrivate = !group.IsPublic;
                actvityOfBar.UserId = group.UserId;
                actvityOfBar.ReferenceId = 0;//没有涉及到的实体
                actvityOfBar.ReferenceTenantTypeId = string.Empty;
                actvityOfBar.SourceId = group.GroupId;
                actvityOfBar.TenantTypeId = TenantTypeIds.Instance().Group();
                actvityOfBar.OwnerId = group.UserId;
                actvityOfBar.OwnerName = group.User.DisplayName;
                actvityOfBar.OwnerType = ActivityOwnerTypes.Instance().User();

                activityService.Generate(actvityOfBar, true);
            }
            else if (auditDirection == false) //删除动态
            {
                activityService.DeleteSource(TenantTypeIds.Instance().Group(), group.GroupId);
            }
        }

        /// <summary>
        /// 审核状态发生变化时处理积分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void GroupEntityPointModule_After(GroupEntity sender, AuditEventArgs eventArgs)
        {
            string pointItemKey = string.Empty;
            string eventOperationType = string.Empty;
            string description = string.Empty;
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();
            PointService pointService = new PointService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //加积分
            {
                pointItemKey = PointItemKeys.Instance().Group_CreateGroup();
                if (eventArgs.OldAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Create();
                else
                    eventOperationType = EventOperationType.Instance().Approved();
            }
            else if (auditDirection == false) //减积分
            {
                pointItemKey = PointItemKeys.Instance().Group_DeleteGroup();
                if (eventArgs.NewAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Delete();
                else
                    eventOperationType = EventOperationType.Instance().Disapproved();
            }
            if (!string.IsNullOrEmpty(pointItemKey))
            {
                description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "群组", sender.GroupName);
            }
            pointService.GenerateByRole(sender.UserId, pointItemKey, description);
        }

        /// <summary>
        /// 自动安装群组的相关应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void InstallApplicationsModule_After(GroupEntity sender, CommonEventArgs eventArgs)
        {
            ApplicationService applicationService = new ApplicationService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                applicationService.InstallApplicationsOfPresentAreaOwner(PresentAreaKeysOfBuiltIn.GroupSpace, sender.GroupId);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                applicationService.DeleteApplicationsOfPresentAreaOwner(PresentAreaKeysOfBuiltIn.GroupSpace, sender.GroupId);
            }
        }

        /// <summary>
        /// 修改皮肤文件的使用人数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ChangeThemeAppearanceUserCountModule_After(GroupEntity sender, CommonEventArgs eventArgs)
        {
            var themeService = new ThemeService();
            PresentAreaService presentAreaService = new PresentAreaService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                var presentArea = presentAreaService.Get(PresentAreaKeysOfBuiltIn.GroupSpace);
                themeService.ChangeThemeAppearanceUserCount(PresentAreaKeysOfBuiltIn.GroupSpace, null, presentArea.DefaultThemeKey + "," + presentArea.DefaultAppearanceKey);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                var presentArea = presentAreaService.Get(PresentAreaKeysOfBuiltIn.GroupSpace);
                string defaultThemeAppearance = presentArea.DefaultThemeKey + "," + presentArea.DefaultAppearanceKey;
                if (!sender.IsUseCustomStyle)
                    themeService.ChangeThemeAppearanceUserCount(PresentAreaKeysOfBuiltIn.GroupSpace, !string.IsNullOrEmpty(sender.ThemeAppearance) ? sender.ThemeAppearance : defaultThemeAppearance, null);
            }
        }
    }
}