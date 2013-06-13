//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common;
using Tunynet.Globalization;

namespace Spacebuilder.Common.EventModules
{
    /// <summary>
    /// 推荐相关事件
    /// </summary>
    public class RecommendEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<RecommendItem>.Instance().After += new CommonEventHandler<RecommendItem, CommonEventArgs>(RecommendPointModule_After);
            EventBus<Tag>.Instance().After += new CommonEventHandler<Tag, CommonEventArgs>(DeleteTag_After);
        }

        /// <summary>
        /// 推荐积分处理
        /// </summary>
        /// <param name="sender">推荐实体</param>
        /// <param name="eventArgs">事件参数</param>
        void RecommendPointModule_After(RecommendItem sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType != EventOperationType.Instance().Create())
            {
                return;
            }
            string pointItemKey = string.Empty;
            PointService pointService = new PointService();
            string description = string.Empty;
            TenantTypeService tenantTypeService = new TenantTypeService();

            var urlGetter = RecommendUrlGetterFactory.Get(sender.TenantTypeId);

            NoticeService noticeService = new NoticeService();
            Notice notice = Notice.New();
            notice.TypeId = NoticeTypeIds.Instance().Hint();

            //notice.TemplateName = "FollowUser";
            notice.UserId = sender.UserId;
            notice.LeadingActorUserId = sender.ReferrerId;
            notice.LeadingActor = sender.ReferrerName;
            notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.ReferrerId));
            notice.RelativeObjectId = sender.UserId;
            notice.RelativeObjectName = sender.ItemName;
            notice.RelativeObjectUrl = sender.DetailUrl;

            if (sender.TenantTypeId == TenantTypeIds.Instance().User())
            {
                pointItemKey = PointItemKeys.Instance().RecommendUser();
                description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_RecommendUser"));
                notice.TemplateName = NoticeTemplateNames.Instance().ManagerRecommended();
            }
            else
            {
                pointItemKey = PointItemKeys.Instance().RecommendContent();
                TenantType tenantType = tenantTypeService.Get(sender.TenantTypeId);
                if (tenantType == null)
                    return;
                description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_RecommendContent"), tenantType.Name, sender.ItemName);
                notice.TemplateName = NoticeTemplateNames.Instance().ManagerRecommended();
            }

            noticeService.Create(notice);
            pointService.GenerateByRole(sender.UserId, pointItemKey, description);
        }

        /// <summary>
        /// 删除话题、标签后删除推荐
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="eventArgs"></param>
        void DeleteTag_After(Tag tag, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType != EventOperationType.Instance().Delete() || tag == null)
            {
                return;
            }
            RecommendService recommendService = new RecommendService();
            recommendService.Delete(tag.TagId, tag.TenantTypeId);
        }

        
    }
}
