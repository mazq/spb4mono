//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using Tunynet.Common;
using Tunynet.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 验证布尔类型的表单项必须勾选
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class IsTrueAttribute : ValidationAttribute, IClientValidatable
    {
        #region Overrides of ValidationAttribute

        /// <summary>
        /// Determines whether the specified value of the object is valid. 
        /// </summary>
        /// <returns>
        /// true if the specified value is valid; otherwise, false. 
        /// </returns>
        /// <param name="value">The value of the specified validation object on which the <see cref="T:System.ComponentModel.DataAnnotations.ValidationAttribute"/> is declared.
        ///                 </param>
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");

            return (bool)value == true;
        }

        #endregion

        /// <summary>
        /// 获取客户端验证规则
        /// </summary>
        /// <param name="metadata">Model元数据</param>
        /// <param name="context">控制器上下文</param>
        /// <returns>客户端验证规则集合</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]
                       {
                           new ModelClientValidationRule{
                             ErrorMessage = this.ErrorMessage,
                             ValidationType = "istrue"                              
                            }
                       };
        }

    }

}