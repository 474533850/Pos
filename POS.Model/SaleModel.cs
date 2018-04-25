using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 促销活动
    /// </summary>
    public class SaleModel : BaseModel
    {
        /// <summary>
        /// 类别
        /// </summary>
        public string xtype { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        /// <returns></returns>
        public string xname { get; set; }
        /// <summary>
        /// 应用方式
        /// </summary>
        public string xkind { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string xunit { get; set; }
        /// <summary>
        /// 活动规则
        /// </summary>
        public int? xrule { get; set; }
        /// <summary>
        /// 最多购买件数
        /// </summary>
        public string xhalfnum { get; set; }
        /// <summary>
        /// 商品范围
        /// </summary>
        public decimal xgood { get; set; }
        /// <summary>
        /// 计价规则
        /// </summary>
        public bool xbys { get; set; }
        /// <summary>
        /// 生效条件
        /// </summary>
        public string xdict { get; set; }
        /// <summary>
        /// 免邮条件
        /// </summary>
        public decimal xdictm { get; set; }
        /// <summary>
        /// 免邮类型
        /// </summary>
        public string xchssend { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        /// <returns></returns>
        public string xchsclss { get; set; }
        /// <summary>
        /// 会员类别
        /// </summary>
        /// <returns></returns>
        public string xchstype { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string xtime1 { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        public string xtime2 { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public bool xstart { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        /// <returns></returns>
        public string xinname { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        /// <returns></returns>
        public string xintime { get; set; }
        /// <summary>
        /// 分部代码
        /// </summary>
        /// <returns></returns>
        public string xls { get; set; }
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 游客可参与
        /// </summary>
        public bool xguest { get; set; }

        /// <summary>
        /// 促销赠品
        /// </summary>
        public List<SalegoodXModel> salegoodXs { get; set; }

        /// <summary>
        /// 促销商品
        /// </summary>
        public List<SalegoodModel> salegoods { get; set; }
        /// <summary>
        /// 活动规则
        /// </summary>
        public List<SaleruleModel> salerules { get; set; }


    }
}
