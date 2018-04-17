using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using WeChat.Common;
using WeChat.Entity;

namespace WeChat.ModelWeChat
{
    /// <summary>
    /// 获取token
    /// </summary>
    public class TokenModel
    {
        #region oldversion
        //static string appid = "wxbf28a5d74131c29b";
        //static string appsecret = "a568400b7c9014fe23f3a036888b2716";
        //public static AccessTokenEn token { get; set; }
        //public static void getAccesss_Token()
        //{
        //    string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
        //    var wc = new WebClient();
        //    var strReturn = wc.DownloadString(url);
        //    token = JsonHelper.DeserializeJsonToObject<AccessTokenEn>(strReturn);
        //}

        #endregion

        private static DateTime GetAccessToken_Time;
        /// <summary>
        /// 过期时间为7200秒
        /// </summary>
        private static int Expires_Period = 7200;
        /// <summary>
        /// 
        /// </summary>
        private static string mAccessToken;

        public static string AppID = ConfigurationManager.AppSettings["AppID"];
        public static string AppSecret = ConfigurationManager.AppSettings["AppSecret"];
        /// <summary>
        /// 调用获取ACCESS_TOKEN,包含判断是否过期
        /// </summary>
        public static string AccessToken
        {
            get
            {
                //如果为空，或者过期，需要重新获取
                if (string.IsNullOrEmpty(mAccessToken) || HasExpired())
                {
                    //获取access_token
                    mAccessToken = GetAccessToken(AppID, AppSecret);
                }

                return mAccessToken;
            }
        }
        /// <summary>
        /// 获取ACCESS_TOKEN方法
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private static string GetAccessToken(string appId, string appSecret)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
            string result = WeChatHelper.GetData(url);
            if(result.Contains("40164"))
            {
                LogHelper.Write("TokenModel_GetAccessToken出错：" + result);
                return null;
            }

            XmlDocument doc = JsonHelper.ParseJson(result, "root");
            XmlNode root = doc.SelectSingleNode("root");
            if (root != null)
            {
                XmlNode access_token = root.SelectSingleNode("access_token");
                if (access_token != null)
                {
                    GetAccessToken_Time = DateTime.Now;
                    if (root.SelectSingleNode("expires_in") != null)
                    {
                        Expires_Period = int.Parse(root.SelectSingleNode("expires_in").InnerText);
                    }
                    return access_token.InnerText;
                }
                else
                {
                    GetAccessToken_Time = DateTime.MinValue;
                }
            }

            return null;
        }
        /// <summary>
        /// 判断Access_token是否过期
        /// </summary>
        /// <returns>bool</returns>
        private static bool HasExpired()
        {
            if (GetAccessToken_Time != null)
            {
                //过期时间，允许有一定的误差，一分钟。获取时间消耗
                if (DateTime.Now > GetAccessToken_Time.AddSeconds(Expires_Period).AddSeconds(-60))
                {
                    return true;
                }
            }
            return false;
        }
    }
}