//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tunynet;

namespace Spacebuilder.Common
{
    public class PauseSiteSettingsEditModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PauseSiteSettingsEditModel()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PauseSiteSettingsEditModel(PauseSiteSettings pauseSiteSettings)
        {
            if (pauseSiteSettings != null)
            {
                IsEnable = pauseSiteSettings.IsEnable;
                PausePageType = pauseSiteSettings.PausePageType;
                PauseAnnouncement = pauseSiteSettings.PauseAnnouncement;
                PauseLink = pauseSiteSettings.PauseLink;
            }
        }

        #region 字段属性
        
        /// <summary>
        /// 站点当前状态
        /// </summary>
        [Display(Name="当前状态")]
        public bool IsEnable { get; set; }

        /// <summary>
        /// 是否允许登陆后台
        /// </summary>
        [Display(Name="允许登录后台")]
        public bool AllowLoginBackGround { get; set; }

        /// <summary>
        /// 暂停页面的类型
        /// </summary>
        [Display(Name="暂停页面类型")]
        public bool PausePageType { get; set; }

        /// <summary>
        /// 暂停公告
        /// </summary>
        [Display(Name="暂停公告")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string PauseAnnouncement { get; set; }

        /// <summary>
        /// 外链
        /// </summary>
        [Display(Name="外链")]
        [Required]
        public string PauseLink { get; set; }

        #endregion

        /// <summary>
        /// 将PauseSiteSettingsEditModel转换为PauseSiteSettings
        /// </summary>
        /// <returns></returns>
        public PauseSiteSettings AsPauseSiteSettings()
        {
            var pauseSiteSettings = DIContainer.Resolve<IPauseSiteSettingsManager>().get();
            pauseSiteSettings.IsEnable = IsEnable;
            pauseSiteSettings.PausePageType = PausePageType;
            pauseSiteSettings.PauseAnnouncement = PauseAnnouncement;
            pauseSiteSettings.PauseLink = PauseLink;
            return pauseSiteSettings;
        } 
    }
}
