using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Coder.Serivces
{
    /// <summary>
    /// 代码生成行为描述
    /// </summary>
    public class CoderActionDescriptor
    {
        /// <summary>
        /// 生成器服务
        /// </summary>
        public Type CoderService { get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 函数信息
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ReturnType { get; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<Type> Params { get; }

        /// <summary>
        /// 元数据集合
        /// </summary>
        public List<object> MetaDatas { get; }

        /// <summary>
        /// 生成器服务
        /// </summary>
        /// <param name="coderService"></param>
        /// <param name="name"></param>
        /// <param name="method"></param>
        public CoderActionDescriptor(Type coderService, string name, string description, MethodInfo method)
        {
            CoderService = coderService;
            Name = name;
            Description = description;
            Method = method;
            ReturnType = method.ReturnType;
            // 加载所有参数
            Params = new List<Type>();
            foreach (var param in method.GetParameters())
            {
                Params.Add(param.ParameterType);
            }
            // 添加所有相关元素
            MetaDatas = new List<object>();
            foreach (var attribute in method.GetCustomAttributes())
            {
                MetaDatas.Add(attribute);
            }
        }
    }
}
