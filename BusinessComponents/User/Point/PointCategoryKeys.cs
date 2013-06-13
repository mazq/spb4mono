//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 积分类型配置类（便于使用PointCategoryKey）
    /// </summary>
    public class PointCategoryKeys
    {
        #region Instance
        private static PointCategoryKeys _instance = new PointCategoryKeys();

        /// <summary>
        /// 获取该类的单例
        /// </summary>
        /// <returns></returns>
        public static PointCategoryKeys Instance()
        {
            return _instance;
        }

        private PointCategoryKeys()
        { }
        #endregion

        /// <summary>
        /// 经验
        /// </summary>
        /// <returns></returns>
        public string ExperiencePoints()
        {
            return "ExperiencePoints";
        }
        /// <summary>
        /// 威望
        /// </summary>
        /// <returns></returns>
        public string ReputationPoints()
        {
            return "ReputationPoints";
        }
        /// <summary>
        /// 金币
        /// </summary>
        /// <returns></returns>
        public string TradePoints()
        {
            return "TradePoints";
        }
    }
}
