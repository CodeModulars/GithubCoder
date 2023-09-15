using Coder.Ioc;
using Coder.Ioc.AsyncDatas;
using Coder.Ioc.Dependency;
using Coder.Script.Actuators.CommandStatements;
using Coder.Script.Actuators.TextStatements;
using Coder.Script.ScriptStatements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Script.Actuators.ScriptStatement
{
    /// <summary>
    /// 脚本语句执行器
    /// </summary>
    public class ScriptStatementActuator : IScriptStatementActuator, IDependencyTransient
    {

        #region DI注入

        private readonly IAsyncDataProvider _asyncDataProvider;
        private readonly IDependencyManager _dependencyManager;

        /// <summary>
        /// 脚本语句执行器
        /// </summary>
        public ScriptStatementActuator(
            IAsyncDataProvider asyncDataProvider,
            IDependencyManager dependencyManager
            )
        {
            _asyncDataProvider = asyncDataProvider;
            _dependencyManager = dependencyManager;
        }

        #endregion

        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        public async Task<string> Render()
        {
            var statement = _asyncDataProvider.GetValue<IScriptStatement>();
            if (statement is null) throw new ExecuteException("未找到执行语句");
            // 文本语句
            if (statement is TextStatement)
            {
                var actuator = _dependencyManager.Resolve<ITextStatementActuator>();
                return await actuator.Render();
            }
            // 指令语句
            if (statement is CommandStatement)
            {
                var actuator = _dependencyManager.Resolve<ICommandStatementActuator>();
                return await actuator.Render();
            }
            return string.Empty;
        }
    }
}
