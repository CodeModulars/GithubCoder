using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script
{
    /// <summary>
    /// 脚本语句
    /// </summary>
    public interface IScriptStatement
    {
        /// <summary>
        /// 脚本语句类型
        /// </summary>
        ScriptStatementType Type { get; }

        /// <summary>
        /// 结点集合
        /// </summary>
        List<IScriptNode> Nodes { get; }

        /// <summary>
        /// 输出
        /// </summary>
        /// <returns></returns>
        string Output();
    }
}
