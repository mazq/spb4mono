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
using System.Text.RegularExpressions;
namespace Tunynet.Common
{
    /// <summary>
    /// 标签业务逻辑类
    /// </summary>
    public class TagService<T> where T : Tag
    {
        #region private item

        private ITagRepository<T> tagRepository;
        private IItemInTagRepository itemInTagRepository;
        private ITagInOwnerRepository tagInOwnerReposiory;
        private IRelatedTagRepository relatedTagRepository;
        private ITagInGroupRepository tagInGroupRepository;
        private ITagGroupRepository tagGroupRepository;

        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        #endregion

        private string tenantTypeId;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public TagService(string tenantTypeId)
            : this(tenantTypeId, new TagRepository<T>(), new ItemInTagRepository(), new TagInOwnerRepository(), new RelatedTagRepository(), new TagInGroupRepository(), new TagGroupRepository())
        {
        }

        /// <summary>
        /// 可设置repository的构造函数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="tagRepository">标签Repository</param>
        /// <param name="itemInTagRepository">内容与标签关系Repository</param>
        /// <param name="tagInOwnerReposiory">标签与拥有者关系Repository</param>
        /// <param name="tagGroupRepository">标签分组Repository</param>
        /// <param name="tagInGroupRepository">标签与分组关系Repository</param>
        /// <param name="relatedTagRepository"></param>
        public TagService(string tenantTypeId, ITagRepository<T> tagRepository, IItemInTagRepository itemInTagRepository, ITagInOwnerRepository tagInOwnerReposiory, IRelatedTagRepository relatedTagRepository, ITagInGroupRepository tagInGroupRepository, ITagGroupRepository tagGroupRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.tagRepository = tagRepository;
            this.itemInTagRepository = itemInTagRepository;
            this.tagInOwnerReposiory = tagInOwnerReposiory;
            this.relatedTagRepository = relatedTagRepository;
            this.tagGroupRepository = tagGroupRepository;
            this.tagInGroupRepository = tagInGroupRepository;
        }

        /// <summary>
        /// 用于标签分割的字符数组
        /// </summary>
        /// <remarks>
        /// 可以在添加标签时用户SplitCharacters中的字符做分割一次录入多个标签
        /// </remarks>
        public static readonly char[] SplitCharacters = new char[] { ',', ';', '，', '；', ' ' };

        /// <summary>
        /// Url特殊字符
        /// </summary>
        private static readonly char[] URLSpecialCharacters = new char[] { '%', '/', '?', '&', '*', '-', ':' };

        /// <summary>
        /// 标签云系数
        /// </summary>
        private static float[] siteTagLevelPartitions = new float[] { 0.0F, 0.01F, 0.04F, 0.09F, 0.16F, 0.25F, 0.36F, 0.49F, 0.64F, 0.81F };

        #region Tags

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="tag">待创建的标签</param>
        /// <param name="logoStream">标题图文件流</param>
        /// <returns>创建成功返回true，否则返回false</returns>
        public bool Create(T tag, Stream logoStream = null)
        {
            //创建数据前，触发相关事件
            EventBus<T>.Instance().OnBefore(tag, new CommonEventArgs(EventOperationType.Instance().Create()));
            long tagId = Convert.ToInt64(tagRepository.Insert(tag));

            if (tagId > 0)
            {
                AddTagInOwner(tag.TagName, tag.TenantTypeId, tag.OwnerId);
                if (logoStream != null)
                {
                    //上传Logo
                    LogoService logoService = new LogoService(TenantTypeIds.Instance().Tag());
                    tag.FeaturedImage = logoService.UploadLogo(tagId, logoStream);
                    tagRepository.Update(tag);
                }
                //若创建成功，触发创建后相关事件
                EventBus<T>.Instance().OnAfter(tag, new CommonEventArgs(EventOperationType.Instance().Create()));
                return true;
            }

            return false;
        }


        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="tag">待创建的标签</param>
        /// <param name="logoStream">标题图文件流</param>
        /// <returns></returns>
        public void Update(T tag, Stream logoStream = null)
        {
            //若更新据前，触发相关事件
            EventBus<T>.Instance().OnBefore(tag, new CommonEventArgs(EventOperationType.Instance().Update()));

            //上传Logo
            if (logoStream != null)
            {
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Tag());
                tag.FeaturedImage = logoService.UploadLogo(tag.TagId, logoStream);
            }

            tagRepository.Update(tag);

