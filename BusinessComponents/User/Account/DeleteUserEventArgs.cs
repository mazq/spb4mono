//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 删除用户事件参数
    /// </summary>
    public class DeleteUserEventArgs : CommonEventArgs
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="takeOverUserName">用于接管删除用户时不能删除的内容(例如：用户创建的群组)</param>
        /// <param name="takeOverAll">是否接管被删除用户的所有内容</param>
        public DeleteUserEventArgs(string takeOverUserName, bool takeOverAll)
            : base(string.Empty)
        {
            this._takeOverUserName = takeOverUserName;
            this._takeOverAll = takeOverAll;
        }

        private string _takeOverUserName;
        /// <summary>
        /// 用于接管删除用户时不能删除的内容(例如：用户创建的群组)
        /// </summary>
        public string TakeOverUserName
        {
            get { return _takeOverUserName; }
        }

        private bool _takeOverAll;
        /// <summary>
        /// 是否接管被删除用户的所有内容
        /// </summary>
        public bool TakeOverAll
        {
            get { return _takeOverAll; }
        }
    }
}
