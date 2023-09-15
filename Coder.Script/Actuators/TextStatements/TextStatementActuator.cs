using Coder.Ioc.AsyncDatas;
using Coder.Ioc.Dependency;
using Coder.Script.ScriptStatements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Script.Actuators.TextStatements
{
    /// <summary>
    /// 文本语句执行器
    /// </summary>
    public class TextStatementActuator : ITextStatementActuator, IDependencyTransient
    {
        private readonly IAsyncDataProvider _asyncDataProvider;
        private readonly TextStatement _statement;
        private readonly FieldCollection _fields;

        /// <summary>
        /// 文本语句执行器
        /// </summary>
        public TextStatementActuator(
            IAsyncDataProvider asyncDataProvider
            )
        {
            _asyncDataProvider = asyncDataProvider;
            _statement = (TextStatement)(_asyncDataProvider.GetValue<IScriptStatement>() ?? throw new Exception($"未找到脚本语句"));
            _fields = _asyncDataProvider.GetValue<FieldCollection>() ?? throw new Exception($"未找到字段集合");
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        public async Task<string> Render()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var node in _statement.Nodes)
            {
                sb.Append(node.Render(_fields));
            }
            sb.AppendLine();
            return await Task.FromResult(sb.ToString());
        }
    }
}
