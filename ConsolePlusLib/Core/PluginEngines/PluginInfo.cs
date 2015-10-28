using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Core.PluginEngines
{
    /// <summary>
    /// 插件信息
    /// </summary>
    public class PluginInfo
    {
        /// <summary>
        /// 插件名
        /// </summary>
        public String PluginName { get; set; }

        /// <summary>
        /// 插件版本
        /// </summary>
        public String PluginVersion { get; set; }

        /// <summary>
        /// 插件作者
        /// </summary>
        public String PluginAuthor { get; set; }

        /// <summary>
        /// 插件注释
        /// </summary>
        public String PluginAnnotation { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public String FilePath { get; set; }

        /// <summary>
        /// 构造插件信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="version">版本</param>
        /// <param name="author">作者</param>
        /// <param name="annotation">注释</param>
        /// <param name="filePath">文件路径</param>
        public PluginInfo(String name, String version, String author, String annotation, String filePath)
        {
            this.PluginName = name;
            this.PluginVersion = version;
            this.PluginAuthor = author;
            this.PluginAnnotation = annotation;
            this.FilePath = filePath;
        }
    }
}
