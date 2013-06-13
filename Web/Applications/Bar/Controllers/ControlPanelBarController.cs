//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tunynet.UI;
using Tunynet.Common;
using Tunynet;
using Spacebuilder.Common;
using Tunynet.Mvc;
using Tunynet.Utilities;
using System.IO;
using Tunynet.Common.Configuration;

namespace Spacebuilder.Bar.Controllers
{
    
    
    
    
    /// <summary>
    /// 帖吧管理Controller
    /// </summary>
    [ManageAuthorize(CheckCookie = false)]
    [TitleFilter(TitlePart = "内容管理", IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = true)]
    public class ControlPanelBarController : Controller
    {
        #region Private Items

        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private BarSectionService barSectionService = new BarSectionService();
        private CategoryService categoryService = new CategoryService();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private BarThreadService barThreadService = new BarThreadService();
        private BarPostService barPostService = new BarPostService();
        private TenantTypeService tenantTypeService = new TenantTypeService();
        PointService pointService = new PointService();


        #endregion

        #region 页面
        
        
        /// <summary>
        /// 后台管理帖吧页面
        /// </summary>
        /// <returns>后台管理帖吧页面</returns>
        [HttpGet]
        public ActionResult ManageBars(ManageBarEditModel model, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("帖吧管理");
            
            //long userId = Request.QueryString.Gets<long>(model.UserId, new List<long>()).FirstOrDefault();
            
            BarSectionQuery query = model.GetQuery();
            long userId = Request.QueryString.Gets<long>("UserId", new List<long>()).FirstOrDefault();
            if (userId > 0)
                query.UserId = userId;
            ViewData["UserId"] = query.UserId;

            PagingDataSet<BarSection> sections = barSectionService.Gets(TenantTypeIds.Instance().Bar(), query, model.PageSize ?? 20, pageIndex);
            //为提升性能，批量获取吧主用户
            userService.GetFullUsers(sections.Select(n => n.UserId));
            ViewData["BarSections"] = sections;
            //foreach (var item in sections)
            //{
            //    
            
            
            //    ViewData["BarSections-Category-" + item.SectionId] = categoryService.GetOwnerCategories(item.SectionId, TenantTypeIds.Instance().BarSection());
            //}
            
            

            PagingDataSet<Category> categories = categoryService.GetCategories(PubliclyAuditStatus.Success, TenantTypeIds.Instance().BarSection(), null, 1);

            
            
            
            
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName", model.CategoryId);

            
            //Dictionary<bool, string> enabledValues = new Dictionary<bool, string> { { true, "是" }, { false, "否" } };
            //ViewData["Enabled"] = new SelectList(enabledValues.Select(n => new { text = n.Value, value = n.Key.ToString().ToLower() }), "value", "text", model.Enabled);
            

            
            

            List<SelectListItem> enabledSelectLists = new List<SelectListItem> {
                 new SelectListItem{ Text="是",Value=true.ToString()},
                 new SelectListItem{ Text="否",Value=false.ToString()}
            };
            ViewData["Enabled"] = new SelectList(enabledSelectLists, "Value", "Text", model.Enabled);
            return View(model);
        }

        /// <summary>
        /// 管理帖子页面
        /// </summary>
        /// <returns>管理帖子页面</returns>
        [HttpGet]
        public ActionResult ManageThreads(ManageThreadEditModel model, int pageIndex = 1, string tenantTypeId = null)
        {
            if (string.IsNullOrEmpty(tenantTypeId))
                tenantTypeId = TenantTypeIds.Instance().Bar();

            pageResourceManager.InsertTitlePart("帖子管理");
            
            

            
            

            List<SelectListItem> SelectListItem_TrueAndFlase = new List<SelectListItem> { new SelectListItem { Text = "是", Value = true.ToString() }, new SelectListItem { Text = "否", Value = false.ToString() } };

            ViewData["IsEssential"] = new SelectList(SelectListItem_TrueAndFlase, "Value", "Text", model.IsEssential);
            ViewData["IsSticky"] = new SelectList(SelectListItem_TrueAndFlase, "Value", "Text", model.IsSticky);

            ViewData["BarThreads"] = barThreadService.Gets(tenantTypeId, model.GetBarThreadQuery(), model.PageSize ?? 20, pageIndex);

            ViewData["TenantType"] = tenantTypeService.Get(tenantTypeId);

            return View(model);
        }
        #endregion

