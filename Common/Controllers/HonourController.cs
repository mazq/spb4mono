//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;
using Tunynet.UI;
using System.Linq;
using System.Collections.Generic;
using Tunynet.Common.Configuration;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户空间Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.UserSpace, IsApplication = false)]
    [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
    public class HonourController : Controller
    {
        private IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
        IUserService userService = DIContainer.Resolve<IUserService>();
        PointService pointService = new PointService();
        UserRankService userRankService = new UserRankService();
        IPointSettingsManager pointSettingsManger = DIContainer.Resolve<IPointSettingsManager>();

        /// <summary>
        /// 我的等级
        /// </summary>
        [HttpGet]
        public ActionResult MyRank(string spaceKey)
        {
            IUser user = userService.GetUser(spaceKey);
            PointSettings pointSettings = pointSettingsManger.Get();

            pageResourceManager.InsertTitlePart("我的等级");

            int totalPoints = pointSettings.CalculateIntegratedPoint(user.ExperiencePoints, user.ReputationPoints);

            SortedList<int, UserRank> userRanks = userRankService.GetAll();
            UserRank userRank = userRankService.Get(user.Rank);
            if (userRank != null)
                ViewData["userRankName"] = userRank.RankName;


            ViewData["userRanks"] = userRanks;
            ViewData["totalPoints"] = totalPoints;
            if (user.Rank + 1 <= userRankService.GetAll().Count())
            {
                ViewData["nextRankName"] = userRankService.Get(user.Rank + 1).RankName;
                ViewData["leftUpgradeExperiencePoints"] = userRankService.Get(user.Rank + 1).PointLower - totalPoints;
            }
            else
                ViewData["leftUpgradeExperiencePoints"] = 0;

            #region 计算进度条百分比

            List<UserRank> ranks = userRanks.Values.ToList();
            int a = (userRanks.Count() - 2) / 3;
            int rank = user.Rank;
            double leftPoints = 0;

            if (rank >= 1 && rank < ranks.ElementAt(a).Rank)
            {
                leftPoints = totalPoints / (double)ranks.ElementAt(a).PointLower / 5;
            }
            else if (rank >= ranks.ElementAt(a).Rank && rank < ranks.ElementAt(2 * a).Rank)
            {
                leftPoints = (totalPoints - ranks.ElementAt(a).PointLower) / (double)(ranks.ElementAt(2 * a).PointLower - ranks.ElementAt(a).PointLower) / 5 + 0.2;
            }

            else if (rank >= ranks.ElementAt(2 * a).Rank && rank < ranks.ElementAt(3 * a).Rank)
            {
                leftPoints = (totalPoints - ranks.ElementAt(2 * a).PointLower) / (double)(ranks.ElementAt(3 * a).PointLower - ranks.ElementAt(2 * a).PointLower) / 5 + 0.4;
            }

            else if (rank >= ranks.ElementAt(3 * a).Rank && rank < userRanks.ToArray()[userRanks.Count() - 1].Value.Rank)
            {
                leftPoints = (totalPoints - ranks.ElementAt(3 * a).PointLower) / (double)(userRanks.ToArray()[userRanks.Count() - 1].Value.PointLower - ranks.ElementAt(3 * a).PointLower) / 5 + 0.6;
            }
            else
            {
                leftPoints = totalPoints  / double.MaxValue + 0.8;
            }
            ViewData["leftPoints"] = leftPoints;
            #endregion
            
            
            return View(user);
        }
        /// <summary>
        /// 积分规则列表
        /// </summary>
        [HttpGet]
        public ActionResult _ListPointItems(string spaceKey)
        {
            PointSettings pointSettings = pointSettingsManger.Get();
            
            IEnumerable<PointItem> pointItems = pointService.GetPointItemsOfIncome();

            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["traPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("TradePoints")).CategoryName;
            ViewData["expPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ExperiencePoints")).CategoryName;
            ViewData["prePoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;
            ViewData["pointCategories"] = pointCategories;
            ViewData["pointRule"] = pointSettings.UserIntegratedPointRuleText;
            

            return View("_ListPointItems", pointItems);
        }

        /// <summary>
        /// 我的积分记录列表
        /// </summary>
        [HttpGet]
        public ActionResult ListPointRecords(string spaceKey, int? pageIndex ,int pageSize=20)
        {

            IUser user = userService.GetUser(spaceKey);
            if (user == null)
                return HttpNotFound();

            pageResourceManager.InsertTitlePart("我的积分记录");
            PagingDataSet<PointRecord> pointRecords = pointService.GetPointRecords(user.UserId, null, "", null, null,pageSize, pageIndex ?? 1);
            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["traPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("TradePoints")).CategoryName;
            ViewData["expPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ExperiencePoints")).CategoryName;
            ViewData["prePoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;

            return View(pointRecords);
        }

        /// <summary>
        /// 左侧导航
        /// </summary>
        [HttpGet]
        public ActionResult _HonourMenus()
        {
            return View();
        }
        /// <summary>
        /// 左侧积分显示
        /// </summary>
        [HttpGet]
        public ActionResult _PointMenus(string spaceKey)
        {
            PointSettings pointSettings = pointSettingsManger.Get();
            IUser user = userService.GetUser(spaceKey);
            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();
            ViewData["totalPoints"] = pointSettings.CalculateIntegratedPoint(user.ExperiencePoints, user.ReputationPoints);
          
            ViewData["traPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("TradePoints"));
            ViewData["expPoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ExperiencePoints"));
            ViewData["prePoint"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints"));
            return View("_PointMenus", user);
        }

    }
    

    #region HonourMenus
    /// <summary>
    /// HonourMenus
    /// </summary>
    public enum HonourMenu
    {
        /// <summary>
        /// 我的等级
        /// </summary>
        MyRank = 0,

        /// <summary>
        /// 积分记录
        /// </summary>
        PointRecord = 1,


    }
    #endregion
}