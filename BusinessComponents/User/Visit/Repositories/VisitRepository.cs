//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 访客记录Repository
    /// </summary>
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        //创建访客记录队列
        private static Queue<long> CreateVisitQueue = new Queue<long>();

        //更新访客记录队列
        private static Queue<long> UpdateVisitQueue = new Queue<long>();

        // 缓存服务
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        #region Insert && Update && Delete

        /// <summary>
        /// 创建访客记录
        /// </summary>
        /// <param name="entity">准备插入的实体</param>
        /// <returns>插入数据的主键</returns>
        public override object Insert(Visit entity)
        {
            if (entity == null) return null;

            entity.Id = IdGenerator.Next();
   
            string cacheKey_TopVisit = GetCacheKey_TopVisits(entity.TenantTypeId, entity.ToObjectId);
            IEnumerable<long> visitsIds_IEnu = GetTopVisitIds(entity.TenantTypeId, entity.ToObjectId);

            if (visitsIds_IEnu == null) visitsIds_IEnu = new List<long>();
            List<long> visitsIds_TopVisit = visitsIds_IEnu.ToList();
            visitsIds_TopVisit.Insert(0, entity.Id);

            cacheService.Set(cacheKey_TopVisit, visitsIds_TopVisit, CachingExpirationType.UsualObjectCollection);

            CreateVisitQueue.Enqueue(entity.Id);
            this.OnInserted(entity);

            return entity.Id;
        }

        /// <summary>
        /// 更新访客记录
        /// </summary>
        /// <param name="entity">准备更新的实体</param>
        public override void Update(Visit entity)
        {
            if (entity == null) return;

            Visit visit = Get(entity.TenantTypeId, entity.VisitorId, entity.ToObjectId);

            entity.Id = visit.Id;
            entity.LastVisitTime = DateTime.UtcNow;
            UpdateVisitQueue.Enqueue(entity.Id);
            this.OnUpdated(entity);
        }

        /// <summary>
        /// 删除访客记录
        /// </summary>
        /// <param name="entity">访客记录</param>
        public override int Delete(Visit entity)
        {
            if (entity == null)
                return 0;

            var sql_Delete = PetaPoco.Sql.Builder;
            sql_Delete.Append("delete from tn_Visit where Id = @0", entity.Id);
            int affectedCount = CreateDAO().Execute(sql_Delete);

            this.OnDeleted(entity);

            return affectedCount;
       } 

        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="beforeDays">多少天以前的数据</param>
        public void Clean(int? beforeDays)
        {
            PetaPocoDatabase dao = CreateDAO();
            var sql_Delete = PetaPoco.Sql.Builder;
            if (beforeDays.HasValue && beforeDays.Value > 0)
            {
                DateTime cleanDate = DateTime.UtcNow.AddDays(-beforeDays.Value);
                sql_Delete.Append("delete from tn_Visit where LastVisitTime < @0", cleanDate);
                dao.Execute(sql_Delete);
            }
            else
            {
                sql_Delete.Append("delete from tn_Visit");
                dao.Execute(sql_Delete);
            }
        }

        /// <summary>
        /// 根据userid删除访问记录
        /// </summary>
        /// <param name="userId">用户的id</param>
        public void CleanByUser(long userId)
        {
            var sql_Delete = PetaPoco.Sql.Builder;
            sql_Delete.Append("delete from tn_Visit where VisitorId = @0", userId);
            sql_Delete.Append("delete from tn_Visit where ToObjectId = @0", userId);
            CreateDAO().Execute(sql_Delete);
        }

        /// <summary>
        /// 清空被访问对象的所有访问记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="toObjectId">被访问对象id</param>
        public void CleanByToObjectId(string tenantTypeId, long toObjectId)
        {
            var sql_Delete = PetaPoco.Sql.Builder;
            sql_Delete.Append("delete from tn_Visit where TenantTypeId = @0 and ToObjectId = @1", tenantTypeId, toObjectId);
            CreateDAO().Execute(sql_Delete);
        }

        /// <summary>
        /// 执行队列
        /// </summary>
        public void ExecQueue()
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();
            var tempCreateVisitQueue = new Queue<long>(CreateVisitQueue);
            CreateVisitQueue.Clear();

            while (tempCreateVisitQueue.Count > 0)
            {
                Visit visit = Get(tempCreateVisitQueue.Dequeue());
                if (visit == null)
                    continue;
                dao.Insert(visit);
            }
            //将队列中的数据更新到数据库
            var tempUpdateVisitQueue = new Queue<long>(UpdateVisitQueue);
            UpdateVisitQueue.Clear();
            while (tempUpdateVisitQueue.Count > 0)
            {
                Visit visit = Get(tempUpdateVisitQueue.Dequeue());
                if (visit == null)
                    continue;
                dao.Update(visit);
            }
            dao.CloseSharedConnection();
        }

        #endregion

        #region Get && Gets

        /// <summary>
        /// 获取访客记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="visitorId">访客用户Id</param>
        /// <param name="toObjectId">被访问对象Id</param>'
        /// <returns>查找到的访问实体</returns>
        public Visit Get(string tenantTypeId, long visitorId, long toObjectId)
        {

            IEnumerable<long> visitsIds = GetTopVisitIds(tenantTypeId, toObjectId);

            foreach (long id in visitsIds)
            {
                Visit visit = Get(id);
                if (visit == null)
                    continue;
                if (visit.VisitorId == visitorId)
                    return visit;
            }

            var sql_Select = PetaPoco.Sql.Builder;
            sql_Select.Where("TenantTypeId = @0", tenantTypeId)
                      .Where("VisitorId = @0", visitorId)
                      .Where("ToObjectId = @0", toObjectId);
            Visit visitFormDB = CreateDAO().FirstOrDefault<Visit>(sql_Select);

            return visitFormDB;
        }

        /// <summary>
        /// 获取访客记录（我去看过谁的内容）前topNumber条记录
        /// </summary>
        /// <param name="visitorId">访客用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">条数</param>
        /// <returns>访客记录列表（我去看过谁的内容）</returns>
        public IEnumerable<Visit> GetTopMyVisits(string tenantTypeId, long visitorId, int topNumber)
        {

            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    return GetCacheKey_TopMyVisits(tenantTypeId, visitorId);
                },
                () =>
                {
                    return PetaPoco.Sql.Builder
                                   .Where("TenantTypeId = @0", tenantTypeId)
                                   .Where("VisitorId = @0", visitorId);
                });
        }

        /// <summary>
        /// 获取访客记录（谁来看过我的内容）前topNumber条记录
        /// </summary>
        /// <param name="toObjectId">被访问对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">条数</param>
        /// <returns>访客记录列表（谁来看过我的内容）</returns>
        public IEnumerable<Visit> GetTopVisits(string tenantTypeId, long toObjectId, int topNumber)
        {
            IEnumerable<long> visitIds = GetTopVisitIds(tenantTypeId, toObjectId);
            IEnumerable<long> topVisitIds = null;
            if (visitIds != null)
                topVisitIds = visitIds.ToList().Take(topNumber);
            else
                return null;

            return PopulateEntitiesByEntityIds(topVisitIds);
        }

        #endregion

        #region Helper Method

        /// <summary>
        /// 获取前一千条谁来看过我的id集合
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="toObjectId">被访问对象Id</param>
        /// <returns></returns>
        private IEnumerable<long> GetTopVisitIds(string tenantTypeId, long toObjectId)
        {
            List<long> peic = null;
            string cacheKey = GetCacheKey_TopVisits(tenantTypeId, toObjectId);
            peic = cacheService.Get<List<long>>(cacheKey);
            if (peic == null)
            {
                var sql_select = PetaPoco.Sql.Builder;
                sql_select.Select("Id", SecondaryMaxRecords)
                          .From("tn_Visit")
                          .Where("ToObjectId = @0", toObjectId)
                          .Where("TenantTypeId = @0", tenantTypeId)
                          .OrderBy("LastVisitTime desc");

                IEnumerable<long> visitIds = CreateDAO().FetchTopPrimaryKeys(1000, "Id", sql_select).Cast<long>();
                if (visitIds != null)
                    peic = visitIds.ToList();

                cacheService.Add(cacheKey, peic, CachingExpirationType.UsualObjectCollection);

            }

            return peic;
        }

        /// <summary>
        /// 我去看过谁的缓存名称
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="visitorId">访客用户Id</param>
        /// <returns></returns>
        private string GetCacheKey_TopMyVisits(string tenantTypeId, long visitorId)
        {
            return string.Format("TopMyVisits::tenantTypeId:{0}-visitorId:{1}", tenantTypeId, visitorId);
        }

        /// <summary>
        /// 谁来看过的的缓存名称
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="toObjectId">被访问对象的id</param>
        /// <returns></returns>
        private string GetCacheKey_TopVisits(string tenantTypeId, long toObjectId)
        {
            return string.Format("TopVisits::tenantTypeId:{0}-toObjectId:{1}", tenantTypeId, toObjectId);
        }

        #endregion

    }
}
