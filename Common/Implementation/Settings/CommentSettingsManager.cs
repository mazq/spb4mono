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
    /// 地区全局设置实现类
    /// </summary>
    public class CommentSettingsManager : ICommentSettingsManager
    {
        private ISettingsRepository<CommentSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public CommentSettingsManager()
        {
            repository = new SettingsRepository<CommentSettings>();
        }

        /// <summary>
        /// 获取LogoSettings
        /// </summary>
        /// <returns></returns>
        public CommentSettings Get()
        {
            return repository.Get();
        }


        public void Save(CommentSettings settings)
        {
            repository.Save(settings);
        }
    }
}