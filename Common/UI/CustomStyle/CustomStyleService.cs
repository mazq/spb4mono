//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tunynet.Common;
using Tunynet.FileStore;
using Tunynet;
using Tunynet.Utilities;
using System.Xml.Linq;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.UI
{

    /// <summary>
    /// 自定义样式业务逻辑类
    /// </summary>
    public class CustomStyleService
    {
        private static readonly string BackgroundImageFileExtension = "jpg";

        private static readonly string BackgroundImageDirectory = "CumstomStyle";

        private ICustomStyleRepository customStyleRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public CustomStyleService()
            : this(new CustomStyleRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public CustomStyleService(ICustomStyleRepository customStyleRepository)
        {
            this.customStyleRepository = customStyleRepository;
        }


        /// <summary>
        /// 获取呈现区域预置的配色方案
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns></returns>
        public IEnumerable<CustomStyle> GetColorSchemes(string presentAreaKey)
        {
            //从 \Themes\[PresentAreaKey]\CustomStyle\ColorScheme.config 读取预置的配色方案
            return customStyleRepository.GetColorSchemes(presentAreaKey);
        }

        /// <summary>
        /// 获取用户自定义风格
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">OwnerId</param>
        /// <returns>无相应数据返回null</returns>
        public CustomStyleEntity Get(string presentAreaKey, long ownerId)
        {
            return customStyleRepository.Get(presentAreaKey, ownerId);
        }

        /// <summary>
        /// 保存用户自定义风格
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">OwnerId</param>
        /// <param name="customStyle">自定义风格实体</param>
        public void Save(string presentAreaKey, long ownerId, CustomStyle customStyle)
        {
            //如果 presentAreaKey+ownerId 存在则更新，否则创建
            customStyleRepository.Save(presentAreaKey, ownerId, customStyle);
        }

        /// <summary>
        /// 上传背景图片
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">OwnerId</param>
        /// <param name="postedFile">上传的图片文件流</param>
        public void UploadBackgroundImage(string presentAreaKey, long ownerId, Stream postedFile)
        {
            if (postedFile == null)
                return;

            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            storeProvider.AddOrUpdateFile(GetBackgroundImageRelativePath(presentAreaKey, ownerId), GetBackgroundImageFileName(ownerId), postedFile);
            postedFile.Dispose();
        }

        /// <summary>
        /// 获取直连URL
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="avatarSizeType"><see cref="BackgroundImageSizeType"/></param>
        /// <returns></returns>
        public string GetBackgroundImageDirectlyUrl(string presentAreaKey, long ownerId)
        {
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            return storeProvider.GetDirectlyUrl(GetBackgroundImageRelativePath(presentAreaKey, ownerId), GetBackgroundImageFileName(ownerId));
        }

        /// <summary>
        /// 获取背景图
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <returns></returns>
        public IStoreFile GetBackgroundImage(string presentAreaKey, long ownerId)
        {
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            return storeProvider.GetFile(GetBackgroundImageRelativePath(presentAreaKey, ownerId), GetBackgroundImageFileName(ownerId));
        }

        /// <summary>
        /// 获取拥有者Id背景图存储的相对路径
        /// </summary>
        private string GetBackgroundImageRelativePath(string presentAreaKey, long ownerId)
        {
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            string idString = ownerId.ToString().PadLeft(15, '0');
            return storeProvider.JoinDirectory(BackgroundImageDirectory, presentAreaKey, idString.Substring(0, 5), idString.Substring(5, 5), idString.Substring(10, 5));
        }

        /// <summary>
        /// 获取背景图文件名称
        /// </summary>
        /// <param name="ownerId">UserID</param>
        private string GetBackgroundImageFileName(long ownerId)
        {
            return string.Format("{0}.{1}", ownerId, BackgroundImageFileExtension);
        }

    }
}
