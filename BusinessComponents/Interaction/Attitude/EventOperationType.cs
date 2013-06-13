using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;

namespace Tunynet.Common
{
    public static class EventOperationTypeExtension
    {
        /// <summary>
        /// 顶
        /// </summary>
        public static string Support(this EventOperationType eventOperationType)
        {
            return "Support";
        }

        /// <summary>
        /// 踩
        /// </summary>
        public static string Oppose(this EventOperationType eventOperationType)
        {
            return "Oppose";
        }
    }
}
