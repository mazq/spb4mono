//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.Search;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户搜索器
    /// </summary>
    public class UserSearcher : ISearcher
    {
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private FollowService followService = new FollowService();
        private UserProfileService userProfileService = new UserProfileService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().User());
        private ISearchEngine searchEngine;
        public static string CODE = "UserSearcher";
        public static string WATERMARK = "搜索用户昵称、用户名";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public UserSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = WebUtility.GetPhysicalFilePath(indexPath);
            this.AsQuickSearch = asQuickSearch;
            this.DisplayOrder = displayOrder;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
        }

        #region 搜索器属性

        public string WaterMark { get { return WATERMARK; } }

        /// <summary>
        /// 搜索器的唯一标识
        /// </summary>
        public string Code { get { return CODE; } }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 是否前台显示
        /// </summary>
        public bool IsDisplay
        {
            get { return true; }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; private set; }

        /// <summary>
        /// Lucene索引路径（完整物理路径，支持unc）
        /// </summary>
        public string IndexPath { get; private set; }

        /// <summary>
        /// 关联的搜索引擎实例
        /// </summary>
        public ISearchEngine SearchEngine
        {
            get
            {
                return searchEngine;
            }
        }

        /// <summary>
        /// 是否作为快捷搜索
        /// </summary>
        public bool AsQuickSearch { get; private set; }

        /// <summary>
        /// 是否基于Lucene实现
        /// </summary>
        public bool IsBaseOnLucene
        {
            get { return true; }
        }

        /// <summary>
        /// 处理快捷搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string QuickSearchActionUrl(string keyword)
        {
            
            //by jiangshl,UserQuickSearch()同样被用于View中，keyword参数由js组装
            return SiteUrls.Instance().UserQuickSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理全局搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string GlobalSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().UserGolbalSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().UserSearch(keyword);
        }

        #endregion


        #region 索引内容维护

        /// <summary>
        /// 重建索引
        /// </summary>  
        public void RebuildIndex()
        {
            //pageSize参数决定了每次批量取多少条数据进行索引。要注意的是，如果是分布式搜索，客户端会将这部分数据通过WCF传递给服务器端，而WCF默认的最大传输数据量是65535B，pageSize较大时这个设置显然是不够用的，WCF会报400错误；系统现在将最大传输量放宽了，但仍要注意一次不要传输过多，如遇异常，可适当调小pageSize的值
            int pageSize = 1000;
            int pageIndex = 1;
            long totalRecords = 0;
            bool isBeginning = true;
            bool isEndding = false;
            UserQuery query = new UserQuery();

            do
            {
                Dictionary<long, dynamic> userDictionary = new Dictionary<long, dynamic>();

                //分页获取用户列表
                PagingDataSet<User> users = userService.GetUsers(query, pageSize, pageIndex);
                totalRecords = users.TotalRecords;

                isEndding = (pageSize * pageIndex < totalRecords) ? false : true;
                List<long> userIds = new List<long>();

                //以用户id为key，存入字典类
                foreach (User user in users.ToList<User>())
                {
                    //构建动态对象，用于存放用户搜索相关字段
                    dynamic dUser = new ExpandoObject();
                    dUser.UserId = user.UserId;
                    dUser.UserName = user.UserName;
                    dUser.TrueName = user.TrueName;
                    dUser.NickName = user.NickName;
                    dUser.DateCreated = user.DateCreated;
                    dUser.LastActivityTime = user.LastActivityTime;

                    //初始化动态对象
                    dUser.Gender = null;
                    dUser.Birthday = null;
                    dUser.LunarBirthday = null;
                    dUser.BirthdayType = null;
                    dUser.NowAreaCode = null;
                    dUser.HomeAreaCode = null;
                    dUser.AvatarImage = null;
                    dUser.Introduction = null;
                    dUser.Schools = new List<string>();
                    dUser.CompanyNames = new List<string>();
                    dUser.TagNames = new List<string>();

                    dUser.AvatarImage = user.Avatar;
                    userDictionary.Add(user.UserId, dUser);
                    userIds.Add(user.UserId);
                }

                //根据用户id列表查询基本资料
                IEnumerable<UserProfile> userProfiles = userProfileService.GetUserProfiles(userIds);
                foreach (UserProfile userProfile in userProfiles)
                {
                    dynamic dUser = userDictionary[userProfile.UserId];
                    dUser.Gender = userProfile.Gender;
                    dUser.Birthday = userProfile.Birthday;
                    dUser.LunarBirthday = userProfile.LunarBirthday;
                    dUser.BirthdayType = userProfile.BirthdayType;
                    dUser.NowAreaCode = userProfile.NowAreaCode;
                    dUser.HomeAreaCode = userProfile.HomeAreaCode;
                    dUser.Introduction = userProfile.Introduction;
                }

                //根据用户id列表查询教育经历
                IEnumerable<long> educationExperienceIds = userProfileService.GetEducationExperienceIdsByUserIds(userIds);
                IEnumerable<EducationExperience> educationExperiences = userProfileService.GetEducationExperiences(educationExperienceIds);
                foreach (EducationExperience educationExperience in educationExperiences)
                {
                    dynamic dUser = userDictionary[educationExperience.UserId];
                    dUser.Schools.Add(educationExperience.School);
                }

                //根据用户id列表查询工作经历
                IEnumerable<long> workExperienceIds = userProfileService.GetWorkExperienceIdsByUserIds(userIds);
                IEnumerable<WorkExperience> workExperiences = userProfileService.GetWorkExperiences(workExperienceIds);
                foreach (WorkExperience workExperience in workExperiences)
                {
                    dynamic dUser = userDictionary[workExperience.UserId];
                    dUser.CompanyNames.Add(workExperience.CompanyName);
                }

                //根据用户id列表查询tags
                
                //fixed by jiangshl,修改了方法名，此处是为了性能考虑，以用户ID列表为参数，批量查询出对应的ItemInTag的ID，再批量查询出ItemInTag实体
                IEnumerable<long> itemInTagIds = tagService.GetItemInTagIdsByItemIds(userIds);
                IEnumerable<ItemInTag> itemInTags = tagService.GetItemInTags(itemInTagIds);
                foreach (ItemInTag itemInTag in itemInTags)
                {
                    dynamic dUser = userDictionary[itemInTag.ItemId];
                    dUser.TagNames.Add(itemInTag.TagName);
                }

                //重建索引
                List<dynamic> userList = new List<dynamic>();
                userList.AddRange(userDictionary.Values);

                IEnumerable<Document> docs = UserIndexDocument.Convert(userList);

                searchEngine.RebuildIndex(docs, isBeginning, isEndding);

                isBeginning = false;
                pageIndex++;
            }
            while (!isEndding);

        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="user">待添加的用户</param>
        public void Insert(User user)
        {
            Insert(new User[] { user });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="users">待添加的用户</param>
        public void Insert(IEnumerable<User> users)
        {
            List<Document> docs = new List<Document>();
            foreach (var user in users)
            {
                Document doc = UserIndexDocument.Convert(user);
                if (doc != null)
                    docs.Add(doc);
            }
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="userId">待删除的用户Id</param>
        public void Delete(long userId)
        {
            searchEngine.Delete(userId.ToString(), UserIndexDocument.UserId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="userIds">待删除的用户Id列表</param>
        public void Delete(IEnumerable<long> userIds)
        {
            foreach (long userId in userIds)
            {
                searchEngine.Delete(userId.ToString(), UserIndexDocument.UserId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="user">待更新的用户</param>
        public void Update(User user)
        {
            Document doc = UserIndexDocument.Convert(user);
            searchEngine.Update(doc, user.UserId.ToString(), UserIndexDocument.UserId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="users">待更新的用户集合</param>
        public void Update(IEnumerable<User> users)
        {
            IEnumerable<Document> docs = UserIndexDocument.Convert(users);
            List<string> userIds = new List<string>();
            foreach (User user in users)
            {
                userIds.Add(user.UserId.ToString());
            }
            searchEngine.Update(docs, userIds, UserIndexDocument.UserId);

        }

        #endregion


        #region 搜索

        /// <summary>
        /// 用户搜索
        /// </summary>
        /// <param name="userQuery">搜索条件</param>
        /// <returns>符合搜索条件的User分页集合</returns>
        public PagingDataSet<User> Search(UserFullTextQuery userQuery)
        {
            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(userQuery);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchEngine.Search(),执行搜索
            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, userQuery.PageIndex, userQuery.PageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            //解析出搜索结果中的用户ID
            List<long> userIds = new List<long>();
            foreach (Document doc in docs)
            {
                long userId = long.Parse(doc.Get(UserIndexDocument.UserId));
                userIds.Add(userId);
            }

            //根据用户ID列表批量查询用户实例
            IEnumerable<User> userList = userService.GetFullUsers(userIds);

            //根据用户ID列表批量查询UserProfile实例，利用Repository的自动缓存机制，减少页面加载时的关联数据库查询次数
            //对于UserProfile的Tag、WorkExperience、EducationExperience，目前页面列表不显示，暂不处理
            userProfileService.GetUserProfiles(userIds);

            //组装分页对象
            PagingDataSet<User> users = new PagingDataSet<User>(userList)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return users;
        }

        /// <summary>
        /// 搜索我和TA共同的内容
        /// </summary>
        /// <param name="myUserId">我的用户ID</param>
        /// <param name="taUserId">TA的用户ID</param>
        public void SearchInterested(long myUserId, long taUserId, out IEnumerable<string> sameTagNames, out IEnumerable<string> sameCompanyNames, out IEnumerable<string> sameSchoolNames)
        {
            sameTagNames = new List<string>();
            sameCompanyNames = new List<string>();
            sameSchoolNames = new List<string>();

            //无效用户ID，直接返回空列表
            if (myUserId <= 0 || taUserId <= 0)
            {
                return;
            }

            //搜索出我和TA的Document，使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            searchBuilder.WithFields(UserIndexDocument.UserId, new List<string> { myUserId.ToString(), taUserId.ToString() });
            searchBuilder.BuildQuery(out query, out filter, out sort);

            IEnumerable<Document> docs = searchEngine.Search(query, filter, sort, 2);

            //应该返回两条Document，分别对应我和TA的UserId，否则直接返回空列表
            if (docs == null || docs.Count() != 2)
            {
                return;
            }

            string[] myTagNames = docs.ElementAt(0).GetValues(UserIndexDocument.TagName);
            string[] taTagNames = docs.ElementAt(1).GetValues(UserIndexDocument.TagName);
            string[] myCompanyNames = docs.ElementAt(0).GetValues(UserIndexDocument.CompanyName);
            string[] taCompanyNames = docs.ElementAt(1).GetValues(UserIndexDocument.CompanyName);
            string[] mySchoolNames = docs.ElementAt(0).GetValues(UserIndexDocument.School);
            string[] taSchoolNames = docs.ElementAt(1).GetValues(UserIndexDocument.School);

            //比较相同的内容
            sameTagNames = myTagNames.Intersect(taTagNames);
            sameCompanyNames = myCompanyNames.Intersect(taCompanyNames);
            sameSchoolNames = mySchoolNames.Intersect(taSchoolNames);
        }

        /// <summary>
        /// 搜索“使用了相同标签”的用户
        /// </summary>
        /// <param name="userId">当前用户的ID（浏览者）</param>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="tagNameDic">存储用户ID到标签列表的映射，用于页面列表输出</param>
        /// <returns></returns>
        public PagingDataSet<User> SearchInterestedWithTags(long userId, int pageIndex, int pageSize, out Dictionary<long, IEnumerable<string>> tagNameDic)
        {
            //Dictionary，用于页面列表输出
            tagNameDic = new Dictionary<long, IEnumerable<string>>();

            //无效用户ID，直接返回空列表
            if (userId <= 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            Query query = null;
            Filter filter = null;
            Sort sort = null;

            //先搜索出当前用户的标签
            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            searchBuilder.WithField(UserIndexDocument.UserId, userId.ToString(), true);
            searchBuilder.BuildQuery(out query, out filter, out sort);

            IEnumerable<Document> docs = searchEngine.Search(query, filter, sort, 1);

            //索引中无此用户，直接返回空列表
            if (docs == null || docs.Count<Document>() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            string[] myTagNames = docs.First<Document>().GetValues(UserIndexDocument.TagName);

            //当前用户无标签，直接返回空列表
            if (myTagNames != null && myTagNames.Count() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            //查找有相同标签的用户
            //先查询当前用户关注的人(包括“悄悄关注”的用户)，此处需要调用数据库查询，因为索引中没有存储“是否悄悄关注”属性
            IEnumerable<long> myFollowedUserIds = followService.GetPortionFollowedUserIds(userId);
            //黑名单用户
            IEnumerable<long> stopUserIds = new PrivacyService().GetStopedUsers(userId).Select(n => n.Key);
            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            searchBuilder = new LuceneSearchBuilder(0);
            //搜索条件需要排除掉当前用户本身
            searchBuilder.WithPhrases(UserIndexDocument.TagName, myTagNames.ToList<string>())
                         .NotWithField(UserIndexDocument.UserId, userId.ToString()) //排除掉当前用户
                         .NotWithFields(UserIndexDocument.UserId, myFollowedUserIds.Select(n => n.ToString()))//排除掉已关注用户
                         .NotWithFields(UserIndexDocument.UserId, stopUserIds.Select(n => n.ToString()));//排除掉黑名单用户
            searchBuilder.BuildQuery(out query, out filter, out sort);

            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, pageIndex, pageSize);
            docs = searchResult.ToList<Document>();

            //如果没有使用相同标签的用户，直接返回空列表
            if (docs == null || docs.Count<Document>() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            //“使用了相同标签”的用户ID列表
            List<long> sameUserIds = new List<long>();
            foreach (Document doc in docs)
            {
                long sameUserId = long.Parse(doc.Get(UserIndexDocument.UserId));
                sameUserIds.Add(sameUserId);

                string[] tagNames = doc.GetValues(UserIndexDocument.TagName);

                //比较获取“相同的标签”
                IEnumerable<string> sameTagNames = myTagNames.Intersect<string>(tagNames);

                //加入Dictionary，用于页面列表输出
                if (!tagNameDic.ContainsKey(sameUserId))
                {
                    tagNameDic.Add(sameUserId, sameTagNames);
                }
            }

            //批量查询“使用了相同标签”的用户列表
            IEnumerable<User> sameUsers = userService.GetFullUsers(sameUserIds).Where(n => n.IsCanbeFollow == true && n.IsActivated == true && n.IsBanned == false);

            //组装分页对象
            PagingDataSet<User> users = new PagingDataSet<User>(sameUsers)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return users;
        }

        /// <summary>
        /// 搜索“供职于同一公司”的用户
        /// </summary>
        /// <param name="userId">当前用户的ID</param>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="companyNameDic">存储用户ID到标签公司名称列表的映射，用于页面列表输出</param>
        /// <returns></returns>
        public PagingDataSet<User> SearchInterestedWithCompanys(long userId, int pageIndex, int pageSize, out Dictionary<long, IEnumerable<string>> companyNameDic)
        {
            //Dictionary，用于页面列表输出
            companyNameDic = new Dictionary<long, IEnumerable<string>>();

            //无效用户ID，直接返回空列表
            if (userId <= 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            Query query = null;
            Filter filter = null;
            Sort sort = null;

            //先搜索出当前用户的公司名称
            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            searchBuilder.WithField(UserIndexDocument.UserId, userId.ToString(), true);
            searchBuilder.BuildQuery(out query, out filter, out sort);

            IEnumerable<Document> docs = searchEngine.Search(query, filter, sort, 1);

            //索引中无此用户，直接返回空列表
            if (docs == null || docs.Count<Document>() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            string[] myCompanyNames = docs.First<Document>().GetValues(UserIndexDocument.CompanyName);

            //当前用户无公司名称，直接返回空列表
            if (myCompanyNames != null && myCompanyNames.Count() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            //List<string> myCompanyNameList=myCompanyNames.Select(n=>ClauseScrubber.CompanyNameScrub(n)).ToList();

            //查找有相同公司名称的用户
            //先查询当前用户关注的人(包括“悄悄关注”的用户)，此处需要调用数据库查询，因为索引中没有存储“是否悄悄关注”属性
            IEnumerable<long> myFollowedUserIds = followService.GetPortionFollowedUserIds(userId);
            //黑名单用户
            IEnumerable<long> stopUserIds = new PrivacyService().GetStopedUsers(userId).Select(n => n.Key);
            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            searchBuilder = new LuceneSearchBuilder(1);
            //搜索条件需要排除掉当前用户本身和已关注用户
            searchBuilder.WithPhrases(UserIndexDocument.CompanyName, myCompanyNames.Select(n => ClauseScrubber.CompanyNameScrub(n)))
                         .NotWithField(UserIndexDocument.UserId, userId.ToString()) //排除掉当前用户
                         .NotWithFields(UserIndexDocument.UserId, myFollowedUserIds.Select(n => n.ToString()))//排除掉已关注用户 
                         .NotWithFields(UserIndexDocument.UserId, stopUserIds.Select(n => n.ToString()));//排除掉黑名单用户
            searchBuilder.BuildQuery(out query, out filter, out sort);

            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, pageIndex, pageSize);
            docs = searchResult.ToList<Document>();

            //如果没有使用相同公司名称的用户，直接返回空列表
            if (docs == null || docs.Count<Document>() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            //“相同公司名称”的用户ID列表
            List<long> sameUserIds = new List<long>();
            foreach (Document doc in docs)
            {
                //比较获取“相同的公司名称”
                List<string> sameCompanyNames = new List<string>();
                string[] companyNames = doc.GetValues(UserIndexDocument.CompanyName);
                foreach (string myCompanyName in myCompanyNames)
                {
                    foreach (string companyName in companyNames)
                    {
                        if (companyName.Equals(myCompanyName) || companyName.Contains(myCompanyName))
                        {
                            sameCompanyNames.Add(myCompanyName);
                            break;
                        }
                    }
                }

                long sameUserId = long.Parse(doc.Get(UserIndexDocument.UserId));
                sameUserIds.Add(sameUserId);

                //加入Dictionary，用于页面列表输出
                if (!companyNameDic.ContainsKey(sameUserId))
                {
                    companyNameDic.Add(sameUserId, sameCompanyNames);
                }
            }

            //批量查询“相同公司名称”的用户列表
            IEnumerable<User> sameUsers = userService.GetFullUsers(sameUserIds).Where(n => n.IsCanbeFollow == true && n.IsActivated == true && n.IsBanned == false);

            //组装分页对象
            PagingDataSet<User> users = new PagingDataSet<User>(sameUsers)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return users;
        }

        /// <summary>
        /// 搜索“毕业于同一学校”的用户/// </summary>
        /// <param name="userId">当前用户的ID</param>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="schoolNameDic">存储用户ID到学校名称列表的映射，用于页面列表输出</param>
        /// <returns></returns>
        public PagingDataSet<User> SearchInterestedWithSchools(long userId, int pageIndex, int pageSize, out Dictionary<long, IEnumerable<string>> schoolNameDic)
        {
            //Dictionary，用于页面列表输出
            schoolNameDic = new Dictionary<long, IEnumerable<string>>();

            //无效用户ID，直接返回空列表
            if (userId <= 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            Query query = null;
            Filter filter = null;
            Sort sort = null;

            //先搜索出当前用户的学校名称
            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            searchBuilder.WithField(UserIndexDocument.UserId, userId.ToString(), true);
            searchBuilder.BuildQuery(out query, out filter, out sort);

            IEnumerable<Document> docs = searchEngine.Search(query, filter, sort, 1);

            //索引中无此用户，直接返回空列表
            if (docs == null || docs.Count() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            string[] mySchoolNames = docs.First<Document>().GetValues(UserIndexDocument.School);

            //当前用户无学校，直接返回空列表
            if (mySchoolNames != null && mySchoolNames.Count() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            //查找有相同学校名称的用户
            //先查询当前用户关注的人(包括“悄悄关注”的用户)，此处需要调用数据库查询，因为索引中没有存储“是否悄悄关注”属性
            IEnumerable<long> myFollowedUserIds = followService.GetPortionFollowedUserIds(userId);
            //黑名单用户
            IEnumerable<long> stopUserIds = new PrivacyService().GetStopedUsers(userId).Select(n => n.Key);
            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            searchBuilder = new LuceneSearchBuilder(1);
            //搜索条件需要排除掉当前用户本身
            searchBuilder.WithPhrases(UserIndexDocument.School, mySchoolNames.ToList())
                         .NotWithField(UserIndexDocument.UserId, userId.ToString()) //排除掉当前用户
                         .NotWithFields(UserIndexDocument.UserId, myFollowedUserIds.Select(n => n.ToString()))//排除掉已关注用户  
                         .NotWithFields(UserIndexDocument.UserId, stopUserIds.Select(n => n.ToString()));//排除掉黑名单用户
            searchBuilder.BuildQuery(out query, out filter, out sort);

            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, pageIndex, pageSize);
            docs = searchResult.ToList<Document>();

            //如果没有使用相同学校名称的用户，直接返回空列表
            if (docs == null || docs.Count() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            //“相同学校名称”的用户ID列表
            List<long> sameUserIds = new List<long>();
            foreach (Document doc in docs)
            {
                //比较获取“相同的学校名称”                
                List<string> sameSchoolNames = new List<string>();
                string[] schoolNames = doc.GetValues(UserIndexDocument.School);
                foreach (string mySchoolName in mySchoolNames)
                {
                    foreach (string schoolName in schoolNames)
                    {
                        if (schoolName.Equals(mySchoolName) || schoolName.Contains(mySchoolName))
                        {
                            sameSchoolNames.Add(mySchoolName);
                            break;
                        }
                    }
                }

                long sameUserId = long.Parse(doc.Get(UserIndexDocument.UserId));
                sameUserIds.Add(sameUserId);

                //加入Dictionary，用于页面列表输出
                if (!schoolNameDic.ContainsKey(sameUserId))
                {
                    schoolNameDic.Add(sameUserId, sameSchoolNames);
                }
            }

            //批量查询“相同学校名称”的用户列表
            IEnumerable<User> sameUsers = userService.GetFullUsers(sameUserIds).Where(n => n.IsCanbeFollow == true && n.IsActivated == true && n.IsBanned == false);

            //组装分页对象
            PagingDataSet<User> users = new PagingDataSet<User>(sameUsers)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return users;
        }


        /// <summary>
        /// 自动完成搜索
        /// </summary>
        /// <param name="userQuery"></param>
        /// <returns></returns>
        public IEnumerable<User> AutoCompleteSearch(UserFullTextQuery userQuery)
        {

            if (string.IsNullOrWhiteSpace(userQuery.Keyword))
            {
                return new List<User>();
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(userQuery);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchService.Search(),执行搜索
            IEnumerable<Document> docs = searchEngine.Search(query, filter, sort, userQuery.PageSize);

            //解析出搜索结果中的用户ID
            List<long> userIds = new List<long>();
            foreach (Document doc in docs)
            {
                long userId = long.Parse(doc.Get(UserIndexDocument.UserId));
                userIds.Add(userId);
            }

            //根据用户ID列表批量查询用户实例
            IEnumerable<User> users = userService.GetFullUsers(userIds);

            //根据用户ID列表批量查询用户相关实例，利用Repository的自动缓存机制，减少页面加载时的关联数据库查询次数
            IEnumerable<UserProfile> userProfiles = userProfileService.GetUserProfiles(userIds);

            return users;
        }

        /// <summary>
        /// 根据用户搜索查询条件构建Lucene查询条件
        /// </summary>
        /// <param name="userQuery"></param>
        /// <returns></returns>
        private LuceneSearchBuilder BuildLuceneSearchBuilder(UserFullTextQuery userQuery)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();

            //搜索词匹配范围
            Dictionary<string, BoostLevel> fieldNameAndBoosts = new Dictionary<string, BoostLevel>();
            switch (userQuery.SearchRange)
            {
                case UserSearchRange.NAME:
                    fieldNameAndBoosts.Add(UserIndexDocument.TrueName, BoostLevel.Hight);
                    fieldNameAndBoosts.Add(UserIndexDocument.PinyinName, BoostLevel.Medium);
                    fieldNameAndBoosts.Add(UserIndexDocument.NickName, BoostLevel.Medium);
                    fieldNameAndBoosts.Add(UserIndexDocument.UserName, BoostLevel.Low);
                    fieldNameAndBoosts.Add(UserIndexDocument.ShortPinyinName, BoostLevel.Low);

                    searchBuilder.WithPhrases(fieldNameAndBoosts, userQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                    break;

                case UserSearchRange.TAG:
                    searchBuilder.WithPhrase(UserIndexDocument.TagName, userQuery.Keyword, BoostLevel.Hight, false);
                    break;

                case UserSearchRange.SCHOOL:
                    searchBuilder.WithPhrase(UserIndexDocument.School, userQuery.Keyword, BoostLevel.Hight, false);
                    break;

                case UserSearchRange.COMPANY:
                    searchBuilder.WithPhrase(UserIndexDocument.CompanyName, userQuery.Keyword, BoostLevel.Hight, false);
                    break;

                case UserSearchRange.NOWAREACODE:
                    if (!string.IsNullOrEmpty(userQuery.Keyword))
                    {
                        fieldNameAndBoosts.Add(UserIndexDocument.TrueName, BoostLevel.Hight);
                        fieldNameAndBoosts.Add(UserIndexDocument.PinyinName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.NickName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.UserName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.TagName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.School, BoostLevel.Low);
                        fieldNameAndBoosts.Add(UserIndexDocument.CompanyName, BoostLevel.Low);
                        fieldNameAndBoosts.Add(UserIndexDocument.ShortPinyinName, BoostLevel.Low);

                        searchBuilder.WithPhrases(fieldNameAndBoosts, userQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                    }
                    if (!string.IsNullOrEmpty(userQuery.NowAreaCode))
                        searchBuilder.WithField(UserIndexDocument.NowAreaCode, userQuery.NowAreaCode.TrimEnd('0'), false, BoostLevel.Hight, false);
                    else
                        searchBuilder.WithFields(UserIndexDocument.NowAreaCode, new string[] { "1", "2", "3" }, false, BoostLevel.Hight, false);
                    break;

                case UserSearchRange.HOMEAREACODE:
                    if (!string.IsNullOrEmpty(userQuery.Keyword))
                    {
                        fieldNameAndBoosts.Add(UserIndexDocument.TrueName, BoostLevel.Hight);
                        fieldNameAndBoosts.Add(UserIndexDocument.PinyinName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.NickName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.UserName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.TagName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.School, BoostLevel.Low);
                        fieldNameAndBoosts.Add(UserIndexDocument.CompanyName, BoostLevel.Low);
                        fieldNameAndBoosts.Add(UserIndexDocument.ShortPinyinName, BoostLevel.Low);

                        searchBuilder.WithPhrases(fieldNameAndBoosts, userQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                    }
                    if (!string.IsNullOrEmpty(userQuery.HomeAreaCode))
                        searchBuilder.WithField(UserIndexDocument.HomeAreaCode, userQuery.HomeAreaCode.TrimEnd('0'), false, BoostLevel.Hight, false);
                    else
                        searchBuilder.WithFields(UserIndexDocument.HomeAreaCode, new string[] {"1","2","3"}, false, BoostLevel.Hight, false);
                    break;

                case UserSearchRange.Gender:
                    if (!string.IsNullOrEmpty(userQuery.Keyword))
                    {
                        fieldNameAndBoosts.Add(UserIndexDocument.TrueName, BoostLevel.Hight);
                        fieldNameAndBoosts.Add(UserIndexDocument.PinyinName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.NickName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.UserName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.TagName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.School, BoostLevel.Low);
                        fieldNameAndBoosts.Add(UserIndexDocument.CompanyName, BoostLevel.Low);
                        fieldNameAndBoosts.Add(UserIndexDocument.ShortPinyinName, BoostLevel.Low);

                        searchBuilder.WithPhrases(fieldNameAndBoosts, userQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                    }
                    searchBuilder.WithField(UserIndexDocument.Gender, ((int)userQuery.Gender).ToString(), false, BoostLevel.Hight, false);
                    break;

                case UserSearchRange.Age:
                    if (!string.IsNullOrEmpty(userQuery.Keyword))
                    {
                        fieldNameAndBoosts.Add(UserIndexDocument.TrueName, BoostLevel.Hight);
                        fieldNameAndBoosts.Add(UserIndexDocument.PinyinName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.NickName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.UserName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.TagName, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(UserIndexDocument.School, BoostLevel.Low);
                        fieldNameAndBoosts.Add(UserIndexDocument.CompanyName, BoostLevel.Low);
                        fieldNameAndBoosts.Add(UserIndexDocument.ShortPinyinName, BoostLevel.Low);

                        searchBuilder.WithPhrases(fieldNameAndBoosts, userQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                    }
                    if (userQuery.AgeMin > userQuery.AgeMax)
                    {
                        int temp = userQuery.AgeMin;
                        userQuery.AgeMin = userQuery.AgeMax;
                        userQuery.AgeMax = temp;
                    }
                    if (userQuery.AgeMin < 0)
                    {
                        userQuery.AgeMin = 0;
                    }
                    if (userQuery.AgeMax > 200)
                    {
                        userQuery.AgeMin = 200;
                    }

                    string yearMax = (DateTime.Now.Year - userQuery.AgeMin).ToString().PadLeft(3, '0');
                    string yearMin = (DateTime.Now.Year - userQuery.AgeMax).ToString().PadLeft(3, '0');

                    searchBuilder.WithinRange(UserIndexDocument.BirthdayYear, yearMin, yearMax,false);
                    break;

                default:
                    fieldNameAndBoosts.Add(UserIndexDocument.TrueName, BoostLevel.Hight);
                    fieldNameAndBoosts.Add(UserIndexDocument.PinyinName, BoostLevel.Medium);
                    fieldNameAndBoosts.Add(UserIndexDocument.NickName, BoostLevel.Medium);
                    fieldNameAndBoosts.Add(UserIndexDocument.UserName, BoostLevel.Medium);
                    fieldNameAndBoosts.Add(UserIndexDocument.TagName, BoostLevel.Medium);
                    fieldNameAndBoosts.Add(UserIndexDocument.School, BoostLevel.Low);
                    fieldNameAndBoosts.Add(UserIndexDocument.CompanyName, BoostLevel.Low);
                    fieldNameAndBoosts.Add(UserIndexDocument.ShortPinyinName, BoostLevel.Low);

                    searchBuilder.WithPhrases(fieldNameAndBoosts, userQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                    break;

            }

            //所在地区过滤
            if (!string.IsNullOrWhiteSpace(userQuery.NowAreaCode) && userQuery.SearchRange != UserSearchRange.NOWAREACODE)
            {
                searchBuilder.WithField(UserIndexDocument.NowAreaCode, userQuery.NowAreaCode.TrimEnd('0'), false, BoostLevel.Hight, true);
            }

            //家乡地区过滤
            if (!string.IsNullOrWhiteSpace(userQuery.HomeAreaCode) && userQuery.SearchRange != UserSearchRange.HOMEAREACODE)
            {
                searchBuilder.WithField(UserIndexDocument.HomeAreaCode, userQuery.HomeAreaCode.TrimEnd('0'), false, BoostLevel.Hight, true);
            }

            //性别过滤
            if (userQuery.Gender != GenderType.NotSet)
            {
                searchBuilder.WithField(UserIndexDocument.Gender, ((int)userQuery.Gender).ToString(), true, BoostLevel.Hight, true);
            }

            //年龄过滤
            if (userQuery.AgeMin > 0 || userQuery.AgeMax > 0)
            {
                if (userQuery.AgeMin > userQuery.AgeMax)
                {
                    int temp = userQuery.AgeMin;
                    userQuery.AgeMin = userQuery.AgeMax;
                    userQuery.AgeMax = temp;
                }
                if (userQuery.AgeMin < 0)
                {
                    userQuery.AgeMin = 0;
                }
                if (userQuery.AgeMax > 200)
                {
                    userQuery.AgeMin = 200;
                }

                string yearMax = (DateTime.Now.Year - userQuery.AgeMin).ToString().PadLeft(3, '0');
                string yearMin = (DateTime.Now.Year - userQuery.AgeMax).ToString().PadLeft(3, '0');

                searchBuilder.WithinRange(UserIndexDocument.BirthdayYear, yearMin, yearMax, true);
            }

            //按最后活动时间倒排序
            searchBuilder.SortByString(UserIndexDocument.LastActivityTime, true);

            return searchBuilder;
        }

        #endregion

    }
}
