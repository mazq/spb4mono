//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 积分类型Repository
    /// </summary>
    public class PointCategoryRepository : Repository<PointCategory>
    {
    
        /// <summary>
        /// 更新积分类型
        /// </summary>
        /// <param name="entity">待更新的积分类型</param>
        public override void Update(PointCategory entity)
        {
            //注意：CategoryKey、Description、DisplayOrder不允许修改   
            //清除缓存
            var sql = Sql.Builder;
            sql.Append("Update tn_PointCategories set CategoryName=@0, Unit = @1, QuotaPerDay = @2 where CategoryKey = @3", entity.CategoryName, entity.Unit, entity.QuotaPerDay, entity.CategoryKey);
            CreateDAO().Execute(sql);
            //done:zhangp,by zhengw:不要从缓存中再获取一遍,这样又把旧数据给取出来了，把这句代码删除，相同问题自行检查
            

            base.OnUpdated(entity);
        }        
    }
}