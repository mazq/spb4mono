//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using System.Collections.Generic;
using System.Xml.Linq;
using Tunynet.Utilities;
using System.IO;

namespace Spacebuilder.Setup
{
    /// <summary>
    /// Application配置管理类
    /// </summary>
    public class ApplicationConfigManager
    {
        private static volatile ApplicationConfigManager _instance = null;
        private static readonly object lockObject = new object();

        private ApplicationConfigManager() { }

        /// <summary>
        /// 单例初始化器
        /// </summary>
        /// <returns></returns>
        public static ApplicationConfigManager Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationConfigManager();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// 获取所有标题图配置
        /// </summary>
        /// <returns></returns>
        public List<XElement> GetAllApplicationConfigs()
        {
            List<XElement> allApplicationConfigs = new List<XElement>();
            //获取Applications中所有的Application.Config
            string applicationsDirectory = WebUtility.GetPhysicalFilePath("~/Applications/");
            foreach (var appPath in Directory.GetDirectories(applicationsDirectory))
            {
                string fileName = Path.Combine(appPath, "Application.Config");
                if (!File.Exists(fileName))
                    continue;

                string configType = string.Empty;
                XElement applicationElement = XElement.Load(fileName);
                allApplicationConfigs.Add(applicationElement);
            }
            return allApplicationConfigs;
        }

    }
}
