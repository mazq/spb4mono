//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Mvc;
using Tunynet.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于做防灌水检查
    /// </summary>
    public class PostIntervalAttribute : ActionFilterAttribute
    {
        private PostIntervalType _postIntervalType = PostIntervalType.LagerContent;
        /// <summary>
        /// 发布时间间隔类型
        /// </summary>
        public PostIntervalType PostIntervalType
        {
            get { return _postIntervalType; }
            set { _postIntervalType = value; }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //发帖间隔时间
            if (PostIntervalManager.CheckPostInterval(_postIntervalType))
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new StatusMessageData(StatusMessageType.Error, "你发的速度太快了"),
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
                return;
            }
        }
    }
}
