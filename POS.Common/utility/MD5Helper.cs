using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace POS.Common.utility
{
   public class MD5Helper
    {
        /// <summary>
        /// 获得MD5码
        /// </summary>
        /// <param name="input">需要的字符串</param>
        /// <returns>MD5码</returns>
        public static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                //暂时使用没有转换为字符串的加密串
               sBuilder.Append(data[i].ToString("x2"));
               // sBuilder.Append(data[i].ToString());
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
