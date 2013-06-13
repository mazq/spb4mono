//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Tunynet.Common;
using Autofac;
using Spacebuilder.Common;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Spacebuilder.Search;
using Spacebuilder.Bar.Search;
using Spacebuilder.Bar.EventModules;
using Tunynet.Globalization;

namespace Spacebuilder.Bar
{
    public class BarConfig : ApplicationConfig
    {
        private XElement tenantAttachmentSettingsElement;
        private XElement tenantLogoSettingsElement;

        /// <summary>
        /// 获取BarConfig实例
        /// </summary>
        public static BarConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(ApplicationIds.Instance().Bar());
            if (app != null)
                return app.Config as BarConfig;
            else
                return null;
        }


        public BarConfig(XElement xElement)
            : base(xElement)
        {
            this.tenantAttachmentSettingsElement = xElement.Element("tenantAttachmentSettings");
            this.tenantLogoSettingsElement = xElement.Element("tenantLogoSettings");
        }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public override int ApplicationId
        {
            get { return ApplicationIds.Instance().Bar(); }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return ApplicationKeys.Instance().Bar(); }
        }

        /// <summary>
        /// 获取SocialDiscussApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(BarApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {

            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.Bar.Resources.Resource", typeof(Spacebuilder.Bar.Resources.Resource).Assembly);

            //注册附件设置
            TenantAttachmentSettings.RegisterSettings(tenantAttachmentSettingsElement);
            //注册标识图设置
            TenantLogoSettings.RegisterSettings(tenantLogoSettingsElement);

            //注册帖吧站点设置
            containerBuilder.Register(c => new BarSettingsManager()).As<IBarSettingsManager>().SingleInstance();
            //注册帖吧正文解析器
            containerBuilder.Register(c => new BarBodyProcessor()).Named<IBodyProcessor>(TenantTypeIds.Instance().Bar()).SingleInstance();
            containerBuilder.Register(c => new BarIndexEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new BarRatingEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new BarPostEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new BarThreadEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new BarSectionEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new BarSectionActivityReceiverGetter()).Named<IActivityReceiverGetter>(ActivityOwnerTypes.Instance().BarSection().ToString()).SingleInstance();
            containerBuilder.Register(c => new BarSearcher("帖吧", "~/App_Data/IndexFiles/Bar", true, 5)).As<ISearcher>().Named<ISearcher>(BarSearcher.CODE).SingleInstance();
            containerBuilder.Register(c => new BarOperationLogEventModule()).As<IEventMoudle>().SingleInstance();

            containerBuilder.Register(c => new BarApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();

            containerBuilder.Register(c => new BarUrlGetter()).As<IBarUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new GroupUrlGetter()).As<IBarUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new BarCommentUrlGetter()).As<ICommentUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new BarOwnerDataGetter()).As<IOwnerDataGetter>().SingleInstance();
            containerBuilder.Register(c => new BarPostOwnerDataGetter()).As<IOwnerDataGetter>().SingleInstance();

            containerBuilder.Register(c => new BarTenantAuthorizationHandler()).As<ITenantAuthorizationHandler>().SingleInstance();
            containerBuilder.Register(c => new BarThreadRecommendUrlGetter()).As<IRecommendUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new BarSectionRecommendUrlGetter()).As<IRecommendUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new BarThreadAtUserAssociatedUrlGetter()).As<IAtUserAssociatedUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new BarPostAtUserAssociatedUrlGetter()).As<IAtUserAssociatedUrlGetter>().SingleInstance();
        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();
            //注册帖吧计数服务
            CountService countService = new CountService(TenantTypeIds.Instance().BarSection());
            countService.RegisterCounts();//注册计数服务
            countService.RegisterCountPerDay();//需要统计阶段计数时，需注册每日计数服务
            countService.RegisterStageCount(CountTypes.Instance().ThreadAndPostCount(), 1);
            //注册帖子计数服务
            countService = new CountService(TenantTypeIds.Instance().BarThread());
            countService.RegisterCounts();//注册计数服务
            countService.RegisterCountPerDay();//需要统计阶段计数时，需注册每日计数服务
            countService.RegisterStageCount(CountTypes.Instance().HitTimes(), 1, 7);


            //注册贴吧用户计数服务
            List<string> tenantTypeIds = new List<string>() { TenantTypeIds.Instance().User(), TenantTypeIds.Instance().Group() };
            OwnerDataSettings.RegisterStatisticsDataKeys(tenantTypeIds
                                                         , OwnerDataKeys.Instance().ThreadCount()
                                                         , OwnerDataKeys.Instance().PostCount()
                                                         , OwnerDataKeys.Instance().FollowSectionCount());

            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().BarThread(), new BarTagUrlGetter());
            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().Group(), new BarTagUrlGetter());
            //添加应用管理员角色
            ApplicationAdministratorRoleNames.Add(ApplicationIds.Instance().Bar(), new List<string> { "BarAdministrator" });

        }
    }
}