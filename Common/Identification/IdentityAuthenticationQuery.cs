using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 身份认证查询条件
    /// </summary>
    public class IdentityAuthenticationQuery
    {
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 身份认证类型ID
        /// </summary>
        public long IdentificationTypeId { get; set; }

        /// <summary>
        /// 开始时间(从)
        /// </summary>
        public DateTime? startTime { get; set; }

        /// <summary>
        /// 结束时间(到)
        /// </summary>
        public DateTime? endTime { get; set; }

        /// <summary>
        /// 身份认证状态
        /// </summary>
        public IdentificationStatus? identificationStatus { get; set; }
    }

    /// <summary>
    /// 身份认证状态
    /// </summary>
    public enum IdentificationStatus
    {
        /// <summary>
        /// 失败
        /// </summary>
       [Display(Name = "未通过")]
        fail,
        /// <summary>
        /// 成功
        /// </summary>
        [Display(Name = "已通过")]
        success,
        /// <summary>
        /// 认证中
        /// </summary>
        [Display(Name = "待处理")]
        pending
    }
}
