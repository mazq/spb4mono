//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 公告业务逻辑
    /// </summary>
    public class AnnouncementService
    {
        #region 构造器

        private IAnnouncementRepository announcementRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AnnouncementService()
            : this(new AnnouncementRepository())
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            this.announcementRepository = announcementRepository;
        }

        #endregion

       

        /// <summary>
        /// 创建公告
        /// </summary>
        /// <param name="announcement">公告实体</param>
        public void Create(Announcement announcement)
        {
            announcementRepository.Insert(announcement);
            announcement.DisplayOrder = announcement.Id;
            announcementRepository.Update(announcement);
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="announcement">公告实体</param>
        public void Update(Announcement announcement)
        {
            announcement.LastModified = DateTime.UtcNow;
            announcementRepository.Update(announcement);
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="announcementId">公告Id</param>
        public void Delete(long announcementId)
        {
            Announcement announcement = Get(announcementId);
            announcementRepository.Delete(announcement);

        }

        /// <summary>
        /// 更变为过期
        /// </summary>
        /// <param name="announcementId">公告Id</param>
        public void ChangeStatusToExpired(long announcementId)
        {
            Announcement announcement = Get(announcementId);
            announcement.ExpiredDate = DateTime.UtcNow.ToLocalTime();
            announcementRepository.Update(announcement);
        
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="announcementId">公告Id</param>
        public Announcement Get(long announcementId)
        {
            return announcementRepository.Get(announcementId);
        }

        /// <summary>
        /// 根据展示区域过去公告
        /// </summary>
        /// <param name="displayArea">显示区域</param>
        /// <returns></returns>
        public IEnumerable<Announcement> Gets(string displayArea)
        {
            return announcementRepository.Gets(displayArea);
        }

        /// <summary>
        /// 获取公告(后台管理)
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="startDate_Expired">过期时间前</param>
        /// <param name="endDate_Expired">过期时间后</param>
        /// <param name="startDate_Update">更新时间前</param>
        /// <param name="endDate_Update">更新时间后</param>
        /// <param name="status">状态</param>
        /// <param name="displayArea">显示区域</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns></returns>
        public PagingDataSet<Announcement> GetForAdmin(string keyword = null, DateTime? startDate_Expired = null, DateTime? endDate_Expired = null, DateTime? startDate_Update = null, DateTime? endDate_Update = null, Announcement_Status? status = null, string displayArea = null, int pageIndex = 1, int pageSize = 20)
        {
            return announcementRepository.GetForAdmin(keyword, startDate_Expired, endDate_Expired, startDate_Update, endDate_Update, status, displayArea, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取公告(前台)
        /// </summary>
        /// <param name="pageSize">pageSize</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <returns></returns>
        public PagingDataSet<Announcement> Gets( int pageSize = 20, int pageIndex = 1)
        {
            return announcementRepository.Gets(pageSize, pageIndex);
        }

        /// <summary>
        /// 改变显示顺序
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="referenceId">referenceId</param>
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            announcementRepository.ChangeDisplayOrder(id, referenceId);
        }
    }
}
