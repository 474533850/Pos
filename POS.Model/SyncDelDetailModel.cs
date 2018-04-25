using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
   public class SyncDelDetailModel
    {
        /// <summary>
        /// xid 对应表的 SID 字段
        /// </summary>
        public int xid { get; set; }
        public int SID { get; set; }
        public string xversion { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string xtbname { get; set; }
    }
}
