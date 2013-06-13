using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.FileStore;
using Tunynet.Imaging;
using Tunynet.Mvc;
using Tunynet.UI;
using Tunynet.Utilities;
using System.Data;

namespace Spacebuilder.Setup.Controllers
{
    /// <summary>
    /// 升级程序
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    public class UpgradeController : Controller
    {
        IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
        string currentDirectory = WebUtility.GetPhysicalFilePath("~/Uploads");
        Dictionary<string, string> applicationDictionary = new Dictionary<string, string> { { "Microblog", "微博" }, { "Blog", "日志" }, { "Photo", "相册" }, { "PointMall", "积分商城" }, { "Group", "群组" }, { "Bar", "帖吧" }, { "Ask", "问答" } };

        /// <summary>
        /// 准备
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Ready()
        {
            return View();
        }

        /// <summary>
        /// 选择升级的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Start()
        {
            string applicationsDirectory = WebUtility.GetPhysicalFilePath("~/Applications/");
            List<string> applicationKeys = new List<string>();
            foreach (string appPath in Directory.GetDirectories(applicationsDirectory))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(appPath);
                if (directoryInfo != null)
                    applicationKeys.Add(directoryInfo.Name);
            }
            return View(applicationKeys);
        }

        /// <summary>
        /// 选择升级的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpgradeData()
        {
            string applicationKeys = Request.Form.Get<string>("applicationKeys");
            return RedirectToAction("StartUpgrade", new { applicationKeys = applicationKeys });
        }

        /// <summary>
        /// 开始升级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StartUpgrade(string applicationKeys)
        {
            ViewData["applicationDictionary"] = applicationDictionary;
            return View();
        }

