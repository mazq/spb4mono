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
    /// 多文件上传设置
    /// </summary>
    public class UploadFileOptions
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns></returns>
        public static UploadFileOptions New()
        {
            UploadFileOptions uploadFileOptions = new UploadFileOptions()
            {
                AdditionalCallBacks = null,
                AdditionalFormDatas = null,
                FileObjName = string.Empty,
                IsAuto = null,
                IsMultiple = null,
                PreventCaching = null,
                ProgressData = string.Empty,
                QueueId = string.Empty,
                QueueSizeLimit = 0,
                RemoveCompleted = null,
                RemoveTimeout = 0,
                SuccessTimeout = 0,
                UploaderUrl = string.Empty,
                UploadLimit = 0,
                AssociateId = 0,
                FileSizeLimit = 0,
                FileTypeDescription = string.Empty,
                FileTypeExts = string.Empty
            };
            return uploadFileOptions;
        }

        /// <summary>
        /// 附件关联Id
        /// </summary>
        public long AssociateId { get; set; }

        /// <summary>
        ///  是否自动上传
        /// </summary>
        ///  <remarks> 默认为为true</remarks>
        public bool? IsAuto { get; set; }

        /// <summary>
        /// 是否允许选择多个文件
        /// </summary>
        /// <remarks>默认为true</remarks>
        public bool? IsMultiple { get; set; }

        /// <summary>
        /// 禁止客户端缓存多文件上传组件插件
        /// </summary>
        /// <remarks>
        /// true-禁止,false-允许
        /// 默认禁止
        /// </remarks>
        public bool? PreventCaching { get; set; }

        /// <summary>
        /// 上传进度的相关信息
        /// </summary>
        /// <remarks>
        /// "percentage"：显示当前上传进度的百分比
        /// "speed"： 显示文件上传的速度
        /// 默认为"percentage"
        /// </remarks>
        public string ProgressData { get; set; }

        /// <summary>
        /// 文件上传列表的外层Html标签Id
        /// </summary>
        /// <remarks>如果为空则会自动添加对应外层标签</remarks>
        public string QueueId { get; set; }

        /// <summary>
        /// 最多添加到上传队列的文件数
        /// </summary>
        /// <remarks>默认999个</remarks>
        public float QueueSizeLimit { set; get; }

        /// <summary>
        /// 上传完成后是否从自动已上传文件列表中移除
        /// </summary>
        /// <remarks>
        /// true-移除,false-不移除
        /// 默认为移除
        /// </remarks>
        public bool? RemoveCompleted { get; set; }

        /// <summary>
        /// 完成后移除的延时设置
        /// </summary>
        /// <remarks>单位:秒,默认为3秒</remarks>
        public float RemoveTimeout { get; set; }

        /// <summary>
        /// 设置上传成功后的等待服务器端响应的超时时间
        /// </summary>
        /// <remarks>单位:秒,默认为30秒</remarks>
        public float SuccessTimeout { get; set; }

        /// <summary>
        /// 设置文件的上传数量的上限
        /// </summary>
        /// <remarks>默认为10</remarks>
        public float UploadLimit { get; set; }

        /// <summary>
        ///uploadify额外的表单参数
        /// </summary>
        public Dictionary<string, object> AdditionalFormDatas { get; set; }

        /// <summary>
        /// uploadify额外的回调事件
        /// </summary>
        public Dictionary<string, object> AdditionalCallBacks { get; set; }

        /// <summary>
        /// 设置请求中文件流的标识默认为"Filedata"
        /// </summary>
        public string FileObjName { get; set; }

        /// <summary>
        /// 设置允许选择文件的大小限制(单位为KB）
        /// </summary>
        public int FileSizeLimit { get; set; }

        /// <summary>
        /// 设置文件类型描述信息（默认为“All Files”）
        /// </summary>
        public string FileTypeDescription { get; set; }

        /// <summary>
        /// 设置允许选择的文件类型
        /// </summary>
        public string FileTypeExts { get; set; }

        /// <summary>
        /// 设置用于上传的URL路径 默认为(~/Home/UploadFile.aspx)
        /// </summary>
        public string UploaderUrl { get; set; }

        #region 连缀方法

        /// <summary>
        /// 设置附件关联Id
        /// </summary>
        /// <param name="associateId">附件关联Id</param>
        public UploadFileOptions SetAssociateId(long associateId)
        {
            this.AssociateId = associateId;
            return this;
        }

        /// <summary>
        /// 设置文件上传URL 默认为(~/Home/UploadFile.aspx)
        /// </summary>
        /// <param name="uploaderUrl">URL</param>
        public UploadFileOptions SetUploaderUrl(string uploaderUrl)
        {
            this.UploaderUrl = uploaderUrl;
            return this;
        }

        /// <summary>
        /// 设置服务器端上传标识name   默认为"Filedata"
        /// </summary>
        /// <param name="fileObjName"></param>
        /// <returns></returns>
        public UploadFileOptions SetFileObjName(string fileObjName)
        {
            this.FileObjName = fileObjName;
            return this;
        }

        /// <summary>
        /// 设置启用可多选文件（默认为允许）
        /// </summary>
        /// <param name="isMultiple">是否启用</param>
        public UploadFileOptions SetMultiple(bool isMultiple)
        {
            this.IsMultiple = isMultiple;
            return this;
        }

        /// <summary>
        /// 设置禁止客户端缓存多文件上传组件插件
        /// </summary>
        /// <remarks>
        /// true-禁止,false-允许
        /// 默认禁止
        /// </remarks>
        /// <param name="preventCaching">是否禁止</param>
        public UploadFileOptions SetPreventCaching(bool preventCaching)
        {
            this.PreventCaching = preventCaching;
            return this;
        }

        /// <summary>
        /// 设置上传进度的相关信息
        /// </summary>
        /// <remarks>
        /// "percentage"：显示当前上传进度的百分比
        /// "speed"： 显示文件上传的速度
        /// 默认为"percentage"
        /// </remarks>
        /// <param name="progressData">样式名称</param>
        /// <returns></returns>
        public UploadFileOptions SetProgressData(string progressData)
        {
            this.ProgressData = progressData;
            return this;
        }

        /// <summary>
        /// 设置文件上传列表的外层Html标签Id
        /// </summary>
        /// <remarks>如果为空则会自动添加对应外层标签</remarks>
        /// <param name="queueId">容器ID</param>
        public UploadFileOptions SetQueueId(string queueId)
        {
            this.QueueId = queueId;
            return this;
        }

        /// <summary>
        /// 设置最多添加到上传队列的文件数
        /// </summary>
        /// <remarks>默认999个</remarks>
        /// <param name="queueSizeLimit">个数</param>
        public UploadFileOptions SetQueueSizeLimit(float queueSizeLimit)
        {
            this.QueueSizeLimit = queueSizeLimit;
            return this;
        }

        /// <summary>
        /// 设置上传完成后是否从自动已上传文件列表中移除
        /// </summary>
        /// <remarks>
        /// true-移除,false-不移除
        /// 默认为移除
        /// </remarks>
        /// <param name="removeCompleted">是否完成</param>
        public UploadFileOptions SetRemoveCompleted(bool removeCompleted)
        {
            this.RemoveCompleted = removeCompleted;
            return this;
        }

        /// <summary>
        /// 完成后移除的延时设置
        /// </summary>
        /// <remarks>单位:秒,默认为3秒</remarks>
        /// <param name="removeTimeout">延时时长</param>
        public UploadFileOptions SetRemoveTimeout(float removeTimeout)
        {
            this.RemoveTimeout = removeTimeout;
            return this;
        }

        /// <summary>
        ///  设置是否为自动上传  默认为true
        /// </summary>
        /// <param name="isAuto">是否自动</param>
        /// <returns></returns>
        public UploadFileOptions SetAutoUpload(bool isAuto)
        {
            this.IsAuto = isAuto;
            return this;
        }

        /// <summary>
        /// 设置上传成功后的等待服务器端响应的超时时间
        /// </summary>
        /// <remarks>单位:秒,默认为30秒</remarks>
        /// <param name="successTimeout">响应时长</param>
        public UploadFileOptions SetSuccessTimeout(float successTimeout)
        {
            this.SuccessTimeout = successTimeout;
            return this;
        }

        /// <summary>
        /// 设置上传文件数量的限制  默认为999
        /// </summary>
        /// <param name="uploadLimit">数量</param>
        public UploadFileOptions SetUploadLimit(float uploadLimit)
        {
            this.UploadLimit = uploadLimit;
            return this;
        }

        /// <summary>
        /// 设置允许上传文件的大小（单位为KB）
        /// </summary>
        /// <param name="fileSizeLimit">大小</param>
        public UploadFileOptions SetFileSizeLimit(int fileSizeLimit)
        {
            this.FileSizeLimit = fileSizeLimit;
            return this;
        }

        /// <summary>
        /// 设置文件类型描述信息（默认为“All Files”）
        /// </summary>
        /// <param name="fileTypeDescription">类型描述信息</param>
        public UploadFileOptions SetFileTypeDescription(string fileTypeDescription)
        {
            this.FileTypeDescription = fileTypeDescription;
            return this;
        }

        /// <summary>
        /// 设置允许选择的文件类型（默认为"*.*"，多种类型请用英文分号隔开，如："*.gif; *.jpg; *.png"）
        /// </summary>
        /// <param name="fileTypeExts">文件类型</param>
        public UploadFileOptions SetFileTypeExts(string fileTypeExts)
        {
            this.FileTypeExts = fileTypeExts;
            return this;
        }

        /// <summary>
        /// 添加额外的uploadify的回调函数
        /// </summary>
        /// <param name="callbackname">函数名称</param>
        /// <param name="functionname">function函数名</param>
        public UploadFileOptions MergeUploadifyCallBack(string callbackname, object functionname)
        {
            if (this.AdditionalCallBacks == null)
                this.AdditionalCallBacks = new Dictionary<string, object>();
            this.AdditionalCallBacks[callbackname] = "[" + functionname + "]";
            return this;
        }

        /// <summary>
        /// 添加额外的表单数据
        /// </summary>
        /// <param name="formDataKey">表单数据Key</param>
        /// <param name="formDataValue">表单数据Value</param>
        public UploadFileOptions MergeUploadifyFormData(string formDataKey, object formDataValue)
        {
            if (this.AdditionalFormDatas == null)
                this.AdditionalFormDatas = new Dictionary<string, object>();
            this.AdditionalFormDatas[formDataKey] = formDataValue;
            return this;
        }

        #endregion 连缀方法
    }
}