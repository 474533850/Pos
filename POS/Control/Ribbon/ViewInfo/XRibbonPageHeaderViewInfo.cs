using DevExpress.XtraBars.Ribbon.ViewInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace POS.Control.Ribbon.ViewInfo
{
    public class XRibbonPageHeaderViewInfo : RibbonPageHeaderViewInfo
    {
        public XRibbonPageHeaderViewInfo(RibbonViewInfo viewInfo) : base(viewInfo)
        {
        }

        protected override Rectangle CalcBounds()
        {
            Rectangle r = base.CalcBounds();
            XRibbonViewInfo xRibbonViewInfo = (this.ViewInfo as XRibbonViewInfo);
            return new Rectangle(r.X, xRibbonViewInfo.TopIndent, ViewInfo.Caption.ContentBounds.Width + XRibbonViewInfo.RightIndent, r.Height);
        }

        protected override Rectangle CalcAvailableHeaderRect()
        {
            Rectangle rect = base.CalcAvailableHeaderRect();
            XRibbonViewInfo xRibbonViewInfo = (this.ViewInfo as XRibbonViewInfo);
            rect.Width -= PageHeaderItemsBounds.Width;
            return rect;
        }
    }
}
