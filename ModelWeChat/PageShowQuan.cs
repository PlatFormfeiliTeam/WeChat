using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Common;
using WeChat.Entity;

namespace WeChat.ModelWeChat
{
    public class PageShowQuan
    {
        /// <summary>
        /// 获取授权用户的基本信息,包括头像，姓名，等等(推荐方法）
        /// </summary>
        /// <param name="accessToken">用户授权之后的accessToken</param>
        /// <param name="openid">用户授权之后的openid</param>
        /// <returns></returns>
        public static WUserEn GetShouQuanMessage()
        {
            //先判断是否有获取到用户授权的Code,HttpContext.Current.Session["ShouquanCode"]
            if (HttpContext.Current.Session["ShouquanCode"] == null || HttpContext.Current.Session["ShouquanCode"].ToString() == "")
            {
                HttpContext.Current.Session["ShouquanCode"] = "123";
                LogHelper.Write("第1步：" + HttpContext.Current.Session["ShouquanCode"]);
                //用户授权的Code
                GetShouQuanCodeUrl(HttpContext.Current.Request.Url.AbsoluteUri.Replace(":8088", ""));

            }
            else if (HttpContext.Current.Request.QueryString["code"] == null || HttpContext.Current.Request.QueryString["code"] == "")
            {
                LogHelper.Write("第3步：" + HttpContext.Current.Session["code"]);
                //用户授权的Code 
                GetShouQuanCodeUrl(HttpContext.Current.Request.Url.AbsoluteUri.Replace(":8088", ""));
            }
            else
            {
                var model = ShouQuanAccessToken(HttpContext.Current.Request.QueryString["code"]);
                LogHelper.Write("第4步：" + model.openid);
                var url = @"https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
                url = string.Format(url, model.access_token, model.openid);
                string getJson = MyHttpHelper.HttpGet(url);
                LogHelper.Write("第5步：" + getJson);
                var ac = JsonHelper.DeserializeJsonToObject<WUserEn>(getJson);
                LogHelper.Write("OpenID：" + ac.OpenID);
                return ac;
            }
            return null;
        }
        /// <summary>
        /// 重新获取用户授权的Code,可以获取用户的基本信息（头像，姓名，等等）(推荐用的方法）
        /// </summary>
        /// <param name="url">目标Url</param>
        /// <returns></returns>
        public static void GetShouQuanCodeUrl(string url)
        {
            LogHelper.Write("第6步：" + url);
            string CodeUrl = "";
            //加密过的url
            string value = HttpUtility.UrlEncode(url);
            //用户授权后的Code
            CodeUrl = @"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
            CodeUrl = string.Format(CodeUrl, TokenModel.AppID, url);
            System.Web.HttpContext.Current.Response.Redirect(CodeUrl);//先跳转到微信的服务器，取得code后会跳回来这页面的

        }
        /// <summary>
        //用户授权之后，获取授权的Access_Token与基本的Access_Token是不同的(推荐方法）
        /// </summary>
        /// <param name="code">用户授权之后的Code</param>
        /// <returns></returns>
        public static OauthAccessToken ShouQuanAccessToken(string code)
        {
            LogHelper.Write("第7步：" + code);
            var url = @"https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            url = string.Format(string.Format(url, TokenModel.AppID, TokenModel.AppSecret, code));
            string getJson = MyHttpHelper.HttpGet(url);
            LogHelper.Write("第8步：" + getJson);
            OauthAccessToken ac = new OauthAccessToken();
            ac = JsonHelper.DeserializeJsonToObject<OauthAccessToken>(getJson);
            return ac;
        }
        public class OauthAccessToken
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
        }
    }
}