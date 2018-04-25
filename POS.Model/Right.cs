using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 权限组
    /// </summary>
    public class Right
    {
        /// <summary>
        /// 权限组名称
        /// </summary>
        public string xright { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public List<Permissions> permissions { get; set; }
    }
}
