//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Tunynet.Utilities;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于数据格式化
    /// </summary>
    public class Formatter
    {

        static AreaService areaService = new AreaService();
        static CategoryService categoryService = new CategoryService();
        /// <summary>
        /// 格式化所在区域
        /// </summary>
        /// <param name="areaCode">areaCode</param>
        ///<param name="showAll">是否显示所有父级区域</param>
        public static string FormatArea(string areaCode, bool showAll = false)
        {
            if (showAll)
                return FormatArea(areaCode, 10);
            return FormatArea(areaCode, 2);
        }

        /// <summary>
        /// 格式化所在地区
        /// </summary>
        /// <param name="areaCode">当前地区的编码</param>
        /// <param name="showCount">显示条数</param>
        /// <param name="character">分隔符</param>
        /// <returns></returns>
        public static string FormatArea(string areaCode, int showCount, string character = "-")
        {
            Area area = areaService.Get(areaCode);
            if (area == null)
                return string.Empty;
            List<Area> areas = new List<Area>();
            GetAllArea(area, ref areas);
            int startNum = areas.Count - showCount < 0 ? 0 : areas.Count - showCount;
            int stopNum = showCount + startNum > areas.Count ? areas.Count - startNum : showCount;
            return string.Join(character, areas.GetRange(startNum, stopNum).Select(n => n.Name));
        }

        /// <summary>
        /// 获取所有的父地区的集合（包含当前地区）
        /// </summary>
        /// <param name="area">当前地区</param>
        /// <param name="areas">所有的父地区</param>
        private static void GetAllArea(Area area, ref List<Area> areas)
        {
            if (string.IsNullOrEmpty(area.ParentCode) || area.AreaCode == "A1560000")
                return;
            areas.Insert(0, area);
            Area areaParent = areaService.Get(area.ParentCode);
            if (areaParent != null)
                GetAllArea(areaParent, ref areas);
        }

        #region 分类显示

        /// <summary>
        /// 格式化显示分类
        /// </summary>
        /// <param name="categoryId">分类Id</param>
        ///<param name="showAll">是否显示所有父级分类</param>
        /// <param name="character">分隔符</param>
        /// <returns></returns>
        public static string FormatCategory(long categoryId, bool showAll = true, string character = "-")
        {
            Category category = categoryService.Get(categoryId);
            if (category == null)
                return string.Empty;
            if (!showAll)
                return category.CategoryName;
            List<Category> categories = new List<Category>();
            GetAllParentCategory(category, ref categories);
            return string.Join(character, categories.Select(n => n.CategoryName));
        }        

        /// <summary>
        /// 获取所有父类
        /// </summary>
        /// <param name="categories">父分类列表</param>
        /// <param name="category">当前分类</param>
        /// <param name="includeSelf">是否包含自己</param>
        /// <param name="maxLength">最大个数（-1：不做限制,0:获取不到任何数据）（从本身一级开始往上数N级）</param>
        private static void GetAllParentCategory(Category category, ref List<Category> categories, bool includeSelf = true, int maxLength = -1)
        {
            if (category == null)
                return;

            if (maxLength == 0 || maxLength < -1)
                return;

            if (maxLength != -1)
                maxLength--;

            if (includeSelf)
                categories.Insert(0, category);

            if (category.Parent == null)
                return;

            GetAllParentCategory(category.Parent, ref categories, true, maxLength);
        }

        #endregion

        #region Transforms

        /// <summary>
        /// 多行纯文本型转化为可以在HTML中显示 
        /// </summary>
        /// <remarks>
        /// 一般在存储到数据库之前进行转化
        /// </remarks>
        /// <param name="plainText">需要转化的纯文本</param>
        /// <param name="keepWhiteSpace">是否保留空格</param>
        public static string FormatMultiLinePlainTextForStorage(string plainText, bool keepWhiteSpace)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (keepWhiteSpace)
            {
                plainText = plainText.Replace(" ", "&nbsp;");
                plainText = plainText.Replace("\t", "&nbsp;&nbsp;");
            }
            plainText = plainText.Replace("\r\n", WebUtility.HtmlNewLine);
            plainText = plainText.Replace("\n", WebUtility.HtmlNewLine);

            return plainText;
        }

        /// <summary>
        /// 多行纯文本型转化为可以在TextArea中正常显示 
        /// </summary>
        /// <remarks>
        /// 一般在进行编辑前进行转化
        /// </remarks>
        /// <param name="plainText">需要转化的纯文本</param>
        /// <param name="keepWhiteSpace">是否保留空格</param>
        public static string FormatMultiLinePlainTextForEdit(string plainText, bool keepWhiteSpace)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            string result = plainText;
            result = result.Replace(WebUtility.HtmlNewLine, "\n");
            if (keepWhiteSpace)
                result = result.Replace("&nbsp;", " ");

            return result;
        }

        /// <summary>
        /// 格式化纯文本评论
        /// </summary>
        /// <remarks>
        /// 进行以下操作：
        /// 1、敏感词过滤
        /// 2、所有链接增加nofollow属性
        /// 3、保留换行及空格的格式
        /// </remarks>
        /// <param name="text">格式化的内容</param>
        /// <param name="enableNoFollow">Should we include the nofollow rel.</param>
        /// <param name="enableConversionToParagraphs">Should newlines be converted to P tags.</param>
        public static string FormatPlainTextComment(string text)
        {
            return FormatPlainTextComment(text, true, true);
        }

        /// <summary>
        /// 格式化评论内容
        /// </summary>
        /// <param name="text">格式化的内容</param>
        /// <param name="enableNoFollow">Should we include the nofollow rel.</param>
        /// <param name="enableConversionToParagraphs">Should newlines be converted to P tags.</param>
        private static string FormatPlainTextComment(string text, bool enableNoFollow, bool enableConversionToParagraphs)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = WebUtility.HtmlEncode(text);

            if (enableNoFollow)
            {
                //Find any links
                StringCollection uniqueMatches = new StringCollection();

                string pattern = @"(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
                MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

                foreach (Match m in matches)
                {
                    if (!uniqueMatches.Contains(m.ToString()))
                    {
                        text = text.Replace(m.ToString(), "<a rel=\"nofollow\" target=\"_new\" href=\"" + m + "\">" + m.ToString().Trim() + "</a>");
                        uniqueMatches.Add(m.ToString());
                    }
                }
            }

            // Replace Line breaks with <br> and every other concurrent space with &nbsp; (to allow line breaking)
            if (enableConversionToParagraphs)
                text = ConvertPlainTextToParagraph(text);// text.Replace("\n", "<br />");

            text = text.Replace("  ", " &nbsp;");

            
            //if (SiteSettingsManager.GetSiteSettings().EnableCensorship)
            //{
            //    //过滤掉敏感词语
            //    text = Censors.CensorPost(text);
            //}

            return text;
        }

        /// <summary>
        /// 把纯文字格式化成html段落
        /// </summary>
        /// <remarks>
        /// 使文本在Html中保留换行的格式
        /// </remarks>
        private static string ConvertPlainTextToParagraph(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            //text = text.Replace("<p>", string.Empty);
            //text = text.Replace("</p>", "\n");

            text = text.Replace("\r\n", "\n").Replace("\r", "\n");

            string[] lines = text.Split('\n');

            StringBuilder paragraphs = new StringBuilder();

            foreach (string line in lines)
            {
                if (line != null && line.Trim().Length > 0)
                    paragraphs.AppendFormat("{0}<br />\n", line);
            }
            return paragraphs.ToString().Remove(paragraphs.ToString().LastIndexOf("<br />"));
        }

        #endregion

        #region Format File Size

        /// <summary>
        /// 友好的文件大小信息
        /// </summary>
        /// <param name="fileSize">文件字节数</param>
        public static string FormatFriendlyFileSize(double fileSize)
        {
            if (fileSize > 0)
            {
                if (fileSize > 1024 * 1024 * 1024)
                    return string.Format("{0:F2}G", (fileSize / (1024 * 1024 * 1024F)));
                else if (fileSize > 1024 * 1024)
                    return string.Format("{0:F2}M", (fileSize / (1024 * 1024F)));
                else if (fileSize > 1024)
                    return string.Format("{0:F2}K", (fileSize / (1024F)));
                else
                    return string.Format("{0:F2}Bytes", fileSize);
            }
            else
                return string.Empty;
        }

        #endregion

        #region Tag

        /// <summary>
        /// 清除标签名称中的非法字词
        /// </summary>
        public static string CleanTagName(string appKey)
        {
            //Remark:20090808_zhengw 删除Url中可编码的特殊字符：'#','&','=','/','%','?','+', '$',
            string[] parts = appKey.Split('!', '.', '@', '^', '*', '(', ')', '[', ']', '{', '}', '<', '>', ',', '\\', '\'', '~', '`', '|');
            appKey = string.Join("", parts);
            return appKey;
        }

        #endregion
    }
}
