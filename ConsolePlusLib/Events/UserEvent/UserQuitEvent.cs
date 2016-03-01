using ConsolePlusLib.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ConsolePlusLib.Events.UserEvent
{
    /// <summary>
    /// 用户退出事件
    /// </summary>
    public class UserQuitArg : EventArgs
    {
        private User User { get; set; }

        public UserQuitArg(ConsolePlusLib.Senders.User user)
        {
            this.User = user;
        }

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <returns></returns>
        public User getUser()
        {
            return this.User;
        }

    }

    /// <summary>
    /// 接口
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void UserQuitHandler(object sender, UserQuitArg e);

    public class UserQuitListener
    {
        /// <summary>
        /// 服务端
        /// </summary>
        private Socket Server { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// 事件
        /// </summary>
        public static event UserQuitHandler UserQuitEvent;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="server"></param>
        public UserQuitListener(Socket server)
        {
            this.Server = server;
        }

        /// <summary>
        /// 监听
        /// </summary>
        public void begin()
        {
            while (true)
            {
                if(!ConsolePlusLib.Core.Server.UserQuitFlag)
                {
                    return;
                }

                try
                {
                    for (int i = 0; i < ConsolePlusLib.Console.Server.getOnlineUsers().Count; i++)
                    {
                        ConsolePlusLib.Senders.User user = ConsolePlusLib.Console.Server.getOnlineUsers()[i];

                        if (!this.isConnecting(user.getClient()))
                        {
                            UserQuitArg arguments = new UserQuitArg(user);

                            if (UserQuitEvent != null)
                            {
                                UserQuitEvent(this, arguments);
                            }

                            ConsolePlusLib.Console.Server.getHandler().reciveMessageEnd(user);
                            ConsolePlusLib.Console.Out.println((user.getPrefix() == null ? "" : user.getPrefix()) + "<" + (user.getName() == null ? user.getIP() + ":" + user.getPort() : user.getName()) + ">" + "下线了!");
                            ConsolePlusLib.Console.Server.getOnlineUsers().Remove(user);
                        }
                    }
                }
                catch(Exception)
                {

                }
            }
        }

        private bool isConnecting(Socket client)
        {
            if (client != null && client.Connected)
            {
                if (!client.Poll(1, SelectMode.SelectRead))
                {
                    return true;
                }
            }

            return false;
        }


        private bool isDisconnecting(Socket client)
        {
            bool blockingState = client.Blocking;
            try
            {
                byte[] tmp = new byte[1];
                client.Blocking = false;
                client.Send(tmp, 0, 0);
                return false;
            }
            catch (SocketException e)
            {
                if (e.NativeErrorCode.Equals(10035))
                    return false;
                else
                    return true;
            }
            finally
            {
                client.Blocking = blockingState;
            }
        }
    }
}
