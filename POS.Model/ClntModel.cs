using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    /// <summary>
    /// 顾客信息(clnt)
    /// </summary>
    public class ClntModel : BaseModel
    {
        /// <summary>
        /// 顾客类别
        /// </summary>
        public string clnttype { get; set; }
        /// <summary>
        /// 顾客代码
        /// </summary>
        public string clntcode { get; set; }
        /// <summary>
        /// 顾客名称
        /// </summary>
        public string clntname { get; set; }
        /// <summary>
        /// 顾客等级
        /// </summary>
        public string clntclss { get; set; }
        /// <summary>
        /// 上级顾客代码
        /// </summary>
        public string clntcodep { get; set; }
        /// <summary>
        /// 上级顾客名称
        /// </summary>
        public string clntnamep { get; set; }
        /// <summary>
        /// 用户代码
        /// </summary>
        public string xuser { get; set; }
        /// <summary>
        /// 所属经销商代码
        /// </summary>
        public string xlsj { get; set; }
        /// <summary>
        /// 所属经销商名称
        /// </summary>
        public string xlsnamej { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string xmaill { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string xpho { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string xname { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>

        public DateTime? xbro { get; set; }
        /// <summary>
        /// 一级地区
        /// </summary>
        public string xarea1 { get; set; }
        /// <summary>
        /// 二级地区
        /// </summary>
        public string xarea2 { get; set; }
        /// <summary>
        /// 三级地区
        /// </summary>
        public string xarea3 { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string xadd { get; set; }
        /// <summary>
        /// 联系QQ
        /// </summary>
        public string xqq { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string xmyidx { get; set; }
        /// <summary>
        /// 已短信验证
        /// </summary>
        public bool xistelcheck { get; set; }
        /// <summary>
        /// 已邮箱验证
        /// </summary>
        public bool xisemlcheck { get; set; }
        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string xwxid { get; set; }
        /// <summary>
        /// 禁用
        /// </summary>
        public bool xstop { get; set; }
        /// <summary>
        /// 介绍顾客代码
        /// </summary>
        public string clntcodej { get; set; }
        /// <summary>
        /// 介绍顾客名称
        /// </summary>
        public string clntnamej { get; set; }
        /// <summary>
        /// 层级路径
        /// </summary>
        public string xlpath { get; set; }
        /// <summary>
        /// 层级数
        /// </summary>
        public int xlnum { get; set; }
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
        /// 分部代码
        /// </summary>
        public string xls { get; set; }
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 分享码
        /// </summary>
        public string sharecode { get; set; }
        /// <summary>
        /// 顾客密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string xnotes { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal xzhe { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public decimal integral { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal balance { get; set; }
        /// <summary>
        /// 消费次数
        /// </summary>
        public int xcontime { get; set; }
        /// <summary>
        /// 是否修改
        /// </summary>
        public bool isUpdate { get; set; }
    }
}
