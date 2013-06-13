//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Events;
using Tunynet.Common;
using Spacebuilder.Bar.Search;
using Spacebuilder.Search;
using Tunynet;

namespace Spacebuilder.Bar.EventModules
{
    /// <summary>
    /// 处理帖吧索引的EventMoudle
    /// </summary>
    public class BarIndexEventModule: IEventMoudle
    {
        private BarThreadService barThreadService = new BarThreadService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().BarThread());

        //因为EventModule.RegisterEventHandler()在web启动时初始化，而UserSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此BarSearcher不能在此处构建，只能再下面的方法中构建
        private BarSearcher barSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<BarThread>.Instance().After += new CommonEventHandler<BarThread, CommonEventArgs>(BarThread_After);
            EventBus<BarPost>.Instance().After += new CommonEventHandler<BarPost, CommonEventArgs>(BarPost_After);
            //EventBus<string, TagEventArgs>.Instance().After += new CommonEventHandler<string, TagEventArgs>(AddTag);
            EventBus<ItemInTag>.Instance().After += new CommonEventHandler<ItemInTag, CommonEventArgs>(DeleteItemInTags);
            EventBus<Tag>.Instance().Before += new CommonEventHandler<Tag, CommonEventArgs>(DeleteUpdateTags_Before);
            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddTagsToBar_BatchAfter);
        }

        #region 标签增量索引
        /// <summary>
        /// 为日志添加标签时触发
        /// </summary>
        private void AddTagsToBar_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().BarThread())
            {
                long barThreadId = eventArgs.ItemId;
                if (barSearcher == null)
                {
                    barSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
                }
                barSearcher.UpdateBarThread(barThreadService.Get(barThreadId));
            }
        }
        /// <summary>
        /// 删除和更新标签时触发
        /// </summary>
        private void DeleteUpdateTags_Before(Tag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().BarThread())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    //根据标签获取所有使用该标签的(内容项)用户
                    IEnumerable<long> barThreadIds = tagService.GetItemIds(sender.TagName, null);
                    if (barSearcher == null)
                    {
                        barSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
                    }
                    barSearcher.UpdateBarThread(barThreadService.GetBarThreads(barThreadIds));
                }
            }
        }
        private void DeleteItemInTags(ItemInTag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().BarThread())
            {
                long barThreadId = sender.ItemId;
                if (barSearcher == null)
                {
                    barSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
                }
                barSearcher.UpdateBarThread(barThreadService.Get(barThreadId));
            }
        }
        #endregion    

        private void BarThread_After(BarThread barThread, CommonEventArgs commonEventArgs)
        {
            if (barThread == null)
            {
                return;
            }

            if (barSearcher == null)
            {
                barSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
            }

            //添加索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                barSearcher.InsertBarThread(barThread);
            }

            //删除索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                barSearcher.DeleteBarThread(barThread.ThreadId);
            }

            //更新索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                barSearcher.UpdateBarThread(barThread);
            }

        }

        private void BarPost_After(BarPost barPost, CommonEventArgs commonEventArgs)
        {
            if (barPost == null)
            {
                return;
            }

            if (barSearcher == null)
            {
                barSearcher = (BarSearcher)SearcherFactory.GetSearcher(BarSearcher.CODE);
            }

            //添加索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                barSearcher.InsertBarPost(barPost);
            }

            //删除索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                barSearcher.DeleteBarPost(barPost.PostId);
            }

            //更新索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                barSearcher.UpdateBarPost(barPost);
            }

        }
    }
}