//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Spacebuilder.Common
{
    /// <summary>
    ///  农历日期时间
    /// </summary>
    public class ChinaDateTime
    {
        private int year, month, dayOfMonth;
        private bool isLeap;
        public DateTime time;

        /// <summary>
        /// 获取当前日期的农历年
        /// </summary>
        public int Year
        {
            get { return year; }
        }

        /// <summary>
        /// 获取当前日期的农历月份
        /// </summary>
        public int Month
        {
            get { return month; }
        }

        /// <summary>
        /// 获取当前日期的农历月中天数
        /// </summary>
        public int DayOfMonth
        {
            get { return dayOfMonth; }
        }

        /// <summary>
        /// 获取当前日期是否为闰月中的日期
        /// </summary>
        public bool IsLeap
        {
            get { return isLeap; }
        }

        ChineseLunisolarCalendar cc;
        /// <summary>
        /// 返回指定公历日期的阴历时间
        /// </summary>
        /// <param name="time"></param>
        public ChinaDateTime(DateTime time)
        {
            cc = new System.Globalization.ChineseLunisolarCalendar();
            if (time > cc.MaxSupportedDateTime || time < cc.MinSupportedDateTime)
                throw new Exception("参数日期时间不在支持的范围内,支持范围：" + cc.MinSupportedDateTime.ToShortDateString() + "到" + cc.MaxSupportedDateTime.ToShortDateString());
            year = cc.GetYear(time);
            month = cc.GetMonth(time);
            if (month > 12)
                month -= 12;
            dayOfMonth = cc.GetDayOfMonth(time);
            isLeap = cc.IsLeapMonth(year, month);
            if (isLeap) month -= 1;
            this.time = time;
        }

        /// <summary>
        /// 返回当前日前的农历日期。
        /// </summary>
        public static ChinaDateTime Now
        {
            get { return new ChinaDateTime(DateTime.Now); }
        }

        /// <summary>
        /// 返回指定农历年，月，日，是否为闰月的农历日期时间
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <param name="DayOfMonth"></param>
        /// <param name="IsLeap"></param>
        public ChinaDateTime(int Year, int Month, int DayOfMonth)
        {
            cc = new System.Globalization.ChineseLunisolarCalendar();
            if (Year >= cc.MaxSupportedDateTime.Year || Year <= cc.MinSupportedDateTime.Year)
                throw new Exception("参数年份时间不在支持的范围内,支持范围：" + cc.MinSupportedDateTime.ToShortDateString() + "到" + cc.MaxSupportedDateTime.ToShortDateString());

            if (Month < 1 || Month > 12)
                throw new Exception("月份必须在1~12范围");
            cc = new System.Globalization.ChineseLunisolarCalendar();

            if (cc.GetDaysInMonth(Year, cc.IsLeapMonth(Year, Month) ? Month + 1 : Month) < DayOfMonth || DayOfMonth < 1)
                throw new Exception("指定的月中的天数不在当前月天数有效范围");
            year = Year;
            month = Month;
            dayOfMonth = DayOfMonth;
            isLeap = IsLeap;
            time = DateTime.Now;
        }

        /// <summary>
        /// 获取当前农历日期的公历时间
        /// </summary>
        public DateTime ToDateTime()
        {
            return cc.ToDateTime(year, isLeap ? month + 1 : month, dayOfMonth, time.Hour, time.Minute, time.Second, time.Millisecond);
        }

        /// <summary>
        /// 获取指定农历时间对应的公历时间
        /// </summary>
        /// <param name="CnTime"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(ChinaDateTime CnTime)
        {
            return CnTime.ToDateTime();
        }

        /// <summary>
        /// 获取指定公历时间转换为农历时间
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static ChinaDateTime ToChinaDateTime(DateTime Time)
        {
            return new ChinaDateTime(Time);
        }

        /// <summary>
        /// 返回该农历日期的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "农历" + year.ToString() + "年" + (isLeap ? "润" : "") + month.ToString() + "月" + dayOfMonth.ToString() + "日";
        }
    }

    /// <summary>
    /// 农历日期帮助类
    /// </summary>
    public static class ChineseCalendarHelper
    {
        public static string GetYear(DateTime time)
        {
            StringBuilder sb = new StringBuilder();
            int year = calendar.GetYear(time);
            int d;
            do
            {
                d = year % 10;
                sb.Insert(0, ChineseNumber[d]);
                year = year / 10;
            } while (year > 0);
            return sb.ToString();
        }

        public static string GetMonth(DateTime time)
        {
            int month = calendar.GetMonth(time);
            int year = calendar.GetYear(time);
            int leap = 0;

            //正月不可能闰月
            for (int i = 3; i <= month; i++)
            {
                if (calendar.IsLeapMonth(year, i))
                {
                    leap = i;
                    break;  //一年中最多有一个闰月
                }

            }
            if (leap > 0) month--;
            return (leap == month + 1 ? "闰" : "") + ChineseMonthName[month - 1];
        }

        public static string GetDay(DateTime time)
        {
            return ChineseDayName[calendar.GetDayOfMonth(time) - 1];
        }

        public static string GetStemBranch(DateTime time)
        {
            int sexagenaryYear = calendar.GetSexagenaryYear(time);
            string stemBranch = CelestialStem.Substring((sexagenaryYear - 1) % 10, 1) + TerrestrialBranch.Substring((sexagenaryYear - 1) % 12, 1);
            return stemBranch;
        }

        private static ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();
        private static string ChineseNumber = "〇一二三四五六七八九";
        public const string CelestialStem = "甲乙丙丁戊己庚辛壬癸";
        public const string TerrestrialBranch = "子丑寅卯辰巳午未申酉戌亥";
        public static readonly string[] ChineseDayName = new string[] {
            "初一","初二","初三","初四","初五","初六","初七","初八","初九","初十",
            "十一","十二","十三","十四","十五","十六","十七","十八","十九","二十",
            "廿一","廿二","廿三","廿四","廿五","廿六","廿七","廿八","廿九","三十"};
        public static readonly string[] ChineseMonthName = new string[] { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二" };
    }
}
