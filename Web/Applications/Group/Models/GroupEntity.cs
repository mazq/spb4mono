//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Utilities;
using Tunynet.Common.Configuration;

namespace Spacebuilder.Group
{
    //设计要点：
    //1、从SerializablePropertiesBase派生；
    //2、缓存分区：UserId；
    //3、需要实现IAuditable；
    //4、审核状态禁止update时更新；
    //5、UserId禁止update时更新；
    


    /// <summary>
    /// 群组实体
    /// </summary>
    [TableName("spb_Groups")]
    [PrimaryKey("GroupId", autoIncrement = false)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class GroupEntity : SerializablePropertiesBase, IAuditable, IEntity
    {
        /// <summary>
        /// 群组实体
        /// </summary>
        public static GroupEntity New()
        {
            GroupEntity group = new GroupEntity()
            {
                GroupName = string.Empty,
                Description = string.Empty,
                Logo = string.Empty,
                ThemeAppearance = string.Empty,
                DateCreated = DateTime.UtcNow,
                IP = WebUtility.GetIP(),
                Announcement = string.Empty,
                MemberCount = 1


            };
            return group;
        }

        #region 需持久化属性

        /// <summary>
        ///GroupId
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        ///群组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        ///群组标识（个性网址的关键组成部分）
        /// </summary>
        public string GroupKey { get; set; }

        /// <summary>
        ///群组介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///所在地区
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        ///群主
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public long UserId { get; set; }

        /// <summary>
        ///logo名称（带部分路径
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        ///是否公开
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        ///加入方式
        /// </summary>
        public JoinWay JoinWay { get; set; }

        /// <summary>
        ///是否允许成员邀请（一直允许群管理员邀请）
        /// </summary>
        public bool EnableMemberInvite { get; set; }

        /// <summary>
        ///审核状态
        /// </summary>
        //[SqlBehavior(~SqlBehaviorFlags.Update)]
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        ///成员数
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        ///成长值
        /// </summary>
        public int GrowthValue { get; set; }

        /// <summary>
        ///设置的皮肤
        /// </summary>
        public string ThemeAppearance { get; set; }

        /// <summary>
        ///是否使用了自定义皮肤
        /// </summary>
        public bool IsUseCustomStyle { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///创建时IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        ///公告
        /// </summary>
        public string Announcement { get; set; }

        #endregion

        #region 扩展属性
        /// <summary>
        /// 判断是否有Logo
        /// </summary>
        [Ignore]
        public bool HasLogo
        {
            get
            {

                return !string.IsNullOrEmpty(Logo);
            }
        }


        /// <summary>
        /// 管理员列表
        /// </summary>
        [Ignore]
        public IEnumerable<User> GroupManagers
        {
            get
            {
                return new GroupService().GetGroupManagers(this.GroupId);
            }
        }

        /// <summary>
        /// 群主
        /// </summary>
        [Ignore]
        public User User
        {
            get
            {
                IUserService userService = DIContainer.Resolve<IUserService>();
                return userService.GetFullUser(this.UserId);
            }
        }

        /// <summary>
        /// 群组分类
        /// </summary>
        [Ignore]
        public Category Category
        {
            get
            {
                IEnumerable<Category> categories = new CategoryService().GetCategoriesOfItem(this.GroupId, null, TenantTypeIds.Instance().Group());
                return categories == null || categories.Count() == 0 ? null : categories.FirstOrDefault();
            }
        }

        private string categoryName;
        /// <summary>
        /// 群组分类名
        /// </summary>
        /// 
        [Ignore]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }


        private IEnumerable<string> tagNames;
        /// <summary>
        /// 主题标签名列表
        /// </summary>
        [Ignore]
        public IEnumerable<string> TagNames
        {
            get
            {
                if (tagNames == null)
                {
                    TagService service = new TagService(TenantTypeIds.Instance().Group());
                    IEnumerable<ItemInTag> tags = service.GetItemInTagsOfItem(this.GroupId);
                    if (tags == null)
                        return new List<string>();
                    return tags.Select(n => n.TagName);
                }
                else
                {
                    return tagNames;
                }
            }
            set
            {
                tagNames = value;
            }
        }

        #endregion

        #region 序列化属性

        /// <summary>
        /// 问题
        /// </summary>
        [Ignore]
        public string Question
        {
            get { return GetExtendedProperty<string>("Question"); }
            set { SetExtendedProperty("Question", value); }
        }

        /// <summary>
        /// 答案
        /// </summary>
        [Ignore]
        public string Answer
        {
            get { return GetExtendedProperty<string>("Answer"); }
            set { SetExtendedProperty("Answer", value); }
        }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.GroupId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        #region IAuditable 实现
        /// <summary>
        /// 审核项Key 
        /// </summary>
        public string AuditItemKey
        {
            get { return AuditItemKeys.Instance().Group(); }
        }

        #endregion

        #region 计数

        /// <summary>
        /// 浏览数
        /// </summary>
        [Ignore]
        public int HitTimes
        {
            get
            {
                CountService countService = new CountService(TenantTypeIds.Instance().Group());
                return countService.Get(CountTypes.Instance().HitTimes(), this.GroupId);
            }
        }

        /// <summary>
        /// 最近7天浏览数
        /// </summary>
        [Ignore]
        public int Last7DaysHitTimes
        {
            get
            {
                CountService countService = new CountService(TenantTypeIds.Instance().Group());
                return countService.GetStageCount(CountTypes.Instance().HitTimes(), 7, this.GroupId);
            }
        }

        /// <summary>
        /// 帖子数
        /// </summary>
        [Ignore]
        public long ThreadAndPostCount
        {
            get
            {
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().Group());
                return ownerDataService.GetLong(this.GroupId, "Bar-ThreadCount") + ownerDataService.GetLong(this.GroupId, "Bar-PostCount");
            }
        }

        /// <summary>
        /// 内容数
        /// </summary>
        [Ignore]
        public long ContentCount
        {
            get
            {
                string tenantTypeId = TenantTypeIds.Instance().Group();
                IEnumerable<string> dataKeys = OwnerDataSettings.GetDataKeys(tenantTypeId);
                if (dataKeys != null && dataKeys.Count() > 0)
                {
                    return new OwnerDataService(tenantTypeId).GetTotalCount(dataKeys, this.GroupId);
                }

                return 0;
            }
        }

        #endregion

    }
}