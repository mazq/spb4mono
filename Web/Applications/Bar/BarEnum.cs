//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spacebuilder.Bar
{
    public enum BarManageableCountType
    {

        //待审核的贴吧
        PendingSection,

        //全部
        All = 3,

        //待审核
        Pending = 20,

        //在审核
        Again = 30,

        //最近24小时
        Last24H

    }
}