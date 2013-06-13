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
//    /// 显示Logo图片
//    /// </summary>
//    public class LogoHandler : DownloadFileHandlerBase
//    {
//        public override void ProcessRequest(HttpContext context)
//        {
//            long associateId = context.Request.QueryString.Get<long>("associateId", 0);

//            string tenantTypeId = context.Request.QueryString.GetString("TenantTypeId", string.Empty);
//            if (string.IsNullOrEmpty(tenantTypeId))
//            {
//                WebUtility.Return404(context);
//            }

//            bool enableCaching = context.Request.QueryString.GetBool("enableCaching", true);

//            string imageSizeTypeKey = context.Request.QueryString.GetString("ImageSizeTypeKey", string.Empty);

//            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
//            LogoService logoService = new LogoService(tenantTypeId);
//            IStoreFile logoFile = null;
//            if (!string.IsNullOrEmpty(imageSizeTypeKey))
//                logoFile = logoService.GetResizedLogo(associateId, imageSizeTypeKey);
//            else
//                logoFile = logoService.GetLogo(associateId);

//            //获取流文件
//            if (logoFile != null)
//            {
//                DateTime lastModified = logoFile.LastModified.ToUniversalTime();
//                if (enableCaching && IsCacheOK(context, lastModified))
//                {
//                    WebUtility.Return304(context);
//                    return;
//                }
//                context.Response.ContentType = MimeTypeConfiguration.GetMimeType(logoFile.Name);
//                DefaultStoreFile fileSystemFile = logoFile as DefaultStoreFile;
//                if (fileSystemFile != null && (!fileSystemFile.FullLocalPath.StartsWith(@"\")))
//                {
//                    context.Response.TransmitFile(fileSystemFile.FullLocalPath);
//                }
//                else
//                {
//                    context.Response.AddHeader("Content-Length", logoFile.Size.ToString("0"));
//                    using (Stream stream = logoFile.OpenReadStream())
//                    {
//                        if (stream == null)
//                        {
//                            WebUtility.Return404(context);
//                            return;
//                        }
//                        long buggerLength = logoFile.Size <= DownloadFileHandlerBase.BufferLength ? logoFile.Size : DownloadFileHandlerBase.BufferLength;
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
//                string imageFullPath = WebUtility.ResolveUrl("~/Themes/Shared/Styles/Images/figure.jpg");
//                if (enableCaching)
//                {
//                    context.Response.Cache.SetExpires(DateTime.Now.AddHours(2));
//                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
//                    context.Response.Cache.SetValidUntilExpires(true);
//                }
//                context.Response.ContentType = "image/png";
//                context.Response.TransmitFile(imageFullPath);
//            }
//            context.Response.Cache.VaryByParams["associateId"] = true;
//            context.Response.Cache.VaryByParams["TenantTypeId"] = true;
//            context.Response.Cache.VaryByParams["ImageSizeTypeKey"] = true;
//            context.Response.Cache.VaryByParams["enableCaching"] = true;
//            context.Response.End();
//        }
//    }
//}