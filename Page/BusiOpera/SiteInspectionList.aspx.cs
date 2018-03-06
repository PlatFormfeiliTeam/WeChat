using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat.Common;
using WeChat.Entity;
using WeChat.ModelBusi;
using WeChat.ModelWeChat;

namespace WeChat.Page.BusiOpera
{
    public partial class SiteInspectionList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            //如果当前用户未登陆，先获取授权 
             if (user == null)
             {
                 WUserEn userInfo = PageShowQuan.GetShouQuanMessage();
                 if (userInfo != null && !string.IsNullOrEmpty(userInfo.OpenID))
                 {//授权成功
                     LogHelper.Write("第9步：" + userInfo.OpenID);
                     WGUserEn wuser = UserModel.getWeChatUser(userInfo.OpenID);
                     if (wuser == null || string.IsNullOrEmpty(wuser.GwyUserName))
                     {//账号未关联，跳转至登录界面
                         LogHelper.Write("第10步：" + userInfo.OpenID);
                         System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SiteInspectionList");
                     }
                     else if (wuser.IsReceiver != 1)
                     {//不是接单单位，无此权限
                         LogHelper.Write("第11步：" + userInfo.OpenID);
                         System.Web.HttpContext.Current.Response.Redirect(@"../WarnPage.aspx");
                     }
                     else
                     {//不需登录，保存当前用户
                         HttpContext.Current.Session["user"] = wuser;
                     }
                     LogHelper.Write("第12步：" + wuser.WCOpenID);
                 }
                 else
                 {//获取授权失败，也跳转至登录页面
                     System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SiteDeclareList");
                 }
             }
             else if (user.IsReceiver != 1)
             {//不是接单单位，无此权限
                 System.Web.HttpContext.Current.Response.Redirect(@"../WarnPage.aspx");
             }
        }

        //微信接口js-sdk config
        [WebMethod]
        public static string getConf(string url)
        {
            return ModelWeChat.SignatureModel.getSignature(url);
        }

        [WebMethod]
        public static string getcuruser()
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
            {
                return "[]";
            }
            return "[{\"USERID\":" + user.GwyUserID + ",\"USERCODE\":'" + user.GwyUserCode + "',\"USERNAME\":'" + user.GwyUserName + "',\"CUSTOMERCODE\":'" + user.CustomerCode + "'}]";
        }

        //查询绑定
        [WebMethod]
        public static string BindList(string inspsiteapplytime_s, string inspsiteapplytime_e, string inspcode, string approvalcode, string ispass, string ischeck, string busitype
            , string lawflag, string isneedclearance, string isfumigation, string modifyflag, string busiunit, string contractno, string ordercode, string cusno, string divideno
            , string customareacode, string submittime_s, string submittime_e, string sitepasstime_s, string sitepasstime_e
            , int start, int itemsPerLoad)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
            {
                return "[]";
            }
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            DataSet ds = SiteInspection.getSiteInspectionInfo(inspsiteapplytime_s, inspsiteapplytime_e, inspcode, approvalcode, ispass, ischeck, busitype
                    , lawflag, isneedclearance, isfumigation, getcode("modifyflag", modifyflag), busiunit, contractno, ordercode, cusno, divideno
                    , customareacode, submittime_s, submittime_e, sitepasstime_s, sitepasstime_e
                    , start, itemsPerLoad, user.CustomerCode);//
            var json = "[{\"data\":" + JsonConvert.SerializeObject(ds.Tables[0], iso) + ",\"sum\":" + ds.Tables[1].Rows[0][0] + "}]";
            return json;
        }

        private static string getcode(string key, string value)
        {
            string code = "";
            if (key == "modifyflag")
            {
                switch (value)
                {
                    case "正常": code = "0"; break;
                    case "删单": code = "1"; break;
                    case "改单": code = "2"; break;
                    case "改单完成": code = "3"; break;
                    default: code = ""; break;
                }
            }
            return code;
        }

        ////现场报检
        //[WebMethod]
        //public static string Siteapply(string ordercode)
        //{
        //    return SiteInspection.Siteapply(ordercode);
        //}

        //现场报检
        [WebMethod]
        public static string Siteapplyall(string ordercode)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
            {
                return "[]";
            }

            string msg = "";
            JArray ja = JArray.Parse(ordercode);
            for (int i = 0; i < ja.Count; i++)
            {
                msg = msg + SiteInspection.Siteapplyall(ja[i].ToString(), user);
                if (i != ja.Count - 1)
                {
                    msg = msg + ",";
                }
            }
            return "[" + msg + "]";
        }

        //报关单详细
        [WebMethod]
        public static string Detail(string ordercode)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyyMMdd HH:mm:ss";

            DataSet ds = SiteInspection.Detail(ordercode);
            var json_order = JsonConvert.SerializeObject(ds.Tables[0], iso);
            var json_insp = JsonConvert.SerializeObject(ds.Tables[1]);
            return "[{\"json_order\":" + json_order + ",\"json_insp\":" + json_insp + "}]";
        }

        ////报关单放行
        //[WebMethod]
        //public static string Pass(string ordercode)
        //{
        //    return SiteInspection.Pass(ordercode);
        //}

        //现场放行
        [WebMethod]
        public static string Passall(string ordercode)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
            {
                return "[]";
            }

            string msg = "";
            JArray ja = JArray.Parse(ordercode);
            for (int i = 0; i < ja.Count; i++)
            {
                msg = msg + SiteInspection.Passall(ja[i].ToString(), user);
                if (i != ja.Count - 1)
                {
                    msg = msg + ",";
                }
            }
            return "[" + msg + "]";
        }

        //查验标志 绑定集装箱数据
        /*
        [WebMethod]
        public static string declcontainerdata(string ordercode)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyyMMdd HH:mm";

            DataSet ds = SiteDeclare.getdeclcontainerdata(ordercode);
            var json_order = JsonConvert.SerializeObject(ds.Tables[0], iso);
            var json_predeclcontainer = JsonConvert.SerializeObject(ds.Tables[1]);
            return "[{\"json_order\":" + json_order + ",\"json_predeclcontainer\":" + json_predeclcontainer + "}]";
        }
        */

        [WebMethod]
        public static string inspcontainerdata(string ordercode)
        {
            DataTable dt = SiteInspection.getinspcontainerdata(ordercode);
            var json = JsonConvert.SerializeObject(dt);
            return json;
        }

        [WebMethod]
        public static string loadcheckdata(string ordercode)
        {
            DataTable dt = SiteInspection.getloadcheckdata(ordercode);
            var json = JsonConvert.SerializeObject(dt);
            return json;
        }

        [WebMethod]
        public static string check_fumigation_save(string ordercode, string inspchecktime, string inspcheckname, string inspcheckid, string inspcheckremark
            , string fumigationtime, string fumigationname, string fumigationid)
        {
            return SiteInspection.check_fumigation_save(ordercode, inspchecktime, inspcheckname, inspcheckid, inspcheckremark
                , fumigationtime, fumigationname, fumigationid);
        }

        //查验图片
        [WebMethod]
        public static string SaveFile(string mediaIds, string ordercode)
        {
            return SiteInspection.SaveFile(mediaIds, ordercode);
        }

        [WebMethod]
        public static string picfileconsult(string ordercode)
        {
            DataTable dt = SiteInspection.picfileconsult(ordercode);
            var json = JsonConvert.SerializeObject(dt);
            return json;
        }


    }
}