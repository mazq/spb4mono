//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户等级的逻辑类
    /// </summary>
    public class UserRankService
    {
        
        //回复：能
        /// <summary>
        /// 带参数的构造函数（测试使用）
        /// </summary>
        /// <param name="userRankRepository">IUserRankRepository</param>
        public UserRankService(IUserRankRepository userRankRepository)
        {
            this.userRankRepository = userRankRepository;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserRankService() : this(new UserRankRepository()) { }

        private IUserRankRepository userRankRepository;

        #region Create/Update/Delete

        /// <summary>
        /// 添加用户等级
        /// </summary>
        /// <param name="userRank">用户等级</param>
        /// <returns>添加成功返回true，否则返回false</returns>
        public bool Create(UserRank userRank)
        {
            EventBus<UserRank>.Instance().OnBefore(userRank, new CommonEventArgs(EventOperationType.Instance().Create()));
            object object_UserRankId = userRankRepository.Insert(userRank);
            if (object_UserRankId == null)
                return false;
            int result = -1;
            int.TryParse(object_UserRankId.ToString(), out result);
            if (result > 0)
                EventBus<UserRank>.Instance().OnAfter(userRank, new CommonEventArgs(EventOperationType.Instance().Create()));
            return result > 0;
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="userRank">用户等级</param>
        public void Update(UserRank userRank)
        {
            EventBus<UserRank>.Instance().OnBefore(userRank, new CommonEventArgs(EventOperationType.Instance().Update()));
            userRankRepository.Update(userRank);
            EventBus<UserRank>.Instance().OnAfter(userRank, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除用户等级
        /// </summary>
        /// <param name="rank">用户级别</param>        
        public void Delete(int rank)
        {
            UserRank userRank = userRankRepository.Get(rank);
            if (userRank == null)
                return;
            EventBus<UserRank>.Instance().OnBefore(userRank, new CommonEventArgs(EventOperationType.Instance().Delete()));
            userRankRepository.Delete(userRank);
            EventBus<UserRank>.Instance().OnAfter(userRank, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// 依据现行规则重置所有用户等级
        /// </summary>
        public void ResetAllUser()
        {
            //按照定义的用户等级设定的积分区间更新用户等级（不考虑缓存即时性）
            userRankRepository.ResetAllUser();
        }

        #endregion


        #region Get

        /// <summary>
        /// 获取用户等级
        /// </summary>
        /// <param name="rank">用户级别(int)</param>
        /// <returns>用户等级实体</returns>
        public UserRank Get(int rank)
        {
            return userRankRepository.Get(rank);
        }

        /// <summary>
        /// 获取所有用户等级
        /// </summary>
        /// <remarks>key=rank , value=UserRank</remarks>
        /// <returns>所有用户级别实体集合</returns>
        public SortedList<int, UserRank> GetAll()
        {
            IEnumerable<UserRank> userRanks = userRankRepository.GetAll();
            SortedList<int, UserRank> lists = new SortedList<int, UserRank>();
            foreach (var userRank in userRanks)
                lists.Add(userRank.Rank, userRank);
            return lists;
        }

        #endregion



    }
}
