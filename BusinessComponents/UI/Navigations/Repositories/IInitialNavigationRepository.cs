using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;

namespace Tunynet.UI
{
    /// <summary>
    /// InitialNavigationRepository仓储接口
    /// </summary>
    public interface IInitialNavigationRepository : IRepository<InitialNavigation>
    {
        /// <summary>
        /// 获取常用操作
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<InitialNavigation> GetCommonOperations(long userId);

        /// <summary>
        /// 功能搜索
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        IEnumerable<InitialNavigation> SearchOperations(string keyword);

    }
}
