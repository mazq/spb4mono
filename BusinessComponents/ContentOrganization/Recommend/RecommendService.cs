//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using System.IO;

namespace Tunynet.Common
{
    /// <summary>
    /// 推荐业务逻辑类
    /// </summary>
    public class RecommendService
    {
        #region 构造器

        private IRecommendItemRepository recommendItemRepository;
        private IRecommendItemTypeRepository recommendItemTypeRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public RecommendService()
            : this(new RecommendItemRepository(), new RecommendItemTypeRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="recommendItemRepository">推荐内容仓储接口</param>
        /// <param name="recommendItemTypeRepository">推荐类别仓储接口</param>
        public RecommendService(IRecommendItemRepository recommendItemRepository, IRecommendItemTypeRepository recommendItemTypeRepository)
        {
            this.recommendItemRepository = recommendItemRepository;
            this.recommendItemTypeRepository = recommendItemTypeRepository;
        }

        #endregion

        #region 推荐类别

        /// <summary>
        /// 创建推荐类别
        /// </summary>
        /// <param name="recommendType">推荐类别实体</param>
        /// <returns>创建成功返回true，失败返回false</returns>
        public bool CreateRecommendType(RecommendItemType recommendType)
        {
            RecommendItemType type = recommendItemTypeRepository.Get(recommendType.TypeId);
            if (type != null)
                return false;
            
            //已修改
            //这里不需要判断是否插入成功吗
            recommendItemTypeRepository.Insert(recommendType);
            RecommendItemType recommendItemType = recommendItemTypeRepository.Get(recommendType.TypeId);

            //创建成功
            if (recommendItemType != null)
            {
                EventBus<RecommendItemType>.Instance().OnAfter(recommendType, new CommonEventArgs(EventOperationType.Instance().Create()));
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新推荐类别
        /// </summary>
        /// <param name="recommendType">推荐类别实体</param>
        /// <returns>更新成功返回true，失败返回false</returns>
        public void UpdateRecommendType(RecommendItemType recommendType)
        {
            recommendItemTypeRepository.Update(recommendType);
            EventBus<RecommendItemType>.Instance().OnAfter(recommendType, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除推荐类别
        /// </summary>
        /// <param name="recommendTypeId">推荐类别Id</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public bool DeleteRecommendType(string recommendTypeId)
        {
            //设计要点
            //1、需要删除：推荐内容；
            RecommendItemType recommendType = recommendItemTypeRepository.Get(recommendTypeId);
            int result = recommendItemTypeRepository.Delete(recommendType);
            if (result > 0)
            {
                EventBus<RecommendItemType>.Instance().OnAfter(recommendType, new CommonEventArgs(EventOperationType.Instance().Delete()));
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取推荐类别
        /// </summary>
        /// <param name="recommendTypeId">推荐类别Id</param>
        public RecommendItemType GetRecommendType(string recommendTypeId)
        {
            return recommendItemTypeRepository.Get(recommendTypeId);
        }

        /// <summary>
        /// 获取推荐类别列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<RecommendItemType> GetRecommendTypes(string tenantTypeId)
        {
            //设计要点
            //1、需要维护缓存即时性，使用tenantTypeId分区版本

            return recommendItemTypeRepository.GetRecommendTypes(tenantTypeId);
        }

        #endregion

        #region 维护推荐内容

        /// <summary>
        /// 创建推荐内容
        /// </summary>
        /// <param name="item">推荐内容实体</param>
        /// <returns>创建成功返回true，失败返回false</returns>
        public bool Create(RecommendItem item)
        {
            //设计要点
            //1、需要触发的事件：OnAfter；
            //2、需确保itemId+recommendTypeId不存在才能创建，避免同一内容被重复推荐
            //3、DisplayOrder需要和主键Id保持一致；
            long itemId = 0;
            long.TryParse(recommendItemRepository.Insert(item).ToString(), out itemId);
            if (itemId > 0)
            {
                item.DisplayOrder = itemId;
                recommendItemRepository.Update(item);
                EventBus<RecommendItem>.Instance().OnAfter(item, new CommonEventArgs(EventOperationType.Instance().Create()));
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新推荐内容
        /// </summary>
        /// <param name="item">推荐内容实体</param>
        /// <returns>更新成功返回true，失败返回false</returns>
        public void Update(RecommendItem item)
        {
            //设计要点
            //1、需要触发的事件：OnAfter；
            recommendItemRepository.Update(item);
            EventBus<RecommendItem>.Instance().OnAfter(item, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 变更推荐内容的排列顺序
        /// </summary>
        /// <param name="id">待调整的推荐Id</param>
        /// <param name="referenceId">参照推荐Id</param>
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            //设计要点
            //1、交换id和referenceId对应推荐内容的DisplayOrder

            RecommendItem item = recommendItemRepository.Get(id);
            RecommendItem referenceItem = recommendItemRepository.Get(referenceId);
            long itemId = item.DisplayOrder;
            item.DisplayOrder = referenceItem.DisplayOrder;
            recommendItemRepository.Update(item);
            referenceItem.DisplayOrder = itemId;
            recommendItemRepository.Update(referenceItem);
        }

        /// <summary>
        /// 上传Logo
        /// </summary>
        /// <param name="recommendId">推荐Id</param>
        /// <param name="stream">Logo文件流</param>
        public void UploadLogo(long recommendId, Stream stream)
        {
            if (stream != null)
            {
                RecommendItem recommend = this.Get(recommendId);
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Recommend());
                recommend.FeaturedImage = logoService.UploadLogo(recommendId, stream);
                this.Update(recommend);
            }
        }

        /// <summary>
        /// 删除Logo
        /// </summary>
        /// <param name="recommendId">推荐Id</param>
        public void DeleteLogo(long recommendId)
        {
            LogoService logoService = new LogoService(TenantTypeIds.Instance().Recommend());
            logoService.DeleteLogo(recommendId);
        }

        /// <summary>
        /// 删除推荐内容
        /// </summary>
        /// <param name="recommendId">推荐Id</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public bool Delete(long recommendId)
        {
            //设计要点
            //1、需要触发的事件：OnAfter；
            //2、同时删除Logo
            RecommendItem item = recommendItemRepository.Get(recommendId);
            int result = recommendItemRepository.Delete(item);
            if (result > 0)
            {
                DeleteLogo(recommendId);
                EventBus<RecommendItem>.Instance().OnAfter(item, new CommonEventArgs(EventOperationType.Instance().Delete()));
            }
            return result > 0;
        }

        /// <summary>
        /// 删除推荐内容
        /// </summary>
        /// <param name="itemId">内容Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public bool Delete(long itemId, string tenantTypeId)
        {
            return recommendItemRepository.Delete(itemId, tenantTypeId);
        }

        /// <summary>
        /// 定期移除过期的推荐内容
        /// </summary>
        public void DeleteExpiredRecommendItems()
        {
            
            recommendItemRepository.DeleteExpiredRecommendItems();
        }

        #endregion

        #region 获取推荐内容

        /// <summary>
        /// 获取推荐内容
        /// </summary>
        /// <param name="recommendId">推荐内容Id</param>
        public RecommendItem Get(long recommendId)
        {
            return recommendItemRepository.Get(recommendId);
        }

        /// <summary>
        /// 获取推荐内容
        /// </summary>
        /// <param name="itemId">推荐内容Id</param>
        /// <param name="recommendTypeId">推荐类型Id</param>
        public RecommendItem Get(long itemId, string recommendTypeId)
        {
            return recommendItemRepository.Get(itemId, recommendTypeId);
        }

        /// <summary>
        /// 获取某种推荐类别下的前N条推荐内容
        /// </summary>
        /// <param name="topNumber">前N条</param>
        /// <param name="recommendTypeId">推荐类别Id</param>
        /// <returns></returns>
        public IEnumerable<RecommendItem> GetTops(int topNumber, string recommendTypeId)
        {
            //设计要点
            //1、需要使用缓存，并使用分区版本recommendTypeId
            return recommendItemRepository.GetTops(topNumber, recommendTypeId);
        }

        /// <summary>
        /// 获取某种推荐类别下的推荐内容分页集合
        /// </summary>
        /// <param name="topNumber">前N条</param>
        /// <param name="recommendTypeId">推荐类别Id</param>
        /// <returns></returns>
        public PagingDataSet<RecommendItem> Gets(string recommendTypeId, int pageIndex)
        {
            //设计要点
            //1、需要使用缓存，并使用分区版本recommendTypeId
            return recommendItemRepository.Gets(recommendTypeId, pageIndex);
        }

        /// <summary>
        /// 获取某条内容的所有推荐
        /// </summary>
        /// <param name="itemId">内容Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public IEnumerable<RecommendItem> Gets(long itemId, string tenantTypeId)
        {
            //设计要点
            //1、不需要使用缓存
            return recommendItemRepository.Gets(itemId, tenantTypeId);
        }

        /// <summary>
        /// 分页获取推荐内容后台管理列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="recommendTypeId">推荐类型Id</param>
        /// <param name="isLink">是否是外链</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<RecommendItem> GetsForAdmin(string tenantTypeId, string recommendTypeId, bool? isLink = null, int pageSize = 20, int pageIndex = 1)
        {
            //设计要点
            //1、不需要使用缓存
            return recommendItemRepository.GetsForAdmin(tenantTypeId, recommendTypeId, isLink, pageSize, pageIndex);
        }

        #endregion
    }
}
