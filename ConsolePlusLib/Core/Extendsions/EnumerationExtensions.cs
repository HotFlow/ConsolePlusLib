using ConsolePlusLib.Core.Output;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Core.Extendsions
{
    public static class EnumerationExtensions
    {

        /// <summary>
        /// 获取注释
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="en">枚举</param>
        /// <param name="type">枚举</param>
        /// <returns>注释</returns>
        public static String getDescription<T>(this T t)
        {
            try
            {
                foreach (T e in Enum.GetValues(typeof(T)))
                {
                    if (e.Equals(t))
                    {
                        FieldInfo EnumInfo = e.GetType().GetField(e.ToString());
                        DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])EnumInfo.
                            GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (EnumAttributes.Length > 0)
                        {
                            return EnumAttributes[0].Description;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Out.println(Level.Error, Color.ForestGreen, ex.ToString());
                return null;
            }
        }
    }
}
