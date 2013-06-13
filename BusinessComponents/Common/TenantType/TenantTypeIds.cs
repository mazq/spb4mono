//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 租户类型Id
    /// </summary>
    public class TenantTypeIds
    {
        #region Instance

        private static volatile TenantTypeIds _instance = null;
        private static readonly object lockObject = new object();

        public static TenantTypeIds Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new TenantTypeIds();
                    }
                }
            }
            return _instance;
        }

        private TenantTypeIds()
        { }

        #endregion Instance

        /// <summary>
        /// 用户
        /// </summary>
        public string User()
        {
            return "000011";
        }

        /// <summary>
        /// 群组
        /// </summary>
        /// <returns></returns>
        public string Group()
        {
            return "101100";
        }

        /// <summary>
        /// 私信
        /// </summary>
        public string Message()
        {
            return "000010";
        }

        /// <summary>
        /// 用户资料
        /// </summary>
        /// <returns></returns>
        public string UserProfile()
        {
            return "000001";
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public string Search()
        {
            return "000021";
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        public string Comment()
        {
            return "000031";
        }

        /// <summary>
        /// 标签
        /// </summary>
        /// <returns></returns>
        public string Tag()
        {
            return "000041";
        }

        /// <summary>
        /// 附件
        /// </summary>
        /// <returns></returns>
        public string Attachment()
        {
            return "000051";
        }

        /// <summary>
        /// 推荐
        /// </summary>
        /// <returns></returns>
        public string Recommend()
        {
            return "000061";
        }
        /// <summary>
        /// 友情链接
        /// </summary>
        /// <returns></returns>
        public string Link()
        {
            return "000071";
        }

        /// <summary>
        /// 站点公告
        /// </summary>
        /// <returns></returns>
        public string Announcement()
        {
            return "000072";
        }

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <returns></returns>
        public string Identification()
        {
            return "000081";
        }
        /// <summary>
        /// 认证标识
        /// </summary>
        /// <returns></returns>
        public string IdentificationType()
        {
            return "000091";
        }

        /// <summary>
        /// 广告
        /// </summary>
        /// <returns></returns>
        public string Advertising()
        {
            return "000073";
        }

        /// <summary>
        /// 广告位
        /// </summary>
        /// <returns></returns>
        public string AdvertisingPosition()
        {
            return "000074";
        }

        /// <summary>
        /// 角色
        /// </summary>
        /// <returns></returns>
        public string Role()
        {
            return "000075";
        }
    }
}
