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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 处理文件附件下载
    /// </summary>
    public class AttachmentDownloadHandler : DownloadFileHandlerBase
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

            string tenantTypeId = context.Request.QueryString.GetString("tenantTypeId", string.Empty);
            if (string.IsNullOrEmpty(tenantTypeId))
            {
                WebUtility.Return404(context);
                return;
            }

            //检查链接是否过期
            string token = context.Request.QueryString.GetString("token", string.Empty);
            bool isTimeout = true;
            long attachmentIdInToken = Utility.DecryptTokenForAttachmentDownload(token, out isTimeout);
            if (isTimeout || attachmentIdInToken != attachmentId)
            {
                WebUtility.Return403(context);
                return;
            }

            AttachmentService<Attachment> attachmentService = new AttachmentService<Attachment>(tenantTypeId);
            Attachment attachment = attachmentService.Get(attachmentId);
            if (attachment == null)
            {
                WebUtility.Return404(context);
                return;
            }

            bool enableCaching = context.Request.QueryString.GetBool("enableCaching", true);
            DateTime lastModified = attachment.DateCreated.ToUniversalTime();
            if (enableCaching && IsCacheOK(context, lastModified))
            {
                WebUtility.Return304(context);
                return;
            }

            //输出文件流
            IStoreFile storeFile = storeProvider.GetFile(attachment.GetRelativePath(), attachment.FileName);
            if (storeFile == null)
            {
                WebUtility.Return404(context);
                return;
            }

            context.Response.Clear();
            //context.Response.ClearHeaders();
            //context.Response.Cache.VaryByParams["attachmentId"] = true;
            string fileExtension = attachment.FileName.Substring(attachment.FileName.LastIndexOf('.') + 1);
            string friendlyFileName = attachment.FriendlyFileName + (attachment.FriendlyFileName.EndsWith(fileExtension) ? "" : "." + fileExtension);
            SetResponsesDetails(context, attachment.ContentType, friendlyFileName, lastModified);

            DefaultStoreFile fileSystemFile = storeFile as DefaultStoreFile;
            if (!fileSystemFile.FullLocalPath.StartsWith(@"\"))
            {
                //本地文件下载
                context.Response.TransmitFile(fileSystemFile.FullLocalPath);
                context.Response.End();
            }
            else
            {
                context.Response.AddHeader("Content-Length", storeFile.Size.ToString("0"));
                context.Response.Buffer = false;
                context.Response.BufferOutput = false;

                using (Stream stream = fileSystemFile.OpenReadStream())
                {
                    if (stream == null)
                    {
                        WebUtility.Return404(context);
                        return;
                    }
                    long bufferLength = fileSystemFile.Size <= DownloadFileHandlerBase.BufferLength ? fileSystemFile.Size : DownloadFileHandlerBase.BufferLength;
                    byte[] buffer = new byte[bufferLength];

                    int readedSize;
                    while ((readedSize = stream.Read(buffer, 0, (int)bufferLength)) > 0 && context.Response.IsClientConnected)
                    {
                        context.Response.OutputStream.Write(buffer, 0, readedSize);
                        context.Response.Flush();
                    }

                    //context.Response.OutputStream.Flush();
                    //context.Response.Flush();
                    stream.Close();
                }
                context.Response.End();
            }
        }

    }
}