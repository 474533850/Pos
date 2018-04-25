using DevExpress.XtraBars.Ribbon.ViewInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraBars.Ribbon;
using System.Drawing;

namespace POS.Control.Ribbon.ViewInfo
{
    public class XRibbonViewInfo : RibbonViewInfo
    {
        public XRibbonViewInfo(RibbonControl ribbon) : base(ribbon)
        {
        }

        protected override void CalcPanel()
        {
            base.CalcPanel();
            if (this.Form != null)
            {
                if (this.Form.RibbonVisibility == RibbonVisibility.Auto
                    || this.Form.RibbonVisibility == RibbonVisibility.Visible)
                {
                    if (this.Ribbon.RibbonStyle == RibbonControlStyle.TabletOfficeEx
                         || this.Ribbon.RibbonStyle == RibbonControlStyle.TabletOffice
                         || this.Ribbon.RibbonStyle == RibbonControlStyle.OfficeUniversal
                         )
                    {
                        if (this.Form.WindowState == System.Windows.Forms.FormWindowState.Maximized)
                        {
                            this.Ribbon.MaximumSize = new Size(0, this.Caption.Bounds.Height + 0);
                        }
                        else
                        {
                            this.Ribbon.MaximumSize = new Size(0, this.Caption.Bounds.Height + 0);
                        }
                    }
                }
            }
        }

        protected override RibbonPageHeaderViewInfo CreateHeaderInfo()
        {
            return new XRibbonPageHeaderViewInfo(this);
        }
        protected override RibbonCaptionViewInfo CreateCaptionInfo()
        {
            return new XRibbonCaptionViewInfo(this);
        }
        protected override int UniversalOfficeApplicationButtonLeftIndent
        {
            get
            {
                return base.UniversalOfficeApplicationButtonLeftIndent + Caption.ContentBounds.X - 10;
            }
        }
        protected override int CalcMinHeight()
        {
            Size s = base.CalcApplicationButtonSize();
            if (this.Form != null)
            {
                if (this.Form.RibbonVisibility == RibbonVisibility.Auto
                    || this.Form.RibbonVisibility == RibbonVisibility.Visible)
                {
                    return base.CalcMinHeight() - Caption.Bounds.Height + TopIndent + TopIndent;
                }
                else
                {
                    return base.CalcMinHeight();
                }
            }
            else
            {
                return base.CalcMinHeight();
            }
        }

        protected override Size CalcApplicationButtonSize()
        {
            Size s = base.CalcApplicationButtonSize();
            s.Width = 1;
            return s;
        }


        public int TopIndent
        {
            get
            {
                if (this.Form != null)
                {
                    if (this.Form.WindowState == System.Windows.Forms.FormWindowState.Maximized)
                    {
                        return 12;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        public static int LeftIndent { get { return -1; } }

        public static int RightIndent { get { return 0; } }

    }
}
