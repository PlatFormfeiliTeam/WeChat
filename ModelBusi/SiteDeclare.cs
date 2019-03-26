using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WeChat.Common;
using WeChat.Entity.Enum;
using WeChat.Entity;

namespace WeChat.ModelBusi
{
    public class SiteDeclare
    {
        public static DataSet getSiteDeclareInfo(string siteapplytime_s, string siteapplytime_e, string declcode, string customareacode, string ispass, string ischeck, string busitype
            , string modifyflag, string auditflag, string busiunit, string ordercode, string cusno, string divideno, string contractno
            , string submittime_s, string submittime_e, string sitepasstime_s, string sitepasstime_e
            , int start, int itemsPerLoad, string customercode)//
        {
            DataSet ds = new DataSet();
            using (DBSession db = new DBSession())
            {
                string where = ""; string where_dec = "";

                if (!string.IsNullOrEmpty(siteapplytime_s)) { where += " and ort.siteapplytime>=to_date('" + siteapplytime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(siteapplytime_e)) { where += " and ort.siteapplytime<=to_date('" + siteapplytime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(declcode)) { where_dec += " and det.declarationcode like '%" + declcode + "%'"; }//where_dec
                if (!string.IsNullOrEmpty(customareacode)) { where += " and ort.customareacode like '%" + customareacode + "%'"; }

                if (!string.IsNullOrEmpty(ispass))
                {
                    if (ispass == "放行") { where += " and ort.declstatus=" + (int)DeclStatusEnum.SitePass; }
                    if (ispass == "未放行") { where += " and ort.declstatus<" + (int)DeclStatusEnum.SitePass; }
                }
                if (!string.IsNullOrEmpty(ischeck))
                {
                    if (ischeck == "查验") { where += " and ort.ischeck=1"; }
                    if (ischeck == "未查验") { where += " and ort.ischeck=0"; }
                }

                if (!string.IsNullOrEmpty(busitype)) { where += " and ort.busitype in (" + busitype + ")"; }
                if (!string.IsNullOrEmpty(modifyflag)) { where_dec += " and det.modifyflag='" + modifyflag + "'"; }//where_dec
                if (!string.IsNullOrEmpty(auditflag)) { where += " and ort.auditflag=1"; }
                
                if (!string.IsNullOrEmpty(busiunit)) { where += " and (ort.BUSIUNITCODE like '%" + busiunit + "%' or ort.BUSIUNITNAME like '%" + busiunit + "%')"; }
                if (!string.IsNullOrEmpty(ordercode)) { where += " and ort.code like '%" + ordercode + "%'"; }
                if (!string.IsNullOrEmpty(cusno)) { where += " and ort.cusno like '%" + cusno + "%'"; }
                if (!string.IsNullOrEmpty(divideno)) { where += " and ort.divideno like '%" + divideno + "%'"; }
                if (!string.IsNullOrEmpty(contractno)) { where += " and ort.CONTRACTNO like '%" + contractno + "%'"; }

                if (!string.IsNullOrEmpty(submittime_s)) { where += " and ort.submittime>=to_date('" + submittime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(submittime_e)) { where += " and ort.submittime<=to_date('" + submittime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(sitepasstime_s)) { where += " and ort.sitepasstime>=to_date('" + sitepasstime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(sitepasstime_e)) { where += " and ort.sitepasstime<=to_date('" + sitepasstime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }

                where += " and ort.receiverunitcode='" + customercode + "'";

                string tempsql = @"select ort.busiunitname,ort.busitype,ort.code
                                    ,ort.totalno,ort.divideno,ort.secondladingbillno,ort.landladingno,ort.associatepedeclno,ort.repwayid
                                    ,(select name from cusdoc.sys_repway where enabled=1 and code=ort.repwayid and rownum=1) repwayname,ort.cusno
                                    ,to_char(ort.siteapplytime,'yyyyMMdd HH24:mi') siteapplytime,ort.goodsnum,ort.goodsgw,ort.contractno
                                    ,to_char(ort.declchecktime,'yyyyMMdd HH24:mi') declchecktime,ort.ischeck,ort.associateno
                                    ,to_char(ort.sitepasstime,'yyyyMMdd HH24:mi') sitepasstime,ort.checkpic,ort.correspondno 
                                    ,ort.checkremark declcheckremark,ort.auditflag,ort.auditcontent 
                                from list_order ort
                                where ort.isinvalid=0" + where
                                    //+ @"and exists (select 1 from list_declaration det where det.isinvalid=0" + where_dec + ")";
                                    + @"and ort.code in (select ordercode from list_declaration det where det.isinvalid=0" + where_dec + ")";

                string pageSql = @"SELECT * FROM ( SELECT tt.*, ROWNUM AS rowno FROM ({0} ORDER BY {1} {2}) tt WHERE ROWNUM <= {4}) table_alias WHERE table_alias.rowno >= {3}";
                string sql = string.Format(pageSql, tempsql, "ort.submittime", "desc", start + 1, start + itemsPerLoad);
                DataTable dt = db.QuerySignle(sql);
                ds.Tables.Add(dt);

                string sql_count = @"select count(1) sum from (" + tempsql + ") a";
                DataTable dt_count = db.QuerySignle(sql_count);
                ds.Tables.Add(dt_count);
            }
            return ds;
        }

//        public static string Siteapply(string ordercode)
//        {
//            string userid = "763"; string username = "ksjsbg"; string realname = "昆山吉时报关有限公司";

//            using (DBSession db = new DBSession())
//            {
//                string sql = "select to_char(siteapplytime,'yyyy/mm/dd hh24:mi:ss') as siteapplytime from list_order where code='" + ordercode + "'";
//                DataTable dt = db.QuerySignle(sql);
//                string curtime = dt.Rows[0]["SITEAPPLYTIME"].ToString();
//                if (curtime != "")
//                {
//                    return curtime.Left(curtime.Length - 3).Replace("/", "");
//                }

//                curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
//                sql = "update list_order set siteapplyuserid='{1}',siteapplyusername='{2}',siteapplytime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),declstatus=150 where code='{0}'";
//                sql = string.Format(sql, ordercode, userid, realname, curtime);
//                int i = db.ExecuteSignle(sql);
//                if (i > 0)
//                {
//                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
//                    msc.redis_OrderStatusLog(ordercode);

//                    //add 20180115 保存操作记录list_times
//                    //sql = @"insert into list_times(id,code,userid,realname,times,type,ispause) 
//                        //values(list_times_id.nextval,'" + ordercode + "','" + userid + "','" + realname + "',sysdate,'0',0)";
//                    //db.ExecuteSignle(sql);

//                    //add 20180119 保存历史记录
//                    sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
//                                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
//                                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
//                                                            ,'{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
//                    sql = string.Format(sql, ordercode, userid, curtime, realname, ordercode, "SITEAPPLYTIME", "现场报关");
//                    db.ExecuteSignle(sql);


//                    sql = @"select code,entrusttype,declstatus,inspstatus from list_order lo where lo.code='" + ordercode + "'";
//                    DataTable dt_order = db.QuerySignle(sql);

//                    //add 20180115 费用异常接口
//                    if (dt_order.Rows[0]["entrusttype"].ToString() == "03")
//                    {
//                        if (Convert.ToInt32(dt_order.Rows[0]["declstatus"].ToString()) >= 160 && Convert.ToInt32(dt_order.Rows[0]["inspstatus"].ToString()) >= 120)
//                        {
//                            msc.FinanceExceptionOrder(ordercode, username, "list_order.siteapplytime现场报关");
//                        }
//                    }
//                    else
//                    {
//                        if (Convert.ToInt32(dt_order.Rows[0]["declstatus"].ToString()) >= 160)
//                        {
//                            msc.FinanceExceptionOrder(ordercode, username, "list_order.siteapplytime现场报关");
//                        }
//                    }   

//                    return curtime.Left(curtime.Length - 3).Replace("/", "");
//                }
//                else
//                {
//                    return "";
//                }

//                /*
//                string curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
//                string sql = "update list_order set siteapplyuserid='{1}',siteapplyusername='{2}',siteapplytime=to_date('{3}','yyyy-MM-dd HH24:mi:ss') where code='{0}'";
//                sql = string.Format(sql, ordercode, "763", "昆山吉时报关有限公司", curtime);
//                int i = db.ExecuteSignle(sql);
//                if (i > 0)
//                {
//                    return curtime.Left(curtime.Length - 3).Replace("/", "");
//                }
//                else
//                {
//                    return "";
//                }*/
//            }
//        }

