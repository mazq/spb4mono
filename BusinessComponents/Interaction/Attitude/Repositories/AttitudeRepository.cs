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

namespace Tunynet.Common
{
    /// <summary>
    /// 顶踩的数据访问
    /// </summary>
    public class AttitudeRepository : Repository<Attitude>, IAttitudeRepository
    {
        private IAttitudeSettingsManager attitudeSettingsManager = DIContainer.Resolve<IAttitudeSettingsManager>();
        private IAttitudeOnlySupportSettingsManager attitudeOnlySupportSettingsManager = DIContainer.Resolve<IAttitudeOnlySupportSettingsManager>();
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();



        /// <summary>
        /// 获取顶踩信息
        /// </summary>
        /// <param name="objectId">操作对象</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="isFromCache">是否从缓存中获取</param>
        public Attitude Get(long objectId, string tenantTypeId, bool isFromCache = true)
        {
            Attitude entity = null;

            string cacheKey = GetCacheKey_Attitude(tenantTypeId, objectId);
            if (isFromCache)
            {
                entity = cacheService.Get<Attitude>(cacheKey);
            }

            if (entity == null)
            {
                var sql = Sql.Builder.Where("ObjectId = @0 and  TenantTypeId = @1", objectId, tenantTypeId);
                entity = CreateDAO().FirstOrDefault<Attitude>(sql);

                if (entity != null && RealTimeCacheHelper.EnableCache)
                {
                    cacheService.Set(cacheKey, entity, CachingExpirationType.SingleObject);
                }
            }

            return entity;
        }

        /// <summary>
        /// 对操作对象进行顶操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">操作用户Id</param>
        /// <param name="mode">顶踩的操作模式</param>
        /// <returns>是否操作成功，Ture-成功</returns>
        public bool Support(long objectId, string tenantTypeId, long userId, AttitudeMode mode = AttitudeMode.Bidirection)
        {
            PetaPocoDatabase dao = CreateDAO();

            dao.OpenSharedConnection();
            AttitudeSettings attitudeSettings = attitudeSettingsManager.Get();
            AttitudeOnlySupportSettings attitudeOnlySupportSettings = attitudeOnlySupportSettingsManager.Get();
            bool returnValue = false;

            var sql = Sql.Builder;
            sql.Where("ObjectId = @0 and  TenantTypeId = @1", objectId, tenantTypeId);
            Attitude entity = dao.FirstOrDefault<Attitude>(sql);
            if (entity == null)
            {
                entity = Attitude.New();
                entity.TenantTypeId = tenantTypeId;
                entity.ObjectId = objectId;
                dao.Insert(entity);
            }

            #region 判断是否双向

            if (AttitudeMode.Bidirection == mode)
            {
                #region 判断是否可修改

                //判断是否可修改
                if (attitudeSettings.IsModify)
                {
                    #region 判断是否取消

                    //判断是否可取消
                    if (attitudeSettings.EnableCancel)
                    {
                        IList<Sql> sqls = new List<Sql>();
                        int affectCount = 0;
                        //判断是否有过操作记录
                        bool? isSupport = IsSupport(objectId, tenantTypeId, userId);
                        switch (isSupport)
                        {   //顶过记录
                            case true:
                                returnValue = false;
                                break;
                            //踩过记录
                            case false:
                                sqls.Add(Sql.Builder.Append(@"DELETE FROM tn_AttitudeRecords WHERE (UserId = @0) AND (TenantTypeId = @1) and (ObjectId = @2)"
                                                            , userId, tenantTypeId, objectId));
                                sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes
                                                              SET OpposeCount = OpposeCount - 1, Comprehensive = Comprehensive - @0
                                                              WHERE (ObjectId = @1) AND (TenantTypeId = @2)"
                                                            , attitudeSettings.OpposeWeights, objectId, tenantTypeId));

                                using (var transaction = dao.GetTransaction())
                                {
                                    affectCount = dao.Execute(sqls);
                                    transaction.Complete();
                                }

                                if (affectCount > 0)
                                {
                                    if (RealTimeCacheHelper.EnableCache)
                                    {
                                        EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);

                                        //更新缓存
                                        Get(objectId, tenantTypeId, false);

                                        //清除顶踩状态缓存
                                        cacheService.Remove(GetCacheKey_IsSupport(objectId, tenantTypeId, userId));
                                    }
                                    returnValue = true;
                                }
                                else
                                {
                                    returnValue = false;
                                }
                                break;

                            default:
                                sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes
                                                              SET SupportCount = SupportCount + 1, Comprehensive = Comprehensive + @0
                                                              WHERE (ObjectId = @1) AND (TenantTypeId = @2)"
                                                            , attitudeSettings.SupportWeights, objectId, tenantTypeId));
                                sqls.Add(Sql.Builder.Append(@"INSERT INTO tn_AttitudeRecords (ObjectId, UserId, TenantTypeId, IsSupport) VALUES (@0, @1, @2, 1)"
                                                            , objectId, userId, tenantTypeId));

