//using Tunynet.Repositories;
//using System.Collections.Generic;
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

namespace Tunynet.Common
{
    ///<summary>
    ///AttachmentDownloadRepository仓储接口
    ///</summary>
    public interface IAttachmentDownloadRepository : IRepository<AttachmentDownloadRecord>
    {

        /// <summary>
        /// 创建新的下载记录
        /// </summary>
        /// <param name="entity">下载记录实体</param>
        /// <returns>下载记录Id</returns>
        object Insert(AttachmentDownloadRecord entity);

        /// <summary>
        /// 更新最后下载时间
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <param name="attachmentId">附件Id</param>
        bool UpdateLastDownloadDate(long userId, long attachmentId);

        /// <summary>
        /// 根据获取用户附件下载记录及附件的Id集合
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <returns>下载记录Id和附件Id的字典集合，{key-下载记录Id:value-附件Id}</returns>
        Dictionary<long, long> GetIds_AttachmentIdsByUser(long userId);

        /// <summary>
        /// 获取附件的前topNumber条下载记录
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="topNumber">返回的记录数</param>
        IEnumerable<AttachmentDownloadRecord> GetTopsByAttachmentId(long attachmentId, int topNumber);

        /// <summary>
        /// 获取附件的下载记录分页显示
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="pageIndex">页码</param>
        PagingDataSet<AttachmentDownloadRecord> GetsByAttachmentId(long attachmentId, int pageIndex);

        /// <summary>
        /// 获取附件依附对象的前topNumber条下载记录
        /// </summary>
        /// <param name="associateId">附件依附对象Id</param>
        /// <param name="topNumber">获取记录数</param>
        IEnumerable<AttachmentDownloadRecord> GetTopsByAssociateId(long associateId, int topNumber);

        /// <summary>
        /// 获取附件依附对象的下载记录分页显示
        /// </summary>
        /// <param name="associateId">依附对象Id</param>
        /// <param name="pageIndex">页码</param>
        PagingDataSet<AttachmentDownloadRecord> GetsByAssociateId(long associateId, int pageIndex);

        /// <summary>
        /// 获取用户的下载记录分页显示
        /// </summary>
        /// <param name="userId">下载用户UserId</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="needToBuy">是否需要购买</param>
        PagingDataSet<AttachmentDownloadRecord> GetsByUserId(long userId, int pageIndex, bool needToBuy = true);

        /// <summary>
        /// 获取拥有者附件的下载记录分页显示
        /// </summary>
        /// <param name="ownerId">附件拥有者Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="needToBuy">是否需要购买</param>
        PagingDataSet<AttachmentDownloadRecord> GetsByOwnerId(long ownerId, int pageIndex, bool needToBuy = true);
    }
}
