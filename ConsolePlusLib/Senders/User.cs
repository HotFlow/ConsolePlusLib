using ConsolePlusLib.Executor;
using ConsolePlusLib.Utils.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsolePlusLib.Events.UserEvent;

namespace ConsolePlusLib.Senders
{
    public class User
    {
        /// <summary>
        /// 进程ID
        /// </summary>
        private int ProcessID { get; set; }

        /// <summary>
        /// 客户端
        /// </summary>
        private Socket Client { get; set; }

        /// <summary>
        /// 线程
        /// </summary>
        public Thread Thread { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        private IPAddress IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        private int Port { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        private String Name { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        private String Prefix { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        private List<Permission> Permissions { get; set; }

        public User(Socket client)
        {
            this.Client = client;
            this.IP = IPAddress.Parse(client.RemoteEndPoint.ToString().Split(':')[0]);
            this.Port = Int32.Parse(client.RemoteEndPoint.ToString().Split(':')[1]);
            this.Permissions = new List<Permission>();
        }

        /// <summary>
        /// 获取用户进程ID
        /// </summary>
        /// <returns></returns>
        public int getProcessID()
        {
            return this.ProcessID;
        }

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns>返回用户名</returns>
        public string getName()
        {
            return this.Name != null ? this.Name : this.IP.ToString() + ":" + this.Port;
        }

        /// <summary>
        /// 设置用户名(在线用户中拥有此名称则不修改)
        /// </summary>
        public Boolean setName(string name)
        {
            foreach (User user in Console.Server.getOnlineUsers())
            {
                if (user.getName() != null)
                {
                    if (user.getName().equalIgnoreCase(name))
                    {
                        return false;
                    }
                }
            }

            UserNameChangedListener listener = new UserNameChangedListener(this, name, this.getName());
            UserNameChangedArg arguments = listener.callEvent();
            this.Name = name;
            return true;
        }

        /// <summary>
        /// 获取称号
        /// </summary>
        /// <returns></returns>
        public string getPrefix()
        {
            return this.Prefix;
        }

        /// <summary>
        /// 设置称号
        /// </summary>
        /// <param name="prefix"></param>
        public void setPrefix(String prefix)
        {
            UserPrefixChangedListener listener = new UserPrefixChangedListener(this, prefix, this.getPrefix());
            UserPrefixChangedArg arguments = listener.callEvent();
            this.Prefix = arguments.getNewPrefix();
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns>返回IP</returns>
        public IPAddress getIP()
        {
            return this.IP;
        }

        /// <summary>
        /// 获取端口
        /// </summary>
        /// <returns>返回端口</returns>
        public int getPort()
        {
            return this.Port;
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns>返回客户端</returns>
        public Socket getClient()
        {
            return this.Client;
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        public void sendMessage(string message, Boolean silence = false)
        {
            MessageSender sender = new MessageSender(this);
            sender.send(message, silence);
        }

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

                UserCommandReciver reciver = new UserCommandReciver(this, command, args);
                reciver.callEvent();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 踢出用户
        /// </summary>
        /// <param name="message"></param>
        public void kick(String message = null)
        {
            if (message != null)
            {
                this.sendMessage(message);
            }

            this.getClient().Shutdown(SocketShutdown.Both);
            this.getClient().Dispose();
        }

        /// <summary>
        /// 获取用户所有的权限
        /// </summary>
        /// <returns></returns>
        public List<Permission> getPermissions()
        {
            return this.Permissions;
        }

        /// <summary>
        /// 用户是否拥有权限
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public Boolean hasPermission(Permission permission)
        {
            foreach (Permission p in this.Permissions)
            {
                if (p.getName().equalIgnoreCase(permission.getName()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 为用户添加权限(权限已存在则不添加)
        /// </summary>
        /// <param name="permission">是否成功添加</param>
        /// <returns></returns>
        public Boolean addPermission(Permission permission)
        {
            foreach (Permission p in this.Permissions)
            {
                if (p.getName().equalIgnoreCase(permission.getName()))
                {
                    return false;
                }
            }

            this.Permissions.Add(permission);
            return true;
        }
    }
}

