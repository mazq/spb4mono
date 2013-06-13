//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 星级评价业务逻辑
    /// </summary>
    public class RatingService
    {
        private IRatingRecordRepository ratingRecordRepository;
        private IRatingRepository ratingRepository;
        private IRatingGradeRepository ratingGradeRepository;
        private string tenantTypeId;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public RatingService(string tenantTypeId)
            : this(tenantTypeId, new RatingGradeRepository(), new RatingRecordRepository(), new RatingRepository())
        {
        }

        /// <summary>
        /// 测试用构造器
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ratingGradeRepository">评价选项数据访问</param>
        /// <param name="ratingRecordRepository">评价操作记录数据访问</param>
        /// <param name="ratingRepository">评价信息数据访问</param>
        public RatingService(string tenantTypeId, IRatingGradeRepository ratingGradeRepository, IRatingRecordRepository ratingRecordRepository, IRatingRepository ratingRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.ratingRecordRepository = ratingRecordRepository;
            this.ratingRepository = ratingRepository;
            this.ratingGradeRepository = ratingGradeRepository;
        }

        #region Rating

        /// <summary>
        /// 删除指定操作Id的评价信息
        /// </summary>
        /// <param name="objectId">操作Id</param>
        public void Delete(long objectId)
        {
            Rating rating = ratingRepository.Get(objectId, tenantTypeId);
            EventBus<Rating>.Instance().OnBefore(rating, new CommonEventArgs(EventOperationType.Instance().Delete()));
            ratingRepository.Delete(rating);
            ratingRecordRepository.ClearRatingRecordsOfObjectId(objectId, tenantTypeId);
            ratingGradeRepository.ClearRatingGradesOfObjectId(objectId, tenantTypeId);
            EventBus<Rating>.Instance().OnAfter(rating, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// 获取星级评价信息
        /// </summary>
        /// <param name="objectId">操作用户Id</param>
        public Rating Get(long objectId)
        {
            return ratingRepository.Get(objectId, tenantTypeId);
        }

        /// <summary>
        /// 对操作对象进行星级评价操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="userId">用户的UserId</param>
        /// <param name="rateNumber">星级类型</param>
        /// <param name="ownerId">拥有者Id</param>
        public bool Rated(long objectId, long userId, int rateNumber, long ownerId = 0)
        {

            Rating rating = Rating.New();
            rating.ObjectId = objectId;
            rating.OwnerId = ownerId;

            EventBus<Rating>.Instance().OnBefore(rating, new CommonEventArgs(EventOperationType.Instance().Create()));
            bool returnValue = ratingRepository.Rated(objectId, tenantTypeId, userId, rateNumber, ownerId);
            EventBus<Rating>.Instance().OnAfter(rating, new CommonEventArgs(EventOperationType.Instance().Create()));

            if (returnValue)
            {
                IEnumerable<RatingGrade> ratingGrades = ratingGradeRepository.GetRatingGrades(objectId, tenantTypeId);
                foreach (var ratingGrade in ratingGrades)
                    EntityData.ForType(typeof(RatingGrade)).RealTimeCacheHelper.IncreaseEntityCacheVersion(ratingGrade.Id);
            }

            return returnValue;
        }

        /// <summary>
        /// 用户当前的操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="userId">用户的userId</param>
        public bool IsRated(long objectId, long userId)
        {
            return ratingRepository.IsRated(objectId, tenantTypeId, userId);
        }

        /// <summary>
        /// 获取前N条操作对象的Id集合
        /// </summary>
        /// <param name="topNumber">前N条</param>
        /// <param name="ownerId">拥有者Id</param>
        public IEnumerable<long> GetTopObjectIds(int topNumber, long ownerId = 0)
        {
            return ratingRepository.GetTopObjectIds(topNumber, tenantTypeId, ownerId);
        }

        /// <summary>
        /// 获取操作对象Id集合
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="ownerId">拥有者Id</param>
        public PagingEntityIdCollection GetPagingObjectIds(int pageIndex = 1, long ownerId = 0)
        {
            return ratingRepository.GetPagingObjectIds(tenantTypeId, pageIndex, ownerId);
        }

        /// <summary>
        /// 删除用户的记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void ClearByUser(long userId)
        {
            ratingRecordRepository.ClearByUser(userId);
        }

        #endregion Rating

        #region RatingRecord

        /// <summary>
        /// 删除N天前的评价记录
        /// </summary>
        /// <param name="beforeDays">间隔天数</param>
        public void CleanRatingRecords(int beforeDays = 0)
        {
            if (beforeDays == 0)
            {
                ratingRecordRepository.Clean(null);
            }
            else
            {
                ratingRecordRepository.Clean(beforeDays);
            }
        }

        /// <summary>
        /// 获取前N条用户的星级评价记录信息
        /// </summary>
        /// <param name="objectId">操作Id</param>
        /// <param name="rateNumber"> 星级类型</param>
        /// <param name="topNumber">前N条</param>
        public IEnumerable<RatingRecord> GetTopRatingRecords(long objectId, int? rateNumber, int topNumber)
        {
            return ratingRecordRepository.GetTopRatingRecords(objectId, tenantTypeId, rateNumber, topNumber);
        }

        #endregion RatingRecord

        #region RatingGrade

        /// <summary>
        /// 获取指定评价选项信息
        /// </summary>
        /// <param name="objectId">评价数据Id</param>
        public IEnumerable<RatingGrade> GetRatingGrades(long objectId)
        {
            return ratingGradeRepository.GetRatingGrades(objectId, tenantTypeId);
        }

        #endregion RatingGrade
    }
}