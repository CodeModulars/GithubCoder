using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Script.Actuators.CommandStatements
{
    /// <summary>
    /// 指令语句
    /// </summary>
    public interface ICommandStatementActuator
    {
        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        Task<string> Render();
    }
}
