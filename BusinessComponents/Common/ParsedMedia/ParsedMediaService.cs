//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Repositories;
using Tunynet.Utilities;
using System.Text.RegularExpressions;
using System.Web;

namespace Tunynet.Common
{
    /// <summary>
    /// 多媒体网址业务逻辑类
    /// </summary>
    public class ParsedMediaService
    {

        private IParsedMediaRepository ParsedMediaRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public ParsedMediaService()
            : this(new ParsedMediaRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public ParsedMediaService(IParsedMediaRepository ParsedMediaRepository)
        {
            this.ParsedMediaRepository = ParsedMediaRepository;
        }


        #region Create/Update

        /// <summary>
        /// 创建多媒体网址
        /// </summary>
        /// <param name="ParsedMedia">多媒体网址实体</param>
        /// <returns>Url别名</returns>
        public string Create(ParsedMedia ParsedMedia)
        {
            ShortUrlService shortUrlService = new ShortUrlService();
            string alias = shortUrlService.Create(ParsedMedia.Url);
            ParsedMedia.Alias = alias;

            EventBus<ParsedMedia>.Instance().OnBefore(ParsedMedia, new CommonEventArgs(EventOperationType.Instance().Create()));
            
            string description = HtmlUtility.StripHtml(ParsedMedia.Description, true, false).Trim();
            if (description.Length > 500)
                ParsedMedia.Description = description.Substring(0, 500) + "…";
            else
                ParsedMedia.Description = description;
            object result = ParsedMediaRepository.Insert(ParsedMedia);
            if ((bool)result)
                EventBus<ParsedMedia>.Instance().OnAfter(ParsedMedia, new CommonEventArgs(EventOperationType.Instance().Create()));
            return alias;
        }

        /// <summary>
        /// 更新多媒体网址
        /// </summary>
        /// <param name="ParsedMedia">多媒体网址实体</param>
        public void Update(ParsedMedia ParsedMedia)
        {
            EventBus<ParsedMedia>.Instance().OnBefore(ParsedMedia, new CommonEventArgs(EventOperationType.Instance().Update()));
            ParsedMediaRepository.Update(ParsedMedia);
            EventBus<ParsedMedia>.Instance().OnAfter(ParsedMedia, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除多媒体网址
        /// </summary>
        /// <param name="alias">多媒体网址别名</param>
        public void Delete(string alias)
        {
            if (string.IsNullOrEmpty(alias))
                return;

            ParsedMedia entity = ParsedMediaRepository.Get(alias);
            if (entity != null)
            {
                EventBus<ParsedMedia>.Instance().OnBefore(entity, new CommonEventArgs(EventOperationType.Instance().Delete()));
                ParsedMediaRepository.Delete(entity);
                EventBus<ParsedMedia>.Instance().OnAfter(entity, new CommonEventArgs(EventOperationType.Instance().Delete()));
            }
        }

        #endregion Create/Update

        #region Get

        /// <summary>
        /// 获取多媒体网址实体
        /// </summary>
        ///<param name="alias">多媒体网址别名</param>
        public ParsedMedia Get(string alias)
        {
            if (!string.IsNullOrEmpty(alias))
                return ParsedMediaRepository.Get(alias);

            return null;
        }

        #endregion Get

        #region 内容解析

        private const string regexRule = @"(?<=(http\:\/\/))[^(\r\n)^ ^\,^\u3002^\uff1b^\uff0c^\uff1a^\u201c^\u201d^\uff08^\uff09^\u3001^\uff1f^\u300a^\u300b]*";

        /// <summary>
        /// 解析内容用于创建AtUser
        /// </summary>
        /// <param name="body">待解析的内容</param>
        /// <param name="videoAlias">视频Url别名</param>
        /// <param name="audioAlias">音乐Url别名</param>
        public string ResolveBodyForEdit(string body, out string videoAlias, out string audioAlias)
        {
            videoAlias = audioAlias = string.Empty;
            if (string.IsNullOrEmpty(body) || !body.Contains("http://"))
                return body;

            string newBody = body, strUrl = string.Empty, strPreUrl = string.Empty;
            string urlDomain = ShortUrlService.GetUrlDomain();
            urlDomain = urlDomain.Substring(7);

            Dictionary<string, string> urls = new Dictionary<string, string>();
            ShortUrlService shortUrlService = new ShortUrlService();

            ParsedMedia parsedMedia = null;
            Regex rg = new Regex(regexRule, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matches = rg.Matches(body);

            if (matches != null)
            {
                foreach (Match m in matches)
                {
                    if (string.IsNullOrEmpty(m.Value))
                        continue;

                    strUrl = m.Value;

                    if (!string.IsNullOrEmpty(strUrl) && !strUrl.Equals(strPreUrl, StringComparison.CurrentCultureIgnoreCase))
                        strPreUrl = strUrl;
                    else
                        continue;

                    if (strUrl.StartsWith(urlDomain) || strUrl.Length == 6)
                    {
                        string temalias = strUrl.Length == 6 ? strUrl : strUrl.Substring(urlDomain.Length + 1);
                        if (temalias.Length == 6)
                        {
                            parsedMedia = Get(temalias);
                            if (strUrl.Contains(urlDomain))
                            {
                                urls[temalias] = strUrl;
                            }
                        }
                        else
                        {
                            temalias = shortUrlService.Create("http://" + strUrl);
                            if (string.IsNullOrEmpty(temalias))
                            {
                                continue;
                            }

                            urls[temalias] = strUrl;
                        }
                    }

                    if (parsedMedia == null)
                        continue;

                    if (parsedMedia.MediaType == MediaType.Video)
                    {
                        if (string.IsNullOrEmpty(videoAlias))
                            videoAlias = parsedMedia.Alias;
                    }
                    else if (parsedMedia.MediaType == MediaType.Audio)
                    {
                        if (string.IsNullOrEmpty(audioAlias))
                            audioAlias = parsedMedia.Alias;
                    }
                }

                if (urls.Count() > 0)
                {
                    foreach (var url in urls)
                    {
                        body = body.Replace("http://" + url.Value, "http://" + url.Key + " ");
                    }
                }
            }

            return body;
        }

        /// <summary>
        /// 解析内容中的AtUser用户展示展示
        /// </summary>
        /// <param name="body">待解析的内容</param>
        /// <param name="associateId">关联项Id</param>
        /// <param name="userId">关联项作者Id</param>
        /// <param name="UrlGenerate">url生成对应标签的方法</param>
        public string ResolveBodyForDetail(string body, long associateId, long userId, Func<string, long, long, ParsedMedia, string> UrlGenerate)
        {
            if (string.IsNullOrEmpty(body) || !body.Contains("http://"))
            {
                return body;
            }

            Regex rg = new Regex(regexRule, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matches = rg.Matches(body);

            if (matches != null)
            {
                string strUrl = string.Empty, preUrl = string.Empty, shortUrl = string.Empty;
                string urlDomain = ShortUrlService.GetUrlDomain().Substring(7);
                ParsedMedia parsedMedia = null;
                ShortUrlService shortUrlService = new ShortUrlService();

                foreach (Match m in matches)
                {
                    if (string.IsNullOrEmpty(m.Value))
                        continue;

                    strUrl = m.Value;

                    if (string.IsNullOrEmpty(strUrl) || strUrl.Equals(preUrl, StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    preUrl = strUrl;

                    if (strUrl.StartsWith(urlDomain))
                    {
                        string temalias = strUrl.Substring(strUrl.LastIndexOf('/') + 1);
                        if (temalias.Length != 6)
                            continue;

                        parsedMedia = Get(temalias);
                        if (parsedMedia == null)
                            continue;

                        shortUrl = shortUrlService.GetShortUrl(temalias, "http://" + urlDomain);
                    }
                    else if (strUrl.Length == 6)
                    {
                        ShortUrlEntity entity = null;
                        shortUrl = shortUrlService.GetShortUrl(strUrl, out entity, "http://" + urlDomain);
                        if (entity == null)
                            continue;

                        parsedMedia = Get(strUrl);
                        if (parsedMedia == null)
                        {
                            parsedMedia = ParsedMedia.New();
                            parsedMedia.MediaType = MediaType.Other;
                            parsedMedia.Alias = entity.Alias;
                            parsedMedia.Url = entity.Url;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    body = body.Replace("http://" + strUrl, UrlGenerate(shortUrl, associateId, userId, parsedMedia));
                }
            }

            return body;
        }

        /// <summary>
        /// 解析内容中的多媒体内容
        /// </summary>
        /// <param name="body">待解析的内容</param>
        /// <param name="TagGenerate">url生成对应标签的方法</param>
        public string ResolveBodyForHtmlDetail(string body, Func<string, ParsedMedia, string> TagGenerate)
        {
            if (string.IsNullOrEmpty(body))
                return body;

            Regex rg = new Regex(@"(\<img(.[^\<]*)?(alt=""(?<alias>[a-zA-Z0-9]*)"")(.[^\<]*)?\>)", RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matches = rg.Matches(body);

            if (matches != null)
            {
                string alias = string.Empty, preAlias = string.Empty, imgHtml = string.Empty, shortUrl = string.Empty;
                ParsedMedia parsedMedia = null;
                ShortUrlService shortUrlService = new ShortUrlService();
                string urlDomain = ShortUrlService.GetUrlDomain();

                foreach (Match m in matches)
                {
                    if (m.Groups["alias"] == null || string.IsNullOrEmpty(m.Groups["alias"].Value))
                        continue;

                    alias = m.Groups["alias"].Value;

                    if (!string.IsNullOrEmpty(alias) && !alias.Equals(preAlias, StringComparison.CurrentCultureIgnoreCase))
                        preAlias = alias;
                    else
                        continue;

                    imgHtml = m.Value;
                    if (alias.Length == 6)
                    {
                        ShortUrlEntity entity = null;
                        shortUrl = shortUrlService.GetShortUrl(alias, out entity, urlDomain);
                        if (entity != null)
                        {
                            parsedMedia = Get(alias);
                            if (parsedMedia == null)
                                continue;
                        }
                        else
                            continue;
                    }

                    body = body.Replace(imgHtml, TagGenerate(shortUrl, parsedMedia));
                }
            }

            return body;
        }

        #endregion

    }
}