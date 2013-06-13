//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 提醒记录数据访问
    /// </summary>
    public class ReminderRecordRepository : Repository<ReminderRecord>, IReminderRecordRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 创建提醒记录
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderMode">提醒方式</param>
        /// <param name="reminderInfoType">提醒信息类型</param>
        /// <param name="objectIds">提醒对象Id集合</param>
        public void CreateRecords(long userId, int reminderMode, int reminderInfoType, IEnumerable<long> objectIds)
        {
            PetaPocoDatabase dao = CreateDAO();

            dao.OpenSharedConnection();
            var sql = Sql.Builder;
            sql.Select("ObjectId")
               .From("tn_ReminderRecords")
               .Where("UserId=@0", userId)
               .Where("ReminderModeId=@0", reminderMode)
               .Where("ReminderInfoTypeId=@0", reminderInfoType);

            IEnumerable<object> oldObjectIds_object = dao.FetchFirstColumn(sql);
            IEnumerable<long> oldObjectIds = oldObjectIds_object.Cast<long>();
            foreach (var objectId in objectIds)
            {
                if (!oldObjectIds.Contains(objectId))
                {
                    ReminderRecord record = ReminderRecord.New();
                    record.UserId = userId;
                    record.ReminderModeId = reminderMode;
                    record.ReminderInfoTypeId = reminderInfoType;
                    record.ObjectId = objectId;
                    dao.Insert(record);
                }
            }

            dao.CloseSharedConnection();

            //更新缓存
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        /// <summary>
        /// 更新提醒记录（只更新最后提醒时间）
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        /// <param name="objectIds">提醒对象Id集合</param>
        public void UpdateRecoreds(long userId, int reminderModeId, int reminderInfoTypeId, IEnumerable<long> objectIds)
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();
            var sql = Sql.Builder;
            IEnumerable<ReminderRecord> records = this.GetRecords(userId, reminderModeId, reminderInfoTypeId);
            foreach (var objectId in objectIds)
            {
                sql.Append("Update tn_ReminderRecords set LastReminderTime = @0 where UserId = @1 and ReminderModeId = @2 and ReminderInfoTypeId = @3 and ObjectId = @4", DateTime.UtcNow, userId, reminderModeId, reminderInfoTypeId, objectId);
                dao.Execute(sql);
            }
            dao.CloseSharedConnection();
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        //方法是否正确
        /// <summary>
        /// 删除用户数据（删除用户的时候调用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            List<Sql> sql_Deletes = new List<Sql>();
            sql_Deletes.Add(PetaPoco.Sql.Builder.Append("delete from tn_ReminderRecords where UserId = @0", userId));
            sql_Deletes.Add(PetaPoco.Sql.Builder.Append("delete from tn_UserReminderSettings where UserId = @0", userId));
            CreateDAO().Execute(sql_Deletes);
        }

        /// <summary>
        /// 清除垃圾提醒记录
        /// </summary>
        /// <param name="storageDay">保留天数</param>
        public void DeleteTrashRecords(int storageDay)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("Delete from tn_ReminderRecords where DateCreated < @0 ", DateTime.UtcNow.AddDays(-storageDay));
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 获取用户所有的提醒记录
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        public IEnumerable<ReminderRecord> GetRecords(long userId, int reminderModeId, int reminderInfoTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
            cacheKey.AppendFormat("ReminderRecords::reminderModeId-{0}::reminderInfoTypeId-{1}", reminderModeId, reminderInfoTypeId);
            IEnumerable<long> recordIds = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());
            if (recordIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Id")
                    .From("tn_ReminderRecords")
                    .Where("UserId = @0", userId)
                    .Where("ReminderModeId =@0", reminderModeId)
                    .Where("ReminderInfoTypeId =@0", reminderInfoTypeId);

                recordIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey.ToString(), recordIds, CachingExpirationType.UsualObjectCollection);
            }

            return PopulateEntitiesByEntityIds<long>(recordIds);
        }
    }
}