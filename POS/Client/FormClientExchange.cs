using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Model;
using POS.BLL;
using POS.Helper;
using System.Threading;
using POS.Common.Enum;
using POS.Common.utility;
using POS.Common;

namespace POS.Client
{
    public partial class FormClientExchange : BaseForm
    {
        GoodBLL goodBLL = new GoodBLL();
        ClientBLL clientBLL = new ClientBLL();
        POSBLL posBLL = new POSBLL();
        ClntModel clnt;
        Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
        //是否允许负库存开单
        bool NGKU_SALE = false;
        PossettingBLL possettingBLL = new PossettingBLL();
        public FormClientExchange(ClntModel clnt)
        {
            InitializeComponent();
            this.clnt = clnt;
            decimal jjie2 = clientBLL.GetJjie2(clnt.clntcode);
            lblIntegral.Text = string.Format("该会员拥有 {0} 个积分", jjie2);
            rluexquatku.DataSource = goodBLL.GetKu2(RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.cnkucode);
        }

        private void FormClientExchange_Load(object sender, EventArgs e)
        {
            bdsData.DataSource = goodBLL.GetExchangeGoods();

            chkPrint.Enabled = GetPermission(Functions.Print, false);

            PossettingModel entity = possettingBLL.GetPossettingByKey(AppConst.NGKU_SALE);
            if (entity != null)
            {
                int result = 0;
                if (int.TryParse(entity.xpvalue, out result))
                {
                    NGKU_SALE = result > 0;
                }
            }
        }

