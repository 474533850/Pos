using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// 客显类型
    /// </summary>
    public enum CustomerDispiayType
    {
        /// <summary>  
        /// 清屏  
        /// </summary>  
        Clear,
        /// <summary>  
        /// 单价  
        /// </summary>  
        Price,
        /// <summary>  
        /// 总计  
        /// </summary>  
        Total,
        /// <summary>  
        /// 收款  
        /// </summary>  
        Recive,
        /// <summary>  
        /// 找零  
        /// </summary>  
        Change
    }
}
