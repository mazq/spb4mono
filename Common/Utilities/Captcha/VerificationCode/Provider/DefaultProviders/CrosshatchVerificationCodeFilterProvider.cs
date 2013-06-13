using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 干扰图像提供者
    /// </summary>
    public sealed class CrosshatchVerificationCodeFilterProvider : VerificationCodeFilterProvider
    {
        private readonly List<Color> colors;
        private float opacity;
        private bool randomStyle;
        private HatchStyle style;

        public CrosshatchVerificationCodeFilterProvider()
        {
            colors = new List<Color>(8);
        }

        public CrosshatchVerificationCodeFilterProvider(params Color[] colors)
        {
            if (colors != null && colors.Length > 0)
                this.colors = new List<Color>(colors);
            else
                this.colors = new List<Color>(8);
        }


        #region Public Properties
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

        /// <summary>
        /// 透明度
        /// </summary>
        public float Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("value", value, "Resources.Errors.ValueBetween0And1Inclusive");

                opacity = value;
            }
        }

        /// <summary>
        /// 随即样式
        /// </summary>
        public bool RandomStyle
        {
            get
            {
                return randomStyle;
            }
            set
            {
                randomStyle = value;
            }
        }

        public HatchStyle Style
        {
            get
            {
                return style;
            }
            set
            {
                if (!Enum.IsDefined(typeof(HatchStyle), value))
                    throw new System.ComponentModel.InvalidEnumArgumentException("value", (int)value, typeof(HatchStyle));

                style = value;
            }
        }

        public override bool CanPostProcess
        {
            get { return true; }
        }

        public override bool CanPreProcess
        {
            get { return false; }
        }
        #endregion

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
                name = "CrosshatchAutoInputProtectionFilterProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Crosshatch Auto-Input Protection Filter Provider");
            }

            base.Initialize(name, config);

            VerificationCodeProviderHelper helper = new VerificationCodeProviderHelper(config);

            opacity = helper.ParseSingle("opacity", false, .15F);
            randomStyle = helper.ParseBoolean("randomStyle", false, true);
            style = helper.ParseEnum("style", false, HatchStyle.DiagonalCross);
            colors.AddRange(helper.ParseCollection<Color>("colors", false, true, false, ','));

            if (colors.Count == 0)
                colors.Add(Color.Black);

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);

                if (!String.IsNullOrEmpty(attr))
                    throw new Exception(string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                        "Resources.Errors.UnrecognizedAttribute", attr));
            }
        }

        public override void PreProcess(Graphics graphics, Size imageSize)
        {
            throw new NotSupportedException();
        }

        public override void PostProcess(Graphics graphics, Size imageSize)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");

            if (colors.Count == 0)
                throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                    "Resources.Errors.NoColorsForFilter", Name));

            Random rnd = new Random();

            Color color = colors[rnd.Next(0, colors.Count)];

            int op = (int)(opacity * 255);

            if (randomStyle)
            {
                int value;
                do
                {
                    value = rnd.Next(0, 53);
                }
                while (!Enum.IsDefined(typeof(HatchStyle), value));

                style = (HatchStyle)value;
            }

            using (HatchBrush brush = new HatchBrush(style, Color.FromArgb(Math.Min(op, 255), color.R, color.G, color.B), Color.Transparent))
            {
                graphics.FillRectangle(brush, 0, 0, imageSize.Width, imageSize.Height);
            }
        }
        #endregion
    }
}
