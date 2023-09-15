using Coder.Script.ScriptStatements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Script.ScriptNodes
{
    /// <summary>
    /// 字符串节点
    /// </summary>
    public class StringNode : ContainerNode
    {

        /// <summary>
        /// 字符串节点
        /// </summary>
        public StringNode()
        {
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        public override string Output()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[String]\"");
            foreach (var node in this)
            {
                sb.Append(node.Output());
            }
            sb.Append("\"");
            return sb.ToString();
        }

        public override string Render(FieldCollection fields)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var node in this)
            {
                sb.Append(node.Render(fields));
            }
            return sb.ToString();
        }
    }
}
