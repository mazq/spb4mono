//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.Events;
using System.Collections.Generic;
using Tunynet.Globalization;


namespace Spacebuilder.Common.EventModules
{
    /// <summary>
    /// 处理用户索引的EventMoudle
    /// </summary>
    public class UserIndexEventModule : IEventMoudle
    {
        private IUserService userService = DIContainer.Resolve<IUserService>();
        private TagService tagService = new TagService(TenantTypeIds.Instance().User());
        private InviteFriendService inviteFriendService = new InviteFriendService();

        //因为EventModule.RegisterEventHandler()在web启动时初始化，而UserSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此UserSearcher不能在此处构建，只能再下面的方法中构建
        private UserSearcher userSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<User>.Instance().After += new CommonEventHandler<User, CommonEventArgs>(User_After);
            EventBus<User, DeleteUserEventArgs>.Instance().After += new CommonEventHandler<User, DeleteUserEventArgs>(DeleteUser_After);            
            EventBus<UserProfile>.Instance().After += new CommonEventHandler<UserProfile, CommonEventArgs>(UserProfile_After);
            EventBus<User, CropAvatarEventArgs>.Instance().After += new CommonEventHandler<User, CropAvatarEventArgs>(FirstUploadAvatar_After);
            EventBus<string, TagEventArgs>.Instance().After += new CommonEventHandler<string, TagEventArgs>(AddTag);
            EventBus<ItemInTag>.Instance().After += new CommonEventHandler<ItemInTag, CommonEventArgs>(DeleteItemInTags);
            EventBus<Tag>.Instance().Before += new CommonEventHandler<Tag, CommonEventArgs>(DeleteUpdateTags_Before);
            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddTagsToUser_BatchAfter);
        }

        #region 标签增量索引
        /// <summary>
        /// 为用户添加标签时触发
        /// </summary>
        private void AddTagsToUser_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().User())
            {
                long userId = eventArgs.ItemId;
                if (userSearcher == null)
                {
                    userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
                }
                userSearcher.Update(userService.GetFullUser(userId));
            }
        }
        /// <summary>
        /// 删除和更新标签时触发
        /// </summary>
        private void DeleteUpdateTags_Before(Tag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().User())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    //根据标签获取所有使用该标签的(内容项)用户
                    IEnumerable<long> userIds = tagService.GetItemIds(sender.TagName, null);
                    if (userSearcher == null)
                    {
                        userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
                    }
                    userSearcher.Update(userService.GetFullUsers(userIds));
                }
            }
        }
        private void DeleteItemInTags(ItemInTag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().User())
            {
                long userId = sender.ItemId;
                if (userSearcher == null)
                {
                    userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
                }
                userSearcher.Update(userService.GetFullUser(userId));
            }
        }
        #endregion    
        private void AddTag(string tagName, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().User())
            {
                long userId = eventArgs.ItemId;
                if (userSearcher == null)
                {
                    userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
                }
                userSearcher.Update(userService.GetFullUser(userId));
            }
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="eventArgs"></param>
        private void DeleteUser_After(User user, DeleteUserEventArgs eventArgs)
        {
            if (userSearcher == null)
            {
                userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            }
                //使用UserSearcher删除索引
                userSearcher.Delete(user.UserId);
 
        }

        private void User_After(User user, CommonEventArgs eventArgs)
        {
            if (user == null)
            {
                return;
            }

            if (userSearcher == null)
            {
                userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            }

            if (eventArgs.EventOperationType == EventOperationType.Instance().Update()
                || eventArgs.EventOperationType == EventOperationType.Instance().ActivateUser()
                || eventArgs.EventOperationType == EventOperationType.Instance().Approved()
                || eventArgs.EventOperationType == EventOperationType.Instance().BanUser()
                || eventArgs.EventOperationType == EventOperationType.Instance().UnbanUser()
                || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {
                //使用UserSearcher更新索引
                userSearcher.Update(user);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                //使用UserSearcher删除索引
                userSearcher.Delete(user.UserId);
            }
        }

        private void UserProfile_After(UserProfile userProfile, CommonEventArgs eventArgs)
        {
            if (userProfile == null)
            {
                return;
            }

            if (userSearcher == null)
            {
                userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            }

            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                //增加新索引的事件由UserProfile触发，因为没有用户基本资料，就没有索引和搜索的价值
                User user = userService.GetFullUser(userProfile.UserId);
                userSearcher.Insert(user);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                //使用UserSearcher更新索引
                User user = userService.GetFullUser(userProfile.UserId);
                userSearcher.Update(user);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                //使用UserSearcher删除索引（Profile删除，User不一定删除）
                User user = userService.GetFullUser(userProfile.UserId);
                userSearcher.Update(user);//因为是删除用户资料，不是删除用户本身，所以此处是update
            }
        }

        private void FirstUploadAvatar_After(User user, CropAvatarEventArgs eventArgs)
        {
            if (user == null)
            {
                return;
            }

            if (userSearcher == null)
            {
                userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            }

            if (eventArgs.IsFirst)
            {
                //使用UserSearcher更新索引
                userSearcher.Update(user);
            }
        }

    }
}
