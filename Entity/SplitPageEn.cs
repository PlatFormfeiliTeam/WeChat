using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WeChat.Entity
{
    /// <summary>
    /// 数据分页实体
    /// </summary>
    [DataContract]
    public class SplitPageEn
    {
        public SplitPageEn(int pageIndex, int pageSize)
        {
            currpg = pageIndex;
            pgsizes = pageSize;
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        [DataMember]
        public int currpg { get; set; }
        private int pgsizes = 20;

        /// <summary>
        /// 每页条数
        /// </summary>
        [DataMember]
        public int pgsize
        {
            get { return pgsizes; }
            set { pgsizes = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember]
        public int pgcount { get; set; }
        private int mdtcount;
        /// <summary>
        /// 总条数
        /// </summary>
        [DataMember]
        public int dtcount
        {
            get { return mdtcount; }

            set
            {
                mdtcount = value;
                if (pgsizes != 0)
                {
                    if (value % pgsizes == 0)
                    {
                        pgcount = value / pgsizes;
                    }
                    else
                    {
                        pgcount = value / pgsizes + 1;
                    }
                }
            }
        }
    }
}