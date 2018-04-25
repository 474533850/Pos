// Developer Express Code Central Example:
// Custom RibbonControl - How to provide CustomDraw capabilities for bar items
// 
// This example illustrates how to implement your own event at a RibbonControl
// level to allow you to draw a bar item link as required.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E3153

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.ViewInfo;

namespace POS.Control.Ribbon.Ribbon
{
    public class XRibbonBarManager : RibbonBarManager
    {
        public XRibbonBarManager(RibbonControl ribbonControl) : base(ribbonControl) { }
        internal new BarSelectionInfo SelectionInfo
        {
            get { return base.SelectionInfo; }
        }
    }
}