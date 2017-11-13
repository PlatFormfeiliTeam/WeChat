using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Entity.Enum;

namespace WeChat.ModelBusi
{
    public class SiteDeclare
    {
        public static DataTable getSiteDeclareInfo(string inout_type, string issiterep, string busitype, string ispass, string startdate, string enddate
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
                        case "报关单号":
                            where += " and det.declarationcode like '%" + morecon + "%'";
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
                    }
                }

                string tempsql = @"select ort.busiunitname,ort.busitype,ort.code
                                    ,ort.totalno,ort.divideno,ort.secondladingbillno,ort.landladingno,ort.associatepedeclno,ort.repwayid
                                    ,(select name from cusdoc.sys_repway where enabled=1 and code=ort.repwayid and rownum=1) repwayname,ort.cusno
                                    ,to_char(ort.handovertime,'yyyyMMdd HH24:mi') handovertime,ort.goodsnum,ort.goodsgw,ort.contractno
                                    ,to_char(ort.declchecktime,'yyyyMMdd HH24:mi') declchecktime,ort.ischeck,ort.associateno
                                    ,to_char(ort.sitepasstime,'yyyyMMdd HH24:mi') sitepasstime,ort.checkpic,ort.correspondno 
                                from list_order ort
                                    left join list_declaration det on ort.code=det.ordercode   
                                where ort.isinvalid=0 and det.isinvalid=0" + where;

                string pageSql = @"SELECT * FROM ( SELECT tt.*, ROWNUM AS rowno FROM ({0} ORDER BY {1} {2}) tt WHERE ROWNUM <= {4}) table_alias WHERE table_alias.rowno >= {3}";
                string sql = string.Format(pageSql, tempsql, "ort.handovertime", "desc", start + 1, start + itemsPerLoad);
               
                return db.QuerySignle(sql);
            }
        }

        public static string Handover(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string sql = "update list_order set handoveruserid='{1}',handoverusername='{2}',handovertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss') where code='{0}'";
                sql = string.Format(sql, ordercode, "763", "昆山吉时报关有限公司", curtime);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    return curtime.Left(curtime.Length - 3).Replace("/", "");
                }
                else
                {
                    return "";
                }
            }
        }

        public static string Pass(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string sql = "update list_order set sitepassuserid='{1}',sitepassusername='{2}',sitepasstime=to_date('{3}','yyyy-MM-dd HH24:mi:ss') where code='{0}'";
                sql = string.Format(sql, ordercode, "763", "昆山吉时报关有限公司", curtime);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    return curtime.Left(curtime.Length - 3).Replace("/", "");
                }
                else
                {
                    return "";
                }
            }
        }

        public static DataSet Detail(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = ""; DataSet ds = new DataSet();

                DataTable dt_order = new DataTable();
                sql = @"select ort.code
                            ,ort.submittime,ort.submitusername,ort.handovertime,ort.handoverusername
                            ,ort.moendtime,ort.moendname,ort.siteapplytime,ort.siteapplyusername 
                            ,ort.coendtime,ort.coendname,ort.declchecktime,ort.declcheckname
                            ,ort.preendtime,ort.preendname,ort.sitepasstime,ort.sitepassusername
                            ,ort.rependtime,ort.rependname,ort.checkpic
                        from list_order ort where ort.isinvalid=0 and code='" + ordercode + "'";
                dt_order = db.QuerySignle(sql);

                //dt_order.TableName = "order";
                ds.Tables.Add(dt_order);


                DataTable dt_decl = new DataTable();
                sql = @"select det.code,det.declarationcode,det.GOODSNUM,det.GOODSGW,det.tradecode,det.TRANSNAME,det.VOYAGENO,det.modifyflag,det.CUSTOMSSTATUS
                        from list_declaration det 
                        where det.isinvalid=0 and ordercode='" + ordercode + "'";
                /*sql = @"select det.code,lda.declarationcode,lda.GOODSNUM,lda.GOODSGW,lda.TRADEMETHOD,lda.TRANSNAME,lda.VOYAGENO,det.modifyflag,det.CUSTOMSSTATUS                                    
                        from list_declaration det     
                            left join (select code,associateno,isinvalid,busitype,cusno from list_order where code='{0}') ort on det.ordercode = ort.code 
                            left join list_declaration_after lda on det.code=lda.code and lda.csid=1
                            left join (select ordercode from list_declaration ld where ld.isinvalid=0 and ld.STATUS!=130 and ld.STATUS!=110) a on det.ordercode=a.ordercode
                            left join list_verification lv on lda.declarationcode=lv.declarationcode 
                            left join (
                                        select ASSOCIATENO from list_order l inner join list_declaration i on l.code=i.ordercode 
                                        where l.ASSOCIATENO is not null and l.isinvalid=0 and i.isinvalid=0 and (i.STATUS!=130 and i.STATUS!=110)          
                                        ) b on ort.ASSOCIATENO=b.ASSOCIATENO
                        where (det.STATUS=130 or det.STATUS=110) and det.isinvalid=0 and ort.isinvalid=0 and det.CUSTOMSSTATUS!='删单或异常'
                            and a.ordercode is null
                            and b.ASSOCIATENO is null";*/
                dt_decl = db.QuerySignle(sql);

                //dt_decl.TableName = "decl";
                ds.Tables.Add(dt_decl);

                return ds;
            }
        }

        public static DataTable getdeclcontainerdata(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = @"select lp.CONTAINERNO,lp.CONTAINERSIZEE
                                from list_predeclcontainer lp   
                                where lp.isinvalid=0 and lp.ordercode='" + ordercode + "'";
                return db.QuerySignle(sql);
            }
        }

    }
}