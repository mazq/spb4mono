//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.UI;

namespace Spacebuilder.Blog.Controllers
{
    /// <summary>
    /// 后台日志管理控制器
    /// </summary>
    [ManageAuthorize(CheckCookie = false)]
    [TitleFilter(TitlePart = "内容管理", IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = true)]
    public class ControlPanelBlogController : Controller
    {
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private BlogService blogService = new BlogService();
        private TenantTypeService tenantTypeService = new TenantTypeService();
        private CategoryService categoryService = new CategoryService();

        /// <summary>
        /// 日志管理
        /// </summary>
        /// <param name="auditStatus">审批状态</param>
        /// <param name="categoryId">日志类别id</param>
        /// <param name="subjectKeywords">标题关键字</param>
        /// <param name="isEssential">是否加精</param>
        /// <param name="userId">作者id</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        public ActionResult ManageBlogs(AuditStatus? auditStatus = null, long? categoryId = null, string subjectKeywords = null, bool? isEssential = null, string userId = null, string tenantTypeId = null, int pageSize = 20, int pageIndex = 1)
        {
            long? blogUserId = null;

            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.Trim(',');
                if (!string.IsNullOrEmpty(userId))
                {
                    blogUserId = long.Parse(userId);
                }
            }

            //获取类别
            IEnumerable<Category> categorys = categoryService.GetOwnerCategories(0, TenantTypeIds.Instance().BlogThread());
            SelectList categoryList = new SelectList(categorys.Select(n => new { text = n.CategoryName, value = n.CategoryId }), "value", "text", categoryId);
            ViewData["categoryList"] = categoryList;

            //组装是否加精
            List<SelectListItem> selectListIsEssential = new List<SelectListItem> { new SelectListItem { Text = "是", Value = true.ToString() }, new SelectListItem { Text = "否", Value = false.ToString() } };
            ViewData["isEssential"] = new SelectList(selectListIsEssential, "Value", "Text", isEssential);
            ViewData["userId"] = blogUserId;

            //获取租户类型
            IEnumerable<TenantType> tenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Blog());
            SelectList tenants = null;
            tenants = new SelectList(tenantTypes.Select(n => new { text = n.Name, value = n.TenantTypeId }), "value", "text", tenantTypeId);
            ViewData["tenants"] = tenants;
            ViewData["tenantscount"] = tenantTypes.Count();

            PagingDataSet<BlogThread> blogs = blogService.GetsForAdmin(null, auditStatus, categoryId, isEssential, blogUserId, subjectKeywords, pageSize, pageIndex);

            pageResourceManager.InsertTitlePart("日志管理");

            return View(blogs);
        }

        /// <summary>
        /// 更新审核状态
        /// </summary>
        /// <param name="threadIds">日志id</param>
        /// <param name="isApprove">审核状态，true为通过审核，false为不通过审核</param>
        public JsonResult _UpdateAuditStatus(IEnumerable<long> threadIds, bool isApprove)
        {
            blogService.Approve(threadIds, isApprove);
            return Json(new StatusMessageData(StatusMessageType.Success, "更新审核状态成功！"));
        }

        /// <summary>
        /// 更新审核状态
        /// </summary>
        /// <param name="threadIds">日志id</param>
        /// <param name="isApprove">审核状态，true为通过审核，false为不通过审核</param>
        public JsonResult _UpdateAuditStatu(long threadId, bool isApprove)
        {
            List<long> threadIds = new List<long>() { threadId };
            blogService.Approve(threadIds, isApprove);
            return Json(new StatusMessageData(StatusMessageType.Success, "更新审核状态成功！"));
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="threadIds">日志id列表</param>
        [HttpPost]
        public JsonResult _Delete(IEnumerable<long> threadIds)
        {
            foreach (var blog in threadIds)
            {
                blogService.Delete(blog);
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "批量删除日志成功！"));
        }

        /// <summary>
        /// 批量设置/取消精华
        /// </summary>
        /// <param name="threadIds">日志id列表</param>
        /// <param name="isEssential">是否精华，true为加精，false为取消精华</param>
        [HttpPost]
        public ActionResult _SetEssential(List<long> threadIds, bool isEssential)
        {
            blogService.SetEssential(threadIds, isEssential);
            return Json(new StatusMessageData(StatusMessageType.Success, isEssential ? "设置精华成功" : "取消精华成功"));

        }


        /// <summary>
        /// 设置日志分类（页面）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _SetCategory(string threadIds)
        {
            IEnumerable<Category> categorys = categoryService.GetOwnerCategories(0, TenantTypeIds.Instance().BlogThread());
            SelectList categoryList = new SelectList(categorys.Select(n => new { text = n.CategoryName, value = n.CategoryId }), "value", "text", categorys.First().CategoryId);
            ViewData["categoryList"] = categoryList;
            return View();
        }

        /// <summary>
        /// 设置日志分类（表单提交）
        /// </summary>
        /// <param name="threadIds">日志id列表</param>
        /// <param name="categoryId">分类id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _SetCategory(string threadIds, long categoryId)
        {
            CategoryService categoryService = new CategoryService();
            IUser currentUser = UserContext.CurrentUser;


            if (!string.IsNullOrEmpty(threadIds))
            {
                var ids = threadIds.TrimEnd(',').Split(',');
                foreach (var id in ids)
                {
                    categoryService.ClearCategoriesFromItem(long.Parse(id), 0, TenantTypeIds.Instance().BlogThread());
                    categoryService.AddCategoriesToItem(new List<long>() { categoryId }, long.Parse(id), currentUser.UserId);
                }

                return Json(new StatusMessageData(StatusMessageType.Success, "设置分类成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请选择要设置的日志！"));
            }
        }

        /// <summary>
        /// 日志后台管理右侧统计
        /// </summary>
        [HttpGet]
        public ActionResult _ManageBlogRightMenu()
        {
            //Dictionary<MicroblogManageableCountType, int> counts = microblogService.GetManageableCounts();
            //return View(counts);
            return View();
        }
    }

}