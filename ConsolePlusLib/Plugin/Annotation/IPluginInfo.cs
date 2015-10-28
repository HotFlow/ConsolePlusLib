using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Plugin.Annotation
{
    /// <summary>
    /// 插件信息
    /// </summary>
    public class IPluginInfo : Attribute
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public String PluginName { get; set; }

        /// <summary>
        /// 插件作者
        /// </summary>
        public String PluginAuthor { get; set; }

        /// <summary>
        /// 插件版本
        /// </summary>
        public String PluginVersion { get; set; }

        /// <summary>
        /// 插件说明
        /// </summary>
        public String PluginAnnotation { get; set; }
    }
}
