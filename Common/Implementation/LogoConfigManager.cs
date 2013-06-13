//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using System.Collections.Generic;
using System.Xml.Linq;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// logo配置管理类
    /// </summary>
    public class LogoConfigManager
    {
        private string configPath = "~/Config/logo.config";


        private static volatile LogoConfigManager _instance = null;
        private static readonly object lockObject = new object();

        private LogoConfigManager() { }

        /// <summary>
        /// 单例初始化器
        /// </summary>
        /// <returns></returns>
        public static LogoConfigManager Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new LogoConfigManager();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// 获取所有标题图配置
        /// </summary>
        /// <returns></returns>
        public XElement GetAllLogoConfigs()
        {
            XElement xelement = XElement.Load(WebUtility.GetPhysicalFilePath(configPath));
            return xelement;
        }

    }
}
