using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    [Serializable]
    public class BaseModel
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 原数据ID
        /// </summary>
        public int? SID { get; set; }

        /// <summary>
        /// xtableid
        /// </summary>
        public int? xtableid { get; set; }

        /// <summary>
        /// xtableid
        /// </summary>
        public int? xsubid { get; set; }

        /// <summary>
        /// 数据版本
        /// </summary>
        public double xversion { get; set; }
    }
}
