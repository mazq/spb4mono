//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Tunynet.Common;
using Tunynet;
using Tunynet.Common.Configuration;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 权限验证服务类
    /// </summary>
    public class Authorizer
    {
        private volatile IAuthorizationService authorizationService = null;
        TenantTypeService tenantTypeService = new TenantTypeService();
        private static readonly object lockObject = new object();
        SystemDataService systemDataService = new SystemDataService();
        UserService userService = new UserService();

        /// <summary>
        /// 授权服务
        /// </summary>
        public IAuthorizationService AuthorizationService
        {
            get
            {
                if (authorizationService == null)
                {
                    lock (lockObject)
                    {
                        if (authorizationService == null)
                        {
                            authorizationService = new AuthorizationService();
                        }
                    }
                }
                return authorizationService;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Authorizer()
            : this(new AuthorizationService())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authorizationService">权限验证服务</param>
        public Authorizer(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        /// <summary>
        /// 是不是管理员
        /// </summary>
        public bool IsAdministrator(int applicationId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            if (AuthorizationService.IsSuperAdministrator(currentUser))
                return true;
            if (AuthorizationService.IsApplicationManager(currentUser, applicationId))
                return true;
            if (IsContentAdministrator(currentUser))
                return true;
            return false;
        }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        /// <returns></returns>
        public bool IsSuperAdministrator(IUser user)
        {

            if (AuthorizationService.IsSuperAdministrator(user))
                return true;
            return false;
        }

        /// <summary>
        /// 是不是内容管理员
        /// </summary>
        public bool IsContentAdministrator(IUser user)
        {
            IEnumerable<Role> rolesOfUser = new RoleService().GetRolesOfUser(user.UserId);
            if (rolesOfUser == null)
            {
                return false;
            }
            return rolesOfUser.Where(n => n.RoleName == RoleNames.Instance().ContentAdministrator()).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 是否具有管理此用户的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool User_Manage(long userId)
        {
            if (userId == 0)
                return false;

            IUser user = userService.GetUser(userId);
            if (user == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            //判断是否是站点创始人
            long FounderId = systemDataService.GetLong("Founder");
            if (FounderId == currentUser.UserId && FounderId != 0)
                return true;

            //判断超级管理员是否有权限
            if (AuthorizationService.IsSuperAdministrator(currentUser) && FounderId != userId && !AuthorizationService.IsSuperAdministrator(user))
                return true;

            //判断内容管理员是否有权限
            if (IsContentAdministrator(currentUser) && FounderId != userId && !AuthorizationService.IsSuperAdministrator(user) && !IsContentAdministrator(user))
                return true;

            return false;
        }


        /// <summary>
        /// 是否具有创建评论的权限
        /// </summary>        
        /// <returns></returns>
        public bool Comment_Create(string tenantTypeId, long? userId = null)
        {
            IUser currentUser = UserContext.CurrentUser;

            //站点设置是否启用了匿名发帖
            TenantCommentSettings settings = TenantCommentSettings.GetRegisteredSettings(tenantTypeId);

            //不允许匿名用户的时候，并且是匿名用户的时候
            if (!settings.AllowAnonymousComment && currentUser == null)
                return false;

            CommentSettings commentSettings = DIContainer.Resolve<ICommentSettingsManager>().Get();

            if (!commentSettings.AllowAnonymousComment && currentUser == null)
                return false;

            //允许匿名用户，并且是匿名用户的时候
            if (currentUser == null)
                return true;

            if (userId.HasValue)
                return new PrivacyService().Validate(userId.Value, currentUser.UserId, PrivacyItemKeys.Instance().Comment());

            return true;
        }

        /// <summary>
        /// 是否具有删除评论的权限
        /// </summary>        
        /// <returns></returns>
        public bool Comment_Delete(Comment comment)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(comment.TenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsOwner(currentUser, comment.UserId, comment.OwnerId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, comment.TenantTypeId, comment.OwnerId))
                return true;
            return false;
        }

        /// <summary>
        /// 是否有权限查看评论
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool Comment_Show(Comment comment)
        {
            if (!comment.IsPrivate)
                return true;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            if (comment.OwnerId == currentUser.UserId && comment.ParentId == 0)
                return true;

            if (comment.ToUserId != 0 && comment.ToUserId == currentUser.UserId)
                return true;
            if (comment.UserId == currentUser.UserId)
                return true;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(comment.TenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, comment.TenantTypeId, comment.OwnerId))
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有删除附件的权限
        /// </summary>        
        /// <returns></returns>
        public bool Attachment_Delete(Attachment attachment)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(attachment.TenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsOwner(currentUser, attachment.UserId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, attachment.TenantTypeId, attachment.OwnerId))
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有编辑附件的权限
        /// </summary>        
        /// <returns></returns>
        public bool Attachment_Edit(Attachment attachment)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(attachment.TenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsOwner(currentUser, attachment.UserId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, attachment.TenantTypeId, attachment.OwnerId))
                return true;
            return false;
        }

        /// <summary>
        /// 是否拥有下载的权限
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public bool Attachment_Download(Attachment attachment)
        {
            //处理仅允许注册用户下载
            //if (thread.OnlyAllowRegisteredUserDownload && currentUser == null)
            //    return false;

            //处理售价
            if (attachment.Price <= 0)
                return true;

            if (new Authorizer().Attachment_Edit(attachment))
                return true;

            AttachmentDownloadService attachementDownloadService = new AttachmentDownloadService();
            if (UserContext.CurrentUser != null && attachementDownloadService.IsDownloaded(UserContext.CurrentUser.UserId, attachment.AttachmentId))
                return true;

            return false;
        }

        /// <summary>
        /// 是否允许购买（包含已经下载过、或者不需要购买）
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public bool Attachment_Buy(Attachment attachment)
        {
            if (Attachment_Download(attachment))
                return true;
            if (UserContext.CurrentUser != null && attachment.Price <= UserContext.CurrentUser.TradePoints)
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有删除标签的权限
        /// </summary>        
        /// <returns></returns>
        public bool Tag_Delete(Tag tag)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(tag.TenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, tag.TenantTypeId, tag.OwnerId))
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有编辑标签的权限
        /// </summary>        
        /// <returns></returns>
        public bool Tag_Edit(Tag tag)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(tag.TenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, tag.TenantTypeId, tag.OwnerId))
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有编辑标签的权限
        /// </summary>        
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="tenantOwnerId">租户的OwnerId</param>
        /// <returns></returns>
        public bool Category_Create(string tenantTypeId, long tenantOwnerId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(tenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, tenantTypeId, tenantOwnerId))
                return true;
            return false;
        }
        /// <summary>
        /// 是否具有编辑分类的权限
        /// </summary>        
        /// <returns></returns>
        public bool Category_Edit(Category category)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(category.TenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, category.TenantTypeId, category.OwnerId))
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有删除分类的权限
        /// </summary>        
        /// <returns></returns>
        public bool Category_Delete(Category category)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(category.TenantTypeId);
            if (tenantType == null)
                return false;
            if (IsAdministrator(tenantType.ApplicationId))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, category.TenantTypeId, category.OwnerId))
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有加关注的权限
        /// </summary>        
        /// <returns></returns>
        public bool Follow(long userId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            return new PrivacyService().Validate(userId, currentUser.UserId, PrivacyItemKeys.Instance().Follow());
        }

        /// <summary>
        /// 是否具有发私信的权限
        /// </summary>        
        /// <returns></returns>
        public bool Message(long userId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            return new PrivacyService().Validate(userId, currentUser.UserId, PrivacyItemKeys.Instance().Message());
        }

        /// <summary>
        /// 是否具有管理推荐内容的权限
        /// </summary>
        /// <returns></returns>
        public bool RecommendItem_Manage(string tenantTypeId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            TenantTypeService tenantTypeService = new TenantTypeService();
            TenantType tenantType = tenantTypeService.Get(tenantTypeId);
            if (tenantType == null)
                return false;

            if (IsAdministrator(tenantType.ApplicationId))
                return true;

            if (currentUser.IsInRoles("ContentAdministrator"))
                return true;

            return false;
        }

        /// <summary>
        /// 是否有删除帐号绑定信息的权限
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>是否用户删除帐号绑定的权限</returns>
        public bool DeleteAccountBinding(long userId)
        {
            if (UserContext.CurrentUser == null)
                return false;

            if (UserContext.CurrentUser.UserId == userId)
                return true;

            if (AuthorizationService.IsSuperAdministrator(UserContext.CurrentUser))
                return true;

            return false;
        }

        #region 友情链接
        /// <summary>
        /// 是否具有管理友情链接的权限
        /// </summary>
        /// <param name="link">链接实体</param>
        /// <returns></returns>
        public bool Link_Manage(int ownerType, long ownerId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            if (AuthorizationService.IsSuperAdministrator(currentUser))
                return true;
            if (ownerType == OwnerTypes.Instance().User())
            {
                if (AuthorizationService.IsOwner(currentUser, ownerId))
                    return true;
            }
            else if (ownerType == OwnerTypes.Instance().Group())
            {
                if (AuthorizationService.IsTenantManager(currentUser, TenantTypeIds.Instance().Group(), ownerId))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否具有编辑友情链接的权限
        /// </summary>
        /// <param name="link">链接实体</param>
        /// <returns></returns>
        public bool Link_Edit(LinkEntity link)
        {
            IUser currentUser = UserContext.CurrentUser;
            TenantType tenantType = tenantTypeService.Get(TenantTypeIds.Instance().Link());
            if (currentUser == null)
                return false;
            if (AuthorizationService.IsSuperAdministrator(currentUser))
                return true;
            if (AuthorizationService.IsTenantManager(currentUser, tenantType.TenantTypeId, link.OwnerId))
                return true;
            if (link.OwnerType == OwnerTypes.Instance().User())
            {
                if (AuthorizationService.IsOwner(currentUser, link.OwnerId))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否具有删除友情链接的权限
        /// </summary>
        /// <param name="link">链接实体</param>
        /// <returns></returns>
        public bool Link_Delete(LinkEntity link)
        {
            return Link_Edit(link);
        }
        #endregion
    }
}
