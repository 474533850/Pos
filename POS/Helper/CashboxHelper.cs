using POS.BLL;
using POS.Common;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace POS.Helper
{
    /// <summary>
    /// 钱箱帮助类
    /// </summary>
    public class CashboxHelper
    {
        PossettingBLL possettingBLL = new PossettingBLL();
        List<PossettingModel> possettings = null;
        public CashboxHelper()
        {
            possettings = possettingBLL.GetPossetting();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            int Internal;
            int InternalHigh;
            int Offset;
            int OffSetHigh;
            int hEvent;
        }
        [DllImport("kernel32.dll")]
        private static extern int CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, int lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);
        [DllImport("kernel32.dll")]
        private static extern bool WriteFile(int hFile, byte[] lpBuffer, int nNumberOfBytesToWriter, out int lpNumberOfBytesWriten, out OVERLAPPED lpOverLapped);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int hObject);
        [DllImport("fnthex32.dll")]
        public static extern int GETFONTHEX(string barcodeText, string fontName, int orient, int height, int width, int isBold, int isItalic, StringBuilder returnBarcodeCMD);
        private int iHandle;
        //打开LPT 端口
        public bool Open()
        {
            string port = "lpt1";
            PossettingModel entity = possettings.Where(r => r.xpname == AppConst.Cashbox_Port).FirstOrDefault();
            if (entity != null && !string.IsNullOrEmpty(entity.xpvalue))
            {
                port = entity.xpvalue.Trim();
            }

            iHandle = CreateFile(port, 0x40000000, 0, 0, 3, 0, 0);
            if (iHandle != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //打印函数，参数为打印机的命令或者其他文本！
        public bool Write()
        {
            //小票打印机的命令  
            string instructions = ((char)27).ToString() + "p" + ((char)0).ToString() + ((char)128).ToString() + ((char)128).ToString();
            PossettingModel entity = possettings.Where(r => r.xpname == AppConst.Cashbox_Order).FirstOrDefault();
            if (entity != null && !string.IsNullOrEmpty(entity.xpvalue))
            {
                instructions = entity.xpvalue.Trim();
            }
            if (iHandle != 1)
            {
                int i;
                OVERLAPPED x;
                byte[] mybyte = System.Text.Encoding.Default.GetBytes(instructions);
                return WriteFile(iHandle, mybyte, mybyte.Length, out i, out x);
            }
            else
            {
                throw new Exception("端口未打开！");
            }
        }
        //关闭打印端口
        public bool Close()
        {
            return CloseHandle(iHandle);
        }
    }
}
