//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;

namespace Spacebuilder.Microblog.EventModules
{
    /// <summary>
    /// 处理微博索引的EventMoudle
    /// </summary>
    public class MicroblogIndexEventModule : IEventMoudle
    {
        private MicroblogService microBlogService = new MicroblogService();

        //因为EventModule.RegisterEventHandler()在web启动时初始化，而UserSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此MicroblogSearcher不能在此处构建，只能再下面的方法中构建
        private MicroblogSearcher microblogSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<MicroblogEntity>.Instance().After += new CommonEventHandler<MicroblogEntity, CommonEventArgs>(MicroblogEntity_After);
        }

        private void MicroblogEntity_After(MicroblogEntity microblogEntity, CommonEventArgs commonEventArgs)
        {
            if (microblogEntity == null)
            {
                return;
            }

            if (microblogSearcher == null)
            {
                microblogSearcher = (MicroblogSearcher)SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
            }

            //添加索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                microblogSearcher.Insert(microblogEntity);
            }

            //删除索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                microblogSearcher.Delete(microblogEntity.MicroblogId);
            }

            //更新索引
            if (commonEventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                microblogSearcher.Update(microblogEntity);
            }

        }
    }
}