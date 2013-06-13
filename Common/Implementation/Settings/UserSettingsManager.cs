//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using Tunynet.Common.Configuration;
using Tunynet.Common;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户设置实现类
    /// </summary>
    public class UserSettingsManager : IUserSettingsManager
    {
        private ISettingsRepository<UserSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public UserSettingsManager()
        {
            repository = new SettingsRepository<UserSettings>();
        }
       
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns></returns>
        public UserSettings Get()
        {
            return repository.Get();
        }

        public void Save(UserSettings userSettings)
        {
            repository.Save(userSettings);
        }
    }
}
