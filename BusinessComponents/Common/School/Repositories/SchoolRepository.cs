
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Common;
using Tunynet.Caching;
using System.Collections.Concurrent;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 地区的数据访问类
    /// </summary>
    public class SchoolRepository : Repository<School>, ISchoolRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 交换学校排列顺序
        /// </summary>
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            var school = Get(id);
            var referenceSchool = Get(referenceId);
            long displayOrder = referenceSchool.DisplayOrder;

            referenceSchool.DisplayOrder = school.DisplayOrder;
            school.DisplayOrder = displayOrder;

            base.Update(school);
            base.Update(referenceSchool);
        }

        /// <summary>
        /// 获取学校
        /// </summary>
        /// <param name="areaCode">地区编码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="schoolType">学校类型</param>
        /// <returns></returns>
        public PagingDataSet<School> Gets(string areaCode, string keyword, SchoolType? schoolType, int pageSize, int pageIndex)
        {
            if (string.IsNullOrEmpty(keyword))
                return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.Stable,
                    () =>
                    {
                        StringBuilder builder = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.GlobalVersion));
                        builder.Append("Schools");
                        builder.AppendFormat("::AreaCode-{0}", areaCode);
                        builder.AppendFormat(":Type-{0}", schoolType);
                        return builder.ToString();
                    }, () => { return GetSql_Gets(areaCode, keyword, schoolType); });
            else
                return GetPagingEntities(pageSize, pageIndex, GetSql_Gets(areaCode, keyword, schoolType));
        }

        /// <summary>
        /// 获取Sql
        /// </summary>
        /// <param name="areaCode">地区编码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="schoolType">学校类型</param>
        /// <returns></returns>
        public Sql GetSql_Gets(string areaCode, string keyword, SchoolType? schoolType)
        {
            Sql sql_Select = Sql.Builder
                .Select("Id")
                .From("tn_Schools");
            if (!string.IsNullOrEmpty(areaCode))
            {
                //特殊处理中国
                if (areaCode == "A1560000")
                {
                    sql_Select.Where("AreaCode like '1%' or AreaCode like '2%' or AreaCode like '3%' or AreaCode like '4%' or AreaCode like '5%' or AreaCode like '6%' or AreaCode like '7%' or AreaCode like '8%' or AreaCode like '9%' or AreaCode like 'A1560000'");
                }
                else
                {
                    //获取地区前缀
                    string schoolAreaCode = areaCode.TrimEnd('0');
                    if (schoolAreaCode.Length % 2 == 1)
                        schoolAreaCode += "0";
                    sql_Select.Where("AreaCode like @0", schoolAreaCode + "%");
                }
            }
            if (schoolType.HasValue)
                sql_Select.Where("SchoolType=@0", schoolType);

            if (!string.IsNullOrEmpty(keyword))
                sql_Select.Where("(Name like @0 or PinyinName like @0 or ShortPinyinName like @0)", "%" + keyword + "%");
            sql_Select.OrderBy("DisplayOrder");
            return sql_Select;
        }
    }
}