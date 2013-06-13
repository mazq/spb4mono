//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 积分项
    /// </summary>
    public static class PointItemKeysExtension
    {
        /// <summary>
        /// 创建微博
        /// </summary>
        public static string Bar_CreateThread(this PointItemKeys pointItemKeys)
        {
            return "Bar_CreateThread";
        }

        /// <summary>
        /// 删除微博
        /// </summary>
        public static string Bar_DeleteThread(this PointItemKeys pointItemKeys)
        {
            return "Bar_DeleteThread";
        }
    }
}
