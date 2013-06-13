//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Lucene.Net.Documents;
using Tunynet.Common;
using NPinyin;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户索引内容
    /// 注意：当前索引了所有用户，索引时为加过滤条件
    /// </summary>
    public static class UserIndexDocument
    {
        #region 用户索引字段

        public static readonly string UserId = "UserId";
        public static readonly string UserName = "UserName";
        public static readonly string PinyinName = "PinyinName";
        public static readonly string ShortPinyinName = "ShortPinyinName";
        public static readonly string TrueName = "TrueName";
        public static readonly string NickName = "NickName";
        public static readonly string Gender = "Gender";
        public static readonly string Birthday = "Birthday";
        public static readonly string BirthdayYear = "BirthdayYear";
        public static readonly string NowAreaCode = "NowAreaCode";
        public static readonly string HomeAreaCode = "HomeAreaCode";
        public static readonly string HasAvatarImage = "HasAvatarImage";
        public static readonly string School = "School";
        public static readonly string CompanyName = "CompanyName";
        public static readonly string Introduction = "Introduction";
        public static readonly string TagName = "TagName";
        public static readonly string DateCreated = "DateCreated";
        public static readonly string LastActivityTime = "LastActivityTime";

        #endregion



        /// <summary>
        /// User转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(User user)
        {
            Document doc = new Document();

            if (user == null)
                return doc;
            //索引用户基本信息
            doc.Add(new Field(UserIndexDocument.UserId, user.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(UserIndexDocument.UserName, user.UserName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.TrueName, user.TrueName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.NickName, user.NickName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.DateCreated, DateTools.DateToString(user.DateCreated, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(UserIndexDocument.LastActivityTime, DateTools.DateToString(user.LastActivityTime, DateTools.Resolution.MINUTE), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //索引用户名称的拼音信息方便支持拼音搜索
            doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.UserName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.UserName.ToLower()).Replace(" ", ""), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.ShortPinyinName, Pinyin.GetInitials(user.UserName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
            if (!string.IsNullOrEmpty(user.NickName))
            {
                doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.NickName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.NickName.ToLower()).Replace(" ", ""), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(UserIndexDocument.ShortPinyinName, Pinyin.GetInitials(user.NickName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
            }
            if (!string.IsNullOrEmpty(user.TrueName))
            {
                doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.TrueName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.TrueName.ToLower()).Replace(" ", ""), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(UserIndexDocument.ShortPinyinName, Pinyin.GetInitials(user.TrueName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
            }

            //索引用户资料
            if (user.Profile != null)
            {
                doc.Add(new Field(UserIndexDocument.Gender, ((int)user.Profile.Gender).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                if (user.Profile.Birthday != null)
                {
                    doc.Add(new Field(UserIndexDocument.Birthday, DateTools.DateToString(user.Profile.Birthday, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));

                    //注意此处不能用NumericField，因为Lucene有NumericField不能序列化的bug，会导致采用WCF分布式部署时调用失败，所以只能以字符串的方式索引
                    string birthdayYear = user.Profile.Birthday.Year.ToString().PadLeft(3, '0');
                    doc.Add(new Field(UserIndexDocument.BirthdayYear, birthdayYear, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }

                if (user.Profile.NowAreaCode != null)
                {
                    doc.Add(new Field(UserIndexDocument.NowAreaCode, user.Profile.NowAreaCode, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }

                if (user.Profile.HomeAreaCode != null)
                {
                    doc.Add(new Field(UserIndexDocument.HomeAreaCode, user.Profile.HomeAreaCode, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }

                int hasAvatarImage = 0;
                if (user.HasAvatar)
                {
                    hasAvatarImage = 1;
                }
                doc.Add(new Field(UserIndexDocument.HasAvatarImage, hasAvatarImage.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                if (user.Profile.Introduction != null)
                {
                    doc.Add(new Field(UserIndexDocument.Introduction, user.Profile.Introduction.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
                }

                foreach (EducationExperience educationExperience in user.Profile.EducationExperience)
                {
                    doc.Add(new Field(UserIndexDocument.School, educationExperience.School.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
                }
                foreach (WorkExperience workExperience in user.Profile.WorkExperience)
                {
                    doc.Add(new Field(UserIndexDocument.CompanyName, workExperience.CompanyName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
                }
            }

            //索引tag
            TagService tagService = new TagService(TenantTypeIds.Instance().User());

            
            //fixed by jiangshl,已按要求修改
            IEnumerable<ItemInTag> itemInTags = tagService.GetItemInTagsOfItem(user.UserId);
            foreach (ItemInTag itemInTag in itemInTags)
            {
                doc.Add(new Field(UserIndexDocument.TagName, itemInTag.TagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            return doc;
        }

        /// <summary>
        /// 将动态类型的用户对象转换为Lucene Document对象
        /// </summary>
        /// <param name="user">dynamic对象</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(dynamic user)
        {
            Document doc = new Document();
            if (user == null)
                return doc;

            doc.Add(new Field(UserIndexDocument.UserId, user.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(UserIndexDocument.UserName, user.UserName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.TrueName, user.TrueName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.NickName, user.NickName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.DateCreated, DateTools.DateToString(user.DateCreated, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(UserIndexDocument.LastActivityTime, DateTools.DateToString(user.LastActivityTime, DateTools.Resolution.MINUTE), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //索引用户名称的拼音信息方便支持拼音搜索
            doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.UserName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.UserName.ToLower()).Replace(" ", ""), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(UserIndexDocument.ShortPinyinName, Pinyin.GetInitials(user.UserName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
            if (!string.IsNullOrEmpty(user.NickName))
            {
                doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.NickName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.NickName.ToLower()).Replace(" ", ""), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(UserIndexDocument.ShortPinyinName, Pinyin.GetInitials(user.NickName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
            }
            if (!string.IsNullOrEmpty(user.TrueName))
            {
                doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.TrueName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(UserIndexDocument.PinyinName, Pinyin.GetPinyin(user.TrueName.ToLower()).Replace(" ", ""), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(UserIndexDocument.ShortPinyinName, Pinyin.GetInitials(user.TrueName.ToLower()), Field.Store.YES, Field.Index.ANALYZED));
            }

            if (user.Gender != null)
            {
                doc.Add(new Field(UserIndexDocument.Gender, ((int)user.Gender).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            }
            if (user.Birthday != null)
            {
                doc.Add(new Field(UserIndexDocument.Birthday, DateTools.DateToString(user.Birthday, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));

                //注意此处不能用NumericField，因为Lucene有NumericField不能序列化的bug，会导致采用WCF分布式部署时调用失败，所以只能以字符串的方式索引
                string birthdayYear = user.Birthday.Year.ToString().PadLeft(3, '0');
                doc.Add(new Field(UserIndexDocument.BirthdayYear, birthdayYear, Field.Store.YES, Field.Index.NOT_ANALYZED));
            }
            if (user.NowAreaCode != null)
            {
                doc.Add(new Field(UserIndexDocument.NowAreaCode, user.NowAreaCode, Field.Store.YES, Field.Index.NOT_ANALYZED));
            }
            if (user.HomeAreaCode != null)
            {
                doc.Add(new Field(UserIndexDocument.HomeAreaCode, user.HomeAreaCode, Field.Store.YES, Field.Index.NOT_ANALYZED));
            }
            if (user.AvatarImage != null)
            {
                int hasAvatarImage = 0;
                if (!string.IsNullOrEmpty(user.AvatarImage.Trim()))
                {
                    hasAvatarImage = 1;
                }
                doc.Add(new Field(UserIndexDocument.HasAvatarImage, hasAvatarImage.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            }
            if (user.Introduction != null)
            {
                doc.Add(new Field(UserIndexDocument.Introduction, user.Introduction.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }
            foreach (string school in user.Schools)
            {
                doc.Add(new Field(UserIndexDocument.School, school.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            foreach (string companyName in user.CompanyNames)
            {
                doc.Add(new Field(UserIndexDocument.CompanyName, companyName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            foreach (string tagName in user.TagNames)
            {
                doc.Add(new Field(UserIndexDocument.TagName, tagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            return doc;
        }

        /// <summary>
        /// 将User列表转换成<see cref="Lucene.Net.Documents.Document"/>列表
        /// </summary>
        /// <param name="users">User列表</param>
        /// <returns>Document列表</returns>
        public static IEnumerable<Document> Convert(IEnumerable<User> users)
        {
            List<Document> docs = new List<Document>();
            foreach (User user in users)
            {
                Document doc = Convert(user);
                docs.Add(doc);
            }

            return docs;
        }

        /// <summary>
        /// 将动态用户对象列表转换成<see cref="Lucene.Net.Documents.Document"/>列表
        /// </summary>
        /// <param name="users"></param>
        /// <returns>Document列表</returns>
        public static IEnumerable<Document> Convert(IEnumerable<dynamic> users)
        {
            List<Document> docs = new List<Document>();
            foreach (dynamic user in users)
            {
                Document doc = Convert(user);
                docs.Add(doc);
            }

            return docs;
        }

    }
}
