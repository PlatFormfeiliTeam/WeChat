using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using WeChat.Common;
using WeChat.Entity;
using System.Configuration;

namespace WeChat.ModelWeChat
{
    /// <summary>
    /// 获取token
    /// </summary>
    public class Jsapi_ticketModel
    {
        private static DateTime GetJsapi_ticket_Time;
        /// <summary>
        /// 过期时间为7200秒
        /// </summary>
        private static int Jsapi_ticket_Expires_Period = 7200;
        /// <summary>
        /// 
        /// </summary>
        private static string mJsapi_ticket;

        public static string AppID = ConfigurationManager.AppSettings["AppID"];
        public static string AppSecret = ConfigurationManager.AppSettings["AppSecret"];
        /// <summary>
        /// 调用获取Jsapi_ticket,包含判断是否过期
        /// </summary>
        public static string Jsapi_ticket
        {
            get
            {
                //如果为空，或者过期，需要重新获取
                if (string.IsNullOrEmpty(mJsapi_ticket) || Jsapi_ticket_HasExpired())
                {
                    //获取access_token
                    mJsapi_ticket = GetJsapi_ticket(AppID, AppSecret);
                }

                return mJsapi_ticket;
            }
        }
        /// <summary>
        /// 获取Jsapi_ticket方法
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private static string GetJsapi_ticket(string appId, string appSecret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", TokenModel.AccessToken);
            string result = WeChatHelper.GetData(url);

            XmlDocument doc = JsonHelper.ParseJson(result, "root");
            XmlNode root = doc.SelectSingleNode("root");
            if (root != null)
            {
                XmlNode ticket = root.SelectSingleNode("ticket");
                if (ticket != null)
                {
                    GetJsapi_ticket_Time = DateTime.Now;
                    if (root.SelectSingleNode("expires_in") != null)
                    {
                        Jsapi_ticket_Expires_Period = int.Parse(root.SelectSingleNode("expires_in").InnerText);
                    }
                    return ticket.InnerText;
                }
                else
                {
                    GetJsapi_ticket_Time = DateTime.MinValue;
                }
            }

            return null;
        }
        /// <summary>
        /// 判断Access_token是否过期
        /// </summary>
        /// <returns>bool</returns>
        private static bool Jsapi_ticket_HasExpired()
        {
            if (GetJsapi_ticket_Time != null)
            {
                //过期时间，允许有一定的误差，一分钟。获取时间消耗
                if (DateTime.Now > GetJsapi_ticket_Time.AddSeconds(Jsapi_ticket_Expires_Period).AddSeconds(-60))
                {
                    return true;
                }
            }
            return false;
        }
    }
}