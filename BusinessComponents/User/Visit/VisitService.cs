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
using Tunynet.Common.Repositories;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户访客记录业务逻辑类
    /// </summary>
    public class VisitService
    {

        private IVisitRepository visitRepository;
        private string tenantTypeId;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public VisitService(string tenantTypeId)
            : this(tenantTypeId, new VisitRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>        
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="visitRepository"><see cref="IVisitRepository"/></param>
        public VisitService(string tenantTypeId, IVisitRepository visitRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.visitRepository = visitRepository;
        }

        #region Visit

        /// <summary>
        /// 访客访问了某内容
        /// </summary>                
        /// <param name="visitorId">访客用户Id</param>
        /// <param name="visitor">访客名称</param>
        /// <param name="toObjectId">被访问对象Id</param>
        /// <param name="toObjectName">被访问对象名称</param>
        /// <remarks>如果记录已经存在，则更新</remarks>
        public void CreateVisit(long visitorId, string visitor, long toObjectId, string toObjectName)
        {
            Visit visit = visitRepository.Get(tenantTypeId, visitorId, toObjectId);
            if (visit == null)
            {
                visit = Visit.New();
                visit.TenantTypeId = tenantTypeId;
                visit.VisitorId = visitorId;
                visit.Visitor = visitor;
                visit.ToObjectId = toObjectId;
                visit.ToObjectName = toObjectName;
                EventBus<Visit>.Instance().OnBefore(visit, new CommonEventArgs(EventOperationType.Instance().Create()));
                visitRepository.Insert(visit);
                EventBus<Visit>.Instance().OnAfter(visit, new CommonEventArgs(EventOperationType.Instance().Create()));
            }
            else
            {
                visit.Visitor = visitor;
                visit.ToObjectName = toObjectName;
                visit.LastVisitTime = DateTime.UtcNow;
                EventBus<Visit>.Instance().OnBefore(visit, new CommonEventArgs(EventOperationType.Instance().Update()));
                visitRepository.Update(visit);
                EventBus<Visit>.Instance().OnAfter(visit, new CommonEventArgs(EventOperationType.Instance().Update()));
            }
        }

        /// <summary>
        /// 删除访客记录
        /// </summary>
        /// <param name="Id">访客记录Id</param>
        public void Delete(long Id)
        {
            Visit visit = visitRepository.Get(Id);
            if (visit == null)
                return;
            EventBus<Visit>.Instance().OnBefore(visit, new CommonEventArgs(EventOperationType.Instance().Update()));
            visitRepository.Delete(visit);
            EventBus<Visit>.Instance().OnAfter(visit, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除N天前的访客记录
        /// </summary>
        /// <param name="beforeDays">间隔天数</param>
        public void Clean(int? beforeDays = null)
        {

            visitRepository.Clean(beforeDays);
        }

        /// <summary>
        /// 根据用户删除访问记录
        /// </summary>
        public void CleanByUser(long userId)
        {
            visitRepository.CleanByUser(userId);
        }

        /// <summary>
        /// 删除被访问对象的所有id
        /// </summary>
        /// <param name="toObjectId"></param>
        public void CleanByToObjectId(long toObjectId)
        {
            visitRepository.CleanByToObjectId(tenantTypeId, toObjectId);
        }

        /// <summary>
        /// 获取我的访客记录（我去看过谁的内容）前topNumber条记录
        /// </summary>
        /// <param name="visitorId">访客用户Id</param>
        /// <param name="topNumber">条数</param>
        /// <returns>访客记录列表（我去看过谁的内容）</returns>
        public IEnumerable<Visit> GetTopMyVisits(long visitorId, int topNumber)
        {
            return visitRepository.GetTopMyVisits(tenantTypeId, visitorId, topNumber);
        }

        /// <summary>
        /// 获取访客记录（谁来看过我的内容）前topNumber条记录
        /// </summary>
        /// <param name="toObjectId">被访问对象Id</param>
        /// <param name="topNumber">条数</param>
        /// <returns>访客记录列表（谁来看过我的内容）</returns>
        public IEnumerable<Visit> GetTopVisits(long toObjectId, int topNumber)
        {
            return visitRepository.GetTopVisits(tenantTypeId, toObjectId, topNumber);
        }



        #endregion

    }
}
