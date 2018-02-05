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

namespace WeChat.Page.MyBusiness
{
    public partial class SubscribeList_decl : System.Web.UI.Page
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
                    System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SubscribeList_decl");
                }
                else if (wuser.IsCustomer != 1 && wuser.IsCompany != 1)
                {//不是接单单位，无此权限
                    LogHelper.Write("第11步：" + userInfo.OpenID);
                    System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SubscribeList_decl");
                }
                else
                {//不需登录，保存当前用户
                    HttpContext.Current.Session["user"] = wuser;
                }
                LogHelper.Write("第12步：" + wuser.WCOpenID);
            }
            else
            {//获取授权失败，也跳转至登录页面
                System.Web.HttpContext.Current.Response.Redirect(@"../Login.aspx?openid=" + userInfo.OpenID + "&nickname=" + userInfo.NickName + "&transferurl=SubscribeList_decl");
            }
        }
        [WebMethod]
        public static string QuerySubscribeInfo(string starttime, string endtime, string istigger, int pagesize, int lastnum, string declarationcode)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || user.GwyUserID <= 0)
                return "";
            DataTable infodt = SubscribeModel.getSubscribeInfo_Decl(starttime, endtime, istigger, pagesize, lastnum, declarationcode, user.GwyUserID);
            if (infodt == null || infodt.Rows.Count == 0)
                return "";
            DataTable resultdt=infodt.Clone();
            try
            {
                foreach (DataRow dr in infodt.DefaultView.ToTable(true, "declarationcode").Rows)
                {
                    //给物流状态赋值
                    DataRow[] resultrow = infodt.Select("declarationcode='" + dr["declarationcode"] + "' and substype='报关状态' and TRIGGERSTATUS=0", " statusvalue");//找到未触发的最小状态
                    if (resultrow.Length > 0)
                    {
                        resultrow[0]["substatus"] = resultrow[0]["substatus"] + "/未触发";
                    }
                    else
                    {//否则找触发里最大的状态
                        resultrow = infodt.Select("declarationcode='" + dr["declarationcode"] + "' and substype='报关状态' and (TRIGGERSTATUS=1 or TRIGGERSTATUS=2)", " statusvalue desc");
                        if (resultrow.Length > 0)
                        {
                            resultrow[0]["substatus"] = resultrow[0]["substatus"] + "/已触发";
                        }
                    }
                    resultdt.Rows.Add(resultrow[0].ItemArray);
                }
                IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                string json = JsonConvert.SerializeObject(resultdt, iso);
                return json;
            }
            catch (Exception e)
            {
                LogHelper.Write("MyBusiness_QueryData:" + e.Message);
                return "";
            }
            
        }

       
    }
}