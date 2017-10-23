using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 数据批量查询
    /// </summary>
    public class QueryDataObjectEn
    {
        public DataSet ds { get; set; }

        public List<SplitPageEn> sp { get; set; }
    }
}