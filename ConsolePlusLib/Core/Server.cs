using ConsolePlusLib.Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsolePlusLib.Core.Extendsions;

namespace ConsolePlusLib.Core
{
    /// <summary>
    /// 服务器类
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 获取所有命令
        /// </summary>
        /// <returns>所有命令<String></returns>
        public List<String> getCommandList()
        {
            List<String> temp = new List<String>();
            foreach (String command in Main.Commands.Keys)
            {
                temp.Add(command);
            }
            return temp;
        }

        /// <summary>
        /// 添加服务端命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="annotation">说明</param>
        /// <returns>是否成功</returns>
        public Boolean addCommand(String command, String annotation)
        {
            try
            {
                Main.Commands.Add(command, annotation);
                return true;
            }
            catch (ArgumentException)
            {
                System.Out.println(Level.Severe, "命令 [" + command + "] 已存在!");
                return false;
            }
        }

        /// <summary>
        /// 更改服务端命令的说明
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="annotation">说明</param>
        /// <returns>是否成功</returns>
        public Boolean setCommandAnnotation(String command, String annotation)
        {
            try
            {
                Main.Commands[command] = annotation;
                return true;
            }
            catch (ArgumentNullException)
            {
                System.Out.println(Level.Severe, "命令 [" + command + "] 不存在!");
                return false;
            }
        }

        /// <summary>
        /// 获取命令注释
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>注释</returns>
        public String getCommandAnnotation(String command)
        {
            foreach(String c in this.getCommandList())
            {
                if(c.equalIgnoreCase(command))
                {
                    return Main.Commands[command];
                }
            }

            return null;
        }

        /// <summary>
        /// 移除服务端命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>是否存在</returns>
        public Boolean removeCommand(String command)
        {
            try
            {
                Main.Commands.Remove(command);
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        /// <summary>
        /// 移除所有服务端命令
        /// </summary>
        public void removeAllCommands()
        {
            foreach (String command in this.getCommandList())
            {
                this.removeCommand(command);
            }
        }
    }

    public class SocketHandler
    {

    }
}
