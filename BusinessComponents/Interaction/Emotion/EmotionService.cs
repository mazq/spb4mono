//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Tunynet.Utilities;
using Tunynet.Repositories;

namespace Tunynet.Common
{

    /// <summary>
    /// 表情业务逻辑类
    /// </summary>
    public class EmotionService
    {
        private EmotionSettings _emotionSettings;
        private string _emoticonConfigName = "Emotions.xml";
        private IRepository<EmotionCategory> emotionRepository;
        private List<EmotionCategory> _categories = null;        //表情分类集合

        /// <summary>
        /// 表情管理构造器
        /// </summary>
        public EmotionService()
        {
            emotionRepository = new Repository<EmotionCategory>();

            //获取表情相关配置
            IEmotionSettingsManager settingsManger = DIContainer.Resolve<IEmotionSettingsManager>();
            _emotionSettings = settingsManger.Get();

            //读取目录下的表情包
            _categories = new List<EmotionCategory>();
            DirectoryInfo dir = new DirectoryInfo(WebUtility.GetPhysicalFilePath(_emotionSettings.EmoticonPath));
            foreach (var info in dir.GetDirectories())
            {
                PopulateEmotionCategory(info.Name, info.FullName);
            }
        }

        #region Delete/Update


        /// <summary>
        /// 删除表情分组
        /// </summary>
        /// <param name="directoryName">表情分类目录名</param>
        public void DeleteEmoticonCategory(string directoryName)
        {

            if (string.IsNullOrEmpty(directoryName))
                return;

            EmotionCategory category = GetEmotionCategory(directoryName);

            if (category == null)
                return;

            if (Directory.Exists(category.PhysicalDirectoryPath))
            {
                DirectoryInfo dir = new DirectoryInfo(category.PhysicalDirectoryPath);

                //将文件夹内的内容设置为可写
                foreach (var fp in dir.GetFiles())
                {
                    File.SetAttributes(fp.FullName, System.IO.FileAttributes.Normal);
                }

                Directory.Delete(category.PhysicalDirectoryPath, true);

                _categories.RemoveAll(n => n.DirectoryName == category.DirectoryName);

                emotionRepository.DeleteByEntityId(category.DirectoryName);
            }
        }

        /// <summary>
        /// 更新表情分类
        /// </summary>
        /// <param name="emotionCategory">表情分类</param>
        public void UpdateEmotionCategory(EmotionCategory emotionCategory)
        {
            emotionRepository.Update(emotionCategory);

            //加载表情
            if (emotionCategory.IsEnabled && (emotionCategory.Emotions == null || emotionCategory.Emotions.Count() == 0))
                LoadEmoticons(emotionCategory);
        }

        #endregion

        #region Get/Gets

        /// <summary>
        /// 根据CategoryID获取EmotionCategory
        /// </summary>
        /// <param name="directoryName">表情分类目录名</param>
        public EmotionCategory GetEmotionCategory(string directoryName)
        {
            if (_categories != null)
            {
                EmotionCategory category = _categories.FirstOrDefault(n => n.DirectoryName == directoryName);
                if (category != null && category.Emotions.Count == 0)
                    LoadEmoticons(category);
                return category;
            }
            return null;
        }

        /// <summary>
        /// 获取所有表情分组
        /// </summary>
        /// <param name="isEnabled">是否只获取启用的标签</param>
        public IList<EmotionCategory> GetEmotionCategories(bool isEnabled = false)
        {
            if (_categories != null)
                return _categories.Where(n => !isEnabled || n.IsEnabled).OrderBy(n => n.DisplayOrder).ToList();

            return _categories;
        }

        #endregion

        #region EmoticonPackageProcess

        /// <summary>
        /// 提取表情
        /// </summary>
        /// <param name="fileName">表情包文件名</param>
        /// <param name="fileStream">表情包文件流</param>
        public void ExtractEmoticon(string fileName, Stream fileStream)
        {
            if (fileStream == null || !fileStream.CanRead)
                return;

            string directoryName = fileName.Substring(0, fileName.LastIndexOf("."));
            string fullPath = _emotionSettings.EnableDirectlyUrl ? _emotionSettings.DirectlyRootUrl : WebUtility.GetPhysicalFilePath(_emotionSettings.EmoticonPath) + "\\" + directoryName + "\\";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            //将表情包存储到指定目录
            SaveEmoticonPackage(fileName, fullPath, fileStream);

            #region 解压压缩包

            Ionic.Zip.ReadOptions ro = new Ionic.Zip.ReadOptions();
            ro.Encoding = System.Text.Encoding.Default;

            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(fullPath + fileName, ro))
            {
                foreach (Ionic.Zip.ZipEntry zipEntry in zip)
                {
                    zipEntry.Extract(fullPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                }
            }

            #endregion

            //组装表情及分类的实体
            PopulateEmotionCategory(directoryName, fullPath);
        }

        /// <summary>
        /// 加载表情分类下的表情
        /// </summary>
        /// <param name="emotionCategory">表情分类</param>
        public void LoadEmoticons(EmotionCategory emotionCategory)
        {
            if (emotionCategory == null)
                return;

            XElement document = XElement.Load(emotionCategory.PhysicalDirectoryPath + "\\" + _emoticonConfigName);
            if (document != null)
            {
                //为启用的分类加载表情
                PopulateEmotion(emotionCategory, document);
            }
        }

