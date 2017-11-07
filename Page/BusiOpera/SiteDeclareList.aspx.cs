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
    public partial class SiteDeclareList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
        private static string getcode(string key, string value)
        {
            string code = "";
            if (key == "busitype")
            {
                switch (value)
                {
                    case "空运": code = "'10','11'"; break;
                    case "海运": code = "'20','21'"; break;
                    case "陆运": code = "'30','31'"; break;
                    case "国内": code = "40-41"; break;
                    case "特殊出口": code = "50"; break;
                    case "特殊进口": code = "51"; break;
                    default: code = ""; break;
                }
            }

            return code;
        }

    }
}