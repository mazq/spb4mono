//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Common
{
    //获取星级评价配置
    public class RatingSettingsManageCancel : IRatingSettingsManager
    {
        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns></returns>
        public RatingSettings Get()
        {
            RatingSettings ratingSettings = new RatingSettings();
            ratingSettings.IsModify = true;
            return ratingSettings;
        }


        public void Save(RatingSettings ratingSettings)
        {
            throw new System.NotImplementedException();
        }
    }
}