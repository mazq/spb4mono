//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Spacebuilder.Common.Configuration;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Tunynet.FileStore;
using Tunynet.Imaging;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 扩展用户业务逻辑
    /// </summary>
    public static class UserServiceExtension
    {

        #region 头像处理

        //Avatar文件扩展名
        private static readonly string AvatarFileExtension = "jpg";

        /// <summary>
        ///  Avatar存储目录名
        /// </summary>
        public static readonly string AvatarDirectory = "Avatars";

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="postedFile">上传的二进制头像文件</param>
        public static void UploadOriginalAvatar(this IUserService userService, long userId, Stream postedFile)
        {
            if (postedFile == null)
                return;
            Image image = Image.FromStream(postedFile);

            IUserProfileSettingsManager userProfileSettingsManager = DIContainer.Resolve<IUserProfileSettingsManager>();
            UserProfileSettings userProfileSettings = userProfileSettingsManager.GetUserProfileSettings();

            //检查是否需要缩放原图
            if (image.Height > userProfileSettings.OriginalAvatarHeight || image.Width > userProfileSettings.OriginalAvatarWidth)
            {
                postedFile = ImageProcessor.Resize(postedFile, userProfileSettings.OriginalAvatarWidth, userProfileSettings.OriginalAvatarHeight, ResizeMethod.KeepAspectRatio);
            }

            string relativePath = GetAvatarRelativePath(userId);

            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            storeProvider.AddOrUpdateFile(relativePath, GetAvatarFileName(userId, AvatarSizeType.Original), postedFile);
            postedFile.Dispose();
            //1、如果原图超过一定尺寸（可以配置宽高像素值）则原图保留前先缩小（原图如果太大，裁剪时不方便操作）再保存
        }

        /// <summary>
        /// 根据用户自己选择的尺寸及位置进行头像裁剪
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="srcWidth">需裁剪的宽度</param>
        /// <param name="srcHeight">需裁剪的高度</param>
        /// <param name="srcX">需裁剪的左上角点坐标</param>
        /// <param name="srcY">需裁剪的左上角点坐标</param>
        public static void CropAvatar(this IUserService userService, long userId, float srcWidth, float srcHeight, float srcX, float srcY)
        {
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            IStoreFile iStoreFile = storeProvider.GetFile(GetAvatarRelativePath(userId), GetAvatarFileName(userId, AvatarSizeType.Original));
            if (iStoreFile == null)
                return;
            User user = GetFullUser(userService, userId);
            if (user == null)
                return;

            bool isFirst = !(user.HasAvatar);
            string avatarRelativePath = GetAvatarRelativePath(userId).Replace(Path.DirectorySeparatorChar, '/');
            avatarRelativePath = avatarRelativePath.Substring(AvatarDirectory.Length + 1);
            user.Avatar = avatarRelativePath + "/" + userId;

            IUserRepository userRepository = userService.GetUserRepository();
            userRepository.UpdateAvatar(user, user.Avatar);

            IUserProfileSettingsManager userProfileSettingsManager = DIContainer.Resolve<IUserProfileSettingsManager>();
            UserProfileSettings userProfileSettings = userProfileSettingsManager.GetUserProfileSettings();

            using (Stream fileStream = iStoreFile.OpenReadStream())
            {
                Stream bigImage = ImageProcessor.Crop(fileStream, new Rectangle((int)srcX, (int)srcY, (int)srcWidth, (int)srcHeight), userProfileSettings.AvatarWidth, userProfileSettings.AvatarHeight);

                Stream smallImage = ImageProcessor.Resize(bigImage, userProfileSettings.SmallAvatarWidth, userProfileSettings.SmallAvatarHeight, ResizeMethod.KeepAspectRatio);
                storeProvider.AddOrUpdateFile(GetAvatarRelativePath(userId), GetAvatarFileName(userId, AvatarSizeType.Big), bigImage);
                storeProvider.AddOrUpdateFile(GetAvatarRelativePath(userId), GetAvatarFileName(userId, AvatarSizeType.Small), smallImage);

                bigImage.Dispose();
                smallImage.Dispose();
                fileStream.Close();
            }

            EventBus<User, CropAvatarEventArgs>.Instance().OnAfter(user, new CropAvatarEventArgs(isFirst));
            //触发用户更新头像事件
        }

        /// <summary>
        /// 删除用户头像
        /// </summary>
        public static void DeleteAvatar(this IUserService userService, long userId)
        {
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();

            //删除文件系统的头像使用以下代码
            storeProvider.DeleteFolder(GetAvatarRelativePath(userId));
        }

        /// <summary>
        /// 获取直连URL
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="avatarSizeType"><see cref="AvatarSizeType"/></param>
        /// <returns></returns>
        public static string GetAvatarDirectlyUrl(this IUserService userService, IUser user, AvatarSizeType avatarSizeType, bool enableClientCaching = false)
        {
            string url = string.Empty;

            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            string directlyRootUrl = storeProvider.DirectlyRootUrl;
            if (!string.IsNullOrEmpty(storeProvider.DirectlyRootUrl))
            {
                url += storeProvider.DirectlyRootUrl;
            }
            else
            {
                url += WebUtility.ResolveUrl("~/Uploads");  //本机存储时仅允许用~/Uploads/
            }

            if (user == null)
            {
                url += "/" + AvatarDirectory + "/avatar_anonymous";
            }
            else
            {
                url += "/" + AvatarDirectory + "/" + user.Avatar;
            }

            switch (avatarSizeType)
            {
                case AvatarSizeType.Original:
                    url += "_original." + AvatarFileExtension;
                    break;
                case AvatarSizeType.Big:
                case AvatarSizeType.Medium:
                    url += "_big." + AvatarFileExtension;
                    break;
                case AvatarSizeType.Small:
                case AvatarSizeType.Micro:
                    url += "." + AvatarFileExtension;
                    break;
                default:
                    url = string.Empty;
                    break;
            }

            if (user != null && user.HasAvatar && !enableClientCaching)
            {
                url += "?lq=" + DateTime.UtcNow.Ticks;
            }

            return url;
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="avatarSizeType">头像尺寸类型</param>
        /// <returns></returns>
        public static IStoreFile GetAvatar(this IUserService userService, long userId, AvatarSizeType avatarSizeType)
        {
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            return storeProvider.GetFile(GetAvatarRelativePath(userId), GetAvatarFileName(userId, avatarSizeType));
        }

        /// <summary>
        /// 获取UserId头像存储的相对路径
        /// </summary>
        public static string GetAvatarRelativePath(long userId)
        {
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            string idString = userId.ToString().PadLeft(15, '0');
            return storeProvider.JoinDirectory(AvatarDirectory, idString.Substring(0, 5), idString.Substring(5, 5), idString.Substring(10, 5));
        }

        /// <summary>
        /// 获取头像文件名称
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <param name="avatarSizeType">头像尺寸类别</param>
        private static string GetAvatarFileName(long userId, AvatarSizeType avatarSizeType)
        {
            string filename;
            switch (avatarSizeType)
            {
                case AvatarSizeType.Original:
                    filename = string.Format("{0}_original.{1}", userId, AvatarFileExtension);
                    break;
                case AvatarSizeType.Big:
                    filename = string.Format("{0}_big.{1}", userId, AvatarFileExtension);
                    break;
                case AvatarSizeType.Medium:
                    filename = string.Format("{0}_big.{1}", userId, AvatarFileExtension);
                    break;
                case AvatarSizeType.Small:
                    filename = string.Format("{0}.{1}", userId, AvatarFileExtension);
                    break;
                case AvatarSizeType.Micro:
                    filename = string.Format("{0}.{1}", userId, AvatarFileExtension);
                    break;
                default:
                    filename = string.Empty;
                    break;
            }
            return filename;
        }

        #endregion 头像处理


        /// <summary>
        /// 获取完整的用户实体
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userId">用户ID</param>        
        public static User GetFullUser(this IUserService userService, long userId)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            return userRepository.GetUser(userId);
        }

        /// <summary>
        /// 获取完整的用户实体
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userName">用户名</param>
        public static User GetFullUser(this IUserService userService, string userName)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            long userId = UserIdToUserNameDictionary.GetUserId(userName);
            return userRepository.GetUser(userId);
        }

        /// <summary>
        /// 获取前N个用户
        /// </summary>
        /// <param name="topNumber">获取用户数</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public static IEnumerable<IUser> GetTopUsers(this IUserService userService, int topNumber, SortBy_User sortBy)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            return userRepository.GetTopUsers(topNumber, sortBy);
        }

        /// <summary>
        /// 根据排序条件分页显示用户
        /// </summary>
        /// <param name="sortBy">排序条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns>根据排序条件倒排序分页显示用户</returns>
        public static PagingDataSet<User> GetPagingUsers(this IUserService userService, SortBy_User? sortBy, int pageIndex, int pageSize)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            return userRepository.GetPagingUsers(sortBy, pageIndex, pageSize);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userQuery">查询用户条件</param>
        /// <param name="pageSize">页面显示条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public static PagingDataSet<User> GetUsers(this IUserService userService, UserQuery userQuery, int pageSize, int pageIndex)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            return userRepository.GetUsers(userQuery, pageSize, pageIndex);
        }

        /// <summary>
        /// 根据用户Id集合组装用户集合
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public static IEnumerable<User> GetFullUsers(this IUserService userService, IEnumerable<long> userIds)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            return userRepository.PopulateEntitiesByEntityIds<long>(userIds);
        }

        /// <summary>
        /// 帐号邮箱通过验证
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userId">用户Id</param>
        public static void UserEmailVerified(this IUserService userService, long userId)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            User user = userRepository.Get(userId);
            if (user == null)
                return;
            user.IsEmailVerified = true;
            userRepository.Update(user);

            EventBus<User>.Instance().OnAfter(user, new CommonEventArgs(EventOperationType.Instance().UserEmailVerified()));
        }

        /// <summary>
        /// 解除符合解除管制标准的用户（永久管制的用户不会自动解除管制）
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userId"></param>
        public static void NoModeratedUser(this IUserService userService, long userId)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            User user = userRepository.Get(userId);
            if (user == null)
                return;
            user.IsModerated = false;
            userRepository.Update(user);
            EventBus<User>.Instance().OnAfter(user, new CommonEventArgs(EventOperationType.Instance().AutoNoModeratedUser()));
        }

        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userId">用户Id</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">themeKey与appearanceKey用逗号关联</param>
        public static void ChangeThemeAppearance(this IUserService userService, long userId, bool isUseCustomStyle, string themeAppearance)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            userRepository.ChangeThemeAppearance(userId, isUseCustomStyle, themeAppearance);
        }

        /// <summary>
        /// 获取用户数据访问实例
        /// </summary>
        /// <param name="userService"></param>
        /// <returns></returns>
        private static IUserRepository GetUserRepository(this IUserService userService)
        {
            IUserRepository userRepository = DIContainer.Resolve<IUserRepository>();
            if (userRepository == null)
                userRepository = new UserRepository();
            return userRepository;
        }

        /// <summary>
        /// 根据用户状态获取用户数
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="isActivated">是否激活</param>
        /// <param name="isBanned">是否封禁</param>
        /// <param name="isModerated">是否管制</param>
        public static Dictionary<UserManageableCountType, int> GetManageableCounts(this IUserService userService, bool isActivated, bool isBanned, bool isModerated)
        {
            IUserRepository userRepository = userService.GetUserRepository();
            return userRepository.GetManageableCounts(isActivated, isBanned, isModerated);
        }

        /// <summary>
        /// 根据用户昵称获取用户
        /// </summary>
        /// <param name="service"></param>
        /// <param name="nickName">用户昵称</param>
        /// <returns></returns>
        public static IUser GetUserByNickName(this IUserService service, string nickName)
        {
            long userId = UserIdToNickNameDictionary.GetUserId(nickName);
            return service.GetUser(userId);
        }
    }
}
