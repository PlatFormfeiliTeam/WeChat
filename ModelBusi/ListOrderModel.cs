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
            DataTable dt = new DataTable();
            try
            {
                using (DBSession db = new DBSession())
                {
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
                    if (!string.IsNullOrEmpty(customs.Trim2()))
                    {
                        strWhere += " and customareacode='" + customs + "'";
                    }
                    if (!string.IsNullOrEmpty(starttime))
                    {
                        strWhere += " and submittime>to_date('" + starttime + "','yyyy-mm-dd hh24:mi:ss')";
                    }
                    if (!string.IsNullOrEmpty(endtime))
                    {
                        strWhere += " and submittime<to_date('" + endtime + " 23:59:59','yyyy-mm-dd hh24:mi:ss')";
                    }
                    sql = string.Format(sql, strWhere, lastIndex + itemsperLoad, lastIndex);
                    dt = db.QuerySignle(sql);
                   
                }
            }
            catch(Exception ex)
            {
                LogHelper.Write("ListOrderModel_getOrder异常：" + ex.Message);
            }
            return dt;
        }


        public DataTable getDeclPath(string orderCode)
        {
            using(DBSession db=new DBSession())
            {
                string sql = "select filename,declcode from list_attachment where ordercode='" + orderCode + "' and filetype=61 order by declcode";
                DataTable dt = db.QuerySignle(sql);
                return dt;
            }
            
        }
        public DataSet getOrderDetail(string code)
        {
            using(DBSession db=new DBSession())
            {
                DataSet ds = new DataSet();
                //业务信息
                string sql = @"select lo.code,lo.totalno,lo.divideno,lo.entrusttype,lo.busitype,lo.submittime,lo.submitusername,lo.moendtime,lo.moendname,lo.coendtime,lo.coendname,
lo.preendtime,lo.preendname,lo.rependtime,lo.rependname,lo.handoverusername,lo.handovertime,lo.siteapplytime,lo.siteapplyusername,lo.sitepasstime,lo.sitepassusername,
lo.inspmoendtime,lo.inspmoendname,lo.inspcoendtime,lo.inspcoendname,lo.insppreendtime,lo.insppreendname,lo.insprependtime,lo.insprependname,
lo.insphandoverusername,lo.insphandovertime,lo.inspsiteapplytime,lo.inspsiteapplyusername,lo.inspsitepasstime,lo.inspsitepassusername
 from list_order lo where lo.code='" + code + "'";
                DataTable dt1 = db.QuerySignle(sql);
                dt1.TableName = "OrderTable";
                ds.Tables.Add(dt1);
                //报关单信息
                sql = @" select ld.code,ld.declarationcode,ld.goodsnum,ld.goodsgw,ld.tradecode,ld.transname,to_char(ld.modifyflag) as modifyflag,ld.customsstatus,
cbd.name as tradename from list_declaration ld left join cusdoc.base_decltradeway cbd on ld.tradecode=cbd.code
where ld.ordercode='" + code + "'";
                DataTable dt2 = db.QuerySignle(sql);
                dt2.TableName = "DeclTable";
                ds.Tables.Add(dt2);
                //报检单信息
                if (dt1.Rows[0]["entrusttype"].ToString2() == "02" || dt1.Rows[0]["entrusttype"].ToString2() == "03")
                {
                    sql = "select li.approvalcode,li.inspectioncode,li.clearancecode,li.modifyflag,li.inspstatus from list_inspection li where li.ordercode='" + code + "'";
                    DataTable dt3 = db.QuerySignle(sql);
                    dt3.TableName = "InspTable";
                    ds.Tables.Add(dt3);
                }
                //物流信息
                if (!string.IsNullOrEmpty(dt1.Rows[0]["totalno"].ToString2()) && !string.IsNullOrEmpty(dt1.Rows[0]["divideno"].ToString2()))
                {
                    sql = @"select ll.totalno,ll.divideno,ll.operater,ll.operate_type,ll.operate_result,ll.operate_date from list_logisticsstatus ll 
where ll.totalno='{0}' and ll.divideno='{1}' order by ll.operate_type,ll.operate_date";
                    DataTable dt4 = db.QuerySignle(string.Format(sql, dt1.Rows[0]["totalno"], dt1.Rows[0]["divideno"]));
                    dt4.TableName = "LogisticsTable";
                    ds.Tables.Add(dt4);
                }
                return ds;
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
                        //yangyang.zhao
                        case "":
                            strWhere += " and declstatus >= " + (int)DeclStatusEnum.DeclOver;
                            break;
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
                        //yangyang.zhao
                        case "":
                            strWhere += " and inspstatus >= " + (int)DeclStatusEnum.DeclOver;
                            break;
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
                        //yangyang.zhao
                        case "":
                            break;
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
                        //yangyang.zhao
                        case "":
                            strWhere += " and busitype in ('10','20','30','50','11','21','31','51')";
                            break;
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
                        case "海运业务":
                            strWhere += " and busitype in ('20','21')";
                            break;
                    }
                    break;
                case "现场申报":
                    switch (str)
                    {
                        //yangyang.zhao
                        case "":
                            break;
                        case "全部":
                            break;
                        case "需现场申报":
                            strWhere += " and HANDOVERTIME is not null";
                            break;
                    }
                    break;
                case "物流状态":
                    switch (str)
                    {
                        //yangyang.zhao
                        case "":
                            break;
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