using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 数据库映射关系
    /// </summary>
    [Serializable]
    public class DBRelation
    {
        /// <summary>
        /// 本地列
        /// </summary>
        public string LColumn { get; set; }
        /// <summary>
        /// 本地说明
        /// </summary>
        public string LCaption { get; set; }
        // <summary>
        /// 服务器列
        /// </summary>
        public string SColumn { get; set; }
        /// <summary>
        /// 服务器说明
        /// </summary>
        public string SCaption { get; set; }
    }
}
