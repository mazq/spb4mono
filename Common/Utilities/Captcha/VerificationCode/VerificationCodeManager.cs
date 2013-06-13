//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Tunynet;
using Tunynet.Caching;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 验证码管理类
    /// </summary>
    public static class VerificationCodeManager
    {
        private static ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        #region Public Properties

        /// <summary>
        /// 缓存存放的位置
        /// </summary>
        public static VerificationCodePersistenceMode PersistenceMode
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
            get
            {
                EnsureProviders();

                return persistenceMode;
            }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
            set
            {
                if (!Enum.IsDefined(typeof(VerificationCodePersistenceMode), value))
                    throw new System.ComponentModel.InvalidEnumArgumentException("value", (int)value, typeof(VerificationCodePersistenceMode));

                persistenceMode = value;

                switch (value)
                {
                    case VerificationCodePersistenceMode.Cache:
                        getPersistedValue = GetCacheValue;
                        break;
                    case VerificationCodePersistenceMode.Session:
                        getPersistedValue = GetSessionValue;
                        break;
                }
            }
        }

        /// <summary>
        /// 默认的文字生成提供者
        /// </summary>
        public static VerificationCodeTextProvider DefaultTextProvider
        {
            get
            {
                EnsureProviders();

                return textProvider;
            }
        }
        /// <summary>
        /// 文字生成提供者
        /// </summary>
        public static AutoInputProtectionTextProviderCollection TextProviders
        {
            get
            {
                EnsureProviders();

                return textProviders;
            }
        }

        /// <summary>
        /// 默认的图片生成提供者
        /// </summary>
        public static VerificationCodeImageProvider DefaultImageProvider
        {
            get
            {
                EnsureProviders();

                return imageProvider;
            }
        }

        /// <summary>
        /// 图片生成提供者
        /// </summary>
        public static AutoInputProtectionImageProviderCollection ImageProviders
        {
            get
            {
                EnsureProviders();

                return imageProviders;
            }
        }

        /// <summary>
        /// 干扰生成提供者
        /// </summary>
        public static AutoInputProtectionFilterProviderCollection FilterProviders
        {
            get
            {
                EnsureProviders();

                return filterProviders;
            }
        }
        #endregion

        #region Private / Protected

        private delegate object GetPersistedValueStrategy(HttpContextBase context, string internalKey, bool remove);

        private static readonly AutoInputProtectionTextProviderCollection textProviders = new AutoInputProtectionTextProviderCollection();
        private static readonly AutoInputProtectionImageProviderCollection imageProviders = new AutoInputProtectionImageProviderCollection();
        private static readonly AutoInputProtectionFilterProviderCollection filterProviders = new AutoInputProtectionFilterProviderCollection();
        private static VerificationCodeImageProvider imageProvider;
        private static VerificationCodeTextProvider textProvider;
        //private static VerificationCodeUserMode userMode;
        private static VerificationCodePersistenceMode persistenceMode;

        private static GetPersistedValueStrategy getPersistedValue = GetCacheValue;
        private volatile static bool initialized;
        private static bool initializedTextProviders, initializedImageProviders;

        #endregion

        #region Methods

        /// <summary>
        /// 从webconfig中加载文字，背景图，以及过滤器的提供者
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        private static void EnsureProviders()
        {
            if (!initialized)
            {
                VerificationCodeSection section = (VerificationCodeSection)WebConfigurationManager.GetSection("spbVerificationCode/autoInputProtection");
                bool hasSection = section != null;

                if (!hasSection)
                    section = new VerificationCodeSection();

                //userMode = section.UserMode;
                PersistenceMode = section.PersistenceMode;

                InitializeTextProviders(hasSection, section);
                InitializeImageProviders(hasSection, section);

                ProviderSettingsCollection pc = new ProviderSettingsCollection();
                pc.Add(new ProviderSettings("filters", "Spacebuilder.Common.CrosshatchVerificationCodeFilterProvider,Spacebuilder.Common"));
                ProvidersHelper.InstantiateProviders(pc, filterProviders, typeof(VerificationCodeFilterProvider));
                initialized = true;
            }
        }

        /// <summary>
        /// 加载TextProvider
        /// </summary>
        /// <param name="configHasSection"></param>
        /// <param name="section"></param>
        private static void InitializeTextProviders(bool configHasSection, VerificationCodeSection section)
        {
            //加载TextProvider 如果webconfig中没有配置 则加载默认的BasicEnglishAutoInputProtectionTextProvider

            if (configHasSection)
            {
                if (!initializedTextProviders)
                {
                    ProvidersHelper.InstantiateProviders(section.TextProviders, textProviders, typeof(VerificationCodeTextProvider));
                    initializedTextProviders = true;
                }

                if (section.DefaultTextProvider.Equals(section.ElementInformation.Properties["defaultTextProvider"].DefaultValue))
                {
                    if (textProviders.Count > 0)
                        textProvider = textProviders[0];
                    else
                        textProvider = new BasicEnglishVerificationCodeTextProvider(
                            new[] { Color.Black, Color.Red, Color.Brown },
                            new[] { new Font("Times New Roman", 1), new Font("Arial", 1), new Font("Microsoft Sans Serif", 1) });
                }
                else
                    textProvider = textProviders[section.DefaultTextProvider];

                if (textProvider == null)
                    throw new InvalidOperationException("Errors.Config_Missing_TextProvider");
            }
            else if (textProvider == null)
            {
                section.TextProviders.Add(CreateProviderSettings(section.DefaultTextProvider, typeof(BasicEnglishVerificationCodeTextProvider),
                    new KeyValuePair<string, string>("colors", "Red,Green,Blue,Brown"),
                    new KeyValuePair<string, string>("fonts", "Times New Roman, Arial, Microsoft Sans Serif")));

                ProvidersHelper.InstantiateProviders(section.TextProviders, textProviders, typeof(VerificationCodeTextProvider));
                textProvider = textProviders[section.DefaultTextProvider];
            }
        }

        /// <summary>
        /// 加载ImageProvider
        /// </summary>
        /// <param name="configHasSection"></param>
        /// <param name="section"></param>
        private static void InitializeImageProviders(bool configHasSection, VerificationCodeSection section)
        {
            //加载ImageProvider 如果webconfig中没有配置 则加载默认的LineNoiseAutoInputProtectionImageProvider

            if (configHasSection)
            {
                if (!initializedImageProviders)
                {
                    ProvidersHelper.InstantiateProviders(section.ImageProviders, imageProviders, typeof(VerificationCodeImageProvider));
                    initializedImageProviders = true;
                }

                if (section.DefaultImageProvider.Equals(section.ElementInformation.Properties["defaultImageProvider"].DefaultValue))
                {
                    if (imageProviders.Count > 0)
                        imageProvider = imageProviders[0];
                    else
                        imageProvider = new LineNoiseVerificationCodeImageProvider();
                }
                else
                    imageProvider = imageProviders[section.DefaultImageProvider];

                if (imageProvider == null)
                    throw new InvalidOperationException("Errors.Config_Missing_ImageProvider");
            }
            else if (imageProvider == null)
            {
                section.ImageProviders.Add(CreateProviderSettings(section.DefaultImageProvider, typeof(LineNoiseVerificationCodeImageProvider)));

                ProvidersHelper.InstantiateProviders(section.ImageProviders, imageProviders, typeof(VerificationCodeImageProvider));
                imageProvider = imageProviders[section.DefaultImageProvider];
            }
        }

        /// <summary>
        /// 加载提供者的设置
        /// </summary>
        private static ProviderSettings CreateProviderSettings(string name, Type type, params KeyValuePair<string, string>[] parameters)
        {
            ProviderSettings settings = new ProviderSettings(name, type.AssemblyQualifiedName);

            foreach (KeyValuePair<string, string> parameter in parameters)
                settings.Parameters.Add(parameter.Key, parameter.Value);

            return settings;
        }

        private static readonly object VerificationCodeImageLockObj = new object();
        /// <summary>
        /// 创建验证码的VerificationCodeImage对象
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        private static VerificationCodeImage GenerateRandomAutoInputProtectionImage(Size size, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
        {
            if (size.Width < 1 || size.Height < 1)
                throw new ArgumentOutOfRangeException("size", size, "Errors.SizePositiveIntRequired");

            EnsureProviders();
            VerificationCodeImage image = null;
            lock (VerificationCodeImageLockObj)
            {
                image = imageProvider.GenerateRandomAutoInputProtectionImage(size, textProvider, characterSet, maximumCharacters, minimumCharacters);
            }
            image.filters = filterProviders;

            return image;
        }

        /// <summary>
        /// 验证用户输入是否正确
        /// </summary>
        /// <param name="text"></param>
        /// <param name="imageText"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static bool ValidateUserInput(string text, string imageText)
        {
            bool passed = DefaultTextProvider.ValidateUserInput(text, imageText);
            return passed;
        }

        /// <summary>
        /// 将文字存入缓存 （用于保存验证码上的字符）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="text"></param>
        /// <param name="publicKey"></param>
        /// <param name="validationKeepAlive"></param>
        /// <param name="validationTimeoutSeconds"></param>
        public static void CacheText(HttpContextBase context, string text, string publicKey,
            bool validationKeepAlive, int validationTimeoutSeconds)
        {
            string textKey = GetCacheKeyForText(publicKey);
            if (PersistenceMode == VerificationCodePersistenceMode.Cache)
            {
                cacheService.Add(textKey, text, new TimeSpan(0, 0, validationTimeoutSeconds));
            }
            else
                context.Session[textKey] = text;
        }

        /// <summary>
        /// 图像的生成和缓存
        /// </summary>
        /// <param name="context">httpcontext</param>
        /// <param name="size">图像大小</param>
        /// <param name="requestTimeoutSeconds">过期时间</param>
        /// <param name="publicKey">生成的publicKey</param>
        /// <param name="characterSet">字符集</param>
        /// <param name="enableLineNoise">是否有干扰线</param>
        /// <param name="maximumCharacters">最大字符长度</param>
        /// <param name="minimumCharacters">最小字符长度</param>
        /// <returns></returns>
        public static VerificationCodeImage GenerateAndCacheImage(HttpContextBase context, Size size, int requestTimeoutSeconds, out string publicKey, CaptchaCharacterSet characterSet, bool enableLineNoise, int? minimumCharacters, int? maximumCharacters)
        {
            MemoryStream stream;
            VerificationCodeImage aipImage;
            int attempt = 0;
            string text;

            do
            {
                aipImage = GenerateRandomAutoInputProtectionImage(size, characterSet, maximumCharacters, minimumCharacters);
                text = aipImage.Text;
            }
            while (!CreateImageStream(ref attempt, context, aipImage, enableLineNoise, out stream));

            lock (GenerateAndCacheImageLockObj)
            {
                publicKey = GeneratePublicCacheKey(context, text);
                string imageKey = GetCacheKeyForImage(publicKey);
                cacheService.Add(imageKey, stream.ToArray(), new TimeSpan(0, 5, 0));
            }
            return aipImage;
        }

        private static readonly object GenerateAndCacheImageLockObj = new object();

        /// <summary>
        ///创建验证码以及图片的数据流
        /// </summary>
        private static bool CreateImageStream(ref int attempt, HttpContextBase context, VerificationCodeImage aipImage, bool isUseLineNoise, out MemoryStream stream)
        {
            stream = null;

            using (Image image = aipImage.CreateCompositeImage(isUseLineNoise))
            {
                try
                {
                    stream = new MemoryStream(image.Width * image.Height);

                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (ArgumentException)
                {
                    if (stream != null)
                        stream.Dispose();
                    attempt++;
                    if (attempt == 3)
                        throw;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 生成PublicKey
        /// </summary>
        /// <param name="context"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GeneratePublicCacheKey(HttpContextBase context, string text)
        {
            using (System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed())
            {
                return Convert.ToBase64String(sha1.ComputeHash(
                    Encoding.UTF8.GetBytes(text + DateTime.Now.Ticks)), //+ Environment.TickCount)),
                    Base64FormattingOptions.None);
            }
        }

        /// <summary>
        /// 图像key
        /// </summary>
        private static string GetCacheKeyForImage(string publicKey)
        {
            return publicKey + "_image";
        }
        /// <summary>
        /// 文字key
        /// </summary>
        private static string GetCacheKeyForText(string publicKey)
        {
            return publicKey + "_text";
        }

        /// <summary>
        ///利用publicKey获取缓存中的图像数据流
        /// </summary>
        public static MemoryStream GetCachedImageStream(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentException("Errors.ImageKeyNotSpecified", "publicKey");
            string imageKey = GetCacheKeyForImage(publicKey);
            try
            {
                byte[] data = (byte[])cacheService.Get(imageKey);

                if (data == null)
                    return null;
                MemoryStream stream = new MemoryStream(data);
                return stream;
            }
            catch
            { }
            return null;
        }

        /// <summary>
        /// 获取验证码的字符（用来验证用户输入的是否正确）并且自动从缓存中移除数据
        /// </summary>
        public static string GetCachedTextAndForceExpire(HttpContextBase context, string publicKey)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            string imageKey = GetCacheKeyForImage(publicKey);
            string textKey = GetCacheKeyForText(publicKey);

            try
            {
                string text = (string)getPersistedValue(context, textKey, true);
                return text;
            }
            finally
            {
                ExpireCachedImage(imageKey);
            }
        }

        /// <summary>
        /// 释放图像资源
        /// </summary>
        private static void ExpireCachedImage(string internalKey)
        {
            if (!string.IsNullOrEmpty(internalKey))
            {
                try
                {
                    cacheService.Remove(internalKey);
                }
                finally { }
            }
        }

        /// <summary>
        /// 从session中获取值
        /// </summary>
        private static object GetSessionValue(HttpContextBase context, string internalKey, bool remove)
        {
            if (context.Session == null)
                throw new Exception("SessionStateRequired"); //SessionStateRequiredException();

            try
            {
                return context.Session[internalKey];
            }
            finally
            {
                if (remove)
                    context.Session.Remove(internalKey);
            }
        }

        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        private static object GetCacheValue(HttpContextBase context, string internalKey, bool remove)
        {
            try
            {
                return cacheService.Get(internalKey);
            }
            finally
            {
                if (remove) cacheService.Remove(internalKey);
            }
        }

        #endregion
    }
}
