//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 公告仓储类接口
    /// </summary>
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        
        /// <summary>
        /// 改变顺序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="referenceId"></param>
        void ChangeDisplayOrder(long id, long referenceId);

        /// <summary>
        ///为后台管理获取公告 
        /// </summary>
        /// <returns></returns>
        PagingDataSet<Announcement> GetForAdmin(string keyword = null, DateTime? startDate_Expired = null, DateTime? endDate_Expired = null, DateTime? startDate_Update = null, DateTime? endDate_Update = null, Announcement_Status? status = null, string displayArea = null, int pageIndex = 1, int pageSize = 20);


        /// <summary>
        /// 为前台显示列表
        /// </summary>
        /// <returns></returns>
        PagingDataSet<Announcement> Gets( int pageSize = 20, int pageIndex = 1);

        /// <summary>
        /// 根据展示区域获取公告
        /// </summary>
        /// <param name="displayArea"></param>
        /// <returns></returns>
        IEnumerable<Announcement> Gets(string displayArea);
       
    }

}
