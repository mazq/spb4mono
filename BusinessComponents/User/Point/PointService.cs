//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-07-04</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-07-04" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;
using Tunynet.Repositories;
using Tunynet.Common.Configuration;
using Tunynet.Logging;
using Tunynet.Events;
using Tunynet.Globalization;

namespace Tunynet.Common
{
    /// <summary>
    /// 积分业务逻辑类
    /// </summary>
    public class PointService
    {
        private IRepository<PointCategory> pointCategoryRepository;
        private IPointItemRepository pointItemRepository;
        private IPointRecordRepository pointRecordRepository;
        private IPointStatisticRepository pointStatisticRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public PointService()
            : this(new PointCategoryRepository(), new PointItemRepository(),
                   new PointRecordRepository(), new PointStatisticRepository())
        {

        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="pointCategoryRepository">积分分类仓储</param>
        /// <param name="pointItemRepository"></param>
        /// <param name="pointRecordRepository"></param>
        /// <param name="pointStatisticRepository"></param>
        public PointService(IRepository<PointCategory> pointCategoryRepository, IPointItemRepository pointItemRepository,
                            IPointRecordRepository pointRecordRepository, IPointStatisticRepository pointStatisticRepository)
        {
            this.pointCategoryRepository = pointCategoryRepository;
            this.pointItemRepository = pointItemRepository;
            this.pointRecordRepository = pointRecordRepository;
            this.pointStatisticRepository = pointStatisticRepository;
        }


        //关于缓存期限：
        //1、PointItem实体、列表 使用CachingExpirationType.RelativelyStable
        //2、PointCategory实体、列表 使用CachingExpirationType.RelativelyStable
        //3、PointRecord实体、列表 使用正常的缓存策略
        //4、积分记录的所有积分类型都是0，则不创建

        #region 积分变更及记录

        /// <summary>
        /// 依据规则增减积分
        /// </summary>
        /// <param name="userId">增减积分的UserId</param>
        /// <param name="pointItemKey">积分项目标识</param>
        /// <param name="description">积分记录描述</param>
        public void GenerateByRole(long userId, string pointItemKey, string description)
        {
            //1、依据pointItemKey查找积分项目，如果未找到则中断执行；
            PointItem pointItem = GetPointItem(pointItemKey);
            if (pointItem == null)
                return;
            if (pointItem.ExperiencePoints == 0 && pointItem.ReputationPoints == 0 && pointItem.TradePoints == 0)
                return;
            //2、检查用户当日各类积分是否达到限额，如果达到限额则不加积分，如果未达到则更新当日积分限额           
            Dictionary<string, int> dictionary = pointStatisticRepository.UpdateStatistic(userId, GetPointCategory2PointsDictionary(pointItem));
            //如果用户当日各类积分都超出限额，则不产生积分
            if (dictionary.Count(n => n.Value != 0) == 0)
                return;

            //3、按照pointItemKey对应的积分项目，生成积分记录，并对用户积分额进行增减；

            int experiencePoints = dictionary[PointCategoryKeys.Instance().ExperiencePoints()];
            int reputationPoints = dictionary[PointCategoryKeys.Instance().ReputationPoints()];
            int tradePoints = dictionary[PointCategoryKeys.Instance().TradePoints()];
            int tradePoints2 = 0;
            int tradePoints3 = 0;
            int tradePoints4 = 0;
            if (dictionary.ContainsKey("TradePoints2"))
            {
                tradePoints2 = dictionary["TradePoints2"];
            }
            if (dictionary.ContainsKey("TradePoints3"))
            {
                tradePoints3 = dictionary["TradePoints3"];
            }
            if (dictionary.ContainsKey("TradePoints4"))
            {
                tradePoints4 = dictionary["TradePoints4"];
            }

            PointRecord pointRecord = new PointRecord(userId, pointItem.ItemName, description, experiencePoints, reputationPoints, tradePoints);
            pointRecord.TradePoints2 = tradePoints2;
            pointRecord.TradePoints3 = tradePoints3;
            pointRecord.TradePoints4 = tradePoints4;
            pointRecordRepository.Insert(pointRecord);
            IUserService userService = DIContainer.Resolve<IUserService>();
            userService.ChangePoints(userId, experiencePoints, reputationPoints, tradePoints, tradePoints2, tradePoints3, tradePoints4);

            CountService countService = new CountService(TenantTypeIds.Instance().User());
            countService.ChangeCount(CountTypes.Instance().ReputationPointsCounts(), userId, userId, pointRecord.ReputationPoints);
        }

        /// <summary>
        /// 积分交易
        /// </summary>
        /// <param name="payerUserId">支付积分人UserId</param>
        /// <param name="payeeUserId">接收积分人UserId</param>
        /// <param name="points">交易积分额</param>
        /// <param name="description">交易描述</param>
        /// <param name="isImmediate">是否即时交易</param>
        public void Trade(long payerUserId, long payeeUserId, int points, string description, bool isImmediate)
        {
            //如果是即时交易，从支付方从交易积分扣除，否则从冻结的交易积分扣除（不足时抛出异常）

            if (points <= 0)
                return;
            //1、首先检查payerUserId是否可以支付积分交易额，如果余额不足抛出异常
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser payer = userService.GetUser(payerUserId);
            if (payer == null)
                throw new ExceptionFacade(string.Format("用户“{0}”不存在或已被删除", payerUserId));

            PointCategory pointCategory = GetPointCategory(PointCategoryKeys.Instance().TradePoints());
            if (pointCategory == null)
                return;

            if (isImmediate && payer.TradePoints < points)
            {
                throw new ExceptionFacade(string.Format("积分余额不足，仅有{0}{2}{3}，不够支付{1}{2}{3}", payer.TradePoints, points, pointCategory.Unit, pointCategory.CategoryName));
            }

            if (!isImmediate && payer.FrozenTradePoints < points)
            {
                throw new ExceptionFacade(string.Format("冻结积分余额不足，仅有{0}{2}{3}，不够支付{1}{2}{3}", payer.FrozenTradePoints, points, pointCategory.Unit, pointCategory.CategoryName));
            }

            IUser payee = userService.GetUser(payeeUserId);
            if (payee == null)
                throw new ExceptionFacade(string.Format("用户“{0}”不存在或已被删除", payeeUserId));

            //2、检查是否需要缴纳交易税，如果需要，则创建系统积分记录，变更系统积分总额
            IPointSettingsManager pointSettingsManager = DIContainer.Resolve<IPointSettingsManager>();
            PointSettings pointSettings = pointSettingsManager.Get();
            int realPoints = points;
            if (pointSettings.TransactionTax > 0 && pointSettings.TransactionTax < 100)
            {
                realPoints = points * (100 - pointSettings.TransactionTax) / 100;
                int taxPoints = points - realPoints;
                if (taxPoints > 0)
                {
                    //done:zhengw,by mazq  交易税 应该放入资源文件（该类描述都应该允许管理员修改）
                    //zhengw回复：已修改
                    PointRecord pointRecord = new PointRecord(0, ResourceAccessor.GetString("Common_TransactionTax"), description, 0, 0, taxPoints);
                    pointRecordRepository.Insert(pointRecord);
                    ChangeSystemTradePoints(taxPoints);
                }
            }

            //3、points去除交易税，分别变更交易双方的积分值，并生成积分记录
            PointRecord payerPointRecord = new PointRecord(payerUserId, ResourceAccessor.GetString("Common_PointTrade"), description, 0, 0, -points);
            pointRecordRepository.Insert(payerPointRecord);
            if (isImmediate)
                userService.ChangePoints(payerUserId, 0, 0, -points);
            else
                userService.ReduceFrozenTradePoints(payerUserId, points);

            PointRecord payeePointRecord = new PointRecord(payeeUserId, ResourceAccessor.GetString("Common_PointTrade"), description, 0, 0, realPoints);
            pointRecordRepository.Insert(payeePointRecord);
            userService.ChangePoints(payeeUserId, 0, 0, realPoints);
        }

        /// <summary>
        /// 用户和系统进行积分交易（例如：用户购买邀请码，礼品兑换）
        /// </summary>
        /// <param name="payerUserId">支付积分人UserId</param>
        /// <param name="points">交易积分额</param>
        /// <param name="description">交易描述</param>
        /// <param name="isImmediate">是否即时交易</param>
        public void TradeToSystem(long payerUserId, int points, string description, bool isImmediate)
        {
            //如果是即时交易，从支付方从交易积分扣除，否则从冻结的交易积分扣除（不足时抛出异常）
            if (points <= 0)
                return;
            //1、首先检查payerUserId是否可以支付积分交易额，如果余额不足抛出异常
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser payer = userService.GetUser(payerUserId);
            if (payer == null)
                throw new ExceptionFacade(string.Format("用户“{0}”不存在或已被删除", payerUserId));

            PointCategory pointCategory = GetPointCategory(PointCategoryKeys.Instance().TradePoints());
            if (pointCategory == null)
                return;

            if (isImmediate && payer.TradePoints < points)
            {
                throw new ExceptionFacade(string.Format("积分余额不足，仅有{0}{2}{3}，不够支付{1}{2}{3}", payer.TradePoints, points, pointCategory.Unit, pointCategory.CategoryName));
            }

            if (!isImmediate && payer.FrozenTradePoints < points)
            {
                throw new ExceptionFacade(string.Format("冻结积分余额不足，仅有{0}{2}{3}，不够支付{1}{2}{3}", payer.FrozenTradePoints, points, pointCategory.Unit, pointCategory.CategoryName));
            }

            //2、points去除交易税，分别变更交易双方的积分值，并生成积分记录
            PointRecord payerPointRecord = new PointRecord(payerUserId, ResourceAccessor.GetString("Common_PointTrade"), description, 0, 0, -points);
            pointRecordRepository.Insert(payerPointRecord);
            if (isImmediate)
                userService.ChangePoints(payerUserId, 0, 0, -points);
            else
                userService.ReduceFrozenTradePoints(payerUserId, points);
            //变更系统积分
            PointRecord pointRecord = new PointRecord(0, ResourceAccessor.GetString("Common_PointTrade"), description, 0, 0, points);
            pointRecordRepository.Insert(pointRecord);
            ChangeSystemTradePoints(points);
        }

        /// <summary>
        /// 奖惩用户
        /// </summary>
        /// <param name="userId">被奖惩用户</param>
        /// <param name="experiencePoints">经验</param>
        /// <param name="reputationPoints">威望</param>
        /// <param name="tradePoints">金币</param>
        /// <param name="description">奖惩理由</param>
        public void Reward(long userId, int experiencePoints, int reputationPoints, int tradePoints, string description)
        {
            if (experiencePoints == 0 && reputationPoints == 0 && tradePoints == 0)
                return;
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.GetUser(userId);
            if (user == null)
                throw new ExceptionFacade(string.Format("用户“{0}”不存在或已被删除", userId));
            //1、增减用户积分额并生成用户积分记录；
            bool isIncome = experiencePoints > 0 || reputationPoints > 0 || tradePoints > 0;
            PointRecord pointRecord = new PointRecord(userId, isIncome ? ResourceAccessor.GetString("Common_AdminReward") : ResourceAccessor.GetString("Common_AdminPunish"), description, experiencePoints, reputationPoints, tradePoints);
            pointRecordRepository.Insert(pointRecord);
            userService.ChangePoints(userId, experiencePoints, reputationPoints, tradePoints);

            //done:zhengw,by mazq 可以减去经验和威望
            //zhengw回复：已修改

            //2、增减系统积分额并生成系统积分记录；
            PointRecord systemPointRecord = new PointRecord(0, isIncome ? ResourceAccessor.GetString("Common_RewardUser") : ResourceAccessor.GetString("Common_PunishUser"), description, -experiencePoints, -reputationPoints, -tradePoints);
            pointRecordRepository.Insert(systemPointRecord);

            //done:zhengw,by mazq 为什么加isIncome ? -tradePoints : tradePoints，tradePoints本身有正负
            //zhengw回复：已修改
            //mazq回复：tradePoints应该是负值
            //zhengw回复：已修改

            ChangeSystemTradePoints(-tradePoints);

            //3、生成操作日志            
            OperationLogService logService = Tunynet.DIContainer.Resolve<OperationLogService>();
            IOperatorInfoGetter operatorInfoGetter = DIContainer.Resolve<IOperatorInfoGetter>();
            if (operatorInfoGetter == null)
                return;

            OperationLogEntry logEntry = new OperationLogEntry(operatorInfoGetter.GetOperatorInfo());
            logEntry.ApplicationId = 0;
            logEntry.Source = string.Empty;
            logEntry.OperationType = EventOperationType.Instance().Update();
            logEntry.OperationObjectName = user.UserName;
            logEntry.OperationObjectId = userId;

            PointCategory experiencePointCategory = GetPointCategory(PointCategoryKeys.Instance().ExperiencePoints());
            if (experiencePointCategory == null)
                return;
            PointCategory reputationPointCategory = GetPointCategory(PointCategoryKeys.Instance().ReputationPoints());
            if (reputationPointCategory == null)
                return;
            PointCategory tradePointCategory = GetPointCategory(PointCategoryKeys.Instance().TradePoints());
            if (tradePointCategory == null)
                return;

            logEntry.Description = string.Format("{0}“{1}”：{2}{3}{4}，{5}{6}{7}，{8}{9}{10}",
                                              isIncome ? ResourceAccessor.GetString("Common_RewardUser") : ResourceAccessor.GetString("Common_PunishUser"), user.UserName,
                                              experiencePoints, experiencePointCategory.Unit, experiencePointCategory.CategoryName,
                                              reputationPoints, reputationPointCategory.Unit, reputationPointCategory.CategoryName,
                                              tradePoints, tradePointCategory.Unit, tradePointCategory.CategoryName);
            logService.Create(logEntry);
        }

        /// <summary>
        /// 创建积分记录
        /// </summary>
        /// <param name="record"></param>
        public void CreateRecord(long userId, string pointItemName, string description, int experiencePoints, int reputationPoints, int tradePoints)
        {
            PointRecord payerPointRecord = new PointRecord(userId, pointItemName, description, experiencePoints, reputationPoints, tradePoints);
            pointRecordRepository.Insert(payerPointRecord);
        }

        /// <summary>
        ///  清理积分记录
        /// </summary>
        /// <param name="beforeDays">清理beforeDays天以前的积分记录</param>
        /// <param name="cleanSystemPointRecords">是否也删除系统积分记录</param>
        public void CleanPointRecords(int beforeDays, bool cleanSystemPointRecords = false)
        {
            pointRecordRepository.CleanPointRecords(beforeDays, cleanSystemPointRecords);
        }

        #endregion


        #region 积分记录



        /// <summary>
        /// 查询用户积分记录
        /// </summary>
        /// <param name="userId">用户Id<remarks>系统积分的UserId=0</remarks></param>
        /// <param name="isIncome">是不是收入的积分</param>
        /// <param name="pointItemName">积分项目名称</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="pageSize">页码尺寸</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagingDataSet<PointRecord> GetPointRecords(long? userId, bool? isIncome, string pointItemName, DateTime? startDate, DateTime? endDate, int pageSize, int pageIndex)
        {
            return pointRecordRepository.GetPointRecords(userId, isIncome, pointItemName, startDate, endDate, pageSize, pageIndex);
        }

        #endregion


        #region 积分统计

        /// <summary>
        /// 删除beforeDays天以前的积分统计
        /// </summary>
        /// <param name="beforeDays">天数</param>
        /// <returns>清除的记录数</returns>
        public int CleanPointStatistics(int beforeDays)
        {
            return pointStatisticRepository.Clean(beforeDays);
        }


        #endregion


        #region PointItem

        /// <summary>
        /// 更新积分项目
        /// </summary>
        /// <param name="pointItem">待更新的积分项目</param>
        public void UpdatePointItem(PointItem pointItem)
        {
            //注意：ItemId、ApplicationId、ItemName、DisplayOrder不允许修改
            pointItemRepository.Update(pointItem);
        }

        /// <summary>
        /// 获取积分项目
        /// </summary>
        /// <param name="itemKey">积分项目标识</param>
        /// <returns>返回itemKey对应的PointItem，如果没有找到返回null</returns>
        public PointItem GetPointItem(string itemKey)
        {
            return pointItemRepository.Get(itemKey);
        }

        /// <summary>
        /// 获取积分项目集合
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <returns>如果无满足条件的积分项目返回空集合</returns>
        public IEnumerable<PointItem> GetPointItems(int? applicationId)
        {
            //排序条件：DisplayOrder正序
            return pointItemRepository.GetPointItems(applicationId);
        }

        /// <summary>
        /// 获取增加积分的积分项目集合
        /// </summary>
        /// <returns>如果无满足条件的积分项目返回空集合</returns>
        public IEnumerable<PointItem> GetPointItemsOfIncome()
        {
            //过滤条件：ExperiencePoints > 0
            //排序条件：DisplayOrder正序
            IEnumerable<PointItem> pointItems = GetPointItems(null);
            if (pointItems != null)
                return pointItems.Where(n => n.ExperiencePoints > 0 || n.ReputationPoints > 0 || n.TradePoints > 0).OrderBy(n => n.DisplayOrder);
            return new List<PointItem>();
        }

        #endregion


        #region PointCategory

        /// <summary>
        /// 更新积分类型
        /// </summary>
        /// <param name="pointCategory">待更新的积分类型</param>
        public void UpdatePointCategory(PointCategory pointCategory)
        {
            //注意：CategoryKey、CategoryName、Description、DisplayOrder不允许修改
            pointCategoryRepository.Update(pointCategory);
        }

        /// <summary>
        /// 获取积分类型
        /// </summary>
        /// <param name="categoryKey">积分类型标识</param>
        /// <returns>返回itemKey对应的PointCategory，如果没有找到返回null</returns>
        public PointCategory GetPointCategory(string categoryKey)
        {
            return pointCategoryRepository.Get(categoryKey);
        }

        /// <summary>
        /// 获取积分类型集合
        /// </summary>
        /// <returns>如果无数据则返回空集合</returns>
        public IEnumerable<PointCategory> GetPointCategories()
        {
            //排序条件：DisplayOrder正序
            return pointCategoryRepository.GetAll("DisplayOrder");
        }

        #endregion


        #region Help Methods

        /// <summary>
        /// 变更系统积分总额
        /// </summary>
        /// <param name="number">变更的积分值<remarks>减积分用负数</remarks></param>
        private void ChangeSystemTradePoints(long number)
        {
            SystemDataService systemDataService = DIContainer.Resolve<SystemDataService>();
            systemDataService.Change(SystemDataKeys.Instance().TradePoints(), number);
        }

        /// <summary>
        /// 根据指定积分分类获取积分项目中的积分
        /// </summary>
        /// <param name="pointItem">积分项目</param>
        /// <returns><remarks>key=PointCategory,value=Points</remarks>积分分类-积分字典</returns>
        private Dictionary<PointCategory, int> GetPointCategory2PointsDictionary(PointItem pointItem)
        {
            Dictionary<PointCategory, int> dictionary = new Dictionary<PointCategory, int>();
            foreach (var category in GetPointCategories())
            {
                int points = 0;
                if (category.CategoryKey == PointCategoryKeys.Instance().ExperiencePoints())
                    points = pointItem.ExperiencePoints;
                else if (category.CategoryKey == PointCategoryKeys.Instance().ReputationPoints())
                    points = pointItem.ReputationPoints;
                else if (category.CategoryKey == PointCategoryKeys.Instance().TradePoints())
                    points = pointItem.TradePoints;
                else if (category.CategoryKey == "TradePoints2")
                    points = pointItem.TradePoints2;
                else if (category.CategoryKey == "TradePoints3")
                    points = pointItem.TradePoints3;
                else if (category.CategoryKey == "TradePoints4")
                    points = pointItem.TradePoints4;
                dictionary[category] = points;
            }
            return dictionary;
        }

        #endregion
    }
}