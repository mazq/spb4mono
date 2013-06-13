//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Tunynet;
using Tunynet.Common;
using Tunynet.Utilities;


namespace Spacebuilder.Common
{
    /// <summary>
    /// 查询IP国家地区及其地区编码
    /// </summary>
    public class IPSeeker
    {
        #region 私有成员变量
        private byte[] data;
        private long firstStartIpOffset;
        private long ipCount;
        private static IPSeeker ipSeeker = null;
        private static object lockObject = new object();
        #endregion

        /// <summary>
        /// 生成IPSeeker实例
        /// </summary>
        /// <returns>IPSeeker实例</returns>
        public static IPSeeker Instance()
        {
            if (ipSeeker == null)
            {
                lock (lockObject)
                {
                    if (ipSeeker == null)
                    {

                        ipSeeker = new IPSeeker();
                    }
                }
            }
            return ipSeeker;
        }

        /// <summary>
        /// 单例模式生成对象
        /// </summary>
        private IPSeeker()
        {
            FileStream fs;
            long lastStartIpOffset;
            using (fs = new FileStream(WebUtility.GetPhysicalFilePath(WebUtility.ResolveUrl("~/Config/IP.dat")), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Dispose();
                fs.Close();
            }

            byte[] buffer = new byte[8];
            Array.Copy(data, 0, buffer, 0, 8);
            firstStartIpOffset = ((buffer[0] + (buffer[1] * 0x100)) + ((buffer[2] * 0x100) * 0x100)) + (((buffer[3] * 0x100) * 0x100) * 0x100);
            lastStartIpOffset = ((buffer[4] + (buffer[5] * 0x100)) + ((buffer[6] * 0x100) * 0x100)) + (((buffer[7] * 0x100) * 0x100) * 0x100);
            ipCount = Convert.ToInt64((double)(((double)(lastStartIpOffset - firstStartIpOffset)) / 7.0));

            if (ipCount < 1L)
            {
                throw new ExceptionFacade("ip FileDataError");
            }
        }


        /// <summary>
        /// 获取IP地址对应的国家与地区
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>IP地址对应的国家地区</returns>
        public string GetAddress(string ip)
        {
            //本机测试时，调用WebUtility.GetIP()方法，获取到的IP为“::1”，非有效IP地址，为了避免这个问题加入了以下判断；by zhengw
            if (ip == "::1")
                ip = "127.0.0.1";
            if (ip.Contains(":"))
                ip = ip.Substring(0, ip.LastIndexOf(":"));
            string country;
            string local;
            Regex regex = new Regex(@"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))");
            Match match = regex.Match(ip);
            if (match != null && !match.Success)
            {
                throw new ArgumentException("IP格式错误");
            }
            long intIP = IpToInt(ip);
            if ((intIP >= IpToInt("127.0.0.1") && (intIP <= IpToInt("127.255.255.255"))))
            {
                country = "本机内部环回地址";
                local = "";
            }
            else
            {
                if ((((intIP >= IpToInt("0.0.0.0")) && (intIP <= IpToInt("2.255.255.255"))) || ((intIP >= IpToInt("64.0.0.0")) && (intIP <= IpToInt("126.255.255.255")))) ||
                ((intIP >= IpToInt("58.0.0.0")) && (intIP <= IpToInt("60.255.255.255"))))
                {
                    country = "网络保留地址";
                    local = "";
                }
            }
            long right = ipCount;
            long left = 0L;
            long middle = 0L;
            long startIp = 0L;
            long endIpOff = 0L;
            long endIp = 0L;
            int countryFlag = 0;
            while (left < (right - 1L))
            {
                middle = (right + left) / 2L;
                startIp = GetStartIp(middle, out endIpOff);
                if (intIP == startIp)
                {
                    left = middle;
                    break;
                }
                if (intIP > startIp)
                {
                    left = middle;
                }
                else
                {
                    right = middle;
                }
            }
            startIp = GetStartIp(left, out endIpOff);
            endIp = GetEndIp(endIpOff, out countryFlag);

            if ((startIp <= intIP) && (endIp >= intIP))
            {

                string outLocal;
                country = GetCountry(endIpOff, countryFlag, out outLocal);
                local = outLocal;
            }
            else
            {
                country = "未知";
                local = "";
            }

            return country + " " + local;
        }

        /// <summary>
        /// 获取所查询地区的编码
        /// </summary>
        /// <param name="ip">要查找的IP地址</param>
        /// <returns>地区编码</returns>
        public string GetAreaCode(string ip)
        {
            string ipAddress = GetAddress(ip);
            if (string.IsNullOrEmpty(ipAddress))
                return string.Empty;
            string ipCountry = ipAddress.Substring(0, ipAddress.LastIndexOf(' '));
            AreaService areaService = new AreaService();

            if (string.IsNullOrEmpty(ipCountry))
                return string.Empty;

            //在中国的省（直辖市）地区查找
            Area china = areaService.Get("A1560000");
            if (china == null)
                return string.Empty;

            foreach (var area in china.Children)
            {
                if (ipCountry.Contains(area.Name.Substring(0, 2)))
                {
                    foreach (var city in area.Children)
                    {
                        if (ipCountry.Contains(city.Name))
                        {
                            foreach (var district in city.Children)
                            {
                                if (ipCountry.Contains(city.Name))
                                {
                                    return city.AreaCode;
                                }
                            }
                            return city.AreaCode;
                        }
                    }
                    return area.AreaCode;
                }
            }

            //先从国家地区中查找
            IEnumerable<Area> areas = areaService.GetRoots();
            foreach (var area in areas)
            {
                if (ipCountry.Contains(area.Name))
                    return area.AreaCode;
            }
            return string.Empty;
        }


        /// <summary>
        /// 将字符串IP转换为长整型
        /// </summary>
        /// <param name="ip">字符串IP</param>
        /// <returns>长整型IP</returns>
        private static long IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            if (ip.Split(separator).Length == 3)
            {
                ip = ip + ".0";
            }
            string[] strArray = ip.Split(separator);
            long num2 = ((long.Parse(strArray[0]) * 0x100L) * 0x100L) * 0x100L;
            long num3 = (long.Parse(strArray[1]) * 0x100L) * 0x100L;
            long num4 = long.Parse(strArray[2]) * 0x100L;
            long num5 = long.Parse(strArray[3]);
            return (((num2 + num3) + num4) + num5);
        }
        /// <summary>
        /// 把长整形IP转换为字符串类型
        /// </summary>
        /// <param name="ip_Int">长整型IP</param>
        /// <returns>字符串IP</returns>
        private static string IntToIP(long ip_Int)
        {
            long num = (long)((ip_Int & 0xff000000L) >> 0x18);
            if (num < 0L)
            {
                num += 0x100L;
            }
            long num2 = (ip_Int & 0xff0000L) >> 0x10;
            if (num2 < 0L)
            {
                num2 += 0x100L;
            }
            long num3 = (ip_Int & 0xff00L) >> 8;
            if (num3 < 0L)
            {
                num3 += 0x100L;
            }
            long num4 = ip_Int & 0xffL;
            if (num4 < 0L)
            {
                num4 += 0x100L;
            }
            return (num.ToString() + "." + num2.ToString() + "." + num3.ToString() + "." + num4.ToString());
        }


