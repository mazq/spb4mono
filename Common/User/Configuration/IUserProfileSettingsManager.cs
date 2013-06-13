//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.Common.Configuration
{
    public interface IUserProfileSettingsManager
    {

        /// <summary>
        /// 获取UserProfileSettings
        /// </summary>
        /// <returns></returns>
        UserProfileSettings GetUserProfileSettings();

        void SaveUserProfileSettings(UserProfileSettings userProfileSettings);
    }
}
