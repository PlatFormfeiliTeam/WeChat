using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WeChat.Common
{
    public static class Extension
    {
        public static bool ToBool(this int value)
        {
            bool result = true;
            if (value > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string Trim2(this string value)
        {
            string result = value;
            if (!value.IsNullOrEmpty())
            {
                result = value.Trim();
            }
            return result;
        }
        /// <summary>
        /// 将日期转化为字符串类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static string DateToString(this DateTime value, string defaultTime)
        {
            string result = defaultTime;
            if (value != null && value.IsDaylightSavingTime())
            {
                result = value.ToString();
            }
            return result;
        }
        /// <summary>
        /// 判断时间是否有效
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static DateTime ToDate(this DateTime value, DateTime defaultTime)
        {
            DateTime result = value;
            string a = value.ToString();
            if (a == "0001/1/1 0:00:00")//表示时间为空
            {
                result = defaultTime;
            }
            return result;
        }
        public static string ToString2(this object value)
        {
            string result = "";
            if (value != null)
            {
                result = value.ToString();
            }
            return result;
        }
        public static string ToString2(this object value, string defaultValue)
        {
            string result = defaultValue;
            if (value != null)
            {
                result = value.ToString();
            }
            return result;
        }
        public static int ToInt(this string value)
        {
            return ToInt(value, 0);
        }

        public static int ToInt(this string value, int defaultValue)
        {
            int result = 0;
            bool success = Int32.TryParse(value, out result);
            if (!success)
            {
                result = defaultValue;
            }
            return result;
        }
        public static double ToDouble(this string value)
        {
            return ToDoule(value, 0.0);
        }

        public static double ToDoule(this string value, double defaultValue)
        {
            double result = 0;
            bool success = Double.TryParse(value, out result);
            if (!success)
            {
                result = defaultValue;
            }
            return result;
        }

        public static bool ToBool(this string value)
        {
            bool result = false;
            bool.TryParse(value, out result);
            return result;
        }

        public static DateTime ToDateTime(this string value)
        {
            return ToDateTime(value, DateTime.Now);
        }

        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            DateTime result;
            DateTime.TryParse(value, out result);
            return result == DateTime.MinValue ? defaultValue : result;
        }

        public static string ToSHA1(this string value)
        {
            string result = string.Empty;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] array = sha1.ComputeHash(Encoding.Unicode.GetBytes(value));
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i].ToString("x2");
            }
            return result;
        }

       

        public static T CopyToEntity<T>(this object entity) where T : new()
        {
            var convertProperties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>();

            var convert = new T();

            foreach (var entityProperty in entityProperties)
            {
                var property = entityProperty;
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == property.Name && prop.PropertyType == property.PropertyType);
                if (convertProperty != null)
                {
                    object val = entityProperty.GetValue(entity);
                    if (val != null)
                        convertProperty.SetValue(convert, val);
                    //convertProperty.SetValue(convert, Convert.ChangeType(val, convertProperty.PropertyType));
                }
            }

            return convert;
        }
    }
}