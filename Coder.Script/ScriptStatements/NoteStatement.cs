using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptStatements
{
    /// <summary>
    /// 注释语句
    /// </summary>
    public class NoteStatement : Statement
    {
        /// <summary>
        /// 注释语句
        /// </summary>
        public NoteStatement() : base(ScriptStatementType.Note)
        {
        }
    }
}
