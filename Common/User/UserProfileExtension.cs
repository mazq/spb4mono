//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-12-04</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-12-04" version="0.5">创建</log>
//--------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 扩展用户业务逻辑
    /// </summary>
    public static class UserProfileExtension
    {
        /// <summary>
        /// 用户的第三人称
        /// </summary>
        /// <param name="userProfile">用户资料实体</param>
        /// <returns></returns>
        public static string ThirdPerson(this UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return string.Empty;
            }

            if (UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == userProfile.UserId)
            {
                return "我";
            }

            string resourceKey = "Common_";
            switch (userProfile.Gender)
            {
                case GenderType.FeMale:
                    resourceKey += "She";
                    break;
                case GenderType.Male:
                    resourceKey += "He";
                    break;
                default:
                    resourceKey += "Ta";
                    break;
            }

            return Tunynet.Globalization.ResourceAccessor.GetString(resourceKey);
        }
    }
}