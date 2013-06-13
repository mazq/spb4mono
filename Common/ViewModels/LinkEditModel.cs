//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    /// <summary>
    /// LinkEditModel
    /// </summary>
    public class LinkEditModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LinkEditModel() 
        {
            this.Description = string.Empty;
            this.ImageUrl = string.Empty;
        }

        /// <summary>
        ///友情链接ID
        /// </summary>
        public long LinkId { get; set; }

        /// <summary>
        ///链接名称(站点)
        /// </summary>
        [Required(ErrorMessage = "请输入链接名称")]
        [StringLength(TextLengthSettings.TEXT_SUBJECT_MAXLENGTH, ErrorMessage = "最多可输入{1}个字")]
        public string LinkNameForSite { get; set; }

        /// <summary>
        ///链接名称(用户、群组)
        /// </summary>
        [Required(ErrorMessage = "请输入链接名称")]
        [StringLength(20, ErrorMessage = "最多可输入{1}个字")]
        public string LinkName { get; set; }

        /// <summary>
        ///链接地址
        /// </summary>
        [Required(ErrorMessage = "请输入链接地址")]
        [RegularExpression(@"^(https?):\/\/([A-z0-9]+[_\-]?[A-z0-9]*\.)*[A-z0-9]+\-?[A-z0-9]+\.[A-z]{2,}(\/.*)*\/?", ErrorMessage = "输入的地址有误")]
        public string LinkUrl { get; set; }

        /// <summary>
        ///Logo地址
        /// </summary>
        [RegularExpression(@"^(https?):\/\/([A-z0-9]+[_\-]?[A-z0-9]*\.)*[A-z0-9]+\-?[A-z0-9]+\.[A-z]{2,}(\/.*)*\/?", ErrorMessage = "输入的地址有误")]
        public string ImageUrl { get; set; }

        /// <summary>
        ///链接说明
        /// </summary>
        [StringLength(TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH, ErrorMessage = "最多可输入{1}个字")]
        public string Description { get; set; }

        /// <summary>
        /// 链接类型
        /// </summary>
        public LinkType LinkType { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// AsLink
        /// </summary>
        /// <returns></returns>
        public LinkEntity AsLink()
        {
            LinkEntity link = null;
            if (this.LinkId > 0)
            {
                link = new LinkService().Get(this.LinkId);
            }
            else
            {
                link = LinkEntity.New();
            }

            if (string.IsNullOrEmpty(this.LinkName))
            {
                link.LinkName = this.LinkNameForSite;
            }
            else
            {
                link.LinkName = this.LinkName;
            }
            
            link.LinkUrl = this.LinkUrl;

            if (this.ImageUrl != null)
            {
                link.ImageUrl = this.ImageUrl;
            }
            else
            {
                link.ImageUrl = string.Empty;
            }

            if (this.Description != null)
            {
                link.Description = this.Description;
            }
            link.LinkType = this.LinkType;
            link.IsEnabled = this.IsEnabled;

            return link;
        }
    }

    /// <summary>
    /// LinkEditModelExtense
    /// </summary>
    public static class LinkEditModelExtense
    {
        /// <summary>
        /// AsLinkEditModel
        /// </summary>
        /// <param name="link">链接实体</param>
        /// <returns></returns>
        public static LinkEditModel AsLinkEditModel(this LinkEntity link)
        {
            LinkEditModel linkEditModel = new LinkEditModel();
            linkEditModel.LinkId = link.LinkId;
            linkEditModel.LinkName = link.LinkName;
            linkEditModel.LinkNameForSite = link.LinkName;
            linkEditModel.LinkUrl = link.LinkUrl;
            linkEditModel.Description = link.Description;
            linkEditModel.ImageUrl = link.ImageUrl;
            linkEditModel.LinkType = link.LinkType;
            linkEditModel.IsEnabled = link.IsEnabled;
            return linkEditModel;
        }
    }
}
