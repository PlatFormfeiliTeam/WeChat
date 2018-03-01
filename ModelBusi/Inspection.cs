using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Common;
using WeChat.Entity;
using WeChat.Entity.Enum;


namespace WeChat.ModelBusi
{
    public class Inspection
    {
        public static DataSet getInspectionInfo(string reptime_s, string reptime_e, string inspcode, string modifyflag, string busitype, string ischeck
            , string ispass, string lawflag, string isneedclearance, string busiunit, string contractno, string ordercode, string cusno, string divideno
            , string customareacode, string approvalcode, string submittime_s, string submittime_e, string sitepasstime_s, string sitepasstime_e
            , int start, int itemsPerLoad, string customercode)//
        {
            DataSet ds = new DataSet();
            using (DBSession db = new DBSession())
            {
                string where = "";
                if (!string.IsNullOrEmpty(reptime_s)) { where += " and li.rependtime>=to_date('" + reptime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(reptime_e)) { where += " and li.rependtime<=to_date('" + reptime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(inspcode)) { where += " and li.inspectioncode like '%" + inspcode + "%'"; }
                
                if (!string.IsNullOrEmpty(modifyflag)) { where += " and li.modifyflag='" + modifyflag + "'"; }
                if (!string.IsNullOrEmpty(busitype)) { where += " and ort.busitype in (" + busitype + ")"; }
                if (!string.IsNullOrEmpty(ischeck))
                {
                    if (ispass == "查验") { where += " and ort.ischeck=1"; }
                    if (ispass == "未查验") { where += " and ort.ischeck=0"; }
                }
                if (!string.IsNullOrEmpty(ispass))
                {
                    if (ispass == "放行") { where += " and ort.inspstatus=" + (int)DeclStatusEnum.SitePass; }
                    if (ispass == "未放行") { where += " and ort.inspstatus<" + (int)DeclStatusEnum.SitePass; }
                }
                if (!string.IsNullOrEmpty(lawflag)) { where += " and ort.lawflag=1"; }
                if (!string.IsNullOrEmpty(isneedclearance)) { where += " and ort.isneedclearance=1"; }

                if (!string.IsNullOrEmpty(busiunit)) { where += " and (ort.BUSIUNITCODE like '%" + busiunit + "%' or ort.BUSIUNITNAME like '%" + busiunit + "%')"; }
                if (!string.IsNullOrEmpty(contractno)) { where += " and ort.CONTRACTNO like '%" + contractno + "%'"; }
                if (!string.IsNullOrEmpty(ordercode)) { where += " and ort.code like '%" + ordercode + "%'"; }
                if (!string.IsNullOrEmpty(cusno)) { where += " and ort.cusno like '%" + cusno + "%'"; }
                if (!string.IsNullOrEmpty(divideno)) { where += " and ort.divideno like '%" + divideno + "%'"; }
                if (!string.IsNullOrEmpty(customareacode)) { where += " and ort.customareacode like '%" + customareacode + "%'"; }

                if (!string.IsNullOrEmpty(submittime_s)) { where += " and ort.submittime>=to_date('" + submittime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(submittime_e)) { where += " and ort.submittime<=to_date('" + submittime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(sitepasstime_s)) { where += " and ort.sitepasstime>=to_date('" + sitepasstime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(sitepasstime_e)) { where += " and ort.sitepasstime<=to_date('" + sitepasstime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }

                where += " and ort.receiverunitcode='" + customercode + "'";

                string tempsql = @"select li.CODE,li.ORDERCODE,li.INSPECTIONCODE,li.TRADEWAY,(select NAME from cusdoc.BASE_TRADEWAY bt WHERE enabled=1 and bt.code=li.TRADEWAY) as TRADEWAYNAME
                                    ,li.rependtime REPTIME,li.APPROVALCODE,li.MODIFYFLAG,li.CLEARANCECODE,li.INSPSTATUS
                                    ,ort.BUSITYPE,ort.BUSIUNITCODE,ort.BUSIUNITNAME,ort.CONTRACTNO,ort.GOODSNUM,ort.GOODSGW,ort.CUSNO,ort.SUBMITTIME
                            from list_inspection li
                                left join list_order ort on li.ordercode = ort.code 
                            where li.isinvalid=0 and ort.isinvalid=0" + where;

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

        public static DataTable AssCon(string preinspcode)
        {
            using (DBSession db = new DBSession())
            {
                DataTable dt = null;

                DataTable dt1 = new DataTable();
                dt1 = db.QuerySignle("select ordercode from list_inspection where code='" + preinspcode + "'");
                string ordercode = dt1.Rows[0][0].ToString();

                DataTable dt2 = new DataTable();
                dt2 = db.QuerySignle("select code from list_order where associateno=(select associateno from list_order where code='" + ordercode + "') and code!='" + ordercode + "'");

                //关联订单不存在
                if (dt2 == null) { return dt; }
                if (dt2.Rows.Count <= 0) { return dt; }

                string ordercode_con = dt2.Rows[0][0].ToString();

                string tempsql = @"select li.CODE,li.ORDERCODE,li.INSPECTIONCODE,li.TRADEWAY,(select NAME from cusdoc.BASE_TRADEWAY bt WHERE enabled=1 and bt.code=li.TRADEWAY) as TRADEWAYNAME
                                    ,li.rependtime REPTIME,li.APPROVALCODE,li.MODIFYFLAG,li.CLEARANCECODE,li.INSPSTATUS
                                    ,ort.BUSITYPE,ort.BUSIUNITCODE,ort.BUSIUNITNAME,ort.CONTRACTNO,ort.GOODSNUM,ort.GOODSGW,ort.CUSNO,ort.SUBMITTIME
                            from list_inspection li
                                left join (select * from list_order where code='{0}') ort on li.ordercode = ort.code 
                            where li.isinvalid=0 and ort.isinvalid=0";
                string sql = string.Format(tempsql, ordercode_con);

                return db.QuerySignle(sql);

            }
        }

        public static bool saveModifyFlag(string preinspcode, int modifyflag, WGUserEn user)
        {
            bool bf = false;
            try
            {
                string userid = user.GwyUserID.ToString(); string username = user.GwyUserCode; string realname = user.GwyUserName;
                //string userid = "763"; string username = "ksjsbg"; string realname = "昆山吉时报关有限公司";

                using (DBSession db = new DBSession())
                {
                    string sql = "";

                    if (modifyflag == 1)
                    {
                        sql = @",delorderuserid='{1}',delorderusername='{2}',delordertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')
                            ,modorderuserid=null,modorderusername=null,modordertime=null
                            ,modfinishuserid=null,modfinishusername=null,modfinishtime=null";
                    }
                    if (modifyflag == 2)
                    {
                        sql = @",delorderuserid=null,delorderusername=null,delordertime=null
                            ,modorderuserid='{1}',modorderusername='{2}',modordertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')
                            ,modfinishuserid=null,modfinishusername=null,modfinishtime=null";
                    }
                    if (modifyflag == 3)
                    {
                        sql = @",delorderuserid=null,delorderusername=null,delordertime=null
                            ,modorderuserid=null,modorderusername=null,modordertime=null
                            ,modfinishuserid='{1}',modfinishusername='{2}',modfinishtime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')";
                    }

                    sql = @"update list_inspection set modifyflag=" + modifyflag + sql + " where code='{0}'";
                    sql = string.Format(sql, preinspcode, userid, realname, DateTime.Now);
                    db.ExecuteSignle(sql);

                    bf = true;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Write("saveModifyFlag_sql:" + ex.Message + "——code:" + preinspcode + " modifyflag:" + modifyflag);
            }
            return bf;

        }

        public static DataTable FileConsult(string preinspcode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select filename,inspcode from list_attachment where ordercode=(select ordercode from list_inspection where code='" + preinspcode + "') and filetype=62 order by inspcode";
                DataTable dt = db.QuerySignle(sql);
                return dt;
            }
        }

        public static DataTable picfileconsult(string preinspcode)
        {
            DataTable dt = new DataTable();
            using (DBSession db = new DBSession())
            {
                string sql = "select filename from list_attachment where filetype=68 and ordercode=(select ordercode from list_inspection where code='" + preinspcode + "')";
                dt = db.QuerySignle(sql);
            }
            return dt;
        }


    }
}