            //若更新成功，触发创建后相关事件
            EventBus<T>.Instance().OnAfter(tag, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="tagId">标签Id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(long tagId)
        {
            T tag = tagRepository.Get(tagId);

            int affectCount = 0;

            if (tag != null)
            {
                //删除数据前，触发相关事件
                EventBus<T>.Instance().OnBefore(tag, new CommonEventArgs(EventOperationType.Instance().Delete()));

                affectCount = tagRepository.Delete(tag);
                if (affectCount > 0)
                {
                    //删除Logo
                    LogoService logoService = new LogoService(TenantTypeIds.Instance().Tag());
                    logoService.DeleteLogo(tagId);

                    //若删除成功，触发删除后相关事件
                    EventBus<T>.Instance().OnAfter(tag, new CommonEventArgs(EventOperationType.Instance().Delete()));

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="ids">评论Id列表</param>
        /// <param name="isApproved">审核状态</param>
        public void UpdateAuditStatus(IEnumerable<long> ids, bool isApproved)
        {
            tagRepository.UpdateAuditStatus(ids, isApproved);
        }

        /// <summary>
        /// 获取Tag
        /// </summary>
        /// <param name="tagId">标签Id</param>
        public T Get(long tagId)
        {
            return tagRepository.Get(tagId);
        }

        /// <summary>
        /// 获取标签实体
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public T Get(string tagName)
        {
            return tagRepository.Get(tagName, tenantTypeId);
        }

        /// <summary>
        /// 获取前N个标签
        /// </summary>
        ///<param name="topNumber">前N条数据</param>
        ///<param name="sortBy">标签排序字段</param>
        /// <returns>{Key:标签实体,Value:标签级别}</returns>
        public Dictionary<T, int> GetTopTags(int topNumber, SortBy_Tag? sortBy)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append(EntityData.ForType(typeof(T)).RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
            cacheKey.AppendFormat("TagCloud-TenantTypeId:{0}", tenantTypeId);
            cacheKey.AppendFormat("TagCloud-TopNumber:{0}", topNumber);

            Dictionary<T, int> tagCloud = cacheService.Get<Dictionary<T, int>>(cacheKey.ToString());

            if (tagCloud == null)
            {
                tagCloud = new Dictionary<T, int>();
                List<T> tags = tagRepository.GetTopTags(tenantTypeId, topNumber, null, sortBy, true).ToList();

                float x = 0;
                foreach (T tag in tags)
                {
                    x = (float)1 / (tag.ItemCount == 0 ? 1 : tag.ItemCount);

                    for (int j = 0; j < 9; j++)
                    {
                        if (x >= siteTagLevelPartitions[j] && x < siteTagLevelPartitions[j + 1])
                        {
                            tagCloud[tag] = 9 - j;
                            break;
                        }
                    }

                    if (x >= siteTagLevelPartitions[9])
                        tagCloud[tag] = 0;
                }

                tagCloud = tagCloud.OrderBy(n => n.Key.TagName).ToDictionary(k => k.Key, v => v.Value);
                cacheService.Add(cacheKey.ToString(), tagCloud, CachingExpirationType.ObjectCollection);
            }
            return tagCloud;
        }

        /// <summary>
        /// 获取前N个标签
        /// </summary>
        ///<param name="topNumber">获取数据的条数</param>
        ///<param name="isFeatured">是否为特色标签</param>
        ///<param name="sortBy">标签排序字段</param>
        public IEnumerable<T> GetTopTags(int topNumber, bool? isFeatured, SortBy_Tag? sortBy)
        {

            return tagRepository.GetTopTags(tenantTypeId, topNumber, isFeatured, sortBy, false);
        }

        /// <summary>
        /// 获取用户的前N个标签
        /// </summary>
        ///<param name="topNumber">前N条数据</param>
        ///<param name="ownerId">拥有者Id</param>
        /// <returns>{Key:标签实体,Value:标签级别}</returns>
        public Dictionary<TagInOwner, int> GetOwnerTopTags(int topNumber, long ownerId)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append(EntityData.ForType(typeof(TagInOwner)).RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
            cacheKey.AppendFormat("TagCloud-TenantTypeId:{0}", tenantTypeId);

            Dictionary<TagInOwner, int> tagCloud = cacheService.Get<Dictionary<TagInOwner, int>>(cacheKey.ToString());

            if (tagCloud == null)
            {
                tagCloud = new Dictionary<TagInOwner, int>();
                IEnumerable<TagInOwner> tags = tagInOwnerReposiory.GetTopTagInOwners(ownerId, tenantTypeId, null, topNumber);

                int i = 0;
                float x = 0;
                foreach (TagInOwner tag in tags)
                {
                    x = (float)i / tags.Count();

                    for (int j = 0; j < 9; j++)
                    {
                        if (x >= siteTagLevelPartitions[j] && x < siteTagLevelPartitions[j + 1])
                        {
                            tagCloud[tag] = 9 - j;
                            break;
                        }
                    }

                    if (x >= siteTagLevelPartitions[9])
                        tagCloud[tag] = 0;

                    i++;
                }

                tagCloud = tagCloud.OrderBy(n => n.Key.TagName).ToDictionary(k => k.Key, v => v.Value);
                cacheService.Add(cacheKey.ToString(), tagCloud, CachingExpirationType.ObjectCollection);
            }
            return tagCloud;
        }

        /// <summary>
        /// 获取前N个标签名
        /// </summary>
        /// <remarks>用于智能提示</remarks>
        ///<param name="keyword">标签名称关键字</param>
        ///<param name="topNumber">前N条数据</param>
        public IEnumerable<string> GetTopTagNames(string keyword, int topNumber)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;

            IEnumerable<T> tags = GetTopTags(1000, null, SortBy_Tag.ItemCountDesc);
            IEnumerable<string> tagNames = null;

            if (tags != null)
            {
                tagNames = tags.Select(n => n.TagName).Where(n => n.Contains(keyword.Trim())).Take(topNumber);
            }

            return tagNames;
        }

        /// <summary>
        /// 获取前N个标签名
        /// </summary>
        /// <remarks>用于智能提示</remarks>
        ///<param name="ownerId">拥有者Id</param>
        ///<param name="keyword">标签名称关键字</param>
        ///<param name="topNumber">前N条数据</param>
        public IEnumerable<string> GetTopTagNames(int ownerId, string keyword, int topNumber)
        {
            List<string> tagNames = new List<string>();

            IEnumerable<TagInOwner> tagInOwners = GetOwnerTags(ownerId);
            if (tagInOwners != null && tagInOwners.Count() > 0)
            {
                tagNames.AddRange(tagInOwners.Select(n => n.TagName).Where(n => n.Contains(keyword)));
            }

            if (tagNames.Count() == 0 || tagNames.Count() < topNumber)
            {
                IEnumerable<string> tempTagNames = tagRepository.GetTopTagNames(tenantTypeId, ownerId, keyword, 1000);

                if (tempTagNames != null && tempTagNames.Count() > 0)
                {
                    tagNames.AddRange(tempTagNames.Where(n => n.Contains(keyword)).Take(topNumber - tagNames.Count()));
                }
            }

            return tagNames;
        }

        /// <summary>
        ///分页检索标签
        /// </summary>
        ///<param name="query">查询条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public PagingDataSet<T> GetTags(TagQuery query, int pageIndex, int pageSize)
        {
            return tagRepository.GetTags(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 根据标签Id列表组装标签实体
        /// </summary>
        /// <param name="tagIds">标签Id集合</param>
        /// <returns></returns>
        public IEnumerable<T> GetTags(IEnumerable<long> tagIds)
        {
            return tagRepository.PopulateEntitiesByEntityIds(tagIds);
        }

        /// <summary>
        /// 根据Id集合获取标签
        /// </summary>
        ///<param name="tagInOwnerIds">前N条数据</param>
        /// <returns>{Key:标签实体,Value:标签级别}</returns>
        public Dictionary<TagInOwner, int> GetTagsByTagInOwnerIds(IEnumerable<long> tagInOwnerIds)
        {
            if (tagInOwnerIds == null)
                return null;
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append(EntityData.ForType(typeof(TagInOwner)).RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", tagInOwnerIds.Count()));
            cacheKey.AppendFormat("TagCloud-TenantTypeId:{0}", tenantTypeId);

            Dictionary<TagInOwner, int> tagCloud = cacheService.Get<Dictionary<TagInOwner, int>>(cacheKey.ToString());

            if (tagCloud == null)
            {
                tagCloud = new Dictionary<TagInOwner, int>();

                IEnumerable<TagInOwner> tags = GetTagInOwners(tagInOwnerIds);

                int i = 0;
                float x = 0;
                foreach (TagInOwner tag in tags)
                {
                    x = (float)i / tags.Count();

                    for (int j = 0; j < 9; j++)
                    {
                        if (x >= siteTagLevelPartitions[j] && x < siteTagLevelPartitions[j + 1])
                        {
                            tagCloud[tag] = 9 - j;
                            break;
                        }
                    }

                    if (x >= siteTagLevelPartitions[9])
                        tagCloud[tag] = 0;

                    i++;
                }

                tagCloud = tagCloud.OrderBy(n => n.Key.TagName).ToDictionary(k => k.Key, v => v.Value);
                cacheService.Add(cacheKey.ToString(), tagCloud, CachingExpirationType.ObjectCollection);
            }
            return tagCloud;
        }


        /// <summary>
        /// 根据标签Id列表组装标签实体
        /// </summary>
        /// <param name="tagIds">标签Id集合</param>
        /// <returns></returns>
        public IEnumerable<TagInOwner> GetTagInOwners(IEnumerable<long> tagInOwnerIds)
        {
            return tagInOwnerReposiory.PopulateEntitiesByEntityIds(tagInOwnerIds);
        }

        /// <summary>
        ///分页检索标签
        /// </summary>
        ///<param name="groupId">标签分组Id</param>
        ///<param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public PagingDataSet<T> GetTagsOfGroup(long groupId, string tenantTypeId, int pageIndex)
        {
            return tagRepository.GetTagsOfGroup(groupId, tenantTypeId, pageIndex);
        }

        #endregion Tags

        #region TagInOnwer

        /// <summary>
        /// 添加标签与拥有者关联
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns>返回主键</returns>
        public long AddTagInOwner(string tagName, string tenantTypeId, long ownerId)
        {
            string name = string.Concat(tagName.Split(URLSpecialCharacters)).Replace("[_]", " ");
            TagInOwner tagInOwner = new TagInOwner();

            tagInOwner.TagName = name;
            tagInOwner.OwnerId = ownerId;
            tagInOwner.TenantTypeId = tenantTypeId;

            //创建数据前，触发相关事件
            EventBus<TagInOwner>.Instance().OnBefore(tagInOwner, new CommonEventArgs(EventOperationType.Instance().Create()));
            long id = tagInOwnerReposiory.AddTagInOwner(tagInOwner);
            EventBus<TagInOwner>.Instance().OnAfter(tagInOwner, new CommonEventArgs(EventOperationType.Instance().Create()));

            return id;
        }

        /// <summary>
        /// 删除拥有者标签
        /// </summary>
        /// <param name="tagInOwnerId">标签与拥有者关系Id</param>
        public void DeleteOwnerTag(long tagInOwnerId)
        {
            TagInOwner tagInOwner = tagInOwnerReposiory.Get(tagInOwnerId);
            if (tagInOwner != null)
            {
                EventBus<TagInOwner>.Instance().OnBefore(tagInOwner, new CommonEventArgs(EventOperationType.Instance().Delete()));
                tagInOwnerReposiory.Delete(tagInOwner);
                EventBus<TagInOwner>.Instance().OnAfter(tagInOwner, new CommonEventArgs(EventOperationType.Instance().Delete()));
            }
        }

        /// <summary>
        /// 清除拥有者的所有标签
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        public void ClearTagsFromOwner(long ownerId)
        {
            tagInOwnerReposiory.ClearTagsFromOwner(ownerId, tenantTypeId);
        }

        /// <summary>
        /// 获取拥有者的标签列表
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        public IEnumerable<TagInOwner> GetOwnerTags(long ownerId)
        {
            return tagInOwnerReposiory.GetTagInOwners(ownerId, tenantTypeId);
        }

        /// <summary>
        /// 分页获取tn_TagsInOwners表的数据(用于建索引)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagingDataSet<TagInOwner> GetOwnerTags(int pageIndex, int pageSize)
        {
            return tagInOwnerReposiory.GetTagInOwners(pageIndex, pageSize);
        }

        /// <summary>
        /// 获取拥有者的前N个标签
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="keyword">标签关键字</param>
        /// <param name="topNumber">前N个标签</param>
        public IEnumerable<TagInOwner> GetTopOwnerTags(long ownerId, string keyword, int topNumber)
        {
            return tagInOwnerReposiory.GetTopTagInOwners(ownerId, tenantTypeId, keyword, topNumber);
        }

        /// <summary>
        /// 获取拥有者的前N个标签（用户构件标签云）
        /// </summary>
        /// <param name="ownerId">标签拥有者Id</param>
        /// <param name="topNumber">获取标签数</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public Dictionary<TagInOwner, int> GetTopOwnerTags(long ownerId, int topNumber, string tenantTypeId = "")
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append(EntityData.ForType(typeof(TagInOwner)).RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
            cacheKey.AppendFormat("TagInOwnerCloud-TenantTypeId:{0}", tenantTypeId);

            Dictionary<TagInOwner, int> tagInOwnerCloud = cacheService.Get<Dictionary<TagInOwner, int>>(cacheKey.ToString());

            if (tagInOwnerCloud == null)
            {
                tagInOwnerCloud = new Dictionary<TagInOwner, int>();
                List<TagInOwner> tagInOwners = tagInOwnerReposiory.GetTopTagInOwners(ownerId, tenantTypeId, string.Empty, topNumber).ToList();

                float x = 0;
                foreach (TagInOwner tagInOwner in tagInOwners)
                {
                    x = (float)1 / (tagInOwner.ItemCount == 0 ? 1 : tagInOwner.ItemCount);

                    for (int j = 0; j < 9; j++)
                    {
                        if (x >= siteTagLevelPartitions[j] && x < siteTagLevelPartitions[j + 1])
                        {
                            tagInOwnerCloud[tagInOwner] = 9 - j;
                            break;
                        }
                    }

                    if (x >= siteTagLevelPartitions[9])
                        tagInOwnerCloud[tagInOwner] = 0;

                }

                cacheService.Add(cacheKey.ToString(), tagInOwnerCloud, CachingExpirationType.ObjectCollection);
            }

            return tagInOwnerCloud;
        }

        #endregion TagInOnwer

        #region ItemInTag

        /// <summary>
        /// 为多个内容项添加相同标签
        /// </summary>
        /// <param name="itemIds">内容项Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tagName">标签名</param>
        public void AddItemsToTag(IEnumerable<long> itemIds, long ownerId, string tagName)
        {
            string name = string.Concat(tagName.Split(URLSpecialCharacters)).Replace("[_]", " ");
            EventBus<long, TagEventArgs>.Instance().OnBatchBefore(itemIds, new TagEventArgs(EventOperationType.Instance().Create(), tenantTypeId, name));
            itemInTagRepository.AddItemsToTag(itemIds, tenantTypeId, ownerId, name);
            EventBus<long, TagEventArgs>.Instance().OnBatchAfter(itemIds, new TagEventArgs(EventOperationType.Instance().Create(), tenantTypeId, name));
        }

        /// <summary>
        /// 为内容项批量设置标签
        /// </summary>
        /// <param name="tagName">标签名称</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="itemId">内容项Id</param>
        public void AddTagToItem(string tagName, long ownerId, long itemId)
        {
            string name = string.Concat(tagName.Split(URLSpecialCharacters)).Replace("[_]", " ");
            EventBus<string, TagEventArgs>.Instance().OnBefore(name, new TagEventArgs(EventOperationType.Instance().Create(), tenantTypeId, itemId));
            itemInTagRepository.AddTagsToItem(new string[] { name }, tenantTypeId, ownerId, itemId);
            EventBus<string, TagEventArgs>.Instance().OnAfter(name, new TagEventArgs(EventOperationType.Instance().Create(), tenantTypeId, itemId));
        }

        /// <summary>
        /// 为内容项批量设置标签
        /// </summary>
        /// <remarks>标签中如果要包含空格需要用""引起来</remarks>
        /// <param name="tagString">待处理的标签字符串</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="itemId">内容项Id</param>
        public void AddTagsToItem(string tagString, long ownerId, long itemId)
        {
            //拆分标签数组
            string[] tagNames = SplitTagString(tagString);
            for (int i = 0; i < tagNames.Length; i++)
            {
                //把空格的占位符替换回来
                tagNames[i] = string.Concat(tagNames[i].Split(URLSpecialCharacters)).Replace("[_]", " ");
            }

            EventBus<string, TagEventArgs>.Instance().OnBatchBefore(tagNames, new TagEventArgs(EventOperationType.Instance().Create(), tenantTypeId, itemId));
            //添加标签关联记录
            itemInTagRepository.AddTagsToItem(tagNames, tenantTypeId, ownerId, itemId);
            EventBus<string, TagEventArgs>.Instance().OnBatchAfter(tagNames, new TagEventArgs(EventOperationType.Instance().Create(), tenantTypeId, itemId));
        }

        /// <summary>
        /// 为内容项批量设置标签
        /// </summary>
        /// <remarks>标签中如果要包含空格需要用""引起来</remarks>
        /// <param name="tagNames">待添加的标签集合</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="itemId">内容项Id</param>
        public void AddTagsToItem(string[] tagNames, long ownerId, long itemId)
        {
            for (int i = 0; i < tagNames.Length; i++)
            {
                //把空格的占位符替换回来
                tagNames[i] = string.Concat(tagNames[i].Split(URLSpecialCharacters)).Replace("[_]", " ");
            }
            EventBus<string, TagEventArgs>.Instance().OnBatchBefore(tagNames, new TagEventArgs(EventOperationType.Instance().Create(), tenantTypeId, itemId));
            itemInTagRepository.AddTagsToItem(tagNames, tenantTypeId, ownerId, itemId);
            EventBus<string, TagEventArgs>.Instance().OnBatchAfter(tagNames, new TagEventArgs(EventOperationType.Instance().Create(), tenantTypeId, itemId));
        }

        /// <summary>
        /// 删除标签与内容项的关联
        /// </summary>
        /// <param name="itemInTagId">内容项与标签关联Id</param>
        public void DeleteTagFromItem(long itemInTagId)
        {
            ItemInTag itemInTag = itemInTagRepository.Get(itemInTagId);
            EventBus<ItemInTag>.Instance().OnBefore(itemInTag, new CommonEventArgs(EventOperationType.Instance().Delete()));
            itemInTagRepository.Delete(itemInTag);
            EventBus<ItemInTag>.Instance().OnAfter(itemInTag, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// 清除内容项的所有标签
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">拥有者Id</param>
        public void ClearTagsFromItem(long itemId, long ownerId)
        {
            EventBus<long, TagEventArgs>.Instance().OnBefore(itemId, new TagEventArgs(EventOperationType.Instance().Delete(), tenantTypeId));
            itemInTagRepository.ClearTagsFromItem(itemId, tenantTypeId, ownerId);
            EventBus<long, TagEventArgs>.Instance().OnAfter(itemId, new TagEventArgs(EventOperationType.Instance().Delete(), tenantTypeId));
        }

        /// <summary>
        /// 获取标签的所有内容项集合
        /// </summary>
        /// <param name="tagName">标签名称</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns>返回指定的内容项Id集合</returns>
        public IEnumerable<long> GetItemIds(string tagName, long? ownerId)
        {
            return itemInTagRepository.GetItemIds(tagName, tenantTypeId, ownerId);
        }

        /// <summary>
        /// 获取标签的内容项集合
        /// </summary>
        /// <param name="tagName">标签名称</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页码(从1开始)</param>
        /// <returns>返回指定页码的内容项Id集合</returns>
        public PagingEntityIdCollection GetItemIds(string tagName, long? ownerId, int pageSize, int pageIndex)
        {
            return itemInTagRepository.GetItemIds(tagName, tenantTypeId, ownerId, pageSize, pageIndex);
        }

        /// <summary>
        /// 获取多个标签的内容项集合
        /// </summary>
        /// <param name="tagNames">标签名称列表</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页码(从1开始)</param>
        /// <returns>返回指定页码的内容项Id集合</returns>
        public PagingEntityIdCollection GetItemIds(IEnumerable<string> tagNames, long? ownerId, int pageSize, int pageIndex)
        {
            return itemInTagRepository.GetItemIds(tagNames, tenantTypeId, ownerId, pageSize, pageIndex);
        }

        /// <summary>
        /// 获取内容项的所有标签
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <returns>返回内容项的标签集合</returns>
        public IEnumerable<ItemInTag> GetItemInTagsOfItem(long itemId)
        {
            IEnumerable<long> tagIds = itemInTagRepository.GetItemInTagIdsOfItem(itemId, tenantTypeId);
            return itemInTagRepository.PopulateEntitiesByEntityIds(tagIds);
        }

        /// <summary>
        /// 获取内容项的前N个标签标签
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="topNumber">前N条记录</param>
        /// <returns>返回内容项的标签集合</returns>
        public IEnumerable<T> GetTopTagsOfItem(long itemId, int topNumber)
        {
            IEnumerable<long> ids = itemInTagRepository.GetTagIdsOfItem(itemId, tenantTypeId);
            if (ids != null && ids.Count() > topNumber)
                ids = ids.Take(topNumber);

            return tagRepository.PopulateEntitiesByEntityIds(ids);
        }

        /// <summary>
        /// 根据内容项获取拥有者设置的标签
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">拥有者Id</param>
        public IEnumerable<TagInOwner> GetOwnerTagsOfItem(long itemId, long ownerId)
        {
            IEnumerable<long> ids = itemInTagRepository.GetTagInOwnerIdsOfItem(itemId, ownerId, tenantTypeId);
            return tagInOwnerReposiory.PopulateEntitiesByEntityIds(ids);
        }

        /// <summary>
        /// 根据用户ID列表获取ItemInTag的ID列表，本方法现用于用户搜索功能的索引生成
        /// </summary>
        /// <param name="userIds">用户ID列表</param>
        /// <returns>ItemInTag的ID列表</returns>
        public IEnumerable<long> GetItemInTagIdsByItemIds(IEnumerable<long> userIds)
        {
            return itemInTagRepository.GetEntityIdsByUserIds(userIds);
        }

        /// <summary>
        /// 根据Id列表获取ItemInTag的实体列表
        /// </summary>
        /// <param name="entityIds">ItemInTag的Id列表</param>
        /// <returns>ItemInTag的实体列表</returns>
        public IEnumerable<ItemInTag> GetItemInTags(IEnumerable<long> entityIds)
        {
            return itemInTagRepository.PopulateEntitiesByEntityIds<long>(entityIds);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="itemId">成员Id</param>
        /// <param name="tagInOwnerId">标签与拥有者关联Id</param>
        /// <returns></returns>
        public Dictionary<string, long> GetTagNamesWithIdsOfItem(long itemId, long? tagInOwnerId = null)
        {
            return itemInTagRepository.GetTagNamesWithIdsOfItem(itemId, tenantTypeId, tagInOwnerId);
        }

        #endregion ItemInTag

        #region RelatedTag

        /// <summary>
        /// 添加相关标签
        /// </summary>
        /// <param name="tagString">待处理的标签字符串</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tagId">标签Id</param>
        public bool AddRelatedTagsToTag(string tagString, int ownerId, long tagId)
        {
            //拆分标签数组
            string[] tagNames = SplitTagString(tagString);
            for (int i = 0; i < tagNames.Length; i++)
            {
                //把空格的占位符替换回来
                tagNames[i] = string.Concat(tagNames[i].Split(URLSpecialCharacters)).Replace("[_]", " ");
            }
            return relatedTagRepository.AddRelatedTagsToTag(tagNames, tenantTypeId, ownerId, tagId) > 0;
        }

        /// <summary>
        /// 清除拥有者的所有标签
        /// </summary>
        /// <param name="tagId">被关联的标签Id</param>
        public void ClearRelatedTagsFromTag(long tagId)
        {
            relatedTagRepository.ClearRelatedTagsFromTag(tagId);
        }

        /// <summary>
        /// 清除关联的标签
        /// </summary>
        /// <remarks>会删除双向的关联关系</remarks>
        /// <param name="relatedTagId">关联的标签Id</param>
        /// <param name="tagId">被关联的标签Id</param>
        public void DeleteRelatedTagFromTag(long relatedTagId, long tagId)
        {
            relatedTagRepository.DeleteRelatedTagFromTag(relatedTagId, tagId);
        }

        /// <summary>
        /// 获取相关标签
        /// </summary>
        /// <param name="tagId">被关联的标签Id</param>
        public IEnumerable<T> GetRelatedTags(long tagId)
        {
            IEnumerable<long> relatedTagIds = relatedTagRepository.GetRelatedTagIds(tagId);
            IEnumerable<T> relatedTags = tagRepository.PopulateEntitiesByEntityIds(relatedTagIds);

            return relatedTags;
        }

        #endregion RelatedTag

        #region TagGroup

        /// <summary>
        /// 创建标签分组
        /// </summary>
        /// <param name="group">待创建分组实体</param>
        public bool CreateGroup(TagGroup group)
        {
            long objectId = Convert.ToInt64(tagGroupRepository.Insert(group));
            return objectId > 0;
        }

        /// <summary>
        /// 更新标签分组
        /// </summary>
        /// <param name="group">待更新分组实体</param>
        public void UpdateGroup(TagGroup group)
        {
            tagGroupRepository.Update(group);
        }

        /// <summary>
        /// 更新标签分组
        /// </summary>
        /// <param name="group">待更新分组实体</param>
        public void DeleteGroup(TagGroup group)
        {
            tagGroupRepository.Delete(group);
        }

        /// <summary>
        /// 获取所有标签分组管理员后台用
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<TagGroup> GetGroups(string tenantTypeId = null)
        {
            return tagGroupRepository.GetGroups(tenantTypeId);
        }

        /// <summary>
        /// 获取标签分组实体
        /// </summary>
        /// <param name="groupId">分组Id</param>
        /// <returns></returns>
        public TagGroup GetGroup(long groupId)
        {
            return tagGroupRepository.Get(groupId);
        }

        /// <summary>
        /// 添加标签与分组关联
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="groupId">拥有者Id</param>
        public int AddTagInGroup(string tagName, long groupId)
        {
            return tagInGroupRepository.AddTagInGroup(tagName, groupId, tenantTypeId);
        }

        /// <summary>
        /// 批量添加标签与分组关联
        /// </summary>
        /// <param name="tagNames">标签名</param>
        /// <param name="groupId">拥有者Id</param>
        public int BatchAddTagsInGroup(IEnumerable<string> tagNames, long groupId)
        {
            return tagInGroupRepository.BatchAddTagsInGroup(tagNames, groupId, tenantTypeId);
        }

        /// <summary>
        /// 批量添加分组给标签
        /// </summary>
        /// <param name="groupIds">分组Id集合</param>
        /// <param name="tagName">标签名</param>
        public int BatchAddGroupsToTag(IEnumerable<long> groupIds, string tagName)
        {
            return tagInGroupRepository.BatchAddGroupsToTag(groupIds, tagName, tenantTypeId);
        }

        /// <summary>
        /// 清除分组的所有标签
        /// </summary>
        /// <param name="groupId">分组Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public int ClearTagsFromGroup(long groupId)
        {
            return tagInGroupRepository.ClearTagsFromGroup(groupId);
        }

        /// <summary>
        /// 获取分组下的标签
        /// </summary>
        /// <param name="groupId">分组Id</param>
        public IEnumerable<string> GetTagsOfGroup(long groupId)
        {
            return tagInGroupRepository.GetTagsOfGroup(groupId);
        }

        /// <summary>
        /// 获取分组下的前N个标签
        /// </summary>
        /// <param name="groupId">分组Id</param>
        /// <param name="topNumber">获取记录数</param>
        public IEnumerable<string> GetTopTagsOfGroup(long groupId, int topNumber)
        {
            IEnumerable<string> tags = tagInGroupRepository.GetTagsOfGroup(groupId);

            if (tags == null)
                return new List<string>();

            return topNumber < tags.Count() ? tags.Take(topNumber) : tags;
        }

        /// <summary>
        /// 根据标签获取标签分组
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public IEnumerable<TagGroup> GetGroupsOfTag(string tagName, string tenantTypeId)
        {
            return tagGroupRepository.GetGroupsOfTag(tagName, tenantTypeId);
        }

        #endregion TagGroup

        #region 标签解析

        /// <summary>
        /// 解析内容用于创建话题
        /// </summary>
        /// <param name="body">待解析的内容</param>
        /// <param name="ownerId">标签拥有者Id</param>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public void ResolveBodyForEdit(string body, long ownerId, long associateId, string tenantTypeId)
        {
            if (!body.Contains("#") || string.IsNullOrEmpty(body))
                return;

            Regex rg = new Regex(@"(?<=(?<!\&)(\#)(?!\d\;))[^\#@]*(?=(?<!\&)(\#)(?![0-9]+\;))", RegexOptions.Multiline | RegexOptions.Singleline);
            Match m = rg.Match(body);

            if (!m.Success)
                return;

            IList<string> tagNames = new List<string>();
            int i = 0, index = -1;

            while (m != null)
            {
                if (i % 2 == 1)
                {
                    m = m.NextMatch();
                    i++;
                    continue;
                }

                if (index == m.Index)
                    break;

                index = m.Index;

                if (!string.IsNullOrEmpty(m.Value) && !tagNames.Contains(m.Value))
                    tagNames.Add(m.Value);
                else
                    continue;

                m = m.NextMatch();
                i++;
            }

            if (tagNames.Count > 0)
            {
                CountService countService = new CountService(TenantTypeIds.Instance().Tag());
                AddTagsToItem(tagNames.ToArray(), ownerId, associateId);

                Dictionary<string, long> tagNamesWithIds = GetTagNamesWithIdsOfItem(associateId);
                if (tagNamesWithIds != null)
                {
                    foreach (KeyValuePair<string, long> pair in tagNamesWithIds)
                    {
                        countService.ChangeCount(CountTypes.Instance().ItemCounts(), pair.Value, ownerId, 1);
                    }
                }
            }
        }

        /// <summary>
        /// 解析内容中的AtUser用户展示展示
        /// </summary>
        /// <param name="body">待解析的内容</param>
        /// <param name="associateId">关联项Id</param>
        /// <param name="ownerId">标签拥有者Id</param>
        /// <param name="TagGenerate">用户生成对应标签的方法</param>
        public string ResolveBodyForDetail(string body, long associateId, long ownerId, Func<KeyValuePair<string, long>, long, string> TagGenerate)
        {
            if (string.IsNullOrEmpty(body) || !body.Contains("#") || ownerId <= 0)
                return body;

            Dictionary<string, long> tagNamesWithIds = itemInTagRepository.GetTagNamesWithIdsOfItem(associateId, tenantTypeId);

            if (tagNamesWithIds != null)
            {
                foreach (var item in tagNamesWithIds)
                {
                    body = body.Replace("#" + item.Key + "#", TagGenerate(item, ownerId));
                }
            }
            return body;
        }

        #endregion

        #region helper method

        /// <summary>
        /// 分割tagString的到标签名集合
        /// </summary>
        /// <remarks>保留引号中标签名的空格</remarks>
        /// <param name="tagString">标签名字符串</param>
        /// <returns></returns>
        private string[] SplitTagString(string tagString)
        {
            if (string.IsNullOrEmpty(tagString))
                return null;

            int count = tagString.Count(s => s == '\"');

            if (count > 1)
            {
                string[] tagArray = tagString.Split('\"');

                for (int i = (tagString.StartsWith("\"") ? 0 : 1); i < (tagString.EndsWith("\"") ? tagArray.Length : tagArray.Length - 1); i++)
                {
                    if (tagArray.Length < i + 1)
                        break;

                    if (i % 2 == 0)
                        continue;

                    tagArray[i] = tagArray[i].Replace(" ", "[_]");
                }

                tagString = String.Concat(tagArray);
            }

            return tagString.Split(SplitCharacters);
        }


        #endregion helper method
    }
}
