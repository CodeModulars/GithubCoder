using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptNodes
{
    /// <summary>
    /// 容器节点
    /// </summary>
    public abstract class ContainerNode : List<SingleNode>, IScriptNode
    {
        public abstract string Output();

        public virtual string Render(FieldCollection fields)
        {
            throw new NotImplementedException();
        }
    }
}
