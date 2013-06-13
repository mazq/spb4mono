//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System.Web.Mvc;
using Tunynet.Common;
using Tunynet.UI;
using Tunynet;
using System;
using System.Collections.Generic;
using System.Linq;
using Tunynet.Mvc;
using Tunynet.Utilities;
using Spacebuilder.Search;
using Tunynet.Search;
using System.Text.RegularExpressions;
using Spacebuilder.Common;
using Tunynet.Logging;
using System.IO;
using System.Web;
using Tunynet.Common.Configuration;
using System.Net.Mail;
using Tunynet.Email;
using System.Text;
using Tunynet.Globalization;


namespace Spacebuilder.Common
{
    /// <summary>
    /// 积分记录控制面板Controller
    /// </summary>
    //[ManageAuthorize(RequireSystemAdministrator = true, CheckCookie = true)]
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = false)]
    [TitleFilter(IsAppendSiteName = true, TitlePart = "后台管理")]
    [ManageAuthorize]
    public class ControlPanelOperationController : Controller
    {
        private AccountBindingService accountBindingService = new AccountBindingService();

        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private RecommendService recommendService = new RecommendService();
        private OperationLogService logService = new OperationLogService();
        private SearchedTermService termService = new SearchedTermService();
        private RoleService roleService = DIContainer.Resolve<RoleService>();
        private ImpeachReportService impeachReportService = new ImpeachReportService();
        private LinkService linkService = new LinkService();
        private CategoryService categoryService = new CategoryService();
        private AnnouncementService announcementService = new AnnouncementService();
        private LogoService logoService = new LogoService(TenantTypeIds.Instance().Link());
        private AdvertisingService advertisingService = new AdvertisingService();
        private MessageService messageService = new MessageService();
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private UserService UserService = new UserService();
        private EmailService emailService = new EmailService();

        /// <summary>
        /// 站点运行首页
        /// </summary>
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        #region 积分记录
        /// <summary>
        /// 管理积分记录
        /// </summary>
        [HttpGet]
        public ActionResult ManagePointRecords(int? pageIndex, int pageSize = 20)
        {
            UserService userService = new UserService();
            PointService pointService = new PointService();
            pageResourceManager.InsertTitlePart("管理积分记录");

            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["traPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("TradePoints")).CategoryName;
            ViewData["expPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ExperiencePoints")).CategoryName;
            ViewData["prePoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;

            
            
            
            bool? isIncome = null;
            DateTime? startDate = null;
            DateTime? endDate = null;
            string isSystemData = Request.QueryString["isSystemData"];
            IEnumerable<long> userIds = Request.QueryString.Gets<long>("userId");
            long? userId = null;
            string pointItemName = Request.QueryString.GetString("pointItemName", null);
            bool isCheck = false;
            isIncome = Request.QueryString.Get<bool?>("incomelist");

            if (isSystemData != null && !isSystemData.Equals("false"))
            {
                userId = 0;
                isCheck = true;
            }
            else if (userIds != null && userIds.Count() > 0)
            {
                userId = userIds.FirstOrDefault();
            }
            ViewData["isCheck"] = isCheck;

            if (Request.QueryString.Get<DateTime>("startdate") != DateTime.MinValue)
                startDate = Request.QueryString.Get<DateTime>("startDate");

            if (Request.QueryString.Get<DateTime>("enddate") != DateTime.MinValue)
                endDate = Request.QueryString.Get<DateTime>("endDate");
            if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            {
                DateTime? changDate = startDate;
                startDate = endDate;
                endDate = changDate;
            }
            if (endDate.HasValue)
                endDate = endDate.Value.AddDays(1).AddMilliseconds(-1);

            Dictionary<bool, string> income = new Dictionary<bool, string> { { true, "收入" }, { false, "支出" } };
            ViewData["incomeList"] = new SelectList(income.Select(n => new { text = n.Value.ToString(), value = n.Key.ToString().ToLower() }), "value", "text", isIncome);

            ViewData["userId"] = userId;
            
            PagingDataSet<PointRecord> pointRecords = pointService.GetPointRecords(userId, isIncome, pointItemName, startDate, endDate, pageSize, pageIndex ?? 1);

            Dictionary<long, string> displayNameDic = new Dictionary<long, string>();
            foreach (var item in pointRecords)
            {
                if (userService.GetUser(item.UserId) != null && userService.GetUser(item.UserId).DisplayName != null)
                    displayNameDic[item.UserId] = userService.GetUser(item.UserId).DisplayName;
            }
            ViewData["displayNameDic"] = displayNameDic;
            return View(pointRecords);
        }
        #endregion


        #region 搜索热词
        /// <summary>
        /// 管理搜索热词
        /// </summary>
        [HttpGet]
        public ActionResult ManageSearchedTerms(int? pageIndex = 1, int pageSize = 20)
        {

            CountService countService = new CountService(TenantTypeIds.Instance().Search());
            StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().Search());
            IEnumerable<ISearcher> searchers = SearcherFactory.GetDisplaySearchers();

            #region 搜索条件
            int stageCountDays = stageCountTypeManager.GetMaxDayCount(CountTypes.Instance().SearchCount());
            string countTypeNday = countService.GetStageCountType(CountTypes.Instance().SearchCount(), stageCountDays);
            string countTypeAll = CountTypes.Instance().SearchCount();

            pageResourceManager.InsertTitlePart("管理搜索热词");

            string term = Request.QueryString.GetString("term", string.Empty).Trim();
            bool isRealtime = false;

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (Request.QueryString.Get<DateTime>("startdate") != DateTime.MinValue)
                startDate = Request.QueryString.Get<DateTime>("startDate");

            if (Request.QueryString.Get<DateTime>("enddate") != DateTime.MinValue)
                endDate = Request.QueryString.Get<DateTime>("endDate");

            if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            {
                DateTime? changDate = startDate;
                startDate = endDate;
                endDate = changDate;
            }
            if (endDate.HasValue)
                endDate = endDate.Value;
            #endregion

            #region 下拉列表

            string searchTypeCode = Request.QueryString.GetString("searchTypeCode", string.Empty);
            Dictionary<string, string> searchTypeDic = new Dictionary<string, string>();
            searchTypeDic[SearcherFactory.GlobalSearchCode] = SearcherFactory.GlobalSearchName;
            foreach (var searcher in searchers)
            {
                searchTypeDic[searcher.Code] = searcher.Name;
            }

            SelectList selectList = new SelectList(searchTypeDic.Select(n => new { text = n.Value, value = n.Key }), "value", "text", searchTypeCode);

            ViewData["searchTypeCode"] = selectList;
            #endregion


            if (string.IsNullOrEmpty(searchTypeCode))
            {
                isRealtime = true;

            }

            ViewData["searchTypeDic"] = searchTypeDic;

            #region Count

            IEnumerable<SearchedTerm> manuals = null;
            if (pageIndex == 1)
                manuals = termService.GetManuals(searchTypeCode);


            Dictionary<long, int> countTypeNdayDic = new Dictionary<long, int>();
            Dictionary<long, int> countTypeAllDic = new Dictionary<long, int>();
            if (manuals != null)
            {
                if (!string.IsNullOrEmpty(term))
                    manuals = manuals.Where(n => Regex.IsMatch(n.Term, "^" + term.Trim()));
                if (!string.IsNullOrEmpty(searchTypeCode))
                    manuals = manuals.Where(n => n.SearchTypeCode == searchTypeCode);
                if (startDate != null)
                    manuals = manuals.Where(n => n.DateCreated >= startDate);
                if (endDate != null)
                    manuals = manuals.Where(n => n.DateCreated <= endDate);
                foreach (var item in manuals)
                {
                    countTypeNdayDic[item.Id] = countService.Get(countTypeNday, item.Id);
                    countTypeAllDic[item.Id] = countService.Get(countTypeAll, item.Id);
                }
            }
            ViewData["manuals"] = manuals;
            
            PagingDataSet<SearchedTerm> searchedTerms = termService.Gets(searchTypeCode, term, startDate, endDate, isRealtime, pageSize, pageIndex ?? 1);

            foreach (var item in searchedTerms)
            {
                countTypeNdayDic[item.Id] = countService.Get(countTypeNday, item.Id);
                countTypeAllDic[item.Id] = countService.Get(countTypeAll, item.Id);
            }

            ViewData["countTypeNdayDic"] = countTypeNdayDic;
            ViewData["countTypeAllDic"] = countTypeAllDic;

            #endregion

            return View(searchedTerms);
        }

        /// <summary>
        /// 编辑搜索热词控件
        /// </summary>
        [HttpGet]
        public ActionResult _EditSearchedTerms(long id, string term, string searchTypeCode)
        {
            IEnumerable<ISearcher> searchers = SearcherFactory.GetDisplaySearchers();
            if (searchers != null)
            {
                List<SelectListItem> itemList = new SelectList(searchers.Select(n => new { text = n.Name, value = n.Code }), "value", "text", searchTypeCode).ToList();
                bool selectedGlobal = false;
                if (searchTypeCode == SearcherFactory.GlobalSearchCode)
                {
                    selectedGlobal = true;
                }
                itemList.Add(new SelectListItem { Text = SearcherFactory.GlobalSearchName, Value = SearcherFactory.GlobalSearchCode, Selected = selectedGlobal });
                ViewData["searchList"] = itemList;
            }


            ViewData["id"] = id;

            return View();
        }

        /// <summary>
        /// 编辑搜索热词
        /// </summary>
        [HttpPost]
        public ActionResult EditSearchedTerms()
        {

            long id = Request.Form.Get<long>("id", 0);
            string term = WebUtility.UrlDecode(Request.Form.GetString("term", string.Empty).Trim());
            string searchTypeCode = Request.Form.GetString("searchList", string.Empty);

            if (id > 0)
            {
                termService.Update(id, term, searchTypeCode);
            }
            else
                termService.CreateByAdministrator(searchTypeCode, term);
            return Redirect(SiteUrls.Instance().ManageSearchedTerms());

        }

        /// <summary>
        /// 删除搜索热词
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteSearchedTerms(long id)
        {
            termService.Delete(id);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        /// <summary>
        /// 交换热词顺序
        /// </summary>
        [HttpPost]
        public JsonResult ChangeDisplayOrder()
        {
            long id = Request.Form.Get<long>("id", 0);
            long referenceId = Request.Form.Get<long>("referenceId", 0);
            termService.ChangeDisplayOrder(id, referenceId);
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }

        #endregion


        #region 推荐

        #region 推荐类别
        /// <summary>
        /// 管理推荐类别页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageRecommendTypes(string tenantTypeId)
        {
            pageResourceManager.InsertTitlePart("推荐类别管理");
            ViewData["tenantList"] = GetTenantSelectList(tenantTypeId);
            IEnumerable<RecommendItemType> recommend = recommendService.GetRecommendTypes(tenantTypeId);
            return View(recommend);
        }
        
        
        /// <summary>
        /// 所属下拉框绑定
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <returns></returns>
        private SelectList GetTenantSelectList(string tenantTypeId)
        {
            TenantTypeService tenantTypeService = new TenantTypeService();
            IEnumerable<TenantType> tenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Recommend());
            
            
            SelectList tenants = new SelectList(tenantTypes, "TenantTypeId", "Name", tenantTypeId);
            return tenants;
        }

        
        
        
        /// <summary>
        /// 编辑推荐类别模式框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditRecommendType(string recommendTypeId = null, string tenantTypeId = null)
        {
            RecommendItemType recommend = recommendService.GetRecommendType(recommendTypeId);
            RecommendItemTypeEditModel recommendItemTypeEditModel = new RecommendItemTypeEditModel();
            if (recommend != null)
                recommendItemTypeEditModel = recommend.AsEditModel();
            ViewData["tenantList"] = GetTenantSelectList(tenantTypeId);
            return View(recommendItemTypeEditModel);
        }

        /// <summary>
        /// 编辑推荐类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditRecommendType(RecommendItemTypeEditModel recommendItemTypeEditModel)
        {
            
            
            if (!ModelState.IsValid)
                return View();
            RecommendItemType recommend = recommendItemTypeEditModel.AsRecommendItemType();
            
            
            recommendService.UpdateRecommendType(recommend);
            ViewData["tenantList"] = GetTenantSelectList(recommendItemTypeEditModel.TenantTypeId);
            return Json(new StatusMessageData(StatusMessageType.Success, "更新成功"));
        }


        /// <summary>
        /// 创建推荐类别模式框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _CreateRecommendType()
        {
            ViewData["TenantTypeId"] = GetTenantSelectList(null);
            return View();
        }

        /// <summary>
        /// 创建推荐类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreateRecommendType(RecommentItemTypeCreateModel recommendItemTypeCreateModel)
        {
            if (recommendItemTypeCreateModel == null || !ModelState.IsValid)
                return View();
            RecommendItemType recommend = recommendItemTypeCreateModel.AsRecommendItemType();
            
            
            ViewData["tenantList"] = GetTenantSelectList(recommendItemTypeCreateModel.TenantTypeId);
            RecommendItemType recommendType = recommendService.GetRecommendType(recommendItemTypeCreateModel.TypeId);
            if (recommendType != null)
            {
                WebUtility.SetStatusCodeForError(Response);
                ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "已经存在的Id");
                return View(recommendItemTypeCreateModel);
            }
            bool isCreated = recommendService.CreateRecommendType(recommend);
            if (!isCreated)
            {
                WebUtility.SetStatusCodeForError(Response);
                ViewData["statusMessageData"] = new StatusMessageData(StatusMessageType.Error, "创建失败");
                return View(recommendItemTypeCreateModel);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "创建成功"));
        }


        /// <summary>
        /// 删除推荐类别
        /// </summary>
        /// <param name="recommendTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteRecommendType(string recommendTypeId = null)
        {
            bool result = recommendService.DeleteRecommendType(recommendTypeId);
            if (result)
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"));
            }
        }

        /// <summary>
        /// 验证推荐类别Id的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateTypeId(string typeId)
        {
            var recommendType = recommendService.GetRecommendType(typeId);
            return Json(recommendType == null, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region 推荐内容
        /// <summary>
        /// 管理推荐内容页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageRecommendItems(string tenantTypeId = null, string recommendTypeId = null, bool? isLink = null, int pageSize = 20, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("推荐内容管理");
            TenantTypeService tenantTypeService = new TenantTypeService();
            IEnumerable<TenantType> tenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Recommend()).Where(n => n.TenantTypeId != TenantTypeIds.Instance().User());
            ViewData["tenantList"] = new SelectList(tenantTypes, "TenantTypeId", "Name", tenantTypeId);
            IEnumerable<RecommendItemType> recommendItemTypes = recommendService.GetRecommendTypes(tenantTypeId).Where(n => n.TenantTypeId != TenantTypeIds.Instance().User());
            ViewData["typeList"] = new SelectList(recommendItemTypes, "TypeId", "Name", recommendTypeId);

            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Text = "是", Value = true.ToString() }, new SelectListItem { Text = "否", Value = false.ToString() } };
            ViewData["isLink"] = new SelectList(items, "Value", "Text", isLink);

            PagingDataSet<RecommendItem> pds = recommendService.GetsForAdmin(tenantTypeId, recommendTypeId, isLink, pageSize, pageIndex);
            return View(pds);

        }
        
        
        /// <summary>
        /// 删除推荐内容
        /// </summary>
        /// <param name="recommendId">实体Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteRecommendItem(long recommendId)
        {
            bool result = recommendService.Delete(recommendId);
            if (result)
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"));
            }
        }
        /// <summary>
        /// 改变推荐内容的排序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeRecommendOrder(long id, long referenceId)
        {
            recommendService.ChangeDisplayOrder(id, referenceId);
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }

        
        
        /// <summary>
        /// 管理推荐用户
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="recommendTypeId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageRecommendUsers(string tenantTypeId = null, string recommendTypeId = null, int pageSize = 20, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("推荐用户管理");
            TenantTypeService tenantTypeService = new TenantTypeService();
            IEnumerable<TenantType> tenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Recommend());
            ViewData["tenantList"] = new SelectList(tenantTypes, "TenantTypeId", "Name", tenantTypeId);

            IEnumerable<RecommendItemType> recommendItemTypes = recommendService.GetRecommendTypes(tenantTypeId).Where(n => n.TenantTypeId == TenantTypeIds.Instance().User());
            ViewData["typeList"] = new SelectList(recommendItemTypes, "TypeId", "Name", recommendTypeId);

            PagingDataSet<RecommendItem> pds = recommendService.GetsForAdmin(TenantTypeIds.Instance().User(), recommendTypeId, null, pageSize, pageIndex);
            return View(pds);

        }

        
        
        #endregion

        #endregion


        #region 第三方帐号绑定

        /// <summary>
        /// 管理第三方帐号类型
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageAccountTypes()
        {
            IEnumerable<AccountType> accountTypes = accountBindingService.GetAccountTypes();
            pageResourceManager.InsertTitlePart("第三方帐号绑定");
            return View(accountTypes);
        }

        /// <summary>
        /// 编辑第三方帐号类型
        /// </summary>
        /// <param name="accountTypeKey"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditAccountType(string accountTypeKey)
        {
            AccountType accountType = accountBindingService.GetAccountType(accountTypeKey);
            if (accountType == null)
                return HttpNotFound();
            ViewData["AccountTypeName"] = accountType.AccountTypeName();
            return View(accountType.AsEditModel());
        }

        /// <summary>
        /// 编辑第三方帐号类型
        /// </summary>
        /// <param name="accountTypeEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditAccountType(AccountTypeEditModel accountTypeEditModel)
        {

            AccountType accountType = accountTypeEditModel.AsAccountType();
            accountBindingService.UpdateAccountType(accountType);
            return Json(new StatusMessageData(StatusMessageType.Success, "更新成功"));
        }

        #endregion


        #region 操作日志
        /// <summary>
        /// 展示页面
        /// </summary>
        /// <param name="Source">来源</param>
        /// <param name="Operator">操作人（可以模糊搜索）</param>
        /// <param name="Keyword">操作说明(搜索操作对象)</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码</param> 
        /// <returns></returns>
        public ActionResult ManageOperationLogs(string Source, string Operator, string Keyword, DateTime? StartDateTime, DateTime? EndDateTime, int pageSize = 20, int pageIndex = 1)
        {
            //实例化检索器
            OperationLogQuery query = new OperationLogQuery();
            query.Source = Source;
            query.Operator = Operator;
            query.Keyword = Keyword;
            query.StartDateTime = StartDateTime;
            //如果结束时间不为空则加一天
            if (EndDateTime != null)
                query.EndDateTime = EndDateTime.Value.AddDays(1);
            //获取检索后实体数组
            PagingDataSet<OperationLogEntry> logs = logService.GetLogs(query, pageSize, pageIndex);
            return View(logs);
        }

        /// <summary>
        /// 删除日志模态窗
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ManageOperationLogs()
        {
            return View();
        }

        /// <summary>
        /// 异步按日期删除日志
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _ManageOperationLogs(DateTime? startDate, DateTime? endDate)
        {
            //如果结束时间不为空则加一天
            if (endDate != null)
                endDate = endDate.Value.AddDays(1);
            //按日期删除日志
            int result = logService.Clean(startDate, endDate);
            //返回删除结果
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        #endregion


        #region 用户举报

        /// <summary>
        /// 管理用户举报
        /// </summary>
        /// <param name="isDisposed">是否已处理</param>
        /// <param name="impeachReason">举报原因</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>View（）</returns>
        [HttpGet]
        public ActionResult ManageImpeachReports(ImpeachReason? impeachReason, bool isDisposed = false, int pageSize = 20, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("用户举报管理");
            ImpeachReportService reportService = new ImpeachReportService();
            PagingDataSet<ImpeachReportEntity> impeachReports = reportService.GetsForAdmin(isDisposed, impeachReason, pageSize, pageIndex);
            return View(impeachReports);
        }

        /// <summary>
        /// 批量删除举报
        /// </summary>
        /// <param name="reportIds">举报Id集合</param>
        /// <returns>Json</returns>
        [HttpPost]
        public ActionResult _DeleteReports(IEnumerable<long> reportIds)
        {
            if (reportIds == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到要删除的举报，操作失败！"));
            }
            foreach (var item in reportIds)
            {
                impeachReportService.Delete(item);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "删除操作成功！"));
        }

        /// <summary>
        /// 批量处理举报
        /// </summary>
        /// <param name="reportIds">举报Id集合</param>
        /// <returns>Json</returns>
        [HttpPost]
        public ActionResult _DisposeReports(IEnumerable<long> reportIds)
        {
            if (reportIds == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到要处理的举报，处理失败！"));
            }
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "找不到处理人，处理失败！"));
            }
            foreach (var item in reportIds)
            {
                impeachReportService.Dispose(item, currentUser.UserId);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "处理成功！"));
        }

        /// <summary>
        ///  显示举报标题和描述的详细信息
        /// </summary>
        /// <param name="text">内容</param>
        /// <returns>View()</returns>
        [HttpGet]
        public ActionResult _ImpeachReportDetail(string text)
        {
            ViewData["text"] = text;
            return View();
        }

        /// <summary>
        /// 显示匿名用户的录入信息
        /// </summary>
        /// <param name="reportId">举报Id</param>
        /// <returns>View（）</returns>
        public ActionResult _ReporterInfo(long reportId)
        {
            var report = impeachReportService.GetReport(reportId);
            if (report != null)
            {
                ViewData["anonymityReport"] = report;
            }
            return View();
        }

        #endregion


        #region 友情链接
        /// <summary>
        /// 友情链接后台管理列表
        /// </summary>
        /// <param name="categoryId">分类标识</param>
        /// <returns></returns>
        public ActionResult ManageLinks(long? categoryId)
        {
            IEnumerable<LinkEntity> links = linkService.GetsOfSiteForAdmin(categoryId);
            ViewData["categories"] = categoryService.GetRootCategories(TenantTypeIds.Instance().Link());
            return View(links);
        }

        /// <summary>
        /// 创建/更新友情链接
        /// </summary>
        /// <param name="linkId">链接标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditLink(long? linkId)
        {
            LinkEntity link = LinkEntity.New();
            LinkEditModel linkEditModel = new LinkEditModel();

            if (linkId.HasValue)
            {
                link = linkService.Get(linkId.Value);
                linkEditModel = link.AsLinkEditModel();
                linkEditModel.LinkId = linkId.Value;
            }

            IEnumerable<Category> categories = categoryService.GetOwnerCategories(0, TenantTypeIds.Instance().Link());
            ViewData["categories"] = new SelectList(categories, "CategoryId", "CategoryName", link.Categories.Count() > 0 ? link.Categories.ElementAt(0).CategoryId : (long)0);

            return View(linkEditModel);
        }

        /// <summary>
        /// 创建/更新友情链接
        /// </summary>
        /// <param name="linkEditModel">LinkEditModel</param>
        /// <param name="categoryId">链接分类标识</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditLink(LinkEditModel linkEditModel, IEnumerable<long> categoryId)
        {
            LinkEntity link = linkEditModel.AsLink();

            string successMsg = string.Empty;
            HttpPostedFileBase localImage = Request.Files["localImage"];

            //更新操作
            if (link.LinkId > 0)
            {
                //获取图片URL
                GetLinkImageUrl(link, localImage);

                //更新图片
                linkService.Update(link);

                //清除链接与类别的联系
                categoryService.ClearCategoriesFromItem(link.LinkId, 0, TenantTypeIds.Instance().Link());

                successMsg = "编辑成功！";
            }
            //创建操作
            else
            {
                linkService.Create(link);

                GetLinkImageUrl(link, localImage);

                linkService.Update(link);

                successMsg = "创建成功！";
            }

            categoryService.AddCategoriesToItem(categoryId, link.LinkId, 0);

            return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Success, successMsg)));
        }

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="link">链接实体</param>
        /// <param name="localImage">本地图片</param>
        private void GetLinkImageUrl(LinkEntity link, HttpPostedFileBase localImage)
        {
            link.LinkType = LinkType.ImageLink;
            //本地图片
            if (localImage != null && !string.IsNullOrEmpty(localImage.FileName))
            {
                link.ImageUrl = logoService.UploadLogo(link.LinkId, localImage.InputStream);
            }
            //文字链接
            else if (string.IsNullOrEmpty(link.ImageUrl))
            {
                link.LinkType = LinkType.TextLink;
            }
        }

        /// <summary>
        /// 改变友情链接显示顺序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        [HttpPost]
        public void _ChangeLinkDisplayOrder(long id, long referenceId)
        {
            LinkEntity link = linkService.Get(id);
            LinkEntity referenceLink = linkService.Get(referenceId);
            long temp = link.DisplayOrder;
            link.DisplayOrder = referenceLink.DisplayOrder;
            referenceLink.DisplayOrder = temp;
            linkService.Update(link);
            linkService.Update(referenceLink);
        }

        /// <summary>
        /// 单个删除友情链接
        /// </summary>
        /// <param name="linkId">链接标识</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _DeleteLink(long linkId)
        {
            LinkEntity link = linkService.Get(linkId);
            linkService.Delete(link);

            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        /// <summary>
        /// 批量删除友情链接
        /// </summary>
        /// <param name="linkIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _BatchDeleteLink(IEnumerable<long> linkIds)
        {
            foreach (long linkId in linkIds)
            {
                LinkEntity link = linkService.Get(linkId);
                linkService.Delete(link);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        #endregion


        #region 公告管理
        /// <summary>
        /// 公告管理
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="startExpiredDate">过期时间</param>
        /// <param name="endExpiredDate">过期时间</param>
        /// <param name="startModifiedDate">更新时间</param>
        /// <param name="endModifiedDate">更新时间</param>
        /// <param name="status">状态</param>
        /// <param name="displayArea">展示区域</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页显示多少条</param>
        /// <returns></returns>
        public ActionResult ManageAnnouncements(string keyword = null, DateTime? startExpiredDate = null, DateTime? endExpiredDate = null, DateTime? startModifiedDate = null, DateTime? endModifiedDate = null, Announcement_Status? status = null, Announcement_DisplayArea? displayArea = null, int pageIndex = 1, int pageSize = 20)
        {
            pageResourceManager.InsertTitlePart("公告管理");

            if (startExpiredDate == DateTime.MinValue)
                startExpiredDate = null;

            if (endExpiredDate == DateTime.MinValue)
                endExpiredDate = null;

            if (startExpiredDate.HasValue && endExpiredDate.HasValue && startExpiredDate.Value > endExpiredDate.Value)
            {
                DateTime? changDate = startExpiredDate;
                startExpiredDate = endExpiredDate;
                endExpiredDate = changDate;
            }

            if (startModifiedDate == DateTime.MinValue)
                startModifiedDate = null;

            if (endModifiedDate == DateTime.MinValue)
                endModifiedDate = null;

            if (startModifiedDate.HasValue && endModifiedDate.HasValue && startModifiedDate.Value > endModifiedDate.Value)
            {
                DateTime? changDate = startModifiedDate;
                startModifiedDate = endModifiedDate;
                endModifiedDate = changDate;
            }

            PagingDataSet<Announcement> announcements = announcementService.GetForAdmin(keyword, startExpiredDate, endExpiredDate, startModifiedDate, endModifiedDate, status, displayArea.ToString(), pageIndex, pageSize);

            return View(announcements);
        }


        /// <summary>
        /// 创建编辑公告
        /// </summary>
        /// <param name="id">公告Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditAnnouncement(long? id = null)
        {
            pageResourceManager.InsertTitlePart("创建公告");

            AnnouncementEditModel model = null;
            if (!id.HasValue)
            {
                model = new AnnouncementEditModel();

                var tempdata = TempData.Get<AnnouncementEditModel>("model", null);
                if (tempdata != null)
                    model = tempdata;
            }
            else
            {
                var announcement = announcementService.Get(id.Value);
                model = announcement.AsEditModel();
            }
            return View(model);
        }

        /// <summary>
        /// 创建编辑公告
        /// </summary>
        /// <param name="model">AnnouncementEditModel</param>
        /// <param name="type">类型（不启用、文字、链接）</param>
        /// <param name="home">在频道首页显示</param>
        /// <param name="userspace">在用户空间首页显示</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAnnouncement(AnnouncementEditModel model, string type, string home, string userspace)
        {
            if (home == null && userspace == null)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "请至少选择一个显示区域");
                TempData["model"] = model;
                return RedirectToAction("EditAnnouncement");
            }

            model.DisplayArea = string.Format("{0},{1}", home, userspace);
            if (type == "disenabled")
            {
                model.EnabledDescription = false;
                model.IsHyperLink = false;
            }
            if (type == "text")
            {
                model.EnabledDescription = true;
                model.IsHyperLink = false;
            }
            if (type == "hyperlink")
            {
                model.EnabledDescription = true;
                model.IsHyperLink = true;
            }

            if (model.ReleaseDate == DateTime.MinValue || model.ExpiredDate == DateTime.MinValue)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "请正确填写日期");
                TempData["model"] = model;
                return RedirectToAction("EditAnnouncement");
            }

            if (model.ReleaseDate > model.ExpiredDate)
            {
                DateTime changDate = model.ReleaseDate;
                model.ReleaseDate = model.ExpiredDate;
                model.ExpiredDate = changDate;
            }

            model.UserId = UserContext.CurrentUser.UserId;
            if (model.Id == 0 || model.Id == null)
            {
                announcementService.Create(model.AsAnnouncement());
            }
            else
            {
                announcementService.Update(model.AsAnnouncement());
            }

            return RedirectToAction("ManageAnnouncements");
        }

        /// <summary>
        /// 批量删除公告
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAnnouncements(IEnumerable<long> ids)
        {
            if (ids.Count() > 0)
            {
                foreach (var id in ids)
                {
                    announcementService.Delete(id);
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"));
            }
        }

        /// <summary>
        /// 批量过期公告
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeStatusToExpired(IEnumerable<long> ids)
        {
            if (ids.Count() > 0)
            {
                foreach (var id in ids)
                {
                    announcementService.ChangeStatusToExpired(id);
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "过期成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "过期失败！"));
            }

        }

        /// <summary>
        /// 交换公告排列顺序
        /// </summary>
        [HttpPost]
        public JsonResult ChangeAnnouncementDisplayOrder(long id, long referenceId)
        {
            announcementService.ChangeDisplayOrder(id, referenceId);

            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }

        #endregion


        #region 广告管理

        #region 广告

        /// <summary>
        /// 创建/编辑广告的get方法
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditAdvertising(long advertisingId = 0)
        {
            pageResourceManager.InsertTitlePart(advertisingId > 0 ? "编辑广告" : "新建广告");

            Advertising advertising = null;
            if (advertisingId > 0)
            {
                advertising = advertisingService.GetAdvertising(advertisingId);
            }

            var textStyle = advertising != null ? advertising.TextStyle : string.Empty;
            int fontSize = 0;
            if (!string.IsNullOrEmpty(textStyle) && textStyle.Contains("px"))
            {
                int index = textStyle.IndexOf("px");
                var sub_first = textStyle.Substring(0, index);
                var sub_second = sub_first.Split(':').Last().Trim();
                fontSize = int.Parse(sub_second);
            }
            ViewData["fontSize"] = fontSize;
            return View(advertising != null ? advertising.AsEditModel() : new AdvertisingEditModel());
        }

        /// <summary>
        /// 创建/编辑商品的post方法
        /// </summary>
        /// <param name="advertisingEditModel">广告EditModel</param>
        /// <param name="positionIds">广告位Id集合</param>
        /// <param name="stream">图片输入流</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAdvertising(AdvertisingEditModel advertisingEditModel)
        {
            Stream stream = null;
            HttpPostedFileBase uploadImageFileName = Request.Files["UploadImageFileName"];
            Advertising advertising = advertisingService.GetAdvertising(advertisingEditModel.AdvertisingId);
            if (uploadImageFileName != null && !string.IsNullOrEmpty(uploadImageFileName.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Advertising());
                if (!tenantLogoSettings.ValidateFileLength(uploadImageFileName.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return View(advertisingEditModel);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                if (!logoSettings.ValidateFileExtensions(uploadImageFileName.FileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return View(advertisingEditModel);
                }

                stream = uploadImageFileName.InputStream;
                advertisingEditModel.UploadImageFileName = uploadImageFileName.FileName;
            }
            else        //当取不到上传的图片文件名时UploadFileName值保持不变
            {
                advertisingEditModel.UploadImageFileName = advertising != null ? advertising.AttachmentUrl : string.Empty;
            }
            var positionIds = advertisingEditModel.PositionIds ?? new List<string>();
            if (advertisingEditModel.AdvertisingId <= 0)
            {

                if (advertisingService.CreateAdvertising(advertisingEditModel.AsAdvertising(), positionIds, stream))
                {
                    return Json(new StatusMessageData(StatusMessageType.Success, "创建广告成功！"));
                }
                return Json(new StatusMessageData(StatusMessageType.Error, "创建广告失败！"));
            }
            advertisingService.UpdateAdvertising(advertisingEditModel.AsAdvertising(), positionIds, stream);
            return Content(System.Web.Helpers.Json.Encode(new StatusMessageData(StatusMessageType.Success, "编辑广告成功！")));
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="advertisingIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteAdvertisings(IEnumerable<long> advertisingIds)
        {
            foreach (var advertisingId in advertisingIds)
            {
                advertisingService.DeleteAdvertising(advertisingId);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }

        /// <summary>
        /// 删除广告示意图
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteAdvertisingImage(long advertisingId)
        {
            advertisingService.DeleteAdvertisingImage(advertisingId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除示意图成功！"));
        }

        /// <summary>
        /// 设置广告是否启用
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <param name="isEnable">是否启用</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SetAdvertisingStatus(List<long> advertisingIds, bool isEnable)
        {
            foreach (var advertisingId in advertisingIds)
            {
                advertisingService.SetAdvertisingStatus(advertisingId, isEnable);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        /// <summary>
        /// 显示用于创建/编辑广告时需要的广告位列表
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <param name="size">广告位大小</param>
        /// <param name="presentAreaKey">投放区域</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _AdvertisingPositionList(long advertisingId = 0, string size = "", string presentAreaKey = "")
        {
            PositionSizeDropDownList("positionSize", size);
            PresentAreasDropDownList("presentAreaKey", presentAreaKey);

            var positionIds = advertisingService.GetPositionsByAdvertisingId(advertisingId).Select(n => n.PositionId);

            int hight = 0;
            int width = 0;
            if (!string.IsNullOrEmpty(size))
            {
                var param = size.Split('*');
                width = int.Parse(param[0]);
                hight = int.Parse(param[1]);
            }
            var positions = advertisingService.GetPositionsForAdmin(presentAreaKey, hight, width);

            ViewData["positionIds"] = positionIds;
            ViewData["advertisingId"] = advertisingId;

            return View(positions);
        }

        /// <summary>
        /// 设置广告字体大小的get方法
        /// </summary>
        /// <param name="fontSize">字体大小</param>
        /// <param name="orignalSize">默认字体大小</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _SetFontSize(int orignalSize, int fontSize = 0)
        {
            if (fontSize == 0)
            {
                fontSize = orignalSize;
            }
            ViewData["fontSize"] = fontSize;
            ViewData["orignalSize"] = orignalSize;
            return View();
        }

        /// <summary>
        /// 显示广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        public ActionResult _ShowAdvertisingDescriptions(long advertisingId)
        {
            Advertising advertising = advertisingService.GetAdvertising(advertisingId);
            return View(advertising.AdvertisingsPositions);
        }

        #endregion





        #endregion

        #region  客服消息

        /// <summary>
        /// 客服消息
        /// </summary>
        /// <param name="uname">发件人</param>
        /// <param name="roleName">角色</param>
        /// <param name="minRank">等级下限</param>
        /// <param name="maxRank">等级上限</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageCustomMessage(string uname, string roleName, long minRank = 0, long maxRank = 0, int pageIndex = 1, int pageSize = 20)
        {
            IEnumerable<Role> roles = roleService.GetRoles();
            ViewData["RoleName"] = new SelectList(roles, "RoleName", "FriendlyRoleName", roleName);
            PagingDataSet<MessageSession> customMessages = messageService.GetCustomerMessages(pageIndex, pageSize, uname, roleName, minRank, maxRank);
                       
            return View(customMessages);
        }

        /// <summary>
        /// 创建私信
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="toUserId">收件人用户Id</param>
        [HttpGet]
        public ActionResult _CustomCreateMessage(string spaceKey, long? toUserId)
        {
            MessageEditModel messageEditModel = new MessageEditModel();

            if (toUserId != null && toUserId.Value > 0)
            {
                User user = userService.GetFullUser(toUserId.Value);
                if (user != null)
                {
                    if (!new Authorizer().Message(user.UserId))
                        return Json(new StatusMessageData(StatusMessageType.Hint, "该用户不允许你发私信！"), JsonRequestBehavior.AllowGet);
                    ViewData["ToUserDisplayName"] = user.DisplayName;
                }
            }

            IMessageSettingsManager messageSettingsManager = DIContainer.Resolve<IMessageSettingsManager>();
            ViewData["maxReceiver"] = messageSettingsManager.Get().MaxReceiver;

            return View(messageEditModel);
        }

        /// <summary>
        /// 创建私信表单提交
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="model">用于表单提交的实体</param>
        [HttpPost]
        public ActionResult _CustomCreateMessage(string spaceKey, MessageEditModel model)
        {
            User currentSpaceUser = (User)UserContext.CurrentUser;
            if (currentSpaceUser == null)
                return HttpNotFound();
            IEnumerable<long> toUserIds = Request.Form.Gets<long>("ToUserIds");
            IEnumerable<User> toUsers = userService.GetFullUsers(toUserIds);
            Message message = null;
            foreach (var toUser in toUsers)
            {
                if (new Authorizer().Message(toUser.UserId))
                {
                    message = model.AsMessage();
                    message.MessageType = MessageType.Common;
                    message.Receiver = toUser.DisplayName;
                    message.ReceiverUserId = toUser.UserId;
                    message.SenderUserId = (long)BuildinMessageUserId.CustomerService;
                    message.Sender = "客服消息";
                    bool value = messageService.Create(message);
                }
            }
            if (message != null)
                return Json(new { messageId = message.MessageId });
            else
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Hint, "您没有权限对此用户发送私信");
            return Json(new { messageId = 0, hint = new StatusMessageData(StatusMessageType.Hint, "您没有权限对此用户发送私信") });
        }


        /// <summary>
        /// 群发私信
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MassMessages()
        {
            //角色下拉列表
            IEnumerable<Role> roles = roleService.GetRoles();
            if (roles != null)
            {
                ViewData["Roles"] = roles.Select(n => new MenuItem { Text = n.FriendlyRoleName, Value = n.RoleName.ToString() });
            }

            int maxUserRank = 1;
            var ranks = new UserRankService().GetAll();
            if (ranks.Count > 0)
                maxUserRank = ranks.Max(n => n.Value.Rank);
            ViewData["maxUserRank"] = maxUserRank;
            return View();
        }

        /// <summary>
        /// 群发私信
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MassMessages(bool isByRole, List<string> roleName, bool isMessage, string messageBody, string messageSubject, int minRank = 0, int maxRank = 0)
        {
            int maxUserRank = 1;
            var ranks = new UserRankService().GetAll();
            if (ranks.Count > 0)
                maxUserRank = ranks.Max(n => n.Value.Rank);
            if (minRank > maxRank)
            {
                int temp = 0;
                temp = maxRank;
                maxRank = minRank;
                minRank = temp;
            }

            
            IEnumerable<IUser> users = null;
            if (isByRole)
            {
                users = UserService.GetUsers(roleName, 0, 0);
            }
            else
            {
                users = UserService.GetUsers(new List<string>(), minRank, maxRank);
            }
            int count = 0;

            if (isMessage)
            {
                foreach (var user in users)
                {
                    Message message = Message.New();
                    message.MessageType = MessageType.CustomerService;
                    message.SenderUserId = (long)BuildinMessageUserId.CustomerService;
                    message.Sender = "客服消息";
                    if (new Authorizer().Message(user.UserId))
                    {
                        message.Receiver = user.DisplayName;
                        message.ReceiverUserId = user.UserId;
                        message.Body = messageBody;
                        bool value = messageService.Create(message);
                        count++;
                    }
                }
            }
            else
            {
                IEmailSettingsManager emailSettingsManager = DIContainer.Resolve<IEmailSettingsManager>();
                EmailSettings emailSettings = emailSettingsManager.Get();
                foreach (var user in users)
                {
                    if (user != null)
                    {
                        try
                        {
                            MailMessage mail = new MailMessage(emailSettings.AdminEmailAddress, user.AccountEmail);
                            mail.Subject = "客服消息";
                            mail.Body = messageBody;
                            mail.IsBodyHtml = false;
                            mail.BodyEncoding = Encoding.UTF8;
                            mail.From = new System.Net.Mail.MailAddress(emailSettings.AdminEmailAddress);
                            mail.IsBodyHtml = false;
                            mail.Sender = new System.Net.Mail.MailAddress(emailSettings.AdminEmailAddress);
                            mail.Subject = messageSubject;
                            mail.SubjectEncoding = Encoding.UTF8;

                            emailService.Enqueue(mail);

                            count++;
                        }
                        catch (Exception ex)
                        {
                            string a = ex.Message;
                        }

                    }
                }
            }

            if (count > 0)
                return Json(new StatusMessageData(StatusMessageType.Success, string.Format("成功发送了{0}条{1}", count, isMessage ? "消息" : "邮件")));
            return Json(new StatusMessageData(StatusMessageType.Error, string.Format("{0}发送失败了！", isMessage ? "消息" : "邮件")));
        }

        /// <summary>
        /// 批量删除私信对话
        /// </summary>
        /// <param name="CheckBoxGroup">CheckBox</param>
        /// <returns></returns>
        public ActionResult DeleteMessageSession(List<long> CheckBoxGroup)
        {
            if (CheckBoxGroup.Count > 0)
            {
                foreach (var id in CheckBoxGroup)
                {
                    messageService.DeleteSession(id);
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "删除失败！"));
            }

        }

        /// <summary>
        /// 异步删除私信对话
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="sessionId">对话ID</param>
        [HttpPost]
        public JsonResult DeleteMessageSessionAsyn(string spaceKey, long sessionId)
        {
            if (messageService.DeleteSession(sessionId))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }

            return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }
        /// <summary>
        /// 删除私信
        /// </summary>
        /// <param name="messageId">私信Id</param>
        /// <param name="sessionId">会话Id</param>
        [HttpPost]
        public JsonResult DeleteMessage(long messageId, long sessionId)
        {
            if (messageService.Delete(messageId, sessionId))
            {
                return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
            }

            return Json(new StatusMessageData(StatusMessageType.Error, "删除失败!"));
        }

        /// <summary>
        /// 私信局部页
        /// </summary>
        /// <param name="messageId">私信Id</param>
        /// <param name="sessionId">会话Id</param>
        /// <param name="isFrame">是否有边框</param>
        /// <returns></returns>
        public ActionResult _Message(long messageId, long sessionId)
        {
            Message message = messageService.Get(messageId);
            ViewData["sessionId"] = sessionId;
            return View(message);
        }

        /// <summary>
        /// 私信列表
        /// </summary>
        /// <param name="sessionId">私信会话Id</param>
        [HttpGet]
        public ActionResult _ListCustomMessages(long sessionId)
        {


            MessageSession messageSession = messageService.GetSession(sessionId);
            if (messageSession == null)
                messageSession = new MessageSession();

            IUser user = userService.GetUser(messageSession.OtherUserId);
            ViewData["displayName"] = user != null ? user.DisplayName : "";
            ViewData["session"] = messageSession;
            ViewData["messages"] = messageService.GetTops(sessionId, 10);
            messageService.SetIsRead(sessionId, messageSession.UserId);
            return View();
        }

        /// <summary>
        /// 私信局部列表
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="sessionId">私信会话Id</param>
        /// <param name="isShowAll">是否显示全部</param>
        /// <returns></returns>
        public ActionResult _ListMessages(long sessionId, long userId, bool isShowAll = false)
        {
            //获取私信会话实体
            MessageSession messageSession = messageService.GetSession(sessionId);
            IEnumerable<Message> iMessages = null;

            if (isShowAll)
            {
                List<Message> messages = messageService.GetTops(sessionId, messageSession.MessageCount).ToList();
                iMessages = messages.Skip<Message>(50);
            }
            else
            {
                iMessages = messageService.GetTops(sessionId, 50);
            }

            IUser user = userService.GetUser(userId);

            ViewData["session"] = messageSession;
            ViewData["userId"] = userId;
            ViewData["otherUser"] = userService.GetUser(messageSession.OtherUserId);
            return View(iMessages);
        }


        #endregion

        #region 其他
        /// <summary>
        /// 管理广告位
        /// </summary>
        /// <param name="presentAreaKey">投放区域</param>
        /// <param name="isEnable">是否启用</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManagePositions(string presentAreaKey, bool? isEnable = null)
        {
            pageResourceManager.InsertTitlePart("广告位管理");

            PresentAreasDropDownList("PresentAreaKey", presentAreaKey);
            BoolDropDownList("isEnable", isEnable);

            IEnumerable<AdvertisingPosition> advertisings = advertisingService.GetPositionsForAdmin(presentAreaKey, isEnable: isEnable);
            return View(advertisings);
        }

        /// <summary>
        /// 广告管理
        /// </summary>
        /// <param name="presentAreaKey">投放区域</param>
        /// <param name="positionId">广告位Id</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="isExpired">是否过期</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageAdvertisings(string presentAreaKey, string positionId, DateTime? startDate, DateTime? endDate, bool? isExpired, bool? isEnable, int pageSize = 20, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("广告管理");

            PresentAreasDropDownList("presentAreaKey", presentAreaKey);
            BoolDropDownList("isExpired", isExpired);
            BoolDropDownList("isEnable", isEnable);
            ViewData["positionId"] = new SelectList(advertisingService.GetPositionsForAdmin(presentAreaKey), "PositionId", "Description", positionId);
            if (startDate > endDate)
            {
                var temp = startDate;
                startDate = endDate;
                endDate = temp;
            }
            PagingDataSet<Advertising> advertisings = advertisingService.GetAdvertisingsForAdmin(presentAreaKey, positionId, startDate, endDate, isExpired, isEnable, pageSize, pageIndex);
            return View(advertisings);
        }

        /// <summary>
        /// 改变广告顺序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _ChangeAdvertisingOrder(long id, long referenceId)
        {
            advertisingService.ChangeDisplayOrder(id, referenceId);
            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"));
        }

        /// <summary>
        /// 投放区域下拉框
        /// </summary>
        /// <param name="name">viewData名称</param>
        /// <param name="presentAreaKey">当前投放区域</param>
        private void PresentAreasDropDownList(string name, string presentAreaKey)
        {
            List<SelectListItem> areas = new List<SelectListItem>();
            IEnumerable<PresentArea> presentAreas = new PresentAreaService().GetAll().Where(n => n.PresentAreaKey != "ControlPanel");
            foreach (var presentArea in presentAreas)
            {
                areas.Add(new SelectListItem { Text = ResourceAccessor.GetString(presentArea.PresentAreaKey), Value = presentArea.PresentAreaKey });
            }
            ViewData[name] = new SelectList(areas, "value", "text", presentAreaKey);
        }

        /// <summary>
        /// 广告位大小下拉框
        /// </summary>
        /// <param name="name">ViewData名称</param>
        /// <param name="size">当前选中的size</param>
        private void PositionSizeDropDownList(string name, string size)
        {
            List<SelectListItem> sizeItems = new List<SelectListItem>();
            IEnumerable<string> sizes = new AdvertisingService().GetAllPositionSize();
            foreach (var item in sizes)
            {
                sizeItems.Add(new SelectListItem { Text = item, Value = item });
            }
            ViewData[name] = new SelectList(sizeItems, "value", "text", size);
        }

        /// <summary>
        /// 布尔值下拉框
        /// </summary>
        /// <param name="name">viewData名称</param>
        /// <param name="isEnable">是否启用</param>
        public void BoolDropDownList(string name, bool? isEnable)
        {
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Text = "是", Value = true.ToString() }, new SelectListItem { Text = "否", Value = false.ToString() } };
            ViewData[name] = new SelectList(items, "Value", "Text", isEnable);
        }

        /// <summary>
        /// 创建/编辑广告位页面
        /// </summary>
        /// <param name="positionId">广告位ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditPosition(string positionId = null)
        {
            AdvertisingPositionEditModel positionEditModel = new AdvertisingPositionEditModel();
            if (!string.IsNullOrEmpty(positionId))
            {
                AdvertisingPosition position = advertisingService.GetPosition(positionId);
                positionEditModel = position.AsAdvertisingPositionEditModel();
            }
            else
            {
                positionEditModel.IsEnable = true;
            }
            PresentAreasDropDownList("PresentAreaKey", positionEditModel.PresentAreaKey);
            ViewData["positionId"] = positionId;
            return View(positionEditModel);
        }

        /// <summary>
        /// 创建/编辑广告位
        /// </summary>
        /// <param name="positionEditModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditPosition(AdvertisingPositionEditModel positionEditModel)
        {
            StatusMessageData message = null;

            AdvertisingPosition position = advertisingService.GetPosition(positionEditModel.PositionId);

            Stream stream = null;
            HttpPostedFileBase positionImage = Request.Files["positionImage"];
            if (positionImage != null && !string.IsNullOrEmpty(positionImage.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().AdvertisingPosition());
                if (!tenantLogoSettings.ValidateFileLength(positionImage.ContentLength))
                {
                    message = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return Json(message);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                if (!logoSettings.ValidateFileExtensions(positionImage.FileName))
                {
                    message = new StatusMessageData(StatusMessageType.Error, "不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return Json(message);
                }

                stream = positionImage.InputStream;
                positionEditModel.FeaturedImage = positionImage.FileName;
            }
            else if (position != null)
            {
                positionEditModel.FeaturedImage = position.FeaturedImage;
            }

            if (position == null)
            {
                advertisingService.CreatePosition(positionEditModel.AsAdvertisingPosition(), stream);
                message = new StatusMessageData(StatusMessageType.Success, "创建广告位成功！");
            }
            else
            {
                advertisingService.UpdatePosition(positionEditModel.AsAdvertisingPosition(), stream);
                message = new StatusMessageData(StatusMessageType.Success, "编辑广告位成功！");
            }
            return Content(System.Web.Helpers.Json.Encode(message));
        }

        /// <summary>
        /// 删除广告位
        /// </summary>
        /// <param name="positionIds">广告位Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _DeletePosition(IEnumerable<string> positionIds)
        {
            StatusMessageData message = null;
            foreach (var positionId in positionIds)
            {
                advertisingService.DeletePosition(positionId);
            }
            message = new StatusMessageData(StatusMessageType.Success, "删除广告位成功！");
            return Json(message);
        }

        /// <summary>
        /// 删除广告位示意图
        /// </summary>
        /// <param name="positionId">广告位Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _DeletePositionImage(string positionId)
        {
            advertisingService.DeletePositionImage(positionId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除示意图成功！"));
        }

        /// <summary>
        /// 验证广告位编码的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidatePositionId(string positionId)
        {
            bool result = false;
            AdvertisingPosition position = advertisingService.GetPosition(positionId);
            if (position != null)
            {
                return Json("此广告位编码已存在", JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 右侧管理栏
        /// </summary>
        /// <returns></returns>
        public ActionResult _AdvertisingRightMenu()
        {
            ViewData["positionCount"] = advertisingService.GetPositionCount();
            ViewData["advertisingCount"] = advertisingService.GetAdvertisingCount();
            return View();
        }
        #endregion

        #region 站点统计
        /// <summary>
        /// CNZZ统计信息页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CNZZStatistics()
        {
            pageResourceManager.InsertTitlePart("站点统计");
            return View();
        }

        /// <summary>
        /// 设置CNZZ启用状态
        /// </summary>
        /// <param name="enable">是否启用</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SetCNZZStatisticsStatus(bool enable = true)
        {
            CNZZStatisticsService.Enable = enable;
            if (enable)
            {
                CNZZStatisticsService.Initialize();
            }
            else
            {
                ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
                SiteSettings siteSettings = siteSettingsManager.Get();
                siteSettings.StatScript = "";
                siteSettingsManager.Save(siteSettings);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, enable ? "启用成功" : "禁用成功"));
        }

        #endregion
    }

    #region Operationenu

    /// <summary>
    /// OperationrMenu
    /// </summary>
    public enum OperationMenu
    {
        /// <summary>
        /// 积分记录管理
        /// </summary>
        ManagePointRecords = 0,

        /// <summary>
        /// 搜索热词管理
        /// </summary>
        ManageSearchedTerms = 1,

        /// <summary>
        /// 推荐类别管理
        /// </summary>
        ManageRecommendTypes = 2,

        /// <summary>
        /// 推荐内容管理
        /// </summary>
        ManageRecommendItems = 3,

        /// <summary>
        /// 推荐用户管理
        /// </summary>
        ManageRecommendUsers = 4,

        /// <summary>
        /// 表情包管理
        /// </summary>
        ManageEmotionCategories = 5,

        /// <summary>
        /// 第三方帐号管理
        /// </summary>
        ManageAccountTypes = 6,

        /// <summary>
        /// 敏感词管理
        /// </summary>
        ManageSensitiveWords = 7,

        /// <summary>
        /// 公告管理
        /// </summary>
        ManageAnnouncements = 8,

        /// <summary>
        /// 管理操作日志
        /// </summary>
        ManageOperationLogs,

        /// <summary>
        /// 用户举报管理
        /// </summary>
        ManageImpeachReports,

        /// <summary>
        /// 友情链接
        /// </summary>
        ManageLinks,

        /// <summary>
        /// 友情链接
        /// </summary>
        ManageCustomMessage,


    }

    #endregion OperationMenu

}
