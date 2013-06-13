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
    /// <summary>
    /// ApplicationInPresentAreaInstallation仓储接口
    /// </summary>
    public interface IAttachmentRepository<T> : IRepository<T> where T : Attachment
    {
        /// <summary>
        /// 删除AssociateId相关的附件
        /// </summary>
        void DeletesByAssociateId(string tenantTypeId, long associateId);

        /// <summary>
        /// 删除UserId相关的附件
        /// </summary>
        /// <param name="userId">上传者Id</param>
        void DeletesByUserId(string tenantTypeId, long userId);

        /// <summary>
        /// 删除OwnerId相关的附件
        /// </summary>
        void DeletesByOwnerId(string tenantTypeId, long ownerId);

        /// <summary>
        /// 依据AssociateId获取附件列表（用于AssociateId与附件一对多关系）
        /// </summary>
        IEnumerable<T> GetsByAssociateId(string tenantTypeId, long associateId);

        /// <summary>
        /// 依据userId获取附件列表（用于userId与附件一对多关系）
        /// </summary>
        /// <param name="userId">附件上传人Id</param>
        /// <returns>附件列表</returns>
        IEnumerable<T> GetsByUserId(string tenantTypeId, long userId);

        /// <summary>
        /// 获取拥有者的所有附件或者拥有者一种租户类型的附件
        /// </summary>
        IEnumerable<T> Gets(long ownerId, string tenantTypeId);

        /// <summary>
        /// 搜索附件并分页显示
        /// </summary>
        PagingDataSet<T> Gets(string tenantTypeId, string keyword, int pageIndex);

        /// <summary>
        /// 获取拥有者一种租户类型的临时附件
        /// </summary>
        IEnumerable<T> GetTemporaryAttachments(long ownerId, string tenantTypeId);

        /// <summary>
        /// 获取需删除的垃圾临时附件
        /// </summary>
        /// <param name="beforeDays">多少天之前的附件</param>
        IEnumerable<T> GetTrashTemporaryAttachments(int beforeDays);

        /// <summary>
        /// 删除垃圾临时附件
        /// </summary>
        void DeleteTrashTemporaryAttachments(int beforeDays);

        /// <summary>
        /// 把临时附件转成正常附件
        /// </summary>
        void ToggleTemporaryAttachments(long ownerId, string tenantTypeId, long associateId);
    }
}
