
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 隐私项目Key管理类
    /// </summary>
    public class PrivacyItemKeys
    {
        #region Instance
        private static PrivacyItemKeys _instance = new PrivacyItemKeys();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static PrivacyItemKeys Instance()
        {
            return _instance;
        }

        private PrivacyItemKeys()
        { }

        #endregion

        //done:zhangp,by zhengw:怎么没写这个类？需要把初始化数据中的ItemKey写成对应的方法来获取

        /// <summary>
        /// 生日
        /// </summary>
        /// <returns></returns>
        public string Birthday()
        {
            return "Birthday";
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        /// <returns></returns>
        public string Mobile()
        {
            return "Mobile";
        }

        /// <summary>
        /// 联系邮箱
        /// </summary>
        /// <returns></returns>
        public string Email()
        {
            return "Email";
        }

        /// <summary>
        /// QQ
        /// </summary>
        /// <returns></returns>
        public string QQ()
        {
            return "QQ";
        }

        /// <summary>
        /// Msn
        /// </summary>
        /// <returns></returns>
        public string Msn()
        {
            return "Msn";
        }

        /// <summary>
        /// 教育信息
        /// </summary>
        /// <returns></returns>
        public string EducationExperience()
        {
            return "EducationExperience";
        }

        /// <summary>
        /// 职业信息
        /// </summary>
        /// <returns></returns>
        public string WorkExperience()
        {
            return "WorkExperience";
        }

        /// <summary>
        /// 空间访问
        /// </summary>
        /// <returns></returns>
        public string VisitUserSpace()
        {
            return "VisitUserSpace";
        }
        
        /// <summary>
        /// 求关注
        /// </summary>
        /// <returns></returns>
        public string InviteFollow()
        {
            return "InviteFollow";
        }

        /// <summary>
        /// 加关注
        /// </summary>
        /// <returns></returns>
        public string Follow()
        {
            return "Follow";
        }

        /// <summary>
        /// 发请求
        /// </summary>
        /// <returns></returns>
        public string Invitation()
        {
            return "Invitation";
        }

        /// <summary>
        /// 发私信
        /// </summary>
        /// <returns></returns>
        public string Message()
        {
            return "Message";
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        public string Comment()
        {
            return "Comment";
        }

        /// <summary>
        /// @提到我
        /// </summary>
        /// <returns></returns>
        public string AtUser()
        {
            return "AtUser";
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        /// <returns></returns>
        public string TrueName()
        {
            return "TrueName";
        }
    }
}