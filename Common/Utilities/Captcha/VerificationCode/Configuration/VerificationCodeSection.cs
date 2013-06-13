using System.Configuration;

namespace Spacebuilder.Common
{
    /// <summary>
    /// Provides programmatic access to the <c>&lt;VerificationCode&gt;</c> configuration file section.
    /// </summary>
    public sealed class VerificationCodeSection : ConfigurationSection
    {
        public VerificationCodeSection() { }

        #region Public Properties
        /// <summary>
        /// 文字生成提供者
        /// </summary>
        [ConfigurationProperty("textProviders")]
        public ProviderSettingsCollection TextProviders
        {
            get
            {
                return (ProviderSettingsCollection)base["textProviders"];
            }
        }

        /// <summary>
        /// 图像生成提供者
        /// </summary>
        [ConfigurationProperty("imageProviders")]
        public ProviderSettingsCollection ImageProviders
        {
            get
            {
                return (ProviderSettingsCollection)base["imageProviders"];
            }
        }

        /// <summary>
        /// 干扰线生成提供者
        /// </summary>
        [ConfigurationProperty("filters")]
        public ProviderSettingsCollection FilterProviders
        {
            get
            {
                return (ProviderSettingsCollection)base["filters"];
            }
        }

        /// <summary>
        /// 默认图像生成提供者
        /// </summary>
        [StringValidator(MinLength = 1)]
        [ConfigurationProperty("defaultImageProvider", DefaultValue = "LineNoiseVerificationCodeImageProvider")]
        public string DefaultImageProvider
        {
            get
            {
                return (string)base["defaultImageProvider"];
            }
            set
            {
                base["defaultImageProvider"] = value;
            }
        }

        /// <summary>
        /// 默认文字生成提供者
        /// </summary>
        [StringValidator(MinLength = 1)]
        [ConfigurationProperty("defaultTextProvider", DefaultValue = "BasicEnglishVerificationCodeTextProvider")]
        public string DefaultTextProvider
        {
            get
            {
                return (string)base["defaultTextProvider"];
            }
            set
            {
                base["defaultTextProvider"] = value;
            }
        }

        //[ConfigurationProperty("userMode", DefaultValue = VerificationCodeUserMode.Client)]
        //public VerificationCodeUserMode UserMode
        //{
        //    get
        //    {
        //        return (VerificationCodeUserMode)base["userMode"];
        //    }
        //    set
        //    {
        //        base["userMode"] = value;
        //    }
        //}

        /// <summary>
        /// 缓存方式
        /// </summary>
        [ConfigurationProperty("persistenceMode", DefaultValue = VerificationCodePersistenceMode.Cache)]
        public VerificationCodePersistenceMode PersistenceMode
        {
            get
            {
                return (VerificationCodePersistenceMode)base["persistenceMode"];
            }
            set
            {
                base["persistenceMode"] = value;
            }
        }
        #endregion

    }
}
