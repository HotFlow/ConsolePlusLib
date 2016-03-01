using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Utils.Extendsions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 无视大小写等于
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="value">值</param>
        /// <returns>是否等于</returns>
        public static Boolean equalIgnoreCase(this String s, String value)
        {
            if (s.ToUpper().Equals(value.ToUpper()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 转整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>整数</returns>
        public static int ToInt(this String s)
        {
            return Int32.Parse(s);
        }

        /// <summary>
        /// 是否为整数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>是否为整数</returns>
        public static Boolean isInt(this String s)
        {
            try
            {
                int i = Int32.Parse(s);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// 是否为IPAddress
        /// </summary>
        /// <param name="ip">字符串</param>
        /// <returns>是否为IPAddress</returns>
        public static Boolean isIPAddress(this String ip)
        {
            try
            {
                IPAddress address = IPAddress.Parse(ip);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static String replaceInfo(this String s, String username, String password, String displayName, String email)
        {
            return s.Replace("%username%", username).Replace("%password%", password).Replace("%displayname%", displayName).Replace("%email%", email);
        }
    }
}
