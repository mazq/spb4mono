//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using System.Collections.Generic;
using Spacebuilder.Common;
using System;
using Tunynet;
using Spacebuilder.Group;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博正文解析器
    /// </summary>
    public class MicroblogBodyProcessor : IMicroblogBodyProcessor
    {
        /// <summary>
        /// 内容处理
        /// </summary>
        /// <param name="body">待处理内容</param>
        /// <param name="associateId">待处理相关项Id</param>
        /// <param name="userId">相关项作者</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns></returns>
        public string Process(string body, string ownerTenantTypeId, long associateId, long userId, long ownerId)
        {
            string tenantTypeId = TenantTypeIds.Instance().Microblog();
            AtUserService atUserService = new AtUserService(tenantTypeId);
            body = atUserService.ResolveBodyForDetail(body, associateId, userId, AtUserTagGenerate);

            TagService tagService = new TagService(tenantTypeId);

            Func<KeyValuePair<string, long>, long, string> topicTagGenerate = delegate(KeyValuePair<string, long> _tagNameWithId, long _ownerId)
            {
                return string.Format("<a href=\"{1}\">#{0}#</a>", _tagNameWithId.Key, MicroblogUrlGetterFactory.Get(ownerTenantTypeId).TopicDetail(_tagNameWithId.Key.Trim(), _ownerId));
            };

            body = tagService.ResolveBodyForDetail(body, associateId, ownerId, topicTagGenerate);

            EmotionService emotionService = DIContainer.Resolve<EmotionService>();
            body = emotionService.EmoticonTransforms(body);

            ParsedMediaService parsedMediaService = new ParsedMediaService();
            body = parsedMediaService.ResolveBodyForDetail(body, associateId, userId, UrlTagGenerate);



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

        /// <summary>
        /// 解析Url
        /// </summary>
        /// <returns></returns>
        private string UrlTagGenerate(string url, long associateId, long userId, ParsedMedia parsedMedia)
        {
            if (parsedMedia == null)
                return string.Empty;
            if (parsedMedia.MediaType == MediaType.Video)
            {
                return string.Format("<a id=\"attachmentsListLiVideo-{0}\" href=\"{3}\" target=\"_blank\" data-microblogId=\"{1}\">{2}<span class=\"tn-icon tn-icon-movie tn-icon-inline\"></span></a>", parsedMedia.Alias, associateId, url, SiteUrls.Instance()._Microblog_Attachments_Video(userId, associateId, parsedMedia.Alias));
            }
            else if (parsedMedia.MediaType == MediaType.Audio)
            {
                return string.Format("<a id=\"attachmentsListLiMusic-{0}\" href=\"{3}\" target=\"_blank\" data-microblogId=\"{1}\" >{2}<span class=\"tn-icon tn-icon-music tn-icon-inline\"></span></a>", parsedMedia.Alias, associateId, url, SiteUrls.Instance()._Microblog_Attachments_Music(userId, associateId, parsedMedia.Alias));
            }
            else
            {
                return string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", parsedMedia.Url, DIContainer.Resolve<IShortUrlSettingsManager>().Get().ShortUrlDomain + url);
            }

        }
    }
}