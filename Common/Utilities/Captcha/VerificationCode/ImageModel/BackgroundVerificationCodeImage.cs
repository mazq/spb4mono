using System;
using System.Drawing;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 背景图生成提供者
    /// </summary>
    public abstract class BackgroundVerificationCodeImage : VerificationCodeImage
    {
        private readonly Image background;

        protected BackgroundVerificationCodeImage(Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
            : base(size, textProvider, characterSet, maximumCharacters, minimumCharacters)
        { }

        protected BackgroundVerificationCodeImage(Image background, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
            // NOTE: MinimumSize size is only specified when background is null so that ArgumentNullException may be thrown by this constructor
            // after the base constructor is finished.
            : base((background == null) ? MinimumSize : background.Size, textProvider, characterSet, maximumCharacters, minimumCharacters)
        {
            if (background == null)
                throw new ArgumentNullException("background");

            this.background = background;
        }

        /// <summary>
        /// 背景图
        /// </summary>
        public Image Background
        {
            get
            {
                return background;
            }
        }


        #region Methods
        /// <summary>
        /// 创建图像
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        protected override Image CreateSurfaceImage(Size size)
        {
            return background;
        }

        protected abstract override void CreateCompositeImage(Graphics graphics, Size size);

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && background != null)
                {
                    background.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion
    }
}
