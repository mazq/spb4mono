//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using Tunynet.Common.Configuration;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 积分设置实现类
    /// </summary>
    public class PointSettingsManager : IPointSettingsManager
    {
        private ISettingsRepository<PointSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public PointSettingsManager()
        {
            repository = new SettingsRepository<PointSettings>();
        }

        /// <summary>
        /// 获取积分设置
        /// </summary>
        /// <returns></returns>
        public PointSettings Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// 保存积分设置
        /// </summary>
        /// <returns></returns>
        public void Save(PointSettings pointSettings)
        {
            repository.Save(pointSettings);
        }
    }
}