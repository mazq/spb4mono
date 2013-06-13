//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.Mvc;
using Tunynet.Common;
using System.Collections.Concurrent;
using Tunynet.Utilities;

namespace Spacebuilder.Setup
{
    public class SetupHelper
    {
        //执行sql脚本
        public static void ExecuteInFile(SqlConnection sqlCon, string pathToScriptFile, out ConcurrentDictionary<string, string> messages, KeyValuePair<string, string> adminInfo = new KeyValuePair<string,string>(), string mainSiteUrl = "")
        {
            string sqlString = "";
            StreamReader reader = null;
            messages = new ConcurrentDictionary<string, string>();

            if (!System.IO.File.Exists(pathToScriptFile))
            {
                throw new Exception("文件" + pathToScriptFile + " 未找到!");
            }
            Stream stream = System.IO.File.OpenRead(pathToScriptFile);
            reader = new StreamReader(stream);
            SqlCommand command = new SqlCommand();
            try
            {
                sqlCon.Open();
            }
            catch (Exception e)
            {
                messages[e.Message] = e.StackTrace;
                reader.Close();
                sqlCon.Close();
                return;
            }

            command.Connection = sqlCon;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandTimeout = 3600;
            while (null != (sqlString = ReadNextFromStream(reader, adminInfo, mainSiteUrl)))
            {
                command.CommandText = sqlString;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (!messages.ContainsKey("在文件：" + pathToScriptFile + " 中产生异常"))
                        messages["在文件：" + pathToScriptFile + " 中产生异常"] = e.Message;
                    else
                        messages["在文件：" + pathToScriptFile + " 中产生异常"] += e.Message;
                    if (!messages.ContainsKey(e.Message))
                        messages[e.Message] = e.StackTrace;
                    else
                        messages[e.Message] += e.StackTrace;
                    reader.Close();
                    sqlCon.Close();
                    return;
                }
            }
            reader.Close();
            sqlCon.Close();
        }

        //读取文件中的下一行
        public static string ReadNextFromStream(StreamReader reader, KeyValuePair<string, string> adminInfo, string mainSiteUrl)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string textLine;
            while (true)
            {
                textLine = reader.ReadLine();
                if (textLine != null)
                {
                    if (textLine.Contains("'SPBVersion'"))
                    {
                        continue;
                    }

                    if (textLine.Contains("admin"))
                    {
                        if (!string.IsNullOrEmpty(adminInfo.Key))
                        {
                            textLine = textLine.Replace("admin", adminInfo.Key);
                        }
                        if (!string.IsNullOrEmpty(adminInfo.Value))
                        {
                            textLine = textLine.Replace("7fef6171469e80d32c0559f88b377245", adminInfo.Value);
                        }
                    }

                    if (!string.IsNullOrEmpty(mainSiteUrl) && textLine.Contains("MainSiteRootUrl"))
                    {
                        textLine = textLine.Replace("\"MainSiteRootUrl\":\"\"", "\"MainSiteRootUrl\":\"" + mainSiteUrl + "\"");
                    }
                }



                if (textLine == null)
                {
                    if (stringBuilder.Length > 0)
                    {
                        return stringBuilder.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                if (textLine.TrimEnd().ToUpper() == "GO")
                {
                    break;
                }
                stringBuilder.AppendFormat("{0}\r\n", textLine);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取安装脚本
        /// </summary>
        /// <returns></returns>
        public static List<string> GetInstallFiles(IList<string> applicationKeys = null, bool createAdministrator = false)
        {
            List<string> fileList = new List<string>();

            if (applicationKeys == null)
            {
                applicationKeys = new List<string>() { "Common" };
                var applicationsDirectory = WebUtility.GetPhysicalFilePath("~/Applications/");
                foreach (string appPath in Directory.GetDirectories(applicationsDirectory))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(appPath);
                    if (directoryInfo != null & directoryInfo.Name != "Setup")
                        applicationKeys.Add(directoryInfo.Name);
                }
            }
            string filePath;
            if (applicationKeys.Contains("Common"))
            {
                filePath = WebUtility.GetPhysicalFilePath("~/Applications/Setup/Setup/Install/SqlServer/");

                if (Directory.Exists(filePath))
                    fileList.AddRange(Directory.GetFiles(filePath).ToList());
            }
            foreach (var applicationKey in applicationKeys)
            {
                filePath = WebUtility.GetPhysicalFilePath("~/Applications/" + applicationKey + "/Setup/Install/SqlServer/");
                if (Directory.Exists(filePath))
                    fileList.AddRange(Directory.GetFiles(filePath).ToList());
            }

            string temp = fileList.FirstOrDefault(n => n.Contains("CreateAdministrator"));
            fileList.Remove(temp);
            if (createAdministrator)
            {
                fileList.Add(temp);
            }
            return fileList;
        }


        /// <summary>
        /// 获取重命名旧表脚本
        /// </summary>
        /// <returns></returns>
        public static string GetUpgradeReNameFile()
        {
            string filePath = WebUtility.GetPhysicalFilePath("~/Applications/Setup/Setup/Upgrade/SqlServer/01_Upgrade_ReName_v3.2_to_v4.0.sql");
            if (File.Exists(filePath))
                return filePath;
            return string.Empty;
        }

        /// <summary>
        /// 获取升级平台脚本
        /// </summary>
        /// <returns></returns>
        public static string GetUpgradeCommonFile()
        {
            string filePath = WebUtility.GetPhysicalFilePath("~/Applications/Setup/Setup/Upgrade/SqlServer/02_Upgrade_Export_v3.2_to_v4.0.sql");
            if (File.Exists(filePath))
                return filePath;
            return string.Empty;
        }

        /// <summary>
        /// 获取应用的升级脚本
        /// </summary>
        /// <returns></returns>
        public static List<string> GetUpgradeFiles(string applicationKey)
        {
            string filePath = WebUtility.GetPhysicalFilePath("~/Applications/" + applicationKey + "/Setup/Upgrade/SqlServer/");
            if (Directory.Exists(filePath))
                return Directory.GetFiles(filePath).ToList();
            return new List<string>();
        }
    }
}