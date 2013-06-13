//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Web;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.FileStore;
using Tunynet.Utilities;
using Tunynet.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 检测文件附件下载权限，如未购买，则先购买
    /// </summary>
    public class AttachmentAuthorizeHandler : DownloadFileHandlerBase
    {
        private IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
        private AttachmentDownloadService attachmentDownloadService = new AttachmentDownloadService();

        public override void ProcessRequest(HttpContext context)
        {
            long attachmentId = context.Request.QueryString.Get<long>("attachmentId", 0);
            if (attachmentId <= 0)
            {
                WebUtility.Return404(context);
                return;
            }

            string tenantTypeId = context.Request.QueryString.Get<string>("tenantTypeId", null);

            if (string.IsNullOrEmpty(tenantTypeId))
            {
                WebUtility.Return404(context);
                return;
            }
            AttachmentService<Attachment> attachmentService = new AttachmentService<Attachment>(tenantTypeId);
            Attachment attachment = attachmentService.Get(attachmentId);
            if (attachment == null)
            {
                WebUtility.Return404(context);
                return;
            }

            IUser currentUser = UserContext.CurrentUser;

            //判断是否有附件的购买权限或下载权限，有下载权限肯定有购买权限，目前只有未登录或积分不够时才判定为没有权限
            if (!new Authorizer().Attachment_Buy(attachment))
            {
                WebUtility.Return403(context);
                return;
            }

            //如果还没有下载权限，则说明积分可以支付附件售价，但是还未购买，则先进行积分交易
            if (!new Authorizer().Attachment_Download(attachment))
            {
                //积分交易
                PointService pointService = new PointService();
                pointService.Trade(currentUser.UserId, attachment.UserId, attachment.Price, string.Format("购买附件{0}", attachment.FriendlyFileName), true);
            }

            //创建下载记录         
            AttachmentDownloadService attachmentDownloadService = new AttachmentDownloadService();
            attachmentDownloadService.Create(currentUser == null ? 0 : currentUser.UserId, attachment.AttachmentId);

            //下载计数
            CountService countService = new CountService(TenantTypeIds.Instance().Attachment());
            countService.ChangeCount(CountTypes.Instance().DownloadCount(), attachment.AttachmentId, attachment.UserId, 1, false);

            bool enableCaching = context.Request.QueryString.GetBool("enableCaching", true);

            context.Response.Status = "302 Object Moved";
            context.Response.StatusCode = 302;

            LinktimelinessSettings linktimelinessSettings = DIContainer.Resolve<ILinktimelinessSettingsManager>().Get();
            string token = Utility.EncryptTokenForAttachmentDownload(linktimelinessSettings.Highlinktimeliness, attachmentId);
            context.Response.Headers.Add("Location", SiteUrls.Instance().AttachmentTempUrl(attachment.AttachmentId, tenantTypeId, token, enableCaching));

            context.Response.Flush();
            context.Response.End();

        }


    }
}