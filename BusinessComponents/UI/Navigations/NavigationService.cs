//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Common;
using Tunynet.Utilities;
using Tunynet.Caching;
using System.Collections.Concurrent;
using Tunynet.Events;

namespace Tunynet.UI
{
    /// <summary>
    /// 导航业务逻辑
    /// </summary>
    public class NavigationService
    {

        //InitialNavigation Repository
        private IRepository<InitialNavigation> initialNavigationRepository;

        //PresentAreaNavigation Repository
        private IPresentAreaNavigationRepository presentAreaNavigationRepository;

        //InitialNavigation Repository
        private IInitialNavigationRepository InitialNavigationRepository = new InitialNavigationRepository();

        //CommonOperation Repository
        private ICommonOperationRepository CommonOperationRepository = new CommonOperationRepository();

        private ApplicationService applicationService;
        /// <summary>
        /// ApplicationService
        /// </summary>
        public ApplicationService ApplicationService
        {
            get
            {
                if (applicationService == null)
                    applicationService = new ApplicationService();

                return applicationService;
            }
            set { applicationService = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NavigationService() :
            this(new Repository<InitialNavigation>(), new PresentAreaNavigationRepository())
        { }

        /// <summary>
        /// 构造函数（主要用于测试用例）
        /// </summary>
        /// <param name="initialNavigationRepository">InitialNavigation仓储</param>
        /// <param name="presentAreaNavigationRepository">PresentAreaNavigation仓储</param>
        public NavigationService(IRepository<InitialNavigation> initialNavigationRepository, IPresentAreaNavigationRepository presentAreaNavigationRepository)
        {
            this.initialNavigationRepository = initialNavigationRepository;
            this.presentAreaNavigationRepository = presentAreaNavigationRepository;
        }


        #region 获取用于呈现的导航
        /// <summary>
        /// 获取第一级导航（用于呈现）
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        /// <returns>返回呈现区域实例OwnerId的导航集合</returns>
        public IEnumerable<Navigation> GetRootNavigations(string presentAreaKey, long ownerId)
        {
            return GetNavigations(presentAreaKey, ownerId).Where(n => n.Depth == 0).ToReadOnly();
        }

        /// <summary>
        /// 获取第一级导航（用于呈现，仅有单个实例的呈现区域使用）
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns>返回presentAreaKey的OwnerId为0的所有一级导航集合</returns>
        public IEnumerable<Navigation> GetRootNavigations(string presentAreaKey)
        {
            return GetRootNavigations(presentAreaKey, 0);
        }

        /// <summary>
        /// 获取导航实体
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="navigationId">导航Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns></returns>
        public Navigation GetNavigation(string presentAreaKey, int navigationId, long ownerId = 0)
        {
            IEnumerable<Navigation> navigations = GetNavigations(presentAreaKey, ownerId);
            return navigations.FirstOrDefault(n => n.NavigationId == navigationId);
        }

        /// <summary>
        /// 获取当前导航路径的NavigationId（按照Depth 从低到高）
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        /// <param name="currentNavigationId">当前导航Id</param>
        /// <returns>返回当前导航路径的NavigationId集合</returns>
        public IEnumerable<int> GetCurrentNavigationPath(string presentAreaKey, long ownerId, int currentNavigationId)
        {
            IEnumerable<Navigation> navigationsOfPresentAreaOwner = GetNavigations(presentAreaKey, ownerId);

            //对于动态导航有可能会与静态导航的NavigationId重复
            IEnumerable<Navigation> currentNavigations = navigationsOfPresentAreaOwner.Where(n => n.NavigationId == currentNavigationId);

            Navigation currentNavigation;
            if (currentNavigations.Count() > 0)
                currentNavigation = currentNavigations.First();
            else
                return Enumerable.Empty<int>();

            List<int> selectedNavigationIds = new List<int>();
            if (currentNavigation.Parent != null)
            {
                List<Navigation> allAscendants = new List<Navigation>();
                RecursiveGetParents(navigationsOfPresentAreaOwner, currentNavigation, ref allAscendants);

                //按照Depth从低到高排序
                allAscendants.Reverse();
                selectedNavigationIds.AddRange(allAscendants.Select(n => n.NavigationId));
            }
            selectedNavigationIds.Add(currentNavigation.NavigationId);

            return selectedNavigationIds;
        }

        /// <summary>
        /// 递归获取Navigation所有祖先
        /// </summary>
        private void RecursiveGetParents(IEnumerable<Navigation> allNavigations, Navigation childNavigation, ref List<Navigation> allAscendants)
        {
            if (childNavigation.Parent != null)
            {
                allAscendants.Add(childNavigation.Parent);
                RecursiveGetParents(allNavigations, childNavigation.Parent, ref allAscendants);
            }
        }

        /// <summary>
        /// 获取呈现区域实例的所有导航
        /// </summary>
        /// <remarks>
        /// 单例呈现区域第一次获取导航数据时，自动初始化该实例的导航数据
        /// </remarks>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        /// <returns>返回呈现区域实例的导航集合</returns>
        protected virtual IEnumerable<Navigation> GetNavigations(string presentAreaKey, long ownerId)
        {
            string cacheKey = GetCacheKey_NavigationOfPresentAreaOwner(presentAreaKey, ownerId);
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            IEnumerable<Navigation> navigations = cacheService.Get<IEnumerable<Navigation>>(cacheKey);
            if (navigations == null)
            {
                List<Navigation> navs = new List<Navigation>();
                IEnumerable<PresentAreaNavigation> presentAreaNavigations = presentAreaNavigationRepository.GetNavigations(presentAreaKey, ownerId);

                //如果是单例的呈现区域自动初始化实例Owner的导航数据
                if (ownerId == 0)
                {
                    presentAreaNavigations = initialNavigationRepository.GetAll()
                                                                          .Where(n => n.PresentAreaKey == presentAreaKey)
                                                                          .Select(n => n.AsPresentAreaNavigation());
                }

                int maxDepth = 0;
                foreach (var presentAreaNavigation in presentAreaNavigations)
                {
                    if (presentAreaNavigation.Depth > maxDepth)
                        maxDepth = presentAreaNavigation.Depth;

                    Navigation nav = ParseNavigation(presentAreaNavigation);
                    if (nav != null)
                    {
                        navs.Add(nav);
                    }
                }

                //处理动态导航
                IEnumerable<ApplicationBase> insatalledApplications = ApplicationService.GetInstalledApplicationsOfOwner(presentAreaKey, ownerId);
                foreach (var insatalledApplication in insatalledApplications)
                {
                    IEnumerable<Navigation> dynamicNavigationsOfApplication = insatalledApplication.GetDynamicNavigations(presentAreaKey, ownerId);
                    if (dynamicNavigationsOfApplication != null && dynamicNavigationsOfApplication.Count() > 0)
                        navs.AddRange(dynamicNavigationsOfApplication);
                }              

                Organize(navs, maxDepth);
                navigations = navs.ToReadOnly();
                cacheService.Add(cacheKey, navigations, CachingExpirationType.UsualObjectCollection);
            }
            return navigations;
        }

        ///<summary>
        /// 为Navigationt组织Parent和Children
        /// </summary>
        private void Organize(List<Navigation> navigations, int maxDepth)
        {
            ConcurrentDictionary<int, Navigation>[] navigationsDictionaryArray = new ConcurrentDictionary<int, Navigation>[maxDepth + 1];
            for (int i = 0; i <= maxDepth; i++)
            {
                navigationsDictionaryArray[i] = new ConcurrentDictionary<int, Navigation>();
            }
            foreach (Navigation nav in navigations)
            {
                navigationsDictionaryArray[nav.Depth][nav.NavigationId] = nav;
            }

            //组织Navigation.Children
            for (int i = maxDepth; i > 0; i--)
            {
                foreach (KeyValuePair<int, Navigation> pair in navigationsDictionaryArray[i])
                {
                    if (!navigationsDictionaryArray[i - 1].ContainsKey(pair.Value.ParentNavigationId))
                        continue;

                    Navigation nav = pair.Value;
                    Navigation parentNavigation = navigationsDictionaryArray[i - 1][pair.Value.ParentNavigationId];

                    nav.Parent = parentNavigation;
                    parentNavigation.AppendChild(nav);
                }
            }
        }

        #endregion


        #region 呈现区域初始化导航管理

        /// <summary>
        /// 添加初始化导航
        /// </summary>
        /// <remarks>
        /// 对于单实例的呈现区域必须设置forceOwnerCreate=true
        /// </remarks>
        /// <param name="initialNavigation">初始化导航</param>
        /// <param name="forceOwnerCreate">是否强制呈现区域实例Owner安装</param>
        /// <exception cref="ArgumentNullException">initialNavigation为空时</exception>
        /// <exception cref="ArgumentException">InitialNavigation已经存在时</exception>
        /// <exception cref="ApplicationException">initialNavigation的ParentNavigationId大于0但是相应的InitialNavigation不存在时</exception>
        /// <returns>返回创建的InitialNavigation的Id</returns>
        public long CreateInitialNavigation(InitialNavigation initialNavigation, bool forceOwnerCreate = false)
        {
            if (initialNavigation == null)
                throw new ArgumentNullException("initialNavigation不能为null");

            if (initialNavigationRepository.Exists(initialNavigation.NavigationId))
                throw new ArgumentException("NavigationId不允许重复");

            initialNavigation.NavigationType = NavigationType.PresentAreaInitial;
            if (initialNavigation.ParentNavigationId > 0)
            {
                InitialNavigation parentNavigation = GetInitialNavigation(initialNavigation.ParentNavigationId);
                if (parentNavigation == null)
                    throw new ApplicationException(string.Format("initialNavigation的父级 {0} 不存在", initialNavigation.ParentNavigationId));

                initialNavigation.Depth = parentNavigation.Depth + 1;
            }
            else
            {
                initialNavigation.Depth = 0;
            }

            EventBus<InitialNavigation>.Instance().OnBefore(initialNavigation, new CommonEventArgs(EventOperationType.Instance().Create(), initialNavigation.ApplicationId));
            var id = Convert.ToInt64(initialNavigationRepository.Insert(initialNavigation));
            EventBus<InitialNavigation>.Instance().OnAfter(initialNavigation, new CommonEventArgs(EventOperationType.Instance().Create(), initialNavigation.ApplicationId));

            if (forceOwnerCreate)
                presentAreaNavigationRepository.ForceOwnerCreate(initialNavigation);

            return id;
        }

        /// <summary>
        /// 更新初始化导航
        /// </summary>
        /// <remarks>
        /// 对于单实例的呈现区域必须设置forceOwnerUpdate=true
        /// </remarks>
        /// <param name="initialNavigation">初始化导航</param>
        /// <param name="forceOwnerUpdate">是否强制呈现区域实例Owner更新</param>
        public void UpdateInitialNavigation(InitialNavigation initialNavigation, bool forceOwnerUpdate = false)
        {
            if (initialNavigation == null)
                return;

            EventBus<InitialNavigation>.Instance().OnBefore(initialNavigation, new CommonEventArgs(EventOperationType.Instance().Update(), initialNavigation.ApplicationId));
            initialNavigationRepository.Update(initialNavigation);
            EventBus<InitialNavigation>.Instance().OnAfter(initialNavigation, new CommonEventArgs(EventOperationType.Instance().Update(), initialNavigation.ApplicationId));

            if (forceOwnerUpdate)
                presentAreaNavigationRepository.ForceOwnerUpdate(initialNavigation);
        }

        /// <summary>
        /// 删除初始化导航
        /// </summary>
        /// <remarks>
        /// 对于单实例的呈现区域必须设置forceOwnerUpdate=true
        /// </remarks>
        /// <param name="navigationId">导航Id</param>
        /// <param name="forceOwnerDelete">是否强制呈现区域实例Owner删除</param>
        /// <exception cref="ArgumentNullException">initialNavigation为空时</exception>
        /// <exception cref="ApplicationException">initialNavigation被锁定时</exception>
        public void DeleteInitialNavigation(int navigationId, bool forceOwnerDelete = false)
        {
            InitialNavigation initialNavigation = GetInitialNavigation(navigationId);
            if (initialNavigation == null)
                throw new ArgumentNullException(string.Format("NavigationId为{0}的InitialNavigation不存在", navigationId));

            if (initialNavigation.IsLocked)
                throw new ApplicationException("锁定状态的InitialNavigation不允许删除");

            List<InitialNavigation> initialNavigationsForDelete = new List<InitialNavigation>();

            IEnumerable<InitialNavigation> descendants = GetDescendants(initialNavigation);
            if (descendants.Count() > 0)
                initialNavigationsForDelete.AddRange(descendants);

            initialNavigationsForDelete.Add(initialNavigation);

            if (forceOwnerDelete)
            {
                presentAreaNavigationRepository.ForceOwnerDelete(initialNavigationsForDelete);
            }

            foreach (var initialNavigationForDelete in initialNavigationsForDelete)
            {
                initialNavigationRepository.Delete(initialNavigationForDelete);
            }
        }

        /// <summary>
        /// 在后台管理呈现区域默认导航时，获取InitialNavigation
        /// </summary>        
        /// <param name="navigationId">导航Id</param>
        /// <returns>返回navigationId对应的初始化导航实体</returns>
        public InitialNavigation GetInitialNavigation(int navigationId)
        {
            InitialNavigation presentAreaInitialNavigation = initialNavigationRepository.Get(navigationId);
            return presentAreaInitialNavigation;
        }

        /// <summary>
        /// 获取呈现区域第一级导航初始化数据
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        public IEnumerable<InitialNavigation> GetRootInitialNavigation(string presentAreaKey)
        {
            IEnumerable<InitialNavigation> presentAreaInitialNavigations = GetInitialNavigations(presentAreaKey);
            return presentAreaInitialNavigations.Where(n => n.Depth == 0);
        }

        /// <summary>
        /// 获取parentInitialNavigation的后代
        /// </summary>
        /// <param name="parentInitialNavigation">父InitialNavigation</param>
        /// <returns>返回parentInitialNavigation的所有后代</returns>
        public IEnumerable<InitialNavigation> GetDescendants(InitialNavigation parentInitialNavigation)
        {
            List<InitialNavigation> descendants = new List<InitialNavigation>();
            IEnumerable<InitialNavigation> initialNavigationsOfPresentArea = GetInitialNavigations(parentInitialNavigation.PresentAreaKey);
            RecursiveGetChildren(initialNavigationsOfPresentArea, parentInitialNavigation, ref  descendants);

            return descendants;
        }

        /// <summary>
        /// 递归获取parentInitialNavigation所有子InitialNavigation
        /// </summary>
        private void RecursiveGetChildren(IEnumerable<InitialNavigation> allInitialNavigations, InitialNavigation parentInitialNavigation, ref List<InitialNavigation> allDescendants)
        {
            IEnumerable<InitialNavigation> children = allInitialNavigations.Where(n => n.ParentNavigationId == parentInitialNavigation.NavigationId);
            if (children.Count() > 0)
            {
                allDescendants.AddRange(children);
                foreach (var child in children)
                {
                    RecursiveGetChildren(allInitialNavigations, child, ref allDescendants);
                }
            }
        }

        /// <summary>
        /// 获取所有呈现区域的导航初始化数据
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        public IEnumerable<InitialNavigation> GetInitialNavigations(string presentAreaKey)
        {
            IEnumerable<InitialNavigation> presentAreaInitialNavigations = initialNavigationRepository.GetAll("Depth,DisplayOrder");
            return presentAreaInitialNavigations.Where(n => n.PresentAreaKey.Equals(presentAreaKey, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion


        #region 呈现区域实例Owner导航管理

        /// <summary>
        /// 添加呈现区域实例导航
        /// </summary>
        /// <param name="presentAreaNavigation">PresentAreaNavigation</param>
        /// <exception cref="ArgumentNullException">presentAreaNavigation为空时</exception>
        /// <exception cref="ArgumentException">presentAreaNavigation的NavigationType不是<see cref="NavigationType"/>.AddedByOwner时</exception>
        /// <exception cref="ArgumentException">该PresentAreaNavigation此呈现区域实例已经创建时</exception>
        /// <returns>返回创建的PresentAreaNavigation的Id</returns>
        public long CreatePresentAreaNavigation(PresentAreaNavigation presentAreaNavigation)
        {
            if (presentAreaNavigation == null)
                throw new ArgumentNullException("presentAreaNavigation不能为null");

            if (presentAreaNavigation.NavigationType != NavigationType.AddedByOwner)
                throw new ArgumentException("仅能处理NavigationType为Application或PresentAreaInitial的navigation");

            IEnumerable<PresentAreaNavigation> presentAreaNavigations = GetPresentAreaNavigations(presentAreaNavigation.PresentAreaKey, presentAreaNavigation.OwnerId);

            if (presentAreaNavigations.Where(n => n.NavigationId == presentAreaNavigation.NavigationId).Count() > 0)
                throw new ArgumentException("NavigationId不允许重复");

            presentAreaNavigation.NavigationType = NavigationType.AddedByOwner;

            if (presentAreaNavigation.ParentNavigationId > 0)
            {
                InitialNavigation parentNavigation = GetInitialNavigation(presentAreaNavigation.ParentNavigationId);
                if (parentNavigation == null)
                    throw new ApplicationException(string.Format("initialNavigation的父级 {0} 不存在", presentAreaNavigation.ParentNavigationId));

                presentAreaNavigation.Depth = parentNavigation.Depth + 1;
            }
            else
            {
                presentAreaNavigation.Depth = 0;
            }
            EventBus<PresentAreaNavigation>.Instance().OnBefore(presentAreaNavigation, new CommonEventArgs(EventOperationType.Instance().Create(), presentAreaNavigation.ApplicationId));
            var id = Convert.ToInt64(presentAreaNavigationRepository.Insert(presentAreaNavigation));
            EventBus<PresentAreaNavigation>.Instance().OnAfter(presentAreaNavigation, new CommonEventArgs(EventOperationType.Instance().Create(), presentAreaNavigation.ApplicationId));

            //移除缓存（分布式缓存情况下，本机缓存会有一定延迟）
            string cacheKey = GetCacheKey_NavigationOfPresentAreaOwner(presentAreaNavigation.PresentAreaKey, presentAreaNavigation.OwnerId);
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            cacheService.Remove(cacheKey);

            return id;
        }

        /// <summary>
        /// 更新导航
        /// </summary>
        /// <param name="presentAreaNavigation">PresentAreaNavigation</param>
        public void UpdatePresentAreaNavigation(PresentAreaNavigation presentAreaNavigation)
        {
            if (presentAreaNavigation == null)
                return;

            EventBus<PresentAreaNavigation>.Instance().OnBefore(presentAreaNavigation, new CommonEventArgs(EventOperationType.Instance().Update(), presentAreaNavigation.ApplicationId));
            presentAreaNavigationRepository.Update(presentAreaNavigation);
            EventBus<PresentAreaNavigation>.Instance().OnAfter(presentAreaNavigation, new CommonEventArgs(EventOperationType.Instance().Update(), presentAreaNavigation.ApplicationId));

            //移除缓存（分布式缓存情况下，本机缓存会有一定延迟）
            string cacheKey = GetCacheKey_NavigationOfPresentAreaOwner(presentAreaNavigation.PresentAreaKey, presentAreaNavigation.OwnerId);
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            cacheService.Remove(cacheKey);
        }

        /// <summary>
        /// 删除导航
        /// </summary>
        /// <param name="id">PresentAreaNavigation实体Id</param>
        public void DeletePresentAreaNavigation(long id)
        {
            PresentAreaNavigation presentAreaNavigation = presentAreaNavigationRepository.Get(id);
            if (presentAreaNavigation == null)
                throw new ArgumentNullException(string.Format("id为{0}的PresentAreaNavigation不存在", id));

            if (presentAreaNavigation.IsLocked)
                throw new ApplicationException("锁定状态的PresentAreaNavigation不允许删除");

            DeletePresentAreaNavigation(presentAreaNavigation);

            //移除缓存（分布式缓存情况下，本机缓存会有一定延迟）
            string cacheKey = GetCacheKey_NavigationOfPresentAreaOwner(presentAreaNavigation.PresentAreaKey, presentAreaNavigation.OwnerId);
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            cacheService.Remove(cacheKey);
        }

        /// <summary>
        /// 在呈现区域实例添加应用的导航
        /// </summary>
        /// <remarks>
        /// 在呈现区域实例添加应用时调用
        /// </remarks>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        /// <param name="applicationId">应用Id</param>
        /// <returns>返回安装的PresentAreaNavigation的Id集合</returns>
        public List<long> InstallPresentAreaNavigationsOfApplication(string presentAreaKey, long ownerId, int applicationId)
        {
            IEnumerable<PresentAreaNavigation> navigationOfPresentAreaOwner = presentAreaNavigationRepository.GetNavigations(presentAreaKey, ownerId).ToList();
            //首先判断已经已经存在该应用的导航，防止重复安装
            if (navigationOfPresentAreaOwner.Where(n => n.ApplicationId == applicationId).Count() == 0)
            {
                List<long> ids = new List<long>();
                IEnumerable<InitialNavigation> initialNavigationOfApplication = GetInitialNavigations(presentAreaKey).Where(n => n.ApplicationId == applicationId);
                foreach (var initialNavigation in initialNavigationOfApplication)
                {
                    PresentAreaNavigation presentAreaNavigation = initialNavigation.AsPresentAreaNavigation();
                    presentAreaNavigation.OwnerId = ownerId;
                    presentAreaNavigationRepository.Insert(presentAreaNavigation);

                    ids.Add(presentAreaNavigation.Id);
                }

                return ids;
            }

            return null;
        }

        /// <summary>
        /// 在呈现区域实例卸载应用的导航
        /// </summary>
        /// <remarks>
        /// 在呈现区域实例卸载应用时调用
        /// </remarks>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        /// <param name="applicationId">应用Id</param>
        public void UnInstallPresentAreaNavigationsOfApplication(string presentAreaKey, long ownerId, int applicationId)
        {
            IEnumerable<PresentAreaNavigation> navigationsOfApplication = GetRootPresentAreaNavigations(presentAreaKey, ownerId).Where(n => n.ApplicationId == applicationId);
            foreach (var navigation in navigationsOfApplication)
            {
                DeletePresentAreaNavigation(navigation);
            }

            //移除缓存（分布式缓存情况下，本机缓存会有一定延迟）
            string cacheKey = GetCacheKey_NavigationOfPresentAreaOwner(presentAreaKey, ownerId);
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            cacheService.Remove(cacheKey);
        }

        /// <summary>
        /// 创建呈现区域实例时初始化导航（不包括应用的导航）
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        public void InitializePresentAreaNavigationsOfPresentAreaOwner(string presentAreaKey, long ownerId)
        {
            IEnumerable<InitialNavigation> initialNavigationOfPresentArea = GetInitialNavigations(presentAreaKey);
            foreach (var initialNavigation in initialNavigationOfPresentArea)
            {
                if (initialNavigation.ApplicationId == 0)
                {
                    PresentAreaNavigation presentAreaNavigation = initialNavigation.AsPresentAreaNavigation();
                    presentAreaNavigation.OwnerId = ownerId;
                    presentAreaNavigationRepository.Insert(presentAreaNavigation);
                }
            }
        }

        /// <summary>
        /// 清除删除呈现区域实例的所有导航
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        public void ClearPresentAreaNavigations(string presentAreaKey, long ownerId)
        {
            presentAreaNavigationRepository.ClearOwnerNavigations(presentAreaKey, ownerId);
        }

        /// <summary>
        /// 重置呈现区域实例的导航
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        public void ResetPresentAreaNavigations(string presentAreaKey, long ownerId)
        {
            ClearPresentAreaNavigations(presentAreaKey, ownerId);

            InitializePresentAreaNavigationsOfPresentAreaOwner(presentAreaKey, ownerId);
            IEnumerable<ApplicationBase> installedApplications = ApplicationService.GetInstalledApplicationsOfOwner(presentAreaKey, ownerId);
            foreach (var app in installedApplications)
            {
                InstallPresentAreaNavigationsOfApplication(presentAreaKey, ownerId, app.ApplicationId);
            }

            //移除缓存（分布式缓存情况下，本机缓存会有一定延迟）
            string cacheKey = GetCacheKey_NavigationOfPresentAreaOwner(presentAreaKey, ownerId);
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            cacheService.Remove(cacheKey);
        }

        /// <summary>
        /// 获取PresentAreaNavigation
        /// </summary>        
        /// <param name="id">PresentAreaNavigation主键</param>
        /// <returns>返回id对应的PresentAreaNavigation</returns>
        public PresentAreaNavigation GetPresentAreaNavigation(long id)
        {
            return presentAreaNavigationRepository.Get(id);
        }

        /// <summary>
        /// 获取呈现区域实例的第一级PresentAreaNavigation
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        /// <returns>返回呈现区域实例的所有导航集合</returns>
        public IEnumerable<PresentAreaNavigation> GetRootPresentAreaNavigations(string presentAreaKey, long ownerId)
        {
            IEnumerable<PresentAreaNavigation> presentAreaNavigations = GetPresentAreaNavigations(presentAreaKey, ownerId);
            return presentAreaNavigations.Where(n => n.Depth == 0);
        }

        /// <summary>
        /// 获取parentPresentAreaNavigation的后代
        /// </summary>
        /// <param name="parentPresentAreaNavigation">PresentAreaNavigation</param>
        /// <returns>返回parentPresentAreaNavigation的所有后代集合</returns>
        public IEnumerable<PresentAreaNavigation> GetDescendants(PresentAreaNavigation parentPresentAreaNavigation)
        {
            List<PresentAreaNavigation> descendants = new List<PresentAreaNavigation>();
            IEnumerable<PresentAreaNavigation> presentAreaNavigationsOfPresentArea = GetPresentAreaNavigations(parentPresentAreaNavigation.PresentAreaKey, parentPresentAreaNavigation.OwnerId);
            RecursiveGetChildren(presentAreaNavigationsOfPresentArea, parentPresentAreaNavigation, ref  descendants);

            return descendants;
        }

        /// <summary>
        /// 递归获取parentInitialNavigation所有子InitialNavigation
        /// </summary>
        private void RecursiveGetChildren(IEnumerable<PresentAreaNavigation> allPresentAreaNavigations, PresentAreaNavigation parentPresentAreaNavigation, ref List<PresentAreaNavigation> allDescendants)
        {
            IEnumerable<PresentAreaNavigation> children = allPresentAreaNavigations.Where(n => n.ParentNavigationId == parentPresentAreaNavigation.NavigationId);
            if (children.Count() > 0)
            {
                allDescendants.AddRange(children);
                foreach (var child in children)
                {
                    RecursiveGetChildren(allPresentAreaNavigations, child, ref allDescendants);
                }
            }
        }

        /// <summary>
        /// 获取呈现区域实例的所有PresentAreaNavigation
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        /// <returns>返回呈现区域实例的导航集合</returns>
        public IEnumerable<PresentAreaNavigation> GetPresentAreaNavigations(string presentAreaKey, long ownerId = 0)
        {
            IEnumerable<PresentAreaNavigation> presentAreaNavigations = presentAreaNavigationRepository.GetNavigations(presentAreaKey, ownerId);
            return presentAreaNavigations;
        }

        /// <summary>
        /// 删除导航
        /// </summary>
        private void DeletePresentAreaNavigation(PresentAreaNavigation presentAreaNavigation)
        {
            if (presentAreaNavigation == null)
                return;

            List<PresentAreaNavigation> presentAreaNavigationsForDelete = new List<PresentAreaNavigation>();

            IEnumerable<PresentAreaNavigation> descendants = GetDescendants(presentAreaNavigation);
            if (descendants.Count() > 0)
                presentAreaNavigationsForDelete.AddRange(descendants);

            presentAreaNavigationsForDelete.Add(presentAreaNavigation);

            EventBus<PresentAreaNavigation>.Instance().OnBatchBefore(presentAreaNavigationsForDelete, new CommonEventArgs(EventOperationType.Instance().Delete(), presentAreaNavigation.ApplicationId));
            foreach (var presentAreaNavigationForDelete in presentAreaNavigationsForDelete)
            {
                presentAreaNavigationRepository.Delete(presentAreaNavigationForDelete);
            }
            EventBus<PresentAreaNavigation>.Instance().OnBatchAfter(presentAreaNavigationsForDelete, new CommonEventArgs(EventOperationType.Instance().Delete(), presentAreaNavigation.ApplicationId));
        }

        #endregion

        #region 常用操作业务逻辑

        /// <summary>
        /// 获取常用操作
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<InitialNavigation> GetCommonOperations(long userId)
        {
            return InitialNavigationRepository.GetCommonOperations(userId);
        }

        /// <summary>
        /// 获取常用操作单个实体
        /// </summary>
        /// <param name="navigationId">导航Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public CommonOperation GetCommonOperation(int navigationId, long userId)
        {
            return CommonOperationRepository.GetCommonOperation(navigationId, userId);
        }

        /// <summary>
        /// 清除该用户的所有常用操作
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void ClearUserCommonOperations(long userId)
        {
            CommonOperationRepository.ClearUserCommonOperations(userId);
        }

        /// <summary>
        /// 更新常用操作
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="commonOperations">常用操作集合</param>
        public void AddCommonOperations(long userId, IEnumerable<CommonOperation> commonOperations)
        {
            CommonOperationRepository.ClearUserCommonOperations(userId);
            foreach (CommonOperation commonOperation in commonOperations)
            {
                CommonOperationRepository.Insert(commonOperation);
            }
        }

        /// <summary>
        /// 功能搜索
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public IEnumerable<InitialNavigation> SearchOperations(string keyword)
        {
            return InitialNavigationRepository.SearchOperations(keyword);
        }


        #endregion


        #region Help Methods

        /// <summary>
        /// 根据对象Id生成动态导航Id
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <returns>生成的导航Id</returns>
        public static int GenerateDynamicNavigationId(object objectId)
        {
            return 90000000 + Convert.ToInt32(objectId);
        }

        /// <summary>
        /// 把PresentAreaNavigation解析成Navigation
        /// </summary>
        /// <remarks>
        /// NavigationType = NavigationType.Application的PresentAreaNavigation需要与相应的InitialNavigation合并
        /// </remarks>
        /// <param name="presentAreaNavigation">PresentAreaNavigation</param>
        protected Navigation ParseNavigation(PresentAreaNavigation presentAreaNavigation)
        {
            if (presentAreaNavigation == null)
                return null;

            Navigation nav = new Navigation()
            {
                OwnerId = presentAreaNavigation.OwnerId,
                NavigationText = presentAreaNavigation.NavigationText,
                NavigationTarget = presentAreaNavigation.NavigationTarget,
                ImageUrl = presentAreaNavigation.ImageUrl,
                IconName = presentAreaNavigation.IconName,
                DisplayOrder = presentAreaNavigation.DisplayOrder
            };

            InitialNavigation initialNavigation = null;
            if (presentAreaNavigation.IsLocked)
            {
                initialNavigation = initialNavigationRepository.Get(presentAreaNavigation.NavigationId);
                if (initialNavigation == null)
                    return null;

                nav.NavigationId = initialNavigation.NavigationId;
                nav.ParentNavigationId = initialNavigation.ParentNavigationId;
                nav.Depth = initialNavigation.Depth;
                nav.PresentAreaKey = initialNavigation.PresentAreaKey;
                nav.ApplicationId = initialNavigation.ApplicationId;
                nav.ResourceName = initialNavigation.ResourceName;
                nav.IconName = initialNavigation.IconName;
                nav.NavigationUrl = initialNavigation.NavigationUrl;
                nav.UrlRouteName = initialNavigation.UrlRouteName;
                nav.RouteDataName = initialNavigation.RouteDataName;
                nav.OnlyOwnerVisible = initialNavigation.OnlyOwnerVisible;
                nav.NavigationType = initialNavigation.NavigationType;
                nav.IsLocked = true;
            }
            else
            {
                nav.NavigationId = presentAreaNavigation.NavigationId;
                nav.ParentNavigationId = presentAreaNavigation.ParentNavigationId;
                nav.Depth = presentAreaNavigation.Depth;
                nav.PresentAreaKey = presentAreaNavigation.PresentAreaKey;
                nav.ApplicationId = presentAreaNavigation.ApplicationId;
                nav.ResourceName = presentAreaNavigation.ResourceName;
                nav.IconName = presentAreaNavigation.IconName;
                nav.NavigationUrl = presentAreaNavigation.NavigationUrl;
                nav.UrlRouteName = presentAreaNavigation.UrlRouteName;
                nav.RouteDataName = presentAreaNavigation.RouteDataName;
                nav.OnlyOwnerVisible = presentAreaNavigation.OnlyOwnerVisible;
                nav.NavigationType = presentAreaNavigation.NavigationType;
                nav.IsLocked = presentAreaNavigation.IsLocked;
            }

            #region 设置IsEnabled
            if (presentAreaNavigation.NavigationType == NavigationType.Application)
            {
                ApplicationBase app = ApplicationService.Get(presentAreaNavigation.ApplicationId);
                if (app == null)
                    return null;

                if (!app.IsEnabled)
                {
                    nav.IsEnabled = false;
                    return nav;
                }
            }

            if (presentAreaNavigation.NavigationType == NavigationType.Application || presentAreaNavigation.NavigationType == NavigationType.PresentAreaInitial)
            {
                if (initialNavigation != null && !initialNavigation.IsEnabled)
                {
                    nav.IsEnabled = false;
                    return nav;
                }
            }

            nav.IsEnabled = presentAreaNavigation.IsEnabled;
            return nav;
            #endregion
        }

        /// <summary>
        /// 把InitialNavigation解析成Navigation
        /// </summary>
        /// <param name="initialNavigation">InitialNavigation</param>
        protected Navigation ParseNavigation(InitialNavigation initialNavigation)
        {
            if (initialNavigation == null)
                return null;

            Navigation nav = new Navigation()
            {
                NavigationId = initialNavigation.NavigationId,
                ParentNavigationId = initialNavigation.ParentNavigationId,
                Depth = initialNavigation.Depth,
                PresentAreaKey = initialNavigation.PresentAreaKey,
                ResourceName = initialNavigation.ResourceName,
                IconName = initialNavigation.IconName,
                NavigationUrl = initialNavigation.NavigationUrl,
                UrlRouteName = initialNavigation.UrlRouteName,
                ApplicationId = initialNavigation.ApplicationId,
                NavigationType = initialNavigation.NavigationType,
                NavigationText = initialNavigation.NavigationText,
                NavigationTarget = initialNavigation.NavigationTarget,
                ImageUrl = initialNavigation.ImageUrl,
                DisplayOrder = initialNavigation.DisplayOrder,
                OnlyOwnerVisible = initialNavigation.OnlyOwnerVisible,
                IsLocked = initialNavigation.IsLocked,
                IsEnabled = initialNavigation.IsEnabled,
                OwnerId = 0
            };
            return nav;
        }

        /// <summary>
        /// 获取用于呈现的呈现区域实例导航的CacheKey
        /// </summary>
        private string GetCacheKey_NavigationOfPresentAreaOwner(string presentAreaKey, long ownerId)
        {
            return string.Format("Navigations::PresentAreaKey-{0}:OwnerId-{1}", presentAreaKey, ownerId);
        }

        #endregion



    }
}
