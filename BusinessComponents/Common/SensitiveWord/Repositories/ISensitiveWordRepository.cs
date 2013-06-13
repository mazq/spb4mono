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
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 敏感词数据访问
    /// </summary>
    public interface ISensitiveWordRepository : IRepository<SensitiveWord>
    {
        /// <summary>
        /// 获取敏感词(管理员后台用)
        /// </summary>
        /// <param name="keyword">带过滤文字关键字</param>
        /// <param name="typeId">敏感词类型</param>
        /// <returns>带过滤文字集合</returns>
        IEnumerable<SensitiveWord> GetSensitiveWords(string keyword, int? typeId);

        /// <summary>
        /// 批量添加敏感词
        /// </summary>
        /// <param name="sensitiveWords">敏感词集合</param>
        void BatchInsert(List<SensitiveWord> sensitiveWords);

        /// <summary>
        /// 添加敏感词
        /// </summary>
        /// <param name="sensitiveWord">敏感词名</param>
        int Create(SensitiveWord sensitiveWord);

        /// <summary>
        /// 更新敏感词
        /// </summary>
        /// <param name="sensitiveWord"></param>
        /// <returns></returns>
        int Update(SensitiveWord sensitiveWord);
        
    }
}
