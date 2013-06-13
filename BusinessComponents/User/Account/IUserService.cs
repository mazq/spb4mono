//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户业务逻辑接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 封禁用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="banDeadline">封禁截止日期</param>
        /// <param name="banReason">封禁原因</param>
        void BanUser(long userId, DateTime banDeadline, string banReason);

        /// <summary>
        /// 解禁用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        void UnbanUser(long userId);

        /// <summary>
        /// 设置用户管制状态
        /// </summary>
        /// <param name="userIds">用户Id集合</param>
        /// <param name="isModerated">是否被管制</param>
        void SetModeratedStatus(IEnumerable<long> userIds, bool isModerated);

        /// <summary>
        /// 变更用户积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="experiencePoints">经验积分值</param>
        /// <param name="reputationPoints">威望积分值</param>
        /// <param name="tradePoints">交易积分值</param>
        /// <param name="tradePoints2">交易积分值2</param>
        /// <param name="tradePoints3">交易积分值3</param>
        /// <param name="tradePoints4">交易积分值4</param>
        void ChangePoints(long userId, int experiencePoints, int reputationPoints, int tradePoints, int tradePoints2 = 0, int tradePoints3 = 0, int tradePoints4 = 0);

        /// <summary>
        /// 冻结交易积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tradePoints">交易积分值</param>
        void FreezeTradePoints(long userId, int tradePoints);

        /// <summary>
        /// 解除冻结交易积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tradePoints">交易积分值</param>
        void UnfreezeTradePoints(long userId, int tradePoints);

        /// <summary>
        /// 减少冻结的交易积分（完成交易时使用）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="frozenTradePoints">需减少的冻结交易积分值</param>
        void ReduceFrozenTradePoints(long userId, int frozenTradePoints);

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="rank">更新后的等级</param>
        void UpdateRank(long userId, int rank);


        #region Get & Gets

        /// <summary>
        /// 根据用户昵称获取用户
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        IUser GetUserByNickName(string nickName);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        IUser GetUser(long userId);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userName">用户名</param>
        IUser GetUser(string userName);

        /// <summary>
        /// 根据账号邮箱获取用户
        /// </summary>
        /// <param name="accountEmail">账号邮箱</param>
        IUser FindUserByEmail(string accountEmail);

        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="accountMobile">手机号</param>
        IUser FindUserByMobile(string accountMobile);

        /// <summary>
        /// 依据UserId集合组装IUser集合
        /// </summary>
        /// <param name="userIds">用户Id集合</param>
        /// <returns></returns>
        IEnumerable<IUser> GetUsers(IEnumerable<long> userIds);

        #endregion


    }
}
