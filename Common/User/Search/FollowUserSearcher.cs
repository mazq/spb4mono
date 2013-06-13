//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spacebuilder.Search;
using Tunynet.Search;
using Tunynet.Common;
using Tunynet;
using Lucene.Net.Documents;
using Lucene.Net.Search;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 关注用户搜索器
    /// </summary>
    public class FollowUserSearcher : ISearcher
    {
        private FollowService followService = new FollowService();
        private IUserService userService = DIContainer.Resolve<IUserService>();

        #region 索引字段

        public static readonly string UserId = "UserId";
        public static readonly string FollowedUserIds = "FollowedUserIds";

        #endregion

        #region 搜索器属性

        private ISearchEngine searchEngine;
        public static string CODE = "FollowUserSearcher";

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
            get { return false; }
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 处理全局搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string GlobalSearchActionUrl(string keyword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 关联的搜索引擎实例
        /// </summary>
        public ISearchEngine SearchEngine
        {
            get
            {
                return searchEngine;
            }
            set
            {
                searchEngine = value;
            }
        }

        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public FollowUserSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = Tunynet.Utilities.WebUtility.GetPhysicalFilePath(indexPath);
            this.AsQuickSearch = asQuickSearch;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="searchEngine">搜索引擎</param>
        public FollowUserSearcher(ISearchEngine searchEngine)
        {
            this.searchEngine = searchEngine;
        }

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

            long followedUserId = 0;
            Document doc = null;
            do
            {
                //分页获取粉丝列表
                PagingDataSet<User> users = userService.GetUsers(query, pageSize, pageIndex);

                totalRecords = users.TotalRecords;
                IEnumerable<long> followerUserIds = users.Select(n => n.UserId);

                isEndding = (pageSize * pageIndex < totalRecords) ? false : true;

                //根据粉丝列表批量查询关注用户列表
                IEnumerable<FollowEntity> followedUsers = followService.GetFollowedUsers(followerUserIds);

                //循环关注用户列表（已按照UserId排序），对每个UserId建立索引
                List<Document> docs = new List<Document>();
                foreach (FollowEntity followedUser in followedUsers)
                {
                    //判断当前记录的被关注用户的ID是否与上一条的被关注用户的ID相同。
                    //如果相同，说明属于同一个用户的关注对象
                    if (followedUser.UserId == followedUserId)
                    {
                        doc.Add(new Field(FollowedUserIds, followedUser.FollowedUserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }
                    else
                    {
                        
                        //fixed by jiangshl 

                        //对于下一个用户，需要重新构建Document对象
                        doc = new Document();
                        doc.Add(new Field(UserId, followedUser.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        doc.Add(new Field(FollowedUserIds, followedUser.FollowedUserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        docs.Add(doc);

                        followedUserId = followedUser.UserId;
                    }
                }

                searchEngine.RebuildIndex(docs, isBeginning, isEndding);

                isBeginning = false;
                pageIndex++;
            }
            while (!isEndding);

        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="userId">待添加的用户ID</param>
        public void Insert(long userId)
        {
            Insert(new long[] { userId });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="userIds">待添加的用户ID列表</param>
        public void Insert(IEnumerable<long> userIds)
        {
            if (userIds == null || userIds.Count() == 0)
            {
                return;
            }

            //根据粉丝列表批量查询关注用户列表
            IEnumerable<FollowEntity> followedUsers = followService.GetFollowedUsers(userIds);
            if (followedUsers == null || followedUsers.Count() == 0)
            {
                return;
            }

            //循环关注用户列表（已按照UserId排序），对每个UserId建立索引
            List<Document> docs = new List<Document>();
            long followedUserId = 0;
            Document doc = null;
            foreach (FollowEntity followedUser in followedUsers)
            {
                //判断当前记录的被关注用户的ID是否与上一条的被关注用户的ID相同。
                //如果相同，说明属于同一个用户的关注对象
                if (followedUser.UserId == followedUserId)
                {
                    doc.Add(new Field(FollowedUserIds, followedUser.FollowedUserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                else
                {
                    //如果不相同，说明开始处理下一个用户的关注对象，需要重新构建Document对象
                    doc = new Document();
                    doc.Add(new Field(UserId, followedUser.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field(FollowedUserIds, followedUser.FollowedUserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    docs.Add(doc);
                }

                followedUserId = followedUser.UserId;
            }

            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="userId">待删除的用户Id</param>
        public void Delete(long userId)
        {
            searchEngine.Delete(userId.ToString(), UserId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="userIds">待删除的用户Id列表</param>
        public void Delete(IEnumerable<long> userIds)
        {
            foreach (long userId in userIds)
            {
                Delete(userId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="userId">待更新的用户ID</param>
        public void Update(long userId)
        {
            Delete(userId);
            Insert(userId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="userIds">待更新的用户ID列表</param>
        public void Update(IEnumerable<long> userIds)
        {
            Delete(userIds);
            Insert(userIds);
        }

        #endregion

        #region 搜索

        /// <summary>
        /// 搜索共同关注的人
        /// </summary>
        /// <param name="userId">粉丝的用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="followedUserIdDic">每个相同关注用户中共同关注的用户ID列表</param>
        /// <param name="followedUserDic">每个共同关注的用户的ID与User的映射</param>
        /// <returns>符合搜索条件的User分页集合</returns>
        public PagingDataSet<User> SearchInterestedWithFollows(long userId, int pageIndex, int pageSize, out Dictionary<long, IEnumerable<long>> followedUserIdDic, out Dictionary<long, User> followedUserDic)
        {
            followedUserIdDic = new Dictionary<long, IEnumerable<long>>();
            followedUserDic = new Dictionary<long, User>();

            if (userId <= 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            //先查询当前用户关注的人(包括“悄悄关注”的用户)，此处需要调用数据库查询，因为索引中没有存储“是否悄悄关注”属性
            IEnumerable<long> myFollowedUserIds = followService.GetPortionFollowedUserIds(userId);
            if (myFollowedUserIds != null && myFollowedUserIds.Count() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            //黑名单用户
            IEnumerable<long> stopUserIds = new PrivacyService().GetStopedUsers(userId).Select(n => n.Key);

            //搜索“我”关注的人中包含“共同关注的人”的用户
            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            searchBuilder.WithFields(FollowedUserIds, myFollowedUserIds.Select(n => n.ToString()), true)
                         .NotWithField(UserId, userId.ToString())//排除掉当前用户
                         .NotWithFields(UserId, myFollowedUserIds.Select(n => n.ToString()))//排除掉已关注用户
                         .NotWithFields(UserIndexDocument.UserId, stopUserIds.Select(n => n.ToString()));//排除掉黑名单用户
            searchBuilder.BuildQuery(out query, out filter, out sort);

            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, pageIndex, pageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            if (docs == null || docs.Count() == 0)
            {
                return new PagingDataSet<User>(new List<User>());
            }

            List<long> sameFollowedUserIdList = new List<long>();

            //解析出搜索结果中的用户ID
            List<long> followerUserIds = new List<long>();
            foreach (Document doc in docs)
            {
                long followerUserId = long.Parse(doc.Get(UserId));
                followerUserIds.Add(followerUserId);

                //“我”关注的人关注的人
                string[] followedUserIds = doc.GetValues(FollowedUserIds);

                //比较获取“共同关注的人”
                IEnumerable<long> sameFollowedUserIds = myFollowedUserIds.Intersect<long>(followedUserIds.Select(n => Convert.ToInt64(n)));
                if (!followedUserIdDic.ContainsKey(followerUserId))
                {
                    followedUserIdDic.Add(followerUserId, sameFollowedUserIds);
                }

                sameFollowedUserIdList.AddRange(sameFollowedUserIds);
            }

            //批量查询“共同关注的用户”列表
            IEnumerable<User> followerUserList = userService.GetFullUsers(followerUserIds).Where(n => n.IsCanbeFollow == true && n.IsActivated == true && n.IsBanned == false);

            //组装分页对象
            PagingDataSet<User> users = new PagingDataSet<User>(followerUserList)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            //批量查询“共同关注的用户”关注的“共同关注用户”列表
            IEnumerable<User> sameFollowedUserList = userService.GetFullUsers(sameFollowedUserIdList.Distinct());
            followedUserDic = sameFollowedUserList.ToDictionary(n => n.UserId);

            return users;
        }

        /// <summary>
        /// 搜索我和TA共同关注的用户
        /// </summary>
        /// <param name="myUserId">我的用户ID</param>
        /// <param name="taUserId">TA的用户列表</param>
        /// <returns>我和TA共同关注的用户列表</returns>
        public IEnumerable<User> SearchInterestedWithFollows(long myUserId, long taUserId)
        {
            //无效用户ID，直接返回空列表
            if (myUserId <= 0 || taUserId <= 0)
            {
                return new List<User>();
            }

            //搜索出我和TA的Document，使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            searchBuilder.WithFields(UserId, new List<string> { myUserId.ToString(), taUserId.ToString() });
            searchBuilder.BuildQuery(out query, out filter, out sort);

            IEnumerable<Document> docs = searchEngine.Search(query, filter, sort, 2);

            //应该返回两条Document，分别对应我和TA的UserId，否则直接返回空列表
            if (docs == null || docs.Count() != 2)
            {
                return new List<User>();
            }

            string[] myFollowedUserIds = docs.ElementAt(0).GetValues(FollowedUserIds);
            string[] taFollowedUserIds = docs.ElementAt(1).GetValues(FollowedUserIds);

            //比较相同的关注用户
            IEnumerable<string> sameFollowedUserIds = myFollowedUserIds.Intersect(taFollowedUserIds);

            //批量查询“共同关注的用户”列表
            IEnumerable<User> sameFollowedUsers = userService.GetFullUsers(sameFollowedUserIds.Select(n => Convert.ToInt64(n)));

            return sameFollowedUsers;
        }

        #endregion


        public string WaterMark
        {
            get { throw new NotImplementedException(); }
        }

    }
}
