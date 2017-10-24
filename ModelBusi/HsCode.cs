using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WeChat.ModelBusi
{
    public class HsCode
    {
        public static DataTable getHsCodeInfo(string hscode, string commodityname, int start, int itemsPerLoad)
        {
            using (DBSession db = new DBSession(true))
            {
                string where = "";
                if (!string.IsNullOrEmpty(hscode))
                {

                    where += " and t.HSCODE||t.EXTRACODE like '%" + hscode + "%'";

                }
                if (!string.IsNullOrEmpty(commodityname))
                {
                    where += " and t.NAME like '%" + commodityname + "%'";
                }
                string tempsql = @"select (SELECT name from base_declproductunit where code =  t.LEGALUNIT) as LEGALUNITNAME
                        ,(SELECT name from base_declproductunit where code =  t.SECONDUNIT) as SECONDUNITNAME
                        ,HSCODE||EXTRACODE AS HSCODEEXTRACODE 
                        ,t.ID,t.name,t.HSCODE,t.EXTRACODE,t.LEGALUNIT,t.SECONDUNIT,t.elements,t.FAVORABLERATE
                    from BASE_COMMODITYHS t where 1=1 and t.yearid=(select id from cusdoc.base_year where kind=11 and customarea=2300 and enabled=1)" + where;
                string pageSql = @"SELECT * FROM ( SELECT tt.*, ROWNUM AS rowno FROM ({0} ORDER BY {1} {2}) tt WHERE ROWNUM <= {4}) table_alias WHERE table_alias.rowno >= {3}";
                string sql = string.Format(pageSql, tempsql, "id", "desc", start + 1, start + itemsPerLoad);

                return db.QuerySignle(sql);
            }
        }

        public static DataTable getHsCodeInfoDetail(string id)
        {
            using (DBSession db = new DBSession(true))
            {
                string sql = @"select (SELECT name from base_declproductunit where code =  t.LEGALUNIT) as LEGALUNITNAME
                                    ,(SELECT name from base_declproductunit where code =  t.SECONDUNIT) as SECONDUNITNAME
                                    ,HSCODE||EXTRACODE AS HSCODEEXTRACODE 
                                    ,t.name,t.HSCODE,t.EXTRACODE,t.LEGALUNIT,t.SECONDUNIT,t.elements,t.FAVORABLERATE,t.VATRATE,t.EXPORTREBATRATE,t.GENERALRATE 
                                    ,t.CUSTOMREGULATORY,t.INSPECTIONREGULATORY   
                                from BASE_COMMODITYHS t where t.id='{0}' and t.yearid=(select id from cusdoc.base_year where kind=11 and customarea=2300 and enabled=1)";
                sql = string.Format(sql, id);
                return db.QuerySignle(sql);
            }
        }
    }
}