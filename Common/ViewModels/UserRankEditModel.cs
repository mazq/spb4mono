//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 创建编辑用户等级
    /// </summary>
    public class UserRankEditModel
    {
        /// <summary>
        /// 用户等级标识
        /// </summary>
        [Range(1, 1000, ErrorMessage = "录入范围是1-1000哦")]
        [Display(Name = "等级")]
        [Required(ErrorMessage = "用户等级为必填项")]
        public int? Rank { get; set; }

        /// <summary>
        /// 用户等级名
        /// </summary>
        [Display(Name = "等级名称")]
        [Required(ErrorMessage = "请输入等级名称")]
        [StringLength(10, ErrorMessage = "最多可以输入10个字")]
        public string RankName { get; set; }

        /// <summary>
        /// 次等级总和积分的下限
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "范围是0-2147483647")]
        [Display(Name = "积分下限")]
        [Required(ErrorMessage = "积分下限为必填项")]
        public int? PointLower { get; set; }

        /// <summary>
        /// 是否是编辑
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        /// 将用户等级转换成UserRank
        /// </summary>
        /// <returns>用户等级</returns>
        public UserRank AsUserRank()
        {
            return new UserRank
            {
                RankName = this.RankName,
                Rank = this.Rank ?? 0,
                PointLower = this.PointLower ?? 0
            };
        }
    }

    /// <summary>
    /// 用户等级扩展类
    /// </summary>
    public static class UserRankExtensions
    {
        /// <summary>
        /// 将用户等级转换为EditModel
        /// </summary>
        /// <param name="userRank"></param>
        /// <returns></returns>
        public static UserRankEditModel AsEditModel(this UserRank userRank)
        {
            return new UserRankEditModel
            {
                PointLower = userRank.PointLower,
                Rank = userRank.Rank,
                RankName = userRank.RankName
            };
        }
    }
}
