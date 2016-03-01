using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Executor.Annotation
{
    /// 权限信息
    /// </summary>
    public class IPermission : Attribute
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public Permission Permission { get; set; }
    }
}
