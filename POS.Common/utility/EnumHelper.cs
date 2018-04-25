using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace POS.Common.utility
{
   public class EnumHelper
    {
        ///<summary>
        /// 返回 Dic<枚举项，描述>
        ///</summary>
        ///<param name="enumType"></param>
        ///<returns>Dic<枚举项，描述></returns>
        public static Dictionary<string, string> GetEnumDictionary(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FieldInfo[] fieldinfos = enumType.GetFields();
            foreach (FieldInfo field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    dic.Add(field.Name, ((DescriptionAttribute)objs[0]).Description);
                }
            }
            return dic;
        }
    }
}
