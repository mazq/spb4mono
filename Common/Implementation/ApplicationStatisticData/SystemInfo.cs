using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using PetaPoco;
using Tunynet.Repositories;
using System.Text.RegularExpressions;
using Tunynet;
using System.Web.Management;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace Spacebuilder.Common
{
    public class SystemInfo
    {
        //操作系统名称
        private string pcName;
        public string PCName
        {
            get
            {
                //如果操作系统为空则获取一下
                if (string.IsNullOrEmpty(this.pcName))
                {
                    RegistryKey rk;
                    rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                    this.pcName = rk.GetValue("ProductName").ToString();
                }
                return this.pcName;
            }
        }

        private int osVersion;
        public int OSVersion
        {
            get
            {
                if (osVersion == 0)
                {
                    RegistryKey rk;
                    rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                    int.TryParse(rk.GetValue("CurrentBuildNumber").ToString(), out this.osVersion);
                }

                return this.osVersion;
            }
        }

        //IIS版本号
        private string iis;
        public string IIS
        {
            get
            {
                //如果IIS为空则获取
                if (string.IsNullOrEmpty(this.iis))
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp");
                    this.iis = "IIS" + key.GetValue("MajorVersion").ToString();
                }
                return this.iis;
            }
        }

        //framework版本号
        private string framework;
        public string Framework
        {
            get
            {
                //如果framework版本号为空则获取
                if (string.IsNullOrEmpty(this.framework))
                {
                    Version v = Environment.Version;
                    if (v != null)
                    {
                        this.framework = v.Major + "." + v.Minor;
                    }
                }
                return this.framework;
            }
        }

        //数据库类型
        private string dataBaseVersion;
        public string DataBaseVersion
        {

            get
            {
                //如果数据库类型为空则获取
                if (string.IsNullOrEmpty(dataBaseVersion))
                {
                    Sql sql = Sql.Builder.Select("@@@version");
                    var DataBase = PetaPocoDatabase.CreateInstance();
                    string DBversion = DataBase.ExecuteScalar<object>(sql).ToString();
                    if (!string.IsNullOrEmpty(DBversion))
                    {
                        Match match = Regex.Match(DBversion, @"^(?<DBversion>.*)-");
                        if (match.Success)
                        {
                            //获得有效字符串
                            this.dataBaseVersion = match.Groups["DBversion"].Value;
                        }
                    }
                }
                return this.dataBaseVersion;
            }
        }

        //.NET信任级别
        private string netTrustLevel;
        public string NetTrustLevel
        {

            get
            {
                TrustSection t = new TrustSection();
                this.netTrustLevel = t.Level;
                return this.netTrustLevel;
            }
        }

        //数据库占用（SQLServer存储过程）
        private string getDBSize;
        public string GetDBSize
        {

            get
            {
                    //Sql sql = Sql.Builder.Append("exec sp_spaceused");
                     //CreateDAO().First<object>(sql);
                     string DBSize;
                    //获得连接字符串
                    string connstr = ConfigurationManager.ConnectionStrings["SqlServer"].ToString();
                    //连接数据库
                    using (SqlConnection connection = new SqlConnection(connstr))
                    {
                        SqlCommand comm = new SqlCommand("sp_spaceused", connection);
                        SqlDataAdapter da = new SqlDataAdapter(comm);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        //获得数据库已占用空间大小
                        DBSize = ds.Tables[0].Rows[0][1].ToString();
                        //string size = ds.Tables[0].Rows[0][2].ToString();
                    }
                    this.getDBSize = DBSize;
                    return this.getDBSize;
            }
        }
    }
}
