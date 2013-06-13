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
    /// 用户动态设置仓储接口
    /// </summary>
    public interface IActivityItemUserSettingRepository 
    {

        void UpdateActivityItemUserSettings(long userId, Dictionary<string, bool> userSettings);

        Dictionary<string, bool> GetActivityItemUserSettings(long userId);
    }
}