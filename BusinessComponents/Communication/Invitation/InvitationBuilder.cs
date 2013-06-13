//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Tunynet.Caching;
using System.Web;
using System.IO;
using Tunynet.Utilities;
using System.Xml;
using System.Dynamic;
using RazorEngine;
using Tunynet.Globalization;

namespace Tunynet.Common
{
    /// <summary>
    /// 请求构建器
    /// </summary>
    public class InvitationBuilder
    {

        #region Instance

        private InvitationBuilder()
        { }

        private static volatile InvitationBuilder _defaultInstance = null;
        private static readonly object lockObject = new object();
        private static bool isInitialized;
        private static Dictionary<string, InvitationTemplate> InvitationTemplates = null;
        /// <summary>
        /// 获取InvitationBuilder实例
        /// </summary>
        /// <returns></returns>
        public static InvitationBuilder Instance()
        {
            if (_defaultInstance == null)
            {
                lock (lockObject)
                {
                    if (_defaultInstance == null)
                    {
                        _defaultInstance = new InvitationBuilder();
                        Initialize();
                    }
                }
            }
            return _defaultInstance;
        }

        #endregion

        /// <summary>
        /// 加载所有请求模板，并预编译
        /// </summary>
        /// <remarks>在Starter中调用</remarks>
        public static void Initialize()
        {
            if (!isInitialized)
            {
                lock (lockObject)
                {
                    if (!isInitialized)
                    {
                        //从 \Languages\zh-CN\InvitationTemplates.xml 及  \Applications\[ApplicationKey]\Languages\zh-CN\Invitations\InvitationTemplates.xml 加载请求模板 
                        InvitationTemplates = LoadInvitationTemplates();
                        isInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// 生成请求内容
        /// </summary>
        /// <param name="Invitation">请求实体</param>
        /// <param name="status">请求状态</param>
        /// <returns>返回生成的请求内容</returns>
        public string Resolve(dynamic Invitation, InvitationStatus status)
        {
            if (Invitation == null)
                return string.Empty;

            if (!InvitationTemplates.ContainsKey(Invitation.InvitationTypeKey))
                throw new ExceptionFacade(new ResourceExceptionDescriptor().WithContentNotFound("请求模板", Invitation.InvitationTypeKey));

            InvitationTemplate invitationTemplate = InvitationTemplates[Invitation.InvitationTypeKey];
            if (invitationTemplate == null)
                return string.Empty;
            string razorTemplate = string.Empty;
            switch (status)
            {
                case InvitationStatus.Unhandled:
                    razorTemplate = invitationTemplate.UnhandledBody;
                    break;
                case InvitationStatus.Accept:
                    razorTemplate = invitationTemplate.AcceptBody;
                    break;
                case InvitationStatus.Refuse:
                    razorTemplate = invitationTemplate.RefuseBody;
                    break;
                default:
                    razorTemplate = invitationTemplate.UnhandledBody;
                    break;
            }
            if (string.IsNullOrEmpty(razorTemplate))
                return string.Empty;
            //使用RazorEngine解析请求内容
            try
            {
                return Razor.Parse(razorTemplate, Invitation, Invitation.InvitationTypeKey);
            }
            catch (Exception e)
            {
                throw new ExceptionFacade(new CommonExceptionDescriptor("编译请求模板时报错"), e);
            }
        }

        /// <summary>
        /// 加载Invitation模板
        /// </summary>
        private static Dictionary<string, InvitationTemplate> LoadInvitationTemplates()
        {
            Dictionary<string, InvitationTemplate> InvitationTemplates;
            
            //回复：已修改
            //mazq回复：不应该用DefaultLanguage吧
            //回复：那先不修改了

            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();
            string language = siteSettings.DefaultLanguage;

            string cacheKey = "InvitationTemplates::" + language;
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();

            InvitationTemplates = cacheService.Get<Dictionary<string, InvitationTemplate>>(cacheKey);

            if (InvitationTemplates == null)
            {
                InvitationTemplates = new Dictionary<string, InvitationTemplate>();

                // Read in the file

                List<string> fileNames = new List<string>();
                //平台级请求模板
                string commonFileName = WebUtility.GetPhysicalFilePath(string.Format("~/Languages/" + language + "/InvitationTemplates.xml"));
                if (File.Exists(commonFileName))
                    fileNames.Add(commonFileName);
                //应用级请求模板
                string applicationsRootDirectory = WebUtility.GetPhysicalFilePath("~/Applications/");
                foreach (var applicationPath in Directory.GetDirectories(applicationsRootDirectory))
                {
                    string applicationInvitationTemplateFileName = Path.Combine(applicationPath, "Languages\\" + language + "\\InvitationTemplates.xml");
                    if (!File.Exists(applicationInvitationTemplateFileName))
                        continue;
                    fileNames.Add(applicationInvitationTemplateFileName);
                }

                dynamic dModel = new ExpandoObject();

                Type modelType = ((object)dModel).GetType();

                foreach (string fileName in fileNames)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);

                    string typeKey;
                    foreach (XmlNode node in doc.GetElementsByTagName("invitation"))
                    {
                        XmlNode attrNode = node.Attributes.GetNamedItem("invitationTypeKey");
                        if (attrNode == null)
                            continue;
                        typeKey = attrNode.InnerText;
                        InvitationTemplate invitationTemplate = new InvitationTemplate(node);
                        InvitationTemplates[typeKey] = invitationTemplate;

                        //编译模板
                        Razor.Compile(invitationTemplate.UnhandledBody, modelType, typeKey);
                    }
                }
                cacheService.Add(cacheKey, InvitationTemplates, CachingExpirationType.Stable);
            }

            return InvitationTemplates;
        }

    }
}