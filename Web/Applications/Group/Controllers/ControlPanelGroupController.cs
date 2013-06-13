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
using Tunynet.Common;
using Tunynet.UI;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Mvc;

namespace Spacebuilder.Group.Controllers
{
    /// <summary>
    /// 群组管理Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = true)]
    [AnonymousBrowseCheck]
    [TitleFilter(TitlePart = "群组", IsAppendSiteName = true)]
    [ManageAuthorize(CheckCookie = false)]
    public class ControlPanelGroupController : Controller
    {
        #region Private Items
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        private GroupService groupService = new GroupService();
        #endregion

        #region 页面
        /// <summary>
        /// 管理群组
        /// </summary>
        /// <param name="model">群组editmodel</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>群组列表</returns>
        public ActionResult ManageGroups(ManageGroupEditModel model, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("群组管理");

            GroupEntityQuery group = model.GetGroupQuery();

            ViewData["Groups"] = groupService.GetsForAdmin(group.AuditStatus, group.CategoryId, group.GroupNameKeyword, group.UserId,
                group.StartDate, group.EndDate, group.minMemberCount, group.maxMemberCount, model.PageSize ?? 20, pageIndex);
            return View(model);
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns>删除群组操作</returns>
        [HttpPost]
        public ActionResult DeleteGroup(long groupId)
        {
            groupService.Delete(groupId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        #region 批量操作-群组
        /// <summary>
        /// 设置群组的审核状态
        /// </summary>
        /// <param name="groupIds">群组id</param>
        /// <param name="isApproved">审核是否通过</param>
        /// <returns>返回审核操作</returns>
        [HttpPost]
        public ActionResult BatchUpdateGroupAuditStatus(List<long> groupIds, bool isApproved = true)
        {
            groupService.Approve(groupIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, "批量设置审核状态成功"));
        }

        /// <summary>
        /// 设置群组的审核状态
        /// </summary>
        /// <param name="groupId">群组id</param>
        /// <param name="isApproved">审核是否通过</param>
        /// <returns>返回审核操作</returns>
        [HttpPost]
        public ActionResult BatchUpdateGroupAuditStatu(long groupId, bool isApproved = true)
        {
            List<long> groupIds = new List<long>() { groupId };
            groupService.Approve(groupIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置审核状态成功"));
        }
        #endregion

        #endregion
    }
}
