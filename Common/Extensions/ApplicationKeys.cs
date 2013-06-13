//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

/// <summary>
/// 获取应用Id
/// </summary>
public class ApplicationKeys
{
    #region Instance

        private static volatile ApplicationKeys _instance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 创建主页实体
        /// </summary>
        /// <returns></returns>
        public static ApplicationKeys Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationKeys();
                    }
                }
            }
            return _instance;
        }

        private ApplicationKeys()
        { }

        #endregion Instance
}