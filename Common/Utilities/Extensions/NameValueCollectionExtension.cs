//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 获取Request.QueryString[key],Request.Form[key],Request.Params[key]并进行类型转换
    /// </summary>
    public static class NameValueCollectionExtension
    {
        /// <summary>
        /// 获取请求的参数
        /// </summary>
        /// <typeparam name="T">必须是基本类型</typeparam>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this NameValueCollection collection, string key)
        {
            if (typeof(T) == typeof(string))
                return Get<T>(collection, key, (T)Convert.ChangeType(string.Empty, typeof(T)));
            else
                return Get<T>(collection, key, default(T));
        }

        /// <summary>
        /// 获取请求的参数 
        /// </summary>
        /// <typeparam name="T">必须是基本类型</typeparam>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            T returnValue = defaultValue;
            if (collection[key] != null)
            {
                Type tType = typeof(T);
                if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (string.IsNullOrEmpty(collection[key].ToString()))
                        return defaultValue;
                    return (T)TypeDescriptor.GetConverter(Nullable.GetUnderlyingType(tType)).ConvertFrom(collection[key]);                    
                }
                else if (tType.IsEnum)
                {

                    return (T)Enum.Parse(tType, collection[key]);
                }
                else
                {
                    try
                    {
                        return (T)Convert.ChangeType(collection[key], tType);
                    }
                    catch
                    {
                        return returnValue;
                    }
                }
            }
            else
                return returnValue;
        }
         /// <summary>
        /// 获取请求中的集合数据
        /// </summary>
        /// <typeparam name="T">必须是基本类型</typeparam>
        /// <param name="collection">被扩展集合</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static IEnumerable<T> Gets<T>(this NameValueCollection collection, string key)
        {
            return Gets<T>(collection, key, default(IEnumerable<T>));
        }

        /// <summary>
        /// 获取请求中的集合数据
        /// </summary>
        /// <typeparam name="T">必须是基本类型</typeparam>
        /// <param name="collection">被扩展集合</param>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static IEnumerable<T> Gets<T>(this NameValueCollection collection, string key, IEnumerable<T> defaultValue)
        {
            if (collection[key] != null)
            {
                if (!string.IsNullOrEmpty(collection[key]))
                {
                    IList<T> iVal = new List<T>();
                    string[] strValArray = collection[key].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string val in strValArray)
                    {
                        try
                        {
                            iVal.Add((T)Convert.ChangeType(val, typeof(T)));
                        }
                        catch { }
                    }

                    return iVal;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取string类型值 
        /// </summary>
        /// <param name="collection">NameValueCollection</param>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认返回值</param>
        public static string GetString(this NameValueCollection collection, string key, string defaultValue)
        {
            string returnValue = defaultValue;
            if (collection[key] != null)
                returnValue = collection[key];

            return returnValue;
        }

        /// <summary>
        /// 获取int类型值
        /// </summary>
        /// <param name="collection">NameValueCollection</param>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认返回值</param>
        public static int GetInt(this NameValueCollection collection, string key, int defaultValue)
        {
            int returnValue = defaultValue;
            if (!string.IsNullOrEmpty(collection[key]))
                int.TryParse(collection[key], out returnValue);

            return returnValue;
        }

        /// <summary>
        /// 获取Float类型值
        /// </summary>
        /// <param name="collection">NameValueCollection</param>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认返回值</param>
        public static float GetFloat(this NameValueCollection collection, string key, float defaultValue)
        {
            float returnValue = defaultValue;
            if (!string.IsNullOrEmpty(collection[key]))
                float.TryParse(collection[key], out returnValue);

            return returnValue;
        }

        /// <summary>
        /// 获取bool类型值
        /// </summary>
        /// <param name="collection">NameValueCollection</param>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认返回值</param>
        public static bool GetBool(this NameValueCollection collection, string key, bool defaultValue)
        {
            bool returnValue = defaultValue;
            if (!string.IsNullOrEmpty(collection[key]))
                bool.TryParse(collection[key], out returnValue);

            return returnValue;
        }


        /// <summary>
        /// 获取Guid类型值
        /// </summary>
        /// <param name="collection">NameValueCollection</param>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认返回值</param>
        public static Guid GetGuid(this NameValueCollection collection, string key, Guid defaultValue)
        {
            Guid returnValue = defaultValue;
            if (!string.IsNullOrEmpty(collection[key]))
            {
                try
                {
                    returnValue = new Guid(collection[key]);
                }
                catch { }
            }

            return returnValue;
        }
    }
}
