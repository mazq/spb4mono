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
    /// 动态项目仓储
    /// </summary>
    public class ActivityItemRepository : Repository<ActivityItem>, IActivityItemRepository
    {

        /// <summary>
        /// 更新动态项目（仅更新IsUserReceived、IsSiteReceived）
        /// </summary>
        /// <param name="activityItems">准备更新的动态项目</param>
        public void UpdateActivityItems(IEnumerable<ActivityItem> activityItems)
        {
            if (activityItems == null)
                return;
            //仅允许更新IsUserReceived、IsSiteReceived
            List<Sql> sql_Updates = new List<Sql>();
            foreach (ActivityItem activityItem in activityItems)
            {
                Sql sql_Update = Sql.Builder;
                sql_Update.Append("update tn_ActivityItems set IsUserReceived=@0,IsSiteReceived=@1 where ItemKey=@2", activityItem.IsUserReceived, activityItem.IsSiteReceived, activityItem.ItemKey);
                sql_Updates.Add(sql_Update);
            }
            CreateDAO().Execute(sql_Updates);

            foreach (var item in activityItems)
            {
                ActivityItem activityItem = base.Get(item.ItemKey);
                if (activityItem == null)
                    continue;
                activityItem.IsUserReceived = item.IsUserReceived;
                activityItem.IsSiteReceived = item.IsSiteReceived;
                base.OnUpdated(activityItem);
            }
        }
    }
}
