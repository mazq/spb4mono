using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 通知类型设置实体类
    /// </summary>
    public class NoticeTypeSettings
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 是否允许
        /// </summary>
        public bool IsAllow { get; set; }
    }
}
