using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Common
{
    public class SwitchHelper
    {
        public static string switchValue(string kind, string str)
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
                case "modifyflag":
                    switch (str)
                    {
                        case "":
                        case "null":
                        case "0":
                            value = "正常";
                            break;
                        case "1":
                            value = "删单";
                            break;
                        case "2":
                            value = "改单";
                            break;
                    }
                    break;
                case "busitype":
                    switch (str)
                    {
                        case "10":
                            value = "空出";
                            break;
                        case "11":
                            value = "空进";
                            break;
                        case "20":
                            value = "海出";
                            break;
                        case "22":
                            value = "海进";
                            break;
                        case "30":
                            value = "陆出";
                            break;
                        case "31":
                            value = "陆进";
                            break;
                        case "40":
                            value = "国内出";
                            break;
                        case "41":
                            value = "国内进";
                            break;
                        case "50":
                            value = "特殊出";
                            break;
                        case "51":
                            value = "特殊进";
                            break;
                    }
                    break;
                case "报关状态":
                    switch (str)
                    {
                        case "申报完成":
                            value = "1010";
                            break;
                        case "已放行":
                            value = "1020";
                            break;
                        case "已结关":
                            value = "1030";
                            break;
                        case "改单完成":
                            value = "1040";
                            break;
                        case "删单完成":
                            value = "1050";
                            break;
                        default:
                            value = "0";
                            break;
                    }
                    break;
                case "物流状态":
                    switch (str)
                    {
                        case "抽单完成":
                            value = "10";
                            break;
                        case "已派车":
                            value = "45";
                            break;
                        case "运输完成":
                            value = "70";
                            break;
                        case "送货完成":
                            value = "80";
                            break;
                        default :
                            value = "0";
                                break;
                    }
                    break;
            }
            return value;
        }
    }
}