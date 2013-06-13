using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;
using Tunynet.Common;
using Tunynet.Repositories;


namespace Tunynet.UI
{
    public class InitialNavigationRepository : Repository<InitialNavigation>, IInitialNavigationRepository
    {
         /// <summary>
        /// 获取常用操作
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<InitialNavigation> GetCommonOperations(long userId)
        {
            Sql sql = Sql.Builder;

            PetaPocoDatabase dao = CreateDAO();

            sql.Select("tn_InitialNavigations.NavigationId")
                .From("tn_InitialNavigations")
                .InnerJoin("tn_CommonOperations").On("tn_CommonOperations.NavigationId=tn_InitialNavigations.NavigationId")
                .Where("tn_InitialNavigations.PresentAreaKey=@0", PresentAreaKeysOfBuiltIn.ControlPanel)
                .Where("tn_CommonOperations.UserId=@0", userId);

            IEnumerable<object> ids = null;

            ids = dao.FetchFirstColumn(sql);

            return PopulateEntitiesByEntityIds(ids);

        }

        /// <summary>
        /// 功能搜索
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public IEnumerable<InitialNavigation> SearchOperations(string keyword)
        {
            PetaPocoDatabase dao = CreateDAO();

            Sql sql = Sql.Builder
                .Select("*")
                .From("tn_InitialNavigations")
                .Where("PresentAreaKey=@0", PresentAreaKeysOfBuiltIn.ControlPanel);

            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Where("NavigationText like @0", "%" + keyword + "%");
            }

            IEnumerable<object> commonOperations = null;

            commonOperations = dao.FetchFirstColumn(sql);

            return PopulateEntitiesByEntityIds(commonOperations);
        }
    }
}
