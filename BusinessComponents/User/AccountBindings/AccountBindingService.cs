//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 账号绑定业务逻辑类
    /// </summary>
    public class AccountBindingService
    {
        private IAccountBindingRepository accountBindingRepository;
        private Repository<AccountType> repository = new Repository<AccountType>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public AccountBindingService()
            : this(new AccountBindingRepository())
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public AccountBindingService(AccountBindingRepository repository)
        {
            this.accountBindingRepository = repository;
        }
        #region 维护账号绑定

        /// <summary>
        /// 创建第三方账号绑定
        /// </summary>
        /// <param name="account"></param>     
        public void CreateAccountBinding(AccountBinding account)
        {
            //设计说明: 
            //插入前，需要检查UserId+AccountTypeKey唯一
            EventBus<AccountBinding>.Instance().OnBefore(account, new CommonEventArgs(EventOperationType.Instance().Create()));
            accountBindingRepository.CreateAccountBinding(account);
            EventBus<AccountBinding>.Instance().OnAfter(account, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// 更新授权凭据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountTypeKey"></param>
        /// <param name="identification"></param>
        /// <param name="accessToken"></param>
        public void UpdateAccessToken(long userId, string accountTypeKey, string identification, string accessToken)
        {
            accountBindingRepository.UpdateAccessToken(userId, accountTypeKey, identification, accessToken);
        }

        /// <summary>
        /// 删除第三方账号绑定
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountTypeKey">第三方账号类型Key</param>
        public void DeleteAccountBinding(long userId, string accountTypeKey)
        {
            AccountBinding accountBinding = accountBindingRepository.GetAccountBinding(userId, accountTypeKey);
            EventBus<AccountBinding>.Instance().OnBefore(accountBinding, new CommonEventArgs(EventOperationType.Instance().Delete()));
            accountBindingRepository.DeleteAccountBinding(userId, accountTypeKey);
            EventBus<AccountBinding>.Instance().OnAfter(accountBinding, new CommonEventArgs(EventOperationType.Instance().Delete()));

        }

        #endregion

        #region 获取绑定

        /// <summary>
        /// 获取单个第三方账号绑定
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="accountTypeKey">第三方账号类型Key</param>
        /// <returns></returns>
        public AccountBinding GetAccountBinding(long userId, string accountTypeKey)
        {
            return accountBindingRepository.GetAccountBinding(userId, accountTypeKey);
        }

        /// <summary>
        /// 获取某用户的所有第三方账号绑定
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>若没有，则返回空集合</returns>
        public IEnumerable<AccountBinding> GetAccountBindings(long userId)
        {
            return accountBindingRepository.GetAccountBindings(userId);
        }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="accountTypeKey">第三方账号类型Key</param>
        /// <param name="Identification">第三方账号标识</param>
        /// <returns>用户Id</returns>
        public long GetUserId(string accountTypeKey, string Identification)
        {
            //设计说明: 
            //无需缓存
            return accountBindingRepository.GetUserId(accountTypeKey, Identification);
        }

        #endregion

        #region 账号类型

        /// <summary>
        /// 更新第三方账号类型
        /// </summary>
        /// <param name="accountType"></param>
        public void UpdateAccountType(AccountType accountType)
        {
            EventBus<AccountType>.Instance().OnBefore(accountType, new CommonEventArgs(EventOperationType.Instance().Update()));
            repository.Update(accountType);
            EventBus<AccountType>.Instance().OnAfter(accountType, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 获取第三方账号类型
        /// </summary>
        /// <param name="accountTypeKey"></param>
        /// <returns></returns>
        public AccountType GetAccountType(string accountTypeKey)
        {
            return repository.Get(accountTypeKey);
        }

        /// <summary>
        /// 获取所有第三方账号类型
        /// </summary>
        /// <returns>若没有，则返回空集合</returns>
        public IEnumerable<AccountType> GetAccountTypes(bool? isEnabled = null, bool? isSync = null)
        {
            //设计说明: 
            //缓存期限：相对稳定，需即时更新
            IEnumerable<AccountType> accountTypes = repository.GetAll();
            if (isEnabled.HasValue)
                accountTypes = accountTypes.Where(n => n.IsEnabled == isEnabled.Value);
            if (isSync.HasValue)
                accountTypes = accountTypes.Where(n => n.IsSync == isSync.Value);
            return accountTypes;
        }

        #endregion
    }
}