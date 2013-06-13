using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    public class IdentificationEditModel
    {
        #region 需持久化属性

        /// <summary>
        /// Id
        /// </summary>
         public long IdentificationId { get; set; }

       /// <summary>
       /// 认证类型Id
       /// </summary>
        public long IdentificationTypeId { get; set; }

        /// <summary>
        ///申请人真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        [Required(ErrorMessage="必填")]
        [StringLength(32, ErrorMessage = "最大长度允许32个字符")]
        public string TrueName { get; set; }

        /// <summary>
        ///申请人身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        [Required(ErrorMessage = "必填")]
        [StringLength(20, ErrorMessage = "最大长度允许20个字符")]
        public string IdNumber { get; set; }

        /// <summary>
        ///认证状态(0=fail,1=success,2=pending)
        /// </summary>
        public IdentificationStatus Status { get; set; }

        /// <summary>
        ///申请人电子邮箱
        /// </summary>
        [Display(Name = "电子邮箱")]
        [Required(ErrorMessage = "必填")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "请输入正确的邮箱")]
        [StringLength(32, ErrorMessage = "最大长度允许32个字符")]
        public string Email { get; set; }

        /// <summary>
        ///申请人手机
        /// </summary>
        [Display(Name = "手机")]
        [Required(ErrorMessage = "必填")]
        [RegularExpression(@"^0{0,1}(13[0-9]|14[0-9]|15[0-9]|18[0-9])[0-9]{8}$", ErrorMessage = "请输入正确的手机号")]
        public string Mobile { get; set; }

        /// <summary>
        ///认证说明
        /// </summary>
        [Display(Name = "认证说明")]
        [Required(ErrorMessage = "必填")]
        [StringLength(120, ErrorMessage = "最大长度允许120个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 扫描图
        /// </summary>
        public string IdentificationLogo { get; set; }

        #endregion

          /// <summary>
        /// 将EditModel转为数据库实体
        /// </summary>
        /// <returns></returns>
        public Identification AsIdentification()
        {
            Identification identification = null;

            if (IdentificationId == 0)
            {
                identification = Identification.New();
            }
            else
            {
                identification =  new IdentificationRepository().Get(this.IdentificationId);
            }
            identification.IdNumber=this.IdNumber;
            identification.TrueName=this.TrueName;
            identification.IdentificationTypeId=this.IdentificationTypeId;
            identification.Description=this.Description;
            identification.Email=this.Email;
            identification.Mobile=this.Mobile;
            identification.UserId = UserContext.CurrentUser.UserId;
            identification.Status = IdentificationStatus.pending;

            return identification;
        }
    }

    /// <summary>
    /// Identification扩展
    /// </summary>
    public static class IdentificationExtensions
    {
        /// <summary>
        /// 将数据库实体转换为EditModel
        /// </summary>
        public static IdentificationEditModel AsEditModel(this Identification identification)
        {
            return new IdentificationEditModel
            {
                IdentificationId = identification.IdentificationId,
                IdentificationTypeId=identification.IdentificationTypeId,
                TrueName=identification.TrueName,
                IdNumber=identification.IdNumber,
                Status=identification.Status,
                Email=identification.Email,
                Mobile=identification.Mobile,
                Description=identification.Description,
                IdentificationLogo=identification.IdentificationLogo
            };
        }
    }
}
