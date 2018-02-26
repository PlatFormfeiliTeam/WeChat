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
    public partial class DeclSubscribeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        [WebMethod]
        public static string QuerySubscribeInfo(string starttime, string endtime, string istigger, int pagesize, int lastnum, string declarationcode)
        {
            WGUserEn user = (WGUserEn)HttpContext.Current.Session["user"];
            if (user == null || user.GwyUserID <= 0)
                return "";
            DataTable infodt = SubscribeModel.getSubscribeInfo_Decl(user.GwyUserID);
            //DataTable infodt = SubscribeModel.getSubscribeInfo_Decl(0);
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

        [WebMethod]
        public static bool DeleteSubscribeInfo(string cusno)
        {
            return SubscribeModel.deleteSubscribe(cusno);
        }
    }
}