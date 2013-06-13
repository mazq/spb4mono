using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 广告的EditModel
    /// </summary>
    public class AdvertisingEditModel
    {
        const string urlRegulation = "^((https|http|ftp|rtsp|mms)?://)"
        + "?(([0-9a-zA-Z_!~*'().&=+$%-]+: )?[0-9a-zA-Z_!~*'().&=+$%-]+@)?" //ftp的user@  
        + "(([0-9]{1,3}.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184  
        + "|" // 允许IP和DOMAIN（域名）
        + "([0-9a-zA-Z_!~*'()-].)*" // 域名- www.  
        + "([0-9a-zA-Z][0-9a-z-]{0,61})?[0-9a-z]." // 二级域名  
        + "[a-zA-Z]{2,6})" // first level domain- .com or .museum  
        + "(:[0-9]{1,4})?" // 端口- :80  
        + "((/?)|" // a slash isn't required if there is no file name  
        + "(/[0-9a-zA-Z_!~*'().;?:@&=+$,%#-]+)+/?)$";
        #region 属性

        /// <summary>
        ///广告Id
        /// </summary>
        public long AdvertisingId { get; set; }

        /// <summary>
        ///广告名称
        /// </summary>
        [Display(Name = "广告名称")]
        [Required(ErrorMessage = "请输入广告名称！")]
        [StringLength(50, ErrorMessage = "不能超过50个字符！")]
        public string Name { get; set; }

        /// <summary>
        ///呈现方式
        /// </summary>
        public AdvertisingType AdvertisingType { get; set; }

        /// <summary>
        ///文字样式
        /// </summary>
        public string TextStyle { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public long DisplayOrder { get; set; }

        /// <summary>
        /// 广告位Id集合
        /// </summary>
        public IEnumerable<string> PositionIds { get; set; }

        /// <summary>
        /// 图片宽度
        /// </summary>
        [Required(ErrorMessage="请输入图片宽度")]
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int ImageWidth { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        [Required(ErrorMessage="请输入图片高度")]
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int ImageHeight { get; set; }

        /// <summary>
        /// Flash宽度
        /// </summary>
        [Required(ErrorMessage = "请输入Flash宽度")]
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int FlashWidth { get; set; }

        /// <summary>
        /// Flash高度
        /// </summary>
        [Required(ErrorMessage = "请输入Flash高度")]
        [RegularExpression("\\d+",ErrorMessage="请输入数字")]
        public int FlashHeight { get; set; }

        #endregion

        #region 显示字段

        /// <summary>
        /// 呈现方式为代码时的内容
        /// </summary>
        [Required(ErrorMessage = "请输入代码！")]
        [StringLength(512, ErrorMessage = "不能超过512个字符！")]
        [AllowHtml]
        public string ScriptContent { get; set; }

        /// <summary>
        /// 呈现方式为文字时的内容
        /// </summary>
        [Required(ErrorMessage = "请输入文字内容！")]
        [StringLength(512, ErrorMessage = "不能超过512个字符！")]
        public string TextContent { get; set; }

        /// <summary>
        /// 是否是上传图片
        /// </summary>
        public bool isUploadImage { get; set; }

        /// <summary>
        /// 本地上传图片的FileName值
        /// </summary>
        public string UploadImageFileName { get; set; }

        /// <summary>
        /// 网络图片的Url
        /// </summary>
        [Required(ErrorMessage = "请输入图片的链接！")]
        [StringLength(512, ErrorMessage = "不能超过512个字符！")]
        [RegularExpression(@"(http|https):\/\/([\w.]+\/?)\S*", ErrorMessage = "请输入正确的图片链接地址！")]
        public string NetImageUrl { get; set; }

        /// <summary>
        /// Flash链接地址
        /// </summary>
        [Required(ErrorMessage = "请输入链接地址！")]
        [StringLength(512, ErrorMessage = "不能超过512个字符！")]
        public string FlashUrl { get; set; }

        /// <summary>
        ///文字链接上链接到的内容的地址
        /// </summary>
        [StringLength(512, ErrorMessage = "不能超过512个字符！")]
        [RegularExpression(urlRegulation, ErrorMessage = "请输入正确的网址！")]
        public string TextLinkedUrl { get; set; }

        /// <summary>
        /// 图片链接上链接到的内容的地址
        /// </summary>
        [StringLength(512, ErrorMessage = "不能超过512个字符！")]
        [RegularExpression(urlRegulation, ErrorMessage = "请输入正确的网址！")]
        public string ImageLinkedUrl { get; set; }

        /// <summary>
        /// 文字链接是否在新窗口打开
        /// </summary>
        public bool IsTextBlank { get; set; }

        /// <summary>
        /// 图片链接是否在新窗口打开
        /// </summary>
        public bool IsImageBlank { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public AdvertisingEditModel()
        {
            this.IsEnable = true;
            this.IsImageBlank = true;
            this.IsTextBlank = true;
            this.isUploadImage = true;
            this.StartDate = DateTime.Now;
            this.EndDate = StartDate.AddMonths(1);
        }


        /// <summary>
        /// 转化为数据库实体
        /// </summary>
        /// <returns></returns>
        public Advertising AsAdvertising()
        {
            AdvertisingService advertisingService = new AdvertisingService();
            Advertising advertising = null;
            if (this.AdvertisingId > 0)
            {
                advertising = advertisingService.GetAdvertising(this.AdvertisingId);
            }
            if (advertising == null)
            {
                advertising = Advertising.New();
            }
            advertising.AdvertisingType = this.AdvertisingType;
            switch (this.AdvertisingType)
            {
                case AdvertisingType.Image:
                    {
                        if (isUploadImage)
                        {
                            advertising.AttachmentUrl = this.UploadImageFileName ?? string.Empty;
                        }
                        else
                        {
                            advertising.AttachmentUrl = this.NetImageUrl ?? string.Empty;
                        }
                        advertising.Url = this.ImageLinkedUrl ?? string.Empty;
                        advertising.TextStyle = string.Empty;
                        advertising.Body = string.Empty;
                        advertising.IsBlank = this.IsImageBlank;
                        advertising.Width = this.ImageWidth;
                        advertising.Height = this.ImageHeight;
                    }
                    break;
                case AdvertisingType.Script:
                    {
                        advertising.Body = this.ScriptContent ?? string.Empty;
                        advertising.Url = string.Empty;
                        advertising.TextStyle = string.Empty;
                        advertising.AttachmentUrl = string.Empty;
                        advertising.IsBlank = true;
                    }
                    break;
                case AdvertisingType.Flash:
                    {
                        advertising.AttachmentUrl = this.FlashUrl ?? string.Empty;
                        advertising.Body = string.Empty;
                        advertising.Url = string.Empty;
                        advertising.TextStyle = string.Empty;
                        advertising.IsBlank = true;
                        advertising.Width = this.FlashWidth;
                        advertising.Height = this.FlashHeight;
                    }
                    break;
                case AdvertisingType.Text:
                    {
                        advertising.Body = this.TextContent ?? string.Empty;
                        advertising.Url = this.TextLinkedUrl ?? string.Empty;
                        advertising.TextStyle = this.TextStyle ?? string.Empty;
                        advertising.AttachmentUrl = string.Empty;
                        advertising.IsBlank = this.IsTextBlank;
                    }
                    break;
            }
            advertising.IsEnable = this.IsEnable;
            advertising.LastModified = DateTime.UtcNow;
            advertising.Name = this.Name ?? string.Empty;
            advertising.StartDate = this.StartDate;
            advertising.EndDate = this.EndDate;
            return advertising;
        }

        #endregion

    }

    #region 扩展

    /// <summary>
    /// 扩展类
    /// </summary>
    public static class AdvertisingExtensions
    {
        /// <summary>
        /// 转化为EditModel
        /// </summary>
        /// <param name="advertising"></param>
        /// <returns></returns>
        public static AdvertisingEditModel AsEditModel(this Advertising advertising)
        {
            AdvertisingEditModel editModel = new AdvertisingEditModel();
            editModel.AdvertisingId = advertising.AdvertisingId;
            editModel.AdvertisingType = advertising.AdvertisingType;
            editModel.DisplayOrder = advertising.DisplayOrder;
            editModel.EndDate = advertising.EndDate;
            editModel.IsEnable = advertising.IsEnable;
            editModel.LastModified = advertising.LastModified;
            editModel.Name = advertising.Name ?? string.Empty;
            editModel.StartDate = advertising.StartDate;
            editModel.TextStyle = advertising.TextStyle;
            editModel.PositionIds = new AdvertisingService().GetPositionsByAdvertisingId(advertising.AdvertisingId).Select(n => n.PositionId);
            switch (advertising.AdvertisingType)
            {
                case AdvertisingType.Image:
                    {
                        //判断链接不以http和https开头，则为上传图片
                        if (!advertising.AttachmentUrl.StartsWith("http://") && !advertising.AttachmentUrl.StartsWith("https://"))
                        {
                            editModel.isUploadImage = true;
                            editModel.UploadImageFileName = advertising.AttachmentUrl;
                            editModel.NetImageUrl = string.Empty;
                        }
                        else
                        {
                            editModel.isUploadImage = false;
                            editModel.NetImageUrl = advertising.AttachmentUrl;
                            editModel.UploadImageFileName = string.Empty;
                        }
                        editModel.ImageWidth = advertising.Width;
                        editModel.ImageHeight = advertising.Height;
                        editModel.IsImageBlank = advertising.IsBlank;
                        editModel.IsTextBlank = true;
                        editModel.ImageLinkedUrl = advertising.Url;
                        editModel.TextContent = string.Empty;
                        editModel.ScriptContent = string.Empty;
                        editModel.TextLinkedUrl = string.Empty;
                        editModel.FlashUrl = string.Empty;
                    }
                    break;
                case AdvertisingType.Script:
                    {
                        editModel.IsImageBlank = true;
                        editModel.IsTextBlank = true;
                        editModel.isUploadImage = true;
                        editModel.UploadImageFileName = string.Empty;
                        editModel.NetImageUrl = string.Empty;
                        editModel.ImageLinkedUrl = string.Empty;
                        editModel.TextContent = string.Empty;
                        editModel.ScriptContent = advertising.Body;
                        editModel.TextLinkedUrl = string.Empty;
                        editModel.FlashUrl = string.Empty;
                    }
                    break;
                case AdvertisingType.Flash:
                    {
                        editModel.IsImageBlank = true;
                        editModel.IsTextBlank = true;
                        editModel.isUploadImage = true;
                        editModel.UploadImageFileName = string.Empty;
                        editModel.NetImageUrl = string.Empty;
                        editModel.ImageLinkedUrl = string.Empty;
                        editModel.TextContent = string.Empty;
                        editModel.ScriptContent = advertising.Body;
                        editModel.TextLinkedUrl = string.Empty;
                        editModel.FlashUrl = advertising.AttachmentUrl;
                        editModel.FlashWidth = advertising.Width;
                        editModel.FlashHeight = advertising.Height;
                    }
                    break;
                case AdvertisingType.Text:
                    {
                        editModel.IsImageBlank = true;
                        editModel.IsTextBlank = advertising.IsBlank;
                        editModel.isUploadImage = true;
                        editModel.UploadImageFileName = string.Empty;
                        editModel.NetImageUrl = string.Empty;
                        editModel.ImageLinkedUrl = string.Empty;
                        editModel.TextContent = advertising.Body;
                        editModel.TextLinkedUrl = advertising.Url;
                        editModel.ScriptContent = string.Empty;
                        editModel.FlashUrl = string.Empty;
                    }
                    break;
            }

            return editModel;
        }
    }

    #endregion


}