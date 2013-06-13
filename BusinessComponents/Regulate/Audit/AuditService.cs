//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Tunynet.Common.Configuration;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 审核业务逻辑类
    /// </summary>
    public class AuditService
    {
        #region 构造器
        private IRepository<AuditItem> auditItemRepository;
        private IAuditItemInUserRoleRepository auditItemInUserRoleRepository;


        /// <summary>
        /// 构造器
        /// </summary>
        public AuditService()
            : this(new Repository<AuditItem>(), new AuditItemInUserRoleRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="auditItemRepository">AuditItem仓储</param>
        /// <param name="auditItemInUserRoleRepository">PermissionItemInUserRoleRepository仓储</param>
        public AuditService(IRepository<AuditItem> auditItemRepository, IAuditItemInUserRoleRepository auditItemInUserRoleRepository)
        {
            this.auditItemRepository = auditItemRepository;
            this.auditItemInUserRoleRepository = auditItemInUserRoleRepository;
        }
        #endregion

        #region 审核项目

        /// <summary>
        /// 获取审核项集合
        /// </summary>
        /// <param name="applicationId">应用程序ID</param>
        /// <returns>审核项集合</returns>
        public IEnumerable<AuditItem> GetAuditItems(int applicationId)
        {
            //获取并缓存所有AuditItem
            //applicationId不为空时，用上述结果过滤得到
            //参考PermissionService.GetPermissionItems()
            IEnumerable<AuditItem> allAuditItems = auditItemRepository.GetAll();
            allAuditItems = allAuditItems.Where(n => n.ApplicationId == applicationId);
            return allAuditItems;

        }

        /// <summary>
        /// 获取AuditItem
        /// </summary>
        /// <param name="itemKey">审核项标识</param>
        /// <returns></returns>
        public AuditItem GetAuditItem(string itemKey)
        {
            return auditItemRepository.Get(itemKey);
        }

        /// <summary>
        /// 获取用户角色对应的审核设置
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>返回roleName对应的审核设置</returns>
        public IEnumerable<AuditItemInUserRole> GetAuditItemsInUserRole(string roleName, int? applicationId = null)
        {
            //缓存策略：常用列表
            IEnumerable<AuditItemInUserRole> roles = auditItemInUserRoleRepository.GetAuditItemsInUserRole(roleName);
            if (applicationId.HasValue)
                roles = roles.Where(n => { AuditItem item = GetAuditItem(n.ItemKey); return item != null && item.ApplicationId == applicationId; });
            return roles;
        }

        /// <summary>
        /// 更新权限规则
        /// </summary>
        /// <param name="AuditItemInUserRoles">待更新的权限项目规则集合</param>
        public void UpdateAuditItemInUserRole(IEnumerable<AuditItemInUserRole> auditItemInUserRoles)
        {
            //同步 GetAuditItemsInUserRole(string roleName) 相关的缓存

            auditItemInUserRoleRepository.UpdateAuditItemInUserRole(auditItemInUserRoles);
            EventBus<AuditItemInUserRole, CommonEventArgs>.Instance().OnBatchAfter(auditItemInUserRoles, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 获取应用的PubliclyAuditStatus设置
        /// </summary>
        /// <param name="applicationId">ApplicationId</param>
        /// <returns></returns>
        public PubliclyAuditStatus GetPubliclyAuditStatus(int applicationId)
        {
            ApplicationDataService applicationDataService = new ApplicationDataService();
            long valueOfPubliclyAuditStatus = applicationDataService.GetLong(applicationId, ApplicationDataKeys.Instance().PubliclyAuditStatus());

            if (valueOfPubliclyAuditStatus != 0)
                return (PubliclyAuditStatus)valueOfPubliclyAuditStatus;
            else
                return PubliclyAuditStatus.Pending_GreaterThanOrEqual;
        }

        /// <summary>
        /// 保存应用的PubliclyAuditStatus设置
        /// </summary>
        /// <param name="applicationId">ApplicationId</param>
        /// <returns></returns>
        public void SavePubliclyAuditStatus(int applicationId, PubliclyAuditStatus publiclyAuditStatus)
        {
            ApplicationDataService applicationDataService = new ApplicationDataService();
            applicationDataService.Change(applicationId, ApplicationDataKeys.Instance().PubliclyAuditStatus(), (long)publiclyAuditStatus);
        }

        /// <summary>
        /// 创建实体时设置审核状态
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="auditable">可审核实体</param>
        public void ChangeAuditStatusForCreate(long userId, IAuditable auditable)
        {
            if (NeedAudit(userId, auditable, AuditStrictDegree.Create))
                auditable.AuditStatus = AuditStatus.Pending;
            else
                auditable.AuditStatus = AuditStatus.Success;
        }

        /// <summary>
        /// 更新实体时设置审核状态
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="auditable">可审核实体</param>
        public void ChangeAuditStatusForUpdate(long userId, IAuditable auditable)
        {
            if (auditable.AuditStatus == AuditStatus.Success)
            {
                if (NeedAudit(userId, auditable, AuditStrictDegree.Update))
                    auditable.AuditStatus = AuditStatus.Again;
            }
        }

        /// <summary>
        /// 判断是否需要在一定的严格程度上需要审核
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="auditable">可审核实体</param>
        /// <param name="strictDegree">审核严格程度</param>
        /// <returns></returns>
        private bool NeedAudit(long userId, IAuditable auditable, AuditStrictDegree strictDegree)
        {
            var user = DIContainer.Resolve<IUserService>().GetUser(userId);
            //匿名用户需要审核
            if (user == null)
                return true;
            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();
            RoleService roleService = new RoleService();

            //不启用审核
            if (!userSettings.EnableAudit)
                return false;

            //如果用户处于免审核角色，则直接通过
            if (roleService.IsUserInRoles(userId, userSettings.NoAuditedRoleNames.ToArray()))
                return false;


            //获取用户所属的角色，并附加上注册用户角色
            IEnumerable<Role> rolesOfUser = roleService.GetRolesOfUser(userId);
            IList<string> roleNamesOfUser = rolesOfUser.Select(n => n.RoleName).ToList();
            roleNamesOfUser.Add(RoleNames.Instance().RegisteredUsers());
            if (user.IsModerated)
                roleNamesOfUser.Add(RoleNames.Instance().ModeratedUser());
            //判断每个用户角色的设置是否可用
            foreach (var roleName in roleNamesOfUser)
            {
                IEnumerable<AuditItemInUserRole> auditItemInUserRoles = GetAuditItemsInUserRole(roleName);
                foreach (var auditItemInUserRole in auditItemInUserRoles)
                {
                    if (auditItemInUserRole.ItemKey.Equals(auditable.AuditItemKey))
                    {
                        if (auditItemInUserRole.StrictDegree == AuditStrictDegree.None)
                            return false;
                        else if (auditItemInUserRole.StrictDegree == AuditStrictDegree.NotSet)
                            break;
                        else if ((int)auditItemInUserRole.StrictDegree >= (int)strictDegree)
                            return true;
                    }
                }
            }

            //如果用户处于免审核用户等级，也直接通过
            if (user.Rank >= userSettings.MinNoAuditedUserRank)
                return false;
                
            return false;
        }

        #endregion

        /// <summary>
        /// 解析审核状态变化前后是否会对其他数据产生正向还负向的影响（例如：是该加积分，还是减积分）
        /// </summary>
        /// <remarks>该方法仅针对于管理员通过审核或不通过审核的情况</remarks>
        /// <param name="oldAuditStatus">变化前的审核状态（若是创建操作，请赋值为null）</param>
        /// <param name="newAuditStatus">变化后的审核状态（若是删除操作，请赋值为null）</param>
        /// <returns>true-正影响，false-负影响，null-未产生影响</returns>
        public bool? ResolveAuditDirection(AuditStatus? oldAuditStatus, AuditStatus? newAuditStatus)
        {
            if (oldAuditStatus == null && newAuditStatus == AuditStatus.Success)
                return true;
            if (oldAuditStatus == AuditStatus.Pending && newAuditStatus == AuditStatus.Success)
                return true;
            if (oldAuditStatus == AuditStatus.Fail && newAuditStatus == AuditStatus.Success)
                return true;
            if (oldAuditStatus == AuditStatus.Success && newAuditStatus == AuditStatus.Fail)
                return false;
            if (oldAuditStatus == AuditStatus.Again && newAuditStatus == AuditStatus.Fail)
                return false;
            if (oldAuditStatus == AuditStatus.Again && newAuditStatus == null)
                return false;
            if (oldAuditStatus == AuditStatus.Success && newAuditStatus == null)
                return false;
            return null;
        }

    }
}
