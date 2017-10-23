using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Entity;
using Dapper;

namespace WeChat.ModelBusi
{
    public class DBSession:IDisposable
    {
        public OracleTransaction tran { get; set; }
        public OracleConnection conn { get; set; }

        public DBSession(bool baseData = false)
        {
            //string strCon = System.Configuration.ConfigurationManager.AppSettings["CTContext"];
            string strCon = "";
            if (baseData)
                strCon = System.Configuration.ConfigurationManager.ConnectionStrings["ConfigData"].ToString();
            else
                strCon = System.Configuration.ConfigurationManager.ConnectionStrings["CTContext"].ToString();
            if (conn == null || conn.State != ConnectionState.Open)
            {
                conn = new OracleConnection(strCon);
                conn.Open();
            }

        }
        public void close(OracleConnection con)
        {
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }

        }

        public void Commit()
        {
            if (tran == null)
            {
                throw new Exception("事务不存在");
            }
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                throw new Exception("数据库连接已经被关闭");
            }
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                try
                {
                    tran.Commit();
                }
                finally
                {
                    tran.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public void BeginTransaction()
        {
            ReOpen();
            this.tran = conn.BeginTransaction();
        }
        public void ReOpen()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        public void Dispose()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public void Rollback()
        {
            if (tran == null)
            {
                throw new Exception("事务不存在");
            }
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                try
                {
                    tran.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public DataTable RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            DataTable dt = new DataTable();
            OracleDataAdapter sqlDA = new OracleDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(storedProcName, parameters);
            sqlDA.Fill(dt);
            return dt;

        }
        private OracleCommand BuildQueryCommand(string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand comm = new OracleCommand(storedProcName, conn);
            comm.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    comm.Parameters.Add(parameter);
                }
            }
            return comm;
        }




        /// <summary>
        /// 返回Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public T QuerySignleEntity<T>(string strSql) where T : class
        {
            try
            {
                return conn.Query<T>(strSql).SingleOrDefault();
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> e)
            {
                throw new Exception(e.Detail.InnerException.Message);
            }

        }
        /// <summary>
        /// 返回List<Entity>集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public List<T> QueryEntity<T>(string strSql) where T : class
        {

            try
            {
                return conn.Query<T>(strSql).ToList();
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> e)
            {
                throw new Exception(e.Detail.InnerException.Message);
            }

        }
        /// <summary>
        /// 单条执行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSignle(string sql)
        {

            try
            {
                return conn.Execute(sql);
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> e)
            {
                throw new Exception(e.Detail.InnerException.Message);
            }
        }
        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public int ExecuteBatch(List<string> sqls)
        {

            int count = 0;
            var trans = conn.BeginTransaction();
            try
            {
                foreach (string sql in sqls)
                {
                    if (!string.IsNullOrEmpty(sql)) count += conn.Execute(sql);
                }
                trans.Commit();
                return count;
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> e)
            {
                tran.Rollback();
                throw new Exception(e.Detail.InnerException.Message);
            }

        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable QuerySignle(string sql)
        {
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> e)
            {
                throw new Exception(e.Detail.InnerException.Message);
            }
        }


        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public QueryDataObjectEn QueryBatch(List<string> sqls, List<bool> isSplit = null, List<SplitPageEn> SplitPageEn = null)
        {
            QueryDataObjectEn qdo = new QueryDataObjectEn();
            OracleTransaction tran = null;
            try
            {
                qdo.ds = new DataSet();
                tran = conn.BeginTransaction();
                OracleCommand cmd = conn.CreateCommand();
                OracleDataAdapter da = null;
                for (int i = 0; i < sqls.Count; i++)
                {
                    if (!string.IsNullOrEmpty(sqls[i]))
                    {
                        cmd.CommandText = sqls[i].Replace(';', ' ');
                        da = new OracleDataAdapter(cmd);
                        DataTable dtCount = new DataTable();
                        da.Fill(dtCount);
                        if (i < isSplit.Count && isSplit[i] && i < SplitPageEn.Count && SplitPageEn[i] != null)
                        {
                            SplitPageEn sp = SplitPageEn[i];
                            sp.dtcount = dtCount.Rows.Count;
                            int begLine = (sp.currpg - 1) * sp.pgsize + 1;
                            int endLine = sp.currpg * sp.pgsize;
                            string exeSql = "select * from (select originsql.*, rownum rn from (" + sqls[i] + ") originsql where rownum <=" + endLine + ") where rn>=" + begLine;
                            cmd.CommandText = exeSql;
                            da = new OracleDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            qdo.ds.Tables.Add(dt);
                            if (qdo.sp == null)
                                qdo.sp = new List<SplitPageEn>();
                            qdo.sp.Add(SplitPageEn[i]);
                        }
                        else
                        {
                            qdo.ds.Tables.Add(dtCount);
                        }
                    }

                }
                return qdo;
            }
            catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> e)
            {
                throw new Exception(e.Detail.InnerException.Message);
            }

        }
        public DateTime GetServerDateTime()
        {

            string sql = "select sysdate from dual";
            var result = QuerySignle(sql);
            if (result != null && result.Rows.Count > 0)
                return result.Rows[0][0].ToDateTime();
            else return DateTime.Now;

        }
    }
}