//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spacebuilder.UI;

namespace Spacebuilder.Common.Repositories
{
    /// <summary>
    /// 自定义样式Repository接口
    /// </summary>    
    public interface ICustomStyleRepository
    {
        /// <summary>
        /// 获取预置的配色方案
        /// </summary>
        /// <param name="presentAreaKey"></param>
        IEnumerable<CustomStyle> GetColorSchemes(string presentAreaKey);

        /// <summary>
        /// 获取用户自定义风格
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">OwnerId</param>
        /// <returns>无相应数据返回null</returns>
        CustomStyleEntity Get(string presentAreaKey, long ownerId);

        /// <summary>
        /// 保存用户自定义风格
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">OwnerId</param>
        /// <param name="customStyle">自定义风格实体</param>
        void Save(string presentAreaKey, long ownerId, CustomStyle customStyle);
    }
}
