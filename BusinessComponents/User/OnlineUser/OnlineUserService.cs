//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Tunynet.Common.Repositories;
using System.Threading;
using System.Linq;
using PetaPoco;
using Tunynet.Repositories;
using Tunynet.Common.Configuration;

namespace Tunynet.Common
{
    /// <summary>
    /// 在线用户业务逻辑类
    /// </summary>
    public class OnlineUserService
    {
        private IOnlineUserRepository onlineUserRepository;
        private IOnlineUserStatisticRepository onlineUserStatisticRepository;
        private static ConcurrentDictionary<string, OnlineUser> OnlineUsersForProcess = new ConcurrentDictionary<string, OnlineUser>();

        /// <summary>
        /// 构造器
        /// </summary>
        public OnlineUserService()
            : this(new OnlineUserRepository(), new OnlineUserStatisticRepository())
        {
        }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="onlineUserRepository"></param>
        /// <param name="onlineUserStatisticRepository"></param>
        public OnlineUserService(IOnlineUserRepository onlineUserRepository,IOnlineUserStatisticRepository onlineUserStatisticRepository)
        {
            this.onlineUserRepository = onlineUserRepository;
            this.onlineUserStatisticRepository = onlineUserStatisticRepository;
        }
   
        /// <summary>
        /// 跟踪登录用户
        /// </summary>
        /// <param name="user"></param>
        public void TrackUser(IUser user)
        {
            if (!OnlineUsersForProcess.ContainsKey(user.UserName))
                OnlineUsersForProcess[user.UserName] = OnlineUser.New(user);                
        }

        /// <summary>
        /// 跟踪匿名用户
        /// </summary>
        /// <param name="userName"></param>
        public void TrackAnonymous(string userName)
        {
            if (!OnlineUsersForProcess.ContainsKey(userName))
                OnlineUsersForProcess[userName] = OnlineUser.NewAnonymous(userName);
        }

        /// <summary>
        /// 用户离线（注销时调用）
        /// </summary>
        /// <param name="userName"></param>
        public void Offline(string userName)
        {
           //立即从在线用户数据库移除用户
           onlineUserRepository.Offline(userName);
        }

        /// <summary>
        /// 获取在线登录用户列表
        /// </summary>
        /// <remarks>key=UserName,value=OnlineUser</remarks>
        public Dictionary<string, OnlineUser> GetLoggedUsers()
        {
            //设计要点：
            //1、缓存期限：集合，无需即时，使用一级缓存
           return onlineUserRepository.GetLoggedUsers();
        }

        /// <summary>
        /// 判断用户是否在线
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsOnline(string userName)
        {
            Dictionary<string, OnlineUser> loggedUsers = GetLoggedUsers();
            return loggedUsers.ContainsKey(userName);
        }

        /// <summary>
        /// 获取匿名登录用户列表
        /// </summary>
        /// <remarks>key=UserName,value=OnlineUser</remarks>
        public IList<OnlineUser> GetAnonymousUsers()
        {
            //设计要点：
            //1、缓存期限：集合，无需即时，使用一级缓存

            return onlineUserRepository.GetAnonymousUsers();
        }

        /// <summary>
        /// 获取在线用户用户数量
        /// </summary>
        /// <returns></returns>
        public int GetLoggedUserCount()
        {
            return GetLoggedUsers().Count;
        }

        /// <summary>
        /// 获取在线匿名用户数量
        /// </summary>
        /// <returns></returns>
        public int GetAnonymousCount()
        {
            return GetAnonymousUsers().Count;
        }

        /// <summary>
        /// 刷新数据库
        /// </summary>
        /// <remarks>
        /// 通过Task调用
        /// </remarks>
        public void Refresh()
        {
            //设计要点：
            //1、把OnlineUsersForProcess更新到数据库，如果UserName存在则更新LastActivityTime，否则添加新在线用户
            //   可参考CountRepository.ExecQueue()
            //2、把超期未活动的用户移除
            //3、更新tn_OnlineUserStatistics : 每日一条记录保留最高记录（依据UserCount）
            //done:mazq,by zhengw:tn_OnlineUserStatistics什么时候插入数据？
            onlineUserRepository.Refresh(OnlineUsersForProcess);           
        }

        /// <summary>
        /// 获取历史最高在线记录
        /// </summary>
        /// <returns></returns>
        public OnlineUserStatistic GetHighest()
        {
            //设计要点：
            //1、缓存期限：常用，无需即时，使用一级缓存

            //获取UserCount最高的记录
            return onlineUserStatisticRepository.GetHighest();
        }

        /// <summary>
        /// 获取在线用户统计记录
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <returns></returns>
        public PagingDataSet<OnlineUserStatistic> GetOnlineUserStatistics(DateTime? startDate, DateTime? endDate)
        {
            //设计说明: 
            //缓存期限：常用，无需即时，使用一级缓存
            return onlineUserStatisticRepository.GetOnlineUserStatistics(startDate, endDate);
        }
        
        //done:mazq,by zhengw:如何获取每日在线用户最高纪录统计列表？
        //mazq回复：已修改
    }
}
