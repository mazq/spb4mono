//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common.Repositories
{

    /// <summary>
    /// 内容隐私设置指定对象接口
    /// </summary>
     public interface IContentPrivacySpecifyObjectsRepository
    {
        /// <summary>
        /// 获取内容隐私设置指定对象
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="contentId">内容Id</param>
        /// <returns><remarks>key=specifyObjectTypeId,value=内容指定对象集合</remarks></returns>
         Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> GetPrivacySpecifyObjects(string tenantTypeId, long contentId);
    
     
        /// <summary>
        /// 更新内容隐私设置
        /// </summary>
        /// <param name="privacyable">可隐私接口</param>
        /// <param name="specifyObjects"><remarks>key=specifyObjectTypeId,value=内容指定对象集合</remarks></param>
         void UpdatePrivacySettings(IPrivacyable privacyable, Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> specifyObjects);
        
     }
}
