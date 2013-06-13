//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Configuration;
using System.Web.Mvc;
using Spacebuilder.Blog.Controllers;
using Tunynet.Common;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 日志路由设置
    /// </summary>
    public class UrlRoutingRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Blog"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;

            #region UserSpace

            //我的日志/他的日志
            context.MapRoute(
                "UserSpace_Blog_Blog", // Route name
                "u/{SpaceKey}/Blog" + extensionForOldIIS, // URL with parameters
                new { controller = "Blog", action = "Blog", CurrentNavigationId = "11100203" } // Parameter defaults
            );

            //我的日志/他的日志
            context.MapRoute(
                "ApplicationCount_Blog", // Route name
                "u/{SpaceKey}/Blog" + extensionForOldIIS, // URL with parameters
                new { controller = "Blog", action = "Blog", CurrentNavigationId = "11100203" } // Parameter defaults
            );

            //日志详细页
            context.MapRoute(
              "UserSpace_Blog_Detail", // Route name
              "u/{SpaceKey}/Blog/{threadId}" + extensionForOldIIS, // URL with parameters
              new { controller = "Blog", action = "Detail" }, // Parameter defaults
              new { threadId = @"(\d+)|(\{\d+\})" }
            );

            //创建日志
            context.MapRoute(
              "UserSpace_Blog_Create", // Route name
              "u/{SpaceKey}/Blog/Create" + extensionForOldIIS, // URL with parameters
              new { controller = "Blog", action = "Edit" } // Parameter defaults
            );

            //编辑日志
            context.MapRoute(
              "UserSpace_Blog_Edit", // Route name
              "u/{SpaceKey}/Blog/Edit-{threadId}" + extensionForOldIIS, // URL with parameters
              new { controller = "Blog", action = "Edit" }, // Parameter defaults
              new { threadId = @"(\d+)|(\{\d+\})" }
            );

            //日志列表页-存档
            context.MapRoute(
              "UserSpace_Blog_List_Archive", // Route name
              "u/{SpaceKey}/Blog/{listType}/{year}-{month}" + extensionForOldIIS, // URL with parameters
              new { controller = "Blog", action = "List" }, // Parameter defaults
              new { year = @"(\d+)|(\{\d+\})", month = @"(\d+)|(\{\d+\})" }
            );

            //日志列表页-分类
            context.MapRoute(
              "UserSpace_Blog_List_Category", // Route name
              "u/{SpaceKey}/Blog/{listType}/{categoryId}" + extensionForOldIIS, // URL with parameters
              new { controller = "Blog", action = "List" }, // Parameter defaults
              new { categoryId = @"(\d+)|(\{\d+\})" }
            );

            //日志列表页-标签
            context.MapRoute(
              "UserSpace_Blog_List_Tag", // Route name
              "u/{SpaceKey}/Blog/{listType}/{tag}" + extensionForOldIIS, // URL with parameters
              new { controller = "Blog", action = "List" }, // Parameter defaults
              new { tag = @"(\S+)|(\{\S+\})" }
            );

            //日志首页
            context.MapRoute(
                "UserSpace_Blog_Home", // Route name
                "u/{SpaceKey}/Blog/Home" + extensionForOldIIS, // URL with parameters
                new { controller = "Blog", action = "Home", CurrentNavigationId = "11100202" } // Parameter defaults
                );


            //日志首页
            context.MapRoute(
              "UserSpace_Blog_Subscribed", // Route name
              "u/{SpaceKey}/Blog/Subscribed" + extensionForOldIIS, // URL with parameters
               new { controller = "Blog", action = "Subscribed", CurrentNavigationId = "11100204" } // Parameter defaults
            );


            context.MapRoute(
                "UserSpace_Blog_Common", // Route name
                "u/{SpaceKey}/Blog/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "Blog", action = "Home" } // Parameter defaults
            );

            #endregion UserSpace

            #region 动态列表控件路由

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateBlogThread", TenantTypeIds.Instance().BlogThread()), // Route name
                "BlogActivity/CreateThread/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "BlogActivity", action = "_CreateBlogThread" } // Parameter defaults
            );

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateBlogComment", TenantTypeIds.Instance().Comment()), // Route name
                "BlogActivity/CreateComment/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "BlogActivity", action = "_CreateBlogComment" } // Parameter defaults
            );

            #endregion

            #region Channel

            //站点分类
            context.MapRoute(
                "Channel_Blog_SiteCategory", // Route name
                "Blog/Category/{categoryId}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelBlog", action = "ListByCategory" }, // Parameter defaults
                new { categoryId = @"(\d+)|(\{\d+\})" }
            );

            //标签
            context.MapRoute(
                "Channel_Blog_Tag", // Route name
                "Blog/Tag/{tagName}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelBlog", action = "ListByTag", CurrentNavigationId = "10100201" }, // Parameter defaults
                new { tagName = @"(\S+)|(\{\S+\})" }
            );

            //日志频道首页
            context.MapRoute(
              "Channel_Blog_Home", // Route name
              "Blog" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelBlog", action = "Home", CurrentNavigationId = "10100202" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Blog_Common", // Route name
                "Blog/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelBlog", action = "Home", CurrentNavigationId = "10100201" } // Parameter defaults
            );

            #endregion

            #region ControlPanel
            context.MapRoute(
                "ControlPanel_Blog_Home", // Route name
                "ControlPanel/Content/Blog/ManageBlogs" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelBlog", action = "ManageBlogs", CurrentNavigationId = "20100201" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Blog_Common", // Route name
                "ControlPanel/Content/Blog/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelBlog", CurrentNavigationId = "20000010" } // Parameter defaults
            );

            #endregion

        }
    }
}