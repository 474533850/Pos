using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 账户
    /// </summary>
    public class PaytModel : BaseModel
    {
        /// <summary>
        /// 帐户类别
        /// </summary>
        public string payttype { get; set; }
        /// <summary>
        /// 帐户代码
        /// </summary>
        /// <returns></returns>
        public string paytcode { get; set; }
        /// <summary>
        /// 帐户名称
        /// </summary>
        /// <returns></returns>
        public string paytname { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        /// <returns></returns>
        public string xnote { get; set; }

    }
}
