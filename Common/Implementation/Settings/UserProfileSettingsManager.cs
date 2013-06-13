//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using Tunynet.Common;
using Spacebuilder.Common.Configuration;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    public class UserProfileSettingsManager : IUserProfileSettingsManager
    {
        private ISettingsRepository<UserProfileSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public UserProfileSettingsManager()
        {
            repository = new SettingsRepository<UserProfileSettings>();
        }

        /// <summary>
        /// 获取UserProfileSettings
        /// </summary>
        public UserProfileSettings GetUserProfileSettings()
        {
            return repository.Get();
        }



        public void SaveUserProfileSettings(UserProfileSettings userProfileSettings)
        {
            repository.Save(userProfileSettings);
        }
    }
}
