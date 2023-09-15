using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Coder.Script.ScriptStatements
{
    /// <summary>
    /// 语句
    /// </summary>
    public abstract class Statement : IScriptStatement
    {
        /// <summary>
        /// 语句类型
        /// </summary>
        public ScriptStatementType Type { get; }

        /// <summary>
        /// 结点集合
        /// </summary>
        public List<IScriptNode> Nodes { get; }

        /// <summary>
        /// 语句
        /// </summary>
        /// <param name="type"></param>
        public Statement(ScriptStatementType type)
        {
            Type = type;
            this.Nodes = new List<IScriptNode>();
        }

        /// <summary>
        /// 输出字符串表示形式
        /// </summary>
        /// <returns></returns>
        public virtual string Output()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            sb.Append(Type.ToString());
            sb.Append(']');
            foreach (IScriptNode node in Nodes)
            {
                //sb.Append(' ');
                sb.Append(node.Output());
            }
            return sb.ToString();
        }
    }
}
