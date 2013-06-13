//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Utilities;
using Spacebuilder.Common;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 帖吧评分的EditModel
    /// </summary>
    public class BarRatingEditModel
    {
        /// <summary>
        /// 评分理由
        /// </summary>
        [Display(Name = "描述")]
        [Required(ErrorMessage = "请输入描述")]
        [StringLength(25, ErrorMessage = "您最多可以输入25个字")]
        public string Reason { get; set; }

        /// <summary>
        /// 评的威望值
        /// </summary>
        [Display(Name = "威望")]
        public int ReputationPoints { get; set; }

        /// <summary>
        /// 剩余的威望值
        /// </summary>
        [Display(Name = "剩余")]
        public int RemainReputationPoints { get; set; }

        /// <summary>
        /// 评论用户的id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 被评论帖子的id
        /// </summary>
        public long ThreadId { get; set; }

        /// <summary>
        /// 评的交易积分
        /// </summary>
        public int TradePoints { get; set; }

        /// <summary>
        /// 转换成数据库存储实体
        /// </summary>
        /// <returns></returns>
        public BarRating AsBarRating()
        {
            return new BarRating
            {
                DateCreated = DateTime.UtcNow,
                IP = WebUtility.GetIP(),
                Reason = this.Reason,
                ReputationPoints = this.ReputationPoints,
                TradePoints = this.TradePoints,
                UserId = UserContext.CurrentUser.UserId,
                ThreadId = this.ThreadId,
                UserDisplayName = UserContext.CurrentUser.DisplayName
            };
        }
    }
}