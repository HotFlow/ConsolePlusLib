using ConsolePlusLib.Events.Server;
using ConsolePlusLib.Executor;
using ConsolePlusLib.Core.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Senders
{
    public class Console
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>成功返回true</returns>
        public Boolean dispatch(string cmd)
        {
            if (cmd.StartsWith("/"))
            {
                String s = cmd.Split(' ')[0];
                String command = s.Substring(1, s.Length - 1);
                String[] args = new String[cmd.Split(' ').Length - 1];

                for (int i = 0; i < cmd.Split(' ').Length; i++)
                {
                    if (i != 0)
                    {
                        args[i - 1] = cmd.Split(' ')[i];
                    }
                }

                Command cmd1 = null;
                
                foreach(Command c in Main.getServer().getCommands().Keys)
                {
                    if(c.getCommand().equalIgnoreCase(command))
                    {
                        cmd1 = c;
                    }
                }

                ServerCommandReciver reciver = new ServerCommandReciver(new Console(), cmd1, args);
                reciver.callEvent();
                return true;
            }
            return false;
        }
    }
}
