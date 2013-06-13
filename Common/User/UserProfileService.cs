//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Spacebuilder.Common.Configuration;
using Tunynet;
using Tunynet.Common;
using Tunynet.Events;


namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户资料业务逻辑类
    /// </summary>
    public class UserProfileService
    {
        private IProfileRepository profileRepository;
        private IEducationExperienceRepository educationExperienceRepository;
        private IWorkExperienceRepository workExperienceRepository;

        public UserProfileService()
            : this(new ProfileRepository(), new EducationExperienceRepository(), new WorkExperienceRepository())
        { }

        public UserProfileService(IProfileRepository profileRepository, IEducationExperienceRepository educationExperienceRepository, IWorkExperienceRepository workExperienceRepository)
        {
            this.profileRepository = profileRepository;
            this.educationExperienceRepository = educationExperienceRepository;
            this.workExperienceRepository = workExperienceRepository;
        }

        #region Create & Update & Get

        /// <summary>
        /// 创建用户资料（基本资料）
        /// </summary>
        /// <param name="userProfile">用户资料</param>
        /// <returns>创建成功返回true，否则返回false</returns>
        public bool Create(UserProfile userProfile)
        {
            if (userProfile == null)
                return false;

            //需要添加检查是否存在的方法
            //如果存在则更新用户资料
            if (profileRepository.UserIdIsExist(userProfile.UserId))
            {
                this.Update(userProfile);
                return false;
            }

            EventBus<UserProfile>.Instance().OnBefore(userProfile, new CommonEventArgs(EventOperationType.Instance().Create()));
                        
            long userId = Convert.ToInt64(profileRepository.Insert(userProfile));
            profileRepository.UpdateIntegrity(userProfile.UserId);

            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Create()));
            return userId > 0;
        }

        /// <summary>
        /// 更新用户资料（基本资料）
        /// </summary>
        /// <param name="userProfile">用户资料</param>
        public void Update(UserProfile userProfile)
        {
            if (userProfile == null)
                return;
            EventBus<UserProfile>.Instance().OnBefore(userProfile, new CommonEventArgs(EventOperationType.Instance().Update()));
            profileRepository.Update(userProfile);
            profileRepository.UpdateIntegrity(userProfile.UserId);

            User user = new UserService().GetFullUser(userProfile.UserId);
            if (!user.HasAvatar)
            {
                string avatarFileName = "avatar";
                if (userProfile.Gender == GenderType.NotSet)
                    avatarFileName += "_default";
                else
                    avatarFileName += "_" + userProfile.Gender.ToString();
                
                IUserRepository userRepository = new UserRepository();
                userRepository.UpdateAvatar(user, avatarFileName);
            }

            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除用户资料
        /// </summary>
        /// <param name="userProfile">用户资料</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(long userId)
        {
            UserProfile userProfile = profileRepository.Get(userId);
            if (userProfile == null)
                return false;
            EventBus<UserProfile>.Instance().OnBefore(userProfile, new CommonEventArgs(EventOperationType.Instance().Delete()));
            profileRepository.Delete(userProfile);
            IEnumerable<EducationExperience> educationExperiences = GetEducationExperiences(userId);
            if (educationExperiences != null)
            {
                foreach (var educationExperience in educationExperiences)
                {
                    educationExperienceRepository.Delete(educationExperience);
                }
            }

            IEnumerable<WorkExperience> workExperiences = GetWorkExperiences(userId);
            if (workExperiences != null)
            {
                foreach (var workExperience in workExperiences)
                {
                    workExperienceRepository.Delete(workExperience);
                }
            }

            //通过UserId  删除个人标签
            TagService tagService = new TagService(TenantTypeIds.Instance().UserProfile());
            tagService.DeleteOwnerTag(userId);
            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Delete()));
            //同时删除教育经历、工作经历、个人标签、头像文件等
            return true;
        }

        /// <summary>
        /// 添加工作经历
        /// </summary>
        /// <param name="workExperience"><see cref="WorkExperience"/></param>
        /// <returns>创建成功返回true，否则返回false</returns>
        public bool CreateWorkExperience(WorkExperience workExperience)
        {
            if (workExperience == null)
                return false;

            long affectId = Convert.ToInt64(workExperienceRepository.Insert(workExperience));

            UserProfile userProfile = profileRepository.Get(workExperience.UserId);
            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Update()));

            if (affectId > 0)
            {
                profileRepository.UpdateIntegrity(workExperience.UserId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新工作经历
        /// </summary>
        /// <param name="workExperience"><see cref="WorkExperience"/></param>
        public void UpdateWorkExperience(WorkExperience workExperience)
        {
            if (workExperience == null) return;
            workExperienceRepository.Update(workExperience);

            UserProfile userProfile = profileRepository.Get(workExperience.UserId);
            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除工作经历
        /// </summary>
        /// <param name="id">工作经历Id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool DeleteWorkExperience(long id)
        {
            if (id == 0) return false;
            WorkExperience workExperience = workExperienceRepository.Get(id);

            if (workExperience == null)
                return false;

            
            //OK
            if (GetWorkExperiences(workExperience.UserId).Count() == 1)
            {
                workExperienceRepository.DeleteByEntityId(id);
                profileRepository.UpdateIntegrity(workExperience.UserId);
            }
            else
            {
                workExperienceRepository.DeleteByEntityId(id);
            }

            UserProfile userProfile = profileRepository.Get(workExperience.UserId);
            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Update()));

            return true;
        }

        /// <summary>
        /// 添加教育经历
        /// </summary>
        /// <param name="educationExperience"><see cref="EducationExperience"/></param>
        /// <returns>创建成功返回true，否则返回false</returns>
        public bool CreateEducationExperience(EducationExperience educationExperience)
        {
            if (educationExperience == null)
                return false;

            long affectId = Convert.ToInt64(educationExperienceRepository.Insert(educationExperience));

            UserProfile userProfile = profileRepository.Get(educationExperience.UserId);
            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Update()));

            if (affectId > 0)
            {
                profileRepository.UpdateIntegrity(educationExperience.UserId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 更新教育经历
        /// </summary>
        /// <param name="educationExperience"><see cref="EducationExperience"/></param>
        public void UpdateEducationExperience(EducationExperience educationExperience)
        {
            if (educationExperience == null)
                return;
            educationExperienceRepository.Update(educationExperience);

            UserProfile userProfile = profileRepository.Get(educationExperience.UserId);
            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除教育经历
        /// </summary>
        /// <param name="id">教育经历Id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool DeleteEducationExperience(long id)
        {
            if (id == 0)
                return false;
            EducationExperience educationExperience = educationExperienceRepository.Get(id);
            if (educationExperience == null)
                return false;
            
            //OK
            if (GetEducationExperiences(educationExperience.UserId).Count() == 1)
            {
                educationExperienceRepository.DeleteByEntityId(id);
                profileRepository.UpdateIntegrity(educationExperience.UserId);
            }
            else
            {
                educationExperienceRepository.DeleteByEntityId(id);
            }


            UserProfile userProfile = profileRepository.Get(educationExperience.UserId);
            EventBus<UserProfile>.Instance().OnAfter(userProfile, new CommonEventArgs(EventOperationType.Instance().Update()));

            return true;
        }

        #endregion Create & Update & Get

        #region Get & Gets

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public UserProfile Get(long userId)
        {
            return profileRepository.Get(userId);
        }

        /// <summary>
        /// 获取用户的所有工作经历
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public IEnumerable<WorkExperience> GetWorkExperiences(long userId)
        {
            return workExperienceRepository.PopulateEntitiesByEntityIds<long>(workExperienceRepository.GetWorkExperienceOfUser(userId));
        }

        /// <summary>
        /// 获取用户的所有教育经历
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public IEnumerable<EducationExperience> GetEducationExperiences(long userId)
        {
            return educationExperienceRepository.PopulateEntitiesByEntityIds<long>(educationExperienceRepository.GetEducationExperienceOfUser(userId));
        }

        /// <summary>
        /// 获取工作经历
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public WorkExperience GetWorkExperience(long id, long userId)
        {
            return workExperienceRepository.PopulateEntitiesByEntityIds<long>(workExperienceRepository.GetWorkExperienceOfUser(userId)).Where(n => n.Id == id).FirstOrDefault<WorkExperience>();
        }

        /// <summary>
        /// 获取教育经历
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public EducationExperience GetEducationExperience(long id, long userId)
        {
            return educationExperienceRepository.PopulateEntitiesByEntityIds<long>(educationExperienceRepository.GetEducationExperienceOfUser(userId)).Where(n => n.Id == id).FirstOrDefault<EducationExperience>();
        }


        /// <summary>
        /// 根据Id列表获取UserProfile的实体列表
        /// </summary>
        /// <param name="entityIds">UserProfile的Id列表</param>
        /// <returns>UserProfile的实体列表</returns>
        public IEnumerable<UserProfile> GetUserProfiles(IEnumerable<long> entityIds)
        {
            return profileRepository.PopulateEntitiesByEntityIds<long>(entityIds);
        }

        /// <summary>
        /// 根据用户ID列表获取教育经历ID列表，本方法现用于用户搜索功能的索引生成
        /// </summary>
        /// <param name="userIds">用户ID列表</param>
        /// <returns>教育经历ID列表</returns>
        public IEnumerable<long> GetEducationExperienceIdsByUserIds(IEnumerable<long> userIds)
        {
            return educationExperienceRepository.GetEntityIdsByUserIds(userIds);
        }

        /// <summary>
        /// 根据Id列表获取教育经历的实体列表
        /// </summary>
        /// <param name="entityIds">教育经历的Id列表</param>
        /// <returns>教育经历的实体列表</returns>
        public IEnumerable<EducationExperience> GetEducationExperiences(IEnumerable<long> entityIds)
        {
            return educationExperienceRepository.PopulateEntitiesByEntityIds<long>(entityIds);
        }

        /// <summary>
        /// 根据用户ID列表获取工作经历ID列表，本方法现用于用户搜索功能的索引生成
        /// </summary>
        /// <param name="userIds">用户ID列表</param>
        /// <returns>工作经历ID列表</returns>
        public IEnumerable<long> GetWorkExperienceIdsByUserIds(IEnumerable<long> userIds)
        {
            return workExperienceRepository.GetEntityIdsByUserIds(userIds);
        }

        /// <summary>
        /// 根据Id列表获取工作经历的实体列表
        /// </summary>
        /// <param name="entityIds">工作经历的Id列表</param>
        /// <returns>工作经历的实体列表</returns>
        public IEnumerable<WorkExperience> GetWorkExperiences(IEnumerable<long> entityIds)
        {
            return workExperienceRepository.PopulateEntitiesByEntityIds<long>(entityIds);
        }

        #endregion Get & Gets


        #region 辅助方法

        ///// <summary>
        /////  获取基本用户资料的完成度
        ///// </summary>
        ///// <param name="userProfile">用户资料</param>
        ///// <returns></returns>
        //private int CountIntegrity(UserProfile userProfile)
        //{
        //    IUserProfileSettingsManager userProfileSettingsManager = DIContainer.Resolve<IUserProfileSettingsManager>();
        //    UserProfileSettings userProfileSettings = userProfileSettingsManager.GetUserProfileSettings();
        //    int[] integrityItems = userProfileSettings.IntegrityProportions;
        //    int integrity = integrityItems[(int)ProfileIntegrityItems.Birthday];

        //    IUser user = new UserService().GetUser(userProfile.UserId, false);
        //    integrity += (user.HasAvatar ? integrityItems[(int)ProfileIntegrityItems.Avatar] : 0);
        //    integrity += (userProfile.HasHomeAreaCode ? integrityItems[(int)ProfileIntegrityItems.HomeArea] : 0);
        //    integrity += (userProfile.HasIM ? integrityItems[(int)ProfileIntegrityItems.IM] : 0);
        //    integrity += (userProfile.HasIntroduction ? integrityItems[(int)ProfileIntegrityItems.Introduction] : 0);
        //    integrity += (userProfile.HasMobile ? integrityItems[(int)ProfileIntegrityItems.Mobile] : 0);
        //    integrity += (userProfile.HasNowAreaCode ? integrityItems[(int)ProfileIntegrityItems.NowArea] : 0);
        //    return integrity;
        //}

        #endregion 辅助方法
    }
}