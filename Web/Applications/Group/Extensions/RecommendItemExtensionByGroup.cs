//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 推荐项类型扩展类
    /// </summary>
    public static class RecommendItemExtensionByGroup
    {
        /// <summary>
        /// 
        /// </summary>
        public static GroupEntity GetGroup(this RecommendItem item)
        {
            return new GroupService().Get(item.ItemId);
        }
    }
}