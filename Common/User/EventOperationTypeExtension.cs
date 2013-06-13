//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 扩展用户业务逻辑
    /// </summary>
    public static class EventOperationTypeExtension
    {

        /// <summary>
        /// 封禁用户
        /// </summary>
        /// <param name="eventOperationType"></param>      
        /// <returns></returns>
        public static string BanUser(this EventOperationType eventOperationType)
        {
            return "BanUser";
        }
        /// <summary>
        /// 解禁用户
        /// </summary>
        /// <param name="eventOperationType"></param>      
        /// <returns></returns>
        public static string UnbanUser(this EventOperationType eventOperationType)
        {
            return "UnbanUser";
        }

        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="eventOperationType"></param>      
        /// <returns></returns>
        public static string ResetPassword(this EventOperationType eventOperationType)
        {
            return "ResetPassword";
        }

        /// <summary>
        /// 激活用户
        /// </summary>
        /// <param name="eventOperationType"></param>      
        /// <returns></returns>
        public static string ActivateUser(this EventOperationType eventOperationType)
        {
            return "ActivateUser";
        }

        /// <summary>
        /// 取消激活用户
        /// </summary>
        /// <param name="eventOperationType"></param>      
        /// <returns></returns>
        public static string CancelActivateUser(this EventOperationType eventOperationType)
        {
            return "NoActivateUser";
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="eventOperationType"></param>      
        /// <returns></returns>
        public static string SignIn(this EventOperationType eventOperationType)
        {
            return "SignIn";
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="eventOperationType"></param>
        /// <returns></returns>
        public static string SignOut(this EventOperationType eventOperationType)
        {
            return "SignOut";
        }

        /// <summary>
        ///  管制用户 
        /// </summary>
        /// <param name="eventOperationType"></param>
        /// <returns></returns>
        public static string ModerateUser(this EventOperationType eventOperationType)
        {
            return "ModerateUser";
        }

        /// <summary>
        /// 取消管制用户
        /// </summary>
        /// <param name="eventOperationType"></param>
        /// <returns></returns>
        public static string CancelModerateUser(this EventOperationType eventOperationType)
        {
            return "CancelModerateUser";
        }

        /// <summary>
        /// 依据规则自动解除管制用户
        /// </summary>
        /// <param name="eventOperationType"></param>
        /// <returns></returns>
        public static string AutoNoModeratedUser(this EventOperationType eventOperationType)
        {
            return "AutoNoModeratedUser";
        }
        
            
        /// <summary>
        /// 帐号邮箱通过验证
        /// </summary>
        /// <param name="eventOperationType"></param>
        /// <returns></returns>
        public static string UserEmailVerified(this EventOperationType eventOperationType)
        {
            return "UserEmailVerified";
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="eventOperationType"></param>
        /// <returns></returns>
        public static string DeleteUser(this EventOperationType eventOperationType)
        {
            return "DeleteUser";
        } 


    }
}
