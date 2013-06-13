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
using Tunynet;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 身份认证申请Repository
    /// </summary>
    public class IdentificationRepository : Repository<Identification>, IIdentificationRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取身份认证的分页集合
        /// </summary>
        public PagingDataSet<Identification> GetIdentifications(IdentificationQuery query, int pageIndex, int pageSize)
        {
            Sql sql = Sql.Builder.Select("*").From("spb_Identifications");
            if (query.UserId>0)
            {
                sql.Where("UserId = @0", query.UserId);
            }
            if (query.IdentificationTypeId > 0)
            {
                sql.Where("IdentificationTypeId = @0", query.IdentificationTypeId);
            }
            if (query.startTime.HasValue)
            {
                sql.Where("DateCreated >= @0", query.startTime.Value);
            }
            if (query.endTime.HasValue)
            {
                sql.Where("DateCreated < @0", query.endTime.Value.AddDays(1));
            }
            if (query.identificationStatus.HasValue)
            {
                sql.Where("Status=@0", query.identificationStatus.Value);
            }
            sql.OrderBy("DateCreated desc");
            return GetPagingEntities(pageSize, pageIndex, sql);
        }

        /// <summary>
        /// 获取某人的某项(或所有)认证实体
        /// </summary>
        public List<Identification> GetUserIdentifications(long userId, long identificationTypeId)
        {
            //构建cacheKey
            StringBuilder cacheKey = new StringBuilder();
            string userIdCacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId);
            cacheKey.AppendFormat("Identifications::identificationTypeId-{0}:userIdCacheKey-{1}", identificationTypeId, userIdCacheKey);

            //先从缓存里取归档项目列表，如果缓存里没有就去数据库取
            List<Identification> identifications = cacheService.Get<List<Identification>>(cacheKey.ToString());
            if (identifications == null)
            {
                identifications = new List<Identification>();
                var sql = Sql.Builder.Select("*").From("spb_Identifications");
                sql.Where("UserId=@0", userId);
                if (identificationTypeId > 0)
                {
                    sql.Where("IdentificationTypeId=@0", identificationTypeId);
                }
                identifications = CreateDAO().Fetch<Identification>(sql);

                //加入缓存
                cacheService.Add(cacheKey.ToString(), identifications, CachingExpirationType.ObjectCollection);
            }
            return identifications;
        }

        /// <summary>
        /// 获取身份认证标识
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="">是否只获取成功的认证状态</param>
        /// <returns></returns>
        public IEnumerable<IdentificationType> GetIdentificationTypes(long userId,bool status = true)
        {
            //构建cacheKey
            StringBuilder cacheKey = new StringBuilder();
            string userIdCacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId);
            cacheKey.AppendFormat("IdentificationTypeIcons::userIdCacheKey-{0}:status-{1}",userIdCacheKey,status);

            //先从缓存里取，如果缓存里没有就去数据库取
            IEnumerable<IdentificationType> identificationTypes = cacheService.Get<IEnumerable<IdentificationType>>(cacheKey.ToString());
            if (identificationTypes == null)
            {
                string strSql = @"select a.* from spb_IdentificationTypes a inner join spb_Identifications b on a.IdentificationTypeId=b.IdentificationTypeId and b.UserId=" + userId.ToString() + " and a.Enabled=1 ";
                if (status)
                {
                    strSql += "and b.Status=" + (int)IdentificationStatus.success;
                }
                var sql = Sql.Builder.Append(strSql);
                identificationTypes = CreateDAO().Fetch<IdentificationType>(sql);
                //加入缓存
                cacheService.Add(cacheKey.ToString(), identificationTypes, CachingExpirationType.ObjectCollection);
            }
            return identificationTypes;
        }
    }
}
