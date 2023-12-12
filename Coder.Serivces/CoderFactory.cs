using Coder.Ioc;
using Coder.Ioc.Dependency;
using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;
using Coder.Serivces.Helpers;
using Suyaa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Coder.Serivces
{
    /// <summary>
    /// 代码生成器工厂
    /// </summary>
    public class CoderFactory
    {

        /// <summary>
        /// 生成行为描述
        /// </summary>
        private List<CoderActionDescriptor>? _actions;
        private readonly IDependencyManager _dependencyManager;

        /// <summary>
        /// 行为集合
        /// </summary>
        public IEnumerable<CoderActionDescriptor> Actions => _actions ?? new List<CoderActionDescriptor>();

        // 获取程序所有的
        private List<Type> GetCoderServics()
        {
            var list = new List<Type>();
            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                try
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsAbstract) continue;
                        // 判断是否实现 ICoderService 接口
                        if (type.HasInterface<ICoderService>())
                        {
                            _dependencyManager.RegisterAuto(type);
                            list.Add(type);
                        }
                    }
                }
                catch
                {
                }
            }
            return list;
        }

        /// <summary>
        /// 获取生成行为描述
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CoderActionDescriptor? GetAction(string name, List<Type> args)
        {
            if (_actions is null) return null;
            var actions = _actions.Where(d => d.Name == name).ToList();
            foreach (var action in actions)
            {
                if (action.Params.Count != args.Count) continue;
                bool found = true;
                for (int i = 0; i < action.Params.Count; i++)
                {
                    if (!action.Params[i].IsAssignableFrom(args[i]))
                    {
                        found = false;
                        break;
                    }
                }
                if (found) return action;
            }
            return null;
        }

        // 获取所有行为
        private List<CoderActionDescriptor> GetActions(IList<Type> types)
        {
            List<CoderActionDescriptor> actions = new List<CoderActionDescriptor>();
            foreach (var type in types) actions.AddRange(type.GetActions());
            return actions;
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        public void Reload()
        {
            // 获取所有的生成服务器
            var types = GetCoderServics();
            // 获取所有行为
            _actions = GetActions(types);
        }

        /// <summary>
        /// 代码生成器工厂
        /// </summary>
        public CoderFactory(IDependencyManager dependencyManager)
        {
            // 依赖管理器
            _dependencyManager = dependencyManager;
            // 从程序目录加载所有的dll
            sy.Assembly.LoadAssemblyFromFolder(sy.Assembly.ExecutionDirectory);
            // 重新加载数据
            this.Reload();
        }
    }
}
