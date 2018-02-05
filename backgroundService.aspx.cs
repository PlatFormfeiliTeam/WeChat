using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat.ModelWeChat;

namespace WeChat
{
    public partial class backgroundService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TemplateModel.ExcuteSubcirbePush_single();
            Response.Write("<script>window.close();</script>");
        }
    }
}