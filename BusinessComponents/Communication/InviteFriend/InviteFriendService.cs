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
using Tunynet.Utilities;

namespace Tunynet.Common
{
    /// <summary>
    /// 邀请好友业务逻辑类
    /// </summary>
    public class InviteFriendService
    {
        /// <summary>
        /// 不带参数的构造方法
        /// </summary>
        public InviteFriendService() : this(new InvitationCodesRepository(), new InvitationCodeStatisticsRepository(), new InviteFriendRecordsRepository()) { }

        /// <summary>
        ///  带参数的初始化方法(主要应用于测试用例)
        /// </summary>
        /// <param name="invitationCodesRepository"></param>
        /// <param name="invitationCodeStatisticsRepository"></param>
        /// <param name="inviteFriendRecordsRepository"></param>
        public InviteFriendService(IInvitationCodesRepository invitationCodesRepository, IInvitationCodeStatisticsRepository invitationCodeStatisticsRepository, IInviteFriendRecordsRepository inviteFriendRecordsRepository)
        {
            this.invitationCodesRepository = invitationCodesRepository;
            this.invitationCodeStatisticsRepository = invitationCodeStatisticsRepository;
            this.inviteFriendRecordsRepository = inviteFriendRecordsRepository;
        }

        #region private items

        private IInvitationCodesRepository invitationCodesRepository = null;
        private IInvitationCodeStatisticsRepository invitationCodeStatisticsRepository = null;
        private IInviteFriendRecordsRepository inviteFriendRecordsRepository = null;

        #endregion

        //关于缓存期限：
        //1、用户邀请码配额 使用CachingExpirationType.SingleObject
        //2、邀请码实体、列表 使用CachingExpirationType.SingleObject、ObjectCollection
        //3、邀请好友记录列表 使用CachingExpirationType.ObjectCollection

        #region 邀请码

        //done:zhengw,by mazq 方法名不好理解 或者直接与上一个方法合并叫 GetInvitationCode(long userId) 有什么问题？
        //zhengw回复：已修改

        /// <summary>
        /// 获取邀请码
        /// </summary>
        /// <param name="userId">申请人</param>
        public string GetInvitationCode(long userId)
        {
            
            //回复：我本身也没有判断吧。
            string code = EncryptionUtility.MD5_16(userId.ToString() + DateTime.UtcNow.Ticks.ToString());
            IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            if (inviteFriendSettings.AllowInvitationCodeUseOnce)
            {
                InvitationCodeStatistic invitationCodeStatistic = GetUserInvitationCodeStatistic(userId);
                //如果配额够用的
                if (ChangeUserInvitationCodeCount(userId, -1, 1, 1))
                {
                    InvitationCode invitationCode = new InvitationCode
                    {
                        Code = code,
                        DateCreated = DateTime.UtcNow,
                        ExpiredDate = DateTime.UtcNow.AddDays(inviteFriendSettings.InvitationCodeTimeLiness),
                        IsMultiple = !inviteFriendSettings.AllowInvitationCodeUseOnce,
                        UserId = userId
                    };
                    invitationCodesRepository.Insert(invitationCode);
                }
                else
                {
                    code = string.Empty;
                }
                //1.用户未使用邀请码配额减1，然后调用ChangeUserInvitationCodeCount进行更新
                //2.过期时间根据InviteFriendSettings.InvitationCodeTimeLiness确定
            }
            else
            {
                string todayCode = invitationCodesRepository.GetTodayCode(userId);
                if (string.IsNullOrEmpty(todayCode))
                {
                    ILinktimelinessSettingsManager manager = DIContainer.Resolve<ILinktimelinessSettingsManager>();
                    InvitationCode invitationCode = new InvitationCode
                    {
                        Code = code,
                        DateCreated = DateTime.UtcNow,
                        ExpiredDate = DateTime.UtcNow.AddDays(inviteFriendSettings.InvitationCodeTimeLiness),
                        IsMultiple = !inviteFriendSettings.AllowInvitationCodeUseOnce,
                        UserId = userId
                    };
                    invitationCodesRepository.Insert(invitationCode);
                }
                else
                {
                    code = todayCode;
                }
                //检查今日是否有生成过的可多次使用的邀请码，若没有，则生成；否则，直接返回
                //过期时间根据LinktimelinessSettings.Lowlinktimeliness确定
                //设置IsMultiple为true
            }
            //向邀请码表中插入数据库
            return code;
        }

        /// <summary>
        /// 获取邀请码实体
        /// </summary>
        /// <param name="invitationCode">邀请码</param>
        public InvitationCode GetInvitationCodeEntity(string invitationCode)
        {
            InvitationCode invitation = invitationCodesRepository.Get(invitationCode);
            IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            if (invitation == null || inviteFriendSettings.AllowInvitationCodeUseOnce == invitation.IsMultiple)
                return null;
            return invitation;
        }

        /// <summary>
        /// 用户购买邀请码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="invitationCodeCount">购买的邀请码数量</param>
        public bool BuyInvitationCodes(long userId, int invitationCodeCount)
        {
            IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            int requiredTradePoints = invitationCodeCount * inviteFriendSettings.InvitationCodeUnitPrice;

            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.GetUser(userId);
            if (user == null || user.TradePoints < requiredTradePoints)
                return false;
            PointService pointService = new PointService();
            pointService.TradeToSystem(userId, requiredTradePoints, "购买邀请码", true);
            return ChangeUserInvitationCodeCount(userId, invitationCodeCount, 0, invitationCodeCount);
            

            //检查用户交易积分是否足以支付购买invitationCodeCount个邀请码配额
            //用户的未使用邀请码配额、购买的邀请码配额加invitationCodeCount，然后调用ChangeUserInvitationCodeCount进行更新
            
            //mazq回复：增加了TradeToSystem方法
        }

