using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;

namespace Tunynet.UI
{
    /// <summary>
    /// CommonOperationRepository仓储接口
    /// </summary>
    public interface ICommonOperationRepository : IRepository<CommonOperation>
    {
       /// <summary>
       /// 清除该用户的所有常用操作
       /// </summary>
       /// <param name="userId">用户Id</param>
        void ClearUserCommonOperations(long userId);

        /// <summary>
        /// 获取常用操作单个实体
        /// </summary>
        /// <param name="navigationId">导航Id</param>
        /// <param name="userId">用户Id</param>
        CommonOperation GetCommonOperation(int navigationId, long userId);

    }
}
