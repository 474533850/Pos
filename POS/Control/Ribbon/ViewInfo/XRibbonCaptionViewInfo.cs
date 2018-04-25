using DevExpress.Utils.Drawing;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.Drawing;
using DevExpress.XtraBars.Ribbon.ViewInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Control.Ribbon.ViewInfo
{
    public class XRibbonCaptionViewInfo : RibbonCaptionViewInfo
    {
        public XRibbonCaptionViewInfo(RibbonViewInfo viewInfo) : base(viewInfo)
        {
        }

        protected override ObjectPainter CreateCaptionPainter()
        {
            if (this.Form != null)
            {
                if (this.Form.RibbonVisibility == RibbonVisibility.Auto
                    || this.Form.RibbonVisibility == RibbonVisibility.Visible)
                {
                    return new RibbonCaptionPainter();
                }
                else
                {
                    return base.CreateCaptionPainter();
                }
            }
            else
            {
                return base.CreateCaptionPainter();

            }
        }
    }
}
