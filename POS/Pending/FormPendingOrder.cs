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
using POS.Common.Enum;
using POS.Common.utility;

namespace POS.Pending
{
    public partial class FormPendingOrder : BaseForm
    {
        POSBLL posBLL = new POSBLL();
        ClientBLL clientBLL = new ClientBLL();
        /// <summary>
        /// 当前的单据
        /// </summary>
        public PoshhModel CurrentPoshh { get; set; }
        public OperationState CurrentOperationState { get; set; }

        private Guid? id;

        public FormPendingOrder(Guid? id)
        {
            InitializeComponent();
            this.id = id;
            CurrentOperationState = OperationState.None;
            Init();
            rpicOperation.Click += RpicOperation_Click;

        }

        private void Init()
        {
            List < PoshhModel > datas = posBLL.GetPOSPending();
            if (id.HasValue)
            {
                datas = datas.Where(r => r.ID == id).ToList();
            }
            bdsPos.DataSource = datas;

            //btnPrint.Visible = bdsPos.List.Count > 0;
            btnInvalid.Visible = bdsPos.List.Count > 0;
            btnAdd.Visible = bdsPos.List.Count > 0;
            btnReceipt.Visible = bdsPos.List.Count > 0;

            Clear();
        }

        #region 选择一张单据
        private void gvOrder_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                PoshhModel entity = posBLL.GetPOSByID(current.ID);
                lblquat.Text = entity.Posbbs.Sum(r => r.xquat).ToString();
                lblUserName.Text = entity.xinname;
                lblTotalMoney.Text = entity.xpay.ToString();
                lblClientName.Text = entity.clntname;
                lblRemark.Text = entity.xnote;

                if (!string.IsNullOrEmpty(entity.clntcode))
                {
                    ClntModel client = clientBLL.GetClientByCode(entity.clntcode);
                    decimal ojie2 = clientBLL.GetOjie2(entity.clntcode);
                    lblBalance.Text = ojie2.ToString();
                    lblxpho.Text = client.xpho;
                    lblAddress.Text = client.xadd;
                }
                else
                {
                    lblBalance.Text = string.Empty;
                    lblxpho.Text = string.Empty;
                    lblAddress.Text = string.Empty;
                }

                bdsDetail.DataSource = entity.Posbbs;
            }
            else
            {
                Clear();
            }
        }
        #endregion

        #region 设置操作按钮的图片
        private void gvDetail_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column == colOperation && e.IsGetData)
            {
                e.Value = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/remove_16x16.png");
            }
        }
        #endregion

        #region 点击操作按钮减掉商品
        private void RpicOperation_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.EClnt))
            {
                return;
            }
            if (MessagePopup.ShowQuestion("确认减掉本商品？") == DialogResult.Yes)
            {
                PosbbModel current = bdsDetail.Current as PosbbModel;
                if (current != null)
                {
                    try
                    {
                        current.xquat -= 1;
                        current.xallp = CalcMoneyHelper.Multiply(current.xpric, current.xquat);
                        Update(current);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region 改变价格或者数量
        private void gvDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column == colxpric || e.Column == colxquat)
            {
                PosbbModel current = bdsDetail.Current as PosbbModel;
                if (current != null)
                {
                    decimal quantity = current.xquat;
                    if (e.Column == colxquat)
                    {
                        if (decimal.TryParse(e.Value.ToString(), out quantity))
                        {
                            if (e.Value != null && decimal.Parse(e.Value.ToString()) == 0)
                            {
                                if (!GetPermission(Functions.EClnt))
                                {
                                    return;
                                }
                                if (MessagePopup.ShowQuestion("确认减掉本商品？") == DialogResult.No)
                                {
                                    gvOrder_FocusedRowObjectChanged(null, null);
                                    return;
                                }
                            }
                        }
                    }
                    current.xallp = CalcMoneyHelper.Multiply(current.xpric, quantity);
                    current.xzhe = CalcMoneyHelper.CalcZhe(current.xpric,current.xpricold);
                    Update(current);
                }
            }
        }
        #endregion

        #region 修改表体
        private void Update(PosbbModel current)
        {
            if (posBLL.UpdatePosbb(current))
            {
                if (bdsDetail.List.Count == 1 && current.xquat == 0)
                {
                    Init();
                }
                else
                {
                    gvOrder_FocusedRowObjectChanged(null, null);
                }
            }
            else
            {
                Init();
            }
        }
        #endregion

        #region 打印
        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 作废
        private void btnInvalid_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.DelPending))
            {
                return;
            }
            if (MessagePopup.ShowQuestion("确认删除当前挂单？") == DialogResult.Yes)
            {
                PoshhModel current = bdsPos.Current as PoshhModel;
                if (current != null)
                {
                    if (posBLL.Invalid(current.ID,string.Empty))
                    {
                        Init();
                    }
                }
            }
        }
        #endregion

        #region 追加

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CurrentOperationState = OperationState.Append;

            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                CurrentPoshh = posBLL.GetPOSByID(current.ID);
                this.DialogResult = DialogResult.OK;
            }
        }
        #endregion

        #region 收款
        private void btnReceipt_Click(object sender, EventArgs e)
        {
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                FormPendingMes frm = new FormPendingMes();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    CurrentOperationState = OperationState.Receipt;
                }
                else
                {
                    CurrentOperationState = OperationState.Edit;
                }
                CurrentPoshh = posBLL.GetPOSByID(current.ID);
                this.DialogResult = DialogResult.OK;
            }
        }
        #endregion

        #region 清空明细
        private void Clear()
        {
            if (bdsPos.List.Count == 0)
            {
                lblquat.Text = string.Empty; ;
                lblUserName.Text = string.Empty;
                lblTotalMoney.Text = string.Empty;
                lblClientName.Text = string.Empty;
                lblRemark.Text = string.Empty;
                lblBalance.Text = string.Empty;
                lblxpho.Text = string.Empty;
                lblAddress.Text = string.Empty;
                bdsDetail.DataSource = null;
            }
        }
        #endregion
    }
}