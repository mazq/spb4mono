//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Xml.Linq;
using Autofac;
using Tunynet.Common;
using Tunynet.Globalization;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Spacebuilder.Common;
using Spacebuilder.Group.EventModules;
using Spacebuilder.Blog;
using Spacebuilder.Search;
using Spacebuilder.Blog.EventModules;
using Tunynet.UI;
using Spacebuilder.Group.Configuration;
using System.Collections.Generic;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组配置类
    /// </summary>
    public class GroupConfig : ApplicationConfig
    {
        private static int applicationId = 1011;
        private XElement tenantLogoSettingsElement;

        /// <summary>
        /// 获取GroupConfig实例
        /// </summary>
        public static GroupConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(applicationId);
            if (app != null)
                return app.Config as GroupConfig;
            else
                return null;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="xElement"></param>
        public GroupConfig(XElement xElement)
            : base(xElement)
        {
            this.tenantLogoSettingsElement = xElement.Element("tenantLogoSettings");
            XAttribute att = xElement.Attribute("minUserRankOfCreateGroup");
            if (att != null)
                int.TryParse(att.Value, out this.minUserRankOfCreateGroup);
        }

        private int minUserRankOfCreateGroup = 5;
        /// <summary>
        /// 允许用户创建群组的最小等级数
        /// </summary>
        public int MinUserRankOfCreateGroup
        {
            get { return minUserRankOfCreateGroup; }
        }

        private int maxDaysOfCreateMemeberActivity = 3;
        /// <summary>
        /// 群组有新成员加入动态的最大天数
        /// </summary>
        /// <remarks>超过此天数的新成员将不会在动态中显示，防止已加入时间很久的成员出现在动态中</remarks>
        public int MaxDaysOfCreateMemeberActivity
        {
            get { return maxDaysOfCreateMemeberActivity; }
        }


        /// <summary>
        /// ApplicationId
        /// </summary>
        public override int ApplicationId
        {
            get { return applicationId; }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return "Group"; }
        }

        /// <summary>
        /// 获取GroupApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(GroupApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {
            //注册标识图设置
            TenantLogoSettings.RegisterSettings(tenantLogoSettingsElement);

            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.Group.Resources.Resource", typeof(Spacebuilder.Group.Resources.Resource).Assembly);
            InvitationType.Register(new InvitationType { Key = InvitationTypeKeys.Instance().InviteJoinGroup(), Name = "邀请参加群组", Description = "" });
            InvitationType.Register(new InvitationType { Key = InvitationTypeKeys.Instance().ApplyJoinGroup(), Name = "申请加入群组", Description = "" });
            containerBuilder.Register(c => new GroupEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new GroupOperationLogEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new GroupMemberApplyEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new GroupMemberEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new GroupActivityReceiverGetter()).Named<IActivityReceiverGetter>(ActivityOwnerTypes.Instance().Group().ToString()).SingleInstance();
            containerBuilder.Register(c => new SetStatusInvitationForJoinGroupEventModule()).As<IEventMoudle>().SingleInstance();
            //groupId与groupKey的查询器
            containerBuilder.Register(c => new DefaultGroupIdToGroupKeyDictionary()).As<GroupIdToGroupKeyDictionary>().SingleInstance();
            containerBuilder.Register(c => new GroupIndexEventModule()).As<IEventMoudle>().SingleInstance();

            //注册全文检索搜索器
            containerBuilder.Register(c => new GroupSearcher("群组", "~/App_Data/IndexFiles/Group", true, 6)).As<ISearcher>().Named<ISearcher>(GroupSearcher.CODE).SingleInstance();

            ThemeService.RegisterThemeResolver(PresentAreaKeysOfBuiltIn.GroupSpace, new GroupSpaceThemeResolver());

            //群组推荐
            containerBuilder.Register(c => new GroupRecommendUrlGetter()).As<IRecommendUrlGetter>().SingleInstance();

            containerBuilder.Register(c => new GroupApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();
            containerBuilder.Register(c => new GroupTenantAuthorizationHandler()).As<ITenantAuthorizationHandler>().SingleInstance();
            containerBuilder.Register(c => new GroupOwnerDataGetter()).As<IOwnerDataGetter>().SingleInstance();
            containerBuilder.Register(c => new JoinGroupOwnerDataGetter()).As<IOwnerDataGetter>().SingleInstance();


        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();
            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().Group(), new GroupTagUrlGetter());
            //注册群组计数服务
            CountService countService = new CountService(TenantTypeIds.Instance().Group());
            countService.RegisterCounts();//注册计数服务
            countService.RegisterCountPerDay();//需要统计阶段计数时，需注册每日计数服务
            countService.RegisterStageCount(CountTypes.Instance().HitTimes(), 7);

            OwnerDataSettings.RegisterStatisticsDataKeys(TenantTypeIds.Instance().User()
                                                         , OwnerDataKeys.Instance().CreatedGroupCount()
                                                         , OwnerDataKeys.Instance().JoinedGroupCount());

            //添加应用管理员角色
            ApplicationAdministratorRoleNames.Add(applicationId, new List<string> { "GroupAdministrator" });
        }
    }
}