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
    public partial class SiteDeclareList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //查询绑定
        [WebMethod]
        public static string BindList(string inout_type, string issiterep, string busitype, string ispass, string startdate, string enddate
            , string radiotype, string morecon, int start, int itemsPerLoad)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();//序列化JSON对象时,日期的处理格式 
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            DataTable dt = SiteDeclare.getSiteDeclareInfo(inout_type, issiterep, busitype, ispass, startdate, enddate, radiotype, morecon, start, itemsPerLoad);
            var json = JsonConvert.SerializeObject(dt, iso);
            return json;
        }

        //报关单交接
        [WebMethod]
        public static string Handover(string ordercode)
        {
            return SiteDeclare.Handover(ordercode);
        }

        //报关单详细
        [WebMethod]
        public static string Detail(string ordercode)
        {
            IsoDateTimeConverter iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyyMMdd HH:mm:ss";

            DataSet ds = SiteDeclare.Detail(ordercode);
            var json_order = JsonConvert.SerializeObject(ds.Tables[0], iso);
            var json_decl = JsonConvert.SerializeObject(ds.Tables[1]);
            return "[{\"json_order\":" + json_order + ",\"json_decl\":" + json_decl + "}]";
        }

        //报关单放行
        [WebMethod]
        public static string Pass(string ordercode)
        {
            return SiteDeclare.Pass(ordercode);
        }

        //查验标志 绑定集装箱数据
        [WebMethod]
        public static string declcontainerdata(string ordercode)
        {
            DataTable dt = SiteDeclare.getdeclcontainerdata(ordercode);
            var json = JsonConvert.SerializeObject(dt);
            return json;
        }

    }
}