using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Model;

namespace POS.Sale
{
    public partial class FormChangeConfirm : BaseForm
    {
        private PoshhModel poshhOld;
        private PoshhModel poshhNew;
        public FormChangeConfirm(PoshhModel poshhOld, PoshhModel poshhNew)
        {
            InitializeComponent();
            this.poshhOld = poshhOld;
            this.poshhNew = poshhNew;
        }

        private void FormChangeConfirm_Load(object sender, EventArgs e)
        {
            List<PosbbModel> posbbOlds = (from p in poshhOld.Posbbs
                                         select new PosbbModel
                                         {
                                             goodcode=p.goodcode,
                                             goodname=p.goodname,
                                             goodtm=p.goodtm,
                                             goodunit=p.goodunit,
                                             xbarcode=p.xbarcode,
                                             goodkind1=p.goodkind1,
                                             goodkind2=p.goodkind2,
                                             goodkind3=p.goodkind3,
                                             goodkind4=p.goodkind4,
                                             goodkind5=p.goodkind5,
                                             goodkind6=p.goodkind6,
                                             goodkind7=p.goodkind7,
                                             goodkind8=p.goodkind8,
                                             goodkind9=p.goodkind9,
                                             goodkind10=p.goodkind10,
                                             goodgive=p.goodgive,
                                             cnkucode=p.cnkucode,
                                             cnkuname=p.cnkuname,
                                             xquat=p.xquat,
                                             xpricold=p.xpricold,
                                             xzhe=p.xzhe,
                                             xpric=p.xpric,
                                             xallp=p.xallp,
                                             xtaxr=p.xtaxr,
                                             xtax=p.xtax,
                                             xprict=p.xprict,
                                             xallpt=p.xallpt,
                                             unitname=p.unitname,
                                             unitrate=p.unitrate,
                                             unitquat=p.unitquat,
                                             unitpric=p.unitpric,
                                             xsalestype=p.xsalestype,
                                             xsalesid=p.xsalesid,
                                             goodXtableID=p.goodXtableID,
                                             PID=p.PID,
                                             xpoints=p.xpoints,
                                             xsendjf=p.xsendjf
                                         }).ToList();
            List<PosbbModel> posbbNews = (from p in poshhNew.Posbbs
                                          select new PosbbModel
                                          {
                                              goodcode = p.goodcode,
                                              goodname = p.goodname,
                                              goodtm = p.goodtm,
                                              goodunit = p.goodunit,
                                              xbarcode = p.xbarcode,
                                              goodkind1 = p.goodkind1,
                                              goodkind2 = p.goodkind2,
                                              goodkind3 = p.goodkind3,
                                              goodkind4 = p.goodkind4,
                                              goodkind5 = p.goodkind5,
                                              goodkind6 = p.goodkind6,
                                              goodkind7 = p.goodkind7,
                                              goodkind8 = p.goodkind8,
                                              goodkind9 = p.goodkind9,
                                              goodkind10 = p.goodkind10,
                                              goodgive = p.goodgive,
                                              cnkucode = p.cnkucode,
                                              cnkuname = p.cnkuname,
                                              xquat = p.xquat,
                                              xpricold = p.xpricold,
                                              xzhe = p.xzhe,
                                              xpric = p.xpric,
                                              xallp = p.xallp,
                                              xtaxr = p.xtaxr,
                                              xtax = p.xtax,
                                              xprict = p.xprict,
                                              xallpt = p.xallpt,
                                              unitname = p.unitname,
                                              unitrate = p.unitrate,
                                              unitquat = p.unitquat,
                                              unitpric = p.unitpric,
                                              xsalestype = p.xsalestype,
                                              xsalesid = p.xsalesid,
                                              goodXtableID = p.goodXtableID,
                                              PID = p.PID,
                                              xpoints = p.xpoints,
                                              xsendjf = p.xsendjf
                                          }).ToList();
            List<PosbbModel> posbbs_In = posbbOlds.Intersect(posbbNews).ToList();
            List<PosbbModel> posbbs_Out = posbbNews.Except(posbbOlds).ToList();
            List<PosbbModel> data = new List<PosbbModel>();
            data.AddRange(posbbs_In);
            data.AddRange(posbbs_Out);
            gridControl1.DataSource = data;
            // List<PosbbModel> data = posbbs_In.AddRange(posbbs_Out);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {

        }
    }
}