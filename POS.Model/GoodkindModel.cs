using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 货品规格
    /// </summary>
    public class GoodkindModel:BaseModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int xno { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        /// <returns></returns>
   public string goodkind { get; set; }
        /// <summary>
        /// 规格选项
        /// </summary>
        /// <returns></returns>
      public string goodkinds { get; set; }

    }
}
