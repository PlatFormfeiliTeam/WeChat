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
using WeChat.ModelBusi;

namespace WeChat.Page.BusiOpera
{
    public partial class DeclareList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string BindList(string declcode, string startdate, string enddate, string busitype, string modifyflag, string customsstatus, int start, int itemsPerLoad)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            DataTable dt = Declare.getDeclareInfo(declcode, startdate, enddate, getcode("busitype", busitype), getcode("modifyflag", modifyflag), customsstatus, start, itemsPerLoad);
            var json = JsonConvert.SerializeObject(dt, iso);
            return json;
        }
        private static string getcode(string key, string value)
        {
            string code = "";
            if (key == "busitype")
            {
                switch (value)
                {
                    case "空运出口": code = "10"; break;
                    case "空运进口": code = "11"; break;
                    case "海运出口": code = "20"; break;
                    case "海运进口": code = "21"; break;
                    case "陆运出口": code = "30"; break;
                    case "陆运进口": code = "31"; break;
                    case "国内业务": code = "40-41"; break;
                    case "特殊出口": code = "50"; break;
                    case "特殊进口": code = "51"; break;
                    default: code = ""; break;
                }
            }

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


        //[WebMethod]
        //public static string BindListDetail(string id)
        //{
        //    IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
        //    iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        //    DataTable dt = HsCode.getHsCodeInfoDetail(id);
        //    var json = JsonConvert.SerializeObject(dt, iso);
        //    return json;
        //}


    }
}