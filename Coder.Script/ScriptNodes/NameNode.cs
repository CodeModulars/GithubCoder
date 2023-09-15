using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptNodes
{
    /// <summary>
    /// 名称节点
    /// </summary>
    public class NameNode : SingleNode
    {
        /// <summary>
        /// 名称节点
        /// </summary>
        /// <param name="name"></param>
        public NameNode(string name)
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
            return Name;
        }
    }
}
