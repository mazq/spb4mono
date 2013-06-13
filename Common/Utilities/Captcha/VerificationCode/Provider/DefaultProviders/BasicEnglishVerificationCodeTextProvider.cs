using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Drawing;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 英文字符生成提供者
    /// </summary>
    public sealed class BasicEnglishVerificationCodeTextProvider : VerificationCodeTextProvider
    {
        public BasicEnglishVerificationCodeTextProvider()
        {
        }

        public BasicEnglishVerificationCodeTextProvider(IEnumerable<Color> colors, IEnumerable<Font> fonts)
            : base(colors, fonts)
        {
        }

        #region Methods
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
                name = "BasicEnglishAutoInputProtectionTextProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Basic English Auto-Input Protection Text Provider");
            }

            base.Initialize(name, config);

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);

                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException(string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                        "Errors.UnrecognizedAttribute", attr));
            }
        }

        /// <summary>
        /// 生成字符集
        /// </summary>
        /// <param name="characterSet"></param>
        /// <returns></returns>
        public override string GenerateRandomAutoInputProtectionText(CaptchaCharacterSet characterSet)
        {
            return VerificationCodeRandomString.Create(MinimumCharacters, MaximumCharacters, characterSet, null);
        }
        #endregion
    }
}
