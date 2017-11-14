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

namespace WeChat.Page.MyBusiness
{
    public partial class SubscribeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string QuerySubscribeInfo(string starttime,string endtime,string istigger,int pagesize,int lastnum)
        {
            DataTable infodt = SubscribeModel.getSubscribeInfo(starttime, endtime, istigger, pagesize, lastnum);
            if (infodt == null || infodt.Rows.Count == 0)
                return "";
            DataTable resultdt=infodt.Clone();
            try
            {
                foreach (DataRow dr in infodt.DefaultView.ToTable(true,"ordercode").Rows)
                {
                    //给物流状态赋值
                    DataRow[] resultrow = infodt.Select("ordercode='" + dr["ordercode"] + "' and substype='物流状态' and TRIGGERSTATUS=0", " statusvalue");//找到未触发的最小状态
                    if (resultrow.Length > 0)
                    {
                        resultrow[0]["sublogstatus"] = resultrow[0]["substatus"] + "/未触发";
                    }
                    else
                    {//否则找触发里最大的状态
                        resultrow = infodt.Select("ordercode='" + dr["ordercode"] + "' and substype='物流状态' and (TRIGGERSTATUS=1 or TRIGGERSTATUS=2)", " statusvalue desc");
                        if (resultrow.Length > 0)
                        {
                            resultrow[0]["sublogstatus"] = resultrow[0]["substatus"] + "/已触发";
                        }
                    }
                    //给报关状态赋值
                    DataRow[] declrow = infodt.Select("ordercode='" + dr["ordercode"] + "' and substype='报关状态' and TRIGGERSTATUS=0", " statusvalue");//找到未触发的最小状态
                    if (resultrow.Length > 0)//存在物流状态，则要把stauts（物流状态）清空，用来保存报关状态
                    {
                        resultrow[0]["substatus"] = "";
                    }
                    if (declrow.Length > 0)
                    {
                        if (resultrow.Length == 0)
                        {
                            resultrow = declrow;//如果前面订阅信息都没有，则以现在为准
                            resultrow[0]["sublogstatus"] = "";
                        }
                        resultrow[0]["substatus"] = declrow[0]["substatus"] + "/未触发";
                    }
                    else
                    {//否则找触发里最大的状态
                        declrow = infodt.Select("ordercode='" + dr["ordercode"] + "' and substype='报关状态' and (TRIGGERSTATUS=1 or TRIGGERSTATUS=2)", " statusvalue desc");
                        if (declrow.Length > 0)
                        {
                            if (resultrow.Length == 0)
                            {
                                resultrow = declrow;//如果前面订阅信息都没有，则以现在为准
                                resultrow[0]["sublogstatus"] = "";
                            }
                            resultrow[0]["substatus"] = declrow[0]["substatus"] + "/已触发";
                        }
                    }
                    if (string.IsNullOrEmpty(resultrow[0]["divideno"].ToString2())) resultrow[0]["divideno"] = "";
                    if (string.IsNullOrEmpty(resultrow[0]["logisticsname"].ToString2())) resultrow[0]["logisticsname"] = "";
                    if (string.IsNullOrEmpty(resultrow[0]["contractno"].ToString2())) resultrow[0]["contractno"] = "";
                    resultrow[0]["declstatus"] = switchValue("declstatus", resultrow[0]["declstatus"].ToString2());
                    resultrow[0]["inspstatus"] = switchValue("inspstatus", resultrow[0]["inspstatus"].ToString2());
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

        private static string switchValue(string kind, string str)
        {
            string value = "";
            switch (kind)
            {
                case "declstatus":
                    switch (str)
                    {
                        case "130":
                            value = "报关完结";
                            break;
                        case "140":
                            value = "报关资料整理";
                            break;
                        case "150":
                            value = "现场报关";
                            break;
                        case "160":
                            value = "海关放行";
                            break;
                    }
                    break;
                case "inspstatus":
                    switch (str)
                    {
                        case "130":
                            value = "报检完结";
                            break;
                        case "140":
                            value = "报检资料整理";
                            break;
                        case "150":
                            value = "现场报检";
                            break;
                        case "160":
                            value = "国检放行";
                            break;
                    }
                    break;
            }
            return value;
        }
    }
}