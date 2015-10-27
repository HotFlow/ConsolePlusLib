using ConsolePlusLib.Core.Extendsions;
using ConsolePlusLib.Core.Output;
using ConsolePlusLib.Executor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Events.Server
{
    /// <summary>
    /// 服务器执行命令事件
    /// </summary>
    public class ServerExecutedCommandArg : EventArgs
    {
        private String Command { get; set; }
        private String[] Args { get; set; }

        /// <summary>
        /// 命令信息
        /// </summary>
        /// <param name="senderType"></param>
        /// <param name="command"></param>
        /// <param name="args"></param>
        public ServerExecutedCommandArg(String command, String[] args)
        {
            this.Command = command;
            this.Args = args;
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        public String getCommand()
        {
            return this.Command;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public String[] getArgs()
        {
            return this.Args;
        }
    }

    /// <summary>
    /// 接口
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServerExecutedCommandHandler(Console sender, ServerExecutedCommandArg e);

    public class ServerCommandReciver
    {
        /// <summary>
        /// 事件
        /// </summary>
        public static event ServerExecutedCommandHandler ServerExecutedCommandEvent;

        /// <summary>
        /// 发送者
        /// </summary>
        private Console Sender { get; set; }

        /// <summary>
        /// 命令
        /// </summary>
        private String Command { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        private String[] Args { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public ServerCommandReciver(Console sender, String command, String[] args)
        {
            this.Sender = sender;
            this.Command = command;
            this.Args = args;
        }

        /// <summary>
        /// 呼出事件
        /// </summary>
        public void callEvent()
        {
            ServerExecutedCommandArg arguments = new ServerExecutedCommandArg(this.Command, this.Args);

            int size = 0;
            foreach (String command in System.Server.getCommandList())
            {
                if (command.equalIgnoreCase(this.Command))
                {
                    System.Out.println("ServerConsole issued server command: /" + this.Command);

                    if (ServerExecutedCommandEvent != null)
                    {
                        ServerExecutedCommandEvent(this.Sender, arguments);
                    }

                    ClassDetector detector = new ClassDetector();
                    foreach (Type type in detector.begin())
                    {
                        try
                        {
                            if (type == null)
                            {
                                continue;
                            }

                            Object instance = Activator.CreateInstance(type);

                            MethodInfo executor = type.GetMethod("CommandExecutor");

                            Object[] parameter = new Object[3];
                            parameter[0] = this.Sender;
                            parameter[1] = this.Command;
                            parameter[2] = this.Args;

                            executor.Invoke(instance, parameter);
                            size++;
                        }
                        catch (Exception ex)
                        {
                            System.Out.println(Level.Severe, ex.ToString());
                        }
                    }

                }
            }

            if (size <= 0)
            {
                System.Out.println("Unknown command. Type \"/help\" for help.");
            }
        }
    }
}
