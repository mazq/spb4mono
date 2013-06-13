//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace Spacebuilder.Common
{
    /// <summary>
    /// 拥有者类型
    /// </summary>
    public class OwnerTypes
    {
        private static volatile OwnerTypes _instance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 创建拥有者类型实体
        /// </summary>
        /// <returns></returns>
        public static OwnerTypes Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new OwnerTypes();
                    }
                }
            }
            return _instance;
        }

        private OwnerTypes()
        { }

        /// <summary>
        /// 拥有者类型为站点
        /// </summary>
        /// <returns></returns>
        public int Site()
        {
            return 0;
        }

        /// <summary>
        /// 拥有者类型为用户
        /// </summary>
        /// <returns></returns>
        public int User()
        {
            return 1;
        }

        /// <summary>
        /// 拥有者类型为群组
        /// </summary>
        /// <returns></returns>
        public int Group()
        {
            return 2;
        }
    }
}