        private void gv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (gv.FocusedColumn == colIsSelected)
                {
                    GoodModel current = bdsData.Current as GoodModel;
                    if (current != null)
                    {
                        current.IsSelected = !current.IsSelected;
                        bdsData.ResetCurrentItem();
                    }
                }
                else
                {
                    if (e.Clicks == 2)
                    {
                        GoodModel current = bdsData.Current as GoodModel;
                        if (current != null)
                        {
                            FormClientExchangeQuery frm = new FormClientExchangeQuery(current.Quantity);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                current.Quantity = frm.Quantity;
                                current.totalxchagjf = CalcMoneyHelper.Multiply(current.Quantity, current.xchagjf);
                            }
                        }
                    }
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Cashier))
            {
                return;
            }
            if (CheckConnect())
            {
                decimal jjie2 = 0;
                try
                {
                    //SyncHelperBLL.SyncData(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.usercode, new List<string> { "jjie2" });
                    //Thread.Sleep(1000);
                    //jjie2 = clientBLL.GetJjie2(clnt.clntcode);
                    UserModel user = RuntimeObject.CurrentUser;
                    jjie2 = SyncHelperBLL.Getjifen(user.bookID, clnt.clntcode, user.xls);

                }
                catch (Exception)
                {
                    MessagePopup.ShowInformation("获取会员的积分失败,请检查网络！");
                }
                List<GoodModel> datas = bdsData.DataSource as List<GoodModel>;
                var query = (from p in datas where p.IsSelected select p).ToList();

                if (query.Count == 0)
                {
                    MessagePopup.ShowInformation("请选择要兑换的礼品！");
                }
                else if (query.Sum(r => r.totalxchagjf) > jjie2)
                {
                    MessagePopup.ShowInformation("你的积分不足兑换礼品！");
                }
                else
                {
                    foreach (var good in query)
                    {
                        List<Ku2Model> ku2 = rluexquatku.DataSource as List<Ku2Model>;
                        decimal xquatku = ku2.Where(r => r.key == good.key).Where(r => r.xquatku.HasValue).Sum(r => r.xquatku.Value);
                        if (!NGKU_SALE)
                        {
                            good.unitrate = 1;
                            if (good.unitname != null && good.unitname != good.goodunit)
                            {
                                //多单位
                                if (good.xmulunit != null)
                                {
                                    List<string> mulunits = good.xmulunit.Split(',').ToList();
                                    string mulunit = mulunits.Where(r => r.ToLower().Contains(good.unitname.ToLower())).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(mulunit))
                                    {
                                        string[] array = mulunit.Split('=');
                                        good.unitrate = decimal.Parse(array[1]);
                                    }
                                }
                            }

                            if (xquatku < good.unitrate)
                            {
                                MessagePopup.ShowInformation(string.Format("商品{0}：不允许负库存开单！", good.goodname));
                                return;
                            }
                        }
                    }
                    PoshhModel poshh = SetPoshh(datas);
                    string billno = string.Empty;
                    bool result = posBLL.AddPOS(poshh, out billno);
                    if (result)
                    {
                        string msg = AppConst.Sync_Cashier_Msg;
                        //实时同步单据抵扣会员积分
                        SyncData(new List<string> { "poshh", "jjie2" }, 0, msg);
                        if (chkPrint.Checked)
                        {
                            System.Threading.Thread.Sleep(1000);
                            poshh.billno = billno;
                            poshh.xintime = DateTime.Now.ToString();
                            poshh.xsendjf = 0;
                            PrintHelper.Print(poshh);
                        }
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessagePopup.ShowError("兑换礼品失败！");
                    }
                }
            }
        }

        #region 赋值POS单
        private PoshhModel SetPoshh(List<GoodModel> datas)
        {
            var query = datas.Where(r => r.IsSelected);
            PoshhModel entity = new PoshhModel();
            entity.posnono = RuntimeObject.CurrentUser.posnono;
            entity.xstate = stateDic[Enum.GetName(typeof(PosState), PosState.Deal)];
            entity.clntcode = clnt.clntcode;
            entity.clntname = clnt.clntname;
            entity.xpho = clnt.xpho;
            entity.xnote = string.Empty;
            entity.xls = RuntimeObject.CurrentUser.xls;
            entity.xlsname = RuntimeObject.CurrentUser.xlsname;
            entity.xinname = RuntimeObject.CurrentUser.username;
            entity.xheallp = query.Sum(r => r.xprico * r.Quantity);
            entity.xpay = 0;
            entity.xhezhe = entity.xheallp;
            entity.xhenojie = 0;

            List<PosbbModel> posbbs = new List<PosbbModel>();
            foreach (GoodModel good in query)
            {
                PosbbModel posbb = new PosbbModel();
                posbb.ID = Guid.NewGuid();
                posbb.goodcode = good.goodcode;
                posbb.xbarcode = good.xbarcode;
                posbb.goodname = good.goodname.Trim();
                posbb.goodunit = good.goodunit.Trim();
                posbb.goodtm = good.goodkind;
                posbb.goodkind1 = good.goodkind1;
                posbb.goodkind2 = good.goodkind2;
                posbb.goodkind3 = good.goodkind3;
                posbb.goodkind4 = good.goodkind4;
                posbb.goodkind5 = good.goodkind5;
                posbb.goodkind6 = good.goodkind6;
                posbb.goodkind7 = good.goodkind7;
                posbb.goodkind8 = good.goodkind8;
                posbb.goodkind9 = good.goodkind9;
                posbb.goodkind10 = good.goodkind10;
                posbb.cnkucode = RuntimeObject.CurrentUser.cnkucode;
                posbb.cnkuname = RuntimeObject.CurrentUser.cnkuname;
                posbb.xquat = good.Quantity;
                posbb.unitquat = good.Quantity;
                posbb.xpricold = good.xprico;
                posbb.unitrate = 1;
                posbb.unitname = good.goodunit.Trim();
                posbb.xzhe = 100;
                posbb.xpric = posbb.xpricold;
                posbb.xallp = CalcMoneyHelper.Multiply(posbb.xpric, posbb.xquat);
                posbb.xtaxr = 0;
                posbb.xtax = CalcMoneyHelper.Multiply(posbb.xallp, posbb.xtaxr);
                posbb.xallpt = CalcMoneyHelper.Add(posbb.xallp, posbb.xtax);
                posbb.xprict = CalcMoneyHelper.Divide(posbb.xallpt, posbb.xquat);
                posbb.xsalestype = "积分兑换";
                posbb.xpoints = good.totalxchagjf;
                posbbs.Add(posbb);
            }
            entity.Posbbs = posbbs;
            return entity;
        }
        #endregion
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string msg = AppConst.Sync_Cashier_Msg;
            SyncData(new List<string> { "poshh", "jjie2" }, 0, msg);
        }
    }
}