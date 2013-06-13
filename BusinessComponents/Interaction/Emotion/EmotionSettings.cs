//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 站点Email设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class EmotionSettings:IEntity
    {
        private string emoticonPath = "~/Images/Emotions";
        /// <summary>
        /// 表情图片的虚拟路径，例如：
        /// ~/Emotions
        /// </summary>
        public string EmoticonPath
        {
            get { return emoticonPath; }
            set { emoticonPath = value; }
        }

        private bool _enableDirectlyUrl = false;
        /// <summary>
        /// 是否启用直连Url
        /// </summary>
        public bool EnableDirectlyUrl
        {
            get { return _enableDirectlyUrl; }
            set { _enableDirectlyUrl = value; }
        }

        private string _directlyRootUrl = string.Empty;
        /// <summary>
        /// 直连Url
        /// </summary>
        public string DirectlyRootUrl
        {
            get { return _directlyRootUrl; }
            set { _directlyRootUrl = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(EmotionSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
        
    }
}
