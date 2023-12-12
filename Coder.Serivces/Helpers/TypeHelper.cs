using Coder.Serivces.Attributes;
using Suyaa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Coder.Serivces.Helpers
{
    /// <summary>
    /// 类型助手
    /// </summary>
    public static class TypeHelper
    {
        // 获取驼峰名称
        private static string GetCamelCase(string name)
        {
            if (name.IsNullOrWhiteSpace()) return name;
            if (name.Length > 1)
            {
                return name[0].ToString().ToLower() + name.Substring(1);
            }
            return name.ToLower();
        }

        // 获取帕斯卡名称
        private static string GetPascalCase(string name)
        {
            if (name.IsNullOrWhiteSpace()) return name;
            if (name.Length > 1)
            {
                return name[0].ToString().ToUpper() + name.Substring(1);
            }
            return name.ToUpper();
        }

        // 获取小写下划线名称
        private static string GetLowerUnderlineCase(string name)
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
        private static string GetUpperUnderlineCase(string name)
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

        /// <summary>
        /// 获取所有行为
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<CoderActionDescriptor> GetActions(this Type type)
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
                string description = string.Empty;
                var descriptionAttribute = method.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null) description = descriptionAttribute.Description;
                actions.Add(new CoderActionDescriptor(type, name, description, method));
            }
            return actions;
        }
    }
}
