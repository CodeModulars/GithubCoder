using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptStatements
{
    /// <summary>
    /// 文本语句
    /// </summary>
    public class TextStatement : Statement
    {
        /// <summary>
        /// 文本语句
        /// </summary>
        public TextStatement() : base(ScriptStatementType.Text)
        {
        }
    }
}
