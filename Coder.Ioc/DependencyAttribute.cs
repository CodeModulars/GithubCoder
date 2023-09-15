using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Ioc
{
    /// <summary>
    /// 依赖特性
    /// </summary>
    public class DependencyAttribute : Attribute
    {
        /// <summary>
        /// 依赖特性
        /// </summary>
        /// <param name="lifeCycle"></param>
        public DependencyAttribute(Lifetimes lifetime)
        {
            Lifetime = lifetime;
        }

        public Lifetimes Lifetime { get; }
    }
}
