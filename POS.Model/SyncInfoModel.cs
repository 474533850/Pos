
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 同步数据的信息
    /// </summary>
    public class SyncInfoModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 表的ID名称
        /// </summary>
        public int SID { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public string tableName { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool isRead { get; set; }
    }
}
