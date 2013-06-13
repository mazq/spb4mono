//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using Tunynet.Common;
using Tunynet.Events;

namespace Spacebuilder.Common.EventModules
{
    internal class UserProfileEventModule
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventModule()
        {
            EventBus<User, CropAvatarEventArgs>.Instance().After += new CommonEventHandler<User, CropAvatarEventArgs>(CropAvatar_After);
            
            EventBus<UserProfile>.Instance().After += new CommonEventHandler<UserProfile, CommonEventArgs>(CreateUserProfile_After);
            EventBus<UserProfile>.Instance().After += new CommonEventHandler<UserProfile, CommonEventArgs>(UpdateUserProfile_After);
            EventBus<UserProfile>.Instance().After += new CommonEventHandler<UserProfile, CommonEventArgs>(DeleteUserProfile_After);
        }

        private void CropAvatar_After(User sender, CropAvatarEventArgs eventArgs)
        {
            
        }

        private void CreateUserProfile_After(UserProfile sender, CommonEventArgs eventArgs)
        {
            
        }

        private void UpdateUserProfile_After(UserProfile sender, CommonEventArgs eventArgs)
        {
        }

        private void DeleteUserProfile_After(UserProfile sender, CommonEventArgs eventArgs)
        {
        }
    }
}