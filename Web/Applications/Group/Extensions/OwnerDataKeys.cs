using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;

namespace Spacebuilder.Group
{
    public static class OwnerDataKeysExtension
    {
        /// <summary>
        /// 创建的群组数
        /// </summary>
        /// <param name="ownerDataKeys"></param>
        /// <returns></returns>
        public static string CreatedGroupCount(this OwnerDataKeys ownerDataKeys)
        {
            return GroupConfig.Instance().ApplicationKey + "-ThreadCount";
        }

        /// <summary>
        /// 加入的群组数
        /// </summary>
        /// <param name="ownerDataKeys"></param>
        /// <returns></returns>
        public static string JoinedGroupCount(this OwnerDataKeys ownerDataKeys)
        {
            return GroupConfig.Instance().ApplicationKey + "JoinedGroupCount";
        }
       
        /// <summary>
        /// 群组的内容数
        /// </summary>
        /// <param name="ownerDataKeys"></param>
        /// <returns></returns>
        public static string GroupContentCount(this OwnerDataKeys ownerDataKeys)
        {
            return GroupConfig.Instance().ApplicationKey + "GroupContentCount";
        }
    }
}