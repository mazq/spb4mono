using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    public class AccountBindingRepository : Repository<AccountBinding>, IAccountBindingRepository
    {


        /// <summary>
        /// 创建第三方账号绑定
        /// </summary>
        /// <param name="account"></param>     
        public void CreateAccountBinding(AccountBinding account)
        {
            //设计说明: 
            //插入前，需要检查UserId+AccountTypeKey唯一
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("tn_AccountBindings")
                .Where("UserId = @0 and AccountTypeKey = @1", account.UserId, account.AccountTypeKey);
            AccountBinding localAccountBinding = CreateDAO().FirstOrDefault<AccountBinding>(sql);

            Sql.Builder.Select("*")
               .From("tn_AccountBindings")
               .Where("Identification = @0 and AccountTypeKey = @1", account.Identification, account.AccountTypeKey);
            AccountBinding remoteAccountBinding = CreateDAO().FirstOrDefault<AccountBinding>(sql);

            if (localAccountBinding == null && remoteAccountBinding == null)
            {
                base.Insert(account);
            }
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
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("tn_AccountBindings")
                .Where("UserId = @0 and AccountTypeKey = @1 and identification = @2", userId, accountTypeKey, identification);
            AccountBinding accountBinding = CreateDAO().FirstOrDefault<AccountBinding>(sql);
            if (accountBinding != null)
            {
                accountBinding.AccessToken = accessToken;
                base.Update(accountBinding);
            }
        }


        /// <summary>
        /// 删除第三方账号绑定
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountTypeKey">第三方账号类型Key</param>
        public void DeleteAccountBinding(long userId, string accountTypeKey)
        {
            Sql sql = Sql.Builder;
            sql.Append("delete from tn_AccountBindings")
                .Where("UserId=@0 and AccountTypeKey=@1", userId, accountTypeKey);
            CreateDAO().Execute(sql);
        }


        //private ICacheService cacheService = DIContainer.Resolve<ICacheService>();
        /// <summary>
        /// 获取单个第三方账号绑定
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="accountTypeKey">第三方账号类型Key</param>
        /// <returns></returns>
        public AccountBinding GetAccountBinding(long userId, string accountTypeKey)
        {
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("tn_AccountBindings")
                .Where("UserId=@0 and AccountTypeKey=@1", userId, accountTypeKey);
            AccountBinding accountBinding = CreateDAO().FirstOrDefault<AccountBinding>(sql);
            return accountBinding;
        }

        /// <summary>
        /// 获取某用户的所有第三方账号绑定
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>若没有，则返回空集合</returns>
        public IEnumerable<AccountBinding> GetAccountBindings(long userId)
        {
            Sql sql = Sql.Builder;
            sql.Select("Id")
                .From("tn_AccountBindings")
                .Where("UserId=@0", userId);
            IEnumerable<long> accountBindingIds = CreateDAO().Fetch<long>(sql);

            return PopulateEntitiesByEntityIds<long>(accountBindingIds);

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
            Sql sql = Sql.Builder;
            sql.Select("UserId")
                .From("tn_AccountBindings")
                .Where("AccountTypeKey= @0 and Identification= @1", accountTypeKey, Identification);
            long userId = CreateDAO().FirstOrDefault<long>(sql);
            return userId;
        }
    }
}
