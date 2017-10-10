using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using WeChat.Common;
using WeChat.Entity;

namespace WeChat.WXModel
{
    /// <summary>
    /// 获取token
    /// </summary>
    public class TokenModel
    {
        static string appid = "wxbf28a5d74131c29b";
        static string appsecret = "a568400b7c9014fe23f3a036888b2716";
        public static AccessTokenEn token { get; set; }
        public static void getAccesss_Token()
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret);
            var wc = new WebClient();
            var strReturn = wc.DownloadString(url);
            token = JsonHelper.DeserializeJsonToObject<AccessTokenEn>(strReturn);
        }
    }
}