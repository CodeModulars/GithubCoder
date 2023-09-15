using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script.ScriptNodes
{
    /// <summary>
    /// 单一控件
    /// </summary>
    public abstract class SingleNode : IScriptNode
    {
        public abstract string Output();

        public virtual string Render(FieldCollection fields)
        {
            throw new NotImplementedException();
        }
    }
}
