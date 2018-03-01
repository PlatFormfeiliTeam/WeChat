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
    public partial class SignOut : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static string UserSignOut(string name, string pwd, string customer)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
                return "{'flag':'false','url':'解绑失败！您当前尚未登录'}";
            if(UserModel.DeleteUser(user.WCOpenID))
            {
                HttpContext.Current.Session["user"] = null;
                return "{'flag':'true','url':'解绑成功'}";
            }
            else
            {
                return "{'flag':'false','url':'解绑失败！当前账号未绑定'}";
            }
            
        }

        [WebMethod]
        public static string getUserInfo()
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || string.IsNullOrEmpty(user.CustomerCode))
                return "{'flag':'false','url':'您当前尚未登录'}";
            else
                return "{'flag':'true','code':'" + user.GwyUserCode + "','pwd':'000000','customercode':'" + user.CustomerCode + "'}";
        }
    }
}