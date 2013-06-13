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
using System.Text;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 积分记录Repository
    /// </summary>
    public class PointRecordRepository : Repository<PointRecord>, IPointRecordRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        //PointRecord实体、列表 使用正常的缓存策略
        /// <summary>
        ///  清理积分记录
        /// </summary>
        /// <param name="beforeDays">清理beforeDays天以前的积分记录</param>
        /// <param name="cleanSystemPointRecords">是否也删除系统积分记录</param>
        public void CleanPointRecords(int beforeDays, bool cleanSystemPointRecords)
        {
            var sql = Sql.Builder;
            sql.Append("Delete from tn_PointRecords")
                .Where("DateCreated < @0 ", DateTime.UtcNow.AddDays(-beforeDays));
            //done:zhangp,by zhengw: 可以再简化下，既然两个分支都用到Database.Execute(sql);那就没有必要放在判断里面
            if (!cleanSystemPointRecords)
                sql.Where("UserId <> @0", 0);
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 查询用户积分记录
        /// </summary>
        /// <param name="userId">用户Id<remarks>系统积分的UserId=0</remarks></param>
        /// <param name="isIncome">是不是收入的积分</param>
        /// <param name="pointItemName">积分项目名称</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public PagingDataSet<PointRecord> GetPointRecords(long? userId, bool? isIncome, string pointItemName, DateTime? startDate, DateTime? endDate,int pageSize, int pageIndex)
        {
            //userId不为空时使用分区缓存
            //done:zhangp,by zhengw:当userId有效并且startDate、endDate无效时，再使用缓存
            //pointItemName:使用后模糊搜索
            var sql = Sql.Builder;
            sql.Select("*")
            .From("tn_PointRecords");
            if (pointItemName != null)
                sql.Where("PointItemName like @0", pointItemName + "%");
            if (isIncome.HasValue)
                sql.Where("IsIncome = @0", isIncome);
            if (userId.HasValue)
                sql.Where("UserId = @0", userId);           
            if (startDate.HasValue)
                sql.Where("DateCreated >= @0", startDate);
            if (endDate.HasValue)
                sql.Where("DateCreated < @0", endDate.Value.AddDays(1));

            sql.OrderBy("DateCreated desc");
            return GetPagingEntities(pageSize, pageIndex, sql);
        }
    }
}