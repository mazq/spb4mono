//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Xml.Linq;
using Autofac;
using Spacebuilder.Blog.EventModules;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Tunynet.Globalization;
using System.Collections.Generic;


namespace Spacebuilder.Blog
{
    public class BlogConfig : ApplicationConfig
    {
        private XElement tenantAttachmentSettingsElement;
        private XElement tenantLogoSettingsElement;
        private XElement tenantCommentSettingsElement;

        /// <summary>
        /// 获取BlogConfig实例
        /// </summary>
        public static BlogConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(ApplicationIds.Instance().Blog());
            if (app != null)
                return app.Config as BlogConfig;
            else
                return null;
        }


        public BlogConfig(XElement xElement)
            : base(xElement)
        {
            this.tenantAttachmentSettingsElement = xElement.Element("tenantAttachmentSettings");
            this.tenantLogoSettingsElement = xElement.Element("tenantLogoSettings");
            this.tenantCommentSettingsElement = xElement.Element("tenantCommentSettings");
        }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public override int ApplicationId
        {
            get { return ApplicationIds.Instance().Blog(); }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return "Blog"; }
        }

        /// <summary>
        /// 获取BlogApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(BlogApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {
            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.Blog.Resources.Resource", typeof(Spacebuilder.Blog.Resources.Resource).Assembly);

            //注册附件设置
            TenantAttachmentSettings.RegisterSettings(tenantAttachmentSettingsElement);
            TenantCommentSettings.RegisterSettings(tenantCommentSettingsElement);

            //注册日志站点设置
            containerBuilder.Register(c => new BlogSettingsManager()).As<IBlogSettingsManager>().SingleInstance();

            //注册日志正文解析器
            containerBuilder.Register(c => new BlogBodyProcessor()).Named<IBodyProcessor>(TenantTypeIds.Instance().Blog()).SingleInstance();

            //注册EventModule
            containerBuilder.Register(c => new BlogThreadEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new BlogIndexEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new BlogOperationLogEventModule()).As<IEventMoudle>().SingleInstance();
            containerBuilder.Register(c => new BlogActivityReceiverGetter()).Named<IActivityReceiverGetter>(ActivityOwnerTypes.Instance().Blog().ToString()).SingleInstance();

            //注册全文检索搜索器
            containerBuilder.Register(c => new BlogSearcher("日志", "~/App_Data/IndexFiles/Blog", true, 3)).As<ISearcher>().Named<ISearcher>(BlogSearcher.CODE).SingleInstance();

            //日志应用数据统计
            containerBuilder.Register(c => new BlogApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();
            containerBuilder.Register(c => new BlogCommentUrlGetter()).As<ICommentUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new BlogRecommendUrlGetter()).As<IRecommendUrlGetter>().SingleInstance();
            containerBuilder.Register(c => new BlogOwnerDataGetter()).As<IOwnerDataGetter>().SingleInstance();
            containerBuilder.Register(c => new BlogAtUserAssociatedUrlGetter()).As<IAtUserAssociatedUrlGetter>().SingleInstance();

        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();

            //注册日志计数服务
            CountService countService = new CountService(TenantTypeIds.Instance().BlogThread());
            
            

            //注册计数服务
            countService.RegisterCounts();

            //需要统计阶段计数时，需注册每日计数服务
            countService.RegisterCountPerDay();

            //注册日志浏览计数服务
            countService.RegisterStageCount(CountTypes.Instance().HitTimes(), 7);

            //注册用户计数服务
            OwnerDataSettings.RegisterStatisticsDataKeys(TenantTypeIds.Instance().User(), OwnerDataKeys.Instance().ThreadCount());

            //标签云中标签的链接
            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().BlogThread(), new BlogTagUrlGetter());

            //添加应用管理员角色
            ApplicationAdministratorRoleNames.Add(ApplicationIds.Instance().Blog(), new List<string> { "BlogAdministrator" });

        }
    }
}