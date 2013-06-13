using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tunynet.UI;

namespace Spacebuilder.Common
{
    //快捷操作编辑实体
    public class OperationEditModel
    {
        /// <summary>
        /// 导航Id
        /// </summary>
        [Display(Name = "操作Id")]
        [Required(ErrorMessage = "请输入操作Id")]
        [Remote("ValidateOperationId", "ControlPanelSettings", ErrorMessage = "此操作Id已存在,请重新输入")]
        [RegularExpression("[0-9]{8}", ErrorMessage = "操作Id必须输入8位数字")]
        public int OperationId { get; set; }

        [Display(Name = "应用Id")]
        public int ApplicationId { get; set; }

        [Display(Name = "关联导航Id")]
        public int? AssociatedNavigationId { get; set; }

        [Display(Name = "呈现区域")]
        public string PresentAreaKey { get; set; }

        [Display(Name = "操作类型u")]
        public ManagementOperationType OperationType { get; set; }

        /// <summary>
        /// 操作文字
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "此项为必填项")]
        public string OperationText { get; set; }

        /// <summary>
        /// 操作文字资源名称
        /// </summary>
        [Display(Name = "resourceKey")]
        [Required(ErrorMessage = "此项为必填项")]
        public string ResourceName { get; set; }

        [Display(Name = "链接")]        
        public string NavigationUrl { get; set; }

        [Display(Name = "路由名")]        
        public string UrlRouteName { get; set; }

        public bool IconNameOption { get; set; }

        [Display(Name = "内置图标名称")]
        public string IconName { get; set; }

        [Display(Name = "图片名称")]
        public string ImageName { get; set; }

        [Display(Name = "链接")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Display(Name = "新窗口")]
        public string NavigationTarget { get; set; }

        [Display(Name = "启用")]
        public bool IsEnabled { get; set; }

        [Display(Name = "仅拥有者可见")]
        public bool OnlyOwnerVisible { get; set; }

        [Display(Name = "锁定")]
        public bool IsLocked { get; set; }

        [Display(Name = "使用资源项")]
        public bool IsUseResourceItem { get; set; }

        [Display(Name = "使用路由")]
        public bool IsUseRoute { get; set; }

        [Display(Name = "完整链接")]
        public bool IsWholeLink { get; set; }

        /// <summary>
        /// 转换为ApplicationManagementOperation用于数据库存储
        /// </summary>
        /// <returns></returns>
        public ApplicationManagementOperation AsApplicationManagementOperation()
        {
            ApplicationManagementOperation operation = null;
            ManagementOperationService managementOperationService = new ManagementOperationService();
            if (managementOperationService.Get(this.OperationId) == null)
            {
                operation = new ApplicationManagementOperation();
                operation.OperationId = this.OperationId;
                
                operation.DisplayOrder = this.OperationId;
            }
            else
            {
                operation = managementOperationService.Get(this.OperationId);
            }
            if (this.AssociatedNavigationId.HasValue)
            operation.AssociatedNavigationId = this.AssociatedNavigationId.Value;
            operation.ApplicationId = this.ApplicationId;
            operation.PresentAreaKey = this.PresentAreaKey;

            operation.OperationType = this.OperationType;

            //使用资源项
            if (this.IsUseResourceItem)
            {
                operation.ResourceName = this.ResourceName;
                operation.OperationText = string.Empty;
            }
            else
            {
                operation.OperationText = this.OperationText;
                operation.ResourceName = string.Empty;
            }

            //使用路由
            if (this.IsUseRoute)
            {
                operation.UrlRouteName = this.UrlRouteName ?? string.Empty;
                operation.NavigationUrl = string.Empty;
            }
            else
            {
                operation.NavigationUrl = this.NavigationUrl ?? string.Empty;
                operation.UrlRouteName = string.Empty;
            }

            if (IconNameOption)
            {
                operation.IconName = this.IconName;
                operation.ImageUrl = null;
            }
            else
            {
                operation.IconName = null;
                if (this.IsWholeLink)
                {
                    operation.ImageUrl = this.ImageUrl;
                }
                else
                {
                    if (this.ImageName != null)
                    { 
                        operation.ImageUrl = "~/Uploads/NavigationImage/" + this.ImageName;
                    }
                    else
                    {
                        operation.ImageUrl = null;
                    }
                }
            }

            operation.NavigationTarget = this.NavigationTarget;
            
            
            operation.OnlyOwnerVisible = this.OnlyOwnerVisible;
            operation.IsEnabled = this.IsEnabled;
           
            operation.IsLocked = this.IsLocked;
            return operation;
        }
    }

    public static class ApplicationManagementOperationOneExtensions
    {
        public static OperationEditModel AsOperationEditModel(this ApplicationManagementOperation operation)
        {
            return new OperationEditModel
            {
                OperationId = operation.OperationId,
                AssociatedNavigationId = operation.AssociatedNavigationId,
                ApplicationId = operation.ApplicationId,
                PresentAreaKey = operation.PresentAreaKey,
                OperationType = operation.OperationType,
                OperationText = operation.OperationText,
                ResourceName = operation.ResourceName,
                IsUseResourceItem = string.IsNullOrEmpty(operation.OperationText.Trim()) ? true : false,
                NavigationUrl = operation.NavigationUrl,
                UrlRouteName = operation.UrlRouteName,
                IsUseRoute = string.IsNullOrEmpty(operation.NavigationUrl.Trim()) ? true : false,
                IconNameOption = string.IsNullOrEmpty(operation.IconName) ? false : true,
                IconName = string.IsNullOrEmpty(operation.IconName) ? string.Empty : operation.IconName,
                ImageUrl = (!string.IsNullOrEmpty(operation.ImageUrl)) && operation.ImageUrl.StartsWith("http://") ? operation.ImageUrl : string.Empty,
                ImageName = (!string.IsNullOrEmpty(operation.ImageUrl)) && !(operation.ImageUrl.StartsWith("http://")) ? operation.ImageUrl.Substring(operation.ImageUrl.LastIndexOf('/') + 1) : string.Empty,
                IsWholeLink = (!string.IsNullOrEmpty(operation.ImageUrl)) && operation.ImageUrl.StartsWith("http://") ? true : false,
                NavigationTarget = operation.NavigationTarget,
                OnlyOwnerVisible = operation.OnlyOwnerVisible,
                IsEnabled = operation.IsEnabled,
                IsLocked = operation.IsLocked
            };
        }
    }
}
