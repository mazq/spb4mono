//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 评论正文解析器
    /// </summary>
    public class CommentBodyProcessor : ICommentBodyProcessor
    {
        /// <summary>
        /// 内容处理
        /// </summary>
        /// <param name="body">待处理内容</param>
        /// <param name="associateId">待处理相关项Id</param>
        /// <param name="userId">相关项作者</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string Process(string body, string tenantTypeId, long associateId, long userId)
        {
            tenantTypeId = TenantTypeIds.Instance().Comment();

            AtUserService atUserService = new AtUserService(tenantTypeId);
            atUserService.ResolveBodyForEdit(body, userId, associateId);
            body = atUserService.ResolveBodyForDetail(body, associateId, userId, AtUserTagGenerate);

            EmotionService emotionService = DIContainer.Resolve<EmotionService>();
            body = emotionService.EmoticonTransforms(body);

            return body;
        }

        /// <summary>
        /// 生成AtUserHtml标签
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="displayName">显示名称</param>
        /// <returns></returns>
        private string AtUserTagGenerate(string userName, string displayName)
        {
            return string.Format("<a href=\"{1}\" target=\"_blank\" title=\"{0}\">@{0}</a> ", displayName, SiteUrls.Instance().SpaceHome(userName));
        }
    }
}