//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using System.Collections.Concurrent;
using System.Web.Routing;
using Tunynet.Events;
using Tunynet.Common;
using System.Web.Mvc;
using Tunynet.Utilities;
using System.IO;
using System.Xml.Linq;

namespace Tunynet.UI
{
    /// <summary>
    /// 皮肤业务逻辑类
    /// </summary>
    public class ThemeService
    {
        //Repository
        private IRepository<ThemeAppearance> appearanceRepository;
        private IRepository<Theme> themeRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ThemeService()
            : this(new Repository<Theme>(), new Repository<ThemeAppearance>())
        {
        }

        /// <summary>
        /// 构造函数（主要用于测试用例）
        /// </summary>
        public ThemeService(IRepository<Theme> themeRepository, IRepository<ThemeAppearance> arrearanceRepository)
        {
            this.themeRepository = themeRepository;
            this.appearanceRepository = arrearanceRepository;
        }

        #region Theme

        /// <summary>
        /// 获取Theme
        /// </summary>
        /// <param name="id">presentAreaKey与themeKey用逗号关联</param>
        public Theme GetTheme(string id)
        {
            return themeRepository.Get(id);
        }

        /// <summary>
        /// 获取ThemeAppearance
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeKey">主题标识</param>        
        public Theme GetTheme(string presentAreaKey, string themeKey)
        {
            string id = string.Join(",", presentAreaKey, themeKey);
            return themeRepository.Get(id);
        }

        #endregion


        #region ThemeAppearance

        /// <summary>
        /// 提取皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域</param>
        /// <param name="fileName">皮肤文件名</param>
        /// <param name="fileStream">皮肤文件流</param>
        public void ExtractThemeAppearance(string presentAreaKey, string fileName, Stream fileStream)
        {
            if (fileStream == null || !fileStream.CanRead)
                return;
            string tempKey = fileName.Substring(0, fileName.LastIndexOf("."));
            string tempPath = WebUtility.GetPhysicalFilePath(string.Format("~/Themes/{0}/temp/{1}", presentAreaKey, tempKey));

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            string fileFullName = Path.Combine(tempPath, fileName);
            //将皮肤包存储到指定目录
            SaveThemeAppearancePackage(fileFullName, fileStream);

            #region 解压压缩包

            Ionic.Zip.ReadOptions ro = new Ionic.Zip.ReadOptions();
            ro.Encoding = System.Text.Encoding.UTF8;

            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(fileFullName, ro))
            {
                foreach (Ionic.Zip.ZipEntry zipEntry in zip)
                {
                    zipEntry.Extract(tempPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                }
            }
            #endregion

            string[] themeConfigFileNames = Directory.GetFiles(tempPath, "Theme.config", SearchOption.AllDirectories);
            if (themeConfigFileNames != null && themeConfigFileNames.Length > 0)
            {
                var themeConfigFileName = themeConfigFileNames[0];
                XElement themeElement = XElement.Load(themeConfigFileName);
                if (themeElement == null)
                {
                    Directory.Delete(tempPath, true);
                    throw new ExceptionFacade("Theme.config配置文件格式不正确");
                }

                var parentDirectory = Directory.GetParent(themeConfigFileName);

                //解析外观
                string appearancesPath = Path.Combine(parentDirectory.FullName, "Appearances");
                int displayOrder = GetThemeAppearances(presentAreaKey, null).Max(n => n.DisplayOrder) + 1;
                string themeKey = string.Empty;
                foreach (var appearanceDirectory in Directory.GetDirectories(appearancesPath))
                {
                    string appearanceConfig = Path.Combine(appearanceDirectory, "Appearance.config");
                    XElement appearanceElement = XElement.Load(appearanceConfig);
                    if (appearanceElement == null)
                    {
                        Directory.Delete(appearanceDirectory, true);
                        throw new ExceptionFacade("Appearance.config配置文件格式不正确");
                    }
                    string appearanceKey = appearanceDirectory.TrimEnd('\\').Substring(appearanceDirectory.LastIndexOf("\\") + 1);
                    ThemeAppearance themeAppearance = new ThemeAppearance(appearanceElement, appearanceKey);
                    themeAppearance.DisplayOrder = displayOrder;
                    displayOrder++;
                    if (appearanceRepository.Get(themeAppearance.Id) != null)
                        appearanceRepository.Update(themeAppearance);
                    else
                    {
                        appearanceRepository.Insert(themeAppearance);
                    }
                    if (string.IsNullOrEmpty(themeKey))
                        themeKey = themeAppearance.ThemeKey;
                }
                //解析主题
                Theme theme = new Theme(themeElement, themeKey);
                if (GetTheme(theme.Id) != null)
                {
                    themeRepository.Update(theme);
                }
                else
                    themeRepository.Insert(theme);

                string themePath = WebUtility.GetPhysicalFilePath(string.Format("~/Themes/{0}/{1}", presentAreaKey, themeKey));

                //移动目录
                if (Directory.Exists(themePath))
                    Directory.Delete(themePath, true);
                parentDirectory.MoveTo(themePath);
                if (Directory.Exists(tempPath))
                    Directory.Delete(tempPath, true);
            }
            else
            {
                string[] appearanceConfigFileNames = Directory.GetFiles(tempPath, "Appearance.config", SearchOption.AllDirectories);
                if (appearanceConfigFileNames != null && appearanceConfigFileNames.Length > 0)
                {
                    string appearanceConfigFileName = appearanceConfigFileNames[0];
                    XElement appearanceDocument = XElement.Load(appearanceConfigFileName);
                    if (appearanceDocument == null)
                    {
                        Directory.Delete(tempPath, true);
                        throw new ExceptionFacade("Appearance.config配置文件格式不正确");
                    }
                    //获取父级目录
                    var parentDirectory = Directory.GetParent(appearanceConfigFileName);
                    ThemeAppearance themeAppearance = new ThemeAppearance(appearanceDocument, parentDirectory.Name);
                    themeAppearance.DisplayOrder = GetThemeAppearances(presentAreaKey, null).Max(n => n.DisplayOrder) + 1;
                    string appearancesPath = WebUtility.GetPhysicalFilePath(string.Format("~/Themes/{0}/{1}/Appearances/", presentAreaKey, themeAppearance.ThemeKey));
                    //更新数据库
                    if (appearanceRepository.Get(themeAppearance.Id) != null)
                        appearanceRepository.Update(themeAppearance);
                    else
                        appearanceRepository.Insert(themeAppearance);

                    string appearancePath = Path.Combine(appearancesPath, themeAppearance.AppearanceKey);
                    //移动目录
                    if (Directory.Exists(appearancePath))
                        Directory.Delete(appearancePath, true);
                    parentDirectory.MoveTo(appearancePath);
                    if (Directory.Exists(tempPath))
                        Directory.Delete(tempPath, true);
                }
                else
                {
                    Directory.Delete(tempPath, true);
                    throw new ExceptionFacade("找不到Appearance.config文件,您上传的不是皮肤包");
                }
            }
        }

