using POS.BLL;
using POS.Common;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Helper
{
    /// <summary>
    /// 计算抹掉金额帮助类
    /// </summary>
    public class CalcEraseAmountHelper
    {
        /// <summary>
        /// 计算金额
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="decimals">保留小数位数</param>
        /// <returns></returns>

        public static decimal Calc(decimal money, int decimals, bool isRound)
        {

            decimal result = 0;
            if (isRound)
            {
                result = Math.Round(money, decimals, MidpointRounding.AwayFromZero);
            }
            else
            {
                result = CutDecimalWithN(money, decimals);
            }

            return result;

        }

        /// <summary>
        /// 使Decimal类型数据保留N位小数且不进行四舍五入操作
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static decimal CutDecimalWithN(decimal d, int decimals)
        {
            int n = decimals;
            string strDecimal = d.ToString();
            int index = strDecimal.IndexOf(".");
            if (index == -1 || strDecimal.Length < index + n + 1)
            {
                strDecimal = string.Format("{0:F" + n + "}", d);
            }
            else
            {
                int length = index;
                if (n != 0)
                {
                    length = index + n + 1;
                }
                strDecimal = strDecimal.Substring(0, length);
            }
            return Decimal.Parse(strDecimal);
        }
    }
}
