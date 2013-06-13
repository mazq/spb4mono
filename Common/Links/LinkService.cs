//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tunynet;
using Tunynet.Common;
using System;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 友情链接业务逻辑
    /// </summary>
    public class LinkService
    {
        private ILinkRepository linkRepository;
        private CategoryService categoryService = new CategoryService();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public LinkService()
            : this(new LinkRepository())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="linkRepository">友情链接仓储实现</param>
        public LinkService(ILinkRepository linkRepository)
        {
            this.linkRepository = linkRepository;
        }
        #endregion

        #region 维护友情链接
        /// <summary>
        /// 创建友情链接
        /// </summary>
        /// <param name="link">友情链接实体</param>
        /// <returns></returns>
        public bool Create(LinkEntity link)
        {
            linkRepository.Insert(link);
            if (link.LinkId > 0)
            {
                link.DisplayOrder = link.LinkId;
                Update(link);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="link">友情链接实体</param>
        /// <returns></returns>
        public void Update(LinkEntity link)
        {
            link.LastModified = DateTime.UtcNow;
            linkRepository.Update(link);
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="link">友情链接实体</param>
        /// <returns></returns>
        public void Delete(LinkEntity link)
        {
            linkRepository.Delete(link);
            categoryService.ClearCategoriesFromItem(link.LinkId, 0, TenantTypeIds.Instance().Link());
        }
        #endregion

        #region 查询友情链接
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="linkId">友情链接标识</param>
        /// <returns></returns>
        public LinkEntity Get(long linkId)
        {
            return linkRepository.Get(linkId);
        }

        /// <summary>
        /// 获取站点友情链接(前台)
        /// </summary>
        /// <param name="categoryId">分类标识</param>
        /// <param name="topNumber">获取数量</param>
        /// <returns></returns>
        public IEnumerable<LinkEntity> GetsOfSite(long categoryId, int topNumber)
        {
            CategoryService categoryService = new CategoryService();
            long totalRecords = 0;
            IEnumerable<long> linkIds = categoryService.GetItemIds(categoryId, false, topNumber, 1, out totalRecords);
            IEnumerable<LinkEntity> links = linkRepository.PopulateEntitiesByEntityIds(linkIds).OrderBy(n => n.DisplayOrder);
            return links;
        }

        /// <summary>
        /// 获取站点友情链接(后台管理)
        /// </summary>
        /// <param name="categoryId">分类标识</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public IEnumerable<LinkEntity> GetsOfSiteForAdmin(long? categoryId)
        {
            return linkRepository.GetsOfSiteForAdmin(categoryId);
        }

        /// <summary>
        /// 获取Owner友情链接
        /// </summary>
        /// <param name="ownerType">拥有者类型</param>
        /// <param name="ownerId">拥有者标识</param>
        /// <param name="topNumber">获取数量</param>
        /// <returns></returns>
        public IEnumerable<LinkEntity> GetsOfOwner(int ownerType, long ownerId, int topNumber)
        {
            return linkRepository.GetsOfOwner(ownerType, ownerId, topNumber);
        }
        #endregion
    }
}
