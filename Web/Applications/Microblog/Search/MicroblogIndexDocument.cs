//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Lucene.Net.Documents;
using Tunynet.Common;
using Spacebuilder.Common;
using Tunynet.Utilities;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博索引文档
    /// </summary>
    public static class MicroblogIndexDocument
    {
        #region 索引字段

        public static readonly string MicroblogId = "MicroblogId";
        public static readonly string IsOriginality = "IsOriginality";
        public static readonly string HasPhoto = "HasPhoto";
        public static readonly string HasMusic = "HasMusic";
        public static readonly string HasVideo = "HasVideo";
        public static readonly string DateCreated = "DateCreated";
        public static readonly string Body = "Body";
        public static readonly string Topic = "Topic";
        public static readonly string TenantTypeId = "TenantTypeId";

        #endregion


        /// <summary>
        /// MicroblogEntity转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="microblog">微博实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(MicroblogEntity microblog)
        {
            Document doc = new Document();
            //索引微博基本信息
            doc.Add(new Field(MicroblogIndexDocument.MicroblogId, microblog.MicroblogId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            if (microblog.OriginalMicroblog != null)
            {
                doc.Add(new Field(MicroblogIndexDocument.Body, HtmlUtility.TrimHtml(microblog.Body, 0).ToLower() + HtmlUtility.TrimHtml(microblog.OriginalMicroblog.Body, 0).ToLower(), Field.Store.NO, Field.Index.ANALYZED));
            }
            else
            {
                doc.Add(new Field(MicroblogIndexDocument.Body, HtmlUtility.TrimHtml(microblog.Body, 0).ToLower(), Field.Store.NO, Field.Index.ANALYZED));
            }
            doc.Add(new Field(MicroblogIndexDocument.DateCreated, DateTools.DateToString(microblog.DateCreated, DateTools.Resolution.MILLISECOND), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(MicroblogIndexDocument.HasMusic, microblog.HasMusic ? "1" : "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(MicroblogIndexDocument.HasPhoto, microblog.HasPhoto ? "1" : "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(MicroblogIndexDocument.HasVideo, microblog.HasVideo ? "1" : "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(MicroblogIndexDocument.IsOriginality, microblog.ForwardedMicroblogId == 0 ? "1" : "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(MicroblogIndexDocument.TenantTypeId, microblog.TenantTypeId, Field.Store.YES, Field.Index.NOT_ANALYZED));


            TagService tagService = new TagService(TenantTypeIds.Instance().Microblog());

            IEnumerable<ItemInTag> itemInTags = tagService.GetItemInTagsOfItem(microblog.MicroblogId);
            foreach (ItemInTag itemInTag in itemInTags)
            {
                doc.Add(new Field(MicroblogIndexDocument.Topic, itemInTag.TagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            return doc;
        }

        /// <summary>
        /// MicroblogEntity批量转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="microblogs">微博实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static IEnumerable<Document> Convert(IEnumerable<MicroblogEntity> microblogs)
        {
            List<Document> docs = new List<Document>();
            foreach (MicroblogEntity microblog in microblogs)
            {
                Document doc = Convert(microblog);
                docs.Add(doc);
            }

            return docs;
        }
    }
}