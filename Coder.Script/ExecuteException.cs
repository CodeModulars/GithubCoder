using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script
{
    /// <summary>
    /// 解析异常
    /// </summary>
    public class ExecuteException : Exception
    {
        /// <summary>
        /// 解析异常
        /// </summary>
        /// <param name="line"></param>
        /// <param name="site"></param>
        /// <param name="chr"></param>
        public ExecuteException(IScriptStatement statement, string? message = null) : base($"语句'{statement.Output()}'执行发生异常{(message is null ? "" : $": {message}")}")
        {
            Statement = statement;
        }

        /// <summary>
        /// 解析异常
        /// </summary>
        /// <param name="line"></param>
        /// <param name="site"></param>
        /// <param name="chr"></param>
        public ExecuteException(string message) : base($"执行发生异常: {message}")
        {
        }

        public IScriptStatement? Statement { get; }
    }
}
