//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 链接时效性设置实现类
    /// </summary>
    public class LinktimelinessSettingsManager : ILinktimelinessSettingsManager
    {
        private ISettingsRepository<LinktimelinessSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public LinktimelinessSettingsManager()
        {
            repository = new SettingsRepository<LinktimelinessSettings>();
        }

        
        /// <summary>
        /// 获取链接时效性设置
        /// </summary>
        /// <returns></returns>
        public LinktimelinessSettings Get()
        {
            return repository.Get();
        }


        public void Save(LinktimelinessSettings linktimelinessSettings)
        {
            repository.Save(linktimelinessSettings);
        }
    }
}
