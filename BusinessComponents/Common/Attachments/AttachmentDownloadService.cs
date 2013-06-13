//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Utilities;
using System.Web;
using Tunynet.Repositories;

namespace Tunynet.Common
{

    /// <summary>
    /// 下载记录业务逻辑类
    /// </summary>
    public class AttachmentDownloadService
    {

        //AttachmentDownload Repository
        private IAttachmentDownloadRepository _attachmentDownloadRepository;
        //
        private IRepository<Attachment> _attachmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AttachmentDownloadService()
            : this(new AttachmentDownloadRepository<AttachmentDownloadRecord>(), new Repository<Attachment>())
        {
        }

        /// <summary>
        /// 可设置repository的构造函数（主要用于测试用例）
        /// </summary>
        public AttachmentDownloadService(IAttachmentDownloadRepository attachmentDownloadRepository, IRepository<Attachment> repository)
        {
            this._attachmentDownloadRepository = attachmentDownloadRepository;
            this._attachmentRepository = repository;
        }

        /// <summary>
        /// 创建下载记录
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <param name="attachmentId">附件Id</param>
        /// <returns>创建成功返回true，否则返回false</returns>
        public bool Create(long userId, long attachmentId)
        {
            Attachment attachment = _attachmentRepository.Get(attachmentId);
            if (attachment == null)
                return false;
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.GetUser(userId);
            if (user == null)
                return false;

            if (IsDownloaded(userId, attachmentId))
            {
                return _attachmentDownloadRepository.UpdateLastDownloadDate(userId, attachmentId);
            }
            else
            {
                AttachmentDownloadRecord record = AttachmentDownloadRecord.New(attachment);
                record.UserId = userId;
                record.UserDisplayName = user.DisplayName;
                long id = 0;
                long.TryParse(_attachmentDownloadRepository.Insert(record).ToString(), out id);

                return id > 0;
            }
        }

        /// <summary>
        /// 用户是否已经下载某个附件
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <param name="attachmentId">附件Id</param>
        /// <returns>曾经下载过返回true，否则返回false</returns>
        public bool IsDownloaded(long userId, long attachmentId)
        {
            Dictionary<long, long> ids_AttachmentIds = _attachmentDownloadRepository.GetIds_AttachmentIdsByUser(userId);

            if (ids_AttachmentIds != null)
            {
                return ids_AttachmentIds.ContainsValue(attachmentId);
            }

            return false;
        }

        /// <summary>
        /// 获取附件的前topNumber条下载记录
        /// </summary>
        /// <param name="attachmentID">附件Id</param>
        /// <param name="topNumber">获取记录条数</param>
        public IEnumerable<AttachmentDownloadRecord> GetTopsByAttachmentId(int attachmentID, int topNumber)
        {
            return _attachmentDownloadRepository.GetTopsByAttachmentId(attachmentID, topNumber);
        }

        /// <summary>
        /// 获取附件的下载记录分页显示
        /// </summary>
        /// <param name="attachmentID">附件Id</param>
        /// <param name="pageIndex">页码</param>
        public PagingDataSet<AttachmentDownloadRecord> GetsByAttachmentId(int attachmentID, int pageIndex)
        {
            return _attachmentDownloadRepository.GetsByAttachmentId(attachmentID, pageIndex);
        }

        /// <summary>
        /// 获取附件依附对象的前topNumber条下载记录
        /// </summary>
        /// <param name="associateId">依附对象Id</param>
        /// <param name="topNumber">获取记录条数</param>
        public IEnumerable<AttachmentDownloadRecord> GetTopsByAssociateId(int associateId, int topNumber)
        {
            return _attachmentDownloadRepository.GetTopsByAssociateId(associateId, topNumber);
        }

        /// <summary>
        /// 获取附件依附对象的下载记录分页显示
        /// </summary>
        /// <param name="associateId">依附对象Id</param>
        /// <param name="pageIndex">页码</param>
        public PagingDataSet<AttachmentDownloadRecord> GetsByAssociateId(int associateId, int pageIndex)
        {
            return _attachmentDownloadRepository.GetsByAssociateId(associateId, pageIndex);
        }

        /// <summary>
        /// 获取用户的下载记录分页显示
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="needToBuy">是否需要购买</param>
        public PagingDataSet<AttachmentDownloadRecord> GetsByUserId(int userId, int pageIndex, bool needToBuy = true)
        {
            return _attachmentDownloadRepository.GetsByUserId(userId, pageIndex, needToBuy);
        }

        /// <summary>
        /// 获取拥有者附件的下载记录分页显示
        /// </summary>
        /// <param name="ownerId">附件拥有者Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="needToBuy">是否需要够买</param>
        public PagingDataSet<AttachmentDownloadRecord> GetsByOwnerId(int ownerId, int pageIndex, bool needToBuy = true)
        {
            return _attachmentDownloadRepository.GetsByOwnerId(ownerId, pageIndex, needToBuy);
        }

    }
}
