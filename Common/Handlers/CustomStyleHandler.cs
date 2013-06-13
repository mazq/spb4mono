//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using RazorEngine;
using Tunynet.Utilities;
using Spacebuilder.UI;
using System.Xml.Linq;
using System.Web.Helpers;
using System.Dynamic;
using System.IO;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 解析自定义样式
    /// </summary>
    public class CustomStyleHandler : DownloadFileHandlerBase
    {

        //使用RazorEngine解析 \Themes\[PresentAreaKey]\CustomStyle\css-dark.xml 或 css-light.xml
        public override void ProcessRequest(HttpContext context)
        {
            string presentAreaKey = context.Request.QueryString.GetString("PresentAreaKey", string.Empty);
            long ownerId = context.Request.QueryString.Get<long>("OwnerId", 0);
            if (string.IsNullOrEmpty(presentAreaKey))
            {
                WebUtility.Return404(context);
            }
            CustomStyle customStyle = null;
            if (ownerId > 0)
            {
                CustomStyleEntity entity = new CustomStyleService().Get(presentAreaKey, ownerId);
                if (entity == null)
                    WebUtility.Return404(context);
                customStyle = entity.CustomStyle;
                //通过QueryString 传递PresentAreaKey、OwnerId 获取css （使用客户端缓存）
                DateTime lastModified = customStyle.LastModified.ToUniversalTime();
                if (IsCacheOK(context, lastModified))
                {
                    WebUtility.Return304(context);
                    return;
                }
                context.Response.Cache.SetCacheability(HttpCacheability.Private);
                context.Response.Cache.SetLastModified(lastModified);
                context.Response.Cache.SetETag(lastModified.Ticks.ToString());
                context.Response.Cache.SetAllowResponseInBrowserHistory(true);
                context.Response.Cache.SetValidUntilExpires(true);
            }
            else
            {
                string customStyleJson = context.Request.QueryString.GetString("CustomStyle", string.Empty);
                bool isDark = context.Request.QueryString.GetBool("isDark", false);
                Dictionary<string, string> dictionary = Json.Decode<Dictionary<string, string>>(customStyleJson);
                customStyle = CustomStyle.New();
                customStyle.IsDark = isDark;
                customStyle.DefinedColours = dictionary;

                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //通过QueryString 传递PresentAreaKey、CustomStyle的json数据 获取css（自定义风格时使用，禁用客户端缓存）
            }

            dynamic model = new ExpandoObject();
            if (customStyle.DefinedColours.ContainsKey(ColorLabel.PageBackground.ToString()))
                model.PageBackground = customStyle.DefinedColours[ColorLabel.PageBackground.ToString()];
            else
                model.PageBackground = "#f2e3bf";
            if (customStyle.DefinedColours.ContainsKey(ColorLabel.ContentBackground.ToString()))
                model.ContentBackground = customStyle.DefinedColours[ColorLabel.ContentBackground.ToString()];
            else
                model.ContentBackground = "#f8f0e6";
            if (customStyle.DefinedColours.ContainsKey(ColorLabel.BorderBackground.ToString()))
                model.BorderBackground = customStyle.DefinedColours[ColorLabel.BorderBackground.ToString()];
            else
                model.BorderBackground = "#ebe6d9";
            if (customStyle.DefinedColours.ContainsKey(ColorLabel.MainTextColor.ToString()))
                model.MainTextColor = customStyle.DefinedColours[ColorLabel.MainTextColor.ToString()];
            else
                model.MainTextColor = "#666";
            if (customStyle.DefinedColours.ContainsKey(ColorLabel.SubTextColor.ToString()))
                model.SubTextColor = customStyle.DefinedColours[ColorLabel.SubTextColor.ToString()];
            else
                model.SubTextColor = "#ccc";
            if (customStyle.DefinedColours.ContainsKey(ColorLabel.MainLinkColor.ToString()))
                model.MainLinkColor = customStyle.DefinedColours[ColorLabel.MainLinkColor.ToString()];
            else
                model.MainLinkColor = "#cc6673";
            if (customStyle.DefinedColours.ContainsKey(ColorLabel.SubLinkColor.ToString()))
                model.SubLinkColor = customStyle.DefinedColours[ColorLabel.SubLinkColor.ToString()];
            else
                model.SubLinkColor = "#efc0ca";

            string templateName = "css-dark";
            if (customStyle.IsDark)
                templateName = "css-light";

            
            
            XElement rootElement = XElement.Load(WebUtility.GetPhysicalFilePath(string.Format("~/Themes/{0}/Custom/css-{1}.xml", presentAreaKey, customStyle.IsDark ? "dark" : "light")));
            try
            {
                //编译模板
                string result = Razor.Parse(rootElement.Value, model, templateName);

                context.Response.Clear();
                context.Response.ContentType = "text/css";

                StreamWriter sw = new StreamWriter(context.Response.OutputStream, System.Text.Encoding.Default);
                sw.Write(result);
                sw.Flush();
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.ToString());
            }
        }
    }
}