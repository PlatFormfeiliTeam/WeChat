using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Common;

namespace WeChat.ModelBusi
{
    public class Declare
    {
        public static DataTable getDeclareInfo(string declcode, string startdate, string enddate, string inouttype, string busitypeid, string modifyflag, string customsstatus, int start, int itemsPerLoad)
        {
            using (DBSession db = new DBSession())
            {
                string where = "";
                if (!string.IsNullOrEmpty(declcode))
                {
                    where += " and lda.declarationcode like '%" + declcode + "%'";
                }
                if (!string.IsNullOrEmpty(startdate))
                {
                    where += " and lda.reptime>=to_date('" + startdate + " 00:00:00','yyyy-mm-dd hh24:mi:ss') ";
                }
                if (!string.IsNullOrEmpty(enddate))
                {
                    where += " and lda.reptime<=to_date('" + enddate + " 23:59:59','yyyy-mm-dd hh24:mi:ss') ";
                }

                if (!string.IsNullOrEmpty(inouttype))
                {
                    switch (inouttype)
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

                if (!string.IsNullOrEmpty(busitypeid))
                {
                    switch (busitypeid)
                    {
                        case "全部":
                            break;
                        case "空运业务":
                            where += " and ort.busitype in ('11','10')";
                            break;
                        case "海运业务":
                            where += " and ort.busitype in ('21','20')";
                            break;
                        case "陆运业务":
                            where += " and ort.busitype in ('31','30')";
                            break;
                        case "国内业务":
                            where += " and ort.busitype in ('41','40')";
                            break;
                        case "特殊区域":
                            where += " and ort.busitype in ('51','50')";
                            break;
                    }                   
                }
                if (!string.IsNullOrEmpty(modifyflag))
                {
                    where += " and det.modifyflag='" + modifyflag + "' ";
                }
                if (!string.IsNullOrEmpty(customsstatus))
                {
                    if (customsstatus == "已结关") { where += " and det.CUSTOMSSTATUS='已结关' "; }
                    if (customsstatus == "未结关") { where += " and det.CUSTOMSSTATUS!='已结关' and det.CUSTOMSSTATUS!='删单或异常' "; }

                    if (customsstatus == "已放行") { where += " and det.CUSTOMSSTATUS='已放行' "; }
                    if (customsstatus == "未放行") { where += " and det.CUSTOMSSTATUS!='已放行' and det.CUSTOMSSTATUS!='已结关' and det.CUSTOMSSTATUS!='删单或异常' "; }
                }

                string tempsql = @"select det.code,det.modifyflag,det.CUSTOMSSTATUS
                                    ,lda.declarationcode,lda.BLNO,lda.CONSIGNEESHIPPER,lda.CONSIGNEESHIPPERNAME,lda.CONTRACTNO,lda.TRADEMETHOD,lda.TRANSNAME,lda.VOYAGENO,lda.reptime
                                    ,lda.GOODSNUM,lda.GOODSGW
                                    ,ort.busitype,ort.cusno,ort.code ordercode
                                from list_declaration det     
                                    left join list_order ort on det.ordercode = ort.code 
                                    left join list_declaration_after lda on det.code=lda.code and lda.csid=1
                                    left join (select ordercode from list_declaration ld where ld.isinvalid=0 and ld.STATUS!=130 and ld.STATUS!=110) a on det.ordercode=a.ordercode
                                    left join list_verification lv on lda.declarationcode=lv.declarationcode ";

                if (busitypeid == "国内业务")
                {
                    tempsql += @" left join (
                                                  select ASSOCIATENO from list_order l inner join list_declaration i on l.code=i.ordercode 
                                                  where l.ASSOCIATENO is not null and l.isinvalid=0 and i.isinvalid=0 and (i.STATUS!=130 and i.STATUS!=110)          
                                                  ) b on ort.ASSOCIATENO=b.ASSOCIATENO";
                }

                tempsql += @" where (det.STATUS=130 or det.STATUS=110) and det.isinvalid=0 and ort.isinvalid=0" + where
                                + @" and a.ordercode is null";

                if (busitypeid == "40-41")
                {
                    tempsql += @" and b.ASSOCIATENO is null";
                }


                string pageSql = @"SELECT * FROM ( SELECT tt.*, ROWNUM AS rowno FROM ({0} ORDER BY {1} {2}) tt WHERE ROWNUM <= {4}) table_alias WHERE table_alias.rowno >= {3}";
                string sql = string.Format(pageSql, tempsql, "lda.reptime", "desc", start + 1, start + itemsPerLoad);

                return db.QuerySignle(sql);
            }
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
                using (DBSession db = new DBSession())
                {
                    string sql = "";

                    if (modifyflag == 1)//删单1
                    {
                        sql = @"select ld.code,ld.ordercode 
                            from list_declaration ld 
                                inner join config_filesplit cfs on ld.busiunitcode=cfs.busiunitcode and cfs.filetype='53' and ld.code='" + predelcode + "'";
                        DataTable dt = db.QuerySignle(sql);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string ordercode = dt.Rows[0]["ordercode"].ToString();
                                if (!string.IsNullOrEmpty(ordercode))
                                {
                                    sql = @"update list_attachmentdetail t1 set t1.filetypeid='162' and t1.ordercode='" + ordercode + "' and t1.filetypeid='53'";
                                    db.ExecuteSignle(sql);
                                }
                            }
                        }
                    }

                    if (modifyflag == 2)//改单2
                    {
                        DateTime time = DateTime.Now;
                        sql = @"update list_declaration_after set dataconfirm='1'
                                ,dataconfirmusertime=to_date('" + time + "','yyyy-MM-dd HH24:mi:ss') where code='" + predelcode + "' and xzlb like '报关单%'";
                        db.ExecuteSignle(sql);
                    }

                    //改单完成3


                    //修改删改单标志
                    //sql = @"update list_declaration set modifyflag=" + modifyflag + " where code='" + predelcode + "'";

                    sql = @"update list_declaration set modifyflag=" + modifyflag;
                    if (modifyflag == 1) { sql += ",delorderuserid='{1}',delorderusername='{2}',delordertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')"; }
                    else { sql += ",modorderuserid='{1}',modorderusername='{2}',modordertime=to_date('{3}','yyyy-MM-dd HH24:mi:ss')"; }
                    sql += " where code='{0}'";
                    sql = string.Format(sql, predelcode, "763", "昆山吉时报关有限公司", DateTime.Now);

                    db.ExecuteSignle(sql);

                    //保存操作记录list_times
                    sql = @"insert into list_times(id,code,userid,realname,times,type,ispause) 
                        values(list_times_id.nextval,'" + predelcode + "','763','昆山吉时报关有限公司',sysdate,'1'," + modifyflag + ")";
                    db.ExecuteSignle(sql);

                    //调用缓存接口redis_DeclarationLog
                    sql = @"select code,ordercode,declarationcode from list_declaration ld where ld.code='" + predelcode + "'";
                    DataTable dt_decl = db.QuerySignle(sql);

                    MethodSvc.MethodServiceClient msc = new MethodSvc.MethodServiceClient();
                    msc.redis_DeclarationLog(dt_decl.Rows[0]["ordercode"].ToString(), predelcode, dt_decl.Rows[0]["declarationcode"].ToString(), "", "0");

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

    }
}