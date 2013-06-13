//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 积分商城应用的邮寄地址仓储接口
    /// </summary>
    public interface IMailAddressRepository : IRepository<MailAddress>
    {
        IEnumerable<MailAddress> GetEmailAddresssOfUser(long userId);
    }
}
