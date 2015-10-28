using ConsolePlusLib.Core.Output;
using ConsolePlusLib.Events.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Core.PluginEngines
{
    /// <summary>
    /// 插件管理器
    /// </summary>
    public class PluginManager
    {
        /// <summary>
        /// 载入所有
        /// </summary>
        public void loadAll(String folder)
        {
            if (Directory.Exists(folder))
            {
                String[] fileNames = Directory.GetFiles(folder, "*.dll");

                foreach (String file in fileNames)
                {
                    this.load(file);
                }
            }
        }

        /// <summary>
        /// 卸载所有
        /// </summary>
        /// <param name="folder"></param>
        public void unloadAll(String folder)
        {
            if (Directory.Exists(folder))
            {
                String[] fileNames = Directory.GetFiles(folder, "*.dll");

                foreach (String file in fileNames)
                {
                    this.unload(file);
                }
            }
        }

        /// <summary>
        /// 载入
        /// </summary>
        public void load(String file)
        {
            try
            {
                Main.PluginFiles.Add(file);
                ServerPluginLoader loader = new ServerPluginLoader(file);

                String name = loader.getPluginInfo().PluginName;
                String version = loader.getPluginInfo().PluginVersion;
                String author = loader.getPluginInfo().PluginAuthor;

                Boolean canLoad = true;

                foreach (PluginInfo info in Main.Plugins.ToArray())
                {
                    if (info.PluginName.Equals(name) && info.PluginAuthor.Equals(author) && info.PluginVersion.Equals(version))
                    {
                        canLoad = false;
                        break;
                    }
                    else if (name == null || version == null)
                    {
                        canLoad = false;
                        break;
                    }
                }

                if (!canLoad)
                {
                    return;
                }

                System.Out.println("[" + name + "] Enabling v." + version);

                loader.load();

                Main.Plugins.Add(loader.getPluginInfo());

            }
            catch (Exception ex)
            {
                System.Out.println(Level.Error, ex.ToString());
            }
        }

        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="file"></param>
        public void unload(String file)
        {
            try
            {
                Main.PluginFiles.Remove(file);
                ServerPluginUnistaller unistaller = new ServerPluginUnistaller(file);

                String name = unistaller.getPluginInfo().PluginName;
                String version = unistaller.getPluginInfo().PluginVersion;
                String author = unistaller.getPluginInfo().PluginAuthor;

                Boolean canUnload = false;

                foreach (PluginInfo info in Main.Plugins.ToArray())
                {
                    if (info.PluginName.Equals(name) && info.PluginAuthor.Equals(author) && info.PluginVersion.Equals(version))
                    {
                        canUnload = true;
                        break;
                    }
                    else if (name == null || version == null)
                    {
                        canUnload = false;
                        break;
                    }
                }

                if (!canUnload)
                {
                    return;
                }

                System.Out.println("[" + name + "] Disabling v." + version);

                unistaller.unload();

                for (int i = 0; i < Main.getServer().getPlugins().Count; i++)
                {
                    PluginInfo info = Main.getServer().getPlugins()[i];

                    if (unistaller.getPluginInfo().PluginName.Equals(info.PluginName) &&
                        unistaller.getPluginInfo().PluginVersion.Equals(info.PluginVersion) &&
                        unistaller.getPluginInfo().PluginAuthor.Equals(info.PluginAuthor) &&
                        unistaller.getPluginInfo().PluginAnnotation.Equals(info.PluginAnnotation) &&
                        unistaller.getPluginInfo().FilePath.Equals(info.FilePath))
                    {
                        Main.Plugins.Remove(info);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Out.println(Level.Error, ex.ToString());
            }
        }
    }
}
