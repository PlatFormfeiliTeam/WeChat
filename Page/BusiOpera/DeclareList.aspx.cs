using Newtonsoft.Json;
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

namespace WeChat.Page.BusiOpera
{
    public partial class DeclareList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string BindList(string declcode, string startdate, string enddate, int start, int itemsPerLoad)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            DataTable dt = Declare.getDeclareInfo(declcode, startdate, enddate, start, itemsPerLoad);
            var json = JsonConvert.SerializeObject(dt, iso);
            return json;
        }

        //[WebMethod]
        //public static string BindListDetail(string id)
        //{
        //    IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
        //    iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        //    DataTable dt = HsCode.getHsCodeInfoDetail(id);
        //    var json = JsonConvert.SerializeObject(dt, iso);
        //    return json;
        //}


    }
}