//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Tunynet.Common.Configuration;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 清理垃圾临时附件任务
    /// </summary>
    public class DeleteTrashDataTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            new AttitudeRepository().DeleteTrashDatas();
            new FavoriteRepository().DeleteTrashDatas(MultiTenantServiceKeys.Instance().Favorites());
            new FavoriteRepository().DeleteTrashDatas(MultiTenantServiceKeys.Instance().Subscribe());
            new AttachmentRepository<Attachment>().DeleteTrashDatas();
            new AtUserRepository().DeleteTrashDatas();
            new TagRepository<Tag>().DeleteTrashDatas();
            new CommentRepository().DeleteTrashDatas();
            new CountRepository().DeleteTrashCount();
        }
    }
}
