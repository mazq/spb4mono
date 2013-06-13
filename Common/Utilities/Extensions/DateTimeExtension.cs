//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Globalization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// DateTime扩展方法
    /// </summary>
    public static class DateTimeExtension
    {

        /// <summary>
        /// 转换成用户所在时区的时间，并按用户设置返回对应的格式化字符串
        /// </summary>
        /// <param name="dateTime">原日期(UTC时间)</param>
        /// <returns>返回用户所在时区时间,并按用户设置返回对应的格式化字符串</returns>
        public static string ToUserDateString(this DateTime dateTime)
        {
            return ToUserDateString(dateTime, false);
        }

        /// <summary>
        /// 转换成用户所在时区的时间，并按用户设置返回对应的格式化字符串
        /// </summary>
        /// <param name="dateTime">原日期(UTC时间)</param>
        /// <param name="displayTime">是否显示时间</param>
        /// <returns>返回用户所在时区时间,并按用户设置返回对应的格式化字符串</returns>
        public static string ToUserDateString(this DateTime dateTime, bool displayTime)
        {
            if (dateTime == DateTime.MinValue)
                return ResourceAccessor.GetString("Common_DatetimeWhenZero");

            DateTime userDate = ConvertToUserDate(dateTime);

            string dateFormat = ResourceAccessor.GetString("Common_DefaultDateFormat");
            if (displayTime)
                return userDate.ToString(dateFormat + " " + ResourceAccessor.GetString("Common_DefaultTimeFormat"));
            else
                return userDate.ToString(dateFormat);
        }

        /// <summary>
        /// 转换成用户所在时区的时间，并返回站点设置的日期格式
        /// </summary>
        /// <param name="dateTime">原日期(UTC时间)</param>
        /// <returns>
        /// <para>返回友好的时间描述，例如:</para>
        /// <list type="bullet">
        /// <item>2分钟以前</item>
        /// <item>昨天 8:39</item>
        /// <item>......</item>
        /// </list>
        /// </returns>
        public static string ToFriendlyDate(this DateTime dateTime, bool showTime = true, string dateFormat = "")
        {
            if (dateTime == DateTime.MinValue)
                return ResourceAccessor.GetString("Common_DatetimeWhenZero");

            if (string.IsNullOrEmpty(dateFormat))
                dateFormat = ResourceAccessor.GetString("Common_DefaultDateFormat");

            string timeFormat = ResourceAccessor.GetString("Common_DefaultTimeFormat");

            DateTime userDate = dateTime.ConvertToUserDate();
            DateTime userNow = DateTime.UtcNow.ConvertToUserDate();

            if (userDate > userNow)
            {
                return userDate.ToString(dateFormat + (showTime ? " " + timeFormat : ""));
            }

            TimeSpan intervalTime = userNow - userDate;

            int intervalDays;

            if (userNow.Year == userDate.Year)
                intervalDays = userNow.DayOfYear - userDate.DayOfYear;
            else
                intervalDays = intervalTime.Days + 1;

            string result = "{0}";
            if (showTime && intervalDays <= 2)
                result = "{0}" + " " + userDate.ToString(timeFormat);

            if (intervalDays > 7)
            {

                if (userDate.Year == userNow.Year)
                {
                    return string.Format(ResourceAccessor.GetString("Common_Pattern_CNMonthDay"),
                                         userDate.Month,
                                         userDate.Day,
                                         showTime && intervalDays <= 2 ? " " + userDate.ToString(timeFormat) : "");
                }

                return userDate.ToString(dateFormat + (showTime ? " " + timeFormat : ""));
            }

            if (intervalDays >= 3)
            {
                string timeScope = string.Format(ResourceAccessor.GetString("Common_Pattern_DayAgo"), intervalDays);
                return string.Format(result, timeScope);
            }

            if (intervalDays == 2)
            {
                return string.Format(result, ResourceAccessor.GetString("Common_BeforeYesterday"));
            }

            if (intervalDays == 1)
            {
                return string.Format(result, ResourceAccessor.GetString("Common_YesterdayAt")); ;
            }

            if (intervalTime.Hours >= 1)
                return string.Format(ResourceAccessor.GetString("Common_Pattern_HoursAgo"), intervalTime.Hours);

            if (intervalTime.Minutes >= 1)
                return string.Format(ResourceAccessor.GetString("Common_Pattern_MinutelAgo"), intervalTime.Minutes);

            if (intervalTime.Seconds >= 1)
                return string.Format(ResourceAccessor.GetString("Common_Pattern_SecondsAgo"), intervalTime.Seconds);

            return ResourceAccessor.GetString("Common_Now");
        }

        /// <summary>
        /// 转换成用户所在时区的时间
        /// </summary>
        /// <param name="dateTime">待转换日期(UTC时间)</param>
        /// <returns>返回用户所在时区的时间</returns>
        public static DateTime ConvertToUserDate(this DateTime dateTime)
        {
            //User currentUser;
            //DateTime userDate = dt.AddHours(currentUser.Timezone);

            
            if (dateTime.Kind == DateTimeKind.Local)
                return dateTime;
            else
                return dateTime.AddHours(8);

        }

    }
}
