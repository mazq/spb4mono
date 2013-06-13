//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Events;

namespace Tunynet.UI
{
    /// <summary>
    /// ApplicationManagementOperation业务逻辑
    /// </summary>
    public class ManagementOperationService
    {

        //ApplicationManagementOperation Repository
        private IRepository<ApplicationManagementOperation> repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManagementOperationService()
            : this(new Repository<ApplicationManagementOperation>())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository">ApplicationManagementOperation仓储</param>
        public ManagementOperationService(IRepository<ApplicationManagementOperation> repository)
        {
            this.repository = repository;
        }


        #region Create & Update & Delete

        /// <summary>
        /// 添加管理操作
        /// </summary>
        /// <param name="applicationManagementOperation">ApplicationManagementOperation</param>
        public void Create(ApplicationManagementOperation applicationManagementOperation)
        {
            if (applicationManagementOperation == null)
                throw new ArgumentNullException("applicationManagementOperation不能为null");

            if (repository.Exists(applicationManagementOperation.OperationId))
                throw new ArgumentException("OperationId不允许重复");
            EventBus<ApplicationManagementOperation>.Instance().OnBefore(applicationManagementOperation, new CommonEventArgs(EventOperationType.Instance().Create(), applicationManagementOperation.ApplicationId));
            repository.Insert(applicationManagementOperation);
            EventBus<ApplicationManagementOperation>.Instance().OnAfter(applicationManagementOperation, new CommonEventArgs(EventOperationType.Instance().Create(), applicationManagementOperation.ApplicationId));
        }

        /// <summary>
        /// 更新管理操作
        /// </summary>
        /// <param name="applicationManagementOperation">ApplicationManagementOperation</param>
        public void Update(ApplicationManagementOperation applicationManagementOperation)
        {
            if (applicationManagementOperation == null)
                throw new ArgumentNullException("applicationManagementOperation不能为null");
            EventBus<ApplicationManagementOperation>.Instance().OnBefore(applicationManagementOperation, new CommonEventArgs(EventOperationType.Instance().Update(), applicationManagementOperation.ApplicationId));
            repository.Update(applicationManagementOperation);
            EventBus<ApplicationManagementOperation>.Instance().OnAfter(applicationManagementOperation, new CommonEventArgs(EventOperationType.Instance().Update(), applicationManagementOperation.ApplicationId));
        }

        /// <summary>
        /// 删除管理操作
        /// </summary>
        /// <param name="operationId">管理操作实体Id</param>
        public void Delete(int operationId)
        {
            ApplicationManagementOperation applicationManagementOperation = repository.Get(operationId);
            if (applicationManagementOperation == null)
                throw new ArgumentNullException(string.Format("OperationId为{0}的ApplicationManagementOperation不存在", operationId));

            if (applicationManagementOperation.IsLocked)
                throw new ApplicationException("锁定状态的ApplicationManagementOperation不允许删除");

            EventBus<ApplicationManagementOperation>.Instance().OnBefore(applicationManagementOperation, new CommonEventArgs(EventOperationType.Instance().Delete(), applicationManagementOperation.ApplicationId));
            repository.DeleteByEntityId(operationId);
            EventBus<ApplicationManagementOperation>.Instance().OnAfter(applicationManagementOperation, new CommonEventArgs(EventOperationType.Instance().Delete(), applicationManagementOperation.ApplicationId));
        }

        #endregion


        #region Get & Gets

        /// <summary>
        /// 获取ApplicationManagementOperation
        /// </summary>
        /// <param name="operationId">管理操作实体Id</param>
        public ApplicationManagementOperation Get(int operationId)
        {
            return repository.Get(operationId);
        }

        /// <summary>
        /// 获取指定呈现区域的所有快捷操作
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="onlyEnabled">是否仅返回启用状态的</param>
        /// <returns>获取呈现区域满足条件的快捷操作</returns>
        public IEnumerable<ApplicationManagementOperation> GetShortcuts(string presentAreaKey, bool onlyEnabled = true)
        {
            return GetManagementOperations(presentAreaKey, ManagementOperationType.Shortcut, onlyEnabled);
        }

        /// <summary>
        /// 获取指定呈现区域的所有管理菜单
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="onlyEnabled">是否仅返回启用状态的</param>
        /// <returns>获取呈现区域满足条件的操作菜单</returns>
        public IEnumerable<ApplicationManagementOperation> GetManagementMenus(string presentAreaKey, bool onlyEnabled = true)
        {
            return GetManagementOperations(presentAreaKey, ManagementOperationType.ManagementMenu, onlyEnabled);
        }

        /// <summary>
        /// 获取指定呈现区域指定类型的管理操作
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="operationType">管理操作类型</param>
        /// <param name="onlyEnabled">是否仅返回启用状态的</param>
        /// <returns>获取呈现区域满足条件的管理操作</returns>
        private IEnumerable<ApplicationManagementOperation> GetManagementOperations(string presentAreaKey, ManagementOperationType operationType, bool onlyEnabled = true)
        {
            IEnumerable<ApplicationManagementOperation> allOperations = repository.GetAll("DisplayOrder");
            IEnumerable<ApplicationManagementOperation> operationsOfPresentArea = allOperations.Where(n => n.OperationType == operationType && n.PresentAreaKey.Equals(presentAreaKey, StringComparison.InvariantCultureIgnoreCase));
            if (onlyEnabled)
                return operationsOfPresentArea.Where(n => n.IsEnabled);
            else
                return operationsOfPresentArea;
        }

        #endregion

    }
}
