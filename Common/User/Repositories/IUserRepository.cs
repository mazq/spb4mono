//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet;
using Tunynet.Common;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户数据访问接口
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">待创建的用户</param>
        /// <param name="userCreateStatus">用户帐号创建状态</param>
        /// <param name="ignoreDisallowedUsername">是否忽略禁用的用户名称</param>
        /// <returns>创建成功返回IUser，创建失败返回null</returns>
        IUser CreateUser(User user, bool ignoreDisallowedUsername, out UserCreateStatus userCreateStatus);

        ///	<summary>
        ///	重设密码（无需验证当前密码，供管理员或忘记密码时使用）
        ///	</summary>
        /// <param name="user">用户</param>
        ///	<param name="newPassword">新密码</param>
        ///	<returns>更新成功返回true，否则返回false</returns>
        bool ResetPassword(User user, string newPassword);


        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户Id</returns>
        long GetUserIdByUserName(string userName);

        /// <summary>
        /// 根据昵称获取用户id
        /// </summary>
        /// <param name="nickName">用户昵称</param>
        /// <returns>用户id</returns>
        long GetUserIdByNickName(string nickName);


        /// <summary>
        /// 根据帐号邮箱获取用户
        /// </summary>
        /// <param name="accountEmail">帐号邮箱</param>
        /// <returns>用户Id</returns>
        long GetUserIdByEmail(string accountEmail);

        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="accountMobile">手机号</param>
        /// <returns>用户Id</returns>
        long GetUserIdByMobile(string accountMobile);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        User GetUser(long userId);

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userQuery">查询用户条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>用户分页集合</returns>
        PagingDataSet<User> GetUsers(UserQuery userQuery, int pageSize, int pageIndex);

        /// <summary>
        /// 查询用户
        /// </summary>
        IEnumerable<IUser> GetUsers(List<string> roleName, int minRank = 0, int maxRank = 0);

        /// <summary>
        /// 解除符合解除管制标准的用户（永久管制的用户不会自动解除管制）
        /// </summary>
        /// <param name="noModeratedUserPoint">用户自动接触管制状态所需的积分（用户综合积分）</param>
        /// <returns>被解除管制的用户集合</returns>
        IEnumerable<User> NoModeratedUsers(int noModeratedUserPoint);

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        long UpdateRank(IUser user, int rank);


        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="avatar">用户头像地址</param>
        void UpdateAvatar(IUser user, string avatar);

        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="experiencePoints">经验积分值</param>
        /// <param name="reputationPoints">威望积分值</param>
        /// <param name="tradePoints">交易积分值</param>
        /// <param name="tradePoints2">交易积分值2</param>
        /// <param name="tradePoints3">交易积分值3</param>
        /// <param name="tradePoints4">交易积分值4</param>
        /// <returns></returns>
        long ChangePoints(IUser user, int experiencePoints, int reputationPoints, int tradePoints, int tradePoints2, int tradePoints3, int tradePoints4);

        /// <summary>
        /// 奖励和惩罚用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="description">理由</param>
        /// <param name="experiencePoints">经验积分值</param>
        /// <param name="reputationPoints">威望积分值</param>
        /// <param name="tradePoints">交易积分值</param>
        /// <param name="isIncome">是否是收入</param>
        void RewardAndPunishment(IUser user, string description, int experiencePoints, int reputationPoints, int tradePoints, bool isIncome);

        /// <summary>
        /// 分页获取主键
        /// </summary>
        /// <param name="userQuery">查询用户的条件</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PagingEntityIdCollection FetchPagingPrimaryKeys(UserQuery userQuery, int pageSize, int pageIndex);

        /// <summary>
        /// 根据用户状态获取用户数
        /// </summary>
        /// <param name="isActivated">是否激活</param>
        /// <param name="isBanned">是否封禁</param>
        /// <param name="isModerated">是否管制</param>
        Dictionary<UserManageableCountType, int> GetManageableCounts(bool isActivated, bool isBanned, bool isModerated);

        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">皮肤标识</param>
        void ChangeThemeAppearance(long userId, bool isUseCustomStyle, string themeAppearance);

        /// <summary>
        /// 获取前N个用户
        /// </summary>
        /// <param name="topNumber">获取用户数</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        IEnumerable<User> GetTopUsers(int topNumber, SortBy_User sortBy);

        /// <summary>
        /// 根据排序条件分页显示用户
        /// </summary>
        /// <param name="sortBy">排序条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns>根据排序条件倒排序分页显示用户</returns>
        PagingDataSet<User> GetPagingUsers(SortBy_User? sortBy, int pageIndex, int pageSize);

        /// <summary>
        /// 获取24小时新增用户
        /// </summary>
        /// <returns></returns>
        int GetUser24H();
    }
}