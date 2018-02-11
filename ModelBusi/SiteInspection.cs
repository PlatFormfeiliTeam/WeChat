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

namespace WeChat.ModelBusi
{
    public class SiteInspection
    {
        public static DataTable getSiteInspectionInfo(string inspsiteapplytime_s, string inspsiteapplytime_e, string inspcode, string approvalcode, string ispass, string ischeck, string busitype
            , string lawflag, string isneedclearance, string isfumigation, string modifyflag, string busiunit, string contractno, string ordercode, string cusno, string divideno
            , string customareacode, string submittime_s, string submittime_e, string sitepasstime_s, string sitepasstime_e
            , int start, int itemsPerLoad)
        {
            using (DBSession db = new DBSession())
            {
                string where = "";

                if (!string.IsNullOrEmpty(inspsiteapplytime_s)) { where += " and ort.inspsiteapplytime>=to_date('" + inspsiteapplytime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(inspsiteapplytime_e)) { where += " and ort.inspsiteapplytime<=to_date('" + inspsiteapplytime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(inspcode)) { where += " and li.inspectioncode like '%" + inspcode + "%'"; }
                if (!string.IsNullOrEmpty(approvalcode)) { where += " and li.approvalcode like '%" + approvalcode + "%'"; }

                if (!string.IsNullOrEmpty(ispass))
                {
                    if (ispass == "放行") { where += " and ort.inspstatus=" + (int)DeclStatusEnum.SitePass; }
                    if (ispass == "未放行") { where += " and ort.inspstatus<" + (int)DeclStatusEnum.SitePass; }
                }
                if (!string.IsNullOrEmpty(ischeck))
                {
                    if (ischeck == "查验") { where += " and ort.inspischeck=1"; }
                    if (ischeck == "未查验") { where += " and ort.inspischeck=0"; }
                }
                if (!string.IsNullOrEmpty(busitype)) { where += " and ort.busitype in (" + busitype + ")"; }
                if (!string.IsNullOrEmpty(lawflag)) { where += " and ort.lawflag=1"; }
                if (!string.IsNullOrEmpty(isneedclearance)) { where += " and ort.isneedclearance=1"; }
                if (!string.IsNullOrEmpty(isfumigation)) { where += " and ort.isfumigation=1"; }
                if (!string.IsNullOrEmpty(modifyflag)) { where += " and li.modifyflag='" + modifyflag + "'"; }

                if (!string.IsNullOrEmpty(busiunit)) { where += " and (ort.BUSIUNITCODE like '%" + busiunit + "%' or ort.BUSIUNITNAME like '%" + busiunit + "%')"; }
                if (!string.IsNullOrEmpty(contractno)) { where += " and ort.CONTRACTNO like '%" + contractno + "%'"; }
                if (!string.IsNullOrEmpty(ordercode)) { where += " and ort.code like '%" + ordercode + "%'"; }
                if (!string.IsNullOrEmpty(cusno)) { where += " and ort.cusno like '%" + cusno + "%'"; }
                if (!string.IsNullOrEmpty(divideno)) { where += " and ort.divideno like '%" + divideno + "%'"; }
                if (!string.IsNullOrEmpty(customareacode)) { where += " and ort.customareacode like '%" + customareacode + "%'"; }

                if (!string.IsNullOrEmpty(submittime_s)) { where += " and ort.submittime>=to_date('" + submittime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(submittime_e)) { where += " and ort.submittime<=to_date('" + submittime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(sitepasstime_s)) { where += " and ort.inspsitepasstime>=to_date('" + sitepasstime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(sitepasstime_e)) { where += " and ort.inspsitepasstime<=to_date('" + sitepasstime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }       

                string tempsql = @"select ort.busiunitname,ort.busitype,ort.code
                                    ,ort.totalno,ort.divideno,ort.secondladingbillno,ort.landladingno,ort.associatepedeclno,ort.repwayid
                                    ,(select name from cusdoc.sys_repway where enabled=1 and code=ort.repwayid and rownum=1) repwayname,ort.cusno
                                    ,to_char(ort.inspsiteapplytime,'yyyyMMdd HH24:mi') inspsiteapplytime,ort.goodsnum,ort.goodsgw,ort.contractno
                                    ,to_char(ort.inspchecktime,'yyyyMMdd HH24:mi') inspchecktime,ort.inspischeck,ort.isfumigation,ort.inspcheckpic
                                    ,to_char(ort.inspsitepasstime,'yyyyMMdd HH24:mi') inspsitepasstime,ort.lawflag,ort.isneedclearance
                                    ,ort.inspcheckremark 
                                from list_order ort
                                    left join list_inspection li on ort.code=li.ordercode 
                                where ort.entrusttype in('02','03') and ort.isinvalid=0 and li.isinvalid=0" + where;

                string pageSql = @"SELECT * FROM ( SELECT tt.*, ROWNUM AS rowno FROM ({0} ORDER BY {1} {2}) tt WHERE ROWNUM <= {4}) table_alias WHERE table_alias.rowno >= {3}";
                string sql = string.Format(pageSql, tempsql, "ort.submittime", "desc", start + 1, start + itemsPerLoad);

                return db.QuerySignle(sql);
            }
        }

