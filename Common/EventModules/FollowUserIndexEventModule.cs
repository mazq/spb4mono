//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Spacebuilder.Search;
using Spacebuilder.Search.Tasks;
using Tunynet.Common;
using Tunynet.Events;

namespace Spacebuilder.Common.EventModules
{
    /// <summary>
    /// 处理用户索引的EventMoudle
    /// </summary>
    public class FollowUserIndexEventModule : IEventMoudle
    {
        //因为EventModule.RegisterEventHandler()在web启动时初始化，而FollowUserSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此UserSearcher不能在此处构建，只能再下面的方法中构建
        private FollowUserSearcher searcher = null;

        public void RegisterEventHandler()
        {
            EventBus<FollowEntity>.Instance().After += new CommonEventHandler<FollowEntity, CommonEventArgs>(FollowUser_After);
        }

        private void FollowUser_After(FollowEntity followEntity,CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update() || eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                if (searcher == null)
                {
                    searcher = (FollowUserSearcher)SearcherFactory.GetSearcher(FollowUserSearcher.CODE);
                }

                searcher.Update(followEntity.UserId);
            }
        }


    }
}