        /// <summary>
        /// 删除邀请码（当邀请码被使用时进行调用）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="invitationCode">邀请码</param>
        public bool DeleteInvitationCode(long userId, string invitationCode)
        {
            return invitationCodesRepository.DeleteInvitationCode(userId, invitationCode);
        }

        /// <summary>
        /// 批量删除过期的邀请码
        /// </summary>
        public void DeleteTrashInvitationCodes()
        {
            
            invitationCodesRepository.DeleteTrashInvitationCodes();
        }

        //done:zhengw,by mazq
        //1、什么情况会用到？
        //2、如何实现？
        //zhengw回复：已删除

        /// <summary>
        /// 获取我的未使用邀请码列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>未使用邀请码列表</returns>
        public PagingDataSet<InvitationCode> GetMyInvitationCodes(long userId, int pageIndex = 1)
        {
            return invitationCodesRepository.GetMyInvitationCodes(userId, pageIndex);
        }

        #endregion

        #region 邀请码配额
        /// <summary>
        /// 变更用户邀请码配额
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userInvitationCodeUnUsedCount">用户未使用邀请码配额增量（若减少请使用负数）</param>
        /// <param name="userInvitationCodeUsedCount">用户使用的邀请码配额增量（若减少请使用负数）</param>
        /// <param name="userInvitationCodeBuyedCount">用户购买的邀请码配额增量（若减少请使用负数）</param>
        /// <returns>是否更新成功</returns>
        public bool ChangeUserInvitationCodeCount(long userId, int userInvitationCodeUnUsedCount, int userInvitationCodeUsedCount, int userInvitationCodeBuyedCount)
        {
            InvitationCodeStatistic invitationCodeStatistic = GetUserInvitationCodeStatistic(userId);
            //done:bianchx by libsh,等于0的时候也不行
            //回复：已经修改了对应的逻辑，不允许在配额不足的时候更新。但是允许在配额不足的时候购买配额
            if (invitationCodeStatistic.CodeUnUsedCount <= 0 && userInvitationCodeUnUsedCount <= 0)
                return false;
            //1、需检查用户邀请码数额统计表是否有数据，若没有则创建，否则进行更新
            //2、用户默认邀请码配额需要使用InviteFriendSettings.DefaultUserInvitationCodeCount  

            return invitationCodeStatisticsRepository.ChangeUserInvitationCodeCount(userId, userInvitationCodeUnUsedCount, userInvitationCodeUsedCount, userInvitationCodeBuyedCount);
        }

        /// <summary>
        /// 获取用户邀请码统计实体
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户邀请码统计实体</returns>
        public InvitationCodeStatistic GetUserInvitationCodeStatistic(long userId)
        {
            InvitationCodeStatistic invitationCodeStatistic = invitationCodeStatisticsRepository.Get(userId);
            if (invitationCodeStatistic == null)
            {
                IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
                InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
                invitationCodeStatistic = new InvitationCodeStatistic
                {
                    CodeUnUsedCount = inviteFriendSettings.DefaultUserInvitationCodeCount,
                    CodeUsedCount = 0,
                    CodeBuyedCount = 0,
                    UserId = userId
                };
            }
            return invitationCodeStatistic;
        }

        #endregion

        #region 邀请好友记录

        /// <summary>
        /// 创建邀请好友记录
        /// </summary>
        /// <param name="inviteFriendRecord">被创建的记录实体</param>
        public void CreateInviteFriendRecord(InviteFriendRecord inviteFriendRecord)
        {
            inviteFriendRecordsRepository.Insert(inviteFriendRecord);
            EventBus<InviteFriendRecord>.Instance().OnAfter(inviteFriendRecord, new CommonEventArgs(EventOperationType.Instance().Create(), 0));
        }

        /// <summary>
        /// 通过被邀请人ID获取邀请人
        /// </summary>
        /// <param name="userId">被邀请人ID</param>
        /// <returns></returns>
        public InviteFriendRecord GetInvitingUserId(long userId)
        {
            return inviteFriendRecordsRepository.GetInvitingUserId(userId);
        }

        /// <summary>
        /// 获取我的邀请好友记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>被邀请的好友Id列表</returns>
        public PagingEntityIdCollection GetMyInviteFriendRecords(long userId, int pageIndex)
        {
            //done:zhengw,by mazq 没看到分页？
            //zhengw回复：已修改
            //mazq回复：怎么获取第几页的数据
            //zhengw回复：已修改

            return inviteFriendRecordsRepository.GetMyInviteFriendRecords(userId, pageIndex);
        }

        #endregion

        /// <summary>
        /// 删除用户的所有邀请好友记录（删除用户的时候使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            invitationCodesRepository.CleanByUser(userId);
            invitationCodeStatisticsRepository.CleanByUser(userId);
            inviteFriendRecordsRepository.CleanByUser(userId);
        }

        /// <summary>
        /// 记录邀请用户奖励
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void RewardingUser(long userId)
        {

            inviteFriendRecordsRepository.RewardingUser(userId);
        }
    }
}
