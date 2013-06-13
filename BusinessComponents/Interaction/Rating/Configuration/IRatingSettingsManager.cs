//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace Tunynet.Common
{
    /// <summary>
    ///  RatingSettingsManager管理器接口
    /// </summary>
    public interface IRatingSettingsManager
    {
        /// <summary>
        /// 获取RatingSettings
        /// </summary>
        RatingSettings Get();

        /// <summary>
        /// 获取RatingSettings
        /// </summary>
        void Save(RatingSettings ratingSettings);
    }
}