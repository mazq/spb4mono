
using Tunynet.Common;
using Tunynet.UI;
using System;

namespace Spacebuilder.Microblog
{
    public class MicroblogApplication : ApplicationBase
    {
        private NavigationService navigationService = new NavigationService();
        private ApplicationService applicationService = new ApplicationService();

        protected MicroblogApplication(ApplicationModel model, ApplicationConfig config)
            : base(model, config)
        { }



        protected override bool Install(string presentAreaKey, long ownerId)
        {
            return true;
        }

        /// <summary>
        /// 卸载应用
        /// </summary>
        /// <param name="presentAreaKey">呈现区域</param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        protected override bool UnInstall(string presentAreaKey, long ownerId)
        {
            if (presentAreaKey == PresentAreaKeysOfBuiltIn.UserSpace)
            {
                new MicroblogService().DeleteUser(ownerId, "", false);
            }

            return true;
        }

        /// <summary>
        /// 删除用户调用方法
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="takeOverUserName">需要转移给的用户</param>
        /// <param name="isTakeOver">是否转移</param>
        protected override void DeleteUser(long userId, string takeOverUserName, bool isTakeOver)
        {
            new MicroblogService().DeleteUser(userId, takeOverUserName, isTakeOver);
        }
    }
}