//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Tunynet.Common.Configuration;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 顶全局设置实现类
    /// </summary>
    public class AttitudeOnlySupportSettingsManager : IAttitudeOnlySupportSettingsManager
    {
        private ISettingsRepository<AttitudeOnlySupportSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public AttitudeOnlySupportSettingsManager()
        {
            repository = new SettingsRepository<AttitudeOnlySupportSettings>();
        }

        /// <summary>
        /// 获取顶设置
        /// </summary>
        /// <returns></returns>
        public AttitudeOnlySupportSettings Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// 保存顶设置
        /// </summary>
        /// <param name="AttitudeOnlySupportSettings"></param>
        public void Save(AttitudeOnlySupportSettings attitudeOnlySupportSettings)
        {
            repository.Save(attitudeOnlySupportSettings);
        }
    }
}