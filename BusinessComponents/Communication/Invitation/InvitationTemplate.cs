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
using System.Xml;
using Tunynet.Utilities;

namespace Tunynet.Common
{
    /// <summary>
    /// 请求模板
    /// </summary>
    public sealed class InvitationTemplate
    {
        /// <summary>
        /// InvitationTemplate构造器
        /// </summary>
        /// <param name="rootNode">InvitationTemplate所属xml文档节点</param>
        public InvitationTemplate(XmlNode rootNode)
        {
            XmlNode attrNode = rootNode.Attributes.GetNamedItem("invitationTypeKey");
            if (attrNode != null)
                invitationTypeKey = attrNode.InnerText;

            XmlNode unhandledBodyNode = rootNode.SelectSingleNode("unhandledBody");
            if (unhandledBodyNode != null)
                this.unhandledBody = unhandledBodyNode.InnerXml;

            XmlNode acceptBodyNode = rootNode.SelectSingleNode("acceptBody");
            if (acceptBodyNode != null)
                this.acceptBody = acceptBodyNode.InnerXml;

            XmlNode refuseBodyNode = rootNode.SelectSingleNode("refuseBody");
            if (refuseBodyNode != null)
                this.refuseBody = refuseBodyNode.InnerXml;
        }

        #region 属性

        private string invitationTypeKey;
        /// <summary>
        /// 模板名称（在Invitation模板中必须保证唯一）
        /// </summary>
        public string InvitationTypeKey
        {
            get { return invitationTypeKey; }
            set { invitationTypeKey = value; }
        }

        private string unhandledBody = string.Empty;
        /// <summary>
        /// 未处理状态时的请求内容模板
        /// </summary>
        public string UnhandledBody
        {
            get { return unhandledBody; }
            set { unhandledBody = value; }
        }

        private string acceptBody;
        /// <summary>
        /// 接受状态时的请求内容模板
        /// </summary>
        public string AcceptBody
        {
            get { return acceptBody; }
            set { acceptBody = value; }
        }

        private string refuseBody;
        /// <summary>
        /// 拒绝状态时的请求内容模板
        /// </summary>
        public string RefuseBody
        {
            get { return refuseBody; }
            set { refuseBody = value; }
        }



        #endregion

    }
}
