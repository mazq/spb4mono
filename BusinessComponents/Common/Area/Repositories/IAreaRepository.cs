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
    /// 地区访问的借口
    /// </summary>
    public interface IAreaRepository
    {
        /// <summary>
        /// 更新地区
        /// </summary>
        /// <param name="area">地区</param>
        void Update(Area area);


        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="areaCode">地区编码</param>
        /// <returns>地区</returns>
        Area Get(string areaCode);

        /// <summary>
        /// 获取根级地区列表
        /// </summary>
        /// <returns>根级地区列表</returns>
        IEnumerable<Area> GetRoots();

        /// <summary>
        /// 获取某一地区的所有后代地区
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns>所有后代地区</returns>
        IEnumerable<Area> GetDescendants(string areaCode);

        /// <summary>
        /// 创建地区
        /// </summary>
        /// <param name="area">地区</param>
        void Insert(Area area);

        /// <summary>
        /// 删除地区
        /// </summary>
        /// <param name="areaCode">地区编码</param>
        void Delete(string areaCode);
    }
}
