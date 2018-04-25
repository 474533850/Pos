using POS.BLL;
using POS.Common;
using POS.Common.Enum;
using POS.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace POS.Helper
{
    /// <summary>
    /// 客显
    /// </summary>
    public class CustomerDisplayHelper
    {
        public string spPortName;
        private int spBaudRate;
        private bool is_Customer_Display = true;
        private CustomerDispiayType dispiayType;
        PossettingBLL possettingBLL = new PossettingBLL();

        #region 客显发送类型
        /// <summary>  
        /// 客显发送类型  
        /// </summary>  
        public CustomerDispiayType DispiayType
        {
            get
            {
                return dispiayType;
            }
            set
            {
                dispiayType = value;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="_spPortName">端口名称（COM1,COM2，COM3...）</param>  
        /// <param name="_spBaudRate">通信波特率（2400,9600....）</param>  
        /// <param name="_spStopBits">停止位</param>  
        /// <param name="_spDataBits">数据位</param>  
        public CustomerDisplayHelper(string _spPortName, int _spBaudRate)
        {
            this.spBaudRate = _spBaudRate;
            this.spPortName = _spPortName;
        }

        public CustomerDisplayHelper()
        {
            List<PossettingModel> possettings = possettingBLL.GetPossetting();
            PossettingModel entity = possettings.Where(r => r.xpname == AppConst.Is_Customer_Display).FirstOrDefault();
            if (entity == null || bool.Parse(entity.xpvalue))
            {
                is_Customer_Display = true;
                this.spPortName = string.Empty;
                this.spBaudRate = 2400;
                entity = possettings.Where(r => r.xpname == AppConst.Customer_Addr).FirstOrDefault();
                if (entity != null && !string.IsNullOrEmpty(entity.xpvalue))
                {
                    this.spPortName = entity.xpvalue;
                }
                entity = possettings.Where(r => r.xpname == AppConst.BaudRate).FirstOrDefault();
                if (entity != null && !string.IsNullOrEmpty(entity.xpvalue))
                {
                    this.spBaudRate = int.Parse(entity.xpvalue);
                }
            }
        }
        #endregion --构造函数

        #region 数据信息展现
        /// <summary>  
        /// 数据信息展现  
        /// </summary>  
        /// <param name="data">发送的数据（清屏可以为null或者空）</param>  
        public void DisplayData(string data)
        {
            if (is_Customer_Display)
            {
                SerialPort serialPort = new SerialPort();

                serialPort.PortName = spPortName;
                serialPort.BaudRate = spBaudRate;
                serialPort.StopBits = StopBits.One;
                serialPort.DataBits = 8;
                serialPort.Open();

                //先清屏  
                serialPort.WriteLine(((char)12).ToString());
                //指示灯  (char)27=ESC  (char)115=s
                string str = ((char)27).ToString() + ((char)115).ToString() + ((int)this.DispiayType).ToString();

                serialPort.WriteLine(str);

                //发送数据
                if (!string.IsNullOrEmpty(data))
                {
                    //  (char)81=Q   (char)65=A   (char)13=CR
                    serialPort.Write(((char)27).ToString() + ((char)81).ToString() + ((char)65).ToString() + data + ((char)13).ToString());

                }

                serialPort.Close();
            }
        }
        #endregion

        #region 获取所有的串口名称
        /// <summary>
        /// 获取所有的串口名称
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }
        #endregion
    }
}
