//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户权限规则
    /// </summary>
    public class ResolvedUserPermission
    {
        internal ResolvedUserPermission() { }

        private ConcurrentDictionary<string, PermissionSetting> userPermissionSettings = new ConcurrentDictionary<string, PermissionSetting>();

        /// <summary>
        /// 合并权限规则
        /// </summary>
        /// <param name="permissionItem">权限项目</param>
        /// <param name="permissionType">权限许可类型</param>
        /// <param name="permissionScope">权限许可范围</param>
        /// <param name="permissionQuota">权限许可额度</param>
        internal void Merge(PermissionItem permissionItem, PermissionType permissionType, PermissionScope permissionScope, float permissionQuota)
        {
            if (userPermissionSettings.ContainsKey(permissionItem.ItemKey))
            {
                PermissionSetting permissionSetting = userPermissionSettings[permissionItem.ItemKey];

                if (permissionSetting.PermissionType == PermissionType.Refuse || permissionType == PermissionType.Refuse)
                    permissionSetting.PermissionType = PermissionType.Refuse;
                else if (permissionSetting.PermissionType == PermissionType.NotSet && permissionType == PermissionType.NotSet)
                    permissionSetting.PermissionType = PermissionType.NotSet;
                else
                {
                    permissionSetting.PermissionType = PermissionType.Allow;

                    if (permissionType == PermissionType.Allow)
                    {
                        if (permissionItem.EnableScope)
                        {
                            if ((int)permissionSetting.PermissionScope < (int)permissionScope)
                                permissionSetting.PermissionScope = permissionScope;
                        }
                        if (permissionItem.EnableQuota)
                        {
                            if (permissionSetting.PermissionQuota < permissionQuota)
                                permissionSetting.PermissionQuota = permissionQuota;
                        }
                    }
                }
                userPermissionSettings[permissionItem.ItemKey] = permissionSetting;
            }
            else
            {
                this.userPermissionSettings[permissionItem.ItemKey] = new PermissionSetting(permissionType, permissionScope, permissionQuota);
            }
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="itemKey">权限项目标识</param>
        /// <returns>通过验证返回true，否则返回false</returns>        
        public bool Validate(string itemKey)
        {
            bool result = false;
            if (userPermissionSettings.ContainsKey(itemKey))
            {
                if (userPermissionSettings[itemKey].PermissionType == PermissionType.Allow)
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="itemKey">权限项目标识</param>
        /// <param name="permissionScope">权限许可范围</param>
        /// <returns>通过验证返回true，否则返回false</returns>  
        public bool Validate(string itemKey, PermissionScope permissionScope)
        {
            bool result = false;
            if (userPermissionSettings.ContainsKey(itemKey))
            {
                PermissionSetting permissionSetting = userPermissionSettings[itemKey];
                if (permissionSetting.PermissionType == PermissionType.Allow && (int)permissionSetting.PermissionScope >= (int)permissionScope)
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="itemKey">权限项目标识</param>
        /// <param name="permissionScope">权限许可范围</param>
        /// <param name="permissionQuota">权限许可额度</param>
        /// <returns>通过验证返回true，否则返回false</returns>
        public bool Validate(string itemKey, PermissionScope permissionScope, float permissionQuota)
        {
            bool result = false;
            if (userPermissionSettings.ContainsKey(itemKey))
            {
                PermissionSetting permissionSetting = userPermissionSettings[itemKey];
                if (permissionSetting.PermissionType == PermissionType.Allow
                    && (int)permissionSetting.PermissionScope >= (int)permissionScope
                    && permissionSetting.PermissionQuota >= permissionQuota)
                    result = true;
            }
            return result;
        }


        /// <summary>
        /// 权限设置
        /// </summary>
        private struct PermissionSetting
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="permissionType"></param>
            /// <param name="permissionScope"></param>
            /// <param name="permissionQuota"></param>
            public PermissionSetting(PermissionType permissionType, PermissionScope permissionScope, float permissionQuota)
            {
                this.permissionType = permissionType;
                this.permissionScope = permissionScope;
                this.permissionQuota = permissionQuota;
            }

            private PermissionType permissionType;
            /// <summary>
            /// 权限许可类型
            /// </summary>
            public PermissionType PermissionType
            {
                get { return permissionType; }
                set { permissionType = value; }
            }

            private PermissionScope permissionScope;
            /// <summary>
            /// 权限许可范围
            /// </summary>
            public PermissionScope PermissionScope
            {
                get { return permissionScope; }
                set { permissionScope = value; }
            }

            private float permissionQuota;
            /// <summary>
            /// 权限许可额度
            /// </summary>
            public float PermissionQuota
            {
                get { return permissionQuota; }
                set { permissionQuota = value; }
            }

        }

    }
}
