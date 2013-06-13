//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Repositories;
using Tunynet.Utilities;
using Spacebuilder.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 积分商城应用的邮寄地址仓储实现
    /// </summary>
    public class MailAddressRepository : Repository<MailAddress>, IMailAddressRepository
    {
        /// <summary>
        /// 获取用户的邮寄地址
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public IEnumerable<MailAddress> GetEmailAddresssOfUser(long userId)
        {
            Sql sql = Sql.Builder;

            sql.Select("spb_MailAddress.*")
               .From("spb_MailAddress")
               .Where("spb_MailAddress.UserId = @0", userId)
               .OrderBy("spb_MailAddress.LastModified desc");

            return GetPagingEntities(base.PrimaryMaxRecords, 1, sql);

           
        }
    }
}
