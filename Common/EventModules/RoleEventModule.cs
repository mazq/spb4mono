//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using Tunynet.Common;
using Tunynet.Events;

namespace Spacebuilder.Common.EventModules
{
    /// <summary>
    /// 用户角色事件处理程序
    /// </summary>
    public class RoleEventModule : IEventMoudle
    {

        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<UserInRole>.Instance().BatchAfter += new BatchEventHandler<UserInRole, CommonEventArgs>(UserInRole_BatchAfter);
            EventBus<UserInRole>.Instance().After += new CommonEventHandler<UserInRole, CommonEventArgs>(UserInRole_After);
        }

        void UserInRole_BatchAfter(IEnumerable<UserInRole> sender, CommonEventArgs eventArgs)
        {
            if (sender == null || sender.Count() == 0)
                return;
            NoticeService noticeService = Tunynet.DIContainer.Resolve<NoticeService>();
            RoleService roleService = new RoleService();
            Notice notice = Notice.New();
            notice.UserId = sender.First().UserId;
            notice.TypeId = NoticeTypeIds.Instance().Hint();
            notice.TemplateName = "RolesChanged";
            List<Role> roles = new List<Role>();
            foreach (var s in sender)
            {
                var role = roleService.Get(s.RoleName);
                if (role == null)
                    continue;
                roles.Add(role);
            }
            notice.RelativeObjectName = string.Join("、", roles.Select(n => n.FriendlyRoleName));
            noticeService.Create(notice);
        }

        void UserInRole_After(UserInRole sender, CommonEventArgs eventArgs)
        {
            RoleService roleService = new RoleService();
            var role = roleService.Get(sender.RoleName);
            if (role == null)
                return;
            NoticeService noticeService = Tunynet.DIContainer.Resolve<NoticeService>();
            Notice notice = Notice.New();
            notice.UserId = sender.UserId;
            notice.TypeId = NoticeTypeIds.Instance().Hint();
            notice.TemplateName = "RoleAdd";
            notice.RelativeObjectName = role.FriendlyRoleName;
            noticeService.Create(notice);
        }
    }
}
