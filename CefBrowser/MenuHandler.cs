using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefBrowser
{
    public class MenuHandler : IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            // model.Clear();
            model.SetLabel(CefMenuCommand.Back, "返回");
            model.SetLabel(CefMenuCommand.Forward, "前进");
            model.SetLabel(CefMenuCommand.Print, "打印");
            if (model.IsVisible(CefMenuCommand.Back))
            {
                model.AddItem(CefMenuCommand.Reload, "重新加载");
            }
            model.SetLabel(CefMenuCommand.Undo, "撤消");
            model.SetLabel(CefMenuCommand.Redo, "重做");
            model.SetLabel(CefMenuCommand.Cut, "剪切");
            model.SetLabel(CefMenuCommand.Copy, "复制");
            model.SetLabel(CefMenuCommand.Paste, "粘贴");
            model.SetLabel(CefMenuCommand.Delete, "删除");
            model.SetLabel(CefMenuCommand.SelectAll, "全选");
            model.Remove(CefMenuCommand.ViewSource);
            if (frame != null && !frame.IsMain)
            {
                model.AddItem(CefMenuCommand.CustomFirst, "重新加载框架");
            }
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            if (frame != null && commandId == CefMenuCommand.CustomFirst)
            {
                frame.LoadUrl(frame.Url);
            }
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