        /// <summary>
        /// 保存皮肤
        /// </summary>
        /// <param name="fileFullName">皮肤文件名</param>
        /// <param name="fileStream">皮肤文件流</param>
        private void SaveThemeAppearancePackage(string fileFullName, Stream fileStream)
        {
            using (FileStream outStream = File.OpenWrite(fileFullName))
            {
                byte[] buffer = new byte[fileStream.Length > 65536 ? 65536 : fileStream.Length];

                int readedSize;
                while ((readedSize = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, readedSize);
                }
                outStream.Flush();
                outStream.Close();
                fileStream.Close();
                fileStream.Dispose();
            }
        }

        /// <summary>
        /// 删除ThemeAppearance
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeAndAppearance">themeKey与appearanceKey用逗号关联</param>
        public void DeleteThemeAppearance(string presentAreaKey, string themeAndAppearance)
        {
            ThemeAppearance themeAppearance = GetThemeAppearance(presentAreaKey, themeAndAppearance);
            if (themeAppearance == null)
                return;
            PresentArea presentArea = new PresentAreaService().Get(presentAreaKey);
            if (presentArea == null)
                return;
            //默认皮肤不允许删除
            if (presentArea.DefaultAppearanceID == themeAppearance.Id)
                return;
            EventBus<ThemeAppearance>.Instance().OnBefore(themeAppearance, new CommonEventArgs(EventOperationType.Instance().Delete()));
            appearanceRepository.Delete(themeAppearance);


            EventBus<ThemeAppearance>.Instance().OnAfter(themeAppearance, new CommonEventArgs(EventOperationType.Instance().Delete()));
            var themeAppearances = GetThemeAppearances(presentAreaKey, null);
            //若主题下没有外观皮肤，则删除主题；
            if (themeAppearance.ThemeKey.ToLower() != "default" && themeAppearances.Count(n => n.ThemeKey.Equals(themeAppearance.ThemeKey, StringComparison.InvariantCultureIgnoreCase)) == 0)
            {
                themeRepository.DeleteByEntityId(string.Join(",", presentAreaKey, themeAppearance.ThemeKey));
                string themeLocation = WebUtility.GetPhysicalFilePath(string.Format("~/Themes/{0}/{1}", presentAreaKey, themeAppearance.ThemeKey));
                if (Directory.Exists(themeLocation))
                {
                    try
                    {
                        Directory.Delete(themeLocation, true);
                    }
                    catch { }
                }
            }
            else//仅将外观物理文件删除
            {
                string themeAppearanceLocation = WebUtility.GetPhysicalFilePath(string.Format("~/Themes/{0}/{1}/Appearances/{2}", presentAreaKey, themeAppearance.ThemeKey, themeAppearance.AppearanceKey));
                if (Directory.Exists(themeAppearanceLocation))
                {
                    try
                    {
                        Directory.Delete(themeAppearanceLocation, true);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// 设置启用禁用状态
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeAndAppearance">themeKey与appearanceKey用逗号关联</param>
        public void SetIsEnabled(string presentAreaKey, string themeAndAppearance, bool isEnabled)
        {
            ThemeAppearance themeAppearance = GetThemeAppearance(presentAreaKey, themeAndAppearance);
            if (themeAppearance == null)
                return;
            EventBus<ThemeAppearance>.Instance().OnBefore(themeAppearance, new CommonEventArgs(EventOperationType.Instance().Update()));
            themeAppearance.IsEnabled = isEnabled;
            appearanceRepository.Update(themeAppearance);
            EventBus<ThemeAppearance>.Instance().OnAfter(themeAppearance, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 调整外观皮肤使用次数
        /// </summary>
        /// <param name="presentAreaKey">呈现区域</param>
        /// <param name="oldThemeAndAppearance">旧外观皮肤Key（若是新增用户或群组，则传空）</param>
        /// <param name="newThemeAndAppearance">新外观皮肤Key（若是删除用户或群组，则传空）</param>
        public void ChangeThemeAppearanceUserCount(string presentAreaKey, string oldThemeAndAppearance, string newThemeAndAppearance)
        {
            if (!string.IsNullOrEmpty(oldThemeAndAppearance))
            {
                var oldAppearance = GetThemeAppearance(presentAreaKey, oldThemeAndAppearance);
                if (oldAppearance != null && oldAppearance.UserCount > 0)
                {
                    oldAppearance.UserCount--;
                    appearanceRepository.Update(oldAppearance);
                }
            }
            if (!string.IsNullOrEmpty(newThemeAndAppearance))
            {
                var newAppearance = GetThemeAppearance(presentAreaKey, newThemeAndAppearance);
                if (newAppearance != null)
                {
                    newAppearance.UserCount++;
                    appearanceRepository.Update(newAppearance);
                }
            }
        }

        /// <summary>
        /// 变更外观的排列顺序
        /// </summary>
        /// <param name="appearanceId">待调整的Id</param>
        /// <param name="referenceAppearanceId">参照Id</param>        
        public void ChangeDisplayOrder(string appearanceId, string referenceAppearanceId)
        {
            ThemeAppearance themeAppearance = appearanceRepository.Get(appearanceId);
            if (themeAppearance == null)
                return;
            ThemeAppearance referenceThemeAppearance = appearanceRepository.Get(referenceAppearanceId);
            if (referenceThemeAppearance == null)
                return;
            int tempDisplayOrder = themeAppearance.DisplayOrder;
            themeAppearance.DisplayOrder = referenceThemeAppearance.DisplayOrder;
            referenceThemeAppearance.DisplayOrder = tempDisplayOrder;
            appearanceRepository.Update(themeAppearance);
            appearanceRepository.Update(referenceThemeAppearance);
        }

        /// <summary>
        /// 获取ThemeAppearance
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeAndAppearance">themeKey与appearanceKey用逗号关联</param>
        /// <returns></returns>
        public ThemeAppearance GetThemeAppearance(string presentAreaKey, string themeAndAppearance)
        {
            string id = string.Join(",", presentAreaKey, themeAndAppearance);
            return appearanceRepository.Get(id);
        }

        /// <summary>
        /// 获取ThemeAppearance
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeKey">主题标识</param>
        /// <param name="appearanceKey">外观标识</param>
        /// <returns></returns>
        public ThemeAppearance GetThemeAppearance(string presentAreaKey, string themeKey, string appearanceKey)
        {
            string id = string.Join(",", presentAreaKey, themeKey, appearanceKey);
            return appearanceRepository.Get(id);
        }

        /// <summary>
        /// 获取呈现区域下的ThemeAppearance
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="isEnabled">ThemeAppearance是否启用</param>
        /// <returns></returns>
        public IEnumerable<ThemeAppearance> GetThemeAppearances(string presentAreaKey, bool? isEnabled = true)
        {
            IEnumerable<ThemeAppearance> allThemeAppearances = appearanceRepository.GetAll("DisplayOrder");

            List<ThemeAppearance> appearances = new List<ThemeAppearance>();
            foreach (var themeAppearance in allThemeAppearances)
            {
                if (themeAppearance.PresentAreaKey.Equals(presentAreaKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!isEnabled.HasValue || themeAppearance.IsEnabled == isEnabled.Value)
                        appearances.Add(themeAppearance);
                }
            }
            return appearances;
        }

        #endregion


        #region ThemeResolver

        private static ConcurrentDictionary<string, IThemeResolver> themeResolvers = new ConcurrentDictionary<string, IThemeResolver>();

        /// <summary>
        /// 注册ThemeResolver
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="themeResolver"><see cref="IThemeResolver"/></param>
        public static void RegisterThemeResolver(string presentAreaKey, IThemeResolver themeResolver)
        {
            if (string.IsNullOrEmpty(presentAreaKey))
                return;

            themeResolvers[presentAreaKey.ToLower()] = themeResolver;
        }

        /// <summary>
        /// 获取请求页面需要的Theme
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="controllerContext"><see cref="RequestContext"/></param>
        /// <returns></returns>
        public static ThemeAppearance GetRequestTheme(string presentAreaKey, RequestContext controllerContext)
        {
            if (string.IsNullOrEmpty(presentAreaKey))
                return null;

            IThemeResolver themeResolver = themeResolvers[presentAreaKey.ToLower()];
            if (themeResolver != null)
                return themeResolver.GetRequestTheme(controllerContext);
            else
                throw new ExceptionFacade(new ResourceExceptionDescriptor("{0}'s ThemeResolver not found!", presentAreaKey));
        }

        /// <summary>
        /// 加载皮肤的css
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="controllerContext"><see cref="RequestContext"/></param>
        public static void IncludeCss(string presentAreaKey, RequestContext controllerContext)
        {
            if (string.IsNullOrEmpty(presentAreaKey))
                return;

            IThemeResolver themeResolver = themeResolvers[presentAreaKey.ToLower()];
            if (themeResolver != null)
                themeResolver.IncludeCss(controllerContext);
            else
                throw new ExceptionFacade(new ResourceExceptionDescriptor("{0}'s ThemeResolver not found!", presentAreaKey));
        }

        /// <summary>
        /// 验证当前用户是否修改皮肤的权限
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <returns></returns>
        public static bool Validate(string presentAreaKey, long ownerId)
        {
            if (string.IsNullOrEmpty(presentAreaKey))
                return false;

            IThemeResolver themeResolver = themeResolvers[presentAreaKey.ToLower()];
            if (themeResolver != null)
                return themeResolver.Validate(ownerId);
            else
                throw new ExceptionFacade(new ResourceExceptionDescriptor("{0}'s ThemeResolver not found!", presentAreaKey));
        }

        /// <summary>
        /// 获取用户当前选中的皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <returns></returns>
        public static string GetThemeAppearance(string presentAreaKey, long ownerId)
        {
            if (string.IsNullOrEmpty(presentAreaKey))
                return null;

            IThemeResolver themeResolver = themeResolvers[presentAreaKey.ToLower()];
            if (themeResolver != null)
                return themeResolver.GetThemeAppearance(ownerId);
            else
                throw new ExceptionFacade(new ResourceExceptionDescriptor("{0}'s ThemeResolver not found!", presentAreaKey));
        }


        /// <summary>
        /// 更新皮肤
        /// </summary>
        /// <param name="presentAreaKey">呈现区域Key</param>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">themeKey与appearanceKey用逗号关联</param>
        public static void ChangeThemeAppearance(string presentAreaKey, long ownerId, bool isUseCustomStyle, string themeAppearance)
        {
            if (string.IsNullOrEmpty(presentAreaKey))
                return;

            IThemeResolver themeResolver = themeResolvers[presentAreaKey.ToLower()];
            if (themeResolver != null)
            {
                themeResolver.ChangeThemeAppearance(ownerId, isUseCustomStyle, themeAppearance);
            }
            else
                throw new ExceptionFacade(new ResourceExceptionDescriptor("{0}'s ThemeResolver not found!", presentAreaKey));
        }

        #endregion

    }
}
