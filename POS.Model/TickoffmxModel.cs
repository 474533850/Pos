using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 线下优惠券明细
    /// </summary>
    public class TickoffmxModel : BaseModel
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string xcode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string  xname { get; set; }
        /// <summary>
        /// 面额
        /// </summary>
        public decimal  xallp { get; set; }
        /// <summary>
        /// 顾客代码
        /// </summary>
        public string  clntcode { get; set; }
        /// <summary>
        /// 顾客名称
        /// </summary>
        public string  clntname { get; set; }
        /// <summary>
        /// 无时间限制
        /// </summary>
        public bool  xnotime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string  xtime1 { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string  xtime2 { get; set; }
        /// <summary>
        /// 多少天后使用
        /// </summary>
        public int  xafter { get; set; }
        /// <summary>
        /// 使用终端
        /// </summary>
        public string  xend { get; set; }
        /// <summary>
        /// 最低金额
        /// </summary>
        public decimal  xminallp { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public string  xusetime { get; set; }
        /// <summary>
        /// 制作人
        /// </summary>
        public string  xinname { get; set; }
        /// <summary>
        /// 制作时间
        /// </summary>
        public string  xintime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string  xstate { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public string  xopusetime { get; set; }
    }
}
