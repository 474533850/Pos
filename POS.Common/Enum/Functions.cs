using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// 功能
    /// </summary>
    public enum Functions
    {
        /// <summary>
        /// 收银
        /// </summary>
        Cashier = 1,
        /// <summary>
        /// 单品改价
        /// </summary>
        SPrice = 2,
        /// <summary>
        /// 整单改价
        /// </summary>
        WPrice = 3,
        /// <summary>
        /// 会员充值
        /// </summary>
        Recharge = 4,
        /// <summary>
        /// 编辑会员
        /// </summary>
        EClnt = 5,
        /// <summary>
        /// 添加会员
        /// </summary>
        AClnt = 6,
        /// <summary>
        /// 删除挂单
        /// </summary>
        DelPending = 7,
        /// <summary>
        /// 系统设置
        /// </summary>
        Setting = 8,
        /// <summary>
        /// 退货
        /// </summary>
        Returns = 9,
        /// <summary>
        /// 换货
        /// </summary>
        Exchange = 10,
        /// <summary>
        /// 反结
        /// </summary>
        Invalid = 11,
        /// <summary>
        /// 打印
        /// </summary>
        Print = 12,
        /// <summary>
        /// 导出
        /// </summary>
        Export = 13,
        /// <summary>
        /// 开启钱箱
        /// </summary>
        OpenCashbox = 14,
        /// <summary>
        /// 补打单据
        /// </summary>
        RPrint = 15,
        /// <summary>
        /// 不显示后台入口
        /// </summary>
        InvisibleBEntrance = 16,
    }
}
