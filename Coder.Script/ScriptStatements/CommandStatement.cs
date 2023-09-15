using Coder.Script.ScriptNodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptStatements
{
    /// <summary>
    /// 文本语句
    /// </summary>
    public class CommandStatement : Statement
    {
        /// <summary>
        /// 文本语句
        /// </summary>
        public CommandStatement() : base(ScriptStatementType.Command)
        {
        }
    }
}
