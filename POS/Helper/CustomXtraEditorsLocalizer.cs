using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors.Controls;

namespace POS.Helper
{
    public class CustomXtraEditorsLocalizer : DevExpress.XtraEditors.Controls.Localizer
    {
        public override string GetLocalizedString(StringId id)
        {
            switch (id)
            {
                case StringId.TextEditMenuCopy: return "复制";
                case StringId.TextEditMenuCut: return "剪切";
                case StringId.TextEditMenuDelete: return "删除";
                case StringId.TextEditMenuPaste: return "粘贴";
                case StringId.TextEditMenuSelectAll: return "全选";
                case StringId.TextEditMenuUndo: return "撤销";
                case StringId.XtraMessageBoxAbortButtonText: return "中断";
                case StringId.XtraMessageBoxCancelButtonText: return "取消";
                case StringId.XtraMessageBoxIgnoreButtonText: return "忽略";
                case StringId.XtraMessageBoxNoButtonText: return "否";
                case StringId.XtraMessageBoxOkButtonText: return "确定";
                case StringId.XtraMessageBoxRetryButtonText: return "重试";
                case StringId.XtraMessageBoxYesButtonText: return "是";
                default: return base.GetLocalizedString(id);
            }

        }
    }
}
