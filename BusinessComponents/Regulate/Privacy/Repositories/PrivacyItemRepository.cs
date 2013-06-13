//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Caching;
using PetaPoco;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 隐私项目仓储
    /// </summary>
    public class PrivacyItemRepository : Repository<PrivacyItem>, IPrivacyItemRepository
    {

        /// <summary>
        /// 更新隐私规则
        /// </summary>
        /// <param name="privacyItems">待更新的隐私项目规则集合</param>
        public void UpdatePrivacyItems(IEnumerable<PrivacyItem> privacyItems)
        {
            if (privacyItems == null)
                return;
            List<Sql> sqls = new List<Sql>();

            PetaPocoDatabase dao = CreateDAO();

            dao.OpenSharedConnection();
            foreach (var privacyItem in privacyItems)
            {
                sqls.Add(Sql.Builder.Append("update tn_PrivacyItems")
                                    .Append("set ItemName = @0, Description = @1, DisplayOrder = @2, PrivacyStatus = @3", privacyItem.ItemName, privacyItem.Description, privacyItem.DisplayOrder, privacyItem.PrivacyStatus)
                                    .Append("where ItemKey = @0 and ItemGroupId = @1 and ApplicationId = @2", privacyItem.ItemKey, privacyItem.ItemGroupId, privacyItem.ApplicationId));

                RealTimeCacheHelper.IncreaseEntityCacheVersion(privacyItem.ItemKey);
                OnUpdated(privacyItem);
            }
            dao.Execute(sqls);
            //done:zhangp,by zhengw:需要递增全局版本号
            dao.CloseSharedConnection();

            RealTimeCacheHelper.IncreaseGlobalVersion();
        }
    }

}
