//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 多文件上传Button属性设置
    /// </summary>
    public class ButtonOptions
    {
        /// <summary>
        /// 用于创建新实体时使用
        /// </summary>
        public static ButtonOptions New()
        {
            ButtonOptions buttonOptions = new ButtonOptions()
            {
                CssClass = string.Empty,
                Height = 0,
                ImageUrl = string.Empty,
                Text = string.Empty,
                Width = 0
            };
            return buttonOptions;
        }

        /// <summary>
        /// 按钮的样式
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// 按钮的背景图路径
        /// </summary>
        /// <remarks>图片URL的全路径</remarks>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 按钮显示文字
        /// </summary>
        /// <remarks>默认为"SELECT FILES"</remarks>
        public string Text { get; set; }

        /// <summary>
        /// 按钮的宽
        /// </summary>
        /// <remarks>单位px,默认宽度值为120</remarks>
        public int Width { get; set; }

        /// <summary>
        /// 按钮的高(单位px 默认高度值为30  )
        /// </summary>
        public int Height { get; set; }

        #region 连缀方法

        /// <summary>
        /// 设置按钮的样式
        /// </summary>
        /// <param name="cssClass">样式</param>
        /// <returns></returns>
        public ButtonOptions SetCssClass(string cssClass)
        {
            this.CssClass = cssClass;
            return this;
        }

        /// <summary>
        /// 设置按钮的背景图片URL
        /// </summary>
        /// <param name="imageUrl">背景图片URL</param>
        /// <returns></returns>
        public ButtonOptions SetImageUrl(string imageUrl)
        {
            this.ImageUrl = imageUrl;
            return this;
        }

        /// <summary>
        /// 设置按钮的文字 (默认为"SELECT FILES")
        /// </summary>
        /// <param name="text">文字</param>
        public ButtonOptions SetText(string text)
        {
            this.Text = text;
            return this;
        }

        /// <summary>
        /// 设置按钮的宽  (单位px 默认宽度值为120  )
        /// </summary>
        /// <param name="width">宽</param>
        public ButtonOptions SetWidth(int width)
        {
            this.Width = width;
            return this;
        }

        /// <summary>
        /// 设置按钮的高 (单位px 默认高度值为30  )
        /// </summary>
        /// <param name="height">高</param>
        public ButtonOptions SetHeight(int height)
        {
            this.Height = height;
            return this;
        }

        #endregion 连缀方法
    }
}