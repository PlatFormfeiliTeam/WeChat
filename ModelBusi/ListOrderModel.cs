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
        public DataTable getOrder(string submittime_s, string submittime_e, string declarationcode, string customarea, string ispass, string ischeck, string busitype,
            string modifyflag, string auditflag, string busiunit, string ordercode, string cusno, string divideno, string contractno, string passtime_s, string passtime_e,
            int itemsperLoad, int lastIndex, string customerCode,string hscode, out string sum)
        {
            DataTable dt = new DataTable();
            sum = "0";
            try
            {
                using (DBSession db = new DBSession())
                {
                    string sql = @"with newtab as
                                    ( select * from ( select rownum as rown ,tab.* from 
                                            (select lo.code,lo.submittime,lo.busiunitname,lo.busitype,lo.cusno,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,
                                            to_char(lo.ischeck) as ischeck,to_char(lo.checkpic) as checkpic,to_char(lo.declstatus) as declstatus,to_char(lo.inspstatus) as inspstatus,
                                            to_char(lo.lawflag) as lawflag,to_char(lo.inspischeck) as inspischeck,lo.logisticsstatus,lo.logisticsname,lo.customareacode,'' as sum 
                                            from list_order lo left join list_declaration ld on lo.code=ld.ordercode  {0} order by lo.submittime desc) tab 
                                            where rownum<={1}) t1 
                                    where t1.rown>{2}）
                                    select nt.*,sb.name as busitypename,sr.name as repwayname from newtab nt 
                                    left join cusdoc.sys_busitype sb on nt.busitype=sb.code 
                                    left join cusdoc.sys_repway sr on nt.repwayid=sr.code";
                    string strWhere = " where submittime is not null";

                    if (!string.IsNullOrEmpty(busiunit)) { strWhere += " and (lo.BUSIUNITCODE like '%" + busiunit + "%' or lo.BUSIUNITNAME like '%" + busiunit + "%')"; }
                    if (!string.IsNullOrEmpty(submittime_s)) { strWhere += " and lo.submittime>=to_date('" + submittime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                    if (!string.IsNullOrEmpty(submittime_e)) { strWhere += " and lo.submittime<=to_date('" + submittime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                    if (!string.IsNullOrEmpty(declarationcode)) { strWhere += " and ld.declarationcode like '%" + declarationcode + "%'"; }
                    if (!string.IsNullOrEmpty(customarea)) { strWhere += " and lo.customareacode like '%" + customarea + "%'"; }

                    if (!string.IsNullOrEmpty(ispass))
                    {
                        if (ispass == "放行") { strWhere += " and lo.declstatus=" + (int)DeclStatusEnum.SitePass; }
                        if (ispass == "未放行") { strWhere += " and lo.declstatus<" + (int)DeclStatusEnum.SitePass; }
                    }
                    if (!string.IsNullOrEmpty(ischeck))
                    {
                        if (ischeck == "查验") { strWhere += " and lo.ischeck=1"; }
                        if (ischeck == "未查验") { strWhere += " and lo.ischeck=0"; }
                    }

                    if (!string.IsNullOrEmpty(busitype)) { strWhere += " and lo.busitype in (" + busitype + ")"; }
                    if (!string.IsNullOrEmpty(modifyflag))
                    {
                        if (ispass == "删单") strWhere += " and ld.modifyflag=1";
                        if (ispass == "改单") strWhere += " and ld.modifyflag=2";
                        if (ispass == "改单完成") strWhere += " and ld.modifyflag=3";
                    }
                    if (!string.IsNullOrEmpty(auditflag)) { strWhere += " and lo.auditflag=1"; }

                    if (!string.IsNullOrEmpty(ordercode)) { strWhere += " and lo.code like '%" + ordercode + "%'"; }
                    if (!string.IsNullOrEmpty(cusno)) { strWhere += " and lo.cusno like '%" + cusno + "%'"; }
                    if (!string.IsNullOrEmpty(divideno)) { strWhere += " and lo.divideno like '%" + divideno + "%'"; }
                    if (!string.IsNullOrEmpty(contractno)) { strWhere += " and lo.CONTRACTNO like '%" + contractno + "%'"; }

                    if (!string.IsNullOrEmpty(passtime_s)) { strWhere += " and lo.sitepasstime>=to_date('" + passtime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                    if (!string.IsNullOrEmpty(passtime_e)) { strWhere += " and lo.sitepasstime<=to_date('" + passtime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; } 

                    
                    //当前登录用户权限控制
                    if (!string.IsNullOrEmpty(customerCode) && !string.IsNullOrEmpty(hscode))
                    {
                        strWhere += " and (lo.busiunitcode='" + hscode + "' or lo.customercode='" + customerCode + "')";
                    }
                    else if (!string.IsNullOrEmpty(customerCode))
                    {
                        strWhere += " and lo.customercode='" + customerCode + "'";
                    }
                    else if (!string.IsNullOrEmpty(hscode))
                    {
                        strWhere += " and lo.busiunitcode='" + hscode + "'";
                    }

                    string sumSql = @"select count(1) from list_order lo left join list_declaration ld on lo.code=ld.ordercode  {0}";
                    DataTable sdt = db.QuerySignle(string.Format(sumSql, strWhere));
                    if (sdt != null && sdt.Rows.Count > 0) sum = sdt.Rows[0][0].ToString2();

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
                                    lo.preendtime,lo.preendname,lo.rependtime,lo.rependname,lo.siteapplytime,lo.siteapplyusername,lo.sitepasstime,lo.sitepassusername,
                                    lo.inspmoendtime,lo.inspmoendname,lo.inspcoendtime,lo.inspcoendname,lo.insppreendtime,lo.insppreendname,lo.insprependtime,lo.insprependname,
                                    lo.inspsiteapplytime,lo.inspsiteapplyusername,lo.inspsitepasstime,lo.inspsitepassusername,lo.auditflagtime,lo.auditflagname,lo.fumigationtime,
                                    lo.fumigationname from list_order lo where lo.code='" + code + "'";
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
                    sql = "select li.approvalcode,li.inspectioncode,li.clearancecode,to_char(li.modifyflag) as modifyflag,li.inspstatus from list_inspection li where li.ordercode='" + code + "'";
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
                            strWhere += " and busitype in ('10','20','30','40','50','11','21','31','41','51')";
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
                            strWhere += " and SITEAPPLYTIME is not null";
                            break;
                    }
                    break;
                case "是否放行":
                    switch (str)
                    {
                        //yangyang.zhao
                        case "":
                            break;
                        case "全部":
                            break;
                        case "已放行":
                            strWhere += " and lo.declstatus=160";
                            break;
                        case "未放行":
                            strWhere += " and lo.declstatus<160";
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