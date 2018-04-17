using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WeChat.ModelWeChat;

namespace WeChat
{
    /// <summary>
    /// BGService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class BGService : System.Web.Services.WebService
    {

        [WebMethod]
        public void TemplateSend()
        {
            TemplateModel.ExcuteSubcirbePush_single();
        }

        [WebMethod]
        public void LoginExceptionSend()
        {
            TemplateModel.ExcuteLoginExceptionPush_single();
        }

        [WebMethod]
        public string  GetAccessToken()
        {
            return TokenModel.AccessToken;
        }
    }
}
