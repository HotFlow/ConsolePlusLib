using ConsolePlusLib.Core;
using ConsolePlusLib.Core.PluginEngines;
using ConsolePlusLib.Executor;
using ConsolePlusLib.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsolePlusLib.Configuration;
using System.IO;
using ConsolePlusLib.Utils.Output;
using System.Threading;
using System.Diagnostics;
using ConsolePlusLib.Utils;
using ConsolePlusLib.Core;

namespace ConsolePlusLib
{
    /// <summary>
    /// 主类
    /// </summary>
    public class Console
    {
        /// <summary>
        /// 程序集
        /// </summary>
        internal static Assembly Assembly { get; set; }

        /// <summary>
        /// 服务器
        /// </summary>
        internal static Server Server { get; set; }

        /// <summary>
        /// 插件文件名字
        /// </summary>
        internal static List<String> PluginFiles { get; set; }

        /// <summary>
        /// 所有指令
        /// </summary>
        internal static Dictionary<Command, ConsolePlugin> Commands = new Dictionary<Command, ConsolePlugin>();

        /// <summary>
        /// 配置
        /// </summary>
        public static JsonConfiguration Config { get; set; }

        /// <summary>
        /// 配置文件
        /// </summary>
        public static readonly String ConfigFile = "config.json";

        /// <summary>
        /// 字体输出
        /// </summary>
        public static Printer Out { get; set; }

        /// <summary>
        /// 枚举工具
        /// </summary>
        public static Enumeration Enum { get; set; }

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="outputBox">输出窗口</param>
        public static void Begin(Assembly assembly, RichTextBox outputBox, String ip, Int32 port)
        {
            Console.Assembly = assembly;
            Console.Out = new Printer(outputBox);
            Console.Server = new Server(ip, port);
            Console.Config = new JsonConfiguration(ConfigFile);

            if (!File.Exists(Console.ConfigFile))
            {
                Console.Config.create();
                Console.Config.load();
            }
            else
            {
                Console.Config.load();
            }
        }

        /// <summary> 
        /// 延时 
        /// </summary> 
        /// <param name="time">毫秒</param> 
        public static void delay(long time)
        {
            Thread t = new Thread(o => Thread.Sleep(unchecked((int)time)));
            t.Start();
            while (t.IsAlive)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 执行一段控制台命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>Process</returns>
        public static Process process(String command)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.Verb = "runas";
            info.Arguments = "/c" + command;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            Process process = new Process();

            process.StartInfo = info;
            process.EnableRaisingEvents = true;

            StringBuilder output = new StringBuilder();

            process.OutputDataReceived += new DataReceivedEventHandler((sender1, e1) =>
            {
                if (!String.IsNullOrEmpty(e1.Data))
                {
                    output.Append(Environment.NewLine + e1.Data);
                }
            });

            process.ErrorDataReceived += new DataReceivedEventHandler((sender1, e1) =>
            {
                if (!String.IsNullOrEmpty(e1.Data))
                {
                    output.Append(Environment.NewLine + e1.Data);
                }
            });

            process.Exited += new EventHandler((sender1, e1) =>
            {
                if (!String.IsNullOrEmpty(output.ToString()))
                {
                    ConsolePlusLib.Console.Out.println(output.ToString());
                }

                process.Close();
            });

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process;
        }

        /// <summary>
        /// 获取当前进程
        /// </summary>
        /// <returns>Process</returns>
        public static Process getCurrentProcess()
        {
            return Process.GetCurrentProcess();
        }

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <returns>服务器</returns>
        public static Server getServer()
        {
            return Console.Server;
        }
    }
}
