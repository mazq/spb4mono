using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 验证码实体
    /// </summary>
    public abstract class VerificationCodeImage : IDisposable
    {
        #region Public Properties
        /// <summary>
        /// 最小尺寸
        /// </summary>
        public static readonly Size MinimumSize = new Size(1, 1);

        /// <summary>
        /// 生成的图片
        /// </summary>
        public Image CompositeImage
        {
            get
            {
                return compositeImage;
            }
        }

        /// <summary>
        /// 字体集合
        /// </summary>
        public IList<Font> Fonts
        {
            get
            {
                return fonts;
            }
        }

        /// <summary>
        /// 颜色集合
        /// </summary>
        public IList<Color> TextColors
        {
            get
            {
                return textColors;
            }
        }

        /// <summary>
        /// 干扰线提供者集合
        /// </summary>
        public ReadOnlyCollection<VerificationCodeFilterProvider> Filters
        {
            get
            {
                List<VerificationCodeFilterProvider> list = new List<VerificationCodeFilterProvider>(filters.Count);

                foreach (VerificationCodeFilterProvider provider in filters)
                    list.Add(provider);

                return list.AsReadOnly();
            }
        }

        /// <summary>
        /// 生成验证码文字 
        /// </summary>
        public string Text
        {
            get
            {
                if (text == null)
                {
                    text = textProvider.GenerateRandomAutoInputProtectionText(this.characterSet);

                    if (string.IsNullOrEmpty(text))
                        throw new InvalidOperationException("Errors.AIP_Text_NullOrEmpty");
                }

                return text;
            }
        }

        /// <summary>
        /// 尺寸
        /// </summary>
        public Size Size
        {
            get
            {
                return size;
            }
        }
        #endregion

        #region Private / Protected
        private readonly VerificationCodeTextProvider textProvider;
        private string text;
        private readonly Size size;
        private Image compositeImage;
        private readonly List<Font> fonts;
        private readonly List<Color> textColors;
        private readonly CaptchaCharacterSet characterSet;

        internal AutoInputProtectionFilterProviderCollection filters;
        #endregion

        #region Constructors

        protected VerificationCodeImage(Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
        {
            if (textProvider == null)
                throw new ArgumentNullException("textProvider");

            if (size.Width < MinimumSize.Width || size.Height < MinimumSize.Height)
                throw new ArgumentOutOfRangeException("size", size, "Errors.SizePositiveIntRequired");

            this.textProvider = textProvider;
            if (maximumCharacters.HasValue)
                this.textProvider.MaximumCharacters = maximumCharacters.Value;
            if (minimumCharacters.HasValue)
                this.textProvider.MinimumCharacters = minimumCharacters.Value;
            this.size = size;
            this.fonts = new List<Font>(textProvider.Fonts);
            this.textColors = new List<Color>(textProvider.Colors);
            this.characterSet = characterSet;
        }


        #endregion

        #region Methods

        private static void PreProcessFilters(AutoInputProtectionFilterProviderCollection filters, Graphics graphics, Size imageSize)
        {
            foreach (VerificationCodeFilterProvider filter in filters)
            {
                if (filter.CanPreProcess)
                    filter.PreProcess(graphics, imageSize);
            }
        }

        private static void PostProcessFilters(AutoInputProtectionFilterProviderCollection filters, Graphics graphics, Size imageSize)
        {
            foreach (VerificationCodeFilterProvider filter in filters)
            {
                if (filter.CanPostProcess)
                    filter.PostProcess(graphics, imageSize);
            }
        }

        /// <summary>
        /// 绘制验证码图片
        /// </summary>
        /// <param name="IsUseLineNoise"></param>
        /// <returns></returns>
        internal Image CreateCompositeImage(bool IsUseLineNoise)
        {
            if (fonts.Count == 0)
                throw new InvalidOperationException("Errors.MissingFontSamplesForOverlay");

            if (textColors.Count == 0)
                throw new InvalidOperationException("Errors.MissingColorSamplesForOverlay");

            //绘制干扰线
            if (IsUseLineNoise)
                compositeImage = CreateSurfaceImage(size);

            bool isEmpty = false;

            if (compositeImage == null)
            {
                isEmpty = true;
                compositeImage = new Bitmap(size.Width, size.Height);
            }
            else if (compositeImage.Size != size)
                compositeImage = new Bitmap(compositeImage, size);

            using (Graphics graphics = Graphics.FromImage(compositeImage))
            {
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clip = new Region(new Rectangle(0, 0, size.Width, size.Height));

                if (isEmpty)
                    graphics.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);

                if (filters != null)
                    PreProcessFilters(filters, graphics, size);

                CreateCompositeImage(graphics, size);

                if (filters != null)
                    PostProcessFilters(filters, graphics, size);
            }

            return compositeImage;
        }

        protected abstract Image CreateSurfaceImage(Size size);
 
        protected abstract void CreateCompositeImage(Graphics graphics, Size size);
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~VerificationCodeImage()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && compositeImage != null)
                compositeImage.Dispose();
        }

        #endregion
    }
}
