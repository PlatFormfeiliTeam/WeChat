using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json.Converters;
using WeChat.ModelBusi;
using System.Data;
using Newtonsoft.Json;

namespace WeChat.Page.FrequentSearch
{
    public partial class HsCodeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string BindList(string hscode, string commodityname, int start, int itemsPerLoad)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            DataTable dt = HsCode.getHsCodeInfo(hscode, commodityname,start,itemsPerLoad);
            var json = JsonConvert.SerializeObject(dt, iso);
            return json;
        }

    }
}