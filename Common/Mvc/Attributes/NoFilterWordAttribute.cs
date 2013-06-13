//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using Tunynet.Common;
using Tunynet.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 表示不过滤敏感词
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class NoFilterWordAttribute : Attribute
    {

    }
}