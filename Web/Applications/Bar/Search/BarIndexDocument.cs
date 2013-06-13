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
using Tunynet.Utilities;

namespace Spacebuilder.Bar.Search
{
    /// <summary>
    /// 帖吧索引文档
    /// </summary>
    public class BarIndexDocument
    {
        private static BarThreadService barThreadService = new BarThreadService();
        private static BarPostService barPostService = new BarPostService();

        #region 帖吧索引字段

        public static readonly string SectionId = "SectionId";
        public static readonly string ThreadId = "ThreadId";
        public static readonly string PostId = "PostId";
        public static readonly string Subject = "Subject";
        public static readonly string Body = "Body";
        public static readonly string Author = "Author";
        public static readonly string Tag = "Tag";
        public static readonly string Category = "Category";
        public static readonly string DateCreated = "DateCreated";
        public static readonly string IsPost = "IsPost";
        public static readonly string TenantTypeId = "TenantTypeId";

        #endregion

        /// <summary>
        /// BarThread转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="BarThread">发帖实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(BarThread barThread)
        {
            Document doc = new Document();

            //索引发帖基本信息
            doc.Add(new Field(BarIndexDocument.SectionId, barThread.SectionId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.ThreadId, barThread.ThreadId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.PostId, "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.Subject, barThread.Subject.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(BarIndexDocument.Body, HtmlUtility.TrimHtml(barThread.GetBody(), 0).ToLower(), Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(BarIndexDocument.Author, barThread.Author, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(BarIndexDocument.IsPost, "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.DateCreated, DateTools.DateToString(barThread.DateCreated, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.TenantTypeId, barThread.TenantTypeId, Field.Store.YES, Field.Index.NOT_ANALYZED));

            //索引发帖tag
            TagService tagService = new TagService(TenantTypeIds.Instance().BarThread());

            IEnumerable<ItemInTag> itemInTags = tagService.GetItemInTagsOfItem(barThread.ThreadId);
            foreach (ItemInTag itemInTag in itemInTags)
            {
                doc.Add(new Field(BarIndexDocument.Tag, itemInTag.TagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            return doc;
        }

        /// <summary>
        /// BarPost转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="BarPost">回帖实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(BarPost barPost)
        {
            Document doc = new Document();

            //索引回帖基本信息
            doc.Add(new Field(BarIndexDocument.SectionId, barPost.SectionId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.ThreadId, "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.PostId, barPost.PostId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //如果回帖没有主题存储其主贴的主题
            if (string.IsNullOrEmpty(barPost.Subject))
            {
                string subject = barThreadService.Get(barPostService.Get(barPost.PostId).ThreadId).Subject.ToLower();
                doc.Add(new Field(BarIndexDocument.Subject, subject, Field.Store.YES, Field.Index.ANALYZED));
            }
            else
            {
                doc.Add(new Field(BarIndexDocument.Subject, barPost.Subject, Field.Store.YES, Field.Index.ANALYZED));
            }
            doc.Add(new Field(BarIndexDocument.Body, HtmlUtility.TrimHtml(barPost.GetBody(), 0).ToLower(), Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(BarIndexDocument.Author, barPost.Author, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(BarIndexDocument.IsPost, "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.DateCreated, DateTools.DateToString(barPost.DateCreated, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BarIndexDocument.TenantTypeId, barPost.TenantTypeId, Field.Store.YES, Field.Index.NOT_ANALYZED));

            return doc;
        }

        /// <summary>
        /// BarThread批量转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="barThreads">发帖实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static IEnumerable<Document> Convert(IEnumerable<BarThread> barThreads)
        {
            List<Document> docs = new List<Document>();
            foreach (BarThread barThread in barThreads)
            {
                Document doc = Convert(barThread);
                docs.Add(doc);
            }

            return docs;
        }

        /// <summary>
        /// BarPost批量转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="barPosts">回帖实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static IEnumerable<Document> Convert(IEnumerable<BarPost> barPosts)
        {
            List<Document> docs = new List<Document>();
            foreach (BarPost barPost in barPosts)
            {
                Document doc = Convert(barPost);
                docs.Add(doc);
            }

            return docs;
        }
    }
}