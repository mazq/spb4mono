//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Resources;
using Autofac;
using System.Collections.Concurrent;
using Tunynet.Utilities;
using System.IO;
using Fasterflect;

namespace Tunynet.Common
{

    /// <summary>
    /// 应用的配置文件
    /// </summary>
    [Serializable]
    public abstract class ApplicationConfig
    {
        #region 加载配置文件
        private static readonly object lockObject = new object();
        private static bool isInitialized;
        private static ConcurrentDictionary<int, ApplicationConfig> applicationConfigs = null;

        /// <summary>
        /// 加载所有的application.config
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        /// <returns>Key=ApplicationId</returns>
        public static void InitializeAll(ContainerBuilder containerBuilder)
        {
            if (!isInitialized)
            {
                lock (lockObject)
                {
                    if (!isInitialized)
                    {
                        applicationConfigs = LoadConfigs();
                        foreach (var config in applicationConfigs.Values)
                        {
                            config.Initialize(containerBuilder);
                        }
                        isInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// 获取某一个Application.Config
        /// </summary>
        /// <param name="applicationId">applicationID</param>
        /// <returns>返回ApplicationConfig</returns>
        public static ApplicationConfig GetConfig(int applicationId)
        {
            if (applicationConfigs != null && applicationConfigs.ContainsKey(applicationId))
                return applicationConfigs[applicationId];
            return null;
        }

        /// <summary>
        /// 加载所有的Application.config文件
        /// </summary>
        private static ConcurrentDictionary<int, ApplicationConfig> LoadConfigs()
        {
            var configs = new ConcurrentDictionary<int, ApplicationConfig>();
            //获取Applications中所有的Application.Config
            string applicationsDirectory = WebUtility.GetPhysicalFilePath("~/Applications/");
            foreach (var appPath in Directory.GetDirectories(applicationsDirectory))
            {
                string fileName = Path.Combine(appPath, "Application.Config");
                if (!File.Exists(fileName))
                    continue;

                string configType = string.Empty;
                XElement applicationElement = XElement.Load(fileName);

                //读取各个application节点中的属性     
                if (applicationElement != null)
                {
                    configType = applicationElement.Attribute("configType").Value;
                    Type applicationConfigClassType = Type.GetType(configType);
                    if (applicationConfigClassType != null)
                    {
                        ConstructorInvoker applicationConfigConstructor = applicationConfigClassType.DelegateForCreateInstance(typeof(XElement));
                        ApplicationConfig app = applicationConfigConstructor(applicationElement) as ApplicationConfig;
                        if (app != null)
                            configs[app.ApplicationId] = app;
                    }
                }
            }
            return configs;
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xElement">XElement</param>
        public ApplicationConfig(XElement xElement)
        {
            if (xElement != null)
            {
                this.applicationElement = xElement;

                XAttribute attr;
                attr = xElement.Attribute("applicationName");
                if (attr != null)
                {
                    if (attr.Value.StartsWith("$RES>") && attr.Value.Length > 5)
                        this.applicationNameResourceName = attr.Value.Substring(5);
                    else
                        this.applicationName = attr.Value;
                }
            }
        }

        private XElement applicationElement = null;
        /// <summary>
        /// ApplicationConfig的根节点
        /// </summary>
        public XElement ApplicationElement
        {
            get { return applicationElement; }
        }

        private string applicationNameResourceName;
        private string applicationName;
        /// <summary>
        /// 应用名称
        /// </summary>
        public string ApplicationName
        {
            get
            {
                if (!string.IsNullOrEmpty(applicationName))
                    return applicationName;
                else if (!string.IsNullOrEmpty(applicationNameResourceName))
                    return Tunynet.Globalization.ResourceAccessor.GetString(applicationNameResourceName, ApplicationId);

                return string.Empty;
            }
        }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public abstract int ApplicationId { get; }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public abstract string ApplicationKey { get; }

        /// <summary>
        /// Application类型
        /// </summary>
        public abstract Type ApplicationType { get; }

        /// <summary>
        /// 应用初始化运行环境（每次站点启动时DI容器构建前调用）
        /// </summary>
        /// <remarks>
        /// 用于注册组件、解析配置文件，不可使用DI容器解析对象因为此时尚未构建
        /// </remarks>
        /// <param name="containerBuilder">DI容器构建器(autofac)</param>
        public abstract void Initialize(ContainerBuilder containerBuilder);

        /// <summary>
        /// 加载应用
        /// </summary>
        /// <remarks>用于应用预启动，可做加载基础数据等操作</remarks>
        public virtual void Load()
        {

        }
    }
}
