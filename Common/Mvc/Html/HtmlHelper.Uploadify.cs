//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet.Common.Configuration;
using Tunynet.Utilities;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 封装Uploadify插件
    /// </summary>
    public static class HtmlHelperUploadifyExtensions
    {
        /// <summary>
        /// Uploadify的Helper方法用于附件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">名称</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="allowedFileExtensions">允许的文件类型
        /// 格式为（jpg,jpeg,gif）</param>
        /// <param name="buttonOptions">指定按钮属性的类</param>
        /// <param name="uploadFileOptions">指定上传配置类</param>
        /// <returns></returns>
        public static MvcHtmlString Uploadify(this HtmlHelper htmlHelper, string name, string tenantTypeId, string allowedFileExtensions = "", ButtonOptions buttonOptions = null, UploadFileOptions uploadFileOptions = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(tenantTypeId))
            {
                throw new ExceptionFacade("参数不能为空");
            }
            TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);
            if (tenantAttachmentSettings == null)
            {
                throw new ExceptionFacade("找不到附件配置");
            }
            if (uploadFileOptions != null && string.IsNullOrEmpty(uploadFileOptions.UploaderUrl))
            {
                uploadFileOptions.MergeUploadifyFormData("tenantTypeId", tenantTypeId);
                string fileobjName = string.IsNullOrEmpty(uploadFileOptions.FileObjName) ? "Filedata" : uploadFileOptions.FileObjName;
                uploadFileOptions.MergeUploadifyFormData("requestName", fileobjName);
                uploadFileOptions.MergeUploadifyFormData("associateId", uploadFileOptions.AssociateId);
            }
            if (uploadFileOptions == null)
            {
                uploadFileOptions = new UploadFileOptions();
                uploadFileOptions.MergeUploadifyFormData("tenantTypeId", tenantTypeId);
                uploadFileOptions.MergeUploadifyFormData("requestName", "Filedata");
                uploadFileOptions.MergeUploadifyFormData("associateId", 0);
            }
            if (string.IsNullOrEmpty(allowedFileExtensions))
            {
                return Uploadify(htmlHelper, name, tenantAttachmentSettings.AllowedFileExtensions, tenantAttachmentSettings.MaxAttachmentLength, uploadFileOptions, buttonOptions);
            }
            else
            {
                return Uploadify(htmlHelper, name, allowedFileExtensions, tenantAttachmentSettings.MaxAttachmentLength, uploadFileOptions, buttonOptions);
            }
        }

        /// <summary>
        /// Uploadify的Helper方法用于非附件上传
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">名称</param>
        /// <param name="allowedFileExtensions">允许的文件类型
        ///  格式为(jpg,txt,doc)</param>
        ///  <param name="fileSizeLimit">允许上传文件大小(单位KB)</param>
        /// <param name="uploadFileOptions">指定上传配置类</param>
        /// <param name="buttonOptions">指定按钮属性的类</param>
        /// <returns></returns>
        public static MvcHtmlString Uploadify(this HtmlHelper htmlHelper, string name, string allowedFileExtensions, int fileSizeLimit, UploadFileOptions uploadFileOptions, ButtonOptions buttonOptions = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(allowedFileExtensions) || fileSizeLimit <= 0 || uploadFileOptions == null)
            {
                throw new ExceptionFacade("参数不能为空");
            }
            if (fileSizeLimit <= 0)
            {
                throw new ExceptionFacade("参数不能小于等于0");
            }

            //data属性字典
            Dictionary<string, object> data = new Dictionary<string, object>();
            //定义属性字典
            Dictionary<string, object> result = new Dictionary<string, object>();
            //参数
            Dictionary<string, object> formData = new Dictionary<string, object>();
            //文件流标识名
            string fileobjName = "Filedata";

            #region uploadify设置

            if (uploadFileOptions != null)
            {
                if (uploadFileOptions.AdditionalCallBacks != null)
                {
                    result = new Dictionary<string, object>(uploadFileOptions.AdditionalCallBacks);
                }

                //定义上传文件标示名称
                if (!string.IsNullOrEmpty(uploadFileOptions.FileObjName))
                {
                    result.TryAdd("fileObjName", uploadFileOptions.FileObjName);
                    fileobjName = uploadFileOptions.FileObjName;
                }
                //设置是否自动上传
                if (uploadFileOptions.IsAuto != null)
                {
                    result.TryAdd("auto", uploadFileOptions.IsAuto);
                }
                //设置可多选文件属性
                if (uploadFileOptions.IsMultiple != null)
                {
                    result.TryAdd("multi", uploadFileOptions.IsMultiple);
                }
                //设置都否缓存SWF文件
                if (uploadFileOptions.PreventCaching != null)
                {
                    result.TryAdd("preventCaching", uploadFileOptions.PreventCaching);
                }
                //设置显示进度的样式
                if (!string.IsNullOrEmpty(uploadFileOptions.ProgressData))
                {
                    result.TryAdd("progressData", uploadFileOptions.ProgressData);
                }
                //设置队列展示容器的ID
                if (!string.IsNullOrEmpty(uploadFileOptions.QueueId))
                {
                    result.TryAdd("queueID", uploadFileOptions.QueueId);
                }

                //设置队列可允许的个数
                result.TryAdd("queueSizeLimit", uploadFileOptions.QueueSizeLimit);

                //设置完成后是否移除属性
                if (uploadFileOptions.RemoveCompleted != null)
                {
                    result.TryAdd("removeCompleted", uploadFileOptions.RemoveCompleted);
                }
                //设置移除的延时属性
                if (uploadFileOptions.RemoveTimeout > 0)
                {
                    result.TryAdd("removeTimeout", uploadFileOptions.RemoveTimeout);
                }
                //设置于服务器端的成功上传的延时属性
                if (uploadFileOptions.SuccessTimeout > 0)
                {
                    result.TryAdd("successTimeout", uploadFileOptions.SuccessTimeout);
                }

                //设置可上传的文件数量限制
                result.TryAdd("uploadLimit", uploadFileOptions.UploadLimit);

                if (uploadFileOptions.AdditionalFormDatas != null)
                {
                    formData = new Dictionary<string, object>(uploadFileOptions.AdditionalFormDatas);
                }
                else
                {
                    formData.TryAdd("requestName", fileobjName);
                }

                result.TryAdd("formData", formData);

                //URL
                long userId = UserContext.CurrentUser != null ? UserContext.CurrentUser.UserId : 0;

                string uploadUrl = SiteUrls.Instance().UploadFile(userId);
                if (!string.IsNullOrEmpty(uploadFileOptions.UploaderUrl))
                {
                    uploadUrl = uploadFileOptions.UploaderUrl + ((uploadFileOptions.UploaderUrl.Contains("?") ? "&" : "?") + "CurrentUserIdToken=" + Utility.EncryptTokenForUploadfile(0.1, userId));
                }

                result.TryAdd("uploader", uploadUrl);
            }

            //设置可允许上传的类型 属性
            string fileTypeExts = FileTypeExts(allowedFileExtensions);
            if (string.IsNullOrEmpty(fileTypeExts))
                fileTypeExts = uploadFileOptions.FileTypeExts;
            if (string.IsNullOrEmpty(fileTypeExts))
                fileTypeExts = "*.*";
            result.TryAdd("fileTypeExts", fileTypeExts);

            //设置文件限制大小属性
            if (fileSizeLimit <= 0)
                fileSizeLimit = uploadFileOptions.FileSizeLimit;
            result.TryAdd("fileSizeLimit", fileSizeLimit);

            if (!string.IsNullOrEmpty(uploadFileOptions.FileTypeDescription))
                result.TryAdd("fileTypeDesc", uploadFileOptions.FileTypeDescription);

            //设置SWF路径
            string swfStr = SiteUrls.FullUrl("~/Scripts/jquery/uploadify/uploadify.swf");
            result.TryAdd("swf", swfStr);

            #endregion uploadify设置

            if (buttonOptions != null)
            {
                #region ButtonOptions设置

                //添加按钮的额外样式
                if (!string.IsNullOrEmpty(buttonOptions.CssClass))
                {
                    result.TryAdd("buttonClass", buttonOptions.CssClass);
                }
                //设置按钮的高度
                if (buttonOptions.Height != 0)
                {
                    result.TryAdd("height", buttonOptions.Height);
                }
                //设置按钮的宽度
                if (buttonOptions.Width != 0)
                {
                    result.TryAdd("width", buttonOptions.Width);
                }
                //设置按钮的显示背景图
                if (!string.IsNullOrEmpty(buttonOptions.ImageUrl))
                {
                    result.TryAdd("buttonImage", buttonOptions.ImageUrl);
                }
                //设置按钮的文本
                if (!string.IsNullOrEmpty(buttonOptions.Text))
                {
                    result.TryAdd("buttonText", buttonOptions.Text);
                }

                #endregion ButtonOptions设置
            }

            TagBuilder builder = new TagBuilder("div");
            //脚本操作标识
            data["plugin"] = "uploadify";
            data.TryAdd("data", Json.Encode(result));
            builder.MergeAttributes(data);
            builder.MergeAttribute("id", name);

            return MvcHtmlString.Create(builder.ToString().Replace("&quot;[", "").Replace("]&quot;", ""));
        }

        /// <summary>
        /// 整理允许上传的文件类型
        /// </summary>
        /// <param name="allowedFileExtensions">从配置文件提取的文件类型字符串</param>
        /// <returns> 整理好的允许上传的文件类型</returns>
        private static string FileTypeExts(string allowedFileExtensions)
        {
            StringBuilder str = new StringBuilder();
            //默认取配置项里的设置
            string[] exts = allowedFileExtensions.Split(',');

            foreach (var ext in exts)
            {
                str.Append("*." + ext + ";");
            }
            return str.ToString().TrimEnd(';');
        }
    }
}