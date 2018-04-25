using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Helper
{
   public class MessagePopup
    {
        /// <summary>
        /// 错误消息提示
        /// </summary>
        /// <param name="message"></param>
        public static System.Windows.Forms.DialogResult ShowError(string message)
        {
            return XtraMessageBox.Show(message, "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        /// <summary>
        /// 警告消息提示
        /// </summary>
        /// <param name="message"></param>
        public static System.Windows.Forms.DialogResult ShowWarning(string message)
        {
            return XtraMessageBox.Show(message, "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="message"></param>
        public static System.Windows.Forms.DialogResult ShowInformation(string message)
        {
            return XtraMessageBox.Show(message, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }

        /// <summary>
        /// 问题提示
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static System.Windows.Forms.DialogResult ShowQuestion(string message)
        {
            return XtraMessageBox.Show(message, "问题", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
        }

        /// <summary>
        /// 问题提示
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static System.Windows.Forms.DialogResult ShowQuestion(string message, System.Windows.Forms.MessageBoxDefaultButton defaultButton)
        {
            return XtraMessageBox.Show(message, "问题", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, defaultButton);
        }
    }
}
