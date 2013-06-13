//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Spacebuilder.Search;
using Tunynet.Common;
using Tunynet.Events;
using Spacebuilder.Group;

namespace Spacebuilder.Blog.EventModules
{
    /// <summary>
    /// 处理群组索引的EventMoudle
    /// </summary>
    public class GroupIndexEventModule : IEventMoudle
    {
        private GroupService groupService = new GroupService();
        private CategoryService categoryService = new CategoryService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Group());

        //因为EventModule.RegisterEventHandler()在web启动时初始化，而GroupSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此GroupSearcher不能在此处构建，只能再下面的方法中构建
        private GroupSearcher groupSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<GroupEntity>.Instance().After += new CommonEventHandler<GroupEntity, CommonEventArgs>(GroupEntity_After);

            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddTagsToGroup_BatchAfter);
            EventBus<Tag>.Instance().Before += new CommonEventHandler<Tag, CommonEventArgs>(DeleteUpdateTags_Before);
            EventBus<ItemInTag>.Instance().After += new CommonEventHandler<ItemInTag, CommonEventArgs>(DeleteItemInTags);

            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddCategoriesToGroup_BatchAfter);
            EventBus<Category>.Instance().Before += new CommonEventHandler<Category, CommonEventArgs>(DeleteUpdateCategories_Before);
        }

        #region 分类增量索引

        /// <summary>
        /// 为群组添加分类时触发
        /// </summary>
        private void AddCategoriesToGroup_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().Group())
            {
                long groupId = eventArgs.ItemId;
                if (groupSearcher == null)
                {
                    groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
                }
                groupSearcher.Update(groupService.Get(groupId));
            }
        }

        /// <summary>
        /// 删除和更新分类时触发
        /// </summary>
        private void DeleteUpdateCategories_Before(Category sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().Group())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    IEnumerable<long> groupIds = categoryService.GetItemIds(sender.CategoryId, false);
                    if (groupSearcher == null)
                    {
                        groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
                    }
                    groupSearcher.Update(groupService.GetGroupEntitiesByIds(groupIds));
                }
            }
        }
        #endregion

        #region 标签增量索引
        /// <summary>
        /// 为群组添加标签时触发
        /// </summary>
        private void AddTagsToGroup_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().Group())
            {
                long groupId = eventArgs.ItemId;
                if (groupSearcher == null)
                {
                    groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
                }
                groupSearcher.Update(groupService.Get(groupId));
            }
        }
        /// <summary>
        /// 删除和更新标签时触发
        /// </summary>
        private void DeleteUpdateTags_Before(Tag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().Group())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    //根据标签获取所有使用该标签的(内容项)群组
                    IEnumerable<long> groupIds = tagService.GetItemIds(sender.TagName, null);
                    if (groupSearcher == null)
                    {
                        groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
                    }
                    groupSearcher.Update(groupService.GetGroupEntitiesByIds(groupIds));
                }
            }
        }
        private void DeleteItemInTags(ItemInTag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().Group())
            {
                long groupId = sender.ItemId;
                if (groupSearcher == null)
                {
                    groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
                }
                groupSearcher.Update(groupService.Get(groupId));
            }
        }
        #endregion

        #region 群组增量索引
        /// <summary>
        /// 群组增量索引
        /// </summary>
        private void GroupEntity_After(GroupEntity group, CommonEventArgs eventArgs)
        {
            if (group == null)
            {
                return;
            }

            if (groupSearcher == null)
            {
                groupSearcher = (GroupSearcher)SearcherFactory.GetSearcher(GroupSearcher.CODE);
            }

            //添加索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                groupSearcher.Insert(group);
            }

            //删除索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                groupSearcher.Delete(group.GroupId);
            }

            //更新索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                groupSearcher.Update(group);
            }
        }
        #endregion
    }
}