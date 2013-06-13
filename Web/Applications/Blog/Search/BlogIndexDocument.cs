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

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 日志索引文档
    /// </summary>
    public class BlogIndexDocument
    {
        #region 索引字段

        public static readonly string ThreadId = "ThreadId";
        public static readonly string TenantTypeId = "TenantTypeId";
        public static readonly string OwnerId = "OwnerId";
        public static readonly string IsEssential = "IsEssential";
        public static readonly string Keywords = "Keywords";
        public static readonly string Summary = "Summary";
        public static readonly string UserId = "UserId";
        public static readonly string Subject = "Subject";
        public static readonly string Body = "Body";
        public static readonly string Author = "Author";
        public static readonly string Tag = "Tag";
        public static readonly string OwnerCategoryName = "OwnerCategoryName";
        public static readonly string SiteCategoryId = "SiteCategoryId";
        public static readonly string DateCreated = "DateCreated";
        public static readonly string AuditStatus = "AuditStatus";
        public static readonly string PrivacyStatus = "PrivacyStatus";

        #endregion

        /// <summary>
        /// BlogThread转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="blogThread">日志实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(BlogThread blogThread)
        {
            Document doc = new Document();

            //索引日志基本信息
            doc.Add(new Field(BlogIndexDocument.ThreadId, blogThread.ThreadId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BlogIndexDocument.TenantTypeId, blogThread.TenantTypeId, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BlogIndexDocument.OwnerId, blogThread.OwnerId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BlogIndexDocument.IsEssential, blogThread.IsEssential ? "1" : "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BlogIndexDocument.Keywords, blogThread.Keywords ?? "", Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(BlogIndexDocument.Summary, blogThread.Summary ?? "", Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(BlogIndexDocument.UserId, blogThread.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BlogIndexDocument.Subject, blogThread.Subject.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(BlogIndexDocument.Body, HtmlUtility.TrimHtml(blogThread.GetBody(),0).ToLower(), Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(BlogIndexDocument.Author, blogThread.Author, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(BlogIndexDocument.DateCreated, DateTools.DateToString(blogThread.DateCreated, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BlogIndexDocument.AuditStatus, ((int)blogThread.AuditStatus).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(BlogIndexDocument.PrivacyStatus, ((int)blogThread.PrivacyStatus).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            //索引日志tag
            foreach (string tagName in blogThread.TagNames)
            {
                doc.Add(new Field(BlogIndexDocument.Tag, tagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            //索引日志用户分类名称
            IEnumerable<string> ownerCategoryNames = blogThread.OwnerCategoryNames;
            if (ownerCategoryNames != null)
            {
                foreach (string ownerCategoryName in ownerCategoryNames)
                {
                    doc.Add(new Field(BlogIndexDocument.OwnerCategoryName, ownerCategoryName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
                }
            }

            //索引日志站点分类ID
            long? siteCategoryId = blogThread.SiteCategoryId;
            if (siteCategoryId.HasValue)
            {
                doc.Add(new Field(BlogIndexDocument.SiteCategoryId, siteCategoryId.Value.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            }

            return doc;
        }

        /// <summary>
        /// BlogThread批量转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="blogThreads">日志实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static IEnumerable<Document> Convert(IEnumerable<BlogThread> blogThreads)
        {
            List<Document> docs = new List<Document>();
            foreach (BlogThread blogThread in blogThreads)
            {
                Document doc = Convert(blogThread);
                docs.Add(doc);
            }

            return docs;
        }


    }
}