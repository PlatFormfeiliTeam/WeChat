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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string UserLogin(string name, string pwd, string customer, string wcopenid, string wcnickname, string transferurl)
        {
            wcnickname = HttpUtility.UrlDecode(wcnickname);
            LogHelper.Write(wcopenid + "----" + wcnickname + "----" + transferurl);
            if (UserModel.UserExsit(name, wcopenid, wcnickname))
            {
                return "{'flag':'false','url':'登录失败！该账号已经绑定其它微信，请先解绑'}";
            }

            WGUserEn user = UserModel.Login(name, pwd, customer,wcopenid,wcnickname);
            if (user != null && !string.IsNullOrEmpty(user.GwyUserCode))
            {
                //1、判断权限
                if (transferurl == "DeclareList" || transferurl == "SiteDeclareList" || transferurl == "SiteInspectionList")
                {
                    if (user.IsReceiver != 1)
                    {
                        return "{'flag':'false','url':'登录失败！该账号不属于接单单位'}";
                    }
                }
                if (transferurl == "MyBusiness" || transferurl == "MyDeclareList" || transferurl == "MyInspectionList")
                {
                    if (user.IsCompany != 1 && user.IsCustomer != 1)
                    {
                        return "{'flag':'false','url':'登录失败！该账号不属于生产企业或委托单位'}";
                    }
                }
                //2、保存当前账号
                UserModel.SaveUser(user);
                //3、页面跳转
                switch (transferurl)
                {
                    case "DeclareList":
                        transferurl = "http://gwy.jishiks.com/Page/BusiOpera/DeclareList.aspx";
                        break;
                    case "SiteDeclareList":
                        transferurl = "http://gwy.jishiks.com/Page/BusiOpera/SiteDeclareList.aspx";
                        break;
                    case "SiteInspectionList":
                        transferurl = "http://gwy.jishiks.com/Page/MyBusiness/SiteInspectionList.aspx";
                        break;
                    case "MyBusiness":
                        transferurl = "http://gwy.jishiks.com/Page/MyBusiness/MyBusiness.aspx";
                        break;
                    case "MyDeclareList":
                        transferurl = "http://gwy.jishiks.com/Page/MyBusiness/MyDeclareList.aspx";
                        break;
                    case "MyInspectionList":
                        transferurl = "http://gwy.jishiks.com/Page/MyBusiness/MyInspectionList.aspx";
                        break;

                    
                }
                return "{'flag':'true','url':'" + transferurl + "'}";
            }
            else
            {
                return "{'flag':'false','url':'登录失败！请重新核对账号'}";
            }
        }
    }
}