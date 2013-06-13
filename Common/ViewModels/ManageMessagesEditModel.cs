using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Tunynet.Email;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 消息设置
    /// </summary>
    public class MessagesSettingEditModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MessagesSettingEditModel()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="noticeSettings"></param>
        /// <param name="invitationSettings"></param>
        public MessagesSettingEditModel(NoticeSettings noticeSettings, InvitationSettings invitationSettings)
        {
            if (noticeSettings != null)
            {
                NoticeTypeSettings = noticeSettings.NoticeTypeSettingses;
            }
            if (invitationSettings!=null)
            {
                InvitationTypeSettings = invitationSettings.InvitationTypeSettingses;
            }
        }

        /// <summary>
        /// 通知类型设置实体类集合
        /// </summary>
        public List<NoticeTypeSettings> NoticeTypeSettings { get; set; }

        /// <summary>
        ///  请求类型设置实体类集合
        /// </summary>
        public List<InvitationTypeSettings> InvitationTypeSettings { get; set; }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <param name="invitationSettings"></param>
        /// <returns></returns>
        public NoticeSettings AsMessagesSettings(out InvitationSettings invitationSettings)
        {
            NoticeSettings noticeSettings = new NoticeSettings();
            noticeSettings.NoticeTypeSettingses = NoticeTypeSettings;

            invitationSettings = new InvitationSettings();
            invitationSettings.InvitationTypeSettingses = InvitationTypeSettings;

            return noticeSettings;
        }
    }
}
