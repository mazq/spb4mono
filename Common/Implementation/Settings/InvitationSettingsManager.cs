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
    /// 请求全局设置实现类
    /// </summary>
    public class InvitationSettingsManager : IInvitationSettingsManager
    {

        private ISettingsRepository<InvitationSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public InvitationSettingsManager()
        {
            repository = new SettingsRepository<InvitationSettings>();
        }

        /// <summary>
        /// 获取请求全局设置
        /// </summary>
        /// <returns></returns>
        public InvitationSettings Get()
        {
            return repository.Get();
        }


        public void Save(InvitationSettings invitationSettings)
        {
            repository.Save(invitationSettings);
        }
    }
}