        public static string Siteapply(string ordercode)
        {
            string userid = "763"; string realname = "昆山吉时报关有限公司";

            using (DBSession db = new DBSession())
            {
                string sql = "select to_char(inspsiteapplytime,'yyyy/mm/dd hh24:mi:ss') as inspsiteapplytime from list_order where code='" + ordercode + "'";
                DataTable dt = db.QuerySignle(sql);
                string curtime = dt.Rows[0]["INSPSITEAPPLYTIME"].ToString();
                if (curtime != "")
                {
                    return curtime.Left(curtime.Length - 3).Replace("/", "");
                }

                curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                sql = "update list_order set inspsiteapplyuserid='{1}',inspsiteapplyusername='{2}',inspsiteapplytime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),inspstatus=150 where code='{0}'";
                sql = string.Format(sql, ordercode, userid, realname, curtime);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
                    msc.redis_OrderStatusLog(ordercode);

                    //add 20180115 保存操作记录list_times
//                    sql = @"insert into list_times(id,code,userid,realname,times,type,ispause) 
//                        values(list_times_id.nextval,'" + ordercode + "','" + userid + "','" + realname + "',sysdate,'0',0)";
//                    db.ExecuteSignle(sql);

                    //add 20180119 保存历史记录
                    sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                                            ,'{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                    sql = string.Format(sql, ordercode, userid, curtime, realname, ordercode, "INSPSITEAPPLYTIME", "现场报检");
                    db.ExecuteSignle(sql);

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
            string userid = "763"; string realname = "昆山吉时报关有限公司";

            using (DBSession db = new DBSession())
            {
                string sql = "select to_char(inspsitepasstime,'yyyy/mm/dd hh24:mi:ss') as inspsitepasstime from list_order where code='" + ordercode + "'";
                DataTable dt = db.QuerySignle(sql);
                string curtime = dt.Rows[0]["INSPSITEPASSTIME"].ToString();
                if (curtime != "")
                {
                    return curtime.Left(curtime.Length - 3).Replace("/", "");
                }

                curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                sql = "update list_order set inspsitepassuserid='{1}',inspsitepassusername='{2}',inspsitepasstime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),inspstatus=160 where code='{0}'";
                sql = string.Format(sql, ordercode, userid, realname, curtime);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
                    msc.redis_OrderStatusLog(ordercode);

                    //add 20180115 保存操作记录list_times
//                    sql = @"insert into list_times(id,code,userid,realname,times,type,ispause) 
//                        values(list_times_id.nextval,'" + ordercode + "','" + userid + "','" + realname + "',sysdate,'0',0)";
//                    db.ExecuteSignle(sql);

                    //add 20180119 保存历史记录
                    sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                                            ,'{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                    sql = string.Format(sql, ordercode, userid, curtime, realname, ordercode, "INSPSITEPASSTIME", "报检放行");
                    db.ExecuteSignle(sql);

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
                            ,ort.submittime,ort.submitusername,ort.inspmoendtime,ort.inspmoendname
                            ,ort.inspsiteapplytime,ort.inspsiteapplyusername,ort.inspcoendtime,ort.inspcoendname 
                            ,ort.inspchecktime,ort.inspcheckname,ort.insppreendtime,ort.insppreendname
                            ,ort.inspsitepasstime,ort.inspsitepassusername,ort.insprependtime,ort.insprependname
                            ,ort.inspcheckpic,ort.fumigationtime,ort.fumigationname 
                        from list_order ort where ort.isinvalid=0 and code='" + ordercode + "'";
                dt_order = db.QuerySignle(sql);

                //dt_order.TableName = "order";
                ds.Tables.Add(dt_order);


                DataTable dt_insp = new DataTable();
                sql = @"select li.APPROVALCODE,li.INSPECTIONCODE,li.CLEARANCECODE,li.MODIFYFLAG,li.INSPSTATUS
                        from list_inspection li 
                        where li.isinvalid=0 and li.ordercode='" + ordercode + "'";
                dt_insp = db.QuerySignle(sql);

                //dt_insp.TableName = "insp";
                ds.Tables.Add(dt_insp);

                return ds;
            }
        }

        /*public static DataSet getinspcontainerdata(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = ""; DataSet ds = new DataSet();

                DataTable dt_order = new DataTable();
                sql = @"select ort.code,ort.cusno,ort.inspchecktime,ort.inspcheckname,ort.inspcheckid,ort.inspischeck 
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
        public static DataTable getinspcontainerdata(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = @"select lp.CONTAINERNO,lp.CONTAINERSIZEE
                                from list_predeclcontainer lp   
                                where lp.isinvalid=0 and lp.ordercode='" + ordercode + "'";
                return db.QuerySignle(sql);
            }
        }

        public static DataTable getloadcheckdata(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = @"select ort.inspischeck,to_char(ort.inspchecktime,'yyyyMMdd HH24:mi') inspchecktime,ort.inspcheckid,ort.inspcheckname,ort.inspcheckremark inspcheckremark
                                   ,ort.isfumigation,to_char(ort.fumigationtime,'yyyyMMdd HH24:mi') fumigationtime,ort.fumigationid,ort.fumigationname
                                from list_order ort   
                                where ort.isinvalid=0 and code='" + ordercode + "'";
                return db.QuerySignle(sql);
            }
        }

        public static string check_fumigation_save(string ordercode, string inspchecktime, string inspcheckname, string inspcheckid, string inspcheckremark
            , string fumigationtime, string fumigationname, string fumigationid)
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
            sql = @"select code,entrusttype,declstatus,inspstatus,inspischeck,isfumigation,inspcheckpic      
                    from list_order lo where lo.code='" + ordercode + "'";
            DataTable dt_order = db.QuerySignle(sql);
            string db_inspischeck = dt_order.Rows[0]["INSPISCHECK"].ToString();
            string db_isfumigation = dt_order.Rows[0]["ISFUMIGATION"].ToString();
            string db_inspcheckpic = dt_order.Rows[0]["INSPCHECKPIC"].ToString();
            int inspischeck = 0; int isfumigation = 0; int inspcheckpic = db_inspcheckpic == "1" ? 1 : 0;

            DataTable dt = db.QuerySignle("select * from list_attachment where ordercode='" + ordercode + "' and filetype='68'");
            foreach (DataRow dr in dt.Rows)
            {
                delfile.Add(dr["FILENAME"] + "");
            }

            db.Dispose();

            try
            {
                db = new DBSession();
                db.BeginTransaction();

                if (inspchecktime != "")
                {
                    inspischeck = 1;
                    if (db_inspischeck != "1")
                    {
                        feoremark += "list_order.inspischeck查验标志为1";

                        sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                            ,'" + ordercode + "','" + userid + "','1','" + realname + "','" + ordercode + "','INSPISCHECK','报检查验'"
                                            + ")";
                        db.QuerySignle(sql);
                    }

                    sql = @"update list_order 
                            set inspischeck=1,inspcheckid='{1}',inspcheckname='{2}',inspchecktime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),inspcheckremark='{4}'  
                            where code='{0}'";
                    sql = string.Format(sql, ordercode, inspcheckid, inspcheckname, inspchecktime, inspcheckremark);
                    db.QuerySignle(sql);  
                }
                else
                {
                    inspcheckpic = 0;
                    if (db_inspischeck == "1")
                    {
                        feoremark += "list_order.inspischeck查验标志为0";

                        sql = @"update list_order 
                            set inspischeck=0,inspcheckid=null,inspcheckname=null,inspchecktime=null,inspcheckpic=0,inspcheckremark='' 
                            where code='" + ordercode + "'";
                        db.QuerySignle(sql);

                        sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                            ,'" + ordercode + "','" + userid + "','0','" + realname + "','" + ordercode + "','INSPISCHECK','报检查验'"
                                            + ")";
                        db.QuerySignle(sql);
                    }

                    sql = "delete LIST_ATTACHMENT where ordercode='" + ordercode + "' and filetype='68'";
                    db.QuerySignle(sql);
                }

                if (fumigationtime != "")
                {
                    isfumigation = 1;
                    if (db_isfumigation != "1")
                    {
                        sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                            ,'" + ordercode + "','" + userid + "','1','" + realname + "','" + ordercode + "','ISFUMIGATION','熏蒸标志'"
                                           + ")";
                        db.QuerySignle(sql);
                    }

                    sql = @"update list_order 
                            set isfumigation=1,fumigationid='{1}',fumigationname='{2}',fumigationtime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')
                            where code='{0}'";
                    sql = string.Format(sql, ordercode, fumigationid, fumigationname, fumigationtime);
                    db.QuerySignle(sql);
                }
                else
                {
                    if (db_isfumigation == "1")
                    {
                        sql = @"update list_order 
                            set isfumigation=0,fumigationid=null,fumigationname=null,fumigationtime=null  
                            where code='" + ordercode + "'";
                        db.QuerySignle(sql);

                        sql = @"insert into list_updatehistory(id,UPDATETIME,TYPE
                                            ,ORDERCODE,USERID,NEWFIELD,NAME,CODE,FIELD,FIELDNAME) 
                                    values(LIST_UPDATEHISTORY_ID.nextval,sysdate,'1'
                                            ,'" + ordercode + "','" + userid + "','0','" + realname + "','" + ordercode + "','ISFUMIGATION','熏蒸标志'"
                                                + ")";
                        db.QuerySignle(sql);
                    }
                }

                db.Commit();
                foreach (string item in delfile)//提交之后删除文件
                {
                    ftp.DeleteFile(item);
                }

                resultmsg = "[{\"INSPISCHECK\":" + inspischeck + ",\"INSPCHECKTIME\":'" + inspchecktime + "',\"INSPCHECKPIC\":" + inspcheckpic
                    + ",\"ISFUMIGATION\":" + isfumigation + ",\"FUMIGATIONTIME\":'" + fumigationtime + "'}]";
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
                                    , ftppath, filename, "68", uploaduserid, customercode, ordercode
                                    , fi.Length, "查验文件", ".jpg");

                                string sql2 = "update list_order set inspcheckpic=1 where code='" + ordercode + "'";

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
                string sql = "select filename from list_attachment where filetype=68 and ordercode='" + ordercode + "'";
                dt = db.QuerySignle(sql);
            }
            return dt;
        }
    }
}