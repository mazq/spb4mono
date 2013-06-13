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

namespace Tunynet.Common
{
    /// <summary>
    ///  顶踩的业务逻辑
    /// </summary>
    public class AttitudeService
    {
        private IAttitudeRepository attitudeRepository;
        private IAttitudeRecordRepository attitudeRecordRepository;
        private AttitudeMode mode;
        private string tenantTypeId;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public AttitudeService(string tenantTypeId)
            : this(tenantTypeId, new AttitudeRepository(), new AttitudeRecordRepository(), new AttitudeMode())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="mode">双向或单向模式</param>
        public AttitudeService(string tenantTypeId, AttitudeMode mode = AttitudeMode.Bidirection)
            : this(tenantTypeId, new AttitudeRepository(), new AttitudeRecordRepository(), mode)
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="attitudeRepository">顶踩数据访问</param>
        /// <param name="attitudeRecordRepository">顶踩记录数据访问</param>
        public AttitudeService(string tenantTypeId, IAttitudeRepository attitudeRepository, IAttitudeRecordRepository attitudeRecordRepository, AttitudeMode mode = AttitudeMode.Bidirection)
        {
            this.mode = mode;
            this.tenantTypeId = tenantTypeId;
            this.attitudeRepository = attitudeRepository;
            this.attitudeRecordRepository = attitudeRecordRepository;
        }

        #region Attitude

        /// <summary>
        /// 获取顶踩信息
        /// </summary>
        /// <param name="objectId">操作对象</param>
        /// <returns></returns>
        public Attitude Get(long objectId)
        {
            return attitudeRepository.Get(objectId, tenantTypeId);
        }

        /// <summary>
        /// 对操作对象进行顶操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="userId">操作用户Id</param>
        /// <param name="mode">顶踩的操作模式</param>
        /// <returns>是否操作成功，Ture-成功</returns>
        public bool Support(long objectId, long userId)
        {
            bool? isSupport = this.IsSupport(objectId, userId);

            EventBus<long, SupportOpposeEventArgs>.Instance().OnBefore(objectId, new SupportOpposeEventArgs(tenantTypeId, userId, !(isSupport.HasValue), EventOperationType.Instance().Support()));

            bool support = attitudeRepository.Support(objectId, tenantTypeId, userId, mode);

            EventBus<long, SupportOpposeEventArgs>.Instance().OnAfter(objectId, new SupportOpposeEventArgs(tenantTypeId, userId, !(isSupport.HasValue), EventOperationType.Instance().Support()));

            return support;
        }

        /// <summary>
        /// 对操作对象进行踩操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="userId">操作用户Id</param>
        /// <returns>是否操作成功，Ture-成功</returns>
        public bool Oppose(long objectId, long userId)
        {
            bool? isSupport = this.IsSupport(objectId, userId);

            EventBus<long, SupportOpposeEventArgs>.Instance().OnBefore(objectId, new SupportOpposeEventArgs(tenantTypeId, userId, !(isSupport.HasValue), EventOperationType.Instance().Oppose()));

            bool oppose = attitudeRepository.Oppose(objectId, tenantTypeId, userId);

            EventBus<long, SupportOpposeEventArgs>.Instance().OnAfter(objectId, new SupportOpposeEventArgs(tenantTypeId, userId, !(isSupport.HasValue), EventOperationType.Instance().Oppose()));

            return oppose;
        }

        /// <summary>
        /// 用户当前操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="userId">用户的UserId</param>
        /// <returns>用户当前所做的操作:True-顶,false-踩,null-未做任何操作</returns>
        public bool? IsSupport(long objectId, long userId)
        {
            return attitudeRepository.IsSupport(objectId, tenantTypeId, userId);
        }

      

        /// <summary>
        /// 获取操作对象的Id集合
        /// </summary>
        ///<param name="sortBy">顶踩排序字段</param>
        ///<param name="pageSize">每页的内容数</param>
        ///<param name="pageIndex">页码</param>
        public PagingEntityIdCollection GetPageObjectIds(SortBy_Attitude sortBy, int pageSize, int pageIndex)
        {
            return attitudeRepository.GetObjectIds(tenantTypeId, sortBy, pageSize, pageIndex);
        }

        #endregion Attitude

        #region AttitudeRecord

        /// <summary>
        /// 获取操作者的Id集合
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="IsSupport">用户是否支持（true为支持，false为反对）</param>
        /// <param name="topNumber">获取条数</param>
        public IEnumerable<long> GetTopOperatedUserIds(long objectId, bool IsSupport, int topNumber)
        {
            return attitudeRecordRepository.GetTopOperatedUserIds(objectId, tenantTypeId, IsSupport, topNumber);
        }

        /// <summary>
        /// 获取用户顶过的内容ID集合
        /// </summary>
        ///<param name="userId">用户Id</param>
        ///<param name="pageSize">每页的内容数</param>
        ///<param name="pageIndex">页码</param>
        public PagingEntityIdCollection GetPageObjectIdsByUserId(long userId, int pageSize, int pageIndex)
        {
            return attitudeRecordRepository.GetObjectIds(tenantTypeId,userId, pageSize, pageIndex);
        }

        #endregion AttitudeRecord
    }
}