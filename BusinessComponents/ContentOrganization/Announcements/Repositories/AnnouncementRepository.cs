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
using PetaPoco;
using System.Threading;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 公告数据仓储类
    /// </summary>
    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="entity">公告实体</param>
        public override void Update(Announcement entity)
        {
            RealTimeCacheHelper.IncreaseGlobalVersion();
            base.Update(entity);
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="entity">公告实体</param>
        /// <returns></returns>
        public override object Insert(Announcement entity)
        {
            RealTimeCacheHelper.IncreaseGlobalVersion();
            return base.Insert(entity);
        }



        /// <summary>
        /// 更改显示顺序
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="referenceId">referenceId</param>
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            var announcement = Get(id);
            var referenceAnnouncement = Get(referenceId);

            long displayOrder = referenceAnnouncement.DisplayOrder;
            referenceAnnouncement.DisplayOrder = announcement.DisplayOrder;
            announcement.DisplayOrder = displayOrder;

            base.Update(announcement);
            base.Update(referenceAnnouncement);

        }

        /// <summary>
        /// 获取公告列表(后台管理)
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="startDate_Expired">过期开始时间</param>
        /// <param name="endDate_Expired">过期结束时间</param>
        /// <param name="startDate_Update">更新开始时间</param>
        /// <param name="endDate_Update">更新结束时间</param>
        /// <param name="status">状态</param>        
        /// <param name="displayArea">呈现区域</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <returns>分页集合</returns>
        public PagingDataSet<Announcement> GetForAdmin(string keyword = null, DateTime? startDate_Expired = null, DateTime? endDate_Expired = null, DateTime? startDate_Update = null, DateTime? endDate_Update = null, Announcement_Status? status = null, string displayArea = null, int pageIndex = 1, int pageSize = 20)
        {
            //不使用缓存
            Sql sql = Sql.Builder;

            sql.Select("Id")
                .From("spb_Announcements");

            if (!string.IsNullOrEmpty(keyword))
                sql.Where("Subject like @0", "%" + keyword + "%");

            if (startDate_Expired.HasValue)
                sql.Where("ExpiredDate >= @0", startDate_Expired);

            if (endDate_Expired.HasValue)
                sql.Where("ExpiredDate <= @0", endDate_Expired);

            if (startDate_Update.HasValue)
                sql.Where("LastModified >= @0", startDate_Update);

            if (endDate_Update.HasValue)
                sql.Where("LastModified <= @0", endDate_Update);

            if (status.HasValue)
            {
                if (status == Announcement_Status.UnPublish)
                    sql.Where("ReleaseDate > @0", DateTime.UtcNow.ToLocalTime());

                if (status == Announcement_Status.Published)
                    sql.Where("ReleaseDate <= @0 and ExpiredDate >= @1", DateTime.UtcNow.ToLocalTime(), DateTime.UtcNow.ToLocalTime());

                if (status == Announcement_Status.Expired)
                    sql.Where("ExpiredDate < @0", DateTime.UtcNow.ToLocalTime());
            }
            if (!string.IsNullOrEmpty(displayArea))
                sql.Where("DisplayArea like @0", "%" + displayArea + "%");

            sql.OrderBy("DisplayOrder desc");

            return GetPagingEntities(pageSize, pageIndex, sql);
        }

        /// <summary>
        /// 获取公告(前台)
        /// </summary>
        /// <param name="pageSize">pageSize</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <returns></returns>
        public PagingDataSet<Announcement> Gets(int pageSize = 20, int pageIndex = 1)
        {
            //使用全局缓存
            Sql sql = Sql.Builder;
            sql.Select("Id")
               .From("spb_Announcements")
               .Where("ReleaseDate <= @0 and ExpiredDate >= @1", DateTime.UtcNow.ToLocalTime(), DateTime.UtcNow.ToLocalTime())
               .OrderBy("DisplayOrder desc");

            return GetPagingEntities(pageSize, pageIndex, Caching.CachingExpirationType.UsualObjectCollection, () =>
            {
                return string.Format("Announcement::GlobalVersion-{0};", RealTimeCacheHelper.GetGlobalVersion());
            }, () =>
            {
                return sql;
            });

        }

        /// <summary>
        /// 根据展示区域获取公告
        /// </summary>
        /// <param name="displayArea">展示区域</param>
        /// <returns></returns>
        public IEnumerable<Announcement> Gets(string displayArea)
        {
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("spb_Announcements")
                .Where("ReleaseDate <= @0 and ExpiredDate >= @1 and DisplayArea like @2", DateTime.UtcNow.ToLocalTime(), DateTime.UtcNow.ToLocalTime(), "%" + displayArea + "%")
                .OrderBy("DisplayOrder desc");

            return base.GetTopEntities(1000, Caching.CachingExpirationType.UsualObjectCollection, () =>
            {
                return string.Format("Announcement::GlobalVersion-{0};displayArea-{1};", RealTimeCacheHelper.GetGlobalVersion(), displayArea);
            }, () =>
            {
                return sql;
            });

        }
    }
}
