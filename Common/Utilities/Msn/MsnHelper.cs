//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Threading;
using System.Web.UI;
using MSNPSharp;

namespace Spacebuilder.Common
{
    /// <summary>
    /// Msn邮箱联系人管理器
    /// </summary>
    public class MsnHelper
    {
        Messenger messenger = new Messenger();

        private int tmpCount = 1;

        AutoResetEvent are;//多线程同步信号

        #region MSN Login
        /// <summary>
        /// 读取MSN好友列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="IsPageAsync"></param>
        /// <returns></returns>
        public ContactList GetMSNContactList(string username, string password, bool IsPageAsync)
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
        public ContactList GetMSNContactList1(string username, string password, bool IsPageAsync)
        {
            if (!messenger.Nameserver.IsSignedIn)
            {
                Login(username, password, IsPageAsync);
            }

            return messenger.Nameserver.ContactList;
        }

        #endregion

        #region 异步登陆MSN
        private Page _workingPage;

        private GetAddressStatus _status = GetAddressStatus.Success;



        public MsnHelper(Page workingPage)
        {
            _workingPage = workingPage;
        }


        public GetAddressStatus GetMSNContactList(string username, string password, out ContactList list)
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
        public ContactList GetMSNContactList(string username, string password)
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

                are.WaitOne(); //UI线程等待

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
    /// <summary>
    ///20110807新增、返回状态结果
    /// </summary>
    public enum GetAddressStatus
    {
        Success, //成功读取
        UidOrPwdError,//用户名或密码错误
        NoAddress, //通讯录中无内容
        NetError, //(网络延时或读取出错)
        MailError,//邮箱格式错误
        NotAuspice//不支持
    }
}