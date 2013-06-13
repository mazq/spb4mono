//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

/// <summary>
/// 获取应用Id
/// </summary>
public class ApplicationIds
{
    #region Instance

        private static volatile ApplicationIds _instance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 创建主页实体
        /// </summary>
        /// <returns></returns>
        public static ApplicationIds Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationIds();
                    }
                }
            }
            return _instance;
        }

        private ApplicationIds()
        { }

        #endregion Instance
}