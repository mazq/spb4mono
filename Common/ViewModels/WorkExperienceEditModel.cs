//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using Tunynet;
using System;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户表单呈现的WorkExperience实体
    /// </summary>
    public class WorkExperienceEditModel
    {

        public long Id { get; set; }

        /// <summary>
        ///公司所在地区
        /// </summary>
        [Display(Name = "所在地区")]
        public string CompanyAreaCode { get; set; }

        /// <summary>
        ///公司名称
        /// </summary>
        [Required(ErrorMessage = "请输入公司名称")]
        [Display(Name = "公司名称")]
        [StringLength(50, ErrorMessage = "内容最大长度允许50个字符")]
        [DataType(DataType.Text)]
        public string CompanyName { get; set; }

        /// <summary>
        ///起始时间
        /// </summary>
        [Required(ErrorMessage = "起始时间为必填项")]
        [Display(Name = "起止时间")]
        public int StartDate { get; set; }

        /// <summary>
        ///截止时间
        /// </summary>
        [Required(ErrorMessage = "截止时间为必填项")]
        [Display(Name = "截止时间")]
        public int EndDate { get; set; }

        /// <summary>
        ///职位描述
        /// </summary>
        [Display(Name = "部门/职位")]
        [StringLength(20, ErrorMessage = "内容最大长度允许20个字符")]
        [DataType(DataType.Text)]
        public string JobDescription { get; set; }


        /// <summary>
        /// 转换为WorkExperience用于数据库存储
        /// </summary>
        public WorkExperience AsWorkExperience(long userId)
        {
            WorkExperience workExperience;
            if (Id > 0)
            {
                UserProfileService userProfileService = new UserProfileService();
                workExperience = userProfileService.GetWorkExperience(Id, userId);

                if (!string.IsNullOrEmpty(CompanyAreaCode))
                    workExperience.CompanyAreaCode = this.CompanyAreaCode;
                else
                    workExperience.CompanyAreaCode = string.Empty;

                workExperience.CompanyName = this.CompanyName;
                workExperience.EndDate = new DateTime(this.EndDate, 1, 1);
                workExperience.StartDate = new DateTime(this.StartDate, 1, 1);

                if (workExperience.StartDate > workExperience.EndDate)
                {
                    DateTime tempDate = workExperience.EndDate;
                    workExperience.EndDate = workExperience.StartDate;
                    workExperience.StartDate = tempDate;
                }

                if (!string.IsNullOrEmpty(JobDescription))
                    workExperience.JobDescription = this.JobDescription;
                else
                    workExperience.JobDescription = string.Empty;

            }
            else
            {
                workExperience = WorkExperience.New();

                if (!string.IsNullOrEmpty(CompanyAreaCode))
                    workExperience.CompanyAreaCode = this.CompanyAreaCode;
                else
                    workExperience.CompanyAreaCode = string.Empty;

                workExperience.CompanyName = this.CompanyName;
                workExperience.EndDate = new DateTime(this.EndDate, 1, 1);
                workExperience.StartDate = new DateTime(this.StartDate, 1, 1);

                if (workExperience.StartDate > workExperience.EndDate)
                {
                    DateTime tempDate = workExperience.EndDate;
                    workExperience.EndDate = workExperience.StartDate;
                    workExperience.StartDate = tempDate;
                }

                if (!string.IsNullOrEmpty(JobDescription))
                    workExperience.JobDescription = this.JobDescription;
                else
                    workExperience.JobDescription = string.Empty;

                workExperience.UserId = userId;
            }
            return workExperience;
        }

    }

    /// <summary>
    /// WorkExperience扩展
    /// </summary>
    public static class WorkExperienceExtensions
    {
        /// <summary>
        /// 转换成EducationExperienceEditModel
        /// </summary>
        /// <param name="workExperience"></param>
        /// <returns></returns>
        public static WorkExperienceEditModel AsEditModel(this WorkExperience workExperience)
        {
            return new WorkExperienceEditModel
            {
                Id = workExperience.Id,
                StartDate = workExperience.StartDate.Year,
                JobDescription = workExperience.JobDescription,
                EndDate = workExperience.EndDate.Year,
                CompanyName = workExperience.CompanyName,
                CompanyAreaCode = workExperience.CompanyAreaCode,
            };
        }
    }
}