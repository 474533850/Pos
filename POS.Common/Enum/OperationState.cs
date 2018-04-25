using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// 操作状态
    /// </summary>
    public enum OperationState
    {
        /// <summary>
        /// 追加
        /// </summary>
        Append,
        /// <summary>
        /// 收银
        /// </summary>
        Receipt,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit,
        /// <summary>
        /// 换货
        /// </summary>
        Change,
        /// <summary>
        /// 快速开单追加
        /// </summary>
        FastAppend,
        None,
    }
}