        public static string Siteapplyall(string ordercode, WGUserEn user)
        {
            string userid = user.GwyUserID.ToString(); string username = user.GwyUserCode; string realname = user.GwyUserName;
            //string userid = "763"; string username = "ksjsbg"; string realname = "昆山吉时报关有限公司";

            using (DBSession db = new DBSession())
            {
                string sql = "select to_char(siteapplytime,'yyyy/mm/dd hh24:mi:ss') as siteapplytime from list_order where code='" + ordercode + "'";
                DataTable dt = db.QuerySignle(sql);
                string curtime = dt.Rows[0]["SITEAPPLYTIME"].ToString();
                if (curtime != "")
                {
                    return "{\"ORDERCODE\":'" + ordercode + "',\"CURTIME\":'" + curtime.Left(curtime.Length - 3).Replace("/", "") + "',\"FLAG\":'',\"ISEXISTS\":'Y'}";
                }

                curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                sql = "update list_order set siteapplyuserid='{1}',siteapplyusername='{2}',siteapplytime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),declstatus=150 where code='{0}' and declstatus<=150";
                sql = string.Format(sql, ordercode, userid, realname, curtime);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
                    msc.redis_OrderStatusLog(ordercode);

                    //add 20180119 保存历史记录
                    sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                                            ,'{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                    sql = string.Format(sql, ordercode, userid, curtime, realname, ordercode, "SITEAPPLYTIME", "现场报关");
                    db.ExecuteSignle(sql);


                    sql = @"select code,entrusttype,declstatus,inspstatus from list_order lo where lo.code='" + ordercode + "'";
                    DataTable dt_order = db.QuerySignle(sql);

                    //add 20180115 费用异常接口
                    if (dt_order.Rows[0]["entrusttype"].ToString() == "03")
                    {
                        if (Convert.ToInt32(dt_order.Rows[0]["declstatus"].ToString()) >= 160 && Convert.ToInt32(dt_order.Rows[0]["inspstatus"].ToString()) >= 120)
                        {
                            msc.FinanceExceptionOrder(ordercode, username, "list_order.siteapplytime现场报关");
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt_order.Rows[0]["declstatus"].ToString()) >= 160)
                        {
                            msc.FinanceExceptionOrder(ordercode, username, "list_order.siteapplytime现场报关");
                        }
                    }

                    return "{\"ORDERCODE\":'" + ordercode + "',\"CURTIME\":'" + curtime.Left(curtime.Length - 3).Replace("/", "") + "',\"FLAG\":'S',\"ISEXISTS\":'N'}"; 
                }
                else
                {
                    return "{\"ORDERCODE\":'" + ordercode + "',\"CURTIME\":'" + curtime.Left(curtime.Length - 3).Replace("/", "") + "',\"FLAG\":'E',\"ISEXISTS\":'N'}"; 
                }
            }
        }

