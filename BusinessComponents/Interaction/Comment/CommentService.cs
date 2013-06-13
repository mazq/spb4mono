//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Repositories;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 评论业务逻辑
    /// </summary>
    public class CommentService
    {
        //Comment Repository
        private ICommentRepository commentRepository;

        private CommentService commentService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommentService()
            : this(new CommentRepository())
        {
        }

        /// <summary>
        /// 可设置repository的构造函数（主要用于测试用例）
        /// </summary>
        public CommentService(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        #region Create & Delete & Update

        /// <summary>
        /// 创建评论
        /// </summary>
        /// <param name="comment">待创建评论</param>
        /// <returns>创建成功返回true，否则返回false</returns>
        public bool Create(Comment comment)
        {
            //触发事件
            EventBus<Comment>.Instance().OnBefore(comment, new CommonEventArgs(EventOperationType.Instance().Create()));
            AuditService auditService = new AuditService();
            auditService.ChangeAuditStatusForCreate(comment.UserId, comment);


            //评论创建
            long commentId = Convert.ToInt64(commentRepository.Insert(comment));


            //更新父级ChildCount
            if (commentId > 0)
            {

                ICommentBodyProcessor commentBodyProcessor = DIContainer.Resolve<ICommentBodyProcessor>();
                comment.Body = commentBodyProcessor.Process(comment.Body, TenantTypeIds.Instance().Comment(), commentId, comment.UserId);
                commentRepository.Update(comment);

                commentRepository.UpdateChildCount(comment.ParentId, false);
                CountService countService = new CountService(comment.TenantTypeId);
                countService.ChangeCount(CountTypes.Instance().CommentCount(), comment.CommentedObjectId, comment.OwnerId, 1, true);
                //触发事件
                EventBus<Comment>.Instance().OnAfter(comment, new CommonEventArgs(EventOperationType.Instance().Create()));
                EventBus<Comment, AuditEventArgs>.Instance().OnAfter(comment, new AuditEventArgs(null, comment.AuditStatus));

            }

            return commentId > 0;
        }

        /// <summary>
        /// 删除评论 
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(long id)
        {
            Comment comment = commentRepository.Get(id);
            long parentID = 0;
            int count = 0;
            if (comment != null)
            {
                //触发事件
                EventBus<Comment>.Instance().OnBefore(comment, new CommonEventArgs(EventOperationType.Instance().Delete()));
                parentID = comment.ParentId;

                count = commentRepository.Delete(id);

                if (count > 0)
                {
                    commentRepository.UpdateChildCount(parentID, true);
                    CountService countService = new CountService(comment.TenantTypeId);
                    countService.ChangeCount(CountTypes.Instance().CommentCount(), comment.CommentedObjectId, comment.OwnerId, -1 - comment.ChildCount, true);

                    //触发事件
                    EventBus<Comment>.Instance().OnAfter(comment, new CommonEventArgs(EventOperationType.Instance().Delete()));
                }
            }
            return count > 0;
        }

        /// <summary>
        /// 批量删除评论
        /// </summary>
        /// <param name="ids">待删除的评论Id列表</param>
        /// <returns>返回删除的评论数量</returns>
        public int Delete(IEnumerable<long> ids)
        {
            int commentCount = 0;
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    Delete(id);
                    commentCount++;
                }
            }
            return commentCount;
        }

        /// <summary>
        /// 更新审核状态
        /// </summary>
        /// <param name="id">待更新id</param>
        /// <param name="isApproved">是否通过审核</param>
        public void UpdateAuditStatus(long id, bool isApproved)
        {
            Comment comment = commentRepository.Get(id);
            AuditStatus auditStatus = isApproved ? AuditStatus.Success : AuditStatus.Fail;
            if (comment.AuditStatus == auditStatus)
                return;
            AuditStatus oldAuditStatus = comment.AuditStatus;
            comment.AuditStatus = auditStatus;
            commentRepository.Update(comment);
            string operationType = isApproved ? EventOperationType.Instance().Approved() : EventOperationType.Instance().Disapproved();
            EventBus<Comment>.Instance().OnAfter(comment, new CommonEventArgs(operationType));
            EventBus<Comment, AuditEventArgs>.Instance().OnAfter(comment, new AuditEventArgs(oldAuditStatus, comment.AuditStatus));
        }

        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="ids">待被更新的Id集合</param>
        /// <param name="isApproved">是否通过审核</param>
        public void BatchUpdateAuditStatus(IEnumerable<long> ids, bool isApproved)
        {
            IEnumerable<Comment> comments = commentRepository.PopulateEntitiesByEntityIds(ids);
            AuditStatus auditStatus = isApproved ? AuditStatus.Success : AuditStatus.Fail;
            string operationType = isApproved ? EventOperationType.Instance().Approved() : EventOperationType.Instance().Disapproved();
            foreach (var comment in comments)
            {
                if (comment.AuditStatus == auditStatus)
                    continue;
                AuditStatus oldAuditStatus = comment.AuditStatus;
                comment.AuditStatus = auditStatus;
                commentRepository.Update(comment);

                EventBus<Comment>.Instance().OnAfter(comment, new CommonEventArgs(operationType));
                EventBus<Comment, AuditEventArgs>.Instance().OnAfter(comment, new AuditEventArgs(oldAuditStatus, comment.AuditStatus));
            }
        }


        /// <summary>
        /// 删除被评论对象的所有评论
        /// </summary>
        /// <remarks>
        /// 供被评论对象删除时调用
        /// </remarks>
        /// <param name="commentedObjectId"></param>
        /// <returns></returns>
        public int DeleteCommentedObjectComments(long commentedObjectId)
        {
            return commentRepository.DeleteCommentedObjectComments(commentedObjectId);
        }

        /// <summary>
        ///  删除用户发布的评论
        /// </summary>
        /// <remarks>
        /// 供用户删除时处理用户相关信息时调用
        /// </remarks>
        /// <param name="userId">UserId</param>
        /// <param name="reserveCommnetsAsAnonymous">true=保留用户发布的评论，但是修改为匿名用户；false=直接删除评论</param>
        /// <returns></returns>
        public int DeleteUserComments(long userId, bool reserveCommnetsAsAnonymous)
        {
            return commentRepository.DeleteUserComments(userId, reserveCommnetsAsAnonymous);
        }

        #endregion

        #region 列表

        /// <summary>
        /// 获取被评论对象的所有评论（用于删除被评论对象时的积分处理）
        /// </summary>
        /// <param name="commentedObjectId">被评论对象ID</param>
        /// <returns></returns>
        public IEnumerable<Comment> GetCommentedObjectComments(long commentedObjectId)
        {
            return commentRepository.GetCommentedObjectComments(commentedObjectId);
        }
        
        /// <summary>
        /// 获取单个评论实体
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <returns>评论</returns>
        public Comment Get(long id)
        {
            return commentRepository.Get(id);
        }

        //顶级评论每页记录数、子评论每页记录数可以分别配置（暂时放在Repository中）

        /// <summary>
        /// 获取顶级评论列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="sortBy">排序字段</param> 
        /// <returns></returns>
        public PagingDataSet<Comment> GetRootComments(string tenantTypeId, long commentedObjectId, int pageIndex, SortBy_Comment sortBy)
        {
            //排序：Id正序
            //缓存分区：CommentedObjectId
            //仅显示可公开对外显示的 PubliclyAuditStatus 

            return commentRepository.GetRootComments(tenantTypeId, commentedObjectId, pageIndex, sortBy);
        }

        /// <summary>
        /// 获取子级评论列表
        /// </summary>
        /// <param name="parentId">父评论Id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="sortBy">排序字段</param> 
        /// <returns></returns>
        public PagingDataSet<Comment> GetChildren(long parentId, int pageIndex, SortBy_Comment sortBy)
        {
            //排序：Id正序
            //缓存分区：ParentId
            //仅显示可公开对外显示的 PubliclyAuditStatus 
            return commentRepository.GetChildren(parentId, pageIndex, sortBy);
        }

        /// <summary>
        /// 获取拥有者的评论
        /// </summary>
        /// <param name="ownerId">评论拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id（如果为null，则获取该拥有者所有评论）</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagingDataSet<Comment> GetOwnerComments(long ownerId, string tenantTypeId, DateTime? startDate, DateTime? endDate, int pageIndex)
        {
            //排序：Id倒序
            //缓存分区：OwnerId
            return commentRepository.GetUserComments(ownerId, null, tenantTypeId, startDate, endDate, pageIndex);
        }

        /// <summary>
        /// 获取用户发布的评论
        /// </summary>
        /// <param name="userId">评论发布人UserId</param>
        /// <param name="tenantTypeId">租户类型Id（如果为null，则获取该拥有者所有评论）</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagingDataSet<Comment> GetUserComments(long userId, string tenantTypeId, DateTime? startDate, DateTime? endDate, int pageIndex)
        {
            //排序：Id倒序
            //缓存分区：UserId
            return commentRepository.GetUserComments(null, userId, tenantTypeId, startDate, endDate, pageIndex);
        }

        /// <summary>
        /// 获取前topNumber条评论
        /// </summary>
        ///<param name="ownerId">评论拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">获取的评论数量</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<Comment> GetTopComments(long ownerId, string tenantTypeId, int topNumber, SortBy_Comment sortBy = SortBy_Comment.DateCreated)
        {
            return commentRepository.GetTopComments(ownerId, tenantTypeId, topNumber, sortBy);
        }

        /// <summary>
        /// 查询用户评论
        /// </summary>
        /// <param name="publiclyAuditStatus">审核状态</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">评论发布人UserId</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagingDataSet<Comment> GetComments(PubliclyAuditStatus? publiclyAuditStatus, string tenantTypeId, long? userId, DateTime? startDate, DateTime? endDate, int pageSize, int pageIndex)
        {
            //排序：Id倒序
            //缓存分区：全局版本
            return commentRepository.GetComments(publiclyAuditStatus, tenantTypeId, userId, startDate, endDate, pageSize, pageIndex);
        }

        /// <summary>
        /// 获取一级评论在评论列表中的页码
        /// </summary>
        /// <param name="commentId">评论id</param>
        /// <returns>页码</returns>
        public int GetPageIndexForCommentInCommens(long commentId, string tenantType, long commentedObjectId, SortBy_Comment sortBy)
        {
            return commentRepository.GetPageIndexForCommentInCommens(commentId, tenantType, commentedObjectId, sortBy);
        }

        public int GetPageIndexForCommentInParentCommens(long commentId, long ParentId, SortBy_Comment sortBy)
        {
            return commentRepository.GetPageIndexForCommentInParentCommens(commentId, ParentId, sortBy);
        }

        #endregion

    }
}
