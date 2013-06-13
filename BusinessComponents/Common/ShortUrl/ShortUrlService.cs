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
using System.Web;

namespace Tunynet.Common
{
    /// <summary>
    /// 短网址业务逻辑类
    /// </summary>
    public class ShortUrlService
    {
        private IShortUrlRepository shortUrlRepository;
        private IShortUrlSettingsManager shortUrlSettingsManager = DIContainer.Resolve<IShortUrlSettingsManager>();

        /// <summary>
        /// 构造器
        /// </summary>
        public ShortUrlService()
            : this(new ShortUrlRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public ShortUrlService(IShortUrlRepository shortUrlRepository)
        {
            this.shortUrlRepository = shortUrlRepository;
        }

        #region Create/Update

        /// <summary>
        /// 创建短网址
        /// </summary>
        /// <param name="url">需要处理的Url</param>
        /// <returns>Url别名</returns>
        public string Create(string url)
        {
            bool urlExists;

            IUrlShortner urlShortner = null;
            ShortUrlSettings settings = shortUrlSettingsManager.Get();

            if (settings.IsEnableOtherShortner)
            {
                urlShortner = DIContainer.Resolve<IUrlShortner>();
            }

            ShortUrlEntity entity = ShortUrlEntity.New(GetUrlAlias(url, out urlExists));
            entity.Url = url;

            //判断Url是否存在
            if (urlExists)
            {
                return entity.Alias;
            }

            string primaryKey = string.Empty;

            EventBus<ShortUrlEntity>.Instance().OnBefore(entity, new CommonEventArgs(EventOperationType.Instance().Create()));

            if (settings.IsEnableOtherShortner && urlShortner != null)
            {
                entity.OtherShortUrl = urlShortner.Shortner(url);
            }

            primaryKey = shortUrlRepository.Insert(entity).ToString();

            EventBus<ShortUrlEntity>.Instance().OnAfter(entity, new CommonEventArgs(EventOperationType.Instance().Create()));

            return primaryKey;
        }

        /// <summary>
        /// 更新短网址
        /// </summary>
        /// <param name="entity">需要更新的短网址信息</param>
        public void Update(ShortUrlEntity entity)
        {
            EventBus<ShortUrlEntity>.Instance().OnBefore(entity, new CommonEventArgs(EventOperationType.Instance().Update()));
            shortUrlRepository.Update(entity);
            EventBus<ShortUrlEntity>.Instance().OnAfter(entity, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除短网址
        /// </summary>
        /// <param name="alias">短网址别名</param>
        public void Delete(string alias)
        {
            if (string.IsNullOrEmpty(alias))
                return;

            ShortUrlEntity entity = shortUrlRepository.Get(alias);
            if (entity != null)
            {
                EventBus<ShortUrlEntity>.Instance().OnBefore(entity, new CommonEventArgs(EventOperationType.Instance().Delete()));
                shortUrlRepository.Delete(entity);
                EventBus<ShortUrlEntity>.Instance().OnAfter(entity, new CommonEventArgs(EventOperationType.Instance().Delete()));
            }
        }

        #endregion Create/Update

        #region Get

        /// <summary>
        /// 获取短网址实体
        /// </summary>
        ///<param name="alias">短网址别名</param>
        public ShortUrlEntity Get(string alias, string urlDomain = "")
        {
            if (!string.IsNullOrEmpty(alias))
                return shortUrlRepository.Get(alias);

            return null;
        }

        /// <summary>
        /// 获取短网址
        /// </summary>
        ///<param name="alias">短网址别名</param>
        ///<param name="urlDomain">自定义短网址域名</param>
        public string GetShortUrl(string alias, string urlDomain = "")
        {
            ShortUrlEntity entity = null;
            return GetShortUrl(alias, out entity, urlDomain);
        }


        /// <summary>
        /// 获取短网址
        /// </summary>
        ///<param name="alias">短网址别名</param>
        ///<param name="entity">要输出的实体</param>
        ///<param name="urlDomain">自定义短网址域名</param>
        public string GetShortUrl(string alias, out ShortUrlEntity entity, string urlDomain = "")
        {
            IShortUrlSettingsManager settingsManager = DIContainer.Resolve<IShortUrlSettingsManager>();
            ShortUrlSettings settings = settingsManager.Get();

            urlDomain = !string.IsNullOrEmpty(urlDomain) ? urlDomain : GetUrlDomain();

            entity = null;
            string shortUrl = string.Empty;

            if (!string.IsNullOrEmpty(alias))
            {
                entity = Get(alias);
                if (entity != null && !string.IsNullOrEmpty(entity.Alias))
                {
                    if (!settings.IsEnableOtherShortner
                        || string.IsNullOrEmpty(entity.OtherShortUrl))
                    {
                        shortUrl = urlDomain + "/" + alias;
                    }
                    else
                    {
                        shortUrl = entity.OtherShortUrl;
                    }
                }
            }

            return shortUrl;
        }


        /// <summary>
        /// 获取Url别名
        /// </summary>
        /// <param name="url">需要处理的Url</param>
        /// <param name="urlExists">Url是否已存在</param>
        /// <returns>返回Url别名</returns>
        public string GetUrlAlias(string url, out bool urlExists)
        {
            ShortUrlSettings settings = shortUrlSettingsManager.Get();

            //生成别名的个数
            int generateCount = 4;

            //要使用生成URL的字符
            string[] chars = new string[]{
                "a","b","c","d","e","f","g","h",
                "i","j","k","l","m","n","o","p",
                "q","r","s","t","u","v","w","x",
                "y","z","0","1","2","3","4","5",
                "6","7","8","9","A","B","C","D",
                "E","F","G","H","I","J","K","L",
                "M","N","O","P","Q","R","S","T",
                "U","V","W","X","Y","Z"
              };

            //对传入网址进行MD5加密
            string urlSalt = EncryptionUtility.MD5(url);
            string[] aliases = new string[generateCount];
            for (int i = 0; i < generateCount; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + urlSalt.Substring(i * 8, 8), 16);
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加
                    aliases[i] += chars[index];
                    //每次循环按位右移5位
                    hexint = hexint >> 5;
                }
            }

            return shortUrlRepository.GetUnusedAlias(aliases, url, out urlExists);
        }

        #endregion Get

        #region Helper Method


        /// <summary>
        /// 获取短网址主域名
        /// </summary>
        /// <returns></returns>
        public static string GetUrlDomain()
        {
            string urlDomain = DIContainer.Resolve<IShortUrlSettingsManager>().Get().ShortUrlDomain;
            if (!string.IsNullOrEmpty(urlDomain))
                return urlDomain;

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                urlDomain = WebUtility.HostPath(HttpContext.Current.Request.Url);
                return urlDomain;
            }
            else
            {
                ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
                SiteSettings siteSettings = siteSettingsManager.Get();
                if (!string.IsNullOrEmpty(siteSettings.MainSiteRootUrl))
                {
                    return siteSettings.MainSiteRootUrl;
                }
            }

            return string.Empty;
        }

        #endregion
    }
}