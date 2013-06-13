using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 干扰线生成提供者
    /// </summary>
    public sealed class LineNoiseVerificationCodeImage : PartitionedVerificationCodeImage
    {
        private readonly IList<Color> colors;

        public LineNoiseVerificationCodeImage(Size size, VerificationCodeTextProvider textProvider, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
            : base(size, textProvider, characterSet, maximumCharacters, minimumCharacters)
        {
            colors = new List<Color>(1);
            colors.Add(Color.Black);
        }

        public LineNoiseVerificationCodeImage(Size size, VerificationCodeTextProvider textProvider, IEnumerable<Color> colors, CaptchaCharacterSet characterSet, int? maximumCharacters, int? minimumCharacters)
            : base(size, textProvider, characterSet, maximumCharacters, minimumCharacters)
        {
            if (colors == null)
                this.colors = new List<Color>(1);
            else
                this.colors = new List<Color>(colors);

            if (this.colors.Count == 0)
                this.colors.Add(Color.Black);
        }

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
        /// 创建图像
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        protected override Image CreateSurfaceImage(Size size)
        {
            if (colors.Count == 0)
                throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                    "Resources.Errors.NoColorsForLineNoise"));

            Bitmap bitmap = new Bitmap(size.Width, size.Height);

            Random rnd = new Random();

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(Brushes.White, graphics.VisibleClipBounds);

                using (GraphicsPath path = new GraphicsPath())
                {
                    int width = size.Width - Margin;
                    int height = size.Height - Margin;
                    int minPosition = Margin + 5;
                    int n = 0;
                    Point end = Point.Empty;
                    bool hasDrawnPath = false;

                    using (Pen pen1 = new Pen(colors[rnd.Next(0, colors.Count)], 1))
                    {
                        using (Pen pen2 = new Pen(colors[rnd.Next(0, colors.Count)], 1.8f))
                        {
                            path.AddBezier(
                                new Point(rnd.Next(Margin, Math.Max(minPosition, width)),
                                    rnd.Next(Margin, Math.Max(minPosition, height))),
                                new Point(rnd.Next(Margin, Math.Max(minPosition, width)),
                                    rnd.Next(Margin, Math.Max(minPosition, height))),
                                new Point(rnd.Next(Margin, Math.Max(minPosition, width)),
                                    rnd.Next(Margin, Math.Max(minPosition, height))),
                                end = new Point(rnd.Next(Margin, Math.Max(minPosition, width)),
                                    rnd.Next(Margin, Math.Max(minPosition, height))));

                            n = rnd.Next(0, 11);

                            if (n < 6)
                            {
                                graphics.DrawPath((n > 2) ? pen1 : pen2, path);
                                path.Reset();
                                hasDrawnPath = true;
                            }

                            for (int i = 0; i < rnd.Next(0, 4); i++)
                            {
                                Point start = end;

                                end = new Point(rnd.Next(Margin, Math.Max(minPosition, width)),
                                    rnd.Next(Margin, Math.Max(minPosition, height)));

                                path.AddLine(start, end);

                                n = rnd.Next(0, 11);

                                if (n < 6)
                                {
                                    graphics.DrawPath((n > 2) ? pen1 : pen2, path);
                                    path.Reset();
                                    hasDrawnPath = true;
                                }
                            }

                            path.AddBezier(end,
                                new Point(rnd.Next(Margin, Math.Max(minPosition, width)),
                                    rnd.Next(Margin, Math.Max(minPosition, height))),
                                new Point(rnd.Next(Margin, Math.Max(minPosition, width)),
                                    rnd.Next(Margin, Math.Max(minPosition, height))),
                                new Point(rnd.Next(Margin, Math.Max(minPosition, width)),
                                    rnd.Next(Margin, Math.Max(minPosition, height))));

                            n = rnd.Next(0, 11);

                            if (!hasDrawnPath || n < 6)
                                graphics.DrawPath((n > 2) ? pen1 : pen2, path);
                        }
                    }
                }
            }

            return bitmap;
        }
        #endregion
    }
}
