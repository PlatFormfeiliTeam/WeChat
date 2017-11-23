using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Common;
using WeChat.Entity;

namespace WeChat.ModelBusi
{
    public class SubscribeModel
    {
        /// <summary>
        /// 获取推送任务
        /// </summary>
        /// <returns></returns>
        public static List<SubcribeInfoEn> getSubscribeTask()
        {
            using(DBSession db=new DBSession())
            {
                string sql = "select * from wechat_subscribe where TriggerStatus=1 and isinvalid=0";
                return db.QueryEntity<SubcribeInfoEn>(sql);
            }
        }
        /// <summary>
        /// 订单_获取最新的N条订阅条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Order(string starttime, string endtime, string flag, int pagesize, int lastnum, string ordercode)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " (subws.codetype=1 or subws.codetype=2) and ";
                    if (!string.IsNullOrEmpty(ordercode))
                    {
                        strWhere += " subws.ordercode='" + ordercode + "'";
                    }
                    if (!string.IsNullOrEmpty(starttime))
                    {
                        strWhere += " subws.SUBSTIME>to_date('" + starttime + "','yyyy-mm-dd hh24:mi:ss') and ";
                    }
                    if (!string.IsNullOrEmpty(endtime))
                    {
                        strWhere += " subws.SUBSTIME<to_date('" + endtime + " 23:59:59','yyyy-mm-dd hh24:mi:ss') and ";
                    }
                    if (flag == "已触发")
                    {
                        strWhere += " (subws.TRIGGERSTATUS=1 or subws.TRIGGERSTATUS=2) and";
                    }
                    else if (flag == "未触发")
                    {
                        strWhere += " subws.TRIGGERSTATUS=0 and";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select lo.busiunitname,lo.busitype,lo.cusno,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,to_char(lo.declstatus) as declstatus,to_char(lo.inspstatus) as inspstatus,lo.logisticsname, 
                            ws.ordercode,ws.triggerstatus, ws.substype,ws.status as substatus ,'' as sublogstatus,ws.statusvalue,sb.name as businame,sr.name as repwayname from wechat_subscribe ws 
                            left join list_order lo on ws.ordercode=lo.code 
                            left join cusdoc.sys_busitype sb on lo.busitype=sb.code 
                            left join cusdoc.sys_repway sr on lo.repwayid=sr.code where ws.ordercode in (
                            select ordercode from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select ordercode,substime, ROW_NUMBER() OVER(partition by ordercode order by substime desc) as rnum from  wechat_subscribe subws where {0} 
                            ）
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2}
                            
                            ) and (ws.codetype=1 or ws.codetype=2) and ws.isinvalid=0 order by ws.ordercode,ws.substype,ws.statusvalue";
                    return db.QuerySignle(string.Format(sql, strWhere, lastnum + pagesize, lastnum));
                }
            }
            catch(Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Order:" + ex.Message);
                return null;
            }
            
        }
        /// <summary>
        /// 预制单_获取最新的N条订阅条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Decl(string starttime, string endtime, string flag, int pagesize, int lastnum, string declcode)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " codetype=3 and ";
                    if (!string.IsNullOrEmpty(declcode))
                    {
                        strWhere += " subws.declcode='" + declcode + "'";
                    }
                    if (!string.IsNullOrEmpty(starttime))
                    {
                        strWhere += " subws.SUBSTIME>to_date('" + starttime + "','yyyy-mm-dd hh24:mi:ss') and ";
                    }
                    if (!string.IsNullOrEmpty(endtime))
                    {
                        strWhere += " subws.SUBSTIME<to_date('" + endtime + " 23:59:59','yyyy-mm-dd hh24:mi:ss') and ";
                    }
                    if (flag == "已触发")
                    {
                        strWhere += " (subws.TRIGGERSTATUS=1 or subws.TRIGGERSTATUS=2) and";
                    }
                    else if (flag == "未触发")
                    {
                        strWhere += " subws.TRIGGERSTATUS=0 and";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select ld.declarationcode,ld.goodsnum,ld.goodsgw,ld.tradecode,ld.modifyflag,ld.customsstatus,ld.transname,ws.declcode,
                            ws.ordercode,ws.triggerstatus, ws.substype,ws.status as substatus ,ws.statusvalue,cbd.name as tradename from wechat_subscribe ws 
                            left join list_declaration ld on ws.declcode=ld.code 
                            left join cusdoc.base_decltradeway cbd on ld.tradecode=cbd.code where ws.ordercode in (
                            select ordercode from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select ordercode,substime, ROW_NUMBER() OVER(partition by ordercode order by substime desc) as rnum from  wechat_subscribe subws where {0} 
                            ）
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2}
                            
                            ) and ws.codetype=3 and ws.isinvalid=0 and declcode is not null order by ws.ordercode,ws.substype,ws.statusvalue";
                    return db.QuerySignle(string.Format(sql, strWhere, lastnum + pagesize, lastnum));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Decl:" + ex.Message);
                return null;
            }

        }
        /// <summary>
        /// 查询订阅详情
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static DataTable getSubscribeInfo(string ordercodes)
        {
            
            using (DBSession db = new DBSession())
            {
                string sql = @"select lo.busiunitname,lo.busitype,lo.cusno,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,lo.declstatus,lo.inspstatus,
                            lo.logisticsname, ws.substype,ws.status,ws.statusvalue,sb.name as businame,sr.name as repwayname from wechat_subscribe ws 
                            left join list_order lo on ws.ordercode=lo.code 
                            left join cusdoc.sys_busitype sb on lo.busitype=sb.code 
                            left join cusdoc.sys_repway sr on lo.repwayid=sr.code where ws.ordercode in ({0}) and ws.isinvalid=0 order by ws.ordercode,ws.substype,ws.statusvalue";
                return db.QuerySignle(string.Format(sql, ordercodes));
            }
        }
        /// <summary>
        /// 信息已推送
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool updateSubscirbeInfo(int id)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "update wechat_subscribe set TriggerStatus=2,pushtime=sysdate where id=" + id;
                return db.ExecuteSignle(sql) == 0 ? true : false;
            }

        }
        /// <summary>
        /// 新增订阅信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="orderCode"></param>
        /// <param name="declcode"></param>
        /// <param name="cusno"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="openid"></param>
        /// <param name="codetype"></param>
        /// <returns></returns>
        public static bool insertSubscribe(string type, string[] status, string orderCode, string declcode, string userid, string username, string openid, string codetype)
        {
            try
            {
                string sql = @"insert into wechat_subscribe(id,ordercode,declcode,userid,username,substime,substype,status,openid,statusvalue,codetype) 
                values(wechat_subscribe_id.nextval,'{0}','{1}','{2}','{3}',sysdate,'{4}','{5}','{6}','{7}','{8}')";
                List<string> sqls = new List<string>();
                for (int i = 0; i < status.Length; i++)
                {
                    string statusvalue = SwitchHelper.switchValue(type, status[i]);
                    sqls.Add(string.Format(sql, orderCode, declcode, userid, username, type, status[i], openid, statusvalue, codetype));
                }
                using (DBSession db = new DBSession())
                {
                    return db.ExecuteBatch(sqls) > 0 ? true : false;
                }
            }
            catch(Exception ex)
            {
                LogHelper.Write("SubscribeModel_insertSubscribe:" + ex.Message + "——code:" + orderCode + declcode);
                return false;
            }
            
        }
        /// <summary>
        /// 获取预制单订阅状态
        /// </summary>
        /// <param name="declcode"></param>
        /// <returns></returns>
        public static DataTable getDeclstatus(string declCode)
        {
            using(DBSession db=new DBSession())
            {
                string sql = "select customsstatus from list_declaration where code='" + declCode + "'";
                return db.QuerySignle(sql);
            }
        }
        /// <summary>
        /// 获取业务物流订阅状态
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public static DataTable getLogisticsstatus(string orderCode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select logisticsname from list_order where code='" + orderCode + "'";
                return db.QuerySignle(sql);
            }
        }
        /// <summary>
        /// 获取业务报关状态
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public static DataTable getOrderstatus(string orderCode)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select declstatus from list_order where code='" + orderCode + "'";
                return db.QuerySignle(sql);
            }
        }
    }

}