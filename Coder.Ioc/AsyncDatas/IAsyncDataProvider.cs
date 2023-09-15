using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Ioc.AsyncDatas
{
    /// <summary>
    /// 异步线程数据供应商
    /// </summary>
    public interface IAsyncDataProvider
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T? GetValue<T>()
            where T : class;

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        void SetValue<T>(T value)
            where T : class;
    }
}
