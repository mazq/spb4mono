//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.FileStore;
using Tunynet.Common.Configuration;
using System.IO;
using Tunynet.Imaging;
using Tunynet.Events;
using System.Drawing;

namespace Tunynet.Common
{

    /// <summary>
    /// 附件业务逻辑类
    /// </summary>
    /// <typeparam name="T">附件实体类</typeparam>
    public class AttachmentService<T> where T : Attachment
    {
        private IUserService userService = DIContainer.Resolve<IUserService>();

        //attachmentRepository
        private IAttachmentRepository<T> attachmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public AttachmentService(string tenantTypeId)
        {
            this.attachmentRepository = new AttachmentRepository<T>();
            this.TenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);
            if (this.TenantAttachmentSettings == null)
                throw new ExceptionFacade("没有注册租户附件设置");
            this.StoreProvider = DIContainer.ResolveNamed<IStoreProvider>(this.TenantAttachmentSettings.StoreProviderName);
        }

        /// <summary>
        /// 可设置repository的构造函数（主要用于测试用例）
        /// </summary>
        /// <param name="attachmentRepository">附件仓储</param>
        /// <param name="tenantAttachmentSettings">租户附件设置</param>
        /// <param name="storeProvider">文件存储Provider</param>
        public AttachmentService(IAttachmentRepository<T> attachmentRepository, TenantAttachmentSettings tenantAttachmentSettings, IStoreProvider storeProvider)
        {
            this.attachmentRepository = attachmentRepository;
            this.TenantAttachmentSettings = tenantAttachmentSettings;
            this.StoreProvider = storeProvider;
        }

        /// <summary>
        /// 文件存储Provider
        /// </summary>
        public IStoreProvider StoreProvider { get; private set; }

        /// <summary>
        /// 租户附件设置
        /// </summary>
        public TenantAttachmentSettings TenantAttachmentSettings { get; private set; }

