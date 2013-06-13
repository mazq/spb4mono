//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Email;
using Tunynet.Events;
using Tunynet.Globalization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户账户业务逻辑
    /// </summary>
    public class MembershipService : IMembershipService
    {
        private IUserRepository userRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public MembershipService()
            : this(new UserRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public MembershipService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">待创建的用户</param>
        /// <param name="password">密码</param>
        /// <param name="userCreateStatus">用户帐号创建状态</param>
        /// <returns>创建成功返回IUser，创建失败返回null</returns>
        public IUser CreateUser(IUser user, string password, out UserCreateStatus userCreateStatus)
        {
            return CreateUser(user, password, string.Empty, string.Empty, false, out userCreateStatus);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">待创建的用户</param>
        /// <param name="password">密码</param>
        /// <param name="passwordQuestion">密码问题</param>
        /// <param name="passwordAnswer">密码答案</param>
        /// <param name="ignoreDisallowedUsername">是否忽略禁用的用户名称</param>
        /// <param name="userCreateStatus">用户帐号创建状态</param>
        /// <returns>创建成功返回IUser，创建失败返回null</returns>
        public IUser CreateUser(IUser user, string password, string passwordQuestion, string passwordAnswer, bool ignoreDisallowedUsername, out UserCreateStatus userCreateStatus)
        {
            User user_object = user as User;
            if (user_object == null)
            {
                userCreateStatus = UserCreateStatus.UnknownFailure;
                return null;
            }

            //密码不合法
            string errorMessage = string.Empty;
            if (!Utility.ValidatePassword(password, out errorMessage))
            {
                userCreateStatus = UserCreateStatus.InvalidPassword;
                return null;
            }

            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();
            user_object.PasswordFormat = (int)userSettings.UserPasswordFormat;
            user_object.Password = UserPasswordHelper.EncodePassword(password, userSettings.UserPasswordFormat);
            user_object.PasswordQuestion = passwordQuestion;
            user_object.PasswordAnswer = passwordAnswer;
            user_object.IsModerated = userSettings.AutomaticModerated;
            EventBus<User, CreateUserEventArgs>.Instance().OnBefore(user_object, new CreateUserEventArgs(password));
            user = userRepository.CreateUser(user_object, ignoreDisallowedUsername, out userCreateStatus);

            if (userCreateStatus == UserCreateStatus.Created)
                EventBus<User, CreateUserEventArgs>.Instance().OnAfter(user_object, new CreateUserEventArgs(password));
            return user;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="takeOverUserName">用于接管删除用户时不能删除的内容(例如：用户创建的群组)</param>
        /// <returns></returns>
        public UserDeleteStatus DeleteUser(long userId, string takeOverUserName)
        {
            return DeleteUser(userId, takeOverUserName, false);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="takeOverUserName">用于接管删除用户的内容(例如：用户创建的群组)</param>
        /// <param name="takeOverAll">是否接管被删除用户的所有内容</param>
        /// <remarks>接管被删除用户的所有内容</remarks>
        /// <returns></returns>
        public UserDeleteStatus DeleteUser(long userId, string takeOverUserName, bool takeOverAll)
        {
            User user = userRepository.Get(userId);
            if (user == null)
                return UserDeleteStatus.DeletingUserNotFound;

            if (takeOverAll)
            {
                long takeOverUserId = userRepository.GetUserIdByUserName(takeOverUserName);
                User takeOverUser = userRepository.Get(takeOverUserId);
                if (takeOverUser == null)
                    return UserDeleteStatus.InvalidTakeOverUsername;
            }

            if (!user.IsModerated && !user.IsForceModerated)
            {
                // 邀请用户被删除时扣除邀请人积分
                PointService pointService = new PointService();
                string pointItemKey = string.Empty;
                pointItemKey = PointItemKeys.Instance().DeleteInvitedUser();

                InviteFriendRecord invitingUser = new InviteFriendService().GetInvitingUserId(user.UserId);
                if (invitingUser != null)
                {
                    string userName = UserIdToUserNameDictionary.GetUserName(invitingUser.UserId);
                    string invitedName = UserIdToUserNameDictionary.GetUserName(user.UserId);
                    string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_DeleteInvitedUser"), userName, invitedName);
                    pointService.GenerateByRole(invitingUser.UserId, pointItemKey, description);
                }
            }
            EventBus<User, DeleteUserEventArgs>.Instance().OnBefore(user, new DeleteUserEventArgs(takeOverUserName, takeOverAll));
            int affectCount = userRepository.Delete(user);

            if (affectCount > 0)
            {
                UserIdToUserNameDictionary.RemoveUserId(userId);
                UserIdToUserNameDictionary.RemoveUserName(user.UserName);
                EventBus<User, DeleteUserEventArgs>.Instance().OnAfter(user, new DeleteUserEventArgs(takeOverUserName, takeOverAll));
                return UserDeleteStatus.Deleted;
            }
            return UserDeleteStatus.UnknownFailure;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(IUser user)
        {
            User user_object = user as User;
            if (user_object == null)
                return;

            EventBus<User>.Instance().OnBefore(user_object, new CommonEventArgs(EventOperationType.Instance().Update()));
            userRepository.Update(user_object);
            EventBus<User>.Instance().OnAfter(user_object, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 批量激活用户
        /// </summary>
        /// <param name="userIds">用户Id集合</param>
        /// <param name="isActivated">激活状态</param>
        public void ActivateUsers(IEnumerable<long> userIds, bool isActivated = true)
        {
            List<User> users = new List<User>();
            foreach (var userId in userIds)
            {
                User user = userRepository.GetUser(userId);
                if (user == null)
                    continue;

                if (user.IsActivated == isActivated)
                    continue;

                user.IsActivated = isActivated;
                user.ForceLogin = !isActivated;

                userRepository.Update(user);
                users.Add(user);
            }
            if (users.Count > 0)
            {
                string eventOperationType = isActivated ? EventOperationType.Instance().ActivateUser() : EventOperationType.Instance().CancelActivateUser();
                EventBus<User>.Instance().OnBatchAfter(users, new CommonEventArgs(eventOperationType));
            }
        }

        ///	<summary>
        ///	更新密码（需要验证当前密码）
        ///	</summary>
        /// <param name="username">用户名</param>
        ///	<param name="password">当前密码</param>
        ///	<param name="newPassword">新密码</param>
        ///	<returns>更新成功返回true，否则返回false</returns>
        public bool ChangePassword(string username, string password, string newPassword)
        {
            if (ValidateUser(username, password) == UserLoginStatus.Success)
                return ResetPassword(username, newPassword);
            return false;
        }

        ///	<summary>
        ///	重设密码（无需验证当前密码，供管理员或忘记密码时使用）
        ///	</summary>
        /// <param name="username">用户名</param>
        ///	<param name="newPassword">新密码</param>
        ///	<remarks>成功时，会自动发送密码已修改邮件</remarks>
        ///	<returns>更新成功返回true，否则返回false</returns>
        public bool ResetPassword(string username, string newPassword)
        {
            long userId = userRepository.GetUserIdByUserName(username);
            User user = userRepository.Get(userId);
            if (user == null)
                return false;

            string storedPassword = UserPasswordHelper.EncodePassword(newPassword, (UserPasswordFormat)user.PasswordFormat);
            EventBus<User>.Instance().OnBefore(user, new CommonEventArgs(EventOperationType.Instance().ResetPassword()));
            bool result = userRepository.ResetPassword(user, storedPassword);

            if (result)
                EventBus<User>.Instance().OnAfter(user, new CommonEventArgs(EventOperationType.Instance().ResetPassword()));

            return result;
        }

        /// <summary>
        /// 验证提供的用户名和密码是否匹配
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回<see cref="UserLoginStatus"/></returns>
        public UserLoginStatus ValidateUser(string username, string password)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(username);

            User user = userRepository.Get(userId);
            if (user == null)
                return UserLoginStatus.InvalidCredentials;

            if (!UserPasswordHelper.CheckPassword(password, user.Password, (UserPasswordFormat)user.PasswordFormat))
                return UserLoginStatus.InvalidCredentials;

            if (!user.IsActivated)
                return UserLoginStatus.NotActivated;
            if (user.IsBanned)
            {
                if (user.BanDeadline >= DateTime.UtcNow)
                    return UserLoginStatus.Banned;
                else
                {
                    user.IsBanned = false;
                    user.BanDeadline = DateTime.UtcNow;
                    userRepository.Update(user);
                }
            }
            return UserLoginStatus.Success;
        }

        /// <summary>
        /// 发送获取密码Email
        /// </summary>
        /// <remarks>
        /// 由具体实现类来决定是否发送之前是否验证accountEmail与username的匹配性
        /// </remarks>
        /// <param name="accountEmail">帐号密码</param>
        /// <param name="username">用户名</param>
        /// <returns>发送成功返回true，否则返回false</returns>
        public bool SendRecoverPasswordEmail(string accountEmail, string username = null)
        {
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.FindUserByEmail(accountEmail);
            if (user == null)
                return false;

            if (!string.IsNullOrEmpty(username) && !user.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                return false;

            EmailService emailService = new EmailService();
            //发送忘记密码邮件
            return emailService.SendAsyn(EmailBuilder.Instance().ResetPassword(user));
        }
    }
}