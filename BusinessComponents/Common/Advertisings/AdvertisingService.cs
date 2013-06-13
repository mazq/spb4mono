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
using Fasterflect;
using Tunynet.Caching;
using Tunynet.UI;
using Tunynet.Events;
using Tunynet.Common.Repositories;
using System.IO;
using System.Drawing;

namespace Tunynet.Common
{
    /// <summary>
    /// 广告业务逻辑类
    /// </summary>
    public class AdvertisingService
    {
        #region 构造器

        private IAdvertisingRepository advertisingRepository;
        private IAdvertisingPositionRepository advertisingPositionRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public AdvertisingService()
            : this(new AdvertisingRepository(), new AdvertisingPositionRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="advertisingRepository">广告仓储接口</param>
        /// <param name="advertisingPositionRepository">广告位仓储接口</param>
        public AdvertisingService(IAdvertisingRepository advertisingRepository, IAdvertisingPositionRepository advertisingPositionRepository)
        {
            this.advertisingRepository = advertisingRepository;
            this.advertisingPositionRepository = advertisingPositionRepository;
        }

        #endregion

        #region 广告

        /// <summary>
        /// 创建广告
        /// </summary>
        /// <param name="advertising">广告</param>
        /// <param name="positionIds">广告位Id集合</param>
        /// <param name="stream">图片流</param>
        /// <returns></returns>
        public bool CreateAdvertising(Advertising advertising, IEnumerable<string> positionIds, Stream stream)
        {
            long advertisingId = 0;
            long.TryParse(advertisingRepository.Insert(advertising).ToString(), out advertisingId);
            if (advertisingId > 0)
            {
                AddPositionsToAdvertising(advertisingId, positionIds);
                advertising.UseredPositionCount = positionIds.Count();
                advertising.DisplayOrder = advertisingId;
                advertisingRepository.Update(advertising);
                UploadAdvertisingImage(advertising, stream);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新广告
        /// </summary>
        /// <param name="advertising">广告</param>
        /// <param name="positionIds">广告位Id集合</param>
        /// <param name="stream">图片流</param>
        /// <returns></returns>
        public void UpdateAdvertising(Advertising advertising, IEnumerable<string> positionIds, Stream stream)
        {
            advertising.UseredPositionCount = positionIds.Count();
            advertisingRepository.Update(advertising);
            ClearPositionsFromAdvertising(advertising.AdvertisingId);
            AddPositionsToAdvertising(advertising.AdvertisingId, positionIds);
            UploadAdvertisingImage(advertising, stream);
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        public bool DeleteAdvertising(long advertisingId)
        {
            Advertising advertising = advertisingRepository.Get(advertisingId);
            int result = advertisingRepository.Delete(advertising);
            if (result > 0)
            {
                ClearPositionsFromAdvertising(advertisingId);

                LogoService logoService = new LogoService(TenantTypeIds.Instance().Advertising());
                logoService.DeleteLogo(advertisingId);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="presentAreaKey">投放区域</param>
        /// <param name="positionId">广告位</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isExpired">是否过期</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <returns></returns>
        public PagingDataSet<Advertising> GetAdvertisingsForAdmin(string presentAreaKey, string positionId, DateTime? startDate, DateTime? endDate, bool? isExpired, bool? isEnable, int pageSize = 20, int pageIndex = 1)
        {
            return advertisingRepository.GetAdvertisingsForAdmin(presentAreaKey, positionId, startDate, endDate, isExpired, isEnable, pageSize, pageIndex);
        }

        /// <summary>
        /// 变更广告的排列顺序
        /// </summary>
        /// <param name="id">待调整的广告Id</param>
        /// <param name="referenceId">参照广告Id</param>
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            Advertising item = advertisingRepository.Get(id);
            Advertising referenceItem = advertisingRepository.Get(referenceId);

            long itemId = item.DisplayOrder;
            item.DisplayOrder = referenceItem.DisplayOrder;

            advertisingRepository.Update(item);

            referenceItem.DisplayOrder = itemId;
            advertisingRepository.Update(referenceItem);
        }

        /// <summary>
        /// 设置广告是否启用
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <param name="isEnable">是否启用</param>
        public void SetAdvertisingStatus(long advertisingId, bool isEnable)
        {
            Advertising advertising = advertisingRepository.Get(advertisingId);
            if (advertising.IsEnable != isEnable)
            {
                advertising.IsEnable = isEnable;
                advertisingRepository.Update(advertising);
            }
        }

        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        public Advertising GetAdvertising(long advertisingId)
        {
            return advertisingRepository.Get(advertisingId);
        }

        /// <summary>
        /// 获取广告统计数据
        /// </summary>
        /// <returns></returns>
        public long GetAdvertisingCount()
        {
            return advertisingRepository.GetAdvertisingCount();
        }

        /// <summary>
        /// 清除广告的所有广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        private void ClearPositionsFromAdvertising(long advertisingId)
        {
            advertisingRepository.ClearPositionsFromAdvertising(advertisingId);
        }

        /// <summary>
        /// 为广告批量设置广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <param name="positionIds">广告位Id集合</param>
        private void AddPositionsToAdvertising(long advertisingId, IEnumerable<string> positionIds)
        {
            advertisingRepository.AddPositionsToAdvertising(advertisingId, positionIds);
        }

        /// <summary>
        /// 根据广告Id取所有的广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        public IEnumerable<AdvertisingPosition> GetPositionsByAdvertisingId(long advertisingId)
        {
            return advertisingRepository.GetPositionsByAdvertisingId(advertisingId);
        }

        /// <summary>
        /// 上传广告图片
        /// </summary>
        /// <param name="advertising">广告实体</param>
        /// <param name="stream">图片流</param>
        private void UploadAdvertisingImage(Advertising advertising, Stream stream)
        {
            if (stream != null)
            {
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Advertising());
                advertising.AttachmentUrl = logoService.UploadLogo(advertising.AdvertisingId, stream);
                advertisingRepository.Update(advertising);
            }
        }

        /// <summary>
        /// 删除广告示意图
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        public void DeleteAdvertisingImage(long advertisingId)
        {
            LogoService logoService = new LogoService(TenantTypeIds.Instance().Advertising());
            logoService.DeleteLogo(advertisingId);

            Advertising advertising = GetAdvertising(advertisingId);
            if (advertising == null)
                return;
            if (advertising.AdvertisingType == AdvertisingType.Image)
                advertising.AttachmentUrl = string.Empty;

            advertisingRepository.Update(advertising);
        }

        #endregion

        #region 广告位

        /// <summary>
        /// 创建广告位
        /// </summary>
        /// <param name="position">广告位</param>
        /// <param name="stream">图片流</param>
        /// <returns></returns>
        public bool CreatePosition(AdvertisingPosition position, Stream stream)
        {
            advertisingPositionRepository.Insert(position);

            AdvertisingPosition advertisingPosition = advertisingPositionRepository.Get(position.PositionId);
            if (advertisingPosition != null)
            {
                UploadPositionImage(position, stream);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新广告位
        /// </summary>
        /// <param name="position">广告位</param>
        /// <param name="stream">图片流</param>
        public void UpdatePosition(AdvertisingPosition position, Stream stream)
        {
            advertisingPositionRepository.Update(position);
            UploadPositionImage(position, stream);
        }

        /// <summary>
        /// 删除广告位
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public bool DeletePosition(string positionId)
        {
            AdvertisingPosition position = advertisingPositionRepository.Get(positionId);
            int result = advertisingPositionRepository.Delete(position);
            if (result > 0)
            {
                LogoService logoService = new LogoService(TenantTypeIds.Instance().AdvertisingPosition());
                logoService.DeleteLogo(positionId);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取广告位列表
        /// </summary>
        /// <param name="presentAreaKey">投放区域</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        /// <param name="isEnable">是否启用</param>
        /// <returns></returns>
        public IEnumerable<AdvertisingPosition> GetPositionsForAdmin(string presentAreaKey, int height = 0, int width = 0, bool? isEnable = null)
        {
            return advertisingPositionRepository.GetPositionsForAdmin(presentAreaKey, height, width, isEnable);
        }

        /// <summary>
        /// 根据广告位Id取所有的广告
        /// </summary>
        /// <param name="positionId">广告位Id</param>
        /// <param name="isEnable">是否启用（null：全部广告,true：仅启用广告,false：仅禁用广告）</param>
        /// <returns></returns>
        public IEnumerable<Advertising> GetAdvertisingsByPositionId(string positionId,bool? isEnable=true)
        {
            return advertisingPositionRepository.GetAdvertisingsByPositionId(positionId,isEnable);
        }

        /// <summary>
        /// 上传示意图
        /// </summary>
        /// <param name="position">广告位</param>
        /// <param name="stream">图片流</param>
        private void UploadPositionImage(AdvertisingPosition position, Stream stream)
        {
            if (stream != null)
            {
                LogoService logoService = new LogoService(TenantTypeIds.Instance().AdvertisingPosition());
                position.FeaturedImage = logoService.UploadLogo(position.PositionId, stream);
                advertisingPositionRepository.Update(position);
            }
        }

        /// <summary>
        /// 删除广告位示意图
        /// </summary>
        /// <param name="positionId">广告位Id</param>
        public void DeletePositionImage(string positionId)
        {
            LogoService logoService = new LogoService(TenantTypeIds.Instance().AdvertisingPosition());
            logoService.DeleteLogo(positionId);

            AdvertisingPosition position = GetPosition(positionId);
            if (position == null)
                return;

            position.FeaturedImage = string.Empty;
            advertisingPositionRepository.Update(position);
        }

        /// <summary>
        /// 获取广告位
        /// </summary>
        /// <param name="positionId">广告位ID</param>
        /// <returns></returns>
        public AdvertisingPosition GetPosition(string positionId)
        {
            return advertisingPositionRepository.Get(positionId);
        }

        /// <summary>
        /// 获取所有的广告位尺寸
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllPositionSize()
        {
            List<string> positionSizes = new List<string>();

            IEnumerable<AdvertisingPosition> positions = advertisingPositionRepository.GetAll();
            foreach (var position in positions)
            {
                positionSizes.Add(position.Width.ToString()+"*"+position.Height.ToString());
            }

            return positionSizes.Distinct();
        }

        /// <summary>
        /// 获取广告位统计数据
        /// </summary>
        /// <returns></returns>
        public long GetPositionCount()
        {
            return advertisingPositionRepository.GetPositionCount();
        }

        #endregion


    }
}
