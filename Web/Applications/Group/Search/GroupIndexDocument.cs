//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Documents;
using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组索引文档
    /// </summary>
    public class GroupIndexDocument
    {
        #region 索引字段

        public static readonly string GroupId = "GroupId";
        public static readonly string GroupName = "GroupName";
        public static readonly string Description = "Description";
        public static readonly string IsPublic = "IsPublic";
        public static readonly string AreaCode = "AreaCode";
        public static readonly string UserId = "UserId";
        public static readonly string AuditStatus = "AuditStatus";
        public static readonly string DateCreated = "DateCreated";
        public static readonly string Tag = "Tag";
        public static readonly string CategoryName = "CategoryName";
        public static readonly string CategoryId = "CategoryId";
        public static readonly string MemberCount = "MemberCount";
        public static readonly string GrowthValue = "GrowthValue";

        #endregion

        /// <summary>
        /// GroupEntity转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="GroupEntity">群组实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(GroupEntity group)
        {
            Document doc = new Document();

            //索引群组基本信息
            doc.Add(new Field(GroupIndexDocument.GroupId, group.GroupId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(GroupIndexDocument.GroupName, group.GroupName, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(GroupIndexDocument.Description, group.Description, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(GroupIndexDocument.IsPublic, group.IsPublic==true ? "1" : "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(GroupIndexDocument.AreaCode, group.AreaCode, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(GroupIndexDocument.UserId, group.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(GroupIndexDocument.AuditStatus, ((int)group.AuditStatus).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(GroupIndexDocument.DateCreated, DateTools.DateToString(group.DateCreated, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(GroupIndexDocument.MemberCount,group.MemberCount.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(GroupIndexDocument.GrowthValue, group.GrowthValue.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            if (group.Category != null)
            {
                doc.Add(new Field(GroupIndexDocument.CategoryName, group.Category.CategoryName, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(GroupIndexDocument.CategoryId, group.Category.CategoryId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            }

            //索引群组tag
            foreach (string tagName in group.TagNames)
            {
                doc.Add(new Field(GroupIndexDocument.Tag, tagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            return doc;
        }

        /// <summary>
        /// group批量转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="groups">日志实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static IEnumerable<Document> Convert(IEnumerable<GroupEntity> groups)
        {
            List<Document> docs = new List<Document>();
            foreach (GroupEntity group in groups)
            {
                Document doc = Convert(group);
                docs.Add(doc);
            }

            return docs;
        }


    }
}