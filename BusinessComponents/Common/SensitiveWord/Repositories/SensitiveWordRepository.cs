//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Linq;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 敏感词的数据访问类
    /// </summary>
    public class SensitiveWordRepository : Repository<SensitiveWord>, ISensitiveWordRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 添加敏感词
        /// </summary>
        /// <param name="sensitiveWord"></param>
        public int Create(SensitiveWord sensitiveWord)
        {
            IEnumerable<string> existWords = CreateDAO().Fetch<string>(Sql.Builder.Append("select Word from tn_SensitiveWords where Word in (@0)", sensitiveWord.Word));
            if (existWords.Count() > 0)
                return -1;
            Insert(sensitiveWord);
            return 1;
        }

        /// <summary>
        /// 更新敏感词
        /// </summary>
        /// <param name="sensitiveWord"></param>
        /// <returns></returns>
        public int Update(SensitiveWord sensitiveWord)
        {
            int id = CreateDAO().ExecuteScalar<int>(Sql.Builder.Append("select Id from tn_SensitiveWords where Word = @0", sensitiveWord.Word));

            RealTimeCacheHelper.IncreaseEntityCacheVersion(sensitiveWord.Id);
            cacheService.Remove(RealTimeCacheHelper.GetCacheKeyOfEntity(sensitiveWord.Id));

            if (id > 0 && id != sensitiveWord.Id)
                return -1;
            base.Update(sensitiveWord);
            return sensitiveWord.Id;
        }

        /// <summary>
        /// 获取敏感词(管理员后台用)
        /// </summary>
        /// <param name="keyword">带过滤文字关键字</param>
        /// <param name="typeId">敏感词类型</param>
        /// <returns>待过滤文字集合</returns>
        public IEnumerable<SensitiveWord> GetSensitiveWords(string keyword, int? typeId)
        {
            PetaPocoDatabase dao = CreateDAO();

            Sql sql = Sql.Builder;
            sql.Select("*")
               .From("tn_SensitiveWords");

            if (typeId.HasValue && typeId.Value > 0)
                sql.Where("TypeId = @0", typeId);

            IEnumerable<object> ids = null;
            if (string.IsNullOrEmpty(keyword))
            {
                //获取缓存Key
                StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.GlobalVersion));
                cacheKey.AppendFormat("SensitiveWords:TypeId-{0}", typeId.ToString());

                ids = cacheService.Get<IEnumerable<object>>(cacheKey.ToString());
                if (ids == null)
                {
                    ids = dao.FetchFirstColumn(sql);
                    cacheService.Add(cacheKey.ToString(), ids, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                sql.Where("Word like @0", "%" + keyword + "%");
                ids = dao.FetchFirstColumn(sql);
            }

            return PopulateEntitiesByEntityIds(ids);
        }

        /// <summary>
        /// 批量添加敏感词
        /// </summary>
        /// <param name="sensitiveWords">敏感词集合</param>
        public void BatchInsert(List<SensitiveWord> sensitiveWords)
        {
            CreateDAO().OpenSharedConnection();

            IList<Sql> sqls = new List<Sql>();

            IEnumerable<string> existWords = CreateDAO().Fetch<string>(Sql.Builder.Append("select Word from tn_SensitiveWords where Word in (@0)", sensitiveWords.Select(n => n.Word)));

            sensitiveWords = sensitiveWords.Where(n => !existWords.Contains(n.Word)).ToList();
            foreach (var sensitiveWord in sensitiveWords)
            {
                sqls.Add(Sql.Builder.Append("insert into tn_SensitiveWords(Word,Replacement,TypeId) values (@0,@1,@2)", sensitiveWord.Word, sensitiveWord.Replacement, sensitiveWord.TypeId));
            }

            CreateDAO().Execute(sqls);
            RealTimeCacheHelper.IncreaseGlobalVersion();
            CreateDAO().CloseSharedConnection();
        }
    }

}
