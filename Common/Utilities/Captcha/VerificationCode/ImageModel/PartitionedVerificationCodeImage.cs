using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Spacebuilder.Common
{
    public class PartitionedVerificationCodeImage : BackgroundVerificationCodeImage
    {
        private int margin = 2;
        private int minFontSize, maxFontSize, minRotation = -35, maxRotation = 35;

        public PartitionedVerificationCodeImage(Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
            : base(size, textProvider, characterSet, maximumCharacters, minimumCharacters)
        {
            if (textProvider == null)
                throw new ArgumentNullException("textProvider");

            minFontSize = textProvider.MinimumFontSize;
            maxFontSize = textProvider.MaximumFontSize;
        }


        public PartitionedVerificationCodeImage(Image background, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
            : base(background, textProvider, characterSet, maximumCharacters, minimumCharacters)
        {
            if (textProvider == null)
                throw new ArgumentNullException("textProvider");

            minFontSize = textProvider.MinimumFontSize;
            maxFontSize = textProvider.MaximumFontSize;
        }


        /// <summary>
        /// 合并层次 默认为2
        /// </summary>
        public int Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
            }
        }

        /// <summary>
        /// 最小文字尺寸
        /// </summary>
        public int MinimumFontSize
        {
            get
            {
                return minFontSize;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "Errors.PositiveIntRequired");

                minFontSize = value;
            }
        }

        /// <summary>
        /// 最大字体尺寸
        /// </summary>
        public int MaximumFontSize
        {
            get
            {
                return maxFontSize;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", value, "Errors.PositiveIntRequired");

                maxFontSize = value;
            }
        }

        /// <summary>
        /// 最小旋转尺度
        /// </summary>
        public int MinimumCharacterRotation
        {
            get
            {
                return minRotation;
            }
            set
            {
                minRotation = value;
            }
        }

        /// <summary>
        /// 最大旋转尺度
        /// </summary>
        public int MaximumCharacterRotation
        {
            get
            {
                return maxRotation;
            }
            set
            {
                maxRotation = value;
            }
        }

        #region Methods
        /// <summary>
        /// 创建图像
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="size"></param>
        /// <remarks>使文字分开 随机字符间距，并使用随机字体</remarks>
        protected sealed override void CreateCompositeImage(Graphics graphics, Size size)
        {
            if (maxFontSize < minFontSize)
                throw new InvalidOperationException("Errors.PartitionedAIPImage_FontSizeMaxLessThanMin");

            if (maxRotation < minRotation)
                throw new InvalidOperationException("Errors.PartitionedAIPImage_RotationMaxLessThanMin");

            using (StringFormat format = new StringFormat(
                StringFormatFlags.NoClip | StringFormatFlags.NoFontFallback | StringFormatFlags.FitBlackBox))
            {
                Random random = new Random();

                float textWidth;
                IEnumerable<CharRenderInfo> characters = GenerateCharacters(graphics, random, format, out textWidth);

                Render(graphics, size, random, characters, format, textWidth);
            }
        }

        /// <summary>
        /// 生成字符集
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="random"></param>
        /// <param name="format"></param>
        /// <param name="textWidth"></param>
        /// <returns></returns>
        private IEnumerable<CharRenderInfo> GenerateCharacters(Graphics graphics, Random random, StringFormat format, out float textWidth)
        {
            // NOTE: Do not use a C# iterator here since textWidth must be calculated immediately
            textWidth = 0;

            string text = Text;
            IList<Color> colors = new List<Color>(TextColors);
            IList<Font> fonts = new List<Font>(Fonts);
            List<CharRenderInfo> generatedCharacters = new List<CharRenderInfo>(text.Length);

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                Font font = new Font(fonts[random.Next(0, fonts.Count)].FontFamily, random.Next(minFontSize, maxFontSize + 1), GraphicsUnit.Pixel);

                SizeF size = graphics.MeasureString(c.ToString(), font, (int)graphics.ClipBounds.Width, format);

                textWidth += size.Width;

                generatedCharacters.Add(new CharRenderInfo(c, font, size, colors[random.Next(0, colors.Count)]));
            }

            return generatedCharacters;
        }

        private void Render(Graphics graphics, Size size, Random random, IEnumerable<CharRenderInfo> characters, StringFormat format, float textWidth)
        {
            string text = Text;

            int x = (text.Length == 1)
                ? (int)(size.Width / 2 - textWidth / 2)
                : margin;

            int stepX = (text.Length == 1)
                ? 0
                : (int)(size.Width - textWidth - margin * 2) / (text.Length - 1);

            foreach (CharRenderInfo c in characters)
            {
                Font font = c.Font;
                int rotate = random.Next(minRotation, maxRotation + 1);
                int maxY = size.Height - font.Height - margin * 2;
                int y = (margin >= maxY) ? margin : random.Next(margin, maxY + 1);

                GraphicsContainer container = graphics.BeginContainer();

                graphics.TranslateTransform(x + c.Size.Width / 2, y + c.Size.Height / 2);
                graphics.RotateTransform(rotate);
                graphics.TranslateTransform(-c.Size.Width / 2, -c.Size.Height / 2);

                RenderCharacter(graphics, c.Char, c.Size, c.Font, c.Color, format, random);

                graphics.EndContainer(container);

                x += (int)c.Size.Width + stepX;
            }
        }

        /// <summary>
        /// 绘制字符
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="character"></param>
        /// <param name="size"></param>
        /// <param name="font"></param>
        /// <param name="color"></param>
        /// <param name="format"></param>
        /// <param name="random"></param>
        protected virtual void RenderCharacter(Graphics graphics, char character, SizeF size, Font font, Color color, StringFormat format, Random random)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                graphics.DrawString(character.ToString(), font, brush, 0, 0, format);
            }
        }
        #endregion

        #region Nested
        /// <summary>
        /// Contains information about an individual character in the text that will be rendered by an instance of <see cref="PartitionedAutoInputProtectionImage"/>.
        /// This class cannot be inherited.
        /// </summary>
        private sealed class CharRenderInfo
        {
            #region Public Properties
            /// <summary>
            /// Gets the <paramref name="char"/> that an instance of this class represents.
            /// </summary>
            public char Char
            {
                get
                {
                    return c;
                }
            }

            /// <summary>
            /// Gets the <see cref="Font"/> in which the character will be drawn.
            /// </summary>
            public Font Font
            {
                get
                {
                    return font;
                }
            }

            /// <summary>
            /// Gets the <see cref="SizeF"/> that is the dimensions of the character.
            /// </summary>
            public SizeF Size
            {
                get
                {
                    return size;
                }
            }

            /// <summary>
            /// Gets the <see cref="Color"/> in which the character will be drawn.
            /// </summary>
            public Color Color
            {
                get
                {
                    return color;
                }
            }
            #endregion

            #region Private / Protected
            private readonly char c;
            private readonly Font font;
            private readonly SizeF size;
            private readonly Color color;
            #endregion

            #region Constructors
            /// <summary>
            /// Constructs a new instance of the <see cref="CharRenderInfo" /> class.
            /// </summary>
            /// <param name="c">The <paramref name="char"/> that the new instance represents.</param>
            /// <param name="font">The <see cref="Font"/> in which the character will be drawn.</param>
            /// <param name="size">The <see cref="SizeF"/> that is the dimensions of the character.</param>
            /// <param name="color">The <see cref="Color"/> in which the character will be drawn.</param>
            public CharRenderInfo(char c, Font font, SizeF size, Color color)
            {
                this.c = c;
                this.font = font;
                this.size = size;
                this.color = color;
            }
            #endregion
        }
        #endregion
    }
}
