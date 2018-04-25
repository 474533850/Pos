using DevExpress.XtraGrid.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Helper
{
   public class CustomGridLocalizer : GridLocalizer
    {
        public override string GetLocalizedString(GridStringId id)
        {
            switch (id)
            {
                case GridStringId.CustomizationFormColumnHint: return "在此拖拉列来定制布局";
                case GridStringId.FindControlFindButton: return "查找";
                case GridStringId.FindControlClearButton: return "清除";
                case GridStringId.FindNullPrompt:return "输入文本回车查询";
                default: return base.GetLocalizedString(id);
            }
        }
    }
}
