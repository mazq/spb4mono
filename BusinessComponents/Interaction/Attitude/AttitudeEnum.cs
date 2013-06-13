using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 顶踩排序字段
    /// </summary>
    public enum SortBy_Attitude
    {
        /// <summary>
        /// 根据综合评价
        /// </summary>
        Comprehensive_Desc,

        /// <summary>
        /// 根据顶的统计数
        /// </summary>
        SupportCount_Desc
    }

    /// <summary>
    /// 顶踩的模式
    /// </summary>
    public enum AttitudeMode
    {
        /// <summary>
        /// 单向操作（用于仅存在顶操作）
        /// </summary>
        Unidirection,

        /// <summary>
        /// 双向操作（用于顶踩操作都存在）
        /// </summary>
        Bidirection
    }

    /// <summary>
    /// 顶踩样式
    /// </summary>
    public enum AttitudeStyle
    {

        /// <summary>
        /// 喜欢（心的形状）
        /// </summary>
        Like,

        /// <summary>
        /// 单向顶（向上的手的形状）
        /// </summary>
        Support,

        /// <summary>
        /// 双向顶和踩（向上向下的手的形状）
        /// </summary>
        SupportOppose,

        /// <summary>
        /// 双向顶和踩（向上向下箭头的形状）
        /// </summary>
        UpDown
    }
}