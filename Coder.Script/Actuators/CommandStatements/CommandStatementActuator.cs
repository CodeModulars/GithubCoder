using Coder.Ioc.AsyncDatas;
using Coder.Ioc.Dependency;
using Coder.Script.ScriptNodes;
using Coder.Script.ScriptStatements;
using Coder.Serivces;
using Suyaa.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Script.Actuators.CommandStatements
{
    /// <summary>
    /// 指令语句
    /// </summary>
    public class CommandStatementActuator : ICommandStatementActuator, IDependencyTransient
    {
        private readonly IAsyncDataProvider _asyncDataProvider;
        private readonly CoderFactory _coderFactory;
        private readonly EArguments _arguments;
        private readonly IDependencyManager _dependencyManager;
        private readonly CommandStatement _statement;
        private readonly FieldCollection _fields;
        private readonly Actuator _actuator;

        /// <summary>
        /// 指令语句
        /// </summary>
        public CommandStatementActuator(
            IAsyncDataProvider asyncDataProvider,
            CoderFactory coderFactory,
            EArguments arguments,
            IDependencyManager dependencyManager
            )
        {
            _asyncDataProvider = asyncDataProvider;
            _coderFactory = coderFactory;
            _arguments = arguments;
            _dependencyManager = dependencyManager;
            _statement = (CommandStatement)(_asyncDataProvider.GetValue<IScriptStatement>() ?? throw new ExecuteException($"未找到脚本语句"));
            _fields = _asyncDataProvider.GetValue<FieldCollection>() ?? throw new ExecuteException($"未找到字段集合");
            _actuator = _asyncDataProvider.GetValue<Actuator>() ?? throw new ExecuteException($"未找到执行器");
        }

        // 设置呈现地址
        private async Task SetRenderPath()
        {
            if (_statement.Nodes.Count < 2) throw new ExecuteException(_statement);
            if (!(_statement.Nodes[1] is StringNode stringNode)) throw new ExecuteException(_statement);
            string target = _arguments["target"];
            string path = _statement.Nodes[1].Render(_fields);
            _actuator.RenderPath = sy.IO.CombinePath(target, path);
            await Task.CompletedTask;
        }

        // 获取节点值
        private object GetNodeValue(IScriptNode scriptNode)
        {
            return scriptNode switch
            {
                StringNode stringNode => stringNode.Render(_fields),
                FieldNode fieldNode => _fields.GetValueRequired(fieldNode.Name),
                _ => throw new ExecuteException(_statement),
            };
        }

        // 设置呈现地址
        private async Task RenderCall()
        {
            if (_statement.Nodes.Count < 3) throw new ExecuteException(_statement);
            if (!(_statement.Nodes[2] is FunctionNameNode functionNameNode)) throw new ExecuteException(_statement);
            List<object> list = new List<object>();
            List<Type> types = new List<Type>();
            for (int i = 3; i < _statement.Nodes.Count; i++)
            {
                var node = _statement.Nodes[i];
                var obj = GetNodeValue(node);
                list.Add(obj);
                types.Add(obj.GetType());
            }
            var action = _coderFactory.GetAction(functionNameNode.Name, types);
            if (action is null) throw new ExecuteException(_statement);
            var service = _dependencyManager.Resolve(action.CoderService);
            if (action.ReturnType.FullName == "System.Void")
            {
                // 执行方法
                action.Method.Invoke(service, list.ToArray());
            }
            else
            {
                // 执行方法并返回结果
                var res = action.Method.Invoke(service, list.ToArray());
                if (_statement.Nodes[1] is FieldNode fieldNode)
                {
                    _fields.SetValue(fieldNode.Name, res);
                }
            }
            //_actuator.RenderPath = path;
            await Task.CompletedTask;
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <returns></returns>
        public async Task<string> Render()
        {
            if (_statement.Nodes.Count < 1) throw new ExecuteException(_statement);
            if (!(_statement.Nodes[0] is NameNode nameNode)) throw new ExecuteException(_statement);
            switch (nameNode.Name)
            {
                case "use": return string.Empty;
                case "render":
                    await SetRenderPath();
                    return string.Empty;
                case "call":
                    await RenderCall();
                    return string.Empty;
                default: throw new ExecuteException(_statement);
            }
        }
    }
}
