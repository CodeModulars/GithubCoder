using System;
using System.Collections.Generic;
using System.Text;
using Coder.Script;

namespace Coder.Script.ScriptNodes
{
    /// <summary>
    /// 名称节点
    /// </summary>
    public class FieldNode : SingleNode
    {
        /// <summary>
        /// 名称节点
        /// </summary>
        /// <param name="value"></param>
        public FieldNode(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 输出
        /// </summary>
        /// <returns></returns>
        public override string Output()
        {
            return "[$]" + Name;
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public override string Render(FieldCollection fields)
        {
            return fields.GetValue(Name)?.ToString() ?? throw new ExecuteException($"缺少字段'{Name}'");
        }
    }
}
