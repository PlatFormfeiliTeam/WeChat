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

namespace WeChat.Page
{
    public partial class SubscribeList : System.Web.UI.Page
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
                    WGUserEn wuser = UserModel.getWeChatUser(userInfo.OpenID);
                    if (wuser == null || string.IsNullOrEmpty(wuser.GwyUserName))
                    {//账号未关联，跳转至登录界面
                        System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SubscribeList");
                    }
                    else
                    {//不需登录，保存当前用户
                        HttpContext.Current.Session["user"] = wuser;
                    }
                }
                else
                {//获取授权失败，也跳转至登录页面
                    System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SubscribeList");
                }
            }
            
        }

        

        [WebMethod]
        public static string getBusiSubscribeInfo()
        {
            //WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            //if (user == null || user.GwyUserID <= 0)
            //    return "";
            //DataTable infodt = SubscribeModel.getNewSubscribeInfo_Order(user.GwyUserID);
            DataTable infodt = SubscribeModel.getNewSubscribeInfo_Order(1124);
            if (infodt == null || infodt.Rows.Count == 0)
                return "";
            try
            {
                foreach (DataRow dr in infodt.Rows)
                {
                    if (string.IsNullOrEmpty(dr["divideno"].ToString2())) dr["divideno"] = "";
                    if (string.IsNullOrEmpty(dr["logisticsname"].ToString2())) dr["logisticsname"] = "";
                    if (string.IsNullOrEmpty(dr["contractno"].ToString2())) dr["contractno"] = "";
                    dr["declstatus"] = SwitchHelper.switchValue("declstatus", dr["declstatus"].ToString2());
                    dr["inspstatus"] = SwitchHelper.switchValue("inspstatus", dr["inspstatus"].ToString2());
                }
                IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                string json = JsonConvert.SerializeObject(infodt, iso);
                return json;
            }
            catch (Exception e)
            {
                LogHelper.Write("MyBusiness_QueryData:" + e.Message);
                return "";
            }
        }

        [WebMethod]
        public static string getDeclSubscribeInfo()
        {
            //WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            //if (user == null || user.GwyUserID <= 0)
            //    return "";
            //DataTable infodt = SubscribeModel.getNewSubscribeInfo_Decl(user.GwyUserID);
            DataTable infodt = SubscribeModel.getNewSubscribeInfo_Decl(1124);
            if (infodt == null || infodt.Rows.Count == 0)
                return "";
            try
            {
                foreach (DataRow dr in infodt.Rows)
                {
                    if (string.IsNullOrEmpty(dr["transname"].ToString2())) dr["transname"] = "";
                    if (string.IsNullOrEmpty(dr["tradename"].ToString2())) dr["tradename"] = "";
                    if (string.IsNullOrEmpty(dr["status"].ToString2())) dr["status"] = "";
                    if (string.IsNullOrEmpty(dr["customsstatus"].ToString2())) dr["customsstatus"] = "";
                }
                IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                string json = JsonConvert.SerializeObject(infodt, iso);
                return json;
            }
            catch (Exception e)
            {
                LogHelper.Write("MyBusiness_QueryData:" + e.Message);
                return "";
            }

        }

        [WebMethod]
        public static bool DeleteSubscribeInfo(string id)
        {
            return SubscribeModel.deleteSubscribe(id);
        }
    }
}