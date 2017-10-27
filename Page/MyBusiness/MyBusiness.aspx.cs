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
using WeChat.ModelBusi;

namespace WeChat.Page.MyBusiness
{
    public partial class MyBusiness : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string data = Request["data"];
        }
        [WebMethod]
        public static string QueryData(string declstatus,string inspstatus,string inout,string busitype,string customs,string sitedeclare,string logisticsstatus,
            string starttime, string endtime, int itemsperload, int lastindex)
        {
            ListOrderModel orderModel = new ListOrderModel();
            DataTable dt = orderModel.getOrder(declstatus, inspstatus, inout, busitype, customs, sitedeclare, logisticsstatus, starttime, endtime, itemsperload, lastindex);
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["ischeck"] = dr["ischeck"].ToString2() == "1" ? "海关查验" : "";
                    dr["checkpic"] = dr["checkpic"].ToString2() == "1" ? "含查验图片" : "";
                    dr["inspcheck"] = dr["inspcheck"].ToString2() == "1" ? "国检查验" : "";
                    dr["lawflag"] = dr["lawflag"].ToString2() == "1" ? "含法检" : "";
                    dr["declstatus"] = switchValue("declstatus", dr["declstatus"].ToString2());
                    dr["inspstatus"] = switchValue("inspstatus", dr["inspstatus"].ToString2());
                    if (string.IsNullOrEmpty(dr["divideno"].ToString2())) dr["divideno"] = "";
                    if (string.IsNullOrEmpty(dr["logisticsstatus"].ToString2())) dr["logisticsstatus"] = "";
                    if (string.IsNullOrEmpty(dr["contractno"].ToString2())) dr["contractno"] = "";
                }
            }
            catch(Exception e)
            {
                LogHelper.Write("MyBusiness_QueryData:" + e.Message);
            }
           
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string json = JsonConvert.SerializeObject(dt, iso);
            return json;
        }

        private static string switchValue(string kind,string str)
        {
            string value = "";
            switch(kind)
            {
                case "declstatus":
                    switch (str)
                    {
                        case "130":
                            value = "报关完结";
                            break;
                        case "140":
                            value = "报关资料整理";
                            break;
                        case "150":
                            value = "现场报关";
                            break;
                        case "160":
                            value = "海关放行";
                            break;
                    }
                    break;
                case "inspstatus":
                    switch (str)
                    {
                        case "130":
                            value = "报检完结";
                            break;
                        case "140":
                            value = "报检资料整理";
                            break;
                        case "150":
                            value = "现场报检";
                            break;
                        case "160":
                            value = "国检放行";
                            break;
                    }
                    break;
            }
            return value;
        }
    }
}