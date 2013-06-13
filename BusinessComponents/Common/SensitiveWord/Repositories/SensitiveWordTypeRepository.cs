//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 敏感词的数据访问类
    /// </summary>
    public class SensitiveWordTypeRepository : Repository<SensitiveWordType>
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 删除敏感词类型
        /// </summary>
        /// <param name="entity">敏感词类型实体</param>
        /// <returns></returns>
        public override int Delete(SensitiveWordType entity)
        {
            int affectCount = base.Delete(entity);

            if (affectCount > 0)
            {
                Sql sql = Sql.Builder;
                sql.Append("update tn_SensitiveWords set TypeId = 0 where TypeId = @0", entity.TypeId);
                CreateDAO().Execute(sql);

                EntityData.ForType(typeof(SensitiveWord)).RealTimeCacheHelper.IncreaseGlobalVersion();
            }

            OnDeleted(entity);
            return affectCount;
        }
    }
}
