using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration.Provider;
using System.Globalization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// helper工具类
    /// </summary>
    public sealed class VerificationCodeProviderHelper
    {
        private readonly NameValueCollection config;
        public VerificationCodeProviderHelper(NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            this.config = config;
        }

        #region Methods
        /// <summary>
        /// 辅助抛出异常
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="format"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static ProviderException CreateException(Exception innerException, string format, params object[] arguments)
        {
            string message = (arguments == null) ? format : string.Format(CultureInfo.CurrentCulture, format, arguments);

            if (innerException == null)
                return new ProviderException(message);
            else
                return new ProviderException(message, innerException);
        }

        /// <summary>
        /// 转换成INT32
        /// </summary>
        /// <param name="name"></param>
        /// <param name="required"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int ParseInt32(string name, bool required, int defaultValue)
        {
            string value = config[name];
            config.Remove(name);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    return int.Parse(value, CultureInfo.CurrentCulture);
                }
                catch (OverflowException ex)
                {
                    throw CreateException(ex, "Errors.ProviderHelper_Overflow", name);
                }
                catch (FormatException ex)
                {
                    throw CreateException(ex, "Errors.ProviderHelper_Format", name);
                }
            }
            else if (required)
                throw CreateException(null, "Errors.ProviderHelper_Required", name);
            else
                return defaultValue;
        }
        /// <summary>
        /// 辅助转化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="required"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float ParseSingle(string name, bool required, float defaultValue)
        {
            string value = config[name];
            config.Remove(name);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    return float.Parse(value, CultureInfo.CurrentCulture);
                }
                catch (OverflowException ex)
                {
                    throw CreateException(ex, "Errors.ProviderHelper_Overflow", name);
                }
                catch (FormatException ex)
                {
                    throw CreateException(ex, "Errors.ProviderHelper_Format", name);
                }
            }
            else if (required)
                throw CreateException(null, "Errors.ProviderHelper_Required", name);
            else
                return defaultValue;
        }
        /// <summary>
        /// 辅助转化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="required"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool ParseBoolean(string name, bool required, bool defaultValue)
        {
            string value = config[name];
            config.Remove(name);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    return bool.Parse(value);
                }
                catch (FormatException ex)
                {
                    throw CreateException(ex, "Errors.ProviderHelper_Format", name);
                }
            }
            else if (required)
                throw CreateException(null, "Errors.ProviderHelper_Required", name);
            else
                return defaultValue;
        }

        /// <summary>
        /// 辅助转化
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <param name="required"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public TEnum ParseEnum<TEnum>(string name, bool required, TEnum defaultValue)
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Errors.ProviderHelper_TEnumInvalid", "TEnum"));

            string value = config[name];
            config.Remove(name);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    return (TEnum)Enum.Parse(typeof(TEnum), value, true);
                }
                catch (ArgumentException ex)
                {
                    throw CreateException(ex, "Errors.ProviderHelper_Enum", name);
                }
            }
            else if (required)
                throw CreateException(null, "Errors.ProviderHelper_Required", name);
            else
                return defaultValue;
        }

        /// <summary>
        /// 辅助转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="includeEmptyItems"></param>
        /// <param name="trimItemsBeforeParse"></param>
        /// <param name="required"></param>
        /// <param name="separators"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public IList<T> ParseList<T>(string name, bool includeEmptyItems, bool trimItemsBeforeParse, bool required, params char[] separators)
        {
            string value = config[name];

            if (string.IsNullOrEmpty(value))
                return null;
            else
                return new List<T>(ParseCollection<T>(name, includeEmptyItems, trimItemsBeforeParse, required, separators));
        }

        /// <summary>
        /// 辅助转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="includeEmptyItems"></param>
        /// <param name="trimItemsBeforeParse"></param>
        /// <param name="required"></param>
        /// <param name="separators"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public IEnumerable<T> ParseCollection<T>(string name, bool includeEmptyItems, bool trimItemsBeforeParse, bool required, params char[] separators)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            if (converter == null || !converter.CanConvertFrom(typeof(string)))
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Errors.ProviderHelper_InvalidListItemConverter", name));

            string value = config[name];
            config.Remove(name);

            if (!string.IsNullOrEmpty(value))
            {
                string[] array = value.Split(separators, (includeEmptyItems) ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in array)
                {
                    string itemToConvert;
                    T convertedItem;

                    if (trimItemsBeforeParse)
                    {
                        itemToConvert = item.Trim();

                        if (!includeEmptyItems && itemToConvert.Length == 0)
                            continue;
                    }
                    else
                        itemToConvert = item;

                    try
                    {
                        convertedItem = (T)converter.ConvertFromString(itemToConvert);
                    }
                    catch (NotSupportedException ex)
                    {
                        throw CreateException(ex, "Errors.ProviderHelper_InvalidListItemConverter", name);
                    }
                    catch (Exception ex)
                    {
                        throw CreateException(ex, "Errors.ProviderHelper_ItemConversionError", name);
                    }

                    yield return convertedItem;
                }
            }
            else if (required)
                throw CreateException(null, "Errors.ProviderHelper_Required", name);
        }
        #endregion
    }
}
