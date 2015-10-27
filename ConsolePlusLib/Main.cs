using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsolePlusLib
{
    /// <summary>
    /// 主类
    /// </summary>
    public class Main
    {
        /// <summary>
        /// 程序集
        /// </summary>
        internal static Assembly Assembly { get; set; }

        /// <summary>
        /// 插件文件名字
        /// </summary>
        internal static List<String> PluginFiles { get; set; }


        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="outputBox">输出窗口</param>
        public static void Begin(Assembly assembly,RichTextBox outputBox)
        {
            Main.Assembly = assembly;
            Main.PluginFiles = new List<String>();
            System.Out = new Core.Output.Printer(outputBox);
        }
    }
}
