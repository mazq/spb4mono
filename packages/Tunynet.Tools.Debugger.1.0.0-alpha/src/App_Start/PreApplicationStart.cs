//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Tunynet.Repositories;
using Tunynet.Tools.Debugger;

[assembly: PreApplicationStartMethod(typeof(PreApplicationStart), "Start")]
namespace Tunynet.Tools.Debugger
{
    public class PreApplicationStart
    {
        /// <summary>
        /// 在应用程序启动时注册Model
        /// </summary>
        public static void Start()
        {
            if (DebuggerConfiguration.Instance().EnableSqlTraceOrResponseTime())
            {
                DynamicModuleUtility.RegisterModule(typeof(SqlTraceHttpModule));
                PetaPocoDatabase.SqlTraceEnabled = DebuggerConfiguration.Instance().EnableSqlTrace();
            }
        }
    }
}