        #region 批量操作-帖子
        /// <summary>
        /// 批量设置/取消精华
        /// </summary>
        /// <param name="threadIds">帖子id</param>
        /// <param name="isEssential">是否精华</param>
        /// <returns>批量设置取消精华</returns>
        [HttpPost]
        public ActionResult BatchSetEssential(List<long> threadIds, bool isEssential = true)
        {
            barThreadService.BatchSetEssential(threadIds, isEssential);
            return Json(new StatusMessageData(StatusMessageType.Success, isEssential ? "批量设置精华成功" : "批量取消精华成功"));
        }

        /// <summary>
        /// 设置帖子的审核状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchUpdateThreadAuditStatus(List<long> threadIds, bool isApproved = true)
        {
            barThreadService.BatchUpdateAuditStatus(threadIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, "批量设置审核状态成功"));
        }
        /// <summary>
        /// 设置帖子的审核状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchUpdateThreadAuditStatu(long threadId, bool isApproved = true)
        {
            List<long> threadIds = new List<long>() { threadId };
            barThreadService.BatchUpdateAuditStatus(threadIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置审核状态成功"));
        }

        /// <summary>
        /// 批量置顶
        /// </summary>
        /// <param name="threadIds">操作的帖子id</param>
        /// <param name="isSticky">是否置顶</param>
        /// <param name="stickyDate">置顶时间</param>
        /// <returns>批量置顶</returns>
        [HttpPost]
        public ActionResult BatchSetSticky(List<long> threadIds, bool isSticky, DateTime stickyDate)
        {
            barThreadService.BatchSetSticky(threadIds, isSticky, stickyDate);
            return Json(new StatusMessageData(StatusMessageType.Success, "批量设置置顶状态成功"));
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="threadIds">准备删除的帖子id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchDeleteThread(List<long> threadIds)
        {
            foreach (var id in threadIds)
                barThreadService.Delete(id);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        #endregion

        #region 回帖

        /// <summary>
        /// 管理回帖页面
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="model">回帖管理的model</param>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns>管理回帖</returns>
        [HttpGet]
        public ActionResult ManagePosts(ManagePostsEditModel model, int pageIndex = 1, string tenantTypeId = null)
        {
            if (string.IsNullOrEmpty(tenantTypeId))
                tenantTypeId = TenantTypeIds.Instance().Bar();

            ViewData["TenantType"] = tenantTypeService.Get(tenantTypeId);

            pageResourceManager.InsertTitlePart("回帖管理");
            
            

            ViewData["BarPosts"] = barPostService.Gets(tenantTypeId, model.AsBarPostQuery(), model.PageSize ?? 20, pageIndex);
            return View(model);
        }

        #endregion

        #region 编辑管理帖子
        /// <summary>
        /// 编辑、添加帖吧
        /// </summary>
        /// <param name="sectionId">准备编辑的帖吧id</param>
        /// <returns>编辑添加帖吧</returns>
        [HttpGet]
        public ActionResult EditSection(long? sectionId = null)
        {
            pageResourceManager.InsertTitlePart("帖吧设置");
            if (sectionId.HasValue)
            {
                
                
                BarSection section = barSectionService.Get(sectionId ?? 0);
                if (section == null)
                    return HttpNotFound();
                ViewData["UserId"] = section.UserId;
                ViewData["ManagerUserIds"] = barSectionService.GetSectionManagers(sectionId ?? 0).Select(n => n.UserId);
                IBarSettingsManager manager = DIContainer.Resolve<IBarSettingsManager>();
                BarSettings settings = manager.Get();
                ViewData["SectionManagerMaxCount"] = settings.SectionManagerMaxCount;

                BarSectionEditModel model = section.AsEditModel();
                IEnumerable<Category> categories = categoryService.GetCategoriesOfItem(section.SectionId, 0, TenantTypeIds.Instance().BarSection());
                if (categories != null && categories.Count() > 0)
                    model.CategoryId = categories.ElementAt(0).CategoryId;

                return View(model);
            }
            else
            {
                ViewData["UserId"] = UserContext.CurrentUser.UserId;
            }
            return View(new BarSectionEditModel());
        }

        /// <summary>
        /// 添加编辑帖吧
        /// </summary>
        /// <param name="model">帖吧的EditModel</param>
        /// <returns>添加编辑帖吧</returns>
        [HttpPost]
        public ActionResult EditSection(BarSectionEditModel model)
        {
            HttpPostedFileBase logoImage = Request.Files["LogoImage"];
            string logoImageFileName = string.Empty;
            Stream stream = null;
            if (logoImage != null && !string.IsNullOrEmpty(logoImage.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().BarSection());
                if (!tenantLogoSettings.ValidateFileLength(logoImage.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("Logo文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return View(model);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                if (!logoSettings.ValidateFileExtensions(logoImage.FileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "Logo文件是不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return View(model);
                }
                stream = logoImage.InputStream;
                logoImageFileName = logoImage.FileName;
            }


            IEnumerable<long> managerUserIds = Request.Form.Gets<long>("ManagerUserIds");

            if (model.SectionId == 0)
            {
                BarSection section = model.AsBarSection();
                
                //long userId = Request.QueryString.Gets<long>(model.UserId, new List<long>()).FirstOrDefault();
                

                section.UserId = Request.Form.Gets<long>("UserId", new List<long>()).FirstOrDefault();
                if (section.UserId == 0)
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "请输入吧主信息");
                    return View(model);
                }
                section.LogoImage = logoImageFileName;
                section.DisplayOrder = model.DisplayOrder ?? 100;
                if (managerUserIds != null && managerUserIds.Count() > 0)
                    managerUserIds = managerUserIds.Where(n => n != section.UserId);

                bool isCreated = barSectionService.Create(section, UserContext.CurrentUser.UserId, managerUserIds, stream);

                categoryService.AddItemsToCategory(new List<long> { section.SectionId }, model.CategoryId, 0);

                if (isCreated)
                {
                    TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "创建成功");
                    return Redirect(SiteUrls.Instance().EditSection(section.SectionId));
                }
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "创建失败");
                return View(model);
            }
            else
            {
                BarSection section = model.AsBarSection();
                
                

                long userId = Request.Form.Gets<long>("UserId", new List<long>()).FirstOrDefault();
                if (userId == 0)
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "必须输入吧主信息");
                    ViewData["ManagerUserIds"] = barSectionService.GetSectionManagers(section.SectionId).Select(n => n.UserId);
                    IBarSettingsManager manager = DIContainer.Resolve<IBarSettingsManager>();
                    BarSettings settings = manager.Get();
                    ViewData["SectionManagerMaxCount"] = settings.SectionManagerMaxCount;
                    return View(model);
                }
                section.UserId = userId;
                if (!string.IsNullOrEmpty(logoImageFileName))
                    section.LogoImage = logoImageFileName;
                section.DisplayOrder = model.DisplayOrder ?? 100;
                barSectionService.Update(section, UserContext.CurrentUser.UserId, managerUserIds, stream);
                categoryService.ClearCategoriesFromItem(section.SectionId, 0, TenantTypeIds.Instance().BarSection());
                categoryService.AddItemsToCategory(new List<long> { section.SectionId }, model.CategoryId, 0);

                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "更新成功");
                return Redirect(SiteUrls.Instance().EditSection(model.SectionId));
            }
        }

        /// <summary>
        /// 删除帖吧
        /// </summary>
        /// <param name="sectionId">被删除帖吧的id</param>
        /// <returns>删除帖吧操作</returns>
        [HttpPost]
        public ActionResult DeleteSection(long sectionId)
        {
            barSectionService.Delete(sectionId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        /// <summary>
        /// 帖吧设置
        /// </summary>
        /// <returns>帖吧设置</returns>
        [HttpGet]
        public ActionResult SectionSettings()
        {
            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["prePoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;

            IBarSettingsManager manager = DIContainer.Resolve<IBarSettingsManager>();
            BarSettings settings = manager.Get();
            return View(settings.AsEditModel());
        }

        /// <summary>
        /// 帖吧设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SectionSettings(SectionSettingsEditModel model)
        {
            IBarSettingsManager manager = DIContainer.Resolve<IBarSettingsManager>();
            manager.Save(model.AsBarSettings());

            TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "保存设置成功");

            return Redirect(SiteUrls.Instance().SectionSettings());
        }
        #endregion

        #region 批量操作帖吧

        /// <summary>
        /// 批量更新帖吧的审核状态
        /// </summary>
        /// <param name="sectionIds">被更新的帖吧id</param>
        /// <param name="isApproved">是否通过审核</param>
        /// <returns>批量更新帖吧的审核状态</returns>
        public ActionResult BatchUpdateSectionAuditStatus(List<long> sectionIds, bool isApproved = true)
        {
            barSectionService.BatchUpdateAuditStatus(sectionIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, isApproved ? "批量通过审核成功" : "批量取消审核成功"));
        }

        /// <summary>
        /// 更新帖吧的审核状态
        /// </summary>
        /// <param name="sectionId">被更新的帖吧id</param>
        /// <param name="isApproved">是否通过审核</param>
        /// <returns>更新帖吧的审核状态</returns>
        public ActionResult BatchUpdateSectionAuditStatu(long sectionId, bool isApproved = true)
        {
            List<long> sectionIds = new List<long>() { sectionId };
            barSectionService.BatchUpdateAuditStatus(sectionIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, isApproved ? "通过审核成功" : "取消审核成功"));
        }

        #endregion

    }
}