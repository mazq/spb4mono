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
using PetaPoco;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 认证标识Repository
    /// </summary>
    public class IdentificationTypeRepository : Repository<IdentificationType>, IIdentificationTypeRepository
    {

        /// <summary>
        /// 获取所有认证标识
        /// </summary>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        public IEnumerable<IdentificationType> GetIdentificationTypes(bool? isEnabled)
        {
            if (isEnabled.HasValue)
                return base.GetAll().Where(n => n.Enabled == isEnabled.Value);
            else
                return base.GetAll();
        }

        /// <summary>
        /// 删除认证标识和该认证标识下的所有申请
        /// </summary>
        /// <param name="identificationTypeId">认证标识Id</param>
        public void DeleteIdentificationTypes(long identificationTypeId)
        {
            Sql sql = Sql.Builder.Append("delete from spb_Identifications where IdentificationTypeId=@0",identificationTypeId);
            CreateDAO().Execute(sql);
            base.Delete(base.Get(identificationTypeId));
        }
    }
}
