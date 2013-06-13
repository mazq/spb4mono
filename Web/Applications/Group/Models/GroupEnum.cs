//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------



namespace Spacebuilder.Group
{
    /// <summary>
    /// 加入群组申请状态
    /// </summary>
    public enum GroupMemberApplyStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 申请成功
        /// </summary>
        Approved = 1,

        /// <summary>
        /// 申请失败
        /// </summary>
        Disapproved = 2
    }


    /// <summary>
    /// 群组成员角色
    /// </summary>
    public enum GroupMemberRole
    {

        /// <summary>
        /// 非群组成员
        /// </summary>
        None = 0,

        /// <summary>
        /// 群组成员
        /// </summary>
        Member = 1,

        /// <summary>
        /// 群组管理员
        /// </summary>
        Manager = 2,

        /// <summary>
        /// 群主
        /// </summary>
        Owner = 3
    }

    /// <summary>
    /// 群组加入方式
    /// </summary>
    public enum JoinWay
    {
        /// <summary>
        /// 直接加入
        /// </summary>
        Direct,

        /// <summary>
        /// 申请加入
        /// </summary>
        ByApply,

        /// <summary>
        /// 仅邀请加入
        /// </summary>
        ByInvite,
        
        
        /// <summary>
        /// 问题验证
        /// </summary>
        ByQuestion
    }


    /// <summary>
    /// 群组排序字段
    /// </summary>
    public enum SortBy_Group
    {
        /// <summary>
        /// 创建时间（倒序）
        /// </summary>
        DateCreated_Desc,

        /// <summary>
        /// 成员数（倒序）
        /// </summary>
        MemberCount_Desc,

        /// <summary>
        /// 成长值（倒序）
        /// </summary>
        GrowthValue_Desc,

        /// <summary>
        /// 浏览数
        /// </summary>
        HitTimes,

        /// <summary>
        /// 阶段浏览计数
        /// </summary>
        StageHitTimes

        
        
    }

    /// <summary>
    /// 群组成员排序字段
    /// </summary>
    public enum SortBy_GroupMember
    {
        /// <summary>
        /// 创建时间（倒序）
        /// </summary>
        DateCreated_Desc,

        /// <summary>
        /// 创建时间（正序）
        /// </summary>
        DateCreated_Asc
    }

    /// <summary>
    /// 群组待审核、需再审核
    /// </summary>
    public enum GroupManageableCountType
    {
        /// <summary>
        /// 待审核
        /// </summary>
        Pending = 1,

        /// <summary>
        /// 需再审核
        /// </summary>
        Again = 2,

        /// <summary>
        /// 总群组数
        /// </summary>
        IsAll = 3,

        /// <summary>
        /// 24小时新增数
        /// </summary>
        IsLast24 = 4
    }

}