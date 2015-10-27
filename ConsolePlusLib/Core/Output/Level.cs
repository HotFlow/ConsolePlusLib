using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlusLib.Core.Output
{
    /// <summary>
    /// 输出等级
    /// </summary>
    public enum Level
    {
        [Description("信息")]
        Info,
        [Description("警告")]
        Warning,
        [Description("重要")]
        Severe,
        [Description("错误")]
        Error,
        [Description("未知")]
        Unknown
    }
}
