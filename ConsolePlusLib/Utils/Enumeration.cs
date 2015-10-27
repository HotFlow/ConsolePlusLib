using ConsolePlusLib.Core.Output;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Utils
{
    public class Enumeration
    {
        public String getDescription<T>(T type)
        {
            try
            {
                foreach (T e in Enum.GetValues(typeof(T)))
                {
                    if (e.Equals(type))
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
