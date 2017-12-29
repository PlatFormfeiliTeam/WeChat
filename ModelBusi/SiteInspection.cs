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
        public static DataTable getSiteInspectionInfo(string inout_type, string issiterep, string lawflag, string isneedclearance, string isfumigation, string busitype, string ispass, string startdate, string enddate
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
                if (!string.IsNullOrEmpty(issiterep))//现场报检时间是否为空
                {
                    if (issiterep == "仅现场") { where += " and ort.inspsiteapplytime is not null"; }
                }

                if (lawflag == "1")//法检
                {
                    where += " and ort.lawflag=1";
                }

                if (isneedclearance == "1")//通关单
                {
                    where += " and ort.isneedclearance=1";
                }

                if (isfumigation == "1")//熏蒸
                {
                    where += " and ort.isfumigation=1";
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
                    switch (ispass)
                    {
                        case "全部":
                            break;
                        case "未放行":
                            where += " and ort.inspstatus<" + (int)DeclStatusEnum.SitePass;
                            break;
                        case "已放行":
                            where += " and ort.inspstatus=" + (int)DeclStatusEnum.SitePass;
                            break;
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
                                    ,to_char(ort.inspsiteapplytime,'yyyyMMdd HH24:mi') inspsiteapplytime,ort.goodsnum,ort.goodsgw,ort.contractno
                                    ,to_char(ort.inspchecktime,'yyyyMMdd HH24:mi') inspchecktime,ort.inspischeck,ort.isfumigation,ort.inspcheckpic
                                    ,to_char(ort.inspsitepasstime,'yyyyMMdd HH24:mi') inspsitepasstime,ort.lawflag,ort.isneedclearance 
                                from list_order ort
                                    left join list_inspection li on ort.code=li.ordercode 
                                where ort.entrusttype in('02','03') and ort.isinvalid=0 and li.isinvalid=0" + where;

                string pageSql = @"SELECT * FROM ( SELECT tt.*, ROWNUM AS rowno FROM ({0} ORDER BY {1} {2}) tt WHERE ROWNUM <= {4}) table_alias WHERE table_alias.rowno >= {3}";
                string sql = string.Format(pageSql, tempsql, "ort.inspsiteapplytime", "desc", start + 1, start + itemsPerLoad);

                return db.QuerySignle(sql);
            }
        }

        public static string Siteapply(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                string curtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string sql = "update list_order set inspsiteapplyuserid='{1}',inspsiteapplyusername='{2}',inspsiteapplytime=to_date('{3}','yyyy-MM-dd HH24:mi:ss') where code='{0}'";
                sql = string.Format(sql, ordercode, "763", "昆山吉时报关有限公司", curtime);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
                    msc.redis_OrderStatusLog(ordercode);

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
                string sql = "update list_order set inspsitepassuserid='{1}',inspsitepassusername='{2}',inspsitepasstime=to_date('{3}','yyyy-MM-dd HH24:mi:ss') where code='{0}'";
                sql = string.Format(sql, ordercode, "763", "昆山吉时报关有限公司", curtime);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
                    msc.redis_OrderStatusLog(ordercode);

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
                            ,ort.inspcheckpic
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

        public static string checksave(string ordercode, string checktime, string checkname, string checkid, string isfumigation)
        {
            using (DBSession db = new DBSession())
            {
                string sql = @"update list_order set inspischeck=1,inspcheckid='{1}',inspcheckname='{2}',inspchecktime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')
                                    ,fumigationname='{2}',fumigationtime=to_date('{3}','yyyy-MM-dd HH24:mi:ss'),isfumigation='{4}' where code='{0}'";
                sql = string.Format(sql, ordercode, checkid, checkname, checktime, isfumigation);
                int i = db.ExecuteSignle(sql);
                if (i > 0)
                {
                    return checktime.Replace("-", "");
                }
                else
                {
                    return "";
                }
            }
        }

        public static string checkcancel(string ordercode)
        {
            using (DBSession db = new DBSession())
            {
                System.Uri Uri = new Uri("ftp://" + ConfigurationManager.AppSettings["FTPServer"] + ":" + ConfigurationManager.AppSettings["FTPPortNO"]);
                string UserName = ConfigurationManager.AppSettings["FTPUserName"];
                string Password = ConfigurationManager.AppSettings["FTPPassword"];
                FtpHelper ftp = new FtpHelper(Uri, UserName, Password);

                DataTable dt = db.QuerySignle("select * from list_attachment where ordercode='" + ordercode + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    ftp.DeleteFile(dr["FILENAME"] + "");
                }

                List<string> sqls = new List<string>();
                string sql = @"update list_order set inspischeck=0,inspcheckid=null,inspcheckname=null,inspchecktime=null,inspcheckpic=0
                                ,fumigationname=null,fumigationtime=null,isfumigation=0 where code='" + ordercode + "'";
                string sql2 = "delete LIST_ATTACHMENT where ordercode='" + ordercode + "' and filetype='68'";

                sqls.Add(sql); sqls.Add(sql2);

                int i = db.ExecuteBatch(sqls);
                if (i > 0)
                {
                    return "sucess";
                }
                else
                {
                    return "";
                }
            }
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