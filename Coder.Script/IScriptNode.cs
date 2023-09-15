using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script
{
    /// <summary>
    /// 节点
    /// </summary>
    public interface IScriptNode
    {
        /// <summary>
        /// 输出
        /// </summary>
        /// <returns></returns>
        string Output();

        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        string Render(FieldCollection fields);
    }
}
