using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace POS.Control.Ribbon
{
    public class FlashBarButtonItemNew : BarButtonItem
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

        public FlashBarButtonItemNew()
        {
            m_SyncContext = SynchronizationContext.Current;
            if (this.Manager != null)
            {
                this.Manager.HighlightedLinkChanged += Manager_HighlightedLinkChanged;
            }
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Enabled = false;
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            ogrinalColor = this.ItemAppearance.Normal.BackColor;
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

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_SyncContext.Post(p =>
            {
                Flash();
            }, null);
        }

        private delegate void FlashCallback();

        private void Flash()
        {
            if (isMouseOn)
            {
                this.ItemAppearance.Normal.BackColor = moveOnColor;
            }
            else
            {
                if (canFlash)
                {
                    if (this.ItemAppearance.Normal.BackColor == Color.SkyBlue)
                    {
                        this.ItemAppearance.Normal.BackColor = Color.Transparent;
                    }
                    else
                    {
                        this.ItemAppearance.Normal.BackColor = Color.SkyBlue;
                    }
                }
                else
                {
                    this.ItemAppearance.Normal.BackColor = ogrinalColor;
                }
            }
        }

    }
}
