using Coder.Ioc;
using Coder.Ioc.Dependency;
using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;
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

        // 获取驼峰名称
        private string GetCamelCase(string name)
        {
            if (name.IsNullOrWhiteSpace()) return name;
            if (name.Length > 1)
            {
                return name[0].ToString().ToLower() + name.Substring(1);
            }
            return name.ToLower();
        }

        // 获取帕斯卡名称
        private string GetPascalCase(string name)
        {
            if (name.IsNullOrWhiteSpace()) return name;
            if (name.Length > 1)
            {
                return name[0].ToString().ToUpper() + name.Substring(1);
            }
            return name.ToUpper();
        }

        // 获取小写下划线名称
        private string GetLowerUnderlineCase(string name)
        {
            if (name.IsNullOrWhiteSpace()) return name;
            if (name.Length > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(name[0].ToString().ToLower());
                for (int i = 1; i < name.Length; i++)
                {
                    if (name[i] >= 'A' && name[i] <= 'Z')
                    {
                        sb.Append('_');
                        sb.Append(name[i].ToString().ToLower());
                    }
                    else
                    {
                        sb.Append(name[i]);
                    }
                }
                return sb.ToString();
            }
            return name.ToLower();
        }

        // 获取小写下划线名称
        private string GetUpperUnderlineCase(string name)
        {
            if (name.IsNullOrWhiteSpace()) return name;
            if (name.Length > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(name[0].ToString().ToLower());
                for (int i = 1; i < name.Length; i++)
                {
                    if (name[i] >= 'A' && name[i] <= 'Z')
                    {
                        sb.Append('_');
                        sb.Append(name[i]);
                    }
                    else
                    {
                        sb.Append(name[i].ToString().ToUpper());
                    }
                }
                return sb.ToString();
            }
            return name.ToUpper();
        }

        // 获取所有行为
        private List<CoderActionDescriptor> GetActions(Type type)
        {
            List<CoderActionDescriptor> actions = new List<CoderActionDescriptor>();
            // 获取所有接口
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<CoderActionAttribute>();
                if (attr is null) continue;
                var name = attr.TransformType switch
                {
                    ActionNameTransformType.Custom => attr.Name,
                    ActionNameTransformType.PascalCase => GetPascalCase(method.Name),
                    ActionNameTransformType.CamelCase => GetCamelCase(method.Name),
                    ActionNameTransformType.LowerUnderlineCase => GetLowerUnderlineCase(method.Name),
                    ActionNameTransformType.UpperUnderlineCase => GetUpperUnderlineCase(method.Name),
                    _ => method.Name,
                };
                actions.Add(new CoderActionDescriptor(type, name, method));
            }
            return actions;
        }

        // 获取所有行为
        private List<CoderActionDescriptor> GetActions(IList<Type> types)
        {
            List<CoderActionDescriptor> actions = new List<CoderActionDescriptor>();
            foreach (var type in types) actions.AddRange(GetActions(type));
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