        /// <summary>
        /// 获取StartIp
        /// </summary>
        /// <returns>StartIp</returns>
        private long GetStartIp(long left, out long endIpOff)
        {
            long leftOffset = firstStartIpOffset + (left * 7L);
            byte[] buffer = new byte[7];
            Array.Copy(data, leftOffset, buffer, 0, 7);
            endIpOff = (Convert.ToInt64(buffer[4].ToString()) + (Convert.ToInt64(buffer[5].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[6].ToString()) * 0x100L) * 0x100L);
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }
        /// <summary>
        /// 获取EndIp
        /// </summary>
        /// <returns>EndIp</returns>
        private long GetEndIp(long endIpOff, out int countryFlag)
        {
            byte[] buffer = new byte[5];
            Array.Copy(data, endIpOff, buffer, 0, 5);
            countryFlag = buffer[4];
            return ((Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }

        /// <summary>
        /// 获取国家
        /// </summary>
        /// <returns>国家</returns>
        private string GetCountry(long endIpOff, int countryFlag, out string local)
        {
            string country;
            long offset = endIpOff + 4L;
            switch (countryFlag)
            {
                case 1:
                case 2:

                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);

                    offset = endIpOff + 8L;
                    local = (1 == countryFlag) ? "" : GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
                default:
                    country = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    local = GetFlagStr(ref offset, ref countryFlag, ref endIpOff);
                    break;
            }

            return country;
        }
        private string GetFlagStr(ref long offset, ref int countryFlag, ref long endIpOff)
        {
            int flag = 0;
            byte[] buffer = new byte[3];

            while (true)
            {
                //用于向前累加偏移量
                long forwardOffset = offset;
                flag = data[forwardOffset++];
                //没有重定向
                if (flag != 1 && flag != 2)
                {
                    break;
                }
                Array.Copy(data, forwardOffset, buffer, 0, 3);
                forwardOffset += 3;
                if (flag == 2)
                {
                    countryFlag = 2;
                    endIpOff = offset - 4L;
                }
                offset = (Convert.ToInt64(buffer[0].ToString()) + (Convert.ToInt64(buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(buffer[2].ToString()) * 0x100L) * 0x100L);
            }
            if (offset < 12L)
            {
                return "";
            }

            return GetStr(ref offset);
        }


        private string GetStr(ref long offset)
        {
            byte lowByte = 0;
            byte highByte = 0;
            StringBuilder stringBuilder = new StringBuilder();
            byte[] bytes = new byte[2];
            Encoding encoding = Encoding.GetEncoding("GB2312");
            while (true)
            {
                lowByte = data[offset++];
                if (lowByte == 0)
                {
                    return stringBuilder.ToString();
                }
                if (lowByte > 0x7f)
                {
                    highByte = data[offset++];
                    bytes[0] = lowByte;
                    bytes[1] = highByte;
                    if (highByte == 0)
                    {
                        return stringBuilder.ToString();
                    }
                    stringBuilder.Append(encoding.GetString(bytes));
                }
                else
                {
                    stringBuilder.Append((char)lowByte);
                }
            }
        }





    }
}
