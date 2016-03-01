using ConsolePlusLib.Utils.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsolePlusLib.Utils.Extendsions;
using ConsolePlusLib.Core.PluginEngines;
using ConsolePlusLib.Plugin;
using ConsolePlusLib.Executor;
using ConsolePlusLib.Senders;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ConsolePlusLib.Events.UserEvent;
using ConsolePlusLib.Core;

namespace ConsolePlusLib.Core
{

    /// <summary>
    /// 服务器
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 插件
        /// </summary>
        internal static List<PluginInfo> Plugins = new List<PluginInfo>();

        /// <summary>
        /// 插件管理器
        /// </summary>
        public PluginManager PluginManager = new PluginManager();

        /// <summary>
        /// 用户加入线程
        /// </summary>
        internal static Thread UserJoinThread { get; set; }

        /// <summary>
        /// 用户加入线程开关控制
        /// </summary>
        internal static Boolean UserJoinFlag { get; set; }

        /// <summary>
        /// 用户退出线程
        /// </summary>
        internal static Thread UserQuitThread { get; set; }

        /// <summary>
        /// 用户退出线程开关控制
        /// </summary>
        internal static Boolean UserQuitFlag { get; set; }

        private List<User> onlineUsers { get; set; }
        private SocketHandler handler { get; set; }
        private List<Permission> permission { get; set; }

