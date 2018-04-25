using POS.Control.Ribbon.ViewInfo;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.ViewInfo;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace POS.Control.Ribbon
{
    [ToolboxItem(true)]
    public partial class XRibbonControl : RibbonControl
    {


        public XRibbonControl()
        {
            InitializeComponent();
        }

        private XRibbonViewInfo viewInfo;

        protected override RibbonViewInfo CreateViewInfo()
        {
            viewInfo = new XRibbonViewInfo(this);
            return viewInfo;
        }

        //禁掉一些自定义的右键菜单
        protected override void RaiseShowCustomizationMenu(RibbonCustomizationMenuEventArgs args)
        {
            if (!base.IsDesignMode)
            {
                args.ShowCustomizationMenu = false;
            }
            base.RaiseShowCustomizationMenu(args);
        }


    }
}
