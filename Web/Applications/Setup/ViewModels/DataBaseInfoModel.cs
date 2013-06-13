using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 创建地区的类
    /// </summary>
    [Serializable]
    public class DataBaseInfoModel
    {

        private string server = ".";
        [Display(Name = "数据库服务器")]
        [Required(ErrorMessage = "请输入数据库服务器")]
        public string Server
        {
            get
            {
                return server;
            }
            set
            {
                server = value;
            }
        }

        [Display(Name = "实例名")]
        public string Instance { get; set; }

        [Display(Name = "端口号")]
        public string Port { get; set; }


        private string database = "Spacebuilder";
        [Display(Name = "数据库名称")]
        [Required(ErrorMessage = "请输入数据库名称")]
        public string DataBase
        {
            get
            {
                return database;
            }
            set
            {
                database = value;
            }
        }

        private string databaseUserName = "sa";
        [Display(Name = "数据库用户帐号")]
        [Required(ErrorMessage = "请输入数据库用户帐号")]
        public string DataBaseUserName
        {
            get
            {
                return databaseUserName;
            }
            set
            {
                databaseUserName = value;
            }
        }

        [Display(Name = "数据库用户密码")]
        [Required(ErrorMessage = "请输入数据库用户密码")]
        public string DataBasePassword { get; set; }


        private string administrator = "admin";
        [Display(Name = "管理员帐号")]
        [Required(ErrorMessage = "请输入管理员帐号")]
        public string Administrator
        {
            get
            {
                return administrator;
            }
            set
            {
                administrator = value;
            }
        }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "请输入密码")]
        public string UserPassword { get; set; }

        [Display(Name = "站点域名")]
        [Required(ErrorMessage = "请输入站点域名")]
        public string MainRootSiteUrl { get; set; }
    }


}
