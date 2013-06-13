//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spacebuilder.Common.Repositories;
using Tunynet.Common;

namespace Spacebuilder.Common.Implementation.Settings
{
    public class PauseSiteSettingsManager : IPauseSiteSettingsManager
    {
        private ISettingsRepository<PauseSiteSettings> repository;

        /// <summary>
        /// 构造器
        /// </summary>
        public PauseSiteSettingsManager()
        {
            repository = new SettingsRepository<PauseSiteSettings>();
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="pauseSiteSettings"></param>
        public void Save(PauseSiteSettings pauseSiteSettings)
        {
            repository.Save(pauseSiteSettings);
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        public PauseSiteSettings get()
        {
            return repository.Get();
        }
    }
}
