//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 帖吧设置的EditModel
    /// </summary>
    public class SectionSettingsEditModel
    {
        /// <summary>
        /// 是否启用评分
        /// </summary>
        public bool EnableRating { get; set; }

        /// <summary>
        /// 威望评分最小值
        /// </summary>
        [Required(ErrorMessage = "请输入最小值")]
        [Range(-10, 10, ErrorMessage = "威望评分区间应在{1}与{2}之间")]
        [RegularExpression(@"[\d]+", ErrorMessage = "威望最小评分必须是数字")]
        public int ReputationPointsMinValue { get; set; }

        /// <summary>
        /// 威望评分最大值
        /// </summary>
        [Required(ErrorMessage = "请输入最大值")]
        [Range(-100, 100, ErrorMessage = "威望评分区间应在{1}与{2}之间")]
        [RegularExpression(@"[\d]+", ErrorMessage = "威望最大评分必须是数字")]
        public int ReputationPointsMaxValue { get; set; }

        /// <summary>
        /// 用户每日威望评分上限
        /// </summary>
        [Required(ErrorMessage = "请输入评分上限")]
        [Range(-1000, 1000, ErrorMessage = "威望评分区间应在{1}与{2}之间")]
        [RegularExpression(@"[\d]+", ErrorMessage = "每日威望评分上限必须是数字")]
        public int UserReputationPointsPerDay { get; set; }

        /// <summary>
        /// 是否仅允许关注用户发帖
        /// </summary>
        public bool OnlyFollowerCreateThread { get; set; }

        /// <summary>
        /// 是否允许用户创建帖吧
        /// </summary>
        public bool EnableUserCreateSection { get; set; }

        /// <summary>
        /// 允许申请帖吧的用户等级下限
        /// </summary>
        [Required(ErrorMessage = "请输入等级下限")]
        [RegularExpression(@"[\d]+", ErrorMessage = "申请贴吧等级下限必须是数字")]
        public int UserRankOfCreateSection { get; set; }

        /// <summary>
        /// 帖子标题的最大长度
        /// </summary>
        [Required(ErrorMessage = "请输入帖子标题的最大长度")]
        public int ThreadSubjectMaxLength { get; set; }

        /// <summary>
        /// 帖子内容的最大长度
        /// </summary>
        [Required(ErrorMessage = "请输入帖子内容的最大长度")]
        public int ThreadBodyMaxLength { get; set; }

        /// <summary>
        /// 回帖内容的最大长度
        /// </summary>
        [Required(ErrorMessage = "请输入回帖内容的最大长度")]
        public int PostBodyMaxLength { get; set; }

        /// <summary>
        /// 转换成BarSetting
        /// </summary>
        /// <returns></returns>
        public BarSettings AsBarSettings()
        {
            return new BarSettings
            {
                UserReputationPointsPerDay = this.UserReputationPointsPerDay,
                UserRankOfCreateSection = this.UserRankOfCreateSection,
                ReputationPointsMinValue = this.ReputationPointsMinValue,
                ReputationPointsMaxValue = this.ReputationPointsMaxValue,
                OnlyFollowerCreateThread = this.OnlyFollowerCreateThread,
                EnableUserCreateSection = this.EnableUserCreateSection,
                ThreadSubjectMaxLength = this.ThreadSubjectMaxLength,
                ThreadBodyMaxLength = this.ThreadBodyMaxLength,
                PostBodyMaxLength = this.PostBodyMaxLength,
                EnableRating = this.EnableRating
            };
        }
    }

    /// <summary>
    /// 帖吧设置扩展类
    /// </summary>
    public static class SectionSettingsEditModelExtensions
    {
        /// <summary>
        /// 转换为EditModel
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>EditModel</returns>
        public static SectionSettingsEditModel AsEditModel(this BarSettings settings)
        {
            return new SectionSettingsEditModel
            {
                EnableRating = settings.EnableRating,
                EnableUserCreateSection = settings.EnableUserCreateSection,
                OnlyFollowerCreateThread = settings.OnlyFollowerCreateThread,
                ReputationPointsMaxValue = settings.ReputationPointsMaxValue,
                ReputationPointsMinValue = settings.ReputationPointsMinValue,
                UserRankOfCreateSection = settings.UserRankOfCreateSection,
                ThreadSubjectMaxLength = settings.ThreadSubjectMaxLength,
                ThreadBodyMaxLength = settings.ThreadBodyMaxLength,
                PostBodyMaxLength = settings.PostBodyMaxLength,
                UserReputationPointsPerDay = settings.UserReputationPointsPerDay
            };
        }
    }
}