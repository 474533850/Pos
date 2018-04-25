using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.BLL;
using POS.Model;
using POS.Helper;
using POS.Common.utility;
using POS.Common.Enum;
using POS.Common;
using DevExpress.XtraBars.Alerter;
using System.Web.Script.Serialization;

namespace POS.Sale
{
    public partial class FormRefund : BaseForm
    {
        PaytBLL paytBLL = new PaytBLL();
        Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
        PoshhModel poshh;
        PoshhModel prePoshh = null;
        decimal diff = 0;
        POSBLL posBLL = new POSBLL();
        PossettingBLL possettingBLL = new PossettingBLL();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Form preForm;
        public FormRefund(PoshhModel poshh, PoshhModel prePoshh, decimal diff, Form preForm)
        {
            InitializeComponent();
            this.poshh = poshh;
            this.poshh.xheallp = 0 - this.poshh.xheallp;
            this.poshh.xpay = 0 - this.poshh.xpay;
            this.prePoshh = prePoshh;
            this.diff = diff;
            this.preForm = preForm;
        }

        private void FormRefund_Load(object sender, EventArgs e)
        {
            List<PaytModel> payts = paytBLL.GetPayt().Where(r => r.payttype.Trim() == "刷卡").ToList();
            Dictionary<string, string> dicPays = new Dictionary<string, string>();
            dicPays.Add(payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)], "现金");
            foreach (var item in payts)
            {
                dicPays.Add(item.paytname, item.paytname);
            }

            luePayt.Properties.DataSource = dicPays;
            luePayt.ItemIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (luePayt.EditValue == null)
            {
                MessagePopup.ShowInformation("请选择退款账户！");
                return;
            }
            //确认返还积分
            if (poshh.clntcode != null && !string.IsNullOrEmpty(poshh.clntcode))
            {
                int jf = ReturnJF(poshh.Posbbs);
                FormReturnJF frmReturnJF = new FormReturnJF(jf);
                poshh.xsendjf  =0- frmReturnJF.JF; ;
                if (frmReturnJF.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
            KeyValuePair<string, string> pay = (KeyValuePair<string, string>)luePayt.GetSelectedDataRow();

            List<BillpaytModel> billpaytList = new List<BillpaytModel>();
            if (pay.Key == payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)])
            {
                BillpaytModel entity = new BillpaytModel();
                entity.paytcode = string.Empty;
                entity.paytname = pay.Key;
                entity.xreceipt = diff;
                entity.xpay = diff;
                entity.billflag = "pos";
                billpaytList.Add(entity);
            }
            else
            {
                BillpaytModel entity = new BillpaytModel();
                entity.paytcode = string.Empty;
                entity.paytname = pay.Key;
                entity.xnote1 = payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)];
                entity.xpay = diff;
                entity.billflag = "pos";
                billpaytList.Add(entity);
            }
            poshh.payts = billpaytList;
            //if (prePoshh != null)
            //{
            //    if (prePoshh.payts != null)
            //    {
            //        poshh.payts.AddRange(prePoshh.payts);
            //    }
            //}
            string billno = string.Empty;
            //付款
            bool result = posBLL.ChangeGoods(poshh, prePoshh.ID, out billno);
            if (result)
            {
                SyncPOS(preForm);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessagePopup.ShowError("开单失败！");
            }
        }

        #region 退货返还积分
        /// <summary>
        ///积分规则type : 1,//类型 0：不使用积分 1：按订单商品总额计算积分 2：为商品单独设置积分 3：按促销规则计算积分
        ///orderTick : 100,//订单金额每多少元赠送一个积分
        ///rate : 0.01,//1个积分可以兑换多少元预存款。
        ///minJfExch:100//最低兑换积分数
        /// </summary>
        /// <returns></returns>
        private int ReturnJF(List<PosbbModel> posbbs)
        {
            int jf = 0;
            PossettingModel possetting = possettingBLL.GetPossettingByKey(AppConst.INTEGRAL_RULES);
            if (possetting != null)
            {
                if (!string.IsNullOrEmpty(possetting.xpvalue))
                {
                    Dictionary<object, object> uclsspricsDic = js.Deserialize<Dictionary<object, object>>(possetting.xpvalue);
                    var query = uclsspricsDic.Where(r => r.Key.ToString() == "type").FirstOrDefault();
                    decimal value = 0;
                    if (decimal.TryParse(query.Value.ToString(), out value))
                    {
                        if (value == 1)
                        {
                            //按订单商品总额计算积分
                            query = uclsspricsDic.Where(r => r.Key.ToString() == "orderTick").FirstOrDefault();
                            if (decimal.TryParse(query.Value.ToString(), out value))
                            {
                                if (value != 0)
                                {
                                    decimal total =Math.Abs(posbbs.Where(r=>r.xchg=="换出").Sum(r => Math.Abs(r.unitquat * r.xpric))- posbbs.Where(r => r.xchg == "换进").Sum(r => Math.Abs(r.unitquat * r.xpric)));
                                    jf = (int)CalcMoneyHelper.Divide(total, value);
                                }
                            }
                        }
                        else if (value == 2)
                        {
                            //为商品单独设置积分
                            jf = (int)posbbs.Where(r => r.xsendjf.HasValue && r.xchg=="换出").Sum(r => r.xsendjf * r.unitquat * r.unitrate)- (int)posbbs.Where(r => r.xsendjf.HasValue && r.xchg == "换进").Sum(r => r.xsendjf * r.unitquat * r.unitrate);

                        }
                    }
                }
            }
            return jf;
        }
        #endregion

        private void FormRefund_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                btnConfirm_Click(null,null);
            }
        }
    }
}