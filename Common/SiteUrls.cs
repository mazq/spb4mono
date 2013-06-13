//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using Tunynet;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.Utilities;
using System.Web.Mvc;
using System.Collections.Generic;
using Tunynet.FileStore;
using Tunynet.Common.Configuration;
using System.IO;
using Tunynet.UI;
using System.Linq;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 站点Url配置
    /// </summary>
    public class SiteUrls
    {
        //平台的AreaName
        private readonly string CommonAreaName = "Common";

        IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
        UserService userService = new UserService();

        #region Instance

        private static volatile SiteUrls _instance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 创建主页实体
        /// </summary>
        /// <returns></returns>
        public static SiteUrls Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new SiteUrls();
                    }
                }
            }
            return _instance;
        }

        private SiteUrls()
        { }

        #endregion Instance

        #region 系统页面

        /// <summary>
        /// 系统消息提示页面（登录时未激活、被封禁状态的提示页面）
        /// </summary>
        /// <returns></returns>
        public string SystemMessage(TempDataDictionary tempData = null, SystemMessageViewModel model = null, string returnUrl = null)
        {
            if (tempData != null && model != null)
            {
                tempData.Remove("SystemMessageViewModel");
                tempData.Add("SystemMessageViewModel", model);
            }
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(returnUrl))
                routeValueDictionary.Add("returnUrl", WebUtility.UrlEncode(returnUrl));
            return CachedUrlHelper.Action("SystemMessage", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        /// <param name="errorMesage"></param>
        /// <returns></returns>
        public string Error(string errorMesage)
        {
            return string.Empty;
        }
        /// <summary>
        /// 站点首页
        /// </summary>
        public string SiteHome()
        {
            return CachedUrlHelper.Action("Home", "Channel", CommonAreaName);
        }


        /// <summary>
        /// 用户提示的页面
        /// </summary>
        /// <returns></returns>
        public string _ListPrompt()
        {
            RouteValueDictionary routevalues = new RouteValueDictionary();
            if (UserContext.CurrentUser != null)
            {
                routevalues.Add("spaceKey", UserContext.CurrentUser.UserName);
            }

            return CachedUrlHelper.Action("_ListPrompt", "MessageCenter", CommonAreaName, routevalues);
        }
        #endregion 系统页面

        #region 公共控件

        /// <summary>
        /// 设置标题图
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        ///<param name="htmlFieldName">隐藏域全名称</param>
        /// <param name="isMultiSelect">是否多选</param>
        /// <param name="attachmentIds">附件Id</param>
        /// <param name="maxSelect">最大选择数量</param>
        /// <returns></returns>
        public string _SetTitleImage(string tenantTypeId, long associateId = 0, string htmlFieldName = "", bool isMultiSelect = false, int maxSelect = 0, string attachmentIds = "")
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("tenantTypeId", tenantTypeId);
            routeValueDictionary.Add("associateId", associateId);
            routeValueDictionary.Add("htmlFieldName", htmlFieldName);
            routeValueDictionary.Add("isMultiSelect", isMultiSelect);
            routeValueDictionary.Add("maxSelect", maxSelect);
            if (!string.IsNullOrEmpty(attachmentIds))
            {
                routeValueDictionary.Add("attachmentIds", attachmentIds);
            }

            return CachedUrlHelper.Action("_SetTitleImage", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 显示标题图选择器中已上传的图片列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="attachmentIds">附件Id</param>
        /// <param name="associateId">关联Id</param>
        /// <param name="isMultiSelect">是否多选</param>
        /// <returns></returns>
        public string _TitleImageList(string tenantTypeId, string attachmentIds, long associateId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("attachmentIds", attachmentIds);
            dic.Add("tenantTypeId", tenantTypeId);
            dic.Add("associateId", associateId);
            return CachedUrlHelper.Action("_TitleImageList", "Channel", CommonAreaName, dic);
        }

        /// <summary>
        /// 设置标题图时根据设置生成不同尺寸的图片，用于图片直连访问
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="attachmentId">附件id</param>
        /// <returns></returns>
        public string _ResizeTitleImage(string tenantTypeId, long? attachmentId = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("tenantTypeId", tenantTypeId);
            if (attachmentId.HasValue)
            {
                routeValueDictionary.Add("attachmentId", attachmentId);
            }

            return CachedUrlHelper.Action("_ResizeTitleImage", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 获取
        /// </summary>
        public string GetSuggestTags(string tenantTypeId, long? ownerId)
        {
            return CachedUrlHelper.Action("GetSuggestTags", "Channel", CommonAreaName, new RouteValueDictionary { { "tenantTypeId", tenantTypeId }, { "ownerId", ownerId } });
        }


        /// <summary>
        /// 获取子地区
        /// </summary>
        public string GetChildAreas()
        {
            return CachedUrlHelper.Action("GetChildAreas", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 获取子级分类
        /// </summary>
        public string GetChildCategories(long exceptCategoryId)
        {
            return CachedUrlHelper.Action("GetChildCategories", "Channel", CommonAreaName, new RouteValueDictionary { { "exceptCategoryId", exceptCategoryId } });
        }



        /// <summary>
        /// 统一的文件上传
        /// </summary>
        public string UploadFile(string CurrentUserIdToken)
        {
            return CachedUrlHelper.Action("UploadFile", "Channel", CommonAreaName, new RouteValueDictionary { { "CurrentUserIdToken", CurrentUserIdToken } });
        }

        /// <summary>
        /// 同意的文件上传
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="timeliness">时间限制</param>
        /// <returns></returns>
        public string UploadFile(long userId, double timeliness = 0.1)
        {
            return UploadFile(Utility.EncryptTokenForUploadfile(timeliness, userId));
        }

        /// <summary>
        /// 验证码地址
        /// </summary>
        /// <returns></returns>
        public string CaptchaImage()
        {
            return CachedUrlHelper.RouteUrl("Captcha");
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <returns></returns>
        public string _DeleteAttachment(string tenantTypeId, long attachmentId)
        {
            return CachedUrlHelper.Action("_DeleteAttachment", "Channel", CommonAreaName, new RouteValueDictionary() { { "tenantTypeId", tenantTypeId }, { "attachmentId", attachmentId } });
        }

        /// <summary>
        /// 文件库文件
        /// </summary>
        public string _EditAttachmentLibraries()
        {
            return CachedUrlHelper.Action("_EditAttachmentLibraries", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 相册图片
        /// </summary>
        public string _EditPhoto()
        {
            return CachedUrlHelper.Action("_EditPhoto", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 网络图片
        /// </summary>
        public string _EditNetImage()
        {
            return CachedUrlHelper.Action("_EditNetImage", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 网络文件
        /// </summary>
        public string _EditNetAttachment()
        {
            return CachedUrlHelper.Action("_EditNetAttachment", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 上传附件列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        public string _ListAttachments(string tenantTypeId, long associateId = 0)
        {
            return CachedUrlHelper.Action("_ListAttachments", "Channel", CommonAreaName, new RouteValueDictionary() { { "tenantTypeId", tenantTypeId }, { "associateId", associateId }, { "t", new Random().Next(1, 100).ToString() } });
        }

        /// <summary>
        /// 上传图片列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        public string _ListImages(string tenantTypeId, long associateId = 0)
        {
            return CachedUrlHelper.Action("_ListImages", "Channel", CommonAreaName, new RouteValueDictionary() { { "tenantTypeId", tenantTypeId }, { "associateId", associateId }, { "t", new Random().Next(1, 100).ToString() } });
        }

        /// <summary>
        /// 图片上传管理
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        public string _ImageManage(string tenantTypeId, long associateId = 0)
        {
            return CachedUrlHelper.Action("_ImageManage", "Channel", CommonAreaName, new RouteValueDictionary() { { "tenantTypeId", tenantTypeId }, { "associateId", associateId }, { "t", new Random().Next(1, 100).ToString() } });
        }

        /// <summary>
        /// Html编辑器中的@用户
        /// </summary>
        /// <param name="textareaId"></param>
        /// <param name="seletorId"></param>
        /// <returns></returns>
        public string _AtUsers()
        {
            return CachedUrlHelper.Action("_AtUsers", "Channel", CommonAreaName);
        }
        /// <summary>
        /// 附件上传管理
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        /// <returns></returns>
        public string _AttachmentManage(string tenantTypeId, long associateId = 0)
        {
            return CachedUrlHelper.Action("_AttachmentManage", "Channel", CommonAreaName, new RouteValueDictionary() { { "tenantTypeId", tenantTypeId }, { "associateId", associateId }, { "t", new Random().Next(1, 100).ToString() } });
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="associateId">附件关联Id</param>
        /// <returns></returns>
        public string _EditAttachment(string tenantTypeId, long associateId = 0)
        {
            return CachedUrlHelper.Action("_EditAttachment", "Channel", CommonAreaName, new RouteValueDictionary() { { "tenantTypeId", tenantTypeId }, { "associateId", associateId }, { "t", new Random().Next(1, 100).ToString() } });
        }

        /// <summary>
        /// 保存附件售价
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string _SavePrice(string tenantTypeId)
        {
            return CachedUrlHelper.Action("_SavePrice", "Channel", CommonAreaName, new RouteValueDictionary() { { "tenantTypeId", tenantTypeId } });
        }


        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="associateId">附件关联Id</param>
        public string _EditImage(string tenantTypeId, long associateId = 0)
        {
            return CachedUrlHelper.Action("_EditImage", "Channel", CommonAreaName, new RouteValueDictionary() { { "tenantTypeId", tenantTypeId }, { "associateId", associateId }, { "t", new Random().Next(1, 100).ToString() } });
        }

        /// <summary>
        /// 获取我关注的人
        /// </summary>
        /// <returns></returns>
        public string GetMyFollowedUsers()
        {
            return CachedUrlHelper.Action("GetMyFollowedUsers", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <returns></returns>
        public string SearchUsers(UserSelectorSearchScope searchScope)
        {
            return CachedUrlHelper.Action("SearchUsers", "Channel", CommonAreaName, new RouteValueDictionary { { "searchScope", searchScope } });
        }

        /// <summary>
        /// 自定义隐私设置
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public string PrivacySpecifyObjectSelector(string itemKey)
        {
            return CachedUrlHelper.Action("PrivacySpecifyObjectSelector", "Channel", CommonAreaName, new RouteValueDictionary { { "itemKey", itemKey } });
        }

        /// <summary>
        /// 多媒体解析
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <returns></returns>
        public string ParseMedia(MediaType mediaType)
        {
            return CachedUrlHelper.Action("ParseMedia", "Channel", CommonAreaName, new RouteValueDictionary { { "mediaType", mediaType } });
        }

        /// <summary>
        /// 获取表情
        /// </summary>
        /// <param name="directoryName">表情目录名</param>
        /// <returns></returns>
        public string GetEmotions(string directoryName = "")
        {
            RouteValueDictionary routeValue = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(directoryName))
            {
                routeValue.Add("directoryName", directoryName);
            }

            return CachedUrlHelper.Action("GetEmotions", "Channel", CommonAreaName, routeValue);
        }

        /// <summary>
        /// @用户提醒
        /// </summary>
        /// <returns></returns>
        public string _AtRemindUser()
        {
            return CachedUrlHelper.Action("_AtRemindUser", "Channel", CommonAreaName);
        }

        ///// <summary>
        ///// 顶踩
        ///// </summary>
        ///// <returns></returns>
        public string _SupportOppose(string tenantTypeId, long objectId, long userId, AttitudeMode mode, bool operation)
        {
            return CachedUrlHelper.Action("_SupportOppose", "Channel", CommonAreaName, new RouteValueDictionary { { "tenantTypeId", tenantTypeId }, { "objectId", objectId }, { "userId", userId }, { "mode", mode }, { "operation", operation } });
        }

        /// <summary>
        /// 自定义皮肤样式地址
        /// </summary>
        /// <returns></returns>
        public string CustomStyle(string presentAreaKey, long? ownerId = null)
        {
            return CachedUrlHelper.RouteUrl("CustomStyle", new RouteValueDictionary { { "presentAreaKey", presentAreaKey }, { "ownerId", ownerId } });
        }


        #endregion 公共控件

        #region 第三方帐号

        /// <summary>
        /// 使用第三方帐号登录
        /// </summary>
        public string LoginToThird(string accountTypeKey)
        {
            return CachedUrlHelper.Action("LoginToThird", "Account", CommonAreaName, new RouteValueDictionary { { "accountTypeKey", accountTypeKey } });
        }

        /// <summary>
        /// 第三方帐号登录回调地址
        /// </summary>
        public string ThirdCallBack(string accountTypeKey)
        {
            return CachedUrlHelper.Action("ThirdCallBack", "Account", CommonAreaName, new RouteValueDictionary { { "accountTypeKey", accountTypeKey } });
        }

        /// <summary>
        /// 首次登录网站完善资料页
        /// </summary>
        public string ThirdRegister()
        {
            return CachedUrlHelper.Action("ThirdRegister", "Account", CommonAreaName);
        }

        /// <summary>
        /// 第三方帐号登录按钮图片地址
        /// </summary>
        public string ThirdLoginButtonUrl(string accountTypeKey)
        {
            return WebUtility.ResolveUrl(string.Format("~/Images/{0}-login.png", accountTypeKey));
        }

        /// <summary>
        /// 第三方帐号登录按钮图片地址
        /// </summary>
        public string ThirdLogoUrl(string accountTypeKey, ThirdLogoSizeType thirdLogoSizeType = ThirdLogoSizeType.Normal)
        {
            string logoName = string.Empty;
            switch (thirdLogoSizeType)
            {
                case ThirdLogoSizeType.Big:
                    logoName = "bigLogo";
                    break;
                case ThirdLogoSizeType.Normal:
                    logoName = "logo";
                    break;
                case ThirdLogoSizeType.Small:
                    logoName = "smallLogo";
                    break;
                default:
                    break;
            }
            return WebUtility.ResolveUrl(string.Format("~/Images/{0}-{1}.png", accountTypeKey.ToLower(), logoName));
        }

        /// <summary>
        /// 管理第三方帐号
        /// </summary>
        /// <param name="spaceKey">用户的spaceKey</param>
        /// <returns>管理第三方帐号的连接</returns>
        public string ManageAccountBindings(string spaceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("ManageAccountBindings", "UserSpaceSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 取消第三方帐号的绑定
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <param name="accountTypeKey">第三方帐号标示</param>
        /// <returns>取消第三方帐号的绑定</returns>
        public string _CancelAccountBinding(string spaceKey, string accountTypeKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            dic.Add("accountTypeKey", accountTypeKey);
            return CachedUrlHelper.Action("_CancelAccountBinding", "UserSpaceSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 一条绑定信息
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <param name="accountTypeKey">第三方帐号类型</param>
        /// <returns>一条绑定信息</returns>
        public string _AccountBinding(string spaceKey, string accountTypeKey = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(accountTypeKey))
                dic.Add("accountTypeKey", accountTypeKey);
            return CachedUrlHelper.Action("_AccountBinding", "UserSpaceSettings", CommonAreaName, dic);
        }

        #endregion

        #region 用户空间（UserSpace）

        #region User 用户相关

        /// <summary>
        /// 动态列表
        /// </summary>
        public string _ActivitiesList(string spaceKey, long? groupId = null, long? applicationId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(spaceKey))
                routeValueDictionary.Add("spaceKey", spaceKey);
            if (groupId != null)
                routeValueDictionary.Add("groupId", groupId);
            if (applicationId != null)
                routeValueDictionary.Add("applicationId", applicationId);
            return CachedUrlHelper.Action("_ActivitiesList", "UserSpace", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        ///  查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        public string GetNewerCount(string spaceKey, int? applicationId = null)
        {
            return CachedUrlHelper.Action("GetNewerCount", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "applicationId", applicationId } });
        }

        /// <summary>
        /// 获取以后进入用户时间线的动态
        /// </summary>
        public string _GetNewerActivities(string spaceKey, int? applicationId = null)
        {
            return CachedUrlHelper.Action("_GetNewerActivities", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "applicationId", applicationId } });
        }

        /// <summary>
        /// 编辑用户简介
        /// </summary>
        public string _EditUserIntroduction(string spaceKey)
        {
            return CachedUrlHelper.Action("_EditUserIntroduction", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }
        /// <summary>
        /// 最近访客
        /// </summary>
        public string _SpaceLastVisitList(string spaceKey)
        {

            return CachedUrlHelper.Action("_SpaceLastVisitList", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 我和他都关注了
        /// </summary>
        public string _TogetherFollowedUsersList(string spaceKey)
        {
            return CachedUrlHelper.Action("_TogetherFollowedUsersList", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 这些人也关注了他
        /// </summary>
        public string _FollowedUsersFromUserList(string spaceKey)
        {
            return CachedUrlHelper.Action("_FollowedUsersFromUserList", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 他的粉丝也关注了
        /// </summary>
        public string _FollowedUsersOfFollowersList(string spaceKey)
        {
            return CachedUrlHelper.Action("_FollowedUsersOfFollowersList", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除该条访客记录
        /// </summary>
        public string DeleteSpaceVisitors(string spaceKey)
        {
            return CachedUrlHelper.Action("DeleteSpaceVisitors", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除该条访客记录
        /// </summary>
        public string DeleteHomeVisitors(string spaceKey)
        {
            return CachedUrlHelper.Action("DeleteHomeVisitors", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 我的主页最近访客
        /// </summary>
        public string _HomeLastVisitList(string spaceKey, int pageIndex = 1)
        {
            return CachedUrlHelper.Action("_HomeLastVisitList", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "pageIndex", pageIndex } });
        }

        /// <summary>
        /// 人气用户
        /// </summary>
        public string _PopularUserList(string spaceKey)
        {
            return CachedUrlHelper.Action("_PopularUserList", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 注册用户的链接
        /// </summary>
        /// <returns></returns>
        public string Register(string returnUrl = null, bool includeReturnUrl = false)
        {
            if (includeReturnUrl)
            {
                HttpContext httpContext = HttpContext.Current;
                string currentPath = httpContext.Request.Url.PathAndQuery;

                returnUrl = SiteUrls.ExtractQueryParams(currentPath)["ReturnUrl"];

                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = WebUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
            }
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(returnUrl))
                routeValueDictionary.Add("returnUrl", WebUtility.UrlEncode(returnUrl));
            return CachedUrlHelper.Action("Register", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 站点登录页面
        /// </summary>
        /// <param name="loginModal">登录模式</param>
        /// <param name="includeReturnUrl">是否包含returnUrl(默认为false)</param>
        /// <param name="returnUrl">回跳地址</param>
        public string Login(bool includeReturnUrl = false, LoginModal loginModal = LoginModal.login, string returnUrl = null, string callBack = null)
        {
            if (includeReturnUrl)
            {
                HttpContext httpContext = HttpContext.Current;
                string currentPath = httpContext.Request.Url.PathAndQuery;

                returnUrl = SiteUrls.ExtractQueryParams(currentPath)["ReturnUrl"];

                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = WebUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
            }
            else if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = WebUtility.UrlEncode(returnUrl);
            }
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(returnUrl))
                dic.Add("returnUrl", returnUrl);
            switch (loginModal)
            {
                case LoginModal._login:
                    return CachedUrlHelper.Action("_Login", "Account", CommonAreaName, dic);
                case LoginModal._LoginInModal:
                    if (!string.IsNullOrEmpty(callBack))
                        dic.Add("callBack", callBack);
                    return CachedUrlHelper.Action("_LoginInModal", "Account", CommonAreaName, dic);
                default:
                    return CachedUrlHelper.Action("Login", "Account", CommonAreaName, dic);
            }
        }


        /// <summary>
        /// 我的首页
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string MyHome(long userId)
        {
            string spaceKey = UserIdToUserNameDictionary.GetUserName(userId);
            if (string.IsNullOrEmpty(spaceKey))
                return string.Empty;

            return MyHome(spaceKey);
        }

        /// <summary>
        /// 我的首页
        /// </summary>
        /// <param name="spaceKey">用户空间标识</param>
        /// <returns></returns>
        public string MyHome(string spaceKey, long? groupId = null, long? applicationId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(spaceKey))
                routeValueDictionary.Add("spaceKey", spaceKey);
            if (groupId != null)
                routeValueDictionary.Add("groupId", groupId);
            if (applicationId != null)
                routeValueDictionary.Add("applicationId", applicationId);

            return CachedUrlHelper.Action("MyHome", "UserSpace", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 我的主页
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string SpaceHome(long userId, int? applicationId = null)
        {
            return SpaceHome(UserIdToUserNameDictionary.GetUserName(userId), applicationId);
        }

        /// <summary>
        /// 我的主页
        /// </summary>
        /// <param name="spaceKey">用户空间标识</param>
        /// <returns></returns>
        public string SpaceHome(string spaceKey, int? applicationId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (applicationId.HasValue)
                dic.Add("applicationId", applicationId);
            return CachedUrlHelper.Action("SpaceHome", "UserSpace", CommonAreaName, dic);
        }

        /// <summary>
        /// 验证邮箱的链接
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string RegisterValidateEmail(long userId)
        {
            ILinktimelinessSettingsManager linktimelinessSettingsManager = DIContainer.Resolve<ILinktimelinessSettingsManager>();
            LinktimelinessSettings linktimelinessSettings = linktimelinessSettingsManager.Get();
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("token", Utility.EncryptTokenForValidateEmail(linktimelinessSettings.Highlinktimeliness, userId));
            return CachedUrlHelper.Action("ActivateEmail", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 重设密码的链接
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public string ResetPassword(long userId)
        {
            ILinktimelinessSettingsManager linktimelinessSettingsManager = DIContainer.Resolve<ILinktimelinessSettingsManager>();
            LinktimelinessSettings linktimelinessSettings = linktimelinessSettingsManager.Get();
            string token = Utility.EncryptTokenFindPassword(linktimelinessSettings.Highlinktimeliness, userId);
            return CachedUrlHelper.Action("ResetPassword", "Account", CommonAreaName, new RouteValueDictionary { { "token", token } });
        }

        /// <summary>
        /// 跳转至忘记密码的页面
        /// </summary>
        /// <returns></returns>
        public string FindPassword()
        {
            return CachedUrlHelper.Action("FindPassword", "Account", CommonAreaName);
        }

        /// <summary>
        /// 注册的时候的条款页
        /// </summary>
        /// <returns></returns>
        public string _Provision()
        {
            return CachedUrlHelper.Action("_Provision", "Account", CommonAreaName);
        }

        /// <summary>
        /// 登出的链接
        /// </summary>
        /// <returns></returns>
        public string Logout()
        {
            return CachedUrlHelper.Action("Logout", "Account", CommonAreaName);
        }

        /// <summary>
        /// 发送邮件成功的提示页面
        /// </summary>
        /// <returns></returns>
        public string SendEmailSucceed()
        {
            return CachedUrlHelper.Action("SendEmailSucceed", "Account", CommonAreaName);
        }

        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string ValidateUserName(string value = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(value))
                routeValueDictionary.Add("value", value);
            return CachedUrlHelper.Action("ValidateUserName", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ValidateEmail(string value = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(value))
                routeValueDictionary.Add("value", value);
            return CachedUrlHelper.Action("ValidateEmail", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 验证密码的方法
        /// </summary>
        /// <param name="value">准备验证的方法</param>
        /// <returns>验证密码的链接</returns>
        public string ValidatePassword(string value = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(value))
                routeValueDictionary.Add("value", value);
            return CachedUrlHelper.Action("ValidatePassword", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 获取附件下载的URL
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="enableCaching">是否缓存</param>
        public string AttachmentUrl(long attachmentId, string tenantTypeId, bool enableCaching = true)
        {
            return WebUtility.ResolveUrl(string.Format("~/Handlers/AttachmentAuthorize.ashx?attachmentId={0}&tenantTypeId={1}&enableCaching={2}", attachmentId, tenantTypeId, enableCaching));
        }

        /// <summary>
        /// 附件下载的直连地址
        /// </summary>
        /// <param name="attachment">附件实体</param>
        /// <param name="enableClientCaching">是否缓存</param>
        /// <returns></returns>
        public string AttachmentDirectUrl(Attachment attachment, bool enableClientCaching = true)
        {
            if (attachment == null)
            {
                return string.Empty;
            }

            string attachmentPath = attachment.GetRelativePath() + "/" + attachment.FileName;
            if (enableClientCaching)
            {
                return storeProvider.GetDirectlyUrl(attachmentPath);
            }
            else
            {
                return storeProvider.GetDirectlyUrl(attachmentPath, DateTime.Now);
            }
        }

        /// <summary>
        /// 附件下载的临时地址，根据设定的时间自动过期
        /// </summary>
        /// <param name="attachmentId">附件id</param>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="token">加密串</param>
        /// <param name="enableCaching">是否缓存</param>
        /// <returns></returns>
        public string AttachmentTempUrl(long attachmentId, string tenantTypeId, string token, bool enableCaching = true)
        {
            return WebUtility.ResolveUrl(string.Format("~/Handlers/Attachment.ashx?attachmentId={0}&tenantTypeId={1}&token={2}&enableCaching={3}", attachmentId, tenantTypeId, token, enableCaching));
        }

        /// <summary>
        /// 获取Logo的URL
        /// </summary>
        /// <param name="relativeLogoUrl">logo的相对路径</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="imageSizeTypeKey">尺寸类型（通过ImageSizeTypeKeys类获取)</param>
        /// <param name="enableClientCaching">是否使用客户端缓存</param>
        /// <returns></returns>
        public string LogoUrl(string relativeLogoUrl, string tenantTypeId, string imageSizeTypeKey, bool enableClientCaching = true)
        {
            if (string.IsNullOrEmpty(relativeLogoUrl))
            {
                return WebUtility.ResolveUrl("~/Themes/Shared/Styles/Images/figure.jpg");
            }

            if (!relativeLogoUrl.ToLower().EndsWith(".gif") && !string.IsNullOrEmpty(imageSizeTypeKey) && imageSizeTypeKey != ImageSizeTypeKeys.Instance().Original())
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(tenantTypeId);
                if (tenantLogoSettings != null && tenantLogoSettings.ImageSizeTypes != null && tenantLogoSettings.ImageSizeTypes.ContainsKey(imageSizeTypeKey))
                {
                    var imageSizeType = tenantLogoSettings.ImageSizeTypes[imageSizeTypeKey];
                    string sizeImageName = storeProvider.GetSizeImageName(relativeLogoUrl, imageSizeType.Key, imageSizeType.Value);

                    if (enableClientCaching)
                    {
                        return storeProvider.GetDirectlyUrl(sizeImageName);
                    }
                    else
                    {
                        return storeProvider.GetDirectlyUrl(sizeImageName, DateTime.Now);
                    }
                }
            }

            if (enableClientCaching)
            {
                return storeProvider.GetDirectlyUrl(relativeLogoUrl);
            }
            else
            {
                return storeProvider.GetDirectlyUrl(relativeLogoUrl, DateTime.Now);
            }
        }

        /// <summary>
        /// 获取图片的URL
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="imageSizeTypeKey">尺寸类型（通过ImageSizeTypeKeys类获取)</param>
        /// <param name="enableClientCaching">是否使用客户端缓存</param>
        /// <returns></returns>
        public string ImageUrl(long attachmentId, string tenantTypeId, string imageSizeTypeKey, bool enableClientCaching = true)
        {
            if (attachmentId <= 0)
            {
                return WebUtility.ResolveUrl("~/Themes/Shared/Styles/Images/default_img.png");
            }

            AttachmentService<Attachment> attachmentService = new AttachmentService<Attachment>(tenantTypeId);
            Attachment attachment = attachmentService.Get(attachmentId);
            return ImageUrl(attachment, tenantTypeId, imageSizeTypeKey, enableClientCaching);
        }

        /// <summary>
        /// 获取图片的URL
        /// </summary>
        /// <param name="attachment">附件</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="imageSizeTypeKey">尺寸类型（通过ImageSizeTypeKeys类获取)</param>
        /// <param name="enableClientCaching">是否使用客户端缓存</param>
        /// <returns></returns>
        public string ImageUrl(Attachment attachment, string tenantTypeId, string imageSizeTypeKey, bool enableClientCaching = true)
        {
            if (attachment == null)
            {
                return WebUtility.ResolveUrl("~/Themes/Shared/Styles/Images/default_img.png");
            }

            return ImageUrl(attachment.GetRelativePath() + "/" + attachment.FileName, tenantTypeId, imageSizeTypeKey, enableClientCaching);
        }

        /// <summary>
        /// 获取图片的URL
        /// </summary>
        /// <param name="relativeImageUrl">图片的相对路径</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="imageSizeTypeKey">尺寸类型（通过ImageSizeTypeKeys类获取)</param>
        /// <param name="enableClientCaching">是否使用客户端缓存</param>
        /// <returns></returns>
        public string ImageUrl(string relativeImageUrl, string tenantTypeId, string imageSizeTypeKey, bool enableClientCaching = true)
        {
            if (string.IsNullOrEmpty(relativeImageUrl))
            {
                return WebUtility.ResolveUrl("~/Themes/Shared/Styles/Images/default_img.png");
            }

            if (!relativeImageUrl.ToLower().EndsWith(".gif") && !string.IsNullOrEmpty(imageSizeTypeKey) && imageSizeTypeKey != ImageSizeTypeKeys.Instance().Original())
            {
                TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);
                if (tenantAttachmentSettings.ImageSizeTypes != null && tenantAttachmentSettings.ImageSizeTypes.Any(n => n.ImageSizeTypeKey == imageSizeTypeKey))
                {
                    var imageSizeType = tenantAttachmentSettings.ImageSizeTypes.First(n => n.ImageSizeTypeKey == imageSizeTypeKey);
                    string sizeImageName = storeProvider.GetSizeImageName(relativeImageUrl, imageSizeType.Size, imageSizeType.ResizeMethod);

                    if (enableClientCaching)
                    {
                        return storeProvider.GetDirectlyUrl(sizeImageName);
                    }
                    else
                    {
                        return storeProvider.GetDirectlyUrl(sizeImageName, DateTime.Now);
                    }
                }
            }

            if (enableClientCaching)
            {
                return storeProvider.GetDirectlyUrl(relativeImageUrl);
            }
            else
            {
                return storeProvider.GetDirectlyUrl(relativeImageUrl, DateTime.Now);
            }
        }

        /// <summary>
        /// 用户头像Url
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="avatarSizeType">头像尺寸</param>
        /// <param name="enableClientCaching">是否启用客户端缓存</param>
        public string UserAvatarUrl(IUser user, AvatarSizeType avatarSizeType, bool enableClientCaching = false)
        {
            return userService.GetAvatarDirectlyUrl(user, avatarSizeType, enableClientCaching);
        }

        /// <summary>
        /// 跳转至忘记密码的页面
        /// </summary>
        /// <returns></returns>
        public string FindPassword(string accountEmail = null, bool isPartial = false)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(accountEmail))
                routeValueDictionary.Add("accountEmail", accountEmail);
            if (isPartial)
                routeValueDictionary.Add("isPartial", isPartial);
            return CachedUrlHelper.Action("FindPassword", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 发送激活邮件
        /// </summary>
        /// <param name="accountEmail">邮箱</param>
        /// <param name="token">登陆凭证</param>
        /// <returns></returns>
        public string _ActivateByEmail(string accountEmail = null, string token = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(accountEmail))
                routeValueDictionary.Add("accountEmail", accountEmail);
            if (!string.IsNullOrEmpty(token))
                routeValueDictionary.Add("token", token);
            return CachedUrlHelper.Action("_ActivateByEmail", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 等待管理员为您激活帐号
        /// </summary>
        /// <returns></returns>
        internal string WaitForAdminExamine()
        {
            return CachedUrlHelper.Action("WaitForAdminExamine", "Account", CommonAreaName);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>修改密码链接</returns>
        public string ChangePassword(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("ChangePassword", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }


        /// <summary>
        /// 用户的关注
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <returns></returns>
        public string FollowedUsers(string spaceKey)
        {
            return string.Empty;
        }

        /// <summary>
        /// 用户的关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string FollowedUsers(long userId)
        {
            return string.Empty;
        }

        /// <summary>
        ///用户的粉丝
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <returns></returns>
        public string Followers(string spaceKey)
        {
            return string.Empty;
        }

        /// <summary>
        ///用户的粉丝
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string Followers(long userId)
        {
            return string.Empty;
        }





        #endregion User 用户相关

        #region 评论

        /// <summary>
        ///  获取我收到的评论
        /// </summary>
        public string ListCommentsInBox(string spaceKey, int? pageIndex, string tenantTypeId = "")
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

            routeValueDictionary.Add("spaceKey", spaceKey);
            if (pageIndex.HasValue)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (!string.IsNullOrEmpty(tenantTypeId))
                routeValueDictionary.Add("tenantTypeId", tenantTypeId);

            return CachedUrlHelper.Action("ListCommentsInBox", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        ///  获取我发出去的评论
        /// </summary>
        public string ListCommentsOutBox(string spaceKey, int? pageIndex, string tenantTypeId = "")
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (pageIndex.HasValue)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (!string.IsNullOrEmpty(tenantTypeId))
                routeValueDictionary.Add("tenantTypeId", tenantTypeId);
            return CachedUrlHelper.Action("ListCommentsOutBox", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="commentId">评论ID</param>
        public string DeleteComment(string spaceKey, long commentId)
        {
            return CachedUrlHelper.Action("DeleteComment", "MessageCenter", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "commentId", commentId } });
        }

        /// <summary>
        ///  获取我发出去的评论
        /// </summary>
        public string AtMeComments(string spaceKey, int? pageIndex, string tenantTypeId = "")
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);

            if (pageIndex.HasValue)
                routeValueDictionary.Add("pageIndex", pageIndex);

            if (!string.IsNullOrEmpty(tenantTypeId))
                routeValueDictionary.Add("tenantTypeId", tenantTypeId);

            return CachedUrlHelper.Action("AtMeComments", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        #endregion

        #region 上传头像

        /// <summary>
        /// 头像异步显示
        /// </summary>
        public string _EditAvatarAsyn(string spaceKey)
        {
            return CachedUrlHelper.Action("_EditAvatarAsyn", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 取消正在切割的头像
        /// </summary>
        public string _CancelAvatar(string spaceKey)
        {
            return CachedUrlHelper.Action("_CancelAvatar", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 裁剪头像
        /// </summary>
        public string _CropAvatar(string spaceKey, float? srcWidth, float? srcHeight, float? srcX, float? srcY)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (srcWidth.HasValue)
                routeValueDictionary.Add("srcWidth", srcWidth);
            if (srcHeight.HasValue)
                routeValueDictionary.Add("srcHeight", srcHeight);
            if (srcX.HasValue)
                routeValueDictionary.Add("srcX", srcX);
            if (srcY.HasValue)
                routeValueDictionary.Add("srcY", srcY);
            return CachedUrlHelper.Action("_CropAvatar", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 编辑用户资料-上传用户头像页面
        /// </summary>
        /// <returns></returns>
        public string EditAvatar(string spaceKey)
        {
            return CachedUrlHelper.Action("EditAvatar", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        /// <returns></returns>
        public string _EditAvatar(string spaceKey)
        {
            return CachedUrlHelper.Action("_EditAvatar", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        #endregion 上传头像

        #region 教育经历

        /// <summary>
        /// 编辑用户资料-教育经历
        /// </summary>
        public string EditUserEducation(string spaceKey)
        {
            return CachedUrlHelper.Action("EditUserEducation", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除用户教育经历
        /// </summary>
        public string DeleteUserEducation(string spaceKey, long educationId)
        {
            return CachedUrlHelper.Action("DeleteUserEducation", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "educationId", educationId } });
        }

        #endregion 教育经历

        #region 工作经历

        /// <summary>
        /// 编辑用户资料-工作经历
        /// </summary>
        public string EditUserWork(string spaceKey)
        {
            return CachedUrlHelper.Action("EditUserWork", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除用户教育经历
        /// </summary>
        public string DeleteUserWork(string spaceKey, long workId)
        {
            return CachedUrlHelper.Action("DeleteUserWork", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "workId", workId } });
        }

        #endregion 工作经历

        #region 基本资料

        /// <summary>
        /// 编辑用户资料-基本资料
        /// </summary>
        public string EditUserProfile(string spaceKey)
        {
            return CachedUrlHelper.Action("EditUserProfile", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 编辑用户资料-激活邮箱
        /// </summary>
        public string SendAsyn(string spaceKey)
        {
            return CachedUrlHelper.Action("SendAsyn", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 编辑用户资料-修改用户账户邮箱
        /// </summary>
        public string _EditUserAccountEmail(string spaceKey)
        {
            return CachedUrlHelper.Action("_EditUserAccountEmail", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 用户内容上的气泡
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public string _ContentPop(string spaceKey)
        {
            return CachedUrlHelper.Action("_ContentPop", "UserSpace", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        #endregion 基本资料

        #region 完善个人资料向导

        /// <summary>
        /// 看看感兴趣的人向导
        /// </summary>
        /// <returns></returns>
        public string UserProfileGuideInterested(string spaceKey)
        {
            return CachedUrlHelper.Action("UserProfileGuideInterested", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 个人标签向导
        /// </summary>
        /// <returns></returns>
        public string UserProfileGuideTag(string spaceKey)
        {
            return CachedUrlHelper.Action("UserProfileGuideTag", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        ///填写个人资料向导 
        /// </summary>
        /// <returns></returns>
        public string UserProfileGuideDetail(string spaceKey)
        {
            return CachedUrlHelper.Action("UserProfileGuideDetail", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 上传头像向导
        /// </summary>
        /// <returns></returns>
        public string UserProfileGuideAvatar(string spaceKey)
        {
            return CachedUrlHelper.Action("UserProfileGuideAvatar", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 完成向导
        /// </summary>
        /// <returns></returns>
        public string _CompleteGuide(string spaceKey)
        {
            return CachedUrlHelper.Action("_CompleteGuide", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 更多感兴趣的人
        /// </summary>
        /// <returns></returns>
        public string _MoreInterested(string spaceKey)
        {
            return CachedUrlHelper.Action("_MoreInterested", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }
        #endregion

        #region 用户等级

        /// <summary>
        /// 我的等级
        /// </summary>
        public string MyRank(string spaceKey)
        {
            return CachedUrlHelper.Action("MyRank", "Honour", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 积分记录列表
        /// </summary>
        public string ListPointRecords(string spaceKey)
        {
            return CachedUrlHelper.Action("ListPointRecords", "Honour", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        #endregion Honour

        #region Invitation 请求

        /// <summary>
        /// 请求页
        /// </summary>
        ///
        /// <returns></returns>
        public string ListInvitations(string spaceKey, int? pageIndex = 1, bool isPartialView = false)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (pageIndex.HasValue && pageIndex != 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (isPartialView)
                routeValueDictionary.Add("isPartialView", isPartialView);
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("ListInvitations", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量设置请求的状态
        /// </summary>
        /// <returns></returns>
        public string BatchSetStatus(string spaceKey, string invitationIds = null, InvitationStatus invitationStatus = InvitationStatus.Accept, int? pageIndex = 1, bool isPromp = false)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(invitationIds))
                routeValueDictionary.Add("invitationIds", invitationIds);
            routeValueDictionary.Add("invitationStatus", invitationStatus);
            if (pageIndex.HasValue && pageIndex != 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (isPromp)
                routeValueDictionary.Add("isPromp", isPromp);
            return CachedUrlHelper.Action("BatchSetStatus", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 气泡中批量接受邀请
        /// </summary>
        /// <param name="spaceKey">用户Key</param>
        /// <param name="invitationIds">被接受的邀请</param>
        /// <returns>链接</returns>
        public string PrompSetInvitations(string spaceKey, string invitationIds = null, InvitationStatus status = InvitationStatus.Accept)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(invitationIds))
                routeValueDictionary.Add("invitationIds", invitationIds);
            routeValueDictionary.Add("status", status);
            return CachedUrlHelper.Action("PrompSetInvitations", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 一条请求的局部视图
        /// </summary>
        /// <returns></returns>
        public string _Invitation(string spaceKey, long id)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", id);
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_Invitation", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 更改一条请求的状态
        /// </summary>
        /// <returns></returns>
        public string _InvitationSetStatus(string spaceKey, long id, InvitationStatus status = InvitationStatus.Accept)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            routeValueDictionary.Add("id", id);
            routeValueDictionary.Add("status", status);
            return CachedUrlHelper.Action("_InvitationSetStatus", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量删除请求
        /// </summary>
        /// <param name="invitationIds"></param>
        /// <returns></returns>
        public string DeleteInvitation(string spaceKey, string invitationIds = null, int? pageIndex = 1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (pageIndex.HasValue && pageIndex > 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (!string.IsNullOrEmpty(invitationIds))
                routeValueDictionary.Add("invitationIds", invitationIds);
            return CachedUrlHelper.Action("DeleteInvitation", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 提示请求页面
        /// </summary>
        /// <returns></returns>
        public string _ListPromptInvitation(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_ListPromptInvitation", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 用户设置请求的页面
        /// </summary>
        /// <returns></returns>
        public string _UserInvitationSettings(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_UserInvitationSettings", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 处理提交的用户请求连接
        /// </summary>
        /// <returns></returns>
        public string _SetUserInvitationSettings(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_SetUserInvitationSettings", "MessageCenter", CommonAreaName, routeValueDictionary);
        }

        #endregion Invitation 请求

        #region 皮肤设置

        /// <summary>
        /// 皮肤设置
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <param name="enableCustomStyle"></param>
        /// <param name="isUseCustomStyle"></param>
        /// <returns></returns>
        public string _ThemeSettings(string presentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace, long ownerId = 0, bool enableCustomStyle = false, bool isUseCustomStyle = false)
        {
            return CachedUrlHelper.Action("_ThemeSettings", "Channel", CommonAreaName, new RouteValueDictionary() { { "presentAreaKey", presentAreaKey }, { "ownerId", ownerId }, { "enableCustomStyle", enableCustomStyle }, { "isUseCustomStyle", isUseCustomStyle } });
        }

        /// <summary>
        /// 更新皮肤设置
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <param name="enableCustomStyle"></param>
        /// <returns></returns>
        public string _ChangeThemeAppearance(string presentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace, long ownerId = 0)
        {
            return CachedUrlHelper.Action("_ChangeThemeAppearance", "Channel", CommonAreaName, new RouteValueDictionary() { { "presentAreaKey", presentAreaKey }, { "ownerId", ownerId } });
        }

        /// <summary>
        /// 自定义皮肤设置
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string _CustomSettings(string presentAreaKey = PresentAreaKeysOfBuiltIn.UserSpace, long ownerId = 0, bool isUseCustomStyle = false)
        {
            return CachedUrlHelper.Action("_CustomSettings", "Channel", CommonAreaName, new RouteValueDictionary() { { "presentAreaKey", presentAreaKey }, { "ownerId", ownerId }, { "isUseCustomStyle", isUseCustomStyle } });
        }

        /// <summary>
        /// 上传背景图
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string _UploadBackgroundImage(string presentAreaKey, long ownerId)
        {
            return CachedUrlHelper.Action("_UploadBackgroundImage", "Channel", CommonAreaName, new RouteValueDictionary() { { "presentAreaKey", presentAreaKey }, { "ownerId", ownerId } });
        }

        /// <summary>
        /// 获取背景图地址
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string _BackgroundImageUrl(string presentAreaKey, long ownerId)
        {
            return new Spacebuilder.UI.CustomStyleService().GetBackgroundImageDirectlyUrl(presentAreaKey, ownerId);
        }

        #endregion

        #region 个人资料

        /// <summary>
        /// 用户个人资料显示
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <returns></returns>
        public string PersonalInformation(string spaceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
            {
                dic.Add("spaceKey", spaceKey);
            }
            return CachedUrlHelper.Action("PersonalInformation", "UserSpace", CommonAreaName, dic);
        }

        #endregion

        #endregion

        #region 用户空间设置（UserSpaceSettings）

        #region 个人标签

        /// <summary>
        /// 创建关联个人标签
        /// </summary>
        /// <param name="spaceKey">用户spaceKey</param>
        public string AddTagToItem(string spaceKey)
        {
            return CachedUrlHelper.Action("AddTagToItem", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 编辑用户资料-个人标签
        /// </summary>
        public string EditUserTags(string spaceKey)
        {
            return CachedUrlHelper.Action("EditUserTags", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 编辑用户资料-删除个人标签
        /// </summary>
        public string DeleteMyUserTag(string spaceKey, long itemInTagId)
        {
            return CachedUrlHelper.Action("DeleteMyUserTag", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "itemInTagId", itemInTagId } });
        }

        /// <summary>
        /// 编辑用户资料-用户个人标签控件
        /// </summary>
        public string _ListMyUserTags(string spaceKey)
        {
            return CachedUrlHelper.Action("_ListMyUserTags", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 管理我的标签，根据租户类型id查找
        /// </summary>
        public string ManageMyTags(string spaceKey, string tenantTypeId)
        {
            return CachedUrlHelper.Action("ManageMyTags", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "tenantTypeId", tenantTypeId } });
        }

        /// <summary>
        /// 创建标签页
        /// </summary>
        public string _CreateMyTag(string spaceKey, string tenantTypeId)
        {
            return CachedUrlHelper.Action("_CreateMyTag", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "tenantTypeId", tenantTypeId } });
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        public string _DeleteMyTags(string spaceKey, string tenantTypeId, long? tagIds = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            dic.Add("tenantTypeId", tenantTypeId);
            if (tagIds.HasValue)
            {
                dic.Add("tagIds", tagIds);
            }
            return CachedUrlHelper.Action("_DeleteMyTags", "UserSpaceSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 编辑标签页 及 提交表单
        /// </summary>
        public string _EditMyTag(string spaceKey, long tagId, string tagName, string tenantTypeId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("tagId", tagId);
            dic.Add("tagName", WebUtility.UrlEncode(tagName));
            dic.Add("tenantTypeId", tenantTypeId);
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_EditMyTag", "UserSpaceSettings", CommonAreaName, dic);
        }
        #endregion 个人标签

        #region 屏蔽用户

        /// <summary>
        /// 屏蔽用户页面
        /// </summary>
        /// <returns>屏蔽用户的链接</returns>
        public string BlockUsers(string spaceKey, string blockUserIds = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(blockUserIds))
                routeValueDictionary.Add("blockUserIds", blockUserIds);
            return CachedUrlHelper.Action("BlockUsers", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 解除屏蔽链接
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="UnBlockId">解除对象id</param>
        /// <returns>解除屏蔽链接</returns>
        public string UnBlock(string spaceKey, long? UnBlockId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (UnBlockId.HasValue)
                routeValueDictionary.Add("UnBlockId", UnBlockId);
            return CachedUrlHelper.Action("UnBlock", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }

        #endregion 屏蔽用户

        #region 黑名单

        /// <summary>
        /// 黑名单
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>黑名单链接</returns>
        public string Blacklist(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("Blacklist", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 从黑名单中移除
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <param name="deleteUserId">被移除的对象</param>
        /// <returns>从黑名单中移除的链接</returns>
        public string DeleteStopedUser(string spaceKey, long? deleteUserId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (deleteUserId.HasValue)
                routeValueDictionary.Add("deleteUserId", deleteUserId);
            return CachedUrlHelper.Action("DeleteStopedUser", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }

        #endregion 黑名单

        #region 分类管理

        /// <summary>
        /// 管理我的标签，根据租户类型id查找
        /// </summary>
        public string ManageMyCategories(string spaceKey, string tenantTypeId)
        {
            return CachedUrlHelper.Action("ManageMyCategories", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "tenantTypeId", tenantTypeId } });
        }

        /// <summary>
        /// 创建用户分类页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="callback">Javascript回调函数</param>
        /// <returns></returns>
        public string _CreateMyCategory(string spaceKey, string tenantTypeId, string callback = "")
        {
            return CachedUrlHelper.Action("_CreateMyCategory", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "tenantTypeId", tenantTypeId }, { "callback", callback } });
        }

        /// <summary>
        /// 创建用户分类页，提交表单
        /// </summary>
        public string _CreateMyCategory(string spaceKey, string tenantTypeId)
        {
            return CachedUrlHelper.Action("_CreateMyCategory", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "tenantTypeId", tenantTypeId } });
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        public string _DeleteMyCategories(string spaceKey, long? categoryIds = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (categoryIds.HasValue)
            {
                dic.Add("categoryIds", categoryIds);
            }
            return CachedUrlHelper.Action("_DeleteMyCategories", "UserSpaceSettings", CommonAreaName, dic);
        }
        /// <summary>
        /// 编辑分类页 及 提交表单
        /// </summary>
        public string _EditMyCategory(string spaceKey, long categoryId, string categoryName)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("categoryId", categoryId);
            dic.Add("categoryName", WebUtility.UrlEncode(categoryName));
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_EditMyCategory", "UserSpaceSettings", CommonAreaName, dic);
        }
        #endregion

        #region 应用管理

        /// <summary>
        /// 应用管理页面
        /// </summary>
        /// <returns>应用管理页面</returns>
        public string UserManageApplications(string spaceKey)
        {
            return CachedUrlHelper.Action("ManageApplications", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 添加应用页面
        /// </summary>
        /// <returns>添加应用页面</returns>
        public string UserAddApplication(string spaceKey)
        {
            return CachedUrlHelper.Action("AddApplication", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 安装应用
        /// </summary>
        /// <returns>安装应用</returns>
        public string InstallApplication(string spaceKey, int applicationId)
        {
            return CachedUrlHelper.Action("InstallApplication", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "applicationId", applicationId } });
        }

        /// <summary>
        /// 卸载应用
        /// </summary>
        /// <returns>卸载应用</returns>
        public string UnInstallApplication(string spaceKey, int applicationId)
        {
            return CachedUrlHelper.Action("UnInstallApplication", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "applicationId", applicationId } });
        }


        #endregion 应用管理

        #region 呈现区域导航管理

        public string ManagePresentAreaNavigations(string spaceKey)
        {
            return CachedUrlHelper.Action("ManagePresentAreaNavigations", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除呈现区域导航
        /// </summary>
        public string DeletePresentAreaNavigation(string spaceKey, long id)
        {
            return CachedUrlHelper.Action("DeletePresentAreaNavigation", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "id", id } });
        }

        /// <summary>
        /// 更改显示顺序
        /// </summary>
        /// <returns></returns>
        public string ChangePresentAreaNavigationOrder(string spaceKey)
        {
            return CachedUrlHelper.Action("ChangePresentAreaNavigationOrder", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        public string ResetPresentAreaNavigation(string spaceKey)
        {
            return CachedUrlHelper.Action("ResetPresentAreaNavigation", "UserSpaceSettings", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 编辑、创建呈现区域导航(页面)
        /// </summary>
        public string _EditPresentAreaNavigation(string spaceKey, long? id)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (id.HasValue)
            {
                dic.Add("id", id.Value);
            }
            return CachedUrlHelper.Action("_EditPresentAreaNavigation", "UserSpaceSettings", CommonAreaName, dic);
        }

        #endregion

        /// <summary>
        /// 编辑、创建呈现区域导航(页面)
        /// </summary>
        public string UserModerated(string spaceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("UserModerated", "UserSpaceSettings", CommonAreaName, dic);
        }

        #endregion

        #region 频道（Channel）

        /// <summary>
        /// 简单首页
        /// </summary>
        /// <returns>简单首页</returns>
        public string SimpleHome()
        {
            return CachedUrlHelper.Action("SimpleHome", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 广场首页动态
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public string _ActivitiesList()
        {
            return CachedUrlHelper.Action("_ActivitiesList", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 标签云
        /// </summary>
        /// <param name="tenantTypeIds">租户类型ID</param>
        /// <param name="num">取的条数</param>
        /// <returns></returns>
        public string TagCloud(string tenantTypeId = "", int num = 20)
        {
            return CachedUrlHelper.Action("_TagCloud", "Channel", CommonAreaName, new RouteValueDictionary { { "tenantTypeId", tenantTypeId }, { "num", num } });
        }


        #region 用户卡片
        /// <summary>
        /// 用户卡片
        /// </summary>
        public string _UserCard(long userId)
        {
            return CachedUrlHelper.Action("_UserCard", "Channel", CommonAreaName, new RouteValueDictionary { { "userId", userId } });
        }

        /// <summary>
        /// 是否显示拉黑标识
        /// </summary>
        public string _IsStopped(long userId)
        {
            return CachedUrlHelper.Action("_IsStopped", "Channel", CommonAreaName, new RouteValueDictionary { { "userId", userId } });
        }
        #endregion

        #region 找人频道

        /// <summary>
        /// 频道首页
        /// </summary>
        /// <returns></returns>
        public string FindUserHome()
        {
            return CachedUrlHelper.Action("Home", "FindUser", CommonAreaName);
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        /// <returns></returns>
        public string FindUserRanking(SortBy_User sortBy, int pageIndex = 1)
        {
            return CachedUrlHelper.Action("Ranking", "FindUser", CommonAreaName, new RouteValueDictionary { { "sortBy", sortBy }, { "pageIndex", pageIndex } });
        }

        /// <summary>
        /// 可能感兴趣的人
        /// </summary>
        public string FindUserInterested()
        {
            return CachedUrlHelper.Action("Interested", "FindUser", CommonAreaName);
        }

        /// <summary>
        /// 可能感兴趣的人-有共同关注的人
        /// </summary>
        public string _InterestedWithFollows()
        {
            return CachedUrlHelper.Action("_InterestedWithFollows", "FindUser", CommonAreaName);
        }

        /// <summary>
        /// 可能感兴趣的人-使用了相同标签
        /// </summary>
        public string _InterestedWithTags()
        {
            return CachedUrlHelper.Action("_InterestedWithTags", "FindUser", CommonAreaName);
        }

        /// <summary>
        /// 可能感兴趣的人-供职于同一公司
        /// </summary>
        public string _InterestedWithCompanys()
        {
            return CachedUrlHelper.Action("_InterestedWithCompanys", "FindUser", CommonAreaName);
        }

        /// <summary>
        /// 可能感兴趣的人-毕业于同一学校
        /// </summary>
        public string _InterestedWithSchools()
        {
            return CachedUrlHelper.Action("_InterestedWithSchools", "FindUser", CommonAreaName);
        }

        /// <summary>
        /// 可能感兴趣的人-混合模式-主区域
        /// </summary>
        public string _InterestedWithAll()
        {
            return CachedUrlHelper.Action("_InterestedWithAll", "FindUser", CommonAreaName);
        }

        /// <summary>
        /// 可能感兴趣的人-混合模式-侧边栏
        /// </summary>
        public string _InterestedWithAllSide()
        {
            return CachedUrlHelper.Action("_InterestedWithAllSide", "FindUser", CommonAreaName);
        }



        #endregion 找人频道

        #region 通用评论

        /// <summary>
        /// 获取一条
        /// </summary>
        /// <param name="id">评论的id</param>
        /// <returns></returns>
        public string _OneComment(long? id = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (id.HasValue)
                routeValueDictionary.Add("id", id);
            return CachedUrlHelper.Action("_OneComment", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// （通用）评论列表
        /// </summary>
        /// <param name="tenantType">评论的租户类型id</param>
        /// <param name="commentedObjectId">被评论对象id</param>
        /// <param name="sortBy">排序方式</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>评论列表</returns>
        public string _CommentList(string tenantType, long commentedObjectId, SortBy_Comment sortBy = SortBy_Comment.DateCreated, int pageIndex = 1, bool showBefor = true, bool showAfter = false)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("tenantType", tenantType);
            routeValueDictionary.Add("commentedObjectId", commentedObjectId);
            if (sortBy != SortBy_Comment.DateCreated)
                routeValueDictionary.Add("sortBy", sortBy);
            if (pageIndex != 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (!showBefor)
                routeValueDictionary.Add("showBefor", showBefor);
            if (showAfter)
                routeValueDictionary.Add("showAfter", showAfter);
            return CachedUrlHelper.Action("_CommentList", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 子级评论局部页面（第一次的时候使用）
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>子级评论局部页面的链接</returns>
        public string _ChildComment(long? parentId = null, bool enableComment = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (parentId.HasValue)
                routeValueDictionary.Add("parentId", parentId);

            if (!enableComment)
                routeValueDictionary.Add("enableComment", enableComment);

            return CachedUrlHelper.Action("_ChildComment", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// （通用）子级评论列表
        /// </summary>
        /// <param name="parentId">父级评论列表id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>排序方式</returns>
        public string _ChildCommentList(long parentId, int pageIndex = 1, SortBy_Comment sortBy = SortBy_Comment.DateCreatedDesc, bool showBefor = true, bool showAfter = false)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("parentId", parentId);
            if (pageIndex > 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (sortBy != SortBy_Comment.DateCreatedDesc)
                routeValueDictionary.Add("sortBy", sortBy);
            if (!showBefor)
                routeValueDictionary.Add("showBefor", showBefor);
            if (showAfter)
                routeValueDictionary.Add("showAfter", showAfter);
            return CachedUrlHelper.Action("_ChildCommentList", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// (通用)删除评论
        /// </summary>
        /// <returns></returns>
        public string _DeleteComment(long commentId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("commentId", commentId);
            return CachedUrlHelper.Action("_DeleteComment", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 评论控件
        /// </summary>
        public string _Comment(long commentedObjectId, long ownerId, string tenantTypeId)
        {
            return CachedUrlHelper.Action("_Comment", "Channel", CommonAreaName, new RouteValueDictionary { { "commentedObjectId", commentedObjectId }, { "ownerId", ownerId }, { "tenantTypeId", tenantTypeId } });
        }

        #endregion

        #region 搜索

        #region 用户搜索

        /// <summary>
        /// 用户搜索
        /// </summary>
        public string UserSearch(string keyword = "", UserSearchRange usr = UserSearchRange.ALL)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                routeValueDictionary.Add("keyword", WebUtility.UrlEncode(keyword));
            }
            if (usr != UserSearchRange.ALL)
            {
                routeValueDictionary.Add("SearchRange", usr);
            }

            return CachedUrlHelper.Action("UserSearch", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 用户全局搜索
        /// </summary>
        public string UserGolbalSearch()
        {
            return CachedUrlHelper.Action("_UserGlobalSearch", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 用户快捷搜索
        /// </summary>
        /// <returns></returns>
        public string UserQuickSearch()
        {
            return CachedUrlHelper.Action("_UserQuickSearch", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 用户搜索自动完成
        /// </summary>
        /// <returns></returns>
        public string UserSearchAutoComplete()
        {
            return CachedUrlHelper.Action("UserSearchAutoComplete", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 根据地区编码获取下一节地区子节点
        /// </summary>
        /// <returns></returns>
        public string GetAreasByParentCode()
        {
            return CachedUrlHelper.Action("GetAreasByParentCode", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 获取搜索热词
        /// </summary>
        /// <returns></returns>
        public string GetHotWords(int topNumber)
        {
            return CachedUrlHelper.Action("GetHotWords", "Channel", CommonAreaName, new RouteValueDictionary() { { "topNumber", topNumber } });
        }

        /// <summary>
        /// 获取搜索历史
        /// </summary>
        /// <returns></returns>
        public string GetUserSearchHistories()
        {
            return CachedUrlHelper.Action("GetUserSearchHistories", "Channel", CommonAreaName);
        }
        #endregion

        #region 全局搜索

        /// <summary>
        /// 全局搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GlobalSearch(string keyword)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                dic.Add("keyword", keyword);
            }
            return CachedUrlHelper.Action("GlobalSearch", "Channel", CommonAreaName, dic);
        }

        /// <summary>
        /// 全局搜索自动完成
        /// </summary>
        /// <returns></returns>
        public string GlobalAutoComplete()
        {
            return CachedUrlHelper.Action("GlobalAutoComplete", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 获取全局搜索热词
        /// </summary>
        /// <returns></returns>
        public string GetGlobalHotWords(int topNumber)
        {
            return CachedUrlHelper.Action("GetGlobalHotWords", "Channel", CommonAreaName, new RouteValueDictionary() { { "topNumber", topNumber } });
        }

        /// <summary>
        /// 获取全局搜索历史
        /// </summary>
        /// <returns></returns>
        public string GetGlobalSearchHistories()
        {
            return CachedUrlHelper.Action("GetGlobalSearchHistories", "Channel", CommonAreaName);
        }

        #endregion

        /// <summary>
        /// 搜索热词、搜索历史
        /// </summary>
        /// <param name="searcherCode"></param>
        /// <param name="clear"></param>
        /// <returns></returns>
        public string SearchHistories(string searcherCode, bool clear = false)
        {
            return CachedUrlHelper.Action("_SearchHistories", "Channel", CommonAreaName, new RouteValueDictionary { { "searcherCode", searcherCode }, { "clear", clear } });
        }

        #endregion

        #endregion

        #region 后台管理（ControlPanel）

        /// <summary>
        /// 后台登录页面
        /// </summary>
        public string ManageLogin()
        {
            HttpContext httpContext = HttpContext.Current;
            string currentPath = httpContext.Request.Url.PathAndQuery;
            string returnUrl = SiteUrls.ExtractQueryParams(currentPath)["ReturnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = WebUtility.UrlEncode(HttpContext.Current.Request.RawUrl);     //把ReturnUrl进行 UrlEncode

            return CachedUrlHelper.Action("ManageLogin", "ControlPanel", CommonAreaName, new RouteValueDictionary() { { "ReturnUrl", returnUrl } });
        }

        /// <summary>
        /// 用户管理页面
        /// </summary>
        public string ManageHome()
        {
            return CachedUrlHelper.Action("Home", "ControlPanel", CommonAreaName);
        }

        /// <summary>
        /// 获取最新产品版本
        /// </summary>
        public string _GetMostRecentVersion()
        {
            return CachedUrlHelper.Action("GetMostRecentVersion", "ControlPanel", CommonAreaName);
        }

        /// <summary>
        /// 改变推荐顺序
        /// </summary>
        /// <returns></returns>
        public string _ChangeRecommendOrder()
        {
            return CachedUrlHelper.Action("_ChangeRecommendOrder", "ControlPanel", CommonAreaName);
        }

        #region 后台功能地图、常用操作、功能搜索

        /// <summary>
        /// 功能地图
        /// </summary>
        /// <returns></returns>
        public string _FunctionMap()
        {
            return CachedUrlHelper.Action("_FunctionMap", "ControlPanel", CommonAreaName);
        }

        /// <summary>
        /// 更新常用操作页
        /// </summary>
        /// <returns></returns>
        public string UpdateCommonOperations()
        {
            return CachedUrlHelper.Action("UpdateCommonOperations", "ControlPanel", CommonAreaName);
        }

        /// <summary>
        /// 添加常用操作
        /// </summary>
        /// <returns></returns>
        public string AddCommonOperations(int? navigationId)
        {
            return CachedUrlHelper.Action("UpdateCommonOperations", "ControlPanel", CommonAreaName, new RouteValueDictionary() { { "id", navigationId } });
        }

        /// <summary>
        /// 功能搜索
        /// </summary>
        /// <returns></returns>
        public string SearchOperations()
        {
            return CachedUrlHelper.Action("SearchOperations", "ControlPanel", CommonAreaName);
        }
        #endregion

        #region 界面设置

        /// <summary>
        /// 应用管理页面
        /// </summary>
        public string ManageApplications()
        {
            return CachedUrlHelper.Action("ManageApplications", "ControlPanelSettings", CommonAreaName);
        }
        /// <summary>
        /// 应用管理页面的启用/禁用按钮
        /// </summary>
        public string SetApplicationStatus(int applicationId)
        {
            return CachedUrlHelper.Action("SetApplicationStatus", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary { { "applicationId", applicationId } });
        }

        /// <summary>
        /// 皮肤管理
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns></returns>
        public string ManageThemes(string presentAreaKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("presentAreaKey", presentAreaKey);
            return CachedUrlHelper.Action("ManageThemes", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 皮肤管理
        /// </summary>
        /// <returns></returns>
        public string ManageThemes()
        {
            return CachedUrlHelper.Action("ManageThemes", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 导入皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns></returns>
        public string _ExtractTheme(string presentAreaKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("presentAreaKey", presentAreaKey);
            return CachedUrlHelper.Action("_ExtractTheme", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 更改皮肤顺序
        /// </summary>
        /// <returns></returns>
        public string _ChangeThemeOrder()
        {
            return CachedUrlHelper.Action("_ChangeThemeOrder", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 设置默认皮肤
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="themeKey"></param>
        /// <param name="appearanceKey"></param>
        /// <returns></returns>
        public string _SetDefaultThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("presentAreaKey", presentAreaKey);
            dic.Add("themeKey", themeKey);
            dic.Add("appearanceKey", appearanceKey);
            return CachedUrlHelper.Action("_SetDefaultThemeAppearance", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 锁定或解锁皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeKey">themeKey</param>
        /// <param name="appearanceKey">appearanceKey</param>
        /// <param name="isLocked">是否锁定</param>
        /// <returns></returns>
        public string _LockThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey, bool isLocked)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("presentAreaKey", presentAreaKey);
            dic.Add("themeKey", themeKey);
            dic.Add("appearanceKey", appearanceKey);
            dic.Add("isLocked", isLocked);
            return CachedUrlHelper.Action("_LockThemeAppearance", "ControlPanelSettings", CommonAreaName, dic);
        }


        /// <summary>
        /// 启用禁用皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeKey">themeKey</param>
        /// <param name="appearanceKey">appearanceKey</param>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        public string _EnableThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey, bool isEnabled)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("presentAreaKey", presentAreaKey);
            dic.Add("themeKey", themeKey);
            dic.Add("appearanceKey", appearanceKey);
            dic.Add("isEnabled", isEnabled);
            return CachedUrlHelper.Action("_EnableThemeAppearance", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeKey"></param>
        /// <param name="appearanceKey"></param>
        /// <returns></returns>
        public string _DeleteThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("presentAreaKey", presentAreaKey);
            dic.Add("themeKey", themeKey);
            dic.Add("appearanceKey", appearanceKey);
            return CachedUrlHelper.Action("_DeleteThemeAppearance", "ControlPanelSettings", CommonAreaName, dic);
        }

        #endregion 界面设置

        #region 用户管理

        /// <summary>
        /// 用户管理页面
        /// </summary>
        public string ManageUsers()
        {
            return CachedUrlHelper.Action("ManageUsers", "ControlPanelUser", CommonAreaName);
        }

        /// <summary>
        /// 用户管理-批量激活、取消激活用户
        /// </summary>
        public string ActivatedUser(bool isActivated)
        {
            return CachedUrlHelper.Action("ActivatedUsers", "ControlPanelUser", CommonAreaName, new RouteValueDictionary() { { "isActivated", isActivated } });
        }

        /// <summary>
        /// 用户管理-批量取消封禁用户
        /// </summary>
        public string UnbanUser(string returnUrl)
        {
            return CachedUrlHelper.Action("UnbanUsers", "ControlPanelUser", CommonAreaName, new RouteValueDictionary() { { "returnUrl", returnUrl } });
        }

        /// <summary>
        /// 用户管理-批量删除用户
        /// </summary>
        public string DeleteUsers()
        {
            return CachedUrlHelper.Action("DeleteUsers", "ControlPanelUser", CommonAreaName);
        }

        /// <summary>
        /// 用户管理-添加新用户
        /// </summary>
        public string _CreateUser()
        {
            return CachedUrlHelper.Action("_CreateUser", "ControlPanelUser", CommonAreaName);
        }

        /// <summary>
        /// 用户管理-封禁用户
        /// </summary>
        public string _BannedUser()
        {
            return CachedUrlHelper.Action("_BannedUser", "ControlPanelUser", CommonAreaName);
        }

        /// <summary>
        /// 用户管理-删除用户
        /// </summary>
        public string _DeleteUser(long? userId)
        {
            return CachedUrlHelper.Action("_DeleteUser", "ControlPanelUser", CommonAreaName, new RouteValueDictionary() { { "userId", userId } });
        }

        /// <summary>
        /// 用户管理-删除用户
        /// </summary>
        public string EditUser(long userId)
        {
            return CachedUrlHelper.Action("EditUser", "ControlPanelUser", CommonAreaName, new RouteValueDictionary() { { "userId", userId } });
        }

        /// <summary>
        /// 用户管理-设置用户角色
        /// </summary>
        public string _SetUserRoles(long userId, string returnUrl)
        {
            return CachedUrlHelper.Action("_SetUserRoles", "ControlPanelUser", CommonAreaName, new RouteValueDictionary() { { "userId", userId }, { "returnUrl", returnUrl } });
        }

        /// <summary>
        /// 用户管理-搜索用户
        /// </summary>
        public string SeachUser(bool? isActivated, bool? isBanned, bool? isModerated)
        {
            return CachedUrlHelper.Action("ManageUsers", "ControlPanelUser", CommonAreaName, new RouteValueDictionary() { { "IsActivated", isActivated }, { "IsBanned", isBanned }, { "IsModerated", isModerated } });
        }

        /// <summary>
        /// 重设用户密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string _ResetUserPassword(long userId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (userId > 0)
            {
                dic.Add("userId", userId);
            }
            return CachedUrlHelper.Action("_ResetUserPassword", "ControlPanelUser", CommonAreaName, dic);
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string ValidateEmail(string email, long userId)
        {
            return CachedUrlHelper.Action("ValidateEmail", "ControlPanelUser", CommonAreaName, new RouteValueDictionary { { "email", email }, { "userId", userId } });
        }




        #endregion 用户管理

        #region 积分管理

        /// <summary>
        /// 积分纪录管理
        /// </summary>
        public string ManagePointRecords()
        {
            return CachedUrlHelper.Action("ManagePointRecords", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 积分规则管理
        /// </summary>
        public string ManagePointItems()
        {
            return CachedUrlHelper.Action("ManagePointItems", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 积分管理-修改积分设置
        /// </summary>
        public string _EditPointSettings()
        {
            return CachedUrlHelper.Action("_EditPointSettings", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 积分管理-修改积分规则
        /// </summary>
        public string _EditPointRules(string itemKey)
        {
            return CachedUrlHelper.Action("_EditPointRules", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "itemKey", itemKey } });
        }

        #endregion 积分管理

        #region 角色管理

        /// <summary>
        /// 用户角色管理页面
        /// </summary>
        public string ManageUserRoles()
        {
            return CachedUrlHelper.Action("ManageUserRoles", "ControlPanelUser", CommonAreaName);
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        public string _CreateRole(string roleName)
        {
            return CachedUrlHelper.Action("_CreateRole", "ControlPanelUser", CommonAreaName, new RouteValueDictionary() { { "roleName", roleName } });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        public string DeleteRole(string roleName)
        {
            return CachedUrlHelper.Action("DeleteRole", "ControlPanelUser", CommonAreaName, new RouteValueDictionary() { { "roleName", roleName } });
        }

        #endregion 色管理

        #region 审核规则管理

        /// <summary>
        /// 审核规则管理
        /// </summary>
        public string ManageAuditItems()
        {
            return CachedUrlHelper.Action("ManageAuditItems", "ControlPanelSettings", CommonAreaName);
        }

        #endregion 审核规则管理

        #region 权限规则管理

        /// <summary>
        /// 权限规则管理
        /// </summary>
        public string ManagePermissionItems()
        {
            return CachedUrlHelper.Action("ManagePermissionItems", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        public string SetRolePermission(string roleName)
        {
            return CachedUrlHelper.Action("ManagePermissionItemsInUserRoles", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "roleName", roleName } });
        }

        #endregion 权限规则管理

        #region 搜索热词管理

        /// <summary>
        /// 搜索热词管理
        /// </summary>
        public string ManageSearchedTerms()
        {
            return CachedUrlHelper.Action("ManageSearchedTerms", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 编辑热词
        /// </summary>
        public string _EditSearchedTerms(long id, string term, string SearchTypeCode)
        {
            return CachedUrlHelper.Action("_EditSearchedTerms", "ControlPanelOperation", CommonAreaName, new RouteValueDictionary() { { "id", id }, { "term", term }, { "SearchTypeCode", SearchTypeCode } });
        }

        /// <summary>
        /// 删除热词
        /// </summary>
        /// <returns></returns>
        public string DeleteSearchedTerms(long id)
        {
            return CachedUrlHelper.Action("DeleteSearchedTerms", "ControlPanelOperation", CommonAreaName, new RouteValueDictionary() { { "id", id } });
        }

        /// <summary>
        /// 交换热词顺序
        /// </summary>
        public string ChangeDisplayOrder()
        {
            return CachedUrlHelper.Action("ChangeDisplayOrder", "ControlPanelOperation", CommonAreaName);
        }
        #endregion 搜索热词管理

        #region 索引管理
        /// <summary>
        /// 索引管理
        /// </summary>
        public string ManageIndex()
        {
            return CachedUrlHelper.Action("ManageIndex", "ControlPanelTool", CommonAreaName);
        }

        /// <summary>
        /// 重建索引
        /// </summary>
        /// <param name="code">CODE</param>
        /// <returns></returns>
        public string RebuildIndex(string code)
        {
            return CachedUrlHelper.Action("RebuildIndex", "ControlPanelTool", CommonAreaName, new RouteValueDictionary() { { "code", code } });
        }

        /// <summary>
        /// 正在执行
        /// </summary>
        /// <param name="message">显示信息</param>
        /// <param name="backUrl">返回上一页</param>
        /// <param name="operationUrl">要执行的url</param>
        /// <returns></returns>
        public string ControlPanelOperating(string message, string backUrl, string operationUrl, string confirmInfo = null)
        {
            return CachedUrlHelper.Action("Operating", "ControlPanel", CommonAreaName, new RouteValueDictionary() { { "message", WebUtility.UrlEncode(message) }, { "backUrl", WebUtility.UrlEncode(backUrl) }, { "operationUrl", WebUtility.UrlEncode(operationUrl) }, { "confirmInfo", WebUtility.UrlEncode(confirmInfo ?? string.Empty) } });
        }

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="message">显示信息</param>
        /// <param name="backUrl">返回上一页</param>
        /// <returns></returns>
        public string ControlPanelSuccess(string message, string backUrl)
        {
            return CachedUrlHelper.Action("Success", "ControlPanel", CommonAreaName, new RouteValueDictionary() { { "message", WebUtility.UrlEncode(message) }, { "backUrl", WebUtility.UrlEncode(backUrl) } });
        }

        /// <summary>
        /// 执行失败
        /// </summary>
        /// <param name="message">显示信息</param>
        /// <param name="operationUrl">重新执行</param>
        /// <param name="backUrl">返回上一页</param>
        /// <returns></returns>
        public string ControlPanelError(string message, string operationUrl, string backUrl)
        {
            return CachedUrlHelper.Action("Error", "ControlPanel", CommonAreaName, new RouteValueDictionary() { { "message", message }, { "operationUrl", operationUrl }, { "backUrl", backUrl } });
        }

        #endregion

        #region 分类管理

        #region 站点类别管理



        /// <summary>
        /// 站点分类
        /// </summary>
        /// <param name="tenantTypeId">租户ID</param>
        /// <param name="level">层级（从1开始）</param>
        /// <returns></returns>
        public string ManageSiteCategories(string tenantTypeId = null, int level = int.MaxValue)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(tenantTypeId))
                dic.Add("tenantTypeId", tenantTypeId);
            if (level != int.MaxValue)
            {
                dic.Add("level", level);
            }
            return CachedUrlHelper.Action("ManageSiteCategories", "ControlPanelContent", CommonAreaName, dic);
        }

        /// <summary>
        /// 更改显示顺序
        /// </summary>
        /// <returns></returns>
        public string ChangeSiteCategoryOrder()
        {
            return CachedUrlHelper.Action("ChangeSiteCategoryOrder", "ControlPanelContent", CommonAreaName);
        }

        /// <summary>
        /// 删除站点分类
        /// </summary>
        /// <param name="categoryId">分类ID</param>
        /// <returns></returns>
        public string DeleteSiteCategory(long categoryId)
        {
            return CachedUrlHelper.Action("DeleteSiteCategory", "ControlPanelContent", CommonAreaName, new RouteValueDictionary() { { "CategoryId", categoryId } });
        }

        /// <summary>
        /// 添加编辑类别页
        /// </summary>
        public string _EditSiteCategory(string TenantTypeId = null, long CategoryId = 0, long ParentId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(TenantTypeId))
            {
                dic.Add("TenantTypeId", TenantTypeId);
            }

            if (CategoryId != 0)
            {
                dic.Add("CategoryId", CategoryId);
            }

            if (ParentId != 0)
            {
                dic.Add("ParentId", ParentId);
            }

            return CachedUrlHelper.Action("_EditSiteCategory", "ControlPanelContent", CommonAreaName, dic);
        }
        /// <summary>
        /// 添加编辑分类
        /// </summary>
        /// <param name="category">分类实体</param>
        /// <returns></returns>
        public string _EditSiteCategory(CategoryEditModel category)
        {
            return CachedUrlHelper.Action("_EditSiteCategory", "ControlPanelContent", CommonAreaName, new RouteValueDictionary() { { "Category", category } });
        }

        /// <summary>
        /// 合并移动站点分类页
        /// </summary>
        public string _MoveSiteCategory(long fromCategoryId = 0, string option = "move")
        {
            return CachedUrlHelper.Action("_MoveSiteCategory", "ControlPanelContent", CommonAreaName, new RouteValueDictionary { { "fromCategoryId", fromCategoryId }, { "option", option } });
        }

        /// <summary>
        /// 合并移动站点分类
        /// </summary>
        public string _MoveSiteCategory(long fromCategoryId = 0, long CategoryId = 0, string option = "move")
        {
            return CachedUrlHelper.Action("_MoveSiteCategory", "ControlPanelContent", CommonAreaName, new RouteValueDictionary { { "fromCategoryId", fromCategoryId }, { "CategoryId", CategoryId }, { "option", option } });
        }

        #endregion 站点类别管理

        #region 导航管理
        /// <summary>
        /// 管理导航
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <returns></returns>
        public string ManageNavigations(string presentAreaKey = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (presentAreaKey != null)
            {
                dic.Add("presentAreaKey", presentAreaKey);
            }
            return CachedUrlHelper.Action("ManageNavigations", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 添加导航
        /// </summary>
        public string _CreateNavigation(int parentNavigationId, int? applicationId, string presentAreaKey = PresentAreaKeysOfBuiltIn.Channel)
        {
            return CachedUrlHelper.Action("_CreateNavigation", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary { { "parentNavigationId", parentNavigationId }, { "applicationId", applicationId }, { "presentAreaKey", presentAreaKey } });
        }

        /// <summary>
        /// 编辑导航
        /// </summary>
        /// <param name="navigationId"></param>
        /// <returns></returns>
        public string _EditNavigation(int? navigationId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (navigationId != null)
            {
                dic.Add("navigationId", navigationId);
            }
            return CachedUrlHelper.Action("_EditNavigation", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 更改显示顺序
        /// </summary>
        public string ChangeNavigationOrder()
        {
            return CachedUrlHelper.Action("ChangeNavigationOrder", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 设置导航状态
        /// </summary>
        public string setNavigationStatus()
        {
            return CachedUrlHelper.Action("setNavigationStatus", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 删除导航
        /// </summary>
        public string _DeleteNavigation(int navigationId)
        {
            return CachedUrlHelper.Action("_DeleteNavigation", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary { { "navigationId", navigationId } });
        }

        /// <summary>
        /// 批量删除导航
        /// </summary>
        public string _BatchRemoveNavigation()
        {
            return CachedUrlHelper.Action("_BatchRemoveNavigation", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 查询子导航
        /// </summary>
        /// <param name="navigationId"></param>
        /// <returns></returns>
        public string ManageChildNavigations(int navigationId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("navigationId", navigationId);
            return CachedUrlHelper.Action("ManageChildNavigations", "ControlPanelSettings", CommonAreaName, dic);
        }
        #endregion

        #region 快捷操作
        /// <summary>
        /// 快捷操作
        /// </summary>
        public string ManageQuickOperations(ManagementOperationType? operationType = null, string presentAreaKey = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (operationType != null)
            {
                dic.Add("operationType", operationType);
            }
            if (presentAreaKey != null)
            {
                dic.Add("presentAreaKey", presentAreaKey);
            }

            return CachedUrlHelper.Action("ManageQuickOperations", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 更改显示顺序
        /// </summary>
        public string ChangeOperationOrder()
        {
            return CachedUrlHelper.Action("ChangeOperationOrder", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 删除快捷操作
        /// </summary>
        public string DeleteOperation(int OperationId)
        {
            return CachedUrlHelper.Action("DeleteOperation", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary { { "OperationId", OperationId } });
        }

        /// <summary>
        /// 批量删除快捷操作
        /// </summary>
        public string BatchRemoveOperation()
        {
            return CachedUrlHelper.Action("BatchRemoveOperation", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 设置导航状态
        /// </summary>
        public string setOperationStatus()
        {
            return CachedUrlHelper.Action("setOperationStatus", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 设置是否新开状态
        /// </summary>
        public string setOperationTarget()
        {
            return CachedUrlHelper.Action("setOperationTarget", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 创建快捷操作
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <returns></returns>
        public string _CreateOperation(ManagementOperationType? operationType, string presentAreaKey = PresentAreaKeysOfBuiltIn.Channel)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("presentAreaKey", presentAreaKey);
            dic.Add("operationType", operationType);
            return CachedUrlHelper.Action("_CreateOperation", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 编辑快捷操作
        /// </summary>
        /// <param name="navigationId"></param>
        /// <returns></returns>
        public string _EditOperation(int operationId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (operationId != null)
            {
                dic.Add("operationId", operationId);
            }
            return CachedUrlHelper.Action("_EditOperation", "ControlPanelSettings", CommonAreaName, dic);
        }


        #endregion

        #region 用户类别管理

        /// <summary>
        /// 管理用户分类
        /// </summary>
        /// <returns></returns>
        public string ManageUserCategories(string keyword, string tenantTypeId, AuditStatus? auditStatus = null, long ownerId = 0, int pageIndex = 1)
        {
            return CachedUrlHelper.Action("ManageUserCategories", "ControlPanelContent", CommonAreaName, new RouteValueDictionary() { { "keyword", keyword }, { "tenantTypeId", tenantTypeId }, { "auditStatus", auditStatus }, { "pageIndex", pageIndex }, { "ownerId", ownerId } });
        }

        /// <summary>
        /// 编辑用户分类页
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public string _EditUserCategory(long categoryId = 0)
        {
            return CachedUrlHelper.Action("_EditUserCategory", "ControlPanelContent", CommonAreaName, new RouteValueDictionary() { { "CategoryId", categoryId } });
        }
        /// <summary>
        /// 编辑用户分类
        /// </summary>
        /// <param name="category">分类实体</param>
        /// <returns></returns>
        public string _EditUserCategory(CategoryEditModel category)
        {
            return CachedUrlHelper.Action("_EditUserCategory", "ControlPanelContent", CommonAreaName, new RouteValueDictionary() { { "Category", category } });
        }
        /// <summary>
        /// 批量更改审核状态
        /// </summary>
        /// <returns></returns>
        public string UpdateAuditStatus()
        {
            return CachedUrlHelper.Action("UpdateAuditStatus", "ControlPanelContent", CommonAreaName);
        }

        /// <summary>
        /// 批量删除用户分类
        /// </summary>
        /// <returns></returns>
        public string DeleteUserCategorys(string categoryIds = "")
        {
            if (string.IsNullOrEmpty(categoryIds))
            {
                return CachedUrlHelper.Action("DeleteUserCategorys", "ControlPanelContent", CommonAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("DeleteUserCategorys", "ControlPanelContent", CommonAreaName, new RouteValueDictionary() { { "categoryIds", categoryIds } });
            }

        }
        #endregion 用户类别管理

        #endregion

        #region 评论管理

        /// <summary>
        /// 管理评论
        /// </summary>
        /// <param name="tenantTypeId">租户类型ID</param>
        /// <returns></returns>
        public string ManageComments(string tenantTypeId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (tenantTypeId != null)
            {
                dic.Add("tenantTypeId", tenantTypeId);
            }
            return CachedUrlHelper.Action("ManageComments", "ControlPanelContent", CommonAreaName, dic);
        }
        /// <summary>
        /// 更新评论的审核状态
        /// </summary>
        public string _UpdateCommentAuditStatus(AuditStatus auditStatus)
        {
            return CachedUrlHelper.Action("_UpdateCommentAuditStatus", "ControlPanelContent", CommonAreaName, new RouteValueDictionary() { { "auditStatus", auditStatus } });
        }

        /// <summary>
        /// 批量/单个删除评论
        /// </summary>
        /// <returns></returns>
        public string _DeleteComments(long? commentIds = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (commentIds != null)
            {
                dic.Add("commentIds", commentIds);
            }
            return CachedUrlHelper.Action("_DeleteComments", "ControlPanelContent", CommonAreaName, dic);
        }

        #endregion

        #region 标签管理
        /// <summary>
        /// 标签审核
        /// </summary>
        public string AuditTags()
        {
            return CachedUrlHelper.Action("AuditTags", "ControlPanelContent", CommonAreaName);
        }

        /// <summary>
        /// 添加、编辑标签
        /// </summary>
        public string EditTag(long tagId = 0)
        {
            if (tagId > 0)
            {
                return CachedUrlHelper.Action("EditTag", "ControlPanelContent", CommonAreaName, new RouteValueDictionary { { "tagId", tagId } });
            }
            else
            {
                return CachedUrlHelper.Action("EditTag", "ControlPanelContent", CommonAreaName);
            }
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        public string DeleteTags(string tagIds = "")
        {
            if (string.IsNullOrEmpty(tagIds))
            {
                return CachedUrlHelper.Action("DeleteTags", "ControlPanelContent", CommonAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("DeleteTags", "ControlPanelContent", CommonAreaName, new RouteValueDictionary { { "tagIds", tagIds } });
            }
        }

        /// <summary>
        /// 添加、编辑标签分组
        /// </summary>
        public string _EditTagGroup(long tagGroupId = 0, string tenantTypeId = "")
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

            if (tagGroupId > 0)
            {
                routeValueDictionary.Add("tagGroupId", tagGroupId);
            }
            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                routeValueDictionary.Add("tenantTypeId", tenantTypeId);
            }
            return CachedUrlHelper.Action("_EditTagGroup", "ControlPanelContent", CommonAreaName, routeValueDictionary);

        }

        /// <summary>
        /// 删除标签分租
        /// </summary>
        /// <returns></returns>
        public string DeleteTagGroups(string tagGroupIds = "")
        {
            if (string.IsNullOrEmpty(tagGroupIds))
            {
                return CachedUrlHelper.Action("DeleteTagGroups", "ControlPanelContent", CommonAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("DeleteTagGroups", "ControlPanelContent", CommonAreaName, new RouteValueDictionary { { "tagGroupIds", tagGroupIds } });
            }
        }

        /// <summary>
        /// 管理标签分组页
        /// </summary>
        /// <returns></returns>
        public string ManageTagGroups(string tenantTypeId = "")
        {
            if (string.IsNullOrEmpty(tenantTypeId))
            {
                return CachedUrlHelper.Action("ManageTagGroups", "ControlPanelContent", CommonAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("ManageTagGroups", "ControlPanelContent", CommonAreaName, new RouteValueDictionary { { "tenantTypeId", tenantTypeId } });
            }
        }

        /// <summary>
        /// 管理标签
        /// </summary>
        /// <returns></returns>
        public string ManageTags(string tenantTypeId = "")
        {
            return CachedUrlHelper.Action("ManageTags", "ControlPanelContent", CommonAreaName, new RouteValueDictionary { { "tenantTypeId", tenantTypeId } });
        }

        /// <summary>
        /// 根据租户ID获取分组
        /// </summary>
        /// <returns></returns>
        public string GetTagGroupsByTenantTypeId()
        {
            return CachedUrlHelper.Action("GetTagGroupsByTenantTypeId", "ControlPanelContent", CommonAreaName);
        }

        /// <summary>
        /// 批量设置标签分组
        /// </summary>
        /// <returns></returns>
        public string _SetTagsGroup(string tenantTypeId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                dic.Add("tenantTypeId", tenantTypeId);
            }
            return CachedUrlHelper.Action("_SetTagsGroup", "ControlPanelContent", CommonAreaName, dic);
        }

        #endregion

        #region 用户等级

        /// <summary>
        /// 管理用户等级页面
        /// </summary>
        public string ManageRanks()
        {
            return CachedUrlHelper.Action("ManageRanks", "ControlPanelUser", CommonAreaName);
        }

        /// <summary>
        /// 编辑或创建用户等级
        /// </summary>
        /// <param name="rank">用户等级</param>
        /// <param name="isEdit">true=>编辑,false=>创建</param>
        public string _EditUserRank(int rank = -1, bool isEdit = false)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (rank > 0)
                routeValueDictionary.Add("rank", rank);
            if (isEdit)
                routeValueDictionary.Add("isEdit", isEdit);
            return CachedUrlHelper.Action("_EditUserRank", "ControlPanelUser", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除一个用户等级的链接
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public string DeleteUserRank(int rank = -1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (rank > 0)
                routeValueDictionary.Add("rank", rank);
            return CachedUrlHelper.Action("DeleteUserRank", "ControlPanelUser", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 根据当前等级规则，重新计算所有用户等级
        /// </summary>
        /// <returns></returns>
        public string ResetAllUser()
        {
            return CachedUrlHelper.Action("ResetAllUser", "ControlPanelUser", CommonAreaName);
        }

        #endregion 用户等级

        #region 表情包管理
        /// <summary>
        /// 表情包管理
        /// </summary>
        public string ManageEmotionCategories()
        {
            return CachedUrlHelper.Action("ManageEmotionCategories", "ControlPanelSettings", CommonAreaName);
        }
        /// <summary>
        /// 表情包启用or禁用
        /// </summary>
        public string SetEmotionCategoryStatus(string directoryName = "")
        {
            return CachedUrlHelper.Action("SetEmotionCategoryStatus", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "directoryName", directoryName } });
        }
        /// <summary>
        /// 表情包添加
        /// </summary>
        public string ExtractEmoticon()
        {
            return CachedUrlHelper.Action("ExtractEmoticon", "ControlPanelSettings", CommonAreaName);
        }
        /// <summary>
        /// 表情包删除
        /// </summary>
        public string DeleteEmotionCategory(string directoryName = "")
        {
            return CachedUrlHelper.Action("DeleteEmotionCategory", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "directoryName", directoryName } });
        }
        /// <summary>
        /// 交换表情包排列顺序
        /// </summary>
        public string ChangeEmotionDisplayOrder()
        {
            return CachedUrlHelper.Action("ChangeEmotionDisplayOrder", "ControlPanelSettings", CommonAreaName);
        }
        #endregion 表情包管理

        #region 推荐内容

        /// <summary>
        /// 推荐内容
        /// </summary>
        /// <param name="tenantTypeId">租户类型ID</param>
        /// <param name="itemId">内容项ID</param>
        /// <param name="recommendItemTypeId">推荐类型ID</param>
        /// <returns></returns>
        public string _RecommendItem(string tenantTypeId = null, long itemId = 0, string itemName = null, string recommendItemTypeId = null, bool showLink = false, long recommendId = 0, bool needRefresh = true, bool showInList = true, long userId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                dic.Add("tenantTypeId", tenantTypeId);
            }
            if (recommendId != 0)
            {
                dic.Add("recommendId", recommendId);
            }
            if (itemId != 0)
            {
                dic.Add("itemId", itemId);
            }
            if (recommendItemTypeId != null)
            {
                dic.Add("recommendItemTypeId", recommendItemTypeId);
            }
            if (!string.IsNullOrEmpty(itemName))
            {
                dic.Add("itemName", WebUtility.UrlEncode(itemName));
            }
            if (showLink != false)
            {
                dic.Add("showLink", showLink);
            }
            if (!needRefresh)
            {
                dic.Add("needRefresh", needRefresh);
            }
            if (showInList != true)
            {
                dic.Add("showInList", showInList);
            }
            if (userId != 0)
            {
                dic.Add("userId", userId);
            }
            return CachedUrlHelper.Action("_RecommendItem", "ControlPanel", CommonAreaName, dic);
        }

        /// <summary>
        /// 推荐项管理
        /// </summary>
        /// <param name="recommendTypeId"></param>
        /// <returns></returns>
        public string _ManageRecommendItems(string recommendTypeId = null, bool showLinkButton = true)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(recommendTypeId))
            {
                dic.Add("recommendTypeId", recommendTypeId);
            }
            if (!showLinkButton)
            {
                dic.Add("showLinkButton", showLinkButton);
            }
            return CachedUrlHelper.Action("_ManageRecommendItems", "ControlPanel", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除推荐内容
        /// </summary>
        /// <param name="recommendId">推荐内容ID</param>
        /// <returns></returns>
        public string _DeleteRecommend(long recommendId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("recommendId", recommendId);
            return CachedUrlHelper.Action("_DeleteRecommend", "ControlPanel", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除推荐内容Logo
        /// </summary>
        /// <param name="recommendId">推荐实体ID</param>
        /// <returns></returns>
        public string _DeleteRecommendLogo(long recommendId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("recommendId", recommendId);
            return CachedUrlHelper.Action("_DeleteRecommendLogo", "ControlPanel", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除推荐内容
        /// </summary>
        /// <param name="recommendId">推荐内容ID</param>
        /// <returns></returns>
        public string _DeleteRecommendItem(long recommendId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();

            if (recommendId != 0)
            {
                dic.Add("recommendId", recommendId);
            }
            return CachedUrlHelper.Action("DeleteRecommendItem", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 管理推荐内容
        /// </summary>
        /// <returns></returns>
        public string ManageRecommendItems(string tenantTypeId = null, string recommendTypeId = null, int pageIndex = 1)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("pageIndex", pageIndex);
            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                dic.Add("tenantTypeId", tenantTypeId);
            }
            if (!string.IsNullOrEmpty(recommendTypeId))
            {
                dic.Add("recommendTypeId", recommendTypeId);
            }
            return CachedUrlHelper.Action("ManageRecommendItems", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 改变推荐顺序
        /// </summary>
        /// <returns></returns>
        public string ChangeRecommendOrder()
        {
            return CachedUrlHelper.Action("ChangeRecommendOrder", "ControlPanelOperation", CommonAreaName);
        }
        #endregion

        #region 推荐用户
        /// <summary>
        /// 管理用户推荐
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="recommendTypeId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public string ManageRecommendUsers(string tenantTypeId = null, string recommendTypeId = null, int pageIndex = 1)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("pageIndex", pageIndex);
            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                dic.Add("tenantTypeId", tenantTypeId);
            }
            if (!string.IsNullOrEmpty(recommendTypeId))
            {
                dic.Add("recommendTypeId", recommendTypeId);
            }
            return CachedUrlHelper.Action("ManageRecommendUsers", "ControlPanelOperation", CommonAreaName, dic);
        }
        /// <summary>
        /// 删除用户推荐
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public string _DeleteUserRecommend(long itemId, string typeId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(typeId))
            {
                dic.Add("typeId", typeId);
            }
            if (itemId != 0)
            {
                dic.Add("itemId", itemId);
            }
            return CachedUrlHelper.Action("DeleteUserRecommend", "ControlPanelOperation", CommonAreaName, dic);
        }
        #endregion

        #region 推荐类别

        /// <summary>
        /// 管理推荐类别
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <returns></returns>
        public string ManageRecommendTypes(string tenantTypeId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                routeValueDictionary.Add("tenantTypeId", tenantTypeId);
            }
            return CachedUrlHelper.Action("ManageRecommendTypes", "ControlPanelOperation", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 编辑推荐类别
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="isEdit"></param>
        /// <param name="recommendTypeId"></param>
        /// <returns></returns>
        public string _EditRecommendType(string tenantTypeId = null, bool isEdit = false, string recommendTypeId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (tenantTypeId != null)
                routeValueDictionary.Add("tenantTypeId", tenantTypeId);

            routeValueDictionary.Add("isEdit", isEdit);
            if (recommendTypeId != null)
                routeValueDictionary.Add("recommendTypeId", recommendTypeId);
            return CachedUrlHelper.Action("_EditRecommendType", "ControlPanelOperation", CommonAreaName, routeValueDictionary);
        }


        /// <summary>
        /// 创建推荐类别
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <returns></returns>

        public string _CreateRecommendType()
        {
            return CachedUrlHelper.Action("_CreateRecommendType", "ControlPanelOperation", CommonAreaName);
        }



        /// <summary>
        /// 删除推荐类别
        /// </summary>
        /// <param name="recommendTypeId">推荐类别ID</param>
        /// <returns></returns>
        public string DeleteRecommendType(string recommendTypeId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(recommendTypeId))
            {
                routeValueDictionary.Add("recommendTypeId", recommendTypeId);
            }
            return CachedUrlHelper.Action("DeleteRecommendType", "ControlPanelOperation", CommonAreaName, routeValueDictionary);
        }
        #endregion

        #region 推荐类别界面
        /// <summary>
        /// 推荐类别界面
        /// </summary>
        /// <returns></returns>
        public string RecommendTypes()
        {
            return CachedUrlHelper.Action("ManageRecommendTypes", "ControlPanelOperation", CommonAreaName);
        }
        #endregion

        #region 第三方帐号绑定后台

        /// <summary>
        /// 管理后台第三方帐号绑定
        /// </summary>
        /// <returns></returns>
        public string ManageAccountTypes()
        {
            return CachedUrlHelper.Action("ManageAccountTypes", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 第三方帐号编辑框
        /// </summary>
        /// <param name="accountTypeKey"></param>
        /// <returns></returns>
        public string _EditAccountType(string accountTypeKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(accountTypeKey))
            {
                dic.Add("accountTypeKey", accountTypeKey);
            }
            return CachedUrlHelper.Action("_EditAccountType", "ControlPanelOperation", CommonAreaName, dic);
        }
        #endregion

        #region 地区管理

        /// <summary>
        ///地区管理界面
        /// </summary>
        /// <returns></returns>
        public string ManageAreas(string areaCode)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(areaCode))
            {
                dictionary.Add("areaCode", areaCode);
            }

            return CachedUrlHelper.Action("ManageAreas", "ControlPanelSettings", CommonAreaName, dictionary);
        }

        /// <summary>
        ///交换地区顺序
        /// </summary>
        /// <returns></returns>
        public string ChangeAreaOrder()
        {
            return CachedUrlHelper.Action("ChangeAreaDisplayOrder", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 删除地区 
        /// </summary>
        /// <returns></returns>
        public string _DeleteArea(string areaCode)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(areaCode))
            {
                dictionary.Add("AreaCode", areaCode);
            }
            return CachedUrlHelper.Action("_DeleteArea", "ControlPanelSettings", CommonAreaName, dictionary);
        }

        /// <summary>
        /// 创建地区
        /// </summary>
        /// <returns></returns>
        public string _CreateArea(string parentCode)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(parentCode))
            {
                dictionary.Add("ParentCode", parentCode);
            }
            return CachedUrlHelper.Action("_CreateArea", "ControlPanelSettings", CommonAreaName, dictionary);
        }

        /// <summary>
        /// 修改地区
        /// </summary>
        /// <returns></returns>
        public string _EditArea(string areaCode)
        {
            return CachedUrlHelper.Action("_EditArea", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "AreaCode", areaCode } });
        }

        #endregion

        #region 学校管理

        /// <summary>
        /// 学校管理界面
        /// </summary>
        /// <returns></returns>
        public string ManageSchools(string areaCode, string keyword = null, SchoolType? schoolType = null)
        {
            return CachedUrlHelper.Action("ManageSchools", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "AreaCode", areaCode }, { "keyword", keyword }, { "schoolType", schoolType } });
        }

        /// <summary>
        /// 交换学校
        /// </summary>
        /// <returns></returns>
        public string ChangeSchoolDisplayOrder()
        {
            return CachedUrlHelper.Action("ChangeSchoolDisplayOrder", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 创建与修改学校
        /// </summary>
        /// <returns></returns>
        public string _EditSchool(long? id = null, string areaCode = null, SchoolType? schoolType = null)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (schoolType.HasValue)
            {
                dictionary.Add("schoolType", schoolType);
            }
            dictionary.Add("id", id);
            dictionary.Add("areaCode", areaCode);
            return CachedUrlHelper.Action("_EditSchool", "ControlPanelSettings", CommonAreaName, dictionary);
        }

        /// <summary>
        /// 删除学校
        /// </summary>
        /// <returns></returns>
        public string DeleteSchool(long id)
        {
            return CachedUrlHelper.Action("DeleteSchool", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "id", id } });
        }
        #endregion

        #region 操作日志

        /// <summary>
        /// 学校选择器
        /// </summary>
        public string ManageOperationLogs()
        {
            return CachedUrlHelper.Action("ManageOperationLogs", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 操作日志管理模态框
        /// </summary>
        /// <returns></returns>
        public string _ManageOperationLogs()
        {
            return CachedUrlHelper.Action("_ManageOperationLogs", "ControlPanelOperation", CommonAreaName);
        }

        #endregion

        #region 敏感词管理

        /// <summary>
        /// 敏感词管理
        /// </summary>
        public string ManageSensitiveWords(string keyword = null, int? typeId = null)
        {
            return CachedUrlHelper.Action("ManageSensitiveWords", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "keyword", keyword }, { "typeId", typeId } });
        }

        /// <summary>
        /// 添加敏感词
        /// </summary>
        /// <returns></returns>
        public string _AddSensitiveWord(int? id = null)
        {
            return CachedUrlHelper.Action("_AddSensitiveWord", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "id", id } });
        }

        /// <summary>
        /// 导入敏感词
        /// </summary>
        /// <returns></returns>
        public string _InputSensitiveWords(int? typeId = null)
        {
            return CachedUrlHelper.Action("_InputSensitiveWords", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "typeId", typeId } });
        }

        /// <summary>
        /// 导出敏感词
        /// </summary>
        /// <returns></returns>
        public string _OutputSensitiveWords()
        {
            return CachedUrlHelper.Action("_OutputSensitiveWords", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 编辑敏感词
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string _EditSensitiveWord(int id)
        {
            return CachedUrlHelper.Action("_EditSensitiveWord", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "id", id } });
        }

        /// <summary>
        /// 删除敏感词
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteSensitiveWords(int ids = 0)
        {
            return CachedUrlHelper.Action("DeleteSensitiveWords", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "ids", ids } });
        }

        /// <summary>
        /// 敏感词类别管理
        /// </summary>
        public string ManageSensitiveWordTypes()
        {
            return CachedUrlHelper.Action("ManageSensitiveWordTypes", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 添加敏感词类别
        /// </summary>
        /// <returns></returns>
        public string _AddSensitiveWordType(int? id = null)
        {
            return CachedUrlHelper.Action("_AddSensitiveWordType", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "id", id } });
        }

        /// <summary>
        /// 删除敏感词类别 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteSensitiveWordTypes(int ids = 0)
        {
            return CachedUrlHelper.Action("DeleteSensitiveWordTypes", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary() { { "ids", ids } });
        }

        #endregion

        #region 公告管理

        /// <summary>
        /// 公告管理
        /// </summary>        
        /// <returns></returns>
        public string ManageAnnouncements()
        {
            return CachedUrlHelper.Action("ManageAnnouncements", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 创建公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string EditAnnouncement(long? id = null)
        {
            return CachedUrlHelper.Action("EditAnnouncement", "ControlPanelOperation", CommonAreaName, new RouteValueDictionary() { { "id", id } });
        }

        /// <summary>
        /// 交换公告
        /// </summary>
        /// <returns></returns>
        public string ChangeAnnouncementDisplayOrder()
        {
            return CachedUrlHelper.Action("ChangeAnnouncementDisplayOrder", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string DeleteAnnouncements(long ids = 0)
        {
            return CachedUrlHelper.Action("DeleteAnnouncements", "ControlPanelOperation", CommonAreaName, new RouteValueDictionary() { { "ids", ids } });
        }

        /// <summary>
        /// 过期公告
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string ChangeStatusToExpired(long ids = 0)
        {
            return CachedUrlHelper.Action("ChangeStatusToExpired", "ControlPanelOperation", CommonAreaName, new RouteValueDictionary() { { "ids", ids } });
        }
        #endregion

        #region 客服消息
        /// <summary>
        /// 客服消息
        /// </summary>
        /// <returns></returns>
        public string ManageCustomMessage()
        {
            return CachedUrlHelper.Action("ManageCustomMessage", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 创建私信的链接
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="toUserId">收件人Id</param>
        /// <returns></returns>
        public string _CustomCreateMessage(string spaceKey, long? toUserId = null)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary() { { "SpaceKey", spaceKey } };
            if (toUserId.HasValue)
                routeValues.Add("ToUserId", toUserId);
            return CachedUrlHelper.Action("_CustomCreateMessage", "ControlPanelOperation", CommonAreaName, routeValues);
        }

        /// <summary>
        /// 群发消息
        /// </summary>
        /// <returns></returns>
        public string MassMessages()
        {
            return CachedUrlHelper.Action("MassMessages", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 删除私信会话链接
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="sessionId">会话Id</param>
        /// <returns></returns>
        public string DeleteMessageSession()
        {

            return CachedUrlHelper.Action("DeleteMessageSession", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 查看所有的消息
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <returns></returns>
        public string _ListCustomMessages(long sessionId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("sessionId", sessionId);
            return CachedUrlHelper.Action("_ListCustomMessages", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 私信局部页
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public string _Message(long? messageId, long? sessionId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (messageId.HasValue)
            {
                dic.Add("messageId", messageId);
            }
            if (sessionId.HasValue)
            {
                dic.Add("sessionId", sessionId);
            }
            return CachedUrlHelper.Action("_Message", "ControlPanelOperation", CommonAreaName, dic);

        }
        #endregion

        #region 站点设置

        /// <summary>
        /// 用户设置管理
        /// </summary>        
        /// <returns></returns>
        public string ManageUserSettings()
        {
            return CachedUrlHelper.Action("UserSettings", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 站点设置
        /// </summary>
        /// <returns></returns>
        public string ManageSiteSettings()
        {
            return CachedUrlHelper.Action("SiteSettings", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 附件设置
        /// </summary>
        /// <returns></returns>
        public string ManageAttachmentSettings()
        {
            return CachedUrlHelper.Action("AttachmentSettings", "ControlPanelSettings", CommonAreaName);
        }

        #endregion

        #region 广告管理

        #region 广告位

        /// <summary>
        /// 创建/编辑广告位页面
        /// </summary>
        /// <param name="positionId">广告位Id</param>
        /// <returns></returns>
        public string _EditPosition(string positionId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(positionId))
            {
                dic.Add("positionId", positionId);
            }
            return CachedUrlHelper.Action("_EditPosition", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除广告位
        /// </summary>
        /// <param name="positionId">广告位Id</param>
        /// <returns></returns>
        public string _DeletePosition(string positionId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(positionId))
            {
                dic.Add("positionIds", positionId);
            }
            return CachedUrlHelper.Action("_DeletePosition", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除广告位示意图
        /// </summary>
        /// <param name="positionId">广告位Id</param>
        /// <returns></returns>
        public string _DeletePositionImage(string positionId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(positionId))
            {
                dic.Add("positionId", positionId);
            }
            return CachedUrlHelper.Action("_DeletePositionImage", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 广告位管理
        /// </summary>
        /// <returns></returns>
        public string ManagePositions()
        {
            return CachedUrlHelper.Action("ManagePositions", "ControlPanelOperation", CommonAreaName);
        }

        #endregion

        #region 广告

        /// <summary>
        /// 创建/编辑广告
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        public string EditAdvertising(long advertisingId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (advertisingId != 0)
            {
                dic.Add("advertisingId", advertisingId);
            }
            return CachedUrlHelper.Action("EditAdvertising", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        public string _DeleteAdvertisings(long advertisingId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (advertisingId != 0)
            {
                dic.Add("advertisingIds", advertisingId);
            }
            return CachedUrlHelper.Action("_DeleteAdvertisings", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 改变广告顺序
        /// </summary>
        /// <returns></returns>
        public string ChangeAdvertisingOrder()
        {
            return CachedUrlHelper.Action("_ChangeAdvertisingOrder", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 设置广告启用状态
        /// </summary>
        /// <returns></returns>
        public string _SetAdvertisingStatus(long advertisingId = 0, bool isEnable = true)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (advertisingId != 0)
            {
                dic.Add("advertisingIds", advertisingId);
            }
            dic.Add("isEnable", isEnable);
            return CachedUrlHelper.Action("_SetAdvertisingStatus", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 管理广告
        /// </summary>
        /// <returns></returns>
        public string ManageAdvertisings(string presentAreaKey = null, string positionId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(presentAreaKey))
            {
                dic.Add("presentAreaKey", presentAreaKey);
            }
            if (!string.IsNullOrEmpty(positionId))
            {
                dic.Add("positionId", positionId);
            }
            return CachedUrlHelper.Action("ManageAdvertisings", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 显示广告位列表局部页
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <param name="size">广告位大小</param>
        /// <param name="presentAreaKey">投放区域</param>
        /// <returns></returns>
        public string _AdvertisingPositionList(long advertisingId, string size = "", string presentAreaKey = "")
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("advertisingId", advertisingId);
            if (!string.IsNullOrEmpty(size))
            {
                dic.Add("size", size);
            }
            if (!string.IsNullOrEmpty(presentAreaKey))
            {
                dic.Add("presentAreaKey", presentAreaKey);
            }
            return CachedUrlHelper.Action("_AdvertisingPositionList", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除广告示意图
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        public string _DeleteAdvertisingImage(long advertisingId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("advertisingId", advertisingId);
            return CachedUrlHelper.Action("_DeleteAdvertisingImage", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 设置广告字体大小
        /// </summary>
        /// <param name="fontSize">字体大小</param>
        /// <param name="orignalSize">默认字体大小</param>
        /// <returns></returns>
        public string _SetFontSize(int fontSize = 0, int orignalSize = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (fontSize > 0)
            {
                dic.Add("fontSize", fontSize);
            }
            dic.Add("orignalSize", orignalSize);
            return CachedUrlHelper.Action("_SetFontSize", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 显示广告位描述
        /// </summary>
        /// <param name="advertisingId"></param>
        /// <returns></returns>
        public string _ShowAdvertisingDescriptions(long advertisingId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("advertisingId", advertisingId);
            return CachedUrlHelper.Action("_ShowAdvertisingDescriptions", "ControlPanelOperation", CommonAreaName, dic);
        }

        #endregion

        #endregion

        #region 邮件设置

        /// <summary>
        /// 检测邮箱
        /// </summary>
        /// <returns></returns>
        public string CheckEmail()
        {
            return CachedUrlHelper.Action("CheckEmail", "ControlPanelSettings", CommonAreaName);
        }
        #endregion

        #region 暂停站点
        /// <summary>
        /// 暂停站点
        /// </summary>
        /// <returns></returns>
        public string PauseSiteSettings()
        {
            return CachedUrlHelper.Action("PauseSiteSettings", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 暂停页面
        /// </summary>
        /// <returns></returns>
        public string PausePage()
        {
            return CachedUrlHelper.Action("PausePage", "Channel", CommonAreaName);

        }

        #endregion

        #region 工具

        /// <summary>
        /// 立即执行任务
        /// </summary>
        /// <param name="id">任务Id</param>
        /// <returns></returns>
        public string RunTask(int? id)
        {
            RouteValueDictionary routeData = new RouteValueDictionary();
            if (id.HasValue)
            {
                routeData.Add("id", id);
            }

            return CachedUrlHelper.Action("RunTask", "ControlPanelTool", CommonAreaName, routeData);
        }

        #endregion

        #endregion 后台

        #region 动态
        /// <summary>
        /// 设置标题图
        /// </summary>
        /// <param name="spaceKey">租户类型Id</param>
        public string _NewActivitie(string spaceKey)
        {
            return CachedUrlHelper.Action("_NewActivitie", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除站点动态
        /// </summary>
        /// <param name="activityId">动态Id</param>
        public string DeleteActivityFromSiteInbox(long activityId)
        {
            return CachedUrlHelper.Action("DeleteActivityFromSiteInbox", "Channel", CommonAreaName, new RouteValueDictionary { { "activityId", activityId } });
        }

        /// <summary>
        /// 删除用户动态
        /// </summary>
        /// <param name="spaceKey">租户类型Id</param>
        public string _DeleteUserActivity(string spaceKey, long activityId)
        {
            return CachedUrlHelper.Action("_DeleteUserActivity", "UserSpace", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "activityId", activityId } });
        }


        /// <summary>
        /// 在动态收件箱中隐藏动态
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public string DeleteActivityFromUserInbox(long activityId)
        {
            return CachedUrlHelper.Action("DeleteActivityFromUserInbox", "Channel", CommonAreaName, new RouteValueDictionary { { "activityId", activityId } });
        }

        /// <summary>
        /// 屏蔽用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string BlockUser(long userId)
        {
            return CachedUrlHelper.Action("BlockUser", "Channel", CommonAreaName, new RouteValueDictionary { { "userId", userId } });
        }

        #region 屏蔽群组

        /// <summary>
        /// 屏蔽用户群组页面
        /// </summary>
        /// <param name="spaceKey">用户空间名</param>
        /// <param name="blockGroupIds">被屏蔽的群组名</param>
        /// <returns>屏蔽用户群组链接</returns>
        public string BlockGroups(string spaceKey, string blockGroupIds = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(blockGroupIds))
                routeValueDictionary.Add("blockGroupIds", blockGroupIds);
            return CachedUrlHelper.Action("BlockGroups", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }

        #endregion



        #endregion

        #region 邀请好友

        /// <summary>
        /// 邀请好友
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string InviteFriend(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("InviteFriend", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 根据用户输入邀请好友
        /// </summary>
        /// <param name="spaceKey">被访问的用户</param>
        /// <param name="emails">用户输入的邮箱</param>
        /// <returns>根据用户输入邀请好友的链接</returns>
        public string _InviteFriendByUserInput(string spaceKey, string emails = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(emails))
                routeValueDictionary.Add("emails", emails);
            return CachedUrlHelper.Action("_InviteFriendByUserInput", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 根据邮箱邀请好友
        /// </summary>
        /// <param name="spaceKey">被访问的用户</param>
        /// <param name="emailDomainName">邮箱的种类</param>
        /// <param name="password">密码</param>
        /// <param name="userName">邮箱密码</param>
        /// <returns>根据用户输入邀请好友的链接</returns>
        public string _InviteFriendByEmail(string spaceKey, string userName = null, string emailDomainName = null, string password = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(userName))
                routeValueDictionary.Add("userName", userName);
            if (!string.IsNullOrEmpty(emailDomainName))
                routeValueDictionary.Add("emailDomainName", emailDomainName);
            if (!string.IsNullOrEmpty(password))
                routeValueDictionary.Add("password", password);
            return CachedUrlHelper.Action("_InviteFriendByEmail", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 从CSV文件获取好友信息，并且发送邮件
        /// </summary>
        /// <param name="spaceKey">被访问的用户</param>
        /// <returns>根据CSV文件邀请好友的链接</returns>
        public string _InviteFriendByCsv(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_InviteFriendByCsv", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 处理用户上传csv文件的方法
        /// </summary>
        /// <param name="spaceKey">被访问控件名</param>
        /// <returns>用户上传post请求的链接地址</returns>
        public string _InviteFriendByCsvPost(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_InviteFriendByCsvPost", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 根据MSN邀请好友的链接
        /// </summary>
        /// <param name="spaceKey">被访问用户名</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public string _InviteFriendByMsn(string spaceKey, string userName = null, string password = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(userName))
                routeValueDictionary.Add("userName", userName);
            if (!string.IsNullOrEmpty(password))
                routeValueDictionary.Add("password", password);
            return CachedUrlHelper.Action("_InviteFriendByMsn", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 选择用户
        /// </summary>
        /// <param name="spaceKey">被访问空间的用户名</param>
        /// <returns>链接</returns>
        public string _ChoiceUser(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_ChoiceUser", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 生成邀请链接
        /// </summary>
        /// <param name="invite">邀请码</param>
        /// <returns></returns>
        public string Invite(string invite = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(invite))
                routeValueDictionary.Add("invite", invite);
            return CachedUrlHelper.Action("Invite", "Account", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 领取一条邀请码
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <returns>领取的链接</returns>
        public string GetNewInvite(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("GetNewInvite", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 购买邀请码配额(完整路径)
        /// </summary>
        /// <returns>购买邀请码配额的方法</returns>
        public string _BuyInviteCount(string spaceKey, int? invitationCodeCount = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (invitationCodeCount.HasValue && invitationCodeCount != 0)
                routeValueDictionary.Add("invitationCodeCount", invitationCodeCount);
            return CachedUrlHelper.Action("_BuyInviteCount", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 我邀请过的好友
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public string InvitedFriends(string spaceKey, int? pageIndex = 1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (pageIndex.HasValue && pageIndex > 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("InvitedFriends", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 邀请确认关注页面
        /// </summary>
        /// <param name="token">身份凭证</param>
        /// <returns></returns>
        public string ConfirmFollow(string token = null, string invite = null, bool? confirm = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(token))
                routeValueDictionary.Add("token", token);
            if (confirm.HasValue)
                routeValueDictionary.Add("confirm", confirm);
            if (!string.IsNullOrEmpty(invite))
                routeValueDictionary.Add("invite", invite);
            return CachedUrlHelper.Action("ConfirmFollow", "Account", CommonAreaName, routeValueDictionary);
        }

        #endregion 邀请好友

        #region 私信

        /// <summary>
        /// 创建私信的链接
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="toUserId">收件人Id</param>
        /// <returns></returns>
        public string _CreateMessage(string spaceKey, long? toUserId = null)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary() { { "SpaceKey", spaceKey } };
            if (toUserId.HasValue)
                routeValues.Add("ToUserId", toUserId);
            return CachedUrlHelper.Action("_CreateMessage", "MessageCenter", CommonAreaName, routeValues);
        }

        /// <summary>
        /// 删除私信会话链接
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="sessionId">会话Id</param>
        /// <returns></returns>
        public string DeleteMessageSession(string spaceKey, long sessionId)
        {
            return CachedUrlHelper.Action("DeleteMessageSession", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "sessionId", sessionId } });
        }

        /// <summary>
        /// 显示私信列表
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="sessionId">会话Id</param>
        public string ListMessages(string spaceKey, long sessionId)
        {
            return CachedUrlHelper.Action("ListMessages", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "sessionId", sessionId } });
        }

        /// <summary>
        /// 显示用户提醒
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <returns></returns>
        public string ListUserReminderSettings(string spaceKey)
        {
            return CachedUrlHelper.Action("ListUserReminderSettings", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 显示私信会话列表
        /// </summary>
        ///<param name="spaceKey">空间标识</param>
        /// <param name="pageIndex">页码</param>
        public string ListMessageSessions(string spaceKey, int? pageIndex)
        {
            RouteValueDictionary routevalues = new RouteValueDictionary() { { "spaceKey", spaceKey } };
            if (pageIndex.HasValue)
                routevalues.Add("pageIndex", pageIndex);

            return CachedUrlHelper.Action("ListMessageSessions", "MessageCenter", CommonAreaName, routevalues);
        }

        /// <summary>
        /// 私信页
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string Message(string spaceKey, long userId)
        {
            
            return string.Empty;
        }

        /// <summary>
        /// 单条私信局部视图
        /// </summary>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="messageId">私信Id(拼接链接时可以不填)</param>
        /// <param name="sessionId">会话Id(拼接链接时可以不填)</param>
        /// <param name="isFrame">是否带边框</param>
        /// <returns></returns>
        public string _Message(string spaceKey, long? messageId, long? sessionId, bool? isFrame)
        {
            RouteValueDictionary routevalues = new RouteValueDictionary() { { "spaceKey", spaceKey } };
            if (messageId.HasValue)
                routevalues.Add("messageId", messageId ?? 0);
            if (isFrame.HasValue)
                routevalues.Add("isFrame", isFrame ?? false);
            if (sessionId.HasValue)
                routevalues.Add("sessionId", sessionId ?? 0);

            return CachedUrlHelper.Action("_Message", "MessageCenter", CommonAreaName, routevalues);
        }

        public string _ListMessages(string spaceKey, long sessionId, long userId, bool isShowAll = false)
        {
            return CachedUrlHelper.Action("_ListMessages", "MessageCenter", CommonAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "sessionId", sessionId }, { "userId", userId }, { "isShowAll", isShowAll } });
        }

        #endregion 私信

        #region 通知

        /// <summary>
        /// 通知页
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string Notice(long userId)
        {
            return string.Empty;
        }

        /// <summary>
        /// 显示通知页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="status">私信状态</param>
        /// <param name="pageIndex">页码</param>
        public string ListNotices(string spaceKey, NoticeStatus? status, int? pageIndex)
        {
            return CachedUrlHelper.Action("ListNotices", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "status", status }, { "pageIndex", pageIndex } });
        }

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="noticeId">通知ID</param>
        /// <param name="status">私信状态</param>
        /// <param name="pageIndex">页码</param>
        public string DeleteNotice(string spaceKey, long noticeId, NoticeStatus? status, int? pageIndex)
        {
            return CachedUrlHelper.Action("DeleteNotice", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "noticeId", noticeId }, { "status", status }, { "pageIndex", pageIndex } });
        }

        /// <summary>
        /// 更新通知为已读
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="noticeId">通知ID</param>
        /// <param name="status">私信状态</param>
        /// <param name="pageIndex">页码</param>
        public string SetIsHandled(string spaceKey, long noticeId, NoticeStatus? status, int? pageIndex)
        {
            return CachedUrlHelper.Action("SetIsHandled", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "noticeId", noticeId }, { "status", status }, { "pageIndex", pageIndex } });
        }

        /// <summary>
        /// 更新通知为已读
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="status">私信状态</param>
        public string ClearAll(string spaceKey, NoticeStatus? status)
        {
            return CachedUrlHelper.Action("ClearAll", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "status", status } });
        }

        /// <summary>
        /// 提示通知页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        public string _ListPromptNotices(string spaceKey)
        {
            return CachedUrlHelper.Action("_ListPromptNotices", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 批量设置通知的状态
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="noticeIds">通知Id集合</param>
        /// <param name="isPromp">是否返回在对话列表显示</param>
        public string BatchSetIsHandled(string spaceKey, string noticeIds = null)
        {
            return CachedUrlHelper.Action("BatchSetIsHandled", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "noticeIds", noticeIds } });
        }

        /// <summary>
        /// 用户设置通知的页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        public string _UserNoticeSettings(string spaceKey)
        {
            return CachedUrlHelper.Action("_UserNoticeSettings", "MessageCenter", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        #endregion 通知

        #region 隐私

        /// <summary>
        /// 隐私规则
        /// </summary>
        /// <returns>隐私规则链接</returns>
        public string ManagePrivacyItems()
        {
            return CachedUrlHelper.Action("ManagePrivacyItems", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 用户隐私设置
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>用户隐私设置连接</returns>
        public string UserPrivacyItemsSettings(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("UserPrivacyItemsSettings", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 恢复用户隐私默认设置
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>恢复用户隐私默认设置链接</returns>
        public string RestoreUserPrivacySettings(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("RestoreUserPrivacySettings", "UserSpaceSettings", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 隐私页面（无权访问页面）
        /// </summary>
        /// <param name="spaceKey">空间名称</param>
        /// <returns>隐私页面（无权访问页面）</returns>
        public string PrivacyHome(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("PrivacyHome", "UserSpace", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 求关注局部页
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>求关注局部页面链接</returns>
        public string _InviteFollow(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_InviteFollow", "UserSpace", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 自定义隐私设置模式框
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="itemName">itemKey</param>
        /// <param name="selectUserGroupIds">选中组的集合id</param>
        /// <param name="selectUserIds">选中的用户id</param>
        /// <returns>隐私设置模式框窗口</returns>
        public string _PrivacySpecifyObjectSelector(long userId, string itemName, string selectUserIds = null, string selectUserGroupIds = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("userId", userId);
            routeValueDictionary.Add("itemName", itemName);
            if (!string.IsNullOrEmpty(selectUserIds))
                routeValueDictionary.Add("selectUserIds", selectUserIds);
            if (!string.IsNullOrEmpty(selectUserGroupIds))
                routeValueDictionary.Add("selectUserGroupIds", selectUserGroupIds);
            return CachedUrlHelper.Action("_PrivacySpecifyObjectSelector", "Channel", CommonAreaName, routeValueDictionary);
        }


        #endregion

        #region 关注

        public string SpaceHomeMore(string spaceKey, int type = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
                routeValueDictionary.Add("spaceKey", spaceKey);
            if (type != 0)
                routeValueDictionary.Add("type", type);
            return CachedUrlHelper.Action("SpaceHomeMore", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 查看用户关注用户页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="sortBy">搜索条件</param>
        /// <param name="pageIndex">当前页码</param>
        public string ListFollowedUsers(string spaceKey, int sortBy = 0, int pageIndex = 1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
                routeValueDictionary.Add("spaceKey", spaceKey);

            if (sortBy != 0)
                routeValueDictionary.Add("sortBy", sortBy);

            if (pageIndex != 1)
                routeValueDictionary.Add("pageIndex", pageIndex);

            return CachedUrlHelper.Action("ListFollowedUsers", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 管理关注用户
        /// </summary>
        public string ManageFollowedUsers(string spaceKey, long? groupId = null, int sortBy = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(spaceKey))
                routeValueDictionary.Add("spaceKey", spaceKey);

            if (sortBy != 0)
                routeValueDictionary.Add("sortBy", sortBy);

            if (groupId != null)
                routeValueDictionary.Add("groupId", groupId);

            return CachedUrlHelper.Action("ManageFollowedUsers", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 关注用户列表
        /// </summary>
        public string ListFollowedUsers()
        {
            return CachedUrlHelper.Action("ListFollowedUsers", "Follow", CommonAreaName);
        }

        /// <summary>
        /// 添加关注用户
        /// </summary>
        public string _AddFollowedUser(string spaceKey, long followedUserId, bool isFollowed = false)
        {
            return CachedUrlHelper.Action("_AddFollowedUser", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "followedUserId", followedUserId }, { "isFollowed", isFollowed } });
        }

        /// <summary>
        /// 批量添加关注用户
        /// </summary>
        public string _BatchAddFollowedUsers(string spaceKey)
        {
            return CachedUrlHelper.Action("_BatchAddFollowedUsers", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 关注话题
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public string _AddFollowedTopic(string spaceKey, long tagId = 0)
        {
            if (tagId > 0)
            {
                return CachedUrlHelper.Action("_AddFollowedTopic", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "tagId", tagId } });
            }
            else
            {
                return CachedUrlHelper.Action("_AddFollowedTopic", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
            }
        }

        /// <summary>
        /// 取消关注话题
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public string _CancelFollowedTopic(string spaceKey, long tagId = 0)
        {
            if (tagId > 0)
            {
                return CachedUrlHelper.Action("_CancelFollowedTopic", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "tagId", tagId } });
            }
            else
            {
                return CachedUrlHelper.Action("_CancelFollowedTopic", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey } });
            }
        }

        /// <summary>
        /// 编辑分组
        /// </summary>
        public string _EditFollowedUsersGroup(string spaceKey, long? groupId)
        {
            Random random = new Random();
            int rdNum = random.Next();
            return CachedUrlHelper.Action("_EditFollowedUsersGroup", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "groupId", groupId }, { "rd", rdNum } });
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        public string DeleteFollowedUsersGroup(string spaceKey, long? groupId)
        {
            return CachedUrlHelper.Action("DeleteFollowedUsersGroup", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "groupId", groupId } });
        }

        /// <summary>
        /// 批量关注
        /// </summary>
        public string _BatchFollow(string spaceKey, long? groupId)
        {
            return CachedUrlHelper.Action("_BatchFollow", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "groupId", groupId } });
        }

        /// <summary>
        /// 设置关注用户备注名
        /// </summary>
        public string _UpdateFollowedUserNoteName(string spaceKey, long followedUserId)
        {
            Random random = new Random();
            int rdNum = random.Next();
            return CachedUrlHelper.Action("_UpdateFollowedUserNoteName", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "followedUserId", followedUserId }, { "rd", rdNum } });
        }

        /// <summary>
        /// 管理我的粉丝页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="sortBy">搜索条件</param>
        /// <param name="pageIndex">当前页码</param>
        public string ManageFollowers(string spaceKey, int sortBy = 0, int pageIndex = 1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
                routeValueDictionary.Add("spaceKey", spaceKey);

            if (sortBy != 0)
                routeValueDictionary.Add("sortBy", sortBy);

            if (pageIndex != 1)
                routeValueDictionary.Add("pageIndex", pageIndex);

            return CachedUrlHelper.Action("ManageFollowers", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 查看用户粉丝页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="sortBy">搜索条件</param>
        /// <param name="pageIndex">当前页码</param>
        public string ListFollowers(string spaceKey, int sortBy = 0, int pageIndex = 1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
                routeValueDictionary.Add("spaceKey", spaceKey);

            if (sortBy != 0)
                routeValueDictionary.Add("sortBy", sortBy);

            if (pageIndex != 1)
                routeValueDictionary.Add("pageIndex", pageIndex);

            return CachedUrlHelper.Action("ListFollowers", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 黑名单
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="stopedUserId">被设为黑名单的用户Id</param>
        public string CreateStopedUser(string spaceKey, long stopedUserId)
        {
            return CachedUrlHelper.Action("CreateStopedUser", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "stopedUserId", stopedUserId } });
        }

        /// <summary>
        /// 悄悄关注
        /// </summary>
        public string QuietlyFollow(string spaceKey, long followedUserId)
        {
            return CachedUrlHelper.Action("QuietlyFollow", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "followedUserId", followedUserId } });
        }

        /// <summary>
        /// 取消粉丝
        /// </summary>
        public string RemoveFollower(string spaceKey, long? followerId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (followerId.HasValue)
                routeValueDictionary.Add("followerId", followerId);
            return CachedUrlHelper.Action("RemoveFollower", "Follow", CommonAreaName, routeValueDictionary);
        }


        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="followedUserId">被关注用户Id</param>
        public string CancelFollow(string spaceKey, long? followedUserId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (followedUserId.HasValue)
                routeValueDictionary.Add("followedUserId", followedUserId);
            return CachedUrlHelper.Action("CancelFollow", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 设置关注用户分组中创建分组
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="groupName">新分组名</param>
        /// <param name="followedUserId">被关注用户Id</param>
        public string CreateNewGroup(string spaceKey = "", string groupName = "", long? followedUserId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
                routeValueDictionary.Add("spaceKey", spaceKey);

            if (!string.IsNullOrEmpty(groupName))
                routeValueDictionary.Add("groupName", groupName);

            if (followedUserId.HasValue)
                routeValueDictionary.Add("followedUserId", followedUserId);

            return CachedUrlHelper.Action("CreateNewGroup", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 设置关注用户分组中创建分组
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="groupName">新分组名</param>
        /// <param name="followedUserId">被关注用户Id</param>
        public string CreateNewGroupInList(string spaceKey = "", string groupName = "", long? followedUserId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);

            if (!string.IsNullOrEmpty(groupName))
                routeValueDictionary.Add("groupName", groupName);

            if (followedUserId.HasValue)
                routeValueDictionary.Add("followedUserId", followedUserId);


            return CachedUrlHelper.Action("CreateNewGroupInList", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 添加关注用户
        /// </summary>
        public string _SetGroupForUser(string spaceKey, long followUserId, long? groupId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            Random random = new Random();
            routeValueDictionary.Add("spaceKey", spaceKey);
            routeValueDictionary.Add("followUserId", followUserId);
            if (groupId != null)
                routeValueDictionary.Add("groupId", groupId);

            routeValueDictionary.Add("rd", random.Next());
            return CachedUrlHelper.Action("_SetGroupForUser", "Follow", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 为用户添加分组或者取消分组
        /// </summary>
        public string SetGroupForFollowedUser(string spaceKey, bool isChecked, int? userGroupId = 0, long? followedUserId = 0)
        {
            return CachedUrlHelper.Action("SetGroupForFollowedUser", "Follow", CommonAreaName, new RouteValueDictionary() { { "spaceKey", spaceKey }, { "userGroupId", userGroupId }, { "followedUserId", followedUserId }, { "isChecked", isChecked } });
        }

        /// <summary>
        /// 判断是否关注用户
        /// </summary>
        public string _IsFollowedUser(long userId)
        {
            return CachedUrlHelper.Action("_IsFollowedUser", "Channel", CommonAreaName, new RouteValueDictionary { { "userId", userId } });
        }
        #endregion

        #region 在线用户
        /// <summary>
        /// 在线用户
        /// </summary>
        /// <returns></returns>
        public string OnlineUser(bool ignoreAnonymousUsers = true)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("ignoreAnonymousUsers", ignoreAnonymousUsers);
            return CachedUrlHelper.Action("OnlineUser", "FindUser", CommonAreaName, dic);
        }
        /// <summary>
        /// 在线用户的推荐用户
        /// </summary>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        public string _RecommendUser(int topNumber)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (topNumber != 0)
                dic.Add("topNumber", topNumber);
            return CachedUrlHelper.Action("_RecommendUser", "FindUser", CommonAreaName, dic);
        }

        #endregion

        #region 站内发布微博
        /// <summary>
        /// 站内发布微博
        /// </summary>
        /// <param name="content">微博内容</param>
        /// <param name="imageUrl">图片Url</param>
        public string _ShareToMicroblog(string content, string imageUrl = null)
        {
            if (UserContext.CurrentUser == null)
                return this.Login(true, SiteUrls.LoginModal._LoginInModal, null);
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", UserContext.CurrentUser.UserName);
            dic.Add("content", WebUtility.UrlEncode(content));
            dic.Add("imageUrl", WebUtility.UrlEncode(imageUrl));
            dic.Add("isShare", true);
            return CachedUrlHelper.Action("_Create", "Microblog", "Microblog", dic);
        }
        #endregion

        #region 用户头像

        /// <summary>
        /// 获取用户连接的头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarSizeType"></param>
        /// <param name="enableClientCaching"></param>
        /// <returns></returns>
        public string _UserAvatarLink(AvatarSizeType avatarSizeType = AvatarSizeType.Small, bool enableClientCaching = false)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (avatarSizeType != AvatarSizeType.Small)
                dic.Add("avatarSizeType", avatarSizeType);
            if (enableClientCaching)
                dic.Add("enableClientCaching", enableClientCaching);
            return CachedUrlHelper.Action("_UserAvatarLink", "Channel", CommonAreaName, dic);
        }

        #endregion

        #region 表情选择器

        /// <summary>
        /// 获取表情选择器Url
        /// </summary>
        public string _EmotionSelector()
        {
            return CachedUrlHelper.Action("_EmotionSelector", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 显示表情包内的表情列表
        /// </summary>
        /// <param name="directoryName">表情包目录名</param>
        /// <returns></returns>
        public string _ListEmotions(string directoryName)
        {
            return CachedUrlHelper.Action("_ListEmotions", "ControlPanelSettings", CommonAreaName, new RouteValueDictionary { { "directoryName", directoryName } });
        }

        #endregion

        #region 学校选择器
        /// <summary>
        /// 学校选择器
        /// </summary>
        public string _SchoolSelector(string inputId, string areaCode = null, string keyword = null, SchoolType? schoolType = null)
        {
            return CachedUrlHelper.Action("_SchoolSelector", "Channel", CommonAreaName, new RouteValueDictionary { { "inputId", inputId }, { "areaCode", areaCode }, { "keyword", keyword }, { "schoolType", schoolType } });
        }

        #endregion

        #region 用户举报

        /// <summary>
        /// 创建举报
        /// </summary>
        /// <param name="reportedUserId">被举报人Id</param>
        /// <param name="url">举报链接</param>
        /// <param name="title">举报标题</param>
        /// <returns>string</returns>
        public string _ImpeachReport(long reportedUserId, string url, string title)
        {
            IUser currentUser = UserContext.CurrentUser;
            long userId;
            if (currentUser == null)
            {
                userId = 0;
            }
            else
            {
                userId = currentUser.UserId;
            }
            return CachedUrlHelper.Action("_ImpeachReport", "Channel", CommonAreaName, new RouteValueDictionary { { "userId", userId }, { "reportedUserId", reportedUserId }, { "title", WebUtility.UrlEncode(title) }, { "url", WebUtility.UrlEncode(url) } });
        }

        /// <summary>
        /// 管理举报
        /// </summary>
        /// <param name="impeachReason">举报原因</param>
        /// <param name="isDisposed">处理状态</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页吗</param>
        /// <returns>string</returns>
        public string ManageImpeachReports(ImpeachReason? impeachReason = null, bool isDisposed = false, int pageSize = 20, int pageIndex = 1)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (impeachReason != null)
            {
                dic.Add("impeachReason", impeachReason);
            }
            dic.Add("isDisposed", isDisposed);
            dic.Add("pageSize", pageSize);
            dic.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("ManageImpeachReports", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 批量处理用户举报
        /// </summary>
        /// <param name="reportId">要处理的举报Id</param>
        /// <returns></returns>
        public string _DisposeReports(long? reportId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (reportId != null)
            {
                dic.Add("reportIds", reportId);
            }
            return CachedUrlHelper.Action("_DisposeReports", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 批量删除举报
        /// </summary>
        /// <param name="reportId">举报Id</param>
        /// <returns></returns>
        public string _DeleteReports(long? reportId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (reportId != null)
            {
                dic.Add("reportIds", reportId);
            }
            return CachedUrlHelper.Action("_DeleteReports", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 显示匿名用户的录入信息
        /// </summary>
        /// <param name="reportId">举报Id</param>
        /// <returns>string</returns>
        public string _AnonymityReporterInfo(long reportId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("reportId", reportId);
            return CachedUrlHelper.Action("_ReporterInfo", "ControlPanelOperation", CommonAreaName, dic);
        }

        #endregion

        #region 友情链接

        #region 站点友情链接后台管理
        /// <summary>
        /// 管理友情链接(后台)
        /// </summary>
        /// <returns></returns>
        public string ManageLinksForAdmin(long? categoryId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (categoryId.HasValue)
            {
                dic.Add("categoryId", categoryId);
            }
            return CachedUrlHelper.Action("ManageLinks", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 创建/编辑友情链接
        /// </summary>
        /// <param name="linkId">友情链接标识</param>
        /// <returns></returns>
        public string _EditLinkForAdmin(long? linkId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (linkId.HasValue)
            {
                dic.Add("linkId", linkId);
            }
            return CachedUrlHelper.Action("_EditLink", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="linkId">链接标识</param>
        /// <returns></returns>
        public string _DeleteLinkForAdmin(long linkId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("linkId", linkId);
            return CachedUrlHelper.Action("_DeleteLink", "ControlPanelOperation", CommonAreaName, dic);
        }

        /// <summary>
        /// 批量删除友情链接
        /// </summary>
        /// <returns></returns>
        public string _BatchDeleteLink()
        {
            return CachedUrlHelper.Action("_BatchDeleteLink", "ControlPanelOperation", CommonAreaName);
        }

        /// <summary>
        /// 调整友情链接的显示顺序
        /// </summary>
        /// <returns></returns>
        public string _ChangeLinkDisplayOrder()
        {
            return CachedUrlHelper.Action("_ChangeLinkDisplayOrder", "ControlPanelOperation", CommonAreaName);
        }
        #endregion

        #region 用户/群组空间友情链接管理
        /// <summary>
        /// 用户/群组友情链接列表展示
        /// </summary>
        /// <param name="ownerId">拥有者标识</param>
        /// <param name="ownerType">拥有者类型</param>
        /// <param name="topNumber">最大条数</param>
        /// <returns></returns>
        public string _OwnerLinks(int ownerType, long ownerId, int topNumber = 1000)
        {
            RouteValueDictionary dic = new RouteValueDictionary();

            dic.Add("ownerType", ownerType);
            dic.Add("ownerId", ownerId);
            dic.Add("topNumber", topNumber);

            return CachedUrlHelper.Action("_OwnerLinks", "Channel", CommonAreaName, dic);
        }

        /// <summary>
        /// 用户/群组友情链接管理（前台）
        /// </summary>
        /// <param name="ownerId">拥有者标识</param>
        /// <param name="ownerType">拥有者类型</param>
        /// <returns></returns>
        public string _ManageLinksForOwner(int ownerType, long ownerId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("ownerType", ownerType);
            dic.Add("ownerId", ownerId);

            return CachedUrlHelper.Action("_ManageLinks", "Channel", CommonAreaName, dic);
        }


        /// <summary>
        /// 创建/编辑文字友情链接
        /// </summary>
        /// <param name="linkId">链接标识</param>
        /// <returns></returns>
        public string _EditTextLinkForOwner(long linkId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (linkId > 0)
            {
                dic.Add("linkId", linkId);
            }
            return CachedUrlHelper.Action("_EditTextLink", "Channel", CommonAreaName, dic);
        }

        /// <summary>
        /// 创建/编辑图片友情链接
        /// </summary>
        /// <param name="linkId">链接标识</param>
        /// <returns></returns>
        public string _EditImageLinkForOwner(long linkId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (linkId > 0)
            {
                dic.Add("linkId", linkId);
            }
            return CachedUrlHelper.Action("_EditImageLink", "Channel", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除用户/群组友情链接
        /// </summary>
        /// <returns></returns>
        public string _DeleteLinkForOwner(long linkId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("linkId", linkId);

            return CachedUrlHelper.Action("_DeleteLink", "Channel", CommonAreaName, dic);
        }

        /// <summary>
        /// 批量更改启用状态
        /// </summary>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        public string _EditLinksStatus(bool isEnabled)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("isEnabled", isEnabled);
            return CachedUrlHelper.Action("_EditLinksStatus", "Channel", CommonAreaName, dic);
        }
        #endregion

        #endregion

        #region Help Methods

        /// <summary>
        /// 获取完整的Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string FullUrl(string url)
        {
            if (string.IsNullOrEmpty(url) || url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                return url;

            string fullUrl = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                fullUrl = WebUtility.HostPath(HttpContext.Current.Request.Url) + WebUtility.ResolveUrl(url);
            else
            {
                ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
                SiteSettings siteSettings = siteSettingsManager.Get();
                if (!string.IsNullOrEmpty(siteSettings.MainSiteRootUrl))
                {
                    return siteSettings.MainSiteRootUrl + WebUtility.ResolveUrl(url);
                }
            }
            if (!string.IsNullOrEmpty(fullUrl))
                return fullUrl;
            else
                return url;
        }

        /// <summary>
        /// 获取url中的查询字符串参数
        /// </summary>
        public static NameValueCollection ExtractQueryParams(string url)
        {
            int startIndex = url.IndexOf("?");
            NameValueCollection values = new NameValueCollection();

            if (startIndex <= 0)
                return values;

            string[] nameValues = url.Substring(startIndex + 1).Split('&');

            foreach (string s in nameValues)
            {
                string[] pair = s.Split('=');

                string name = pair[0];
                string value = string.Empty;

                if (pair.Length > 1)
                    value = pair[1];

                values.Add(name, value);
            }

            return values;
        }

        /// <summary>
        /// 登录模式
        /// </summary>
        public enum LoginModal
        {
            login = 0,
            _login = 1,
            _LoginInModal = 2
        }

        #endregion Help Methods

        #region 身份认证

        #region 后台

        /// <summary>
        ///认证标识管理
        /// </summary>
        /// <returns></returns>
        public string ManageIdentificationTypes()
        {
            return CachedUrlHelper.Action("ManageIdentificationTypes", "ControlPanelUser", CommonAreaName);
        }

        /// <summary>
        ///认证申请管理
        /// </summary>
        /// <returns></returns>
        public string ManageIdentifications()
        {
            return CachedUrlHelper.Action("ManageIdentifications", "ControlPanelUser", CommonAreaName);
        }

        /// <summary>
        ///添加 编辑认证标识模态窗口
        /// </summary>
        /// <returns></returns>
        public string _EditIdentificationType(long identificationTypeId = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("identificationTypeId", identificationTypeId);
            return CachedUrlHelper.Action("_EditIdentificationType", "ControlPanelUser", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除认证标识
        /// </summary>
        /// <returns></returns>
        public string _DeleteIdentificationType(long identificationTypeId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("identificationTypeId", identificationTypeId);
            return CachedUrlHelper.Action("_DeleteIdentificationType", "ControlPanelUser", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 处理身份认证
        /// </summary>
        /// <param name="identificationId">认证申请Id</param>
        /// <param name="status">状态（true：通过；false：不通过；）</param>
        /// <returns></returns>
        public string _DisposeIdentification(long identificationIds, IdentificationStatus status)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("identificationIds", identificationIds);
            routeValueDictionary.Add("status", status);
            return CachedUrlHelper.Action("_DisposeIdentification", "ControlPanelUser", CommonAreaName, routeValueDictionary);
        }
        /// <summary>
        /// 删除身份认证
        /// </summary>
        /// <param name="identificationIds">认证申请Id</param>
        /// <returns></returns>
        public string _DeleteIdentification(long identificationIds)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("identificationIds", identificationIds);
            return CachedUrlHelper.Action("_DeleteIdentification", "ControlPanelUser", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 申请人详细信息
        /// </summary>
        /// <param name="identificationId">认证申请Id</param>
        /// <returns></returns>
        [HttpGet]
        public string _ViewIdentification(long identificationId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("identificationId", identificationId);
            return CachedUrlHelper.Action("_ViewIdentification", "ControlPanelUser", CommonAreaName, routeValueDictionary);
        }
        #endregion

        #region 用户空间

        /// <summary>
        /// 更新身份认证
        /// </summary>
        /// <returns></returns>
        public string UpdateIdentification(string spaceKey, long identificationId = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (identificationId != 0)
            {
                routeValueDictionary.Add("identificationId", identificationId);
            }
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("EditIdentification", "UserSpace", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除身份认证
        /// </summary>
        /// <returns></returns>
        public string _DeleteIdentification(string spaceKey, long identificationId = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (identificationId != 0)
            {
                routeValueDictionary.Add("identificationId", identificationId);
            }
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("_DeleteIdentification", "UserSpace", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 身份认证状态页
        /// </summary>
        /// <returns></returns>
        public string IdentificationResult(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("IdentificationResult", "UserSpace", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 判断该用户有没有申请过某认证
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public string IsApplied(string spaceKey)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("IsApplied", "UserSpace", CommonAreaName, routeValueDictionary);
        }

        #endregion

        #endregion

        #region 站点公告

        /// <summary>
        /// 公告详细页
        /// </summary>
        /// <param name="announcementId"></param>
        /// <returns></returns>
        public string AnnouncementDetail(long announcementId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("announcementId", announcementId);
            return CachedUrlHelper.Action("AnnouncementDetail", "Channel", CommonAreaName, dic);

        }

        /// <summary>
        /// 公告列表
        /// </summary>
        /// <returns></returns>
        public string AnnouncementList()
        {
            return CachedUrlHelper.Action("AnnouncementList", "Channel", CommonAreaName);
        }

        /// <summary>
        /// 前台管理员删除公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteAnnouncement(long id)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("id", id);
            return CachedUrlHelper.Action("DeleteAnnouncement", "Channel", CommonAreaName, dic);
        }

        #endregion

        #region 后台奖惩用户

        /// <summary>
        /// 批量奖惩
        /// </summary>
        /// <returns></returns>
        public string _RewardUsers(long? userId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("userId", userId);
            return CachedUrlHelper.Action("_RewardUsers", "ControlPanelUser", CommonAreaName, dic);
        }
        #endregion

        #region 附件

        /// <summary>
        /// 购买附件的
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="attachementId">附件的id</param>
        /// <returns>购买附件的链接</returns>
        public string _BuyAttachement(string tenantTypeId, long attachementId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("tenantTypeId", tenantTypeId);
            dic.Add("attachementId", attachementId);
            return CachedUrlHelper.Action("_BuyAttachement", "Channel", CommonAreaName, dic);
        }

        /// <summary>
        /// 购买附件的post请求
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="attachementId">附件的id</param>
        /// <returns>购买附件的postid链接</returns>
        public string BuyAttachementPost(long attachementId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("attachementId", attachementId);
            return CachedUrlHelper.Action("BuyAttachementPost", "Channel", CommonAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 附件的购买记录
        /// </summary>
        /// <param name="attachementId">附件的id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public string _BuyAttachementRecord(long attachementId, int pageIndex = 1)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("attachementId", attachementId);
            if (pageIndex > 1)
                dic.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("_BuyAttachementRecord", "Channel", CommonAreaName, dic);
        }

        #endregion

        #region 视频/音乐

        /// <summary>
        /// 添加音乐
        /// </summary>
        /// <param name="textAreaId">TextAreaId</param>
        /// <returns></returns>
        public string _AddMusic(string textAreaId = null)
        {
            RouteValueDictionary routeValue = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(textAreaId))
            {
                routeValue = new RouteValueDictionary() { { "textAreaId", textAreaId } };
            }
            return CachedUrlHelper.Action("_AddMusic", "Channel", "Common", routeValue);
        }

        /// <summary>
        /// 添加音乐
        /// </summary>
        /// <param name="textAreaId">TextAreaId</param>
        /// <returns></returns>
        public string _AddVideo(string textAreaId = null)
        {
            RouteValueDictionary routeValue = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(textAreaId))
            {
                routeValue = new RouteValueDictionary() { { "textAreaId", textAreaId } };
            }
            return CachedUrlHelper.Action("_AddVideo", "Channel", "Common", routeValue);
        }

        /// <summary>
        /// 音乐详细页
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public string _MusicDetail(string alias)
        {
            return CachedUrlHelper.Action("_MusicDetail", "Channel", "Common", new RouteValueDictionary() { { "alias", alias } });
        }

        /// <summary>
        /// 视频详细页
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public string _VideoDetail(string alias)
        {
            return CachedUrlHelper.Action("_VideoDetail", "Channel", "Common", new RouteValueDictionary() { { "alias", alias } });
        }

        #endregion

        #region 后台工具管理首页
        public string ControlPanelTool()
        {
            return CachedUrlHelper.Action("Home", "ControlPanelTool", "Common");
        }
        #endregion

        #region 重启站点
        /// <summary>
        /// 重启站点
        /// </summary>
        /// <returns></returns>
        public string UnloadAppDomain()
        {
            return CachedUrlHelper.Action("_UnloadAppDomain", "ControlPanelTool", "Common");
        }
        #endregion

        #region 重建缩略图

        /// <summary>
        /// 重建缩略图
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns>重建缩略图</returns>
        public string RebuildingThumbnails()
        {
            return CachedUrlHelper.Action("RebuildingThumbnails", "ControlPanelTool", CommonAreaName);
        }

        /// <summary>
        /// 重建缩略图
        /// </summary>
        /// <param name="tenantTypeId">租户类型</param>
        /// <returns></returns>
        public string _RebuildingThumbnails(string tenantTypeId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("tenantTypeId", tenantTypeId);
            return CachedUrlHelper.Action("_RebuildingThumbnails", "ControlPanelTool", CommonAreaName, dic);
        }

        /// <summary>
        /// 重建log
        /// </summary>
        /// <param name="tenantTypeId">租户类型</param>
        /// <returns></returns>
        public string _ReBuildingLogs(string tenantTypeId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("tenantTypeId", tenantTypeId);
            return CachedUrlHelper.Action("_ReBuildingLogs", "ControlPanelTool", CommonAreaName, dic);
        }

        #endregion

        #region 清除缓存
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <returns></returns>
        public string ResetCache()
        {
            return CachedUrlHelper.Action("_ResetCache", "ControlPanelTool", "Common");
        }
        #endregion

        #region 后台邮件设置

        /// <summary>
        /// 发件箱列表
        /// </summary>
        /// <returns></returns>
        public string ListOutBox()
        {
            return CachedUrlHelper.Action("ListOutBox", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 添加发件邮箱
        /// </summary>
        /// <param name="id">对应的id</param>
        /// <returns></returns>
        public string _EditOutBox(long? id = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();

            if (id.HasValue)
                dic.Add("id", id);

            return CachedUrlHelper.Action("_EditOutBox", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 设置已经发送数目
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public string _SetOutBoxSendCount(long id, int count)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("id", id);
            dic.Add("count", count);
            return CachedUrlHelper.Action("_SetOutBoxSendCount", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 删除发件邮箱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string _DeleteOutBox(long id)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("id", id);
            return CachedUrlHelper.Action("_DeleteOutBox", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 测试发件箱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string _TestOutBox(long id)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("id", id);
            return CachedUrlHelper.Action("_TestOutBox", "ControlPanelSettings", CommonAreaName, dic);
        }

        /// <summary>
        /// 自动补全Smtp设置
        /// </summary>
        /// <returns></returns>
        public string _AutoCompletionSmtpSetting()
        {
            return CachedUrlHelper.Action("_AutoCompletionSmtpSetting", "ControlPanelSettings", CommonAreaName);
        }

        /// <summary>
        /// 管理邮件其他设置
        /// </summary>
        /// <returns></returns>
        public string ManageEmailOtherSettings()
        {
            return CachedUrlHelper.Action("ManageEmailOtherSettings", "ControlPanelSettings", CommonAreaName);
        }

        #endregion

        #region 站点统计
        /// <summary>
        /// 设置CNZZ统计的开启状态
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public string _SetCNZZStatisticsStatus(bool enable = true)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!enable)
                dic.Add("enable", enable);
            return CachedUrlHelper.Action("_SetCNZZStatisticsStatus", "ControlPanelOperation", CommonAreaName, dic);
        }
        #endregion
    }

    /// <summary>
    /// 用户头像类型
    /// </summary>
    public enum UserAvatarType
    {
        /// <summary>
        /// 正常显示用户已经上传的头像
        /// </summary>
        Normal,

        /// <summary>
        /// 未上传头像用户，未设置性别
        /// </summary>
        NoAvatar_Default,

        /// <summary>
        /// 未上传头像用户，性别设置为男
        /// </summary>
        NoAvatar_Male,

        /// <summary>
        /// 未上传头像用户，性别设置为女
        /// </summary>
        NoAvatar_Female
    }

    /// <summary>
    /// 第三方帐号Logo尺寸类型
    /// </summary>
    public enum ThirdLogoSizeType
    {
        /// <summary>
        /// 大尺寸
        /// </summary>
        Big = 1,

        /// <summary>
        /// 正常尺寸
        /// </summary>
        Normal = 2,

        /// <summary>
        /// 小尺寸
        /// </summary>
        Small = 3
    }
}
