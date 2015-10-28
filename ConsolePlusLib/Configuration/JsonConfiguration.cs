using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ConsolePlusLib.Configuration
{
    /// <summary>
    /// Json配置
    /// </summary>
    public partial class JsonConfiguration
    {
        private String file { set; get; }
        private Dictionary<String, Object> dictionary { set; get; }
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        /// <summary>
        /// 构造新Json配置
        /// </summary>
        /// <param name="file"></param>
        public JsonConfiguration(String file)
        {
            this.file = file;
        }

        /// <summary>
        /// 新建
        /// </summary>
        public void create()
        {
            FileStream fs = new FileInfo(file).Create();
            fs.Close();
            StreamWriter sw = new StreamWriter(file);
            sw.Write("{");
            sw.Write("}");
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// 载入
        /// </summary>
        public void load()
        {
            this.loadToDictionary();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void save(String file)
        {
            this.saveFromDictionary(file);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public String getValue(String path)
        {
            return getDictionaryValue(this.dictionary, path);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public String getString(String path)
        {
            return getDictionaryValue(this.dictionary, path);
        }

        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public Int32 getInteger(String path)
        {
            try
            {
                return Int32.Parse(getDictionaryValue(this.dictionary, path));
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public Boolean getBoolean(String path)
        {
            switch (getDictionaryValue(this.dictionary, path).ToLower())
            {
                case "true":
                    return true;
                case "false":
                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public IPAddress getIPAddress(String path)
        {
            try
            {
                return IPAddress.Parse(getDictionaryValue(this.dictionary, path));
            }
            catch (FormatException)
            {
                return null;
            }
        }


        /// <summary>
        /// 获取单精小数
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public float getFloat(String path)
        {
            try
            {
                return float.Parse(getDictionaryValue(this.dictionary, path));
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取小数
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public Double getDouble(String path)
        {
            try
            {
                return Double.Parse(getDictionaryValue(this.dictionary, path));
            }
            catch (FormatException)
            {
                return 0;
            }
        }


        /// <summary>
        /// 获取长精小数
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public long getLong(String path)
        {
            try
            {
                return long.Parse(getDictionaryValue(this.dictionary, path));
            }
            catch (FormatException)
            {
                return 0;
            }
        }


        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值<String, Object></returns>
        public Dictionary<String, Object> getMap(String path)
        {
            return getDictionaryMap(this.dictionary, path);
        }

        /// <summary>
        /// 获取字符串列表
        /// </summary>
        /// <param name="path">键路径</param>
        /// <returns>值</returns>
        public ArrayList getList(String path)
        {
            return getDictionaryList(this.dictionary, path);
        }


        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="path">键路径</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public Boolean set(String path, String value)
        {
            return setDictionaryValue(this.dictionary, path, value);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="path">键路径</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public Boolean set(String path, Boolean value)
        {
            return setDictionaryValue(this.dictionary, path, value);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="path">键路径</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public Boolean set(String path, ArrayList value)
        {
            return setDictionaryValue(this.dictionary, path, value);
        }


        /// <summary>
        /// 该项是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        public Boolean hasConfigurationSection(String path)
        {
            try
            {
                if (getMap(path).Keys.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
