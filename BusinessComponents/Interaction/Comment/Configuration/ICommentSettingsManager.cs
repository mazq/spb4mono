
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common.Configuration
{

    /// <summary>
    /// LogoSettings管理器接口
    /// </summary>
    public interface ICommentSettingsManager
    {
        /// <summary>
        /// 获取CommentSettings
        /// </summary>
        /// <returns></returns>
        CommentSettings Get();

        /// <summary>
        /// 保存CommentSettings
        /// </summary>
        /// <param name="settings"></param>
        void Save(CommentSettings settings);
    }
}
