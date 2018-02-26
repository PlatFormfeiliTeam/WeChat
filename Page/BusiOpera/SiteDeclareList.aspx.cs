using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    public partial class SiteDeclareList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WUserEn userInfo = PageShowQuan.GetShouQuanMessage();
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.OpenID))
            {//授权成功
                LogHelper.Write("第9步：" + userInfo.OpenID);
                WGUserEn wuser = UserModel.getWeChatUser(userInfo.OpenID);
                if (wuser == null || string.IsNullOrEmpty(wuser.GwyUserName))
                {//账号未关联，跳转至登录界面
                    LogHelper.Write("第10步：" + userInfo.OpenID);
                    System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SiteDeclareList");
                }
                else if (wuser.IsReceiver != 1)
                {//不是接单单位，无此权限
                    LogHelper.Write("第11步：" + userInfo.OpenID);
                    System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SiteDeclareList");
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
        public static string BindList(string siteapplytime_s, string siteapplytime_e, string declcode, string customareacode, string ispass, string ischeck, string busitype
            , string modifyflag, string auditflag, string busiunit, string ordercode, string cusno, string divideno, string contractno
            , string submittime_s, string submittime_e, string sitepasstime_s, string sitepasstime_e
            , int start, int itemsPerLoad)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
            {
                return "[]";
            }
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            DataTable dt = SiteDeclare.getSiteDeclareInfo(siteapplytime_s, siteapplytime_e, declcode, customareacode, ispass, ischeck, busitype
                , getcode("modifyflag", modifyflag), auditflag, busiunit, ordercode, cusno, divideno, contractno
                , submittime_s, submittime_e, sitepasstime_s, sitepasstime_e
                , start, itemsPerLoad, user.CustomerCode);
            var json = JsonConvert.SerializeObject(dt, iso);
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

        //报关单交接
        [WebMethod]
        public static string Siteapply(string ordercode)
        {
            return SiteDeclare.Siteapply(ordercode);
        }

        //报关单详细
        [WebMethod]
        public static string Detail(string ordercode)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyyMMdd HH:mm:ss";

            DataSet ds = SiteDeclare.Detail(ordercode);
            var json_order = JsonConvert.SerializeObject(ds.Tables[0], iso);
            var json_decl = JsonConvert.SerializeObject(ds.Tables[1]);
            return "[{\"json_order\":" + json_order + ",\"json_decl\":" + json_decl + "}]";
        }

        //报关单放行
        [WebMethod]
        public static string Pass(string ordercode)
        {
            return SiteDeclare.Pass(ordercode);
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
        public static string declcontainerdata(string ordercode)
        {
            DataTable dt = SiteDeclare.getdeclcontainerdata(ordercode);
            var json = JsonConvert.SerializeObject(dt);
            return json;
        }

        [WebMethod]
        public static string loadcheckdata(string ordercode)
        {
            DataTable dt = SiteDeclare.getloadcheckdata(ordercode);
            var json = JsonConvert.SerializeObject(dt);
            return json;
        }

        [WebMethod]
        public static string check_audit_save(string ordercode, string checktime, string checkname, string checkid, string checkremark
            , string auditflagtime, string auditflagname, string auditflagid, string auditcontent)
        {
            return SiteDeclare.check_audit_save(ordercode, checktime, checkname, checkid, checkremark
                , auditflagtime, auditflagname, auditflagid, auditcontent);
        }

        //查验图片
        [WebMethod]
        public static string SaveFile(string mediaIds, string ordercode)
        {
            return SiteDeclare.SaveFile(mediaIds, ordercode);            
        }

        [WebMethod]
        public static string picfileconsult(string ordercode)
        {
            DataTable dt = SiteDeclare.picfileconsult(ordercode);
            var json = JsonConvert.SerializeObject(dt);
            return json;
        }

    }
}