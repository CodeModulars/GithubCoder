using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptNodes
{
    /// <summary>
    /// 数值
    /// </summary>
    public class NumberNode : SingleNode
    {
        /// <summary>
        /// 数值
        /// </summary>
        /// <param name="value"></param>
        public NumberNode(double value)
        {
            Value = value;
        }

        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// 输出
        /// </summary>
        /// <returns></returns>
        public override string Output()
        {
            return "[D]" + Value.ToString();
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public override string Render(FieldCollection fields)
        {
            return Value.ToString();
        }
    }
}
