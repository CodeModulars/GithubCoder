using Coder.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Coder.Ioc.ServiceCollection
{
    /// <summary>
    /// 依赖控制器
    /// </summary>
    public class DependencyManager : IDependencyManager
    {
        // DI容器
        private readonly Microsoft.Extensions.DependencyInjection.ServiceCollection _services;
        private static ServiceProvider? _provider;

        /// <summary>
        /// 依赖控制器
        /// </summary>
        public DependencyManager()
        {
            _services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            _services.AddSingleton<IDependencyManager>(this);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="lifeCycle"></param>
        public void Register(Type implementationType, Lifetimes lifeCycle)
        {
            switch (lifeCycle)
            {
                case Lifetimes.Singleton:
                    _services.AddSingleton(implementationType);
                    break;
                default:
                    _services.AddTransient(implementationType);
                    break;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifeCycle"></param>
        public void Register(Type serviceType, Type implementationType, Lifetimes lifeCycle)
        {
            switch (lifeCycle)
            {
                case Lifetimes.Singleton:
                    _services.AddSingleton(serviceType, implementationType);
                    break;
                default:
                    _services.AddTransient(serviceType, implementationType);
                    break;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationInstance"></param>
        public void Register(Type serviceType, object implementationInstance)
        {
            _services.AddSingleton(serviceType, implementationInstance);
        }

        /// <summary>
        /// 抽取
        /// </summary>
        /// <param name="type"></param>
        public object Resolve(Type type)
        {
            _provider ??= _services.BuildServiceProvider();
            var obj = _provider.GetService(type);
            if (obj is null) throw new DependencyException(type);
            return obj;
        }
    }
}