//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户表单呈现的EducationExperience实体
    /// </summary>
    public class EducationExperienceEditModel
    {

        public long Id { get; set; }


        private DegreeType degree = DegreeType.Undergraduate;
        /// <summary>
        ///学历
        /// </summary>
        [Required(ErrorMessage = "学历为必填选项")]
        [Display(Name = "学历")]
        public DegreeType Degree
        {
            get { return degree; }
            set { degree = value; }
        }

        /// <summary>
        ///学校名称
        /// </summary>
        [Required(ErrorMessage = "请输入学校名称")]
        [Display(Name = "学校名称")]
        [StringLength(50, ErrorMessage = "学校名称最大长度允许50个字符")]
        public string School { get; set; }

        /// <summary>
        ///入学年份
        /// </summary>
        [Required(ErrorMessage = "入学年份为必填项")]
        [Display(Name = "入学年份")]
        public int StartYear { get; set; }

        /// <summary>
        ///院系/班级
        /// </summary>
        [Display(Name = "院系/班级")]
        [StringLength(20, ErrorMessage = "内容最大长度允许20个字符")]
        public string Department { get; set; }



        /// <summary>
        /// 转换为EducationExperience用于数据库存储
        /// </summary>
        public EducationExperience AsEducationExperience(long userId)
        {
            EducationExperience educationExperience;
            if (Id > 0)
            {
                UserProfileService userProfileService = new UserProfileService();
                educationExperience = userProfileService.GetEducationExperience(Id, userId);
                educationExperience.Degree = this.Degree;
                educationExperience.School = this.School;
                educationExperience.StartYear = this.StartYear;

                if (!string.IsNullOrEmpty(Department))
                    educationExperience.Department = this.Department;
                else
                    educationExperience.Department = string.Empty;

            }
            else
            {
                educationExperience = EducationExperience.New();
                educationExperience.Degree = this.Degree;
                educationExperience.School = this.School;
                educationExperience.StartYear = this.StartYear;

                if (!string.IsNullOrEmpty(Department))
                    educationExperience.Department = this.Department;
                else
                    educationExperience.Department = string.Empty;

                educationExperience.UserId = userId;
            }
            return educationExperience;
        }

    }

    /// <summary>
    /// EducationExperience扩展
    /// </summary>
    public static class EducationExperienceExtensions
    {
        /// <summary>
        /// 转换成EducationExperienceEditModel
        /// </summary>
        /// <param name="educationExperience"></param>
        /// <returns></returns>
        public static EducationExperienceEditModel AsEditModel(this EducationExperience educationExperience)
        {
            return new EducationExperienceEditModel
            {
                Id = educationExperience.Id,
                Degree = educationExperience.Degree,
                School = educationExperience.School,
                Department = educationExperience.Department,
                StartYear = educationExperience.StartYear,
            };
        }
    }
}