using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat.Common;
using WeChat.Entity;
using WeChat.ModelBusi;

namespace WeChat.Page
{
    public partial class Login : System.Web.UI.Page
    {
        private static string wcopenid = "", wcnickname = "", transferUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            LogHelper.Write("进入Login页面:" + request["openid"]);
            wcopenid = request["openid"];
            wcnickname = request["nickname"];
            transferUrl = request["transferurl"];
        }

        [WebMethod]
        public static string UserLogin(string name,string pwd,string customer)
        {
            WGUserEn user = UserModel.Login(name, pwd, customer,wcopenid,wcnickname);
            if (user != null && !string.IsNullOrEmpty(user.GwyUserCode))
            {
                //1、判断权限
                if (transferUrl == "DeclareList" || transferUrl == "SiteDeclareList" || transferUrl == "SiteInspectionList")
                {
                    if (user.IsCompany != 1 && user.IsCustomer != 1)
                    {
                        return "{'flag':'false','url':'登录失败！该账号不属于生产企业或委托单位'}";
                    }
                }
                if (transferUrl == "MyBusiness" || transferUrl == "SubscribeList_busi" || transferUrl == "SubscribeList_decl")
                {
                    if (user.IsReceiver != 1)
                    {
                        return "{'flag':'false','url':'登录失败！该账号不属于接单单位'}";
                    }
                }
                //2、保存当前账号
                UserModel.SaveUser(user);
                //3、页面跳转
                switch(transferUrl)
                {
                    case "DeclareList":
                        transferUrl = "http://gwy.jishiks.com/Page/BusiOpera/DeclareList.aspx";
                        break;
                    case "SiteDeclareList":
                        transferUrl = "http://gwy.jishiks.com/Page/BusiOpera/SiteDeclareList.aspx";
                        break;
                    case "SiteInspectionList":
                        transferUrl = "http://gwy.jishiks.com/Page/MyBusiness/SiteInspectionList.aspx";
                        break;
                    case "MyBusiness":
                        transferUrl = "http://gwy.jishiks.com/Page/MyBusiness/MyBusiness.aspx";
                        break;
                    case "SubscribeList_busi":
                        transferUrl = "http://gwy.jishiks.com/Page/MyBusiness/SubscribeList_busi.aspx";
                        break;
                    case "SubscribeList_decl":
                        transferUrl = "http://gwy.jishiks.com/Page/MyBusiness/SubscribeList_decl.aspx";
                        break;

                    
                }
                return "{'flag':'true','url':'" + transferUrl + "'}";
            }
            else
            {
                return "{'flag':'false','url':'登录失败！请重新核对账号'}";
            }
        }
    }
}