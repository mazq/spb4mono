//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展枚举下拉列表
    /// </summary>
    public static class HtmlHelperEnumDropDownListExtensions
    {
        /// <summary>
        /// 显示列表项是枚举类型的下拉列表
        /// </summary>
        /// <remarks>
        /// 支持可空枚举类型，通过在枚举项上加[Display(Name="")]的形式来为下拉列表添加列表项名称
        /// </remarks>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="optionLabel">空值对应的列表项名称（例如：“请选择”）</param>
        /// <param name="htmlAttributes">下拉列表Html属性集合</param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel = null, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            IEnumerable<SelectListItem> items = GetSelectListItems<TEnum>(metadata.Model, optionLabel);
            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        /// <summary>
        /// 显示列表项是枚举类型的下拉列表
        /// </summary>
        /// <remarks>
        /// 支持可空枚举类型，通过在枚举项上加[Display(Name="")]的形式来为下拉列表添加列表项名称
        /// </remarks>
        /// <param name="htmlHelper">被扩展对象</param>
        /// <param name="name">控件名</param>
        /// <param name="selectValue">被选中项的值</param>
        /// <param name="optionLabel">空值对应的列表项名称（例如：“请选择”）</param>
        /// <param name="htmlAttributes">下拉列表Html属性集合</param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum selectValue, string optionLabel = null, object htmlAttributes = null)
        {

            IEnumerable<SelectListItem> items = GetSelectListItems<TEnum>(selectValue, optionLabel);
            return htmlHelper.DropDownList(name, items, htmlAttributes);
        }

        #region Help methods
        /// <summary>
        /// 获取列表项
        /// </summary>
        internal static IEnumerable<SelectListItem> GetSelectListItems<TEnum>(object selectValue, string optionLabel)
        {
            Type enumType = typeof(TEnum);
            Type underlyingType = Nullable.GetUnderlyingType(enumType);
            if (underlyingType != null)
            {
                enumType = underlyingType;
            }
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = GetEnumDescription(value),
                                                    Value = value.ToString(),
                                                    Selected = value.Equals(selectValue)
                                                };

            if (!string.IsNullOrEmpty(optionLabel))
                items = (new List<SelectListItem> { new SelectListItem { Text = optionLabel, Value = string.Empty } }).Concat(items);
            return items;
        }

        private static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attribute = fi.GetCustomAttributes(
                  typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                   .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                   .FirstOrDefault();
            if (attribute != null)
                return attribute.Name;
            return value.ToString();
        }

        #endregion
    }
}
