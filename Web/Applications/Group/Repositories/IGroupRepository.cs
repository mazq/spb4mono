//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet;
using Tunynet.Common;
using Tunynet.Caching;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组仓储接口
    /// </summary>
    public interface IGroupRepository : IRepository<GroupEntity>
    {
        /// <summary>
        /// 根据群组Key获取群组Id
        /// </summary>
        /// <param name="groupKey">群组Key</param>
        /// <returns>群组Id</returns>
        long GetGroupIdByGroupKey(string groupKey);

        /// <summary>
        /// 获取前N个排行群组
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        IEnumerable<GroupEntity> GetTops(int topNumber, string areaCode, long? categoryId, SortBy_Group sortBy);

        /// <summary>
        /// 获取匹配的前N个排行群组
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="keyword">关键字</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        IEnumerable<GroupEntity> GetMatchTops(int topNumber, string keyword, string areaCode, long? categoryId, SortBy_Group sortBy);

        /// <summary>
        /// 根据标签名获取群组分页集合
        /// </summary>
        /// <param name="tagName">标签名</param></param>
        /// <param name="sortBy">排序依据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页列表</returns>
        PagingDataSet<GroupEntity> GetsByTag(string tagName, SortBy_Group sortBy, int pageSize, int pageIndex);

        /// <summary>
        /// 获取用户创建的群组列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<GroupEntity> GetMyCreatedGroups(long userId, bool ignoreAudit);

        /// <summary>
        /// 分页获取排行数据
        /// </summary>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PagingDataSet<GroupEntity> Gets(string areaCode, long? categoryId, SortBy_Group sortBy, int pageSize, int pageIndex);

        /// <summary>
        /// 群组成员也加入的群组
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        IEnumerable<GroupEntity> GroupMemberAlsoJoinedGroups(long groupId, int topNumber);

        /// <summary>
        /// 获取用户加入的群组列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PagingDataSet<GroupEntity> GetMyJoinedGroups(long userId, int pageSize, int pageIndex);

        /// <summary>
        /// 分页获取群组后台管理列表
        /// </summary>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="keywords">名称关键字</param>
        /// <param name="ownerUserId">群主</param>
        /// <param name="minDateTime">创建时间下限值</param>
        /// <param name="maxDateTime">创建时间上限值</param>
        /// <param name="minMemberCount">成员数量下限值</param>
        /// <param name="maxMemberCount">成员数量上限值</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PagingDataSet<GroupEntity> GetsForAdmin(AuditStatus? auditStatus, long? categoryId, string keywords, long? ownerUserId, DateTime? minDateTime, DateTime? maxDateTime, int? minMemberCount, int? maxMemberCount, int pageSize, int pageIndex);

        /// <summary>
        /// 更换群主
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="newOwnerUserId">新群主UserId</param>
        void ChangeGroupOwner(long groupId, long newOwnerUserId);

        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">皮肤标识</param>
        void ChangeThemeAppearance(long groupId, bool isUseCustomStyle, string themeAppearance);


        /// <summary>
        /// 每天定时计算各个群组的成长值
        /// </summary>
        void CalculateGrowthValues();

        /// <summary>
        /// 删除用户记录（删除用户时使用）
        /// </summary>
        /// <param name="userId">被删除用户</param>
        /// <param name="takeOver">接管用户</param>
        /// <param name="takeOverAll">是否接管被删除用户的所有内容</param>
        void DeleteUser(long userId, Common.User takeOver, bool takeOverAll);

        /// <summary>
        /// 根据审核状态获取群组数
        /// </summary>
        /// <param name="Pending">待审核</param>
        /// <param name="Again">需再审核</param>
        /// <returns></returns>
        Dictionary<GroupManageableCountType, int> GetManageableCounts();

        /// <summary>
        /// 获取群组管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        Dictionary<string, long> GetManageableDatas(string tenantTypeId = null);

        /// <summary>
        /// 获取群组统计数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        Dictionary<string, long> GetStatisticDatas(string tenantTypeId = null);


        /// <summary>
        /// 获取我关注的用户加入的群组
        /// </summary>
        /// <param name="userId">当前用户的userId</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        IEnumerable<GroupEntity> FollowedUserAlsoJoinedGroups(long userId, int topNumber);

    }
}