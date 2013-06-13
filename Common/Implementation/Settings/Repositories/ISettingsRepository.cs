//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.Common.Repositories
{
    /// <summary>
    /// 设置Repository接口
    /// </summary>
    /// <typeparam name="TSettingsEntity">设置的实体类</typeparam>
    public interface ISettingsRepository<TSettingsEntity> where TSettingsEntity : class
    {
        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns>settings</returns>
        TSettingsEntity Get();

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="settings">settings</param>
        void Save(TSettingsEntity settings);

    }
}
