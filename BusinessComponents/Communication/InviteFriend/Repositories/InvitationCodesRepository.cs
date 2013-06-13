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
using Tunynet.Caching;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 邀请码数据访问
    /// </summary>
    public class InvitationCodesRepository : Repository<InvitationCode>, IInvitationCodesRepository
    {
        #region private item
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();
        private int pageSize = 5;
        #endregion
        /// <summary>
        /// 删除邀请码
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="invitationCode">邀请码</param>
        /// <returns>是否成功</returns>
        public bool DeleteInvitationCode(long userId, string invitationCode)
        {
            InvitationCode InvitationCodeEntity = Get(invitationCode);
            if (InvitationCodeEntity == null || InvitationCodeEntity.UserId != userId)
                return false;
            return Delete(InvitationCodeEntity) > 0;
        }

        /// <summary>
        /// 获取我未使用的邀请码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PagingDataSet<InvitationCode> GetMyInvitationCodes(long userId, int pageIndex)
        {
            string cacheKey = GetCacheKey_MyInvitationCodes(userId);
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
             () => { return cacheKey; },
             () =>
             {
                 return PetaPoco.Sql.Builder
                     .Select("*")
                     .From("tn_InvitationCodes")
                     .Where("UserId=@0", userId)
                     .Where("IsMultiple = @0", false)
                     .OrderBy("DateCreated desc");
             });
        }

        /// <summary>
        /// 清除用户数据
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            var sql_Delete = PetaPoco.Sql.Builder
                .Append("delete from tn_InvitationCodes where UserId = @0", userId);
            CreateDAO().Execute(sql_Delete);
        }

        /// <summary>
        /// 清除过期的邀请码
        /// </summary>
        public void DeleteTrashInvitationCodes()
        {
            var sql_Delete = PetaPoco.Sql.Builder
               .Append("delete from tn_InvitationCodes where ExpiredDate < @0", DateTime.UtcNow);
            CreateDAO().Execute(sql_Delete);
        }

        /// <summary>
        /// 获取今天的邀请码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetTodayCode(long userId)
        {
            string cacheKey = GetCacheKey_TodayCode(userId);
            string code = cacheService.Get<string>(cacheKey);
            if (!string.IsNullOrEmpty(code))
                return code;
            DateTime dtNow = DateTime.UtcNow;
            DateTime today = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
            //done:bianchx by libsh,应该加上IsMultiple=true？？
            //回复：已经修改了对应的方法
            var sql_Select = PetaPoco.Sql.Builder
                .Select("Code")
                .From("tn_InvitationCodes")
                .Where("UserId=@0", userId)
                .Where("DateCreated>@0", today)
                .Where("IsMultiple = @0", true);
            code = CreateDAO().FirstOrDefault<string>(sql_Select);
            if (!string.IsNullOrEmpty(code))
                cacheService.Add(cacheKey, code, CachingExpirationType.SingleObject);
            return code;
        }

        /// <summary>
        /// 获取我的邀请码列表的cachekey
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>cachekey</returns>
        private string GetCacheKey_MyInvitationCodes(long userId)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("MyInvitationCodes::UserId-{0};AreaVersion-{1}", userId, areaVersion);
        }

        /// <summary>
        /// 获取今天邀请码cacheke
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>今天邀请码cacheke</returns>
        private string GetCacheKey_TodayCode(long userId)
        {
            return string.Format("TodayCode::userId-{0}", userId);
        }
    }
}
