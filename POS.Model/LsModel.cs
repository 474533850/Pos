using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    public class LsModel : BaseModel
    {
        /// <summary>
        /// 类别
        /// </summary>
        /// <returns></returns>
        public string xlstype { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        /// <returns></returns>
        public string xls { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 上级代码
        /// </summary>
        public string xlsp { get; set; }
        /// <summary>
        /// 上级名称
        /// </summary>
        public string xlsnamep { get; set; }
        /// <summary>
        /// 性质
        /// </summary>
        public string xdotype { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string xstate { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string lsclass { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string xpost1 { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string xpost2 { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string xpost3 { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string xaddr { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string xfax { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public string xinname { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public string xintime { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public string xlastlogintime { get; set; }
        /// <summary>
        /// 经营开始时间
        /// </summary>
        public string xjytime1 { get; set; }
        /// <summary>
        /// 经营结束时间
        /// </summary>
        public string xjytime2 { get; set; }

    }
}
