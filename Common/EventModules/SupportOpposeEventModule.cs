using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common;
using Tunynet.Globalization;
using Tunynet;

namespace Spacebuilder.Common.EventModules
{
    public class SupportOpposeEventModule : IEventMoudle
    {

        private PointService pointService = new PointService();
        private ApplicationService applicationService = new ApplicationService();

        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        void IEventMoudle.RegisterEventHandler()
        {
            EventBus<long, SupportOpposeEventArgs>.Instance().After += new CommonEventHandler<long, SupportOpposeEventArgs>(SupportEventModule_After);
        }

        /// <summary>
        /// 顶踩的积分处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SupportEventModule_After(long objectId, SupportOpposeEventArgs eventArgs)
        {
            //如果不是第一次顶踩，则不处理
            if (!eventArgs.FirstTime)
            {
                return;
            }

            //处理积分和威望
            string pointItemKey = PointItemKeys.Instance().CreateEvaluation();

            if (eventArgs.EventOperationType == EventOperationType.Instance().Support())
            {
                //顶时产生积分
                string eventOperationType = EventOperationType.Instance().Support();
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "内容");
                pointService.GenerateByRole(eventArgs.UserId, pointItemKey, description);

            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Oppose())
            {
                //踩时产生积分
                string eventOperationType = EventOperationType.Instance().Oppose();
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "内容");
                pointService.GenerateByRole(eventArgs.UserId, pointItemKey, description);

            }
        }
    }
}
