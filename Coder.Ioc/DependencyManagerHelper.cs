using Coder.Ioc.Dependency;
using Suyaa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Ioc
{
    /// <summary>
    /// 依赖管理器助手
    /// </summary>
    public static class DependencyManagerHelper
    {
        #region 按接口注册

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <exception cref="DependencyException"></exception>
        public static bool RegisterByInterface(this IDependencyManager manager, Type serviceType, Type implementationType)
        {
            // 是否实现了单例
            if (Types.Singleton.IsAssignableFrom(implementationType))
            {
                manager.Register(serviceType, implementationType, Lifetimes.Singleton);
                return true;
            }
            // 是否实现了瞬态
            if (Types.Transient.IsAssignableFrom(implementationType))
            {
                manager.Register(serviceType, implementationType, Lifetimes.Transient);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="implementationType"></param>
        /// <exception cref="DependencyException"></exception>
        public static bool RegisterByInterface(this IDependencyManager manager, Type implementationType)
        {
            // 是否实现了单例
            if (Types.Singleton.IsAssignableFrom(implementationType))
            {
                manager.Register(implementationType, Lifetimes.Singleton);
                return true;
            }
            // 是否实现了瞬态
            if (Types.Transient.IsAssignableFrom(implementationType))
            {
                manager.Register(implementationType, Lifetimes.Transient);
                return true;
            }
            return false;
        }

        #endregion

        #region 按特性注册

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <exception cref="DependencyException"></exception>
        public static bool RegisterByAttribute(this IDependencyManager manager, Type serviceType, Type implementationType)
        {
            // 获取特性
            var attr = implementationType.GetCustomAttribute<DependencyAttribute>();
            if (attr is null) return false;
            // 注册
            manager.Register(serviceType, implementationType, attr.Lifetime);
            return true;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="implementationType"></param>
        /// <exception cref="DependencyException"></exception>
        public static bool RegisterByAttribute(this IDependencyManager manager, Type implementationType)
        {
            // 获取特性
            var attr = implementationType.GetCustomAttribute<DependencyAttribute>();
            if (attr is null) return false;
            // 注册
            manager.Register(implementationType, attr.Lifetime);
            return true;
        }

        #endregion

        #region 自动判断接口或特性注册

        /// <summary>
        /// 自动判断生命周期并注册
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        public static bool RegisterAuto(this IDependencyManager manager, Type serviceType, Type implementationType)
        {
            // 优先按接口注册
            if (manager.RegisterByInterface(serviceType, implementationType)) return true;
            // 按特性注册
            if (manager.RegisterByAttribute(serviceType, implementationType)) return true;
            return false;
        }

        /// <summary>
        /// 自动判断生命周期并注册
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="implementationType"></param>
        public static bool RegisterAuto(this IDependencyManager manager, Type implementationType)
        {
            // 优先按接口注册
            if (manager.RegisterByInterface(implementationType)) return true;
            // 按特性注册
            if (manager.RegisterByAttribute(implementationType)) return true;
            return false;
        }

        #endregion

        /// <summary>
        /// 注册
        /// </summary>
        public static void Register<TService, TImplementation>(this IDependencyManager manager)
        {
            var type = typeof(TImplementation);
            if (!manager.RegisterAuto(typeof(TService), type)) throw new DependencyException(type);
        }

        /// <summary>
        /// 注册
        /// </summary>
        public static void Register<T>(this IDependencyManager manager, Type type)
        {
            if (!manager.RegisterAuto(typeof(T), type)) throw new DependencyException(type);
        }

        /// <summary>
        /// 注册类型到
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="type"></param>
        public static void RegisterAllInterfaces(this IDependencyManager manager, Type type)
        {
            // 获取所有接口
            var ifs = type.GetInterfaces();
            foreach (var ifc in ifs)
            {
                if (ifc == Types.Transient || ifc == Types.Singleton) continue;
                // 注册类
                manager.RegisterAuto(ifc, type);
            }
        }

        /// <summary>
        /// 按程序集注册
        /// </summary>
        /// <param name="manager"></param>
        public static void RegisterAssembly(this IDependencyManager manager, Assembly assembly)
        {
            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    // 跳过所有的接口
                    if (type.IsInterface) continue;
                    // 条码抽象类
                    if (type.IsAbstract) continue;
                    // 自动判断并注册类型
                    manager.RegisterAllInterfaces(type);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 注册所有类
        /// </summary>
        /// <param name="manager"></param>
        public static void RegisterAll(this IDependencyManager manager)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                manager.RegisterAssembly(assembly);
            }
        }

        /// <summary>
        /// 抽取
        /// </summary>
        /// <param name="type"></param>
        public static T Resolve<T>(this IDependencyManager manager)
        {
            return (T)manager.Resolve(typeof(T));
        }

        /// <summary>
        /// 获取服务类获取所有的可实现类
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static List<Type> GetResolveTypes<T>(this IDependencyManager manager)
        {
            return manager.GetResolveTypes(typeof(T));
        }
    }
}
