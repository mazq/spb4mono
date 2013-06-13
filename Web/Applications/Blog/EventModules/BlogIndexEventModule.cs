//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Spacebuilder.Search;
using Tunynet.Common;
using Tunynet.Events;

namespace Spacebuilder.Blog.EventModules
{
    /// <summary>
    /// 处理日志索引的EventMoudle
    /// </summary>
    public class BlogIndexEventModule : IEventMoudle
    {
        private BlogService blogService = new BlogService();
        private CategoryService categoryService = new CategoryService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().BlogThread());

        //因为EventModule.RegisterEventHandler()在web启动时初始化，而BlogSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此BlogSearcher不能在此处构建，只能再下面的方法中构建
        private BlogSearcher blogSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<BlogThread>.Instance().After += new CommonEventHandler<BlogThread, CommonEventArgs>(BlogThread_After);

            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddTagsToBlog_BatchAfter);
            EventBus<Tag>.Instance().Before += new CommonEventHandler<Tag, CommonEventArgs>(DeleteUpdateTags_Before);
            EventBus<ItemInTag>.Instance().After += new CommonEventHandler<ItemInTag, CommonEventArgs>(DeleteItemInTags);

            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddCategoriesToBlog_BatchAfter);
            EventBus<Category>.Instance().Before += new CommonEventHandler<Category, CommonEventArgs>(DeleteUpdateCategories_Before);
        }

        
        #region 分类增量索引

        /// <summary>
        /// 为日志添加分类时触发
        /// </summary>
        private void AddCategoriesToBlog_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                long blogThreadId = eventArgs.ItemId;
                if (blogSearcher == null)
                {
                    blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                }
                blogSearcher.Update(blogService.Get(blogThreadId));
            }
        }

        /// <summary>
        /// 删除和更新分类时触发
        /// </summary>
        private void DeleteUpdateCategories_Before(Category sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    IEnumerable<long> blogThreadIds = categoryService.GetItemIds(sender.CategoryId, false);
                    if (blogSearcher == null)
                    {
                        blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                    }
                    blogSearcher.Update(blogService.GetBlogThreads(blogThreadIds));
                }
            }
        }
        #endregion

        #region 标签增量索引

        /// <summary>
        /// 为日志添加标签时触发
        /// </summary>
        private void AddTagsToBlog_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                long blogThreadId = eventArgs.ItemId;
                if (blogSearcher == null)
                {
                    blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                }
                blogSearcher.Update(blogService.Get(blogThreadId));
            }
        }
        /// <summary>
        /// 删除和更新标签时触发
        /// </summary>
        private void DeleteUpdateTags_Before(Tag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId==TenantTypeIds.Instance().BlogThread())
            {
                if (eventArgs.EventOperationType==EventOperationType.Instance().Delete()||eventArgs.EventOperationType==EventOperationType.Instance().Update())
                {
                    //根据标签获取所有使用该标签的(内容项)日志
                    IEnumerable<long> blogThreadIds = tagService.GetItemIds(sender.TagName, null);
                    if (blogSearcher == null)
                    {
                       blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                    }
                    blogSearcher.Update(blogService.GetBlogThreads(blogThreadIds));
                }
            }
        }
        private void DeleteItemInTags(ItemInTag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                long barThreadId = sender.ItemId;
                if (blogSearcher == null)
                {
                    blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                }
                blogSearcher.Update(blogService.Get(barThreadId));
            }
        }
        #endregion    

        #region 日志增量索引
        /// <summary>
        /// 日志增量索引
        /// </summary>
        private void BlogThread_After(BlogThread blog, CommonEventArgs eventArgs)
        {
            if (blog == null)
            {
                return;
            }

            if (blogSearcher == null)
            {
                blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
            }

            //添加索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                if (!blog.IsDraft)
                {
                    blogSearcher.Insert(blog);
                }
            }

            //删除索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                blogSearcher.Delete(blog.ThreadId);
            }

            //更新索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                if (!blog.IsDraft)
                {
                    blogSearcher.Update(blog);
                }
            }
        }
        #endregion
    }
}