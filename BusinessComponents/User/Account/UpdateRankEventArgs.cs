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
    /// 更新用户等级
    /// </summary>
    public class UpdateRankEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="rank"></param>
        public UpdateRankEventArgs(int rank)
            : base(Tunynet.Events.EventOperationType.Instance().Update())
        {
            this.rank = rank;
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="rank"></param>
        public UpdateRankEventArgs(int rank, string eventOperationType)
            : base(eventOperationType)
        {
            this.rank = rank;
        }

        private int rank;

        /// <summary>
        /// 用户等级
        /// </summary>
        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }
    }
}
