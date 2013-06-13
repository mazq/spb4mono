//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using Tunynet;
using System;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户表单呈现的RoleEditModel实体
    /// </summary>
    public class RoleEditModel
    {
        /// <summary>
        ///角色名称
        /// </summary>
        [Display(Name = "角色名称")]
        [Required(ErrorMessage = "请填写角色名称")]
        [StringLength(24)]
        [RegularExpression(@"[^\u4e00-\u9fa5.-]*", ErrorMessage = "名称中包含不支持的字符")]
        public string RoleName { get; set; }

        /// <summary>
        ///所属应用
        /// </summary>
        [Required(ErrorMessage = "所属应用为必填选项")]
        [Display(Name = "所属应用")]
        public int ApplicationId { get; set; }


        private bool connectToUser = true;
        /// <summary>
        ///是否直接关联到用户
        /// </summary>
        [Display(Name = "允许关联用户")]
        public bool ConnectToUser
        {
            get
            {
                return connectToUser;
            }
            set
            {
                connectToUser = value;
            }
        }

        /// <summary>
        ///角色描述
        /// </summary>
        [Display(Name = "角色描述")]
        [Required(ErrorMessage = "请填写角色描述")]
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        ///角色友好名称
        /// </summary>
        [Display(Name = "角色友好名称")]
        [Required(ErrorMessage = "请填写角色友好名称")]
        [StringLength(24)]
        public string FriendlyRoleName { get; set; }

        /// <summary>
        ///角色标识图
        /// </summary>
        [Display(Name = "角色标识图")]
        public string RoleImage { get; set; }

        /// <summary>
        ///是否内置角色
        /// </summary>
        [Display(Name = "是否内置角色")]
        [Required(ErrorMessage = "是否内置角色为必填选项")]
        public bool IsBuiltIn { get; set; }

        private bool isEnabled = true;
        /// <summary>
        ///是否启用角色
        /// </summary>
        [Display(Name = "是否启用角色")]
        [Required(ErrorMessage = "是否启用角色为必填选项")]
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
            }
        }

        private bool isPublic = true;
        /// <summary>
        ///是否公开角色
        /// </summary>
        [Display(Name = "是否公开角色")]
        [Required(ErrorMessage = "是否公开角色为必填选项")]
        public bool IsPublic
        {
            get
            {
                return isPublic;
            }
            set
            {
                isPublic = value;
            }
        }

        /// <summary>
        /// 转换为Role用于数据库存储
        /// </summary>
        public Role AsRole()
        {
            RoleService roleService = DIContainer.Resolve<RoleService>();

            Role role = roleService.Get(RoleName);
            if (role != null)
            {
                role.FriendlyRoleName = this.FriendlyRoleName;
                role.ApplicationId = this.ApplicationId;
                role.Description = this.Description;
                role.IsEnabled = this.IsEnabled;
                role.IsPublic = this.IsPublic;
                role.ConnectToUser = this.ConnectToUser;
                role.RoleImage = this.RoleImage;
            }
            else
            {
                role = new Role();
                role.RoleName = this.RoleName;
                role.FriendlyRoleName = this.FriendlyRoleName;
                role.ApplicationId = this.ApplicationId;
                role.Description = this.Description;
                role.IsEnabled = this.IsEnabled;
                role.IsPublic = this.IsPublic;
                role.RoleImage = this.RoleName;
                role.ConnectToUser = this.ConnectToUser;
            }
            return role;
        }

    }

    /// <summary>
    /// Role扩展
    /// </summary>
    public static class RoleExtensions
    {
        /// <summary>
        /// 转换成RoleEditModel
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static RoleEditModel AsEditModel(this Role role)
        {
            return new RoleEditModel
            {
                RoleName = role.RoleName,
                FriendlyRoleName = role.FriendlyRoleName,
                ApplicationId = role.ApplicationId,
                Description = role.Description,
                IsEnabled = role.IsEnabled,
                IsPublic = role.IsPublic,
                RoleImage = role.RoleImage,
                ConnectToUser = role.ConnectToUser
            };
        }
    }
}