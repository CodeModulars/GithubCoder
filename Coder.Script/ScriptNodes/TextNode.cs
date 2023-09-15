using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptNodes
{
    /// <summary>
    /// 文本
    /// </summary>
    public class TextNode : SingleNode
    {
        /// <summary>
        /// 文本
        /// </summary>
        /// <param name="value"></param>
        public TextNode(string value)
        {
            Value = value;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 输出
        /// </summary>
        /// <returns></returns>
        public override string Output()
        {
            return Value;
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public override string Render(FieldCollection fields)
        {
            return Value;
        }
    }
}
