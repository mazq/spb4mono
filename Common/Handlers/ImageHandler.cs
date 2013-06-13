////------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
//using System;
//using System.IO;
//using System.Web;
//using Spacebuilder.Common;
//using Tunynet;
//using Tunynet.Common;
//using Tunynet.Common.Configuration;
//using Tunynet.FileStore;
//using Tunynet.Utilities;

//namespace Spacebuilder.Common
//{
//    /// <summary>
//    /// 显示附件图片
//    /// </summary>
//    public class ImageHandler : DownloadFileHandlerBase
//    {
//        public override void ProcessRequest(HttpContext context)
//        {
//            long attachmentId = context.Request.QueryString.Get<long>("AttachmentId", 0);

//            string tenantTypeId = context.Request.QueryString.GetString("TenantTypeId", string.Empty);
//            if (string.IsNullOrEmpty(tenantTypeId))
//            {
//                WebUtility.Return404(context);
//            }

//            bool enableCaching = context.Request.QueryString.GetBool("enableCaching", true);


//            string imageSizeTypeKey = context.Request.QueryString.GetString("ImageSizeTypeKey", string.Empty);


//            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
//            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(tenantTypeId);
//            Attachment attachment = attachementService.Get(attachmentId);
//            //获取流文件
//            if (attachment != null && attachment.MediaType == MediaType.Image)
//            {
//                IStoreFile imageFile = null;
//                if (string.IsNullOrEmpty(imageSizeTypeKey))
//                    imageFile = storeProvider.GetFile(attachment.GetRelativePath(), attachment.FileName);
//                else
//                {
//                    TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(attachment.TenantTypeId);
//                    if (tenantAttachmentSettings == null)
//                        WebUtility.Return404(context);
//                    if (tenantAttachmentSettings.ImageSizeTypes == null || !tenantAttachmentSettings.ImageSizeTypes.ContainsKey(imageSizeTypeKey))
//                        imageFile = storeProvider.GetFile(attachment.GetRelativePath(), attachment.FileName);
//                    else
//                    {
//                        var pair = tenantAttachmentSettings.ImageSizeTypes[imageSizeTypeKey];
//                        imageFile = storeProvider.GetResizedImage(attachment.GetRelativePath(), attachment.FileName, pair.Key, pair.Value);
//                    }
//                }

//                if (imageFile == null)
//                    WebUtility.Return404(context);
//                DateTime lastModified = imageFile.LastModified.ToUniversalTime();
//                if (enableCaching && IsCacheOK(context, lastModified))
//                {
//                    WebUtility.Return304(context);
//                    return;
//                }
//                context.Response.ContentType = MimeTypeConfiguration.GetMimeType(attachment.FileName);
//                DefaultStoreFile fileSystemFile = imageFile as DefaultStoreFile;
//                if (fileSystemFile != null && (!fileSystemFile.FullLocalPath.StartsWith(@"\")))
//                {
//                    context.Response.TransmitFile(fileSystemFile.FullLocalPath);
//                }
//                else
//                {
//                    context.Response.AddHeader("Content-Length", imageFile.Size.ToString("0"));
//                    using (Stream stream = imageFile.OpenReadStream())
//                    {
//                        if (stream == null)
//                        {
//                            WebUtility.Return404(context);
//                            return;
//                        }
//                        long buggerLength = imageFile.Size <= DownloadFileHandlerBase.BufferLength ? imageFile.Size : DownloadFileHandlerBase.BufferLength;
//                        byte[] buffer = new byte[buggerLength];

//                        int readedSize;
//                        while ((readedSize = stream.Read(buffer, 0, (int)buggerLength)) > 0)
//                        {
//                            if (!context.Response.IsClientConnected)
//                                break;
//                            context.Response.OutputStream.Write(buffer, 0, readedSize);
//                        }
//                        stream.Close();
//                        stream.Dispose();
//                        context.Response.OutputStream.Flush();
//                        context.Response.Flush();
//                    }
//                }
//                if (enableCaching)
//                {
//                    context.Response.Cache.SetCacheability(HttpCacheability.Private);
//                    context.Response.Cache.SetLastModified(lastModified);
//                    context.Response.Cache.SetETag(lastModified.Ticks.ToString());
//                    context.Response.Cache.SetAllowResponseInBrowserHistory(true);
//                    context.Response.Cache.SetValidUntilExpires(true);
//                }

//                else
//                {
//                    context.Response.Cache.SetExpires(DateTime.Now.AddMonths(-1));
//                }
//            }
//            else
//            {
//                string imageFullPath = WebUtility.ResolveUrl("~/Themes/Shared/Styles/Images/default_img.png");
//                if (enableCaching)
//                {
//                    context.Response.Cache.SetExpires(DateTime.Now.AddHours(2));
//                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
//                    context.Response.Cache.SetValidUntilExpires(true);
//                }
//                context.Response.ContentType = "image/png";
//                context.Response.TransmitFile(imageFullPath);
//            }
//            context.Response.Cache.VaryByParams["AttachmentId"] = true;
//            context.Response.Cache.VaryByParams["TenantTypeId"] = true;
//            context.Response.Cache.VaryByParams["ImageSizeTypeKey"] = true;
//            context.Response.Cache.VaryByParams["enableCaching"] = true;
//            context.Response.End();
//        }
//    }
//}