using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Entity;

namespace WeChat.ModelBusi
{
    public class WebNoticeModel
    {
        /// <summary>
        /// 资讯动态
        /// </summary>
        /// <returns></returns>
        public List<WebNoticeEn> getTwoNotice()
        {
            using(DBSession db=new DBSession())
            {
                string sql = @"select * from ( select wn.*,nc.name as typename from web_notice wn left join newscategory nc on wn.type=nc.id 
where publishdate is not null and isinvalid=0 and type in (4,5,6) order by publishdate desc) where  rownum<3 ";
                return db.QueryEntity<WebNoticeEn>(sql);
            }
        }
    }
}