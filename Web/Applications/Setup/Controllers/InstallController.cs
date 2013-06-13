using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Spacebuilder.Common;
using Tunynet.Common;
using Tunynet.UI;
using System.Collections.Concurrent;
using Tunynet.Mvc;
using Tunynet.Utilities;

namespace Spacebuilder.Setup.Controllers
{
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    public class InstallController : Controller
    {
        /// <summary>
        /// 
        /// 安装开始
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Start()
        {
            return View();
        }

        /// <summary>
        /// 第一步环境检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Step1_EnvironmentCheck()
        {
            return View();
        }

        /// <summary>
        /// 环境检查局部页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Step1_EnvironmentCheck()
        {
            Dictionary<string, bool> directoryPermissions = new Dictionary<string, bool>();
            directoryPermissions["App_Data"] = CheckFolderWriteable(Server.MapPath(@"~\App_Data"));
            directoryPermissions["Themes"] = CheckFolderWriteable(Server.MapPath(@"~\Themes"));
            directoryPermissions["Webconfig"] = CheckWebConfig();
            ViewData["DirectoryPermissions"] = directoryPermissions;

            return View();
        }

        /// <summary>
        /// 第二步填写数据库相关信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Step2_DatabaseInfo()
        {

            DataBaseInfoModel model = new DataBaseInfoModel();
            if (TempData["TempModel"] != null)
            {
                model = TempData["TempModel"] as DataBaseInfoModel;
                TempData["TempModel"] = TempData["TempModel"];
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Step2_Wait(DataBaseInfoModel model)
        {
            return View(model);
        }

        /// <summary>
        /// 第二步-等待安装完成
        /// </summary>
        /// <remarks>主要处理数据库结构及</remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Step2_Wait(DataBaseInfoModel model)
        {
            string server = string.Format("{0}{1}{2}", model.Server
                                                     , !string.IsNullOrEmpty(model.Port) ? ":" + model.Port : ""
                                                     , !string.IsNullOrEmpty(model.Instance) ? "\\" + model.Instance : "");

            String connectString = string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no",
                                                 server, model.DataBaseUserName, model.DataBasePassword);


            TempData["TempModel"] = model;
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
            //检测服务器是否可以连接上
            SqlConnection sqlConnection = GetSqlConnection(connectString, out messages);
            if (sqlConnection == null)
            {
                if (string.IsNullOrEmpty(model.Instance))
                {
                    connectString = string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no",
                                                  model.Server + "\\SQLEXPRESS", model.DataBaseUserName, model.DataBasePassword);
                    sqlConnection = GetSqlConnection(connectString, out messages);
                }
            }

            if (sqlConnection == null)
            {
                TempData["Error"] = messages;
                return Json(new { });
            }

            //尝试打开数据库链接
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                messages[e.Message] = e.StackTrace;
                TempData["Error"] = messages;
                return Json(new { });
            }

            SqlCommand command = new SqlCommand("select @@Version", sqlConnection);
            var val = command.ExecuteScalar();


            if (val == null || string.IsNullOrEmpty(val.ToString()))
            {
                messages["要求数据库为Sql 2005及以上"] = string.Empty;
                TempData["Error"] = messages;

                return Json(new { });
            }

            int dbVersion = Convert.ToInt32(val.ToString().Substring(21, 4));
            if (dbVersion < 2005)
            {
                messages["要求数据库为Sql 2005及以上,当前为" + val] = string.Empty;
                TempData["Error"] = messages;

                return Json(new { });
            }


            command = new SqlCommand(string.Format("select 1 from master..sysdatabases where [name]='{0}'", model.DataBase), sqlConnection);
            val = command.ExecuteScalar();

            //创建空数据库
            if (val == null)
            {
                command.CommandText = string.Format(" create database {0}; ALTER DATABASE {0} SET RECOVERY SIMPLE; ", model.DataBase);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    sqlConnection.Close();
                    messages[e.Message] = e.StackTrace;
                    TempData["Error"] = messages;
                    return Json(new { });
                }
                sqlConnection.Close();
            }
            else
            {
                try
                {
                    sqlConnection.Close();
                    sqlConnection = GetSqlConnection(connectString + ";database=" + model.DataBase, out messages);
                    sqlConnection.Open();
                    command.Connection = sqlConnection;
                    command.CommandText = "select count(*) from sysobjects where (xtype = 'u')";

                    int count = 0;
                    int.TryParse(command.ExecuteScalar().ToString(), out count);

                    if (count > 1)
                    {
                        command.CommandText = "select count(1) from tn_SystemData";
                        command.Connection = sqlConnection;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    sqlConnection.Close();
                    messages["当前数据库不是本程序数据库或一个空库！"] = "";
                    TempData["Error"] = messages;
                    return Json(new { });
                }
                sqlConnection.Close();
            }
            //修改web.config中数据库链接字符串
            connectString += ";database=" + model.DataBase;

            SetWebConfig(connectString, out messages);
            return Json(new { success = true, connectString = connectString });
        }