        public Server(String ip, int port)
        {
            this.onlineUsers = new List<User>();
            this.permission = new List<Permission>();
            this.handler = new SocketHandler(IPAddress.Parse(ip), port);

            if (!this.handler.isConnected())
            {
                return;
            }
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public List<Permission> getPermissions()
        {
            return this.permission;
        }

        /// <summary>
        /// 获取处理器
        /// </summary>
        /// <returns></returns>
        public SocketHandler getHandler()
        {
            return this.handler;
        }

        /// <summary>
        /// 获取所有在线用户
        /// </summary>
        /// <returns></returns>
        public List<User> getOnlineUsers()
        {
            return this.onlineUsers;
        }

        /// <summary>
        /// 获取指定在线用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public User getOnlineUser(String name)
        {
            foreach (User user in this.onlineUsers)
            {
                if (user.getName().equalIgnoreCase(name))
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定在线用户
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public User getOnlineUser(IPAddress ip, int port)
        {
            foreach (User user in this.onlineUsers)
            {
                if (user.getIP().Equals(ip) && user.getPort().Equals(port))
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns></returns>
        public List<PluginInfo> getPlugins()
        {
            return Server.Plugins;
        }

        /// <summary>
        /// 获取所有命令
        /// </summary>
        /// <returns>所有命令<String></returns>
        public Dictionary<Command, ConsolePlugin> getCommands()
        {
            return Console.Commands;
        }

        /// <summary>
        /// 获取命令列表
        /// </summary>
        /// <returns></returns>
        public List<Command> getCommandList()
        {
            return Console.Commands.Keys.ToList();
        }

        /// <summary>
        /// 添加服务端命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="annotation">说明</param>
        /// <returns>是否成功</returns>
        public Boolean addCommand(ConsolePlugin plugin, Command command)
        {
            try
            {
                Console.Commands.Add(command, plugin);
                return true;
            }
            catch (ArgumentException)
            {
                Console.Out.println(Level.Severe, "命令 [" + command + "] 已存在!");
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
            foreach (Command c in this.getCommands().Keys)
            {
                if (c.getCommand().equalIgnoreCase(command))
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
                foreach (Command c in Console.Commands.Keys)
                {
                    if (c.getCommand().equalIgnoreCase(command))
                    {
                        Console.Commands.Remove(c);
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
                foreach (Command c in Console.Commands.Keys)
                {
                    if (c.Equals(command))
                    {
                        Console.Commands.Remove(c);
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
        /// <summary>
        /// 监听IP
        /// </summary>
        private IPAddress IP { get; set; }

        /// <summary>
        /// 监听端口
        /// </summary>
        private Int32 Port { get; set; }

        /// <summary>
        /// 服务器
        /// </summary>
        private Socket Server { get; set; }

        /// <summary>
        /// 是否连接
        /// </summary>
        private Boolean Connected { get; set; }

        /// <summary>
        /// 创建一个服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public SocketHandler(IPAddress ip, Int32 port)
        {
            this.Connected = false;
            this.IP = ip;
            this.Port = port;

            Console.Out.println("正在尝试启动服务器地址(IP:" + this.IP.ToString() + ",Port:" + this.Port.ToString() + ")...");

            IPEndPoint iep = null;

            //检查地址是否空
            try
            {
                iep = new IPEndPoint(this.IP, this.Port);
            }
            catch (Exception ex)
            {
                ConsolePlusLib.Console.Out.println(Level.Severe, "尚未提供监听地址!");
                ConsolePlusLib.Console.Out.println(Level.Error, ex.ToString());
                return;
            }

            this.Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            //服务器监听
            try
            {
                ConsolePlusLib.Console.Out.println("服务器正在监听...");
                this.Server.Bind(iep);
                this.Server.Listen(10);
            }
            catch (SocketException ex)
            {
                ConsolePlusLib.Console.Out.println(Level.Warning, "这个地址不能作为监听地址!");
                ConsolePlusLib.Console.Out.println(Level.Error, ex.ToString());
                return;
            }

            this.Connected = true;
        }


        /// <summary>
        /// 开始监听
        /// </summary>
        public void start()
        {
            try
            {
                UserJoinListener uj = new UserJoinListener(this.Server);
                ConsolePlusLib.Core.Server.UserJoinFlag = true;
                ConsolePlusLib.Core.Server.UserJoinThread = new Thread(uj.begin);
                ConsolePlusLib.Core.Server.UserJoinThread.Start();

                UserQuitListener uq = new UserQuitListener(this.Server);
                ConsolePlusLib.Core.Server.UserQuitFlag = true;
                ConsolePlusLib.Core.Server.UserQuitThread = new Thread(uq.begin);
                ConsolePlusLib.Core.Server.UserQuitThread.Start();

                ConsolePlusLib.Console.Out.println("服务器已启动!");
            }
            catch (Exception ex)
            {
                ConsolePlusLib.Console.Out.println(Level.Warning, "服务器监听失败!");
                ConsolePlusLib.Console.Out.println(Level.Error, ex.ToString());
            }
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void stop()
        {
            ConsolePlusLib.Console.Out.println("服务器监听正在关闭...");
            try
            {
                for (int i = 0; i < ConsolePlusLib.Console.Server.getOnlineUsers().Count; i++)
                {
                    ConsolePlusLib.Console.Server.getOnlineUsers()[i].kick("服务器关闭");
                }

                ConsolePlusLib.Core.Server.UserJoinFlag = false;
                ConsolePlusLib.Core.Server.UserQuitFlag = false;

                ConsolePlusLib.Console.delay(500);

                this.Server.Close(10);
                this.Server.Dispose();

                ConsolePlusLib.Console.Out.println("服务器已关闭.");
            }
            catch (Exception ex)
            {
                ConsolePlusLib.Console.Out.println(Level.Warning, "服务器关闭失败!");
                ConsolePlusLib.Console.Out.println(Level.Error, ex.ToString());
            }
        }

        /// <summary>
        /// 开始监听指定用户的信息
        /// </summary>
        public void reciveMessageBegin(User user)
        {
            MessageReciver reciver = new MessageReciver(user);
            user.Thread = new Thread(reciver.begin);
            user.Thread.Start();
        }

        /// <summary>
        /// 停止监听指定用户的信息
        /// </summary>
        /// <param name="user"></param>
        public void reciveMessageEnd(User user)
        {
            user.Thread.Abort();
        }

        /// <summary>
        /// 发送信息到用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public void sendMessage(User user, String message)
        {
            MessageSender sender = new MessageSender(user);
            sender.send(message);
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        /// <returns></returns>
        public Boolean isConnected()
        {
            return this.Connected;
        }
    }
}
