//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using PetaPoco;
using Spacebuilder.Common.Configuration;
using Tunynet;
using Tunynet.Repositories;
using System.Collections.Generic;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户资料的数据访问
    /// </summary>
    public class ProfileRepository : Repository<UserProfile>, IProfileRepository
    {
        /// <summary>
        /// 检查用户是否存在用户资料
        /// </summary>
        /// <param name="userId">用户Id</param>
        public bool UserIdIsExist(long userId)
        {
            var sql = Sql.Builder;
            sql.Select("Count(*)")
               .From("spb_Profiles")
               .Where("UserId = @0", userId);
            int count = CreateDAO().ExecuteScalar<int>(sql);
            return count > 0;
        }

        /// <summary>
        /// 更新完成度
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void UpdateIntegrity(long userId)
        {
            UserProfileSettings userProfileSettings = DIContainer.Resolve<IUserProfileSettingsManager>().GetUserProfileSettings();
            int[] integrityItems = userProfileSettings.IntegrityProportions;
            int integrity = integrityItems[(int)ProfileIntegrityItems.Birthday];
            
            PetaPocoDatabase dao =CreateDAO();
            dao.OpenSharedConnection();
            var sql = Sql.Builder;
            sql.Select("Count(*)")
               .From("spb_EducationExperiences")
               .Where("UserId = @0", userId);

            int countEducation = dao.ExecuteScalar<int>(sql);

            if (countEducation > 0)
            {
                integrity += integrityItems[(int)ProfileIntegrityItems.EducationExperience];
            }

            sql = Sql.Builder;
            sql.Select("count(*)")
               .From("spb_WorkExperiences")
               .Where("UserId = @0", userId);
            int countWork = dao.ExecuteScalar<int>(sql);

            if (countWork > 0)
            {
                integrity += integrityItems[(int)ProfileIntegrityItems.WorkExperience];
            }

            sql = Sql.Builder;
            sql.Where("userId = @0", userId);
            UserProfile userProfile = dao.FirstOrDefault<UserProfile>(sql);
            if (userProfile != null)
            {
                IUser user = new UserService().GetUser(userProfile.UserId);

                integrity += (user.HasAvatar ? integrityItems[(int)ProfileIntegrityItems.Avatar] : 0);
                integrity += (userProfile.HasHomeAreaCode ? integrityItems[(int)ProfileIntegrityItems.HomeArea] : 0);
                integrity += (userProfile.HasIM ? integrityItems[(int)ProfileIntegrityItems.IM] : 0);
                integrity += (userProfile.HasIntroduction ? integrityItems[(int)ProfileIntegrityItems.Introduction] : 0);
                integrity += (userProfile.HasMobile ? integrityItems[(int)ProfileIntegrityItems.Mobile] : 0);
                integrity += (userProfile.HasNowAreaCode ? integrityItems[(int)ProfileIntegrityItems.NowArea] : 0);

                userProfile.Integrity = integrity;
                Update(userProfile);
            }

            dao.CloseSharedConnection();
        }

    }
}