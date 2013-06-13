//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common;
using Spacebuilder.Search;

namespace Spacebuilder.Common.EventModules
{
    /// <summary>
    /// 标签增量索引事件类
    /// </summary>
    public class TagIndexEventModule : IEventMoudle
    {
        private TagSearcher tagSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<Tag>.Instance().After += new CommonEventHandler<Tag, CommonEventArgs>(Tag_After);
            EventBus<TagInOwner>.Instance().After += new CommonEventHandler<TagInOwner, CommonEventArgs>(TagInOwner_After);
        }

        private void TagInOwner_After(TagInOwner sender, CommonEventArgs eventArgs)
        {
            if (sender == null)
            {
                return;
            }
            if (tagSearcher == null)
            {
                tagSearcher = (TagSearcher)SearcherFactory.GetSearcher(TagSearcher.CODE);
            }
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                tagSearcher.InsertTagInOwner(sender);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                tagSearcher.DeleteTagInOwner(sender.Id);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                tagSearcher.UpdateTagInOwner(sender);
            }
        }

        private void Tag_After(Tag sender, CommonEventArgs eventArgs)
        {
            if (sender==null)
	        {
		      return;
	        }
            if (tagSearcher == null)
            {
                tagSearcher = (TagSearcher)SearcherFactory.GetSearcher(TagSearcher.CODE);
            }
            if (eventArgs.EventOperationType==EventOperationType.Instance().Create())
            {
                tagSearcher.InsertTag(sender);
            }
            else if(eventArgs.EventOperationType==EventOperationType.Instance().Delete())
            {
                tagSearcher.DeleteTag(sender.TagId);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                tagSearcher.UpdateTag(sender);
            }
        }


    }
}
