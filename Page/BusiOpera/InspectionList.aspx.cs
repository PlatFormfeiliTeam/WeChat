﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
    public partial class InspectionList : System.Web.UI.Page
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
                    LogHelper.Write("第9步：" + userInfo.OpenID);
                    WGUserEn wuser = UserModel.getWeChatUser(userInfo.OpenID);
                    if (wuser == null || string.IsNullOrEmpty(wuser.GwyUserName))
                    {//账号未关联，跳转至登录界面
                        LogHelper.Write("第10步：" + userInfo.OpenID);
                        System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=DeclareList");
                    }
                    else if (wuser.IsReceiver != 1)
                    {//不是接单单位，无此权限
                        LogHelper.Write("第11步：" + userInfo.OpenID);
                        System.Web.HttpContext.Current.Response.Redirect(@"../WarnPage.aspx");
                    }
                    else
                    {//不需登录，保存当前用户
                        HttpContext.Current.Session["user"] = wuser;
                    }
                    LogHelper.Write("第12步：" + wuser.WCOpenID);
                }
                else
                {//获取授权失败，也跳转至登录页面
                    System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=DeclareList");
                }
            }
            else if (user.IsReceiver != 1)
            {//不是接单单位，无此权限
                System.Web.HttpContext.Current.Response.Redirect(@"../WarnPage.aspx");
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
        public static string BindList(string reptime_s, string reptime_e, string inspcode, string modifyflag, string busitype, string ischeck
            , string ispass, string lawflag, string isneedclearance, string busiunit, string contractno, string ordercode, string cusno, string divideno
            , string customareacode, string approvalcode, string submittime_s, string submittime_e, string sitepasstime_s, string sitepasstime_e
            , int start, int itemsPerLoad)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
            {
                return "[]";
            }
            DataSet ds = Inspection.getInspectionInfo(reptime_s, reptime_e, inspcode, getcode("modifyflag", modifyflag), busitype, ischeck
                , ispass, lawflag, isneedclearance, busiunit, contractno, ordercode, cusno, divideno
                , customareacode, approvalcode, submittime_s, submittime_e, sitepasstime_s, sitepasstime_e
                , start, itemsPerLoad, user.CustomerCode);

            //DataSet ds = Inspection.getInspectionInfo(reptime_s, reptime_e, inspcode, getcode("modifyflag", modifyflag), busitype, ischeck
            //    , ispass, lawflag, isneedclearance, busiunit, contractno, ordercode, cusno, divideno
            //    , customareacode, approvalcode, submittime_s, submittime_e, sitepasstime_s, sitepasstime_e
            //    , start, itemsPerLoad, "KSJSBGYXGS");

            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            var json = "[{\"data\":" + JsonConvert.SerializeObject(ds.Tables[0], iso) + ",\"sum\":" + ds.Tables[1].Rows[0][0] + "}]";
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

        //关联报检单
        [WebMethod]
        public static string AssCon(string preinspcode)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            DataTable dt = Inspection.AssCon(preinspcode);

            var json = "[]";
            if (dt != null)
            {
                json = JsonConvert.SerializeObject(dt, iso);
            }
            return json;
        }

        //删改单维护
        [WebMethod]
        public static string ModifySave(string preinspcode, int modifyflag)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
            {
                return "[]";
            }
            bool bf = Inspection.saveModifyFlag(preinspcode, modifyflag, user);
            var jsonstr = "false";
            if (bf) { jsonstr = "success"; }
            return jsonstr;
        }

        //报检单调阅
        [WebMethod]
        public static string FileConsult(string preinspcode)
        {
            DataTable dt = Inspection.FileConsult(preinspcode);
            string json = "";
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string downfile = HttpRuntime.AppDomainAppPath + @"\TempFile\tempPdf\" + dr["inspcode"] + ".pdf";
                    bool bf = true;
                    if (!File.Exists(downfile))
                    {
                        //PDF获取文件
                        System.Uri Uri = new Uri("ftp://" + ConfigurationManager.AppSettings["FTPServer"] + ":" + ConfigurationManager.AppSettings["FTPPortNO"]);
                        string UserName = ConfigurationManager.AppSettings["FTPUserName"];
                        string Password = ConfigurationManager.AppSettings["FTPPassword"];
                        FtpHelper ftp = new FtpHelper(Uri, UserName, Password);
                        bf = ftp.DownloadFile("/" + dr["filename"].ToString2() + "", downfile);
                    }
                    if (bf)
                    {
                        //pdf转picture
                        string picPath = HttpRuntime.AppDomainAppPath + @"\TempFile\tempPic\";
                        json = ConvertPDF.pdfToPic(downfile, picPath, dr["inspcode"].ToString2());
                    }
                }
            }
            return json;
        }

        //查验调阅
        [WebMethod]
        public static string picfileconsult(string preinspcode)
        {
            DataTable dt = Inspection.picfileconsult(preinspcode);
            var json = JsonConvert.SerializeObject(dt);
            return json;
        }


    }
}