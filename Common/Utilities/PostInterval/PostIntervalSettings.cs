//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 发帖时间间隔配置类
    /// </summary>
    public class PostIntervalSettings
    {
        private static PostIntervalSettings postIntervalSettings;

        #region 配置属性

        //允许连续发布内容次数
        public Dictionary<PostIntervalType, int> PostCounts = new Dictionary<PostIntervalType, int>();

        //发帖时间间隔
        public Dictionary<PostIntervalType, int> PostIntervals = new Dictionary<PostIntervalType, int>();

        private static bool enablePostInterval = false;
        /// <summary>
        /// 是否启用发帖时间间隔
        /// </summary>
        public bool EnablePostInterval
        {
            get { return enablePostInterval; }
        }

        #endregion

        PostIntervalSettings()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("PostInterval:EnablePostInterval"))
                enablePostInterval = Convert.ToBoolean(ConfigurationManager.AppSettings["PostInterval:EnablePostInterval"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("PostInterval:PostIntervalLargeContent"))
                PostIntervals[PostIntervalType.LagerContent] = Convert.ToInt32(ConfigurationManager.AppSettings["PostInterval:PostIntervalLargeContent"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("PostInterval:PostIntervalMicroContent"))
                PostIntervals[PostIntervalType.MicroContent] = Convert.ToInt32(ConfigurationManager.AppSettings["PostInterval:PostIntervalMicroContent"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("PostInterval:PostIntervalComment"))
                PostIntervals[PostIntervalType.Comment] = Convert.ToInt32(ConfigurationManager.AppSettings["PostInterval:PostIntervalComment"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("PostInterval:LargeContentCount"))
                PostCounts[PostIntervalType.LagerContent] = Convert.ToInt32(ConfigurationManager.AppSettings["PostInterval:LargeContentCount"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("PostInterval:MicroContentCount"))
                PostCounts[PostIntervalType.MicroContent] = Convert.ToInt32(ConfigurationManager.AppSettings["PostInterval:MicroContentCount"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("PostInterval:CommentCount"))
                PostCounts[PostIntervalType.Comment] = Convert.ToInt32(ConfigurationManager.AppSettings["PostInterval:CommentCount"].ToString());
        }

        /// <summary>
        /// 实例化PostInterval配置
        /// </summary>
        public static PostIntervalSettings Instance()
        {
            if (postIntervalSettings == null)
                postIntervalSettings = new PostIntervalSettings();

            return postIntervalSettings;
        }
    }
}
