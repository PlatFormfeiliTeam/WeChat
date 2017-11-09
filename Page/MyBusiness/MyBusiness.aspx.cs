using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
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
        /// <summary>
        /// 获取业务信息
        /// </summary>
        /// <param name="declstatus"></param>
        /// <param name="inspstatus"></param>
        /// <param name="inout"></param>
        /// <param name="busitype"></param>
        /// <param name="customs"></param>
        /// <param name="sitedeclare"></param>
        /// <param name="logisticsstatus"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="itemsperload"></param>
        /// <param name="lastindex"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [WebMethod]
        public static string QueryOrderDetail(string code)
        {
            try
            {
                ListOrderModel orderModel = new ListOrderModel();
                DataSet ds = orderModel.getOrderDetail(code);
                ds.Tables[0].Rows[0]["busitype"] = switchValue("busitype", ds.Tables[0].Rows[0]["busitype"].ToString2());
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dr["modifyflag"] = switchValue("modifyflag", dr["modifyflag"].ToString2());
                }
                IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                string json = JsonConvert.SerializeObject(ds, iso);
                return json;
            }
            catch (Exception e)
            {
                LogHelper.Write("MyBusiness_QueryOrderDetail异常:" + e.Message);
                return "";
            }

        }
        /// <summary>
        /// 订阅状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        [WebMethod]
        public static string SubscribeStatus(string status,string orderCode)
        {
            return "";
        }
        /// <summary>
        /// 调阅报关单
        /// </summary>
        /// <param name="declCode"></param>
        /// 
        [WebMethod]
        public static string GetDeclPdf(string orderCode)
        {
            ListOrderModel orderModel = new ListOrderModel();
            DataTable dt = orderModel.getDeclPath(orderCode);
            string json = "{'path':[";
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string output = HttpRuntime.AppDomainAppPath + "\\TempFile\\tempPdf\\" + dr["declcode"] + ".pdf";
                    if (!File.Exists(output))
                    {
                        //PDF获取文件
                        System.Uri Uri = new Uri("ftp://" + ConfigurationManager.AppSettings["FTPServer"] + ":" + ConfigurationManager.AppSettings["FTPPortNO"]);
                        string UserName = ConfigurationManager.AppSettings["FTPUserName"];
                        string Password = ConfigurationManager.AppSettings["FTPPassword"];
                        FtpHelper ftp = new FtpHelper(Uri, UserName, Password);
                        bool file = ftp.DownloadFile("/" + dr["filename"].ToString2() + "", output);
                        
                    }
                    //pdf转picture
                    string picPath = HttpRuntime.AppDomainAppPath + "\\TempFile\\tempPic\\" ;
                    json += ConvertPDF.pdfToPic(output, picPath, dr["declcode"].ToString2());
                }
            }
            if (json.Length > 9)
            {
                json = json.Substring(0, json.Length - 1);
            }
            json += "]}";
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
                case "modifyflag":
                    switch(str)
                    {
                        case "":
                        case "null":
                        case "0":
                            value = "正常";
                            break;
                        case "1":
                            value = "删单";
                            break;
                        case "2":
                            value = "改单";
                            break;
                    }
                    break;
                case "busitype":
                    switch (str)
                    {
                        case "10":
                            value = "空出";
                            break;
                        case "11":
                            value = "空进";
                            break;
                        case "20":
                            value = "海出";
                            break;
                        case "22":
                            value = "海进";
                            break;
                        case "30":
                            value = "陆出";
                            break;
                        case "31":
                            value = "陆进";
                            break;
                        case "40":
                            value = "国内出";
                            break;
                        case "41":
                            value = "国内进";
                            break;
                        case "50":
                            value = "特殊出";
                            break;
                        case "51":
                            value = "特殊进";
                            break;
                    }
                    break;
            }
            return value;
        }
    }
}