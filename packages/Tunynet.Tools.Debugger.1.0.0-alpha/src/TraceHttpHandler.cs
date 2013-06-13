//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using log4net;

namespace Tunynet.Tools.Debugger
{
    class TraceHttpHandler : IHttpHandler
    {

        VirtualPathProvider _virtualPathProvider;

        public TraceHttpHandler() : this(null) { }

        public TraceHttpHandler(VirtualPathProvider virtualPathProvider)
        {
            _virtualPathProvider = virtualPathProvider ?? HostingEnvironment.VirtualPathProvider;
        }

        public void ProcessRequest(HttpContext context)
        { }

        public void ProcessRequest(HttpContext context, List<SqlTraceEntity> sqlTraces, Stopwatch sw)
        {
            var request = context.Request;
            if (!IsRoutedRequest(request))
            {
                return;
            }
            sw.Stop();

            string generatedUrlInfo = string.Empty;
            var requestContext = request.RequestContext;

            string htmlFormat = @"<html>
                                    <div id=""haackroutedebugger"" style=""background-color: #fff;"">
                                        <style>
                                            #haackroutedebugger, #haackroutedebugger td, #haackroutedebugger th {{background-color: #fff; font-family: verdana, helvetica, san-serif; font-size: small;}}
                                            #haackroutedebugger tr.header td, #haackroutedebugger tr.header th {{background-color: #ffc;}}
                                        </style>
                                        <hr style=""width: 100%; border: solid 1px #000; margin:0; padding:0;"" />
                                        <h1 style=""margin: 0; padding: 4px; border-bottom: solid 1px #bbb; padding-left: 10px; font-size: 1.2em; background-color: #ffc;"">Sql 跟踪</h1>
                                        <div id=""main"" style=""margin-top:0; padding-top:0"">
                                            <p style=""font-size: .9em; padding-top:0"">
                                            </p>
                                            <p style=""font-size: .9em;"">
                                                 <code></code>
                                            </p>
                                            {2}
                                            {1}
                                            <br/>
                                            <hr style=""clear: both;"" />
                                            <table border=""1"" cellpadding=""3"" cellspacing=""0"">
                                                {0}
                                            </table>
                                        </div>
                                    </div>";

            string sqls = string.Empty;
            if (sqlTraces != null)
            {
                if (DebuggerConfiguration.Instance().SqlTraceToPage())
                {
                    string[] sqlTypes = new string[] { "select", "insert", "update", "delete" };
                    foreach (var type in sqlTypes.ToList())
                    {
                        sqls += OutSql(sqlTraces, type);
                    }
                }

                if (DebuggerConfiguration.Instance().SqlTraceToLog())
                {
                    ILog log4net = LogManager.GetLogger("ExceptionLogger");
                    foreach (SqlTraceEntity sqlTrace in sqlTraces.ToList())
                    {
                        log4net.Debug(sqlTrace.Sql + "\t\t执行时间" + sqlTrace.ElapsedMilliseconds + "ms");
                    }
                }

            }

            string responseTime = string.Empty;
            if (DebuggerConfiguration.Instance().EnableResponseTime())
            {
                responseTime = @"<div style=""float: left; margin-left: 10px;"">
                                    <table border=""1"" width=""300"">
                                        <caption style=""font-weight: bold;"">页面响应时间</caption>
                                        <tr class=""header""><td>" + sw.ElapsedMilliseconds + @"ms</td></tr>
                                    </table>
                                </div>";
            }

            sw.Reset();
            sqlTraces.Clear();
            if (DebuggerConfiguration.Instance().SqlTraceToPage())
            {
                context.Response.Write(string.Format(htmlFormat, sqls, responseTime, generatedUrlInfo));
            }
        }

        /// <summary>
        /// 对sql语句进行分类统计
        /// </summary>
        private string OutSql(List<SqlTraceEntity> sqlTraces, string sqlType)
        {
            string sqls = string.Empty;
            List<SqlTraceEntity> subSqls = sqlTraces.Where(n => n.Sql.StartsWith(sqlType, StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (subSqls.Count > 0)
            {
                sqls += @"<tr class=""header"">
                              <th>" + sqlType + "语句:" + subSqls.Count + @"条 总执行时间：" + sqlTraces.Sum(n => n.ElapsedMilliseconds) + @"ms</th><th style=""width:200px;"">执行时间</th>
                          </tr>";
                foreach (SqlTraceEntity sqlTrace in subSqls.ToList())
                {
                    sqls += string.Format(@"<tr><td>{0}</td><td>{1} ms</td></tr>", sqlTrace.Sql, sqlTrace.ElapsedMilliseconds);
                }
            }

            return sqls;
        }

        /// <summary>
        /// 判断请求是否为动态页面
        /// </summary>
        private bool IsRoutedRequest(HttpRequest request)
        {
            string path = request.AppRelativeCurrentExecutionFilePath;
            if (path != "~/" && (_virtualPathProvider.FileExists(path) || _virtualPathProvider.DirectoryExists(path)))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 接口成员
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

    }
}
