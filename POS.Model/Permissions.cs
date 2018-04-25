using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// 权限
        /// </summary>
        public string permission { get; set; }
        /// <summary>
        /// 最低折扣
        /// </summary>
        public int minDiscount { get; set; }
        /// <summary>
        /// 折让限额
        /// </summary>
        public int discounQuota { get; set; }
    }
}
