//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-6-11</createdate>
//<author>bianchx</author>
//<email>bianchx@tunynet.com</email>
//<log date="2012-6-11" version="0.5">新建</log>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using MSNPSharp;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 获取MSN联系人的实现类
    /// </summary>
    public class MsnContactAccessor : IMsnContactAccessor
    {
        /// <summary>
        /// 获取Msn联系人
        /// </summary>
        /// <param name="userName">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="isSuccessLogin">是否成功登录</param>
        /// <returns>Key:联系人Email地址，Value：联系人名称</returns>
        public Dictionary<string, string> GetContacts(string userName, string password, out bool isSuccessLogin)
        {
            GetAddressStatus result;
            ContactList contactList;
            _status = GetAddressStatus.Success;
            notTimeout = false;
            result = GetMSNContactList(userName, password, out contactList);
            Dictionary<string, string> EmailAdders = new Dictionary<string, string>();
            foreach (KeyValuePair<int, Contact> tmp in contactList)
            {
                string Name = string.IsNullOrEmpty(tmp.Value.NickName) == true ? tmp.Value.Name : tmp.Value.NickName;
                string Email = tmp.Value.Mail;
                EmailAdders[Email] = Name;
            }
            switch (result)
            {
                case GetAddressStatus.Success:
                    isSuccessLogin = notTimeout;
                    break;
                default:
                    isSuccessLogin = false;
                    break;
            }
            return EmailAdders;
        }


        Messenger messenger = new Messenger();

        private int tmpCount = 1;

        bool notTimeout = false;

        AutoResetEvent are;//多线程同步信号

        #region MSN Login
        /// <summary>
        /// 读取MSN好友列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="IsPageAsync"></param>
        /// <returns></returns>
        private ContactList GetMSNContactList(string username, string password, bool IsPageAsync)
        {
            if (!messenger.Nameserver.IsSignedIn)
            {
                Login(username, password, IsPageAsync);
            }

            return messenger.Nameserver.ContactList;

        }

        /// <summary>
        /// 登陆MSN
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="IsPageAsync"></param>
        /// <returns></returns>
        private void Login(string username, string password, bool IsPageAsync)
        {
            if (IsPageAsync)
            {
                SynchronizationContext context = System.ComponentModel.AsyncOperationManager.SynchronizationContext;

                try
                {
                    System.ComponentModel.AsyncOperationManager.SynchronizationContext = new System.Threading.SynchronizationContext();

                    messenger.Credentials = new Credentials(username, password, (MsnProtocol)Enum.Parse(typeof(MsnProtocol), MsnProtocol.MSNP18.ToString()));

                    messenger.Connect();

                    Thread.Sleep(3000);           //线程休眠2秒

                    while (tmpCount < 6)          //重连5次
                    {
                        if (!messenger.Connected || !messenger.Nameserver.IsSignedIn)
                        {
                            tmpCount++;

                            this.Login(username, password, IsPageAsync);
                        }
                    }

                }
                finally
                {
                    System.ComponentModel.AsyncOperationManager.SynchronizationContext = context;
                }
            }

        }

        /// <summary>
        /// 读取MSN好友列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="IsPageAsync"></param>
        /// <returns></returns>
        private ContactList GetMSNContactList1(string username, string password, bool IsPageAsync)
        {
            if (!messenger.Nameserver.IsSignedIn)
            {
                Login(username, password, IsPageAsync);
            }

            return messenger.Nameserver.ContactList;
        }

        #endregion

        #region 异步登陆MSN
        //private System.Windows.Forms.VisualStyles.VisualStyleElement.Page _workingPage;

        private GetAddressStatus _status = GetAddressStatus.Success;



        //private GetAddressByMsn(System.Windows.Forms.VisualStyles.VisualStyleElement.Page workingPage)
        //{
        //    _workingPage = workingPage;
        //}


        private GetAddressStatus GetMSNContactList(string username, string password, out ContactList list)
        {
            if (!messenger.Nameserver.IsSignedIn)
            {
                Login(username, password);
            }

            list = messenger.Nameserver.ContactList;

            return _status;

        }

        /// <summary>
        /// 读取MSN好友列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private ContactList GetMSNContactList(string username, string password)
        {
            if (!messenger.Nameserver.IsSignedIn)
            {
                Login(username, password);
            }

            return messenger.Nameserver.ContactList;
        }
        /// <summary>
        /// 登陆MSN
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void Login(string username, string password)
        {
            are = new AutoResetEvent(false);

            SynchronizationContext context = AsyncOperationManager.SynchronizationContext;

            try
            {
                AsyncOperationManager.SynchronizationContext = new SynchronizationContext();

                messenger.Nameserver.SignedIn += new EventHandler<EventArgs>(Nameserver_SignedIn);

                messenger.NameserverProcessor.ConnectingException += new EventHandler<ExceptionEventArgs>(NameserverProcessor_ConnectingException);

                messenger.Nameserver.ExceptionOccurred += new EventHandler<ExceptionEventArgs>(Nameserver_ExceptionOccurred);

                messenger.Nameserver.AuthenticationError += new EventHandler<ExceptionEventArgs>(Nameserver_AuthenticationError);

                if (messenger.Connected)
                {
                    messenger.Disconnect();
                }

                messenger.Credentials = new Credentials(username, password, (MsnProtocol)Enum.Parse(typeof(MsnProtocol), MsnProtocol.MSNP18.ToString()));

                messenger.Connect();

                notTimeout = are.WaitOne(25000); //UI线程等待

            }
            finally
            {
                AsyncOperationManager.SynchronizationContext = context;
            }
        }

        private void Nameserver_SignedIn(object sender, EventArgs e)
        {
            are.Set();//通知UI线程可以继续干活了。。
            //登陆后的操作
            _status = GetAddressStatus.Success;
        }

        private void Nameserver_ExceptionOccurred(object sender, ExceptionEventArgs e)
        {
            are.Set();
            // ignore the unauthorized exception, since we're handling that error in another method.
            if (e.Exception is UnauthorizedException)
                return;
            //_workingPage.Page.Response.Write("MSN服务器连接异常");
            //Response.Write("MSN服务器连接异常");
        }

        private void NameserverProcessor_ConnectingException(object sender, ExceptionEventArgs e)
        {
            are.Set();
            //_workingPage.Page.Response.Write("MSN服务器连接异常");
            //System.Web.UI.WebControls.Label lblError = _workingPage.Page.FindControl("lbtError") as System.Web.UI.WebControls.Label;
            //lblError.Text = "MSN服务器连接异常";
            _status = GetAddressStatus.MailError;
        }

        private void Nameserver_AuthenticationError(object sender, ExceptionEventArgs e)
        {
            are.Set();
            _status = GetAddressStatus.UidOrPwdError;
            //_workingPage.Page.RegisterClientScriptBlock("alert", "<script>alert('您的MSN帐号或密码错误');</script>");
            //_workingPage.Page.Response.Write("您的MSN帐号或密码错误");
            //System.Web.UI.WebControls.Label lblError = _workingPage.Page.FindControl("lbtError") as System.Web.UI.WebControls.Label;
            //lblError.Text = "您的MSN帐号或密码错误";
        }

        #endregion

    }
}
