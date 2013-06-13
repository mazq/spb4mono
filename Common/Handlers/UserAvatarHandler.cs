////------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

//using System;
//using System.IO;
//using System.Web;
//using Spacebuilder.Common;
//using Tunynet.FileStore;
//using Tunynet.Utilities;

//namespace Spacebuilder.Common
//{
//    /// <summary>
//    /// 显示用户头像
//    /// </summary>
//    public class UserAvatarHandler : DownloadFileHandlerBase
//    {
//        public override void ProcessRequest(HttpContext context)
//        {
//            long userId = context.Request.QueryString.Get<long>("UserId", -1);
//            AvatarSizeType avatarSizeType = (AvatarSizeType)context.Request.QueryString.GetInt("AvatarSizeType", (int)AvatarSizeType.Small);
//            bool enableCaching = context.Request.QueryString.GetBool("EnableCaching", true);

//            if (userId <= 0)
//            {
//                WebUtility.Return404(context);
//                return;
//            }

//            UserProfileService userProfileService = new UserProfileService();
//            UserProfile userProfile = userProfileService.Get(userId);
//            if (userProfile != null)
//            {
//                IStoreFile avatarFile = userProfileService.GetAvatar(userId, avatarSizeType);

//                if (avatarFile == null)
//                {
//                    WebUtility.Return404(context);
//                    return;
//                }

//                DateTime lastModified = avatarFile.LastModified.ToUniversalTime();
//                if (enableCaching && IsCacheOK(context, lastModified))
//                {
//                    WebUtility.Return304(context);
//                    return;
//                }
//                else
//                {
//                    context.Response.ContentType = "image/jpeg";

//                    DefaultStoreFile fileSystemFile = avatarFile as DefaultStoreFile;
//                    if (fileSystemFile != null && (!fileSystemFile.FullLocalPath.StartsWith(@"\")))
//                    {
//                        // Send files stored on UNC paths explicitly to avoid a bug with TransmitFile.
//                        context.Response.TransmitFile(fileSystemFile.FullLocalPath);
//                    }
//                    else
//                    {
//                        context.Response.AddHeader("Content-Length", avatarFile.Size.ToString("0"));
//                        using (Stream stream = avatarFile.OpenReadStream())
//                        {
//                            if (stream == null)
//                            {
//                                WebUtility.Return404(context);
//                                return;
//                            }

//                            long bufferLength = avatarFile.Size <= DownloadFileHandlerBase.BufferLength ? avatarFile.Size : DownloadFileHandlerBase.BufferLength;
//                            byte[] buffer = new byte[bufferLength];
//                            int readedSize;
//                            while ((readedSize = stream.Read(buffer, 0, (int)bufferLength)) > 0)
//                            {
//                                if (!context.Response.IsClientConnected)
//                                    break;

//                                context.Response.OutputStream.Write(buffer, 0, readedSize);
//                            }

//                            stream.Close();
//                            stream.Dispose();
//                            context.Response.OutputStream.Flush();
//                            context.Response.Flush();
//                        }
//                    }

//                    if (enableCaching)
//                    {
//                        // Browser cache settings
//                        context.Response.Cache.SetCacheability(HttpCacheability.Private);
//                        context.Response.Cache.SetLastModified(lastModified);
//                        context.Response.Cache.SetETag(lastModified.Ticks.ToString());
//                        context.Response.Cache.SetAllowResponseInBrowserHistory(true);
//                        context.Response.Cache.SetValidUntilExpires(true);
//                    }
//                    else
//                    {
//                        context.Response.Cache.SetExpires(DateTime.Now.AddMonths(-1));
//                    }
//                }
//            }
//            else
//            {
//                string avatarFileName = "avatar{0}{1}.png";
//                string strAvatarSizeType = (avatarSizeType == AvatarSizeType.Big || avatarSizeType == AvatarSizeType.Medium) ? "_big" : string.Empty;
//                if (userProfile != null && userProfile.Gender != GenderType.NotSet)
//                {
//                    avatarFileName = string.Format(avatarFileName, "_" + userProfile.Gender.ToString().ToLower(), strAvatarSizeType);
//                }
//                else
//                {
//                    avatarFileName = string.Format(avatarFileName, "_default", strAvatarSizeType);
//                }

//                string avatarFullPath = context.Server.MapPath("~/Themes/Shared/Styles/Images/" + avatarFileName);
//                if (enableCaching)
//                {
//                    context.Response.Cache.SetExpires(DateTime.Now.AddHours(2));
//                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
//                    context.Response.Cache.SetValidUntilExpires(true);
//                }

//                context.Response.ContentType = "image/png";
//                context.Response.TransmitFile(avatarFullPath);
//            }

//            context.Response.Cache.VaryByParams["UserId"] = true;
//            context.Response.Cache.VaryByParams["AvatarSizeType"] = true;
//            context.Response.Cache.VaryByParams["enableCaching"] = true;

//            context.Response.End();
//        }
//    }
//}