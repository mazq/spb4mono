//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace Spacebuilder.Common
{
    /// <summary>
    /// 通过用户数据仓储实现查询
    /// </summary>
    public class DefaultUserIdToUserNameDictionary : UserIdToUserNameDictionary
    {
        private IUserRepository userRepository;
        /// <summary>
        /// 构造器
        /// </summary>
        public DefaultUserIdToUserNameDictionary()
            : this(new UserRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public DefaultUserIdToUserNameDictionary(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// 根据用户Id获取用户名
        /// </summary>
        /// <returns>
        /// 用户名
        /// </returns>
        protected override string GetUserNameByUserId(long userId)
        {
            User user = userRepository.Get(userId);
            if (user != null)
                return user.UserName;
            return null;
        }

        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>
        /// 用户Id
        /// </returns>
        protected override long GetUserIdByUserName(string userName)
        {
            return userRepository.GetUserIdByUserName(userName);
        }
    }
}
