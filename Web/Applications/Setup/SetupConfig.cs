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

namespace Spacebuilder.Setup
{
    public class SetupConfig : ApplicationConfig
    {
        private static int applicationId = 1111;
        private XElement tenantAttachmentSettingsElement;
        private XElement tenantLogoSettingsElement;
        private XElement tenantCommentSettingsElement;

        /// <summary>
        /// 获取SetupConfig实例
        /// </summary>
        public static SetupConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(applicationId);
            if (app != null)
                return app.Config as SetupConfig;
            else
                return null;
        }


        public SetupConfig(XElement xElement)
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
            get { return applicationId; }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return "Setup"; }
        }

        /// <summary>
        /// 获取SetupApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(SetupApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {
            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.Setup.Resources.Resource", typeof(Spacebuilder.Setup.Resources.Resource).Assembly);
            //注册附件设置
            //TenantAttachmentSettings.RegisterSettings(tenantAttachmentSettingsElement);
        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();
        }
    }
}