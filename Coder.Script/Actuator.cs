using Coder.Ioc;
using Coder.Ioc.AsyncDatas;
using Coder.Ioc.Dependency;
using Coder.Script.Actuators.ScriptStatement;
using Coder.Script.ScriptStatements;
using Suyaa;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script
{
    /// <summary>
    /// 执行器
    /// </summary>
    public class Actuator
    {
        private readonly IDependencyManager _dependencyManager;
        private readonly IEnumerable<IScriptStatement> _statements;
        private readonly FieldCollection _fields;

        /// <summary>
        /// 呈现地址
        /// </summary>
        public string RenderPath { get; set; }

        /// <summary>
        /// 执行器
        /// </summary>
        /// <param name="dependencyManager"></param>
        /// <param name="statements"></param>
        public Actuator(IDependencyManager dependencyManager, IEnumerable<IScriptStatement> statements)
        {
            _dependencyManager = dependencyManager;
            _statements = statements;
            _fields = new FieldCollection();
            this.RenderPath = string.Empty;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="statement"></param>
        public void ExecuteStatement(IScriptStatement statement) { }

        /// <summary>
        /// 执行
        /// </summary>
        public async void Execute()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var statement in _statements)
                {
                    if (statement is NoteStatement) continue;
                    // 获取一个执行器
                    var dataProvider = _dependencyManager.Resolve<IAsyncDataProvider>();
                    dataProvider.SetValue(this);
                    dataProvider.SetValue(statement);
                    dataProvider.SetValue(_fields);
                    // 获取一个执行器
                    var actuator = _dependencyManager.Resolve<IScriptStatementActuator>();
                    sb.Append(await actuator.Render());
                }
                // 判断输出地址
                if (this.RenderPath.IsNullOrWhiteSpace()) throw new Exception($"缺少输出地址");
                // 获取目录并创建
                var folder = sy.IO.GetFolderPath(this.RenderPath);
                sy.IO.CreateFolder(folder);
                // 输出内容
                sy.IO.WriteUtf8FileContent(this.RenderPath, sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
