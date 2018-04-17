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

namespace WeChat.Page
{
    public partial class BusiSubsDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string code = HttpContext.Current.Session["code"].ToString2();
            LogHelper.Write("进入BusiSubsDetail页面，code=" + code);
        }

        [WebMethod]
        public static string getInfo(string code)
        {
            ListOrderModel model = new ListOrderModel();
            DataTable dt = model.getSubsInfo(code);

            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["ischeck"] = dr["ischeck"].ToString2() == "1" ? "海关查验" : "";
                    dr["checkpic"] = dr["checkpic"].ToString2() == "1" ? "含查验图片" : "";
                    dr["inspischeck"] = dr["inspischeck"].ToString2() == "1" ? "国检查验" : "";
                    dr["lawflag"] = dr["lawflag"].ToString2() == "1" ? "含法检" : "";
                    dr["declstatus"] = SwitchHelper.switchValue("declstatus", dr["declstatus"].ToString2());
                    dr["inspstatus"] = SwitchHelper.switchValue("inspstatus", dr["inspstatus"].ToString2());
                    if (string.IsNullOrEmpty(dr["divideno"].ToString2())) dr["divideno"] = "";
                    if (string.IsNullOrEmpty(dr["logisticsstatus"].ToString2())) dr["logisticsstatus"] = "";
                    if (string.IsNullOrEmpty(dr["contractno"].ToString2())) dr["contractno"] = "";
                }
            }
            catch (Exception e)
            {
                LogHelper.Write("BusiSubsDetail:" + e.Message);
            }

            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string json = JsonConvert.SerializeObject(dt, iso);
            return json;
        }
    }
}