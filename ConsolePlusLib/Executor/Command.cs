using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Executor
{
    public class Command
    {
        /// <summary>
        /// 命令
        /// </summary>
        private String command { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        private String annotation { get; set; }

        /// <summary>
        /// 构造Command
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="annotation">注释</param>
        public Command(String command,String annotation)
        {
            this.command = command;
            this.annotation = annotation;
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns>命令</returns>
        public String getCommand()
        {
            return this.command;
        }

        /// <summary>
        /// 获取注释
        /// </summary>
        /// <returns>注释</returns>
        public String getAnnotation()
        {
            return this.annotation;
        }
    }
}
