using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS
{
    public static class RuntimeObject
    {
        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public static UserModel CurrentUser = null;
        /// <summary>
        /// 账套名称
        /// </summary>
        public static string SID = string.Empty;
    }
}
