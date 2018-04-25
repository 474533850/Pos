using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS.Sale
{
    public partial class FormSaleQuery : BaseForm
    {
        public FormSaleQuery()
        {
            InitializeComponent();
            panelBottom_SizeChanged(null,null);
        }

        private void panelBottom_SizeChanged(object sender, EventArgs e)
        {
            this.panelPage.Left= (panelBottom.Width -panelPage.Width)/ 2;
        }
    }
}
