using DevExpress.Skins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Alerter;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraSplashScreen;
using IWshRuntimeLibrary;
using POS.BLL;
using POS.Client;
using POS.Common;
using POS.Common.Enum;
using POS.Common.utility;
using POS.Function;
using POS.Helper;
using POS.Model;
using POS.Pending;
using POS.Setting;
using POS.Shifts;
using POS.WxPayAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace POS.Sale
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        ClientBLL clientBLL = new ClientBLL();
        GoodBLL goodBLL = new GoodBLL();
        POSBLL posBLL = new POSBLL();
        SaleBLL saleBLL = new SaleBLL();
        PossettingBLL possettingBLL = new PossettingBLL();
        PosBanBLL posBanBLL = new PosBanBLL();
        // 当前会员
        ClntModel client = null;
        //当前单据
        PoshhModel currentPoshh;
        //换货前的单据
        PoshhModel changePoshh;
        OperationState currentOperationState = OperationState.None;
        Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
        Dictionary<string, string> tyeDic = EnumHelper.GetEnumDictionary(typeof(SaleType));
        JavaScriptSerializer js = new JavaScriptSerializer();
        List<SaleModel> sales = null;
        List<Ku2Model> ku2 = null;
        //线程锁
        private static System.Threading.Mutex mutex = new System.Threading.Mutex();
        //浏览器名称
        string exeName = "CefBrowser";

        System.Timers.Timer timer_Sync = null;
        System.Timers.Timer timer_Network = null;
        //表格布局文件路径
        string filePath = Path.Combine(Application.StartupPath, "MainGridlayout.xml");
        string filePath_default = Path.Combine(Application.StartupPath, "MainGridlayout_Default.xml");
        //是否提示
        private bool isShowMsg = false;
        public FormMain()
        {
            InitializeComponent();

            Init();
            //设置复选框的皮肤
            Skin skin = EditorsSkins.GetSkin(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveLookAndFeel);
            SkinElement elem = skin[EditorsSkins.SkinCheckBox];
            Image image = elem.Image.Image;
            int imagesCount = elem.Image.ImageCount;
            elem.Image.SetImage(new Bitmap(image, new Size(22, imagesCount * 22)), Color.Transparent);

            GetSyscInfo();
            this.ActiveControl = bteProduct;
        }

        private void Init()
        {
            bdsPos.DataSource = new List<PosbbModel>();
            bstLs.Caption = RuntimeObject.CurrentUser.xlsname;
            bstBook.Caption = RuntimeObject.CurrentUser.bookID;
            bstUser.Caption = RuntimeObject.CurrentUser.username;
            bstTime.Caption = DateTime.Now.ToString("yyyy年MM月dd日");
            bstWarehouse.Caption = RuntimeObject.CurrentUser.cnkuname;

            string skinName = POS.Properties.Settings.Default.SkinName;
            if (string.IsNullOrEmpty(skinName))
            {
                defaultLookAndFeel1.LookAndFeel.SkinName = "Xmas 2008 Blue";
            }
            else
            {
                defaultLookAndFeel1.LookAndFeel.SkinName = skinName;
            }

            Dictionary<string, string> skins = new Dictionary<string, string>
            {
                { "Office 2016 Dark","简洁沉稳"},
                { "Xmas 2008 Blue","天际之蓝"},
                { "Valentine","素雅温馨"},
                { "Summer 2008","绽放盛夏"},
                { "Springtime","春天时间"},
            };
            foreach (var skin in skins)
            {
                var item = new BarButtonItem() { Caption = skin.Value, Tag = skin.Key };
                item.ItemClick += Item_ItemClick;
                barSkin.AddItem(item);
            }

            //foreach (DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins)
            //{
            //    var item = new BarButtonItem() { Caption = skin.SkinName, Tag = skin.SkinName };
            //    item.ItemClick += Item_ItemClick;
            //    barSkin.AddItem(item);
            //}


            ZBIsEnabled();

            rluexchg.DataSource = AppConst.xchgDic;

        }
        #region 设置皮肤
        private void Item_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(e.Item.Tag.ToString());
            Properties.Settings.Default.SkinName = e.Item.Tag.ToString();
            Properties.Settings.Default.Save();
        }
        #endregion

        void InitSales()
        {
            sales = saleBLL.GetAllCurrentSales();
            this.Invoke((MethodInvoker)delegate
            {
                rlueSale.DataSource = sales;
            });

        }

        #region 获取同步的消息
        void GetSyscInfo()
        {
            List<SyncInfoModel> datas = SyncHelperBLL.GetSyncInfo();
            var query = (from p in datas where p.tableName == "sale" select p).ToList();
            if (query.Count > 0)
            {
                bbtnSaleNotice.CanFlash = true;
                blBtnNotify.CanFlash = true;
            }
        }
        #endregion

        private void FormMain_Load(object sender, EventArgs e)
        {
            timer_Sync = new System.Timers.Timer();
            timer_Sync.Elapsed += Timer_Sync_Elapsed;
            InitSales();
            StartSync();

            timer_Network = new System.Timers.Timer();
            timer_Network.Elapsed += Timer_Network_Elapsed;
            timer_Network.Interval = 3000;
            timer_Network.Start();

            if (!System.IO.File.Exists(filePath_default))
            {
                gv.SaveLayoutToXml(filePath_default);
            }
            if (!System.IO.File.Exists(filePath))
            {
                gv.SaveLayoutToXml(filePath);
            }
            else
            {
                gv.RestoreLayoutFromXml(filePath);
            }

            GridLocalizer.Active = new CustomGridLocalizer();
            Localizer.Active = new CustomXtraEditorsLocalizer();
        }

        #region 检查网络连接状态
        private void Timer_Network_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckNetwor();
        }

        private void CheckNetwor()
        {
            UserModel user = RuntimeObject.CurrentUser;
            DateTime currentTime = DateTime.Now;
            string caption = "连接";
            if (SyncHelperBLL.CheckConnect(out currentTime, user.bookID, user.username, user.password))
            {
                caption = "连接";
            }
            else
            {
                caption = "断开";
            }
            CrossThread(caption);
        }
        private void CrossThread(string caption)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        SetStatus(caption);
                    });
                }
                else
                {
                    SetStatus(caption);
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetStatus(string caption)
        {
            try
            {
                lblNetwork.Caption = caption;
                //blBtnService.Visibility = BaseForm.GetPermission(Functions.InvisibleBEntrance, false) == true
                //        ? BarItemVisibility.Never : BarItemVisibility.Always;
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 是否允许总部开单
        private bool ZBIsEnabled()
        {
            bool isBill = true;
            string isZBBill = ConfigurationManager.AppSettings["isZBBill"];
            if (!string.IsNullOrEmpty(isZBBill))
            {
                if (RuntimeObject.CurrentUser.xls == "ZB")
                {
                    isBill = false;
                    if (bool.TryParse(isZBBill, out isBill))
                    {
                    }
                    btnDetail.Enabled = isBill;
                    btnDel.Enabled = isBill;
                    btnClear.Enabled = isBill;
                    btnPending.Enabled = isBill;
                    btnPos.Enabled = isBill;
                    if (currentOperationState == OperationState.FastAppend)
                    {
                        btnReceipt.Enabled = false;
                    }
                    else
                    {
                        btnReceipt.Enabled = isBill;
                    }
                    btnFastBilling.Enabled = isBill;
                }
            }
            return isBill;
        }
        #endregion

        #region 等待窗口
        public void CreateWaitDialog()
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(DevExpress.XtraWaitForm.DemoWaitForm), false, true);
                SplashScreenManager.Default.SetWaitFormCaption("请稍候...");
                SplashScreenManager.Default.SetWaitFormDescription("正在加载窗体资源...");
            }
            catch (Exception)
            {
                CloseWaitDialog();
            }
        }

        public void SetWaitDialogCaption(string description)
        {
            if (SplashScreenManager.Default != null)
                SplashScreenManager.Default.SetWaitFormDescription(description);
        }

        public void CloseWaitDialog()
        {
            if (SplashScreenManager.Default != null) SplashScreenManager.CloseForm();
        }

        #endregion

        #region 开启自动同步数据
        private void StartSync()
        {
            timer_Sync.Stop();
            List<PossettingModel> possettings = possettingBLL.GetPossetting();

            PossettingModel entity = possettings.Where(r => r.xpname == AppConst.SyncData_TimeSpan).FirstOrDefault();
            if (entity == null)
            {
                timer_Sync.Interval = 1000 * 60;
            }
            else
            {
                timer_Sync.Interval = 1000 * 60 * int.Parse(entity.xpvalue);
            }
            entity = possettings.Where(r => r.xpname == AppConst.SyncData_Type).FirstOrDefault();
            if (entity == null || int.Parse(entity.xpvalue) == 0)
            {
                timer_Sync.Start();
            }
        }
        #endregion

        #region 定时器用来同步同步数据
        private void Timer_Sync_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UserModel user = RuntimeObject.CurrentUser;
            DateTime currentTime = DateTime.Now;
            if (SyncHelperBLL.CheckConnect(out currentTime, user.bookID, user.username, user.password))
            {
                try
                {
                    mutex.WaitOne();
                    bool ck = SyncHelperBLL.Cksign(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls);
                    if (!ck)
                    {
                        new BaseForm().Alert(this, "验证失败，请检查密钥是否正确！");
                    }
                    SyncHelperBLL.SyncData(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.usercode, AppConst.AllTableNames);
                    mutex.ReleaseMutex();
                    InitSales();
                    GetSyscInfo();
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

        #region 交接班
        private void blbtnShift_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateWaitDialog();
            Shift();
            CloseWaitDialog();
        }

        private bool Shift()
        {
            gv.SaveLayoutToXml(filePath);
            if (bdsPos.List.Count > 0)
            {
                MessagePopup.ShowInformation("当前交易未结束,不能交接班！");
                return false;
            }
            FormPosBan frm = new FormPosBan();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 新增会员

        private void bbtnClientAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!BaseForm.GetPermission(Functions.AClnt))
            {
                return;
            }
            CreateWaitDialog();
            FormClientAdd frm = new FormClientAdd();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                client = frm.CurrentClient;
                SetClientInfo();
                bteClientCode.Text = client.xpho;
            }
            CloseWaitDialog();
        }
        #endregion

        #region 会员统计
        private void bbtnClientQuery_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormClientStatistics frm = new FormClientStatistics();
            frm.ShowDialog();
        }
        #endregion

        #region 销售单据
        private void blbtnSaleQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateWaitDialog();
            if (bdsPos.List.Count > 0)
            {
                MessagePopup.ShowInformation("当前交易未结束,不能查看销售单据！");
                return;
            }
            if (currentOperationState != OperationState.None)
            {
                MessagePopup.ShowInformation("当前交易未结束,不能查看销售单据！");
                return;
            }
            FormPOSQuery frm = new FormPOSQuery();
            DialogResult frmResult = frm.ShowDialog();
            if (frmResult == DialogResult.OK)
            {
                currentPoshh = frm.CurrentPoshh;
                currentOperationState = frm.CurrentOperationState;
                SetFastBillingBtnText(currentOperationState);
                if (currentOperationState == OperationState.FastAppend)
                {
                    //快速开单（追加商品)
                    btnFastBilling.Enabled = true;
                    btnReceipt.Enabled = false;
                }
                else if (currentOperationState == OperationState.Change)
                {
                    //换货
                    currentPoshh = frm.CurrentPoshh;
                    currentPoshh.Posbbs.ForEach(r => r.xchg = "换进");
                    changePoshh = new CloneEntityHelper().Clone<PoshhModel>(frm.CurrentPoshh);
                    SetGoodSpecification();
                    bdsPos.DataSource = currentPoshh.Posbbs;
                    CalcSummary();
                    SetChangeGoodsInfo(true);
                    CalcGoodsDifference();
                    if (currentPoshh != null && !string.IsNullOrEmpty(currentPoshh.clntcode))
                    {
                        client = clientBLL.GetClientByCode(currentPoshh.clntcode);
                        SetClientInfo();
                    }
                }
                else
                {
                    currentPoshh.xstate = stateDic[Enum.GetName(typeof(PosState), PosState.Deal)];
                    SetGoodSpecification();
                    foreach (var item in currentPoshh.Posbbs)
                    {
                        Guid id = Guid.NewGuid();
                        var query = (from p in currentPoshh.Posbbs where p.PID == item.ID select p).ToList();
                        query.ForEach(r => r.PID = id);
                        item.ID = id;
                    }
                    bdsPos.DataSource = currentPoshh.Posbbs;
                    CalcSummary();
                }
                if (currentPoshh != null && !string.IsNullOrEmpty(currentPoshh.clntcode))
                {
                    client = clientBLL.GetClientByCode(currentPoshh.clntcode);
                    SetClientInfo();
                }
            }
            CloseWaitDialog();
        }
        #endregion

        #region 系统设置
        private void blBtnSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateWaitDialog();
            FormSetting frm = new FormSetting();
            if (frm.ShowDialog() == DialogResult.OK)
            {

            }
            CloseWaitDialog();
            InitSales();
            StartSync();
            SetClientInfo();
            SetCustomerDisplay(colxallp.SummaryItem.SummaryValue.ToString());
        }
        #endregion

        #region 快捷键

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ZBIsEnabled())
            {
                return;
            }

            if (e.KeyCode == Keys.F9)
            {
                //搜索会员
                SearchClient();
            }
            if (e.KeyCode == Keys.F4)
            {
                //详细
                ShowDetail();
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
                //删除
                btnDel_Click(null, null);
            }
            else if (e.Control && e.KeyCode == Keys.Z)
            {
                //清空
                Clear();

            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                //挂单
                Pending();
            }
            else if (e.Control && e.KeyCode == Keys.Q)
            {
                //取单
                GetPendingOrder(null);
            }
            else if (e.KeyCode == Keys.Space)
            {
                bteProduct.Focus();
                if (currentOperationState == OperationState.FastAppend)
                {
                    btnFastBilling_Click(null, null);
                }
                else
                {
                    //收银
                    btnReceipt_Click(null, null);
                }
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                //弹钱箱
                btnOpenCashbox_Click(null, null);
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                //列设置
                tsmColumnsSetting_Click(null, null);
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                //还原默认设置
                tsmRestore_Click(null, null);
            }
        }
        #endregion

        #region 锁屏
        private void blBtnLock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormLock frm = new FormLock();
            frm.ShowDialog();
        }
        #endregion

        #region 云后台
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        private void blBtnService_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormWxPay frm = new FormWxPay();
            frm.ShowDialog();
            return;
            try
            {
                UserModel user = RuntimeObject.CurrentUser;
                Uri url = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
                url = new Uri(url, string.Format("xc.jsp?id={0}", user.bookID));
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string path = Path.Combine(Path.Combine(basePath, exeName), exeName + ".exe");
                if (!System.IO.File.Exists(path))
                {
                    //调用系统默认的浏览器   
                    System.Diagnostics.Process.Start(url.ToString());
                }
                else
                {
                    Process[] temp = Process.GetProcessesByName(exeName);
                    if (temp.Length > 0)
                    {
                        IntPtr handle = temp[0].MainWindowHandle;
                        // 激活，显示在最前
                        SwitchToThisWindow(handle, true);
                    }
                    else
                    {
                        string argument = string.Format("{0} {1} {2}", url, user.username, user.passwordExpress);
                        Process.Start(path, argument);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 所有功能

        private void blBtnAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateWaitDialog();
            FormFunction frm = new FormFunction();
            frm.ShowDialog();
            CloseWaitDialog();
        }
        #endregion

        #region 搜索货品
        private void bteProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                bteProduct.EditValue = bteProduct.Text.Trim();
            }
            if (e.KeyCode == Keys.Enter)
            {
                SearchProduct();
            }
        }

        private void SearchProduct()
        {
            if (!string.IsNullOrEmpty(bteProduct.Text.Trim()))
            {
                DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在查询货品信息，请稍后……", new Size(250, 100));
                dlg.Show();
                List<GoodModel> goods = goodBLL.GetGoodByKey(bteProduct.Text.Trim(), RuntimeObject.CurrentUser.xls);
                dlg.Close();

                if (goods.Count == 0)
                {
                    MessagePopup.ShowInformation("没有找到匹配的商品！");
                }
                else if (goods.Count == 1)
                {
                    //加入到Pos表体
                    AddPos(goods.First());
                }
                else if (goods.Count > 1)
                {
                    FormProductQuery frm = new FormProductQuery(bteProduct.Text.Trim());
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        GoodModel good = frm.CurrentGood;
                        //加入到Pos表体
                        AddPos(good);
                    }
                }
                bteProduct.EditValue = null;
            }
        }

        #endregion

        #region 显示右键
        private void gv_MouseUp(object sender, MouseEventArgs e)
        {
            GridHitInfo gridInfo = gv.CalcHitInfo(e.Location);

            if (gridInfo.InRow && e.Button == MouseButtons.Right)
            {
                bool visible = bdsPos.List.Count > 0;
                tsmDetail.Visible = visible;
                tsmDel.Visible = visible;
                tsmClear.Visible = visible;
                tsmPending.Visible = visible;
                tsmPos.Visible = !visible;

                contextMenuStrip1.Show(e.X, e.Y + panelTop.Height);
            }
            else if (gridInfo.HitTest == GridHitTest.EmptyRow && e.Button == MouseButtons.Right)
            {
                if (bdsPos.List.Count == 0)
                {
                    tsmDetail.Visible = false;
                    tsmDel.Visible = false;
                    tsmClear.Visible = false;
                    tsmPending.Visible = false;
                    tsmPos.Visible = true;
                    contextMenuStrip1.Show(e.X, e.Y + 80 + panelTop.Height);
                }
                else
                {
                    tsmDetail.Visible = false;
                    tsmDel.Visible = false;
                    tsmClear.Visible = false;
                    tsmPending.Visible = false;
                    tsmPos.Visible = false;
                    contextMenuStrip1.Show(e.X, e.Y + 80 + panelTop.Height);
                }
            }
        }
        #endregion

        #region 双击单元格查看明细
        private void gv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                ShowDetail();
            }
        }
        #endregion

        #region 查询会员
        private void bteClientCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (isShowMsg)
                {
                    isShowMsg = false;
                }
                else
                {
                    QueryClnt();
                }
            }
        }

        private void QueryClnt()
        {
            if (string.IsNullOrEmpty(bteClientCode.Text.Trim()))
            {
                // bteProduct.Focus();
                isShowMsg = true;
                MessagePopup.ShowInformation("请输入会员号！");
                isShowMsg = false;
                return;
            }
            List<ClntModel> clinets = null;
            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在查询会员信息，请稍后……", new Size(250, 100));
            dlg.Show();
            try
            {
                clinets = clientBLL.GetClientByKey(bteClientCode.Text.Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dlg.Close();
            }

            if (clinets.Count == 0)
            {
                isShowMsg = true;
                MessagePopup.ShowInformation("查询不到相关会员！");
                isShowMsg = false;
            }
            else if (clinets.Count == 1)
            {
                client = clinets.FirstOrDefault();
                SetClientInfo();
            }
            else if (clinets.Count > 1)
            {
                FormClientQuery frm = new FormClientQuery(clinets, bteClientCode.Text.Trim());
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    client = frm.currentClient;
                    SetClientInfo();
                }
                else
                {
                    bteClientCode.Text = client == null ? string.Empty : client.clntcode;
                }
            }
        }

        private void bteClientName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (client == null)
            {
                //搜索会员
                SearchClient();
            }
            else
            {
                //查看会员明细
                FormClientDetail frm = new FormClientDetail(client.ID, true, ZBIsEnabled());
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //取消会员
                    if (frm.currentClient == null)
                    {
                        ClearClient();
                    }
                    else
                    {
                        client = frm.currentClient;
                        SetClientInfo();
                    }
                }
                else
                {
                    decimal ojie2 = clientBLL.GetOjie2(client.clntcode);
                    bteBalance.EditValue = ojie2;
                    client = frm.currentClient;
                    SetClientInfo();
                }
            }
        }
        #endregion

        #region 赋值会员信息
        private void SetClientInfo()
        {
            if (client != null)
            {
                bteClientCode.Text = client.clntcode;
                bteClientName.Text = client.clntname;
                decimal ojie2 = clientBLL.GetOjie2(client.clntcode);
                decimal jjie2 = clientBLL.GetJjie2(client.clntcode);
                bteBalance.EditValue = ojie2;
                bteIntegral.EditValue = jjie2;
            }
        }

        #endregion

        #region 清除会员信息
        private void bteClientCode_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bteClientCode.Text.Trim()))
            {
                ClearClient();
            }
        }
        private void ClearClient()
        {
            client = default(ClntModel);
            bteClientCode.EditValue = null;
            bteClientName.EditValue = null;
            bteBalance.EditValue = 0;
            bteIntegral.EditValue = 0;
        }
        #endregion

        #region 搜索会员
        /// <summary>
        /// 搜索会员
        /// </summary>
        private void SearchClient()
        {
            FormClientSearch frm = new FormClientSearch(false);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                client = frm.currentClient;
                SetClientInfo();
            }
        }
        #endregion

        #region 删除
        private void btnDel_Click(object sender, EventArgs e)
        {
            PosbbModel current = bdsPos.Current as PosbbModel;
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            if (current != null)
            {
                if (currentOperationState != OperationState.None)
                {
                    var query = from p in data where p.isGift == false select p;
                    if (query.Count() == 1)
                    {
                        if (currentOperationState == OperationState.Append)
                        {
                            if (MessagePopup.ShowQuestion("你清空了购物车,是否结束取单收银？") == DialogResult.Yes)
                            {
                                //currentOperationState = OperationState.None;
                                //SetBtnText(currentOperationState);
                                //btnFastBilling.Enabled = true;
                                Clear();
                                return;
                            }
                        }
                        else if (currentOperationState == OperationState.Change)
                        {
                            if (MessagePopup.ShowQuestion("你清空了购物车,是否结束换货开单？") == DialogResult.Yes)
                            {
                                //currentOperationState = OperationState.None;
                                //SetChangeGoodsInfo(false);
                                //ClearClient();
                                //SetBtnText(currentOperationState);
                                Clear();
                                return;
                            }
                        }
                        else if (currentOperationState == OperationState.FastAppend)
                        {
                            if (MessagePopup.ShowQuestion("你清空了购物车,是否结束追加开单？") == DialogResult.Yes)
                            {
                                //currentOperationState = OperationState.None;
                                //SetFastBillingBtnText(currentOperationState);
                                //btnReceipt.Enabled = true;
                                Clear();
                                return;
                            }
                        }
                    }
                }
                //if (bdsPos.List.Count == 1)
                //{
                //    if (MessagePopup.ShowQuestion("你清空了购物车,是否退出会员？") == DialogResult.Yes)
                //    {
                //        ClearClient();
                //    }
                //}
                if (current.isGift)
                {
                    bdsPos.RemoveCurrent();
                }
                else
                {
                    var query = (from p in data where p.xsalesid.HasValue && p.xsalesid == current.xsalesid && p.PID == current.ID select p).ToList();
                    if (query.Count > 0)
                    {
                        bdsPos.RemoveCurrent();
                        for (int i = query.Count - 1; i >= 0; i--)
                        {
                            bdsPos.Remove(query[i]);
                        }
                    }
                    else
                    {
                        bdsPos.RemoveCurrent();
                    }
                }
                CalcSummary();
            }
        }
        #endregion

        #region 详细
        private void btnDetail_Click(object sender, EventArgs e)
        {
            ShowDetail();
        }
        #endregion

        #region 清空
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        #endregion

        #region 挂单
        private void btnPending_Click(object sender, EventArgs e)
        {
            Pending();
        }
        #endregion

        #region 取单
        private void btnPos_Click(object sender, EventArgs e)
        {
            GetPendingOrder(null);
        }
        #endregion

        #region  Pos单添加货品
        /// <summary>
        /// Pos单添加货品
        /// </summary>
        /// <param name="good"></param>
        private void AddPos(GoodModel good)
        {
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
            int? saleID = null;
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            var query = data.Where(r => r.goodcode == good.goodcode && r.goodtm == good.goodkind && r.isGift == false && r.unitname == good.unitname).FirstOrDefault();
            if (query != null)
            {
                if (good.unitrate > 0)
                {
                    query.xquat += good.unitrate;
                    query.unitquat += 1;
                    query.xallp = query.xpric * query.unitquat;
                }
                else
                {
                    query.xquat += 1;
                    query.unitquat += 1;
                    query.xallp = query.xpric * query.unitquat;
                }
                query.xtax = CalcMoneyHelper.Multiply(query.xallp, query.xtaxr);
                query.xallpt = CalcMoneyHelper.Add(query.xallp, query.xtax);
                query.xprict = CalcMoneyHelper.Divide(query.xallpt, query.unitquat);

                saleID = query.xsalesid;
                bdsPos.ResetCurrentItem();
                gv.FocusedRowHandle = bdsPos.List.IndexOf(query);
            }
            else
            {
                PosbbModel entity = new PosbbModel();
                entity.ID = Guid.NewGuid();
                entity.goodcode = good.goodcode;
                entity.xbarcode = good.xbarcode;
                entity.goodname = good.goodname.Trim();
                entity.goodunit = good.goodunit.Trim();
                entity.goodtm = good.goodkind;
                entity.goodkind1 = good.goodkind1;
                entity.goodkind2 = good.goodkind2;
                entity.goodkind3 = good.goodkind3;
                entity.goodkind4 = good.goodkind4;
                entity.goodkind5 = good.goodkind5;
                entity.goodkind6 = good.goodkind6;
                entity.goodkind7 = good.goodkind7;
                entity.goodkind8 = good.goodkind8;
                entity.goodkind9 = good.goodkind9;
                entity.goodkind10 = good.goodkind10;
                if (good.unitrate > 0)
                {
                    entity.xquat = good.unitrate;
                    entity.unitquat = 1;
                    entity.unitrate = good.unitrate;
                    //entity.xpricold = CalcMoneyHelper.Divide(good.xprico * good.quantity, entity.xquat);
                }
                else
                {
                    entity.xquat = 1;
                    entity.unitquat = 1;
                    entity.unitrate = 1;
                }
                entity.xpricold = good.xprico;

                entity.unitname = good.unitname;
                entity.xzhe = 100;
                //根据计算规则计算折扣价格
                //entity.xpric = entity.xpricold * (entity.xzhe / 100);
                entity.xpric = entity.xpricold;
                entity.xallp = CalcMoneyHelper.Multiply(entity.xpric, entity.unitquat);
                entity.xtaxr = 0;
                entity.xtax = CalcMoneyHelper.Multiply(entity.xallp, entity.xtaxr);
                entity.xallpt = CalcMoneyHelper.Add(entity.xallp, entity.xtax);
                entity.xprict = CalcMoneyHelper.Divide(entity.xallpt, entity.unitquat);
                entity.goodXtableID = good.xtableid.Value;
                entity.xsendjf = good.xsendjf;
                if (currentOperationState == OperationState.Change)
                {
                    entity.xchg = "换出";
                }
                bdsPos.Add(entity);
                gv.FocusedRowHandle = bdsPos.List.Count - 1;
            }


            PosbbModel current = bdsPos.Current as PosbbModel;
            SetSale(current, good);

            Sale(good, current.xsalesid, current.ID);
            CalcSummary();
            if (chkEnter.Checked)
            {
                gridControl1.Focus();
                gv.FocusedColumn = colunitquat;
                gv.ShowEditor();
            }
        }
        #endregion

        #region 计算总数
        private void CalcSummary()
        {
            if (colunitquat.Summary.Count == 0)
            {
                this.colunitquat.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "unitquat", "{0:0.##}")});
            }
            gv.PostEditor();
            gv.UpdateSummary();
            decimal totalXallp = CalcTotalMoney();
            lblTotalQuantity.Text = string.Format("共计 {0} 件商品", colunitquat.SummaryItem.SummaryValue);
            lblTotalMoney.Text = string.Format("￥{0}", totalXallp);
            SetCustomerDisplay(totalXallp.ToString());
            CalcGoodsDifference();
        }
        /// <summary>
        /// 计算总金额
        /// </summary>
        /// <returns></returns>
        private decimal CalcTotalMoney()
        {
            List<PosbbModel> posbbs = bdsPos.DataSource as List<PosbbModel>;
            decimal totalXallp = 0;
            if (posbbs != null)
            {
                totalXallp = posbbs.Where(r => r.xchg != "换进").Sum(r => r.xallp) - posbbs.Where(r => r.xchg == "换进").Sum(r => r.xallp);
            }
            return totalXallp;
        }
        #endregion

        #region 客显
        private void SetCustomerDisplay(string dispiay)
        {
            CustomerDisplayHelper disliay;
            try
            {
                disliay = new CustomerDisplayHelper();
                disliay.DispiayType = CustomerDispiayType.Total;
                disliay.DisplayData(dispiay);

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 无码收银
        private void bteNoBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                NoBarcode();
            }
        }

        private void NoBarcode()
        {
            decimal price;
            if (decimal.TryParse(bteNoBarcode.Text.Trim(), out price))
            {
                PosbbModel entity = new PosbbModel();
                entity.goodname = "无码商品";
                entity.xpricold = price;
                entity.xzhe = 100;
                entity.unitquat = 1;
                entity.unitrate = 1;
                entity.xquat = 1;
                entity.xpric = entity.xpricold * entity.xquat;
                entity.xallp = CalcMoneyHelper.Multiply(entity.xpric, entity.xquat);
                bdsPos.Add(entity);
                gv.FocusedRowHandle = bdsPos.List.Count - 1;
                CalcSummary();
                bteNoBarcode.EditValue = null;
            }
            else
            {
                MessagePopup.ShowInformation("请输入正确的价格！");
            }
        }


        #endregion

        #region 表格值发生改变
        private void rtxtunitquat_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (e.NewValue != null && !string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                PosbbModel current = gv.GetFocusedRow() as PosbbModel;
                if (current != null)
                {
                    if (e.NewValue.ToString().Contains("-"))
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
        private void gv_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            PosbbModel current = bdsPos.Current as PosbbModel;
            if (current != null)
            {
                if (e.Column == colunitquat)
                {
                    //数量
                    current.xallp = CalcMoneyHelper.Multiply(e.Value, current.xpric);
                    current.xtax = CalcMoneyHelper.Multiply(current.xallp, current.xtaxr);
                    current.xallpt = CalcMoneyHelper.Add(current.xallp, current.xtax);
                    current.xprict = CalcMoneyHelper.Divide(current.xallpt, current.unitquat);
                    current.xquat = CalcMoneyHelper.Multiply(e.Value, current.unitrate);
                    if (e.Value != null && e.Value.ToString() != string.Empty)
                    {
                        if (current.xsalesid.HasValue)
                        {
                            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
                            var query = (from p in data where p.xsalesid.HasValue && p.xsalesid == current.xsalesid && p.isGift && p.PID == current.ID select p).ToList();
                            if (query.Count > 0)
                            {
                                for (int i = query.Count - 1; i >= 0; i--)
                                {
                                    bdsPos.Remove(query[i]);
                                }
                            }
                        }

                        GoodModel good = SetGood(current);
                        if (good != null)
                        {
                            SetSale(current, good);
                            int? saleID = current.xsalesid;
                            Sale(good, saleID, current.ID);
                        }
                    }
                }
                else if (e.Column == colxpric)
                {
                    //现价
                    if (e.Value != null && e.Value.ToString() != string.Empty)
                    {
                        current.xallp = CalcMoneyHelper.Multiply(current.unitquat, e.Value);
                        current.xzhe = CalcMoneyHelper.CalcZhe(e.Value, current.xpricold);
                        current.xtax = CalcMoneyHelper.Multiply(current.xallp, current.xtaxr);
                        current.xallpt = CalcMoneyHelper.Add(current.xallp, current.xtax);
                        current.xprict = CalcMoneyHelper.Divide(current.xallpt, current.unitquat);
                    }
                }
                CalcSummary();
            }
        }

        private GoodModel SetGood(PosbbModel current)
        {
            GoodModel good = new GoodModel();
            if (!string.IsNullOrEmpty(current.goodcode))
            {
                GoodModel currentgood = goodBLL.GetGoodByCode(current.goodcode);
                good.goodcode = current.goodcode;
                good.goodtype1 = currentgood.goodtype1;
                good.goodtype2 = currentgood.goodtype2;
                good.goodtype3 = currentgood.goodtype3;
                good.xtableid = current.goodXtableID;
                good.goodunit = current.goodunit;
                good.unitname = current.unitname;
                good.unitrate = current.unitrate.HasValue ? current.unitrate.Value : 1;
                return good;
            }
            else
            {
                return null;
            }
        }

        private void SetSale(PosbbModel current, GoodModel good)
        {
            if (IsClntDay(good))
            {
                return;
            }
            if (current.isGift == false)
            {
                string clntcode = string.Empty;
                if (client != null)
                {
                    clntcode = client.clntcode;
                }
                List<int> saleIDs = saleBLL.GetSaleIDs(clntcode, current.goodcode, current.goodXtableID);
                var data = sales.Where(r => r.xtableid.HasValue && saleIDs.Contains(r.xtableid.Value));

                List<SaleModel> sales_satisfy = new List<SaleModel>();
                foreach (SaleModel sale in data)
                {
                    SaleModel currentSale = saleBLL.GetSaleByID(sale.xtableid.Value);
                    List<PosbbModel> posbbs = null;
                    var rule = GetSalerules(currentSale, good, out posbbs);
                    if (rule != null)
                    {
                        sales_satisfy.Add(sale);
                    }
                }

                SaleModel query = sales_satisfy.OrderByDescending(r => r.xintime).FirstOrDefault();
                if (query != null)
                {
                    if (query.xtype != "t" || good.goodunit == good.unitname)
                    {
                        current.xsalesid = query.xtableid;
                    }
                }
            }
        }
        #endregion

        #region 右键详细
        private void tsmDetail_Click(object sender, EventArgs e)
        {
            ShowDetail();
        }
        #endregion

        #region 右键删除
        private void tsmDel_Click(object sender, EventArgs e)
        {
            btnDel_Click(null, null);
        }
        #endregion

        #region 右键清空
        private void tsmClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        #endregion

        #region 右键挂单
        private void tsmPending_Click(object sender, EventArgs e)
        {
            Pending();
        }
        #endregion

        #region 右键取单
        private void tsmPos_Click(object sender, EventArgs e)
        {
            GetPendingOrder(null);
        }
        #endregion

        #region 显示明细
        /// <summary>
        /// 显示明细
        /// </summary>
        private void ShowDetail()
        {
            PosbbModel current = bdsPos.Current as PosbbModel;
            if (current != null && !current.isGift)
            {
                FormPOSDetail frm = new FormPOSDetail(current);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    current.xtax = CalcMoneyHelper.Multiply(current.xallp, current.xtaxr);
                    current.xallpt = CalcMoneyHelper.Add(current.xallp, current.xtax);
                    current.xprict = CalcMoneyHelper.Divide(current.xallpt, current.unitquat);
                    CalcSummary();
                }
            }
        }
        #endregion

        #region 挂单
        private void Pending()
        {
            if (bdsPos.List.Count == 0)
            {
                MessagePopup.ShowInformation("当前单据没有商品，请先选择商品！");
            }
            else if (currentOperationState != OperationState.None)
            {
                MessagePopup.ShowInformation("无法重复挂单！");
            }
            else
            {
                //挂单
                if (MessagePopup.ShowQuestion("确定要挂单？") == DialogResult.Yes)
                {
                    try
                    {
                        PoshhModel poshh = SetPoshh(PosState.Pending);
                        string billno = string.Empty;
                        if (posBLL.AddPOS(poshh, out billno))
                        {
                            bdsPos.Clear();
                            ClearClient();
                        }
                        else
                        {
                            MessagePopup.ShowError("添加失败！");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessagePopup.ShowError(string.Format("未知错误！{0}", ex.Message));
                    }
                }
            }
        }
        #endregion

        #region 取单
        private void GetPendingOrder(Guid? id)
        {
            if (currentOperationState != OperationState.None)
            {
                MessagePopup.ShowInformation("当前单据尚未收银，请勿取单！");
                return;
            }
            //取单
            FormPendingOrder frm = new FormPendingOrder(id);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                currentOperationState = frm.CurrentOperationState;
                SetBtnText(currentOperationState);
                btnReceipt.Enabled = true;
                btnFastBilling.Enabled = false;
                currentPoshh = frm.CurrentPoshh;
                SetGoodSpecification();

                if (currentPoshh != null && !string.IsNullOrEmpty(currentPoshh.clntcode))
                {
                    client = clientBLL.GetClientByCode(currentPoshh.clntcode);
                    SetClientInfo();
                }

                if (currentOperationState == OperationState.Edit)
                {
                    bdsPos.DataSource = currentPoshh.Posbbs;
                    CalcSummary();

                }
                else if (currentOperationState == OperationState.Receipt)
                {
                    bdsPos.DataSource = currentPoshh.Posbbs;
                    CalcSummary();
                    btnReceipt_Click(null, null);
                }
                else if (currentOperationState == OperationState.Append)
                {

                }
            }
        }
        #endregion

        #region 赋值POS单
        /// <summary>
        /// 赋值POS单
        /// </summary>
        /// <param name="posState"></param>
        /// <returns></returns>
        private PoshhModel SetPoshh(PosState posState)
        {
            PoshhModel entity = new PoshhModel();
            entity.posnono = RuntimeObject.CurrentUser.posnono;
            entity.xstate = stateDic[Enum.GetName(typeof(PosState), posState)];
            if (client != null)
            {
                entity.clntcode = client.clntcode;
                entity.clntname = client.clntname;
            }
            entity.xnote = string.Empty;
            entity.xls = RuntimeObject.CurrentUser.xls;
            entity.xlsname = RuntimeObject.CurrentUser.xlsname;
            entity.xinname = RuntimeObject.CurrentUser.username;
            entity.xheallp = Math.Abs(CalcGoodMoney());
            entity.xpay = Math.Abs(CalcTotalMoney());
            entity.xhezhe = CalcMoneyHelper.Subtract(entity.xheallp, entity.xpay);
            entity.xhenojie = 0;
            List<PosbbModel> datas = bdsPos.DataSource as List<PosbbModel>;
            entity.Posbbs = datas;
            return entity;
        }
        #endregion

        #region 计算货款 
        /// <summary>
        /// 计算货款
        /// </summary>
        /// <returns></returns>
        private decimal CalcGoodMoney()
        {
            List<PosbbModel> datas = bdsPos.DataSource as List<PosbbModel>;

            decimal query = datas.Where(r => r.xchg != "换进").Sum(r => r.xallp) - datas.Where(r => r.xchg == "换进").Sum(r => r.xallp);
            return query;
        }

        private decimal CalcGoodMoney(List<PosbbModel> datas)
        {
            decimal query = datas.Where(r => r.xchg != "换进").Sum(r => r.xallp) - datas.Where(r => r.xchg == "换进").Sum(r => r.xallp);
            return query;
        }
        #endregion

        #region 赋值收银按钮
        private void SetBtnText(OperationState state)
        {
            if (state == OperationState.Append)
            {
                btnReceipt.Text = "追加【空格键】";
            }
            else
            {
                btnReceipt.Text = "收款【空格键】";
            }
        }
        #endregion

        #region 赋值快速开单按钮
        private void SetFastBillingBtnText(OperationState state)
        {
            if (state == OperationState.FastAppend)
            {
                btnFastBilling.Text = "追加";
            }
            else
            {
                btnFastBilling.Text = "快速开单";
            }
        }
        #endregion

        #region 设置换货的信息
        /// <summary>
        /// 设置换货的信息
        /// </summary>
        /// <param name="flag"></param>
        private void SetChangeGoodsInfo(bool flag)
        {
            lblDifference.Visible = flag;
            if (flag)
            {
                lblDifference.Tag = currentPoshh;
            }
            else
            {
                lblDifference.Tag = string.Empty;
            }
            btnFastBilling.Enabled = !flag;
            bteClientCode.Enabled = !flag;
            bteClientName.Enabled = !flag;

        }
        #endregion

        #region 计算换货的差额
        /// <summary>
        /// 计算换货的差额
        /// </summary>
        private decimal CalcGoodsDifference()
        {
            if (lblDifference.Tag != null && !string.IsNullOrEmpty(lblDifference.Tag.ToString()))
            {
                PoshhModel poshh = lblDifference.Tag as PoshhModel;
                // decimal diff = CalcMoneyHelper.Subtract((0-poshh.xpay), (0-CalcTotalMoney()));
                decimal diff = CalcTotalMoney();
                if (diff < 0)
                {
                    lblDifference.Text = string.Format("退差额￥{0}", Math.Abs(diff));
                }
                else
                {
                    lblDifference.Text = string.Format("补差额￥{0}", Math.Abs(diff));
                }
                return diff;
            }
            else
            {
                lblDifference.Text = string.Format("补差额￥{0}", 0);
            }
            return 0;
        }
        #endregion

        #region 清空
        private void Clear()
        {
            ClearClient();
            bdsPos.Clear();
            currentOperationState = OperationState.None;
            SetBtnText(currentOperationState);
            SetFastBillingBtnText(currentOperationState);
            currentPoshh = null;
            changePoshh = null;
            CalcSummary();
            btnReceipt.Enabled = true;
            btnFastBilling.Enabled = true;
            btnClear.Enabled = false;
            SetChangeGoodsInfo(false);
            this.ActiveControl = bteProduct;
        }
        #endregion

        #region 收款
        private void btnReceipt_Click(object sender, EventArgs e)
        {
            if (!BaseForm.GetPermission(Functions.Cashier))
            {
                return;
            }
            if (btnReceipt.Enabled)
            {
                btnReceipt.Enabled = false;
                gv.PostEditor();
                gv.CloseEditor();
                bdsPos.EndEdit();
                #region 追加
                if (currentOperationState == OperationState.Append)
                {
                    if (bdsPos.List.Count == 0)
                    {
                        if (MessagePopup.ShowQuestion("当前没有商品,是否取消追加？") == DialogResult.Yes)
                        {
                            currentOperationState = OperationState.None;
                            SetBtnText(currentOperationState);
                            btnFastBilling.Enabled = true;
                            btnReceipt.Enabled = true;
                            return;
                        }
                        else
                        {
                            btnReceipt.Enabled = true;
                        }
                    }
                    else if (MessagePopup.ShowQuestion("确认继续追加当前挂单？") == DialogResult.Yes)
                    {
                        List<PosbbModel> datas = new List<PosbbModel>();
                        List<PosbbModel> currentDetail = bdsPos.DataSource as List<PosbbModel>;
                        datas.AddRange(currentPoshh.Posbbs);
                        datas.AddRange(currentDetail);
                        currentPoshh.xpay = datas.Select(r => r.xallp).Sum();
                        currentPoshh.xheallp = CalcGoodMoney(datas);
                        currentPoshh.xhezhe = CalcMoneyHelper.Subtract(currentPoshh.xheallp, currentPoshh.xpay);
                        currentPoshh.xhenojie = 0;
                        currentPoshh.Posbbs = currentDetail;
                        Guid id = currentPoshh.ID;
                        if (posBLL.Append(currentPoshh))
                        {
                            Clear();
                            GetPendingOrder(id);
                        }
                    }
                    else
                    {
                        btnReceipt.Enabled = true;
                    }

                }
                #endregion
                else
                {
                    //收款
                    if (bdsPos.List.Count == 0)
                    {
                        MessagePopup.ShowInformation("当前没有订单，请先添加商品！");
                        btnReceipt.Enabled = true;
                        return;
                    }

                    PoshhModel poshh = SetPoshh(PosState.Deal);
                    if (currentPoshh != null)
                    {
                        poshh.ID = currentPoshh.ID;
                    }
                    if (poshh.Posbbs.Count == 0)
                    {
                        MessagePopup.ShowError("请先添加商品！");
                        btnReceipt.Enabled = true;
                        return;
                    }
                    if (!CheckInventory(poshh.Posbbs))
                    {
                        btnReceipt.Enabled = true;
                        return;
                    }
                    foreach (var item in poshh.Posbbs)
                    {
                        if (string.IsNullOrEmpty(item.cnkucode))
                        {
                            item.cnkucode = RuntimeObject.CurrentUser.cnkucode;
                            item.cnkuname = RuntimeObject.CurrentUser.cnkuname;
                        }
                    }
                    if (client != null)
                    {
                        client.balance = decimal.Parse(bteBalance.Text.Trim());
                    }
                    //if (currentOperationState == OperationState.Change)
                    //{
                    //    FormChangeConfirm frm_Change = new FormChangeConfirm(changePoshh, currentPoshh);
                    //    if (frm_Change.ShowDialog() != DialogResult.OK)
                    //    {
                    //        btnReceipt.Enabled = true;
                    //        return;
                    //    }
                    //}
                    CreateWaitDialog();
                    PoshhModel poshh_Pre = null;
                    if (lblDifference.Tag != null && !string.IsNullOrEmpty(lblDifference.Tag.ToString()))
                    {
                        poshh_Pre = lblDifference.Tag as PoshhModel;
                        if (poshh_Pre != null)
                        {
                            poshh.pbillno = poshh_Pre.billno;
                        }
                        decimal diff = CalcGoodsDifference();
                        if (diff < 0)
                        {
                            //退差额
                            FormRefund frmR = new FormRefund(poshh, poshh_Pre, diff, this);
                            if (frmR.ShowDialog() == DialogResult.OK)
                            {
                                //if (poshh_Pre.posnono != RuntimeObject.CurrentUser.posnono)
                                //{
                                //    posBanBLL.UpdatePosban(poshh.xpay, currentPoshh.posnono);
                                //}
                                Clear();
                            }
                            btnReceipt.Enabled = true;
                            return;
                        }
                    }
                    FormReceipt frm = new FormReceipt(poshh, client, false, poshh_Pre, this);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        //if (poshh_Pre != null)
                        //{
                        //    if (poshh_Pre.posnono != RuntimeObject.CurrentUser.posnono)
                        //    {
                        //        posBanBLL.UpdatePosban(poshh.xpay, currentPoshh.posnono);
                        //    }
                        //}
                        Clear();
                        frm.Close();
                        frm.Dispose();
                    }
                    btnReceipt.Enabled = true;
                    CloseWaitDialog();
                }
            }
        }
        #endregion

        #region 设置货品规格
        private void SetGoodSpecification()
        {
            foreach (var item in currentPoshh.Posbbs)
            {
                List<string> goodkinds = new List<string>();
                if (!string.IsNullOrEmpty(item.goodkind1))
                {
                    goodkinds.Add(item.goodkind1);
                }
                if (!string.IsNullOrEmpty(item.goodkind2))
                {
                    goodkinds.Add(item.goodkind2);
                }
                if (!string.IsNullOrEmpty(item.goodkind3))
                {
                    goodkinds.Add(item.goodkind3);
                }
                if (!string.IsNullOrEmpty(item.goodkind4))
                {
                    goodkinds.Add(item.goodkind4);
                }
                if (!string.IsNullOrEmpty(item.goodkind5))
                {
                    goodkinds.Add(item.goodkind5);
                }
                if (!string.IsNullOrEmpty(item.goodkind6))
                {
                    goodkinds.Add(item.goodkind6);
                }
                if (!string.IsNullOrEmpty(item.goodkind7))
                {
                    goodkinds.Add(item.goodkind7);
                }
                if (!string.IsNullOrEmpty(item.goodkind8))
                {
                    goodkinds.Add(item.goodkind8);
                }
                if (!string.IsNullOrEmpty(item.goodkind9))
                {
                    goodkinds.Add(item.goodkind9);
                }
                if (!string.IsNullOrEmpty(item.goodkind10))
                {
                    goodkinds.Add(item.goodkind10);
                }
                if (goodkinds.Count > 0)
                {
                    item.goodtm = string.Join(",", goodkinds);
                }
            }
        }
        #endregion

        #region 关闭窗体
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bdsPos.List.Count > 0)
            {
                MessagePopup.ShowInformation("当前交易未结束,不能最小化！");
                e.Cancel = true;
            }
            else
            {
                if (btnExit.Tag == null)
                {
                    if (!Shift())
                        e.Cancel = true;
                }
            }
        }
        #endregion

        #region 促销活动
        private void Sale(GoodModel good, int? saleID, Guid id)
        {
            string clntcode = client == null ? string.Empty : client.clntcode;

            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            PosbbModel current = data.Where(r => r.goodXtableID == good.xtableid).FirstOrDefault();

            if (current != null)
            {
                current.xtimes = null;
                current.xsalestype = string.Empty;
                if (!string.IsNullOrEmpty(clntcode))
                {
                    DateTime date = DateTime.Now;
                    ClntDayModel clntday = saleBLL.GetClntDay(date.Day, date.Month);
                    if (clntday != null)
                    {

                        ClntclssModel clntclss = clientBLL.GetClntclssByClass(client.clntclss);
                        List<ClntdRuleModel> clntprices = clntday.clntprices;
                        var query = clntprices.Where(r => (r.classtype == "小类" && r.goodtype == good.goodtype3)
                        || (r.classtype == "中类" && r.goodtype == good.goodtype2)
                        || (r.classtype == "大类" && r.goodtype == good.goodtype1)
                        ).FirstOrDefault();
                        if (query != null)
                        {
                            if (SetGoodtypePrice(current, clntclss, query.uclssprics) || query.xtimes > 0)
                            {
                                current.xsalesid = null;
                                current.xtimes = query.xtimes > 0 ? query.xtimes : 1;
                                bdsPos.ResetCurrentItem();
                                return;
                            }
                        }

                    }
                }
            }

            SaleModel sale = null;
            if (saleID.HasValue)
            {
                sale = saleBLL.GetSaleByID(saleID.Value);
            }
            else
            {
                sale = saleBLL.GetSale(clntcode, good, good.xtableid.Value);
            }
            if (sale != null)
            {
                if (string.IsNullOrEmpty(sale.xkind))
                {
                    sale.xkind = ((int)SaleKind.Product).ToString();
                }
                //减价
                if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.j)])
                {
                    JianJia(sale, good);
                }
                //赠品
                else if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.p)])
                {
                    Gift(sale, good, id);
                }
                //打折
                else if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.z)])
                {
                    Discount(sale, good);
                }
                //加送
                else if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.a)])
                {
                    JiaSong(sale, good, id);
                }
                //特价
                else if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.t)])
                {
                    TeJia(sale, good);
                }
            }
            else
            {
                ClntPrice(good);
            }
            CalcSummary();
        }

        #region 减价
        private void JianJia(SaleModel sale, GoodModel good)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            List<SaleruleModel> rlues = sale.salerules;
            //单品
            if (int.Parse(sale.xkind) == (int)SaleKind.Product)
            {
                List<PosbbModel> posbbs = null;
                var rule = GetSalerules(sale, good, out posbbs);
                if (rule != null)
                {
                    foreach (var item in posbbs)
                    {
                        decimal amount = CalcMoneyHelper.Multiply(item.xpricold, item.xquat);
                        item.xallp = CalcMoneyHelper.Subtract(amount, rule.xdo);
                        item.xzhe = CalcMoneyHelper.CalcZhe(item.xallp, amount);
                        item.xpric = CalcMoneyHelper.Multiply(item.xpricold, (item.xzhe / 100));
                        item.xsalesid = sale.xtableid;
                        item.xsalestype = sale.xtype;
                    }
                }
                else
                {
                    foreach (var item in posbbs)
                    {
                        item.xpric = item.xpricold;
                        item.xzhe = 100;
                        item.xallp = CalcMoneyHelper.Multiply(item.xpric, item.xquat);
                        item.xsalesid = null;
                        item.xsalestype = string.Empty;
                    }
                    ClntPrice(good);
                }
            }
            //整单
            else if (int.Parse(sale.xkind) == (int)SaleKind.Order)
            {
                var query = from p in data where (p.goodcode == good.goodcode && p.goodXtableID == good.xtableid) || p.xsalesid == good.xtableid select p;
                if (sale.xunit == "元")
                {

                }
                else if (sale.xunit == "个")
                {

                }
            }
            //半价
            else if (int.Parse(sale.xkind) == (int)SaleKind.Half)
            {
                var query = (from p in data where p.goodcode == good.goodcode && p.goodXtableID == good.xtableid && p.unitname == good.unitname select p).ToList();
                decimal totalCount = query.Sum(r => r.unitquat);
                List<PosbbModel> posbbs = new List<PosbbModel>();
                if (totalCount > 1)
                {
                    //半价数量
                    int halfCount = (int)totalCount / 2;
                    //正价数量
                    int priceCount = (int)halfCount;
                    int index = data.IndexOf(query.FirstOrDefault());

                    CloneEntityHelper cloneEntity = new CloneEntityHelper();
                    PosbbModel price = cloneEntity.Clone<PosbbModel>(query.FirstOrDefault());
                    price.xzhe = 100;
                    price.unitquat = priceCount;
                    if (price.unitrate.HasValue)
                    {
                        price.xquat = priceCount * price.unitrate.Value;
                    }
                    else
                    {
                        price.xquat = priceCount;
                    }
                    price.xpric = price.xpricold;
                    price.xallp = CalcMoneyHelper.Multiply(price.xpric, price.unitquat);
                    price.xsalesid = sale.xtableid;
                    price.xsalestype = sale.xtype;
                    posbbs.Add(price);

                    PosbbModel half = cloneEntity.Clone<PosbbModel>(query.FirstOrDefault());
                    half.ID = Guid.NewGuid();
                    half.PID = price.ID;
                    half.xzhe = 50;
                    half.unitquat = halfCount;
                    if (price.unitrate.HasValue)
                    {
                        half.xquat = halfCount * half.unitrate.Value;
                    }
                    else
                    {
                        half.xquat = halfCount;
                    }
                    half.xpric = CalcMoneyHelper.Divide(half.xpricold, 2);
                    half.xallp = CalcMoneyHelper.Multiply(half.xpric, half.unitquat);
                    half.xsalesid = sale.xtableid;
                    half.xsalestype = sale.xtype;
                    posbbs.Add(half);

                    decimal normalCount = totalCount - halfCount - priceCount;

                    if (normalCount > 0)
                    {
                        PosbbModel normal = cloneEntity.Clone<PosbbModel>(query.FirstOrDefault());
                        normal.xzhe = 100;
                        normal.unitquat = normalCount;
                        if (price.unitrate.HasValue)
                        {
                            normal.xquat = normalCount * normal.unitrate.Value;
                        }
                        else
                        {
                            normal.xquat = normalCount;
                        }
                        normal.xpric = normal.xpricold;
                        normal.xallp = CalcMoneyHelper.Multiply(normal.xpric, normal.unitquat);
                        normal.xsalesid = null;
                        normal.xsalestype = string.Empty;
                        posbbs.Add(normal);
                    }

                    foreach (var item in query)
                    {
                        data.Remove(item);
                    }
                    data.InsertRange(index, posbbs);
                    ClntPrice(good);
                }
                else
                {
                    ClntPrice(good);
                }
            }
        }
        #endregion

        #region 赠品
        private void Gift(SaleModel sale, GoodModel good, Guid id)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            List<SaleruleModel> rlues = sale.salerules;

            //单品
            if (int.Parse(sale.xkind) == (int)SaleKind.Product)
            {
                List<PosbbModel> posbbs = null;
                var rule = GetSalerules(sale, good, out posbbs);
                if (rule != null)
                {
                    int quantity = 1;
                    if (sale.xunit == "个")
                    {
                        quantity = (int)(posbbs.Sum(r => r.unitquat) / rule.xhave);
                    }
                    else
                    {
                        quantity = (int)(posbbs.Sum(r => r.xallp) / rule.xhave);
                    }
                    List<SalegoodXModel> salegoodXs = sale.salegoodXs.Where(r => r.xno == rule.xdo).ToList();
                    foreach (var item in posbbs)
                    {
                        item.xsalesid = sale.xtableid;
                        item.xsalestype = sale.xtype;
                        int rowIndex = data.IndexOf(item);
                        foreach (var goodx in salegoodXs)
                        {
                            rowIndex++;
                            //添加赠品
                            GoodModel entity = goodBLL.GetGoodByID(goodx.xgoodid, goodx.goodcode);
                            AddGift(entity, quantity, sale, rowIndex, id);
                        }
                    }
                }
                else
                {
                    foreach (var item in posbbs)
                    {
                        item.xsalesid = null;
                        item.xsalestype = string.Empty;
                    }
                    ClntPrice(good);
                }

            }
            //整单
            else if (int.Parse(sale.xkind) == (int)SaleKind.Order)
            {
                if (sale.xunit == "元")
                {

                }
                else if (sale.xunit == "个")
                {

                }
            }
        }
        #endregion

        #region 打折
        private void Discount(SaleModel sale, GoodModel good)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            List<SaleruleModel> rlues = sale.salerules;
            //单品
            if (int.Parse(sale.xkind) == (int)SaleKind.Product)
            {
                List<PosbbModel> posbbs = null;
                var rule = GetSalerules(sale, good, out posbbs);
                if (rule != null)
                {
                    foreach (var item in posbbs)
                    {
                        item.xzhe = rule.xdo * 10;
                        item.xpric = CalcMoneyHelper.Multiply(item.xpricold, (item.xzhe / 100));
                        item.xallp = CalcMoneyHelper.Multiply(item.xpric, item.xquat);
                        item.xsalesid = sale.xtableid;
                        item.xsalestype = sale.xtype;
                    }
                }
                else
                {
                    foreach (var item in posbbs)
                    {
                        item.xpric = item.xpricold;
                        item.xzhe = 100;
                        item.xallp = CalcMoneyHelper.Multiply(item.xpric, item.xquat);
                        item.xsalesid = null;
                        item.xsalestype = string.Empty;
                    }
                    ClntPrice(good);
                }

            }
            //整单
            else if (int.Parse(sale.xkind) == (int)SaleKind.Order)
            {
                var query = from p in data where (p.goodcode == good.goodcode && p.goodXtableID == good.xtableid) || p.xsalesid == good.xtableid select p;
                if (sale.xunit == "元")
                {

                }
                else if (sale.xunit == "个")
                {

                }
            }
        }
        #endregion

        #region 加送
        private void JiaSong(SaleModel sale, GoodModel good, Guid id)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            List<SaleruleModel> rlues = sale.salerules;

            //单品
            if (int.Parse(sale.xkind) == (int)SaleKind.Product)
            {
                List<PosbbModel> posbbs = null;
                var rules = GetSalerules_JiaSong(sale, good, out posbbs);
                if (rules != null)
                {
                    int quantity = (int)posbbs.Sum(r => r.xquat);
                    // List<SalegoodXModel> salegoodXs = sale.salegoodXs.Where(r => r.xno == rule.xdo).ToList();
                    foreach (var item in posbbs)
                    {
                        item.xsalesid = sale.xtableid;
                        item.xsalestype = sale.xtype;
                        int rowIndex = data.IndexOf(item);
                        foreach (var goodx in sale.salegoodXs)
                        {
                            rowIndex++;
                            //添加加送赠品
                            GoodModel entity = goodBLL.GetGoodByID(goodx.xgoodid, goodx.goodcode);
                            foreach (var rule in rules)
                            {
                                AddJiaSong(entity, 1, rule.xhave, sale.salegoodXs.Count, sale, rowIndex, id);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in posbbs)
                    {
                        item.xsalesid = null;
                        item.xsalestype = string.Empty;
                    }
                    ClntPrice(good);
                }
            }
            //整单
            else if (int.Parse(sale.xkind) == (int)SaleKind.Order)
            {
                if (sale.xunit == "元")
                {

                }
                else if (sale.xunit == "个")
                {

                }
            }
        }
        #endregion

        #region 特价
        private void TeJia(SaleModel sale, GoodModel good)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            List<SaleruleModel> rlues = sale.salerules;
            List<PosbbModel> posbbs = (from p in data where p.goodcode == good.goodcode && p.unitname == good.unitname && p.goodXtableID == good.xtableid select p).ToList();

            var rule = rlues.FirstOrDefault();
            if (rule != null && good.goodunit == good.unitname)
            {
                foreach (var item in posbbs)
                {
                    item.xpric = rule.xhave;
                    item.xallp = CalcMoneyHelper.Multiply(item.xpric, item.xquat);
                    item.xzhe = CalcMoneyHelper.CalcZhe(item.xpric, item.xpricold);
                    item.xsalesid = sale.xtableid;
                    item.xsalestype = sale.xtype;
                }
            }
            else
            {
                foreach (var item in posbbs)
                {
                    item.xpric = item.xpricold;
                    item.xzhe = 100;
                    item.xallp = CalcMoneyHelper.Multiply(item.xpric, item.xquat);
                    item.xsalesid = null;
                    item.xsalestype = string.Empty;
                }
                ClntPrice(good);
            }
        }
        #endregion

        #region 获取当前货品满足的促销规则
        private SaleruleModel GetSalerules(SaleModel sale, GoodModel good, out List<PosbbModel> posbbs)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            List<SaleruleModel> rlues = sale.salerules;

            posbbs = new List<PosbbModel>();
            //单品
            if (string.IsNullOrEmpty(sale.xkind) || int.Parse(sale.xkind) == (int)SaleKind.Product || int.Parse(sale.xkind) == (int)SaleKind.Half)
            {
                var query = (from p in data where p.goodcode == good.goodcode && p.goodXtableID == good.xtableid select p).ToList();
                posbbs.AddRange(query);
            }
            //整单
            else if (int.Parse(sale.xkind) == (int)SaleKind.Order)
            {
                var query = (from p in data where (p.goodcode == good.goodcode && p.goodXtableID == good.xtableid) || p.xsalesid == good.xtableid select p).ToList();
                posbbs.AddRange(query);
            }

            if (posbbs.Count > 0)
            {
                decimal total = 0;

                if (sale.xunit == "元" || sale.xtype == "t")
                {
                    total = posbbs.Sum(r => r.xpricold * r.unitquat);
                }
                else if (sale.xunit == "个")
                {
                    total = posbbs.Sum(r => r.unitquat);
                }
                if (int.Parse(sale.xkind) == (int)SaleKind.Half)
                {
                    if (total > 1)
                    {
                        return new SaleruleModel();
                    }
                    else
                    {
                        return null;
                    }
                }
                var rule = rlues.Where(r => r.xhave <= total).OrderByDescending(r => r.xhave).FirstOrDefault();
                return rule;
            }
            else
            {
                return null;
            }
        }

        private List<SaleruleModel> GetSalerules_JiaSong(SaleModel sale, GoodModel good, out List<PosbbModel> posbbs)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            List<SaleruleModel> rlues = sale.salerules;

            posbbs = new List<PosbbModel>();
            //单品
            if (int.Parse(sale.xkind) == (int)SaleKind.Product)
            {
                var query = (from p in data where p.goodcode == good.goodcode && p.goodXtableID == good.xtableid select p).ToList();
                posbbs.AddRange(query);
            }
            //整单
            else if (int.Parse(sale.xkind) == (int)SaleKind.Order)
            {
                var query = (from p in data where (p.goodcode == good.goodcode && p.goodXtableID == good.xtableid) || p.xsalesid == good.xtableid select p).ToList();
                posbbs.AddRange(query);
            }

            if (posbbs.Count > 0)
            {
                decimal total = 0;

                if (sale.xunit == "元")
                {
                    total = posbbs.Sum(r => r.xallp);
                }
                else if (sale.xunit == "个")
                {
                    total = posbbs.Sum(r => r.xquat);
                }
                return rlues;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region  添加赠品
        /// <summary>
        /// 添加赠品
        /// </summary>
        /// <param name="good"></param>
        private void AddGift(GoodModel good, int quantity, SaleModel sale, int rowIndex, Guid id)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            var query = data.Where(r => r.goodcode == good.goodcode && r.goodtm == good.goodkind && r.isGift && r.xallp == 0 && r.xsalesid == sale.xtableid && r.PID == id).FirstOrDefault();
            if (query != null)
            {

                query.xquat = quantity;
                query.unitquat = quantity;
                //query.xquat = 1;
                //query.unitquat = 1;
                query.xallp = 0;
                bdsPos.ResetCurrentItem();
            }
            else
            {
                PosbbModel entity = new PosbbModel();
                entity.ID = Guid.NewGuid();
                entity.PID = id;
                entity.isGift = true;
                entity.goodcode = good.goodcode;
                entity.xbarcode = good.xbarcode;
                entity.goodname = good.goodname.Trim();
                entity.goodunit = good.goodunit.Trim();
                entity.goodtm = good.goodkind;
                entity.xpricold = good.xprico;
                entity.goodkind1 = good.goodkind1;
                entity.goodkind2 = good.goodkind2;
                entity.goodkind3 = good.goodkind3;
                entity.goodkind4 = good.goodkind4;
                entity.goodkind5 = good.goodkind5;
                entity.goodkind6 = good.goodkind6;
                entity.goodkind7 = good.goodkind7;
                entity.goodkind8 = good.goodkind8;
                entity.goodkind9 = good.goodkind9;
                entity.goodkind10 = good.goodkind10;
                entity.xquat = quantity;
                entity.unitquat = quantity;
                //entity.xquat = 1;
                //entity.unitquat = 1;
                entity.unitrate = 1;
                entity.unitname = good.goodunit.Trim();
                entity.xzhe = 0;
                entity.xpric = 0;
                entity.xallp = 0;
                entity.goodXtableID = good.xtableid.Value;
                entity.xsalesid = sale.xtableid;
                entity.xsalestype = sale.xtype;
                bdsPos.Insert(rowIndex, entity);
            }
            CalcSummary();
        }
        #endregion

        #region  添加加送赠品
        /// <summary>
        /// 添加加送赠品
        /// </summary>
        /// <param name="good"></param>
        private void AddJiaSong(GoodModel good, int quantity, decimal totalMoney, int totalCount, SaleModel sale, int rowIndex, Guid id)
        {
            decimal price = CalcMoneyHelper.Divide(totalMoney, totalCount);
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            var query = data.Where(r => r.goodcode == good.goodcode && r.goodtm == good.goodkind && r.isGift && r.xpric == price && r.xsalesid == sale.xtableid && r.PID == id).FirstOrDefault();
            if (query != null)
            {
                query.xquat = quantity;
                query.unitquat = quantity;
                query.xallp = CalcMoneyHelper.Multiply(price, quantity);
                bdsPos.ResetCurrentItem();
            }
            else
            {
                PosbbModel entity = new PosbbModel();
                entity.ID = Guid.NewGuid();
                entity.PID = id;
                entity.isGift = true;
                entity.goodcode = good.goodcode;
                entity.xbarcode = good.xbarcode;
                entity.goodname = good.goodname.Trim();
                entity.goodunit = good.goodunit.Trim();
                entity.goodtm = good.goodkind;
                entity.xpricold = price;
                entity.goodkind1 = good.goodkind1;
                entity.goodkind2 = good.goodkind2;
                entity.goodkind3 = good.goodkind3;
                entity.goodkind4 = good.goodkind4;
                entity.goodkind5 = good.goodkind5;
                entity.goodkind6 = good.goodkind6;
                entity.goodkind7 = good.goodkind7;
                entity.goodkind8 = good.goodkind8;
                entity.goodkind9 = good.goodkind9;
                entity.goodkind10 = good.goodkind10;
                entity.xquat = quantity;
                entity.unitquat = quantity;
                entity.unitrate = 1;
                entity.unitname = good.goodunit.Trim();
                entity.xzhe = CalcMoneyHelper.CalcZhe(price, entity.xpricold);
                entity.xpric = price;
                entity.xallp = CalcMoneyHelper.Multiply(price, quantity);
                entity.goodXtableID = good.xtableid.Value;
                entity.xsalesid = sale.xtableid;
                entity.xsalestype = sale.xtype;
                bdsPos.Insert(rowIndex, entity);
            }
            CalcSummary();
        }
        #endregion

        #region 会员价格
        /// <summary>
        /// 会员价格
        /// </summary>
        /// <param name="good"></param>
        private void ClntPrice(GoodModel good)
        {
            List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
            PosbbModel current = data.Where(r => r.goodXtableID == good.xtableid && r.isGift == false && r.xsalesid.HasValue == false && r.unitname == good.unitname).FirstOrDefault();

            if (current != null)
            {
                if (client != null)
                {
                    //货品分类级别折扣价->会员等级价->会员折扣

                    ClntclssModel clntclss = clientBLL.GetClntclssByClass(client.clntclss);
                    //  GoodModel currentGood = goodBLL.GetGoodByID(good.xtableid.Value);
                    //if (currentGood != null)
                    //{
                    Goodtype3Model goodtype3 = goodBLL.GetGoodtype3(good.goodtype3);
                    //示例：{ "1":"90","2":"80"}，说明: "级别XTABLEID":"折扣%"
                    if (goodtype3 != null && !string.IsNullOrEmpty(goodtype3.uclssprics))
                    {
                        if (SetGoodtypePrice(current, clntclss, goodtype3.uclssprics))
                            return;
                    }

                    Goodtype2Model goodtype2 = goodBLL.GetGoodtype2(good.goodtype2);
                    if (goodtype2 != null && !string.IsNullOrEmpty(goodtype2.uclssprics))
                    {
                        if (SetGoodtypePrice(current, clntclss, goodtype2.uclssprics))
                            return;
                    }
                    Goodtype1Model goodtype1 = goodBLL.GetGoodtype1(good.goodtype1);
                    if (goodtype1 != null && !string.IsNullOrEmpty(goodtype1.uclssprics))
                    {
                        if (SetGoodtypePrice(current, clntclss, goodtype1.uclssprics))
                            return;
                    }
                    //}

                    Goodpric2Model goodpric2 = goodBLL.GetGoodpric2(good.xtableid.Value, client.clntclss);

                    if (goodpric2 != null && goodpric2.xpric > 0)
                    {
                        current.xzhe = CalcMoneyHelper.CalcZhe(goodpric2.xpric, current.xpricold);
                        current.xpric = goodpric2.xpric;
                        current.xallp = CalcMoneyHelper.Multiply(current.xpric, current.unitquat);
                        return;
                    }

                    current.xzhe = client.xzhe;
                    current.xpric = CalcMoneyHelper.Multiply(current.xpricold, (client.xzhe / 100));
                    current.xallp = CalcMoneyHelper.Multiply(current.xpric, current.unitquat);
                }
                else
                {
                    if (current.xzhe <= 0 || !current.xsalesid.HasValue)
                    {
                        current.xzhe = 100;
                    }

                    current.xpric = CalcMoneyHelper.Multiply(current.xpricold, (current.xzhe / 100));
                    current.xallp = CalcMoneyHelper.Multiply(current.xpric, current.unitquat);
                }
                current.xtax = CalcMoneyHelper.Multiply(current.xallp, current.xtaxr);
                current.xallpt = CalcMoneyHelper.Add(current.xallp, current.xtax);
                current.xprict = CalcMoneyHelper.Divide(current.xallpt, current.unitquat);
            }
        }

        #region 货品分类折扣价
        private bool SetGoodtypePrice(PosbbModel current, ClntclssModel clntclss, string uclssprics)
        {
            if (uclssprics != string.Empty)
            {
                Dictionary<object, object> uclsspricsDic = js.Deserialize<Dictionary<object, object>>(uclssprics);
                var query = uclsspricsDic.Where(r => int.Parse(r.Key.ToString()) == clntclss.xtableid).FirstOrDefault();
                decimal zhe = 0;
                if (decimal.TryParse(query.Value.ToString(), out zhe))
                {
                    current.xzhe = zhe == 0 ? 100 : zhe;
                    current.xpric = CalcMoneyHelper.Multiply(current.xpricold, (current.xzhe / 100));
                    current.xallp = CalcMoneyHelper.Multiply(current.xpric, current.unitquat);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #endregion

        #region 判断是否满足会员日规则
        /// <summary>
        /// 判断是否满足会员日规则
        /// </summary>
        /// <param name="good"></param>
        /// <returns></returns>
        private bool IsClntDay(GoodModel good)
        {
            if (client != null)
            {
                DateTime date = DateTime.Now;
                ClntDayModel clntday = saleBLL.GetClntDay(date.Day, date.Month);
                if (clntday != null)
                {
                    List<ClntdRuleModel> clntprices = clntday.clntprices;
                    var query = clntprices.Where(r => r.classtype == "小类" && r.goodtype == good.goodtype3).FirstOrDefault();
                    if (query != null)
                    {
                        return true;
                    }
                    query = clntprices.Where(r => r.classtype == "中类" && r.goodtype == good.goodtype2).FirstOrDefault();
                    if (query != null)
                    {
                        return true;
                    }
                    query = clntprices.Where(r => r.classtype == "大类" && r.goodtype == good.goodtype1).FirstOrDefault();
                    if (query != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #endregion

        #region 表体数据源发生改变

        private void bdsPos_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            bool visible = bdsPos.List.Count > 0;
            btnDetail.Enabled = visible;
            btnDel.Enabled = visible;
            btnClear.Enabled = visible;
            btnPending.Enabled = visible;
            btnPos.Enabled = !visible;
            if (currentOperationState == OperationState.FastAppend)
            {
                btnFastBilling.Enabled = true;
            }
            else
            {
                if (currentOperationState != OperationState.Change)
                {
                    btnFastBilling.Enabled = !visible;
                }
                else
                {
                    btnClear.Enabled = true;
                }
            }
            gv_FocusedColumnChanged(null, null);
        }
        #endregion

        #region 过滤货品的促销活动
        private void gv_ShownEditor(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view.FocusedColumn == colxsalesid)
            {
                if (view.ActiveEditor is DevExpress.XtraEditors.LookUpEdit)
                {
                    LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                    PosbbModel focusRow = view.GetFocusedRow() as PosbbModel;
                    if (focusRow != null)
                    {
                        if (focusRow.isGift == false)
                        {
                            string clntcode = string.Empty;
                            if (client != null)
                            {
                                clntcode = client.clntcode;
                            }
                            GoodModel good = SetGood(focusRow);
                            if (good == null)
                            {
                                edit.Properties.DataSource = null;
                                return;
                            }
                            if (IsClntDay(good))
                            {
                                edit.Properties.DataSource = null;
                                return;
                            }
                            List<int> saleIDs = saleBLL.GetSaleIDs(clntcode, focusRow.goodcode, focusRow.goodXtableID);
                            var data = sales.Where(r => r.xtableid.HasValue && saleIDs.Contains(r.xtableid.Value));
                            List<SaleModel> sales_satisfy = new List<SaleModel>();
                            foreach (SaleModel sale in data)
                            {
                                SaleModel currentSale = saleBLL.GetSaleByID(sale.xtableid.Value);
                                List<PosbbModel> posbbs = null;
                                var rule = GetSalerules(currentSale, good, out posbbs);
                                if (rule != null || sale.xtype == "a")
                                {
                                    if (focusRow.unitname == focusRow.goodunit || sale.xtype != "t")
                                    {
                                        sales_satisfy.Add(sale);
                                    }
                                }
                            }
                            edit.Properties.DataSource = sales_satisfy;

                        }
                        else
                        {
                            edit.Properties.DataSource = null;
                        }
                    }
                }
            }
        }
        #endregion

        #region 切换促销活动
        private void rlueSale_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit lueSale = sender as LookUpEdit;
            if (lueSale.EditValue != null)
            {
                PosbbModel current = bdsPos.Current as PosbbModel;
                if (current != null)
                {
                    if (current.xsalesid.HasValue)
                    {
                        List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;
                        var query = (from p in data where p.xsalesid.HasValue && p.xsalesid == current.xsalesid && p.isGift select p).ToList();
                        if (query.Count > 0)
                        {
                            for (int i = query.Count - 1; i >= 0; i--)
                            {
                                bdsPos.Remove(query[i]);
                            }
                        }
                    }
                    GoodModel good = SetGood(current);
                    gv.PostEditor();

                    current.xpric = current.xpricold;
                    current.xzhe = 100;
                    current.xallp = CalcMoneyHelper.Multiply(current.xpricold, current.unitquat);
                    current.xtax = CalcMoneyHelper.Multiply(current.xallp, current.xtaxr);
                    current.xallpt = CalcMoneyHelper.Add(current.xallp, current.xtax);
                    current.xprict = CalcMoneyHelper.Divide(current.xallpt, current.unitquat);
                    if (good == null)
                    {
                        return;
                    }
                    Sale(good, int.Parse(lueSale.EditValue.ToString()), current.ID);
                }
            }
        }
        #endregion

        #region 切换会员

        private void bteClientName_EditValueChanged(object sender, EventArgs e)
        {
            if (bdsPos.List.Count > 0)
            {
                List<PosbbModel> data = bdsPos.DataSource as List<PosbbModel>;

                List<PosbbModel> details = data.Where(r => r.isGift == false).ToList();

                foreach (PosbbModel current in details)
                {
                    if (current.xsalesid.HasValue)
                    {
                        var query = (from p in data where p.xsalesid.HasValue && p.xsalesid == current.xsalesid && p.isGift select p).ToList();
                        if (query.Count > 0)
                        {
                            for (int i = query.Count - 1; i >= 0; i--)
                            {
                                bdsPos.Remove(query[i]);
                            }
                        }
                    }
                    GoodModel good = SetGood(current);
                    if (good == null)
                    {
                        return;
                    }
                    int? saleID = current.xsalesid;
                    Sale(good, saleID, current.ID);
                }
            }
        }
        #endregion

        #region 会员充值
        private void bteBalance_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!ZBIsEnabled())
            {
                return;
            }
            if (client != null)
            {
                FormClientRecharge frm = new FormClientRecharge(client);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //Thread.Sleep(1000);
                    //decimal ojie2 = clientBLL.GetOjie2(client.clntcode);
                    try
                    {
                        UserModel user = RuntimeObject.CurrentUser;
                        bteBalance.EditValue = SyncHelperBLL.Getyck(user.bookID, client.clntcode, user.xls);
                    }
                    catch (Exception)
                    {
                        MessagePopup.ShowError(AppConst.Connect_Msg);
                    }
                }
            }
            else
            {
                FormClientSearch frm = new FormClientSearch(true);
                if (frm.ShowDialog() == DialogResult.OK)
                {

                }
            }

        }
        #endregion

        #region 积分兑换
        private void bteIntegral_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!ZBIsEnabled())
            {
                return;
            }
            if (client != null)
            {
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
                            if (value != 0)
                            {
                                FormClientExchange frm = new FormClientExchange(client);
                                if (frm.ShowDialog() == DialogResult.OK)
                                {
                                    //Thread.Sleep(1000);
                                    //decimal jjie2 = clientBLL.GetJjie2(client.clntcode);
                                    //bteIntegral.EditValue = jjie2;
                                    try
                                    {
                                        ClearClient();
                                        //bteIntegral.EditValue = SyncHelperBLL.Getjifen(RuntimeObject.CurrentUser.bookID, client.clntcode);
                                    }
                                    catch (Exception)
                                    {
                                        MessagePopup.ShowError(AppConst.Connect_Msg);
                                    }
                                }
                            }
                            else
                            {
                                MessagePopup.ShowInformation(AppConst.Open_integral_Msg);
                            }
                        }
                        else
                        {
                            MessagePopup.ShowInformation(AppConst.Open_integral_Msg);
                        }

                    }
                    else
                    {
                        MessagePopup.ShowInformation(AppConst.Open_integral_Msg);
                    }
                }
                else
                {
                    MessagePopup.ShowInformation(AppConst.Open_integral_Msg);
                }
            }
            else
            {
                MessagePopup.ShowInformation("请先选择会员！");
            }

        }
        #endregion

        #region 弹窗关键字操作
        private void bteProduct_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FormPopup frm = new FormPopup(1);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.Key != string.Empty)
                {
                    bteProduct.Text = frm.Key;
                    SearchProduct();
                }
            }
        }

        private void bteClientCode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FormPopup frm = new FormPopup(2);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.Key != string.Empty)
                {
                    bteClientCode.Text = frm.Key;
                    QueryClnt();
                }
            }
        }

        private void bteNoBarcode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FormPopup frm = new FormPopup(3);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.Key != string.Empty)
                {
                    bteNoBarcode.Text = frm.Key;
                    NoBarcode();
                }
            }
        }
        #endregion

        #region 查看促销消息
        private void bbtnSaleNotice_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateWaitDialog();
            bbtnSaleNotice.CanFlash = false;
            SyncHelperBLL.UpdateSyncInfo();
            FormMessageCenter frm = new FormMessageCenter();
            frm.ShowDialog();
            CloseWaitDialog();
        }
        #endregion

        #region 消息中心
        private void blBtnNotify_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreateWaitDialog();
            blBtnNotify.CanFlash = false;
            SyncHelperBLL.UpdateSyncInfo();
            FormMessageCenter frm = new FormMessageCenter();
            frm.ShowDialog();
            CloseWaitDialog();
        }
        #endregion

        #region 弹钱箱
        private void btnOpenCashbox_Click(object sender, EventArgs e)
        {
            if (!BaseForm.GetPermission(Functions.OpenCashbox))
            {
                return;
            }
            CashboxHelper cb = new CashboxHelper();
            cb.Open();
            cb.Write();
            cb.Close();
        }
        #endregion

        #region 列设置
        private void tsmColumnsSetting_Click(object sender, EventArgs e)
        {
            gv.ShowCustomization();
            gv.CustomizationForm.Text = "自定义列";
        }
        #endregion

        #region 还原默认设置
        private void tsmRestore_Click(object sender, EventArgs e)
        {
            try
            {
                gv.RestoreLayoutFromXml(filePath_default);
            }
            catch (FileNotFoundException ex)
            {
                MessagePopup.ShowError("找不到目标文件！");
            }
        }
        #endregion

        #region  回车键切换光标
        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e)
        {
            DevExpress.XtraGrid.GridControl gc = (DevExpress.XtraGrid.GridControl)sender;

            DevExpress.XtraGrid.Views.Grid.GridView gv = (DevExpress.XtraGrid.Views.Grid.GridView)gc.FocusedView;
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (chkEnter.Checked)
                {
                    if (gv.FocusedColumn == colunitquat)
                    {
                        gv.FocusedColumn = colxpric;
                    }
                    else if (gv.FocusedColumn == colxpric)
                    {
                        if (gv.FocusedRowHandle == gv.RowCount - 1)
                        {
                            gv.PostEditor();
                            gv.CloseEditor();
                            colxpric.OptionsColumn.AllowFocus = false;
                            this.ActiveControl = bteProduct;
                            bteProduct.Focus();
                            colxpric.OptionsColumn.AllowFocus = true;
                        }
                        else
                        {
                            gv.FocusedRowHandle += 1;
                            gv.FocusedColumn = colunitquat;
                        }
                    }
                    gv.ShowEditor();
                    e.Handled = true;

                }
            }
        }
        #endregion

        #region 快速开单
        private void btnFastBilling_Click(object sender, EventArgs e)
        {
            if (!BaseForm.GetPermission(Functions.Cashier))
            {
                return;
            }
            if (btnFastBilling.Enabled)
            {
                btnFastBilling.Enabled = false;
                if (currentOperationState == OperationState.FastAppend)
                {
                    if (bdsPos.List.Count == 0)
                    {
                        if (MessagePopup.ShowQuestion("当前没有商品,是否取消追加？") == DialogResult.Yes)
                        {
                            currentOperationState = OperationState.None;
                            SetFastBillingBtnText(currentOperationState);
                            btnReceipt.Enabled = true;
                            Clear();
                            return;
                        }
                        else
                        {
                            btnFastBilling.Enabled = true;
                        }
                    }
                    else if (MessagePopup.ShowQuestion("确认追加当前单据？") == DialogResult.Yes)
                    {
                        List<PosbbModel> currentDetail = bdsPos.DataSource as List<PosbbModel>;
                        decimal xallp = currentDetail.Sum(r => r.xallp);
                        if (xallp != currentPoshh.xpay)
                        {
                            MessagePopup.ShowInformation("录入单据的金额和收款的金额不一致！");
                            btnFastBilling.Enabled = true;
                            return;
                        }
                        if (!CheckInventory(currentDetail))
                        {
                            btnFastBilling.Enabled = true;
                            return;
                        }
                        currentPoshh.Posbbs = currentDetail;
                        foreach (var item in currentPoshh.Posbbs)
                        {
                            if (string.IsNullOrEmpty(item.cnkucode))
                            {
                                item.cnkucode = RuntimeObject.CurrentUser.cnkucode;
                                item.cnkuname = RuntimeObject.CurrentUser.cnkuname;
                            }
                        }
                        if (posBLL.FastBillingAppend(currentPoshh))
                        {
                            Clear();
                            BaseForm baseForm = new BaseForm();
                            baseForm.SyncPOS(this);
                        }
                    }
                    else
                    {
                        btnFastBilling.Enabled = true;
                    }
                }
                else
                {
                    PoshhModel poshh = SetPoshh(PosState.Additional);

                    if (client != null)
                    {
                        client.balance = decimal.Parse(bteBalance.Text.Trim());
                    }
                    FormReceipt frm = new FormReceipt(poshh, client, true);
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        Clear();
                    }
                    btnFastBilling.Enabled = true;
                }
            }
        }
        #endregion

        #region 创建云后台快捷方式
        private void btnCreateShortcut_ItemClick(object sender, ItemClickEventArgs e)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(Path.Combine(basePath, exeName), exeName + ".exe");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (!System.IO.File.Exists(path))
            {
                string fileName = Path.Combine(Path.Combine(Application.StartupPath, "image"), "logo.ico");
                Uri url = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
                url = new Uri(url, string.Format("xc.jsp?id={0}", RuntimeObject.CurrentUser.bookID));
                System.IO.StreamWriter objWriter = System.IO.File.CreateText(Path.Combine(desktopPath, "星城云后台.url"));
                objWriter.WriteLine("[InternetShortcut]");
                objWriter.WriteLine("URL=" + url);
                objWriter.WriteLine("IDList=");
                objWriter.WriteLine("HotKey=0");
                objWriter.WriteLine("IconFile=" + fileName);
                objWriter.WriteLine("IconIndex = 0");
                objWriter.Close();
            }
            else
            {
                WshShell shell = new WshShell();
                string shotcutName = "星城云后台.lnk";
                string shortcutAddress = Path.Combine(desktopPath, shotcutName);
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.Description = "";
                shortcut.TargetPath = path;
                shortcut.Save();
            }
        }
        #endregion

        #region 检查负库存
        private bool CheckInventory(List<PosbbModel> posbbs)
        {
            bool NGKU_SALE = false;
            PossettingModel entity = possettingBLL.GetPossettingByKey(AppConst.NGKU_SALE);
            if (entity != null)
            {
                int result = 0;
                if (int.TryParse(entity.xpvalue, out result))
                {
                    NGKU_SALE = result > 0;
                }
            }

            if (!NGKU_SALE)
            {
                ku2 = goodBLL.GetKu2(RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.cnkucode);
                var query = (from p in posbbs
                             group p by new { p.goodcode, p.goodname, p.goodunit, p.goodtm, p.xchg } into g
                             select new
                             {
                                 goodcode = g.Key.goodcode,
                                 goodname = g.Key.goodname,
                                 goodunit = g.Key.goodunit,
                                 goodtm = g.Key.goodtm,
                                 xquat = g.Sum(r => r.xquat),
                                 xchg = g.Key.xchg
                             }).ToList();
                foreach (var item in query)
                {
                    if (item.xquat == 0)
                    {
                        MessagePopup.ShowInformation(string.Format("{0}数量不能为零！", item.goodname));
                        return false;
                    }
                    if (item.xchg != "换进")
                    {
                        string key = (item.goodcode + item.goodunit + item.goodtm).Trim();
                        decimal xquatku = ku2.Where(r => r.key == key).Where(r => r.xquatku.HasValue).Sum(r => r.xquatku.Value);
                        if (xquatku < item.xquat)
                        {
                            MessagePopup.ShowInformation(string.Format("{0}不允许负库存开单！", item.goodname));
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region 退出
        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnExit.Tag = string.Empty;
            ApplicationHelper.Restart();
        }
        #endregion

        #region 表格 FocusedColumnChanged 事件
        private void gv_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (currentOperationState != OperationState.Change)
            {
                colxchg.OptionsColumn.AllowEdit = false;
            }
            else
            {
                colxchg.OptionsColumn.AllowEdit = true;
            }
        }
        #endregion

        #region 赠品不允许修改数量和价格
        RepositoryItem _disabledItem;
        private void gv_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column == colunitquat || e.Column == colxpric)
            {
                PosbbModel row = gv.GetRow(e.RowHandle) as PosbbModel;
                if (row != null && row.isGift)
                {
                    _disabledItem = (RepositoryItem)e.RepositoryItem.Clone();
                    _disabledItem.ReadOnly = true;
                    _disabledItem.Enabled = false;
                    _disabledItem.AllowFocused = false;
                    e.RepositoryItem = _disabledItem;
                }
            }
        }
        #endregion
    }


}