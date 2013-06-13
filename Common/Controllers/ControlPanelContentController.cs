//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.Logging;
using Tunynet.UI;
using Tunynet.Utilities;
using System.Web;
using Tunynet.Common.Configuration;
using Tunynet.Mvc;
using Tunynet.Search;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 控制面板内容Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = false)]
    [TitleFilter(IsAppendSiteName = true, TitlePart = "内容管理")]
    public class ControlPanelContentController : Controller
    {
        private CategoryService categoryService = new CategoryService();
        private TenantTypeService tenantTypeService = new TenantTypeService();
        private CommentService commentService = new CommentService();
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();

        /// <summary>
        /// 控制面板工具首页
        /// </summary>
        [HttpGet]
        [ManageAuthorize(CheckApplication = false)]
        public ActionResult Home()
        {
            ApplicationService applicationService = new ApplicationService();
            //获得所有应用的应用名
            IEnumerable<ApplicationBase> applications = applicationService.GetAll();
            //初始化待处理应用数组
            List<ApplicationStatisticData> allManageableDatas = new List<ApplicationStatisticData>();
            //初始化数据统计项实体数组
            List<ApplicationStatisticItem> statisticItems = new List<ApplicationStatisticItem>();

            var authorizer = new Authorizer();
            var isApplicationAdmin = !UserContext.CurrentUser.IsInRoles(RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator());

            //遍历应用名
            foreach (var application in applications)
            {
                if (isApplicationAdmin)
                {
                    if (!authorizer.IsAdministrator(application.ApplicationId))
                        continue;
                }
                //获取所有待处理数据实体
                IEnumerable<ApplicationStatisticData> manageableDatas = DIContainer.ResolveNamed<IApplicationStatisticDataGetter>(application.ApplicationKey).GetManageableDatas();
                //获取所有数据统计应用数据实体
                IEnumerable<ApplicationStatisticData> applicationStatisticDatas = DIContainer.ResolveNamed<IApplicationStatisticDataGetter>(application.ApplicationKey).GetStatisticDatas();
                //将所有待处理数据实体添加到未处理应用数组
                allManageableDatas.AddRange(manageableDatas);
                //遍历所有应用简称
                foreach (string shortName in applicationStatisticDatas.Select(n => n.ShortName))
                {
                    //如果数组中已存在该应用则继续
                    var item = statisticItems.Where(n => n.ShortName == shortName).FirstOrDefault();
                    if (statisticItems.Contains(item))
                    {
                        continue;
                    }
                    //获取该简称下的应用数组
                    IEnumerable<ApplicationStatisticData> datas = applicationStatisticDatas.Where(n => n.ShortName == shortName);
                    //获取该简称下的总数应用
                    ApplicationStatisticData applicationStatisticDataTotal = datas.Where(n => n.DataKey == ApplicationStatisticDataKeys.Instance().TotalCount()).FirstOrDefault();
                    //初始化总数
                    long totalCount = 0;
                    //初始化URL
                    string itemUrl = null;
                    //如果存在总数应用
                    if (applicationStatisticDataTotal != null)
                    {
                        //为数据统计项实体总数和url赋值
                        totalCount = applicationStatisticDataTotal.Value;
                        itemUrl = applicationStatisticDataTotal.Url;
                    }
                    //获取该简称下的24小时新增数应用
                    var applicationStatisticDataLast24H = datas.Where(n => n.DataKey == ApplicationStatisticDataKeys.Instance().Last24HCount()).FirstOrDefault();
                    //初始化24小时新增数
                    long Last24H = 0;
                    //如果存在24小时新增数应用
                    if (applicationStatisticDataLast24H != null)
                        //为总数项实体24小时新增数赋值
                        Last24H = applicationStatisticDataLast24H.Value;
                    //实例化数据统计项实体
                    ApplicationStatisticItem appItem = new ApplicationStatisticItem(shortName, totalCount, Last24H);
                    appItem.Url = itemUrl;
                    //添加到数组
                    statisticItems.Add(appItem);
                }
            }
            //获取待处理事项实体数组
            ViewData["allManageableDatas"] = allManageableDatas;

            return View(statisticItems);

        }



        #region 标签管理

        /// <summary>
        /// 标签管理页
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult ManageTags(string keyword, string tenantTypeId, bool? isFeatured, AuditStatus? auditStatus = null, int pageIndex = 1, int pageSize = 20)
        {
            pageResourceManager.InsertTitlePart("标签管理");

            TagService tagService = new TagService(tenantTypeId);

            PubliclyAuditStatus? publiclyAuditStatus = null;
            if (auditStatus != null)
            {
                publiclyAuditStatus = (PubliclyAuditStatus?)auditStatus;
            }
            TagQuery tagQuery = new TagQuery();
            tagQuery.PubliclyAuditStatus = publiclyAuditStatus;
            tagQuery.Keyword = keyword;
            tagQuery.TenantTypeId = tenantTypeId;
            if (isFeatured.HasValue)
            {
                tagQuery.IsFeatured = isFeatured.Value;
            }
            PagingDataSet<Tag> tags = tagService.GetTags(tagQuery, pageIndex, pageSize);

            //所属下拉框
            List<TenantType> tenantTypesList = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Tag()).ToList<TenantType>();
            tenantTypesList.Insert(0, new TenantType { ApplicationId = 0, Name = "不限", TenantTypeId = "" });

            SelectList tenants = new SelectList(tenantTypesList.Select(n => new { text = n.Name, value = n.TenantTypeId }), "value", "text", tenantTypeId);
            ViewData["tenants"] = tenants;

            //所属名称
            Dictionary<string, string> tenantsDictionary = tenantTypesList.ToDictionary(n => n.TenantTypeId, n => n.Name);
            ViewData["tenantsDictionary"] = tenantsDictionary;

            return View(tags);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult AuditTags(string tagIds, bool isApproved)
        {
            TagService tagService = new TagService(string.Empty);
            if (!string.IsNullOrEmpty(tagIds))
            {
                IEnumerable<long> updateTagIds = tagIds.TrimEnd(',').Split(',').Select(t => Convert.ToInt64(t));
                tagService.UpdateAuditStatus(updateTagIds, isApproved);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult DeleteTags(string tagIds)
        {
            TagService tagService = new TagService(string.Empty);
            if (!string.IsNullOrEmpty(tagIds))
            {
                List<long> deleteTagIds = new List<long>();

                foreach (var tagId in tagIds.TrimEnd(',').Split(','))
                {
                    tagService.Delete(long.Parse(tagId));
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据租户ID获取分组
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult GetTagGroupsByTenantTypeId(string tenantTypeId)
        {
            TagService tagService = new TagService(string.Empty);
            IEnumerable<TagGroup> tagGroups = tagService.GetGroups(tenantTypeId);
            if (tagGroups.Count() == 0)
            {
                return Json(new { text = "无分组", value = "" }, JsonRequestBehavior.AllowGet);
            }
            TagGroup tagGroupEntity = new TagGroup();
            tagGroupEntity.GroupName = "无分组";
            tagGroupEntity.GroupId = 0;
            List<TagGroup> tagGroupsList = tagGroups.ToList<TagGroup>();
            tagGroupsList.Insert(0, tagGroupEntity);
            return Json(tagGroupsList.Select(tagGroup => new { text = tagGroup.GroupName, value = tagGroup.GroupId }), JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 添加/编辑标签页
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpGet]
        public ActionResult EditTag(long tagId = 0)
        {

            TagService tagService = new TagService(string.Empty);
            TagEditModel tagEditModel = null;

            //创建
            if (tagId == 0)
            {
                tagEditModel = new TagEditModel();
                ViewData["seletedTagNames"] = new List<string>();

                //所属租户类型下拉框
                SelectList tenants = GetTenantSelectList(MultiTenantServiceKeys.Instance().Tag(), null);
                ViewData["tenants"] = tenants;

                //标签分组下拉框
                SelectList tagGroups = GetTagGroupSelectList(0, tenants.First().Value);
                ViewData["tagGroups"] = tagGroups;

                ViewData["editTagTitle"] = "创建标签";

                pageResourceManager.InsertTitlePart("添加标签");

            }//编辑
            else
            {
                Tag tag = tagService.Get(tagId);
                tagEditModel = tag.AsTagEditModel();

                //所属租户类型下拉框
                SelectList tenants = GetTenantSelectList(MultiTenantServiceKeys.Instance().Tag(), tagEditModel.TenantTypeId);
                ViewData["tenants"] = tenants;

                //标签分组下拉框
                SelectList tagGroups = GetTagGroupSelectList(tagEditModel.GroupId, tagEditModel.TenantTypeId);
                ViewData["tagGroups"] = tagGroups;

                //取相关标签
                IEnumerable<string> seletedTagNames = tagService.GetRelatedTags(tagId).Select(n => n.TagName);
                ViewData["seletedTagNames"] = seletedTagNames;

                ViewData["editTagTitle"] = "编辑标签";

                pageResourceManager.InsertTitlePart("编辑标签");
            }

            return View(tagEditModel);
        }

        /// <summary>
        /// 添加/编辑标签
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpPost]
        public ActionResult EditTag(TagEditModel tagEditModel)
        {

            System.IO.Stream stream = null;

            //是否创建
            bool isCreate = tagEditModel.TagId == 0;

            if (isCreate)
            {
                ViewData["editTagTitle"] = "创建标签";
            }
            else
            {
                ViewData["editTagTitle"] = "编辑标签";
            }

            TagService tagService = new TagService(tagEditModel.TenantTypeId);

            //是特色标签
            if (tagEditModel.IsFeatured)
            {

                HttpPostedFileBase tagLogo = Request.Files["tagLogo"];
                string fileName = tagLogo == null ? "" : tagLogo.FileName;
                if (string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(tagEditModel.FeaturedImage))
                {

                    //所属租户类型下拉框
                    SelectList tenants = GetTenantSelectList(MultiTenantServiceKeys.Instance().Tag(), tagEditModel.TenantTypeId);
                    ViewData["tenants"] = tenants;

                    //标签分组下拉框
                    SelectList tagGroups = GetTagGroupSelectList(tagEditModel.GroupId, tagEditModel.TenantTypeId);
                    ViewData["tagGroups"] = tagGroups;

                    //取到用户设置的相关标签
                    ViewData["seletedTagNames"] = tagEditModel.RelatedTags[1].Split(',').ToList<string>();

                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "图片不能为空");

                    return View(tagEditModel);
                }
                else if (!string.IsNullOrEmpty(fileName))
                {
                    //校验附件的扩展名
                    ILogoSettingsManager logoSettingsManager = DIContainer.Resolve<ILogoSettingsManager>();
                    if (!logoSettingsManager.Get().ValidateFileExtensions(fileName))
                    {
                        //所属租户类型下拉框
                        SelectList tenants = GetTenantSelectList(MultiTenantServiceKeys.Instance().Tag(), tagEditModel.TenantTypeId);
                        ViewData["tenants"] = tenants;

                        //标签分组下拉框
                        SelectList tagGroups = GetTagGroupSelectList(tagEditModel.GroupId, tagEditModel.TenantTypeId);
                        ViewData["tagGroups"] = tagGroups;

                        //取到用户设置的相关标签
                        ViewData["seletedTagNames"] = tagEditModel.RelatedTags[1].Split(',').ToList<string>();

                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "只允许上传后缀名为 .gif .jpg .jpeg .png 的文件");

                        return View(tagEditModel);
                    }

                    //校验附件的大小
                    TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Tag());
                    if (!tenantLogoSettings.ValidateFileLength(tagLogo.ContentLength))
                    {
                        //所属租户类型下拉框
                        SelectList tenants = GetTenantSelectList(MultiTenantServiceKeys.Instance().Tag(), tagEditModel.TenantTypeId);
                        ViewData["tenants"] = tenants;

                        //标签分组下拉框
                        SelectList tagGroups = GetTagGroupSelectList(tagEditModel.GroupId, tagEditModel.TenantTypeId);
                        ViewData["tagGroups"] = tagGroups;

                        //取到用户设置的相关标签
                        ViewData["seletedTagNames"] = tagEditModel.RelatedTags[1].Split(',').ToList<string>();

                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}KB", tenantLogoSettings.MaxLogoLength));

                        return View(tagEditModel);
                    }

                    stream = tagLogo.InputStream;
                    tagEditModel.FeaturedImage = fileName;

                }
            }

            //获取相关标签
            string relatedTags = tagEditModel.RelatedTags[1];

            //创建
            if (isCreate)
            {
                Tag tag = tagEditModel.AsTag();
                tagService.Create(tag, stream);

                //添加到分组
                if (tagEditModel.GroupId > 0)
                {
                    tagService.BatchAddGroupsToTag(new List<long>() { tagEditModel.GroupId }, tagEditModel.TagName);
                }

                //创建相关标签
                if (!string.IsNullOrEmpty(relatedTags))
                {
                    tagService.AddRelatedTagsToTag(relatedTags, 0, tag.TagId);
                }
            }//更新
            else
            {
                Tag tag = tagEditModel.AsTag();
                tagService.Update(tag, stream);

                //添加到分组
                if (tagEditModel.GroupId > 0)
                {
                    tagService.BatchAddGroupsToTag(new List<long>() { tagEditModel.GroupId }, tagEditModel.TagName);
                }

                //创建相关标签
                if (!string.IsNullOrEmpty(relatedTags))
                {
                    tagService.ClearRelatedTagsFromTag(tagEditModel.TagId);
                    tagService.AddRelatedTagsToTag(relatedTags, 0, tagEditModel.TagId);
                }
            }

            return RedirectToAction("ManageTags");
        }

        /// <summary>
        /// 标签分组管理
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult ManageTagGroups(string tenantTypeId = "")
        {
            pageResourceManager.InsertTitlePart("标签分组管理");

            TagService tagService = new TagService(tenantTypeId);
            IEnumerable<TagGroup> tagGroups = tagService.GetGroups(tenantTypeId);

            //所属下拉框
            SelectList tenants = GetTenantSelectList(MultiTenantServiceKeys.Instance().Tag(), tenantTypeId);
            ViewData["tenants"] = tenants;

            //所属名称
            Dictionary<string, string> tenantsDictionary = tenants.ToDictionary(n => n.Value, n => n.Text);
            ViewData["tenantsDictionary"] = tenantsDictionary;

            return View(tagGroups);
        }

        /// <summary>
        /// 添加/编辑标签分组页
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpGet]
        public ActionResult _EditTagGroup(long tagGroupId = 0, string tenantTypeId = "")
        {
            TagGroupEditModel tagGroupEditModel = null;

            //添加
            if (tagGroupId == 0)
            {
                tagGroupEditModel = new TagGroupEditModel();
                tagGroupEditModel.TenantTypeId = tenantTypeId;
            }//编辑
            else
            {
                TagService tagService = new TagService(string.Empty);
                tagGroupEditModel = tagService.GetGroup(tagGroupId).AsTagGroupEditModel();
            }

            //所属下拉框
            SelectList tenants = GetTenantSelectList(MultiTenantServiceKeys.Instance().Tag(), tagGroupEditModel.TenantTypeId);
            ViewData["tenants"] = tenants;

            return View(tagGroupEditModel);
        }

        /// <summary>
        /// 添加/编辑标签分组
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpPost]
        public ActionResult _EditTagGroup(TagGroupEditModel tagGroupEditModel)
        {
            TagService tagService = new TagService(tagGroupEditModel.TenantTypeId);
            IEnumerable<TagGroup> tagGroups = tagService.GetGroups(tagGroupEditModel.TenantTypeId);
            if (tagGroups.Select(n => n.GroupName).Contains(tagGroupEditModel.GroupName))
            {
                return Json("same", JsonRequestBehavior.AllowGet);
            }
            //添加
            if (tagGroupEditModel.GroupId == 0)
            {
                tagService.CreateGroup(tagGroupEditModel.AsTagGroup());
            }//编辑
            else
            {
                tagService.UpdateGroup(tagGroupEditModel.AsTagGroup());
            }


            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量删除标签分组
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult DeleteTagGroups(string tagGroupIds)
        {
            TagService tagService = new TagService(string.Empty);
            if (!string.IsNullOrEmpty(tagGroupIds))
            {
                List<long> deleteTagIds = new List<long>();

                foreach (var tagGroupId in tagGroupIds.TrimEnd(',').Split(','))
                {
                    tagService.DeleteGroup(tagService.GetGroup((long.Parse(tagGroupId))));
                }
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 批量设置标签类别
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpGet]
        public ActionResult _SetTagsGroup(string tenantTypeId)
        {
            ViewData["tenantTypeId"] = tenantTypeId;

            List<SelectListItem> groupItems = new List<SelectListItem>();
            IEnumerable<TagGroup> groups = new TagService(tenantTypeId).GetGroups(tenantTypeId);
            foreach (var item in groups)
            {
                groupItems.Add(new SelectListItem { Text = item.GroupName, Value = item.GroupId.ToString() });
            }
            ViewData["groupSelectList"] = new SelectList(groupItems, "value", "text", groupItems);
            return View();
        }

        /// <summary>
        /// 批量设置标签分组的Post方法
        /// </summary>
        /// <param name="tagNames">标签name集合</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="groupId">分组Id</param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpPost]
        public ActionResult _SetTagsGroup(IEnumerable<string> tagNames, string tenantTypeId, long groupId)
        {
            var tagService = new TagService(tenantTypeId);
            foreach (var tagName in tagNames)
            {
                tagService.BatchAddGroupsToTag(new List<long> { groupId }, tagName);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功！"));
        }

        #endregion

        #region 类别管理

        #region 站点类别管理

        /// <summary>
        /// 管理站点类别
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult ManageSiteCategories(string tenantTypeId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return HttpNotFound();
            var authorizer = new Authorizer();

            pageResourceManager.InsertTitlePart("站点类别管理");

            List<TenantType> managedTenantTypes = new List<TenantType>();
            IEnumerable<TenantType> allTenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().SiteCategory());

            foreach (var tenantType in allTenantTypes)
            {
                if (authorizer.IsAdministrator(tenantType.ApplicationId))
                {
                    managedTenantTypes.Add(tenantType);
                }
            }
            SelectList tenantList = new SelectList(managedTenantTypes.Select(n => new { text = n.Name, value = n.TenantTypeId }), "value", "text", tenantTypeId);

            if (string.IsNullOrEmpty(tenantTypeId))
            {
                tenantTypeId = tenantList.First().Value;
            }
            IEnumerable<Category> categorys = categoryService.GetOwnerCategories(0, tenantTypeId);

            ViewData["tenantList"] = tenantList;
            ViewData["tenantTypeId"] = tenantTypeId;
            return View(categorys);
        }

        /// <summary>
        /// 删除站点分类
        /// </summary>
        /// <param name="categoryId">分类ID</param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpPost]
        public JsonResult DeleteSiteCategory(long categoryId)
        {
            //删除关联项
            categoryService.ClearItemsFromCategory(categoryId);

            //删除分类
            var result = categoryService.Delete(categoryId);

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
        /// 添加编辑类别页
        /// </summary>
        /// <param name="tenantTypeId">租户ID</param>
        /// <param name="categoryId">分类ID</param>
        /// <param name="parentId">父类ID</param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpGet]
        public ActionResult _EditSiteCategory(string tenantTypeId = null, long categoryId = 0, long parentId = 0)
        {
            CategoryEditModel categoryEditModel = new CategoryEditModel();

            //编辑
            if (categoryId != 0)
            {
                Category category = categoryService.Get(categoryId);
                categoryEditModel = category.AsCategoryEditModel();
            }
            else
            {
                if (!string.IsNullOrEmpty(tenantTypeId))
                {
                    categoryEditModel.TenantTypeId = tenantTypeId;
                }
                if (parentId != 0)
                {
                    Category category = categoryService.Get(parentId);
                    categoryEditModel.Depth = category.Depth + 1;
                    categoryEditModel.ParentId = parentId;
                    categoryEditModel.ParentName = category.CategoryName;
                }

            }
            return View(categoryEditModel);

        }

        /// <summary>
        /// 编辑添加分类
        /// </summary>
        /// <param name="category">分类实体</param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpPost]
        public JsonResult _EditSiteCategory(CategoryEditModel category)
        {
            //添加
            if (category.CategoryId == 0)
            {
                Category _category = category.AsCategory();
                _category.AuditStatus = AuditStatus.Success;

                var result = categoryService.Create(_category);
                if (result)
                {
                    return Json(new StatusMessageData(StatusMessageType.Success, "添加成功！"));
                }
                else
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "添加失败！"));
                }
            }
            //编辑
            else
            {
                categoryService.Update(category.AsCategory());

                return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
            }
        }

        /// <summary>
        /// 合并移动站点分类页
        /// </summary>
        /// <param name="fromCategoryId"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpGet]
        public ActionResult _MoveSiteCategory(long fromCategoryId = 0, string option = "move")
        {
            CategoryEditModel categoryEditModel = new CategoryEditModel();
            int maxDepth = 0;
            if (fromCategoryId != 0)
            {
                Category category = categoryService.Get(fromCategoryId);
                categoryEditModel = category.AsCategoryEditModel();
                maxDepth = category.MaxDepth;
            }
            ViewData["option"] = option;
            ViewData["maxDepth"] = maxDepth;
            return View(categoryEditModel);
        }

        /// <summary>
        /// 合并移动站点分类
        /// </summary>
        /// <param name="fromCategoryId"></param>
        /// <param name="CategoryId"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpPost]
        public JsonResult _MoveSiteCategory(long fromCategoryId = 0, long CategoryId = 0, string option = "move")
        {
            string errormsg = "请选择要移动或合并的项";
            int level = Request.Form.Get<int>("level", int.MaxValue);
            Category toCategory = categoryService.Get(CategoryId);
            Category fromCategory = categoryService.Get(fromCategoryId);
            if (fromCategoryId != 0 && CategoryId != 0)
            {
                if (option == "merge")
                {
                    try
                    {
                        categoryService.Merge(fromCategoryId, CategoryId);
                        return Json(new StatusMessageData(StatusMessageType.Success, "合并成功"));
                    }
                    catch (Exception e)
                    {
                        errormsg = e.Message;
                    }
                }
                if (option == "move")
                {
                    try
                    {
                        if (fromCategory.MaxDepth + toCategory.Depth < level)
                        {
                            categoryService.Move(fromCategoryId, CategoryId);
                            return Json(new StatusMessageData(StatusMessageType.Success, "移动成功"));
                        }
                        else
                        {
                            return Json(new StatusMessageData(StatusMessageType.Error, "移动失败，该类别只支持" + level + "层"));
                        }
                    }
                    catch (Exception e)
                    {
                        errormsg = e.Message;
                    }
                }
            }
            return Json(new StatusMessageData(StatusMessageType.Error, errormsg));
        }

        /// <summary>
        /// 更改站点分类显示顺序
        /// </summary>
        /// <returns></returns>
        [ManageAuthorize]
        public JsonResult ChangeSiteCategoryOrder(int fromCategoryId, int toCategoryId)
        {
            Category fromCategory = categoryService.Get(fromCategoryId);
            Category toCategory = categoryService.Get(toCategoryId);

            long temp = fromCategory.DisplayOrder;

            fromCategory.DisplayOrder = toCategory.DisplayOrder;
            categoryService.Update(fromCategory);

            toCategory.DisplayOrder = temp;
            categoryService.Update(toCategory);

            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 用户类别管理

        /// <summary>
        /// 管理用户类别
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="auditStatus"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpGet]
        public ActionResult ManageUserCategories(string keyword, string tenantTypeId, AuditStatus? auditStatus = null, string ownerId = null, int pageIndex = 1, int pageSize = 20)
        {
            pageResourceManager.InsertTitlePart("用户类别管理");
            //所属下拉框
            SelectList tenants = GetTenantSelectList(MultiTenantServiceKeys.Instance().UserCategory(), tenantTypeId);
            ViewData["tenants"] = tenants;

            long userId = 0;
            if (!string.IsNullOrEmpty(ownerId))
            {
                ownerId = ownerId.Trim(',');
                if (!string.IsNullOrEmpty(ownerId))
                {
                    userId = long.Parse(ownerId);
                }
            }

            PubliclyAuditStatus? publiclyAuditStatus = null;
            if (auditStatus != null)
            {
                publiclyAuditStatus = (PubliclyAuditStatus?)auditStatus;
            }

            CategoryService categoryService = new CategoryService();
            
            PagingDataSet<Category> categorys = categoryService.GetOwnerCategories(publiclyAuditStatus, tenantTypeId, keyword, userId, pageSize, pageIndex);
            ViewData["userId"] = userId;
            return View(categorys);
        }

        /// <summary>
        /// 编辑用户类别页
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpGet]
        public ActionResult _EditUserCategory(long categoryId = 0)
        {
            //编辑
            Category category = categoryService.Get(categoryId);
            CategoryEditModel categoryEditModel = category.AsCategoryEditModel();

            return View(categoryEditModel);
        }

        /// <summary>
        /// 编辑用户分类
        /// </summary>
        /// <param name="category">分类实体</param>
        /// <returns></returns>
        [ManageAuthorize]
        [HttpPost]
        public JsonResult _EditUserCategory(CategoryEditModel category)
        {
            categoryService.Update(category.AsCategory());

            return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
        }

        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="auditStatus"></param>
        /// <returns></returns>
        [ManageAuthorize]
        public JsonResult UpdateAuditStatus(string categoryIds, AuditStatus auditStatus)
        {
            if (!string.IsNullOrEmpty(categoryIds))
            {
                IEnumerable<long> updatecategoryIds = categoryIds.TrimEnd(',').Split(',').Select(t => Convert.ToInt64(t));
                categoryService.UpdateAuditStatus(updatecategoryIds, auditStatus);
                return Json(new StatusMessageData(StatusMessageType.Success, "更新审核状态成功！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请选择要更新的项！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 批量删除用户类别
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        [ManageAuthorize]
        public JsonResult DeleteUserCategorys(string categoryIds)
        {
            if (!string.IsNullOrEmpty(categoryIds))
            {
                var ids = categoryIds.TrimEnd(',').Split(',');
                foreach (var categoryId in ids)
                {
                    categoryService.ClearItemsFromCategory(long.Parse(categoryId));
                    categoryService.Delete(long.Parse(categoryId));
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "批量删除类别成功！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请选择要删除的项！"), JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #endregion

        #region 评论管理

        /// <summary>
        /// 管理评论
        /// </summary>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="tenantTypeId">租户类型ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        [ManageAuthorize]
        public ActionResult ManageComments(AuditStatus? auditStatus = null, string tenantTypeId = null, string userId = null, DateTime? startDate = null, DateTime? endDate = null, int pageSize = 20, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("评论管理");
            long? userId2 = null;

            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.Trim(',');
                if (!string.IsNullOrEmpty(userId))
                {
                    userId2 = long.Parse(userId);
                }
            }

            if (endDate.HasValue)
            {
                endDate = endDate.Value.AddDays(1);
            }
            
            ViewData["tenants"] = GetTenantSelectList(MultiTenantServiceKeys.Instance().Comment(), tenantTypeId);
            ViewData["userId"] = userId2;
            PagingDataSet<Comment> comments = commentService.GetComments((PubliclyAuditStatus?)auditStatus, tenantTypeId, userId2, startDate, endDate, pageSize, pageIndex);

            return View(comments);
        }

        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="commentIds">评论ID集合</param>
        /// <param name="auditStatus">审核状态</param>
        /// <returns></returns>
        [ManageAuthorize]
        public JsonResult _UpdateCommentAuditStatus(IEnumerable<long> commentIds, AuditStatus auditStatus)
        {
            
            commentService.BatchUpdateAuditStatus(commentIds, auditStatus == AuditStatus.Success);
            return Json(new StatusMessageData(StatusMessageType.Success, "更新评论审核状态成功！"));
        }

        /// <summary>
        /// 批量删除评论
        /// </summary>
        /// <param name="commentIds">评论ID集合</param>
        /// <returns></returns>
        [ManageAuthorize]
        public JsonResult _DeleteComments(IEnumerable<long> commentIds)
        {
            commentService.Delete(commentIds);
            return Json(new StatusMessageData(StatusMessageType.Success, "批量删除评论成功！"));
        }

        #endregion

        /// <summary>
        /// 获取租户类型
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <returns></returns>
        private SelectList GetTenantSelectList(string serviceKey, string tenantTypeId)
        {
            IEnumerable<TenantType> tenantTypes = tenantTypeService.Gets(serviceKey);
            SelectList tenants = null;
            if (string.IsNullOrEmpty(tenantTypeId))
            {
                tenants = new SelectList(tenantTypes.Select(n => new { text = n.Name, value = n.TenantTypeId }), "value", "text");
            }
            else
            {
                tenants = new SelectList(tenantTypes.Select(n => new { text = n.Name, value = n.TenantTypeId }), "value", "text", tenantTypeId);
            }
            return tenants;
        }

        /// <summary>
        /// 维护所属下拉框及分组下拉框的状态
        /// </summary>
        private SelectList GetTagGroupSelectList(long groupId, string tenantTypeId)
        {
            TagService tagService = new TagService(string.Empty);

            //分组下拉框
            IEnumerable<TagGroup> tagGroups = tagService.GetGroups(tenantTypeId);
            SelectList groupList = null;

            //如果该租户下没有分组
            if (tagGroups.Count() == 0)
            {
                groupList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "无分组", Value = "0" } }, "value", "text");
            }
            else
            {
                TagGroup tagGroupEntity = new TagGroup();
                tagGroupEntity.GroupName = "无分组";
                tagGroupEntity.GroupId = 0;
                List<TagGroup> tagGroupsList = tagGroups.ToList<TagGroup>();
                tagGroupsList.Insert(0, tagGroupEntity);
                groupList = new SelectList(tagGroupsList.Select(n => new { text = n.GroupName, value = n.GroupId }), "value", "text", groupId);
            }
            return groupList;
        }
    }
}
