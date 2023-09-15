using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Script.Actuators.ScriptStatement
{
    /// <summary>
    /// 脚本语句执行器
    /// </summary>
    public interface IScriptStatementActuator
    {
        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        Task<string> Render();
    }
}
