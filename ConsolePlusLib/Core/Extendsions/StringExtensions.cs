using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Core.Extendsions
{
    public static class StringExtensions
    {
        public static Boolean equalIgnoreCase(this String s, String value)
        {
            if (s.ToUpper().Equals(value.ToUpper()))
            {
                return true;
            }
            return false;
        }

        public static int ToInt(this String s)
        {
            return Int32.Parse(s);
        }

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
