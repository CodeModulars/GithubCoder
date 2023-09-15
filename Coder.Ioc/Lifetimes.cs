﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Ioc
{
    /// <summary>
    /// 生命周期
    /// </summary>
    public enum Lifetimes
    {
        /// <summary>
        /// 单例
        /// </summary>
        Singleton = 0x0,
        /// <summary>
        /// 瞬态
        /// </summary>
        Transient = 0x1,
    }
}
