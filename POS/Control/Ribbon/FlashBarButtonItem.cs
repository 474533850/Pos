using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POS.Control.Ribbon
{
    public class FlashBarButtonItem : BarButtonItem
    {
        private Color ogrinalColor;
        private Color moveOnColor = Color.FromArgb(80, Color.WhiteSmoke.R, Color.WhiteSmoke.G, Color.WhiteSmoke.B);
        private Color flashColor1 = Color.Transparent;
        private Color flashColor2 = Color.FromArgb(80, Color.WhiteSmoke.R, Color.WhiteSmoke.G, Color.WhiteSmoke.B);
        private bool canFlash = false;
        private bool isMouseOn = false;
        SynchronizationContext m_SyncContext = null;
        /// <summary>
        /// 是否闪烁
        /// </summary>
        public bool CanFlash
        {
            get { return canFlash; }
            set { canFlash = value; }
        }

        public FlashBarButtonItem()
        {
            m_SyncContext = SynchronizationContext.Current;
            if (this.Manager != null)
            {
                this.Manager.HighlightedLinkChanged += Manager_HighlightedLinkChanged;
            }
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Enabled = false;
            timer.Tick += Timer_Tick;
            timer.Start();

            ogrinalColor = this.ItemAppearance.Normal.BackColor;
            this.ItemAppearance.Hovered.BackColor = Color.DarkOrange;
        }

        void Manager_HighlightedLinkChanged(object sender, HighlightedLinkChangedEventArgs e)
        {
            if (e.Link.Item == this)
            {
                isMouseOn = true;
                this.ItemAppearance.Normal.BackColor = moveOnColor;
            }
            else
            {
                isMouseOn = false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            m_SyncContext.Post(p =>
            {
                Flash();
            }, null);
        }

        private delegate void FlashCallback();

        bool selectedVisible = true;
        private void Flash()
        {
            if (canFlash)
            {
                this.Manager.SelectLink(selectedVisible ? this.Links[0] : null);
                selectedVisible = !selectedVisible;
            }
        }

    }
}
