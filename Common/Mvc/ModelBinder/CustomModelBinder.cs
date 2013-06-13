

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tunynet.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.Reflection;
using System.Collections;

namespace Spacebuilder.Common
{
    public class CustomModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = base.BindModel(controllerContext, bindingContext);

            if (value == null)
                return value;
            if (controllerContext.RouteData.Values.ContainsKey(bindingContext.ModelName))
                return value;

            string[] tempArray = null;

            if (bindingContext.ModelType.FullName.Contains("System.String") && value is Array)
            {
                tempArray = (string[])value;
            }

            //内容过滤
            if ((tempArray != null && tempArray.Length > 0) || value is string && !string.IsNullOrEmpty(value as string))
            {
                if (controllerContext.Controller.ValueProvider.ContainsPrefix(bindingContext.ModelName) || bindingContext.ModelMetadata.ContainerType != null)
                {
                    //处理敏感词
                    WordFilterStatus status = WordFilterStatus.Banned;
                    if (tempArray != null && tempArray.Length > 0)
                    {
                        for (int i = 0; i < tempArray.Length; i++)
                        {

                            tempArray[i] = WordFilter.SensitiveWordFilter.Filter(tempArray[i], out status);
                            if (status == WordFilterStatus.Banned)
                            {
                                bindingContext.ModelState.AddModelError("SensitiveWord", "内容未通过验证或包含非法词语！");
                                break;
                            }
                        }

                        return tempArray;
                    }

                    string tempValue = (value as string).Trim();
                    Type type = bindingContext.ModelMetadata.ContainerType;
                    PropertyInfo propertyInfo = null;
                    if (type != null)
                    {
                        propertyInfo = type.GetProperty(bindingContext.ModelName);
                    }

                    var noFilterWordAttribute = propertyInfo != null ? Attribute.GetCustomAttribute(propertyInfo, typeof(NoFilterWordAttribute)) as NoFilterWordAttribute : null;
                    if (noFilterWordAttribute == null)
                    {
                        tempValue = WordFilter.SensitiveWordFilter.Filter(tempValue, out status);
                        if (status == WordFilterStatus.Banned)
                        {
                            bindingContext.ModelState.AddModelError("SensitiveWord", "内容未通过验证或包含非法词语！");
                            return tempValue;
                        }
                    }

                    if (propertyInfo != null)
                    {
                        var dataTypeAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(DataTypeAttribute)) as DataTypeAttribute;
                        if (dataTypeAttribute != null)
                        {
                            if (dataTypeAttribute.DataType == DataType.MultilineText)
                            {
                                //处理多行纯文本
                                tempValue = Formatter.FormatMultiLinePlainTextForStorage(tempValue, false);
                            }
                            else if (dataTypeAttribute.DataType == DataType.Html)
                            {
                                //处理html标签
                                tempValue = HtmlUtility.CleanHtml(tempValue, TrustedHtmlLevel.HtmlEditor);
                            }
                        }
                    }

                    return tempValue;
                }

            }

            return value;
        }
    }
}