        /// <summary>
        /// 升级数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Database_Ready()
        {
            SqlConnection sqlConnection = GetConnection();
            //注册应用中的Application.Config
            foreach (var applicationConfig in ApplicationConfigManager.Instance().GetAllApplicationConfigs())
            {
                if (applicationConfig.Element("tenantAttachmentSettings") != null)
                {
                    TenantAttachmentSettings.RegisterSettings(applicationConfig.Element("tenantAttachmentSettings"));
                }
                if (applicationConfig.Element("tenantLogoSettings") != null)
                {
                    TenantLogoSettings.RegisterSettings(applicationConfig.Element("tenantLogoSettings"));
                }
            }

            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();

            //修改3.2版本的表名
            string reNameSqlFile = SetupHelper.GetUpgradeReNameFile();
            try
            {
                SetupHelper.ExecuteInFile(sqlConnection, reNameSqlFile, out messages);
            }
            catch (Exception e)
            {
                if (!messages.ContainsKey("在文件：" + reNameSqlFile + " 中产生异常"))
                    messages["在文件：" + reNameSqlFile + " 中产生异常"] = e.Message + "\r\n";
                else
                    messages["在文件：" + reNameSqlFile + " 中产生异常"] += e.Message + "\r\n";
            }

            foreach (var message in messages)
            {
                WriteLogFile(string.Format("{0}:{1}", message.Key, message.Value));
            }
            if (messages.Count > 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "升级数据库准备失败，请查看升级日志"));
            else
                return Json(new StatusMessageData(StatusMessageType.Success, "升级数据库准备就绪。"));
        }

        /// <summary>
        /// 安装4.0版本数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Database_InstallNewVersion(string applicationKeys)
        {
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
            SqlConnection sqlConnection = GetConnection();
            IEnumerable<string> appKeys = Request.QueryString.Gets<string>("applicationKeys", new List<string>());
            List<string> files = SetupHelper.GetInstallFiles(appKeys.ToList());
            //依次执行这些脚本文件
            foreach (var file in files)
            {
                try
                {
                    SetupHelper.ExecuteInFile(sqlConnection, file, out messages);
                }
                catch (Exception e)
                {
                    if (!messages.ContainsKey("在文件：" + file + " 中产生异常"))
                        messages["在文件：" + file + " 中产生异常"] = e.Message + "\r\n";
                    else
                        messages["在文件：" + file + " 中产生异常"] += e.Message + "\r\n";
                }
            }
            foreach (var message in messages)
            {
                WriteLogFile(string.Format("{0}:{1}", message.Key, message.Value));
            }

            if (messages.Count > 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "4.0版本数据库安装失败，请查看升级日志"));
            else
                return Json(new StatusMessageData(StatusMessageType.Success, "4.0版本数据库安装成功！"));
        }

        /// <summary>
        /// 执行3.2=>4.0平台升级脚本
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Database_Common()
        {
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
            SqlConnection sqlConnection = GetConnection();

            string upgradeFile = SetupHelper.GetUpgradeCommonFile();

            try
            {
                SetupHelper.ExecuteInFile(sqlConnection, upgradeFile, out messages);
            }
            catch (Exception e)
            {
                if (!messages.ContainsKey("在文件：" + upgradeFile + " 中产生异常"))
                    messages["在文件：" + upgradeFile + " 中产生异常"] = e.Message + "\r\n";
                else
                    messages["在文件：" + upgradeFile + " 中产生异常"] += e.Message + "\r\n";
            }

            foreach (var message in messages)
            {
                WriteLogFile(string.Format("{0}:{1}", message.Key, message.Value));
            }
            ResetPassword();
            if (messages.Count > 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "3.2=>4.0平台升级脚本执行失败，请查看升级日志"));
            else
                return Json(new StatusMessageData(StatusMessageType.Success, "3.2=>4.0平台升级脚本执行成功！"));
        }

        /// <summary>
        /// 执行3.2=>4.0日志升级脚本
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Database_Applcation(string applicationKey)
        {
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
            SqlConnection sqlConnection = GetConnection();
            List<string> upgradeFiles = SetupHelper.GetUpgradeFiles(applicationKey);
            string applicationName = string.Empty;
            if (applicationDictionary.ContainsKey(applicationKey))
                applicationName = applicationDictionary[applicationKey];
            else
                return new EmptyResult();
            //依次执行这些脚本文件
            foreach (var file in upgradeFiles)
            {
                try
                {
                    SetupHelper.ExecuteInFile(sqlConnection, file, out messages);
                }
                catch (Exception e)
                {
                    if (!messages.ContainsKey("在文件：" + file + " 中产生异常"))
                        messages["在文件：" + file + " 中产生异常"] = e.Message + "\r\n";
                    else
                        messages["在文件：" + file + " 中产生异常"] += e.Message + "\r\n";
                }
            }

            foreach (var message in messages)
            {
                WriteLogFile(string.Format("{0}:{1}", message.Key, message.Value));
            }

            if (messages.Count > 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "3.2=>4.0" + applicationName + "升级脚本执行失败，请查看升级日志"));
            else
                return Json(new StatusMessageData(StatusMessageType.Success, "3.2=>4.0" + applicationName + "升级脚本执行成功！"));
        }

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetConnection()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }

        /// <summary>
        /// 升级附件-将旧附件移动到Old
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_MoveToOld()
        {
            SqlConnection sqlConnection = GetConnection();
            try
            {
                SqlCommand deleteVersion = new SqlCommand("delete from tn_SystemData where Datakey='Version' or Datakey='SPBVersion'", sqlConnection);
                SqlCommand command = new SqlCommand("insert into tn_SystemData(Datakey,LongValue,DecimalValue) values ('SPBVersion',0,4.0)", sqlConnection);
                sqlConnection.Open();
                deleteVersion.ExecuteNonQuery();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
            }
            //将旧附件移动到Old目录中
            string oldDirectory = Path.Combine(currentDirectory, "Old");
            if (!Directory.Exists(oldDirectory))
            {
                Directory.CreateDirectory(oldDirectory);
                foreach (var directory in Directory.GetDirectories(currentDirectory))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                    if (directoryInfo == null || directoryInfo.Name == "Old")
                        continue;
                    string newDirectory = Path.Combine(oldDirectory, directoryInfo.Name);
                    Directory.Move(directory, newDirectory);
                }
            }
            string message = "旧附件已移动到Old目录中";
            WriteLogFile(message);
            return Json(new StatusMessageData(StatusMessageType.Success, message));
        }

        /// <summary>
        /// 升级用户头像附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_UserAvatar()
        {
            //升级用户头像附件
            string newAvatarsDirectoryName = "Avatars";
            string avatarMessage = string.Empty;
            bool result = UpdateAttachmentAvatars("Avatars", newAvatarsDirectoryName, out avatarMessage);
            WriteLogFile(avatarMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, avatarMessage));
        }

        /// <summary>
        /// 升级友情链接附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_Link()
        {
            //升级友情链接
            string linkMessage = string.Empty;
            bool result = UpdateLinkBySQL(out linkMessage);
            WriteLogFile(linkMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, linkMessage));
        }

        /// <summary>
        /// 升级微博附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_Microblog()
        {
            string oldDirectoryName = "MicroBlog";
            string microblogMessage = string.Empty;
            bool result = UpdateAttachmentBySQL(oldDirectoryName, "100101", out microblogMessage);//微博            
            WriteLogFile(microblogMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, microblogMessage));
        }

        /// <summary>
        /// 升级帖吧附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_Bar()
        {
            string oldDirectoryName = "Forums";
            string barThreadMessage = string.Empty;
            bool result = UpdateAttachmentBySQL(oldDirectoryName, "101202", out barThreadMessage, true);//帖子
            string barPostMessage = string.Empty;
            UpdateAttachmentBySQL(oldDirectoryName, "101203", out barPostMessage);//回帖
            string barSectionLogoMessage = string.Empty;
            UpdateAttachmentAvatars("ForumSectionLogos", "BarSectionLogo", out barSectionLogoMessage, "*.*");//贴吧Logo
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine(barThreadMessage).AppendLine(barPostMessage).AppendLine(barSectionLogoMessage);
            WriteLogFile(barThreadMessage);
            WriteLogFile(barPostMessage);
            WriteLogFile(barSectionLogoMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, logMessage.ToString()));
        }

        /// <summary>
        /// 升级日志附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_Blog()
        {
            string blogMessage = string.Empty;
            bool result = UpdateAttachmentBySQL("Blogs", "100201", out blogMessage);//日志           
            WriteLogFile(blogMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, blogMessage));
        }

        /// <summary>
        /// 升级群组附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_Group()
        {
            string groupLogoMessage = string.Empty;
            bool result = UpdateAttachmentAvatars("ClubLogos", "GroupLogo", out groupLogoMessage);//群组Logo
            WriteLogFile(groupLogoMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, groupLogoMessage));
        }

        /// <summary>
        /// 升级问答附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_Ask()
        {
            string askAnswerMessage = string.Empty;
            UpdateAttachmentBySQL("Rewards", "101302", out askAnswerMessage);//回答
            string askQuestionMessage = string.Empty;
            bool result = UpdateAttachmentBySQL("Rewards", "101301", out askQuestionMessage, true);//问题
            StringBuilder askMessage = new StringBuilder();
            askMessage.AppendLine(askAnswerMessage).AppendLine(askQuestionMessage);
            WriteLogFile(askAnswerMessage);
            WriteLogFile(askQuestionMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, askMessage.ToString()));
        }

        /// <summary>
        /// 升级相册附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_Photo()
        {
            string photoMessage = string.Empty;
            bool result = UpdatePhotoBySQL(out photoMessage);
            WriteLogFile(photoMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, photoMessage));
        }

        /// <summary>
        /// 升级积分商城附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upgrading_Attachment_PointMall()
        {
            string pointMallMessage = string.Empty;
            bool result = UpdatePointMallBySQL(out pointMallMessage);
            WriteLogFile(pointMallMessage);
            return Json(new StatusMessageData(result ? StatusMessageType.Success : StatusMessageType.Error, pointMallMessage));
        }


        /// <summary>
        /// 重设密码，并写入日志文件
        /// </summary>
        public void ResetPassword()
        {
            List<string> resetPasswordUserNames = new List<string>();
            SqlConnection connection = GetConnection();

            //创建重设密码日志
            string fileName = Path.Combine(currentDirectory, "resetPassword.log");
            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);
            EnsureFileExist(fileName);

            StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8);

            //获取所有密码格式为2的用户并重设密码
            try
            {
                SqlCommand selectCommand = new SqlCommand("select UserName from tn_Users where PasswordFormat=2", connection);
                connection.Open();

                SqlDataReader dr = selectCommand.ExecuteReader();
                while (dr.Read())
                {
                    string userName = dr.GetString(0);
                    resetPasswordUserNames.Add(userName);
                }
                dr.Close();

                sw.WriteLine(DateTime.Now);
                foreach (var userName in resetPasswordUserNames)
                {
                    //long passDateTime.UtcNow.Ticks;
                    int password = new Random().Next(100000, 999999);
                    //将用户密码格式为2的改为0，并且修改密码
                    SqlCommand updateCommand = new SqlCommand("update tn_Users set PasswordFormat=0,Password='" + password + "' where PasswordFormat=2 and UserName='" + userName + "'", connection);
                    updateCommand.ExecuteNonQuery();
                    sw.WriteLine("用户名：{0}密码：{1}", userName, password);
                }
                sw.WriteLine("---------------------------------------------------------------------------------");
                sw.Close();
                connection.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                sw.Close();
                connection.Close();
            }
        }


        /// <summary>
        /// 完成
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Complete()
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
        /// 暂停站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UnloadAppDomain()
        {
            System.Web.HttpRuntime.UnloadAppDomain();
            return new EmptyResult();
        }

        /// <summary>
        /// 重设密码日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult _ResetPasswordLog()
        {
            return File(Path.Combine(currentDirectory, "resetPassword.log"), "text/plain", "resetPassword.log");
        }

        /// <summary>
        /// 升级日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult _UpgradeLog()
        {
            return File(GetLogFileName(), "text/plain", "upgrade.log");
        }

        /// <summary>
        /// 设置不同尺寸的Logo
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="fileName"></param>
        /// <param name="relativePath"></param>
        private void SetResizedLogo(string tenantTypeId, string fileName, string relativePath)
        {
            TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(tenantTypeId);
            if (tenantLogoSettings != null && tenantLogoSettings.ImageSizeTypes != null && tenantLogoSettings.ImageSizeTypes.Count > 0)
            {
                foreach (var imageSizeType in tenantLogoSettings.ImageSizeTypes.Values)
                {
                    string sizedFileName = storeProvider.GetSizeImageName(fileName, imageSizeType.Key, imageSizeType.Value);
                    storeProvider.DeleteFile(relativePath, sizedFileName);
                    IStoreFile file = storeProvider.GetResizedImage(relativePath, fileName, imageSizeType.Key, imageSizeType.Value);
                }
            }
        }

        /// <summary>
        /// 设置不同尺寸的图片（附件中）
        /// </summary>
        /// <param name="attachment">附件</param>
        private void SetResizedAttachmentImage(Attachment attachment)
        {
            TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(attachment.TenantTypeId);
            if (tenantAttachmentSettings != null && tenantAttachmentSettings.ImageSizeTypes != null && tenantAttachmentSettings.ImageSizeTypes.Count > 0)
            {
                foreach (var imageSizeType in tenantAttachmentSettings.ImageSizeTypes)
                {
                    IStoreFile file = storeProvider.GetResizedImage(attachment.GetRelativePath(), attachment.FileName, imageSizeType.Size, imageSizeType.ResizeMethod);
                }
            }
        }

        /// <summary>
        /// 设置不同尺寸的用户头像
        /// </summary>
        /// <param name="userId"></param>
        private void SetUserAvatars(long userId)
        {
            IStoreFile iStoreFile = storeProvider.GetFile(GetRelativePathById(userId, "Avatars"), userId + "_big.jpg");
            if (iStoreFile == null)
                return;
            using (Stream fileStream = iStoreFile.OpenReadStream())
            {
                Stream bigImage = ImageProcessor.Crop(fileStream, new Rectangle(0, 0, 160, 160), 160, 160);
                Stream smallImage = ImageProcessor.Resize(bigImage, 50, 50, ResizeMethod.KeepAspectRatio);
                storeProvider.AddOrUpdateFile(GetRelativePathById(userId, "Avatars"), userId + ".jpg", smallImage);

                bigImage.Dispose();
                smallImage.Dispose();
                fileStream.Close();
            }
        }

        /// <summary>
        /// 更新友情链接附件
        /// </summary>
        /// <returns></returns>
        private bool UpdateLinkBySQL(out string message)
        {
            int allCount = 0;
            int moveCount = 0;
            bool result = false;
            StringBuilder messageBuilder = new StringBuilder();

            LinkRepository linkRepository = new LinkRepository();
            IEnumerable<LinkEntity> imageLinks = linkRepository.GetAll().Where(n => n.LinkType == LinkType.ImageLink).Where(n => !string.IsNullOrEmpty(n.ImageUrl));
            Directory.SetCurrentDirectory(currentDirectory);

            allCount = imageLinks.Count();

            foreach (var link in imageLinks)
            {
                string newRelativePath = GetRelativePathById(link.LinkId, "Link");
                string oldSubDirectory = string.Empty;

                if (link.OwnerType == OwnerTypes.Instance().Group())
                {
                    oldSubDirectory = "Club";
                }
                else if (link.OwnerType == OwnerTypes.Instance().Site())
                {
                    oldSubDirectory = "Site";
                }
                else if (link.OwnerType == OwnerTypes.Instance().User())
                {
                    oldSubDirectory = "User";
                }

                if (!string.IsNullOrEmpty(oldSubDirectory))
                {
                    string oldDirectory = Path.Combine(currentDirectory, GetOldLinkRelativePath("LinkImages", oldSubDirectory, link.OwnerId));
                    foreach (var oldFile in Directory.GetFiles(oldDirectory))
                    {
                        FileInfo oldFileInfo = new FileInfo(oldFile);
                        if (link.ImageUrl.Contains(oldFileInfo.Name))
                        {
                            string newFilePath = Path.Combine(newRelativePath, oldFileInfo.Name);
                            if (!Directory.Exists(newRelativePath))
                                Directory.CreateDirectory(newRelativePath);
                            if (!System.IO.File.Exists(newFilePath))
                            {
                                System.IO.File.Move(oldFile, newFilePath);
                                storeProvider.GetResizedImage(newRelativePath, oldFileInfo.Name, new Size(150, 50), Tunynet.Imaging.ResizeMethod.KeepAspectRatio);
                                moveCount++;
                            }
                            break;
                        }
                    }
                }
            }
            if (allCount > 0)
            {
                messageBuilder.AppendFormat("{0}:一共找到{1}个可以升级的友情链接图片", Path.Combine(currentDirectory, "Old", "LinkImages"), allCount);
                if (moveCount > 0)
                {
                    result = true;
                    messageBuilder.AppendFormat("，成功升级了{0}个友情链接图片", moveCount);
                }             
            }
            message = messageBuilder.ToString();
            return result;
        }

        /// <summary>
        /// 升级附件
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="oldDirectory"></param>
        /// <param name="checkPasswordAsAssociateId">是否检查附件密码作为关联Id(用于解决3.2到4.0升级过程中，AssociateId发生变化的情况。目前是将旧AssociateId存储在了Password字段中。)</param>
        /// <returns></returns>
        private bool UpdateAttachmentBySQL(string oldDirectory, string tenantTypeId, out string message, bool checkPasswordAsAssociateId = false)
        {
            long allCount = 0;
            long moveCount = 0;
            bool result = false;
            StringBuilder messageBuilder = new StringBuilder();

            AttachmentService attachmentService = new AttachmentService(tenantTypeId);
            PagingDataSet<Attachment> attachments = attachmentService.Gets(tenantTypeId, null, 1);

            Directory.SetCurrentDirectory(currentDirectory);

            for (int i = 1; i < attachments.PageCount + 1; i++)
            {
                if (i > 1)
                {
                    attachments = attachmentService.Gets(tenantTypeId, null, i);
                }
                foreach (var attachment in attachments)
                {
                    string relativePath = attachment.GetRelativePath();
                    if (!Directory.Exists(relativePath))
                        Directory.CreateDirectory(relativePath);

                    string newFilePath = storeProvider.JoinDirectory(relativePath, attachment.FileName);
                    string oldFilePath = Path.Combine(GetOldRelativePath(oldDirectory, attachment.AssociateId), attachment.FileName);
                    if (checkPasswordAsAssociateId)
                    {
                        long oldAssociateId = 0;
                        if (!string.IsNullOrEmpty(attachment.Password))
                            long.TryParse(attachment.Password, out oldAssociateId);
                        if (oldAssociateId <= 0)
                            continue;
                        oldFilePath = Path.Combine(GetOldRelativePath(oldDirectory, oldAssociateId), attachment.FileName);
                    }

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        allCount++;
                        if (!System.IO.File.Exists(newFilePath))
                        {
                            System.IO.File.Move(oldFilePath, newFilePath);
                            if (attachment.MediaType == MediaType.Image)
                                SetResizedAttachmentImage(attachment);
                            moveCount++;
                        }
                    }                    
                }
            }
            if (allCount > 0)
            {
                messageBuilder.AppendFormat("{0}:一共找到{1}个可以升级的附件", Path.Combine(currentDirectory, "Old", oldDirectory), allCount);
                if (moveCount > 0)
                {
                    result = true;
                    messageBuilder.AppendFormat("，成功升级了{0}个附件", moveCount);
                }
            }
            else
            {
                messageBuilder.AppendFormat("{0}:没有找到需要升级的附件", Path.Combine(currentDirectory, "Old", oldDirectory));
                result = true;
            }
            message = messageBuilder.ToString();
            return result;
        }

        /// <summary>
        /// 升级积分商城附件
        /// </summary>
        /// <returns></returns>
        private bool UpdatePointMallBySQL(out string message)
        {
            long allCount = 0;
            long moveCount = 0;
            bool result = false;
            StringBuilder messageBuilder = new StringBuilder();

            string tenantTypeId = "200101";
            AttachmentService attachmentService = new AttachmentService(tenantTypeId);
            PagingDataSet<Attachment> attachments = attachmentService.Gets(tenantTypeId, null, 1);

            Directory.SetCurrentDirectory(currentDirectory);

            for (int i = 1; i < attachments.PageCount + 1; i++)
            {
                if (i > 1)
                {
                    attachments = attachmentService.Gets(tenantTypeId, null, i);
                }
                foreach (var attachment in attachments)
                {
                    string relativePath = attachment.GetRelativePath();
                    if (!Directory.Exists(relativePath))
                        Directory.CreateDirectory(relativePath);

                    string newFilePath = storeProvider.JoinDirectory(relativePath, attachment.FileName);
                    string idString = attachment.AssociateId.ToString().PadLeft(9, '0');
                    string oldRelativePath = storeProvider.JoinDirectory("Old", "ConvertibleGiftImages", idString.Substring(0, 3), idString.Substring(3, 3), idString.Substring(6, 2));

                    string oldFilePath = Path.Combine(oldRelativePath, attachment.FileName);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        allCount++;
                        if (!System.IO.File.Exists(newFilePath))
                        {
                            System.IO.File.Move(oldFilePath, newFilePath);
                            if (attachment.MediaType == MediaType.Image)
                                SetResizedAttachmentImage(attachment);
                            moveCount++;
                        }
                    }                   
                }
            }

            if (allCount > 0)
            {
                messageBuilder.AppendFormat("{0}:一共找到{1}个可以升级的附件", Path.Combine(currentDirectory, "Old", "ConvertibleGiftImages"), allCount);
                if (moveCount > 0)
                {
                    result = true;
                    messageBuilder.AppendFormat("，成功升级了{0}个附件", moveCount);
                }
            }
            else
            {
                messageBuilder.AppendFormat("{0}:没有找到需要升级的附件", Path.Combine(currentDirectory, "Old", Path.Combine(currentDirectory, "Old", "ConvertibleGiftImages")));
                result = true;
            }
            message = messageBuilder.ToString();
            return result;
        }

        /// <summary>
        /// 升级头像/群组Logo
        /// </summary>
        /// <param name="oldDirectoryName">旧目录名</param>
        /// <param name="newDirectoryName">新目录名</param>
        /// <param name="searchPattern">搜索字符串</param>
        /// <returns></returns>
        private bool UpdateAttachmentAvatars(string oldDirectoryName, string newDirectoryName, out string message, string searchPattern = "*.jpg-original.jpg")
        {
            int moveCount = 0;
            int allCount = 0;
            string newAvatarFilePath = string.Empty;
            bool result = false;
            Directory.SetCurrentDirectory(currentDirectory);

            //获取到文件夹下的所有文件
            StringBuilder messageBuilder = new StringBuilder();
            string oldFilePath = Path.Combine(currentDirectory, "Old", oldDirectoryName);
            string[] oldAvatarFilePaths = Directory.GetFiles(oldFilePath, searchPattern, SearchOption.AllDirectories);

            allCount = oldAvatarFilePaths.Length;

            if (allCount > 0)
            {
                foreach (var oldAvatarFilePath in oldAvatarFilePaths)
                {
                    long objectId = GetObjectIdByFilePath(oldAvatarFilePath);
                    if (objectId <= 0)
                    {
                        continue;
                    }

                    string newAvatarDirectoryPath = GetRelativePathById(objectId, newDirectoryName);
                    if (!Directory.Exists(newAvatarDirectoryPath))
                        Directory.CreateDirectory(newAvatarDirectoryPath);

                    if (newDirectoryName == "Avatars")
                    {
                        newAvatarFilePath = Path.Combine(newAvatarDirectoryPath, objectId + "_big.jpg");
                    }
                    else
                    {
                        newAvatarFilePath = Path.Combine(newAvatarDirectoryPath, objectId + ".jpg");
                    }
                    if (!System.IO.File.Exists(newAvatarFilePath))
                    {
                        System.IO.File.Move(oldAvatarFilePath, newAvatarFilePath);
                        if (newDirectoryName == "GroupLogo")
                        {
                            SetResizedLogo("101100", objectId + ".jpg", newAvatarDirectoryPath);
                        }
                        else if (newDirectoryName == "BarSectionLogo")
                        {
                            SetResizedLogo("101201", objectId + ".jpg", newAvatarDirectoryPath);
                        }
                        else if (newDirectoryName == "Avatars")
                        {
                            SetUserAvatars(objectId);
                        }
                        moveCount++;
                    }
                }

                messageBuilder.AppendFormat("{0}:一共找到{1}个可以升级的附件", oldFilePath, allCount);
                if (moveCount > 0)
                {
                    result = true;
                    messageBuilder.AppendFormat("，成功升级了{0}个附件", moveCount);
                }
            }
            message = messageBuilder.ToString();
            return result;
        }

        /// <summary>
        /// 升级照片附件
        /// </summary>
        /// <returns></returns>
        private bool UpdatePhotoBySQL(out string message)
        {
            SqlConnection sqlConnection = GetConnection();
            List<long> photoIds = new List<long>();
            List<long> albumIds = new List<long>();
            Dictionary<long, List<long>> photosInAlbum = new Dictionary<long, List<long>>();
            Directory.SetCurrentDirectory(currentDirectory);
            int allCount = 0;
            int moveCount = 0;
            bool result = false;
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                sqlConnection.Open();
                SqlCommand selectCommand = new SqlCommand("select PhotoId,AlbumId from spb_Photos", sqlConnection);
                SqlDataReader dr = selectCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    photoIds.Add(dr.GetInt64(0));
                    albumIds.Add(dr.GetInt64(1));
                    if (!photosInAlbum.ContainsKey(dr.GetInt64(1)))
                        photosInAlbum[dr.GetInt64(1)] = new List<long>();
                    photosInAlbum[dr.GetInt64(1)].Add(dr.GetInt64(0));
                }
                dr.Close();
                sqlConnection.Close();

                foreach (var albumId in albumIds)
                {
                    string newPhotoRelatedPath = GetRelativePathById(albumId, "Photo");
                    if (!Directory.Exists(newPhotoRelatedPath))
                        Directory.CreateDirectory(newPhotoRelatedPath);

                    foreach (var photoId in photosInAlbum[albumId])
                    {
                        string oldPhotoRelatedPath = Path.Combine(currentDirectory, GetOldRelativePath("Galleries", photoId));
                        if (!Directory.Exists(oldPhotoRelatedPath))
                            continue;
                        foreach (var photoFilePath in Directory.GetFiles(oldPhotoRelatedPath))
                        {
                            allCount++;
                            FileInfo oldFileInfo = new FileInfo(photoFilePath);
                            string newPhotoFilePath = Path.Combine(currentDirectory, newPhotoRelatedPath, oldFileInfo.Name);
                            if (!System.IO.File.Exists(newPhotoFilePath))
                            {
                                System.IO.File.Move(photoFilePath, newPhotoFilePath);
                                moveCount++;
                                string tenantTypeId = "100302";
                                TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);
                                if (tenantAttachmentSettings != null && tenantAttachmentSettings.ImageSizeTypes != null && tenantAttachmentSettings.ImageSizeTypes.Count > 0)
                                {
                                    foreach (var imageSizeType in tenantAttachmentSettings.ImageSizeTypes)
                                    {
                                        IStoreFile file = storeProvider.GetResizedImage(newPhotoRelatedPath, oldFileInfo.Name, imageSizeType.Size, imageSizeType.ResizeMethod);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                sqlConnection.Close();
            }

            messageBuilder.AppendFormat("{0}:一共找到{1}个可以升级的照片", storeProvider.JoinDirectory("Old", "Galleries"), allCount);
            if (moveCount > 0)
            {
                result = true;
                messageBuilder.AppendFormat("，成功升级了{0}个照片", moveCount);
            }
            message = messageBuilder.ToString();
            return result;
        }


        /// <summary>
        /// 旧程序附件目录
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="directory"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public string GetOldRelativePath(string directory, long objectId)
        {
            string idString = objectId.ToString().PadLeft(12, '0');
            return storeProvider.JoinDirectory("Old", directory, idString.Substring(0, 3), idString.Substring(3, 3), idString.Substring(6, 3), idString.Substring(9, 3));
        }


        /// <summary>
        /// 旧友情链接相对地址
        /// </summary>
        /// <param name="oldDirectory"></param>
        /// <param name="oldSubDirectory"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public string GetOldLinkRelativePath(string oldDirectory, string oldSubDirectory, long objectId)
        {
            string idString = objectId.ToString().PadLeft(9, '0');
            return storeProvider.JoinDirectory("Old", oldDirectory, oldSubDirectory, idString.Substring(0, 3), idString.Substring(3, 3), idString.Substring(6, 3));
        }

        /// <summary>
        /// 根据文件路径解析用户id
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private long GetObjectIdByFilePath(string path)
        {
            Regex regex = new Regex(@"\\(?<objectId>[\d]+?)\.");
            int index = path.IndexOf("Uploads");
            if (index > 0)
            {
                path = path.Substring(index, path.Length - index);
            }
            Match match = regex.Match(path);

            long objectId = 0;

            if (match.Success)
            {
                string objectIdStr = match.Groups["objectId"].Value;
                long.TryParse(objectIdStr, out objectId);
            }
            return objectId;
        }

        /// <summary>
        /// 获取新头像/群组Logo/贴吧Logo/友情链接存储的相对路径
        /// </summary>
        private string GetRelativePathById(long objectId, string directoryName)
        {
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            string idString = objectId.ToString().PadLeft(15, '0');
            return storeProvider.JoinDirectory(directoryName, idString.Substring(0, 5), idString.Substring(5, 5), idString.Substring(10, 5));
        }

        #region 升级日志文件
        /// <summary>
        /// 获取升级日志文件名
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName()
        {
            return currentDirectory + "\\upgrade.log";
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
        private bool WriteLogFile(string message)
        {
            string fileName = GetLogFileName();
            if (!EnsureFileExist(fileName))
                return false;

            StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8);   //该编码类型不会改变已有文件的编码类型
            sw.WriteLine(DateTime.Now.ToString() + "：" + message);
            sw.Close();
            return true;
        }

        /// <summary>
        /// 读取日志文件
        /// </summary>
        /// <param name="logFilePath">日志文件路径</param>
        /// <returns></returns>
        private string ReadLogFile(string logFilePath)
        {
            StreamReader sr = new StreamReader(logFilePath);
            StringBuilder log = new StringBuilder();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                log.Append(line.ToString() + "<br/>");
            }
            return log.ToString();
        }
        #endregion
    }
}
