using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// POS设置
    /// </summary>
    public class PossettingModel : BaseModel
    {
        /// <summary>
        /// 是否为全局系统参数
        /// </summary>
        public bool issys { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string xpname { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string  xpvalue { get; set; }
        /// <summary>
        /// 用户代码
        /// </summary>
        public string usercode { get; set; }

    }
}
