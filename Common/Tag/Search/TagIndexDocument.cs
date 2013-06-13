//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    public class TagIndexDocument
    {
        #region 标签索引字段
        public static readonly string TagName = "TagName";
        public static readonly string TenantTypeId = "TenantTypeId";
        public static readonly string ItemCount = "ItemCount";
        public static readonly string OwnerId = "OwnerId";
        public static readonly string TagId = "TagId";  //tn_tags表的tagid
        public static readonly string TagInOwnerId = "TagInOwnerId"; //tn_taginowners表的id
        #endregion

        public static IEnumerable<Document> Convert(IEnumerable<Tag> tags)
        {
            List<Document> docs = new List<Document>();
            foreach (Tag tag in tags)
            {
                Document doc = Convert(tag);
                docs.Add(doc);
            }

            return docs;
        }

        public static Document Convert(Tag tag)
        {
            Document doc = new Document();
            doc.Add(new Field(TagIndexDocument.TagName,tag.TagName,Field.Store.YES,Field.Index.ANALYZED));
            doc.Add(new Field(TagIndexDocument.TenantTypeId, tag.TenantTypeId,Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TagIndexDocument.ItemCount, tag.ItemCount.ToString(),Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TagIndexDocument.OwnerId,"0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TagIndexDocument.TagId, tag.TagId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TagIndexDocument.TagInOwnerId, "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            return doc;
        }

        public static IEnumerable<Document> Convert(IEnumerable<TagInOwner> tagInOwners)
        {
            List<Document> docs = new List<Document>();
            foreach (TagInOwner tagInOwner in tagInOwners)
            {
                Document doc = Convert(tagInOwner);
                docs.Add(doc);
            }
            return docs;
        }

        public static Document Convert(TagInOwner tagInOwner)
        {
            Document doc = new Document();
            doc.Add(new Field(TagIndexDocument.TagName, tagInOwner.TagName, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(TagIndexDocument.TenantTypeId, tagInOwner.TenantTypeId, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TagIndexDocument.ItemCount, tagInOwner.ItemCount.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TagIndexDocument.OwnerId,tagInOwner.OwnerId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TagIndexDocument.TagId,"0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TagIndexDocument.TagInOwnerId, tagInOwner.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            return doc;
        }
    }
}
