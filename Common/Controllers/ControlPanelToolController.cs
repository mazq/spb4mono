//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Spacebuilder.Search;
using Spacebuilder.Tasks;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Logging;
using Tunynet.Mvc;
using Tunynet.Tasks;
using Tunynet.UI;
using Tunynet.Utilities;
using Tunynet.Common.Configuration;
using Tunynet.FileStore;
using System.IO;
using System.Linq;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 控制面板工具Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = false)]
    [TitleFilter(IsAppendSiteName = true, TitlePart = "后台管理")]
    [ManageAuthorize]
    public class ControlPanelToolController : Controller
    {
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private OperationLogService logService = new OperationLogService();
        private User currentUser = (User)UserContext.CurrentUser;
        private TaskService taskService = new TaskService();

        #region private items

        #endregion

        /// <summary>
        /// 控制面板工具首页
        /// </summary>
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// 管理索引
        /// </summary>
        [HttpGet]
        public ActionResult ManageIndex()
        {
            pageResourceManager.InsertTitlePart("索引管理");
            IEnumerable<ISearcher> searchers = SearcherFactory.GetSearchers();
            return View(searchers);
        }

        /// <summary>
        /// 重建索引
        /// </summary>
        /// <param name="code">CODE</param>
        /// <returns></returns>
        public ActionResult RebuildIndex(string code)
        {
            ISearcher searcher = SearcherFactory.GetSearcher(code);

            string name = searcher.Name;
            string manageIndexUrl = SiteUrls.Instance().ManageIndex();
            string rebuildIndexUrl = SiteUrls.Instance().RebuildIndex(code);

            #region 操作日志
            OperationLogEntry log = new OperationLogEntry();
            log.DateCreated = DateTime.UtcNow;
            log.OperatorUserId = currentUser.UserId;
            log.Operator = currentUser.DisplayName;
            log.OperatorIP = Tunynet.Utilities.WebUtility.GetIP();
            log.OperationType = EntityOperationType.Update.ToString();
            log.OperationObjectName = "重建" + name + "索引";
            log.Source = "索引管理";
            log.AccessUrl = rebuildIndexUrl;
            #endregion

            string url = SiteUrls.Instance().ControlPanelOperating("正在执行[重建\"" + name + "\"索引]操作，请耐心等待", manageIndexUrl, rebuildIndexUrl);

            searcher.RebuildIndex();

            log.Description = "重建" + name + "索引成功";
            logService.Create(log);

            return Redirect(SiteUrls.Instance().ControlPanelSuccess("执行成功", SiteUrls.Instance().ManageIndex()));
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public ActionResult ResetCache()
        {
            return Redirect(SiteUrls.Instance().ControlPanelOperating("正在执行清除缓存操作", SiteUrls.Instance().ControlPanelTool(), SiteUrls.Instance().ResetCache(), "你确定要清除缓存吗？"));
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public ActionResult _ResetCache()
        {
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            cacheService.Clear();
            return Redirect(SiteUrls.Instance().ControlPanelSuccess("执行成功", SiteUrls.Instance().ControlPanelTool()));
        }

        /// <summary>
        /// 重启站点
        /// </summary>
        public ActionResult UnloadAppDomain()
        {
            return Redirect(SiteUrls.Instance().ControlPanelOperating("正在执行重启站点操作", SiteUrls.Instance().ControlPanelTool(), SiteUrls.Instance().UnloadAppDomain(), "你确定要重启站点吗？"));
        }

        /// <summary>
        /// 重启站点
        /// </summary>
        public ActionResult _UnloadAppDomain()
        {
            System.Web.HttpRuntime.UnloadAppDomain();
            return Redirect(SiteUrls.Instance().ControlPanelSuccess("执行成功", SiteUrls.Instance().ControlPanelTool()));
        }

        /// <summary>
        /// 重建缩略图
        /// </summary>
        /// <returns></returns>
        public ActionResult RebuildingThumbnails()
        {
            IEnumerable<TenantAttachmentSettings> tenantAttachmentSettings = TenantAttachmentSettings.GetAll();

            ViewData["TenantAttachmentSettings"] = tenantAttachmentSettings;
            ViewData["TenantLogoSettings"] = TenantLogoSettings.GetAll();

            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            string rootPath = WebUtility.GetPhysicalFilePath("~");
            string uploadsPath = storeProvider.StoreRootPath;
            if (storeProvider.StoreRootPath.StartsWith(rootPath))
                uploadsPath = uploadsPath.Replace(rootPath,"");
            ViewData["uploadsPath"] = uploadsPath;
            Dictionary<string, string> ApplicationName = new Dictionary<string, string>();

            TenantTypeService tenantTypeService = new TenantTypeService();
            IEnumerable<TenantType> tenantTypes = tenantTypeService.Gets(null, null);

            foreach (var item in tenantTypes)
            {
                TenantType type = tenantTypeService.Get(item.TenantTypeId);
                ApplicationName[item.TenantTypeId] = type != null ? type.Name : item.Name;
            }

            ViewData["ApplicationName"] = ApplicationName;

            return View();
        }

        /// <summary>
        /// 重建缩略图
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns></returns>
        public ActionResult _RebuildingThumbnails(string tenantTypeId)
        {
            TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();

            if (tenantAttachmentSettings == null)
                return Content(string.Empty);

            string path = WebUtility.GetPhysicalFilePath(Path.Combine(storeProvider.StoreRootPath, tenantAttachmentSettings.TenantAttachmentDirectory));

            ResetThumbnails(path, tenantAttachmentSettings);

            //重建缩略图的代码
            return Redirect(SiteUrls.Instance().ControlPanelSuccess("执行成功", SiteUrls.Instance().RebuildingThumbnails()));
        }

        /// <summary>
        /// 重新创建Log
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ReBuildingLogs(string tenantTypeId)
        {
            TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(tenantTypeId);

            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();

            if (tenantLogoSettings == null)
                return Content(string.Empty);

            string path = WebUtility.GetPhysicalFilePath(Path.Combine(storeProvider.StoreRootPath, tenantLogoSettings.TenantLogoDirectory));

            ResetLogos(path, tenantLogoSettings);

            //重建缩略图的代码
            return Redirect(SiteUrls.Instance().ControlPanelSuccess("执行成功", SiteUrls.Instance().RebuildingThumbnails()));
        }

        /// <summary>
        /// 重置缩略图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="settings"></param>
        private void ResetThumbnails(string path, TenantAttachmentSettings settings)
        {
            if (settings.ImageSizeTypes == null || settings.ImageSizeTypes.Count == 0)
                return;

            if (!Directory.Exists(path))
                return;

            List<string> files = Directory.GetFiles(path).ToList();

            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();

            List<string> originalList = new List<string>();
            List<string> fileList = new List<string>();

            foreach (var file in files)
            {
                string suffix = "gif,jpg,png,bmp,jpeg";

                bool isContinue = true;

                foreach (var item in suffix.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (file.ToLower().EndsWith(item))
                    {
                        isContinue = false;
                        break;
                    }
                }

                if (isContinue)
                    continue;

                string fileName = GetUploadFileName(file);
                if (originalList.Contains(fileName))
                    continue;

                originalList.Add(fileName);

                foreach (var item in settings.ImageSizeTypes)
                {
                    string name = Path.Combine(path, storeProvider.GetSizeImageName(fileName, item.Size, item.ResizeMethod));

                    if (!fileList.Contains(name))
                        fileList.Add(name);

                    if (files.Contains(name))
                        continue;
                    try { storeProvider.GetResizedImage(path, fileName, item.Size, item.ResizeMethod); }
                    catch (Exception) { }
                }
            }

            //删除无用数据
            foreach (var file in files)
            {
                string name = GetFileNameWistOutPath(file);
                if (!fileList.Contains(file) && GetUploadFileName(file) != name && GetOriginalFileName(file) != name)
                    System.IO.File.Delete(file);
            }

            string[] paths = Directory.GetDirectories(path);
            foreach (var item in paths)
                ResetThumbnails(item, settings);
        }

        /// <summary>
        /// 根据配置重置Logo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="settings"></param>
        private void ResetLogos(string path, TenantLogoSettings settings)
        {
            if (!Directory.Exists(path))
                return;

            if (settings == null || settings.ImageSizeTypes == null || settings.ImageSizeTypes.Count == 0)
                return;

            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();

            string[] files = Directory.GetFiles(path);

            List<string> originalList = new List<string>();
            List<string> fileList = new List<string>();

            foreach (var file in files)
            {
                if (file.EndsWith(".db"))
                    continue;

                string fileName = GetUploadFileName(file);

                if (originalList.Contains(fileName))
                    continue;

                originalList.Add(fileName);

                foreach (var type in settings.ImageSizeTypes.Values)
                {
                    string name = storeProvider.GetSizeImageName(GetFileNameWistOutPath(file), type.Key, type.Value);
                    string filePath = Path.Combine(path, name);
                    if (!fileList.Contains(filePath))
                        fileList.Add(filePath);
                    if (files.Contains(filePath))
                        continue;

                    try
                    {
                        storeProvider.GetResizedImage(path, GetUploadFileName(filePath), type.Key, type.Value);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            //删除无用数据
            foreach (var file in files)
            {
                string name = GetFileNameWistOutPath(file);
                if (!fileList.Contains(file) && GetUploadFileName(file) != name && GetOriginalFileName(file) != name)
                    System.IO.File.Delete(file);
            }

            string[] paths = Directory.GetDirectories(path);
            foreach (var item in paths)
                ResetLogos(item, settings);
        }

        /// <summary>
        /// 获取程序处理之后的原图名称
        /// </summary>
        /// <returns></returns>
        private string GetOriginalFileName(string filePath)
        {
            string fileName = GetFileNameWistOutPath(filePath);
            return fileName.Substring(0, fileName.IndexOf('.')) + "-original" + fileName.Substring(fileName.LastIndexOf("."));
        }

        /// <summary>
        /// 获取文件的目录，不包含文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetFilePathWithOutFileName(string filePath)
        {
            return filePath.Substring(0, filePath.LastIndexOf('\\'));
        }

        /// <summary>
        /// 获取用户上传的时候使用的文件名称
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        private string GetUploadFileName(string filePath)
        {
            string fileName = GetFileNameWistOutPath(filePath);
            return fileName.Substring(0, fileName.IndexOf('.')) + fileName.Substring(fileName.LastIndexOf("."));
        }

        /// <summary>
        /// 没有路径的文件名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        private string GetFileNameWistOutPath(string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf('\\') + 1);
        }

        #region 自运行任务

        /// <summary>
        /// 管理任务
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageTasks(int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("任务管理");
            //todo:libsh:底层方法需要调整，需获取任务运行状态
            IEnumerable<TaskDetail> tasks = taskService.GetAll();
            return View(tasks);
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTask(int Id)
        {
            pageResourceManager.InsertTitlePart("编辑任务");
            TaskDetail taskDetail = taskService.Get(Id);

            if (taskDetail == null)
                return HttpNotFound();

            TaskDetailEditModel editModel = taskDetail.AsEditModel();
            InitRules(editModel);
            return View(editModel);
        }

        /// <summary>
        /// 管理任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTask(TaskDetailEditModel model)
        {
            InitRules(model);

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                taskService.Update(model.AsTaskDetail());
            }
            catch (Exception e)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "更新失败！");
                return View(model);
            }

            TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "更新成功！");
            return this.RedirectToAction("ManageTasks");
        }

        /// <summary>
        /// 直接运行任务
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RunTask(int id)
        {
            TaskDetail td = taskService.Get(id);
            if (td == null)
            {
                return Json(new { status = true, message = "执行失败!" });

            }

            if (Utility.IsDistributedDeploy() && (td.RunAtServer == RunAtServer.Master || td.RunAtServer == RunAtServer.Search))
            {
                try
                {
                    TaskServiceClient client = new TaskServiceClient("WCFTaskService");
                    client.RunTask(id);
                }
                catch (Exception e)
                {
                    return Json(new { message = "执行失败!" });
                }
            }
            else
            {
                TaskSchedulerFactory.GetScheduler().Run(id);
            }

            return Json(new { success = true, message = "执行成功!" });
        }

        /// <summary>
        /// 初始化任务规则
        /// </summary>
        private void InitRules(TaskDetailEditModel editModel)
        {
            List<SelectListItem> seconds = new List<SelectListItem>();
            List<SelectListItem> minutes = new List<SelectListItem>();
            List<SelectListItem> hours = new List<SelectListItem>();
            List<SelectListItem> mouth = new List<SelectListItem>();
            List<SelectListItem> day = new List<SelectListItem>();
            List<SelectListItem> dayOfMouth = new List<SelectListItem>();

            for (int i = 0; i < 60; i++)
            {
                seconds.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Seconds == i.ToString() });
                minutes.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Minutes == i.ToString() });
                if (i > 0 && i <= 23)
                    hours.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Hours == i.ToString() });

                if (i > 0 && i <= 12)
                    mouth.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Mouth == i.ToString() });

                if (i > 0 && i <= 31)
                {
                    day.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.Day == i.ToString() });
                    dayOfMouth.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = editModel.DayOfMouth == i.ToString() });
                }
            }

            ViewData["Seconds"] = seconds;
            ViewData["Minutes"] = minutes;
            ViewData["Hours"] = hours;
            ViewData["Mouth"] = mouth;
            ViewData["Day"] = day;
            ViewData["DayOfMouth"] = dayOfMouth;

            ViewData["Frequency"] = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "每天", Value  = ((int)TaskFrequency.EveryDay).ToString(), Selected = TaskFrequency.EveryDay == editModel.Frequency },
                new SelectListItem(){ Text = "每周", Value  = ((int)TaskFrequency.Weekly).ToString(), Selected = TaskFrequency.Weekly == editModel.Frequency },
                new SelectListItem(){ Text = "每月", Value  = ((int)TaskFrequency.PerMonth).ToString(), Selected = TaskFrequency.PerMonth == editModel.Frequency  }
            };
            ViewData["Number"] = new List<SelectListItem>() { 
                new SelectListItem(){ Text = "第一周", Value  = "1",Selected = editModel.Number == "1" },
                new SelectListItem(){ Text = "第二周", Value  = "2",Selected = editModel.Number == "2" },
                new SelectListItem(){ Text = "第三周", Value  = "3",Selected = editModel.Number == "3" },
                new SelectListItem(){ Text = "第四周", Value  = "4",Selected = editModel.Number == "4" }

            };

            ViewData["DayOfWeek"] = new Dictionary<string, string>() { { "周一", "2" }, { "周二", "3" }, { "周三", "4" }, { "周四", "5" }, { "周五", "6" }, { "周六", "7" }, { "周日", "1" } };
            ViewData["WeeklyOfMouth"] = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "周一", Value  = "2", Selected = editModel.WeeklyOfMouth == "2" },
                new SelectListItem(){ Text = "周二", Value  = "3", Selected = editModel.WeeklyOfMouth == "3" },
                new SelectListItem(){ Text = "周三", Value  = "4", Selected = editModel.WeeklyOfMouth == "4" },
                new SelectListItem(){ Text = "周四", Value  = "5", Selected = editModel.WeeklyOfMouth == "5" },
                new SelectListItem(){ Text = "周五", Value  = "6", Selected = editModel.WeeklyOfMouth == "6" },
                new SelectListItem(){ Text = "周六", Value  = "7", Selected = editModel.WeeklyOfMouth == "7" },
                new SelectListItem(){ Text = "周日", Value  = "1", Selected = editModel.WeeklyOfMouth == "1" }
            };
        }

        #endregion

    }
}