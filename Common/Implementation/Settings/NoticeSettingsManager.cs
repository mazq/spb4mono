//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Common;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 通知全局设置实现类
    /// </summary>
    public class NoticeSettingsManager : INoticeSettingsManager
    {
        private ISettingsRepository<NoticeSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public NoticeSettingsManager()
        {
            repository = new SettingsRepository<NoticeSettings>();
        }
        /// <summary>
        /// 获取用户的全局设置
        /// </summary>
        /// <returns></returns>
        public NoticeSettings Get()
        {
            return repository.Get();
        }


        public void Save(NoticeSettings noticeSettings)
        {
            repository.Save(noticeSettings);
        }
    }
}