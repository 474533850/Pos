using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    public class GoodModel : BaseModel
    {
        /// <summary>
        /// 货品规格+货品代码
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 货品大类
        /// </summary>
        public string goodtype1 { get; set; }
        /// <summary>
        /// 货品中类
        /// </summary>
        public string goodtype2 { get; set; }
        /// <summary>
        /// 货品小类
        /// </summary>
        public string goodtype3 { get; set; }
        /// <summary>
        /// 货品代码
        /// </summary>
        public string goodcode { get; set; }
        /// <summary>
        /// 货品名称
        /// </summary>
        public string goodname { get; set; }
        /// <summary>
        /// 货品名称简拼
        /// </summary>
        public string xjpiny { get; set; }
        /// <summary>
        /// 货品名称全拼
        /// </summary>
        public string xqpiny { get; set; }
        /// <summary>
        /// 货品单位
        /// </summary>
        public string goodunit { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public string xbarcode { get; set; }
        /// <summary>
        /// 下架
        /// </summary>
        public bool xshow { get; set; }
        /// <summary>
        /// 货品类型
        /// </summary>
        public string goodkeys { get; set; }
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal xprico { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal xweight { get; set; }
        /// <summary>
        /// 赠送积分
        /// </summary>
        public decimal? xsendjf { get; set; }
        /// <summary>
        /// 换购积分
        /// </summary>
        public decimal xchagjf { get; set; }
        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string goodkind { get; set; }

        /// <summary>
        /// 规格1
        /// </summary>
        public string goodkind1 { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
        public string goodkind2 { get; set; }
        /// <summary>
        /// 规格3
        /// </summary>
        public string goodkind3 { get; set; }
        /// <summary>
        /// 规格4
        /// </summary>
        public string goodkind4 { get; set; }
        /// <summary>
        /// 规格5
        /// </summary>
        public string goodkind5 { get; set; }
        /// <summary>
        /// 规格6
        /// </summary>
        public string goodkind6 { get; set; }
        /// <summary>
        /// 规格7
        /// </summary>
        public string goodkind7 { get; set; }
        /// <summary>
        /// 规格8
        /// </summary>
        public string goodkind8 { get; set; }
        /// <summary>
        /// 规格9
        /// </summary>
        public string goodkind9 { get; set; }
        /// <summary>
        /// 规格10
        /// </summary>
        public string goodkind10 { get; set; }
        /// <summary>
        /// 多单位,示例：箱=12,件=6
        /// </summary>
        public string xmulunit { get; set; }
        /// <summary>
        /// 当前单位(条码单位)
        /// </summary>
        public string unitname { get; set; }
        /// <summary>
        /// 对换率
        /// </summary>
        public decimal unitrate { get; set; }

        public bool IsSelected { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public decimal xquatku { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 换购总积分
        /// </summary>
        public decimal totalxchagjf { get; set; }
    }
}
