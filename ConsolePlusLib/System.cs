using ConsolePlusLib.Core.Output;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsolePlusLib
{
    /// <summary>
    /// 系统应用类
    /// </summary>
    public class System
    {
        /// <summary>
        /// 字体输出
        /// </summary>
        public static Printer Out { get; set; }

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
                    ConsolePlusLib.System.Out.println(output.ToString());
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

    }
}
