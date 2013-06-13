//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Spacebuilder.Common;
using Tunynet.Common;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展HtmlHelper
    /// </summary>
    public static class HtmlHelperSetTitleImageExtensions
    {
        /// <summary>
        /// 输出设置标题图的标签（单选）
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="expression">选择实体中类别属性的lamda表达式</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id （默认为0）</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString SetTitleImageFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, long>> expression, string tenantTypeId, long associateId = 0)
        {
            if (string.IsNullOrEmpty(tenantTypeId))
            {
                throw new ExceptionFacade("租户类型Id不能为空");
            }

            string htmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            long attachmentId = (long)metadata.Model;

            List<Attachment> attachments = new List<Attachment>();
            if (attachmentId > 0)
            {
                var attachment = attachementService.Get(attachmentId);
                if (attachment != null)
                {
                    attachments.Add(attachment);
                }
            }

            return htmlHelper.EditorForModel("SetTitleImage", new { tenantTypeId = tenantTypeId, associateId = associateId, htmlFieldName = htmlFieldName, attachments = attachments, isMultiSelect = false });
        }

        /// <summary>
        /// 输出设置标题图的标签（多选）
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="expression">选择实体中类别属性的lamda表达式</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id （默认为0）</param>
        /// <param name="isMultiSelect"></param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString SetTitleImageFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, string tenantTypeId, long associateId = 0, int maxSelect = 5)
        {
            if (string.IsNullOrEmpty(tenantTypeId))
            {
                throw new ExceptionFacade("租户类型Id不能为空");
            }


            string htmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string featuredImageIds = (string)metadata.Model;

            List<Attachment> attachments = new List<Attachment>();
            if (!string.IsNullOrEmpty(featuredImageIds))
            {
                string[] attachmentIds = featuredImageIds.TrimEnd(',').Split(',');
                foreach (var attachmentId in attachmentIds)
                {
                    var attachment = attachementService.Get(long.Parse(attachmentId));
                    if (attachment != null)
                    {
                        attachments.Add(attachment);
                    }
                }
            }

            return htmlHelper.EditorForModel("SetTitleImage", new
            {
                tenantTypeId = tenantTypeId,
                associateId = associateId,
                htmlFieldName = htmlFieldName,
                attachments = attachments,
                isMultiSelect = true,
                maxSelect = maxSelect
            });
        }
    }
}