//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Linq;

namespace Tunynet.Tools.Debugger
{
    public class DebuggerConfiguration
    {
        private static string sqlTraceSetting = string.Empty;
        private static string responseTimeSetting = string.Empty;
        private static DebuggerConfiguration debuggerConfiguration;

        DebuggerConfiguration()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("TunynetDebug:SqlTrace"))
                sqlTraceSetting = ConfigurationManager.AppSettings["TunynetDebug:SqlTrace"].ToString();

            if (ConfigurationManager.AppSettings.AllKeys.Contains("TunynetDebug:ResponseTime"))
                responseTimeSetting = ConfigurationManager.AppSettings["TunynetDebug:ResponseTime"].ToString();
        }
        /// <summary>
        /// 实例化Debugger配置
        /// </summary>
        public static DebuggerConfiguration Instance()
        {
            if (debuggerConfiguration == null)
                debuggerConfiguration = new DebuggerConfiguration();

            return debuggerConfiguration;
        }

        /// <summary>
        /// 是否启用了Sql跟踪或运行时间显示
        /// </summary>
        /// <returns></returns>
        public bool EnableSqlTraceOrResponseTime()
        {
            return EnableSqlTrace() || EnableResponseTime();
        }

        /// <summary>
        /// 判断Sql跟踪是否启用
        /// </summary>
        /// <returns></returns>
        public bool EnableSqlTrace()
        {
            if (SqlTraceToLog() || SqlTraceToPage())
                return true;

            return false;
        }

        /// <summary>
        /// 判断页面运行时间是否启用
        /// </summary>
        /// <returns></returns>
        public bool EnableResponseTime()
        {
            if (string.IsNullOrEmpty(responseTimeSetting))
                return false;

            if (responseTimeSetting.Equals(ResponseTimeSetting.Page, StringComparison.InvariantCultureIgnoreCase) ||
                    responseTimeSetting.Equals(ResponseTimeSetting.Database, StringComparison.InvariantCultureIgnoreCase) ||
                    responseTimeSetting.Equals(ResponseTimeSetting.PageAndDatabase, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }

        /// <summary>
        /// Sql跟踪结果是否从日志输出
        /// </summary>
        public bool SqlTraceToLog()
        {
            if (string.IsNullOrEmpty(sqlTraceSetting))
                return false;

            if (sqlTraceSetting.Equals(SqlTraceSettings.Log, StringComparison.InvariantCultureIgnoreCase) ||
                sqlTraceSetting.Equals(SqlTraceSettings.PageAndLog, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }

        /// <summary>
        /// Sql跟踪结果是否从页面输出
        /// </summary>
        public bool SqlTraceToPage()
        {
            if (string.IsNullOrEmpty(sqlTraceSetting))
                return false;

            if (sqlTraceSetting.Equals(SqlTraceSettings.Page, StringComparison.InvariantCultureIgnoreCase) ||
                sqlTraceSetting.Equals(SqlTraceSettings.PageAndLog, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }



        /// <summary>
        /// SqlTrace 设置
        /// </summary>
        class SqlTraceSettings
        {
            /// <summary>
            /// SQL跟踪输出到页面
            /// </summary>
            internal static string Page = "Page";

            /// <summary>
            /// SQL跟踪输出到日志文件
            /// </summary>
            internal static string Log = "Log";

            /// <summary>
            /// SQL跟踪输出到页面和日志文件
            /// </summary>
            internal static string PageAndLog = "PageAndLog";

            /// <summary>
            /// 禁用SQL跟踪
            /// </summary>
            internal static string Disabled = "Disabled";
        }

        /// <summary>
        /// 页面执行时间设置
        /// </summary>
        class ResponseTimeSetting
        {
            /// <summary>
            /// 页面执行时间输出到页面
            /// </summary>
            internal static string Page = "Page";

            /// <summary>
            /// 页面执行时间输出到数据库
            /// </summary>
            internal static string Database = "Database";

            /// <summary>
            /// 页面执行时间输出到页面和数据库
            /// </summary>
            internal static string PageAndDatabase = "PageAndDatabase";

            /// <summary>
            /// 禁用页面执行时间输出
            /// </summary>
            internal static string Disabled = "Disabled";
        }
    }

}
