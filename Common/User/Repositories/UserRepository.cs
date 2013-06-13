//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Repositories;
using Tunynet.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户数据访问
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();
        private IPointRecordRepository pointRecordRepository = new PointRecordRepository();

        #region Create/Update/Delete

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">待创建的用户</param>
        /// <param name="ignoreDisallowedUsername">是否忽略禁用的用户名称</param>
        /// <param name="userCreateStatus">用户帐号创建状态</param>
        /// <returns>创建成功返回IUser，创建失败返回null</returns>
        public IUser CreateUser(User user, bool ignoreDisallowedUsername, out UserCreateStatus userCreateStatus)
        {
            userCreateStatus = UserCreateStatus.UnknownFailure;

            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();
            if (!ignoreDisallowedUsername)
            {
                if (userSettings.DisallowedUserNames.Split(new char[] { ',', '，' }).Any(n => n.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    //用户输入字段为禁用字段
                    userCreateStatus = UserCreateStatus.DisallowedUsername;
                    return null;
                }
            }

            //判断用户名是否唯一
            if (GetUserIdByUserName(user.UserName) > 0)
            {
                userCreateStatus = UserCreateStatus.DuplicateUsername;
                return null;
            }

            if (GetUser(user.UserId) != null)
            {
                userCreateStatus = UserCreateStatus.DuplicateUsername;
                return null;
            }

            //判断邮箱是否唯一
            if (GetUserIdByEmail(user.AccountEmail) > 0)
            {
                userCreateStatus = UserCreateStatus.DuplicateEmailAddress;
                return null;
            }
            //如果不允许手机号重复的时候
            if (!userSettings.RequiresUniqueMobile)
            {
                if (GetUserIdByMobile(user.AccountMobile) > 0)
                {
                    userCreateStatus = UserCreateStatus.DuplicateMobile;
                    return null;
                }
            }
            user.UserId = IdGenerator.Next();
            object userId_objcet = base.Insert(user);

            if (userId_objcet != null && (long)userId_objcet > 0)
                userCreateStatus = UserCreateStatus.Created;
            return user;
        }

        ///	<summary>
        ///	重设密码（无需验证当前密码，供管理员或忘记密码时使用）
        ///	</summary>
        /// <param name="user">用户</param>
        ///	<param name="newPassword">新密码</param>
        ///	<returns>更新成功返回true，否则返回false</returns>
        public bool ResetPassword(User user, string newPassword)
        {
            if (user == null)
                return false;
            var sql_update = PetaPoco.Sql.Builder;
            sql_update.Append("update tn_Users set Password = @0 where UserId = @1", newPassword, user.UserId);
            int affectCount = CreateDAO().Execute(sql_update);
            if (affectCount > 0)
            {
                user.Password = newPassword;
                base.OnUpdated(user);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 解除符合解除管制标准的用户（永久管制的用户不会自动解除管制）
        /// </summary>
        /// <param name="noModeratedUserPoint">用户自动接触管制状态所需的积分（用户综合积分）</param>
        /// <returns>被解除管制的用户集合</returns>
        public IEnumerable<User> NoModeratedUsers(int noModeratedUserPoint)
        {
            if (noModeratedUserPoint <= 0)
                return new List<User>();
            PointSettings pointSettings = DIContainer.Resolve<IPointSettingsManager>().Get();

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            var sql_select = PetaPoco.Sql.Builder;
            sql_select.Select("UserId")
            .From("tn_Users")
            .Where("IsForceModerated = @0 ", false)
            .Where("IsModerated = @0", true);

            sql_select.Where("ExperiencePoints * @0 + ReputationPoints * @1 > @2", pointSettings.ExperiencePointsCoefficient, pointSettings.ReputationPointsCoefficient, noModeratedUserPoint);
            IEnumerable<object> userIds_object = dao.FetchFirstColumn(sql_select);

            var sql_update = PetaPoco.Sql.Builder;
            sql_update.Append("update tn_Users set IsModerated = @0 ", false)
            .Where("IsForceModerated = @0 ", false)
            .Where("IsModerated = @0", true)
            .Where("ExperiencePoints * @0 + ReputationPoints * @1 > @2", pointSettings.ExperiencePointsCoefficient, pointSettings.ReputationPointsCoefficient, noModeratedUserPoint);

            dao.Execute(sql_update);
            dao.CloseSharedConnection();

            return PopulateEntitiesByEntityIds<long>(userIds_object.Cast<long>());
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="rank">用户等级</param>
        /// <returns>受影响条数</returns>
        public long UpdateRank(IUser user, int rank)
        {
            if (user == null)
                return -1;
            User updateUser = user as User;

            var sql_Update = PetaPoco.Sql.Builder;
            sql_Update.Append("update tn_Users set Rank=@0 where UserId = @1", rank, user.UserId);
            int affectedCount = CreateDAO().Execute(sql_Update);

            updateUser.Rank = rank;
            base.OnUpdated(updateUser);
            return affectedCount;
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="avatar">用户头像地址</param>
        public void UpdateAvatar(IUser user, string avatar)
        {
            if (user == null)
                return;

            var sql_Update = PetaPoco.Sql.Builder;
            sql_Update.Append("update tn_Users set Avatar=@0 where UserId = @1", avatar, user.UserId);
            int affectedCount = CreateDAO().Execute(sql_Update);

            User updateUser = user as User;
            updateUser.Avatar = avatar;
            base.OnUpdated(updateUser);
        }

        /// <summary>
        /// 更改用户积分
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <param name="experiencePoints">经验积分值</param>
        /// <param name="reputationPoints">威望积分值</param>
        /// <param name="tradePoints">交易积分值</param>
        /// <param name="tradePoints2">交易积分值2</param>
        /// <param name="tradePoints3">交易积分值3</param>
        /// <param name="tradePoints4">交易积分值4</param>
        /// <returns></returns>
        public long ChangePoints(IUser user, int experiencePoints, int reputationPoints, int tradePoints, int tradePoints2, int tradePoints3, int tradePoints4)
        {
            if (user == null)
                return -1;
            User updateUser = Get(user.UserId);

            var sql_Update = PetaPoco.Sql.Builder;
            sql_Update.Append("update tn_Users set ExperiencePoints=ExperiencePoints+@0,ReputationPoints=ReputationPoints+@1,TradePoints=TradePoints+@2,TradePoints2=TradePoints2+@3,TradePoints3=TradePoints3+@4,TradePoints4=TradePoints4+@5 where UserId = @6", experiencePoints, reputationPoints, tradePoints, tradePoints2, tradePoints3, tradePoints4, user.UserId);
            int affectedCount = CreateDAO().Execute(sql_Update);

            updateUser.ExperiencePoints += experiencePoints;
            updateUser.ReputationPoints += reputationPoints;
            updateUser.TradePoints += tradePoints;
            updateUser.TradePoints2 += tradePoints2;
            updateUser.TradePoints3 += tradePoints3;
            updateUser.TradePoints4 += tradePoints4;
            base.OnUpdated(updateUser);

            return affectedCount;
        }

        /// <summary>
        /// 奖励和惩罚用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="description">理由</param>
        /// <param name="experiencePoints">经验积分值</param>
        /// <param name="reputationPoints">威望积分值</param>
        /// <param name="tradePoints">交易积分值</param>
        /// <param name="isIncome">是否是收入</param>
        public void RewardAndPunishment(IUser user, string description, int experiencePoints, int reputationPoints, int tradePoints, bool isIncome)
        {
            //更新用户积分
            ChangePoints(user, experiencePoints, reputationPoints, tradePoints, 0, 0, 0);

            //产生积分记录
            PointRecord pointRecord = PointRecord.New();
            pointRecord.UserId = user.UserId;
            if (isIncome)
            {
                pointRecord.PointItemName = "系统奖励";
            }
            else
            {
                pointRecord.PointItemName = "系统扣除";
            }
            if (string.IsNullOrEmpty(description))
            {
                if (isIncome)
                {
                    pointRecord.Description = "系统奖励了你经验:" + experiencePoints + ",威望:" + reputationPoints + ",金币:" + tradePoints;
                }
                else
                {
                    pointRecord.Description = "系统扣除了你经验:" + -experiencePoints + ",威望:" + -reputationPoints + ",金币:" + -tradePoints;
                }
            }
            else
            {
                if (isIncome)
                {
                    pointRecord.Description = description + ",系统奖励了你经验:" + experiencePoints + ",威望:" + reputationPoints + ",金币:" + tradePoints;
                }
                else
                {
                    pointRecord.Description = description + ",系统扣除了你经验:" + -experiencePoints + ",威望:" + -reputationPoints + ",金币:" + -tradePoints;
                }
            }
            pointRecord.ExperiencePoints = experiencePoints;
            pointRecord.ReputationPoints = reputationPoints;
            pointRecord.TradePoints = tradePoints;
            pointRecord.TradePoints2 = 0;
            pointRecord.TradePoints3 = 0;
            pointRecord.TradePoints4 = 0;
            pointRecord.IsIncome = isIncome;
            pointRecordRepository.Insert(pointRecord);
        }

        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">皮肤标识</param>
        public void ChangeThemeAppearance(long userId, bool isUseCustomStyle, string themeAppearance)
        {
            User updateUser = Get(userId);
            if (updateUser == null)
                return;

            var sql_Update = PetaPoco.Sql.Builder;
            sql_Update.Append("update tn_Users set ThemeAppearance = @0,IsUseCustomStyle = @1 where UserId = @2", themeAppearance ?? string.Empty, isUseCustomStyle, userId);
            int affectedCount = CreateDAO().Execute(sql_Update);
            if (affectedCount > 0)
            {
                updateUser.ThemeAppearance = themeAppearance ?? string.Empty;
                updateUser.IsUseCustomStyle = isUseCustomStyle;
                base.OnUpdated(updateUser);
            }
        }

        #endregion

        #region Get && Gets

        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户Id</returns>
        public long GetUserIdByUserName(string userName)
        {
            var sql_Select = Sql.Builder.Select("UserId").From("tn_Users").Where("UserName = @0", userName);
            return CreateDAO().FirstOrDefault<long>(sql_Select);
        }

        /// <summary>
        /// 获取用户id根据用户昵称
        /// </summary>
        /// <param name="nickName">用户昵称</param>
        /// <returns>用户id</returns>
        public long GetUserIdByNickName(string nickName)
        {
            var sql_Select = Sql.Builder.Select("UserId").From("tn_Users").Where("NickName = @0", nickName);
            return CreateDAO().FirstOrDefault<long>(sql_Select);
        }

        /// <summary>
        /// 根据帐号邮箱获取用户
        /// </summary>
        /// <param name="accountEmail">帐号邮箱</param>
        /// <returns>用户Id</returns>
        public long GetUserIdByEmail(string accountEmail)
        {
            var sql_Select = Sql.Builder;
            sql_Select.Append("select UserId from tn_Users where AccountEmail = @0", accountEmail);
            return CreateDAO().FirstOrDefault<long>(sql_Select);
        }

        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="accountMobile">手机号</param>
        /// <returns>用户Id</returns>
        public long GetUserIdByMobile(string accountMobile)
        {
            var sql_Select = Sql.Builder;
            sql_Select.Append("select UserId from tn_Users where AccountMobile = @0", accountMobile);
            return CreateDAO().FirstOrDefault<long>(sql_Select);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId">用户ID</param>        
        public User GetUser(long userId)
        {
            User user = base.Get(userId);
            return user;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userQuery">查询用户条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>用户分页集合</returns>
        public PagingDataSet<User> GetUsers(UserQuery userQuery, int pageSize, int pageIndex)
        {
            var sql_select = PetaPoco.Sql.Builder;
            sql_select.Select("*").From("tn_Users");

            buildSqlWhere(userQuery, ref sql_select);

            switch (userQuery.UserSortBy)
            {
                case UserSortBy.UserId:
                    sql_select.OrderBy("UserId");
                    break;
                case UserSortBy.UserId_Desc:
                    sql_select.OrderBy("UserId DESC");
                    break;
                case UserSortBy.LastActivityTime:
                    sql_select.OrderBy("LastActivityTime");
                    break;
                case UserSortBy.LastActivityTime_Desc:
                    sql_select.OrderBy("LastActivityTime DESC");
                    break;
                case UserSortBy.IsActivated:
                    sql_select.OrderBy("IsActivated");
                    break;
                case UserSortBy.IsActivated_Desc:
                    sql_select.OrderBy("IsActivated DESC");
                    break;
                case UserSortBy.IsModerated:
                    sql_select.OrderBy("IsModerated");
                    break;
                case UserSortBy.IsModerated_Desc:
                    sql_select.OrderBy("IsModerated DESC");
                    break;
                default:
                    sql_select.OrderBy("UserId DESC");
                    break;
            }

            //if (!string.IsNullOrEmpty(userQuery.Keyword) || !string.IsNullOrEmpty(userQuery.AccountEmailFilter) || userQuery.RegisterTimeLowerLimit.HasValue || userQuery.RegisterTimeUpperLimit.HasValue)
            return GetPagingEntities(pageSize, pageIndex, sql_select);
            //else
            //    return base.GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
            //         () =>
            //         {
            //             StringBuilder sb = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.GlobalVersion));
            //             if (!string.IsNullOrEmpty(userQuery.RoleName))
            //                 sb.AppendFormat("UserRoleName-{0}", userQuery.RoleName);
            //             if (userQuery.IsActivated.HasValue)
            //                 sb.AppendFormat("IsActivated-{0}", userQuery.IsActivated);
            //             if (userQuery.IsBanned.HasValue)
            //                 sb.AppendFormat("IsBanned-{0}", userQuery.IsBanned);
            //             if (userQuery.IsModerated.HasValue)
            //                 sb.AppendFormat("IsModerated-{0}", userQuery.IsModerated);
            //             if (userQuery.UserRankLowerLimit.HasValue)
            //                 sb.AppendFormat("UserRankLowerLimit-{0}", userQuery.UserRankLowerLimit);
            //             if (userQuery.UserRankUpperLimit.HasValue)
            //                 sb.AppendFormat("UserRankUpperLimit-{0}", userQuery.UserRankUpperLimit);
            //             sb.AppendFormat("UserSortBy-{0}", ((int?)userQuery.UserSortBy ?? 2));
            //             return sb.ToString();
            //         },
            //         () =>
            //         {
            //             return sql_select;
            //         });
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="roleName">用户角色</param>
        /// <param name="minRank">最低等级</param>
        /// <param name="maxRank">最高等级</param>
        public IEnumerable<IUser> GetUsers(List<string> roleName, int minRank = 0, int maxRank = 0)
        {
            Sql sql = Sql.Builder;
            sql.Select("tn_Users.UserId")
                .From("tn_Users");

            if (minRank > 0)
            {
                sql.Where("tn_Users.Rank>=@0", minRank);
            }
            if (maxRank > 0)
            {
                sql.Where("tn_Users.Rank<=@0", maxRank);
            }
            if (roleName != null && roleName.Count() > 0)
            {
                sql.InnerJoin("tn_UsersInRoles")
                    .On("tn_Users.UserId=tn_UsersInRoles.UserId")
                    .Where("tn_UsersInRoles.RoleName in (@0)", roleName);
            }
            IEnumerable<long> userIds = CreateDAO().Fetch<long>(sql);
            return PopulateEntitiesByEntityIds(userIds);

        }


        /// <summary>
        /// 根据用户状态获取用户数
        /// </summary>
        /// <param name="isActivated">是否激活</param>
        /// <param name="isBanned">是否封禁</param>
        /// <param name="isModerated">是否管制</param>
        public Dictionary<UserManageableCountType, int> GetManageableCounts(bool isActivated, bool isBanned, bool isModerated)
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            Dictionary<UserManageableCountType, int> countType = new Dictionary<UserManageableCountType, int>();
            var sql_selectIsActivated = PetaPoco.Sql.Builder;
            sql_selectIsActivated.Select("count(*)").From("tn_Users");
            sql_selectIsActivated.Where("IsActivated=@0", isActivated);

            countType[UserManageableCountType.IsActivated] = dao.FirstOrDefault<int>(sql_selectIsActivated);

            var sql_selectIsBanned = PetaPoco.Sql.Builder;
            sql_selectIsBanned.Select("count(*)").From("tn_Users");
            sql_selectIsBanned.Where("IsBanned=@0", isBanned);

            countType[UserManageableCountType.IsBanned] = dao.FirstOrDefault<int>(sql_selectIsBanned);

            var sql_selectIsModerated = PetaPoco.Sql.Builder;
            sql_selectIsModerated.Select("count(*)").From("tn_Users");
            sql_selectIsModerated.Where("IsModerated=@0", isModerated);

            countType[UserManageableCountType.IsModerated] = dao.FirstOrDefault<int>(sql_selectIsModerated);

            var sql_selectIsAll = PetaPoco.Sql.Builder;
            sql_selectIsAll.Select("count(*)").From("tn_Users");

            countType[UserManageableCountType.IsAll] = dao.FirstOrDefault<int>(sql_selectIsAll);

            var sql_selectIsLast24 = PetaPoco.Sql.Builder;
            sql_selectIsLast24.Select("count(*)").From("tn_Users");
            sql_selectIsLast24.Where("DateCreated > @0 and  DateCreated < @1", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));

            countType[UserManageableCountType.IsLast24] = dao.FirstOrDefault<int>(sql_selectIsLast24);

            dao.CloseSharedConnection();

            return countType;
        }

        /// <summary>
        /// 获取前N个用户
        /// </summary>
        /// <param name="topNumber">获取用户数</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<User> GetTopUsers(int topNumber, SortBy_User sortBy)
        {
            IEnumerable<User> topUsers = null;
            topUsers = GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                () =>
                {
                    //获取缓存
                    StringBuilder cacheKey = new StringBuilder("TopUsers:");
                    cacheKey.AppendFormat("SortBy-{0}", (int)sortBy);

                    return cacheKey.ToString();
                },
                () =>
                {
                    var sql = PetaPoco.Sql.Builder;
                    var whereSql = Sql.Builder;
                    var orderSql = Sql.Builder;
                    whereSql.Where("IsActivated =1 and IsBanned = 0");

                    CountService countService = new CountService(TenantTypeIds.Instance().User());
                    StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().User());
                    string countTableName = countService.GetTableName_Counts();
                    int stageCountDays;
                    string stageCountType;

                    switch (sortBy)
                    {
                        case SortBy_User.FollowerCount:
                            orderSql.OrderBy("FollowerCount desc");
                            break;
                        case SortBy_User.ReputationPoints:
                            orderSql.OrderBy("ReputationPoints desc");
                            break;
                        case SortBy_User.DateCreated:
                            orderSql.OrderBy("UserId desc");
                            break;
                        case SortBy_User.PreWeekHitTimes:
                            stageCountDays = 7;
                            stageCountType = stageCountTypeManager.GetStageCountType(CountTypes.Instance().HitTimes(), stageCountDays);
                            sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, stageCountType))
                            .On("UserId = c.ObjectId");
                            orderSql.OrderBy("c.StatisticsCount desc");
                            break;
                        case SortBy_User.HitTimes:
                            sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, CountTypes.Instance().HitTimes()))
                            .On("UserId = c.ObjectId");
                            orderSql.OrderBy("c.StatisticsCount desc");
                            break;
                        case SortBy_User.PreWeekReputationPoints:
                            stageCountDays = 7;
                            stageCountType = stageCountTypeManager.GetStageCountType(CountTypes.Instance().ReputationPointsCounts(), stageCountDays);
                            sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, stageCountType))
                            .On("UserId = c.ObjectId");
                            orderSql.OrderBy("c.StatisticsCount desc");
                            break;
                        default:
                            orderSql.OrderBy("FollowerCount desc");
                            break;
                    }
                    return sql.Append(whereSql).Append(orderSql);
                });
            return topUsers;
        }

        /// <summary>
        /// 根据排序条件分页显示用户
        /// </summary>
        /// <param name="sortBy">排序条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns>根据排序条件倒排序分页显示用户</returns>
        public PagingDataSet<User> GetPagingUsers(SortBy_User? sortBy, int pageIndex, int pageSize)
        {
            PagingDataSet<User> users =
            GetPagingEntities(pageSize, pageIndex, Tunynet.Caching.CachingExpirationType.ObjectCollection,
            () =>
            {
                StringBuilder cacheKey = new StringBuilder("PagingTopUsers:");
                cacheKey.AppendFormat("SortBy-{0}", sortBy);

                return cacheKey.ToString();
            },
            () =>
            {
                var sql = PetaPoco.Sql.Builder;
                var whereSql = Sql.Builder;
                var orderSql = Sql.Builder;
                whereSql.Where("IsActivated =1 and IsBanned = 0");

                CountService countService = new CountService(TenantTypeIds.Instance().User());
                StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().User());
                string countTableName = countService.GetTableName_Counts();
                int stageCountDays;
                string stageCountType;

                if (sortBy.HasValue)
                {
                    switch (sortBy)
                    {
                        case SortBy_User.FollowerCount:
                            orderSql.OrderBy("FollowerCount desc");
                            break;
                        case SortBy_User.Rank:
                            orderSql.OrderBy("Rank desc");
                            break;
                        case SortBy_User.ReputationPoints:
                            orderSql.OrderBy("ReputationPoints desc");
                            break;
                        case SortBy_User.TradePoints:
                            orderSql.OrderBy("TradePoints desc");
                            break;
                        case SortBy_User.DateCreated:
                            orderSql.OrderBy("UserId desc");
                            break;
                        case SortBy_User.PreWeekHitTimes:
                            stageCountDays = 7;
                            stageCountType = stageCountTypeManager.GetStageCountType(CountTypes.Instance().HitTimes(), stageCountDays);
                            sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, stageCountType))
                            .On("UserId = c.ObjectId");
                            orderSql.OrderBy("c.StatisticsCount desc");
                            break;
                        case SortBy_User.HitTimes:
                            sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, CountTypes.Instance().HitTimes()))
                            .On("UserId = c.ObjectId");
                            orderSql.OrderBy("c.StatisticsCount desc");
                            break;
                        case SortBy_User.PreWeekReputationPoints:
                            stageCountDays = 7;
                            stageCountType = stageCountTypeManager.GetStageCountType(CountTypes.Instance().ReputationPointsCounts(), stageCountDays);
                            sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, stageCountType))
                            .On("UserId = c.ObjectId");
                            orderSql.OrderBy("c.StatisticsCount desc");
                            break;
                        default:
                            orderSql.OrderBy("UserId desc");
                            break;
                    }
                }
                return sql.Append(whereSql).Append(orderSql);
            });
            return users;
        }
        #endregion

        /// <summary>
        /// 从UserQuery构建PetaPoco.Sql的where条件
        /// </summary>
        /// <param name="userQuery">UserQuery查询条件</param>
        /// <param name="sql">PetaPoco.Sql对象</param>
        private void buildSqlWhere(UserQuery userQuery, ref PetaPoco.Sql sql)
        {
            if (sql == null)
            {
                sql = PetaPoco.Sql.Builder;
            }

            if (!string.IsNullOrEmpty(userQuery.AccountEmailFilter))
                sql.Where("AccountEmail like @0", "%" + userQuery.AccountEmailFilter + "%");
            if (userQuery.IsActivated.HasValue)
                sql.Where("IsActivated = @0", userQuery.IsActivated);
            if (userQuery.IsBanned.HasValue)
                sql.Where("IsBanned = @0", userQuery.IsBanned);
            if (userQuery.IsModerated.HasValue)
            {
                if (userQuery.IsModerated.Value)
                {
                    sql.Where("IsModerated = @0 or IsForceModerated = @0", userQuery.IsModerated);
                }
                else
                {
                    sql.Where("IsModerated = @0 and IsForceModerated = @0", userQuery.IsModerated);
                }
            }

            if (!string.IsNullOrEmpty(userQuery.Keyword))
                sql.Where("UserName like @0 or TrueName like @0 or NickName like @0", "%" + userQuery.Keyword + "%");
            if (!string.IsNullOrEmpty(userQuery.RoleName))
                sql.Where("UserId in (Select UserId from tn_UsersInRoles where RoleName = @0)", userQuery.RoleName);
            if (userQuery.RegisterTimeLowerLimit.HasValue)
                sql.Where("DateCreated >= @0", userQuery.RegisterTimeLowerLimit.Value.ToUniversalTime());
            if (userQuery.RegisterTimeUpperLimit.HasValue)
                sql.Where("DateCreated <= @0", userQuery.RegisterTimeUpperLimit.Value.AddDays(1).ToUniversalTime());
            if (userQuery.UserRankLowerLimit.HasValue)
                sql.Where("Rank >= @0", userQuery.UserRankLowerLimit);
            if (userQuery.UserRankUpperLimit.HasValue)
                sql.Where("Rank<=@0", userQuery.UserRankUpperLimit);
        }

        #region 用户搜索建索引相关的查询

        /// <summary>
        /// 分页获取主键
        /// </summary>
        /// <param name="userQuery">查询用户的条件</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingEntityIdCollection FetchPagingPrimaryKeys(UserQuery userQuery, int pageSize, int pageIndex)
        {
            PetaPoco.Sql sql = sql = PetaPoco.Sql.Builder;
            buildSqlWhere(userQuery, ref sql);

            return CreateDAO().FetchPagingPrimaryKeys<User>((long)pageSize, pageSize, pageIndex, sql);
        }

        #endregion

        /// <summary>
        /// 获取24小时新增用户
        /// </summary>
        /// <returns></returns>
        public int GetUser24H()
        {
            Sql sql = Sql.Builder;
            sql.Select("count(*)")
                .From("tn_Users")
                .Where("DateCreated >@0", DateTime.UtcNow.AddDays(-1));
            int userCount24H =  CreateDAO().FirstOrDefault<int>(sql);
            return userCount24H;

        }


    }
}
