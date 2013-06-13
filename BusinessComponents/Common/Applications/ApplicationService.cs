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
using Fasterflect;
using Tunynet.Caching;
using Tunynet.UI;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 应用业务逻辑
    /// </summary>
    public class ApplicationService
    {
        //Application Repository
        private IRepository<ApplicationModel> applicationRepository;

        //ApplicationInPresentAreaSetting Repository
        private IRepository<ApplicationInPresentAreaSetting> applicationSettingRepository;

        //ApplicationInPresentAreaInstallation Repository
        private IApplicationInPresentAreaInstallationRepository applicationInstallationRepository;

        private Func<int, ApplicationConfig> getConfig;

        private NavigationService navigationService;
        /// <summary>
        /// NavigationService
        /// </summary>
        public NavigationService NavigationService
        {
            get
            {
                if (navigationService == null)
                    navigationService = new NavigationService();

                return navigationService;
            }
            set { navigationService = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationService()
            : this(new Repository<ApplicationModel>(), new Repository<ApplicationInPresentAreaSetting>(),
            new ApplicationInPresentAreaInstallationRepository(), ApplicationConfig.GetConfig)
        {
        }

        /// <summary>
        /// 可设置repository的构造函数（主要用于测试用例）
        /// </summary>
        public ApplicationService(IRepository<ApplicationModel> applicationRepository,
            IRepository<ApplicationInPresentAreaSetting> applicationSettingRepository,
            IApplicationInPresentAreaInstallationRepository applicationInstallationRepository,
            Func<int, ApplicationConfig> getConfig)
        {
            this.applicationRepository = applicationRepository;
            this.applicationSettingRepository = applicationSettingRepository;
            this.applicationInstallationRepository = applicationInstallationRepository;
            this.getConfig = getConfig;
        }


        #region Application Get && SetStatus

        /// <summary>
        /// 获取Application集合
        /// </summary>
        /// <param name="onlyIsEnabled">仅启用的应用</param>
        /// <returns>返回满足条件的应用集合</returns>
        public IEnumerable<ApplicationBase> GetAll(bool? onlyIsEnabled = null)
        {
            string cacheKey = GetCacheKey_AllApplicationBases();
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            IEnumerable<ApplicationBase> allApplications = cacheService.Get<IEnumerable<ApplicationBase>>(cacheKey);
            if (allApplications == null)
            {
                IEnumerable<ApplicationModel> applicationModels = applicationRepository.GetAll("DisplayOrder");
                List<ApplicationBase> apps = new List<ApplicationBase>();
                foreach (var model in applicationModels)
                {
                    ApplicationConfig config = getConfig(model.ApplicationId);
                    if (config == null)
                        continue;
                    Type applicationClassType = config.ApplicationType;
                    if (applicationClassType == null)
                        continue;

                    //使用Fasterflect提升反射效率
                    ConstructorInvoker applicationConstructor = applicationClassType.DelegateForCreateInstance(typeof(ApplicationModel), typeof(ApplicationConfig));
                    ApplicationBase app = applicationConstructor(model, config) as ApplicationBase;
                    if (app != null)
                        apps.Add(app);
                }
                allApplications = apps;
                cacheService.Add(cacheKey, allApplications, CachingExpirationType.Stable);
            }

            if (onlyIsEnabled.HasValue)
                return allApplications.Where(a => a.IsEnabled == true);

            return allApplications;
        }

        /// <summary>
        /// 依据ApplicationId获取应用
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        public ApplicationBase Get(int applicationId)
        {
            IEnumerable<ApplicationBase> apps = GetAll();
            return apps.Where(a => a.ApplicationId == applicationId).SingleOrDefault();
        }

        /// <summary>
        /// 依据ApplicationKey获取应用
        /// </summary>
        /// <param name="applicationKey">applicationKey</param>
        public ApplicationBase Get(string applicationKey)
        {
            IEnumerable<ApplicationBase> apps = GetAll();
            return apps.Where(a => a.ApplicationKey.Equals(applicationKey, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
        }

        /// <summary>
        /// 设置应用启用禁用状态
        /// </summary>
        /// <param name="applicationId">applicationId</param>
        /// <param name="isEnabled">是否启用</param>
        /// <remarks>设置成功返回true，设置失败返回false</remarks>
        public bool SetStatus(int applicationId, bool isEnabled)
        {
            ApplicationModel applicationModel = applicationRepository.Get(applicationId);
            if (applicationModel != null && applicationModel.IsEnabled != isEnabled)
            {
                EventBus<ApplicationModel>.Instance().OnBefore(applicationModel, new CommonEventArgs(EventOperationType.Instance().Update(), applicationId));
                applicationModel.IsEnabled = isEnabled;
                applicationRepository.Update(applicationModel);
                EventBus<ApplicationModel>.Instance().OnAfter(applicationModel, new CommonEventArgs(EventOperationType.Instance().Update(), applicationId));

                ICacheService cacheService = DIContainer.Resolve<ICacheService>();
                if (cacheService.EnableDistributedCache)
                {
                    cacheService.Remove(GetCacheKey_AllApplicationBases());
                }
                else
                {
                    ApplicationBase app = Get(applicationId);
                    app.IsEnabled = isEnabled;
                }
                return true;
            }

            return false;
        }

        #endregion


        #region 在呈现区域安装/卸载

        /// <summary>
        /// 为呈现区域实例安装应用
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        /// <param name="applicationId">applicationId</param>
        /// <returns>安装成功返回true，安装失败返回false</returns>
        public bool Install(string presentAreaKey, long ownerId, int applicationId)
        {
            ApplicationBase app = Get(applicationId);
            if (app == null || !app.IsEnabled)
                return false;

            if (!IsAvailable(presentAreaKey, applicationId))
                return false;

            if (IsInstalled(presentAreaKey, ownerId, applicationId))
                return false;

            EventBus<ApplicationBase>.Instance().OnBefore(app, new CommonEventArgs(EventOperationType.Instance().Create(), applicationId));
            if (app.Install(presentAreaKey, ownerId))
            {
                //添加安装记录
                ApplicationInPresentAreaInstallation applicationInstallation = new ApplicationInPresentAreaInstallation()
                {
                    PresentAreaKey = presentAreaKey,
                    OwnerId = ownerId,
                    ApplicationId = applicationId
                };
                applicationInstallationRepository.Insert(applicationInstallation);

                //安装应用的导航
                NavigationService.InstallPresentAreaNavigationsOfApplication(presentAreaKey, ownerId, applicationId);
                EventBus<ApplicationBase>.Instance().OnAfter(app, new CommonEventArgs(EventOperationType.Instance().Create(), applicationId));
                return true;
            }

            return false;
        }

        /// <summary>
        /// 为呈现区域实例卸载应用
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///     <item>呈现区域的内置应用不允许移除</item>
        ///     <item>未安装的应用移除失败</item>
        /// </list>
        /// </remarks>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        /// <param name="applicationId">applicationId</param>
        /// <returns>卸载成功返回true，卸载失败返回false</returns>
        public bool UnInstall(string presentAreaKey, long ownerId, int applicationId)
        {
            ApplicationBase app = Get(applicationId);
            if (app == null)
                return false;

            //呈现区域的内置应用不允许移除
            if (IsBuiltIn(presentAreaKey, applicationId))
                return false;

            if (!IsInstalled(presentAreaKey, ownerId, applicationId))
                return false;

            EventBus<ApplicationBase>.Instance().OnBefore(app, new CommonEventArgs(EventOperationType.Instance().Delete(), applicationId));
            if (app.UnInstall(presentAreaKey, ownerId))
            {
                ApplicationInPresentAreaInstallation applicationInstallation = applicationInstallationRepository.Fetch(presentAreaKey, ownerId, applicationId);
                if (applicationInstallation != null)
                {
                    applicationInstallationRepository.DeleteByEntityId(applicationInstallation.Id);

                    //卸载应用的导航
                    NavigationService.UnInstallPresentAreaNavigationsOfApplication(presentAreaKey, ownerId, applicationId);
                    EventBus<ApplicationBase>.Instance().OnAfter(app, new CommonEventArgs(EventOperationType.Instance().Delete(), applicationId));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 创建呈现区域实例时，安装该表现区域自动安装的应用
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        public void InstallApplicationsOfPresentAreaOwner(string presentAreaKey, long ownerId)
        {
            IEnumerable<int> automaticInstalledApplicationIds = GetAutomaticInstalledApplicationIds(presentAreaKey);
            foreach (var applicationId in automaticInstalledApplicationIds)
            {
                Install(presentAreaKey, ownerId, applicationId);
            }
            NavigationService.InitializePresentAreaNavigationsOfPresentAreaOwner(presentAreaKey, ownerId);
        }

        /// <summary>
        /// 删除呈现区域实例时清除应用相关的内容
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        public void DeleteApplicationsOfPresentAreaOwner(string presentAreaKey, long ownerId)
        {
            IEnumerable<ApplicationBase> installedApplications = GetInstalledApplicationsOfOwner(presentAreaKey, ownerId);

            foreach (var app in installedApplications)
            {
                if (app.UnInstall(presentAreaKey, ownerId))
                {
                    ApplicationInPresentAreaInstallation applicationInstallation = applicationInstallationRepository.Fetch(presentAreaKey, ownerId, app.ApplicationId);
                    if (applicationInstallation != null)
                        applicationInstallationRepository.DeleteByEntityId(applicationInstallation.Id);
                }
            }

            //清除该呈现区域实例的所有导航数据
            NavigationService.ClearPresentAreaNavigations(presentAreaKey, ownerId);
        }


        /// <summary>
        /// 判断Owner是否安装过此应用
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        /// <param name="applicationId">applicationId</param>
        /// <returns>已经安装返回true，未安装返回false</returns>
        public bool IsInstalled(string presentAreaKey, long ownerId, int applicationId)
        {
            IEnumerable<int> applicationIds = applicationInstallationRepository.GetInstalledApplicationIds(presentAreaKey, ownerId);
            if (applicationIds.Contains(applicationId))
                return true;

            return false;
        }

        /// <summary>
        /// 判断某呈现区域是否可以使用此应用
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="applicationId">applicationId</param>
        /// <returns>可以使用返回true，不能使用返回false</returns>
        public bool IsAvailable(string presentAreaKey, int applicationId)
        {
            IEnumerable<ApplicationInPresentAreaSetting> applicationInPresentAreaSettings = applicationSettingRepository.GetAll();
            return applicationInPresentAreaSettings.Where(a => a.PresentAreaKey.Equals(presentAreaKey, StringComparison.InvariantCultureIgnoreCase) && a.ApplicationId == applicationId).Count() > 0;
        }

        /// <summary>
        /// 判断应用在某呈现区域是否内置应用
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="applicationId">applicationId</param>
        /// <returns>内置应用返回true，否则返回false</returns>
        public bool IsBuiltIn(string presentAreaKey, int applicationId)
        {
            IEnumerable<int> builtInApplicationIds = GetBuiltInApplicationIds(presentAreaKey);
            return builtInApplicationIds.Contains(applicationId);
        }

        /// <summary>
        /// 获取呈现区域内置ApplicationId
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns>返回该呈现区域的所有内置应用的ApplicationId集合</returns>
        private IEnumerable<int> GetBuiltInApplicationIds(string presentAreaKey)
        {
            IEnumerable<ApplicationInPresentAreaSetting> applicationInPresentAreaSettings = applicationSettingRepository.GetAll();
            return applicationInPresentAreaSettings.Where(a => a.PresentAreaKey.Equals(presentAreaKey, StringComparison.InvariantCultureIgnoreCase) && a.IsBuiltIn).Select(n => n.ApplicationId).Distinct();
        }

        /// <summary>
        /// 获取呈现区域自动安装的ApplicationId
        /// </summary>
        /// <remarks>
        /// 包括IsBuiltIn=true的应用
        /// </remarks>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns>返回该呈现区域的所有已经安装的ApplicationId集合</returns>
        public IEnumerable<int> GetAutomaticInstalledApplicationIds(string presentAreaKey)
        {
            IEnumerable<ApplicationInPresentAreaSetting> applicationInPresentAreaSettings = applicationSettingRepository.GetAll();
            return applicationInPresentAreaSettings.Where(a => a.PresentAreaKey.Equals(presentAreaKey, StringComparison.InvariantCultureIgnoreCase) && (a.IsBuiltIn || a.IsAutoInstall)).Select(n => n.ApplicationId).Distinct();
        }

        /// <summary>
        /// 获取拥有者已安装的应用列表
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">拥有者ID</param>
        /// <returns>应用列表</returns>
        public IEnumerable<ApplicationBase> GetInstalledApplicationsOfOwner(string presentAreaKey, long ownerId)
        {
            IEnumerable<int> applicationIds = applicationInstallationRepository.GetInstalledApplicationIds(presentAreaKey, ownerId);
            List<ApplicationBase> apps = new List<ApplicationBase>();

            foreach (var applicationId in applicationIds)
            {
                ApplicationBase app = Get(applicationId);
                if (app != null)
                    apps.Add(app);
            }
            apps.OrderBy(a => a.DisplayOrder);

            return apps;
        }

        #endregion


        #region 删除用户

        /// <summary>
        /// 删除用户在应用中的数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="takeOverUserName">用于接管删除用户时不能删除的内容(例如：用户创建的群组)</param>
        /// <param name="isTakeOver">是否接管被删除用户可被接管的内容</param>
        public void DeleteUser(long userId, string takeOverUserName, bool isTakeOver)
        {
            IEnumerable<ApplicationBase> allApplications = GetAll();
            foreach (var app in allApplications)
            {
                try
                {
                    app.DeleteUser(userId, takeOverUserName, isTakeOver);
                }
                catch { }
            }
        }

        #endregion

        #region Help Methods

        /// <summary>
        /// 所有ApplicationBase的CacheKey
        /// </summary>        
        private string GetCacheKey_AllApplicationBases()
        {
            return "applications_all";
        }

        #endregion

    }
}
