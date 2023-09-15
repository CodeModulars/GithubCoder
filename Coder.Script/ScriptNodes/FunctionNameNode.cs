using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptNodes
{
    /// <summary>
    /// 函数名称节点
    /// </summary>
    public class FunctionNameNode : SingleNode
    {
        /// <summary>
        /// 函数名称节点
        /// </summary>
        /// <param name="value"></param>
        public FunctionNameNode(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        public override string Output()
        {
            return "[FN]" + Name;
        }
    }
}
