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
    /// 顶踩全局设置实现类
    /// </summary>
    public class AttitudeSettingsManager : IAttitudeSettingsManager
    {
        private ISettingsRepository<AttitudeSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public AttitudeSettingsManager()
        {
            repository = new SettingsRepository<AttitudeSettings>();
        }
        AttitudeSettings settings = null;

        /// <summary>
        /// 获取顶踩设置
        /// </summary>
        /// <returns></returns>
        public AttitudeSettings Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// 保存顶踩设置
        /// </summary>
        /// <param name="AttitudeSettings"></param>
        public void Save(AttitudeSettings attitudeSettings)
        {
            repository.Save(attitudeSettings);
        }
    }
}