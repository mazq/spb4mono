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

namespace Tunynet.Common
{
    /// <summary>
    /// 用户更改积分的参数
    /// </summary>
    public class ChangePointsEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 更新积分
        /// </summary>
        /// <param name="experiencePoints">经验积分</param>
        /// <param name="reputationPoints">威望积分</param>
        /// <param name="tradePoints">交易积分</param>
        /// <param name="tradePoints2">交易积分2</param>
        /// <param name="tradePoints3">交易积分3</param>
        /// <param name="tradePoints4">交易积分4</param>
        public ChangePointsEventArgs(int experiencePoints, int reputationPoints, int tradePoints, int tradePoints2, int tradePoints3, int tradePoints4)
            : base(Tunynet.Events.EventOperationType.Instance().Update())
        {
            this.experiencePoints = experiencePoints;
            this.reputationPoints = reputationPoints;
            this.tradePoints = tradePoints;
            this.tradePoints2 = tradePoints2;
            this.tradePoints3 = tradePoints3;
            this.tradePoints4 = tradePoints4;
        }

        /// <summary>
        /// 更新积分
        /// </summary>
        /// <param name="experiencePoints">经验积分</param>
        /// <param name="reputationPoints">威望积分</param>
        /// <param name="tradePoints">交易积分</param>
        /// <param name="tradePoints2">交易积分2</param>
        /// <param name="tradePoints3">交易积分3</param>
        /// <param name="tradePoints4">交易积分4</param>
        /// <param name="eventOperationType">事件类型</param>
        public ChangePointsEventArgs(int experiencePoints, int reputationPoints, int tradePoints, int tradePoints2, int tradePoints3, int tradePoints4, string eventOperationType)
            : base(eventOperationType)
        {
            this.experiencePoints = experiencePoints;
            this.reputationPoints = reputationPoints;
            this.tradePoints = tradePoints;
            this.tradePoints2 = tradePoints2;
            this.tradePoints3 = tradePoints3;
            this.tradePoints4 = tradePoints4;
        }

        /// <summary>
        /// 经验值
        /// </summary>
        private int experiencePoints;

        /// <summary>
        /// 经验值
        /// </summary>
        public int ExperiencePoints
        {
            get { return experiencePoints; }
            set { experiencePoints = value; }
        }

        /// <summary>
        /// 威望积分
        /// </summary>
        public int reputationPoints;

        /// <summary>
        /// 威望积分
        /// </summary>
        public int ReputationPoints
        {
            get { return reputationPoints; }
            set { reputationPoints = value; }
        }

        /// <summary>
        /// 交易积分
        /// </summary>
        public int tradePoints;

        /// <summary>
        /// 交易积分
        /// </summary>
        public int TradePoints
        {
            get { return tradePoints; }
            set { tradePoints = value; }
        }

        /// <summary>
        /// 交易积分2
        /// </summary>
        public int tradePoints2;

        /// <summary>
        /// 交易积分2
        /// </summary>
        public int TradePoints2
        {
            get { return tradePoints2; }
            set { tradePoints2 = value; }
        }

        /// <summary>
        /// 交易积分3
        /// </summary>
        public int tradePoints3;

        /// <summary>
        /// 交易积分3
        /// </summary>
        public int TradePoints3
        {
            get { return tradePoints3; }
            set { tradePoints3 = value; }
        }

        /// <summary>
        /// 交易积分4
        /// </summary>
        public int tradePoints4;

        /// <summary>
        /// 交易积分4
        /// </summary>
        public int TradePoints4
        {
            get { return tradePoints4; }
            set { tradePoints4 = value; }
        }
    }
}
