using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script
{
    /// <summary>
    /// 脚本语句类型
    /// </summary>
    public enum ScriptStatementType
    {
        /// <summary>
        /// 注释
        /// </summary>
        Note = 0,
        /// <summary>
        /// 文本
        /// </summary>
        Text = 1,
        /// <summary>
        /// 指令
        /// </summary>
        Command = 2,
    }
}
