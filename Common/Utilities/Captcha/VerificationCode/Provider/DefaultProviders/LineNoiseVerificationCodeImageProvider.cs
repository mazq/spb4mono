using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Drawing;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 干扰线提供者
    /// </summary>
    public sealed class LineNoiseVerificationCodeImageProvider : PartitionedVerificationCodeImageProvider	// derived after 1.0.0 RTW
    {
        private readonly List<Color> colors = new List<Color>(8) { Color.Red, Color.Blue, Color.Black, Color.Green, Color.Black, Color.Black, Color.BurlyWood, Color.Brown };

        public LineNoiseVerificationCodeImageProvider() { }

        /// <summary>
        /// 颜色集合
        /// </summary>
        public IList<Color> Colors
        {
            get
            {
                return colors;
            }
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
                name = "LineNoiseAutoInputProtectionImageProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Line Noise Auto-Input Protection Image Provider");
            }

            VerificationCodeProviderHelper helper = new VerificationCodeProviderHelper(config);

            colors.AddRange(helper.ParseCollection<Color>("colors", false, true, false, ','));

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
        /// 生成图像
        /// </summary>
        /// <param name="size"></param>
        /// <param name="textProvider"></param>
        /// <param name="characterSet"></param>
        /// <param name="maximumCharacters"></param>
        /// <param name="minimumCharacters"></param>
        /// <returns></returns>
        public override VerificationCodeImage GenerateRandomAutoInputProtectionImage(Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
        {
            return GenerateAutoInputProtectionImage(null, size, textProvider, characterSet, maximumCharacters, minimumCharacters);
        }

        /// <summary>
        /// 生成干扰图像
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="textProvider"></param>
        /// <param name="characterSet"></param>
        /// <param name="maximumCharacters"></param>
        /// <param name="minimumCharacters"></param>
        /// <returns></returns>
        public override VerificationCodeImage GenerateAutoInputProtectionImage(string name, Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
        {
            LineNoiseVerificationCodeImage image = new LineNoiseVerificationCodeImage(size, textProvider, colors, characterSet, maximumCharacters, minimumCharacters);

            // code added after 1.0.0 RTW: 
            if (Margin != -1)
                image.Margin = Margin;

            if (MinimumCharacterRotation != -1)
                image.MinimumCharacterRotation = MinimumCharacterRotation;

            if (MaximumCharacterRotation != -1)
                image.MaximumCharacterRotation = MaximumCharacterRotation;

            return image;
        }
        #endregion
    }
}