        /// <summary>
        /// 安装数据库表结构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Step2_Install_Schema()
        {
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
            string connectString = Request.Form.Get<string>("connectString", string.Empty);
            //连接新库
            SqlConnection dbConnection = GetSqlConnection(connectString, out messages);

            if (messages.Keys.Count > 0)
            {
                WriteLogFile(messages);
                return Json(new { });
            }

            List<string> fileList = SetupHelper.GetInstallFiles().Where(n => n.Contains("Schema")).ToList();
            string message = string.Empty;
            foreach (var file in fileList)
            {
                try
                {
                    SetupHelper.ExecuteInFile(dbConnection, file, out messages);
                }
                catch { }
                if (messages.Count > 0)
                {
                    WriteLogFile(messages);
                    return Json(new StatusMessageData(StatusMessageType.Error, "安装数据库表结构时出现错误，请查看安装日志！"));
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "安装数据库表结构成功！"));
        }

        /// <summary>
        /// 数据库初始化及创建系统管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Step2_Install_InitialData()
        {
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
            string connectString = Request.Form.Get<string>("connectString", string.Empty);
            //连接新库
            SqlConnection dbConnection = GetSqlConnection(connectString, out messages);

            if (messages.Keys.Count > 0)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "连接字符串不对！"));
            }
            string administrator = Request.Form.Get<string>("Administrator", string.Empty);
            string userPassword = Request.Form.Get<string>("UserPassword", string.Empty);
            KeyValuePair<string, string> adminInfo = new KeyValuePair<string, string>(administrator, UserPasswordHelper.EncodePassword(userPassword, Tunynet.Common.UserPasswordFormat.MD5));
            string mainRootSiteUrl = Request.Form.Get<string>("MainRootSiteUrl", string.Empty);
            List<string> fileList = SetupHelper.GetInstallFiles(null, true).Where(n => n.Contains("InitialData") || n.Contains("CreateAdministrator")).ToList();
            string message = string.Empty;
            foreach (var file in fileList)
            {
                try
                {
                    SetupHelper.ExecuteInFile(dbConnection, file, out messages, adminInfo, mainRootSiteUrl);
                }
                catch { }
                if (messages.Count > 0)
                {
                    WriteLogFile(messages);
                    return Json(new StatusMessageData(StatusMessageType.Error, "执行数据库初始化脚本时出现错误，请查看安装日志！"));
                }
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "安装数据库表结构成功！"));
        }

        /// <summary>
        /// 安装日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult InstallLog()
        {
            return File(GetLogFileName(), "text/plain", "install.log");
        }

        /// <summary>
        /// 获取安装日志文件名
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName()
        {
            string currentDirectory = WebUtility.GetPhysicalFilePath("~/Uploads");
            return currentDirectory + "\\install.log";
        }

        /// <summary>
        /// 确保文件已被创建
        /// </summary>
        /// <param name="fileName">带路径的文件名</param>
        /// <returns></returns>
        private bool EnsureFileExist(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                return true;
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                    fs.Close();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 将升级信息写入升级日志中
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool WriteLogFile(ConcurrentDictionary<string, string> messages)
        {

            string fileName = GetLogFileName();
            if (!EnsureFileExist(fileName))
                return false;

            StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8);   //该编码类型不会改变已有文件的编码类型
            foreach (var message in messages)
            {
                sw.WriteLine(DateTime.Now.ToString() + "：" + string.Format("{0}:{1}", message.Key, message.Value));
            }
            sw.Close();
            return true;
        }


        /// <summary>
        /// 安装成功
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Success()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("insert into tn_SystemData(Datakey,LongValue,DecimalValue) values ('SPBVersion',0,4.0)", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e) { }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetSite()
        {
            CheckWebConfig();
            return new EmptyResult();
        }


        #region Helper Method

        private bool CheckFolderWriteable(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Server.MapPath(path));
                return true;
            }

            try
            {
                string testFilePath = string.Format("{0}/test{1}{2}{3}{4}.txt", path, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                FileStream TestFile = System.IO.File.Create(testFilePath);
                TestFile.WriteByte(Convert.ToByte(true));
                TestFile.Close();
                System.IO.File.Delete(testFilePath);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 检测web.config的权限
        /// </summary>
        /// <returns></returns>
        private bool CheckWebConfig()
        {
            FileInfo FileInfo = new FileInfo(Server.MapPath("~/Web.config"));
            if (!FileInfo.Exists)
                return false;

            System.Xml.XmlDocument xmldocument = new System.Xml.XmlDocument();
            xmldocument.Load(FileInfo.FullName);
            try
            {
                XmlNode moduleNode = xmldocument.SelectSingleNode("//httpModules");
                if (moduleNode.HasChildNodes)
                {
                    for (int i = 0; i < moduleNode.ChildNodes.Count; i++)
                    {
                        XmlNode node = moduleNode.ChildNodes[i];
                        if (node.Name == "add")
                        {
                            if (node.Attributes.GetNamedItem("name").Value == "SpaceBuilderModule")
                            {
                                moduleNode.RemoveChild(node);
                                break;
                            }
                        }
                    }
                }
                xmldocument.Save(FileInfo.FullName);
            }
            catch
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 设置文件权限
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="username">需要设置权限的用户名</param>
        private bool SetAccount(string filePath, string username)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            FileSecurity fileSecurity = fileInfo.GetAccessControl();

            try
            {
                fileSecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, AccessControlType.Allow));
                fileInfo.SetAccessControl(fileSecurity);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置文件夹访问权限
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="userName">需要设置权限的用户名</param>
        /// <param name="rights">访问权限</param>
        /// <param name="allowOrDeny">允许拒绝访问</param>
        private bool SetFolderACL(string folderPath, string userName, FileSystemRights rights, AccessControlType allowOrDeny)
        {

            InheritanceFlags inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            return SetFolderACL(folderPath, userName, rights, allowOrDeny, inherits, PropagationFlags.None, AccessControlModification.Add);

        }

        /// <summary>
        /// 设置文件夹访问权限
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="userName">需要设置权限的用户名</param>
        /// <param name="rights">访问权限</param>
        /// <param name="allowOrDeny">允许拒绝访问</param>
        /// <param name="inherits">继承标志指定访问控制项 (ACE) 的继承语义</param>
        /// <param name="propagateToChildren">指定如何将访问面控制项 (ACE) 传播到子对象。仅当存在继承标志时，这些标志才有意义</param>
        /// <param name="addResetOrRemove">指定要执行的访问控制修改的类型。此枚举由 System.Security.AccessControl.ObjectSecurity 类及其子类的方法使用</param>
        private bool SetFolderACL(string folderPath, string userName, FileSystemRights rights, AccessControlType allowOrDeny, InheritanceFlags inherits, PropagationFlags propagateToChildren, AccessControlModification addResetOrRemove)
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            DirectorySecurity dSecurity = folder.GetAccessControl(AccessControlSections.All);
            FileSystemAccessRule accRule = new FileSystemAccessRule(userName, rights, inherits, propagateToChildren, allowOrDeny);

            bool modified;
            dSecurity.ModifyAccessRule(addResetOrRemove, accRule, out modified);
            folder.SetAccessControl(dSecurity);

            return modified;
        }

        //设置web.config
        private void SetWebConfig(string connectionString, out ConcurrentDictionary<string, string> messages)
        {
            messages = new ConcurrentDictionary<string, string>();
            System.IO.FileInfo FileInfo = new FileInfo(Server.MapPath("~/web.config"));

            if (!FileInfo.Exists)
            {
                messages[string.Format("文件 : {0} 不存在", Server.MapPath("~/web.config"))] = "";
            }

            System.Xml.XmlDocument xmldocument = new System.Xml.XmlDocument();
            xmldocument.Load(FileInfo.FullName);

            bool FoundIt = false;
            XmlNode connNode = xmldocument.SelectSingleNode("//connectionStrings");

            if (connNode.HasChildNodes)
            {
                for (int i = 0; i < connNode.ChildNodes.Count; i++)
                {
                    XmlNode Node = connNode.ChildNodes[i];
                    if (Node.Name == "add")
                    {
                        try
                        {
                            if (Node.Attributes.GetNamedItem("name").Value == "SqlServer")
                            {
                                Node.Attributes.GetNamedItem("connectionString").Value = connectionString;
                            }

                            FoundIt = true;
                        }
                        catch (Exception e)
                        {
                            messages[e.Message] = e.StackTrace;
                            FoundIt = true;
                        }
                    }
                }

                if (!FoundIt)
                {
                    messages["修改 web.config 时出错"] = "";
                }

                xmldocument.Save(FileInfo.FullName);
            }
        }

        /// <summary>
        /// 从web.config中获取连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetConnectionStringFromWebConfig()
        {
            string connectionString = string.Empty;
            System.IO.FileInfo FileInfo = new FileInfo(Server.MapPath("~/web.config"));

            if (!FileInfo.Exists)
                return string.Empty;

            System.Xml.XmlDocument xmldocument = new System.Xml.XmlDocument();
            xmldocument.Load(FileInfo.FullName);

            XmlNode connNode = xmldocument.SelectSingleNode("//connectionStrings");

            if (connNode.HasChildNodes)
            {
                for (int i = 0; i < connNode.ChildNodes.Count; i++)
                {
                    XmlNode Node = connNode.ChildNodes[i];
                    if (Node.Name == "add")
                    {
                        try
                        {
                            if (Node.Attributes.GetNamedItem("name").Value == "SqlServer")
                            {
                                connectionString = Node.Attributes.GetNamedItem("connectionString").Value;
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            return connectionString;
        }

        //获取链接
        private SqlConnection GetSqlConnection(string ConnectionString, out ConcurrentDictionary<string, string> messages)
        {
            messages = new ConcurrentDictionary<string, string>();
            try
            {
                return new SqlConnection(ConnectionString);
            }
            catch (Exception e)
            {
                messages[e.Message] = e.StackTrace;
                return null;
            }
        }
        #endregion
    }
}
