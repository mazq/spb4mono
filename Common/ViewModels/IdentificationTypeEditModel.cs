using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    public class IdentificationTypeEditModel
    {
        #region 需持久化属性


        /// <summary>
        /// 认证类型Id
        /// </summary>
        public long IdentificationTypeId { get; set; }

        /// <summary>
        ///姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "必填")]
        [StringLength(32, ErrorMessage = "最大长度允许32个字符")]
        public string Name { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        [Display(Name = "身份证号")]
        [Required(ErrorMessage = "必填")]
        [StringLength(120, ErrorMessage = "最大长度允许120个字符")]
         public string Description { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool Enabled { get; set; }

        public string IdentificationTypeLogo { get; set; }
       
        #endregion

        /// <summary>
        /// 将EditModel转为数据库实体
        /// </summary>
        /// <returns></returns>
        public IdentificationType AsIdentificationType()
        {
            IdentificationType identificationtype = null;

            if (IdentificationTypeId == 0)
            {
                identificationtype = IdentificationType.New();
            }
            else
            {
                identificationtype = new IdentificationTypeRepository().Get(this.IdentificationTypeId);
            }
            identificationtype.CreaterId = UserContext.CurrentUser.UserId;
            identificationtype.DateCreated = DateTime.Now;
            identificationtype.Description = this.Description;
            identificationtype.Enabled = this.Enabled;
            identificationtype.Name = this.Name;

            return identificationtype;
        }
    }

    /// <summary>
    /// Identification扩展
    /// </summary>
    public static class IdentificationTypeExtensions
    {
        /// <summary>
        /// 将数据库实体转换为EditModel
        /// </summary>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public static IdentificationTypeEditModel AsEditModel(this IdentificationType identificationType)
        {
            return new IdentificationTypeEditModel
            {
                Name=identificationType.Name,
                Description=identificationType.Description,
                Enabled=identificationType.Enabled,
                IdentificationTypeId=identificationType.IdentificationTypeId,
                IdentificationTypeLogo = identificationType.IdentificationTypeLogo
            };
        }
    }
}
