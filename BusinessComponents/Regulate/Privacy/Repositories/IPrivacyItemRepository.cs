//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{

    /// <summary>
    /// 隐私项目接口
    /// </summary>
    public interface IPrivacyItemRepository : IRepository<PrivacyItem>
    {
        /// <summary>
        /// 更新隐私规则
        /// </summary>
        /// <param name="privacyItems">待更新的隐私项目规则集合</param>
        void UpdatePrivacyItems(IEnumerable<PrivacyItem> privacyItems);
    }
}
