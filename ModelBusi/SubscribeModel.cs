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
        public static DataTable getSubscribeInfo_Order(string starttime, string endtime, string flag, int pagesize, int lastnum, string cusno, int userId)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " (subws.codetype=1 or subws.codetype=2) and ";
                    if (!string.IsNullOrEmpty(cusno))
                    {
                        strWhere += " subws.cusno='" + cusno + "' and ";
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
                    if (userId > 0)
                    {
                        strWhere += " subws.userid=" + userId + " and";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select lo.busiunitname,lo.busitype,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,to_char(lo.declstatus) as declstatus,to_char(lo.inspstatus) as inspstatus,lo.logisticsname, 
                            ws.cusno,ws.triggerstatus, ws.substype,ws.status as substatus ,'' as sublogstatus,ws.statusvalue,sb.name as businame,sr.name as repwayname from wechat_subscribe ws 
                            left join list_order lo on ws.cusno=lo.cusno 
                            left join cusdoc.sys_busitype sb on lo.busitype=sb.code 
                            left join cusdoc.sys_repway sr on lo.repwayid=sr.code where ws.cusno in (
                            select cusno from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select cusno,substime, ROW_NUMBER() OVER(partition by cusno order by substime desc) as rnum from  wechat_subscribe subws 
                                where {0}  and subws.cusno is not null)
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2})
                            and (ws.codetype=1 or ws.codetype=2) and ws.isinvalid=0 order by ws.cusno,ws.substype,ws.statusvalue";
                    sql = string.Format(sql, strWhere, lastnum + pagesize, lastnum);
                    return db.QuerySignle(sql);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Order:" + ex.Message);
                return null;
            }
            
        }

        /// <summary>
        /// 订单_获取本人未推送的所有订阅信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Order(int userId)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " (subws.codetype=1 or subws.codetype=2) and (subws.TRIGGERSTATUS=0 or subws.TRIGGERSTATUS=1) and ";
                    
                    if (userId > 0)
                    {
                        strWhere += " subws.userid=" + userId + " and";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select lo.busiunitname,lo.busitype,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,to_char(lo.declstatus) as declstatus,to_char(lo.inspstatus) as inspstatus,lo.logisticsname, 
                            ws.cusno,ws.triggerstatus, ws.substype,ws.status as substatus ,'' as sublogstatus,ws.statusvalue,sb.name as businame,sr.name as repwayname from wechat_subscribe ws 
                            left join list_order lo on ws.cusno=lo.cusno 
                            left join cusdoc.sys_busitype sb on lo.busitype=sb.code 
                            left join cusdoc.sys_repway sr on lo.repwayid=sr.code where ws.cusno in (
                            select cusno from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select cusno,substime, ROW_NUMBER() OVER(partition by cusno order by substime desc) as rnum from  wechat_subscribe subws 
                                where {0}  and subws.cusno is not null)
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2})
                            and (ws.codetype=1 or ws.codetype=2) and ws.isinvalid=0 order by ws.cusno,ws.substype,ws.statusvalue";
                    sql = string.Format(sql, strWhere, 100, 0);
                    return db.QuerySignle(sql);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Order:" + ex.Message);
                return null;
            }

        }
        /// <summary>
        /// 预制单_获取最新的N条订阅条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Decl(string starttime, string endtime, string flag, int pagesize, int lastnum, string declarationcode,int userId)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " subws.codetype=3 and ";
                    if (!string.IsNullOrEmpty(declarationcode))
                    {
                        strWhere += " subws.declarationcode='" + declarationcode + "' and ";
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
                    if (userId >= 0)
                    {
                        strWhere += " subws.userid=" + userId + " and ";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select ld.declarationcode,ld.goodsnum,ld.goodsgw,ld.tradecode,ld.modifyflag,ld.customsstatus,ld.transname,ws.declarationcode,
                            ws.cusno,ws.triggerstatus, ws.substype,ws.status as substatus ,ws.statusvalue,cbd.name as tradename from wechat_subscribe ws 
                            left join list_declaration ld on ws.declarationcode=ld.declarationcode 
                            left join cusdoc.base_decltradeway cbd on ld.tradecode=cbd.code where ws.declarationcode in (
                            select declarationcode from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select declarationcode,substime, ROW_NUMBER() OVER(partition by declarationcode order by substime desc) as rnum from  wechat_subscribe subws 
                            where {0} and declarationcode is not null)
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2}
                            ) and ws.codetype=3 and ws.isinvalid=0 and ws.declarationcode is not null order by ws.declarationcode,ws.substype,ws.statusvalue";
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
        /// 预制单_获取本人未推送的所有订阅信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Decl(int userId)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " subws.codetype=3 and (subws.TRIGGERSTATUS=0 or subws.TRIGGERSTATUS=1) and  ";
                    
                    if (userId >= 0)
                    {
                        strWhere += " subws.userid=" + userId + " and ";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select ld.declarationcode,ld.goodsnum,ld.goodsgw,ld.tradecode,ld.modifyflag,ld.customsstatus,ld.transname,ws.declarationcode,
                            ws.cusno,ws.triggerstatus, ws.substype,ws.status as substatus ,ws.statusvalue,cbd.name as tradename from wechat_subscribe ws 
                            left join list_declaration ld on ws.declarationcode=ld.declarationcode 
                            left join cusdoc.base_decltradeway cbd on ld.tradecode=cbd.code where ws.declarationcode in (
                            select declarationcode from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select declarationcode,substime, ROW_NUMBER() OVER(partition by declarationcode order by substime desc) as rnum from  wechat_subscribe subws 
                            where {0} and declarationcode is not null)
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2}
                            ) and ws.codetype=3 and ws.isinvalid=0 and ws.declarationcode is not null order by ws.declarationcode,ws.substype,ws.statusvalue";
                    return db.QuerySignle(string.Format(sql, strWhere, 100, 0));
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
                            left join list_order lo on ws.cusno=lo.code 
                            left join cusdoc.sys_busitype sb on lo.busitype=sb.code 
                            left join cusdoc.sys_repway sr on lo.repwayid=sr.code where ws.cusno in ({0}) and ws.isinvalid=0 order by ws.cusno,ws.substype,ws.statusvalue";
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
        /// <param name="cusno"></param>
        /// <param name="declarationcode"></param>
        /// <param name="cusno"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="openid"></param>
        /// <param name="codetype"></param>
        /// <returns></returns>
        public static bool insertSubscribe(string type, string[] status, string cusno, string declarationcode, int userid, string username, string openid, string codetype)
        {
            try
            {
                string sql = @"insert into wechat_subscribe(id,cusno,declarationcode,userid,username,substime,substype,status,openid,statusvalue,codetype) 
                values(wechat_subscribe_id.nextval,'{0}','{1}','{2}','{3}',sysdate,'{4}','{5}','{6}','{7}','{8}')";
                List<string> sqls = new List<string>();
                for (int i = 0; i < status.Length; i++)
                {
                    string statusvalue = SwitchHelper.switchValue(type, status[i]);
                    sqls.Add(string.Format(sql, cusno, declarationcode, userid, username, type, status[i], openid, statusvalue, codetype));
                }
                using (DBSession db = new DBSession())
                {
                    return db.ExecuteBatch(sqls) > 0 ? true : false;
                }
            }
            catch(Exception ex)
            {
                LogHelper.Write("SubscribeModel_insertSubscribe:" + ex.Message + "——code:" + cusno + declarationcode);
                return false;
            }
            
        }
        /// <summary>
        /// 获取预制单订阅状态
        /// </summary>
        /// <param name="declarationcode"></param>
        /// <returns></returns>
        public static DataTable getDeclstatus(string declarationCode)
        {
            using(DBSession db=new DBSession())
            {
                string sql = "select customsstatus from list_declaration where declarationcode='" + declarationCode + "'";
                return db.QuerySignle(sql);
            }
        }
        /// <summary>
        /// 获取业务物流订阅状态
        /// </summary>
        /// <param name="cusno"></param>
        /// <returns></returns>
        public static DataTable getLogisticsstatus(string cusno)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select logisticsname from list_order where cusno='" + cusno + "'";
                return db.QuerySignle(sql);
            }
        }
        /// <summary>
        /// 获取业务报关状态
        /// </summary>
        /// <param name="cusno"></param>
        /// <returns></returns>
        public static DataTable getOrderstatus(string cusno)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select declstatus from list_order where code='" + cusno + "'";
                return db.QuerySignle(sql);
            }
        }
        /// <summary>
        /// 获取触发状态（0已订阅，1已触发，2已推送）
        /// </summary>
        /// <param name="cusno"></param>
        /// <returns></returns>
        public static DataTable GetTriggerstatus(string cusno, string checkedStatus, string type, string declarationcode)
        {
            using (DBSession db = new DBSession())
            {
                string sql;
                if (type.Equals("报关状态"))
                {
                     sql = "select triggerstatus from wechat_subscribe where cusno='" + cusno +
                                 "'  and status = '" + checkedStatus + "' and substype = '" + type + "' and declarationcode = '"+declarationcode+"' ";
                }
                else
                {
                    sql = "select triggerstatus from wechat_subscribe where cusno='" + cusno + "'  and status = '" + checkedStatus + "' and substype = '" + type + "' ";
                }
                 
                return db.QuerySignle(sql);
            }
        }

    }

}