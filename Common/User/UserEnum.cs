//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 性别类型
    /// </summary>
    public enum GenderType
    {
        /// <summary>
        /// 未设置
        /// </summary>
        NotSet = 0,
        /// <summary>
        /// 男
        /// </summary>
        Male = 1,
        /// <summary>
        /// 女
        /// </summary>
        FeMale = 2
    }

    /// <summary>
    /// 学历类型
    /// </summary>
    public enum DegreeType
    {
        /// <summary>
        /// 小学
        /// </summary>
        [Display(Name = "小学")]
        PrimarySchool = 7,
        /// <summary>
        /// 初中
        /// </summary>
        [Display(Name = "初中")]
        MiddleSchool = 6,
        /// <summary>
        /// 中专/技校
        /// </summary>
        [Display(Name = "中专/技校")]
        VocationalSchool = 5,
        /// <summary>
        /// 高中
        /// </summary>
        [Display(Name = "高中")]
        HighSchool = 4,
        /// <summary>
        /// 大专
        /// </summary>
        [Display(Name = "大专")]
        CommunityCollege = 3,
        /// <summary>
        /// 本科
        /// </summary>
        [Display(Name = "本科")]
        Undergraduate = 2,
        /// <summary>
        /// 硕士
        /// </summary>
        [Display(Name = "硕士")]
        Master = 1,
        /// <summary>
        /// 博士
        /// </summary>
        [Display(Name = "博士")]
        Doctor = 0

    }

    /// <summary>
    /// 生日类型
    /// </summary>
    public enum BirthdayType
    {
        /// <summary>
        /// 公历生日
        /// </summary>
        Birthday = 1,
        /// <summary>
        /// 阴历生日
        /// </summary>
        LunarBirthday = 2
    }

    /// <summary>
    /// 证件类型
    /// </summary>
    public enum CertificateType
    {
        /// <summary>
        /// 居民身份证
        /// </summary>
        Residentcard = 0,
        /// <summary>
        /// 军官证
        /// </summary>
        SergeantsCard = 1,
        /// <summary>
        /// 学生证
        /// </summary>
        StudentCard = 2,
        /// <summary>
        /// 驾驶证
        /// </summary>
        DriverCard = 3,
        /// <summary>
        /// 护照
        /// </summary>
        passport = 4,
        /// <summary>
        /// 港澳通行证
        /// </summary>
        HongKongPermit = 5
    }

    /// <summary>
    /// 头像尺寸类型
    /// </summary>
    public enum AvatarSizeType
    {
        /// <summary>
        /// 原始尺寸
        /// </summary>
        Original = 0,

        /// <summary>
        /// 大头像
        /// </summary>
        Big = 1,

        /// <summary>
        /// 中头像
        /// </summary>
        Medium = 2,

        /// <summary>
        /// 小头像
        /// </summary>
        Small = 3,

        /// <summary>
        /// 微头像
        /// </summary>
        Micro = 4
    }

    /// <summary>
    /// 用户资料完整度有关项目
    /// </summary>
    public enum ProfileIntegrityItems
    {
        /// <summary>
        /// 头像
        /// </summary>
        Avatar = 0,

        /// <summary>
        /// 生日
        /// </summary>
        Birthday = 1,

        /// <summary>
        /// 所在地
        /// </summary>
        NowArea = 2,

        /// <summary>
        /// 家乡
        /// </summary>
        HomeArea = 3,

        /// <summary>
        /// 即时通讯帐号
        /// </summary>
        IM = 4,

        /// <summary>
        /// 手机号码
        /// </summary>
        Mobile = 5,

        /// <summary>
        /// 教育经历
        /// </summary>
        EducationExperience = 6,

        /// <summary>
        /// 工作经历
        /// </summary>
        WorkExperience = 7,

        /// <summary>
        /// 自我介绍
        /// </summary>
        Introduction = 8
    }

    /// <summary>
    /// 用户激活、管制、封禁数
    /// </summary>
    public enum UserManageableCountType
    {
        /// <summary>
        /// 激活
        /// </summary>
        IsActivated = 1,

        /// <summary>
        /// 封禁
        /// </summary>
        IsBanned = 2,

        /// <summary>
        /// 管制
        /// </summary>
        IsModerated = 3,

        /// <summary>
        /// 总用户数
        /// </summary>
        IsAll = 4,

        /// <summary>
        /// 24小时新增数
        /// </summary>
        IsLast24 = 5

    }

    /// <summary>
    /// 用户排序字段
    /// </summary>
    public enum SortBy_User
    {
        FollowerCount,
        ReputationPoints,
        PreWeekReputationPoints,
        HitTimes,
        TradePoints,
        PreWeekHitTimes,
        Rank,
        DateCreated
    }
}