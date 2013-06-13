//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 动态接收人获取器
    /// </summary>
    public interface IActivityReceiverGetter
    {
        //done:mazq,by zhengw:需走查以下变更：默认向粉丝圈推送动态
        /// <summary>
        /// 获取接收人UserId集合
        /// </summary>
        /// <param name="activityService">动态业务逻辑类</param>
        /// <param name="activity">动态</param>
        /// <returns></returns>
        IEnumerable<long> GetReceiverUserIds(ActivityService activityService, Activity activity);
    }
}