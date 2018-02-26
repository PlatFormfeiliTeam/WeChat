using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat.Common;
using WeChat.Entity;
using WeChat.ModelBusi;
using WeChat.ModelWeChat;

namespace WeChat.Page.MyBusiness
{
    public partial class MyBusiness : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string data = Request["data"];
            WUserEn userInfo = PageShowQuan.GetShouQuanMessage();
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.OpenID))
            {//授权成功
                LogHelper.Write("第9步：" + userInfo.OpenID);
                WGUserEn wuser = UserModel.getWeChatUser(userInfo.OpenID);
                if (wuser == null || string.IsNullOrEmpty(wuser.GwyUserName))
                {//账号未关联，跳转至登录界面
                    LogHelper.Write("第10步：" + userInfo.OpenID);
                    System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=MyBusiness");
                }
                else if (wuser.IsCustomer != 1 && wuser.IsCompany != 1)
                {//不是接单单位，无此权限
                    LogHelper.Write("第11步：" + userInfo.OpenID);
                    System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=MyBusiness");
                }
                else
                {//不需登录，保存当前用户
                    HttpContext.Current.Session["user"] = wuser;
                }
                LogHelper.Write("第12步：" + wuser.WCOpenID);
            }
            else
            {//获取授权失败，也跳转至登录页面
                System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=MyBusiness");
            }
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
        public static string QueryData(string submittimestart, string submittimeend, string declarationcode, string customarea, string ispass, string ischeck, string busitype,
            string modifyflag, string auditflag, string ordercode, string cusno, string divideno, string contractno, string passtimestart, string passtimeend,
            int itemsperload, int lastindex)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
                return "";
            string customerCode = user.CustomerCode;
            string hsCode = user.HSCode;
            if (user.IsCompany != 1)//如果不是企业角色，不能查出其对应经营单位的订单
                hsCode = "";
            if (user.IsCustomer != 1)//如果不是委托单位角色，不能查出其对应委托单位的订单
                customerCode = "";
            ListOrderModel orderModel = new ListOrderModel();
            DataTable dt = orderModel.getOrder(submittimestart, submittimeend, declarationcode, customarea, ispass, ischeck, busitype, modifyflag, auditflag, ordercode,
                cusno, divideno, contractno, passtimestart, passtimeend, itemsperload, lastindex, customerCode, hsCode);
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
                ds.Tables[0].Rows[0]["busitype"] = SwitchHelper.switchValue("busitype", ds.Tables[0].Rows[0]["busitype"].ToString2());
                
                //ds.Tables[2].Rows[0]["modifyflag"] = SwitchHelper.switchValue("modifyflag", ds.Tables[2].Rows[0]["modifyflag"].ToString2());
                
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dr["modifyflag"] = SwitchHelper.switchValue("modifyflag", dr["modifyflag"].ToString2());
                }
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    dr["modifyflag"] = SwitchHelper.switchValue("modifyflag", dr["modifyflag"].ToString2());
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
        public static string SubscribeStatus(string type, string status, string cusno, string declarationcode)
        {
            try
            {
                //存放订阅的数据数组
                List<string> orderData = new List<string>();
                //订阅过的节点
                string orderedLine = "";
                string codetype = "0";
                if (status.Length > 0)
                    status = status.Substring(0, status.Length - 1);
                string[] st = status.Split(',');
                //判断是否订阅的信息是否已经触发
                if (type == "报关状态")
                {
                    DataTable dt = SubscribeModel.getDeclstatus(declarationcode);
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (dt.Rows.Count > 0 && SwitchHelper.switchValue(type, st[i]).ToInt32() <= SwitchHelper.switchValue(type, dt.Rows[0][0].ToString2()).ToInt32())
                        {
                            return "订阅失败，报关状态已过：" + st[i] + "";
                        }
                    }
                    codetype = "3";
                }
                else if(type=="物流状态")
                {
                    DataTable dt = SubscribeModel.getLogisticsstatus(cusno);
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (dt.Rows.Count > 0 && SwitchHelper.switchValue(type, st[i]).ToInt32() <= SwitchHelper.switchValue(type, dt.Rows[0][0].ToString2()).ToInt32())
                        {
                            return "订阅失败，物流状态已过：" + st[i] + "";
                        }
                    }
                    codetype = "2";
                }
                else if (type == "业务状态")
                {
                    DataTable dt = SubscribeModel.getOrderstatus(cusno);
                    for (int i = 0; i < st.Length; i++)
                    {
                        if (dt.Rows.Count > 0 && SwitchHelper.switchValue(type, st[i]).ToInt32() <= dt.Rows[0][0].ToString2().ToInt32())
                        {
                            return "订阅失败，业务状态已过：" + st[i] + "";
                        }
                    }
                    codetype = "1";
                }
                //防止重复订阅
                for (int i = 0; i < st.Length; i++)
                {
                    DataTable getTriggerStatus = SubscribeModel.GetTriggerstatus(cusno, st[i], type,declarationcode);
                    if (getTriggerStatus.Rows.Count > 0)
                    {
                        orderData.Add(st[i]);
                        //return st[i] + "已订阅请勿重复订阅";
                    }
                    
                }
                if (orderData.Count == 0)
                {
                    try
                    {
                        WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
                        SubscribeModel.insertSubscribe(type, st, cusno, declarationcode, user.GwyUserID, user.GwyUserName, user.WCOpenID, codetype);
                        return "订阅成功";
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    
                }
                else
                {
                    for (int i = 0; i < orderData.Count; i++)
                    {
                        orderedLine = orderedLine + orderData[i].ToString() + ",";
                    }
                    int length = orderedLine.Length;
                    return orderedLine.Substring(0, length - 1) + "已订阅请勿重复订阅";
                }

            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
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
            string json = "";
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string output = HttpRuntime.AppDomainAppPath + @"\TempFile\tempPdf\" + dr["declcode"] + ".pdf";
                    bool bf = true;
                    if (!File.Exists(output))
                    {
                        //PDF获取文件
                        System.Uri Uri = new Uri("ftp://" + ConfigurationManager.AppSettings["FTPServer"] + ":" + ConfigurationManager.AppSettings["FTPPortNO"]);
                        string UserName = ConfigurationManager.AppSettings["FTPUserName"];
                        string Password = ConfigurationManager.AppSettings["FTPPassword"];
                        FtpHelper ftp = new FtpHelper(Uri, UserName, Password);
                        bf = ftp.DownloadFile("/" + dr["filename"].ToString2() + "", output);
                        
                    }
                    if (bf)
                    {
                        //pdf转picture
                        string picPath = HttpRuntime.AppDomainAppPath + @"\TempFile\tempPic\";
                        json += ConvertPDF.pdfToPic(output, picPath, dr["declcode"].ToString2());
                    }
                }
            }
            return json;
        }
       

        




    }
}