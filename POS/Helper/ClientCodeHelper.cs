using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Helper
{
    /// <summary>
    /// 会员代码帮助类
    /// </summary>
    public class ClientCodeHelper
    {
        /// <summary>
        /// 根据GUID获取19位的唯一数字序列 
        /// </summary>
        /// <returns></returns>
        public static string GenerateClientCode()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0).ToString();
        }
    }
}
