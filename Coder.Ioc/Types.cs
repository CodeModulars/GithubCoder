using Coder.Ioc.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Ioc
{
    /// <summary>
    /// 类型集合
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// 单例类型
        /// </summary>
        public static Type Singleton = typeof(IDependencySingleton);

        /// <summary>
        /// 瞬态类型
        /// </summary>
        public static Type Transient = typeof(IDependencyTransient);
    }
}
