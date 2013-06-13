//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-03-20</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-09-06" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Xml.Linq;
using System.Drawing;
using Tunynet.Imaging;
using Tunynet.Utilities;
using System.IO;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 租户标识图配置类
    /// </summary>
    public class TenantCommentSettings
    {

        private static ConcurrentDictionary<string, TenantCommentSettings> registeredTenantCommentSettings = new ConcurrentDictionary<string, TenantCommentSettings>();

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="xElement">标识图配置节点</param>
        private TenantCommentSettings(XElement xElement)
        {
            if (xElement != null)
            {
                var att = xElement.Attribute("EnableComment");
                if (att != null)
                {
                    bool temp = true;
                    if (bool.TryParse(att.Value, out temp))
                        this._enableComment = temp;
                }

                att = xElement.Attribute("ShowCommentCount");
                if (att != null)
                {
                    bool temp = true;
                    if (bool.TryParse(att.Value, out temp))
                        _showCommentCount = temp;
                }

                att = xElement.Attribute("EnableSupportOppose");
                if (att != null)
                {
                    bool temp = false;
                    if (bool.TryParse(att.Value, out temp))
                        _enableSupportOppose = temp;
                }

                att = xElement.Attribute("ShowLowCommentOnLoad");
                if (att != null)
                {
                    bool temp = false;
                    if (bool.TryParse(att.Value, out temp))
                        _showLowCommentOnLoad = temp;
                }

                att = xElement.Attribute("MaxCommentLength");
                if (att != null)
                    int.TryParse(att.Value, out _maxCommentLength);

                att = xElement.Attribute("EnablePrivate");
                if (att != null)
                {
                    bool temp = false;
                    if (bool.TryParse(att.Value, out temp))
                        _enablePrivate = temp;
                }

                att = xElement.Attribute("AllowAnonymousComment");
                if (att != null)
                {
                    bool temp = false;
                    if (bool.TryParse(att.Value, out temp))
                        _allowAnonymousComment = temp;
                }

                att = xElement.Attribute("EntryBoxAutoHeight");
                if (att != null)
                {
                    bool temp = false;
                    if (bool.TryParse(att.Value, out temp))
                        _entryBoxAutoHeight = temp;
                }

                att = xElement.Attribute("CommentClass");
                if (att != null)
                    _commentClass = att.Value;

                att = xElement.Attribute("tenantTypeId");
                if (att != null)
                    _tenantTypeId = att.Value;

            }
        }

        private TenantCommentSettings() { }

        /// <summary>
        /// 获取注册的TenantCommentSettings
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public static TenantCommentSettings GetRegisteredSettings(string tenantTypeId)
        {
            TenantCommentSettings TenantCommentSettings;
            if (registeredTenantCommentSettings.TryGetValue(tenantTypeId, out TenantCommentSettings))
                return TenantCommentSettings;
            return new TenantCommentSettings();
        }

        /// <summary>
        /// 获取所有注册的TenantCommentSettings
        /// </summary>
        /// <returns>TenantCommentSettings集合</returns>
        public static IEnumerable<TenantCommentSettings> GetAll()
        {
            return registeredTenantCommentSettings.Values;
        }

        /// <summary>
        /// 注册TenantCommentSettings
        /// </summary>
        /// <remarks>若xElement下有多个add节点，会同时注册多个TenantCommentSettings</remarks>
        /// <param name="xElement">标识图配置节点，会据此寻找其下所有子节点add</param>
        public static void RegisterSettings(XElement xElement)
        {
            IEnumerable<TenantCommentSettings> settings = xElement.Elements("add").Select(n => new TenantCommentSettings(n));
            foreach (var item in settings)
            {
                registeredTenantCommentSettings[item.TenantTypeId] = item;
            }
        }

        #region 属性
        private string _tenantTypeId = string.Empty;
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return _tenantTypeId; }
        }

        private bool? _enableComment;

        /// <summary>
        /// 是否显示评论
        /// </summary>
        public bool EnableComment
        {
            get
            {
                if (_enableComment.HasValue)
                    return _enableComment.Value;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.EnableComment;
            }
        }

        bool? _showCommentCount = null;

        /// <summary>
        /// 是否显示评论数
        /// </summary>
        public bool ShowCommentCount
        {
            get
            {
                if (_showCommentCount.HasValue)
                    return _showCommentCount.Value;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.ShowCommentCount;
            }
        }

        bool? _enableSupportOppose = null;

        /// <summary>
        /// 是否启用顶踩
        /// </summary>
        public bool EnableSupportOppose
        {
            get
            {
                if (_enableSupportOppose.HasValue)
                    return _enableSupportOppose.Value;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.EnableSupportOppose;
            }
        }

        private bool? _showLowCommentOnLoad = null;

        /// <summary>
        /// 是否在页面加载的时候显示二级评论
        /// </summary>
        public bool ShowLowCommentOnLoad
        {
            get
            {
                if (_showLowCommentOnLoad.HasValue)
                    return _showLowCommentOnLoad.Value;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.ShowLowCommentOnLoad;
            }
        }

        private int _maxCommentLength = 0;

        /// <summary>
        /// 评论的最大字数
        /// </summary>
        public int MaxCommentLength
        {
            get
            {
                if (_maxCommentLength > 0)
                    return _maxCommentLength;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.MaxCommentLength;
            }

        }

        private bool? _enablePrivate = null;

        /// <summary>
        /// 是否在页面加载的时候显示二级评论
        /// </summary>
        public bool EnablePrivate
        {
            get
            {
                if (_enablePrivate.HasValue)
                    return _enablePrivate.Value;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.EnablePrivate;
            }
        }

        private bool? _allowAnonymousComment = null;

        /// <summary>
        /// 是否允许匿名用户评论
        /// </summary>
        public bool AllowAnonymousComment
        {
            get
            {
                if (_allowAnonymousComment.HasValue)
                    return _allowAnonymousComment.Value;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.AllowAnonymousComment;
            }
        }

        private bool? _entryBoxAutoHeight = null;

        /// <summary>
        /// 是否允许匿名用户评论
        /// </summary>
        public bool EntryBoxAutoHeight
        {
            get
            {
                if (_entryBoxAutoHeight.HasValue)
                    return _entryBoxAutoHeight.Value;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.EntryBoxAutoHeight;
            }
        }

        private string _commentClass = null;

        public string CommentClass
        {
            get
            {
                if (_commentClass != null)
                    return _commentClass;

                CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();
                return commentSettings.CommentClass;
            }
        }


        #endregion
    }
}
