
using POS.Common;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Common.utility
{
    /// <summary>
    /// 计算金额帮助类
    /// </summary>
    public class CalcMoneyHelper
    {
        /// <summary>
        /// 加法四舍五入
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="decimals">保留小数位数</param>
        /// <returns></returns>

        public static decimal Add(object obj1, object obj2)
        {
            int decimals = AppConst.DecimalPlaces;
            decimal result = 0;
            if (obj1 != null && !string.IsNullOrEmpty(obj1.ToString()))
            {
                decimal d1;
                if (decimal.TryParse(obj1.ToString(), out d1))
                {
                    if (obj2 != null && !string.IsNullOrEmpty(obj2.ToString()))
                    {
                        decimal d2;
                        if (decimal.TryParse(obj2.ToString(), out d2))
                        {
                            if (AppConst.IsRound)
                            {
                                result = Math.Round((d1 + d2), decimals, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                result = CutDecimalWithN(d1 + d2);
                            }
                        }
                    }
                }
            }
            return result;

        }

        /// <summary>
        /// 减法四舍五入
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="decimals">保留小数位数</param>
        /// <returns></returns>

        public static decimal Subtract(object obj1, object obj2)
        {
            int decimals = AppConst.DecimalPlaces;
            decimal result = 0;
            if (obj1 != null && !string.IsNullOrEmpty(obj1.ToString()))
            {
                decimal d1;
                if (decimal.TryParse(obj1.ToString(), out d1))
                {
                    if (obj2 != null && !string.IsNullOrEmpty(obj2.ToString()))
                    {
                        decimal d2;
                        if (decimal.TryParse(obj2.ToString(), out d2))
                        {
                            if (AppConst.IsRound)
                            {
                                result = Math.Round((d1 - d2), decimals, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                result = CutDecimalWithN(d1 - d2);
                            }
                        }
                    }
                }
            }
            return result;

        }
        /// <summary>
        /// 乘法四舍五入
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="decimals">保留小数位数</param>
        /// <returns></returns>

        public static decimal Multiply(object obj1, object obj2)
        {
            int decimals = AppConst.DecimalPlaces;
            decimal result = 0;
            if (obj1 != null && !string.IsNullOrEmpty(obj1.ToString()))
            {
                decimal d1;
                if (decimal.TryParse(obj1.ToString(), out d1))
                {
                    if (obj2 != null && !string.IsNullOrEmpty(obj2.ToString()))
                    {
                        decimal d2;
                        if (decimal.TryParse(obj2.ToString(), out d2))
                        {
                            if (AppConst.IsRound)
                            {
                                result = Math.Round((d1 * d2), decimals, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                result = CutDecimalWithN(d1 * d2);
                            }
                        }
                    }
                }
            }
            return result;

        }

        /// <summary>
        /// 除法四舍五入
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="decimals">保留小数位数</param>
        /// <returns></returns>

        public static decimal Divide(object obj1, object obj2)
        {
            int decimals = AppConst.DecimalPlaces;
            decimal result = 0;
            if (obj1 != null && !string.IsNullOrEmpty(obj1.ToString()))
            {
                decimal d1;
                if (decimal.TryParse(obj1.ToString(), out d1))
                {
                    if (obj2 != null && !string.IsNullOrEmpty(obj2.ToString()))
                    {
                        decimal d2;
                        if (decimal.TryParse(obj2.ToString(), out d2))
                        {
                            if (d2 != 0)
                            {
                                if (AppConst.IsRound)
                                {
                                    result = Math.Round((d1 / d2), decimals, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    result = CutDecimalWithN(d1 / d2);
                                }
                            }
                        }
                    }
                }
            }
            return result;

        }


        /// <summary>
        /// 折扣四舍五入
        /// </summary>
        /// <param name="obj1">现价</param>
        /// <param name="obj2">原价</param>
        /// <param name="decimals">保留小数位数</param>
        /// <returns></returns>

        public static decimal CalcZhe(object obj1, object obj2)
        {
            int decimals = AppConst.DecimalPlaces;
            decimal result = 0;
            if (obj1 != null && !string.IsNullOrEmpty(obj1.ToString()))
            {
                decimal d1;
                if (decimal.TryParse(obj1.ToString(), out d1))
                {
                    if (obj2 != null && !string.IsNullOrEmpty(obj2.ToString()))
                    {
                        decimal d2;
                        if (decimal.TryParse(obj2.ToString(), out d2))
                        {
                            if (d2 != 0)
                            {
                                if (AppConst.IsRound)
                                {
                                    result = Math.Round((d1 / d2) * 100, decimals, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    result = CutDecimalWithN(d1 / d2);
                                }
                            }
                            else
                            {
                                result = 100;
                            }
                        }
                    }
                }
            }
            return result;

        }

        /// <summary>
        /// 使Decimal类型数据保留N位小数且不进行四舍五入操作
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static decimal CutDecimalWithN(decimal d)
        {
            int n = AppConst.DecimalPlaces;
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
