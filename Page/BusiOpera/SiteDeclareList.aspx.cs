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

        //查询绑定
        [WebMethod]
        public static string BindList(string inout_type, string issiterep, string busitype, string ispass, string startdate, string enddate
            , string radiotype, string morecon, int start, int itemsPerLoad)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            DataTable dt = SiteDeclare.getSiteDeclareInfo(inout_type, issiterep, busitype, ispass, startdate, enddate, radiotype, morecon, start, itemsPerLoad);
            var json = JsonConvert.SerializeObject(dt, iso);
            return json;
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
        public static string checksave(string ordercode, string checktime, string checkname, string checkid, string checkremark)
        {
            return SiteDeclare.checksave(ordercode, checktime, checkname, checkid, checkremark);
        }

        [WebMethod]
        public static string checkcancel(string ordercode)
        {
            return SiteDeclare.checkcancel(ordercode);
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

        [WebMethod]
        public static string auditsave(string ordercode, string auditflagtime, string auditflagname, string auditflagid, string auditcontent)
        {
            return SiteDeclare.auditsave(ordercode, auditflagtime, auditflagname, auditflagid, auditcontent);
        }

        [WebMethod]
        public static string auditcancel(string ordercode)
        {
            return SiteDeclare.auditcancel(ordercode);
        }
    }
}