                                using (var transaction = dao.GetTransaction())
                                {
                                    affectCount = dao.Execute(sqls);
                                    transaction.Complete();
                                }
                                if (affectCount > 0)
                                {
                                    if (RealTimeCacheHelper.EnableCache)
                                    {
                                        EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                        //更新缓存
                                        Get(objectId, tenantTypeId, false);

                                        cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), true, CachingExpirationType.SingleObject);
                                    }

                                    returnValue = true;
                                }
                                else
                                {
                                    returnValue = false;
                                }

                                break;
                        }
                    }
                    else
                    {
                        IList<Sql> sqls = new List<Sql>();
                        int affectCount = 0;
                        //判断是否有过操作记录
                        bool? isSupport = IsSupport(objectId, tenantTypeId, userId);
                        switch (isSupport)
                        {  //已经顶过
                            case true:
                                returnValue = false;
                                break;
                            //已经踩过
                            case false:
                                sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes
                                                              SET  OpposeCount = OpposeCount - 1 ,SupportCount = SupportCount + 1, Comprehensive = Comprehensive + @0 - @1
                                                              WHERE (ObjectId = @2) AND (TenantTypeId = @3)"
                                                             , attitudeSettings.SupportWeights, attitudeSettings.OpposeWeights, objectId, tenantTypeId
                                                                       ));
                                sqls.Add(Sql.Builder.Append(@"UPDATE tn_AttitudeRecords
                                                              SET IsSupport =1
                                                              WHERE(ObjectId = @0) AND (UserId = @1) AND (TenantTypeId = @2)"
                                                            , objectId, userId, tenantTypeId));

                                using (var transaction = dao.GetTransaction())
                                {
                                    affectCount = dao.Execute(sqls);
                                    transaction.Complete();
                                }
                                if (affectCount > 0)
                                {
                                    if (RealTimeCacheHelper.EnableCache)
                                    {
                                        EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                        //更新缓存
                                        Get(objectId, tenantTypeId, false);
                                        cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), true, CachingExpirationType.SingleObject);
                                    }

                                    returnValue = true;
                                }
                                else
                                {
                                    returnValue = false;
                                }
                                break;
                            default:
                                //没有操作过的记录就  添加记录
                                sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes
                                                              SET SupportCount = SupportCount + 1, Comprehensive =  Comprehensive + @0
                                                              WHERE (ObjectId = @1) AND (TenantTypeId = @2)"
                                                            , attitudeSettings.SupportWeights, objectId, tenantTypeId));
                                sqls.Add(Sql.Builder.Append(@"INSERT INTO tn_AttitudeRecords (ObjectId, UserId, TenantTypeId, IsSupport) VALUES (@0, @1, @2, 1)"
                                                            , objectId, userId, tenantTypeId));
                                using (var transaction = dao.GetTransaction())
                                {
                                    affectCount = dao.Execute(sqls);
                                    transaction.Complete();
                                }
                                if (affectCount > 0)
                                {
                                    if (RealTimeCacheHelper.EnableCache)
                                    {
                                        EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                        //更新缓存
                                        Get(objectId, tenantTypeId, false);
                                        cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), true, CachingExpirationType.SingleObject);
                                    }

                                    returnValue = true;
                                }
                                else
                                {
                                    returnValue = false;
                                }
                                break;
                        }
                    }

                    #endregion 判断是否取消
                }
                else
                {
                    //先判断一下是否有顶踩记录
                    IList<Sql> sqls = new List<Sql>();
                    int affectCount = 0;
                    //判断是否有过操作记录
                    bool? isSupport = IsSupport(objectId, tenantTypeId, userId);
                    if (isSupport == null)
                    {
                        //没有记录就  添加记录
                        sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes SET SupportCount = SupportCount + 1, Comprehensive = Comprehensive + @0
                                                                        WHERE (ObjectId = @2) AND (TenantTypeId = @3)"
                                                                , attitudeSettings.SupportWeights, attitudeSettings.OpposeWeights, objectId, tenantTypeId
                                                              ));
                        sqls.Add(Sql.Builder.Append(@"INSERT INTO tn_AttitudeRecords (ObjectId, UserId, TenantTypeId, IsSupport) VALUES (@0, @1, @2, 1)"
                                                                , objectId, userId, tenantTypeId));
                        using (var transaction = dao.GetTransaction())
                        {
                            affectCount = dao.Execute(sqls);
                            transaction.Complete();
                        }
                        if (affectCount > 0)
                        {
                            if (RealTimeCacheHelper.EnableCache)
                            {
                                EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                //更新缓存
                                Get(objectId, tenantTypeId, false);
                                cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), true, CachingExpirationType.SingleObject);
                            }

                            returnValue = true;
                        }
                        else
                        {
                            returnValue = false;
                        }
                    }
                }

                #endregion 判断是否可修改
            }
            else
            {  //单向

                #region 是否可取消操作

                //是否可取消操作
                if (attitudeOnlySupportSettings.IsCancel)
                {
                    IList<Sql> sqls = new List<Sql>();
                    int affectCount = 0;
                    //判断是否有过操作记录
                    bool? isSupport = IsSupport(objectId, tenantTypeId, userId);
                    switch (isSupport)
                    {
                        //顶过记录
                        case true:
                            sqls.Add(Sql.Builder.Append(@"DELETE FROM tn_AttitudeRecords WHERE UserId = @0 and TenantTypeId = @1 and ObjectId = @2"
                                                        , userId, tenantTypeId, objectId));
                            sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes
                                                          SET SupportCount = SupportCount-1, Comprehensive = SupportCount-1
                                                          WHERE ObjectId = @0 and TenantTypeId = @1", objectId, tenantTypeId));
                            using (var transaction = dao.GetTransaction())
                            {
                                affectCount = dao.Execute(sqls);
                                transaction.Complete();
                            }
                            if (affectCount > 0)
                            {
                                if (RealTimeCacheHelper.EnableCache)
                                {
                                    EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);//更新缓存
                                    Get(objectId, tenantTypeId, false);

                                    //清除顶踩状态缓存
                                    cacheService.Remove(GetCacheKey_IsSupport(objectId, tenantTypeId, userId));
                                }

                                returnValue = true;
                            }
                            else
                            {
                                returnValue = false;
                            }
                            break;
                        default:

                            sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes SET SupportCount = SupportCount+1, Comprehensive = SupportCount+1
                                                                        WHERE (ObjectId = @0) AND (TenantTypeId = @1)"
                                                                    , objectId, tenantTypeId));
                            sqls.Add(Sql.Builder.Append(@"INSERT INTO tn_AttitudeRecords (ObjectId, UserId, TenantTypeId, IsSupport) VALUES (@0, @1, @2, 1)"
                                                                    , objectId, userId, tenantTypeId));
                            using (var transaction = dao.GetTransaction())
                            {
                                affectCount = dao.Execute(sqls);
                                transaction.Complete();
                            }
                            if (affectCount > 0)
                            {
                                if (RealTimeCacheHelper.EnableCache)
                                {
                                    EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                    //更新缓存
                                    Get(objectId, tenantTypeId, false);
                                    cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), true, CachingExpirationType.SingleObject);
                                }

                                returnValue = true;
                            }
                            else
                            {
                                returnValue = false;
                            }
                            break;
                    }
                }
                else
                {
                    //不可取消的单向操作
                    IList<Sql> sqls = new List<Sql>();
                    int affectCount = 0;
                    //判断是否有过操作记录
                    bool? isSupport = IsSupport(objectId, tenantTypeId, userId);
                    if (isSupport == null)
                    {
                        sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes SET SupportCount = SupportCount + 1, Comprehensive = SupportCount + 1
                                                      WHERE (ObjectId = @0) AND (TenantTypeId = @1)"
                                                    , objectId, tenantTypeId));
                        sqls.Add(Sql.Builder.Append(@"INSERT INTO tn_AttitudeRecords (ObjectId, UserId, TenantTypeId, IsSupport) VALUES (@0, @1, @2, 1)"
                                                    , objectId, userId, tenantTypeId));
                        using (var transaction = dao.GetTransaction())
                        {
                            affectCount = dao.Execute(sqls);
                            transaction.Complete();
                        }
                        if (affectCount > 0)
                        {
                            if (RealTimeCacheHelper.EnableCache)
                            {
                                EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                //更新缓存
                                Get(objectId, tenantTypeId, false);
                                cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), true, CachingExpirationType.SingleObject);
                            }

                            returnValue = true;
                        }
                        else
                        {
                            returnValue = false;
                        }
                    }
                }

                #endregion 是否可取消操作
            }

            #endregion 判断是否双向

            dao.CloseSharedConnection();
            return returnValue;
        }

        /// <summary>
        /// 对操作对象进行踩操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">操作用户Id</param>
        /// <returns>是否操作成功，Ture-成功</returns>
        public bool Oppose(long objectId, string tenantTypeId, long userId)
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();
            AttitudeSettings attitudeSettings = attitudeSettingsManager.Get();
            bool returnValue = false;

            var sql = Sql.Builder;
            sql.Where("ObjectId = @0 and  TenantTypeId = @1", objectId, tenantTypeId);
            Attitude entity = dao.FirstOrDefault<Attitude>(sql);
            if (entity == null)
            {
                entity = Attitude.New();
                entity.TenantTypeId = tenantTypeId;
                entity.ObjectId = objectId;
                dao.Insert(entity);
            }

            #region 双向的踩

            //判断是否是双向

            #region 判断是否可修改

            //判断是否可修改
            if (attitudeSettings.IsModify)
            {
                #region 判断是否取消

                //判断是否可取消
                if (attitudeSettings.EnableCancel)
                {
                    //先判断一下是否有顶踩记录
                    IList<Sql> sqls = new List<Sql>();
                    int affectCount = 0;
                    //判断是否有过操作记录
                    bool? isSupport = IsSupport(objectId, tenantTypeId, userId);
                    switch (isSupport)
                    {   //顶过记录
                        case true:
                            sqls.Add(Sql.Builder.Append(@"DELETE FROM tn_AttitudeRecords WHERE (UserId =@0) AND (TenantTypeId = @1) and (ObjectId= @2)"
                                                             , userId, tenantTypeId, objectId));
                            sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes SET SupportCount = SupportCount-1, Comprehensive = Comprehensive - @0
                                                                        WHERE (ObjectId = @1) AND (TenantTypeId = @2)"
                                                         , attitudeSettings.SupportWeights, objectId, tenantTypeId));
                            affectCount = dao.Execute(sqls);
                            if (affectCount > 0)
                            {
                                if (RealTimeCacheHelper.EnableCache)
                                {
                                    EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                    //更新缓存
                                    Get(objectId, tenantTypeId, false);
                                    //清除顶踩状态缓存
                                    cacheService.Remove(GetCacheKey_IsSupport(objectId, tenantTypeId, userId));
                                }
                                returnValue = true;
                            }
                            else
                            {
                                returnValue = false;
                            }
                            break;
                        //踩过记录
                        case false:
                            returnValue = false;
                            break;

                        default:
                            sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes SET OpposeCount = OpposeCount+1, Comprehensive = Comprehensive + @0
                                                                        WHERE (ObjectId = @1) AND (TenantTypeId = @2)"
                                                                    , attitudeSettings.OpposeWeights, objectId, tenantTypeId
                                                                  ));
                            sqls.Add(Sql.Builder.Append(@"INSERT INTO tn_AttitudeRecords (ObjectId, UserId, TenantTypeId, IsSupport) VALUES (@0, @1, @2, 0)"
                                                                    , objectId, userId, tenantTypeId));
                            affectCount = dao.Execute(sqls);
                            if (affectCount > 0)
                            {
                                if (RealTimeCacheHelper.EnableCache)
                                {
                                    EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                    //更新缓存
                                    Get(objectId, tenantTypeId, false);
                                    cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), false, CachingExpirationType.SingleObject);
                                }

                                returnValue = true;
                            }
                            else
                            {
                                returnValue = false;
                            }
                            break;
                    }
                }

                else
                {
                    IList<Sql> sqls = new List<Sql>();
                    int affectCount = 0;
                    //判断是否有过操作记录
                    bool? isSupport = IsSupport(objectId, tenantTypeId, userId);
                    switch (isSupport)
                    {  //已经顶过
                        case true:
                            sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes
                                                                        SET  OpposeCount = OpposeCount +1 ,SupportCount = SupportCount-1
                                                                        , Comprehensive = Comprehensive - @0 + @1
                                                                        WHERE (ObjectId = @2) AND (TenantTypeId = @3)"
                                                              , attitudeSettings.SupportWeights, attitudeSettings.OpposeWeights, objectId, tenantTypeId
                                                            ));
                            sqls.Add(Sql.Builder.Append(@"UPDATE tn_AttitudeRecords SET IsSupport =0
                                                                        WHERE(ObjectId = @0) AND (UserId = @1) AND (TenantTypeId = @2)"
                                                                    , objectId, userId, tenantTypeId));
                            affectCount = dao.Execute(sqls);
                            if (affectCount > 0)
                            {
                                if (RealTimeCacheHelper.EnableCache)
                                {
                                    EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                    //更新缓存
                                    Get(objectId, tenantTypeId, false);
                                    cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), false, CachingExpirationType.SingleObject);
                                }

                                returnValue = true;
                            }
                            else
                            {
                                returnValue = false;
                            }
                            break;
                        //已经踩过
                        case false:
                            returnValue = false;
                            break;

                        default:
                            //没有记录就更新信息  添加记录
                            sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes SET OpposeCount = OpposeCount+1, Comprehensive = Comprehensive + @0
                                                                        WHERE (ObjectId = @1) AND (TenantTypeId = @2)"
                                                                    , attitudeSettings.OpposeWeights, objectId, tenantTypeId
                                                                  ));
                            sqls.Add(Sql.Builder.Append(@"INSERT INTO tn_AttitudeRecords (ObjectId, UserId, TenantTypeId, IsSupport) VALUES (@0, @1, @2, 0)"
                                                                    , objectId, userId, tenantTypeId));
                            affectCount = dao.Execute(sqls);
                            if (affectCount > 0)
                            {
                                if (RealTimeCacheHelper.EnableCache)
                                {
                                    EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                                    //更新缓存
                                    Get(objectId, tenantTypeId, false);
                                    cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), false, CachingExpirationType.SingleObject);
                                }

                                returnValue = true;
                            }
                            else
                            {
                                returnValue = false;
                            }
                            break;
                    }
                }

                #endregion 判断是否取消
            }
            else
            {
                //先判断一下是否有顶踩记录
                IList<Sql> sqls = new List<Sql>();
                int affectCount = 0;
                //判断是否有过操作记录
                bool? isSupport = IsSupport(objectId, tenantTypeId, userId);
                if (isSupport == null)
                {
                    //没有记录就更新信息  添加记录
                    sqls.Add(Sql.Builder.Append(@"UPDATE tn_Attitudes SET OpposeCount = OpposeCount+1, Comprehensive =  Comprehensive + @0
                                                                        WHERE (ObjectId = @1) AND (TenantTypeId = @2)"
                                                            , attitudeSettings.OpposeWeights, objectId, tenantTypeId
                                                          ));
                    sqls.Add(Sql.Builder.Append(@"INSERT INTO tn_AttitudeRecords (ObjectId, UserId, TenantTypeId, IsSupport) VALUES (@0, @1, @2, 0)"
                                                            , objectId, userId, tenantTypeId));
                    affectCount = dao.Execute(sqls);
                    if (affectCount > 0)
                    {
                        if (RealTimeCacheHelper.EnableCache)
                        {
                            EntityData.ForType(typeof(AttitudeRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                            //更新缓存
                            Get(objectId, tenantTypeId, false);
                            cacheService.Set(GetCacheKey_IsSupport(objectId, tenantTypeId, userId), false, CachingExpirationType.SingleObject);
                        }

                        returnValue = true;
                    }
                    else
                    {
                        returnValue = false;
                    }
                }
            }

            #endregion 判断是否可修改

            #endregion 双向的踩

            dao.CloseSharedConnection();
            return returnValue;
        }

        /// <summary>
        /// 用户当前操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">用户的UserId</param>
        /// <returns>用户当前所做的操作:True-顶,false-踩,null-未做任何操作</returns>
        public bool? IsSupport(long objectId, string tenantTypeId, long userId)
        {
            var sql = Sql.Builder;
            sql
            .Select("IsSupport")
            .From("tn_AttitudeRecords")
            .Where("ObjectId = @0 and  TenantTypeId = @1 and UserId = @2", objectId, tenantTypeId, userId);

            string cacheKey = GetCacheKey_IsSupport(objectId, tenantTypeId, userId);

            bool? isSupport = cacheService.Get(cacheKey) as bool?;
            if (isSupport == null)
            {
                //获取是否支持
                isSupport = CreateDAO().FirstOrDefault<bool?>(sql);
                if (isSupport != null)
                {
                    cacheService.Add(cacheKey, isSupport, CachingExpirationType.SingleObject);
                }
            }

            return isSupport;
        }

        /// <summary>
        /// 获取操作对象的Id集合
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<param name="sortBy">顶踩排序字段</param>
        ///<param name="pageSize">每页的内容数</param>
        ///<param name="pageIndex">页码</param>
        public PagingEntityIdCollection GetObjectIds(string tenantTypeId, SortBy_Attitude sortBy, int pageSize, int pageIndex)
        {
            var sql = Sql.Builder;
            sql.Select("ObjectId")
               .From("tn_Attitudes")
               .Where("TenantTypeId = @0", tenantTypeId);

            //是否按综合评价取倒序列表
            if (sortBy == SortBy_Attitude.Comprehensive_Desc)
            {
                sql.OrderBy("Comprehensive DESC ");
            }
            else
            {
                sql.OrderBy("SupportCount DESC ");
            }

            PagingEntityIdCollection objectIds = null;
            string cacheKey = GetCacheKey_ObjectIds(tenantTypeId, sortBy);
            objectIds = cacheService.Get<PagingEntityIdCollection>(cacheKey);
            if (objectIds == null)
            {
                objectIds = CreateDAO().FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "ObjectId", sql);
                cacheService.Add(cacheKey, objectIds, CachingExpirationType.ObjectCollection);
            }
            return objectIds;
        }

        /// <summary>
        /// 删除垃圾数据
        /// </summary>
        /// <param name="serviceKey">服务标识</param>
        public void DeleteTrashDatas()
        {
            IEnumerable<TenantType> tenantTypes = new TenantTypeService().Gets(MultiTenantServiceKeys.Instance().Attitude());

            List<Sql> sqls = new List<Sql>();
            sqls.Add(Sql.Builder.Append("delete from tn_AttitudeRecords where not exists (select 1 from tn_Users where UserId = tn_AttitudeRecords.UserId)"));

            foreach (var tenantType in tenantTypes)
            {
                Type type = Type.GetType(tenantType.ClassType);
                if (type == null)
                    continue;

                var pd = PetaPoco.Database.PocoData.ForType(type);
                sqls.Add(Sql.Builder.Append("delete from tn_Attitudes")
                                    .Where("not exists (select 1 from " + pd.TableInfo.TableName + " where ObjectId = " + pd.TableInfo.PrimaryKey + ") and TenantTypeId = @0"
                                    , tenantType.TenantTypeId));

                sqls.Add(Sql.Builder.Append("delete from tn_AttitudeRecords")
                                    .Where("not exists (select 1 from " + pd.TableInfo.TableName + " where ObjectId = " + pd.TableInfo.PrimaryKey + ") and TenantTypeId = @0"
                                    , tenantType.TenantTypeId));

            }

            CreateDAO().Execute(sqls);
        }

        /// <summary>
        /// 获取操作Id集合的CacheKey
        /// </summary>
        /// <param name="tenantTypeId"> 租户类型Id</param>
        /// <param name="sortBy">排序类型</param>
        /// <returns></returns>
        private string GetCacheKey_ObjectIds(string tenantTypeId, SortBy_Attitude sortBy)
        {
            return string.Format("ObjectIds::TenantTypeId-{0}:SortBy-{1}", tenantTypeId, Convert.ToInt32(sortBy));
        }

        /// <summary>
        /// 获取态度CacheKey
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="objectId">操作对象</param>
        /// <returns></returns>
        private string GetCacheKey_Attitude(string tenantTypeId, long objectId)
        {
            return string.Format("ObjectAttitude::Object:{0}-TenantTypeId:{1}", objectId, tenantTypeId);
        }

        /// <summary>
        /// 获取是否发表态度CacheKey
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string GetCacheKey_IsSupport(long objectId, string tenantTypeId, long userId)
        {
            return string.Format("IsSupport::Object:{0}-TenantTypeId:{1}-UserId:{2}", objectId, tenantTypeId, userId);
        }
    }
}