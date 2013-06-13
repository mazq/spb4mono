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
using Tunynet.Caching;
using PetaPoco;

namespace Tunynet.UI
{
    /// <summary>
    /// 常用操作数据访问
    /// </summary>
    public class CommonOperationRepository : Repository<CommonOperation>, ICommonOperationRepository
    {         
        /// <summary>
        /// 清除用户的常用操作
        /// </summary>
        /// <param name="userId">用户ID</param>
        public void ClearUserCommonOperations(long userId)
        {
           Sql sql = Sql.Builder;
           sql.Append("delete from tn_CommonOperations where UserId = @0", userId);          
           CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 获取常用操作
        /// </summary>
        /// <param name="navigationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CommonOperation GetCommonOperation(int navigationId, long userId)
        {
            Sql sql = Sql.Builder;
            PetaPocoDatabase dao = CreateDAO();
            sql.Select("*")
                .From("tn_CommonOperations")
                .Where("UserId=@0",userId)
                .Where("NavigationId=@0",navigationId);

            CommonOperation commonOperation = null;

            commonOperation = dao.FirstOrDefault<CommonOperation>(sql);

            return commonOperation;
        }

    }

}
