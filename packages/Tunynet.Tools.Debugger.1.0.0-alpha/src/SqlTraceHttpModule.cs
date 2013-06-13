//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Web;
using Tunynet.Repositories;
using System.Linq;

namespace Tunynet.Tools.Debugger
{
    public class SqlTraceHttpModule : IHttpModule
    {
        private static Stopwatch stopwatchOfResponseTime = new Stopwatch();

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(context_BeginRequest);
            context.EndRequest += OnEndRequest;
        }

        /// <summary>
        /// 请求开始时执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_BeginRequest(object sender, System.EventArgs e)
        {
            stopwatchOfResponseTime.Start();
        }

        /// <summary>
        /// 请求结束时执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnEndRequest(object sender, System.EventArgs e)
        {
            //by mazq,2012-9-11 过滤掉了异步请求
            if (HttpContext.Current.Request.AcceptTypes.Contains("text/html"))
            {
                if (HttpContext.Current.Request.Headers.Get("X-Requested-With") == null)
                {
                    var handler = new TraceHttpHandler();
                    handler.ProcessRequest(HttpContext.Current, PetaPocoDatabase.GetTracedSqls(), stopwatchOfResponseTime);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
