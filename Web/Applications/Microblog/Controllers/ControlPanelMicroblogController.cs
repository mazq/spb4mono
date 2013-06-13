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
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Mvc;
using Tunynet.UI;
using System.Text.RegularExpressions;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博后台控制面板
    /// </summary>
    [ManageAuthorize(CheckCookie = false)]
    [TitleFilter(IsAppendSiteName = true, TitlePart = "内容管理")]
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = true)]
    public class ControlPanelMicroblogController : Controller
    {
        private string tenantTypeId = TenantTypeIds.Instance().Microblog();

        #region Service

        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();

        private MicroblogService microblogService = new MicroblogService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Microblog());

        #endregion

        #region 管理微博

        #region 操作

        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="microblogIds">微博ID集合</param>
        /// <param name="isApproved">是否审核通过</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateMicroblogAuditStatus(string microblogIds, bool isApproved)
        {
            if (!string.IsNullOrEmpty(microblogIds))
            {
                IEnumerable<long> updatecategoryIds = microblogIds.TrimEnd(',').Split(',').Select(t => Convert.ToInt64(t));
                foreach (var microblogId in updatecategoryIds)
                {
                    microblogService.UpdateAuditStatus(microblogId, isApproved);
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "更新审核状态成功！"));
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请选择要更新的项！"));
            }
        }

        /// <summary>
        /// 删除微博
        /// </summary>
        /// <param name="microblogIds">微博ID集合</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteMicroblogs(string microblogIds)
        {
            if (string.IsNullOrEmpty(microblogIds))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请选择要删除的项！"));
            }

            var ids = microblogIds.TrimEnd(',').Split(',');
            foreach (var microblogId in ids)
            {
                microblogService.Delete(long.Parse(microblogId));
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "批量删除微博成功！"));

        }

        #endregion

        #region 列表

        /// <summary>
        /// 管理微博
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageMicroblogs(int pageIndex = 1, string userId = null, string keyword = null, MediaType? mediaType = null, AuditStatus? auditStatus = null, DateTime? startdate = null, DateTime? enddate = null, int pageSize = 20, string tenantTypeId = null, long? ownerId = null)
        {
            pageResourceManager.InsertTitlePart("微博管理");

            #region 组装搜索条件
            MicroblogQuery query = new MicroblogQuery();

            long? id = null;
            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.TrimStart(',').TrimEnd(',');
                if (!string.IsNullOrEmpty(userId))
                {
                    id = long.Parse(userId);
                }
            }
            query.OwnerId = ownerId;
            query.Keyword = keyword;
            query.UserId = id;
            query.MediaType = mediaType;
            query.AuditStatus = auditStatus;
            query.TenantTypeId = tenantTypeId;
            if (startdate != DateTime.MinValue)
                query.StartDate = startdate;

            if (enddate != DateTime.MinValue && enddate.HasValue)
                query.EndDate = enddate.Value.AddDays(1);

            #endregion

            ViewData["userId"] = id;
            PagingDataSet<MicroblogEntity> microblogs = microblogService.GetMicroblogs(query, pageSize, pageIndex);
            if (tenantTypeId == null)
            {
                tenantTypeId = TenantTypeIds.Instance().User();
            }
            ViewData["tenantTypeId"] = tenantTypeId;
            return View(microblogs);
        }

        #endregion

        #endregion

        #region 管理话题

        #region 操作

        /// <summary>
        /// 添加/编辑话题页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTopic(long topicId = 0)
        {
            TagEditModel tagEditModel = null;
            //创建
            if (topicId == 0)
            {
                tagEditModel = new TagEditModel();

                //标签分组下拉框
                SelectList topicGroups = GetTagGroupSelectList(0, TenantTypeIds.Instance().Microblog());
                ViewData["topicGroups"] = topicGroups;

                ViewData["editTopicTitle"] = "创建话题";

                pageResourceManager.InsertTitlePart("创建话题");

            }//编辑
            else
            {
                Tag tag = tagService.Get(topicId);
                tagEditModel = tag.AsTagEditModel();

                //话题分组下拉框
                SelectList topicGroups = GetTagGroupSelectList(tagEditModel.TopicGroupId, tagEditModel.TenantTypeId);
                ViewData["topicGroups"] = topicGroups;

                //取相关微博
                string users = tagEditModel.RelatedObjectIds.TrimStart(',').TrimEnd(',');
                if (!string.IsNullOrEmpty(users))
                {
                    IEnumerable<long> seletedUserIds = tagEditModel.RelatedObjectIds.TrimStart(',').TrimEnd(',').Split(',').Select(t => Convert.ToInt64(t));
                    ViewData["seletedUserIds"] = seletedUserIds;
                }

                ViewData["editTopicTitle"] = "编辑话题";

                pageResourceManager.InsertTitlePart("编辑话题");
            }

            return View(tagEditModel);
        }

        /// <summary>
        /// 添加/编辑话题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTopic(TagEditModel tagEditModel)
        {

            System.IO.Stream stream = null;

            //是否创建
            bool isCreate = tagEditModel.TagId == 0;

            if (isCreate)
            {
                ViewData["editTopicTitle"] = "创建话题";
            }
            else
            {
                ViewData["editTopicTitle"] = "编辑话题";
            }

            TagService tagService = new TagService(tagEditModel.TenantTypeId);
            IEnumerable<long> userIds = Request.Form.Gets<long>("RelatedObjectIds");
            //是特色标签
            if (tagEditModel.IsFeatured)
            {

                HttpPostedFileBase tagLogo = Request.Files["tagLogo"];
                string fileName = tagLogo == null ? "" : tagLogo.FileName;
                if (string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(tagEditModel.FeaturedImage))
                {

                    //标签分组下拉框
                    SelectList topicGroups = GetTagGroupSelectList(tagEditModel.TopicGroupId, tagEditModel.TenantTypeId);
                    ViewData["topicGroups"] = topicGroups;

                    //取到用户设置的相关标签
                    ViewData["seletedUserIds"] = userIds;

                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "图片不能为空");

                    return View(tagEditModel);
                }
                else if (!string.IsNullOrEmpty(fileName))
                {
                    //校验附件的扩展名
                    ILogoSettingsManager logoSettingsManager = DIContainer.Resolve<ILogoSettingsManager>();
                    if (!logoSettingsManager.Get().ValidateFileExtensions(fileName))
                    {
                        //标签分组下拉框
                        SelectList topicGroups = GetTagGroupSelectList(tagEditModel.TopicGroupId, tagEditModel.TenantTypeId);
                        ViewData["topicGroups"] = topicGroups;

                        //取到用户设置的相关标签
                        ViewData["seletedUserIds"] = userIds;

                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "只允许上传后缀名为 .gif .jpg .jpeg .png 的文件");

                        return View(tagEditModel);
                    }

                    //校验附件的大小
                    TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Tag());
                    if (!tenantLogoSettings.ValidateFileLength(tagLogo.ContentLength))
                    {

                        //标签分组下拉框
                        SelectList topicGroups = GetTagGroupSelectList(tagEditModel.TopicGroupId, tagEditModel.TenantTypeId);
                        ViewData["topicGroups"] = topicGroups;

                        //取到用户设置的相关标签
                        ViewData["seletedUserIds"] = userIds;

                        ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}KB", tenantLogoSettings.MaxLogoLength));

                        return View(tagEditModel);
                    }

                    stream = tagLogo.InputStream;
                    tagEditModel.FeaturedImage = fileName;

                }
            }

            //获取相关微博
            IEnumerable<long> seletedUserIds = userIds;

            //创建
            if (isCreate)
            {
                Tag tag = tagEditModel.AsTag();
                tagService.Create(tag, stream);

                //添加到分组
                if (tagEditModel.TopicGroupId > 0)
                {
                    tagService.BatchAddGroupsToTag(new List<long>() { tagEditModel.TopicGroupId }, tagEditModel.TagName);
                }

            }//更新
            else
            {
                Tag tag = tagEditModel.AsTag();
                tagService.Update(tag, stream);

                //添加到分组
                if (tagEditModel.TopicGroupId > 0)
                {
                    tagService.BatchAddGroupsToTag(new List<long>() { tagEditModel.TopicGroupId }, tagEditModel.TagName);
                }
            }

            return RedirectToAction("ManageMicroblogTopics");
        }

        /// <summary>
        /// 删除微博话题
        /// </summary>
        /// <param name="topicIds">话题ID集合</param>
        /// <returns></returns>
        public JsonResult DeleteTopics(string topicIds)
        {
            if (!string.IsNullOrEmpty(topicIds))
            {
                var ids = topicIds.TrimEnd(',').Split(',');
                foreach (var topicId in ids)
                {
                    tagService.Delete(long.Parse(topicId));
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "批量删除话题成功！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请选择要删除的项！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 设置话题分组页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _SetTopicGroup(string topicIds = null)
        {
            SelectList topicGroups = GetTagGroupSelectList(0, TenantTypeIds.Instance().Microblog());
            ViewData["topicGroups"] = topicGroups;
            return View();
        }

        /// <summary>
        /// 设置话题分组
        /// </summary>
        /// <param name="topicIds">话题ID集合</param>
        /// <param name="groupId">分组ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _SetTopicGroup(string topicIds, long groupId)
        {
            if (!string.IsNullOrEmpty(topicIds))
            {
                var ids = topicIds.TrimEnd(',').Split(',');
                foreach (var id in ids)
                {
                    Tag topic = tagService.Get(long.Parse(id));
                    tagService.BatchAddGroupsToTag(new List<long>() { groupId }, topic.TagName);
                }

                return Json(new StatusMessageData(StatusMessageType.Success, "设置分组成功！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请选择要设置的话题！"), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 列表

        /// <summary>
        /// 管理微博话题
        /// </summary>
        public ActionResult ManageMicroblogTopics(string keyword = "", bool? isFeatured = null, int pageIndex = 1, int pageSize = 30)
        {
            pageResourceManager.InsertTitlePart("微博话题管理");

            TagQuery tagQuery = new TagQuery();
            tagQuery.TenantTypeId = TenantTypeIds.Instance().Microblog();
            tagQuery.Keyword = keyword;
            tagQuery.IsFeatured = isFeatured;

            PagingDataSet<Tag> topics = tagService.GetTags(tagQuery, pageIndex, pageSize);
            return View(topics);
        }

        /// <summary>
        /// 维护所属下拉框及分组下拉框的状态
        /// </summary>
        private SelectList GetTagGroupSelectList(long groupId, string tenantTypeId)
        {

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

        #endregion

        #endregion

        #region 管理话题分组

        #region 操作

        /// <summary>
        /// 添加/编辑标签分组页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditTopicGroup(long topicGroupId = 0)
        {
            TagGroupEditModel topicGroupEditModel = null;

            //添加
            if (topicGroupId == 0)
            {
                topicGroupEditModel = new TagGroupEditModel();
                topicGroupEditModel.TenantTypeId = tenantTypeId;
            }//编辑
            else
            {
                topicGroupEditModel = tagService.GetGroup(topicGroupId).AsTagGroupEditModel();
            }

            return View(topicGroupEditModel);
        }

        /// <summary>
        /// 添加/编辑标签分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditTopicGroup(TagGroupEditModel topicGroupEditModel)
        {
            //添加
            if (topicGroupEditModel.GroupId == 0)
            {
                tagService.CreateGroup(topicGroupEditModel.AsTagGroup());
                return Json(new StatusMessageData(StatusMessageType.Success, "添加话题分组成功！"), JsonRequestBehavior.AllowGet);
            }//编辑
            else
            {
                tagService.UpdateGroup(topicGroupEditModel.AsTagGroup());
                return Json(new StatusMessageData(StatusMessageType.Success, "编辑话题分组成功！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除话题分组
        /// </summary>
        /// <param name="topicGroupIds">分组ID集合</param>
        /// <returns></returns>
        public JsonResult DeleteTopicGroups(string topicGroupIds)
        {
            if (!string.IsNullOrEmpty(topicGroupIds))
            {
                foreach (var topicGroupId in topicGroupIds.TrimEnd(',').Split(','))
                {
                    tagService.ClearTagsFromGroup(long.Parse(topicGroupId));
                    tagService.DeleteGroup(tagService.GetGroup((long.Parse(topicGroupId))));
                }
                return Json(new StatusMessageData(StatusMessageType.Success, "批量删除话题分组成功！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "请选择要删除的项！"), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 列表

        /// <summary>
        /// 管理话题分组
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageTopicGroups()
        {
            pageResourceManager.InsertTitlePart("话题分组管理");
            IEnumerable<TagGroup> topicGroups = tagService.GetGroups(tenantTypeId);
            return View(topicGroups);
        }

        #endregion

        #endregion
    }
}