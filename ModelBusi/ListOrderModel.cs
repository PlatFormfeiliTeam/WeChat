using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Entity.Enum;
using WeChat.Common;

namespace WeChat.ModelBusi
{
    public class ListOrderModel
    {
        /// <summary>
        /// 查询业务信息
        /// </summary>
        /// <param name="declstatus"></param>
        /// <param name="inspstatus"></param>
        /// <param name="inout"></param>
        /// <param name="busitype"></param>
        /// <param name="customs"></param>
        /// <param name="sitedeclare"></param>
        /// <param name="logisticsstatus"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable getOrder(string declstatus,string inspstatus,string inout,string busitype,string customs,string sitedeclare,string logisticsstatus,
            string starttime, string endtime, int itemsperLoad, int lastIndex)
        {
            using(DBSession db=new DBSession())
            {
                DataTable dt = new DataTable();
                string sql = @"with newtab as
                            ( select * from ( select rownum as rown ,tab.* from 
                            (select code,submittime,busiunitname,busitype,cusno,divideno,repwayid,contractno,goodsnum,goodsgw,to_char(ischeck) as ischeck,
                            to_char(checkpic) as checkpic,to_char(declstatus) as declstatus,to_char(inspstatus) as inspstatus,to_char(lawflag) as lawflag,
                            to_char(inspcheck) as inspcheck,logisticsstatus,logisticsname,customareacode 
                            from list_order {0} order by submittime) tab where rownum<={1}) t1 where t1.rown>{2}）
                            select nt.*,sb.name as busitypename,sr.name as repwayname from newtab nt 
                            left join cusdoc.sys_busitype sb on nt.busitype=sb.code 
                            left join cusdoc.sys_repway sr on nt.repwayid=sr.code";
                string strWhere = " where 1=1";
                strWhere += switchValue("报关状态", declstatus);
                strWhere += switchValue("报检状态", inspstatus);
                strWhere += switchValue("进出口", inout);
                strWhere += switchValue("业务类型", busitype);
                strWhere += switchValue("现场申报", sitedeclare);
                strWhere += switchValue("物流状态", logisticsstatus);
                if(!string.IsNullOrEmpty(customs.Trim2()))
                {
                    strWhere += " and customareacode='" + customs + "'";
                }
                if(!string.IsNullOrEmpty(starttime))
                {
                    strWhere += " and submittime>to_date('" + starttime + "','yyyy-mm-dd hh24:mi:ss')";
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    strWhere += " and submittime<to_date('" + endtime + " 23:59:59','yyyy-mm-dd hh24:mi:ss')";
                }
                sql = string.Format(sql, strWhere, lastIndex + itemsperLoad, lastIndex);
                dt = db.QuerySignle(sql);
                return dt;
            }
        }

        private string switchValue(string kind, string str)
        {
            str = str.Trim2();
            kind = kind.Trim2();
            string strWhere = "";
            switch(kind)
            {
                case "报关状态":
                    switch(str)
                    {
                        case "全部":
                            strWhere += " and declstatus >= " + (int)DeclStatusEnum.DeclOver;
                            break;
                        case "申报完结":
                            strWhere += " and declstatus = " + (int)DeclStatusEnum.DeclOver;
                            break;
                        case "现场报关":
                            strWhere += " and declstatus = " + (int)DeclStatusEnum.SiteApply;
                            break;
                        case "现场放行":
                            strWhere += " and declstatus = " + (int)DeclStatusEnum.SitePass;
                            break;
                    }
                    break;
                case "报检状态":
                    switch (str)
                    {
                        case "全部":
                            strWhere += " and inspstatus >= " + (int)DeclStatusEnum.DeclOver;
                            break;
                        case "申报完结":
                            strWhere += " and inspstatus = " + (int)DeclStatusEnum.DeclOver;
                            break;
                        case "现场报检":
                            strWhere += " and inspstatus = " + (int)DeclStatusEnum.SiteApply;
                            break;
                        case "现场放行":
                            strWhere += " and inspstatus = " + (int)DeclStatusEnum.SitePass;
                            break;
                    }
                    break;
                case "进出口":
                    switch (str)
                    {
                        case "全部":
                            break;
                        case "进口":
                            strWhere += " and busitype in ('11','21','31','41','51')";
                            break;
                        case "出口":
                            strWhere += " and busitype in ('10','20','30','40','50')";
                            break;
                    }
                    break;
                case "业务类型":
                    switch (str)
                    {
                        case "全部（不含国内）":
                            strWhere += " and busitype in ('10','20','30','50','11','21','31','51')";
                            break;
                        case "国内业务":
                            strWhere += " and busitype in ('40','41')";
                            break;
                        case "特殊区域":
                            strWhere += " and busitype in ('50','51')";
                            break;
                        case "空运业务":
                            strWhere += " and busitype in ('10','11')";
                            break;
                        case "陆运业务":
                            strWhere += " and busitype in ('30','31')";
                            break;
                    }
                    break;
                case "现场申报":
                    switch (str)
                    {
                        case "全部":
                            break;
                        case "需现场申报":
                            strWhere += " and ischeck=1 or inspcheck=1";
                            break;
                    }
                    break;
                case "物流状态":
                    switch (str)
                    {
                        case "全部":
                            break;
                        case "待抽单":
                            strWhere += " and logisticsstatus=" + (int)LogisticsStatusEnum.DrawBillAccepted;
                            break;
                        case "抽单完成":
                            strWhere += " and logisticsstatus=" + (int)LogisticsStatusEnum.DrawBillFinished;
                            break;
                        case "未派车":
                            strWhere += " and logisticsstatus<" + (int)LogisticsStatusEnum.SendCar;
                            break;
                        case "已派车":
                            strWhere += " and logisticsstatus=" + (int)LogisticsStatusEnum.SendCar;
                            break;
                        case "未运抵":
                            strWhere += " and logisticsstatus<" + (int)LogisticsStatusEnum.TransportFinished;
                            break;
                        case "运输完成":
                            strWhere += " and logisticsstatus=" + (int)LogisticsStatusEnum.TransportFinished;
                            break;
                        case "未送货":
                            strWhere += " and logisticsstatus<" + (int)LogisticsStatusEnum.DeliveryFinished;
                            break;
                        case "送货完成":
                            strWhere += " and logisticsstatus=" + (int)LogisticsStatusEnum.DeliveryFinished;
                            break;
                    }
                    break;
            }
            return strWhere;
        }
    }
}