using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Common;
using WeChat.Entity.Enum;

namespace WeChat.ModelBusi
{
    public class Declare
    {
        public static DataSet getDeclareInfo(string reptime_s, string reptime_e, string declcode, string customsstatus, string modifyflag, string busitype, string ischeck
            , string ispass, string busiunit, string ordercode, string cusno, string tradeway, string contractno, string blno
            , string submittime_s, string submittime_e, string sitepasstime_s, string sitepasstime_e
            , int start, int itemsPerLoad, string customercode)//
        {
            DataSet ds = new DataSet();
            using (DBSession db = new DBSession())
            {
                string where = "";
                if (!string.IsNullOrEmpty(reptime_s)) { where += " and lda.reptime>=to_date('" + reptime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(reptime_e)) { where += " and lda.reptime<=to_date('" + reptime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(declcode)) { where += " and lda.declarationcode like '%" + declcode + "%'"; }
                if (!string.IsNullOrEmpty(customsstatus))
                {
                    if (customsstatus == "已结关") { where += " and det.CUSTOMSSTATUS='已结关'"; }
                    if (customsstatus == "未结关") { where += " and det.CUSTOMSSTATUS!='已结关' and det.CUSTOMSSTATUS!='删单或异常'"; }
                }
                if (!string.IsNullOrEmpty(modifyflag)) { where += " and det.modifyflag='" + modifyflag + "'"; }
                if (!string.IsNullOrEmpty(busitype)) { where += " and ort.busitype in (" + busitype + ")"; }
                if (!string.IsNullOrEmpty(ischeck))
                {
                    if (ispass == "查验") { where += " and ort.ischeck=1"; }
                    if (ispass == "未查验") { where += " and ort.ischeck=0"; }
                }
                if (!string.IsNullOrEmpty(ispass))
                {
                    if (ispass == "放行") { where += " and ort.declstatus=" + (int)DeclStatusEnum.SitePass; }
                    if (ispass == "未放行") { where += " and ort.declstatus<" + (int)DeclStatusEnum.SitePass; }
                }
                if (!string.IsNullOrEmpty(busiunit)) { where += " and (lda.BUSIUNITCODE like '%" + busiunit + "%' or lda.BUSIUNITNAME like '%" + busiunit + "%')"; }
                if (!string.IsNullOrEmpty(ordercode)) { where += " and det.ORDERCODE like '%" + ordercode + "%'"; }
                if (!string.IsNullOrEmpty(cusno)) { where += " and ort.CUSNO like '%" + cusno + "%'"; }
                if (!string.IsNullOrEmpty(tradeway)) { where += " and lda.trademethod like '%" + tradeway + "%'"; }
                if (!string.IsNullOrEmpty(contractno)) { where += " and ort.CONTRACTNO like '%" + contractno + "%'"; }
                if (!string.IsNullOrEmpty(blno)) { where += " and lda.BLNO like '%" + blno + "%'"; }

                if (!string.IsNullOrEmpty(submittime_s)) { where += " and ort.submittime>=to_date('" + submittime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(submittime_e)) { where += " and ort.submittime<=to_date('" + submittime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(sitepasstime_s)) { where += " and ort.sitepasstime>=to_date('" + sitepasstime_s + " 00:00:00','yyyy-mm-dd hh24:mi:ss') "; }
                if (!string.IsNullOrEmpty(sitepasstime_e)) { where += " and ort.sitepasstime<=to_date('" + sitepasstime_e + " 23:59:59','yyyy-mm-dd hh24:mi:ss') "; }

                where += " and ort.receiverunitcode='" + customercode + "'";

                string tempsql = @"select det.code,det.modifyflag,det.CUSTOMSSTATUS
                                    ,lda.declarationcode,lda.BLNO,lda.CONSIGNEESHIPPER,lda.CONSIGNEESHIPPERNAME,lda.CONTRACTNO,lda.TRADEMETHOD,lda.TRANSNAME,lda.VOYAGENO,lda.reptime
                                    ,lda.GOODSNUM,lda.GOODSGW
                                    ,ort.busitype,ort.cusno,ort.code ordercode
                                from list_declaration det     
                                    left join list_order ort on det.ordercode = ort.code 
                                    left join list_declaration_after lda on det.code=lda.code and lda.csid=1
                                    left join (select ordercode from list_declaration ld where ld.isinvalid=0 and ld.STATUS!=130 and ld.STATUS!=110) a on det.ordercode=a.ordercode
                                    left join list_verification lv on lda.declarationcode=lv.declarationcode ";

                if (busitype.Contains("'40'") || busitype.Contains("'41'"))//国内业务
                {
                    tempsql += @" left join (
                                                  select ASSOCIATENO from list_order l inner join list_declaration i on l.code=i.ordercode 
                                                  where l.ASSOCIATENO is not null and l.isinvalid=0 and i.isinvalid=0 and (i.STATUS!=130 and i.STATUS!=110)          
                                                  ) b on ort.ASSOCIATENO=b.ASSOCIATENO";
                }

                tempsql += @" where (det.STATUS=130 or det.STATUS=110) and det.isinvalid=0 and ort.isinvalid=0" + where
                                + @" and a.ordercode is null";

                if (busitype.Contains("'40'") || busitype.Contains("'41'"))//国内业务
                {
                    tempsql += @" and b.ASSOCIATENO is null";
                }
                
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

        public static DataTable AssCon(string predelcode)
        {
            using (DBSession db = new DBSession())
            {
                DataTable dt = null;

                DataTable dt1 = new DataTable();
                dt1 = db.QuerySignle("select ordercode from list_declaration where code='" + predelcode + "'");
                string ordercode = dt1.Rows[0][0].ToString();

                DataTable dt2 = new DataTable();
                dt2 = db.QuerySignle("select code from list_order where associateno=(select associateno from list_order where code='" + ordercode + "') and code!='" + ordercode + "'");

                //关联订单不存在
                if (dt2 == null) { return dt; }
                if (dt2.Rows.Count <= 0) { return dt; }

                string ordercode_con = dt2.Rows[0][0].ToString();

                string tempsql = @"select det.code,det.modifyflag,det.CUSTOMSSTATUS
                                    ,lda.declarationcode,lda.BLNO,lda.CONSIGNEESHIPPER,lda.CONSIGNEESHIPPERNAME,lda.CONTRACTNO,lda.TRADEMETHOD,lda.TRANSNAME,lda.VOYAGENO,lda.reptime
                                    ,lda.GOODSNUM,lda.GOODSGW
                                    ,ort.busitype,ort.cusno
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
                                    and b.ASSOCIATENO is null";
                string sql = string.Format(tempsql, ordercode_con);

                return db.QuerySignle(sql);

            }
        }


        public static bool saveModifyFlag(string predelcode, int modifyflag)
        {
            bool bf = false;
            try
            {
                string userid = "763"; string username = "ksjsbg"; string realname = "昆山吉时报关有限公司";

                using (DBSession db = new DBSession())
                {
                    string sql = "";

                    sql = @"select code,ordercode,declarationcode from list_declaration ld where ld.code='" + predelcode + "'";
                    DataTable dt_decl = db.QuerySignle(sql);
                    string ordercode = dt_decl.Rows[0]["ordercode"].ToString();

                    if (modifyflag == 1)//删单1
                    {
                        sql = @"select ld.code,ld.ordercode from list_declaration ld inner join config_filesplit cfs on ld.busiunitcode=cfs.busiunitcode and cfs.filetype='53' and ld.code='" + predelcode + "'";
                        DataTable dt = db.QuerySignle(sql);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(ordercode))
                                {
                                    sql = @"update list_attachmentdetail t1 set t1.filetypeid='162' where t1.ordercode='" + ordercode + "' and t1.filetypeid='53'";
                                    db.ExecuteSignle(sql);
                                }
                            }
                        }
                    }

                    if (modifyflag == 2)//改单2
                    {
                        DateTime time = DateTime.Now;
                        sql = @"update list_declaration_after set dataconfirm='1',dataconfirmusertime=to_date('" + time + "','yyyy-MM-dd HH24:mi:ss') where code='" + predelcode + "' and xzlb like '报关单%'";
                        db.ExecuteSignle(sql);
                    }

                    //改单完成3

                    //修改删改单标志
                    sql = @"update list_declaration set modifyflag=" + modifyflag;
                    //if (modifyflag == 1) { sql += ",delorderuserid='{1}',delorderusername='{2}',delordertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')"; }
                    //if (modifyflag == 2) { sql += ",modorderuserid='{1}',modorderusername='{2}',modordertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')"; }
                    //if (modifyflag == 3) { sql += ",modfinishuserid='{1}',modfinishusername='{2}',modfinishtime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')"; }

                    if (modifyflag == 1)
                    {
                        sql += @",delorderuserid='{1}',delorderusername='{2}',delordertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')
                            ,modorderuserid=null,modorderusername=null,modordertime=null
                            ,modfinishuserid=null,modfinishusername=null,modfinishtime=null";
                    }
                    if (modifyflag == 2)
                    {
                        sql += @",delorderuserid=null,delorderusername=null,delordertime=null
                            ,modorderuserid='{1}',modorderusername='{2}',modordertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')
                            ,modfinishuserid=null,modfinishusername=null,modfinishtime=null";
                    }
                    if (modifyflag == 3)
                    {
                        sql += @",delorderuserid=null,delorderusername=null,delordertime=null
                            ,modorderuserid=null,modorderusername=null,modordertime=null
                            ,modfinishuserid='{1}',modfinishusername='{2}',modfinishtime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')";
                    }

                    sql += " where code='{0}'";
                    sql = string.Format(sql, predelcode, userid, realname, DateTime.Now);
                    db.ExecuteSignle(sql);


                    //修改订单的报关状态
                    sql = "select customsstatus from list_declaration where ordercode='" + ordercode + "'  and isinvalid=0 and modifyflag<>1";
                    bool flag = true;
                    DataTable dt_order_status = db.QuerySignle(sql);
                    if (dt_order_status != null)
                    {
                        if (dt_order_status.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt_order_status.Rows)
                            {
                                if (dr["customsstatus"].ToString2() == "" || (dr["customsstatus"].ToString2() != "已结关" && dr["customsstatus"].ToString2() != "已放行"))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        sql = "update list_order set declstatus=160,sitepassusername='system_tool_modify',sitepasstime=sysdate,siteapplyuserid=-2 where code='" + ordercode + "'";
                        db.ExecuteSignle(sql);
                    }

                    //---------------------------------------------------------------------------------------------------------------
                    //保存操作记录list_times
                    //sql = @"insert into list_times(id,code,userid,realname,times,type,ispause) 
                        //values(list_times_id.nextval,'" + predelcode + "','" + userid + "','" + realname + "',sysdate,'1'," + modifyflag + ")";
                    //db.ExecuteSignle(sql);

                   

                    //调用缓存接口redis_DeclarationLog
                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
                    msc.redis_DeclarationLog(ordercode, predelcode, dt_decl.Rows[0]["declarationcode"].ToString(), "", "0");


                    sql = @"select code,entrusttype,declstatus,inspstatus from list_order lo where lo.code='" + ordercode + "'";
                    DataTable dt_order = db.QuerySignle(sql);

                    if (dt_order.Rows[0]["entrusttype"].ToString() == "03")
                    {
                        if (Convert.ToInt32(dt_order.Rows[0]["declstatus"].ToString()) >= 160 && Convert.ToInt32(dt_order.Rows[0]["inspstatus"].ToString()) >= 120)
                        {
                            //add 20180115 费用异常接口
                            msc.FinanceExceptionOrder(ordercode, username, "list_declaration.modifyflag修改为" + modifyflag.ToString());
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dt_order.Rows[0]["declstatus"].ToString()) >= 160)
                        {
                            //add 20180115 费用异常接口
                            msc.FinanceExceptionOrder(dt_decl.Rows[0]["ordercode"].ToString(), username, "list_declaration.modifyflag修改为" + modifyflag.ToString());
                        }
                    }

                    bf = true;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Write("saveModifyFlag_sql:" + ex.Message + "——code:" + predelcode + " modifyflag:" + modifyflag);
            }
             return bf;
            
        }

        public static DataTable FileConsult(string predelcode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select filename,declcode from list_attachment where ordercode=(select ordercode from list_declaration where code='" + predelcode + "') and filetype=61 order by declcode";
                DataTable dt = db.QuerySignle(sql);
                return dt;
            }
        }

        public static DataTable picfileconsult(string predelcode)
        {
            DataTable dt = new DataTable();
            using (DBSession db = new DBSession())
            {
                string sql = "select filename from list_attachment where filetype=67 and ordercode=(select ordercode from list_declaration where code='" + predelcode + "')";
                dt = db.QuerySignle(sql);
            }
            return dt;
        }

    }
}