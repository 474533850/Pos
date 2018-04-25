using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 线下优惠券
    /// </summary>
    public class TickoffModel:BaseModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string xname { get; set; }
        /// <summary>
        /// 面额
        /// </summary>
        public decimal xallp { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
     public int   xcount { get; set; }
        /// <summary>
        /// 无时间限制
        /// </summary>
      public bool  xnotime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string    xtime1 { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string xtime2 { get; set; }
        /// <summary>
        /// 多少天后使用
        /// </summary>
        public int xafter { get; set; }
        /// <summary>
        /// 使用终端
        /// </summary>
        public string xend { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public string goodtype { get; set; }
        /// <summary>
        /// 商品品牌
        /// </summary>
        public string goodbank { get; set; }
        /// <summary>
        /// 最低金额
        /// </summary>
        public decimal xminallp { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public string xinname { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public string xintime { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string xnote { get; set; }
        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }

    }
}
