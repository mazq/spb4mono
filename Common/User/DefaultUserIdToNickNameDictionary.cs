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
    public class DefaultUserIdToNickNameDictionary : UserIdToNickNameDictionary
    {
        private IUserRepository userRepository;
        /// <summary>
        /// 构造器
        /// </summary>
        public DefaultUserIdToNickNameDictionary()
            : this(new UserRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public DefaultUserIdToNickNameDictionary(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// 根据用户id获取用户昵称
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected override string GetNickNameByUserId(long userId)
        {
            User user = userRepository.Get(userId);
            if (user != null)
                return user.NickName;
            return null;
        }

        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>
        /// 用户Id
        /// </returns>
        protected override long GetUserIdByNickName(string userName)
        {
            return userRepository.GetUserIdByNickName(userName);
        }
    }
}
