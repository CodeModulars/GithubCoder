using Coder.Ioc.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Coder.Ioc.AsyncDatas
{
    /// <summary>
    /// 异步线程数据供应商
    /// </summary>
    public sealed class AsyncDataProvider : IAsyncDataProvider, IDependencyTransient
    {
        private static AsyncLocal<Dictionary<Type, object>> _asyncLocal = new AsyncLocal<Dictionary<Type, object>>();

        /// <summary>
        /// 异步线程数据供应商
        /// </summary>
        public AsyncDataProvider()
        {

        }

        // 自动创建字典
        private void AutoCreateDictionary()
        {
            lock (_asyncLocal)
            {
                if (_asyncLocal.Value is null) _asyncLocal.Value = new Dictionary<Type, object>();
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetValue<T>()
            where T : class
        {
            var type = typeof(T);
            AutoCreateDictionary();
            var dict = _asyncLocal.Value;
            if (dict.ContainsKey(type)) return (T)dict[type];
            return null;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void SetValue<T>(T value)
            where T : class
        {
            var type = typeof(T);
            AutoCreateDictionary();
            lock (_asyncLocal)
            {
                var dict = _asyncLocal.Value;
                dict[type] = value;
            }
        }
    }
}
