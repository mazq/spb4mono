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
using Spacebuilder.Microblog.EventModules;
using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Globalization;

namespace Spacebuilder.Microblog
{
    public class MicroblogConfig : ApplicationConfig
    {
        private XElement tenantAttachmentSettingsElement;

        /// <summary>
        /// 获取MicroblogConfig实例
        /// </summary>
        public static MicroblogConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(ApplicationIds.Instance().Microblog());
            if (app != null)
                return app.Config as MicroblogConfig;
            else
                return null;
        }


        public MicroblogConfig(XElement xElement)
            : base(xElement)
        {
            this.tenantAttachmentSettingsElement = xElement.Element("tenantAttachmentSettings");
        }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public override int ApplicationId
        {
            get { return ApplicationIds.Instance().Microblog(); }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return ApplicationKeys.Instance().Microblog(); }
        }

        /// <summary>
        /// 获取SocialDiscussApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(MicroblogApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {

            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.Microblog.Resources.Resource", typeof(Spacebuilder.Microblog.Resources.Resource).Assembly);

            //注册附件设置
            TenantAttachmentSettings.RegisterSettings(tenantAttachmentSettingsElement);
            containerBuilder.Register(c => new MicroblogEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new MicroblogOperationLogEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new MicroblogIndexEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new MicroblogSearcher("微博", "~/App_Data/IndexFiles/Microblog", true, 2)).As<ISearcher>().Named<ISearcher>(MicroblogSearcher.CODE).SingleInstance();
            containerBuilder.Register(c => new MicroblogBodyProcessor()).As<IMicroblogBodyProcessor>().SingleInstance();
            containerBuilder.Register(c => new MicroblogCommentUrlGetter()).As<ICommentUrlGetter>().SingleInstance();

            containerBuilder.Register(c => new MicroblogUrlGetter()).As<IMicroblogUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new GroupUrlGetter()).As<IMicroblogUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new MicroblogApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();
            containerBuilder.Register(c => new MicroblogTenantAuthorizationHandler()).As<ITenantAuthorizationHandler>().SingleInstance();
            containerBuilder.Register(c => new MicroblogTopicRecommendUrlGetter()).As<IRecommendUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new MicroblogOwnerDataGetter()).As<IOwnerDataGetter>().SingleInstance();
            containerBuilder.Register(c => new MicroblogAtUserAssociatedUrlGetter()).As<IAtUserAssociatedUrlGetter>().SingleInstance();
        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();

            //注册日志Rss浏览计数服务
            CountService countService = new CountService(TenantTypeIds.Instance().Microblog());
            countService.RegisterCounts();//注册计数服务
            countService.RegisterCountPerDay();//需要统计阶段计数时，需注册每日计数服务
            countService.RegisterStageCount(CountTypes.Instance().CommentCount(), 1);

            //注册微博用户计数服务
            List<string> tenantTypeIds = new List<string>() { TenantTypeIds.Instance().User(), TenantTypeIds.Instance().Group() };
            OwnerDataSettings.RegisterStatisticsDataKeys(tenantTypeIds, OwnerDataKeys.Instance().ThreadCount());

            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().Microblog(), new MicroblogTagUrlGetter());

            //添加应用管理员角色
            ApplicationAdministratorRoleNames.Add(ApplicationIds.Instance().Microblog(), new List<string> { "MicroblogAdministrator" });
        }
    }
}