//        public static string Pass(string ordercode)
//        {
//            string userid = "763"; string realname = "昆山吉时报关有限公司";

//            using (DBSession db = new DBSession())
//            {
//                string sql = "select to_char(sitepasstime,'yyyy/mm/dd hh24:mi:ss') as sitepasstime from list_order where code='" + ordercode + "'";
//                DataTable dt = db.QuerySignle(sql);
//                string curtime = dt.Rows[0]["SITEPASSTIME"].ToString();
//                if (curtime != "")
//                {
//                    return curtime.Left(curtime.Length - 3).Replace("/", "");
//                }

//                curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
//                sql = "update list_order set sitepassuserid='{1}',sitepassusername='{2}',sitepasstime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),declstatus=160 where code='{0}'";
//                sql = string.Format(sql, ordercode, userid, realname, curtime);
//                int i = db.ExecuteSignle(sql);
//                if (i > 0)
//                {
//                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
//                    msc.redis_OrderStatusLog(ordercode);

//                    //add 20180115 保存操作记录list_times
////                    sql = @"insert into list_times(id,code,userid,realname,times,type,ispause) 
////                        values(list_times_id.nextval,'" + ordercode + "','" + userid + "','" + realname + "',sysdate,'0',0)";
////                    db.ExecuteSignle(sql);

//                    //add 20180119 保存历史记录
//                    sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
//                                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
//                                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
//                                                            ,'{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
//                    sql = string.Format(sql, ordercode, userid, curtime, realname, ordercode, "SITEPASSTIME", "报关放行");
//                    db.ExecuteSignle(sql);


//                    return curtime.Left(curtime.Length - 3).Replace("/", "");
//                }
//                else
//                {
//                    return "";
//                }

