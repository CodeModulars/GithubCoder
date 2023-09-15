using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script
{
    /// <summary>
    /// 字段集合
    /// </summary>
    public class FieldCollection : Dictionary<string, object>
    {
        /// <summary>
        /// 字段集合
        /// </summary>
        public FieldCollection() { }

        /// <summary>
        /// 字段集合
        /// </summary>
        /// <param name="parent"></param>
        public FieldCollection(FieldCollection parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// 父集合
        /// </summary>
        public FieldCollection? Parent { get; }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetValueRequired(string name)
        {
            var value = GetValue(name);
            if (value is null) throw new ExecuteException($"字段'{name}'不存在");
            return value;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object? GetValue(string name)
        {
            // 当前集合中有，直接返回
            if (this.ContainsKey(name))
            {
                return this[name];
            }
            // 当前集合没有，则递归父对象
            if (this.Parent is null) return null;
            return this.Parent.GetValue(name);
        }

        /// <summary>
        /// 判断字段集合与所有父对象集合中是否存在名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsName(string name)
        {
            if (this.ContainsKey(name)) return true;
            if (this.Parent is null) return false;
            return this.Parent.ContainsName(name);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetValue(string name, object value)
        {
            // 当前集合中有，直接赋值
            if (this.ContainsKey(name))
            {
                this[name] = value;
                return;
            }
            // 没有父对象则添加值
            if (this.Parent is null)
            {
                this.Add(name, value);
                return;
            }
            // 判断父链条中是否存在名称
            if (this.Parent.ContainsName(name))
            {
                this.Parent.SetValue(name, value);
                return;
            }
            // 添加值
            this.Add(name, value);
        }
    }
}
