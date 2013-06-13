//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 媒体网址数据访问
    /// </summary>
    public class ParsedMediaRepository : Repository<ParsedMedia>, IParsedMediaRepository
    {
        /// <summary>
        /// 插入新数据的方法
        /// </summary>
        /// <param name="entity">准备插入的实体</param>
        /// <returns>是否插入成功</returns>
        public override object Insert(ParsedMedia entity)
        {            
            
            //回复：已经修复了对应的问题

            PetaPocoDatabase dao = CreateDAO();
            int affectedCount = -1;
            dao.OpenSharedConnection();
            bool isExist = dao.Exists<ParsedMedia>(entity.Alias);
            if (!isExist)
            {
                Sql sql_Insert = Sql.Builder;
                sql_Insert.Append("insert into tn_ParsedMedias(Alias,Url,MediaType,Name,Description,ThumbnailUrl,PlayerUrl,SourceFileUrl,DateCreated) values(@0,@1,@2,@3,@4,@5,@6,@7,@8)"
                , entity.Alias, entity.Url, entity.MediaType, entity.Name, entity.Description, entity.ThumbnailUrl, entity.PlayerUrl, entity.SourceFileUrl, entity.DateCreated);
                affectedCount = dao.Execute(sql_Insert);
                base.OnInserted(entity);
            }
            dao.CloseSharedConnection();
            return affectedCount == 1;
            
            //回复：已经处理缓存
        }
    }
}