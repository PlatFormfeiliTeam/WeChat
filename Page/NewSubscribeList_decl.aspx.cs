using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat.ModelBusi;
using WeChat.Common;
using Newtonsoft.Json;
using WeChat.Entity;
using WeChat.ModelWeChat;

namespace WeChat.Page
{
    public partial class NewSubscribeList_decl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        [WebMethod]
        public static string QuerySubscribeInfo(string subscribestart, string subscribeend,string declarationcode, string istrigger, string busitype, string busiunit, string ordercode,
            string cusno, string contract, string submitstart, string submitend, int pagesize, int lastnum)
        {
            string sum = "0";
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || user.GwyUserID <= 0)
                return "";
            DataTable infodt = SubscribeModel.getNewSubscribeInfo_Decl(subscribestart, subscribeend, declarationcode, istrigger, busitype, busiunit, ordercode, cusno, contract,
                submitstart, submitend, pagesize, lastnum, user.GwyUserID, out sum);
            //DataTable infodt = SubscribeModel.getNewSubscribeInfo_Decl(subscribestart, subscribeend, declarationcode, istrigger, busitype, busiunit, ordercode, cusno, contract,
            //    submitstart, submitend, pagesize, lastnum, 1124, out sum);
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
                    dr["sum"] = sum;
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
        public static string NewQuerySubscribeInfo()
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || user.GwyUserID <= 0)
                return "";
            DataTable infodt = SubscribeModel.getNewSubscribeInfo_Decl(user.GwyUserID);
            //DataTable infodt = SubscribeModel.getNewSubscribeInfo_Decl(1124);
            
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