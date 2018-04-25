using POS.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace POS.Common
{
    public class AppConst
    {
        /// <summary>
        /// 当前数据库版本号
        /// </summary>1
        public static readonly string current_dbVersion = "1.0.0.5";
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public static readonly string ConfigName = "config.xml";
        /// <summary>
        /// 配置文件夹的路径
        /// </summary>
        public static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
        /// <summary>
        /// 配置文件所在的路径，包括文件名
        /// </summary>
        public static string ConfigFilePath = Path.Combine(ConfigPath, ConfigName);

        /// <summary>
        /// 配置数据库文件夹的路径
        /// </summary>
        public static readonly string dbMapPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbMap");

        /// <summary>
        /// 金额小数位数默认为2位
        /// </summary>
        public static int DecimalPlaces = 3;
        /// <summary>
        /// 是否四舍五入
        /// </summary>
        public static bool IsRound = true;
        /// <summary>
        /// 数据库名称
        /// </summary>
        public static readonly string dbName = "Pos.db3";
        /// <summary>
        /// 数据库存放目录
        /// </summary>
        public static readonly string sqliteDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db");
        /// <summary>
        /// 升级文件名称
        /// </summary>
        public static readonly string UpgradeFileName = "upgrade.zip";
        /// <summary>
        /// 升级文件存放目录
        /// </summary>
        public static readonly string UpgradeDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFolder");
        /// <summary>
        ///连接服务器提示信息
        /// </summary>
        //public static readonly string Connect_Msg = "访问远程服务器失败，请检查网络环境或本机运行环境！";
        public static readonly string Connect_Msg = "网络已断开，请检查网络！";
        /// <summary>
        ///同步收款单据提示
        /// </summary>
        public static readonly string Sync_Cashier_Msg = string.Format("你有收银订单未能及时上传服务器，{0}它将影响云端销售数据的统计或会员{0}的余额扣款，请及时检查网络并上传该单据！", Environment.NewLine);
        /// <summary>
        ///开启积分提示
        /// </summary>
        public static readonly string Open_integral_Msg = string.Format("你还没有开启积分兑换礼品功能，请登录云端后台{0}【电商】-【基础设置】-【电商参数设置】{0}-“积分兑换设置”，选择合适的兑换规则！", Environment.NewLine);

        #region 配置Key
        /// <summary>
        /// 是否选择班次登录
        /// </summary>
        public static readonly string HAS_BAN_TYPE = "HAS_BAN_TYPE";
        /// <summary>
        /// 商店名称
        /// </summary>
        public static readonly string Shop_Name = "Shop_Name";
        /// <summary>
        /// 商店地址
        /// </summary>
        public static readonly string Shop_Address = "Shop_Address";
        /// <summary>
        /// 客服电话
        /// </summary>
        public static readonly string Consumer_Pho = "Consumer_Pho";
        /// <summary>
        /// 同步数据类型 0、根据网络状态 1、手动
        /// </summary>
        public static readonly string SyncData_Type = "SyncData_Type";
        /// <summary>
        /// 网络检查时间间隔
        /// </summary>
        public static readonly string SyncData_TimeSpan = "SyncData_TimeSpan";
        /// <summary>
        /// 默认仓库
        /// </summary>
        public static readonly string DEF_CKC = "DEF_CKC";
        /// <summary>
        /// 打印规格
        /// </summary>
        public static readonly string Print_Kind = "Print_Kind";
        /// <summary>
        /// 打印张数
        /// </summary>
        public static readonly string Print_Num = "Print_Num";
        /// <summary>
        /// 是否客显
        /// </summary>
        public static readonly string Is_Customer_Display = "Is_Customer_Display";
        /// <summary>
        /// 客显地址
        /// </summary>
        public static readonly string Customer_Addr = "Customer_Addr";
        /// <summary>
        /// 波特率
        /// </summary>
        public static readonly string BaudRate = "BaudRate";
        /// <summary>
        /// 钱箱端口
        /// </summary>
        public static readonly string Cashbox_Port = "Cashbox_Port";
        /// <summary>
        /// 开钱箱指令
        /// </summary>
        public static readonly string Cashbox_Order = "Cashbox_Order";
        /// <summary>
        /// 同步数据删除最大的版本号
        /// </summary>
        public static readonly string SyncDel_MaxVersion = "SyncDel_MaxVersion";
        /// <summary>
        /// 同步数据POS单最大的版本号（用来记录修改）
        /// </summary>
        public static readonly string SyncPOS_MaxVersion = "SyncPOS_MaxVersion";
        /// <summary>
        /// 默认小票打印机
        /// </summary>
        public static readonly string DefaultPrinter = "DefaultPrinter";
        /// <summary>
        /// 凭证默认打印机
        /// </summary>
        public static readonly string DefaultVoucherPrinter = "DefaultVoucherPrinter";
        /// <summary>
        /// 贝壳设备号
        /// </summary>
        //public static readonly string Deviceid = "Deviceid";
        /// <summary>
        /// 贝壳机器号数据格式：{"分部代码":"机器号"}示例：{"YYD":"juntao001", "ZB":"juntao000"}
        /// </summary>
        public static readonly string PAYCORE_MACHINE_IDS = "PAYCORE_MACHINE_IDS";
        /// <summary>
        /// 收银是否打印小票
        /// </summary>
        public static readonly string IsPrintBill = "IsPrintBill";
        /// <summary>
        /// 密钥
        /// </summary>
        public static readonly string SecretKey = "SecretKey";
        /// <summary>
        /// 收银金额零头处理
        /// </summary>
        public static readonly string SCALE_MODE = "SCALE_MODE";
        /// <summary>
        /// 是否允许负库存开单
        /// </summary>
        public static readonly string NGKU_SALE = "NGKU_SALE";
        /// <summary>
        ///积分规则type : 1,//类型 0：不使用积分 1：按订单商品总额计算积分 2：为商品单独设置积分 3：按促销规则计算积分
        ///orderTick : 100,//订单金额每多少元赠送一个积分
        ///rate : 0.01,//1个积分可以兑换多少元预存款。
        ///minJfExch:100//最低兑换积分数
        ///maxJFExch //最大积分百分比
        /// </summary>
        public static readonly string INTEGRAL_RULES = "INTEGRAL_RULES";
        /// <summary>
        /// 移动支付地址
        /// </summary>
        public static readonly Uri payUrl = new Uri(ConfigurationManager.AppSettings["payUrl"].ToString());

        /// <summary>
        /// 权限
        /// {
        ///"店员组":{
        ///"permission":7,//权限
        ///"minDiscount":85//最低折扣
        ///"discounQuota":100//折让限额
        ///}
        /// </summary>
        public static readonly string POS_AUTHS = "POS_AUTHS";
        /// <summary>
        /// 支付通道 0：贝壳通道 1：原生通道
        /// </summary>
        public static readonly string PaymentChannel = "PaymentChannel";
        #endregion

        #region 所有的表名
        /// <summary>
        /// 所有的表名
        /// </summary>
        public static List<string> AllTableNames = new List<string>
        {
                "ls"
            ,"user"
            ,"cnku"
            ,"ku2"
            ,"cnku"
            ,"goodtype1"
            ,"goodtype2"
            ,"goodtype3"
            ,"goodkeys"
            ,"good"
            ,"goodpric"
            ,"goodpric2"
            ,"gbarcode"
            ,"goodkind"
            ,"clnttype"
            ,"clntclss"
            ,"clnt"
            ,"jjie2"
            ,"ojie2"
            ,"sale"
            ,"salegood"
            ,"salegoodX"
            ,"salerule"
            ,"poshh"
            ,"posbb"
            ,"posbantype"
            ,"posban"
            ,"possetting"
            ,"billpayt"
            ,"tickoff"
            ,"tickoffmx"
            ,"ofhh"
            ,"ofbb"
            ,"payt"
            //,"clntday"
            // ,"clntdrule"
        };
        #endregion

        #region 零头处理类型
        public static List<int> ScaleMode = new List<int>
        {
            //不抹零
            0,
            //四舍五入到角
            1,
            //四舍五入到元
            2,
            //抹掉零头分
            3,
            //抹掉零头角
            4
        };
        #endregion

        public static Dictionary<string, string> xchgDic = new Dictionary<string, string> { { "换进", "换进" }, { "换出", "换出" } };

        #region 数据字典
        /// <summary>
        /// 数据库表集合
        /// </summary>
        public static Dictionary<string, string> dicDB = new Dictionary<string, string> {
            //货品相关表
            { "goodtype1","货品大类"},
            { "goodtype2","货品中类"},
            { "goodtype3","货品小类"},
            { "goodpric","货品价格"},
            { "goodkeys","货品类型"},
            { "goodpric2","等级价格"},
            { "goodkind","货品规格"},
            { "gbarcode","一品多码"},
            { "good","货品信息"},
            //会员相关表
            { "clnt","顾客信息"},
            { "clnttype","顾客类别"},
            { "clntclss","顾客等级"},
            { "jjie2","积分结余"},
            { "ojie2","预存款结余"},
            { "ofhh","收款单表头"},
            { "ofbb","收款单表体"},
            //pos单
            { "poshh","POS单据表头"},
            { "posbb","POS单据表体"},
            { "posbantype","班次"},
            { "posban","当班记录"},
            { "billpayt","支付明细"},
            { "possetting","POS设置"},
            //促销活动
            { "sale","促销活动"},
            { "salegood","促销活动货品"},
            { "salegoodX","促销赠品"},
            { "salerule","活动规则"},
            //优惠券
            { "tickoff","线下优惠券"},
            { "tickoffmx","线下优惠券明细"},
             //分部及库存
            { "ls","分部信息"},
            { "user","用户信息"},
            { "ku2","当前进销存帐"},
            { "cnku","分部仓库"},
             //会员日
            { "clntday","会员日"},
            { "clntdrule","会员日规则"},
            { "clntdgood","会员日活动商品"}
        };
        public static Dictionary<string, List<DBStructModel>> dicDBStruct = new Dictionary<string, List<DBStructModel>>
        {
            #region  货品相关表
            //货品大类
            { "goodtype1",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype1", Caption="货品大类"},
                    new DBStructModel{ Column="uclssprics", Caption="级别折扣设置"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //货品中类
            { "goodtype2",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype1", Caption="货品大类"},
                    new DBStructModel{ Column="goodtype2", Caption="货品中类"},
                    new DBStructModel{ Column="uclssprics", Caption="级别折扣设置"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //货品小类
            { "goodtype3",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype1", Caption="货品大类"},
                    new DBStructModel{ Column="goodtype2", Caption="货品中类"},
                    new DBStructModel{ Column="goodtype3", Caption="货品小类"},
                    new DBStructModel{ Column="uclssprics", Caption="级别折扣设置"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //货品价格
            { "goodpric",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="xpric", Caption="价格"},
                    new DBStructModel{ Column="goodkind1", Caption="规格1"},
                    new DBStructModel{ Column="goodkind2", Caption="规格2"},
                    new DBStructModel{ Column="goodkind3", Caption="规格3"},
                    new DBStructModel{ Column="goodkind4", Caption="规格4"},
                    new DBStructModel{ Column="goodkind5", Caption="规格5"},
                    new DBStructModel{ Column="goodkind6", Caption="规格6"},
                    new DBStructModel{ Column="goodkind7", Caption="规格7"},
                    new DBStructModel{ Column="goodkind8", Caption="规格8"},
                    new DBStructModel{ Column="goodkind9", Caption="规格9"},
                    new DBStructModel{ Column="goodkind10", Caption="规格10"},
                    new DBStructModel{ Column="xchagjf", Caption="换购积分"},
                    new DBStructModel{ Column="xsendjf", Caption="赠送积分"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
             //货品类型
            { "goodkeys",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodkeys", Caption="货品类型"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
             //等级价格
            { "goodpric2",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="clntclss", Caption="顾客等级"},
                    new DBStructModel{ Column="xpric", Caption="价格"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //货品规格
            { "goodkind",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="xno", Caption="序号"},
                    new DBStructModel{ Column="goodkind", Caption="规格"},
                    new DBStructModel{ Column="goodkinds", Caption="规格选项"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //一品多码
            { "gbarcode",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="xbarcode", Caption="条形码"},
                    new DBStructModel{ Column="goodunit", Caption="单位"},
                    new DBStructModel{ Column="xtype", Caption="类型(通用, SKU)"},
                    new DBStructModel{ Column="xpric", Caption="条码价格"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //货品信息
            { "good",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype1", Caption="货品大类"},
                    new DBStructModel{ Column="goodtype2", Caption="货品中类"},
                    new DBStructModel{ Column="goodtype3", Caption="货品小类"},
                    new DBStructModel{ Column="goodcode", Caption="货品代码"},
                    new DBStructModel{ Column="goodname", Caption="货品名称"},
                    new DBStructModel{ Column="xjpiny", Caption="货品名称简拼"},
                    new DBStructModel{ Column="xqpiny", Caption="货品名称全拼"},
                    new DBStructModel{ Column="goodunit", Caption="货品单位"},
                    new DBStructModel{ Column="xmulunit", Caption="多单位"},
                    new DBStructModel{ Column="xshow", Caption="下架"},
                    new DBStructModel{ Column="goodkeys", Caption="货品类型"},
                    new DBStructModel{ Column="xprico", Caption="销售价格"},
                    new DBStructModel{ Column="xweight", Caption="重量"},
                    new DBStructModel{ Column="xsendjf", Caption="赠送积分"},
                    new DBStructModel{ Column="xchagjf", Caption="换购积分"},
                    new DBStructModel{ Column="xls", Caption="分部代码"},
                    new DBStructModel{ Column="xlsname", Caption="分部名称"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            #endregion 货品相关表结束

            #region 会员相关表
             //顾客信息
            { "clnt",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="clnttype", Caption="顾客类别"},
                    new DBStructModel{ Column="clntcode", Caption=" 顾客代码"},
                    new DBStructModel{ Column="clntname", Caption="顾客名称"},
                    new DBStructModel{ Column="clntclss", Caption="顾客等级"},
                    new DBStructModel{ Column="clntcodep", Caption="上级顾客代码"},
                    new DBStructModel{ Column="clntnamep", Caption="上级顾客名称"},
                    new DBStructModel{ Column="xuser", Caption="用户代码"},
                    new DBStructModel{ Column="xlsj ", Caption="所属经销商代码"},
                    new DBStructModel{ Column="xlsnamej", Caption="所属经销商名称"},
                    new DBStructModel{ Column="xmaill", Caption="电子邮箱"},
                    new DBStructModel{ Column="xpho", Caption="手机号码"},
                    new DBStructModel{ Column="xname", Caption="姓名"},
                    new DBStructModel{ Column="xsex", Caption="性别"},
                    new DBStructModel{ Column="xbro", Caption="出生日期"},
                    new DBStructModel{ Column="xarea1", Caption="一级地区"},
                    new DBStructModel{ Column="xarea2", Caption="二级地区"},
                    new DBStructModel{ Column="xarea3", Caption="三级地区"},
                    new DBStructModel{ Column="xadd", Caption="详细地址"},
                    new DBStructModel{ Column="xqq", Caption="联系QQ"},
                    new DBStructModel{ Column="xmyidx", Caption="身份证号"},
                    new DBStructModel{ Column="xistelcheck", Caption="已短信验证"},
                    new DBStructModel{ Column="xisemlcheck", Caption="已邮箱验证"},
                    new DBStructModel{ Column="xwxid", Caption="微信OpenID"},
                    new DBStructModel{ Column="xstop", Caption="禁用"},
                    new DBStructModel{ Column="clntcodej", Caption="介绍顾客代码"},
                    new DBStructModel{ Column="clntnamej", Caption="介绍顾客名称"},
                    new DBStructModel{ Column="xlpath", Caption="层级路径"},
                    new DBStructModel{ Column="xlnum", Caption="层级数"},
                    new DBStructModel{ Column="xinname", Caption="录入人"},
                    new DBStructModel{ Column="xintime", Caption="录入时间"},
                    new DBStructModel{ Column="xlastlogintime", Caption="最后登录时间"},
                    new DBStructModel{ Column="xls", Caption="分部代码"},
                    new DBStructModel{ Column="xlsname", Caption="分部名称"},
                    new DBStructModel{ Column="sharecode", Caption="分享码"},
                    new DBStructModel{ Column="password", Caption="登录密码"},
                    new DBStructModel{ Column="xnotes", Caption="备注"},
                    new DBStructModel{ Column="xcontime", Caption="消费次数"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //顾客类别
            { "clnttype",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="clnttype", Caption="顾客类别"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
             //顾客等级
            { "clntclss",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="clnttype", Caption="顾客类别"},
                    new DBStructModel{ Column="clntclss", Caption="顾客等级"},
                    new DBStructModel{ Column="xzhe", Caption="折扣百分比"},
                    new DBStructModel{ Column="xdefaul", Caption="是否默认"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
             //积分结余
            { "jjie2",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="clntcode" ,Caption="顾客代码"},
                    new DBStructModel{ Column="clntname" ,Caption="顾客名称"},
                    new DBStructModel{ Column="xlast" ,Caption="上期结余"},
                    new DBStructModel{ Column="xdo" ,Caption="本期消费"},
                    new DBStructModel{ Column="xpay", Caption="本期积分"},
                    new DBStructModel{ Column="xjie", Caption="期末结余"},
                    new DBStructModel{ Column="xtableid" ,Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid" ,Caption="XSUBID"},
                    new DBStructModel{ Column="xversion" ,Caption="数据版本"},
                }
            },
            //预存款结余
            { "ojie2",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="clntcode",Caption="顾客代码"},
                    new DBStructModel{ Column="clntname",Caption="顾客名称"},
                    new DBStructModel{ Column="xlast",Caption="上期结余金额"},
                    new DBStructModel{ Column="xpay",Caption="收款金额"},
                    new DBStructModel{ Column="xhk",Caption="划扣金额"},
                    new DBStructModel{ Column="xdo",Caption="销售金额"},
                    new DBStructModel{ Column="xzhe",Caption="折让金额"},
                    new DBStructModel{ Column="xjie",Caption="期末结余金额"},
                    new DBStructModel{ Column="xxflast",Caption="上期消费"},
                    new DBStructModel{ Column="xxfnow",Caption="本期消费"},
                    new DBStructModel{ Column="xxfjie",Caption="期末消费"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            //收款单表头
            { "ofhh",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xdate",Caption="收款日期"},
                    new DBStructModel{ Column="clntcode",Caption="顾客代码"},
                    new DBStructModel{ Column="clntname",Caption="顾客名称"},
                    new DBStructModel{ Column="xnote",Caption="备注"},
                    new DBStructModel{ Column="xinname",Caption="录入人"},
                    new DBStructModel{ Column="xintime",Caption="录入时间"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            //收款单表体
            { "ofbb",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xnoteb",Caption="摘要"},
                    new DBStructModel{ Column="xztype",Caption="支付方式"},
                    new DBStructModel{ Column="xzstate",Caption="支付状态"},
                    new DBStructModel{ Column="paybillno",Caption="支付单号"},
                    new DBStructModel{ Column="xfee",Caption="收款金额"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            #endregion 会员相关表结束

            #region pos单相关表
             //POS单据表头
            { "poshh",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="billno",Caption="单号"},
                    new DBStructModel{ Column="posnono",Caption="当班单号"},
                    new DBStructModel{ Column="xtype",Caption="单据类别"},
                    new DBStructModel{ Column="xstate",Caption="状态"},
                    new DBStructModel{ Column="xdate",Caption="日期"},
                    new DBStructModel{ Column="paytype",Caption="支付方式"},
                    new DBStructModel{ Column="transno",Caption="交易流水号"},
                    new DBStructModel{ Column="clntcode",Caption="顾客代码"},
                    new DBStructModel{ Column="clntname",Caption="顾客名称"},
                    new DBStructModel{ Column="xheallp",Caption="货款金额"},
                    new DBStructModel{ Column="xpay",Caption="付款金额"},
                    new DBStructModel{ Column="xrpay",Caption="舍去金额"},
                    new DBStructModel{ Column="xhezhe",Caption="折让金额"},
                    new DBStructModel{ Column="xhejie",Caption="结算金额"},
                    new DBStructModel{ Column="xnowzhe",Caption="结算折让"},
                    new DBStructModel{ Column="xhenojie",Caption="欠付金额"},
                    new DBStructModel{ Column="xnote",Caption="摘要"},
                    new DBStructModel{ Column="workcode",Caption="职员代码"},
                    new DBStructModel{ Column="workname",Caption="职员名称"},
                    new DBStructModel{ Column="xls",Caption="分部代码"},
                    new DBStructModel{ Column="xlsname",Caption="分部名称"},
                    new DBStructModel{ Column="xinname",Caption="录入人"},
                    new DBStructModel{ Column="xintime",Caption="录入时间"},
                    new DBStructModel{ Column="xhe",Caption="已审"},
                    new DBStructModel{ Column="xheman",Caption="审核人"},
                    new DBStructModel{ Column="xhetime",Caption="审核时间"},
                    new DBStructModel{ Column="xterm",Caption="结算期间"},
                    new DBStructModel{ Column="pbillno",Caption="上级单号"},
                    new DBStructModel{ Column="xpoints",Caption="支付积分数"},
                    new DBStructModel{ Column="xsendjf",Caption="赠送积分数"},
                }
            },
            //POS单据表体
            { "posbb",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="goodcode",Caption="货品代码"},
                    new DBStructModel{ Column="goodname",Caption="货品名称"},
                    new DBStructModel{ Column="goodtm",Caption="货品规格"},
                    new DBStructModel{ Column="goodunit",Caption="货品单位"},
                    new DBStructModel{ Column="unitname",Caption="当前单位"},
                    new DBStructModel{ Column="unitrate",Caption="对换率"},
                    new DBStructModel{ Column="unitquat",Caption="当前数量"},
                    new DBStructModel{ Column="goodkind1",Caption="规格1"},
                    new DBStructModel{ Column="goodkind2",Caption="规格2"},
                    new DBStructModel{ Column="goodkind3",Caption="规格3"},
                    new DBStructModel{ Column="goodkind4",Caption="规格4"},
                    new DBStructModel{ Column="goodkind5",Caption="规格5"},
                    new DBStructModel{ Column="goodkind6",Caption="规格6"},
                    new DBStructModel{ Column="goodkind7",Caption="规格7"},
                    new DBStructModel{ Column="goodkind8",Caption="规格8"},
                    new DBStructModel{ Column="goodkind9",Caption="规格9"},
                    new DBStructModel{ Column="goodkind10",Caption="规格10"},
                    new DBStructModel{ Column="goodgive",Caption="组装包"},
                    new DBStructModel{ Column="cnkucode",Caption="仓库代码"},
                    new DBStructModel{ Column="cnkuname",Caption="仓库名称"},
                    new DBStructModel{ Column="xquat",Caption="数量"},
                    new DBStructModel{ Column="xtquat",Caption="已退数量"},
                    new DBStructModel{ Column="xpricold",Caption="单价"},
                    new DBStructModel{ Column="xzhe",Caption="折扣"},
                    new DBStructModel{ Column="xpric",Caption="折后单价"},
                    new DBStructModel{ Column="xallp",Caption="金额"},
                    new DBStructModel{ Column="xtaxr",Caption="税率"},
                    new DBStructModel{ Column="xtax",Caption="税额"},
                    new DBStructModel{ Column="xprict",Caption="含税单价"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
             //班次
            { "posbantype",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="posbcode",Caption="班次"},
                    new DBStructModel{ Column="xtime1",Caption="起始时间"},
                    new DBStructModel{ Column="xtime2",Caption="终止时间"},
                    new DBStructModel{ Column="xnote",Caption="说明"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},

                }
            },
             //交班记录
            { "posban",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xposset",Caption="POS类别"},
                    new DBStructModel{ Column="posnono",Caption="当班单号"},
                    new DBStructModel{ Column="posposi",Caption="门店（分部代码）"},
                    new DBStructModel{ Column="posbcode",Caption="班次"},
                    new DBStructModel{ Column="xopcode",Caption="收银员代码"},
                    new DBStructModel{ Column="xopname",Caption="收银员"},
                    new DBStructModel{ Column="xtime1",Caption="当班开始时间"},
                    new DBStructModel{ Column="xtime2",Caption="当班结束时间"},
                    new DBStructModel{ Column="xjielst",Caption="上次结余"},
                    new DBStructModel{ Column="xjiepos",Caption="当班收入"},
                    new DBStructModel{ Column="xjienow",Caption="当前备用金"},
                    new DBStructModel{ Column="xjiehav",Caption="结留现金"},
                    new DBStructModel{ Column="xjieget",Caption="结转现金"},
                    new DBStructModel{ Column="xjieok",Caption="已结"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},

                }
            },
            //支付明细
            { "billpayt",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="paytcode",Caption="帐户代码"},
                    new DBStructModel{ Column="paytname",Caption="帐户名称"},
                    new DBStructModel{ Column="xpay",Caption="金额"},
                    new DBStructModel{ Column="xnote1",Caption="说明一"},
                    new DBStructModel{ Column="xnote2",Caption="说明二"},
                    new DBStructModel{ Column="billflag",Caption="单标志"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            //pos设置
            { "possetting",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="issys",Caption="是否为全局系统参数"},
                    new DBStructModel{ Column="xpname",Caption="参数名称"},
                    new DBStructModel{ Column="xpvalue",Caption="参数值"},
                    new DBStructModel{ Column="usercode",Caption="用户代码"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            #endregion pos单关表结束

            #region  促销活动相关表
            //促销活动
            { "sale",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype1", Caption="货品大类"},
                    new DBStructModel{ Column="uclssprics", Caption="级别折扣设置"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //促销活动货品
            { "salegood",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype1", Caption="货品大类"},
                    new DBStructModel{ Column="goodtype2", Caption="货品中类"},
                    new DBStructModel{ Column="uclssprics", Caption="级别折扣设置"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //促销赠品
            { "salegoodX",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype1", Caption="货品大类"},
                    new DBStructModel{ Column="goodtype2", Caption="货品中类"},
                    new DBStructModel{ Column="goodtype3", Caption="货品小类"},
                    new DBStructModel{ Column="uclssprics", Caption="级别折扣设置"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
             //活动规则
            { "salerule",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xhave",Caption="满"},
                    new DBStructModel{ Column="xdo",Caption="打"},
                    new DBStructModel{ Column="xfen",Caption="积分"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="对应 sale 表的 XTABLEID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            #endregion 促销相关表结束

            #region  优惠券相关表
            //线下优惠券
            { "tickoff",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xname",Caption="名称"},
                    new DBStructModel{ Column="xallp",Caption="面额"},
                    new DBStructModel{ Column="xcount",Caption="数量"},
                    new DBStructModel{ Column="xnotime",Caption="无时间限制"},
                    new DBStructModel{ Column="xtime1",Caption="开始时间"},
                    new DBStructModel{ Column="xtime2",Caption="结束时间"},
                    new DBStructModel{ Column="xafter",Caption="多少天后使用"},
                    new DBStructModel{ Column="xend",Caption="使用终端"},
                    new DBStructModel{ Column="goodtype",Caption="商品类别"},
                    new DBStructModel{ Column="goodbank",Caption="商品品牌"},
                    new DBStructModel{ Column="xminallp",Caption="最低金额"},
                    new DBStructModel{ Column="xinname",Caption="录入人"},
                    new DBStructModel{ Column="xintime",Caption="录入时间"},
                    new DBStructModel{ Column="xnote",Caption="说明"},
                    new DBStructModel{ Column="xls",Caption="分部代码"},
                    new DBStructModel{ Column="xlsname",Caption="分部名称"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},

                }
            },
             //线下优惠券明细
            { "tickoffmx",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xcode",Caption="编码"},
                    new DBStructModel{ Column="xname",Caption="名称"},
                    new DBStructModel{ Column="xallp",Caption="面额"},
                    new DBStructModel{ Column="clntcode",Caption="顾客代码"},
                    new DBStructModel{ Column="clntname",Caption="顾客名称"},
                    new DBStructModel{ Column="xnotime",Caption="无时间限制"},
                    new DBStructModel{ Column="xtime1",Caption="开始时间"},
                    new DBStructModel{ Column="xtime2",Caption="结束时间"},
                    new DBStructModel{ Column="xafter",Caption="多少天后使用"},
                    new DBStructModel{ Column="xend",Caption="使用终端"},
                    new DBStructModel{ Column="xminallp",Caption="最低金额"},
                    new DBStructModel{ Column="xusetime",Caption="领取时间"},
                    new DBStructModel{ Column="xinname",Caption="制作人"},
                    new DBStructModel{ Column="xintime",Caption="制作时间"},
                    new DBStructModel{ Column="xstate",Caption="状态"},
                    new DBStructModel{ Column="xopusetime",Caption="使用时间"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="对应 tickoff 表 XTABLEID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            #endregion 优惠券相关表结束

            #region  分部及库存相关表
            //分部信息
            { "ls",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xlstype",Caption="类别"},
                    new DBStructModel{ Column="xls",Caption="代码"},
                    new DBStructModel{ Column="xlsname",Caption="名称"},
                    new DBStructModel{ Column="xlsp",Caption="上级代码"},
                    new DBStructModel{ Column="xlsnamep",Caption="上级名称"},
                    new DBStructModel{ Column="xdotype",Caption="性质"},
                    new DBStructModel{ Column="xstate",Caption="状态"},
                    new DBStructModel{ Column="lsclass",Caption="等级"},
                    new DBStructModel{ Column="xpost1",Caption="省"},
                    new DBStructModel{ Column="xpost2",Caption="市"},
                    new DBStructModel{ Column="xpost3",Caption="区县"},
                    new DBStructModel{ Column="xaddr",Caption="详细地址"},
                    new DBStructModel{ Column="xfax",Caption="传真"},
                    new DBStructModel{ Column="xinname",Caption="录入人"},
                    new DBStructModel{ Column="xintime",Caption="录入时间"},
                    new DBStructModel{ Column="xlastlogintime",Caption="最后登录时间"},
                    new DBStructModel{ Column="xjytime1",Caption="经营开始时间"},
                    new DBStructModel{ Column="xjytime2",Caption="经营结束时间"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            //用户信息
            { "user",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="usercode",Caption="用户代码"},
                    new DBStructModel{ Column="username",Caption="用户名称"},
                    new DBStructModel{ Column="password",Caption="登录密码"},
                    new DBStructModel{ Column="xls",Caption="分部代码"},
                    new DBStructModel{ Column="xlsname",Caption="分部名称"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},

                }
            },
            //当前进销存帐
            { "ku2",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID", Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype1", Caption="货品大类"},
                    new DBStructModel{ Column="goodtype2", Caption="货品中类"},
                    new DBStructModel{ Column="goodtype3", Caption="货品小类"},
                    new DBStructModel{ Column="uclssprics", Caption="级别折扣设置"},
                    new DBStructModel{ Column="xtableid", Caption="xtableid"},
                    new DBStructModel{ Column="xsubid", Caption="xsubid"},
                    new DBStructModel{ Column="xversion", Caption="数据版本" }
                }
            },
            //分部仓库
            { "cnku",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xls",Caption="分部代码"},
                    new DBStructModel{ Column="xlsname",Caption="分部名称"},
                    new DBStructModel{ Column="cnkucode",Caption="仓库代码"},
                    new DBStructModel{ Column="cnkuname",Caption="仓库名称"},
                    new DBStructModel{ Column="goodcode",Caption="货品代码"},
                    new DBStructModel{ Column="goodname",Caption="货品名称"},
                    new DBStructModel{ Column="goodunit",Caption="货品单位"},
                    new DBStructModel{ Column="goodkind1",Caption="规格1"},
                    new DBStructModel{ Column="goodkind2",Caption="规格2"},
                    new DBStructModel{ Column="goodkind3",Caption="规格3"},
                    new DBStructModel{ Column="goodkind4",Caption="规格4"},
                    new DBStructModel{ Column="goodkind5",Caption="规格5"},
                    new DBStructModel{ Column="goodkind6",Caption="规格6"},
                    new DBStructModel{ Column="goodkind7",Caption="规格7"},
                    new DBStructModel{ Column="goodkind8",Caption="规格8"},
                    new DBStructModel{ Column="goodkind9",Caption="规格9"},
                    new DBStructModel{ Column="goodkind10",Caption="规格10"},
                    new DBStructModel{ Column="xpricqc",Caption="期初单价"},
                    new DBStructModel{ Column="xquatqc",Caption="期初数量"},
                    new DBStructModel{ Column="xallpqc",Caption="期初金额"},
                    new DBStructModel{ Column="xpricin",Caption="进货单价"},
                    new DBStructModel{ Column="xquatin",Caption="进货数量"},
                    new DBStructModel{ Column="xallpin",Caption="进货金额"},
                    new DBStructModel{ Column="xpricot",Caption="销售单价"},
                    new DBStructModel{ Column="xquatot",Caption="销售数量"},
                    new DBStructModel{ Column="xallpot",Caption="销售金额"},
                    new DBStructModel{ Column="xchenot",Caption="销售成本"},
                    new DBStructModel{ Column="xlirnot",Caption="销售毛润"},
                    new DBStructModel{ Column="xpricku",Caption="库存单价"},
                    new DBStructModel{ Column="xquatku",Caption="库存数量"},
                }
            },
            #endregion 分部及库存相关表结束

            #region  会员日相关表
            //会员日
            { "clntday",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xday",Caption="会员日（每月几号）"},
                    new DBStructModel{ Column="xmonth",Caption="所在月份"},
                    new DBStructModel{ Column="xstart",Caption="启用"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID"},
                    new DBStructModel{ Column="xnote",Caption="备注"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            //会员日规则
            { "clntdrule",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="goodtype",Caption="货品分类"},
                    new DBStructModel{ Column="uclssprics",Caption="级别折扣设置"},
                    new DBStructModel{ Column="xtimes",Caption="n倍积分"},
                    new DBStructModel{ Column="classtype",Caption="分类类型 (大类、中类、小类)"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID 关联clntday表的 xsubid"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            //会员日活动商品
            { "clntdgood",new List<DBStructModel>
                {
                    new DBStructModel{ Column="SID",Caption="原数据ID"},
                    new DBStructModel{ Column="xgtype",Caption="类型(a:所有SKU, s:单个SKU)"},
                    new DBStructModel{ Column="xgoodid",Caption="xgtype为 a 时对应 good 表的XTABLEID,为 s 时对应goodpric表的XTABLEID"},
                    new DBStructModel{ Column="xzhe",Caption="折扣"},
                    new DBStructModel{ Column="xpric",Caption="折后单价"},
                    new DBStructModel{ Column="xtableid",Caption="XTABLEID"},
                    new DBStructModel{ Column="xsubid",Caption="XSUBID 关联clntday表的 xsubid"},
                    new DBStructModel{ Column="xversion",Caption="数据版本"},
                }
            },
            #endregion 会员日相关表结束
        };
        #endregion



    }
}