        #region Create & Delete


        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="attachment">附件</param>
        /// <param name="contentStream">文件流</param>
        public void Create(T attachment, Stream contentStream)
        {
            if (contentStream == null)
            {
                return;
            }

            if (attachment.MediaType == MediaType.Image)
            {
                //检查是否需要缩放原图
                if (TenantAttachmentSettings.MaxImageWidth > 0 || TenantAttachmentSettings.MaxImageHeight > 0)
                {
                    int maxWidth = TenantAttachmentSettings.MaxImageWidth > 0 ? TenantAttachmentSettings.MaxImageWidth : attachment.Width;
                    int maxHeight = TenantAttachmentSettings.MaxImageHeight > 0 ? TenantAttachmentSettings.MaxImageHeight : attachment.Height;

                    if (attachment.Width > maxWidth || attachment.Height > maxHeight)
                    {
                        Stream resizedStream = ImageProcessor.Resize(contentStream, maxWidth, maxHeight, ResizeMethod.KeepAspectRatio);
                        if (resizedStream != contentStream)
                        {
                            contentStream.Dispose();
                        }
                        contentStream = resizedStream;
                    }
                }

                IAttachmentSettingsManager attachmentSettingsManager = DIContainer.Resolve<IAttachmentSettingsManager>();
                AttachmentSettings attachmentSettings = attachmentSettingsManager.Get();

                Image image = Image.FromStream(contentStream);
                bool isGif = ImageProcessor.IsGIFAnimation(image);

                //检查是否需要打水印
                if (!isGif && TenantAttachmentSettings.EnableWatermark && attachmentSettings.WatermarkSettings.WatermarkType != WatermarkType.None && image.Width >= attachmentSettings.WatermarkSettings.WatermarkMinWidth && image.Height >= attachmentSettings.WatermarkSettings.WatermarkMinHeight)
                {
                    ImageProcessor imageProcessor = new ImageProcessor();

                    if (attachmentSettings.WatermarkSettings.WatermarkType == WatermarkType.Text)
                    {
                        TextWatermarkFilter watermarkFilter = new TextWatermarkFilter(attachmentSettings.WatermarkSettings.WatermarkText, attachmentSettings.WatermarkSettings.WatermarkLocation, attachmentSettings.WatermarkSettings.WatermarkOpacity);
                        imageProcessor.Filters.Add(watermarkFilter);
                    }
                    else if (attachmentSettings.WatermarkSettings.WatermarkType == WatermarkType.Image)
                    {
                        ImageWatermarkFilter watermarkFilter = new ImageWatermarkFilter(attachmentSettings.WatermarkSettings.WatermarkImagePhysicalPath, attachmentSettings.WatermarkSettings.WatermarkLocation, attachmentSettings.WatermarkSettings.WatermarkOpacity);
                        imageProcessor.Filters.Add(watermarkFilter);
                    }

                    //如果需要添加水印，则除水印图片以外，还需要保留原图
                    StoreProvider.AddOrUpdateFile(attachment.GetRelativePath(), attachment.GetOriginalFileName(), contentStream);

                    //给原始尺寸的图添加水印
                    using (Stream watermarkImageStream = imageProcessor.Process(contentStream))
                    {
                        StoreProvider.AddOrUpdateFile(attachment.GetRelativePath(), attachment.FileName, watermarkImageStream);
                    }

                    //根据设置生成不同尺寸的图片，并添加水印
                    if (TenantAttachmentSettings.ImageSizeTypes != null && TenantAttachmentSettings.ImageSizeTypes.Count > 0)
                    {
                        foreach (var imageSizeType in TenantAttachmentSettings.ImageSizeTypes)
                        {
                            Stream resizedStream = ImageProcessor.Resize(contentStream, imageSizeType.Size.Width, imageSizeType.Size.Height, imageSizeType.ResizeMethod);
                            image = Image.FromStream(resizedStream);
                            if (image.Width >= attachmentSettings.WatermarkSettings.WatermarkMinWidth && image.Height >= attachmentSettings.WatermarkSettings.WatermarkMinHeight)
                            {
                                using (Stream watermarkImageStream = imageProcessor.Process(resizedStream))
                                {
                                    StoreProvider.AddOrUpdateFile(attachment.GetRelativePath(), StoreProvider.GetSizeImageName(attachment.FileName, imageSizeType.Size, imageSizeType.ResizeMethod), watermarkImageStream);
                                }
                            }
                            else
                            {
                                StoreProvider.AddOrUpdateFile(attachment.GetRelativePath(), StoreProvider.GetSizeImageName(attachment.FileName, imageSizeType.Size, imageSizeType.ResizeMethod), resizedStream);
                            }

                            if (resizedStream != contentStream)
                            {
                                resizedStream.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    StoreProvider.AddOrUpdateFile(attachment.GetRelativePath(), attachment.FileName, contentStream);

                    if (!isGif)
                    {
                        //根据设置生成不同尺寸的图片
                        if (TenantAttachmentSettings.ImageSizeTypes != null && TenantAttachmentSettings.ImageSizeTypes.Count > 0)
                        {
                            foreach (var imageSizeType in TenantAttachmentSettings.ImageSizeTypes)
                            {
                                Stream resizedStream = ImageProcessor.Resize(contentStream, imageSizeType.Size.Width, imageSizeType.Size.Height, imageSizeType.ResizeMethod);
                                StoreProvider.AddOrUpdateFile(attachment.GetRelativePath(), StoreProvider.GetSizeImageName(attachment.FileName, imageSizeType.Size, imageSizeType.ResizeMethod), resizedStream);
                                if (resizedStream != contentStream)
                                {
                                    resizedStream.Dispose();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                StoreProvider.AddOrUpdateFile(attachment.GetRelativePath(), attachment.FileName, contentStream);
            }

            if (contentStream != null)
            {
                contentStream.Dispose();
            }

            EventBus<T>.Instance().OnBefore(attachment, new CommonEventArgs(EventOperationType.Instance().Create()));
            attachmentRepository.Insert(attachment);
            EventBus<T>.Instance().OnAfter(attachment, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// 附件重新命名（修改FriendlyFileName）
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="newFriendlyFileName">新附件名</param>
        public void Rename(long attachmentId, string newFriendlyFileName)
        {
            T attachment = Get(attachmentId);
            if (attachment != null)
            {
                attachment.FriendlyFileName = newFriendlyFileName;
                attachmentRepository.Update(attachment);
            }
        }

        /// <summary>
        /// 附件重新调整售价（修改Price）
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="price">新售价</param>
        public void UpdatePrice(long attachmentId, int price)
        {
            T attachment = Get(attachmentId);
            if (attachment != null)
            {
                attachment.Price = price;
                attachmentRepository.Update(attachment);
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        public void Delete(long attachmentId)
        {
            T attachment = Get(attachmentId);
            if (attachment != null)
            {
                Delete(attachment);
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="attachment">附件</param>
        public void Delete(T attachment)
        {
            DeleteStoredFile(attachment);
            EventBus<T>.Instance().OnBefore(attachment, new CommonEventArgs(EventOperationType.Instance().Delete()));
            attachmentRepository.Delete(attachment);
            EventBus<T>.Instance().OnAfter(attachment, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// 删除AssociateId相关的附件
        /// </summary>
        /// <param name="associateId">附件关联Id（例如：博文Id、帖子Id）</param>
        public virtual void DeletesByAssociateId(long associateId)
        {
            IEnumerable<T> attachments = GetsByAssociateId(associateId);
            foreach (var attachment in attachments)
            {
                DeleteStoredFile(attachment);
            }
            string tenantTypeId = this.TenantAttachmentSettings.TenantTypeId;
            attachmentRepository.DeletesByAssociateId(tenantTypeId, associateId);
        }

        /// <summary>
        /// 删除OwnerId相关的附件
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        public virtual void DeletesByOwnerId(long ownerId)
        {
            IEnumerable<T> attachments = Gets(ownerId);
            foreach (var attachment in attachments)
            {
                DeleteStoredFile(attachment);
            }
            string tenantTypeId = this.TenantAttachmentSettings.TenantTypeId;
            attachmentRepository.DeletesByOwnerId(tenantTypeId, ownerId);
        }
        
        /// <summary>
        /// 删除UserId相关的附件
        /// </summary>
        /// <param name="userId">上传者Id</param>
        public virtual void DeletesByUserId(long userId)
        {
            IEnumerable<T> attachments = GetsByUserId(userId);
            foreach (var attachment in attachments)
            {
                DeleteStoredFile(attachment);
            }
            string tenantTypeId = this.TenantAttachmentSettings.TenantTypeId;
            attachmentRepository.DeletesByUserId(tenantTypeId, userId);
        }

        /// <summary>
        /// 删除文件系统中的文件
        /// </summary>
        /// <param name="attachment">附件</param>
        protected void DeleteStoredFile(T attachment)
        {
            //如果属于图片附件，则还需删除生成的图片缩略图及附件原图
            if (attachment.MediaType == MediaType.Image)
            {
                //删除所有图(包括原始图、缩略图)
                StoreProvider.DeleteFiles(attachment.GetRelativePath(), attachment.FileName);
            }
            else
            {
                StoreProvider.DeleteFile(attachment.GetRelativePath(), attachment.FileName);
            }
        }

        /// <summary>
        /// 为指定用户生成指定附件的拷贝
        /// </summary>
        /// <param name="attachmentId">指定附件的id</param>
        /// <param name="userId">指定用户的id</param>
        /// <returns>新附件实体</returns>
        public T CloneForUser(long attachmentId, long userId)
        {
            return CloneForUser(attachmentRepository.Get(attachmentId), userId);
        }

        /// <summary>
        /// 为指定用户生成指定附件的拷贝
        /// </summary>
        /// <param name="attachment">指定附件实体</param>
        /// <param name="userId">指定用户的id</param>
        /// <returns>新附件实体</returns>
        public T CloneForUser(T attachment, long userId)
        {
            //复制数据库记录
            T newAttachment = (T)new Attachment();
            newAttachment.ContentType = attachment.ContentType;
            newAttachment.FileLength = attachment.FileLength;
            newAttachment.FriendlyFileName = attachment.FriendlyFileName;
            newAttachment.Height = attachment.Height;
            newAttachment.MediaType = attachment.MediaType;
            newAttachment.OwnerId = userId;
            newAttachment.TenantTypeId = attachment.TenantTypeId;
            newAttachment.UserDisplayName = userService.GetUser(userId).DisplayName;
            newAttachment.UserId = userId;
            newAttachment.Width = attachment.Width;
            newAttachment.FileName = newAttachment.GenerateFileName();

            EventBus<T>.Instance().OnBefore(newAttachment, new CommonEventArgs(EventOperationType.Instance().Create()));
            attachmentRepository.Insert(newAttachment);
            EventBus<T>.Instance().OnAfter(newAttachment, new CommonEventArgs(EventOperationType.Instance().Create()));

            //复制文件
            Stream stream = null;
            try
            {
                IStoreFile file = StoreProvider.GetFile(attachment.GetRelativePath(), attachment.FileName);
                stream = file.OpenReadStream();
                StoreProvider.AddOrUpdateFile(newAttachment.GetRelativePath(), newAttachment.FileName, stream);
            }
            catch
            {
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            //根据不同租户类型的设置生成不同尺寸的图片，用于图片直连访问
            if (newAttachment.MediaType == MediaType.Image)
            {
                TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(newAttachment.TenantTypeId);
                if (tenantAttachmentSettings != null && tenantAttachmentSettings.ImageSizeTypes != null && tenantAttachmentSettings.ImageSizeTypes.Count > 0)
                {
                    foreach (var imageSizeType in tenantAttachmentSettings.ImageSizeTypes)
                    {
                        IStoreFile file = StoreProvider.GetResizedImage(newAttachment.GetRelativePath(), newAttachment.FileName, imageSizeType.Size, imageSizeType.ResizeMethod);
                    }
                }
            }

            return newAttachment;
        }

        #endregion


        #region Get & Gets

        /// <summary>
        /// 依据attachmentId获取附件
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        public T Get(long attachmentId)
        {
            return attachmentRepository.Get(attachmentId);
        }

        /// <summary>
        /// 依据AssociateId获取单个附件（用于AssociateId与附件一对一关系）
        /// </summary>
        /// <param name="associateId">附件关联Id</param>
        /// <returns>附件</returns>
        public T GetByAssociateId(long associateId)
        {
            IEnumerable<T> attachments = GetsByAssociateId(associateId);
            if (attachments == null)
                return null;
            return attachments.FirstOrDefault<T>();
        }

        /// <summary>
        /// 依据AssociateId获取附件列表（用于AssociateId与附件一对多关系）
        /// </summary>
        /// <param name="associateId">附件关联Id</param>
        /// <returns>附件列表</returns>
        public IEnumerable<T> GetsByAssociateId(long associateId)
        {
            string tenantTypeId = this.TenantAttachmentSettings.TenantTypeId;
            return attachmentRepository.GetsByAssociateId(tenantTypeId, associateId);
        }

        /// <summary>
        /// 依据userId获取附件列表（用于userId与附件一对多关系）
        /// </summary>
        /// <param name="userId">附件上传人Id</param>
        /// <returns>附件列表</returns>
        public IEnumerable<T> GetsByUserId(long userId)
        {
            string tenantTypeId = this.TenantAttachmentSettings.TenantTypeId;
            return attachmentRepository.GetsByUserId(tenantTypeId, userId);
        }

        /// <summary>
        /// 获取拥有者的所有附件或者拥有者一种租户类型的附件
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns>附件列表</returns>
        public IEnumerable<T> Gets(long ownerId, string tenantTypeId = null)
        {
            return attachmentRepository.Gets(ownerId, tenantTypeId);
        }

        /// <summary>
        /// 搜索附件并分页显示
        /// </summary>
        /// <param name="tenantTypeId">附件租户类型</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>附件分页列表</returns>
        public PagingDataSet<T> Gets(string tenantTypeId, string keyword, int pageIndex)
        {
            return attachmentRepository.Gets(tenantTypeId, keyword, pageIndex);
        }

        /// <summary>
        /// 获取直连URL
        /// </summary>
        /// <param name="attachment">附件</param>
        /// <returns>返回可以http直连该附件的url</returns>
        public string GetDirectlyUrl(T attachment)
        {
            return StoreProvider.GetDirectlyUrl(attachment.GetRelativePath(), attachment.FileName);
        }

        #endregion


        #region 临时附件

        /// <summary>
        /// 获取拥有者一种租户类型的临时附件
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<T> GetTemporaryAttachments(long ownerId, string tenantTypeId)
        {
            return attachmentRepository.GetTemporaryAttachments(ownerId, tenantTypeId);
        }

        /// <summary>
        /// 删除拥有者的临时附件
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public void DeleteTemporaryAttachments(long ownerId, string tenantTypeId)
        {
            IEnumerable<T> attachments = GetTemporaryAttachments(ownerId, tenantTypeId);

            if (attachments.Count() == 0)
                return;

            foreach (var attachment in attachments)
            {
                Delete(attachment.AttachmentId);
            }
        }

        /// <summary>
        /// 删除垃圾临时附件
        /// </summary>
        public void DeleteTrashTemporaryAttachments()
        {
            IAttachmentSettingsManager attachmentSettingsManager = DIContainer.Resolve<IAttachmentSettingsManager>();
            if (attachmentSettingsManager == null)
                return;
            AttachmentSettings attachmentSettings = attachmentSettingsManager.Get();
            int temporaryAttachmentStorageDay = attachmentSettings.TemporaryAttachmentStorageDay;
            if (temporaryAttachmentStorageDay < 1)
                temporaryAttachmentStorageDay = 1;
            IEnumerable<T> attachments = attachmentRepository.GetTrashTemporaryAttachments(temporaryAttachmentStorageDay);
            if (attachments.Count() == 0)
                return;
            foreach (var attachment in attachments)
            {
                DeleteStoredFile(attachment);
            }
            attachmentRepository.DeleteTrashTemporaryAttachments(temporaryAttachmentStorageDay);
        }

        /// <summary>
        /// 把临时附件转成正常附件
        /// </summary>
        public void ToggleTemporaryAttachments(long ownerId, string tenantTypeId, long associateId)
        {
            attachmentRepository.ToggleTemporaryAttachments(ownerId, tenantTypeId, associateId);
        }

        #endregion

    }
}
