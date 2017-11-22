﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Entity.Enum;

namespace WeChat.ModelBusi
{
    public class SiteInspection
    {
        public static DataTable getSiteInspectionInfo(string inout_type, string issiterep, string lawflag, string isneedclearance, string busitype, string ispass, string startdate, string enddate
            , string radiotype, string morecon, int start, int itemsPerLoad)
        {
            using (DBSession db = new DBSession())
            {
                string where = "";
                if (!string.IsNullOrEmpty(inout_type))//进出口
                {
                    switch (inout_type)
                    {
                        case "全部":
                            break;
                        case "进口":
                            where += " and ort.busitype in ('11','21','31','41','51')";
                            break;
                        case "出口":
                            where += " and ort.busitype in ('10','20','30','40','50')";
                            break;
                    }

                }
                if (!string.IsNullOrEmpty(issiterep))//现场申报->现场交接时间是否为空
                {
                    if (issiterep == "仅现场") { where += " and ort.handovertime is not null"; }
                }

                if (lawflag == "1")//法检
                {
                    where += " and ort.lawflag='" + lawflag + "'";
                }

                if (isneedclearance == "1")//通关单
                {
                    where += " and ort.isneedclearance='" + isneedclearance + "'";
                }

                if (!string.IsNullOrEmpty(busitype))//业务类型
                {
                    switch (busitype)
                    {
                        case "全部":
                            break;
                        case "空运":
                            where += " and ort.busitype in ('10','11')";
                            break;
                        case "海运":
                            where += " and ort.busitype in ('20','21')";
                            break;
                        case "陆运":
                            where += " and ort.busitype in ('30','31')";
                            break;
                        case "国内":
                            where += " and ort.busitype in ('40','41')";
                            break;
                        case "特殊区域":
                            where += " and ort.busitype in ('50','51')";
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(ispass))//放行情况
                {
                    if (issiterep == "未放行")
                    {
                        where += " and ort.declstatus<" + (int)DeclStatusEnum.SitePass;
                    }
                    if (issiterep == "已放行")
                    {
                        where += " and ort.declstatus=" + (int)DeclStatusEnum.SitePass;
                    }
                }
                if (!string.IsNullOrEmpty(startdate))//委托日期
                {
                    where += " and ort.submittime>=to_date('" + startdate + " 00:00:00','yyyy-mm-dd hh24:mi:ss') ";
                }
                if (!string.IsNullOrEmpty(enddate))
                {
                    where += " and ort.submittime<=to_date('" + enddate + " 23:59:59','yyyy-mm-dd hh24:mi:ss') ";
                }

                if (!string.IsNullOrEmpty(radiotype))//更多查询
                {
                    switch (radiotype)
                    {
                        case "报检单号":
                            where += " and li.inspectioncode like '%" + morecon + "%'";
                            break;
                        case "收发货人":
                            where += " and ort.busiunitname like '%" + morecon + "%'";
                            break;
                        case "客户编号":
                            where += " and ort.cusno like '%" + morecon + "%'";
                            break;
                        case "业务编号":
                            where += " and ort.code like '%" + morecon + "%'";
                            break;
                        case "通关单号":
                            where += " and li.clearancecode like '%" + morecon + "%'";
                            break;
                    }
                }

                string tempsql = @"select ort.busiunitname,ort.busitype,ort.code
                                    ,ort.totalno,ort.divideno,ort.secondladingbillno,ort.landladingno,ort.associatepedeclno,ort.repwayid
                                    ,(select name from cusdoc.sys_repway where enabled=1 and code=ort.repwayid and rownum=1) repwayname,ort.cusno
                                    ,to_char(ort.handovertime,'yyyyMMdd HH24:mi') handovertime,ort.goodsnum,ort.goodsgw,ort.contractno
                                    ,to_char(ort.declchecktime,'yyyyMMdd HH24:mi') declchecktime,ort.ischeck,ort.checkpic
                                    ,to_char(ort.sitepasstime,'yyyyMMdd HH24:mi') sitepasstime,ort.lawflag,ort.isneedclearance 
                                from list_order ort
                                    left join list_inspection li on ort.code=li.ordercode 
                                where ort.entrusttype in('02','03') and ort.isinvalid=0 and li.isinvalid=0" + where;

                string pageSql = @"SELECT * FROM ( SELECT tt.*, ROWNUM AS rowno FROM ({0} ORDER BY {1} {2}) tt WHERE ROWNUM <= {4}) table_alias WHERE table_alias.rowno >= {3}";
                string sql = string.Format(pageSql, tempsql, "ort.handovertime", "desc", start + 1, start + itemsPerLoad);

                return db.QuerySignle(sql);
            }
        }



    }
}