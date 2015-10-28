using ConsolePlusLib.Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsolePlusLib.Core.Extendsions;
using ConsolePlusLib.Core.PluginEngines;
using ConsolePlusLib.Plugin;
using ConsolePlusLib.Executor;

namespace ConsolePlusLib.Core
{
    /// <summary>
    /// 服务器
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns></returns>
        public List<PluginInfo> getPlugins()
        {
            return Main.Plugins;
        }

        /// <summary>
        /// 获取所有命令
        /// </summary>
        /// <returns>所有命令<String></returns>
        public Dictionary<Command, ConsolePlugin> getCommands()
        {
            return Main.Commands;
        }

        /// <summary>
        /// 添加服务端命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="annotation">说明</param>
        /// <returns>是否成功</returns>
        public Boolean addCommand(ConsolePlugin plugin,Command command)
        {
            try
            {
                Main.Commands.Add(command, plugin);
                return true;
            }
            catch (ArgumentException)
            {
                System.Out.println(Level.Severe, "命令 [" + command + "] 已存在!");
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
            foreach(Command c in this.getCommands().Keys)
            {
                if(c.getCommand().equalIgnoreCase(command))
                {
                    return c.getAnnotation();
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
                foreach(Command c in Main.Commands.Keys)
                {
                    if(c.getCommand().equalIgnoreCase(command))
                    {
                        Main.Commands.Remove(c);
                    }
                }
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        /// <summary>
        /// 移除服务端命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>是否存在</returns>
        public Boolean removeCommand(Command command)
        {
            try
            {
                foreach (Command c in Main.Commands.Keys)
                {
                    if (c.Equals(command))
                    {
                        Main.Commands.Remove(c);
                    }
                }
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
            foreach (Command command in this.getCommands().Keys)
            {
                this.removeCommand(command.getCommand());
            }
        }
    }

    public class SocketHandler
    {

    }
}
