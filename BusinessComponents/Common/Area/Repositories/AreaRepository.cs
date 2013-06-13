//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

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
    public class AreaRepository : IAreaRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 缓存设置
        /// </summary>
        protected static RealTimeCacheHelper RealTimeCacheHelper { get { return EntityData.ForType(typeof(Area)).RealTimeCacheHelper; } }


        /// <summary>
        /// 默认PetaPocoDatabase实例
        /// </summary>
        protected PetaPocoDatabase CreateDAO()
        {
            return PetaPocoDatabase.CreateInstance();
        }



        #region Insert,Delete,Update

        /// <summary>
        /// 插入地区数据
        /// </summary>
        /// <param name="area"></param>
        public void Insert(Area area)
        {

            Database database = CreateDAO();
            database.OpenSharedConnection();
            if (string.IsNullOrEmpty(area.ParentCode))
            {
                area.Depth = 0;
                area.ChildCount = 0;
            }
            else
            {
                Area areaParent = Get(area.ParentCode);
                if (areaParent == null)
                    return;
                area.Depth = areaParent.Depth + 1;
                area.ChildCount = 0;
            }
            object areaCode = database.Insert(area);
            if (areaCode != null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Append("update tn_Areas set ChildCount=ChildCount+1 where AreaCode=@0", area.ParentCode);
                database.Execute(sql);
            }

            var sql_SetDisplayOrder = Sql.Builder
                .Append("update tn_Areas set DisplayOrder= (select MAX(DisplayOrder) from tn_Areas)+1 where AreaCode = @0", area.AreaCode);
            database.Execute(sql_SetDisplayOrder);

            database.CloseSharedConnection();
            //清空缓存
            ClearChache();
        }

        /// <summary>
        /// 更新子节点信息
        /// </summary>
        /// <param name="area">要更新的地区实体</param>
        /// <returns>更新之后的实体</returns>
        public void Update(Area area)
        {
            Database database = CreateDAO();
            int newParentDepth = 0;
            //过滤错误：判定，如果不为空但是取不到就是错的
            if (!string.IsNullOrEmpty(area.ParentCode))
            {
                Area newParentArea = Get(area.ParentCode);
                if (newParentArea != null)
                    newParentDepth = newParentArea.Depth;
                else
                    return;
            }

            var sql_selete = PetaPoco.Sql.Builder;
            sql_selete.Select("*").From("tn_Areas")
                .Where("AreaCode = @0", area.AreaCode);
            Area oldArea = database.FirstOrDefault<Area>(sql_selete);

            area.Depth = newParentDepth + 1;
            IList<PetaPoco.Sql> sql_updates = new List<PetaPoco.Sql>();
            //在没有更新父节点的情况下，仅更新自身的属性。
            sql_updates.Add(new PetaPoco.Sql("update tn_Areas set Name = @1,PostCode = @2,DisplayOrder = @3 where AreaCode= @0", area.AreaCode, area.Name, area.PostCode, area.DisplayOrder));
            //如果用户调整了父节点
            if (area.ParentCode.ToLower() != oldArea.ParentCode.ToLower())
            {
                //如果用户更新了其父节点，更新自己的深度，更新原来的父节点和新的父节点的childcount
                sql_updates.Add(new PetaPoco.Sql("update tn_Areas set Depth = @1,ParentCode = @2 where AreaCode = @0", area.AreaCode, area.Depth, area.ParentCode));
                sql_updates.Add(new PetaPoco.Sql("update tn_Areas set ChildCount = ChildCount - 1 where AreaCode = @0", oldArea.ParentCode));
                sql_updates.Add(new PetaPoco.Sql("update tn_Areas set ChildCount = ChildCount + 1 where AreaCode = @0", area.ParentCode));

                int differenceDepth = area.Depth - oldArea.Depth;

                //如果原来的父节点与新的父节点不是在同一等级上，更新所有的子节点的深度。
                if (differenceDepth != 0)
                {
                    IEnumerable<Area> childAreas = GetDescendants(area.AreaCode);
                    if (childAreas != null && childAreas.Count() > 0)
                    {
                        foreach (Area childArea in childAreas)
                            sql_updates.Add(new PetaPoco.Sql("update tn_Areas set Depth = Depth + @1 where AreaCode = @0", childArea.AreaCode, differenceDepth));
                    }
                }
            }
            database.Execute(sql_updates);
            ClearChache();
        }

        /// <summary>
        /// 删除地区点
        /// </summary>
        /// <param name="areaCode">地区编码</param>
        public void Delete(string areaCode)
        {
            //删除数据库数据
            IList<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();
            sqls.Add(new PetaPoco.Sql("delete from tn_Areas where AreaCode = @0", areaCode));

            IEnumerable<Area> descendantAreas = GetDescendants((string)areaCode);
            foreach (Area item in descendantAreas)
            {
                sqls.Add(new PetaPoco.Sql("delete from tn_Areas where AreaCode = @0", item.AreaCode));
            }

            Area area = Get(areaCode);
            sqls.Add(new PetaPoco.Sql("update tn_Areas set ChildCount=ChildCount-1 where AreaCode=@0", area.ParentCode));
            CreateDAO().Execute(sqls);
            //更新缓存
            ClearChache();
        }

        #endregion

        #region Get && Gets
        /// <summary>
        /// 获取某一地区
        /// </summary>
        /// <param name="areaCode">地区编码</param>
        /// <returns>地区实体</returns>
        public Area Get(string areaCode)
        {
            Dictionary<string, Area> areaDictionary = GetAreaDictionary();
            if (areaDictionary.ContainsKey(areaCode))
                return areaDictionary[areaCode];
            return null;
        }

        /// <summary>
        /// 获取所有子地区
        /// </summary>
        public IEnumerable<Area> GetDescendants(string parentAreaCode)
        {
            Area parentArea = Get(parentAreaCode);
            Dictionary<string, Area> allChildAreas = new Dictionary<string, Area>();
            if (parentArea != null)
                RecursiveGetAllAreas(parentArea, ref allChildAreas);

            return allChildAreas.Values;
        }

        /// <summary>
        /// 获取根地区
        /// </summary>
        /// <returns>根地区列表</returns>
        public IEnumerable<Area> GetRoots()
        {
            
            //回复：已经修改，以后注意
            IEnumerable<Area> rootAreas = cacheService.Get<IEnumerable<Area>>(GetCacheKey_AreaRootIEnumerable());
            if (rootAreas == null)
            {
                Dictionary<string, Area> areaDictionary = GetAreaDictionary();
                if (areaDictionary == null)
                    return null;
                rootAreas = areaDictionary.Values.Where(n => string.IsNullOrEmpty(n.ParentCode));
                cacheService.Add(GetCacheKey_AreaRootIEnumerable(), rootAreas, CachingExpirationType.Stable);
            }
            return rootAreas;
        }

        #endregion

        #region Help Methods
        /// <summary>
        /// 获取地区的字典类型
        /// </summary>
        /// <returns>地区的字典类型</returns>
        private Dictionary<string, Area> GetAreaDictionary()
        {   
            string cachekey = GetCacheKey_AreaDictionary();
            Dictionary<string, Area> areaDictionary = cacheService.Get<Dictionary<string, Area>>(cachekey);
            if (areaDictionary == null)
            {
                areaDictionary = GetAllAreas();
                cacheService.Add(cachekey, areaDictionary, CachingExpirationType.Stable);
            }
            return areaDictionary;
        }
        /// <summary>
        /// 递归获取parentArea所有子Area
        /// </summary>
        /// <param name="parentArea">父地区</param>
        /// <param name="allChildAreas">递归获取的所有子地区</param>
        private void RecursiveGetAllAreas(Area parentArea, ref Dictionary<string, Area> allChildAreas)
        {
            if (parentArea.Children.Count() > 0)
            {
                foreach (Area area in parentArea.Children)
                {
                    allChildAreas[area.AreaCode] = area;
                    RecursiveGetAllAreas(area, ref allChildAreas);
                }
            }
        }

        /// <summary>
        /// 获取全部的地区的方法
        /// </summary>
        /// <returns>所有的地区</returns>
        private Dictionary<string, Area> GetAllAreas()
        {
            Database database = CreateDAO();

            database.OpenSharedConnection();
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select Max(Depth) from tn_Areas");
            int maxDepth = database.FirstOrDefault<int?>(sql) ?? 0;
            var sql_GetAll = PetaPoco.Sql.Builder;
            sql_GetAll.Select("*")
                .From("tn_Areas")
                .OrderBy("DisplayOrder,AreaCode");
            List<Area> AreasList = database.Fetch<Area>(sql_GetAll);
            database.CloseSharedConnection();
            return Organize(AreasList, maxDepth);
        }

        /// <summary>
        /// 生成类别深度信息并对类别进行计数统计
        /// </summary>
        private Dictionary<string, Area> Organize(List<Area> allAreas, int maxDepth)
        {
            Dictionary<string, Area>[] areasDictionaryArray = new Dictionary<string, Area>[maxDepth + 1];
            for (int i = 0; i <= maxDepth; i++)
            {
                areasDictionaryArray[i] = new Dictionary<string, Area>();
            }

            foreach (Area _area in allAreas)
            {
                areasDictionaryArray[_area.Depth][_area.AreaCode] = _area;
            }

            //组织Area.Childs
            for (int i = maxDepth; i > 0; i--)
            {
                foreach (KeyValuePair<string, Area> pair in areasDictionaryArray[i])
                {
                    areasDictionaryArray[i - 1][pair.Value.ParentCode].AppendChild(pair.Value);
                }
            }

            Dictionary<string, Area> organizedAreas = new Dictionary<string, Area>();
            foreach (Area area in allAreas)
            {
                organizedAreas[area.AreaCode] = area;
            }
            return organizedAreas;
        }


        /// <summary>
        /// 获取地区的地点集合的cachekey
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey_AreaDictionary()
        {
            return "AreaDictionary" + RealTimeCacheHelper.GetGlobalVersion();
        }

        private string GetCacheKey_AreaRootIEnumerable()
        {
            return "AreaRootList" + RealTimeCacheHelper.GetGlobalVersion();
        }

        private void ClearChache()
        {
            RealTimeCacheHelper.IncreaseGlobalVersion();
        }
        #endregion

    }
}
