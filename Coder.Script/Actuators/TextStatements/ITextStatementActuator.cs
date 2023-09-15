using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Script.Actuators.TextStatements
{
    /// <summary>
    /// 脚本语句执行器
    /// </summary>
    public interface ITextStatementActuator
    {
        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        Task<string> Render();
    }
}
