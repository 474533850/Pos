using POS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace POS.Common.utility
{
    /// <summary>
    /// xml配置文件帮助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 加载本地配置文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ConfigModel LoadConfig(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ConfigModel));
            StreamReader sr = new StreamReader(file);
            ConfigModel config = xs.Deserialize(sr) as ConfigModel;
            sr.Close();

            return config;
        }

        /// <summary>
        /// 保存本地配置我文件
        /// </summary>
        /// <param name="file"></param>
        public static void SaveConfig(string file, ConfigModel config)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ConfigModel));
            StreamWriter sw = new StreamWriter(file);
            xs.Serialize(sw, config);
            sw.Close();
        }

        /// <summary>
        /// 保存本地配置我文件
        /// </summary>
        /// <param name="file"></param>
        public static void SaveConfig<T>(string file, T config)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StreamWriter sw = new StreamWriter(file);
            xs.Serialize(sw, config);
            sw.Close();
        }

        /// <summary>
        /// 加载本地配置文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<T> LoadConfig<T>(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<T>));
            StreamReader sr = new StreamReader(file);
            List<T> config = xs.Deserialize(sr) as List<T>;
            sr.Close();
            return config;
        }


    }
}
