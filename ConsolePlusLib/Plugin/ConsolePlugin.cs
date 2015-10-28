using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Plugin
{
    /// <summary>
    /// 插件接口
    /// 需要额外提供[IPluginInfo(PluginName = "插件名称", PluginAuthor = "插件作者", PluginVersion = "插件版本", PluginAnnotation = "插件说明")]
    /// </summary>
    public interface ConsolePlugin
    {
        /// <summary>
        /// 插件载入
        /// </summary>
        void onEnable();

        /// <summary>
        /// 插件卸载
        /// </summary>
        void onDisable();
    }
}
