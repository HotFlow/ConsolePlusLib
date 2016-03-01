using ConsolePlusLib.Utils.Output;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsolePlusLib.Utils.Extendsions;

namespace ConsolePlusLib.Utils.Output
{
    /// <summary>
    /// 输出器
    /// </summary>
    public class Printer
    {
        private RichTextBox outputBox { get; set; }

        public Printer(RichTextBox outputBox)
        {
            this.outputBox = outputBox;
        }

        /// <summary>
        /// 输出信息到输出窗口
        /// </summary>
        /// <param name="level">输出等级</param>
        /// <param name="color">字体颜色</param>
        /// <param name="message">输出信息</param>
        public void println(Level level, Color color, String message)
        {
            this.outputBox.SelectionColor = color;
            this.outputBox.AppendText("[" +
                DateTime.Now.ToString("HH:mm:ss ") + level.getDescription() + "]: " +
                message + Environment.NewLine);
            this.outputBox.ScrollToCaret();
        }

        /// <summary>
        /// 输出信息到输出窗口
        /// </summary>
        /// <param name="color">字体颜色</param>
        /// <param name="message">输出信息</param>
        public void println(Color color, String message)
        {
            this.outputBox.SelectionColor = color;
            this.outputBox.AppendText("[" +
                DateTime.Now.ToString("HH:mm:ss ") + Level.Info.getDescription() + "]: " +
                message + Environment.NewLine);
            this.outputBox.ScrollToCaret();
        }

        /// <summary>
        /// 输出信息到输出窗口
        /// </summary>
        /// <param name="level">输出等级</param>
        /// <param name="message">输出信息</param>
        public void println(Level level, String message)
        {
            this.outputBox.SelectionColor = Color.ForestGreen;
            this.outputBox.AppendText("[" +
                DateTime.Now.ToString("HH:mm:ss ") + level.getDescription() + "]: " +
                message + Environment.NewLine);
            this.outputBox.ScrollToCaret();
        }

        /// <summary>
        /// 输出信息到输出窗口
        /// </summary>
        /// <param name="message">输出信息</param>
        public void println(String message)
        {
            this.outputBox.SelectionColor = Color.ForestGreen;
            this.outputBox.AppendText("[" +
                DateTime.Now.ToString("HH:mm:ss ") + Level.Info.getDescription() + "]: " +
                message + Environment.NewLine);
            this.outputBox.ScrollToCaret();
        }
    }
}
