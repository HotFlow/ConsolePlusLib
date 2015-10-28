using ConsolePlusLib.Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Executor
{
    /// <summary>
    /// 命令执行器
    /// </summary>
    public interface CommandExecutor
    {
        void CommandExecutor(object sender, String command, String[] args);
    }

    /// <summary>
    /// 命令执行器类扫描器
    /// </summary>
    public class ClassDetector
    {
        /// <summary>
        /// 开始扫描
        /// </summary>
        /// <returns>继承CommandExecutor的类</returns>
        public List<Type> begin()
        {
            List<Type> temp = new List<Type>();

            foreach (String file in Main.PluginFiles)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    foreach (Type type in assembly.GetTypes())
                    {
                        foreach (Type t in type.GetInterfaces())
                        {
                            if (t.Equals(typeof(CommandExecutor)))
                            {
                                temp.Add(type);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Out.println(Level.Severe, ex.ToString());
                }
            }


            List<Assembly> Assemblies = new List<Assembly>();

            Assemblies.Add(this.GetType().Assembly);
            Assemblies.Add(Main.Assembly);

            foreach (Assembly assembly in Assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (Type t in type.GetInterfaces())
                    {
                        if (t.Equals(typeof(CommandExecutor)))
                        {
                            temp.Add(type);
                        }
                    }
                }
            }

            return temp;
        }
    }
}
