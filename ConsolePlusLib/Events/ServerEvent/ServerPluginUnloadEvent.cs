using ConsolePlusLib.Core.PluginEngines;
using ConsolePlusLib.Plugin;
using ConsolePlusLib.Plugin.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Events.ServerEvent
{
    /// <summary>
    /// 服务器插件卸载事件参数
    /// </summary>
    public class ServerPluginUnloadArg : EventArgs
    {
        private PluginInfo Info { get; set; }
        private Boolean Canceled { get; set; }

        /// <summary>
        /// 插件卸载信息
        /// </summary>
        /// <param name="info">插件信息</param>
        public ServerPluginUnloadArg(PluginInfo info)
        {
            this.Info = info;
        }

        /// <summary>
        /// 获取插件资料
        /// </summary>
        /// <returns>插件信息</returns>
        public PluginInfo getPluginInfo()
        {
            return this.Info;
        }

        /// <summary>
        /// 设置事件是否取消
        /// </summary>
        /// <param name="bln">是否取消</param>
        public void setCanceled(Boolean bln)
        {
            this.Canceled = bln;
        }

        /// <summary>
        /// 获取事件是否取消
        /// </summary>
        /// <returns>是否取消</returns>
        public Boolean isCanceled()
        {
            return this.Canceled;
        }

    }

    /// <summary>
    /// 接口
    /// </summary>
    /// <param name="sender">发送者</param>
    /// <param name="e">事件参数</param>
    public delegate void ServerPluginUnloadHandler(object sender, ServerPluginUnloadArg e);

    /// <summary>
    /// 服务器插件卸载器
    /// </summary>
    public class ServerPluginUnistaller
    {
        /// <summary>
        /// 插件信息
        /// </summary>
        public PluginInfo Info { get; private set; }

        /// <summary>
        /// 服务器插件卸载事件
        /// </summary>
        public static event ServerPluginUnloadHandler PluginUnloadEvent;

        /// <summary>
        /// 插件
        /// </summary>
        private String Plugin { get; set; }

        /// <summary>
        /// 程序集
        /// </summary>
        private Assembly Assembly { get; set; }

        /// <summary>
        /// 主类
        /// </summary>
        private Type Type { get; set; }

        private Object Instance { get; set; }

        /// <summary>
        /// 载入插件
        /// </summary>
        /// <param name="file">文件路径</param>
        public ServerPluginUnistaller(String file)
        {
            this.Plugin = file;
            this.Info = new PluginInfo(null, null, null, null, file);
            this.Assembly = Assembly.LoadFrom(this.Plugin);

            setType();

            if (this.Type == null)
            {
                return;
            }

            this.Instance = Activator.CreateInstance(this.Type);
            setInfo();
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public void unload()
        {
            try
            {
                MethodInfo onDisable = this.Type.GetMethod("onDisable");

                ServerPluginUnloadArg arguments = new ServerPluginUnloadArg(this.Info);

                //注册Event
                if (PluginUnloadEvent != null)
                {
                    PluginUnloadEvent(this, arguments);

                    if (arguments.isCanceled())
                    {
                        return;
                    }
                }

                //执行插件
                onDisable.Invoke(this.Instance, null);

            }
            catch (Exception ex)
            {
                Console.Out.println(ex.ToString());
            }

        }


        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <returns>插件信息</returns>
        public PluginInfo getPluginInfo()
        {
            return this.Info;
        }


        /// <summary>
        /// 设置主类
        /// </summary>
        /// <returns></returns>
        private void setType()
        {
            foreach (Type type in this.Assembly.GetTypes())
            {
                if (type.IsDefined(typeof(IPluginInfo), false))
                {
                    foreach (Type interFace in type.GetInterfaces())
                    {
                        if (interFace.Equals(typeof(ConsolePlugin)))
                        {
                            this.Type = type;
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        private void setInfo()
        {
            if (this.Type.IsDefined(typeof(IPluginInfo), false))
            {
                IPluginInfo info = (IPluginInfo)this.Type.GetCustomAttributes(typeof(IPluginInfo), false)[0];
                this.Info.PluginName = info.PluginName;
                this.Info.PluginAuthor = info.PluginAuthor;
                this.Info.PluginVersion = info.PluginVersion;
                this.Info.PluginAnnotation = info.PluginAnnotation;
            }
        }

    }
}
