using Coder.Ioc.Dependency;
using Coder.Serivces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script
{

    /*
     * 脚本规范
     * 以行为单位进行脚本处理
     * # 注释行 - 不进行任何处理
     * @render path 设置
     * @set $key value 设置变量
     * @use name 使用插件
     * @call $key name(args) 执行函数调用
     * @if name(args) 定义判断执行
     * @end if 定义判断结束
     * @each key list 定义重复执行
     * @end each 定义重复结束 
     * $(key) 获取变量
     * $@ 强制@符号
     * $$ 强制$符号
     */

    /// <summary>
    /// 脚本引擎
    /// </summary>
    public class Engine
    {
        // 解析器
        private readonly Parser _parser;
        // 所有语句
        private readonly IEnumerable<IScriptStatement> _statements;
        // 服务工厂
        private readonly CoderFactory _coderFactory;
        private readonly IDependencyManager _dependencyManager;

        /// <summary>
        /// 解析并生成脚本引擎
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static Engine Create(IDependencyManager dependencyManager, string script) => new Engine(dependencyManager, script);

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="script"></param>
        public Engine(IDependencyManager dependencyManager, string script)
        {
            // 依赖管理器
            _dependencyManager = dependencyManager;
            // 脚本解析
            _parser = new Parser(script);
            // 获取所有语句
            _statements = _parser.Statements;
            // 初始化服务工厂
            _coderFactory = new CoderFactory(_dependencyManager);
            _dependencyManager.Register(typeof(CoderFactory), _coderFactory);
        }

        /// <summary>
        /// 输出 .crss 文件
        /// </summary>
        /// <param name="path"></param>
        public void Output(string path)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IScriptStatement statement in _statements)
            {
                sb.AppendLine(statement.Output());
            }
            sy.IO.WriteUtf8FileContent(path, sb.ToString());
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void Execute()
        {
            Actuator actuator = new Actuator(_dependencyManager, _statements);
            actuator.Execute();
        }
    }
}
