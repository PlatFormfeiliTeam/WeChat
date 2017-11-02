using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WeChat.ModelBusi
{
    public class Declare
    {
        public static DataTable getDeclareInfo(string declcode, string startdate, string enddate, string busitypeid, string modifyflag, string customsstatus, int start, int itemsPerLoad)
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
                if (!string.IsNullOrEmpty(busitypeid))
                {
                    where += " and instr('" + busitypeid + "',ort.BUSITYPE)>0 ";
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
                                    ,ort.busitype,ort.cusno
                                from list_declaration det     
                                    left join list_order ort on det.ordercode = ort.code 
                                    left join list_declaration_after lda on det.code=lda.code and lda.csid=1
                                    left join (select ordercode from list_declaration ld where ld.isinvalid=0 and ld.STATUS!=130 and ld.STATUS!=110) a on det.ordercode=a.ordercode
                                    left join list_verification lv on lda.declarationcode=lv.declarationcode ";

                if (busitypeid == "40-41")
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


        public static int saveModifyFlag(string predelcode, int modifyflag)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "update list_declaration set modifyflag={1} where code='{0}'";
                sql = string.Format(sql, predelcode, modifyflag);
                //return db.ExecuteSignle(sql);
                return 1;
            }
        }

        public static string FileConsult(string predelcode)
        {
            return "";

        }

    }
}