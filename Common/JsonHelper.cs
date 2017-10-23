using WeChat.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Xml;

namespace WeChat.Common
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
            
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }


        /// <summary>
        /// 【通用】获取JSON指定类型返回值
        /// </summary>
        /// <typeparam name="T">指定非错误的返回类型</typeparam>
        /// <param name="JsonData">序列化前JSON字符</param>
        /// <returns>JSON序列化后数据</returns>
        public static ReturnDataEn<T> GetJson<T>(string JsonData)
        {
            var result = new ReturnDataEn<T>();
            if (JsonData.IndexOf("errcode") != -1)
            {
                result.ResponseState = false;
                result.ErrorData = JsonConvert.DeserializeObject<AppErrorEn>(JsonData);
            }
            else
            {
                result.ResponseState = true;
                result.ResponseData = JsonConvert.DeserializeObject<T>(JsonData);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public static XmlDocument ParseJson(string json, string rootName)
        {
            return JsonConvert.DeserializeXmlNode(json, rootName);
        }
        /// <summary>
        /// xml转为实体(如果xml没有entity节点，是否报错——待测试)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlToEntity<T>(string xml) where T : class
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            string jsonStr = JsonConvert.SerializeXmlNode(xmlDoc);
            T result = DeserializeJsonToObject<T>(jsonStr);
            return result;
        }
        /// <summary>
        /// xml转为实体（待完善：1暂时只支持一层节点，2实体属性为数值类型时若数据为空转换报错）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json"></param>
        /// <returns>对象实体</returns>
        public static T XmlToEntity_my<T>(this string xml) where T : new()
        {
            try
            {
                T each = new T();
                if (string.IsNullOrEmpty(xml))
                {
                    return each ;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                PropertyInfo[] pis = each.GetType().GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    XmlNode node = xmlDoc.SelectSingleNode("xml/" + pi.Name);
                    if (node != null)
                    {
                        SetPropertyValue(each, pi, node.InnerText);
                    }
                }

                    return each;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void SetPropertyValue(object each, PropertyInfo pi, string value)
        {
            try
            {
               
                if (pi.PropertyType == System.Type.GetType("System.Int32"))
                {
                    int i = 0;
                    try
                    {
                        i = Int32.Parse(value);
                    }
                    catch(Exception e){
                        
                    }
                    pi.SetValue(each, i);
                }
                else if (pi.PropertyType == System.Type.GetType("System.Int64"))
                {
                    Int64 i = 0;
                    try
                    {
                        i = Int64.Parse(value);
                    }
                    catch (Exception e)
                    {

                    }
                    pi.SetValue(each, i);
                }
                else if (pi.PropertyType == System.Type.GetType("System.Decimal"))
                {
                    decimal i = 0;
                    try
                    {
                        i = Decimal.Parse(value);
                    }
                    catch (Exception e)
                    {

                    }
                    pi.SetValue(each, i);
                }
                else if (pi.PropertyType == System.Type.GetType("System.Double"))
                {
                    double i = 0;
                    try
                    {
                        i = Double.Parse(value);
                    }
                    catch (Exception e)
                    {

                    }
                    pi.SetValue(each, i);
                }
                else if (pi.PropertyType == System.Type.GetType("System.DateTime"))
                {
                    Double i = 0;
                    DateTime s = new DateTime(1970, 1, 1);
                    try
                    {
                        i = Double.Parse(value);
                    }
                    catch(Exception e)
                    {

                    }
                    s.AddSeconds(i);
                    pi.SetValue(each, s);
                }
                else if (pi.PropertyType.IsEnum)
                {
                    pi.SetValue(each, System.Enum.Parse(pi.PropertyType, value));
                }
                else if (pi.PropertyType == System.Type.GetType("System.Boolean"))
                {
                    try
                    {
                        if (Int32.Parse(value) == 1)
                        {
                            pi.SetValue(each, true);
                        }
                        else
                        {
                            pi.SetValue(each, false);
                        }
                    }
                    catch(Exception e)
                    {
                        pi.SetValue(each, false);
                    }
                    

                }
                else if (pi.PropertyType == System.Type.GetType("System.DBNull"))
                {
                    //pi.SetValue(null);
                }
                else
                {
                    pi.SetValue(each, value);
                }
            }
            catch (Exception e)
            {
                LogHelper.Write("XmlToEntity_my异常：" + e.Message);
                throw;
            }
        }
    }
}