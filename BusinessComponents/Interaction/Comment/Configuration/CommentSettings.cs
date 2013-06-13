
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Imaging;
using Tunynet.Caching;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 全局设置类
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class CommentSettings : IEntity
    {
        private bool _enableComment = true;

        /// <summary>
        /// 是否启用评论
        /// </summary>
        public bool EnableComment
        {
            get { return _enableComment; }
            set { _enableComment = value; }
        }


        public bool _showCommentCount = false;

        /// <summary>
        /// 是否显示评论数
        /// </summary>
        public bool ShowCommentCount
        {
            get { return _showCommentCount; }
            set { _showCommentCount = value; }
        }

        private bool _enableSupportOppose = false;

        /// <summary>
        /// 是否启用顶踩
        /// </summary>
        public bool EnableSupportOppose
        {
            get { return _enableSupportOppose; }
            set { _enableSupportOppose = value; }
        }

        private bool _showLowCommentOnLoad = true;

        /// <summary>
        /// 二级回复是否在页面一加载的时候显示
        /// </summary>
        public bool ShowLowCommentOnLoad
        {
            get { return _showLowCommentOnLoad; }
            set { _showLowCommentOnLoad = value; }
        }

        private int _maxCommentLength = 140;

        /// <summary>
        /// 评论录入的最大字数
        /// </summary>
        public int MaxCommentLength
        {
            get { return _maxCommentLength; }
            set { _maxCommentLength = value; }
        }

        private bool _enablePrivate = false;

        /// <summary>
        /// 是否启用悄悄话
        /// </summary>
        public bool EnablePrivate
        {
            get { return _enablePrivate; }
            set { _enablePrivate = value; }
        }

        private bool _allowAnonymousComment = false;

        /// <summary>
        /// 是否允许匿名用户评论
        /// </summary>
        public bool AllowAnonymousComment
        {
            get { return _allowAnonymousComment; }
            set { _allowAnonymousComment = value; }
        }

        private bool _entryBoxAutoHeight = true;

        /// <summary>
        /// 录入框是否自适应高度
        /// </summary>
        public bool EntryBoxAutoHeight
        {
            get { return _entryBoxAutoHeight; }
            set { _entryBoxAutoHeight = value; }
        }

        private string _commentClass = string.Empty;

        /// <summary>
        /// 评论的样式
        /// </summary>
        public string CommentClass
        {
            get { return _commentClass; }
            set { _commentClass = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(CommentSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}