        /// <summary>
        /// 保存表情包
        /// </summary>
        /// <param name="fileName">表情包文件名</param>
        /// <param name="fullPath">文件存储路径</param>
        /// <param name="fileStream">表情包文件流</param>
        private void SaveEmoticonPackage(string fileName, string fullPath, Stream fileStream)
        {
            using (FileStream outStream = File.OpenWrite(fullPath + fileName))
            {
                byte[] buffer = new byte[fileStream.Length > 65536 ? 65536 : fileStream.Length];

                int readedSize;
                while ((readedSize = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, readedSize);
                }

                outStream.Flush();
                outStream.Close();
            }
        }

        /// <summary>
        /// 组装表情分类实体
        /// </summary>
        /// <param name="directoryName">目录名</param>
        /// <param name="fullPath">表情包目录</param>
        private void PopulateEmotionCategory(string directoryName, string fullPath)
        {
            if (string.IsNullOrEmpty(directoryName))
                return;
            string configFullName = fullPath + "\\" + _emoticonConfigName;
            if (!File.Exists(configFullName))
            {
                Directory.Delete(fullPath, true);
                throw new ExceptionFacade("找不到文件：" + configFullName + ",您上传的不是表情包");
            }
            XElement document = XElement.Load(configFullName);
            if (document == null)
            {
                Directory.Delete(fullPath, true);
                return;
            }
            EmotionCategory category;

            if (!emotionRepository.Exists(directoryName))
            {
                category = EmotionCategory.New(directoryName);
                int maxDisplayOrder = emotionRepository.GetAll().Max(n => n.DisplayOrder);
                category.DisplayOrder = maxDisplayOrder + 1;
                emotionRepository.Insert(category);
            }
            else
            {
                category = emotionRepository.Get(directoryName);
            }

            category.InitPropertyValue(document, fullPath);

            if (category != null)
            {
                if (_categories.Where(n => n.DirectoryName == category.DirectoryName).Count() == 0)
                    _categories.Add(category);

                if (category.IsEnabled)
                {
                    PopulateEmotion(category, document);
                }
            }
        }

        /// <summary>
        /// 组装表情实体
        /// </summary>
        /// <param name="category">表情分类</param>
        /// <param name="categoryElement">表情分类配置节点</param>
        private void PopulateEmotion(EmotionCategory category, XElement categoryElement)
        {
            string emoticonPath = WebUtility.ResolveUrl(_emotionSettings.EmoticonPath);
            IEnumerable<XElement> emotionsElements = categoryElement.Elements();

            foreach (var emotionElement in emotionsElements)
            {
                Emotion emotion = Emotion.New(emotionElement);
                emotion.FormatedCode = string.Format("[{0}{1}]", !string.IsNullOrEmpty(category.CodePrefix) ? category.CodePrefix + ":" : string.Empty, emotion.Code);
                emotion.ImageUrl = string.Format("{0}/{1}/{2}", emoticonPath, category.DirectoryName, emotion.FileName);

                if (category.Emotions.Where(n => n.Code == emotion.Code).Count() == 0)
                    category.Emotions.Add(emotion);

                PrepareTransformsInfo(category);
            }
        }

        #endregion

        #region Emoticon & User Transforms

        private Dictionary<string, string> emoctionDictionary = new Dictionary<string, string>();
        private Char firstCharCheck = '[';
        private BitArray allCharCheck = new BitArray(char.MaxValue);
        private int maxWordLength = 0;
        private int minWordLength = int.MaxValue;

        /// <summary>
        /// 将所有表情写入集合中供下面的转换
        /// </summary>
        /// <param name="category">表情分类</param>
        private void PrepareTransformsInfo(EmotionCategory category)
        {
            if (category == null || category.Emotions == null)
            {
                return;
            }

            foreach (Emotion emotion in category.Emotions)
            {
                string smileyPattern = emotion.FormatedCode;
                string replacePattern = string.Format("<img src=\"{0}\" title=\"{1}\" alt=\"{1}\" />", emotion.ImageUrl, emotion.Description);

                maxWordLength = Math.Max(maxWordLength, smileyPattern.Length);
                minWordLength = Math.Min(minWordLength, smileyPattern.Length);

                foreach (char c in smileyPattern)
                {
                    allCharCheck[c] = true;
                }
                emoctionDictionary[smileyPattern] = replacePattern;
            }
        }

        /// <summary>
        /// 以含表情图片的html代码替换字符串formattedPost中的SmileyCode
        /// </summary>
        /// <param name="formattedPost">被替换的字符串</param>
        public string EmoticonTransforms(string formattedPost)
        {
            List<string> needReplacedSub = new List<string>();
            int index = 0;
            while (index < formattedPost.Length)
            {
                if (formattedPost[index] != firstCharCheck)
                {
                    index++;
                    continue;
                }

                for (int j = 1; j <= Math.Min(maxWordLength, formattedPost.Length - index); j++)
                {
                    if (allCharCheck[formattedPost[index + j - 1]] == false)
                    {
                        continue;
                    }

                    string sub = formattedPost.Substring(index, j);

                    if (emoctionDictionary.ContainsKey(sub))
                    {
                        needReplacedSub.Add(sub);
                    }
                }

                index++;
            }

            for (int i = 0; i < needReplacedSub.Count; i++)
            {
                formattedPost = formattedPost.Replace(needReplacedSub[i], emoctionDictionary[needReplacedSub[i]]);
            }

            return formattedPost;
        }

        #endregion
    }
}