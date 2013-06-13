using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    public class InvitedFriendViewModel
    {
        /// <summary>
        /// 用户的id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 对外显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 关注人的数量
        /// </summary>
        public long FollowedCount { get; set; }

        /// <summary>
        /// 粉丝数量
        /// </summary>
        public long FollowerCount { get; set; }

        /// <summary>
        /// 微博的数量
        /// </summary>
        public long MicroblogCount { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderType GenderType { get; set; }

        /// <summary>
        /// 被邀请用户
        /// </summary>
        public User User
        {
            get { return (User)new UserService().GetUser(this.UserId); }
        }
    }
}