//                /*string curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
//                string sql = "update list_order set sitepassuserid='{1}',sitepassusername='{2}',sitepasstime=to_date('{3}','yyyy-MM-dd HH24:mi:ss') where code='{0}'";
//                sql = string.Format(sql, ordercode, "763", "昆山吉时报关有限公司", curtime);
//                int i = db.ExecuteSignle(sql);
//                if (i > 0)
//                {
//                    return curtime.Left(curtime.Length - 3).Replace("/", "");
//                }
//                else
//                {
//                    return "";
//                }*/
//            }
//        }

        public static string Passall(string ordercode, WGUserEn user)
        {
            string userid = user.GwyUserID.ToString(); string username = user.GwyUserCode; string realname = user.GwyUserName;
            //string userid = "763"; string username = "ksjsbg"; string realname = "昆山吉时报关有限公司";

            using (DBSession db = new DBSession())
            {
                string sql = "select to_char(sitepasstime,'yyyy/mm/dd hh24:mi:ss') as sitepasstime from list_order where code='" + ordercode + "'";
                DataTable dt = db.QuerySignle(sql);
                string curtime = dt.Rows[0]["SITEPASSTIME"].ToString();
                if (curtime != "")
                {
                    return "{\"ORDERCODE\":'" + ordercode + "',\"CURTIME\":'" + curtime.Left(curtime.Length - 3).Replace("/", "") + "',\"FLAG\":'',\"ISEXISTS\":'Y'}";
                }

                curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                sql = "update list_order set sitepassuserid='{1}',sitepassusername='{2}',sitepasstime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),declstatus=160 where code='{0}' and declstatus<=160";
                sql = string.Format(sql, ordercode, userid, realname, curtime);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
                    msc.redis_OrderStatusLog(ordercode);

                    //add 20180119 保存历史记录
                    sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                                            ,'{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                    sql = string.Format(sql, ordercode, userid, curtime, realname, ordercode, "SITEPASSTIME", "报关放行");
                    db.ExecuteSignle(sql);

                    return "{\"ORDERCODE\":'" + ordercode + "',\"CURTIME\":'" + curtime.Left(curtime.Length - 3).Replace("/", "") + "',\"FLAG\":'S',\"ISEXISTS\":'N'}"; 
                }
                else
                {
                    return "{\"ORDERCODE\":'" + ordercode + "',\"CURTIME\":'" + curtime.Left(curtime.Length - 3).Replace("/", "") + "',\"FLAG\":'E',\"ISEXISTS\":'N'}"; 
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
                            ,ort.submittime,ort.submitusername,ort.moendtime,ort.moendname
                            ,ort.siteapplytime,ort.siteapplyusername,ort.coendtime,ort.coendname 
                            ,ort.declchecktime,ort.declcheckname,ort.preendtime,ort.preendname
                            ,ort.sitepasstime,ort.sitepassusername,ort.rependtime,ort.rependname
                            ,ort.checkpic,ort.auditflagtime,ort.auditflagname                             
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

        /*public static DataSet getdeclcontainerdata(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = ""; DataSet ds = new DataSet();

                DataTable dt_order = new DataTable();
                sql = @"select ort.code,ort.cusno,ort.declchecktime,ort.declcheckname,ort.declcheckid,ort.ischeck 
                        from list_order ort where ort.isinvalid=0 and code='" + ordercode + "'";
                dt_order = db.QuerySignle(sql);
                ds.Tables.Add(dt_order);

                DataTable dt_predeclcontainer = new DataTable();
                sql = @"select lp.CONTAINERNO,lp.CONTAINERSIZEE
                                from list_predeclcontainer lp   
                                where lp.isinvalid=0 and lp.ordercode='" + ordercode + "'";
                dt_predeclcontainer = db.QuerySignle(sql);
                ds.Tables.Add(dt_predeclcontainer);
                return ds;
            }
        }*/
        public static DataTable getdeclcontainerdata(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string sql =  @"select lp.CONTAINERNO,lp.CONTAINERSIZEE
                                from list_predeclcontainer lp   
                                where lp.isinvalid=0 and lp.ordercode='" + ordercode + "'";
                return db.QuerySignle(sql);
            }
        }

        public static DataTable getloadcheckdata(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = @"select ort.ischeck,to_char(ort.declchecktime,'yyyyMMdd HH24:mi') declchecktime,ort.declcheckid,ort.declcheckname,ort.checkremark declcheckremark
                                    ,ort.auditflag,to_char(ort.auditflagtime,'yyyyMMdd HH24:mi') auditflagtime,ort.auditflagid,ort.auditflagname,ort.auditcontent 
                                from list_order ort   
                                where ort.isinvalid=0 and code='" + ordercode + "'";
                return db.QuerySignle(sql);
            }
        }

        public static string check_audit_save(string ordercode, string checktime, string checkname, string checkid, string checkremark
            , string auditflagtime, string auditflagname, string auditflagid, string auditcontent)
        {
            MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();

            System.Uri Uri = new Uri("ftp://" + ConfigurationManager.AppSettings["FTPServer"] + ":" + ConfigurationManager.AppSettings["FTPPortNO"]);
            string UserName = ConfigurationManager.AppSettings["FTPUserName"];
            string Password = ConfigurationManager.AppSettings["FTPPassword"];
            FtpHelper ftp = new FtpHelper(Uri, UserName, Password);

            string username = "ksjsbg"; string userid = "763"; string realname = "昆山吉时报关有限公司";
            string sql = ""; string resultmsg = "[]";
            string feoremark = "";//记录是否需要调用费用接口
            List<string> delfile = new List<string>();

            DBSession db = new DBSession();
            sql = @"select code,entrusttype,declstatus,inspstatus,ischeck,auditflag,checkpic      
                    from list_order lo where lo.code='" + ordercode + "'";
            DataTable dt_order = db.QuerySignle(sql);
            string db_ischeck = dt_order.Rows[0]["ISCHECK"].ToString();
            string db_auditflag = dt_order.Rows[0]["AUDITFLAG"].ToString();
            string db_checkpic = dt_order.Rows[0]["CHECKPIC"].ToString();
            int ischeck = 0; int auditflag = 0; int checkpic = db_checkpic == "1" ? 1 : 0; 
             
            DataTable dt = db.QuerySignle("select * from list_attachment where ordercode='" + ordercode + "' and filetype='67'");
            foreach (DataRow dr in dt.Rows)
            {
                delfile.Add(dr["FILENAME"] + "");
            }

            db.Dispose();
            
            try
            {
                db = new DBSession();
                db.BeginTransaction();

                if (checktime != "")
                {
                    ischeck = 1;
                    if (db_ischeck != "1")
                    {
                        feoremark += "list_order.ischeck查验标志为1";

                        sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                            ,'" + ordercode + "','" + userid + "','1','" + realname + "','" + ordercode + "','ISCHECK','报关查验'"
                                            + ")";
                        db.QuerySignle(sql);
                    }

                    sql = @"update list_order 
                            set ischeck=1,declcheckid='{1}',declcheckname='{2}',declchecktime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),checkremark='{4}'  
                            where code='{0}'";
                    sql = string.Format(sql, ordercode, checkid, checkname, checktime, checkremark);
                    db.QuerySignle(sql);
                }
                else
                {
                    checkpic = 0;
                    if (db_ischeck == "1")
                    {
                        feoremark += "list_order.ischeck查验标志为0";

                        sql = @"update list_order 
                            set ischeck=0,declcheckid=null,declcheckname=null,declchecktime=null,checkpic=0,checkremark='' 
                            where code='" + ordercode + "'";
                        db.QuerySignle(sql);

                        sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                            ,'" + ordercode + "','" + userid + "','0','" + realname + "','" + ordercode + "','ISCHECK','报关查验'"
                                            + ")";
                        db.QuerySignle(sql);
                    }

                    sql = "delete LIST_ATTACHMENT where ordercode='" + ordercode + "' and filetype='67'";
                    db.QuerySignle(sql);
                }

                if (auditflagtime != "")
                {
                    auditflag = 1;
                    if (db_auditflag != "1")
                    {
                        feoremark += "list_order.auditflag稽核标志修改为1";

                        sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                            ,'" + ordercode + "','" + userid + "','1','" + realname + "','" + ordercode + "','AUDITFLAG','稽核标志'"
                                           + ")";
                        db.QuerySignle(sql);
                    }

                    sql = @"update list_order 
                            set auditflag=1,auditflagid='{1}',auditflagname='{2}',auditflagtime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),auditcontent='{4}' 
                            where code='{0}'";
                    sql = string.Format(sql, ordercode, auditflagid, auditflagname, auditflagtime, auditcontent);
                    db.QuerySignle(sql);
                }
                else
                {
                    if (db_auditflag == "1")
                    {
                        feoremark += "list_order.auditflag稽核标志修改为0";

                        sql = @"update list_order 
                            set auditflag=0,auditflagid=null,auditflagname=null,auditflagtime=null,auditcontent=''  
                            where code='" + ordercode + "'";
                        db.QuerySignle(sql);

                        sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                            ,'" + ordercode + "','" + userid + "','0','" + realname + "','" + ordercode + "','AUDITFLAG','稽核标志'"
                                                + ")";
                        db.QuerySignle(sql);
                    }
                }

                db.Commit();
                foreach (string item in delfile)//提交之后删除文件
                {
                    ftp.DeleteFile(item);
                }

                resultmsg = "[{\"ISCHECK\":" + ischeck + ",\"DECLCHECKTIME\":'" + checktime + "',\"CHECKPIC\":" + checkpic
                    + ",\"AUDITFLAG\":" + auditflag + ",\"AUDITFLAGTIME\":'" + auditflagtime + "'}]";
            }
            catch (Exception ex)
            {
                db.Rollback();
            }
            finally
            {
                db.Dispose();
            }
            

            //============================================================================================================费用接口
            if (feoremark != "")
            {
                //add 20180115 费用异常接口
                if (dt_order.Rows[0]["entrusttype"].ToString() == "03")
                {
                    if (Convert.ToInt32(dt_order.Rows[0]["declstatus"].ToString()) >= 160 && Convert.ToInt32(dt_order.Rows[0]["inspstatus"].ToString()) >= 120)
                    {
                        msc.FinanceExceptionOrder(ordercode, username, feoremark);
                    }
                }
                else
                {
                    if (Convert.ToInt32(dt_order.Rows[0]["declstatus"].ToString()) >= 160)
                    {
                        msc.FinanceExceptionOrder(ordercode, username, feoremark);
                    }
                }
            }
            //============================================================================================================
            return resultmsg;
        }

        public static string SaveFile(string mediaIds, string ordercode)
        {
            string str = "false";
            if (!string.IsNullOrEmpty(mediaIds))
            {
                string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", ModelWeChat.TokenModel.AccessToken, mediaIds);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                using (WebResponse wr = req.GetResponse())
                {
                    string dicPath = System.AppDomain.CurrentDomain.BaseDirectory;
                    string filename = Guid.NewGuid() + ".jpg";
                    string filepath = @"uploadimage\" + filename;

                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(wr.ResponseUri, dicPath + filepath);                    

                    if (File.Exists(dicPath + filepath))//ftp 到文件服务器,然后往数据库插入一笔记录
                    {
                        FileInfo fi = new FileInfo(dicPath + filepath);

                        System.Uri Uri = new Uri("ftp://" + ConfigurationManager.AppSettings["FTPServer"] + ":" + ConfigurationManager.AppSettings["FTPPortNO"]);
                        string UserName = ConfigurationManager.AppSettings["FTPUserName"];
                        string Password = ConfigurationManager.AppSettings["FTPPassword"];
                        FtpHelper ftp = new FtpHelper(Uri, UserName, Password);
                        string ftppath = "/67/" + ordercode + "/" + filename;
                        bool bf = ftp.UploadFile(dicPath + filepath, ftppath, true); ;
                        if (bf)
                        {
                            using (DBSession db = new DBSession())
                            {
                                List<string> sqls = new List<string>();
                                int uploaduserid = 763;
                                string customercode = "KSJSBGYXGS";

                                string sql = @"insert into LIST_ATTACHMENT (id
                                                ,filename,originalname,filetype,uploadtime,uploaduserid,customercode,ordercode
                                                ,sizes,filetypename,filesuffix)
                                        values(List_Attachment_Id.Nextval,'{0}','{1}','{2}',sysdate,{3},'{4}','{5}'
                                                ,'{6}','{7}','{8}')";
                                sql = string.Format(sql
                                    , ftppath, filename, "67", uploaduserid, customercode, ordercode
                                    , fi.Length, "查验文件", ".jpg");

                                string sql2 = "update list_order set checkpic=1 where code='" + ordercode + "'";

                                sqls.Add(sql); sqls.Add(sql2);

                                int i = db.ExecuteBatch(sqls);
                                if (i > 0)//插入成功，后删除本地文件
                                {
                                    
                                    str = "success"; 
                                    fi.Delete();
                                }
                                else//插入失败后，远程删除文件，本地文件暂且留着
                                {
                                    ftp.DeleteFile(ftppath);
                                }
                            }

                        }//ftp失败，本地文件暂且留着
                    }

                }
            }

            return str;

        }

        public static DataTable picfileconsult(string ordercode)
        {
            DataTable dt = new DataTable();
            using (DBSession db = new DBSession())
            {
                string sql = "select filename from list_attachment where filetype=67 and ordercode='" + ordercode + "'";
                dt = db.QuerySignle(sql);     
            }
            return dt;
        }

    }
}