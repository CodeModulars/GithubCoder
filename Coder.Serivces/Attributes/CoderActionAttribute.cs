using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Serivces.Attributes
{
    /// <summary>
    /// 行为名称转换类型
    /// </summary>
    public enum ActionNameTransformType
    {
        /// <summary>
        /// 不转换
        /// </summary>
        None = 0,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 1,
        /// <summary>
        /// 驼峰命名法
        /// </summary>
        CamelCase = 0x11,
        /// <summary>
        /// 帕斯卡命名法
        /// </summary>
        PascalCase = 0x21,
        /// <summary>
        /// 大写下划线命名法
        /// </summary>
        UpperUnderlineCase = 0x31,
        /// <summary>
        /// 小写下划线命名法
        /// </summary>
        LowerUnderlineCase = 0x32,
    }

    /// <summary>
    /// 代码生成方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CoderActionAttribute : Attribute
    {
        /// <summary>
        /// 代码生成方法
        /// </summary>
        public CoderActionAttribute()
        {
            Name = string.Empty;
            TransformType = ActionNameTransformType.None;
        }

        /// <summary>
        /// 代码生成方法
        /// </summary>
        public CoderActionAttribute(string name)
        {
            Name = name;
            TransformType = ActionNameTransformType.Custom;
        }

        /// <summary>
        /// 代码生成方法
        /// </summary>
        public CoderActionAttribute(ActionNameTransformType transformType)
        {
            Name = string.Empty;
            TransformType = transformType;
        }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 转换类型
        /// </summary>
        public ActionNameTransformType TransformType { get; }
    }
}
