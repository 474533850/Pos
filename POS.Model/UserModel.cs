using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserModel : BaseModel
    {
        /// <summary>
        /// 用户代码
        /// </summary>
        public string usercode { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 登录密码明文
        /// </summary>
        public string passwordExpress { get; set; }
        /// <summary>
        /// 仓库代码
        /// </summary>
        public string cnkucode { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string cnkuname { get; set; }
        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 当班单号
        /// </summary>
        public string posnono { get; set; }
        /// <summary>
        /// 账套
        /// </summary>
        public string bookID { get; set; }
    }
}
